extends SceneTree

const OUTPUT_DIR := "res://artifacts/visual-review/phase3a-terrain-asset-qualification"
const WIDTH := 1280
const HEIGHT := 720
const GALLERY_SCENE := "res://scenes/Debug/VisualReview/TerrainAssetQualification.tscn"

func _initialize() -> void:
	root.size = Vector2i(WIDTH, HEIGHT)
	call_deferred("_capture")

func _capture() -> void:
	var dir := DirAccess.open("res://")
	if dir == null:
		printerr("Unable to open project root.")
		quit(1)
	dir.make_dir_recursive("artifacts/visual-review/phase3a-terrain-asset-qualification")

	var packed := load(GALLERY_SCENE) as PackedScene
	if packed == null:
		printerr("Unable to load %s" % GALLERY_SCENE)
		quit(2)

	var scene := packed.instantiate()
	root.add_child(scene)
	for i in 8:
		await process_frame

	var image := root.get_texture().get_image()
	if image == null or image.is_empty():
		printerr("Qualification viewport image was empty")
		quit(3)
	var path := "%s/qualification_summary.png" % OUTPUT_DIR
	var save_error := image.save_png(path)
	if save_error != OK:
		printerr("Failed to save %s with error %s" % [path, save_error])
		quit(4)
	print("CAPTURED %s size=%dx%d" % [path, WIDTH, HEIGHT])
	quit(0)
