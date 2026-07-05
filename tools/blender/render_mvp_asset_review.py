#!/usr/bin/env python3
"""Render auto-framed review PNGs for one MVP Asset Pack source.

Uses Cycles CPU so it works in headless containers without a GPU/GL context.

Run with Blender:
  blender --background --python tools/blender/render_mvp_asset_review.py -- source.blend output_dir
"""
from __future__ import annotations

import math
import sys
from pathlib import Path

import bpy
from mathutils import Vector

# View name -> (yaw degrees, pitch degrees, distance multiplier)
VIEWS = {
    "hero_three_quarter.png": (-35, 18, 2.6),
    "front.png": (-90, 8, 2.8),
    "back_three_quarter.png": (140, 22, 2.8),
}


def args() -> tuple[Path, Path]:
    if "--" not in sys.argv:
        raise SystemExit("usage: blender --background --python render_mvp_asset_review.py -- source.blend output_dir")
    tail = sys.argv[sys.argv.index("--") + 1:]
    if len(tail) != 2:
        raise SystemExit("expected source.blend and output_dir")
    return Path(tail[0]), Path(tail[1])


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
    mat = bpy.data.materials.new("ReviewFloor")
    mat.use_nodes = True
    bsdf = mat.node_tree.nodes["Principled BSDF"]
    bsdf.inputs["Base Color"].default_value = (0.045, 0.047, 0.055, 1)
    bsdf.inputs["Roughness"].default_value = 0.85
    floor.data.materials.append(mat)
    key_positions = (
        ((-1.2, -1.6, 2.0), 85, 4.0, (1.0, 0.92, 0.80)),
        ((1.8, -0.4, 1.2), 22, 3.0, (0.65, 0.72, 1.0)),
        ((0.2, 1.9, 0.9), 30, 3.5, (0.85, 0.70, 1.0)),
    )
    for i, (direction, energy, size, color) in enumerate(key_positions):
        bpy.ops.object.light_add(type="AREA", location=[c * radius for c in direction])
        light = bpy.context.object
        light.data.energy = energy * radius * radius
        light.data.size = size * radius * 0.5
        light.data.color = color
        target = Vector((0, 0, radius * 0.35))
        direction_vec = target - light.location
        light.rotation_euler = direction_vec.to_track_quat("-Z", "Y").to_euler()


def render_view(name: str, yaw: float, pitch: float, dist_mul: float,
                center: Vector, radius: float, extents: Vector, output_dir: Path) -> None:
    # Guarantee tall or wide assets fit: 45mm lens on a 36mm sensor at 4:3
    # has ~21.8 deg horizontal and ~16.7 deg vertical half-FOV.
    fit_v = (extents.z / 2) / math.tan(math.radians(16.7)) * 1.35 + radius * 0.4
    fit_h = (max(extents.x, extents.y) / 2) / math.tan(math.radians(21.8)) * 1.25 + radius * 0.4
    dist = max(radius * dist_mul, fit_v, fit_h)
    yaw_r, pitch_r = math.radians(yaw), math.radians(pitch)
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
    print(f"MVP_REVIEW_RENDERED {output_dir / name}")


def main() -> None:
    source, output_dir = args()
    bpy.ops.wm.open_mainfile(filepath=str(source))
    output_dir.mkdir(parents=True, exist_ok=True)
    low, high = scene_bounds()
    center = (low + high) / 2
    radius = max((high - low).length / 2, 0.2)
    setup_stage(radius)
    scene = bpy.context.scene
    scene.render.engine = "CYCLES"
    scene.cycles.samples = 48
    scene.cycles.use_denoising = True
    scene.cycles.device = "CPU"
    scene.render.resolution_x = 960
    scene.render.resolution_y = 720
    scene.view_settings.view_transform = "Filmic"
    scene.view_settings.look = "Medium High Contrast"
    for name, (yaw, pitch, dist_mul) in VIEWS.items():
        render_view(name, yaw, pitch, dist_mul, center, radius, high - low, output_dir)


if __name__ == "__main__":
    main()
