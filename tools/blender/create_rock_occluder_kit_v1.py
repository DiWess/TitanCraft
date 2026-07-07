#!/usr/bin/env python3
"""Create the TC_ENV_RockOccluder_V1 Blender source.

Fills a real, documented gap: docs/art/crash-site-object-asset-inventory.md's
"Foreground occluders" entry lists "Rock occluder" as a required variant
("Chunky partial forms near camera edges... Terrain and wreckage palettes...
Knee to above-player"). The scene's three foreground blocking-rock props
(scenes/Main/Main.tscn VolcanicRock_1/2/3) still use a plain scaled BoxMesh
(BoxMesh_rock) -- a placeholder box, not the chunky irregular boulder shape
the inventory calls for -- even though these are close-range, player-visible
obstacles along the main route.

This kit builds one chunky, irregular, flat-bottomed low-poly boulder using
the same dark basalt palette as assets/Materials/VolcanicRock.tres (the
material the placeholder boxes already use), so a straight mesh swap keeps
tone continuity. The boulder is visual-only; VolcanicRock_1/2/3 keep their
existing separate CollisionShape3D "Collision_BlockingRock" siblings
untouched -- this script does not touch collision.

This is a Blender Asset Forge standalone candidate only: visual-only,
collisionless, and not integrated into any production scene by this script.

Run with Blender:
  blender --background --python tools/blender/create_rock_occluder_kit_v1.py
"""
from __future__ import annotations

from pathlib import Path

import bmesh
import bpy

ROOT = Path(__file__).resolve().parents[2]
SOURCE_DIR = ROOT / "assets/Source/Blender/Production"
OUTPUT = SOURCE_DIR / "TC_ENV_RockOccluder_V1.blend"

# Matches assets/Materials/VolcanicRock.tres, the material the BoxMesh_rock
# placeholder already uses in scenes/Main/Main.tscn, for tone continuity.
VOLCANIC_ROCK = (0.082353, 0.090196, 0.101961)


def reset_scene() -> None:
    bpy.ops.wm.read_factory_settings(use_empty=True)


def pbr(name: str, color: tuple[float, float, float], rough: float = 0.9, metal: float = 0.05) -> bpy.types.Material:
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


def _jitter_vertices(mesh: bpy.types.Mesh, seed: int, amount: float) -> None:
    """Deterministic per-vertex displacement for a chunky, irregular silhouette."""
    bm = bmesh.new()
    bm.from_mesh(mesh)
    bm.verts.ensure_lookup_table()
    for i, vert in enumerate(bm.verts):
        h = ((i * 374761393 + seed * 668265263) ^ ((i * 374761393 + seed * 668265263) >> 13)) & 0xFFFFFF
        offset = (h / 0xFFFFFF - 0.5) * 2.0 * amount
        vert.co += vert.normal * offset
    bm.to_mesh(mesh)
    bm.free()


def _flatten_base(obj: bpy.types.Object, floor_z: float) -> None:
    """Push any vertex below floor_z back up to it, so the boulder sits flush on the ground."""
    for vert in obj.data.vertices:
        if vert.co.z < floor_z:
            vert.co.z = floor_z


def build_rock_occluder() -> None:
    basalt = pbr("TC_MAT_VolcanicRock_Boulder", VOLCANIC_ROCK, rough=0.9, metal=0.05)
    bpy.ops.mesh.primitive_ico_sphere_add(subdivisions=1, radius=1.0, location=(0, 0, 1.0))
    obj = bpy.context.object
    obj.name = "TC_ENV_RockOccluder_V1"
    obj.scale = (1.5, 1.15, 0.95)
    bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
    _jitter_vertices(obj.data, seed=7, amount=0.22)
    _flatten_base(obj, floor_z=0.05)
    obj.data.update()
    bpy.ops.object.shade_flat()
    _finalize(obj, basalt)


def main() -> None:
    reset_scene()
    build_rock_occluder()
    OUTPUT.parent.mkdir(parents=True, exist_ok=True)
    bpy.ops.wm.save_as_mainfile(filepath=str(OUTPUT))
    print(f"ROCK_OCCLUDER_BLEND_WRITTEN {OUTPUT}")


if __name__ == "__main__":
    main()
