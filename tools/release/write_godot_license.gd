extends SceneTree

# Writes docs/release/GODOT_LICENSE.txt from the engine itself: the Godot
# licence and its third-party component notices, which must ship with any
# exported game. Run: godot --headless --path . --script tools/release/write_godot_license.gd

func _initialize() -> void:
	var lines := PackedStringArray()
	lines.append("This game uses the Godot Engine, available under the following license:\n")
	lines.append(Engine.get_license_text())
	lines.append("\n\nGodot Engine third-party components:\n")
	for info in Engine.get_copyright_info():
		lines.append("- %s" % info["name"])
		for part in info["parts"]:
			lines.append("  Files: %s" % ", ".join(part["files"]))
			lines.append("  Copyright: %s" % "; ".join(part["copyright"]))
			lines.append("  License: %s" % part["license"])
	lines.append("\n\nLicense texts:\n")
	var licenses := Engine.get_license_info()
	for key in licenses:
		lines.append("--- %s ---\n%s\n" % [key, licenses[key]])
	var file := FileAccess.open("res://docs/release/GODOT_LICENSE.txt", FileAccess.WRITE)
	file.store_string("\n".join(lines))
	file.close()
	print("GODOT_LICENSE_WRITTEN")
	quit(0)
