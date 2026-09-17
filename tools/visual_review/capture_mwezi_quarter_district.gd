extends SceneTree

# Evidence capture for the Mwezi Quarter district integration and the combat
# feedback overlay.
#
# Two things the existing phase3a capture cannot show:
#   1. Player-eye-height views along the mission route. The district is walked
#      through, not inspected from 3 m up, and axis 5 (world / level design) is
#      judged on what the player actually sees.
#   2. The HUD and the combat feedback overlay. capture_phase3a_production_
#      integration.gd disables every CanvasLayer before shooting, so the
#      crosshair, hit marker and damage-direction indicator are invisible to it.

const OUTPUT_DIR := "res://artifacts/visual-review/mwezi-quarter-district"
const WIDTH := 1280
const HEIGHT := 720
const MAIN_SCENE := "res://scenes/Main/Main.tscn"
const EYE_HEIGHT := 1.65

# Player-eye views walking the mission route, in loop order.
const WORLD_VIEWS := [
	{"name":"district_01_spawn_eye_level","position":Vector3(0.0,EYE_HEIGHT,1.5),"target":Vector3(0.0,1.4,-10.0),"fov":75.0},
	{"name":"district_02_west_alley_to_savepoint","position":Vector3(-7.0,EYE_HEIGHT,-5.0),"target":Vector3(-12.0,1.2,-12.0),"fov":75.0},
	{"name":"district_03_east_alley_to_metal","position":Vector3(4.0,EYE_HEIGHT,-2.0),"target":Vector3(9.0,1.2,-5.0),"fov":75.0},
	{"name":"district_04_workbench_courtyard","position":Vector3(9.5,EYE_HEIGHT,-9.0),"target":Vector3(12.0,1.2,-13.0),"fov":75.0},
	{"name":"district_05_arcade_from_courtyard","position":Vector3(11.0,EYE_HEIGHT,-15.0),"target":Vector3(5.4,2.4,-17.0),"fov":75.0},
	{"name":"district_06_plaza_and_tower","position":Vector3(14.0,EYE_HEIGHT,-13.0),"target":Vector3(22.6,5.0,-26.6),"fov":75.0},
	{"name":"district_07_beacon_on_the_harbour_front","position":Vector3(23.0,EYE_HEIGHT,-17.0),"target":Vector3(28.0,1.4,-20.0),"fov":75.0},
	{"name":"district_08_terrace_overlook","position":Vector3(-2.0,EYE_HEIGHT,-17.0),"target":Vector3(-6.6,2.0,-20.6),"fov":75.0},
]

func _initialize() -> void:
	root.size = Vector2i(WIDTH, HEIGHT)
	call_deferred("_capture_all")

func _capture_all() -> void:
	var dir := DirAccess.open("res://")
	if dir == null:
		printerr("Unable to open project root for visual review output.")
		quit(1)
	dir.make_dir_recursive("artifacts/visual-review/mwezi-quarter-district")

	var packed := load(MAIN_SCENE) as PackedScene
	if packed == null:
		printerr("Unable to load %s" % MAIN_SCENE)
		quit(2)

	var scene := packed.instantiate()
	root.add_child(scene)
	for i in 8:
		await process_frame

	# World views: a free camera with every CanvasLayer hidden, so neither the
	# HUD nor the intro title card occludes the architecture being reviewed.
	var hud := scene.get_node_or_null("HUD")
	for layer in scene.find_children("*", "CanvasLayer", true, false):
		layer.visible = false
	var camera := Camera3D.new()
	camera.name = "MweziQuarterReviewCamera"
	camera.current = true
	scene.add_child(camera)
	for view in WORLD_VIEWS:
		camera.fov = view["fov"]
		camera.global_position = view["position"]
		camera.look_at(view["target"], Vector3.UP)
		for i in 3:
			await process_frame
		if not await _save(view["name"]):
			quit(3)

	# HUD views: hand the camera back to the player and show the overlay.
	camera.current = false
	# Only the HUD comes back for the overlay shots; the title card stays hidden.
	if hud != null:
		hud.visible = true
	var player := scene.get_node_or_null("Player")
	var player_camera := scene.get_node_or_null("Player/Head/Camera3D") as Camera3D
	if player != null and player_camera != null:
		player_camera.current = true
		for i in 4:
			await process_frame
		if not await _save("district_09_first_person_hud"):
			quit(3)

		var overlay := _find_overlay(scene)
		if overlay != null:
			# Non-lethal hit marker plus a damage arc from the Scout's side, so
			# both combat feedback states are on one reviewable frame.
			overlay.call("ShowHitMarker", false)
			overlay.call("ShowDamageFrom", player.global_position, 0.0, Vector3(20.0, 1.0, -16.0))
			for i in 2:
				await process_frame
			if not await _save("district_10_hit_marker_and_damage_direction"):
				quit(3)

			overlay.call("ShowHitMarker", true)
			for i in 2:
				await process_frame
			if not await _save("district_11_lethal_hit_marker"):
				quit(3)

	quit(0)

func _find_overlay(_scene: Node) -> Node:
	# Resolved through the same group the player controller uses, rather than by
	# node name: the overlay lives inside the instanced HUD scene.
	return get_first_node_in_group("combat_feedback_overlay")

func _save(name: String) -> bool:
	var image := root.get_texture().get_image()
	if image == null or image.is_empty():
		printerr("Viewport image was empty for %s" % name)
		return false
	var path := "%s/%s.png" % [OUTPUT_DIR, name]
	var save_error := image.save_png(path)
	if save_error != OK:
		printerr("Failed to save %s with error %s" % [path, save_error])
		return false
	print("CAPTURED %s" % path)
	return true
