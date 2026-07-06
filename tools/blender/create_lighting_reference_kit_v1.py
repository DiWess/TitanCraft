#!/usr/bin/env python3
"""Create the TC_LightingReference_V1 Blender source (Stage B candidate #9).

Standalone, non-scene reference kit demonstrating the four approved functional
glow colors and intensities already used across production candidates in
tools/blender/create_mvp_asset_pack_v1.py. It exists so future asset work reuses
these exact values instead of inventing new glow colors/strengths, per the
Stage A rule that emissive accents are "rare functional signals, not full-scene
decoration" (docs/art/titancraft-visual-identity.md).

Each of the four stations pairs one neutral, non-emissive plinth with one
emissive sample block and a flat text label naming its functional role:

  1. INTERACTION (orange, strength 1.8)  -- matches TC_Workbench_HoloScreen
  2. DANGER      (red,    strength 4.0)  -- matches TC_MVP_RedLED
  3. POWERED     (cyan,   strength 5.0)  -- matches TC_SavePoint save-point ring
  4. ALIEN       (purple, strength 7.0)  -- matches TC_Beacon_Core active state

This is a Blender Asset Forge standalone candidate only: visual-only,
collisionless, and not integrated into any production scene (Stage C is
gated separately; see docs/production/current-status.md).

Run with Blender:
  blender --background --python tools/blender/create_lighting_reference_kit_v1.py
"""
from __future__ import annotations

import math
from pathlib import Path

import bpy

ROOT = Path(__file__).resolve().parents[2]
SOURCE = ROOT / "assets/Source/Blender/Production/TC_LightingReference_V1.blend"

DARK_METAL = (0.045, 0.050, 0.058)
LABEL_OFFWHITE = (0.478, 0.435, 0.356)
ORANGE_GLOW = (1.000, 0.380, 0.060)
RED_GLOW = (1.000, 0.060, 0.040)
CYAN_GLOW = (0.100, 0.850, 0.900)
PURPLE_GLOW = (0.480, 0.180, 1.000)

STATIONS = (
    ("Interaction", ORANGE_GLOW, 1.8, "INT"),
    ("Danger", RED_GLOW, 4.0, "DNG"),
    ("Powered", CYAN_GLOW, 5.0, "PWR"),
    ("Alien", PURPLE_GLOW, 7.0, "ALN"),
)

STATION_SPACING = 1.2


def reset_scene() -> None:
    bpy.ops.wm.read_factory_settings(use_empty=True)


def pbr(name: str, color: tuple[float, float, float], rough: float = 0.6, metal: float = 0.0,
        emission: tuple[float, float, float] | None = None, strength: float = 0.0) -> bpy.types.Material:
    existing = bpy.data.materials.get(name)
    if existing:
        return existing
    mat = bpy.data.materials.new(name)
    mat.use_nodes = True
    bsdf = mat.node_tree.nodes["Principled BSDF"]
    bsdf.inputs["Base Color"].default_value = (*color, 1.0)
    bsdf.inputs["Roughness"].default_value = rough
    bsdf.inputs["Metallic"].default_value = metal
    if emission is not None:
        bsdf.inputs["Emission Color"].default_value = (*emission, 1.0)
        bsdf.inputs["Emission Strength"].default_value = strength
    return mat


def _finalize(obj: bpy.types.Object, mat: bpy.types.Material, bevel: float) -> bpy.types.Object:
    obj.data.materials.append(mat)
    bpy.ops.object.select_all(action="DESELECT")
    obj.select_set(True)
    bpy.context.view_layer.objects.active = obj
    bpy.ops.object.transform_apply(location=True, rotation=True, scale=True)
    if bevel > 0:
        mod = obj.modifiers.new("Bevel", "BEVEL")
        mod.width = bevel
        mod.segments = 2
        mod.limit_method = "ANGLE"
        mod.angle_limit = math.radians(40)
        obj.modifiers.new("WeightedNormal", "WEIGHTED_NORMAL")
    obj["titancraft_visual_only"] = True
    obj["titancraft_collision"] = "none"
    obj.select_set(False)
    return obj


def box(name: str, mat: bpy.types.Material, loc: tuple[float, float, float], dim: tuple[float, float, float],
        bevel: float = 0.015) -> bpy.types.Object:
    bpy.ops.mesh.primitive_cube_add(size=1, location=loc)
    obj = bpy.context.object
    obj.name = name
    obj.scale = dim
    return _finalize(obj, mat, bevel)


def text_label(name: str, body: str, mat: bpy.types.Material, loc: tuple[float, float, float]) -> bpy.types.Object:
    bpy.ops.object.text_add(location=loc, rotation=(math.radians(90), 0, 0))
    obj = bpy.context.object
    obj.name = name
    obj.data.body = body
    obj.data.size = 0.10
    obj.data.extrude = 0.004
    obj.data.resolution_u = 3
    obj.data.align_x = "CENTER"
    obj.data.align_y = "CENTER"
    bpy.ops.object.convert(target="MESH")
    return _finalize(bpy.context.object, mat, bevel=0)


def build_station(index: int, role: str, color: tuple[float, float, float], strength: float, label: str) -> None:
    x = (index - (len(STATIONS) - 1) / 2) * STATION_SPACING
    plinth = pbr("TC_LightingRef_Plinth", DARK_METAL, rough=0.55, metal=0.4)
    label_mat = pbr("TC_LightingRef_LabelText", LABEL_OFFWHITE, rough=0.6)
    sample_mat = pbr(f"TC_LightingRef_{role}Sample", (0.02, 0.02, 0.02), rough=0.3,
                      emission=color, strength=strength)

    box(f"TC_LightingRef_{role}_Plinth", plinth, (x, 0, 0.05), (0.40, 0.40, 0.10), bevel=0.015)
    box(f"TC_LightingRef_{role}_Sample", sample_mat, (x, 0, 0.175), (0.16, 0.16, 0.15), bevel=0.006)
    text_label(f"TC_LightingRef_{role}_Label", label, label_mat, (x, 0.24, 0.001))


def main() -> None:
    reset_scene()
    for i, (role, color, strength, label) in enumerate(STATIONS):
        build_station(i, role, color, strength, label)
    SOURCE.parent.mkdir(parents=True, exist_ok=True)
    bpy.ops.wm.save_as_mainfile(filepath=str(SOURCE))
    print(f"TC_LIGHTING_REFERENCE_V1_BLEND_WRITTEN {SOURCE}")


if __name__ == "__main__":
    main()
