extends SceneTree

# Plays TitanCraft from spawn to victory using nothing but real input, and
# records what happened as measured evidence.
#
# Why this exists
# ---------------
# tests/Integration/IntegrationTestRunner.cs reaches all eleven MVP smoke
# milestones, but it gets there by TELEPORTING the player beside each objective
# (`player.GlobalPosition = ...`) and calling TryInteract(). That proves the
# mission logic. It has never proved that a player can WALK the journey: that the
# routes are open, that each objective can be reached and used from where a
# player actually arrives, that the Scout can be beaten by aiming and swinging.
#
# So this run never sets the player's position, never parks the Scout, and never
# calls gameplay methods directly. It steers with injected mouse motion, walks
# with the move actions, and interacts and attacks with real InputEventAction
# events -- which is also the only path that exercises the input map, because
# FirstPersonController reads `interact` and `attack` in _UnhandledInput.
#
# The one exception is recorded, not hidden: if a leg cannot be walked after
# repeated unstuck attempts, the run marks that leg BLOCKED and places the
# player at the objective so the remaining legs still produce evidence. A
# BLOCKED leg is a failed playtest finding, never a pass.
#
# What this is not
# ----------------
# It is not a feel verdict. No agent experiences play;
# studio/decisions/quality_benchmark_v2_agent_gate_delegation.md replaces feel
# with measured proxies, and that is all this records. Frame timings come from
# the container's software renderer and are labelled as such -- they are not a
# hardware performance claim.
#
# Run:
#   xvfb-run -a godot --path . --script tools/visual_review/playthrough_journey.gd
#   xvfb-run -a godot --path . --script tools/visual_review/playthrough_journey.gd -- --defeat
#
# --defeat plays README section 30's "the player respawns after death" on the
# real path: save at the save point, collect something AFTER saving, die to the
# Scout, click "Reload Last Save", and check what came back. The integration
# suite cannot cover this -- it disables the end-screen scene change in every
# scenario -- and the death transition uses the same immediate scene change
# that broke the beacon.

const OUTPUT_DIR := "res://artifacts/visual-review/playthrough"
const WIDTH := 1280
const HEIGHT := 720
const MAIN_SCENE := "res://scenes/Main/Main.tscn"
const SAVE_PATH := "user://crash_site_save.json"
const SAVE_BACKUP := "user://crash_site_save.json.playthrough-backup"

# The level generator asserts these straight legs are clear of every district
# building (tools/level/build_mwezi_quarter_district.py ROUTES). Walking them
# checks that promise against everything else in the scene as well.
const LEGS := [
	["spawn_to_metal", "ResourceDrop_MetalPickup", "collect"],
	["metal_to_spawn", "@spawn", ""],
	["spawn_to_biomass", "ResourceDrop_BiomassPickup", "collect"],
	["biomass_to_spawn", "@spawn", ""],
	["spawn_to_electronics", "ResourceDrop_ElectronicsPickup", "collect"],
	["electronics_to_workbench", "Placeholder_Workbench", "craft"],
	["workbench_to_scout", "Placeholder_GalaxabrainScout", "fight"],
	["scout_to_component", "Placeholder_GalaxabrainScout/GalaxabrainComponentPickup", "recover"],
	["component_to_save_point", "Placeholder_SavePoint", "save"],
	["save_point_to_beacon", "Placeholder_Beacon", "activate"],
]

# The save point is walled on its north side (C7_Wall_1) and boxed in to the
# north-east, and is reached from the east along the route the level generator
# certified (component -> save point; (-4, -13) lies on it). The defeat legs
# join that route rather than inventing a diagonal: this mode tests respawn,
# not path-finding. Electronics are collected BEFORE saving (must survive the
# reload) and Biomass AFTER (must be lost).
const DEFEAT_LEGS := [
	["spawn_to_electronics", "ResourceDrop_ElectronicsPickup", "collect"],
	["electronics_to_route", "@-4,-13", ""],
	["route_to_save_point", "Placeholder_SavePoint", "save"],
	["save_point_to_route", "@-4,-13", ""],
	["route_to_electronics", "@0,-10", ""],
	["electronics_to_spawn", "@0,0", ""],
	["spawn_to_biomass", "ResourceDrop_BiomassPickup", "collect"],
	["biomass_to_spawn", "@0,0", ""],
	["spawn_to_electronics_again", "@0,-10", ""],
	["electronics_to_workbench", "@12,-12", ""],
	["workbench_to_scout", "Placeholder_GalaxabrainScout", "die"],
]

