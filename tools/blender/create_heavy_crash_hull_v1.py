#!/usr/bin/env python3
"""Create the project-authored TC_HeavyCrashHull_V1 Blender source.

Run with Blender:
  blender --background --python tools/blender/create_heavy_crash_hull_v1.py
"""
from __future__ import annotations

from pathlib import Path
import bpy

ROOT = Path(__file__).resolve().parents[2]
SOURCE = ROOT / "assets/Source/Blender/Production/TC_HeavyCrashHull_V1.blend"


def _reset_scene() -> None:
    bpy.ops.object.select_all(action="SELECT")
    bpy.ops.object.delete()


def _material(name: str, color: tuple[float, float, float, float]) -> bpy.types.Material:
    material = bpy.data.materials.new(name)
    material.diffuse_color = color
    return material


def _mesh_object(name: str, verts: list[tuple[float, float, float]], faces: list[tuple[int, ...]], material: bpy.types.Material) -> bpy.types.Object:
    mesh = bpy.data.meshes.new(f"{name}Mesh")
    mesh.from_pydata(verts, [], faces)
    mesh.update()
    obj = bpy.data.objects.new(name, mesh)
    bpy.context.collection.objects.link(obj)
    mesh.materials.append(material)
    obj["titancraft_collision"] = "none"
    obj["titancraft_visual_only"] = True
    return obj


def _box(name: str, center: tuple[float, float, float], size: tuple[float, float, float], material: bpy.types.Material) -> bpy.types.Object:
    cx, cy, cz = center
    sx, sy, sz = size
    x, y, z = sx / 2, sy / 2, sz / 2
    verts = [
        (cx - x, cy - y, cz - z), (cx + x, cy - y, cz - z), (cx + x, cy + y, cz - z), (cx - x, cy + y, cz - z),
        (cx - x, cy - y, cz + z), (cx + x, cy - y, cz + z), (cx + x, cy + y, cz + z), (cx - x, cy + y, cz + z),
    ]
    faces = [(0, 1, 2, 3), (4, 7, 6, 5), (0, 4, 5, 1), (1, 5, 6, 2), (2, 6, 7, 3), (3, 7, 4, 0)]
    return _mesh_object(name, verts, faces, material)


def main() -> None:
    _reset_scene()
    hull = _material("TC_MAT_worn_off_white_hull", (0.72, 0.68, 0.58, 1))
    graphite = _material("TC_MAT_graphite_underside_interior", (0.08, 0.09, 0.09, 1))
    steel = _material("TC_MAT_worn_steel_ribs_panels", (0.38, 0.36, 0.32, 1))
    orange = _material("TC_MAT_muted_orange_markings", (0.75, 0.28, 0.08, 1))
    cyan = _material("TC_MAT_localized_cyan_breach_slot", (0.05, 0.65, 0.8, 1))

    hull_verts = [
        (-5.9, -2.15, 0), (-5.2, 2.0, 0), (-5.4, -1.65, 2.8), (-4.9, 1.6, 2.45),
        (4.9, -2.35, 0), (5.7, 2.05, 0), (4.5, -1.75, 3.1), (5.2, 1.55, 2.55),
    ]
    hull_faces = [(0, 4, 6, 2), (1, 3, 7, 5), (2, 6, 7, 3), (0, 1, 5, 4), (0, 2, 3, 1), (4, 5, 7, 6)]
    _mesh_object("TC_HeavyCrashHull_V1_broad_angular_thick_hull", hull_verts, hull_faces, hull)

    underside_verts = [
        (-5.6, -1.75, -0.08), (-5.0, 1.65, -0.08), (5.2, 1.75, -0.08), (4.7, -1.9, -0.08),
        (-5.3, -1.45, 0.18), (4.5, -1.55, 0.18), (5.0, 1.4, 0.18), (-4.8, 1.35, 0.18),
    ]
    _mesh_object("TC_HeavyCrashHull_V1_flat_graphite_underside", underside_verts, [(0, 1, 2, 3), (0, 4, 7, 1), (3, 2, 6, 5)], graphite)

    for index, x in enumerate([-2.8, -1.7, -0.6, 0.6, 1.8], start=1):
        _box(f"TC_HeavyCrashHull_V1_exposed_internal_rib_{index}", (x, -2.22, 1.25), (0.18, 0.32, 2.25), steel)
    _box("TC_HeavyCrashHull_V1_torn_breach_lower_lip", (-0.4, -2.45, 0.55), (4.4, 0.28, 0.32), steel)
    _box("TC_HeavyCrashHull_V1_torn_breach_upper_lip", (-0.2, -2.35, 2.55), (3.7, 0.3, 0.28), steel)

    for index, x in enumerate([-3.8, -1.2, 1.4, 3.6], start=1):
        _box(f"TC_HeavyCrashHull_V1_manufactured_hull_panel_{index}", (x, 2.08, 1.35), (1.55, 0.22, 1.05), hull)
    for index, x in enumerate([-4.0, -1.0, 2.2], start=1):
        _box(f"TC_HeavyCrashHull_V1_worn_steel_panel_seam_{index}", (x, 2.22, 1.4), (0.12, 0.18, 2.1), steel)

    _box("TC_HeavyCrashHull_V1_crushed_front_offwhite_plate", (-5.8, 0.4, 1.0), (0.65, 2.8, 1.5), hull)
    _box("TC_HeavyCrashHull_V1_crushed_front_graphite_void", (-6.05, -0.9, 0.85), (0.36, 1.15, 1.25), graphite)
    _box("TC_HeavyCrashHull_V1_crumpled_front_steel_strut", (-5.7, -1.65, 1.8), (0.32, 0.35, 1.6), steel)

    _box("TC_HeavyCrashHull_V1_rear_engine_mount_graphite", (5.78, 0, 1.15), (0.7, 2.7, 1.7), graphite)
    _box("TC_HeavyCrashHull_V1_rear_engine_upper_lug", (5.95, -0.95, 2.0), (0.85, 0.55, 0.45), steel)
    _box("TC_HeavyCrashHull_V1_rear_engine_lower_lug", (5.95, 0.95, 0.55), (0.85, 0.55, 0.45), steel)
    _box("TC_HeavyCrashHull_V1_rear_engine_coupler_bar", (6.15, 0, 1.25), (0.35, 0.45, 2.1), steel)

    _box("TC_HeavyCrashHull_V1_muted_orange_side_marking", (1.6, 2.34, 2.05), (2.1, 0.08, 0.28), orange)
    _box("TC_HeavyCrashHull_V1_muted_orange_top_marking", (-2.3, 0.25, 2.92), (2.4, 0.32, 0.07), orange)
    _box("TC_HeavyCrashHull_V1_cyan_local_breach_slot", (0.85, -2.62, 1.65), (0.9, 0.08, 0.18), cyan)

    bpy.ops.object.light_add(type="AREA", location=(0, -6, 6))
    bpy.context.object.data.energy = 400
    bpy.context.object.data.size = 5
    SOURCE.parent.mkdir(parents=True, exist_ok=True)
    bpy.ops.wm.save_as_mainfile(filepath=str(SOURCE))
    print(f"TC_HEAVY_CRASH_HULL_SOURCE_WRITTEN {SOURCE}")


if __name__ == "__main__":
    main()
