#!/usr/bin/env python3
"""Render standard review PNGs for the already-committed TC_HullRibOccluder_V1 OBJ.

TC_HullRibOccluder_V1 was authored directly as a committed text OBJ
(tools/blender/create_hull_rib_occluder_v1.py) with no render step in its
original script, unlike every other Blender Asset Forge kit. This script
adds the missing evidence step without changing the committed mesh: it
imports the existing OBJ read-only, assigns a neutral steel material
consistent with the other Stage A wreckage props (matching
TC_MAT_worn_steel_ribs_panels' grey used on TC_HeavyCrashHull_V1), and
reuses the auto-framed 3-view render approach from render_mvp_asset_review.py.

Run with Blender:
  blender --background --python tools/blender/render_hull_rib_occluder_v1_reviews.py
"""
from __future__ import annotations

import math
from pathlib import Path

import bpy
from mathutils import Vector

ROOT = Path(__file__).resolve().parents[2]
SOURCE_OBJ = ROOT / "assets/Production/Custom/StageA/TC_HullRibOccluder_V1.obj"
OUTPUT_DIR = ROOT / "artifacts/asset-review/TC_HullRibOccluder_V1"

STEEL_GREY = (0.180, 0.190, 0.205)

VIEWS = {
    "hero_three_quarter.png": (-35, 18, 2.6),
    "front.png": (-90, 8, 2.8),
    "back_three_quarter.png": (140, 22, 2.8),
}


def scene_bounds() -> tuple[Vector, Vector]:
    low = Vector((1e9, 1e9, 1e9))
    high = Vector((-1e9, -1e9, -1e9))
    depsgraph = bpy.context.evaluated_depsgraph_get()
    for obj in bpy.context.scene.objects:
        if obj.type != "MESH":
            continue
        mesh = obj.evaluated_get(depsgraph).to_mesh()
        for vertex in mesh.vertices:
            world = obj.matrix_world @ vertex.co
            low = Vector(map(min, low, world))
            high = Vector(map(max, high, world))
        obj.evaluated_get(depsgraph).to_mesh_clear()
    return low, high


def setup_stage(radius: float) -> None:
    scene = bpy.context.scene
    if scene.world is None:
        scene.world = bpy.data.worlds.new("ReviewWorld")
    scene.world.use_nodes = True
    bg = scene.world.node_tree.nodes["Background"]
    bg.inputs[0].default_value = (0.016, 0.017, 0.022, 1)
    bg.inputs[1].default_value = 1.0

    bpy.ops.mesh.primitive_plane_add(size=radius * 30, location=(0, 0, -0.002))
    floor = bpy.context.object
    floor_mat = bpy.data.materials.new("TC_ReviewFloor")
    floor_mat.use_nodes = True
    bsdf = floor_mat.node_tree.nodes["Principled BSDF"]
    bsdf.inputs["Base Color"].default_value = (0.045, 0.047, 0.055, 1)
    bsdf.inputs["Roughness"].default_value = 0.85
    floor.data.materials.append(floor_mat)

    key_positions = (
        ((-1.2, -1.6, 2.0), 85, 4.0, (1.0, 0.92, 0.80)),
        ((1.8, -0.4, 1.2), 22, 3.0, (0.65, 0.72, 1.0)),
        ((0.2, 1.9, 0.9), 30, 3.5, (0.85, 0.70, 1.0)),
    )
    for direction, energy, size, color in key_positions:
        bpy.ops.object.light_add(type="AREA", location=[c * radius for c in direction])
        light = bpy.context.object
        light.data.energy = energy * radius * radius
        light.data.size = size * radius * 0.5
        light.data.color = color
        target = Vector((0, 0, radius * 0.35))
        direction_vec = target - light.location
        light.rotation_euler = direction_vec.to_track_quat("-Z", "Y").to_euler()


def render_view(name: str, yaw: float, pitch: float, dist_mul: float, center: Vector, radius: float, output_dir: Path) -> None:
    yaw_r, pitch_r = math.radians(yaw), math.radians(pitch)
    dist = radius * dist_mul
    location = center + Vector((
        dist * math.cos(pitch_r) * math.cos(yaw_r),
        dist * math.cos(pitch_r) * math.sin(yaw_r),
        dist * math.sin(pitch_r),
    ))
    bpy.ops.object.camera_add(location=location)
    camera = bpy.context.object
    direction = center - camera.location
    camera.rotation_euler = direction.to_track_quat("-Z", "Y").to_euler()
    camera.data.lens = 45
    bpy.context.scene.camera = camera
    bpy.context.scene.render.filepath = str(output_dir / name)
    bpy.ops.render.render(write_still=True)
    bpy.data.objects.remove(camera, do_unlink=True)
    print(f"HULL_RIB_OCCLUDER_REVIEW_RENDERED {output_dir / name}")


def main() -> None:
    bpy.ops.wm.read_factory_settings(use_empty=True)
    bpy.ops.wm.obj_import(filepath=str(SOURCE_OBJ))
    imported = [o for o in bpy.context.selected_objects if o.type == "MESH"]
    if not imported:
        raise SystemExit(f"no mesh imported from {SOURCE_OBJ}")
    mat = bpy.data.materials.new("TC_HullRibOccluder_ReviewSteel")
    mat.use_nodes = True
    mat.node_tree.nodes["Principled BSDF"].inputs["Base Color"].default_value = (*STEEL_GREY, 1.0)
    mat.node_tree.nodes["Principled BSDF"].inputs["Roughness"].default_value = 0.7
    for obj in imported:
        obj.data.materials.append(mat)

    OUTPUT_DIR.mkdir(parents=True, exist_ok=True)
    low, high = scene_bounds()
    center = (low + high) / 2
    radius = max((high - low).length / 2, 0.2)
    setup_stage(radius)

    scene = bpy.context.scene
    scene.render.engine = "CYCLES"
    scene.cycles.samples = 48
    scene.cycles.use_denoising = bool(getattr(bpy.app.build_options, "openimagedenoise", False))
    scene.cycles.device = "CPU"
    scene.render.resolution_x = 960
    scene.render.resolution_y = 720
    scene.view_settings.view_transform = "Filmic"
    scene.view_settings.look = "Medium High Contrast"

    for name, (yaw, pitch, dist_mul) in VIEWS.items():
        render_view(name, yaw, pitch, dist_mul, center, radius, OUTPUT_DIR)


if __name__ == "__main__":
    main()
