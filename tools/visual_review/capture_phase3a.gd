extends SceneTree

const OUTPUT_DIR := "res://artifacts/visual-review"
const WIDTH := 1280
const HEIGHT := 720
const MAIN_SCENE := "res://scenes/Main/Main.tscn"

const VIEWS := [
	{
		"name": "phase3a_spawn_overview",
		"position": Vector3(0.0, 2.0, 4.5),
		"target": Vector3(-3.5, 1.2, -16.0),
		"fov": 72.0
	},
	{
		"name": "phase3a_ship_hero",
		"position": Vector3(-17.0, 3.0, -12.5),
		"target": Vector3(-2.8, 1.1, -18.3),
		"fov": 66.0
	},
	{
		"name": "phase3a_ship_oblique",
		"position": Vector3(7.5, 2.8, -31.0),
		"target": Vector3(-2.0, 1.0, -18.2),
		"fov": 66.0
	},
	{
		"name": "phase3a_galaxabrain_combat",
		"position": Vector3(15.0, 2.0, -12.0),
		"target": Vector3(20.0, 1.0, -16.0),
		"fov": 50.0
	},
	{
		"name": "phase3a_interactables_wide",
		"position": Vector3(2.0, 5.0, 8.0),
		"target": Vector3(2.0, 0.9, -10.5),
		"fov": 74.0
	}
]

func _initialize() -> void:
	root.size = Vector2i(WIDTH, HEIGHT)
	call_deferred("_capture_all")

func _capture_all() -> void:
	var dir := DirAccess.open("res://")
	if dir == null:
		printerr("Unable to open project root for visual review output.")
		quit(1)
	if not dir.dir_exists("artifacts"):
		dir.make_dir("artifacts")
	if not dir.dir_exists("artifacts/visual-review"):
		dir.make_dir_recursive("artifacts/visual-review")

	var packed := load(MAIN_SCENE) as PackedScene
	if packed == null:
		printerr("Unable to load %s" % MAIN_SCENE)
		quit(1)

	var scene := packed.instantiate()
	root.add_child(scene)
	_hide_review_obstructions(scene)

	var camera := Camera3D.new()
	camera.name = "Phase3AReviewCamera"
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
			quit(2)
		var path := "%s/%s.png" % [OUTPUT_DIR, view["name"]]
		var save_error := image.save_png(path)
		if save_error != OK:
			printerr("Failed to save %s with error %s" % [path, save_error])
			quit(3)
		print("CAPTURED %s position=%s target=%s fov=%.1f size=%dx%d" % [path, view["position"], view["target"], view["fov"], WIDTH, HEIGHT])

	quit(0)

func _hide_review_obstructions(scene: Node) -> void:
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
