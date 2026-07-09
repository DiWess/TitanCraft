extends SceneTree

const OUTPUT_DIR := "res://artifacts/visual-review/main-menu"
const WIDTH := 1280
const HEIGHT := 720
const MENU_SCENE := "res://scenes/UI/MainMenu.tscn"

func _initialize() -> void:
	root.size = Vector2i(WIDTH, HEIGHT)
	call_deferred("_capture")

func _capture() -> void:
	var dir := DirAccess.open("res://")
	if dir == null:
		printerr("Unable to open project root for visual review output.")
		quit(1)
	dir.make_dir_recursive("artifacts/visual-review/main-menu")

	var packed := load(MENU_SCENE) as PackedScene
	if packed == null:
		printerr("Unable to load %s" % MENU_SCENE)
		quit(2)

	var scene := packed.instantiate()
	# Disable the menu script's _Ready side effects only if they would quit;
	# MainMenu.cs just toggles Continue based on save existence, which is safe.
	root.add_child(scene)

	for i in 8:
		await process_frame

	var image := root.get_texture().get_image()
	if image == null or image.is_empty():
		printerr("Viewport image was empty for main menu capture")
		quit(3)
	var path := "%s/main_menu.png" % OUTPUT_DIR
	var save_error := image.save_png(path)
	if save_error != OK:
		printerr("Failed to save %s with error %s" % [path, save_error])
		quit(4)
	print("CAPTURED %s size=%dx%d" % [path, WIDTH, HEIGHT])
	quit(0)
