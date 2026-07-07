#!/usr/bin/env python3
"""Create the TC_ENV_DistantSilhouettes_V1 Blender source.

Fills a real, documented gap: docs/art/crash-site-object-asset-inventory.md's
"Distant silhouettes" entry calls for three variants -- basalt ridge, alien
arc, smoke plume -- "huge and unreachable," background-only, "enhances mood
but never redirects objective route." The current background in
scenes/Main/Main.tscn (DistantRock_4..7) is four plain scaled BoxMesh cubes,
never replaced with anything resembling those three variants.

Each variant is a single simple, chunky, low-poly silhouette meant to be seen
only from far away (per the inventory's own "simple dark layered forms with
atmospheric fade" shape language) -- not a hero asset, not something a player
can approach or interact with.

This is a Blender Asset Forge standalone candidate only: visual-only,
collisionless, and not integrated into any production scene by this script.

Run with Blender:
  blender --background --python tools/blender/create_distant_silhouettes_kit_v1.py
  blender --background --python tools/blender/create_distant_silhouettes_kit_v1.py -- --asset TC_ENV_DistantSilhouette_BasaltRidge_V1
"""
from __future__ import annotations

import math
import sys
from pathlib import Path

import bpy

ROOT = Path(__file__).resolve().parents[2]
SOURCE_DIR = ROOT / "assets/Source/Blender/Production/DistantSilhouettes_V1"

# Matches TC_MAT_restrained_distant_violet_basalt in
# tools/blender/create_stage_a_terrain_diorama_kit_v1.py, for background-piece
# palette consistency.
DISTANT_VIOLET_BASALT = (0.16, 0.12, 0.21)
DISTANT_HAZE = (0.20, 0.19, 0.24)


def reset_scene() -> None:
    bpy.ops.wm.read_factory_settings(use_empty=True)


def pbr(name: str, color: tuple[float, float, float], rough: float = 0.85, metal: float = 0.05) -> bpy.types.Material:
    existing = bpy.data.materials.get(name)
    if existing:
        return existing
    mat = bpy.data.materials.new(name)
    mat.use_nodes = True
    bsdf = mat.node_tree.nodes["Principled BSDF"]
    bsdf.inputs["Base Color"].default_value = (*color, 1.0)
    bsdf.inputs["Roughness"].default_value = rough
    bsdf.inputs["Metallic"].default_value = metal
    return mat


def _finalize(obj: bpy.types.Object, mat: bpy.types.Material) -> bpy.types.Object:
    obj.data.materials.append(mat)
    bpy.ops.object.select_all(action="DESELECT")
    obj.select_set(True)
    bpy.context.view_layer.objects.active = obj
    bpy.ops.object.transform_apply(location=True, rotation=True, scale=True)
    obj["titancraft_visual_only"] = True
    obj["titancraft_collision"] = "none"
    obj.select_set(False)
    return obj


def cone(name: str, mat: bpy.types.Material, loc: tuple[float, float, float], r_base: float, r_top: float,
         depth: float, rot: tuple[float, float, float] = (0, 0, 0), scale: tuple[float, float, float] = (1, 1, 1),
         vertices: int = 5) -> bpy.types.Object:
    bpy.ops.mesh.primitive_cone_add(vertices=vertices, radius1=r_base, radius2=r_top, depth=depth,
                                    location=loc, rotation=[math.radians(a) for a in rot])
    obj = bpy.context.object
    obj.name = name
    obj.scale = scale
    return _finalize(obj, mat)


def torus(name: str, mat: bpy.types.Material, loc: tuple[float, float, float], major_r: float, minor_r: float,
          rot: tuple[float, float, float] = (0, 0, 0), major_seg: int = 10, minor_seg: int = 5) -> bpy.types.Object:
    bpy.ops.mesh.primitive_torus_add(location=loc, rotation=[math.radians(a) for a in rot],
                                     major_radius=major_r, minor_radius=minor_r,
                                     major_segments=major_seg, minor_segments=minor_seg)
    obj = bpy.context.object
    obj.name = name
    return _finalize(obj, mat)


