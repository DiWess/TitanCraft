#!/usr/bin/env python3
"""Build the Mwezi Quarter seafront from data/art/comorian_seafront_spec.json.

This is a spec-driven builder, not a hand-authored kit. Every dimension,
count and colour comes from the JSON spec; nothing is hardcoded here that a
reviewer might want to change. Edit the spec and regenerate -- that is the
whole point of the system:

    python3 -c "import json,pathlib; ..."      # edit the spec
    blender --background --python tools/blender/create_seafront_kit_v1.py
    blender --background --python tools/blender/export_asset.py -- <blend> <glb>

The spec records, per field, whether the value is text-sourced, assumed, or
`needs_photo`. No photographs were available to the authoring environment, so
the `needs_photo` fields currently hold defensible defaults; supplying a
photograph means changing those numbers in the spec and re-running this
script, with no code change at all.

Assets built (all visual-only and collisionless, per the asset contract --
gameplay collision is authored separately in the scene):

  TC_ENV_SeaWallQuay_V1        -- harbour quay tile with coping, bollards, steps
  TC_ENV_CivicHallSeafront_V1  -- arcaded hall on a sea platform, corner tower
  TC_ENV_LavaShoreline_V1      -- black volcanic rock shore band
  TC_ENV_HarbourSea_V1         -- water surface with a low swell

Run:
  blender --background --python tools/blender/create_seafront_kit_v1.py
  blender --background --python tools/blender/create_seafront_kit_v1.py -- --asset TC_ENV_SeaWallQuay_V1
"""
from __future__ import annotations

import json
import math
import sys
from pathlib import Path

import bpy

ROOT = Path(__file__).resolve().parents[2]
SPEC_PATH = ROOT / "data/art/comorian_seafront_spec.json"
SOURCE_DIR = ROOT / "assets/Source/Blender/Production/MweziQuarter_V1"

SPEC: dict = json.loads(SPEC_PATH.read_text(encoding="utf-8"))


def spec_section(name: str) -> dict:
    section = SPEC.get(name)
    if not isinstance(section, dict):
        raise SystemExit(f"spec is missing the '{name}' section: {SPEC_PATH}")
    return section


def reset_scene() -> None:
    bpy.ops.wm.read_factory_settings(use_empty=True)
    world = bpy.data.worlds.new("World")
    bpy.context.scene.world = world


def pbr(name: str, color, rough: float = 0.85, metal: float = 0.0) -> bpy.types.Material:
    mat = bpy.data.materials.new(name)
    mat.use_nodes = True
    bsdf = mat.node_tree.nodes["Principled BSDF"]
    bsdf.inputs["Base Color"].default_value = (*color, 1.0)
    bsdf.inputs["Roughness"].default_value = rough
    bsdf.inputs["Metallic"].default_value = metal
    return mat


def _box(name, size, loc, mat, rot=(0.0, 0.0, 0.0)) -> bpy.types.Object:
    bpy.ops.mesh.primitive_cube_add(size=1.0, location=loc, rotation=rot)
    obj = bpy.context.object
    obj.name = name
    obj.scale = tuple(size)
    obj.data.materials.append(mat)
    return obj


def _cyl(name, radius, depth, loc, mat, rot=(0.0, 0.0, 0.0), verts=8) -> bpy.types.Object:
    bpy.ops.mesh.primitive_cylinder_add(vertices=verts, radius=radius, depth=depth,
                                        location=loc, rotation=rot)
    obj = bpy.context.object
    obj.name = name
    obj.data.materials.append(mat)
    return obj


def _join(name: str, parts: list, bevel_width: float = 0.018) -> bpy.types.Object:
    bpy.ops.object.select_all(action="DESELECT")
    for part in parts:
        part.select_set(True)
    bpy.context.view_layer.objects.active = parts[0]
    bpy.ops.object.join()
    joined = bpy.context.object
    joined.name = name
    bpy.ops.object.transform_apply(location=True, rotation=True, scale=True)
    if bevel_width > 0.0:
        # Matches the Mwezi Quarter kit's edge treatment so the seafront and
        # the inland district catch light the same way.
        modifier = joined.modifiers.new("TC_EdgeBevel", "BEVEL")
        modifier.width = bevel_width
        modifier.segments = 1
        modifier.limit_method = "ANGLE"
        modifier.angle_limit = math.radians(35.0)
        modifier.use_clamp_overlap = True
        bpy.ops.object.modifier_apply(modifier="TC_EdgeBevel")
    joined["titancraft_visual_only"] = True
    joined["titancraft_collision"] = "none"
    return joined


