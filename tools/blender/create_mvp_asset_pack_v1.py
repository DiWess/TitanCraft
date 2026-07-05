#!/usr/bin/env python3
"""Create the TitanCraft MVP Asset Pack V1 Blender sources.

Builds the seven Crash Site MVP concept assets as deterministic, visual-only
Blender 4.0.2 sources matching the approved MVP concept sheet:

  1. TC_PROP_Workbench_V1      salvage bench, robot arm, orange holo panel
  2. TC_PROP_Beacon_Dormant_V1 closed four-petal beacon obelisk
  3. TC_PROP_Beacon_Active_V1  opened petals, purple crystal core
  4. TC_PICKUP_Metal_V1        grey hull-shard pile
  5. TC_PICKUP_Biomass_V1      red crystalline organic cluster
  6. TC_PICKUP_Electronics_V1  dark module crates with orange lids
  7. TC_PICKUP_Component_V1    purple alien crystal cluster

plus the remaining Crash Site MVP roster:

  8. TC_CHAR_GalaxabrainScout_V1  spindly quadruped biomech, purple threat core
  9. TC_PLAYER_MechanicalArm_V1   first-person segmented mechanical arm reward
 10. TC_PROP_SavePoint_V1         cyan checkpoint pillar
 11. TC_ENV_CrashDebris_A_V1      off-white bent hull shard landmark

Run with Blender:
  blender --background --python tools/blender/create_mvp_asset_pack_v1.py
  blender --background --python tools/blender/create_mvp_asset_pack_v1.py -- --asset TC_PROP_Workbench_V1
"""
from __future__ import annotations

import math
import random
import sys
from pathlib import Path

import bpy
from mathutils import Vector

ROOT = Path(__file__).resolve().parents[2]
SOURCE_DIR = ROOT / "assets/Source/Blender/Production/MVP_Pack_V1"

# Concept-sheet palette (linear RGB).
BEIGE_HULL = (0.478, 0.435, 0.356)
DARK_METAL = (0.045, 0.050, 0.058)
MID_METAL = (0.180, 0.190, 0.205)
STEEL_SCRAP = (0.400, 0.410, 0.430)
ORANGE_PAINT = (0.720, 0.230, 0.035)
ORANGE_GLOW = (1.000, 0.380, 0.060)
RED_GLOW = (1.000, 0.060, 0.040)
CYAN_GLOW = (0.100, 0.850, 0.900)
PURPLE_GLOW = (0.480, 0.180, 1.000)
PURPLE_CRYSTAL = (0.220, 0.080, 0.420)
BIOMASS_RED = (0.150, 0.010, 0.016)
RUBBER_DARK = (0.028, 0.028, 0.032)


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


def _finalize(obj: bpy.types.Object, mat: bpy.types.Material, bevel: float, segments: int = 1) -> bpy.types.Object:
    obj.data.materials.append(mat)
    bpy.ops.object.select_all(action="DESELECT")
    obj.select_set(True)
    bpy.context.view_layer.objects.active = obj
    bpy.ops.object.transform_apply(location=True, rotation=True, scale=True)
    if bevel > 0:
        mod = obj.modifiers.new("Bevel", "BEVEL")
        mod.width = bevel
        mod.segments = segments
        mod.limit_method = "ANGLE"
        mod.angle_limit = math.radians(40)
        obj.modifiers.new("WeightedNormal", "WEIGHTED_NORMAL")
    obj["titancraft_visual_only"] = True
    obj["titancraft_collision"] = "none"
    obj.select_set(False)
    return obj


def box(name: str, mat: bpy.types.Material, loc: tuple[float, float, float], dim: tuple[float, float, float],
        rot: tuple[float, float, float] = (0, 0, 0), bevel: float = 0.015) -> bpy.types.Object:
    bpy.ops.mesh.primitive_cube_add(size=1, location=loc, rotation=[math.radians(a) for a in rot])
    obj = bpy.context.object
    obj.name = name
    obj.scale = dim
    return _finalize(obj, mat, bevel)


def cyl(name: str, mat: bpy.types.Material, loc: tuple[float, float, float], radius: float, depth: float,
        rot: tuple[float, float, float] = (0, 0, 0), vertices: int = 12, bevel: float = 0.01) -> bpy.types.Object:
    bpy.ops.mesh.primitive_cylinder_add(vertices=vertices, radius=radius, depth=depth,
                                        location=loc, rotation=[math.radians(a) for a in rot])
    obj = bpy.context.object
    obj.name = name
    return _finalize(obj, mat, bevel)


