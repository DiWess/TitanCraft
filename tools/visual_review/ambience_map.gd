extends SceneTree

# Measures how the Mwezi Quarter's ambience layers by place.
#
# The walked journey (playthrough_journey.gd) proves an ambient loop is audible
# on every leg, on real input. It cannot show placement: the sea's swell moves
# its level by about 16 dB every 8 s, which swamps a distance difference in the
# second a player stands at an objective. So this probe stands the listener at
# fixed stations for one full 24 s loop each and averages every ambient bus.
#
# It places the player directly -- it is a measurement of the mix, not a
# playthrough, and claims nothing about routes. Audio mixes on wall-clock time,
# so each station is timed in real milliseconds, not frames.
#
# Each ambient player is routed to its own bus for the run; the game's bus
# layout is not changed on disk.
#
# Run: xvfb-run -a godot --path . --script tools/visual_review/ambience_map.gd
# Output: artifacts/visual-review/ambience/ambience_map.json

const MAIN_SCENE := "res://scenes/Main/Main.tscn"
const OUTPUT_DIR := "res://artifacts/visual-review/ambience"
const LAYER := "AudioLayer_Ambient"
const FLOOR_DB := -80.0
const LOOP_MS := 24_000
const SETTLE_MS := 500

# Station name -> node the listener stands beside, or a fixed x,z.
const STATIONS := [
	["spawn", "@0,0"],
	["workbench", "Placeholder_Workbench"],
	["wreck_save_point", "Placeholder_SavePoint"],
	["scout_arena", "Placeholder_GalaxabrainScout"],
	["harbour_beacon", "Placeholder_Beacon"],
]

var _scene: Node
var _player: CharacterBody3D
var _buses: Array[String] = []

func _initialize() -> void:
	call_deferred("_run")

func _run() -> void:
	DirAccess.make_dir_recursive_absolute(ProjectSettings.globalize_path(OUTPUT_DIR))
	_scene = (load(MAIN_SCENE) as PackedScene).instantiate()
	root.add_child(_scene)
	current_scene = _scene
	_player = _scene.get_node("Player") as CharacterBody3D
	# The Scout would hunt a player parked in its arena; this is a mix
	# measurement, so take it out of play.
	var scout := _scene.get_node_or_null("Placeholder_GalaxabrainScout")
	var scout_position := Vector3.INF
	if scout != null:
		scout_position = scout.global_position
		scout.process_mode = Node.PROCESS_MODE_DISABLED
	_route()
	await process_frame
	var ground_y := _player.global_position.y
	var report := {"loop_ms": LOOP_MS, "note": "mean of per-frame bus peak, dB, floor %.0f" % FLOOR_DB, "stations": []}
	for station in STATIONS:
		var at := _station_position(station[1], scout_position)
		_player.global_position = Vector3(at.x, ground_y, at.z)
		_player.velocity = Vector3.ZERO
		var levels := await _measure()
		var entry := {"station": station[0], "position": [snappedf(at.x, 0.1), snappedf(at.z, 0.1)], "levels_db": levels}
		report["stations"].append(entry)
		print("AMBIENCE_STATION %s" % JSON.stringify(entry))
	var file := FileAccess.open("%s/ambience_map.json" % OUTPUT_DIR, FileAccess.WRITE)
	file.store_string(JSON.stringify(report, "  "))
	quit(0)

func _station_position(target: String, scout_position: Vector3) -> Vector3:
	if target.begins_with("@"):
		var xz := target.substr(1).split(",")
		return Vector3(float(xz[0]), 0.0, float(xz[1]))
	if target == "Placeholder_GalaxabrainScout":
		return scout_position
	# Stand 2 m off the object, toward spawn, where a player uses it.
	var node := _scene.get_node(target) as Node3D
	var p := node.global_position
	var back := Vector3(-p.x, 0.0, -p.z).normalized() * 2.0
	return p + back

func _route() -> void:
	for child in _scene.get_node(LAYER).get_children():
		if not (child is AudioStreamPlayer or child is AudioStreamPlayer3D):
			continue
		var bus_name := "Probe_%s" % child.name
		AudioServer.add_bus()
		var index := AudioServer.bus_count - 1
		AudioServer.set_bus_name(index, bus_name)
		AudioServer.set_bus_send(index, "Master")
		child.bus = bus_name
		_buses.append(bus_name)

func _measure() -> Dictionary:
	var settle := Time.get_ticks_msec() + SETTLE_MS
	while Time.get_ticks_msec() < settle:
		await process_frame
	var sums := {}
	for bus_name in _buses:
		sums[bus_name] = 0.0
	var samples := 0
	var until := Time.get_ticks_msec() + LOOP_MS
	while Time.get_ticks_msec() < until:
		await process_frame
		for bus_name in _buses:
			var index := AudioServer.get_bus_index(bus_name)
			var peak := maxf(AudioServer.get_bus_peak_volume_left_db(index, 0), AudioServer.get_bus_peak_volume_right_db(index, 0))
			sums[bus_name] += maxf(peak, FLOOR_DB)
		samples += 1
	var levels := {}
	for bus_name in _buses:
		levels[bus_name.trim_prefix("Probe_AmbientLoop_")] = snappedf(sums[bus_name] / maxf(samples, 1), 0.1)
	return levels
