#!/usr/bin/env python3
"""Create TC_TerrainDioramaKit_V1 Blender source and GLB export candidate.

Run with Blender:
  blender --background --python tools/blender/create_stage_a_terrain_diorama_kit_v1.py
"""
from __future__ import annotations

from math import cos, pi, radians, sin
from pathlib import Path

import bpy

ROOT = Path(__file__).resolve().parents[2]
SOURCE = ROOT / "assets/Source/Blender/StageA/TC_TerrainDioramaKit_V1.blend"
EXPORT = ROOT / "assets/Production/Generated/StageA/TC_TerrainDioramaKit_V1.glb"


def reset_scene() -> None:
    bpy.ops.object.select_all(action="SELECT")
    bpy.ops.object.delete()


def material(name: str, color: tuple[float, float, float, float], roughness: float = 0.88) -> bpy.types.Material:
    mat = bpy.data.materials.new(name)
    mat.diffuse_color = color
    mat.use_nodes = True
    bsdf = mat.node_tree.nodes.get("Principled BSDF")
    if bsdf:
        bsdf.inputs["Base Color"].default_value = color
        bsdf.inputs["Roughness"].default_value = roughness
        bsdf.inputs["Metallic"].default_value = 0.0
    return mat


def mesh_object(name: str, verts: list[tuple[float, float, float]], faces: list[tuple[int, ...]], mat: bpy.types.Material) -> bpy.types.Object:
    mesh = bpy.data.meshes.new(f"{name}Mesh")
    mesh.from_pydata(verts, [], faces)
    mesh.update()
    obj = bpy.data.objects.new(name, mesh)
    bpy.context.collection.objects.link(obj)
    mesh.materials.append(mat)
    obj["titancraft_collision"] = "none"
    obj["titancraft_visual_only"] = True
    return obj


def low_cylinder(name: str, radius: float, height: float, sides: int, z: float, mat: bpy.types.Material, scale_y: float = 1.0) -> bpy.types.Object:
    verts: list[tuple[float, float, float]] = []
    for level_z in (z, z + height):
        for i in range(sides):
            a = 2 * pi * i / sides
            wobble = 1.0 + 0.08 * sin(i * 1.7)
            verts.append((radius * wobble * cos(a), radius * scale_y * (1.0 + 0.06 * cos(i)) * sin(a), level_z))
    faces: list[tuple[int, ...]] = []
    faces.append(tuple(range(sides - 1, -1, -1)))
    faces.append(tuple(range(sides, sides * 2)))
    for i in range(sides):
        faces.append((i, (i + 1) % sides, sides + (i + 1) % sides, sides + i))
    return mesh_object(name, verts, faces, mat)


def basin(mat: bpy.types.Material) -> None:
    rings = [(0.0, 1), (3.0, 16), (6.4, 24), (9.4, 32)]
    verts: list[tuple[float, float, float]] = [(0, 0, -0.62)]
    ring_indices: list[list[int]] = [[0]]
    for r, count in rings[1:]:
        ids: list[int] = []
        for i in range(count):
            a = 2 * pi * i / count
            lane_lift = 0.16 if abs(sin(a)) < 0.22 or cos(a) > 0.72 else 0.0
            fracture = 0.13 * sin(i * 2.3) + 0.08 * cos(i * 4.1)
            y_scale = 0.78 + 0.04 * sin(i)
            z = -0.58 + 0.09 * r + lane_lift + fracture
            ids.append(len(verts))
            verts.append((r * (1 + 0.05 * sin(i * 1.9)) * cos(a), r * y_scale * sin(a), z))
        ring_indices.append(ids)
    faces: list[tuple[int, ...]] = []
    for i in range(16):
        faces.append((0, ring_indices[1][i], ring_indices[1][(i + 1) % 16]))
    for inner, outer in ((ring_indices[1], ring_indices[2]), (ring_indices[2], ring_indices[3])):
        ratio = len(outer) // len(inner)
        for i, start in enumerate(inner):
            faces.append((start, outer[i * ratio], outer[(i * ratio + 1) % len(outer)]))
            faces.append((start, outer[(i * ratio + 1) % len(outer)], inner[(i + 1) % len(inner)]))
    obj = mesh_object("TC_TerrainDioramaKit_V1_main_concave_ash_basin", verts, faces, mat)
    obj["asset_piece"] = "main concave ash basin"


def rock(name: str, x: float, y: float, z: float, sx: float, sy: float, sz: float, mat: bpy.types.Material, rot: float = 0) -> bpy.types.Object:
    verts = [(-.55,-.45,0),(.5,-.5,.08),(.58,.35,0),(-.45,.55,.05),(-.38,-.34,1),(.35,-.42,.86),(.45,.28,1.08),(-.32,.44,.78)]
    ca, sa = cos(radians(rot)), sin(radians(rot))
    out = []
    for vx, vy, vz in verts:
        px, py = vx * sx, vy * sy
        out.append((x + ca * px - sa * py, y + sa * px + ca * py, z + vz * sz))
    return mesh_object(name, out, [(0,1,2,3),(4,7,6,5),(0,4,5,1),(1,5,6,2),(2,6,7,3),(3,7,4,0)], mat)


def mound(name: str, x: float, y: float, sx: float, sy: float, h: float, mat: bpy.types.Material, sides: int = 12) -> bpy.types.Object:
    verts = [(x, y, h)]
    for i in range(sides):
        a = 2 * pi * i / sides
        verts.append((x + sx * (1 + .08 * sin(i)) * cos(a), y + sy * (1 + .06 * cos(i * 2)) * sin(a), -0.03))
    faces = [tuple(range(1, sides + 1))]
    for i in range(sides):
        faces.append((0, 1 + i, 1 + ((i + 1) % sides)))
    obj = mesh_object(name, verts, faces, mat)
    return obj


