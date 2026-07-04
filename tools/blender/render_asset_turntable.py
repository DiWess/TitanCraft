#!/usr/bin/env python3
"""Render a single PNG turntable/contact-sheet for a Blender asset.

Run with Blender:
  blender --background --python tools/blender/render_asset_turntable.py -- source.blend out.png neutral|material
"""
from __future__ import annotations

import math
import sys
from pathlib import Path
import bpy
from mathutils import Vector


def _args() -> tuple[Path, Path, str]:
    if "--" not in sys.argv:
        raise SystemExit("usage: blender --background --python render_asset_turntable.py -- source.blend out.png neutral|material")
    tail = sys.argv[sys.argv.index("--") + 1:]
    if len(tail) != 3:
        raise SystemExit("expected source.blend output.png neutral|material")
    mode = tail[2]
    if mode not in {"neutral", "material"}:
        raise SystemExit("mode must be neutral or material")
    return Path(tail[0]), Path(tail[1]), mode


def _look_at(obj: bpy.types.Object, target: Vector) -> None:
    direction = target - obj.location
    obj.rotation_euler = direction.to_track_quat('-Z', 'Y').to_euler()


def main() -> None:
    source, output, mode = _args()
    bpy.ops.wm.open_mainfile(filepath=str(source))
    if mode == "neutral":
        neutral = bpy.data.materials.new("TC_NeutralReviewGrey")
        neutral.diffuse_color = (0.62, 0.62, 0.58, 1)
        for obj in bpy.context.scene.objects:
            if obj.type == "MESH":
                obj.data.materials.clear(); obj.data.materials.append(neutral)
    bpy.ops.object.light_add(type="AREA", location=(2.5, -3.5, 4.0))
    bpy.context.object.data.energy = 420
    bpy.context.object.data.size = 4
    bpy.ops.object.camera_add(location=(3.2, -4.2, 2.8))
    camera = bpy.context.object
    _look_at(camera, Vector((0, 0, 0.45)))
    camera.data.lens = 55
    bpy.context.scene.camera = camera
    bpy.context.scene.render.resolution_x = 1280
    bpy.context.scene.render.resolution_y = 720
    bpy.context.scene.eevee.taa_render_samples = 32
    bpy.context.scene.world.color = (0.08, 0.10, 0.12)
    output.parent.mkdir(parents=True, exist_ok=True)
    bpy.context.scene.render.filepath = str(output)
    bpy.ops.render.render(write_still=True)
    print(f"BLENDER_TURNTABLE_RENDERED mode={mode} output={output}")


if __name__ == "__main__":
    main()