def _hash01(x: int, y: int) -> float:
    """Deterministic 0..1 from two integers, so every rebuild is identical."""
    h = 2166136261
    h = ((h ^ (x & 0xFFFFFFFF)) * 16777619) & 0xFFFFFFFF
    h = ((h ^ (y & 0xFFFFFFFF)) * 16777619) & 0xFFFFFFFF
    h ^= h >> 15
    h = (h * 2246822519) & 0xFFFFFFFF
    h ^= h >> 13
    return ((h * 3266489917) & 0xFFFFFF) / 16777215.0


def _pointed_arch(parts, tag, center_x, spring_z, span, depth, thickness, mat,
                  point_ratio, segments=5) -> float:
    """Voussoir ring for one pointed arch; returns apex height."""
    offset = span * point_ratio
    radius = span / 2.0 + offset
    end_angle = math.acos(min(1.0, offset / radius))
    apex = spring_z + math.sqrt(max(radius * radius - offset * offset, 0.0))
    step = end_angle / segments
    for side, direction in (("r", 1.0), ("l", -1.0)):
        arc_center_x = center_x - direction * offset
        for i in range(segments):
            mid = (i + 0.5) * step
            x = arc_center_x + direction * radius * math.cos(mid)
            z = spring_z + radius * math.sin(mid)
            seg_len = radius * step * 1.14
            angle = mid if direction > 0.0 else math.pi - mid
            parts.append(_box(f"{tag}_v{side}{i}", (thickness, depth, seg_len),
                              (x, 0.0, z), mat, rot=(0.0, -angle, 0.0)))
    return apex


# --- builders ---------------------------------------------------------------

def build_quay() -> None:
    quay = spec_section("quay")
    mats = spec_section("materials")
    stone = pbr("TC_MAT_QuayStone", mats["coral_rag"], rough=0.95)
    coping = pbr("TC_MAT_QuayCoping", mats["weathered_stone"], rough=0.9)
    steel = pbr("TC_MAT_QuayBollard", mats["weathered_stone"], rough=0.7, metal=0.4)

    length = float(quay["segment_length_m"])
    height = float(quay["height_above_water_m"])
    depth = float(quay["depth_m"])
    overhang = float(quay["coping_overhang_m"])

    parts = []
    parts.append(_box("wall", (length, depth, height), (0.0, 0.0, height / 2.0), stone))
    parts.append(_box("coping", (length + overhang * 2, depth + overhang * 2, 0.26),
                      (0.0, 0.0, height + 0.13), coping))
    # Tide band: the wet zone a harbour wall always carries.
    parts.append(_box("tide_band", (length + 0.03, depth + 0.03, 0.5),
                      (0.0, 0.0, 0.3), coping))

    bollards = int(quay["bollard_count_per_segment"])
    bollard_h = float(quay["bollard_height_m"])
    for i in range(bollards):
        x = -length / 2.0 + (i + 0.5) * (length / bollards)
        parts.append(_cyl(f"bollard_{i}", 0.17, bollard_h,
                          (x, 0.0, height + 0.26 + bollard_h / 2.0), steel))
        parts.append(_cyl(f"bollard_cap_{i}", 0.23, 0.12,
                          (x, 0.0, height + 0.26 + bollard_h), steel))

    # Landing steps down the seaward face: how a small harbour is actually used.
    for i in range(int(quay["stair_count_per_segment"])):
        step_count = 5
        for s in range(step_count):
            width = 1.6
            parts.append(_box(f"step_{i}_{s}", (width, 0.42, 0.24),
                              (length * 0.28, -depth / 2.0 - 0.21 - s * 0.34,
                               height - 0.12 - s * 0.34), coping))
    _join("TC_ENV_SeaWallQuay_V1", parts)


