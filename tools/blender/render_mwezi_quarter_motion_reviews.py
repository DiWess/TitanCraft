#!/usr/bin/env python3
"""Render motion review PNGs for the animated Mwezi Quarter assets.

A still of an animated prop proves nothing: a rig that never moves renders
identically to one that does. Each asset is therefore rendered at four frames
spread across its loop, from a fixed camera, so the displacement between
frames is the evidence.

Every view carries a render-only 1.8 m scale post and a ground plane, matching
the static kit's review contract.

Run with Blender:
  blender --background --python tools/blender/render_mwezi_quarter_motion_reviews.py
  blender --background --python tools/blender/render_mwezi_quarter_motion_reviews.py -- --asset TC_ENV_PalmSway_V1
"""
from __future__ import annotations

import math
import sys
from pathlib import Path

import bpy
from mathutils import Vector

ROOT = Path(__file__).resolve().parents[2]
SOURCE_DIR = ROOT / "assets/Source/Blender/Production/MweziQuarter_V1"
REVIEW_ROOT = ROOT / "artifacts/asset-review"

SCALE_REFERENCE_HEIGHT_M = 1.8
RESOLUTION_X = 1280
RESOLUTION_Y = 720
SENSOR_WIDTH_MM = 36.0
LENS_MM = 38.0
FRAMING_MARGIN = 1.22

ASSETS = [
    "TC_ENV_PalmSway_V1",
    "TC_ENV_LaundryLine_V1",
    "TC_ENV_AwningCloth_V1",
    "TC_ENV_BannerCloth_V1",
]

# Frames across the 120-frame loop: rest, out, back through rest, and return.
MOTION_FRAMES = [1, 30, 60, 90]
VIEW_DIRECTION = Vector((-0.62, -0.84, 0.22))


def _look_at(obj: bpy.types.Object, target: Vector) -> None:
    direction = target - obj.location
    obj.rotation_euler = direction.to_track_quat("-Z", "Y").to_euler()


def _scene_bounds() -> tuple[Vector, Vector]:
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


def _framing_distance(low: Vector, high: Vector) -> float:
    size = high - low
    sensor_height_mm = SENSOR_WIDTH_MM * RESOLUTION_Y / RESOLUTION_X
    half_h = math.atan((SENSOR_WIDTH_MM / 2.0) / LENS_MM)
    half_v = math.atan((sensor_height_mm / 2.0) / LENS_MM)
    return max(
        (math.hypot(size.x, size.y) / 2.0) / math.tan(half_h),
        (size.z / 2.0) / math.tan(half_v),
        1.5,
    ) * FRAMING_MARGIN


def _add_props(low: Vector, high: Vector) -> None:
    bpy.ops.mesh.primitive_cylinder_add(
        vertices=12, radius=0.22, depth=SCALE_REFERENCE_HEIGHT_M,
        location=(low.x + (high.x - low.x) * 0.16, low.y - 1.0, SCALE_REFERENCE_HEIGHT_M / 2),
    )
    post = bpy.context.object
    post.name = "TC_ReviewScaleReference_1p8m"
    marker = bpy.data.materials.new("TC_ReviewScaleReferenceOrange")
    marker.use_nodes = True
    marker.node_tree.nodes["Principled BSDF"].inputs["Base Color"].default_value = (0.85, 0.30, 0.05, 1)
    post.data.materials.append(marker)

    span = max((high - low).length, 6.0) * 2.0
    bpy.ops.mesh.primitive_plane_add(size=span, location=(0.0, 0.0, min(low.z, 0.0) - 0.01))
    plane = bpy.context.object
    plane.name = "TC_ReviewGroundPlane"
    mat = bpy.data.materials.new("TC_ReviewGround")
    mat.use_nodes = True
    bsdf = mat.node_tree.nodes["Principled BSDF"]
    bsdf.inputs["Base Color"].default_value = (0.30, 0.28, 0.25, 1)
    bsdf.inputs["Roughness"].default_value = 0.95
    plane.data.materials.append(mat)


def _render_asset(asset: str) -> None:
    source = SOURCE_DIR / f"{asset}.blend"
    bpy.ops.wm.open_mainfile(filepath=str(source))
    if not any(obj.type == "ARMATURE" for obj in bpy.context.scene.objects):
        raise SystemExit(f"{asset} has no armature; it is not an animated asset")

    low, high = _scene_bounds()
    center = (low + high) / 2
    radius = max((high - low).length / 2, 1.0)
    _add_props(low, high)

    distance = _framing_distance(low, high)
    camera_location = center + VIEW_DIRECTION.normalized() * distance
    bpy.ops.object.camera_add(location=camera_location)
    camera = bpy.context.object
    _look_at(camera, center)
    camera.data.lens = LENS_MM
    camera.data.sensor_width = SENSOR_WIDTH_MM
    camera.data.clip_end = max(radius, distance) * 30
    bpy.context.scene.camera = camera

    bpy.ops.object.light_add(type="AREA", location=(center.x - radius * 1.4, center.y - radius * 1.6, center.z + radius * 2.0))
    bpy.context.object.data.energy = 700 * radius
    bpy.context.object.data.size = radius * 1.6
    bpy.ops.object.light_add(type="AREA", location=(center.x + radius * 1.8, center.y + radius * 1.2, center.z + radius * 1.1))
    bpy.context.object.data.energy = 220 * radius
    bpy.context.object.data.size = radius * 1.4

    bpy.context.scene.render.resolution_x = RESOLUTION_X
    bpy.context.scene.render.resolution_y = RESOLUTION_Y
    if hasattr(bpy.context.scene, "eevee"):
        bpy.context.scene.eevee.taa_render_samples = 32
    bpy.context.scene.world.color = (0.16, 0.19, 0.23)

    output_dir = REVIEW_ROOT / asset
    output_dir.mkdir(parents=True, exist_ok=True)
    for frame in MOTION_FRAMES:
        bpy.context.scene.frame_set(frame)
        bpy.context.view_layer.update()
        path = output_dir / f"motion_frame_{frame:03d}.png"
        bpy.context.scene.render.filepath = str(path)
        bpy.ops.render.render(write_still=True)
        print(f"MOTION_REVIEW_RENDERED {path}")


def main() -> None:
    only = None
    if "--" in sys.argv:
        tail = sys.argv[sys.argv.index("--") + 1:]
        if len(tail) == 2 and tail[0] == "--asset":
            only = tail[1]
        elif tail:
            raise SystemExit("usage: blender --background --python render_mwezi_quarter_motion_reviews.py [-- --asset TC_NAME]")
    for asset in ([only] if only else ASSETS):
        if not (SOURCE_DIR / f"{asset}.blend").exists():
            raise SystemExit(f"missing generated source blend: {asset}")
        _render_asset(asset)


if __name__ == "__main__":
    main()
