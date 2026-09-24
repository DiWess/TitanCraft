#!/usr/bin/env python3
"""Create the TC_ENV Mwezi Quarter District Kit V1 Blender sources.

Purpose
-------
The MVP's single map (README section 14) is currently dressed almost entirely
with volcanic rock, ship wreckage and camp props. It has one landmark
(`SignalSpire_Landmark`), no interior/exterior transitions, no readable street
structure, and no architecture at all, which is the root cause of the
world/level-design and visual-presentation gaps recorded in
`docs/production/quality-scorecard-log.md` (axis 5 and axis 6).

This kit adds the built environment the crash site lands in: a weathered
coastal stone quarter whose architecture, materials and street grain are drawn
from the Swahili-Comorian coral-rag tradition of the Indian Ocean coast --
lime-washed coral stone walls over a basalt and crushed-coral rubble core,
arcaded courtyards on square piers, carved timber doors bleached grey by age
and salt air, heavy lintels, flat roofs with parapets, external stairs, and a
seawall facing the water.

Creative-property note (README section 67, "Regles de propriete creative")
-------------------------------------------------------------------------
The in-game place carries its own name, the **Mwezi Quarter**, and its own
fiction. The arcaded hall asset is a secular civic/heritage ruin -- a former
harbour assembly hall -- not a place of worship, and no real building, real
place name, or religious function is reproduced or named anywhere in this kit
or in the scenes that consume it. The reference is a regional building
tradition, in the same way the existing wreck assets reference aerospace
salvage generally.

Assets (all visual-only, collisionless -- gameplay collision stays authored in
the scene as explicit BoxShape3D volumes, per the scene collision contract
asserted by `tests/Integration/IntegrationTestRunner.cs`):

- TC_ENV_CoralWallSegment_V1 -- rendered wall run with plaster loss to the dark core
- TC_ENV_ArcadeBay_V1        -- three-bay pointed arcade on square piers
- TC_ENV_QuarterTower_V1     -- tapering stone tower with belvedere (landmark)
- TC_ENV_CarvedDoorway_V1    -- carved bleached-timber double door in a stone surround
- TC_ENV_StoneHouse_V1       -- two-storey flat-roof house with external stair
- TC_ENV_StairTerrace_V1     -- stepped terrace, the map's vertical element
- TC_ENV_SeawallRun_V1       -- seawall coping with mooring bollards
- TC_ENV_MarketStall_V1      -- shaded stall with counter and baskets
- TC_ENV_PalmCluster_V1      -- coastal palm pair for silhouette variety
- TC_ENV_CoralRubble_V1      -- collapsed coral-stone rubble

Run with Blender:
  blender --background --python tools/blender/create_badjanani_district_kit_v1.py
  blender --background --python tools/blender/create_badjanani_district_kit_v1.py -- --asset TC_ENV_ArcadeBay_V1
"""
from __future__ import annotations

import math
import sys
from pathlib import Path

import bpy

ROOT = Path(__file__).resolve().parents[2]
SOURCE_DIR = ROOT / "assets/Source/Blender/Production/MweziQuarter_V1"

# Palette. The first four tones are the quarter's own coral/lime/basalt family;
# the last three are matched to existing repo materials so the district sits in
# the same scene as the wreck without a tonal seam. Every value here is mirrored
# in data/art/comorian_seafront_spec.json so the seafront kit and this kit read
# as one settlement; change it there and here together.
LIME_WASH = (0.858824, 0.831373, 0.760784)   # sun-bleached lime render
CORAL_STONE = (0.560784, 0.509804, 0.423529) # dressed coral rag: copings, jambs, trim
CORAL_SHADOW = (0.352941, 0.317647, 0.262745) # weathered/eroded stone
# Where the render has actually fallen away, the wall's rubble core shows, and
# written description of the quarter is specific about it: the stones behind the
# collapsed grey plaster are "noires comme de l'encre" -- ink-black. The core is
# a basalt / crushed-coral / sea-sand mix, so it reads near-black, NOT as the
# pale coral rag this kit first used. See section 3.5 of
# docs/art/references/comorian-seafront-reference-v1.md.
PLASTER_LOSS_CORE = (0.090196, 0.086275, 0.082353)
# The quarter's woodwork is described as "d'un bois fendille, blanchi par l'age
# et le climat" -- split and bleached by age and salt air. A dark oiled hardwood
# is what a new door looks like; nothing in this ruined quarter is new.
BLEACHED_TIMBER = (0.435294, 0.400000, 0.352941)
TERRACOTTA = (0.615686, 0.325490, 0.211765)  # tile, pot, awning cloth
PALM_FROND = (0.278431, 0.341176, 0.203922)  # dry coastal frond
GRAPHITE = (0.145098, 0.164706, 0.188235)    # assets/Materials/HumanGraphite.tres
WORN_STEEL = (0.431373, 0.454902, 0.470588)  # assets/Materials/HumanWornSteel.tres
ORANGE = (0.909804, 0.470588, 0.133333)      # assets/Materials/HumanOrangeInteractive.tres


