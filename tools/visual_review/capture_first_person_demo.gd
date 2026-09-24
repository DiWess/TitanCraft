extends SceneTree

# Records a scripted first-person run through the Mwezi Quarter as a numbered
# PNG sequence, for assembly into an animation by
# tools/visual_review/build_first_person_demo_gif.py.
#
# Why this drives real input instead of posing the camera
# -------------------------------------------------------
# The point is to show movement and combat FEEL: stride-locked view bob, strafe
# lean, viewmodel sway, landing impact, weapon kick, hit markers, the
# damage-direction arc. All of it is produced by FirstPersonController and
# CameraShaker reacting to real input. A camera flown along a path would show
# the architecture and none of the feel, and would look identical whether or
# not those systems worked at all.
#
# So this presses the game's own actions and injects real mouse motion, then
# films the player's own camera. The Galaxabrain Scout runs its own AI: it
# detects, chases and attacks by itself, which is what produces the damage
# indicator rather than a staged trigger.
#
# Arming the player uses the real loop too. FirstPersonController.Inventory is
# a plain C# property and therefore invisible to GDScript (get() returns null),
# so rather than adding debug API to gameplay code, the run collects the three
# resources and crafts the Mk I at the workbench through TryInteract, exactly
# as a player would. That setup is deliberately NOT filmed -- capture is off
# until the showcase begins -- because the onboarding capture already covers
# collecting and crafting, and this recording is about how the game feels to
# move and fight in.
#
# Run:
#   xvfb-run -a godot --path . --script tools/visual_review/capture_first_person_demo.gd

const OUTPUT_DIR := "res://artifacts/visual-review/first-person-demo"
const WIDTH := 960
const HEIGHT := 540
const MAIN_SCENE := "res://scenes/Main/Main.tscn"

# Capture every Nth rendered frame. The engine ticks at 60 fps, so 5 gives a
# 12 fps recording: fast enough to read a weapon kick, slow enough to keep the
# assembled animation a sane size.
const CAPTURE_EVERY := 5

# Where the filmed part starts: the workbench-courtyard side of the harbour
# plaza, inside the Scout's 12 m detection range so the encounter happens on
# its own.
const SHOWCASE_START := Vector3(13.5, 1.4, -9.0)

var _frame_index := 0
var _captured := 0
var _capturing := false
var _player: CharacterBody3D = null
var _scene: Node = null

func _initialize() -> void:
	root.size = Vector2i(WIDTH, HEIGHT)
	call_deferred("_run")

func _run() -> void:
	var dir := DirAccess.open("res://")
	if dir == null:
		printerr("Unable to open project root.")
		quit(1)
		return
	dir.make_dir_recursive("artifacts/visual-review/first-person-demo")

	var packed := load(MAIN_SCENE) as PackedScene
	if packed == null:
		printerr("Unable to load %s" % MAIN_SCENE)
		quit(2)
		return
	_scene = packed.instantiate()
	root.add_child(_scene)
	await process_frame

	# The intro title card would sit over the whole recording. The HUD stays:
	# the crosshair, hit marker and damage arc are part of what this shows.
	var title_card := _scene.get_node_or_null("IntroTitleCard")
	if title_card != null:
		title_card.visible = false

	_player = _scene.get_node_or_null("Player") as CharacterBody3D
	if _player == null:
		printerr("Player not found in scene.")
		quit(3)
		return

	for i in 10:
		await process_frame

	# The Scout is parked while the player is teleported around collecting
	# resources. Left running it detects the player at the workbench, closes,
	# and lands eight hits during setup -- the first take began the showcase
	# at 20/100 health and hit the death screen four seconds in.
	var scout := _scene.get_node_or_null("Placeholder_GalaxabrainScout") as Node3D
	var scout_home := Vector3.ZERO
	if scout != null:
		scout_home = scout.global_position
		scout.process_mode = Node.PROCESS_MODE_DISABLED

	if not await _prepare_mechanical_arm():
		printerr("FIRST_PERSON_DEMO_SETUP_FAILED could not craft the Mk I")
		quit(4)
		return

	# --- filmed from here -----------------------------------------------
	_player.global_position = SHOWCASE_START
	# The Scout stays parked through the movement beats and is released at the
	# combat beat. Released early it crosses the plaza during the walk and
	# kills the player mid-demo -- the second take died four seconds in.
	if scout != null:
		scout.global_position = scout_home
	await _advance(12, false)

	# The showcase is only honest if it starts from a clean slate; the HUD is
	# the one player-visible source of truth reachable from here.
	var health := _read_hud_health()
	if health != "Health: 100/100":
		printerr("FIRST_PERSON_DEMO_SETUP_FAILED player entered the showcase at '%s'" % health)
		quit(5)
		return

	_capturing = true

	await _beat_look_around()
	await _beat_walk_and_strafe()
	await _beat_jump()
	await _beat_sprint()
	await _beat_face_the_scout(scout)
	await _beat_attack(scout)
	await _beat_settle(scout)

	print("FIRST_PERSON_DEMO_FRAMES %d" % _captured)
	quit(0)

