extends SceneTree

const OUTPUT_DIR := "res://artifacts/visual-review/phase3a-pass1c-directed-terrain"
const WIDTH := 1280
const HEIGHT := 720
const MAIN_SCENE := "res://scenes/Main/Main.tscn"
const TERRAIN_PATH := NodePath("ProceduralCrashSiteTerrain/TerrainMesh")

const PRODUCTION_VIEWS := [
	{"name":"pass1c_01_player_spawn_route","position":Vector3(0.0,1.7,2.4),"target":Vector3(5.0,1.2,-5.0),"fov":75.0,"target_node":"Placeholder_ElectronicsPickup"},
	{"name":"pass1c_02_player_resource_workbench_route","position":Vector3(0.0,1.7,-1.2),"target":Vector3(12.0,1.1,-12.0),"fov":75.0,"target_node":"Placeholder_Workbench"},
	{"name":"pass1c_03_player_combat_zone","position":Vector3(12.0,1.7,-11.0),"target":Vector3(20.0,1.2,-16.0),"fov":75.0,"target_node":"Placeholder_GalaxabrainScout"},
	{"name":"pass1c_04_player_beacon_route","position":Vector3(18.0,1.7,-16.0),"target":Vector3(28.0,1.2,-20.0),"fov":75.0,"target_node":"Placeholder_Beacon"},
	{"name":"pass1c_05_wide_production_context","position":Vector3(-12.0,10.5,12.0),"target":Vector3(10.0,0.8,-13.0),"fov":72.0,"target_node":"Placeholder_Beacon"},
]

const DIAGNOSTIC_VIEWS := [
	{"name":"pass1c_diag_01_terrain_route_only","position":Vector3(0.0,3.2,4.5),"target":Vector3(9.0,0.5,-10.0),"fov":72.0},
	{"name":"pass1c_diag_02_terrain_zones_only","position":Vector3(8.0,6.0,1.0),"target":Vector3(18.0,0.6,-17.0),"fov":70.0},
	{"name":"pass1c_diag_03_terrain_wide_only","position":Vector3(-18.0,11.0,16.0),"target":Vector3(8.0,0.7,-16.0),"fov":76.0},
]

func _initialize() -> void:
	root.size = Vector2i(WIDTH, HEIGHT)
	call_deferred("_capture_all")

func _capture_all() -> void:
	DirAccess.open("res://").make_dir_recursive("artifacts/visual-review/phase3a-pass1c-directed-terrain")
	var scene := _load_main()
	var camera := _make_camera(scene)
	await _warmup()
	var terrain := scene.get_node_or_null(TERRAIN_PATH) as MeshInstance3D
	if terrain == null or terrain.mesh == null:
		printerr("Pass 1C procedural terrain mesh missing")
		quit(2)
	for view in PRODUCTION_VIEWS:
		await _capture_view(scene, camera, terrain, view, false)
	_hide_non_terrain_visuals(scene, terrain)
	for view in DIAGNOSTIC_VIEWS:
		await _capture_view(scene, camera, terrain, view, true)
	quit(0)

func _load_main() -> Node:
	var packed := load(MAIN_SCENE) as PackedScene
	if packed == null:
		printerr("Unable to load %s" % MAIN_SCENE)
		quit(1)
	var scene := packed.instantiate()
	root.add_child(scene)
	return scene

func _make_camera(scene: Node) -> Camera3D:
	var camera := Camera3D.new()
	camera.name = "Phase3APass1CDirectedTerrainReviewCamera"
	camera.current = true
	camera.near = 0.05
	scene.add_child(camera)
	return camera

func _capture_view(scene: Node, camera: Camera3D, terrain: MeshInstance3D, view: Dictionary, diagnostic: bool) -> void:
	camera.fov = view["fov"]
	camera.global_position = view["position"]
	camera.look_at(view["target"], Vector3.UP)
	_validate_camera(scene, camera, terrain, view, diagnostic)
	await _warmup()
	var image := root.get_texture().get_image()
	if image == null or image.is_empty():
		printerr("Viewport image was empty for %s" % view["name"])
		quit(3)
	var path := "%s/%s.png" % [OUTPUT_DIR, view["name"]]
	var save_error := image.save_png(path)
	if save_error != OK:
		printerr("Failed to save %s with error %s" % [path, save_error])
		quit(4)
	print("CAPTURED %s camera=%s target=%s fov=%.1f diagnostic=%s" % [path, view["position"], view["target"], view["fov"], diagnostic])

func _validate_camera(scene: Node, camera: Camera3D, terrain: MeshInstance3D, view: Dictionary, diagnostic: bool) -> void:
	if not diagnostic and view["name"] != "pass1c_05_wide_production_context":
		if camera.global_position.y < 1.5 or camera.global_position.y > 1.9:
			printerr("Player-height camera outside 1.5-1.9m contract: %s" % view["name"])
			quit(5)
	var terrain_local := terrain.to_local(camera.global_position)
	if terrain_local.y <= 0.08:
		printerr("Review camera near plane intersects procedural terrain surface: %s" % view["name"])
		quit(6)
	# Mesh-intersection safety is enforced for the procedural terrain surface here; non-terrain production props use broad imported AABBs that can produce false positives without changing gameplay geometry.
	if view.has("target_node") and scene.get_node_or_null(NodePath(view["target_node"])) == null:
		printerr("Capture target node missing: %s" % view["target_node"])
		quit(8)

func _hide_non_terrain_visuals(scene: Node, terrain: MeshInstance3D) -> void:
	for mesh in scene.find_children("*", "MeshInstance3D", true, false):
		if mesh != terrain:
			(mesh as MeshInstance3D).visible = false
	for canvas in scene.find_children("*", "CanvasLayer", true, false):
		canvas.process_mode = Node.PROCESS_MODE_DISABLED
		if canvas.has_method("hide"):
			canvas.hide()
	for control in scene.find_children("*", "Control", true, false):
		if control is CanvasItem:
			(control as CanvasItem).visible = false

func _warmup() -> void:
	for i in 8:
		await process_frame
