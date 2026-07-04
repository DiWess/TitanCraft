#!/usr/bin/env python3
"""Export one TitanCraft Blender source asset to GLB.

Run with Blender:
  blender --background --python tools/blender/export_asset.py -- source.blend out.glb
"""
from __future__ import annotations

import sys
from pathlib import Path
import bpy


def _args() -> tuple[Path, Path]:
    if "--" not in sys.argv:
        raise SystemExit("usage: blender --background --python tools/blender/export_asset.py -- source.blend out.glb")
    tail = sys.argv[sys.argv.index("--") + 1:]
    if len(tail) != 2:
        raise SystemExit("expected source .blend and destination .glb")
    source, dest = Path(tail[0]), Path(tail[1])
    if source.suffix.lower() != ".blend":
        raise SystemExit(f"source must be .blend: {source}")
    if dest.suffix.lower() not in {".glb", ".gltf"}:
        raise SystemExit(f"destination must be .glb/.gltf: {dest}")
    return source, dest


def _mark_visual_only() -> None:
    for obj in bpy.context.scene.objects:
        if obj.type == "MESH":
            obj["titancraft_visual_only"] = True
            obj["titancraft_collision"] = "none"


def main() -> None:
    source, dest = _args()
    bpy.ops.wm.open_mainfile(filepath=str(source))
    if not any(obj.type == "MESH" for obj in bpy.context.scene.objects):
        raise SystemExit(f"no mesh objects found in {source}")
    _mark_visual_only()
    dest.parent.mkdir(parents=True, exist_ok=True)
    bpy.ops.export_scene.gltf(
        filepath=str(dest),
        export_format="GLB",
        export_apply=True,
        export_yup=True,
        export_materials="EXPORT",
        use_selection=False,
        export_extras=True,
    )
    print(f"BLENDER_ASSET_EXPORTED source={source} dest={dest}")


if __name__ == "__main__":
    main()