def taper(name: str, mat: bpy.types.Material, loc: tuple[float, float, float], r_base: float, r_top: float,
          depth: float, rot: tuple[float, float, float] = (0, 0, 0), scale: tuple[float, float, float] = (1, 1, 1),
          vertices: int = 4, bevel: float = 0.012) -> bpy.types.Object:
    bpy.ops.mesh.primitive_cone_add(vertices=vertices, radius1=r_base, radius2=r_top, depth=depth,
                                    location=loc, rotation=[math.radians(a) for a in rot])
    obj = bpy.context.object
    obj.name = name
    obj.scale = scale
    return _finalize(obj, mat, bevel)


def gem(name: str, mat: bpy.types.Material, loc: tuple[float, float, float], radius: float,
        scale: tuple[float, float, float], rot: tuple[float, float, float] = (0, 0, 0),
        jitter: float = 0.0, rng: random.Random | None = None) -> bpy.types.Object:
    bpy.ops.mesh.primitive_ico_sphere_add(subdivisions=1, radius=radius, location=loc,
                                          rotation=[math.radians(a) for a in rot])
    obj = bpy.context.object
    obj.name = name
    obj.scale = scale
    if jitter > 0 and rng is not None:
        for vertex in obj.data.vertices:
            vertex.co += vertex.normal * rng.uniform(-jitter, jitter)
    return _finalize(obj, mat, bevel=0)


def text_marking(name: str, body: str, mat: bpy.types.Material, loc: tuple[float, float, float],
                 size: float, rot: tuple[float, float, float]) -> bpy.types.Object:
    bpy.ops.object.text_add(location=loc, rotation=[math.radians(a) for a in rot])
    obj = bpy.context.object
    obj.name = name
    obj.data.body = body
    obj.data.size = size
    obj.data.extrude = 0.004
    obj.data.resolution_u = 3
    obj.data.align_x = "CENTER"
    obj.data.align_y = "CENTER"
    bpy.ops.object.convert(target="MESH")
    return _finalize(bpy.context.object, mat, bevel=0)


def cable(name: str, mat: bpy.types.Material, points: list[tuple[float, float, float]], radius: float) -> bpy.types.Object:
    curve = bpy.data.curves.new(name, "CURVE")
    curve.dimensions = "3D"
    curve.resolution_u = 5
    curve.bevel_depth = radius
    curve.bevel_resolution = 1
    curve.use_fill_caps = True
    spline = curve.splines.new("NURBS")
    spline.points.add(len(points) - 1)
    for point, co in zip(spline.points, points):
        point.co = (*co, 1.0)
    spline.use_endpoint_u = True
    obj = bpy.data.objects.new(name, curve)
    bpy.context.collection.objects.link(obj)
    bpy.ops.object.select_all(action="DESELECT")
    obj.select_set(True)
    bpy.context.view_layer.objects.active = obj
    bpy.ops.object.convert(target="MESH")
    return _finalize(bpy.context.object, mat, bevel=0)