const INTERACT_STOP_M := 1.8       # inside the 3 m interaction ray
const WAYPOINT_STOP_M := 0.9
const STRIKE_RANGE_M := 2.4        # Mk I reach is 3.0 m
const STUCK_WINDOW_TICKS := 90     # 1.5 s at 60 Hz
const STUCK_MIN_PROGRESS_M := 0.25
const MAX_STUCK_EVENTS := 4
const LEG_TIMEOUT_S := 60.0
const MAX_SWINGS := 60

var _scene: Node = null
var _player: CharacterBody3D = null
var _spawn := Vector3.ZERO
var _tick := 0
var _capture_index := 0
var _frame_usec: Array[int] = []
var _last_frame_usec := 0
var _onboarding_log: Array = []
var _last_onboarding := ""
var _legs_report: Array = []
var _findings: Array = []
var _damage_events := 0
var _last_health := -1
var _defeat_mode := false
var _defeat_report := {}
# Ambience probe: each ambient loop is routed to its own bus (harness-only; the
# game's own bus layout is untouched) so its level can be read per leg.
const AMBIENCE_LAYER := "AudioLayer_Ambient"
const AMBIENCE_FLOOR_DB := -80.0
const AMBIENCE_AUDIBLE_DB := -60.0
var _ambience_buses: Array[String] = []
var _ambience_sum := {}
var _ambience_samples := 0
const AMBIENCE_ARRIVAL_TICKS := 60
var _ambience_recent := {}
# Cue probe: one-shot cues routed to their own buses, each with a capture
# effect so every mixed sample is inspected. Polling the bus peak once per frame
# is not enough: the container renders at a few frames per wall-clock second and
# the peak only reflects the latest mix buffer, so a 0.3 s cue is mostly missed.
# An onset is a 10 ms block above the audible level after a block below it.
const CUE_BLOCK := 441
const CUE_CAPTURE_SECONDS := 10.0
const CUE_NODES := [
	"AudioLayer_Player/Footsteps_Metal",
	"AudioLayer_Player/Footsteps_Rock",
	"AudioLayer_Player/Footsteps_Ash",
	"AudioLayer_Enemy/Scout_Alert",
	"AudioLayer_Enemy/Scout_Attack",
	"AudioLayer_Enemy/Scout_Hurt",
]
var _cues := {}
var _cue_state := {}

func _initialize() -> void:
	root.size = Vector2i(WIDTH, HEIGHT)
	process_frame.connect(_on_process_frame)
	call_deferred("_run")

func _on_process_frame() -> void:
	var now := Time.get_ticks_usec()
	if _last_frame_usec != 0:
		_frame_usec.append(now - _last_frame_usec)
	_last_frame_usec = now

# --- the journey ------------------------------------------------------------

func _run() -> void:
	DirAccess.make_dir_recursive_absolute(ProjectSettings.globalize_path(OUTPUT_DIR))
	# A playthrough must start from a new game. The game resumes from its
	# checkpoint on launch (by design -- smoke milestone 11), so the second and
	# third takes of this run silently continued take 1's save: they began at
	# the save point on 60 health with the Scout already dead, and their
	# "blocked legs" and "failed fight" were that stale state, not the game.
	# The save is moved aside, not deleted, and restored in _finish(), so
	# running this on a machine with a real save costs the player nothing.
	_isolate_save()
	var packed := load(MAIN_SCENE) as PackedScene
	if packed == null:
		_abort("cannot load %s" % MAIN_SCENE, 2)
		return
	_scene = packed.instantiate()
	root.add_child(_scene)
	current_scene = _scene
	_player = _scene.get_node_or_null("Player") as CharacterBody3D
	if _player == null:
		_abort("Player not found", 3)
		return

	_route_ambience()
	_route_cues()

	# Let the scene settle and the intro title card play out exactly as a player
	# sees it; it is a timed fade, not a blocking screen.
	await _ticks(6)
	_spawn = _player.global_position
	_last_health = _health_value()
	await _capture("00_spawn_title_card")
	await _ticks(60 * 5)
	await _capture("01_spawn_after_title")

	# Onboarding: look, then move, then jump -- the first three prompts are
	# about controls, and a real player does them before anything else.
	await _look_around()
	await _jump_once()

	_defeat_mode = "--defeat" in OS.get_cmdline_user_args()
	for leg in (DEFEAT_LEGS if _defeat_mode else LEGS):
		var ok: bool = await _play_leg(leg[0], leg[1], leg[2])
		if _is_over():
			break
		if not ok and leg[2] != "":
			_findings.append("objective step '%s' did not complete" % leg[0])

	await _finish()

