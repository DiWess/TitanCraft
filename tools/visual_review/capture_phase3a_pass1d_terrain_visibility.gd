extends SceneTree

const OUTPUT_DIR := "res://artifacts/visual-review/phase3a-pass1d-terrain-visibility"
const WIDTH := 1280
const HEIGHT := 720
const MAIN_SCENE := "res://scenes/Main/Main.tscn"
const TERRAIN_PATH := NodePath("ProceduralCrashSiteTerrain/TerrainMesh")

const PRODUCTION_VIEWS := [
	{"name":"pass1d_prod_01_spawn_route","position":Vector3(0.0,1.7,2.4),"target":Vector3(5.0,1.2,-5.0),"fov":75.0},
	{"name":"pass1d_prod_02_resource_workbench","position":Vector3(0.0,1.7,-1.2),"target":Vector3(12.0,1.1,-12.0),"fov":75.0},
	{"name":"pass1d_prod_03_combat_zone","position":Vector3(12.0,1.7,-11.0),"target":Vector3(20.0,1.2,-16.0),"fov":75.0},
	{"name":"pass1d_prod_04_beacon_route","position":Vector3(18.0,1.7,-16.0),"target":Vector3(28.0,1.2,-20.0),"fov":75.0},
]

const TECHNICAL_VIEWS := [
	{"name":"pass1d_zone_id","mode":"zone","position":Vector3(8.0,18.0,-12.0),"target":Vector3(8.2,0.0,-12.4),"fov":58.0},
	{"name":"pass1d_height_gradient","mode":"height","position":Vector3(8.0,18.0,-12.0),"target":Vector3(8.2,0.0,-12.4),"fov":58.0},
	{"name":"pass1d_normals","mode":"normal","position":Vector3(8.0,18.0,-12.0),"target":Vector3(8.2,0.0,-12.4),"fov":58.0},
	{"name":"pass1d_wireframe_top","mode":"wire","position":Vector3(8.0,22.0,-12.0),"target":Vector3(8.2,0.0,-12.4),"fov":55.0},
	{"name":"pass1d_route_mask","mode":"route","position":Vector3(8.0,18.0,-12.0),"target":Vector3(8.2,0.0,-12.4),"fov":58.0},
	{"name":"pass1d_neutral_lighting","mode":"neutral","position":Vector3(8.0,10.0,10.0),"target":Vector3(8.2,0.0,-12.4),"fov":70.0},
	{"name":"pass1d_production_material_terrain_only","mode":"production","position":Vector3(8.0,10.0,10.0),"target":Vector3(8.2,0.0,-12.4),"fov":70.0},
]

const ZONE_COLORS := {
	Color(0.40,0.35,0.29): Color(1.0,0.93,0.18),
	Color(0.27,0.24,0.20): Color(0.25,0.95,0.30),
	Color(0.22,0.20,0.18): Color(0.2,0.65,1.0),
	Color(0.18,0.17,0.16): Color(1.0,0.35,0.1),
	Color(0.16,0.135,0.115): Color(0.65,0.25,1.0),
	Color(0.145,0.135,0.13): Color(1.0,0.15,0.45),
}

func _initialize() -> void:
	root.size = Vector2i(WIDTH, HEIGHT)
	call_deferred("_capture_all")

func _capture_all() -> void:
	DirAccess.open("res://").make_dir_recursive("artifacts/visual-review/phase3a-pass1d-terrain-visibility")
	var scene := _load_main()
	var camera := _make_camera(scene)
	await _warmup()
	var terrain := scene.get_node_or_null(TERRAIN_PATH) as MeshInstance3D
	if terrain == null or terrain.mesh == null:
		printerr("Pass 1D procedural terrain mesh missing")
		quit(2)
	var original_mesh := terrain.mesh
	var original_material := terrain.material_override
	var original_materials := _remember_materials(scene)
	_hide_canvas(scene)
	for view in PRODUCTION_VIEWS:
		_restore_materials(original_materials)
		terrain.mesh = original_mesh
		terrain.material_override = original_material
		_show_all_meshes(scene)
		await _capture_view(camera, view, "%s.png" % view["name"])
		_apply_category_mask(scene, terrain)
		await _capture_view(camera, view, "%s_category_mask.png" % view["name"])
	for view in TECHNICAL_VIEWS:
		_restore_materials(original_materials)
		_hide_non_terrain_visuals(scene, terrain)
		terrain.mesh = _diagnostic_mesh(original_mesh, view["mode"])
		terrain.material_override = _diagnostic_material(view["mode"])
		await _capture_view(camera, view, "%s.png" % view["name"])
	terrain.mesh = original_mesh
	terrain.material_override = original_material
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
	camera.name = "Phase3APass1DTerrainVisibilityCamera"
	camera.current = true
	camera.near = 0.05
	scene.add_child(camera)
	return camera

func _capture_view(camera: Camera3D, view: Dictionary, filename: String) -> void:
	camera.fov = view["fov"]
	camera.global_position = view["position"]
	camera.look_at(view["target"], Vector3.UP)
	var is_production := view.has("name") and str(view["name"]).begins_with("pass1d_prod_")
	if is_production and (camera.global_position.y < 1.5 or camera.global_position.y > 1.9):
		printerr("Pass 1D production camera height invalid: %s" % view["name"])
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