# --- setup (not filmed) -----------------------------------------------------

func _prepare_mechanical_arm() -> bool:
	for pickup_name in ["ResourceDrop_MetalPickup", "ResourceDrop_BiomassPickup",
						"ResourceDrop_ElectronicsPickup"]:
		if not await _walk_up_and_interact(pickup_name):
			printerr("setup: could not collect %s" % pickup_name)
			return false
	return await _walk_up_and_interact("Placeholder_Workbench")

func _walk_up_and_interact(node_name: String) -> bool:
	var target := _scene.get_node_or_null(node_name) as Node3D
	if target == null:
		return false
	# Stand 1.8 m away on the -Z side, inside the 3 m interaction range.
	var stand := target.global_position + Vector3(0.0, 0.35, 1.8)
	_player.global_position = stand
	await _advance(4, false)
	await _aim_at(target, 40, false)
	for attempt in 6:
		if _player.call("TryInteract"):
			await _advance(4, false)
			return true
		await _advance(4, false)
	return false

# --- filmed beats -----------------------------------------------------------

func _beat_look_around() -> void:
	# A slow pan establishes the quarter and shows idle viewmodel sway.
	await _look_over(34, Vector2(-7.0, 0.0))
	await _look_over(44, Vector2(6.0, 0.0))
	await _look_over(16, Vector2(0.0, -2.0))

func _beat_walk_and_strafe() -> void:
	# Deliberately short. Headless capture renders well below real time while
	# physics keeps its fixed tick, so each rendered frame carries the player
	# much further than 1/60 s would: the first cut walked straight into a
	# courtyard wall and spent two beats staring at stonework.
	Input.action_press("move_forward")
	await _advance(22)
	# Strafe both ways to show the camera lean.
	Input.action_press("move_right")
	await _advance(12)
	Input.action_release("move_right")
	Input.action_press("move_left")
	await _advance(12)
	Input.action_release("move_left")
	Input.action_release("move_forward")
	await _advance(10)

func _beat_jump() -> void:
	Input.action_press("jump")
	await _advance(3)
	Input.action_release("jump")
	# Long enough to land and show the impact dip.
	await _advance(45)

func _beat_sprint() -> void:
	Input.action_press("sprint")
	Input.action_press("move_forward")
	await _advance(24)
	Input.action_release("move_forward")
	Input.action_release("sprint")
	await _advance(14)

func _read_hud_health() -> String:
	var label := _scene.get_node_or_null("HUD/Panel/Margin/VBox/Health") as Label
	return "" if label == null else label.text

func _beat_face_the_scout(scout: Node3D) -> void:
	if scout == null:
		await _advance(18)
		return
	# Stage the encounter: put the Scout a readable distance ahead of wherever
	# the movement beats left the player, then hand it back its own AI. From
	# here on it detects, closes and attacks entirely on its own.
	var ahead := -_player.global_transform.basis.z
	ahead.y = 0.0
	scout.global_position = _player.global_position + ahead.normalized() * 8.5 - Vector3(0.0, 0.2, 0.0)
	scout.process_mode = Node.PROCESS_MODE_INHERIT
	await _aim_at(scout, 24, true)
	await _advance(6)
	print("DEMO_ENCOUNTER distance=%.1f health=%s" % [
		_player.global_position.distance_to(scout.global_position), _read_hud_health()])