func _play_leg(leg_name: String, target_path: String, action: String) -> bool:
	var target: Node3D = null
	var fixed := Vector3.INF
	if target_path.begins_with("@") and target_path != "@spawn":
		var xz := target_path.substr(1).split(",")
		fixed = Vector3(float(xz[0]), _spawn.y, float(xz[1]))
	elif target_path != "@spawn":
		target = _scene.get_node_or_null(target_path) as Node3D
		if target == null:
			_findings.append("%s: target node '%s' missing" % [leg_name, target_path])
			return false

	var stop := WAYPOINT_STOP_M if action == "" else INTERACT_STOP_M
	if action == "fight" or action == "die":
		stop = STRIKE_RANGE_M
	var goal := func() -> Vector3:
		if fixed != Vector3.INF:
			return fixed
		return _spawn if target == null else target.global_position

	var leg: Dictionary = await _walk_to(leg_name, goal, stop)
	var ok := true
	if action == "fight":
		ok = await _fight(target, leg)
	elif action == "die":
		ok = await _die_and_reload(target, leg)
	elif action != "":
		ok = await _interact(target, action, leg)
	leg["objective_after"] = _hud("Panel/Margin/VBox/Objective")
	leg["health_after"] = _hud("Panel/Margin/VBox/Health")
	leg["ambience_db"] = _take_ambience_levels()
	if not _ambience_buses.is_empty() and leg["ambience_db"]["leg"].values().max() < AMBIENCE_AUDIBLE_DB:
		_findings.append("%s: no ambient loop audible (loudest %.1f dB)" % [leg_name, leg["ambience_db"]["leg"].values().max()])
	_legs_report.append(leg)
	print("PLAYTHROUGH_LEG %s" % JSON.stringify(leg))
	await _capture("leg_%02d_%s" % [_legs_report.size(), leg_name])
	return ok

# --- movement ---------------------------------------------------------------

func _walk_to(leg_name: String, goal: Callable, stop_radius: float) -> Dictionary:
	var start: Vector3 = _player.global_position
	var target: Vector3 = goal.call()
	var straight := _flat(target - start).length()
	var leg := {
		"leg": leg_name, "straight_m": snappedf(straight, 0.01), "path_m": 0.0,
		"seconds": 0.0, "stuck_events": 0, "result": "", "health_before": _hud("Panel/Margin/VBox/Health"),
		"start": _fmt(start),
	}
	var ticks := 0
	var path := 0.0
	var prev := start
	var best := straight
	var best_tick := 0
	var stuck := 0
	var timeout_ticks := int(LEG_TIMEOUT_S * Engine.physics_ticks_per_second)

	while ticks < timeout_ticks:
		if _is_over():
			leg["result"] = "run_ended"
			break
		target = goal.call()
		var to := _flat(target - _player.global_position)
		var dist := to.length()
		if dist <= stop_radius:
			leg["result"] = "walked"
			break

		var desired := atan2(-to.x, -to.z)
		var yaw_delta := wrapf(desired - _player.rotation.y, -PI, PI)
		_turn(yaw_delta, 0.0)
		# Turn on the spot when facing well away; otherwise walk and correct.
		if absf(yaw_delta) < 0.6:
			Input.action_press("move_forward")
		else:
			Input.action_release("move_forward")

		await _ticks(1)
		ticks += 1
		path += _flat(_player.global_position - prev).length()
		prev = _player.global_position

		if dist < best - STUCK_MIN_PROGRESS_M:
			best = dist
			best_tick = ticks
		elif ticks - best_tick > STUCK_WINDOW_TICKS:
			stuck += 1
			print("PLAYTHROUGH_STUCK leg=%s at=%s dist=%.2f attempt=%d" % [
				leg_name, _fmt(_player.global_position), dist, stuck])
			if stuck == 1:
				await _capture("stuck_%s" % leg_name)
			if stuck > MAX_STUCK_EVENTS:
				leg["result"] = "blocked"
				break
			await _unstuck(stuck)
			best = _flat(goal.call() - _player.global_position).length()
			best_tick = ticks

	Input.action_release("move_forward")
	if leg["result"] == "":
		leg["result"] = "timeout"
	if leg["result"] in ["blocked", "timeout"]:
		# Recorded as a failed walk. The player is placed at the objective only
		# so the rest of the journey still produces evidence.
		_findings.append("%s: could not be walked (%s) at %s, %.1f m short" % [
			leg_name, leg["result"], _fmt(_player.global_position),
			_flat(goal.call() - _player.global_position).length()])
		var place: Vector3 = goal.call()
		var back := _flat(_spawn - place).normalized() * (stop_radius * 0.8)
		_player.global_position = place + back + Vector3(0.0, 0.6, 0.0)
		leg["placed_to_continue"] = true
		await _ticks(10)
	await _ticks(8)
	leg["end"] = _fmt(_player.global_position)
	leg["stuck_events"] = stuck
	leg["path_m"] = snappedf(path, 0.01)
	leg["seconds"] = snappedf(float(ticks) / Engine.physics_ticks_per_second, 0.01)
	return leg

