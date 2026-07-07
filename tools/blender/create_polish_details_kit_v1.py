#!/usr/bin/env python3
"""Create the TC_PolishDetails_V1 Blender source (Stage B candidate #10).

Standalone, non-scene reference kit demonstrating six approved surface-wear
techniques on small sample panels, per docs/art/ASSET_MANIFEST_V1.md's
"Polish Details (Surface Wear, Texturing Samples)" entry: weld scars,
corrosion streaks, paint chipping, mechanical seams, dent deformation, and
salvaged patches. Wear is represented with authored geometry and material
color, not texture maps, matching the project's simplified-PBR direction
(docs/art/titancraft-visual-identity.md) rather than photoreal decals.

Companion to tools/blender/create_lighting_reference_kit_v1.py (Stage B
candidate #9): together they close out the 10-candidate Stream 1 list in
docs/art/STAGE_B_ORCHESTRATION_BRIEF.md.

This is a Blender Asset Forge standalone candidate only: visual-only,
collisionless, and not integrated into any production scene.

Run with Blender:
  blender --background --python tools/blender/create_polish_details_kit_v1.py
"""
from __future__ import annotations

import math
from pathlib import Path

import bpy

ROOT = Path(__file__).resolve().parents[2]
SOURCE = ROOT / "assets/Source/Blender/Production/TC_PolishDetails_V1.blend"

OFF_WHITE = (0.478, 0.435, 0.356)
DARK_METAL = (0.045, 0.050, 0.058)
MID_METAL = (0.180, 0.190, 0.205)
STEEL = (0.400, 0.410, 0.430)
RUST_BROWN = (0.220, 0.110, 0.045)
LABEL_OFFWHITE = (0.478, 0.435, 0.356)

PANEL_SPACING = 0.26


def reset_scene() -> None:
    bpy.ops.wm.read_factory_settings(use_empty=True)


def pbr(name: str, color: tuple[float, float, float], rough: float = 0.6, metal: float = 0.0) -> bpy.types.Material:
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


def box(name: str, mat: bpy.types.Material, loc: tuple[float, float, float], dim: tuple[float, float, float],
        rot: tuple[float, float, float] = (0, 0, 0)) -> bpy.types.Object:
    bpy.ops.mesh.primitive_cube_add(size=1, location=loc, rotation=[math.radians(a) for a in rot])
    obj = bpy.context.object
    obj.name = name
    obj.scale = dim
    return _finalize(obj, mat)


def cyl(name: str, mat: bpy.types.Material, loc: tuple[float, float, float], radius: float, depth: float,
        rot: tuple[float, float, float] = (0, 0, 0), vertices: int = 8) -> bpy.types.Object:
    bpy.ops.mesh.primitive_cylinder_add(vertices=vertices, radius=radius, depth=depth,
                                        location=loc, rotation=[math.radians(a) for a in rot])
    obj = bpy.context.object
    obj.name = name
    return _finalize(obj, mat)


def text_label(name: str, body: str, mat: bpy.types.Material, loc: tuple[float, float, float]) -> bpy.types.Object:
    bpy.ops.object.text_add(location=loc, rotation=(math.radians(90), 0, 0))
    obj = bpy.context.object
    obj.name = name
    obj.data.body = body
    obj.data.size = 0.06
    obj.data.extrude = 0.003
    obj.data.resolution_u = 2
    obj.data.align_x = "CENTER"
    obj.data.align_y = "CENTER"
    bpy.ops.object.convert(target="MESH")
    return _finalize(bpy.context.object, mat)


def _panel(role: str, x: float, base_mat: bpy.types.Material) -> None:
    box(f"TC_PolishRef_{role}_Panel", base_mat, (x, 0, 0.15), (0.22, 0.02, 0.16))


def build_weld_scar(x: float) -> None:
    """Raised weld bead + spot-weld bumps, clearly proud of the plate surface."""
    steel = pbr("TC_PolishRef_Steel", STEEL, rough=0.42, metal=0.9)
    dark = pbr("TC_PolishRef_Dark", DARK_METAL, rough=0.55, metal=0.5)
    _panel("WeldScar", x, steel)
    box(f"TC_PolishRef_WeldScar_Bead", dark, (x, -0.03, 0.15), (0.20, 0.04, 0.03), rot=(0, 0, 8))
    for i in range(5):
        cyl(f"TC_PolishRef_WeldScar_Bump_{i}", dark, (x - 0.08 + i * 0.04, -0.04, 0.15), 0.022, 0.05, rot=(90, 0, 0))


def build_corrosion(x: float) -> None:
    """Rust streak + corrosion pits, proud enough to read as buildup, not a decal."""
    steel = pbr("TC_PolishRef_Steel", STEEL, rough=0.42, metal=0.9)
    rust = pbr("TC_PolishRef_Rust", RUST_BROWN, rough=0.9, metal=0.05)
    _panel("Corrosion", x, steel)
    box(f"TC_PolishRef_Corrosion_Streak", rust, (x, -0.028, 0.15), (0.09, 0.03, 0.15), rot=(0, 0, 18))
    for i, dz in enumerate((-0.045, 0.0, 0.045)):
        cyl(f"TC_PolishRef_Corrosion_Pit_{i}", rust, (x + 0.01 * i, -0.032, 0.15 + dz), 0.028, 0.035, rot=(90, 0, 0))


