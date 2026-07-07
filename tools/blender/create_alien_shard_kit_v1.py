#!/usr/bin/env python3
"""Create the TC_ENV_AlienShard_V1 Blender source.

Fills a real, documented gap: docs/art/crash-site-object-asset-inventory.md's
"Alien shard props" entry ("Jagged dark violet shards with irregular lean...
Carapace splinters, embedded fins, rib fragments... Required variants: Small,
medium, embedded cluster... Mood only; not mistaken for resources"). The
scene's five AlienCrystal_1..5 route/arena dressing props (scenes/Main/Main.tscn)
all reuse one plain PrismMesh (a clean 4-sided pyramid) at different uniform
scales -- a placeholder, not the jagged irregular-lean shard silhouette the
inventory calls for.

This kit builds the three required variants using jittered, tilted cone
"splinters" (reusing the same jitter technique as
create_rock_occluder_kit_v1.py) so each shard reads as a broken, angular
carapace fragment rather than a clean toy crystal. Small and medium are
single shards; embedded cluster is three shards of mixed height and lean
grouped together, per the inventory's own three-variant list -- not a
"crystal forest" (the inventory's own forbidden pattern), since this script
does not increase the number of AlienCrystal_N instances the scene places.

This is a Blender Asset Forge standalone candidate only: visual-only,
collisionless, and not integrated into any production scene by this script.

Run with Blender:
  blender --background --python tools/blender/create_alien_shard_kit_v1.py
  blender --background --python tools/blender/create_alien_shard_kit_v1.py -- --asset TC_ENV_AlienShard_Small_V1
"""
from __future__ import annotations

import math
import sys
from pathlib import Path

import bmesh
import bpy

ROOT = Path(__file__).resolve().parents[2]
SOURCE_DIR = ROOT / "assets/Source/Blender/Production/AlienShard_V1"

# Matches assets/Materials/AlienVioletEmissive.tres, the material the
# placeholder PrismMesh_crystal already uses, for tone/glow continuity.
ALIEN_VIOLET = (0.478431, 0.247059, 0.949020)


def reset_scene() -> None:
    bpy.ops.wm.read_factory_settings(use_empty=True)


def pbr_emissive(name: str, color: tuple[float, float, float]) -> bpy.types.Material:
    mat = bpy.data.materials.new(name)
    mat.use_nodes = True
    bsdf = mat.node_tree.nodes["Principled BSDF"]
    bsdf.inputs["Base Color"].default_value = (*color, 1.0)
    bsdf.inputs["Roughness"].default_value = 0.35
    bsdf.inputs["Metallic"].default_value = 0.1
    bsdf.inputs["Emission Color"].default_value = (*color, 1.0)
    bsdf.inputs["Emission Strength"].default_value = 1.2
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


def _jitter_vertices(mesh: bpy.types.Mesh, seed: int, amount: float) -> None:
    bm = bmesh.new()
    bm.from_mesh(mesh)
    bm.verts.ensure_lookup_table()
    for i, vert in enumerate(bm.verts):
        h = ((i * 374761393 + seed * 668265263) ^ ((i * 374761393 + seed * 668265263) >> 13)) & 0xFFFFFF
        offset = (h / 0xFFFFFF - 0.5) * 2.0 * amount
        vert.co += vert.normal * offset
    bm.to_mesh(mesh)
    bm.free()


def _shard(name: str, mat: bpy.types.Material, loc: tuple[float, float, float], radius: float, height: float,
           lean_deg: float, seed: int, vertices: int = 5) -> bpy.types.Object:
    bpy.ops.mesh.primitive_cone_add(vertices=vertices, radius1=radius, radius2=radius * 0.08, depth=height,
                                     location=(loc[0], loc[1], loc[2] + height / 2))
    obj = bpy.context.object
    obj.name = name
    obj.rotation_euler = (math.radians(lean_deg), math.radians(lean_deg * 0.4), math.radians(seed * 47))
    bpy.ops.object.transform_apply(location=False, rotation=True, scale=False)
    _jitter_vertices(obj.data, seed=seed, amount=radius * 0.35)
    obj.data.update()
    bpy.ops.object.shade_flat()
    return _finalize(obj, mat)


def build_small() -> None:
    mat = pbr_emissive("TC_MAT_AlienShard_Violet", ALIEN_VIOLET)
    _shard("TC_ENV_AlienShard_Small_V1", mat, (0, 0, 0), radius=0.22, height=1.0, lean_deg=8, seed=3)


def build_medium() -> None:
    mat = pbr_emissive("TC_MAT_AlienShard_Violet", ALIEN_VIOLET)
    _shard("TC_ENV_AlienShard_Medium_V1", mat, (0, 0, 0), radius=0.34, height=2.1, lean_deg=-10, seed=11)


def build_embedded_cluster() -> None:
    mat = pbr_emissive("TC_MAT_AlienShard_Violet", ALIEN_VIOLET)
    spec = (
        (0.0, 0.0, 0.30, 1.7, 6, 13),
        (0.28, 0.12, 0.20, 1.05, -14, 19),
        (-0.22, -0.15, 0.16, 0.75, 18, 27),
    )
    parts = []
    for i, (x, y, radius, height, lean, seed) in enumerate(spec):
        part = _shard(f"TC_ENV_AlienShard_EmbeddedCluster_V1_Part{i}", mat, (x, y, 0), radius, height, lean, seed)
        parts.append(part)
    bpy.ops.object.select_all(action="DESELECT")
    for part in parts:
        part.select_set(True)
    bpy.context.view_layer.objects.active = parts[0]
    bpy.ops.object.join()
    joined = bpy.context.object
    joined.name = "TC_ENV_AlienShard_EmbeddedCluster_V1"


ASSETS = {
    "TC_ENV_AlienShard_Small_V1": build_small,
    "TC_ENV_AlienShard_Medium_V1": build_medium,
    "TC_ENV_AlienShard_EmbeddedCluster_V1": build_embedded_cluster,
}


def main() -> None:
    only = None
    if "--" in sys.argv:
        tail = sys.argv[sys.argv.index("--") + 1:]
        if len(tail) == 2 and tail[0] == "--asset":
            only = tail[1]
        elif tail:
            raise SystemExit("usage: blender --background --python create_alien_shard_kit_v1.py [-- --asset TC_NAME]")
    names = [only] if only else list(ASSETS)
    for name in names:
        if name not in ASSETS:
            raise SystemExit(f"unknown asset: {name}")
        reset_scene()
        ASSETS[name]()
        output = SOURCE_DIR / f"{name}.blend"
        output.parent.mkdir(parents=True, exist_ok=True)
        bpy.ops.wm.save_as_mainfile(filepath=str(output))
        print(f"ALIEN_SHARD_BLEND_WRITTEN {output}")


if __name__ == "__main__":
    main()