func _beat_attack(scout: Node3D) -> void:
	# Close only to strike range and stop. Walking blind into the Scout put the
	# player at 0.8 m, where every swing missed and the Scout killed them in
	# four exchanges -- Mk I range is 3.0 m, so the fight belongs at about 2.4.
	Input.action_press("move_forward")
	for i in 60:
		if scout == null or _player.global_position.distance_to(scout.global_position) <= 2.4:
			break
		await _advance(1)
	Input.action_release("move_forward")
	await _advance(6)
	for swing in 4:
		await _aim_at(scout, 8, true)
		var landed: bool = _player.call("TryAttack")
		print("DEMO_SWING %d landed=%s distance=%.1f health=%s" % [
			swing, landed,
			_player.global_position.distance_to(scout.global_position) if scout != null else -1.0,
			_read_hud_health()])
		await _advance(24)

func _beat_settle(scout: Node3D) -> void:
	# Park the Scout again for the outro. Left live it kept swinging through
	# the hold and the reel ended on the death screen, which is not what this
	# recording is for.
	if scout != null:
		scout.process_mode = Node.PROCESS_MODE_DISABLED
	await _settle_body()

func _settle_body() -> void:
	# Hold on the aftermath: any damage arc from the Scout's own attacks plays
	# out here, and the camera settles back to rest.
	await _look_over(14, Vector2(3.0, 0.0))
	await _advance(16)

# --- helpers ----------------------------------------------------------------

func _aim_at(target: Node3D, max_steps: int, capture: bool) -> void:
	# Turn through injected mouse motion rather than by setting rotation, so
	# the turn drives viewmodel sway exactly as a player's would -- and so the
	# controller's own yaw and pitch stay authoritative.
	#
	# Pitch matters as much as yaw here. The Mk I strike is a hitscan ray cast
	# straight out of the camera, so the look-around beat's residual upward
	# tilt (about 4.6 degrees) was enough to send every swing over the Scout's
	# head at 2 m. Yaw-only aiming looked correct on screen and missed every
	# time.
	var sensitivity: float = _player.get("MouseSensitivity")
	var head := _player.get_node_or_null("Head") as Node3D
	var camera := _player.get_node_or_null("Head/Camera3D") as Node3D
	for i in max_steps:
		var aim_point: Vector3 = target.global_position + Vector3(0.0, 0.55, 0.0)
		var to_target: Vector3 = aim_point - _player.global_position
		var desired_yaw := atan2(-to_target.x, -to_target.z)
		var yaw_delta: float = wrapf(desired_yaw - _player.rotation.y, -PI, PI)

		var pitch_delta := 0.0
		if head != null and camera != null:
			var eye: Vector3 = camera.global_position
			var flat := Vector2(aim_point.x - eye.x, aim_point.z - eye.z).length()
			var desired_pitch := atan2(aim_point.y - eye.y, maxf(flat, 0.01))
			pitch_delta = desired_pitch - head.rotation.x

		if absf(yaw_delta) < 0.015 and absf(pitch_delta) < 0.015:
			break
		var yaw_step: float = clampf(yaw_delta, -0.09, 0.09)
		var pitch_step: float = clampf(pitch_delta, -0.06, 0.06)
		_inject_mouse(Vector2(-yaw_step / sensitivity, -pitch_step / sensitivity))
		await _advance(1, capture)

func _look_over(frames: int, per_frame: Vector2) -> void:
	for i in frames:
		_inject_mouse(per_frame)
		await _advance(1)

func _inject_mouse(relative: Vector2) -> void:
	var event := InputEventMouseMotion.new()
	event.relative = relative
	event.screen_relative = relative
	Input.parse_input_event(event)

func _advance(frames: int, capture: bool = true) -> void:
	for i in frames:
		await process_frame
		_frame_index += 1
		if capture and _capturing and _frame_index % CAPTURE_EVERY == 0:
			_save_frame()

func _save_frame() -> void:
	var image := root.get_texture().get_image()
	if image == null or image.is_empty():
		return
	var path := "%s/frame_%04d.png" % [OUTPUT_DIR, _captured]
	if image.save_png(path) != OK:
		printerr("Failed to save %s" % path)
		return
	_captured += 1
