#!/usr/bin/env python3
"""Render the six local review PNGs for TC_HeavyCrashHull_V1.

Cameras auto-frame from the evaluated scene bounds so every view shows the
full hull silhouette, and each render includes a 1.8 m astronaut-height
scale reference (render-only, never part of the exported asset) per the
Stage A review lesson that unframed or scale-less PNGs are not reviewable.

Run with Blender after source generation:
  blender --background --python tools/blender/render_heavy_crash_hull_v1_reviews.py
"""
from __future__ import annotations

from pathlib import Path
import bpy
from mathutils import Vector

ROOT = Path(__file__).resolve().parents[2]
SOURCE = ROOT / "assets/Source/Blender/Production/TC_HeavyCrashHull_V1.blend"
REVIEW_DIR = ROOT / "artifacts/asset-review/TC_HeavyCrashHull_V1"

SCALE_REFERENCE_HEIGHT_M = 1.8

# View name -> (unit direction from hull center to camera, distance multiplier, lens mm)
VIEWS = {
    "front_three_quarter.png": (Vector((-0.66, -0.62, 0.42)), 2.3, 34),
    "side_silhouette.png": (Vector((0.0, -1.0, 0.18)), 2.4, 38),
    "rear_engine.png": (Vector((1.0, -0.28, 0.22)), 2.1, 30),
}


def _look_at(obj: bpy.types.Object, target: Vector) -> None:
    direction = target - obj.location
    obj.rotation_euler = direction.to_track_quat("-Z", "Y").to_euler()


def _apply_neutral_material() -> None:
    neutral = bpy.data.materials.new("TC_NeutralReviewGrey")
    neutral.diffuse_color = (0.62, 0.62, 0.58, 1)
    for obj in bpy.context.scene.objects:
        if obj.type == "MESH":
            obj.data.materials.clear()
            obj.data.materials.append(neutral)


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


def _add_scale_reference(bounds_low: Vector, bounds_high: Vector) -> None:
    # A 1.8 m astronaut-height post beside the hull's -Y edge keeps human
    # scale readable in every framed view without touching the asset data.
    base_x = bounds_low.x + (bounds_high.x - bounds_low.x) * 0.25
    base_y = bounds_low.y - 1.2
    bpy.ops.mesh.primitive_cylinder_add(
        vertices=12,
        radius=0.22,
        depth=SCALE_REFERENCE_HEIGHT_M,
        location=(base_x, base_y, SCALE_REFERENCE_HEIGHT_M / 2),
    )
    post = bpy.context.object
    post.name = "TC_ReviewScaleReference_1p8m"
    marker = bpy.data.materials.new("TC_ReviewScaleReferenceOrange")
    marker.diffuse_color = (0.85, 0.30, 0.05, 1)
    post.data.materials.append(marker)


def _render(name: str, view_direction: Vector, distance_multiplier: float, lens: float, neutral: bool) -> None:
    bpy.ops.wm.open_mainfile(filepath=str(SOURCE))
    if neutral:
        _apply_neutral_material()

    low, high = _scene_bounds()
    center = (low + high) / 2
    radius = max((high - low).length / 2, 1.0)
    _add_scale_reference(low, high)

    camera_location = center + view_direction.normalized() * radius * distance_multiplier
    camera_location.z = max(camera_location.z, center.z + radius * 0.15)

    bpy.ops.object.light_add(type="AREA", location=(center.x, center.y - radius * 1.5, center.z + radius * 1.5))
    bpy.context.object.data.energy = 650 * radius
    bpy.context.object.data.size = radius * 1.5
    bpy.ops.object.light_add(type="AREA", location=camera_location)
    bpy.context.object.data.energy = 450 * radius
    bpy.context.object.data.size = radius

    bpy.ops.object.camera_add(location=camera_location)
    camera = bpy.context.object
    _look_at(camera, center)
    camera.data.lens = lens
    camera.data.clip_end = radius * 20
    bpy.context.scene.camera = camera
    bpy.context.scene.render.resolution_x = 1280
    bpy.context.scene.render.resolution_y = 720
    bpy.context.scene.eevee.taa_render_samples = 32
    bpy.context.scene.world.color = (0.075, 0.085, 0.095)
    REVIEW_DIR.mkdir(parents=True, exist_ok=True)
    bpy.context.scene.render.filepath = str(REVIEW_DIR / name)
    bpy.ops.render.render(write_still=True)
    print(f"TC_HEAVY_CRASH_HULL_REVIEW_RENDERED {REVIEW_DIR / name}")


def main() -> None:
    if not SOURCE.exists():
        raise SystemExit(f"missing generated source blend: {SOURCE}")
    for prefix, neutral in (("neutral", True), ("material", False)):
        for view_name, (direction, multiplier, lens) in VIEWS.items():
            _render(f"{prefix}_{view_name}", direction, multiplier, lens, neutral)


if __name__ == "__main__":
    main()
