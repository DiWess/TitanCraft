extends SceneTree

const OUTPUT_DIR := "res://artifacts/visual-review/phase3a-pass1b-procedural-terrain"
const WIDTH := 1280
const HEIGHT := 720
const MAIN_SCENE := "res://scenes/Main/Main.tscn"

const VIEWS := [
	{"name":"procedural_terrain_01_spawn_route","position":Vector3(0.0,7.0,8.0),"target":Vector3(4.0,0.5,-7.0),"fov":70.0},
	{"name":"procedural_terrain_02_resource_workbench_route","position":Vector3(-10.0,7.0,4.0),"target":Vector3(8.0,0.5,-10.0),"fov":68.0},
	{"name":"procedural_terrain_03_combat_zone","position":Vector3(12.0,7.2,-5.0),"target":Vector3(22.0,0.6,-18.0),"fov":64.0},
	{"name":"procedural_terrain_04_beacon_route","position":Vector3(2.0,8.0,-8.0),"target":Vector3(26.0,0.6,-20.0),"fov":66.0},
	{"name":"procedural_terrain_05_wide_crash_site","position":Vector3(-26.0,13.0,18.0),"target":Vector3(8.0,0.7,-16.0),"fov":76.0},
]

func _initialize() -> void:
	root.size = Vector2i(WIDTH, HEIGHT)
	call_deferred("_capture_all")

func _capture_all() -> void:
	var dir := DirAccess.open("res://")
	if dir == null:
		printerr("Unable to open project root.")
		quit(1)
	dir.make_dir_recursive("artifacts/visual-review/phase3a-pass1b-procedural-terrain")
	var packed := load(MAIN_SCENE) as PackedScene
	if packed == null:
		printerr("Unable to load %s" % MAIN_SCENE)
		quit(2)
	var scene := packed.instantiate()
	root.add_child(scene)
	_hide_runtime_nodes(scene)
	var camera := Camera3D.new()
	camera.name = "Phase3APass1BProceduralTerrainReviewCamera"
	camera.current = true
	scene.add_child(camera)
	for i in 12:
		await process_frame
	var terrain := scene.get_node_or_null(NodePath("ProceduralCrashSiteTerrain/TerrainMesh")) as MeshInstance3D
	if terrain == null or terrain.mesh == null:
		printerr("Procedural terrain mesh missing for capture")
		quit(3)
	var aabb := terrain.get_aabb()
	for view in VIEWS:
		camera.fov = view["fov"]
		camera.global_position = view["position"]
		camera.look_at(view["target"], Vector3.UP)
		if aabb.has_point(terrain.to_local(camera.global_position)):
			printerr("Review camera inside procedural terrain AABB: %s" % view["name"])
			quit(4)
		for i in 3:
			await process_frame
		var image := root.get_texture().get_image()
		if image == null or image.is_empty():
			printerr("Viewport image was empty for %s" % view["name"])
			quit(5)
		var path := "%s/%s.png" % [OUTPUT_DIR, view["name"]]
		var save_error := image.save_png(path)
		if save_error != OK:
			printerr("Failed to save %s with error %s" % [path, save_error])
			quit(6)
		print("CAPTURED %s camera=%s target=%s fov=%.1f" % [path, view["position"], view["target"], view["fov"]])
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
	var player := scene.get_node_or_null(NodePath("Player"))
	if player is Node:
		player.process_mode = Node.PROCESS_MODE_DISABLED
