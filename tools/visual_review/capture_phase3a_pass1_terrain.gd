extends SceneTree

const OUTPUT_DIR := "res://artifacts/visual-review/phase3a-pass1-terrain"
const WIDTH := 1280
const HEIGHT := 720
const MAIN_SCENE := "res://scenes/Main/Main.tscn"

const VIEWS := [
	{"name":"terrain_01_spawn_route","position":Vector3(0.0,2.5,6.0),"target":Vector3(2.0,0.9,-12.0),"fov":72.0},
	{"name":"terrain_02_foreground_midground","position":Vector3(-20.0,4.2,8.0),"target":Vector3(-4.0,0.9,-18.0),"fov":68.0},
	{"name":"terrain_03_combat_zone","position":Vector3(13.0,3.4,-8.0),"target":Vector3(23.0,1.0,-23.0),"fov":62.0},
	{"name":"terrain_04_wide_crash_site","position":Vector3(-34.0,10.0,22.0),"target":Vector3(2.0,1.0,-27.0),"fov":78.0},
]

func _initialize() -> void:
	root.size = Vector2i(WIDTH, HEIGHT)
	call_deferred("_capture_all")

func _capture_all() -> void:
	var dir := DirAccess.open("res://")
	if dir == null:
		printerr("Unable to open project root.")
		quit(1)
	dir.make_dir_recursive("artifacts/visual-review/phase3a-pass1-terrain")

	var packed := load(MAIN_SCENE) as PackedScene
	if packed == null:
		printerr("Unable to load %s" % MAIN_SCENE)
		quit(2)

	var scene := packed.instantiate()
	root.add_child(scene)
	_hide_runtime_nodes(scene)

	var camera := Camera3D.new()
	camera.name = "Phase3APass1TerrainReviewCamera"
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

func _hide_runtime_nodes(scene: Node) -> void:
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