def build_workbench() -> None:
    """C7 salvage workbench with robot arm, orange holo panel and floor power block."""
    beige = pbr("TC_MVP_HullBeige", BEIGE_HULL, rough=0.62, metal=0.12)
    dark = pbr("TC_MVP_DarkMetal", DARK_METAL, rough=0.48, metal=0.65)
    mid = pbr("TC_MVP_MidMetal", MID_METAL, rough=0.45, metal=0.75)
    orange = pbr("TC_MVP_OrangePaint", ORANGE_PAINT, rough=0.42)
    holo = pbr("TC_MVP_OrangeHolo", (0.02, 0.01, 0.0), rough=0.3, emission=ORANGE_GLOW, strength=5.0)
    red_led = pbr("TC_MVP_RedLED", (0.02, 0.0, 0.0), rough=0.3, emission=RED_GLOW, strength=4.0)
    rubber = pbr("TC_MVP_Rubber", RUBBER_DARK, rough=0.85)

    # Chassis
    box("TC_Workbench_Skid", dark, (0, 0, 0.05), (2.25, 0.95, 0.10))
    box("TC_Workbench_LegL", dark, (-0.78, 0, 0.42), (0.52, 0.82, 0.66))
    box("TC_Workbench_LegR", dark, (0.78, 0, 0.42), (0.52, 0.82, 0.66))
    box("TC_Workbench_Top", beige, (0, 0, 0.84), (2.35, 1.00, 0.12), bevel=0.03)
    box("TC_Workbench_WorkSurface", mid, (0.05, 0, 0.905), (1.70, 0.80, 0.02), bevel=0.006)
    box("TC_Workbench_FrontPanel", beige, (0, -0.45, 0.45), (1.02, 0.05, 0.52))
    # Drawers on the left leg
    box("TC_Workbench_DrawerA", mid, (-0.78, -0.44, 0.56), (0.42, 0.04, 0.16), bevel=0.008)
    box("TC_Workbench_DrawerB", mid, (-0.78, -0.44, 0.34), (0.42, 0.04, 0.16), bevel=0.008)
    box("TC_Workbench_HandleA", orange, (-0.78, -0.472, 0.56), (0.20, 0.025, 0.03), bevel=0.005)
    box("TC_Workbench_HandleB", orange, (-0.78, -0.472, 0.34), (0.20, 0.025, 0.03), bevel=0.005)
    # Orange hazard stripe and status LED on the right leg
    box("TC_Workbench_StripeR", orange, (0.78, -0.42, 0.66), (0.44, 0.02, 0.08), bevel=0.005)
    box("TC_Workbench_LED", red_led, (0.55, -0.478, 0.45), (0.05, 0.012, 0.02), bevel=0)
    text_marking("TC_Workbench_Marking", "C7", beige, (0, -0.482, 0.46), 0.26, (90, 0, 0))
    # Holo panel on a mast at the back-left
    cyl("TC_Workbench_HoloMast", dark, (-0.62, 0.38, 1.08), 0.03, 0.42, vertices=8)
    box("TC_Workbench_HoloFrame", dark, (-0.62, 0.40, 1.48), (0.82, 0.035, 0.50), rot=(-12, 0, 0), bevel=0.01)
    box("TC_Workbench_HoloScreen", holo, (-0.62, 0.378, 1.48), (0.74, 0.012, 0.42), rot=(-12, 0, 0), bevel=0)
    # Robot arm: base turret on the back-right of the work surface
    cyl("TC_Workbench_ArmBase", mid, (0.72, 0.32, 0.945), 0.15, 0.07, vertices=12)
    cyl("TC_Workbench_ArmTurret", dark, (0.72, 0.32, 1.01), 0.10, 0.10, vertices=10)
    gem("TC_Workbench_ArmShoulder", mid, (0.72, 0.32, 1.08), 0.085, (1, 1, 1))
    # Upper arm leans toward the bench centre, forearm reaches down over the surface.
    shoulder = (0.72, 0.32, 1.08)
    upper_len, upper_tilt = 0.56, -35.0
    uvec = (-upper_len * math.sin(math.radians(-upper_tilt)), 0.0, upper_len * math.cos(math.radians(-upper_tilt)))
    elbow = (shoulder[0] + uvec[0], shoulder[1], shoulder[2] + uvec[2])
    box("TC_Workbench_ArmUpper", beige, (shoulder[0] + uvec[0] / 2, shoulder[1], shoulder[2] + uvec[2] / 2),
        (0.10, 0.10, upper_len), rot=(0, upper_tilt, 0), bevel=0.012)
    gem("TC_Workbench_ArmElbow", mid, elbow, 0.075, (1, 1, 1))
    fore_len, fore_tilt = 0.50, -118.0
    fvec = (-fore_len * math.sin(math.radians(-fore_tilt)), 0.0, fore_len * math.cos(math.radians(-fore_tilt)))
    wrist = (elbow[0] + fvec[0], elbow[1], elbow[2] + fvec[2])
    box("TC_Workbench_ArmFore", beige, (elbow[0] + fvec[0] / 2, elbow[1], elbow[2] + fvec[2] / 2),
        (0.08, 0.08, fore_len), rot=(0, fore_tilt, 0), bevel=0.01)
    cyl("TC_Workbench_ArmWrist", dark, wrist, 0.05, 0.08, rot=(0, fore_tilt, 0), vertices=8)
    for side, dy in (("A", -0.035), ("B", 0.035)):
        taper(f"TC_Workbench_Gripper{side}", dark,
              (wrist[0] - 0.10 * math.sin(math.radians(-fore_tilt)), wrist[1] + dy,
               wrist[2] + 0.10 * math.cos(math.radians(-fore_tilt))),
              0.028, 0.008, 0.14, rot=(0, fore_tilt, 0), bevel=0.004)
    # Bench-top clamp block and floor power block with cable
    box("TC_Workbench_Clamp", orange, (-0.35, -0.12, 0.94), (0.20, 0.14, 0.07), bevel=0.01)
    box("TC_Workbench_PowerBlock", dark, (1.55, 0.18, 0.16), (0.48, 0.40, 0.32), bevel=0.02)
    box("TC_Workbench_PowerStripe", orange, (1.55, 0.18, 0.335), (0.40, 0.32, 0.02), bevel=0.005)
    cable("TC_Workbench_Cable", rubber,
          [(1.12, 0.30, 0.55), (1.35, 0.52, 0.14), (1.62, 0.48, 0.05), (1.66, 0.24, 0.30)], 0.020)