func _unstuck(attempt: int) -> void:
	# What a player does when something is in the way: look left and right,
	# take the open side, walk past it, then carry on. The first version only
	# hopped and strafed, then steered straight back into the same obstacle --
	# which is how a plainly visible 1.5 m cover crate
	# (Environment/CoverElements/CrashSiteCargoCrate3) got reported as a route
	# snag it never was.
	var left := _free_distance(deg_to_rad(55.0))
	var right := _free_distance(deg_to_rad(-55.0))
	var turn := deg_to_rad(55.0) if left >= right else deg_to_rad(-55.0)
	# Alternate on repeat attempts in case the first choice was a dead end.
	if attempt % 2 == 0:
		turn = -turn
	var target_yaw := _player.rotation.y + turn
	for i in 30:
		var remaining := wrapf(target_yaw - _player.rotation.y, -PI, PI)
		if absf(remaining) < 0.03:
			break
		_turn(remaining, 0.0)
		await _ticks(1)
	Input.action_press("move_forward")
	Input.action_press("jump")
	await _ticks(3)
	Input.action_release("jump")
	await _ticks(50)
	Input.action_release("move_forward")

func _free_distance(yaw_offset: float) -> float:
	# How far the player could walk in a direction before hitting something, at
	# waist height, ignoring the player's own body.
	var from := _player.global_position + Vector3(0.0, 0.6, 0.0)
	var yaw := _player.rotation.y + yaw_offset
	var dir := Vector3(-sin(yaw), 0.0, -cos(yaw))
	var query := PhysicsRayQueryParameters3D.create(from, from + dir * 4.0)
	query.exclude = [_player.get_rid()]
	var hit := _player.get_world_3d().direct_space_state.intersect_ray(query)
	return 4.0 if hit.is_empty() else from.distance_to(hit["position"])

# --- interaction and combat ---------------------------------------------------

func _interact(target: Node3D, action: String, leg: Dictionary) -> bool:
	for attempt in 6:
		if _is_over():
			leg["interaction"] = "run_ended_on_attempt_%d" % (attempt + 1)
			return action == "activate" and _victory_reached()
		var before := _hud_snapshot()
		await _aim_at(target, 40)
		await _send_action("interact")
		await _ticks(12)
		if _is_over() or _hud_snapshot() != before or not is_instance_valid(target) or not target.is_visible_in_tree():
			leg["interaction"] = "ok_on_attempt_%d" % (attempt + 1)
			leg["feedback"] = _hud("ActionFeedback")
			return true
		# Record what the interaction ray actually hit, using the same query as
		# FirstPersonController.TryInteract, so a miss is a diagnosis rather than
		# a guess about the aim.
		leg["miss_%d" % (attempt + 1)] = _probe_interaction_ray()
		# Close in a little and try again, as a player would.
		Input.action_press("move_forward")
		await _ticks(6)
		Input.action_release("move_forward")
	leg["interaction"] = "failed_after_6_attempts"
	return false

func _fight(scout: Node3D, leg: Dictionary) -> bool:
	var objective_before := _hud("Panel/Margin/VBox/Objective")
	var swings := 0
	var start_tick := _tick
	var health_start := _health_value()
	while swings < MAX_SWINGS:
		if _is_over():
			break
		if not is_instance_valid(scout) or not scout.is_visible_in_tree() \
				or _hud("Panel/Margin/VBox/Objective") != objective_before:
			break
		var dist := _flat(scout.global_position - _player.global_position).length()
		if dist > STRIKE_RANGE_M:
			# The Scout moves; chase it on real input.
			var to := _flat(scout.global_position - _player.global_position)
			_turn(wrapf(atan2(-to.x, -to.z) - _player.rotation.y, -PI, PI), 0.0)
			Input.action_press("move_forward")
			await _ticks(1)
			continue
		Input.action_release("move_forward")
		await _aim_at(scout, 10)
		await _send_action("attack")
		swings += 1
		if swings == 1:
			# Take 1's first-swing frame showed an enclosed dark volume and no
			# Scout. Record where everything is so the frame can be explained.
			var cam := _player.get_node_or_null("Head/Camera3D") as Node3D
			leg["fight_first_swing"] = {
				"player": _fmt(_player.global_position),
				"scout": _fmt(scout.global_position),
				"camera": _fmt(cam.global_position) if cam != null else "?",
				"camera_to_scout_m": snappedf(cam.global_position.distance_to(scout.global_position), 0.01) if cam != null else -1.0,
				"forward_ray": _probe_interaction_ray(),
			}
			await _capture("fight_first_swing")
		await _ticks(20)
	Input.action_release("move_forward")
	var won := _hud("Panel/Margin/VBox/Objective") != objective_before and not _is_over()
	leg["fight"] = {
		"swings": swings,
		"seconds": snappedf(float(_tick - start_tick) / Engine.physics_ticks_per_second, 0.01),
		"health_start": health_start, "health_end": _health_value(), "won": won,
	}
	await _capture("fight_after")
	return won