def build_paint_chip(x: float) -> None:
    """Base coat with graphite substrate showing through at chipped patches."""
    paint = pbr("TC_PolishRef_Paint", OFF_WHITE, rough=0.62, metal=0.12)
    dark = pbr("TC_PolishRef_Dark", DARK_METAL, rough=0.55, metal=0.5)
    _panel("PaintChip", x, paint)
    for i, (dx, dz, s) in enumerate(((-0.06, 0.05, 0.05), (0.05, -0.03, 0.035), (0.02, 0.06, 0.03), (-0.04, -0.06, 0.038))):
        box(f"TC_PolishRef_PaintChip_Chip_{i}", dark, (x + dx, -0.03, 0.15 + dz), (s, 0.03, s))


def build_mechanical_seam(x: float) -> None:
    """Bolted service seam: raised strip with a row of bolt heads."""
    mid = pbr("TC_PolishRef_Mid", MID_METAL, rough=0.45, metal=0.75)
    dark = pbr("TC_PolishRef_Dark", DARK_METAL, rough=0.55, metal=0.5)
    _panel("MechSeam", x, mid)
    box(f"TC_PolishRef_MechSeam_Strip", dark, (x, -0.03, 0.15), (0.20, 0.04, 0.06))
    for i in range(4):
        cyl(f"TC_PolishRef_MechSeam_Bolt_{i}", dark, (x - 0.075 + i * 0.05, -0.045, 0.15), 0.022, 0.05, rot=(90, 0, 0))


def build_dent(x: float) -> None:
    """Crumpled/dented deformation.

    Primitive geometry has no boolean-recess step in this toolchain, so the
    dent is authored as a proud, dark, asymmetric bulge (crumpled metal folds
    outward as often as it recesses) rather than a true inward recess. It
    must sit on the panel's camera-facing side -- negative Y, since
    render_mvp_asset_review.py frames this kit from -Y looking toward +Y --
    to be visible at all; an earlier version placed it at positive Y, fully
    occluded behind the panel from every rendered angle.
    """
    steel = pbr("TC_PolishRef_Steel", STEEL, rough=0.42, metal=0.9)
    dark = pbr("TC_PolishRef_Dark", DARK_METAL, rough=0.75, metal=0.25)
    _panel("Dent", x, steel)
    box(f"TC_PolishRef_Dent_Bulge", dark, (x, -0.025, 0.15), (0.16, 0.03, 0.13), rot=(6, 4, 5))
    box(f"TC_PolishRef_Dent_Fold", dark, (x + 0.03, -0.035, 0.16), (0.06, 0.02, 0.05), rot=(-8, 10, -12))


def build_salvage_patch(x: float) -> None:
    """Mismatched salvage patch riveted over a hole, proud of the base plate."""
    steel = pbr("TC_PolishRef_Steel", STEEL, rough=0.42, metal=0.9)
    mid = pbr("TC_PolishRef_Mid", MID_METAL, rough=0.45, metal=0.75)
    dark = pbr("TC_PolishRef_Dark", DARK_METAL, rough=0.55, metal=0.5)
    _panel("SalvagePatch", x, steel)
    box(f"TC_PolishRef_SalvagePatch_Patch", mid, (x, -0.032, 0.15), (0.18, 0.035, 0.14), rot=(0, 0, -6))
    for dx, dz in ((-0.07, 0.05), (0.07, 0.05), (-0.07, -0.05), (0.07, -0.05)):
        cyl(f"TC_PolishRef_SalvagePatch_Bolt_{dx}_{dz}", dark, (x + dx, -0.05, 0.15 + dz), 0.020, 0.035, rot=(90, 0, 0))


STATIONS = (
    ("WLD", build_weld_scar),
    ("COR", build_corrosion),
    ("CHP", build_paint_chip),
    ("SEA", build_mechanical_seam),
    ("DNT", build_dent),
    ("PAT", build_salvage_patch),
)


def main() -> None:
    reset_scene()
    label_mat = pbr("TC_PolishRef_LabelText", LABEL_OFFWHITE, rough=0.6)
    for i, (label, build) in enumerate(STATIONS):
        x = (i - (len(STATIONS) - 1) / 2) * PANEL_SPACING
        build(x)
        text_label(f"TC_PolishRef_{label}_Label", label, label_mat, (x, 0.05, 0.001))
    SOURCE.parent.mkdir(parents=True, exist_ok=True)
    bpy.ops.wm.save_as_mainfile(filepath=str(SOURCE))
    print(f"TC_POLISH_DETAILS_V1_BLEND_WRITTEN {SOURCE}")


if __name__ == "__main__":
    main()
