#!/usr/bin/env python3
"""Export one animated TitanCraft Blender source asset to GLB.

Separate from `tools/blender/export_asset.py` because that exporter passes
`export_apply=True`, which applies modifiers -- including the Armature modifier
that carries the skin. Applying it would bake the rest pose and silently strip
the motion, producing a file that looks correct and never moves.

Run with Blender:
  blender --background --python tools/blender/export_animated_asset.py -- source.blend out.glb
"""
from __future__ import annotations

import sys
from pathlib import Path
import bpy


def _args() -> tuple[Path, Path]:
    if "--" not in sys.argv:
        raise SystemExit("usage: blender --background --python tools/blender/export_animated_asset.py -- source.blend out.glb")
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
    if not any(obj.type == "ARMATURE" for obj in bpy.context.scene.objects):
        raise SystemExit(f"no armature found in {source}; use export_asset.py for static assets")
    if not bpy.data.actions:
        raise SystemExit(f"no action found in {source}; the asset would export without motion")
    _mark_visual_only()
    dest.parent.mkdir(parents=True, exist_ok=True)
    bpy.ops.export_scene.gltf(
        filepath=str(dest),
        export_format="GLB",
        export_apply=False,
        export_yup=True,
        export_materials="EXPORT",
        use_selection=False,
        export_extras=True,
        export_animations=True,
        export_frame_range=True,
        export_force_sampling=True,
        export_skins=True,
    )
    print(f"BLENDER_ANIMATED_ASSET_EXPORTED source={source} dest={dest}")


if __name__ == "__main__":
    main()