# --- defeat and respawn -------------------------------------------------------

func _die_and_reload(scout: Node3D, leg: Dictionary) -> bool:
	# Unarmed, so the only way through is the Scout's own AI doing its job.
	Input.action_release("move_forward")
	var saved := _read_save()
	var start_tick := _tick
	while not _is_over() and _tick - start_tick < 60 * 60:
		if is_instance_valid(scout):
			await _aim_at(scout, 4)
		await _ticks(6)
	# The end-screen change is deferred to the next idle frame, and between the
	# old scene leaving and the new one arriving current_scene is null. Wait for
	# the new scene rather than sampling that gap.
	for i in 300:
		if current_scene != null and current_scene != _scene:
			break
		await process_frame
	var death := {
		"seconds_to_die": snappedf(float(_tick - start_tick) / Engine.physics_ticks_per_second, 0.01),
		"defeat_screen": _defeat_reached(),
		"saved_state": saved,
	}
	_defeat_report = death
	leg["defeat"] = death
	if not _defeat_reached():
		_findings.append("death did not reach the defeat screen")
		return false
	await _ticks(90)   # let the reveal animation finish, as a player would wait
	await _capture("defeat_screen")

	# "Reload Last Save" with a real mouse click at the button's centre.
	var button := current_scene.get_node_or_null("Menu/ReloadButton") as Button
	if button == null:
		_findings.append("defeat screen has no Reload button")
		return false
	var at := button.get_global_rect().get_center()
	death["reload_button_at"] = "(%.0f, %.0f)" % [at.x, at.y]
	for pressed in [true, false]:
		var click := InputEventMouseButton.new()
		click.button_index = MOUSE_BUTTON_LEFT
		click.pressed = pressed
		click.position = at
		click.global_position = at
		Input.parse_input_event(click)
		Input.flush_buffered_events()
		await process_frame
	for i in 240:
		if current_scene != null and str(current_scene.scene_file_path).ends_with("Main.tscn"):
			break
		await process_frame
	if current_scene == null or not str(current_scene.scene_file_path).ends_with("Main.tscn"):
		_findings.append("Reload Last Save did not return to the game scene")
		return false

	# Adopt the reloaded scene and let the checkpoint restore run.
	_scene = current_scene
	_player = _scene.get_node_or_null("Player") as CharacterBody3D
	await _ticks(30)
	var save_point := _scene.get_node_or_null("Placeholder_SavePoint") as Node3D
	var respawn := {
		"player": _fmt(_player.global_position),
		"distance_to_save_point_m": snappedf(_flat(save_point.global_position - _player.global_position).length(), 0.01) if save_point != null else -1.0,
		"health": _hud("Panel/Margin/VBox/Health"),
		"resources": _hud("Panel/Margin/VBox/Resources"),
		"objective": _hud("Panel/Margin/VBox/Objective"),
	}
	death["after_reload"] = respawn
	await _capture("after_reload")
	return true

func _read_save() -> Dictionary:
	var file := FileAccess.open(SAVE_PATH, FileAccess.READ)
	if file == null:
		return {}
	var parsed = JSON.parse_string(file.get_as_text())
	return parsed if parsed is Dictionary else {}

# --- controls onboarding ------------------------------------------------------

func _look_around() -> void:
	for i in 70:
		_inject_mouse(Vector2(-6.0, 0.0))
		await _ticks(1)
	for i in 70:
		_inject_mouse(Vector2(6.0, 0.0))
		await _ticks(1)

var _jump := {}