def _beacon_base(dark: bpy.types.Material, beige: bpy.types.Material, orange: bpy.types.Material,
                 red_led: bpy.types.Material) -> None:
    cyl("TC_Beacon_Plinth", dark, (0, 0, 0.09), 0.62, 0.18, rot=(0, 0, 22.5), vertices=8, bevel=0.02)
    cyl("TC_Beacon_Ring", beige, (0, 0, 0.26), 0.50, 0.16, rot=(0, 0, 22.5), vertices=8, bevel=0.02)
    cyl("TC_Beacon_Mount", dark, (0, 0, 0.40), 0.30, 0.12, vertices=10, bevel=0.015)
    box("TC_Beacon_WarnPlate", orange, (0, -0.575, 0.09), (0.34, 0.03, 0.10), bevel=0.005)
    box("TC_Beacon_LED", red_led, (0.30, -0.52, 0.26), (0.10, 0.02, 0.03), bevel=0)


def _beacon_petals(tilt: float, beige: bpy.types.Material, dark: bpy.types.Material,
                   inner: bpy.types.Material | None = None) -> None:
    length, base_z, offset = 1.75, 0.44, 0.24
    for i in range(4):
        ang = i * 90.0
        rad = math.radians(ang)
        tilt_rad = math.radians(tilt)
        cx = offset * math.cos(rad) + (length / 2) * math.sin(tilt_rad) * math.cos(rad)
        cy = offset * math.sin(rad) + (length / 2) * math.sin(tilt_rad) * math.sin(rad)
        cz = base_z + (length / 2) * math.cos(tilt_rad)
        taper(f"TC_Beacon_Petal_{i}", beige, (cx, cy, cz), 0.26, 0.05, length,
              rot=(0, tilt, ang), scale=(1, 0.42, 1), bevel=0.02)
        spine_r = offset + 0.145
        sx = spine_r * math.cos(rad) + (length / 2) * math.sin(tilt_rad) * math.cos(rad)
        sy = spine_r * math.sin(rad) + (length / 2) * math.sin(tilt_rad) * math.sin(rad)
        taper(f"TC_Beacon_PetalSpine_{i}", dark, (sx, sy, cz), 0.10, 0.02, length * 0.92,
              rot=(0, tilt, ang), scale=(0.5, 0.5, 1), bevel=0.008)
        if inner is not None:
            in_r = offset - 0.07
            ix = in_r * math.cos(rad) + (length / 2) * math.sin(tilt_rad) * math.cos(rad)
            iy = in_r * math.sin(rad) + (length / 2) * math.sin(tilt_rad) * math.sin(rad)
            taper(f"TC_Beacon_PetalGlow_{i}", inner, (ix, iy, cz), 0.14, 0.02, length * 0.85,
                  rot=(0, tilt, ang), scale=(0.35, 0.8, 1), bevel=0)


def build_beacon_dormant() -> None:
    """Closed rescue beacon: four petals folded into a tapered obelisk."""
    beige = pbr("TC_MVP_HullBeige", BEIGE_HULL, rough=0.62, metal=0.12)
    dark = pbr("TC_MVP_DarkMetal", DARK_METAL, rough=0.48, metal=0.65)
    orange = pbr("TC_MVP_OrangePaint", ORANGE_PAINT, rough=0.42)
    red_led = pbr("TC_MVP_RedLED", (0.02, 0.0, 0.0), rough=0.3, emission=RED_GLOW, strength=4.0)
    _beacon_base(dark, beige, orange, red_led)
    _beacon_petals(-5.0, beige, dark)


def build_beacon_active() -> None:
    """Activated rescue beacon: petals opened, emissive purple crystal core."""
    beige = pbr("TC_MVP_HullBeige", BEIGE_HULL, rough=0.62, metal=0.12)
    dark = pbr("TC_MVP_DarkMetal", DARK_METAL, rough=0.48, metal=0.65)
    orange = pbr("TC_MVP_OrangePaint", ORANGE_PAINT, rough=0.42)
    red_led = pbr("TC_MVP_RedLED", (0.02, 0.0, 0.0), rough=0.3, emission=RED_GLOW, strength=4.0)
    purple = pbr("TC_MVP_PurpleGlow", (0.01, 0.0, 0.03), rough=0.3, emission=PURPLE_GLOW, strength=7.0)
    crystal = pbr("TC_MVP_PurpleCrystal", PURPLE_CRYSTAL, rough=0.22, emission=PURPLE_GLOW, strength=2.5)
    rubber = pbr("TC_MVP_Rubber", RUBBER_DARK, rough=0.85)
    _beacon_base(dark, beige, orange, red_led)
    _beacon_petals(38.0, beige, dark, inner=purple)
    cyl("TC_Beacon_CoreDisk", purple, (0, 0, 0.47), 0.24, 0.03, vertices=12, bevel=0)
    rng = random.Random(31)
    gem("TC_Beacon_Core", crystal, (0, 0, 1.05), 0.17, (0.45, 0.45, 3.4), rot=(0, 0, 15))
    for i, (dx, dy, tip) in enumerate(((0.14, 0.05, 18), (-0.12, 0.10, -16), (0.02, -0.15, 12))):
        gem(f"TC_Beacon_CoreShard_{i}", crystal, (dx, dy, 0.72), 0.09, (0.4, 0.4, 2.2),
            rot=(tip, tip * 0.6, i * 40), jitter=0.008, rng=rng)
    box("TC_Beacon_PowerBox", dark, (0.95, -0.30, 0.12), (0.34, 0.28, 0.24), bevel=0.015)
    cable("TC_Beacon_Cable", rubber,
          [(0.58, -0.12, 0.14), (0.78, -0.22, 0.05), (0.92, -0.28, 0.10), (0.95, -0.30, 0.20)], 0.018)


