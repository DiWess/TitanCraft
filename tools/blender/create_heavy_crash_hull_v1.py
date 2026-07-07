#!/usr/bin/env python3
"""Create the project-authored TC_HeavyCrashHull_V1 Blender source.

Run with Blender:
  blender --background --python tools/blender/create_heavy_crash_hull_v1.py
"""
from __future__ import annotations

from pathlib import Path
from math import radians
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


def _rotated_box(
    name: str,
    center: tuple[float, float, float],
    size: tuple[float, float, float],
    rotation_degrees: tuple[float, float, float],
    material: bpy.types.Material,
) -> bpy.types.Object:
    obj = _box(name, center, size, material)
    obj.rotation_euler = tuple(radians(value) for value in rotation_degrees)
    return obj


def _cylinder(
    name: str,
    center: tuple[float, float, float],
    radius: float,
    depth: float,
    vertices: int,
    material: bpy.types.Material,
) -> bpy.types.Object:
    bpy.ops.mesh.primitive_cylinder_add(vertices=vertices, radius=radius, depth=depth, location=center, rotation=(0, radians(90), 0))
    obj = bpy.context.object
    obj.name = name
    obj.data.name = f"{name}Mesh"
    bpy.ops.object.transform_apply(location=True, rotation=True, scale=True)
    obj.data.materials.append(material)
    obj["titancraft_collision"] = "none"
    obj["titancraft_visual_only"] = True
    return obj