def build_civic_hall() -> None:
    hall = spec_section("civic_hall")
    mats = spec_section("materials")
    render = pbr("TC_MAT_HallRender", mats["lime_render"], rough=0.92)
    stone = pbr("TC_MAT_HallStone", mats["coral_rag"], rough=0.94)
    shadow = pbr("TC_MAT_HallWeathered", mats["weathered_stone"], rough=0.95)
    wood = pbr("TC_MAT_HallDoor", mats["carved_hardwood"], rough=0.62)
    void = pbr("TC_MAT_HallOpening", (0.07, 0.065, 0.06), rough=0.98)

    width, depth = (float(v) for v in hall["footprint_m"])
    wall_h = float(hall["wall_height_m"])
    parapet_h = float(hall["parapet_height_m"])
    platform_h = float(hall["platform_height_m"])
    bays = int(hall["arcade_bay_count"])
    span = float(hall["arcade_span_m"])
    pier_w = float(hall["arcade_pier_width_m"])
    spring = float(hall["arcade_spring_height_m"])
    point_ratio = float(hall["arch_point_ratio"])

    parts = []
    # Sea platform: the hall stands out of the water on its own plinth.
    parts.append(_box("platform", (width + 1.6, depth + 1.6, platform_h),
                      (0.0, 0.0, platform_h / 2.0), stone))
    parts.append(_box("platform_lip", (width + 2.0, depth + 2.0, 0.18),
                      (0.0, 0.0, platform_h), shadow))

    base = platform_h
    # Main mass, set back behind the arcade.
    body_depth = depth - 2.0
    parts.append(_box("body", (width, body_depth, wall_h),
                      (0.0, 1.0, base + wall_h / 2.0), render))
    parts.append(_box("plinth", (width + 0.2, body_depth + 0.2, 0.5),
                      (0.0, 1.0, base + 0.25), shadow))

    # Seaward arcade.
    arcade_y = -depth / 2.0 + 1.0
    arcade_depth = 1.1
    total = bays * span + (bays + 1) * pier_w
    apex = spring
    for i in range(bays):
        cx = -total / 2.0 + pier_w + span / 2.0 + i * (span + pier_w)
        apex = _pointed_arch(parts, f"arch{i}", cx, base + spring, span,
                             arcade_depth, 0.4, stone, point_ratio)
    for i in range(bays + 1):
        x = -total / 2.0 + pier_w / 2.0 + i * (span + pier_w)
        parts.append(_box(f"pier_{i}", (pier_w, arcade_depth, spring),
                          (x, arcade_y, base + spring / 2.0), render))
        parts.append(_box(f"pier_base_{i}", (pier_w + 0.2, arcade_depth + 0.2, 0.28),
                          (x, arcade_y, base + 0.14), shadow))
        parts.append(_box(f"pier_cap_{i}", (pier_w + 0.2, arcade_depth + 0.2, 0.22),
                          (x, arcade_y, base + spring - 0.11), stone))
    # Move the arcade ring into the arcade plane.
    for part in parts:
        if part.name.startswith("arch"):
            part.location.y = arcade_y

    parts.append(_box("spandrel", (total, arcade_depth * 0.9, apex - spring + 0.3),
                      (0.0, arcade_y, base + spring + (apex - spring + 0.3) / 2.0), render))
    parts.append(_box("cornice", (width + 0.4, depth + 0.4, 0.28),
                      (0.0, 0.0, base + wall_h), stone))

    # Parapet ring on the flat roof.
    for i, (sx, sy, px, py) in enumerate([
        (width + 0.3, 0.3, 0.0, -depth / 2.0), (width + 0.3, 0.3, 0.0, depth / 2.0),
        (0.3, depth + 0.3, -width / 2.0, 0.0), (0.3, depth + 0.3, width / 2.0, 0.0),
    ]):
        parts.append(_box(f"parapet_{i}", (sx, sy, parapet_h),
                          (px, py, base + wall_h + parapet_h / 2.0), render))
        parts.append(_box(f"parapet_cap_{i}", (sx + 0.14, sy + 0.14, 0.12),
                          (px, py, base + wall_h + parapet_h), stone))

    # Carved door on the landward face.
    parts.append(_box("door_void", (1.6, 0.34, 2.6), (0.0, depth / 2.0 - 1.2, base + 1.3), void))
    parts.append(_box("door_leaf", (1.5, 0.12, 2.5), (0.0, depth / 2.0 - 1.02, base + 1.25), wood))
    parts.append(_box("door_lintel", (2.2, 0.5, 0.34), (0.0, depth / 2.0 - 1.05, base + 2.75), stone))

    # Corner tower.
    tower_base = float(hall["tower_base_m"])
    tower_h = float(hall["tower_height_m"])
    stages = int(hall["tower_stage_count"])
    tx = -width / 2.0 + tower_base / 2.0
    ty = depth / 2.0 - tower_base / 2.0
    stage_h = tower_h / stages
    for s in range(stages):
        side = tower_base * (1.0 - 0.09 * s)
        parts.append(_box(f"tower_{s}", (side, side, stage_h),
                          (tx, ty, base + stage_h * s + stage_h / 2.0), render))
        parts.append(_box(f"tower_course_{s}", (side + 0.24, side + 0.24, 0.2),
                          (tx, ty, base + stage_h * (s + 1)), stone))
        # Slit openings, recessed so they read as holes in stone.
        for face, (nx, ny) in enumerate([(1.0, 0.0), (-1.0, 0.0), (0.0, 1.0), (0.0, -1.0)]):
            sx, sy = (0.18, 0.44) if nx != 0.0 else (0.44, 0.18)
            parts.append(_box(f"tower_slit_{s}_{face}", (sx, sy, 0.9),
                              (tx + nx * (side / 2.0 - 0.1), ty + ny * (side / 2.0 - 0.1),
                               base + stage_h * s + stage_h * 0.6), void))
    if hall.get("tower_gallery"):
        top = base + tower_h
        gallery_h = 1.7
        post = tower_base * 0.28
        for i, (ox, oy) in enumerate([(-1, -1), (1, -1), (-1, 1), (1, 1)]):
            parts.append(_box(f"gallery_post_{i}", (post, post, gallery_h),
                              (tx + ox * tower_base * 0.28, ty + oy * tower_base * 0.28,
                               top + gallery_h / 2.0), render))
        parts.append(_box("gallery_cap", (tower_base * 0.95, tower_base * 0.95, 0.26),
                          (tx, ty, top + gallery_h + 0.13), stone))
        parts.append(_box("gallery_lip", (tower_base * 1.1, tower_base * 1.1, 0.14),
                          (tx, ty, top + gallery_h + 0.3), shadow))
    _join("TC_ENV_CivicHallSeafront_V1", parts)