func _jump_once() -> void:
	# README section 30 lists "the player can jump", and nothing had measured it:
	# the suite only checks the action is mapped and the parameters are valid.
	var floor_y := _player.global_position.y
	var peak := floor_y
	Input.action_press("jump")
	await _ticks(3)
	Input.action_release("jump")
	for i in 60:
		await _ticks(1)
		peak = maxf(peak, _player.global_position.y)
	_jump = {"rise_m": snappedf(peak - floor_y, 0.01), "landed": _player.is_on_floor()}

# --- end of run -------------------------------------------------------------

func _finish() -> void:
	# Victory now holds the world on screen before the end screen
	# (CrashSiteEndScreenNavigator.VictoryHoldSeconds), so film the climax
	# while it plays, then wait for the end screen to arrive.
	if not _defeat_mode and not _is_over():
		await _ticks(60)
		await _capture("beacon_climax")
	for i in 900:
		if _victory_reached():
			break
		await _ticks(1)
	await _ticks(30)
	await _capture("99_end_screen")
	var sorted := _frame_usec.duplicate()
	sorted.sort()
	var n := sorted.size()
	var mean := 0.0
	for v in sorted:
		mean += v
	mean = mean / maxf(n, 1)
	var report := {
		"result": ("respawned" if _defeat_report.get("after_reload") else "defeat_not_recovered") if _defeat_mode else ("victory" if _victory_reached() else ("defeat" if _defeat_reached() else "incomplete")),
		"defeat": _defeat_report,
		"end_scene": "" if current_scene == null else str(current_scene.scene_file_path),
		"game_seconds": snappedf(float(_tick) / Engine.physics_ticks_per_second, 0.01),
		"legs_walked": _legs_report.filter(func(l): return l.get("result") == "walked").size(),
		"legs_total": _legs_report.size(),
		"damage_events": _damage_events,
		"ambience_buses": _ambience_buses,
		"cues_heard": _cues,
		"jump": _jump,
		"onboarding_prompts_seen": _onboarding_log,
		"findings": _findings,
		"captures": _capture_index,
		"render_proxy": {
			"note": "container software renderer under xvfb; not a hardware performance claim",
			"frames": n,
			"mean_ms": snappedf(mean / 1000.0, 0.01),
			"p95_ms": snappedf((sorted[int(n * 0.95)] if n > 0 else 0) / 1000.0, 0.01),
			"max_ms": snappedf((sorted[n - 1] if n > 0 else 0) / 1000.0, 0.01),
		},
		"legs": _legs_report,
	}
	var steps := 0
	for node_path in CUE_NODES:
		if node_path.contains("Footsteps") and _cues.has(node_path.get_file()):
			steps += _cues[node_path.get_file()]["onsets"]
	if steps == 0:
		_findings.append("no footstep was heard while walking")
	var file := FileAccess.open("%s/playthrough_report.json" % OUTPUT_DIR, FileAccess.WRITE)
	if file != null:
		file.store_string(JSON.stringify(report, "  "))
	print("PLAYTHROUGH_REPORT %s" % JSON.stringify(report))
	_restore_save()
	quit(0 if report["result"] in ["victory", "respawned"] and _findings.is_empty() else 1)

func _victory_reached() -> bool:
	return current_scene != null and str(current_scene.scene_file_path).contains("VictoryScreen")

func _defeat_reached() -> bool:
	return current_scene != null and str(current_scene.scene_file_path).contains("DefeatScreen")

func _is_over() -> bool:
	return current_scene != _scene or not is_instance_valid(_player)

# --- ambience probe ---------------------------------------------------------

func _route_ambience() -> void:
	var layer := _scene.get_node_or_null(AMBIENCE_LAYER)
	if layer == null:
		_findings.append("%s missing: no ambience to measure" % AMBIENCE_LAYER)
		return
	for child in layer.get_children():
		if not (child is AudioStreamPlayer or child is AudioStreamPlayer3D):
			continue
		var bus_name := "Probe_%s" % child.name
		AudioServer.add_bus()
		var index := AudioServer.bus_count - 1
		AudioServer.set_bus_name(index, bus_name)
		AudioServer.set_bus_send(index, "Master")
		child.bus = bus_name
		_ambience_buses.append(bus_name)
		_ambience_sum[bus_name] = 0.0
		_ambience_recent[bus_name] = []