def build_pickup_metal() -> None:
    """Metal resource: pile of angular salvaged hull shards."""
    steel = pbr("TC_MVP_SteelScrap", STEEL_SCRAP, rough=0.42, metal=0.9)
    beige = pbr("TC_MVP_HullBeige", BEIGE_HULL, rough=0.62, metal=0.12)
    orange = pbr("TC_MVP_OrangePaint", ORANGE_PAINT, rough=0.42)
    rng = random.Random(7)
    for i in range(7):
        ang = rng.uniform(0, 360)
        dist = rng.uniform(0.02, 0.16)
        mat = beige if i in (2, 5) else steel
        taper(f"TC_Metal_Shard_{i}", mat,
              (dist * math.cos(math.radians(ang)), dist * math.sin(math.radians(ang)), 0.035 + i * 0.012),
              rng.uniform(0.09, 0.14), rng.uniform(0.015, 0.04), rng.uniform(0.26, 0.44),
              rot=(rng.uniform(78, 100), rng.uniform(-14, 14), rng.uniform(0, 360)),
              scale=(1, 0.22, 1), bevel=0.008)
    box("TC_Metal_PaintMark", orange, (0.05, -0.02, 0.115), (0.10, 0.05, 0.012), rot=(4, -8, 30), bevel=0)


def build_pickup_biomass() -> None:
    """Biomass resource: dark-red faceted organic cluster with growth spikes."""
    flesh = pbr("TC_MVP_BiomassRed", BIOMASS_RED, rough=0.32, emission=(0.35, 0.01, 0.02), strength=0.5)
    dark = pbr("TC_MVP_DarkMetal", DARK_METAL, rough=0.48, metal=0.65)
    rng = random.Random(13)
    blobs = ((0, 0, 0.16, 0.17), (0.14, 0.06, 0.10, 0.11), (-0.13, 0.05, 0.09, 0.10),
             (0.03, -0.14, 0.08, 0.09), (-0.05, 0.13, 0.07, 0.08))
    for i, (x, y, z, r) in enumerate(blobs):
        gem(f"TC_Biomass_Blob_{i}", flesh, (x, y, z), r, (1, 1, 0.92),
            rot=(rng.uniform(0, 90), rng.uniform(0, 90), 0), jitter=0.016, rng=rng)
    for i in range(5):
        ang = 25 + i * 72.0
        taper(f"TC_Biomass_Spike_{i}", flesh,
              (0.16 * math.cos(math.radians(ang)), 0.16 * math.sin(math.radians(ang)), 0.20),
              0.025, 0.002, 0.16, rot=(rng.uniform(-35, -15), rng.uniform(-12, 12), ang), vertices=5, bevel=0)
    gem("TC_Biomass_Substrate", dark, (0, 0, 0.02), 0.20, (1, 0.9, 0.18), jitter=0.02, rng=rng)


def build_pickup_electronics() -> None:
    """Electronics resource: stacked dark modules with orange lids and cyan indicators."""
    dark = pbr("TC_MVP_DarkMetal", DARK_METAL, rough=0.48, metal=0.65)
    mid = pbr("TC_MVP_MidMetal", MID_METAL, rough=0.45, metal=0.75)
    orange = pbr("TC_MVP_OrangePaint", ORANGE_PAINT, rough=0.42)
    cyan = pbr("TC_MVP_CyanLED", (0.0, 0.02, 0.02), rough=0.3, emission=CYAN_GLOW, strength=4.0)
    box("TC_Electronics_CrateMain", dark, (0, 0, 0.10), (0.32, 0.26, 0.20), bevel=0.02)
    box("TC_Electronics_LidMain", orange, (0, 0, 0.21), (0.28, 0.22, 0.03), bevel=0.008)
    box("TC_Electronics_ModuleA", mid, (0.24, -0.10, 0.06), (0.16, 0.13, 0.12), rot=(0, 0, -18), bevel=0.012)
    box("TC_Electronics_ModuleB", dark, (-0.06, 0.02, 0.27), (0.18, 0.15, 0.10), rot=(0, 0, 12), bevel=0.012)
    box("TC_Electronics_LidB", orange, (-0.06, 0.02, 0.325), (0.15, 0.12, 0.02), rot=(0, 0, 12), bevel=0.005)
    box("TC_Electronics_IndicatorA", cyan, (0.005, -0.132, 0.14), (0.10, 0.006, 0.015), bevel=0)
    box("TC_Electronics_IndicatorB", cyan, (0.27, -0.155, 0.08), (0.05, 0.006, 0.012), rot=(0, 0, -18), bevel=0)
    cyl("TC_Electronics_Antenna", mid, (-0.10, -0.06, 0.38), 0.006, 0.14, vertices=6, bevel=0)


