extends SceneTree

const GALLERY_SCENE := "res://scenes/Debug/VisualReview/TerrainAssetQualification.tscn"

func _initialize() -> void:
	var packed := load(GALLERY_SCENE) as PackedScene
	if packed == null:
		printerr("Unable to load %s" % GALLERY_SCENE)
		quit(1)
	print("TERRAIN_ASSET_QUALIFICATION_SCENE_LOAD_OK")
	quit(0)
