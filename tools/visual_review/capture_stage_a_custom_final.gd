extends SceneTree

const OUTPUT_DIR := "res://artifacts/visual-review/stage-a-custom-final"
const MAIN_SCENE := "res://scenes/Main/Main.tscn"
const WIDTH := 1280
const HEIGHT := 720
const VIEWS := [
	{"name":"stage_a_custom_01_player_spawn", "position":Vector3(0.0,2.2,5.0), "target":Vector3(-4.5,1.0,-13.0), "fov":72.0},
	{"name":"stage_a_custom_02_wreck_three_quarter", "position":Vector3(-12.0,4.0,2.0), "target":Vector3(-4.0,1.0,-13.0), "fov":60.0},
	{"name":"stage_a_custom_03_wide_composition", "position":Vector3(-24.0,9.0,18.0), "target":Vector3(0.0,0.9,-18.0), "fov":78.0}
]

func _initialize() -> void:
	root.size = Vector2i(WIDTH, HEIGHT)
	call_deferred("_capture_all")

func _capture_all() -> void:
	DirAccess.open("res://").make_dir_recursive("artifacts/visual-review/stage-a-custom-final")
	var packed := load(MAIN_SCENE) as PackedScene
	if packed == null:
		printerr("Unable to load %s" % MAIN_SCENE)
		quit(2)
	var scene := packed.instantiate()
	root.add_child(scene)
	_disable_runtime_ui(scene)
	var camera := Camera3D.new()
	camera.name = "StageACustomFinalReviewCamera"
	camera.current = true
	scene.add_child(camera)
	for frame in 8:
		await process_frame
	for view in VIEWS:
		camera.fov = view["fov"]
		camera.global_position = view["position"]
		camera.look_at(view["target"], Vector3.UP)
		for frame in 4:
			await process_frame
		var image := root.get_texture().get_image()
		var path := "%s/%s.png" % [OUTPUT_DIR, view["name"]]
		var err := image.save_png(path)
		if err != OK:
			printerr("Failed to save %s" % path)
			quit(3)
		print("FINAL_CAPTURED %s" % path)
	_make_before_after()
	quit(0)

func _make_before_after() -> void:
	var before := Image.load_from_file("res://artifacts/visual-review/stage-a-custom-before/01_spawn_overview.png")
	var after := Image.load_from_file("%s/stage_a_custom_01_player_spawn.png" % OUTPUT_DIR)
	if before == null or after == null or before.is_empty() or after.is_empty():
		print("BEFORE_AFTER_SKIPPED missing ignored before screenshot")
		return
	before.resize(640, 360)
	after.resize(640, 360)
	var combined := Image.create(1280, 360, false, Image.FORMAT_RGB8)
	combined.fill(Color(0.02, 0.02, 0.025, 1))
	combined.blit_rect(before, Rect2i(Vector2i.ZERO, before.get_size()), Vector2i.ZERO)
	combined.blit_rect(after, Rect2i(Vector2i.ZERO, after.get_size()), Vector2i(640, 0))
	var err := combined.save_png("%s/stage_a_custom_before_after.png" % OUTPUT_DIR)
	if err == OK:
		print("FINAL_CAPTURED %s/stage_a_custom_before_after.png" % OUTPUT_DIR)

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