func _sample_ambience() -> void:
	# Mean of the per-frame peak, in dB clamped at the floor: a steady reading
	# of what the loop contributes while the leg is walked, not one spike.
	for bus_name in _ambience_buses:
		var index := AudioServer.get_bus_index(bus_name)
		var peak := maxf(AudioServer.get_bus_peak_volume_left_db(index, 0), AudioServer.get_bus_peak_volume_right_db(index, 0))
		_ambience_sum[bus_name] += maxf(peak, AMBIENCE_FLOOR_DB)
		var recent: Array = _ambience_recent[bus_name]
		recent.append(maxf(peak, AMBIENCE_FLOOR_DB))
		if recent.size() > AMBIENCE_ARRIVAL_TICKS:
			recent.pop_front()
	_ambience_samples += 1

func _take_ambience_levels() -> Dictionary:
	# "leg": mean over the whole walk; "arrival": the last second, standing at
	# the objective -- the reading that shows whether placement layers by place.
	var leg := {}
	var arrival := {}
	for bus_name in _ambience_buses:
		var key := bus_name.trim_prefix("Probe_AmbientLoop_")
		leg[key] = snappedf(_ambience_sum[bus_name] / maxf(_ambience_samples, 1), 0.1)
		var recent: Array = _ambience_recent[bus_name]
		var total := 0.0
		for value in recent:
			total += value
		arrival[key] = snappedf(total / maxf(recent.size(), 1), 0.1)
		_ambience_sum[bus_name] = 0.0
	_ambience_samples = 0
	return {"leg": leg, "arrival": arrival}

func _route_cues() -> void:
	for node_path in CUE_NODES:
		var player := _scene.get_node_or_null(node_path)
		if player == null:
			_findings.append("cue node %s missing" % node_path)
			continue
		var bus_name := "Probe_%s" % player.name
		AudioServer.add_bus()
		var index := AudioServer.bus_count - 1
		AudioServer.set_bus_name(index, bus_name)
		AudioServer.set_bus_send(index, "Master")
		var capture := AudioEffectCapture.new()
		capture.buffer_length = CUE_CAPTURE_SECONDS
		AudioServer.add_bus_effect(index, capture)
		player.bus = bus_name
		_cues[str(player.name)] = {"onsets": 0, "max_db": AMBIENCE_FLOOR_DB}
		_cue_state[str(player.name)] = {"capture": capture, "above": false}

func _sample_cues() -> void:
	var threshold := db_to_linear(AMBIENCE_AUDIBLE_DB)
	for cue_name in _cues:
		var state: Dictionary = _cue_state[cue_name]
		var capture: AudioEffectCapture = state["capture"]
		var available := capture.get_frames_available()
		if available <= 0:
			continue
		var buffer := capture.get_buffer(available)
		var cue: Dictionary = _cues[cue_name]
		var loudest := 0.0
		var start := 0
		while start < buffer.size():
			var block_peak := 0.0
			for i in range(start, mini(start + CUE_BLOCK, buffer.size())):
				block_peak = maxf(block_peak, maxf(absf(buffer[i].x), absf(buffer[i].y)))
			var above := block_peak > threshold
			if above and not state["above"]:
				cue["onsets"] += 1
			state["above"] = above
			loudest = maxf(loudest, block_peak)
			start += CUE_BLOCK
		if loudest > 0.0:
			cue["max_db"] = snappedf(maxf(cue["max_db"], linear_to_db(loudest)), 0.1)

# --- helpers ----------------------------------------------------------------

func _ticks(count: int) -> void:
	for i in count:
		await physics_frame
		_tick += 1
		_observe()

func _observe() -> void:
	if _is_over():
		return
	_sample_ambience()
	_sample_cues()
	var prompt := _hud("OnboardingPrompt")
	if prompt != _last_onboarding:
		_last_onboarding = prompt
		_onboarding_log.append({"t": snappedf(float(_tick) / Engine.physics_ticks_per_second, 0.01), "text": prompt})
	var health := _health_value()
	if _last_health >= 0 and health >= 0 and health < _last_health:
		_damage_events += 1
	_last_health = health

func _turn(yaw_delta: float, pitch_delta: float) -> void:
	var sensitivity: float = _player.get("MouseSensitivity")
	var yaw_step := clampf(yaw_delta, -0.09, 0.09)
	var pitch_step := clampf(pitch_delta, -0.06, 0.06)
	_inject_mouse(Vector2(-yaw_step / sensitivity, -pitch_step / sensitivity))