def reset_scene() -> None:
    bpy.ops.wm.read_factory_settings(use_empty=True)
    # The review renderer (tools/blender/render_asset_review.py) sets
    # scene.world.color, so the saved source must carry a World block.
    world = bpy.data.worlds.new("World")
    bpy.context.scene.world = world


def pbr(name: str, color: tuple[float, float, float], rough: float = 0.85,
        metal: float = 0.0, emission: tuple[float, float, float] | None = None,
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


def _join(name: str, parts: list[bpy.types.Object], bevel_width: float = 0.018) -> bpy.types.Object:
    bpy.ops.object.select_all(action="DESELECT")
    for part in parts:
        part.select_set(True)
    bpy.context.view_layer.objects.active = parts[0]
    bpy.ops.object.join()
    joined = bpy.context.object
    joined.name = name
    bpy.ops.object.transform_apply(location=True, rotation=True, scale=True)
    if bevel_width > 0.0:
        # Flat-shaded primitive architecture reads as paper until its edges
        # catch light. A single narrow bevel segment, clamped so it cannot
        # eat thin members, is what separates a stone building from a box.
        modifier = joined.modifiers.new("TC_EdgeBevel", "BEVEL")
        modifier.width = bevel_width
        modifier.segments = 1
        modifier.limit_method = "ANGLE"
        modifier.angle_limit = math.radians(35.0)
        modifier.use_clamp_overlap = True
        modifier.harden_normals = False
        bpy.ops.object.modifier_apply(modifier="TC_EdgeBevel")
    _tag(joined)
    return joined


def _pointed_arch(parts: list[bpy.types.Object], tag: str, center_x: float, spring_z: float,
                  span: float, depth: float, thickness: float,
                  mat: bpy.types.Material, segments: int = 5) -> float:
    """Append voussoir boxes forming one pointed arch; return the apex height.

    Two circular arcs whose centres are pushed apart horizontally meet at a
    point above the centre line -- the pointed profile used throughout coastal
    coral-stone arcading, as opposed to a plain Roman semicircle.
    """
    offset = span * 0.24
    radius = span / 2.0 + offset
    # The arc is swept from the springing (theta = 0) to where it crosses the
    # arch centre line (x = center_x), which is what makes the apex a point.
    end_angle = math.acos(min(1.0, offset / radius))
    apex = spring_z + math.sqrt(max(radius * radius - offset * offset, 0.0))
    step = end_angle / segments
    for side, direction in (("r", 1.0), ("l", -1.0)):
        arc_center_x = center_x - direction * offset
        for i in range(segments):
            mid = (i + 0.5) * step
            x = arc_center_x + direction * radius * math.cos(mid)
            z = spring_z + radius * math.sin(mid)
            seg_len = radius * step * 1.14  # slight overlap closes the joints
            # Rotate each voussoir so its thickness axis stays radial.
            angle = mid if direction > 0.0 else math.pi - mid
            parts.append(_box(f"{tag}_vous_{side}{i}", (thickness, depth, seg_len),
                              (x, 0.0, z), mat, rot=(0.0, -angle, 0.0)))
    return apex


def _cut_plaster_loss(skin: bpy.types.Object, patches: list[tuple[float, float, float, float]],
                      skin_depth: float, plaster: float) -> None:
    """Cut patch openings through both faces of a plaster skin.

    The cutters pass clean through the skin so the opening is a real hole and
    the rubble core behind it is what the camera sees. Cutting only part-way
    would leave a dimple in the plaster with more plaster at the bottom of it,
    which is the failure mode this helper exists to avoid.
    """
    for i, (x, z, w, h) in enumerate(patches):
        bpy.ops.mesh.primitive_cube_add(size=1.0, location=(x, 0.0, z))
        cutter = bpy.context.object
        cutter.name = f"cut_plaster_loss_{i}"
        # Overshoot the skin depth so the boolean has no coplanar faces to
        # resolve; coplanar cutters are the usual source of boolean artefacts.
        cutter.scale = (w, skin_depth + plaster * 4.0, h)
        modifier = skin.modifiers.new(f"TC_PlasterLoss_{i}", "BOOLEAN")
        modifier.operation = "DIFFERENCE"
        modifier.object = cutter
        modifier.solver = "EXACT"
        bpy.context.view_layer.objects.active = skin
        bpy.ops.object.modifier_apply(modifier=modifier.name)
        bpy.data.objects.remove(cutter, do_unlink=True)


def build_coral_wall_segment() -> None:
    """A 6 m wall run: lime render above, dark rubble core where the render failed."""
    render_mat = pbr("TC_MAT_LimeRender", LIME_WASH, rough=0.92)
    stone_mat = pbr("TC_MAT_CoralRag", CORAL_STONE, rough=0.95)
    shadow_mat = pbr("TC_MAT_CoralWeathered", CORAL_SHADOW, rough=0.96)
    core_mat = pbr("TC_MAT_PlasterLossCore", PLASTER_LOSS_CORE, rough=0.97)
    parts: list[bpy.types.Object] = []
    length, height, thick = 6.0, 3.0, 0.45
    # The wall is modelled the way it is actually built: a rubble core of
    # basalt, crushed coral and sea sand, carrying a thin lime-plaster skin.
    # That ordering is what makes plaster loss free -- cut the skin and the
    # dark core is already there behind it.
    #
    # Two earlier attempts got this wrong and the review renders showed it.
    # Making each patch a slab slightly THICKER than the wall read as three
    # dark panels hung on a white wall, the same protruding-void failure caught
    # on the window openings. Recessing the slab instead made the patches
    # vanish entirely: a recess buried inside a solid render box is not a hole,
    # it is hidden geometry. A hole needs the skin genuinely cut away.
    #
    # The core is inset by the plaster thickness on X and Z as well as Y, so the
    # skin wraps it on every face. Sharing a coplanar end face with the skin
    # z-fights, and the review render showed exactly that as a hatched stripe
    # down the end of the wall.
    plaster = 0.04
    parts.append(_box("wall_core", (length - plaster * 2.0, thick, height - plaster * 2.0),
                      (0.0, 0.0, height / 2.0), core_mat))
    skin = _box("wall_plaster", (length, thick + plaster * 2.0, height),
                (0.0, 0.0, height / 2.0), render_mat)
    # Patch 3 is pulled in from the wall end so its opening still lands on core
    # rather than on the inset gap between core and skin.
    patches = [(-2.0, 0.55, 1.5, 1.1), (1.1, 1.5, 1.9, 0.9), (2.35, 0.4, 0.9, 0.8)]
    _cut_plaster_loss(skin, patches, thick + plaster * 2.0, plaster)
    parts.append(skin)
    # Coping ledge: the shadow line that makes the silhouette read at distance.
    parts.append(_box("wall_coping", (length + 0.24, thick + 0.22, 0.18), (0.0, 0.0, height + 0.09), stone_mat))
    parts.append(_box("wall_string_course", (length + 0.1, thick + 0.1, 0.1), (0.0, 0.0, height * 0.62), stone_mat))
    # Loose rubble stones standing proud of the core inside each opening, so a
    # patch reads as broken masonry rather than a flat dark rectangle. They stop
    # short of the plaster face, which is what keeps the recess legible.
    for i, (x, z, w, h) in enumerate(patches):
        for j, (ox, oz, sw, sh) in enumerate([
            (-0.26, -0.24, 0.42, 0.30), (0.22, -0.30, 0.34, 0.24),
            (-0.10, 0.18, 0.48, 0.26), (0.30, 0.26, 0.30, 0.22),
        ]):
            parts.append(_box(
                f"wall_patch_{i}_stone_{j}",
                (w * sw, thick + plaster, h * sh),
                (x + w * ox, 0.0, z + h * oz), core_mat,
                rot=(0.0, 0.05 * (1 if j % 2 else -1), 0.0)))
    # Plinth: damp course, always darker on a real coastal wall.
    parts.append(_box("wall_plinth", (length + 0.16, thick + 0.16, 0.42), (0.0, 0.0, 0.21), shadow_mat))
    # Two shallow buttress piers for shadow rhythm along the run.
    for i, x in enumerate((-1.9, 1.9)):
        parts.append(_box(f"wall_pier_{i}", (0.55, thick + 0.34, height - 0.2), (x, 0.0, (height - 0.2) / 2.0), render_mat))
    _join("TC_ENV_CoralWallSegment_V1", parts)


def build_arcade_bay() -> None:
    """Three pointed arches on square piers -- the quarter's civic hall front."""
    render_mat = pbr("TC_MAT_ArcadeRender", LIME_WASH, rough=0.9)
    stone_mat = pbr("TC_MAT_ArcadeStone", CORAL_STONE, rough=0.94)
    shadow_mat = pbr("TC_MAT_ArcadeShadow", CORAL_SHADOW, rough=0.95)
    parts: list[bpy.types.Object] = []
    bay_span = 2.6
    pier_w, depth = 0.7, 1.0
    spring = 2.2
    apex = spring
    centres = (-bay_span - pier_w, 0.0, bay_span + pier_w)
    for i, cx in enumerate(centres):
        apex = _pointed_arch(parts, f"arch{i}", cx, spring, bay_span, depth, 0.42, stone_mat)
    # Piers between and beside the arches, with a capital band at the springing.
    pier_xs = [centres[0] - bay_span / 2.0 - pier_w / 2.0]
    for cx in centres:
        pier_xs.append(cx + bay_span / 2.0 + pier_w / 2.0)
    for i, x in enumerate(pier_xs):
        parts.append(_box(f"pier_{i}", (pier_w, depth, spring), (x, 0.0, spring / 2.0), render_mat))
        parts.append(_box(f"pier_base_{i}", (pier_w + 0.2, depth + 0.2, 0.3), (x, 0.0, 0.15), shadow_mat))
        parts.append(_box(f"pier_cap_{i}", (pier_w + 0.22, depth + 0.22, 0.24), (x, 0.0, spring - 0.12), stone_mat))
    width = (pier_xs[-1] - pier_xs[0]) + pier_w
    # Spandrel wall filling above the arches, then the entablature and parapet.
    parts.append(_box("spandrel", (width, depth * 0.92, apex - spring + 0.2),
                      (0.0, 0.0, spring + (apex - spring + 0.2) / 2.0), render_mat))
    parts.append(_box("cornice", (width + 0.4, depth + 0.32, 0.26), (0.0, 0.0, apex + 0.33), stone_mat))
    parapet_y = -depth * 0.18
    parts.append(_box("parapet", (width, depth * 0.5, 0.62), (0.0, parapet_y, apex + 0.77), render_mat))
    # Parapet merlons: the notched skyline that identifies the hall at distance.
    # They share the parapet's depth plane so the skyline reads as one wall
    # rather than as blocks hovering behind a ledge.
    merlons = 7
    for i in range(merlons):
        x = -width / 2.0 + (i + 0.5) * (width / merlons)
        parts.append(_box(f"merlon_{i}", (width / merlons * 0.52, depth * 0.5, 0.38),
                          (x, parapet_y, apex + 1.27), stone_mat))
    _join("TC_ENV_ArcadeBay_V1", parts)


def build_quarter_tower() -> None:
    """Tapering stone tower with an open belvedere: the map's primary landmark."""
    render_mat = pbr("TC_MAT_TowerRender", LIME_WASH, rough=0.9)
    stone_mat = pbr("TC_MAT_TowerStone", CORAL_STONE, rough=0.94)
    shadow_mat = pbr("TC_MAT_TowerShadow", CORAL_SHADOW, rough=0.95)
    lamp_mat = pbr("TC_MAT_TowerBeaconGlass", ORANGE, rough=0.3, emission=ORANGE, emission_strength=4.0)
    parts: list[bpy.types.Object] = []
    # Three tapering stages; each string course reads as a shadow ring.
    stages = [(3.2, 0.0, 3.4), (2.7, 3.4, 3.0), (2.25, 6.4, 2.4)]
    for i, (side, base_z, h) in enumerate(stages):
        parts.append(_box(f"stage_{i}", (side, side, h), (0.0, 0.0, base_z + h / 2.0), render_mat))
        parts.append(_box(f"course_{i}", (side + 0.26, side + 0.26, 0.22), (0.0, 0.0, base_z + h), stone_mat))
    parts.append(_box("plinth", (3.7, 3.7, 0.5), (0.0, 0.0, 0.25), shadow_mat))
    top = stages[-1][1] + stages[-1][2]
    # Narrow slit openings, two per face. The dark void sits INSIDE the wall
    # plane with a stone reveal just proud of it, so each slit reads as a
    # hole punched through stone rather than a tile stuck on the render.
    void_mat = pbr("TC_MAT_TowerOpening", (0.07, 0.065, 0.06), rough=0.98)
    for face, (nx, ny) in enumerate([(1.0, 0.0), (-1.0, 0.0), (0.0, 1.0), (0.0, -1.0)]):
        for j, (z, stage_half) in enumerate(((1.7, 1.6), (4.6, 1.35))):
            sx, sy = (0.2, 0.46) if nx != 0.0 else (0.46, 0.2)
            rx, ry = (0.1, 0.62) if nx != 0.0 else (0.62, 0.1)
            parts.append(_box(f"slit_void_{face}_{j}", (sx, sy, 0.95),
                              (nx * (stage_half - 0.1), ny * (stage_half - 0.1), z), void_mat))
            parts.append(_box(f"slit_reveal_{face}_{j}", (rx, ry, 1.15),
                              (nx * (stage_half + 0.01), ny * (stage_half + 0.01), z), shadow_mat))
    # Belvedere: four corner posts, lintel ring and a flat cap.
    belv_h = 1.9
    for i, (x, y) in enumerate([(-0.85, -0.85), (0.85, -0.85), (-0.85, 0.85), (0.85, 0.85)]):
        parts.append(_box(f"belv_post_{i}", (0.34, 0.34, belv_h), (x, y, top + belv_h / 2.0), render_mat))
    parts.append(_box("belv_cap", (2.65, 2.65, 0.28), (0.0, 0.0, top + belv_h + 0.14), stone_mat))
    parts.append(_box("belv_cap_lip", (2.95, 2.95, 0.14), (0.0, 0.0, top + belv_h + 0.3), shadow_mat))
    # Salvaged signal lamp lashed to the belvedere: narrative hook and, in the
    # scene, the warm point the player navigates back to.
    parts.append(_cyl("lamp_housing", 0.3, 0.42, (0.0, 0.0, top + belv_h + 0.58),
                      pbr("TC_MAT_TowerLampSteel", WORN_STEEL, rough=0.6, metal=0.5), verts=8))
    parts.append(_cyl("lamp_glass", 0.22, 0.3, (0.0, 0.0, top + belv_h + 0.86), lamp_mat, verts=8))
    _join("TC_ENV_QuarterTower_V1", parts)


def build_carved_doorway() -> None:
    """Carved double door, timber bleached grey by age, in a heavy stone surround."""
    stone_mat = pbr("TC_MAT_DoorSurroundStone", CORAL_STONE, rough=0.94)
    render_mat = pbr("TC_MAT_DoorRender", LIME_WASH, rough=0.9)
    wood_mat = pbr("TC_MAT_DoorTimber", BLEACHED_TIMBER, rough=0.74)
    brass_mat = pbr("TC_MAT_DoorBrass", (0.541176, 0.396078, 0.180392), rough=0.42, metal=0.75)
    parts: list[bpy.types.Object] = []
    opening_w, opening_h, wall_t = 1.9, 2.6, 0.5
    # Surround: two jambs, a lintel, and a carved frieze above it.
    for i, x in enumerate((-(opening_w / 2.0 + 0.38), opening_w / 2.0 + 0.38)):
        parts.append(_box(f"jamb_{i}", (0.76, wall_t + 0.16, opening_h), (x, 0.0, opening_h / 2.0), stone_mat))
    parts.append(_box("lintel", (opening_w + 1.52, wall_t + 0.22, 0.46), (0.0, 0.0, opening_h + 0.23), stone_mat))
    parts.append(_box("frieze", (opening_w + 1.1, wall_t + 0.3, 0.3), (0.0, 0.0, opening_h + 0.61), render_mat))
    # Frieze dentils: the carved rhythm above the lintel.
    for i in range(9):
        x = -(opening_w + 0.9) / 2.0 + (i + 0.5) * ((opening_w + 0.9) / 9.0)
        parts.append(_box(f"dentil_{i}", (0.14, wall_t + 0.36, 0.2), (x, 0.0, opening_h + 0.61), stone_mat))
    # Recessed dark reveal so the opening reads as depth, not a painted panel.
    parts.append(_box("reveal", (opening_w, wall_t * 0.5, opening_h),
                      (0.0, 0.12, opening_h / 2.0), pbr("TC_MAT_DoorReveal", (0.09, 0.08, 0.07), rough=0.98)))
    # Two door leaves, each with a carved frame-and-panel face.
    for leaf, sign in enumerate((-1.0, 1.0)):
        cx = sign * opening_w / 4.0
        parts.append(_box(f"leaf_{leaf}", (opening_w / 2.0 - 0.03, 0.12, opening_h - 0.1),
                          (cx, -0.1, (opening_h - 0.1) / 2.0), wood_mat))
        for row in range(3):
            parts.append(_box(f"leaf_{leaf}_rail_{row}", (opening_w / 2.0 - 0.09, 0.06, 0.12),
                              (cx, -0.18, 0.55 + row * 0.82), stone_mat))
        # Brass boss studs -- the signature detail of a carved coastal door.
        for row in range(4):
            for col in range(2):
                parts.append(_cyl(f"boss_{leaf}_{row}_{col}", 0.055, 0.1,
                                  (cx + (col - 0.5) * 0.42, -0.2, 0.35 + row * 0.7), brass_mat,
                                  rot=(math.pi / 2.0, 0.0, 0.0), verts=6))
    parts.append(_box("threshold", (opening_w + 1.6, wall_t + 0.4, 0.16), (0.0, 0.0, 0.08), stone_mat))
    _join("TC_ENV_CarvedDoorway_V1", parts)


def build_stone_house() -> None:
    """Two-storey flat-roof house with parapet, shutters and an external stair."""
    render_mat = pbr("TC_MAT_HouseRender", LIME_WASH, rough=0.92)
    stone_mat = pbr("TC_MAT_HouseStone", CORAL_STONE, rough=0.94)
    shadow_mat = pbr("TC_MAT_HouseShadow", CORAL_SHADOW, rough=0.95)
    shutter_mat = pbr("TC_MAT_HouseShutter", BLEACHED_TIMBER, rough=0.76)
    dark_mat = pbr("TC_MAT_HouseOpening", (0.08, 0.075, 0.07), rough=0.98)
    parts: list[bpy.types.Object] = []
    w, d, h = 7.0, 6.0, 5.6
    parts.append(_box("body", (w, d, h), (0.0, 0.0, h / 2.0), render_mat))
    parts.append(_box("plinth", (w + 0.2, d + 0.2, 0.5), (0.0, 0.0, 0.25), shadow_mat))
    parts.append(_box("floor_band", (w + 0.12, d + 0.12, 0.16), (0.0, 0.0, 2.9), stone_mat))
    # Parapet ring above the flat roof: low enough to read as a roof edge, not
    # as a container rim.
    for i, (sx, sy, px, py) in enumerate([(w + 0.24, 0.26, 0.0, -d / 2.0), (w + 0.24, 0.26, 0.0, d / 2.0),
                                          (0.26, d + 0.24, -w / 2.0, 0.0), (0.26, d + 0.24, w / 2.0, 0.0)]):
        parts.append(_box(f"parapet_{i}", (sx, sy, 0.46), (px, py, h + 0.23), render_mat))
        parts.append(_box(f"parapet_cap_{i}", (sx + 0.14, sy + 0.14, 0.1), (px, py, h + 0.51), stone_mat))
    # Ground-floor door and upper shuttered windows on the front (-Y) face.
    # Every opening is pushed INTO the wall so it reads as a hole with a
    # reveal, never as a dark panel stuck on the render.
    face = -d / 2.0
    parts.append(_box("door_void", (1.3, 0.34, 2.3), (-1.6, face + 0.2, 1.15), dark_mat))
    parts.append(_box("door_head", (1.74, 0.5, 0.24), (-1.6, face + 0.02, 2.42), stone_mat))
    for i, x in enumerate((-2.2, 0.4, 2.4)):
        parts.append(_box(f"win_void_{i}", (1.0, 0.3, 1.2), (x, face + 0.18, 4.1), dark_mat))
        parts.append(_box(f"win_reveal_{i}", (1.24, 0.14, 1.44), (x, face + 0.05, 4.1), stone_mat))
        parts.append(_box(f"win_sill_{i}", (1.36, 0.46, 0.14), (x, face - 0.04, 3.4), stone_mat))
        # Shutters hang open against the render, flanking the recess.
        for leaf, sign in enumerate((-1.0, 1.0)):
            parts.append(_box(f"win_shutter_{i}_{leaf}", (0.44, 0.07, 1.34),
                              (x + sign * 0.84, face - 0.06, 4.1), shutter_mat))
    # Side windows so the block is not blank from the street.
    for i, y in enumerate((-1.5, 1.5)):
        parts.append(_box(f"side_win_{i}", (0.3, 0.9, 1.1), (w / 2.0 - 0.16, y, 4.0), dark_mat))
        parts.append(_box(f"side_win_reveal_{i}", (0.12, 1.14, 1.34), (w / 2.0 - 0.02, y, 4.0), stone_mat))
    # External stair climbing the +X flank to the roof terrace.
    steps = 9
    for i in range(steps):
        parts.append(_box(f"stair_{i}", (1.5, 0.52, 0.34),
                          (w / 2.0 + 0.75, -d / 2.0 + 0.4 + i * 0.52, 0.17 + i * 0.62), stone_mat))
    parts.append(_box("stair_wall", (0.28, steps * 0.52 + 0.4, 1.0),
                      (w / 2.0 + 1.44, -d / 2.0 + 0.4 + steps * 0.26, 3.0), render_mat, rot=(-0.87, 0.0, 0.0)))
    _join("TC_ENV_StoneHouse_V1", parts)


def build_stair_terrace() -> None:
    """Stepped stone terrace: the map's readable vertical element."""
    stone_mat = pbr("TC_MAT_TerraceStone", CORAL_STONE, rough=0.94)
    render_mat = pbr("TC_MAT_TerraceRender", LIME_WASH, rough=0.92)
    shadow_mat = pbr("TC_MAT_TerraceShadow", CORAL_SHADOW, rough=0.95)
    parts: list[bpy.types.Object] = []
    # Platform the steps rise to.
    parts.append(_box("platform", (6.0, 5.0, 1.9), (0.0, 1.6, 0.95), render_mat))
    parts.append(_box("platform_cap", (6.3, 5.3, 0.2), (0.0, 1.6, 1.98), stone_mat))
    steps = 6
    for i in range(steps):
        width = 4.4 - i * 0.18
        parts.append(_box(f"step_{i}", (width, 0.62, 0.32),
                          (0.0, -1.2 - i * 0.62, 1.74 - i * 0.32), stone_mat))
    # Flanking cheek walls give the stair a silhouette instead of a stack.
    for i, sign in enumerate((-1.0, 1.0)):
        parts.append(_box(f"cheek_{i}", (0.4, 4.2, 1.7), (sign * 2.4, -2.3, 0.85), render_mat, rot=(0.28, 0.0, 0.0)))
        parts.append(_box(f"cheek_cap_{i}", (0.52, 4.2, 0.16), (sign * 2.4, -2.3, 1.72), shadow_mat, rot=(0.28, 0.0, 0.0)))
    # Low parapet along the back of the platform.
    parts.append(_box("back_parapet", (6.0, 0.34, 0.9), (0.0, 3.9, 2.35), render_mat))
    parts.append(_box("back_parapet_cap", (6.2, 0.5, 0.14), (0.0, 3.9, 2.87), stone_mat))
    _join("TC_ENV_StairTerrace_V1", parts)


def build_seawall_run() -> None:
    """Seawall coping with mooring bollards: the water edge of the quarter."""
    stone_mat = pbr("TC_MAT_SeawallStone", CORAL_STONE, rough=0.95)
    shadow_mat = pbr("TC_MAT_SeawallWet", CORAL_SHADOW, rough=0.72)
    steel_mat = pbr("TC_MAT_SeawallBollard", WORN_STEEL, rough=0.7, metal=0.4)
    parts: list[bpy.types.Object] = []
    length = 10.0
    parts.append(_box("wall", (length, 1.1, 1.5), (0.0, 0.0, 0.75), stone_mat))
    parts.append(_box("coping", (length + 0.3, 1.4, 0.24), (0.0, 0.0, 1.62), shadow_mat))
    # Tide line: the darker band that sells the wall as a water edge.
    parts.append(_box("tide_band", (length + 0.04, 1.16, 0.4), (0.0, 0.0, 0.3), shadow_mat))
    for i in range(4):
        x = -length / 2.0 + 1.6 + i * 2.2
        parts.append(_cyl(f"bollard_{i}", 0.17, 0.75, (x, 0.0, 2.1), steel_mat, verts=8))
        parts.append(_cyl(f"bollard_cap_{i}", 0.24, 0.14, (x, 0.0, 2.52), steel_mat, verts=8))
    # Two eroded gaps where the sea has taken the facing stone.
    for i, (x, w) in enumerate([(-2.6, 1.3), (3.1, 0.9)]):
        parts.append(_box(f"erosion_{i}", (w, 1.2, 0.55), (x, 0.0, 0.62), shadow_mat))
    _join("TC_ENV_SeawallRun_V1", parts)


def build_market_stall() -> None:
    """Shaded stall: a small, human-scale silhouette for street dressing."""
    wood_mat = pbr("TC_MAT_StallTimber", BLEACHED_TIMBER, rough=0.8)
    cloth_mat = pbr("TC_MAT_StallCloth", TERRACOTTA, rough=0.95)
    basket_mat = pbr("TC_MAT_StallBasket", (0.482353, 0.376471, 0.219608), rough=0.9)
    stone_mat = pbr("TC_MAT_StallStone", CORAL_STONE, rough=0.94)
    parts: list[bpy.types.Object] = []
    for i, (x, y) in enumerate([(-1.3, -0.9), (1.3, -0.9), (-1.3, 0.9), (1.3, 0.9)]):
        h = 2.3 if y < 0 else 2.0
        parts.append(_cyl(f"post_{i}", 0.07, h, (x, y, h / 2.0), wood_mat, verts=6))
    slope = math.atan2(0.3, 1.8)
    parts.append(_box("canopy", (3.1, 2.1, 0.07), (0.0, 0.0, 2.18), cloth_mat, rot=(slope, 0.0, 0.0)))
    parts.append(_box("valance", (3.1, 0.05, 0.26), (0.0, -1.02, 2.2), cloth_mat))
    parts.append(_box("counter", (2.7, 0.8, 0.12), (0.0, -0.3, 0.92), wood_mat))
    parts.append(_box("counter_leg_l", (0.14, 0.7, 0.9), (-1.2, -0.3, 0.45), wood_mat))
    parts.append(_box("counter_leg_r", (0.14, 0.7, 0.9), (1.2, -0.3, 0.45), wood_mat))
    for i, (x, y, r, h) in enumerate([(-0.9, 0.5, 0.34, 0.4), (0.0, 0.62, 0.28, 0.32), (0.95, 0.45, 0.3, 0.46)]):
        parts.append(_cyl(f"basket_{i}", r, h, (x, y, h / 2.0), basket_mat, verts=8))
    parts.append(_box("stone_step", (2.4, 0.5, 0.16), (0.0, -1.35, 0.08), stone_mat))
    _join("TC_ENV_MarketStall_V1", parts)


def build_palm_cluster() -> None:
    """Two coastal palms of different heights, for skyline break-up."""
    trunk_mat = pbr("TC_MAT_PalmTrunk", (0.376471, 0.325490, 0.243137), rough=0.92)
    frond_mat = pbr("TC_MAT_PalmFrond", PALM_FROND, rough=0.88)
    parts: list[bpy.types.Object] = []
    for palm, (ox, oy, height, lean) in enumerate([(-1.1, 0.0, 6.2, 0.12), (1.3, 0.8, 4.6, -0.16)]):
        segments = 6
        seg_h = height / segments
        for i in range(segments):
            t = i / segments
            # A palm trunk bends; each segment steps further along the lean.
            x = ox + lean * (t ** 1.6) * height * 0.45
            radius = 0.22 - 0.07 * t
            parts.append(_cyl(f"p{palm}_trunk_{i}", radius, seg_h * 1.06,
                              (x, oy, seg_h * (i + 0.5)), trunk_mat,
                              rot=(0.0, lean * 0.9 * t, 0.0), verts=6))
        crown_x = ox + lean * height * 0.45
        crown_z = height
        # Each frond is three shortening segments whose droop increases along
        # its length. A palm frond hangs in an arc; a single flat blade reads
        # as a cardboard star, which is what the first review render showed.
        for f in range(7):
            angle = f * (2.0 * math.pi / 7.0) + (0.22 if palm else 0.0)
            reach = 0.0
            for seg, (seg_len, width, droop) in enumerate(
                    ((1.1, 0.34, 0.18), (1.0, 0.26, 0.62), (0.85, 0.16, 1.15))):
                mid = reach + seg_len / 2.0
                horizontal = mid * math.cos(droop)
                parts.append(_box(f"p{palm}_frond_{f}_{seg}", (seg_len, width, 0.045),
                                  (crown_x + math.cos(angle) * horizontal,
                                   oy + math.sin(angle) * horizontal,
                                   crown_z - 0.12 - mid * math.sin(droop) * 0.85),
                                  frond_mat, rot=(0.0, droop, angle)))
                reach += seg_len * 0.88
        parts.append(_cyl(f"p{palm}_crown", 0.26, 0.4, (crown_x, oy, crown_z - 0.1), trunk_mat, verts=6))
    _join("TC_ENV_PalmCluster_V1", parts)


def build_coral_rubble() -> None:
    """Collapsed coral-stone rubble: route edging and cover silhouette."""
    stone_mat = pbr("TC_MAT_RubbleStone", CORAL_STONE, rough=0.96)
    shadow_mat = pbr("TC_MAT_RubbleShadow", CORAL_SHADOW, rough=0.97)
    render_mat = pbr("TC_MAT_RubbleRender", LIME_WASH, rough=0.93)
    core_mat = pbr("TC_MAT_RubbleCore", PLASTER_LOSS_CORE, rough=0.97)
    parts: list[bpy.types.Object] = []
    # Deterministic scatter: fixed tuples rather than random, so the asset
    # regenerates byte-comparably for provenance hashing. Weathered stone
    # dominates and lime-rendered faces are the exception -- a collapsed wall
    # shows mostly its broken core, and the first review render read as a pile
    # of clean white boxes precisely because that ratio was inverted. Some of
    # the broken blocks are core_mat: the wall mix is "un melange de basalte, de
    # corail broye et de sable de mer", so a collapsed wall shows ink-black
    # basalt among the pale coral, not one uniform grey.
    blocks = [
        (-1.35, -0.42, 0.26, (1.05, 0.72, 0.52), (0.14, 0.38, 0.35), core_mat),
        (0.15, 0.18, 0.36, (1.25, 0.95, 0.72), (-0.22, 0.12, -0.5), stone_mat),
        (1.42, -0.25, 0.20, (0.82, 0.78, 0.4), (0.34, -0.26, 0.9), core_mat),
        (0.62, -1.15, 0.17, (0.66, 0.6, 0.34), (-0.08, 0.44, 1.5), stone_mat),
        (-0.55, 1.02, 0.25, (0.9, 0.55, 0.5), (0.41, -0.18, -1.1), render_mat),
        (1.05, 1.15, 0.14, (0.52, 0.48, 0.28), (0.19, 0.29, 0.4), shadow_mat),
        (-1.85, 0.72, 0.13, (0.46, 0.42, 0.26), (-0.31, 0.09, 2.2), stone_mat),
        (0.05, 0.05, 0.58, (1.5, 1.3, 0.3), (0.06, -0.09, 0.15), core_mat),
        (2.0, 0.45, 0.11, (0.4, 0.36, 0.22), (0.46, 0.31, 0.7), stone_mat),
        (-0.2, -1.75, 0.10, (0.42, 0.34, 0.2), (-0.27, 0.2, 1.9), shadow_mat),
        # Chips and spall: the small debris that keeps a rubble pile from
        # reading as a stack of crates.
        (0.95, -0.62, 0.07, (0.26, 0.22, 0.13), (0.5, 0.35, 1.2), stone_mat),
        (-0.85, -0.05, 0.08, (0.3, 0.19, 0.15), (-0.42, 0.55, 0.3), core_mat),
        (1.72, 0.92, 0.06, (0.22, 0.2, 0.11), (0.28, -0.4, 2.6), render_mat),
        (-1.15, 1.55, 0.07, (0.25, 0.23, 0.12), (0.6, 0.15, 1.7), stone_mat),
        (0.38, 1.62, 0.06, (0.2, 0.18, 0.1), (-0.35, 0.48, 0.85), shadow_mat),
    ]
    for i, (x, y, z, size, rot, mat) in enumerate(blocks):
        parts.append(_box(f"rubble_{i}", size, (x, y, z), mat, rot=rot))
    # Two protruding timbers from the collapsed roof structure.
    timber_mat = pbr("TC_MAT_RubbleTimber", BLEACHED_TIMBER, rough=0.84)
    parts.append(_box("timber_a", (2.2, 0.16, 0.14), (-0.3, -0.5, 0.72), timber_mat, rot=(0.0, 0.34, 0.5)))
    parts.append(_box("timber_b", (1.7, 0.13, 0.12), (0.8, 0.6, 0.58), timber_mat, rot=(0.0, -0.2, -0.8)))
    _join("TC_ENV_CoralRubble_V1", parts)


ASSETS = {
    "TC_ENV_CoralWallSegment_V1": build_coral_wall_segment,
    "TC_ENV_ArcadeBay_V1": build_arcade_bay,
    "TC_ENV_QuarterTower_V1": build_quarter_tower,
    "TC_ENV_CarvedDoorway_V1": build_carved_doorway,
    "TC_ENV_StoneHouse_V1": build_stone_house,
    "TC_ENV_StairTerrace_V1": build_stair_terrace,
    "TC_ENV_SeawallRun_V1": build_seawall_run,
    "TC_ENV_MarketStall_V1": build_market_stall,
    "TC_ENV_PalmCluster_V1": build_palm_cluster,
    "TC_ENV_CoralRubble_V1": build_coral_rubble,
}


def main() -> None:
    only = None
    if "--" in sys.argv:
        tail = sys.argv[sys.argv.index("--") + 1:]
        if len(tail) == 2 and tail[0] == "--asset":
            only = tail[1]
        elif tail:
            raise SystemExit("usage: blender --background --python create_badjanani_district_kit_v1.py [-- --asset TC_NAME]")
    names = [only] if only else list(ASSETS)
    for name in names:
        if name not in ASSETS:
            raise SystemExit(f"unknown asset: {name}")
        reset_scene()
        ASSETS[name]()
        output = SOURCE_DIR / f"{name}.blend"
        output.parent.mkdir(parents=True, exist_ok=True)
        bpy.ops.wm.save_as_mainfile(filepath=str(output))
        print(f"DISTRICT_KIT_BLEND_WRITTEN {output}")


if __name__ == "__main__":
    main()
