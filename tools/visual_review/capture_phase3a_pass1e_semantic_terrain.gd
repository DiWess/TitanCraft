extends SceneTree

const OUTPUT_DIR := "res://artifacts/visual-review/phase3a-pass1e-semantic-terrain"
const WIDTH := 1280
const HEIGHT := 720
const MAIN_SCENE := "res://scenes/Main/Main.tscn"
const TERRAIN_ROOT := NodePath("ProceduralCrashSiteTerrain")
const FEATURES := ["RouteSurface","CentralPlateau","SpawnBasaltShelf","ResourceBasaltShelf","WorkbenchRidge","CombatRidge","BeaconShelf","CraterNorthwest","CraterSoutheast","HorizonSegments"]
const FEATURE_COLORS := {"RouteSurface":Color(1,0.92,0.1),"CentralPlateau":Color(0.25,0.9,0.25),"SpawnBasaltShelf":Color(0.15,0.55,1),"ResourceBasaltShelf":Color(0.1,0.85,1),"WorkbenchRidge":Color(1,0.35,0.08),"CombatRidge":Color(1,0.1,0.1),"BeaconShelf":Color(0.85,0.25,1),"CraterNorthwest":Color(0.55,0.25,1),"CraterSoutheast":Color(0.75,0.3,1),"HorizonSegments":Color(1,0.15,0.45)}
const DEBUG_VIEWS := [
	{"name":"pass1e_zone_id","mode":"all","position":Vector3(8,18,-12),"target":Vector3(8,0,-14),"fov":58.0},
	{"name":"pass1e_route_topology","mode":"route","position":Vector3(8,18,-12),"target":Vector3(8,0,-14),"fov":58.0},
	{"name":"pass1e_plateau_and_shelves","mode":"plateau","position":Vector3(-4,8,7),"target":Vector3(1,0,-8),"fov":68.0},
	{"name":"pass1e_ridges_and_beacon_shelf","mode":"ridges","position":Vector3(10,7,0),"target":Vector3(23,0,-23),"fov":70.0},
	{"name":"pass1e_craters","mode":"craters","position":Vector3(5,18,-35),"target":Vector3(5,0,-27),"fov":62.0},
	{"name":"pass1e_horizon_segments","mode":"horizon","position":Vector3(10,5,-5),"target":Vector3(10,1,-36),"fov":72.0},
	{"name":"pass1e_neutral_lighting","mode":"neutral","position":Vector3(0,4,5),"target":Vector3(12,0,-12),"fov":75.0},
	{"name":"pass1e_wireframe_wide","mode":"wire","position":Vector3(-8,14,12),"target":Vector3(10,0,-16),"fov":74.0},
]
const REVIEW_VIEWS := [
	{"name":"pass1e_01_spawn_route_terrain_only","position":Vector3(0,1.7,2.4),"target":Vector3(5,0.8,-5),"fov":75.0},
	{"name":"pass1e_02_resource_workbench_terrain_only","position":Vector3(0,1.7,-1.2),"target":Vector3(12,0.8,-12),"fov":75.0},
	{"name":"pass1e_03_combat_ridge_terrain_only","position":Vector3(12,1.7,-11),"target":Vector3(23,1.2,-23),"fov":75.0},
	{"name":"pass1e_04_beacon_shelf_terrain_only","position":Vector3(18,1.7,-16),"target":Vector3(31,1.2,-23),"fov":75.0},
	{"name":"pass1e_05_crater_view","position":Vector3(-9,2.2,-18),"target":Vector3(-20,0.1,-24),"fov":72.0},
	{"name":"pass1e_06_wide_semantic_terrain","position":Vector3(-16,10,14),"target":Vector3(9,0,-16),"fov":76.0},
]

func _initialize() -> void:
	root.size = Vector2i(WIDTH, HEIGHT)
	call_deferred("_capture_all")

func _capture_all() -> void:
	DirAccess.open("res://").make_dir_recursive("artifacts/visual-review/phase3a-pass1e-semantic-terrain")
	var scene := _load_main()
	var terrain := scene.get_node_or_null(TERRAIN_ROOT) as Node3D
	if terrain == null:
		printerr("Pass 1E semantic terrain missing")
		quit(2)
	_hide_non_terrain_visuals(scene, terrain)
	var camera := _make_camera(scene)
	await _warmup()
	for view in DEBUG_VIEWS:
		_apply_debug_materials(terrain, view["mode"])
		await _capture_view(camera, view, "%s.png" % view["name"])
	for view in REVIEW_VIEWS:
		_apply_production_materials(terrain)
		await _capture_view(camera, view, "%s.png" % view["name"])
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
	camera.name = "Phase3APass1ESemanticTerrainCamera"
	camera.current = true
	camera.near = 0.05
	scene.add_child(camera)
	return camera

