#!/usr/bin/env python3
"""Create the TC_ENV Base Camp Dressing Kit V1 Blender sources.

Fills real, documented gaps:

- docs/art/crash-site-object-asset-inventory.md's "Foreground occluders" entry
  lists "cable occluder" as a required variant ("Cropped rocks, wreckage ribs,
  cable loops") that has never been built (rock and hull-rib variants exist).
- The same inventory's base-pad shape language calls for "Rectangular pads,
  crates, cables, tool frames" around the workbench hub, and the approved
  reference-mood direction (docs/release/evidence/
  titancraft-visual-reference-mood-pass-2026-07-09.md) shows the camp hub read
  as a lived-in base: orange awning, supply crates, and pole-mounted work
  lights. The production scene currently has four bare OmniLight3D sources
  with no visible fixture producing the light, and no camp-canopy or crate
  dressing at all.

Four visual-only, collisionless assets, matching existing repo palette tones:

- TC_ENV_CampAwning_V1   — sloped orange canopy on graphite poles
- TC_ENV_SupplyCrateStack_V1 — three stacked salvage crates, orange accents
- TC_ENV_LightPole_V1    — camp work-light pole with warm emissive head
- TC_ENV_CableOccluder_V1 — sagging twin cable run between two anchors

This is a Blender Asset Forge standalone candidate script only: it does not
integrate anything into production scenes.

Run with Blender:
  blender --background --python tools/blender/create_base_camp_dressing_kit_v1.py
  blender --background --python tools/blender/create_base_camp_dressing_kit_v1.py -- --asset TC_ENV_LightPole_V1
"""
from __future__ import annotations

import math
import sys
from pathlib import Path

import bpy

ROOT = Path(__file__).resolve().parents[2]
SOURCE_DIR = ROOT / "assets/Source/Blender/Production"

# Palette matched to existing repo materials for tone continuity:
GRAPHITE = (0.145098, 0.164706, 0.188235)   # assets/Materials/HumanGraphite.tres
WORN_STEEL = (0.431373, 0.454902, 0.470588) # assets/Materials/HumanWornSteel.tres
ORANGE = (0.909804, 0.470588, 0.133333)     # assets/Materials/HumanOrangeInteractive.tres
TARP_ORANGE = (0.72, 0.33, 0.10)            # matte tarp variant of the orange accent
WARM_LAMP = (1.0, 0.62, 0.22)               # matches BaseLamp light color family


def reset_scene() -> None:
    bpy.ops.wm.read_factory_settings(use_empty=True)
    # The review renderer (tools/blender/render_asset_review.py) sets
    # scene.world.color, so the saved source must carry a World block.
    world = bpy.data.worlds.new("World")
    bpy.context.scene.world = world


def pbr(name: str, color: tuple[float, float, float], rough: float = 0.85,
        metal: float = 0.1, emission: tuple[float, float, float] | None = None,
        emission_strength: float = 0.0) -> bpy.types.Material:
    mat = bpy.data.materials.new(name)
    mat.use_nodes = True
    bsdf = mat.node_tree.nodes["Principled BSDF"]
    bsdf.inputs["Base Color"].default_value = (*color, 1.0)
    bsdf.inputs["Roughness"].default_value = rough
    bsdf.inputs["Metallic"].default_value = metal
    if emission is not None:
        bsdf.inputs["Emission Color"].default_value = (*emission, 1.0)
        bsdf.inputs["Emission Strength"].default_value = emission_strength
    return mat


def _tag(obj: bpy.types.Object) -> None:
    obj["titancraft_visual_only"] = True
    obj["titancraft_collision"] = "none"


def _box(name: str, size: tuple[float, float, float], loc: tuple[float, float, float],
         mat: bpy.types.Material, rot: tuple[float, float, float] = (0.0, 0.0, 0.0)) -> bpy.types.Object:
    bpy.ops.mesh.primitive_cube_add(size=1.0, location=loc, rotation=rot)
    obj = bpy.context.object
    obj.name = name
    # The base cube spans exactly 1.0 per axis, so the scale factor IS the size.
    obj.scale = (size[0], size[1], size[2])
    obj.data.materials.append(mat)
    return obj