func _aim_at(target: Node3D, max_steps: int) -> void:
	# Yaw AND pitch: the Mk I strike and the interaction probe are rays straight
	# out of the camera, so a residual tilt misses at close range even when the
	# crosshair looks right. (Lesson from the first-person demo capture.)
	var head := _player.get_node_or_null("Head") as Node3D
	var camera := _player.get_node_or_null("Head/Camera3D") as Node3D
	for i in max_steps:
		if not is_instance_valid(target) or _is_over():
			return
		var aim_point: Vector3 = target.global_position + Vector3(0.0, 0.45, 0.0)
		var to_target: Vector3 = aim_point - _player.global_position
		var yaw_delta: float = wrapf(atan2(-to_target.x, -to_target.z) - _player.rotation.y, -PI, PI)
		var pitch_delta := 0.0
		if head != null and camera != null:
			var eye: Vector3 = camera.global_position
			var flat := Vector2(aim_point.x - eye.x, aim_point.z - eye.z).length()
			pitch_delta = atan2(aim_point.y - eye.y, maxf(flat, 0.01)) - head.rotation.x
		if absf(yaw_delta) < 0.015 and absf(pitch_delta) < 0.015:
			return
		_turn(yaw_delta, pitch_delta)
		await _ticks(1)

func _probe_interaction_ray() -> String:
	var camera := _player.get_node_or_null("Head/Camera3D") as Camera3D
	if camera == null:
		return "no camera"
	var reach: float = _player.get("InteractionRange")
	var from := camera.global_position
	var query := PhysicsRayQueryParameters3D.create(from, from - camera.global_transform.basis.z * reach)
	query.collide_with_areas = true
	query.collide_with_bodies = true
	var hit := _player.get_world_3d().direct_space_state.intersect_ray(query)
	if hit.is_empty():
		return "nothing within %.1f m" % reach
	var collider := hit["collider"] as Node
	return "%s at %.2f m" % [str(_scene.get_path_to(collider)) if collider != null else "?",
		from.distance_to(hit["position"])]

func _send_action(action: String) -> void:
	var press := InputEventAction.new()
	press.action = action
	press.pressed = true
	Input.parse_input_event(press)
	Input.flush_buffered_events()
	await _ticks(2)
	var release := InputEventAction.new()
	release.action = action
	release.pressed = false
	Input.parse_input_event(release)
	Input.flush_buffered_events()

func _inject_mouse(relative: Vector2) -> void:
	var event := InputEventMouseMotion.new()
	event.relative = relative
	event.screen_relative = relative
	Input.parse_input_event(event)
	# Deliver it now. Godot otherwise buffers input until the next rendered
	# frame, and this container renders at ~8 fps while physics ticks at 60,
	# so the steering loop could re-read a rotation that had not yet applied
	# its own queued turns. Flushing keeps the controller's yaw current for
	# every correction.
	Input.flush_buffered_events()

func _hud(path: String) -> String:
	if _is_over():
		return ""
	var label := _scene.get_node_or_null("HUD/" + path) as Label
	return "" if label == null else label.text

func _hud_snapshot() -> String:
	var parts := []
	for p in ["Panel/Margin/VBox/Objective", "Panel/Margin/VBox/Resources",
			"Panel/Margin/VBox/MechanicalArmState", "ActionFeedback"]:
		parts.append(_hud(p))
	return "|".join(parts)

func _health_value() -> int:
	var text := _hud("Panel/Margin/VBox/Health")
	var m := RegEx.create_from_string("(\\d+)\\s*/").search(text)
	return -1 if m == null else int(m.get_string(1))

func _capture(label: String) -> void:
	await process_frame
	await process_frame
	var image := root.get_texture().get_image()
	if image == null or image.is_empty():
		return
	var path := "%s/%02d_%s.png" % [OUTPUT_DIR, _capture_index, label]
	if image.save_png(path) == OK:
		_capture_index += 1

func _flat(v: Vector3) -> Vector3:
	return Vector3(v.x, 0.0, v.z)

func _fmt(v: Vector3) -> String:
	return "(%.1f, %.1f, %.1f)" % [v.x, v.y, v.z]

func _abort(message: String, code: int) -> void:
	printerr("PLAYTHROUGH_ABORT %s" % message)
	_restore_save()
	quit(code)

func _isolate_save() -> void:
	if FileAccess.file_exists(SAVE_PATH):
		DirAccess.rename_absolute(ProjectSettings.globalize_path(SAVE_PATH),
			ProjectSettings.globalize_path(SAVE_BACKUP))

func _restore_save() -> void:
	# Drop the checkpoint this run wrote, then put the player's own save back.
	if FileAccess.file_exists(SAVE_PATH):
		DirAccess.remove_absolute(ProjectSettings.globalize_path(SAVE_PATH))
	if FileAccess.file_exists(SAVE_BACKUP):
		DirAccess.rename_absolute(ProjectSettings.globalize_path(SAVE_BACKUP),
			ProjectSettings.globalize_path(SAVE_PATH))