def build_pickup_component() -> None:
    """Mission component: purple alien crystal cluster on a dark rock shard."""
    crystal = pbr("TC_MVP_PurpleCrystal", PURPLE_CRYSTAL, rough=0.22, emission=PURPLE_GLOW, strength=1.4)
    dark = pbr("TC_MVP_DarkMetal", DARK_METAL, rough=0.7, metal=0.2)
    rng = random.Random(21)
    gem("TC_Component_Core", crystal, (0, 0, 0.26), 0.115, (0.55, 0.55, 2.3), rot=(0, 0, 20))
    sats = ((0.12, 0.04, 24, 0.07, 1.7), (-0.11, 0.06, -28, 0.06, 1.5),
            (0.02, -0.13, 15, 0.055, 1.4), (-0.04, 0.12, -12, 0.05, 1.2))
    for i, (x, y, tilt, r, stretch) in enumerate(sats):
        gem(f"TC_Component_Shard_{i}", crystal, (x, y, 0.14), r, (0.5, 0.5, stretch),
            rot=(tilt, tilt * 0.7, i * 55))
    gem("TC_Component_Rock", dark, (0, 0, 0.03), 0.18, (1, 0.85, 0.22), jitter=0.025, rng=rng)


def strut(name: str, mat: bpy.types.Material, start: tuple[float, float, float], end: tuple[float, float, float],
          r_start: float, r_end: float, scale: tuple[float, float, float] = (1, 1, 1),
          vertices: int = 4, bevel: float = 0.008) -> bpy.types.Object:
    """Tapered prism spanning two points; used for limbs and struts."""
    p1, p2 = Vector(start), Vector(end)
    direction = p2 - p1
    bpy.ops.mesh.primitive_cone_add(vertices=vertices, radius1=r_start, radius2=r_end,
                                    depth=direction.length, location=(p1 + p2) / 2)
    obj = bpy.context.object
    obj.name = name
    obj.rotation_euler = direction.to_track_quat("Z", "Y").to_euler()
    obj.scale = scale
    return _finalize(obj, mat, bevel)


def build_scout() -> None:
    """Galaxabrain Scout: spindly quadruped biomech with an emissive purple core."""
    carbon = pbr("TC_MVP_AlienCarbon", (0.030, 0.032, 0.040), rough=0.38, metal=0.55)
    plate = pbr("TC_MVP_AlienPlate", (0.085, 0.090, 0.105), rough=0.45, metal=0.65)
    purple = pbr("TC_MVP_PurpleGlow", (0.01, 0.0, 0.03), rough=0.3, emission=PURPLE_GLOW, strength=7.0)
    crystal = pbr("TC_MVP_PurpleCrystal", PURPLE_CRYSTAL, rough=0.22, emission=PURPLE_GLOW, strength=1.4)
    rng = random.Random(47)
    # Torso pod with layered carapace plates
    gem("TC_Scout_Torso", carbon, (0, 0, 1.10), 0.42, (1.0, 0.72, 0.62), rot=(0, 12, 0), jitter=0.02, rng=rng)
    box("TC_Scout_PlateTop", plate, (0.02, 0, 1.36), (0.55, 0.42, 0.10), rot=(0, 14, 0), bevel=0.02)
    box("TC_Scout_PlateBack", plate, (-0.30, 0, 1.16), (0.28, 0.48, 0.30), rot=(0, -18, 0), bevel=0.02)
    # Forward head cluster with glowing braincase
    strut("TC_Scout_Neck", plate, (0.30, 0, 1.18), (0.62, 0, 1.34), 0.10, 0.06)
    gem("TC_Scout_Head", carbon, (0.72, 0, 1.36), 0.16, (1.2, 0.85, 0.9), rot=(0, 20, 0), jitter=0.012, rng=rng)
    gem("TC_Scout_Braincase", crystal, (0.76, 0, 1.44), 0.10, (0.9, 0.7, 0.9), jitter=0.01, rng=rng)
    box("TC_Scout_EyeSlit", purple, (0.86, 0, 1.35), (0.05, 0.14, 0.025), rot=(0, 20, 0), bevel=0)
    # Chest core
    gem("TC_Scout_Core", purple, (0.26, 0, 1.02), 0.11, (0.8, 0.8, 1.1))
    # Four spider legs: hip -> raised knee -> bladed foot
    for i, ang in enumerate((50, 130, 230, 310)):
        rad = math.radians(ang)
        hip = (0.30 * math.cos(rad), 0.34 * math.sin(rad), 1.06)
        knee = (0.85 * math.cos(rad), 0.88 * math.sin(rad), 1.62)
        foot = (1.15 * math.cos(rad), 1.18 * math.sin(rad), 0.0)
        strut(f"TC_Scout_LegUpper_{i}", plate, hip, knee, 0.075, 0.045, bevel=0.01)
        gem(f"TC_Scout_Knee_{i}", carbon, knee, 0.07, (1, 1, 1))
        strut(f"TC_Scout_LegLower_{i}", carbon, knee, foot, 0.045, 0.012, scale=(1, 0.55, 1), bevel=0.008)
        strut(f"TC_Scout_LegGlow_{i}", purple,
              ((hip[0] + knee[0]) / 2, (hip[1] + knee[1]) / 2, (hip[2] + knee[2]) / 2 + 0.03),
              (knee[0], knee[1], knee[2] + 0.02), 0.02, 0.008, bevel=0)


