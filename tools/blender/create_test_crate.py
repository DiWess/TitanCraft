#!/usr/bin/env python3
"""Create the non-production TC_TestCrate Blender source asset.

Run with Blender:
  blender --background --python tools/blender/create_test_crate.py
"""
from __future__ import annotations

from pathlib import Path
import bpy

ROOT = Path(__file__).resolve().parents[2]
OUTPUT = ROOT / "assets/Source/Blender/_templates/TC_TestCrate.blend"


def material(name: str, color: tuple[float, float, float, float]) -> bpy.types.Material:
    mat = bpy.data.materials.new(name)
    mat.diffuse_color = color
    return mat


def cube_part(name: str, location: tuple[float, float, float], scale: tuple[float, float, float], mat: bpy.types.Material, bevel_width: float) -> bpy.types.Object:
    bpy.ops.mesh.primitive_cube_add(size=1, location=location)
    obj = bpy.context.object
    obj.name = name
    obj.dimensions = scale
    bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
    obj.data.materials.append(mat)
    obj["titancraft_collision"] = "none"
    obj["titancraft_visual_only"] = True
    bevel = obj.modifiers.new("pipeline bevel proof", "BEVEL")
    bevel.width = bevel_width
    bevel.segments = 2
    obj.modifiers.new("pipeline weighted normals proof", "WEIGHTED_NORMAL")
    return obj


def main() -> None:
    bpy.ops.object.select_all(action="SELECT")
    bpy.ops.object.delete()
    body = material("TC_TestCrate_Body", (0.55, 0.49, 0.38, 1))
    band = material("TC_TestCrate_Bands", (0.18, 0.20, 0.22, 1))
    cube_part("TC_TestCrate", (0, 0, 0), (1.0, 1.0, 1.0), body, 0.035)
    cube_part("TC_TestCrate_Band_X", (0, 0, 0.52), (1.08, 0.16, 0.08), band, 0.012)
    cube_part("TC_TestCrate_Band_Z", (0.52, 0, 0), (0.08, 0.16, 1.08), band, 0.012)
    cube_part("TC_TestCrate_SideHandle", (0, -0.53, 0), (0.42, 0.06, 0.16), band, 0.012)
    # Keep the asset origin at world zero for predictable Godot placement.
    for obj in bpy.context.scene.objects:
        if obj.type == "MESH":
            bpy.context.view_layer.objects.active = obj
            obj.select_set(True)
            bpy.ops.object.transform_apply(location=True, rotation=True, scale=True)
            obj.select_set(False)
    OUTPUT.parent.mkdir(parents=True, exist_ok=True)
    bpy.ops.wm.save_as_mainfile(filepath=str(OUTPUT))
    print(f"TC_TEST_CRATE_BLEND_WRITTEN {OUTPUT}")


if __name__ == "__main__":
    main()