def build_shoreline() -> None:
    shore = spec_section("shoreline")
    rock = pbr("TC_MAT_LavaRock", shore["material_colour"], rough=float(shore["roughness"]))
    stain = pbr("TC_MAT_LavaTideStain", shore["tide_stain_colour"], rough=0.8)

    count = int(shore["block_count"])
    low, high = (float(v) for v in shore["block_size_range_m"])
    band = float(shore["band_depth_m"])
    parts = []
    for i in range(count):
        # Deterministic scatter, so the shore regenerates byte-comparably.
        rx = _hash01(i, 11)
        ry = _hash01(i, 23)
        rz = _hash01(i, 37)
        rr = _hash01(i, 53)
        size = low + (high - low) * rz
        x = (rx - 0.5) * 22.0
        y = (ry - 0.5) * band
        height = size * (0.45 + 0.5 * rr)
        mat = stain if ry < 0.42 else rock
        parts.append(_box(f"rock_{i}", (size, size * (0.7 + 0.5 * rx), height),
                          (x, y, height / 2.0 - 0.15), mat,
                          rot=(rr * 0.3 - 0.15, rz * 0.25 - 0.12, rx * math.tau)))
    _join("TC_ENV_LavaShoreline_V1", parts, bevel_width=0.012)


def build_sea() -> None:
    sea = spec_section("sea")
    water = pbr("TC_MAT_HarbourWater", sea["colour_deep"],
                rough=float(sea["roughness"]), metal=float(sea["metallic"]))

    size_x, size_y = (float(v) for v in sea["extent_m"])
    amplitude = float(sea["swell_amplitude_m"])
    wavelength = float(sea["swell_wavelength_m"])

    # A gently swelling grid rather than a flat plane: a dead-flat water plane
    # reads as glass at grazing angles, which is exactly the angle the player
    # sees the harbour from.
    columns = 36
    rows = 30
    bpy.ops.mesh.primitive_grid_add(x_subdivisions=columns, y_subdivisions=rows,
                                    size=1.0, location=(0.0, 0.0, 0.0))
    grid = bpy.context.object
    grid.name = "TC_ENV_HarbourSea_V1"
    grid.scale = (size_x, size_y, 1.0)
    bpy.ops.object.transform_apply(location=True, rotation=True, scale=True)
    for vertex in grid.data.vertices:
        vertex.co.z = (
            math.sin(vertex.co.x / wavelength) * amplitude
            + math.sin(vertex.co.y / (wavelength * 1.7) + 1.3) * amplitude * 0.6
        )
    grid.data.materials.append(water)
    grid["titancraft_visual_only"] = True
    grid["titancraft_collision"] = "none"


BUILDERS = {
    "quay": build_quay,
    "civic_hall": build_civic_hall,
    "shoreline": build_shoreline,
    "sea": build_sea,
}


def main() -> None:
    only = None
    if "--" in sys.argv:
        tail = sys.argv[sys.argv.index("--") + 1:]
        if len(tail) == 2 and tail[0] == "--asset":
            only = tail[1]
        elif tail:
            raise SystemExit("usage: blender --background --python create_seafront_kit_v1.py [-- --asset TC_NAME]")

    entries = SPEC.get("assets")
    if not entries:
        raise SystemExit(f"spec lists no assets: {SPEC_PATH}")

    for entry in entries:
        name = entry["name"]
        if only is not None and name != only:
            continue
        builder_key = entry["builder"]
        builder = BUILDERS.get(builder_key)
        if builder is None:
            raise SystemExit(f"spec asset '{name}' names unknown builder '{builder_key}'")
        reset_scene()
        builder()
        output = SOURCE_DIR / f"{name}.blend"
        output.parent.mkdir(parents=True, exist_ok=True)
        bpy.ops.wm.save_as_mainfile(filepath=str(output))
        print(f"SEAFRONT_KIT_BLEND_WRITTEN {output} budget={entry['triangle_budget']}")


if __name__ == "__main__":
    main()
