extends SceneTree
# Captures the crafted mechanical-arm first-person state through the real player
# camera rig. Closes the evidence gap recorded in
# docs/production/stage-c-integration-validation-2026-07-18.md (finding #1):
# production_07 could not show the arm because it stays hidden until crafted.
# The crafted state itself is inventory-driven and covered by integration tests;
# this script only toggles the script-controlled visibility node for the render.

const OUTPUT_DIR := "res://artifacts/visual-review/crafted-arm-first-person"
const WIDTH := 1280
const HEIGHT := 720
const MAIN_SCENE := "res://scenes/Main/Main.tscn"

# Yaw is applied to the player body, pitch to the head, matching the runtime rig.
# arm_built false = default pre-craft state (bare astronaut arm visible);
# arm_built true simulates the crafted state's visibility swap.
const VIEWS := [
	{"name": "precraft_arm_01_camp_backdrop", "yaw_degrees": -45.0, "pitch_degrees": -6.0, "arm_built": false},
	{"name": "crafted_arm_01_camp_backdrop", "yaw_degrees": -45.0, "pitch_degrees": -6.0, "arm_built": true},
	{"name": "crafted_arm_02_crash_hull_backdrop", "yaw_degrees": 17.0, "pitch_degrees": -4.0, "arm_built": true},
]

func _initialize() -> void:
	root.size = Vector2i(WIDTH, HEIGHT)
	call_deferred("_capture_all")

func _capture_all() -> void:
	var dir := DirAccess.open("res://")
	if dir == null:
		printerr("Unable to open project root for visual review output.")
		quit(1)
		return
	dir.make_dir_recursive("artifacts/visual-review/crafted-arm-first-person")

	var packed := load(MAIN_SCENE) as PackedScene
	if packed == null:
		printerr("Unable to load %s" % MAIN_SCENE)
		quit(2)
		return

	var scene := packed.instantiate()
	root.add_child(scene)
	_disable_runtime_ui(scene)

	var player := scene.get_node_or_null("Player")
	if player == null:
		printerr("Player node not found in %s" % MAIN_SCENE)
		quit(3)
		return
	var head := player.get_node_or_null("Head")
	var camera := player.get_node_or_null("Head/Camera3D") as Camera3D
	var arm_visual := player.get_node_or_null("Head/Camera3D/MechanicalArmVisual")
	var bare_arm_visual := player.get_node_or_null("Head/Camera3D/BareArmVisual")
	if head == null or camera == null or arm_visual == null or bare_arm_visual == null:
		printerr("Player camera rig, MechanicalArmVisual, or BareArmVisual not found.")
		quit(4)
		return

	camera.current = true

	for i in 8:
		await process_frame

	for view in VIEWS:
		arm_visual.visible = view["arm_built"]
		bare_arm_visual.visible = not view["arm_built"]
		player.rotation_degrees.y = view["yaw_degrees"]
		head.rotation_degrees.x = view["pitch_degrees"]
		for i in 3:
			await process_frame
		var image := root.get_texture().get_image()
		if image == null or image.is_empty():
			printerr("Viewport image was empty for %s" % view["name"])
			quit(5)
			return
		var path := "%s/%s.png" % [OUTPUT_DIR, view["name"]]
		var save_error := image.save_png(path)
		if save_error != OK:
			printerr("Failed to save %s with error %s" % [path, save_error])
			quit(6)
			return
		print("CRAFTED_ARM_CAPTURE_SAVED %s" % path)

	quit(0)

func _disable_runtime_ui(scene: Node) -> void:
	for node in scene.find_children("*", "CanvasLayer", true, false):
		if node.has_method("hide"):
			node.hide()