def build_mechanical_arm() -> None:
    """First-person mechanical arm reward: segmented forearm, purple energy seams."""
    beige = pbr("TC_MVP_HullBeige", BEIGE_HULL, rough=0.62, metal=0.12)
    dark = pbr("TC_MVP_DarkMetal", DARK_METAL, rough=0.48, metal=0.65)
    mid = pbr("TC_MVP_MidMetal", MID_METAL, rough=0.45, metal=0.75)
    purple = pbr("TC_MVP_PurpleGlow", (0.01, 0.0, 0.03), rough=0.3, emission=PURPLE_GLOW, strength=5.0)
    orange = pbr("TC_MVP_OrangePaint", ORANGE_PAINT, rough=0.42)
    # Forearm along +Y from the elbow, resting on the ground plane for review.
    segments = ((0.10, 0.155, 0.20), (0.32, 0.145, 0.18), (0.52, 0.135, 0.14))
    for i, (y, r, depth) in enumerate(segments):
        cyl(f"TC_Arm_Segment_{i}", beige if i % 2 == 0 else mid, (0, y, 0.17), r, depth,
            rot=(90, 0, 0), vertices=10, bevel=0.015)
        cyl(f"TC_Arm_Seam_{i}", purple, (0, y + depth / 2 + 0.012, 0.17), r * 0.82, 0.018,
            rot=(90, 0, 0), vertices=10, bevel=0)
    cyl("TC_Arm_Elbow", dark, (0, -0.04, 0.17), 0.165, 0.10, rot=(90, 0, 0), vertices=10, bevel=0.015)
    box("TC_Arm_TopPlate", dark, (0, 0.30, 0.335), (0.16, 0.42, 0.035), bevel=0.01)
    box("TC_Arm_StripePlate", orange, (0.13, 0.22, 0.24), (0.02, 0.16, 0.05), bevel=0.005)
    # Wrist and simplified powered fist
    cyl("TC_Arm_Wrist", dark, (0, 0.66, 0.17), 0.10, 0.08, rot=(90, 0, 0), vertices=10, bevel=0.01)
    box("TC_Arm_Palm", mid, (0, 0.78, 0.17), (0.20, 0.16, 0.20), bevel=0.02)
    for i, x in enumerate((-0.065, 0.0, 0.065)):
        box(f"TC_Arm_Finger_{i}", dark, (x, 0.90, 0.19), (0.05, 0.10, 0.06), rot=(-8, 0, 0), bevel=0.01)
    box("TC_Arm_Thumb", dark, (-0.12, 0.80, 0.11), (0.05, 0.09, 0.05), rot=(0, 0, -20), bevel=0.01)
    box("TC_Arm_Knuckle", purple, (0, 0.856, 0.245), (0.15, 0.02, 0.02), bevel=0)