func _capture_view(camera: Camera3D, view: Dictionary, filename: String) -> void:
	camera.fov = view["fov"]
	camera.global_position = view["position"]
	camera.look_at(view["target"], Vector3.UP)
	if str(view["name"]).contains("_terrain_only") and (camera.global_position.y < 1.5 or camera.global_position.y > 1.9):
		printerr("Pass 1E player-height camera invalid: %s" % view["name"])
		quit(5)
	await _warmup()
	var image := root.get_texture().get_image()
	if image == null or image.is_empty():
		printerr("Viewport image empty for %s" % filename)
		quit(3)
	var save_error := image.save_png("%s/%s" % [OUTPUT_DIR, filename])
	if save_error != OK:
		printerr("Failed to save %s: %s" % [filename, save_error])
		quit(4)
	print("CAPTURED %s/%s camera=%s target=%s fov=%.1f" % [OUTPUT_DIR, filename, view["position"], view["target"], view["fov"]])

func _hide_non_terrain_visuals(scene: Node, terrain: Node3D) -> void:
	for mesh in scene.find_children("*", "MeshInstance3D", true, false):
		(mesh as MeshInstance3D).visible = _is_descendant_or_self(mesh, terrain)
	for canvas in scene.find_children("*", "CanvasLayer", true, false):
		if canvas.has_method("hide"):
			canvas.hide()

func _is_descendant_or_self(node: Node, ancestor: Node) -> bool:
	var current: Node = node
	while current != null:
		if current == ancestor:
			return true
		current = current.get_parent()
	return false

func _apply_debug_materials(terrain: Node3D, mode: String) -> void:
	for mesh in terrain.find_children("*", "MeshInstance3D", true, false):
		var visual := mesh as MeshInstance3D
		var top := _semantic_group_name(visual)
		visual.material_override = _flat_material(_debug_color(top, mode))
		visual.visible = mode != "wire" or top != "TerrainMesh"

func _apply_production_materials(terrain: Node3D) -> void:
	for mesh in terrain.find_children("*", "MeshInstance3D", true, false):
		var visual := mesh as MeshInstance3D
		visual.visible = visual.name != "TerrainMesh"
		visual.material_override = _flat_material(FEATURE_COLORS.get(_semantic_group_name(visual), Color(0.42,0.38,0.32)))

func _semantic_group_name(mesh: MeshInstance3D) -> String:
	var current: Node = mesh
	while current != null and current.get_parent() != null and current.get_parent().name != "ProceduralCrashSiteTerrain":
		current = current.get_parent()
	return current.name

func _debug_color(name: String, mode: String) -> Color:
	if mode == "neutral": return Color(0.55,0.55,0.55)
	if mode == "route": return Color.WHITE if name == "RouteSurface" else Color(0.06,0.06,0.06)
	if mode == "plateau": return FEATURE_COLORS.get(name, Color(0.05,0.05,0.05)) if name in ["CentralPlateau","SpawnBasaltShelf","ResourceBasaltShelf"] else Color(0.04,0.04,0.04)
	if mode == "ridges": return FEATURE_COLORS.get(name, Color(0.04,0.04,0.04)) if name in ["WorkbenchRidge","CombatRidge","BeaconShelf"] else Color(0.04,0.04,0.04)
	if mode == "craters": return FEATURE_COLORS.get(name, Color(0.04,0.04,0.04)) if name.begins_with("Crater") else Color(0.04,0.04,0.04)
	if mode == "horizon": return FEATURE_COLORS.get(name, Color(0.04,0.04,0.04)) if name == "HorizonSegments" else Color(0.04,0.04,0.04)
	if mode == "wire": return Color.WHITE
	return FEATURE_COLORS.get(name, Color(0.8,0.8,0.8))

func _flat_material(color: Color) -> StandardMaterial3D:
	var material := StandardMaterial3D.new()
	material.shading_mode = BaseMaterial3D.SHADING_MODE_UNSHADED
	material.albedo_color = color
	return material

func _warmup() -> void:
	for i in 8:
		await process_frame
