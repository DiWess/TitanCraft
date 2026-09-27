extends SceneTree

# Golden screens: composed beauty shots of the Mwezi Quarter for review and
# presentation, at 1920x1080 on the Forward+ renderer the game ships with.
#
# Run (needs a Vulkan driver; in a container, Mesa's software lavapipe works):
#   VK_ICD_FILENAMES=/usr/share/vulkan/icd.d/lvp_icd.json \
#     xvfb-run -a -s "-screen 0 1920x1080x24" godot --path . \
#     --script tools/visual_review/capture_golden_shots.gd
# Output: artifacts/visual-review/golden/*.png (gitignored).
#
# World shots hide the HUD and the intro title card and freeze the player, so
# they show the place, not the interface; the last three show the game as
# played. Camera positions are hand-composed against the mission anchors in
# scenes/Main/Main.tscn. Shots are timed in wall-clock milliseconds because a
# software renderer runs far below real time and tweens run on process time.

const OUTPUT_DIR := "res://artifacts/visual-review/golden"
const MAIN_SCENE := "res://scenes/Main/Main.tscn"
const VICTORY_SCENE := "res://scenes/UI/VictoryScreen.tscn"
const SIZE := Vector2i(1920, 1080)
const EYE := 1.65

# name, camera position, look target, field of view
const WORLD_SHOTS := [
	["01_the_quarter_at_dusk", Vector3(-14.0, 14.0, 10.0), Vector3(8.0, 0.0, -12.0), 60.0],
	["02_spawn_crash_site", Vector3(2.5, 2.4, 1.5), Vector3(-4.0, 0.9, -12.5), 68.0],
	["03_west_alley_to_the_save_point", Vector3(-6.5, EYE, -4.5), Vector3(-12.0, 1.3, -12.0), 70.0],
	["04_east_alley_lit_arrival", Vector3(4.0, EYE, -1.0), Vector3(8.5, 0.8, -4.5), 70.0],
	["05_workbench_courtyard", Vector3(15.5, EYE, -9.5), Vector3(12.0, 1.0, -12.3), 68.0],
	["06_the_scout_in_its_arena", Vector3(16.4, 1.5, -14.9), Vector3(20.0, 1.3, -16.0), 58.0],
	["07_plaza_and_tower", Vector3(14.0, 1.8, -12.0), Vector3(22.6, 5.0, -26.6), 70.0],
	["08_beacon_on_the_harbour_front", Vector3(23.0, EYE, -16.5), Vector3(28.5, 1.8, -20.5), 70.0],
]

var _main: Node

func _initialize() -> void:
	root.size = SIZE
	call_deferred("_run")

func _run() -> void:
	DirAccess.make_dir_recursive_absolute(ProjectSettings.globalize_path(OUTPUT_DIR))
	print("GOLDEN renderer=%s adapter=%s" % [RenderingServer.get_current_rendering_method(), RenderingServer.get_video_adapter_name()])
	_main = (load(MAIN_SCENE) as PackedScene).instantiate()
	root.add_child(_main)
	current_scene = _main
	await _settle(1500)

	# First-person, as played: HUD up, title card gone, arm in view.
	_main.get_node("IntroTitleCard").visible = false
	await _settle(800)
	await _save("09_first_person_as_played")

	var player := _main.get_node("Player")
	player.process_mode = Node.PROCESS_MODE_DISABLED
	var scout := _main.get_node_or_null("Placeholder_GalaxabrainScout")
	if scout != null:
		scout.process_mode = Node.PROCESS_MODE_DISABLED
	_main.get_node("HUD").visible = false

	var camera := Camera3D.new()
	_main.add_child(camera)
	camera.current = true
	for shot in WORLD_SHOTS:
		camera.fov = shot[3]
		camera.global_position = shot[1]
		camera.look_at(shot[2], Vector3.UP)
		await _settle(900)
		await _save(shot[0])

	# The ending, as victory plays it: lit beacon, letterbox, caption, push-in.
	_main.get_node("Placeholder_Beacon").RestoreActivated(true)
	_main.get_node("VictoryEndingShot").Play()
	await _settle(3500)
	var ending := root.get_texture().get_image()
	ending.save_png("%s/10_the_ending.png" % ProjectSettings.globalize_path(OUTPUT_DIR))
	print("GOLDEN saved 10_the_ending")

	# The victory screen over the ending's last frame, as the navigator hands it on.
	_main.queue_free()
	await process_frame
	var screen := (load(VICTORY_SCENE) as PackedScene).instantiate()
	root.add_child(screen)
	var snapshot := screen.get_node("Snapshot") as TextureRect
	snapshot.texture = ImageTexture.create_from_image(ending)
	snapshot.visible = true
	var backdrop := screen.get_node("Backdrop") as ColorRect
	backdrop.color = Color(backdrop.color, 0.55)
	await _settle(2500)
	await _save("11_victory")
	quit(0)

func _settle(milliseconds: int) -> void:
	var until := Time.get_ticks_msec() + milliseconds
	while Time.get_ticks_msec() < until:
		await process_frame
	await process_frame

func _save(name: String) -> void:
	root.get_texture().get_image().save_png("%s/%s.png" % [ProjectSettings.globalize_path(OUTPUT_DIR), name])
	print("GOLDEN saved %s" % name)