def build_save_point() -> None:
    """Save point: compact checkpoint pillar with cyan emissive ring and strip."""
    beige = pbr("TC_MVP_HullBeige", BEIGE_HULL, rough=0.62, metal=0.12)
    dark = pbr("TC_MVP_DarkMetal", DARK_METAL, rough=0.48, metal=0.65)
    cyan = pbr("TC_MVP_CyanLED", (0.0, 0.02, 0.02), rough=0.3, emission=CYAN_GLOW, strength=5.0)
    orange = pbr("TC_MVP_OrangePaint", ORANGE_PAINT, rough=0.42)
    cyl("TC_SavePoint_Base", dark, (0, 0, 0.07), 0.42, 0.14, rot=(0, 0, 30), vertices=6, bevel=0.02)
    taper("TC_SavePoint_Column", beige, (0, 0, 0.80), 0.26, 0.17, 1.32, rot=(0, 0, 30), vertices=6, bevel=0.02)
    cyl("TC_SavePoint_Ring", cyan, (0, 0, 0.24), 0.30, 0.05, rot=(0, 0, 30), vertices=6, bevel=0)
    box("TC_SavePoint_Strip", cyan, (0.24, 0, 0.86), (0.03, 0.09, 0.90), rot=(0, -2.2, 0), bevel=0)
    cyl("TC_SavePoint_CapMount", dark, (0, 0, 1.50), 0.19, 0.08, rot=(0, 0, 30), vertices=6, bevel=0.012)
    cyl("TC_SavePoint_Emitter", cyan, (0, 0, 1.56), 0.13, 0.04, vertices=10, bevel=0)
    box("TC_SavePoint_WarnPlate", orange, (0, -0.40, 0.07), (0.26, 0.03, 0.08), bevel=0.005)


def build_crash_debris() -> None:
    """Crash debris landmark: bent off-white hull shard with exposed scorched ribs."""
    beige = pbr("TC_MVP_HullBeige", BEIGE_HULL, rough=0.62, metal=0.12)
    soot = pbr("TC_MVP_SootMetal", (0.055, 0.050, 0.048), rough=0.75, metal=0.35)
    mid = pbr("TC_MVP_MidMetal", MID_METAL, rough=0.45, metal=0.75)
    orange = pbr("TC_MVP_OrangePaint", ORANGE_PAINT, rough=0.42)
    box("TC_Debris_PlateMain", beige, (0, 0, 0.85), (1.9, 0.14, 1.9), rot=(0, -28, 8), bevel=0.03)
    box("TC_Debris_PlateBent", beige, (0.95, 0.18, 0.28), (1.1, 0.12, 0.9), rot=(6, 38, 14), bevel=0.03)
    box("TC_Debris_EdgeScorch", soot, (-0.72, -0.10, 1.42), (0.55, 0.16, 1.1), rot=(4, -28, 8), bevel=0.02)
    for i, dy in enumerate((-0.45, 0.0, 0.45)):
        box(f"TC_Debris_Rib_{i}", mid, (0.1 + 0.06 * i, dy, 0.42), (0.09, 0.09, 0.84), rot=(0, 12, 0), bevel=0.012)
    box("TC_Debris_Chunk", soot, (-0.55, 0.62, 0.16), (0.62, 0.5, 0.32), rot=(0, 0, 24), bevel=0.02)
    box("TC_Debris_Marking", orange, (0.18, -0.075, 1.05), (0.55, 0.015, 0.18), rot=(0, -28, 8), bevel=0)


ASSETS = {
    "TC_PROP_Workbench_V1": build_workbench,
    "TC_PROP_Beacon_Dormant_V1": build_beacon_dormant,
    "TC_PROP_Beacon_Active_V1": build_beacon_active,
    "TC_PICKUP_Metal_V1": build_pickup_metal,
    "TC_PICKUP_Biomass_V1": build_pickup_biomass,
    "TC_PICKUP_Electronics_V1": build_pickup_electronics,
    "TC_PICKUP_Component_V1": build_pickup_component,
    "TC_CHAR_GalaxabrainScout_V1": build_scout,
    "TC_PLAYER_MechanicalArm_V1": build_mechanical_arm,
    "TC_PROP_SavePoint_V1": build_save_point,
    "TC_ENV_CrashDebris_A_V1": build_crash_debris,
}


def main() -> None:
    only = None
    if "--" in sys.argv:
        tail = sys.argv[sys.argv.index("--") + 1:]
        if len(tail) == 2 and tail[0] == "--asset":
            only = tail[1]
        elif tail:
            raise SystemExit("usage: blender --background --python create_mvp_asset_pack_v1.py [-- --asset TC_NAME]")
    names = [only] if only else list(ASSETS)
    for name in names:
        if name not in ASSETS:
            raise SystemExit(f"unknown asset: {name}")
        reset_scene()
        ASSETS[name]()
        output = SOURCE_DIR / f"{name}.blend"
        output.parent.mkdir(parents=True, exist_ok=True)
        bpy.ops.wm.save_as_mainfile(filepath=str(output))
        print(f"MVP_PACK_BLEND_WRITTEN {output}")


if __name__ == "__main__":
    main()