func _diagnostic_mesh(source: Mesh, mode: String) -> ArrayMesh:
	var arrays := source.surface_get_arrays(0)
	var vertices: PackedVector3Array = arrays[Mesh.ARRAY_VERTEX]
	var normals: PackedVector3Array = arrays[Mesh.ARRAY_NORMAL]
	var original_colors: PackedColorArray = arrays[Mesh.ARRAY_COLOR]
	if mode == "wire":
		return _wire_mesh(vertices, original_colors)
	var colors := PackedColorArray()
	colors.resize(vertices.size())
	for i in vertices.size():
		match mode:
			"zone": colors[i] = _zone_debug_color(original_colors[i])
			"height": colors[i] = _height_color(vertices[i].y)
			"normal": colors[i] = Color(normals[i].x * 0.5 + 0.5, normals[i].y * 0.5 + 0.5, normals[i].z * 0.5 + 0.5)
			"route": colors[i] = Color.WHITE if _is_route_color(original_colors[i]) else Color(0.08,0.08,0.08)
			"neutral": colors[i] = Color(0.55,0.55,0.55)
			_: colors[i] = original_colors[i]
	arrays[Mesh.ARRAY_COLOR] = colors
	var mesh := ArrayMesh.new()
	mesh.add_surface_from_arrays(Mesh.PRIMITIVE_TRIANGLES, arrays)
	return mesh

func _wire_mesh(vertices: PackedVector3Array, original_colors: PackedColorArray) -> ArrayMesh:
	var lines := PackedVector3Array()
	var colors := PackedColorArray()
	for i in range(0, vertices.size(), 3):
		var edge_color := Color.WHITE if _is_route_color(original_colors[i]) else Color(0.08,0.08,0.08)
		for pair in [[0,1],[1,2],[2,0]]:
			lines.append(vertices[i + pair[0]])
			lines.append(vertices[i + pair[1]])
			colors.append(edge_color)
			colors.append(edge_color)
	var arrays := []
	arrays.resize(Mesh.ARRAY_MAX)
	arrays[Mesh.ARRAY_VERTEX] = lines
	arrays[Mesh.ARRAY_COLOR] = colors
	var mesh := ArrayMesh.new()
	mesh.add_surface_from_arrays(Mesh.PRIMITIVE_LINES, arrays)
	return mesh

func _diagnostic_material(mode: String) -> StandardMaterial3D:
	var material := StandardMaterial3D.new()
	material.albedo_color = Color.WHITE
	material.vertex_color_use_as_albedo = true
	material.roughness = 0.9
	if mode != "neutral" and mode != "production":
		material.shading_mode = BaseMaterial3D.SHADING_MODE_UNSHADED
	return material

func _zone_debug_color(source: Color) -> Color:
	var best := Color(1,1,1)
	var best_distance := 999.0
	for key in ZONE_COLORS.keys():
		var d: float = abs(source.r-key.r)+abs(source.g-key.g)+abs(source.b-key.b)
		if d < best_distance:
			best_distance = d
			best = ZONE_COLORS[key]
	return best

func _height_color(y: float) -> Color:
	var t: float = clamp((y - 0.03) / 4.17, 0.0, 1.0)
	return Color(t, min(1.0, 0.25 + t), 1.0 - t)

func _is_route_color(color: Color) -> bool:
	return color.r > 0.55 and color.g > 0.50

func _apply_category_mask(scene: Node, terrain: MeshInstance3D) -> void:
	_show_all_meshes(scene)
	for mesh in scene.find_children("*", "MeshInstance3D", true, false):
		var visual := mesh as MeshInstance3D
		visual.material_override = _flat_material(_category_color(str(visual.get_path()), visual == terrain))

func _remember_materials(scene: Node) -> Dictionary:
	var materials := {}
	for mesh in scene.find_children("*", "MeshInstance3D", true, false):
		var visual := mesh as MeshInstance3D
		materials[visual.get_path()] = visual.material_override
	return materials

func _restore_materials(materials: Dictionary) -> void:
	for path in materials.keys():
		var node := root.get_node_or_null(path)
		if node is MeshInstance3D:
			(node as MeshInstance3D).material_override = materials[path]

func _category_color(path: String, is_terrain: bool) -> Color:
	if is_terrain: return Color(0,1,0)
	if path.contains("CrashedShip") or path.contains("AuthenticatedCrashSiteVisuals"): return Color(1,0,0)
	if path.contains("Player") or path.contains("MechanicalArm"): return Color(0,0,1)
	if path.contains("Galaxabrain"): return Color(1,0,1)
	if path.contains("Pickup") or path.contains("Workbench") or path.contains("SavePoint") or path.contains("Beacon"): return Color(1,1,0)
	return Color(0,1,1)

func _flat_material(color: Color) -> StandardMaterial3D:
	var material := StandardMaterial3D.new()
	material.shading_mode = BaseMaterial3D.SHADING_MODE_UNSHADED
	material.albedo_color = color
	return material

func _hide_non_terrain_visuals(scene: Node, terrain: MeshInstance3D) -> void:
	for mesh in scene.find_children("*", "MeshInstance3D", true, false):
		(mesh as MeshInstance3D).visible = mesh == terrain

func _show_all_meshes(scene: Node) -> void:
	for mesh in scene.find_children("*", "MeshInstance3D", true, false):
		(mesh as MeshInstance3D).visible = true

func _hide_canvas(scene: Node) -> void:
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