def _cyl(name: str, radius: float, depth: float, loc: tuple[float, float, float],
         mat: bpy.types.Material, rot: tuple[float, float, float] = (0.0, 0.0, 0.0),
         verts: int = 8) -> bpy.types.Object:
    bpy.ops.mesh.primitive_cylinder_add(vertices=verts, radius=radius, depth=depth,
                                        location=loc, rotation=rot)
    obj = bpy.context.object
    obj.name = name
    obj.data.materials.append(mat)
    return obj


def _join(name: str, parts: list[bpy.types.Object]) -> bpy.types.Object:
    bpy.ops.object.select_all(action="DESELECT")
    for part in parts:
        part.select_set(True)
    bpy.context.view_layer.objects.active = parts[0]
    bpy.ops.object.join()
    joined = bpy.context.object
    joined.name = name
    bpy.ops.object.transform_apply(location=True, rotation=True, scale=True)
    _tag(joined)
    return joined


def build_camp_awning() -> None:
    pole_mat = pbr("TC_MAT_AwningPole", GRAPHITE, rough=0.7, metal=0.4)
    tarp_mat = pbr("TC_MAT_AwningTarp", TARP_ORANGE, rough=0.95, metal=0.0)
    parts: list[bpy.types.Object] = []
    # Four poles; front pair taller than rear pair for a readable slope.
    for (x, y, h) in [(-1.5, -1.0, 2.5), (1.5, -1.0, 2.5), (-1.5, 1.0, 2.1), (1.5, 1.0, 2.1)]:
        parts.append(_cyl(f"pole_{x}_{y}", 0.055, h, (x, y, h / 2.0), pole_mat, verts=6))
    # Sloped canopy resting on the pole tops.
    slope = math.atan2(2.5 - 2.1, 2.0)
    parts.append(_box("canopy", (3.6, 2.5, 0.06), (0.0, 0.0, 2.32), tarp_mat, rot=(slope, 0.0, 0.0)))
    # Front valance strip for silhouette interest.
    parts.append(_box("valance", (3.6, 0.05, 0.28), (0.0, -1.22, 2.42), tarp_mat))
    # Rear cross brace.
    parts.append(_cyl("brace", 0.04, 3.0, (0.0, 1.0, 1.9), pole_mat, rot=(0.0, math.pi / 2.0, 0.0), verts=6))
    _join("TC_ENV_CampAwning_V1", parts)


def build_supply_crate_stack() -> None:
    crate_mat = pbr("TC_MAT_CrateSteel", WORN_STEEL, rough=0.8, metal=0.3)
    dark_mat = pbr("TC_MAT_CrateGraphite", GRAPHITE, rough=0.85, metal=0.2)
    accent_mat = pbr("TC_MAT_CrateAccent", ORANGE, rough=0.6, metal=0.1)
    parts: list[bpy.types.Object] = []
    # Two base crates, slightly offset; one dark, one steel.
    parts.append(_box("crate_a", (0.95, 0.95, 0.85), (-0.55, 0.0, 0.425), crate_mat))
    parts.append(_box("crate_a_lid", (1.0, 1.0, 0.1), (-0.55, 0.0, 0.9), dark_mat))
    parts.append(_box("crate_b", (0.85, 0.85, 0.7), (0.55, 0.12, 0.35), dark_mat))
    parts.append(_box("crate_b_lid", (0.9, 0.9, 0.08), (0.55, 0.12, 0.74), crate_mat))
    # Top crate rotated for a broken-grid salvage look.
    parts.append(_box("crate_c", (0.8, 0.8, 0.65), (-0.4, 0.05, 1.28), crate_mat, rot=(0.0, 0.0, 0.32)))
    parts.append(_box("crate_c_lid", (0.85, 0.85, 0.08), (-0.4, 0.05, 1.64), dark_mat, rot=(0.0, 0.0, 0.32)))
    # Orange hazard accent strips on two faces.
    parts.append(_box("accent_a", (0.97, 0.08, 0.2), (-0.55, -0.44, 0.5), accent_mat))
    parts.append(_box("accent_c", (0.1, 0.82, 0.18), (-0.75, 0.05, 1.3), accent_mat, rot=(0.0, 0.0, 0.32)))
    _join("TC_ENV_SupplyCrateStack_V1", parts)


