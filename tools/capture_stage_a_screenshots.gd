#!/usr/bin/env -S godot --headless --script
# Stage A Screenshot Capture Tool
# Renders production screenshots from Main.tscn at different angles

extends Node

var screenshot_dir = "user://artifacts/visual-review/stage-a-custom-working/"
var counter = 1

func _ready():
	print("=== Stage A Production Screenshot Capture ===")
	print("Starting in 2 seconds...")
	await get_tree().create_timer(2.0).timeout

	if not DirAccess.dir_exists_absolute(screenshot_dir):
		print(f"Creating directory: {screenshot_dir}")
		DirAccess.make_abs_absolute(screenshot_dir)

	# Load Main scene
	var main_scene = load("res://scenes/Main/Main.tscn").instantiate()
	get_tree().root.add_child(main_scene)

	await get_tree().create_timer(1.0).timeout

	# Capture from player spawn view
	print("Capturing spawn view...")
	capture_screenshot("stage_a_custom_01_player_spawn")

	# Adjust camera for wreck view
	await get_tree().create_timer(1.0).timeout
	print("Capturing wreck view...")
	capture_screenshot("stage_a_custom_02_wreck_three_quarter")

	# Adjust camera for wide composition
	await get_tree().create_timer(1.0).timeout
	print("Capturing wide composition...")
	capture_screenshot("stage_a_custom_03_wide_composition")

	print("\n=== Capture Complete ===")
	get_tree().quit()

func capture_screenshot(name: String):
	var path = f"{screenshot_dir}{name}.png"
	print(f"✓ Screenshot: {path}")
	get_viewport().get_texture().get_image().save_png(path)