def main() -> None:
    _reset_scene()
    hull = _material("TC_MAT_worn_off_white_hull", (0.72, 0.68, 0.58, 1))
    graphite = _material("TC_MAT_graphite_underside_interior", (0.08, 0.09, 0.09, 1))
    steel = _material("TC_MAT_worn_steel_ribs_panels", (0.38, 0.36, 0.32, 1))
    orange = _material("TC_MAT_muted_orange_markings", (0.75, 0.28, 0.08, 1))
    cyan = _material("TC_MAT_localized_cyan_breach_slot", (0.05, 0.65, 0.8, 1))

    hull_verts = [
        (-5.9, -2.0, 0.08), (-5.1, 1.95, 0.0), (-5.45, -1.25, 2.55), (-4.75, 1.45, 2.25),
        (-2.9, -2.35, -0.05), (-2.3, 2.15, 0.02), (-2.75, -1.75, 3.05), (-2.2, 1.75, 2.62),
        (0.6, -2.9, 0.0), (0.95, 2.05, -0.03), (0.2, -1.95, 3.15), (1.2, 1.55, 2.7),
        (3.4, -2.3, -0.12), (3.8, 2.1, 0.0), (3.1, -1.4, 2.95), (4.1, 1.4, 2.5),
        (5.15, -1.75, 0.05), (5.85, 1.55, 0.12), (4.65, -0.85, 2.55), (5.35, 1.0, 2.22),
    ]
    hull_faces = [
        (0, 4, 6, 2), (4, 8, 10, 6), (12, 16, 18, 14),
        (1, 3, 7, 5), (5, 7, 11, 9), (9, 11, 15, 13), (13, 15, 19, 17),
        (2, 6, 7, 3), (6, 10, 11, 7), (10, 14, 15, 11), (14, 18, 19, 15),
        (0, 1, 5, 4), (4, 5, 9, 8), (8, 9, 13, 12), (12, 13, 17, 16),
        (0, 2, 3, 1), (16, 17, 19, 18),
    ]
    _mesh_object("TC_HeavyCrashHull_V1_broken_asymmetric_industrial_hull_shell", hull_verts, hull_faces, hull)

    underside_verts = [
        (-5.6, -1.75, -0.08), (-5.0, 1.65, -0.08), (5.2, 1.75, -0.08), (4.7, -1.9, -0.08),
        (-5.3, -1.45, 0.18), (4.5, -1.55, 0.18), (5.0, 1.4, 0.18), (-4.8, 1.35, 0.18),
    ]
    _mesh_object("TC_HeavyCrashHull_V1_flat_graphite_underside", underside_verts, [(0, 1, 2, 3), (0, 4, 7, 1), (3, 2, 6, 5)], graphite)

    for index, x in enumerate([-2.9, -1.85, -0.8, 0.3, 1.45, 2.55], start=1):
        _rotated_box(f"TC_HeavyCrashHull_V1_large_exposed_internal_rib_{index}", (x, -2.58, 1.38), (0.4, 0.62, 2.85), (0, 0, -2 if index % 2 else 2), steel)
        _box(f"TC_HeavyCrashHull_V1_graphite_void_between_ribs_{index}", (x + 0.36, -2.7, 1.36), (0.3, 0.1, 1.7), graphite)
    _rotated_box("TC_HeavyCrashHull_V1_thick_torn_breach_lower_lip", (-0.55, -2.78, 0.46), (5.9, 0.42, 0.42), (0, 0, -4), steel)
    _rotated_box("TC_HeavyCrashHull_V1_thick_torn_breach_upper_lip", (-0.35, -2.68, 2.72), (5.4, 0.42, 0.36), (0, 0, 5), steel)
    _rotated_box("TC_HeavyCrashHull_V1_breach_forward_ripped_vertical_edge", (-3.55, -2.62, 1.48), (0.42, 0.48, 2.45), (0, 0, -16), steel)
    _rotated_box("TC_HeavyCrashHull_V1_breach_rear_ripped_vertical_edge", (2.75, -2.7, 1.44), (0.46, 0.5, 2.2), (0, 0, 13), steel)
    for index, (x, z, rz) in enumerate([(-3.1, 2.35, -6), (-1.25, 2.75, 7), (1.05, 2.8, -5), (2.7, 2.4, 8)], start=1):
        _rotated_box(f"TC_HeavyCrashHull_V1_curled_torn_breach_panel_{index}", (x, -2.82, z), (1.05, 0.32, 0.24), (0, 0, rz), hull)
    for index, (x, z, rz) in enumerate([(-3.65, 0.1, 4), (-2.05, -0.05, -3), (0.75, -0.08, 3), (2.35, 0.02, -4), (4.05, 0.0, 4)], start=1):
        _rotated_box(f"TC_HeavyCrashHull_V1_uneven_buried_torn_lower_edge_{index}", (x, -0.25, z), (1.05, 4.4, 0.24), (0, 0, rz), graphite if index % 2 else steel)

    for index, x in enumerate([-3.8, -1.2, 1.4, 3.6], start=1):
        _box(f"TC_HeavyCrashHull_V1_manufactured_hull_panel_{index}", (x, 2.08, 1.35), (1.55, 0.22, 1.05), hull)
    for index, x in enumerate([-4.0, -1.0, 2.2], start=1):
        _box(f"TC_HeavyCrashHull_V1_worn_steel_panel_seam_{index}", (x, 2.22, 1.4), (0.12, 0.18, 2.1), steel)

    _rotated_box("TC_HeavyCrashHull_V1_crushed_front_folded_offwhite_plate_left", (-5.95, -0.75, 1.05), (0.72, 1.85, 1.65), (0, -18, -18), hull)
    _rotated_box("TC_HeavyCrashHull_V1_crushed_front_folded_offwhite_plate_right", (-5.72, 0.85, 1.34), (0.58, 1.25, 1.85), (0, 14, 17), hull)
    _box("TC_HeavyCrashHull_V1_crushed_front_large_graphite_void", (-6.15, -0.28, 1.12), (0.52, 2.35, 1.95), graphite)
    for index, (y, z, ry) in enumerate([(-1.55, 1.9, -10), (-0.65, 0.7, 8), (0.35, 2.05, -6), (1.25, 0.95, 11)], start=1):
        _rotated_box(f"TC_HeavyCrashHull_V1_jagged_front_steel_fracture_strut_{index}", (-5.82, y, z), (0.34, 0.42, 1.45), (0, ry, 0), steel)

    _box("TC_HeavyCrashHull_V1_deep_rear_engine_mount_graphite_socket", (5.82, 0, 1.22), (0.82, 2.95, 2.05), graphite)
    _cylinder("TC_HeavyCrashHull_V1_rear_engine_outer_worn_steel_ring", (6.28, 0, 1.25), 1.32, 0.34, 20, steel)
    _cylinder("TC_HeavyCrashHull_V1_rear_engine_inner_graphite_nozzle", (6.52, 0, 1.25), 0.76, 0.52, 18, graphite)
    _cylinder("TC_HeavyCrashHull_V1_rear_engine_broken_core_ring", (6.86, 0, 1.25), 0.48, 0.25, 16, steel)
    for index, angle in enumerate([0, 45, 90, 135, 180, 225, 270, 315], start=1):
        _rotated_box(f"TC_HeavyCrashHull_V1_rear_engine_radial_vane_{index}", (6.48, 0, 1.25), (0.28, 0.16, 1.72), (angle, 0, 0), steel)
    for index, (y, z, rz) in enumerate([(-1.35, 2.25, -12), (1.35, 2.05, 10), (-1.32, 0.3, 15), (1.25, 0.45, -14)], start=1):
        _rotated_box(f"TC_HeavyCrashHull_V1_rear_engine_heavy_mount_strut_{index}", (5.72, y, z), (1.25, 0.28, 0.34), (0, 0, rz), steel)

    _box("TC_HeavyCrashHull_V1_muted_orange_side_marking", (1.6, 2.34, 2.05), (2.1, 0.08, 0.28), orange)
    _box("TC_HeavyCrashHull_V1_muted_orange_top_marking", (-2.3, 0.25, 2.92), (2.4, 0.32, 0.07), orange)
    _box("TC_HeavyCrashHull_V1_cyan_local_breach_slot", (0.85, -3.04, 1.65), (1.45, 0.1, 0.22), cyan)

    bpy.ops.object.light_add(type="AREA", location=(0, -6, 6))
    bpy.context.object.data.energy = 400
    bpy.context.object.data.size = 5
    SOURCE.parent.mkdir(parents=True, exist_ok=True)
    bpy.ops.wm.save_as_mainfile(filepath=str(SOURCE))
    print(f"TC_HEAVY_CRASH_HULL_SOURCE_WRITTEN {SOURCE}")


if __name__ == "__main__":
    main()