def build_basalt_ridge() -> None:
    """Jagged dark rock silhouette: a cluster of tall irregular basalt spires."""
    basalt = pbr("TC_DistantSilhouette_Basalt", DISTANT_VIOLET_BASALT, rough=0.9, metal=0.05)
    spires = (
        (0.0, 0.0, 1.1, 0.55, 3.2, 5, 0),
        (2.0, 0.3, 0.75, 0.4, 2.2, 5, 15),
        (-1.6, -0.2, 0.9, 0.45, 2.6, 6, -10),
        (0.9, 0.5, 0.55, 0.28, 1.6, 5, 25),
        (-0.7, 0.4, 0.65, 0.32, 1.9, 5, -20),
    )
    for i, (x, y, r_base, r_top, height, verts, tilt) in enumerate(spires):
        cone(f"TC_DistantSilhouette_BasaltRidge_Spire_{i}", basalt, (x, y, height / 2), r_base, r_top, height,
             rot=(tilt, tilt * 0.4, i * 37), vertices=verts)


def build_alien_arc() -> None:
    """Biomechanical arc: a partial dark ring suggesting distant alien structure, not a building."""
    plate = pbr("TC_DistantSilhouette_AlienPlate", (0.06, 0.05, 0.09), rough=0.6, metal=0.4)
    torus("TC_DistantSilhouette_AlienArc_Ring", plate, (0, 0, 1.6), 1.6, 0.16, rot=(6, 0, -12), major_seg=8, minor_seg=4)
    cone("TC_DistantSilhouette_AlienArc_SpineA", plate, (-1.2, 0.1, 0.55), 0.22, 0.05, 1.3, rot=(-8, 4, 20), vertices=4)
    cone("TC_DistantSilhouette_AlienArc_SpineB", plate, (1.3, -0.1, 0.5), 0.20, 0.04, 1.2, rot=(8, -4, -18), vertices=4)


def build_smoke_plume() -> None:
    """Simplified static silhouette of a rising smoke/haze column (no particle system)."""
    haze = pbr("TC_DistantSilhouette_Haze", DISTANT_HAZE, rough=1.0, metal=0.0)
    layers = (
        (0.0, 0.0, 0.55, 0.42, 0.36, 1.1),
        (0.15, 0.05, 1.35, 0.30, 0.42, 1.3),
        (-0.1, 0.08, 2.15, 0.18, 0.48, 1.5),
        (0.25, -0.05, 2.85, 0.05, 0.30, 1.1),
    )
    for i, (x, y, z, r_top, r_base, height) in enumerate(layers):
        cone(f"TC_DistantSilhouette_SmokePlume_Layer_{i}", haze, (x, y, z), r_base, r_top, height,
             rot=(0, 0, i * 33), vertices=7)


ASSETS = {
    "TC_ENV_DistantSilhouette_BasaltRidge_V1": build_basalt_ridge,
    "TC_ENV_DistantSilhouette_AlienArc_V1": build_alien_arc,
    "TC_ENV_DistantSilhouette_SmokePlume_V1": build_smoke_plume,
}


def main() -> None:
    only = None
    if "--" in sys.argv:
        tail = sys.argv[sys.argv.index("--") + 1:]
        if len(tail) == 2 and tail[0] == "--asset":
            only = tail[1]
        elif tail:
            raise SystemExit("usage: blender --background --python create_distant_silhouettes_kit_v1.py [-- --asset TC_NAME]")
    names = [only] if only else list(ASSETS)
    for name in names:
        if name not in ASSETS:
            raise SystemExit(f"unknown asset: {name}")
        reset_scene()
        ASSETS[name]()
        output = SOURCE_DIR / f"{name}.blend"
        output.parent.mkdir(parents=True, exist_ok=True)
        bpy.ops.wm.save_as_mainfile(filepath=str(output))
        print(f"DISTANT_SILHOUETTES_BLEND_WRITTEN {output}")


if __name__ == "__main__":
    main()
