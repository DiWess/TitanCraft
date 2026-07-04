#!/usr/bin/env python3
"""Render the six local review PNGs for TC_HeavyCrashHull_V1.

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


def _look_at(obj: bpy.types.Object, target: tuple[float, float, float]) -> None:
    direction = Vector(target) - obj.location
    obj.rotation_euler = direction.to_track_quat("-Z", "Y").to_euler()


def _apply_neutral_material() -> None:
    neutral = bpy.data.materials.new("TC_NeutralReviewGrey")
    neutral.diffuse_color = (0.62, 0.62, 0.58, 1)
    for obj in bpy.context.scene.objects:
        if obj.type == "MESH":
            obj.data.materials.clear()
            obj.data.materials.append(neutral)


def _render(name: str, camera_location: tuple[float, float, float], neutral: bool) -> None:
    bpy.ops.wm.open_mainfile(filepath=str(SOURCE))
    if neutral:
        _apply_neutral_material()
    bpy.ops.object.light_add(type="AREA", location=(0, -5, 6))
    bpy.context.object.data.energy = 650
    bpy.context.object.data.size = 5
    bpy.ops.object.camera_add(location=camera_location)
    camera = bpy.context.object
    _look_at(camera, (0, 0, 1.2))
    camera.data.lens = 35
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
        _render(f"{prefix}_front_three_quarter.png", (-8, -6, 4), neutral)
        _render(f"{prefix}_side_silhouette.png", (0, -10, 2.4), neutral)
        _render(f"{prefix}_rear_engine.png", (8, 5, 3.6), neutral)


if __name__ == "__main__":
    main()
