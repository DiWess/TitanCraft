extends SceneTree

## Captures review screenshots of the MVP Asset Pack V1 scene replacement in Main.tscn.
## Run: godot --headless --path . --script tools/visual_review/capture_mvp_scene_replacement.gd

const OUTPUT_DIR := "res://artifacts/visual-review/mvp-scene-replacement"
const WIDTH := 1280
const HEIGHT := 720
const MAIN_SCENE := "res://scenes/Main/Main.tscn"

const VIEWS := [
	{"name":"mvp_01_workbench_zone","position":Vector3(9.0,2.4,-8.5),"target":Vector3(12.0,1.4,-12.0),"fov":58.0},
	{"name":"mvp_02_savepoint","position":Vector3(-9.5,2.2,-9.0),"target":Vector3(-12.0,1.4,-12.0),"fov":55.0},
	{"name":"mvp_03_beacon_objective","position":Vector3(24.0,2.6,-16.0),"target":Vector3(28.0,1.4,-20.0),"fov":58.0},
	{"name":"mvp_04_scout_arena","position":Vector3(16.0,2.2,-12.5),"target":Vector3(20.0,1.6,-16.0),"fov":55.0},
	{"name":"mvp_05_metal_drop","position":Vector3(6.5,1.8,-2.5),"target":Vector3(8.0,0.8,-4.0),"fov":50.0},
	{"name":"mvp_06_biomass_drop","position":Vector3(-6.5,1.8,-2.5),"target":Vector3(-8.0,0.8,-4.0),"fov":50.0},
	{"name":"mvp_07_electronics_drop","position":Vector3(1.6,1.8,-8.2),"target":Vector3(0.0,0.8,-10.0),"fov":50.0},
	{"name":"mvp_08_debris_route","position":Vector3(7.0,2.6,-7.5),"target":Vector3(9.3,1.2,-10.5),"fov":60.0},
	{"name":"mvp_09_spawn_composition","position":Vector3(0.0,2.4,4.0),"target":Vector3(12.0,1.0,-12.0),"fov":75.0}
]

func _initialize() -> void:
	root.size = Vector2i(WIDTH, HEIGHT)
	call_deferred("_capture_all")

func _capture_all() -> void:
	var dir := DirAccess.open("res://")
	if dir == null:
		printerr("Unable to open project root for visual review output.")
		quit(1)
	dir.make_dir_recursive("artifacts/visual-review/mvp-scene-replacement")

	var packed := load(MAIN_SCENE) as PackedScene
	if packed == null:
		printerr("Unable to load %s" % MAIN_SCENE)
		quit(2)

	var scene := packed.instantiate()
	root.add_child(scene)
	_disable_runtime_ui(scene)

	var camera := Camera3D.new()
	camera.name = "MvpSceneReplacementReviewCamera"
	camera.current = true
	scene.add_child(camera)

	for i in 8:
		await process_frame

	for view in VIEWS:
		camera.fov = view["fov"]
		camera.global_position = view["position"]
		camera.look_at(view["target"], Vector3.UP)
		for i in 3:
			await process_frame
		var image := root.get_texture().get_image()
		if image == null or image.is_empty():
			printerr("Viewport image was empty for %s" % view["name"])
			quit(3)
		var path := "%s/%s.png" % [OUTPUT_DIR, view["name"]]
		var save_error := image.save_png(path)
		if save_error != OK:
			printerr("Failed to save %s with error %s" % [path, save_error])
			quit(4)
		print("CAPTURED %s position=%s target=%s fov=%.1f size=%dx%d" % [path, view["position"], view["target"], view["fov"], WIDTH, HEIGHT])

	quit(0)

func _disable_runtime_ui(scene: Node) -> void:
	for node in scene.find_children("*", "CanvasLayer", true, false):
		if node.has_method("hide"):
			node.hide()
		node.process_mode = Node.PROCESS_MODE_DISABLED
	for node in scene.find_children("*", "Control", true, false):
		if node is CanvasItem:
			node.visible = false
		node.process_mode = Node.PROCESS_MODE_DISABLED
	for node_name in ["HUD", "PauseMenu", "HudBinder", "EndScreenNavigator"]:
		var node := scene.get_node_or_null(NodePath(node_name))
		if node is CanvasItem:
			node.visible = false
		elif node is Node:
			node.process_mode = Node.PROCESS_MODE_DISABLED
	var player := scene.get_node_or_null(NodePath("Player"))
	if player is Node:
		player.process_mode = Node.PROCESS_MODE_DISABLED
