extends SceneTree

const OUTPUT_DIR := "res://artifacts/visual-review/phase3a-production-integration"
const WIDTH := 1280
const HEIGHT := 720
const MAIN_SCENE := "res://scenes/Main/Main.tscn"

const VIEWS := [
	{"name":"production_01_spawn_overview","position":Vector3(0.0,2.2,5.0),"target":Vector3(-4.0,1.0,-13.0),"fov":72.0},
	{"name":"production_02_crashed_ship_hero","position":Vector3(-12.0,4.0,2.0),"target":Vector3(-4.0,1.1,-13.0),"fov":60.0},
	{"name":"production_03_ship_rear_engines","position":Vector3(7.5,3.2,-21.0),"target":Vector3(-4.0,1.0,-13.0),"fov":56.0},
	{"name":"production_04_resource_workbench_zone","position":Vector3(8.0,3.0,1.0),"target":Vector3(9.5,0.9,-10.0),"fov":62.0},
	{"name":"production_05_savepoint_beacon_zone","position":Vector3(-8.0,3.2,-6.0),"target":Vector3(18.0,1.3,-18.0),"fov":68.0},
	{"name":"production_06_galaxabrain_combat_distance","position":Vector3(14.0,2.4,-9.0),"target":Vector3(20.0,1.0,-16.0),"fov":50.0},
	{"name":"production_07_mechanical_arm_first_person","position":Vector3(0.2,1.8,0.25),"target":Vector3(0.4,1.35,-2.0),"fov":75.0},
	{"name":"production_08_wide_terrain_composition","position":Vector3(-24.0,9.0,18.0),"target":Vector3(0.0,0.9,-18.0),"fov":78.0}
]

func _initialize() -> void:
	root.size = Vector2i(WIDTH, HEIGHT)
	call_deferred("_capture_all")

func _capture_all() -> void:
	var dir := DirAccess.open("res://")
	if dir == null:
		printerr("Unable to open project root for visual review output.")
		quit(1)
	dir.make_dir_recursive("artifacts/visual-review/phase3a-production-integration")

	var packed := load(MAIN_SCENE) as PackedScene
	if packed == null:
		printerr("Unable to load %s" % MAIN_SCENE)
		quit(2)

	var scene := packed.instantiate()
	root.add_child(scene)
	_disable_runtime_ui(scene)

	var camera := Camera3D.new()
	camera.name = "Phase3AProductionReviewCamera"
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
