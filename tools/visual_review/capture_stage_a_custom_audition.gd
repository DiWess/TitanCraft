extends SceneTree

const OUTPUT_DIR := "res://artifacts/visual-review/stage-a-custom-audition"
const WIDTH := 1280
const HEIGHT := 720
const MESHES := [
	"TC_HullMain", "TC_NoseCrushed", "TC_WingSheared", "TC_EngineExposed", "TC_InternalRibs", "TC_BreachDebris",
	"TC_BasaltForeground", "TC_BasaltMidgroundA", "TC_BasaltMidgroundB", "TC_RidgeA", "TC_RidgeB", "TC_RidgeC",
	"TC_FracturedGround", "TC_AshPatch"
]

func _initialize() -> void:
	root.size = Vector2i(WIDTH, HEIGHT)
	call_deferred("_capture_all")

func _capture_all() -> void:
	DirAccess.open("res://").make_dir_recursive("artifacts/visual-review/stage-a-custom-audition")
	var scene := Node3D.new()
	root.add_child(scene)
	var material := StandardMaterial3D.new()
	material.albedo_color = Color(0.62, 0.62, 0.62, 1.0)
	material.roughness = 0.78
	var light := DirectionalLight3D.new()
	light.light_energy = 1.6
	light.rotation_degrees = Vector3(-42, -32, 0)
	scene.add_child(light)
	var camera := Camera3D.new()
	camera.current = true
	camera.fov = 52.0
	scene.add_child(camera)
	for i in MESHES.size():
		var mesh := load("res://assets/Production/Custom/StageA/%s.obj" % MESHES[i]) as ArrayMesh
		if mesh == null:
			printerr("Missing audition mesh: %s" % MESHES[i])
			quit(2)
		var inst := MeshInstance3D.new()
		inst.mesh = mesh
		inst.material_override = material
		inst.name = MESHES[i]
		scene.add_child(inst)
		var aabb := inst.get_aabb()
		var radius = max(max(aabb.size.x, aabb.size.y), aabb.size.z)
		camera.global_position = Vector3(radius * 0.9, radius * 0.65, radius * 1.45)
		camera.look_at(Vector3(0, aabb.size.y * 0.28, 0), Vector3.UP)
		for frame in 4:
			await process_frame
		var path := "%s/%02d_%s.png" % [OUTPUT_DIR, i + 1, MESHES[i]]
		var err := root.get_texture().get_image().save_png(path)
		if err != OK:
			printerr("Failed to save audition screenshot %s" % path)
			quit(3)
		print("AUDITION_CAPTURED %s" % path)
		inst.queue_free()
		await process_frame
	quit(0)