def build_light_pole() -> None:
    pole_mat = pbr("TC_MAT_LightPole", GRAPHITE, rough=0.7, metal=0.4)
    steel_mat = pbr("TC_MAT_LightPoleSteel", WORN_STEEL, rough=0.75, metal=0.3)
    lamp_mat = pbr("TC_MAT_LampFace", WARM_LAMP, rough=0.3, metal=0.0,
                   emission=WARM_LAMP, emission_strength=3.0)
    parts: list[bpy.types.Object] = []
    parts.append(_cyl("base", 0.28, 0.14, (0.0, 0.0, 0.07), steel_mat, verts=8))
    parts.append(_cyl("mast", 0.06, 3.0, (0.0, 0.0, 1.57), pole_mat, verts=6))
    # Angled head arm reaching toward the lit area.
    parts.append(_cyl("arm", 0.045, 0.7, (0.0, 0.28, 3.12), pole_mat,
                      rot=(math.radians(65.0), 0.0, 0.0), verts=6))
    # Lamp housing with an emissive face pointed down/out.
    parts.append(_box("housing", (0.34, 0.4, 0.16), (0.0, 0.56, 3.24), steel_mat,
                      rot=(math.radians(-25.0), 0.0, 0.0)))
    parts.append(_box("lamp_face", (0.28, 0.34, 0.04), (0.0, 0.6, 3.16), lamp_mat,
                      rot=(math.radians(-25.0), 0.0, 0.0)))
    _join("TC_ENV_LightPole_V1", parts)


def build_cable_occluder() -> None:
    anchor_mat = pbr("TC_MAT_CableAnchor", GRAPHITE, rough=0.85, metal=0.2)
    cable_mat = pbr("TC_MAT_Cable", (0.06, 0.065, 0.075), rough=0.9, metal=0.1)
    parts: list[bpy.types.Object] = []
    span = 3.6
    parts.append(_box("anchor_a", (0.3, 0.3, 0.5), (-span / 2.0, 0.0, 0.25), anchor_mat))
    parts.append(_box("anchor_b", (0.3, 0.3, 0.55), (span / 2.0, 0.0, 0.275), anchor_mat))
    # Two sagging cables approximated with short segments along a parabola.
    for cable_i, (y_off, top_z, sag) in enumerate([(0.05, 0.42, 0.22), (-0.05, 0.5, 0.3)]):
        segments = 7
        prev = None
        for i in range(segments + 1):
            t = i / segments
            x = -span / 2.0 + t * span
            z = top_z - sag * (1.0 - (2.0 * t - 1.0) ** 2)
            point = (x, y_off, z)
            if prev is not None:
                mx = (prev[0] + point[0]) / 2.0
                mz = (prev[2] + point[2]) / 2.0
                seg_len = math.dist(prev, point)
                angle = math.atan2(point[2] - prev[2], point[0] - prev[0])
                parts.append(_cyl(f"cable{cable_i}_seg{i}", 0.028, seg_len * 1.08,
                                  (mx, y_off, mz), cable_mat,
                                  rot=(0.0, math.pi / 2.0 - angle, 0.0), verts=5))
            prev = point
    _join("TC_ENV_CableOccluder_V1", parts)


ASSETS = {
    "TC_ENV_CampAwning_V1": build_camp_awning,
    "TC_ENV_SupplyCrateStack_V1": build_supply_crate_stack,
    "TC_ENV_LightPole_V1": build_light_pole,
    "TC_ENV_CableOccluder_V1": build_cable_occluder,
}


def main() -> None:
    only = None
    if "--" in sys.argv:
        tail = sys.argv[sys.argv.index("--") + 1:]
        if len(tail) == 2 and tail[0] == "--asset":
            only = tail[1]
        elif tail:
            raise SystemExit("usage: blender --background --python create_base_camp_dressing_kit_v1.py [-- --asset TC_NAME]")
    names = [only] if only else list(ASSETS)
    for name in names:
        if name not in ASSETS:
            raise SystemExit(f"unknown asset: {name}")
        reset_scene()
        ASSETS[name]()
        output = SOURCE_DIR / f"{name}.blend"
        output.parent.mkdir(parents=True, exist_ok=True)
        bpy.ops.wm.save_as_mainfile(filepath=str(output))
        print(f"BASE_CAMP_DRESSING_BLEND_WRITTEN {output}")


if __name__ == "__main__":
    main()
