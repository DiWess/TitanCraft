extends SceneTree

const OUTPUT_DIR := "res://artifacts/visual-review/maya-stage-a-working"
const WIDTH := 1280
const HEIGHT := 720
const MAIN_SCENE := "res://scenes/Main/Main.tscn"

const REVIEW_VIEWS := [
	{"name":"maya_stage_a_02_wreck_three_quarter","position":Vector3(-13.0,3.2,-4.2),"target":Vector3(-4.2,1.05,-13.0),"fov":58.0},
	{"name":"maya_stage_a_03_wide_composition","position":Vector3(-20.0,7.0,9.0),"target":Vector3(0.5,0.9,-15.0),"fov":74.0},
]

func _initialize() -> void:
	root.size = Vector2i(WIDTH, HEIGHT)
	call_deferred("_capture_all")

func _capture_all() -> void:
	var dir := DirAccess.open("res://")
	if dir == null:
		printerr("Unable to open project root for Stage A capture output.")
		quit(1)
	dir.make_dir_recursive("artifacts/visual-review/maya-stage-a-working")

	var packed := load(MAIN_SCENE) as PackedScene
	if packed == null:
		printerr("Unable to load %s" % MAIN_SCENE)
		quit(2)
	var scene := packed.instantiate()
	root.add_child(scene)
	_disable_runtime_ui(scene)
	_hide_non_stage_a_visuals(scene)

	for i in 8:
		await process_frame

	var player_camera := scene.get_node_or_null(NodePath("Player/Head/Camera3D")) as Camera3D
	if player_camera == null:
		printerr("Player camera missing for Stage A spawn capture.")
		quit(3)
	player_camera.current = true
	player_camera.look_at(Vector3(-4.2, 1.2, -13.0), Vector3.UP)
	await _capture_current("maya_stage_a_01_player_spawn")

	var review_camera := Camera3D.new()
	review_camera.name = "StageAMayaReviewCamera"
	scene.add_child(review_camera)
	for view in REVIEW_VIEWS:
		review_camera.current = true
		review_camera.fov = view["fov"]
		review_camera.global_position = view["position"]
		review_camera.look_at(view["target"], Vector3.UP)
		await _capture_current(view["name"])

	quit(0)

func _capture_current(name: String) -> void:
	for i in 3:
		await process_frame
	var image := root.get_texture().get_image()
	if image == null or image.is_empty():
		printerr("Viewport image was empty for %s" % name)
		quit(4)
	var path := "%s/%s.png" % [OUTPUT_DIR, name]
	var save_error := image.save_png(path)
	if save_error != OK:
		printerr("Failed to save %s with error %s" % [path, save_error])
		quit(5)
	print("CAPTURED %s" % path)

func _disable_runtime_ui(scene: Node) -> void:
	for node in scene.find_children("*", "CanvasLayer", true, false):
		if node.has_method("hide"):
			node.hide()
		node.process_mode = Node.PROCESS_MODE_DISABLED
	for node in scene.find_children("*", "Control", true, false):
		if node is CanvasItem:
			node.visible = false
		node.process_mode = Node.PROCESS_MODE_DISABLED
	var player := scene.get_node_or_null(NodePath("Player"))
	if player is Node:
		player.process_mode = Node.PROCESS_MODE_DISABLED

func _hide_non_stage_a_visuals(scene: Node) -> void:
	# Stage A review isolates terrain, wreck, spawn-route, lighting and background without editing B-E production nodes.
	for node_path in [
		"Placeholder_MetalPickup", "Placeholder_BiomassPickup", "Placeholder_ElectronicsPickup",
		"Placeholder_Workbench", "Placeholder_SavePoint", "Placeholder_Beacon",
		"Placeholder_GalaxabrainScout", "AlienCrystal_1", "AlienCrystal_2", "AlienCrystal_3",
		"AlienCrystal_4", "AlienCrystal_5", "AlienZoneLight"
	]:
		var node := scene.get_node_or_null(NodePath(node_path))
		if node is Node3D:
			node.visible = false