def add_box(name: str, loc: tuple[float, float, float], scale: tuple[float, float, float], mat: bpy.types.Material, rot_z: float = 0) -> bpy.types.Object:
    bpy.ops.mesh.primitive_cube_add(size=1, location=loc, rotation=(0, 0, radians(rot_z)))
    obj = bpy.context.object
    obj.name = name
    obj.data.name = f"{name}Mesh"
    obj.dimensions = scale
    bpy.ops.object.transform_apply(location=True, rotation=True, scale=True)
    obj.data.materials.append(mat)
    obj["titancraft_collision"] = "none"
    obj["titancraft_visual_only"] = True
    return obj


def main() -> None:
    reset_scene()
    ash = material("TC_MAT_matte_ash_gray_dust", (0.34, 0.33, 0.30, 1))
    ash_light = material("TC_MAT_dusty_route_edge_highlights", (0.55, 0.52, 0.44, 1))
    basalt = material("TC_MAT_faceted_dark_basalt", (0.075, 0.08, 0.085, 1))
    scorch = material("TC_MAT_muted_brown_scorch_contact", (0.22, 0.13, 0.075, 1))
    violet = material("TC_MAT_restrained_distant_violet_basalt", (0.16, 0.12, 0.21, 1))
    capsule_mat = material("TC_MAT_player_capsule_scale_reference", (0.15, 0.48, 0.72, 1))
    basin(ash)
    for i, a in enumerate([15, 48, 83, 128, 190, 235, 286, 330], 1):
        x, y = 9.0 * cos(radians(a)), 6.7 * sin(radians(a))
        rock(f"TC_TerrainDioramaKit_V1_raised_fractured_rim_segment_{i}", x, y, .25, 2.1, .65, 2.0 + (i % 3) * .45, basalt, a + 20)
    for i, (x, y, sx, sy, sz) in enumerate([(-5,-2,1.0,.8,1.5),(-2,3,1.3,.7,1.2),(3.7,-2.4,1.1,.9,1.7),(5.4,1.7,1.0,.8,1.25)], 1):
        rock(f"TC_TerrainDioramaKit_V1_basalt_outcrop_{i}", x, y, .0, sx, sy, sz, basalt, i * 31)
    for i, (x, y) in enumerate([(-3.8,-.9),(-1.3,1.2),(1.8,-1.1),(3.7,.9),(-.4,-2.7),(5.6,-.7)], 1):
        mound(f"TC_TerrainDioramaKit_V1_ash_drift_mound_{i}", x, y, 1.15, .42, .32, ash_light)
    for i, (x, y, sx, sy, h) in enumerate([(-4.7,.15,1.5,.75,.62),(-3.1,-1.25,1.25,.58,.5),(-2.0,1.4,1.0,.52,.42)], 1):
        mound(f"TC_TerrainDioramaKit_V1_hull_burial_contact_mound_{i}", x, y, sx, sy, h, scorch)
    for i, (x, y) in enumerate([(-6,1.7),(-3,2.15),(.2,2.0),(2.8,1.75),(5.2,1.35)], 1):
        rock(f"TC_TerrainDioramaKit_V1_route_edge_grounded_marker_{i}", x, y, .05, .42, .28, .75, ash_light if i % 2 else basalt, -12)
    for i, (x, y, sz) in enumerate([(-10,5.3,3.2),(-6,5.7,2.4),(0,6.0,3.6),(6.5,5.4,2.7),(10.5,4.9,3.4)], 1):
        rock(f"TC_TerrainDioramaKit_V1_distant_basalt_silhouette_piece_{i}", x, y, .25, 1.2, .35, sz, violet, 8 * i)
    add_box("TC_TerrainDioramaKit_V1_embedded_hull_proxy_scale_only", (-3.6, .1, .72), (3.4, 1.1, .42), scorch, -8)
    bpy.ops.mesh.primitive_uv_sphere_add(segments=12, ring_count=6, radius=.38, location=(1.7, -4.0, 1.25))
    bpy.context.object.name = "TC_TerrainDioramaKit_V1_player_capsule_scale_reference_head"
    bpy.context.object.data.materials.append(capsule_mat)
    bpy.ops.mesh.primitive_cylinder_add(vertices=12, radius=.42, depth=1.6, location=(1.7, -4.0, .45))
    bpy.context.object.name = "TC_TerrainDioramaKit_V1_player_capsule_scale_reference_body"
    bpy.context.object.data.materials.append(capsule_mat)
    for obj in bpy.context.scene.objects:
        if obj.type == "MESH":
            obj["titancraft_collision"] = "none"
            obj["titancraft_visual_only"] = True
    bpy.ops.object.light_add(type="AREA", location=(0, -6, 7))
    bpy.context.object.data.energy = 520
    bpy.context.object.data.size = 6
    bpy.ops.object.camera_add(location=(8, -8, 5.2), rotation=(radians(60), 0, radians(43)))
    bpy.context.scene.camera = bpy.context.object
    SOURCE.parent.mkdir(parents=True, exist_ok=True)
    bpy.ops.wm.save_as_mainfile(filepath=str(SOURCE))
    EXPORT.parent.mkdir(parents=True, exist_ok=True)
    bpy.ops.export_scene.gltf(filepath=str(EXPORT), export_format="GLB", export_apply=True, export_yup=True, export_materials="EXPORT", export_extras=True)
    print(f"TC_TERRAIN_DIORAMA_KIT_V1_SOURCE_WRITTEN {SOURCE}")
    print(f"TC_TERRAIN_DIORAMA_KIT_V1_GLB_WRITTEN {EXPORT}")


if __name__ == "__main__":
    main()
