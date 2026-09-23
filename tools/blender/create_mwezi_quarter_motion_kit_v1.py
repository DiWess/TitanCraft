#!/usr/bin/env python3
"""Create the animated TC_ENV Mwezi Quarter Motion Kit V1 Blender sources.

Why skeletal animation rather than moving child objects
-------------------------------------------------------
`tools/blender/validate_blender_asset.py` requires every mesh object to sit at
a clean origin. A cloth or frond animated as a separate child object sits at
its pivot, not at world zero, so that route fails the asset contract outright.
An armature keeps one mesh at the origin and drives the motion through bones,
which satisfies the contract, exports cleanly to glTF, and gives Godot a real
AnimationPlayer to run.

Each asset carries a single looping action. `src/World/EnvironmentMotionPlayer.cs`
starts it with a per-instance phase offset so a street of them does not sway in
lockstep.

Assets:

- TC_ENV_PalmSway_V1     -- palm whose trunk and crown bend on a slow cycle
- TC_ENV_LaundryLine_V1  -- washing strung between two poles, each sheet swinging
- TC_ENV_AwningCloth_V1  -- wall awning whose front edge lifts and settles
- TC_ENV_BannerCloth_V1  -- hanging cloth banner swinging from a bracket

Run with Blender:
  blender --background --python tools/blender/create_mwezi_quarter_motion_kit_v1.py
  blender --background --python tools/blender/create_mwezi_quarter_motion_kit_v1.py -- --asset TC_ENV_BannerCloth_V1
"""
from __future__ import annotations

import math
import sys
from pathlib import Path

import bpy

ROOT = Path(__file__).resolve().parents[2]
SOURCE_DIR = ROOT / "assets/Source/Blender/Production/MweziQuarter_V1"

# Shared with the static district kit so the two read as one quarter.
LIME_WASH = (0.858824, 0.831373, 0.760784)
CORAL_STONE = (0.560784, 0.509804, 0.423529)
HARDWOOD = (0.223529, 0.145098, 0.086275)
TERRACOTTA = (0.615686, 0.325490, 0.211765)
PALM_FROND = (0.278431, 0.341176, 0.203922)
PALM_TRUNK = (0.376471, 0.325490, 0.243137)
CLOTH_WHITE = (0.827451, 0.811765, 0.768627)
CLOTH_BLUE = (0.396078, 0.478431, 0.541176)
CLOTH_OCHRE = (0.729412, 0.564706, 0.313726)
WORN_STEEL = (0.431373, 0.454902, 0.470588)

# 24 fps, 120 frames: a 5 second loop, long enough not to read as a repeat.
FRAME_RATE = 24
LOOP_FRAMES = 120


def reset_scene() -> None:
    bpy.ops.wm.read_factory_settings(use_empty=True)
    world = bpy.data.worlds.new("World")
    bpy.context.scene.world = world
    bpy.context.scene.render.fps = FRAME_RATE
    bpy.context.scene.frame_start = 1
    bpy.context.scene.frame_end = LOOP_FRAMES


def pbr(name: str, color: tuple[float, float, float], rough: float = 0.85,
        metal: float = 0.0) -> bpy.types.Material:
    mat = bpy.data.materials.new(name)
    mat.use_nodes = True
    bsdf = mat.node_tree.nodes["Principled BSDF"]
    bsdf.inputs["Base Color"].default_value = (*color, 1.0)
    bsdf.inputs["Roughness"].default_value = rough
    bsdf.inputs["Metallic"].default_value = metal
    return mat


def _box(name: str, size: tuple[float, float, float], loc: tuple[float, float, float],
         mat: bpy.types.Material, rot: tuple[float, float, float] = (0.0, 0.0, 0.0)) -> bpy.types.Object:
    bpy.ops.mesh.primitive_cube_add(size=1.0, location=loc, rotation=rot)
    obj = bpy.context.object
    obj.name = name
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


def _join(name: str, parts: list[bpy.types.Object], bevel_width: float = 0.014) -> bpy.types.Object:
    bpy.ops.object.select_all(action="DESELECT")
    for part in parts:
        part.select_set(True)
    bpy.context.view_layer.objects.active = parts[0]
    bpy.ops.object.join()
    joined = bpy.context.object
    joined.name = name
    bpy.ops.object.transform_apply(location=True, rotation=True, scale=True)
    if bevel_width > 0.0:
        # Same edge treatment as the static district kit, so a swaying palm and
        # the wall behind it catch light the same way. Applied BEFORE skinning:
        # a bevel modifier stacked under an armature modifier would be stripped
        # by the animated exporter, which cannot apply modifiers.
        modifier = joined.modifiers.new("TC_EdgeBevel", "BEVEL")
        modifier.width = bevel_width
        modifier.segments = 1
        modifier.limit_method = "ANGLE"
        modifier.angle_limit = math.radians(35.0)
        modifier.use_clamp_overlap = True
        modifier.harden_normals = False
        bpy.ops.object.modifier_apply(modifier="TC_EdgeBevel")
    joined["titancraft_visual_only"] = True
    joined["titancraft_collision"] = "none"
    return joined


def _build_armature(name: str, bones: list[tuple[str, tuple[float, float, float],
                                                 tuple[float, float, float], str | None]]
                    ) -> bpy.types.Object:
    """Create an armature from (bone name, head, tail, parent name) tuples."""
    bpy.ops.object.armature_add(enter_editmode=False, location=(0.0, 0.0, 0.0))
    armature = bpy.context.object
    armature.name = name
    armature.data.name = f"{name}_Data"
    bpy.ops.object.mode_set(mode="EDIT")
    edit_bones = armature.data.edit_bones
    # armature_add seeds a default bone that would otherwise deform the mesh.
    for existing in list(edit_bones):
        edit_bones.remove(existing)
    for bone_name, head, tail, parent_name in bones:
        bone = edit_bones.new(bone_name)
        bone.head = head
        bone.tail = tail
        if parent_name is not None:
            bone.parent = edit_bones[parent_name]
            bone.use_connect = False
    bpy.ops.object.mode_set(mode="OBJECT")
    return armature


def _skin(mesh: bpy.types.Object, armature: bpy.types.Object) -> None:
    """Bind the mesh to the armature with automatic weights."""
    bpy.ops.object.select_all(action="DESELECT")
    mesh.select_set(True)
    armature.select_set(True)
    bpy.context.view_layer.objects.active = armature
    bpy.ops.object.parent_set(type="ARMATURE_AUTO")


def _keyframe_loop(armature: bpy.types.Object,
                   tracks: dict[str, list[tuple[int, tuple[float, float, float]]]]) -> None:
    """Keyframe bone euler rotations.

    Every track must repeat its frame-1 pose on the final frame, otherwise the
    loop visibly snaps when Godot wraps the animation.
    """
    bpy.ops.object.select_all(action="DESELECT")
    armature.select_set(True)
    bpy.context.view_layer.objects.active = armature
    bpy.ops.object.mode_set(mode="POSE")
    for bone_name, keys in tracks.items():
        pose_bone = armature.pose.bones[bone_name]
        pose_bone.rotation_mode = "XYZ"
        first = keys[0][1]
        last = keys[-1][1]
        if keys[0][0] != 1 or keys[-1][0] != LOOP_FRAMES or first != last:
            raise SystemExit(
                f"{armature.name}/{bone_name}: track must start at frame 1 and end at "
                f"frame {LOOP_FRAMES} on the same pose, or the loop will snap"
            )
        for frame, rotation in keys:
            bpy.context.scene.frame_set(frame)
            pose_bone.rotation_euler = rotation
            pose_bone.keyframe_insert(data_path="rotation_euler", frame=frame)
    bpy.ops.object.mode_set(mode="OBJECT")
    # Smooth interpolation everywhere: cloth and fronds ease, they do not tick.
    for action in bpy.data.actions:
        for fcurve in action.fcurves:
            for point in fcurve.keyframe_points:
                point.interpolation = "BEZIER"


def build_palm_sway() -> None:
    """A palm bending through a slow cycle, trunk base to crown."""
    trunk_mat = pbr("TC_MAT_MotionPalmTrunk", PALM_TRUNK, rough=0.92)
    frond_mat = pbr("TC_MAT_MotionPalmFrond", PALM_FROND, rough=0.88)
    parts: list[bpy.types.Object] = []
    height = 5.8
    segments = 6
    seg_h = height / segments
    for i in range(segments):
        radius = 0.21 - 0.06 * (i / segments)
        parts.append(_cyl(f"trunk_{i}", radius, seg_h * 1.06, (0.0, 0.0, seg_h * (i + 0.5)),
                          trunk_mat, verts=6))
    parts.append(_cyl("crown", 0.25, 0.38, (0.0, 0.0, height - 0.08), trunk_mat, verts=6))
    for f in range(7):
        angle = f * (2.0 * math.pi / 7.0)
        reach = 0.0
        for seg, (seg_len, width, droop) in enumerate(
                ((1.05, 0.32, 0.2), (0.95, 0.24, 0.64), (0.8, 0.15, 1.18))):
            mid = reach + seg_len / 2.0
            horizontal = mid * math.cos(droop)
            parts.append(_box(f"frond_{f}_{seg}", (seg_len, width, 0.045),
                              (math.cos(angle) * horizontal, math.sin(angle) * horizontal,
                               height - 0.1 - mid * math.sin(droop) * 0.85),
                              frond_mat, rot=(0.0, droop, angle)))
            reach += seg_len * 0.88
    mesh = _join("TC_ENV_PalmSway_V1", parts)

    armature = _build_armature("TC_RIG_PalmSway", [
        ("trunk_lower", (0.0, 0.0, 0.0), (0.0, 0.0, height * 0.45), None),
        ("trunk_upper", (0.0, 0.0, height * 0.45), (0.0, 0.0, height * 0.85), "trunk_lower"),
        ("crown", (0.0, 0.0, height * 0.85), (0.0, 0.0, height + 1.4), "trunk_upper"),
    ])
    _skin(mesh, armature)
    # A gust arrives, holds, and releases: the lower trunk barely moves, the
    # crown carries most of the travel, which is how a palm actually reads.
    _keyframe_loop(armature, {
        "trunk_lower": [(1, (0.0, 0.0, 0.0)), (40, (0.012, 0.02, 0.0)),
                        (80, (-0.008, -0.014, 0.0)), (LOOP_FRAMES, (0.0, 0.0, 0.0))],
        "trunk_upper": [(1, (0.0, 0.0, 0.0)), (35, (0.03, 0.045, 0.0)),
                        (75, (-0.022, -0.03, 0.0)), (LOOP_FRAMES, (0.0, 0.0, 0.0))],
        "crown": [(1, (0.0, 0.0, 0.0)), (30, (0.055, 0.075, 0.02)),
                  (70, (-0.04, -0.05, -0.015)), (LOOP_FRAMES, (0.0, 0.0, 0.0))],
    })


def build_laundry_line() -> None:
    """Washing on a line between two poles, each sheet swinging on its own beat."""
    pole_mat = pbr("TC_MAT_MotionLaundryPole", HARDWOOD, rough=0.72)
    rope_mat = pbr("TC_MAT_MotionLaundryRope", (0.482353, 0.431373, 0.337255), rough=0.95)
    cloths = [
        pbr("TC_MAT_MotionClothWhite", CLOTH_WHITE, rough=0.95),
        pbr("TC_MAT_MotionClothBlue", CLOTH_BLUE, rough=0.95),
        pbr("TC_MAT_MotionClothOchre", CLOTH_OCHRE, rough=0.95),
    ]
    parts: list[bpy.types.Object] = []
    span = 4.6
    line_z = 2.5
    for i, x in enumerate((-span / 2.0, span / 2.0)):
        parts.append(_cyl(f"pole_{i}", 0.06, line_z + 0.2, (x, 0.0, (line_z + 0.2) / 2.0),
                          pole_mat, verts=6))
    parts.append(_cyl("rope", 0.02, span, (0.0, 0.0, line_z), rope_mat,
                      rot=(0.0, math.pi / 2.0, 0.0), verts=5))
    sheets = [(-1.55, 0.95, 1.1), (-0.15, 0.8, 0.85), (1.35, 1.0, 1.25)]
    for i, (x, width, drop) in enumerate(sheets):
        parts.append(_box(f"sheet_{i}", (width, 0.03, drop), (x, 0.0, line_z - drop / 2.0),
                          cloths[i % len(cloths)]))
        parts.append(_box(f"peg_{i}", (0.06, 0.07, 0.1), (x - width / 2.0 + 0.08, 0.0, line_z),
                          pole_mat))
    mesh = _join("TC_ENV_LaundryLine_V1", parts)

    armature = _build_armature("TC_RIG_LaundryLine", [
        ("line", (0.0, 0.0, line_z), (0.0, 0.0, line_z + 0.4), None),
    ] + [
        (f"sheet_{i}", (x, 0.0, line_z), (x, 0.0, line_z - drop), "line")
        for i, (x, _width, drop) in enumerate(sheets)
    ])
    _skin(mesh, armature)
    # Offset beats per sheet: washing on one line never swings in unison.
    _keyframe_loop(armature, {
        "line": [(1, (0.0, 0.0, 0.0)), (LOOP_FRAMES, (0.0, 0.0, 0.0))],
        "sheet_0": [(1, (0.0, 0.0, 0.0)), (28, (0.12, 0.0, 0.05)),
                    (64, (-0.09, 0.0, -0.04)), (92, (0.05, 0.0, 0.02)),
                    (LOOP_FRAMES, (0.0, 0.0, 0.0))],
        "sheet_1": [(1, (0.0, 0.0, 0.0)), (44, (0.15, 0.0, -0.06)),
                    (82, (-0.07, 0.0, 0.03)), (LOOP_FRAMES, (0.0, 0.0, 0.0))],
        "sheet_2": [(1, (0.0, 0.0, 0.0)), (20, (0.08, 0.0, 0.03)),
                    (58, (-0.13, 0.0, -0.05)), (100, (0.06, 0.0, 0.02)),
                    (LOOP_FRAMES, (0.0, 0.0, 0.0))],
    })


def build_awning_cloth() -> None:
    """A wall awning whose unsecured front edge lifts and settles."""
    frame_mat = pbr("TC_MAT_MotionAwningFrame", HARDWOOD, rough=0.7)
    cloth_mat = pbr("TC_MAT_MotionAwningCloth", TERRACOTTA, rough=0.95)
    stone_mat = pbr("TC_MAT_MotionAwningBracket", CORAL_STONE, rough=0.94)
    parts: list[bpy.types.Object] = []
    width, reach = 3.0, 1.7
    top_z = 2.7
    for i, x in enumerate((-width / 2.0 + 0.1, width / 2.0 - 0.1)):
        parts.append(_box(f"bracket_{i}", (0.16, 0.5, 0.16), (x, 0.22, top_z), stone_mat))
        parts.append(_cyl(f"stay_{i}", 0.035, reach, (x, reach / 2.0, top_z - 0.18), frame_mat,
                          rot=(math.pi / 2.0, 0.0, 0.0), verts=5))
    parts.append(_cyl("front_rail", 0.04, width, (0.0, reach - 0.05, top_z - 0.36), frame_mat,
                      rot=(0.0, math.pi / 2.0, 0.0), verts=5))
    parts.append(_box("cloth", (width, reach, 0.035), (0.0, reach / 2.0 - 0.05, top_z - 0.2),
                      cloth_mat, rot=(math.atan2(0.36, reach), 0.0, 0.0)))
    parts.append(_box("valance", (width, 0.04, 0.3), (0.0, reach - 0.05, top_z - 0.52), cloth_mat))
    mesh = _join("TC_ENV_AwningCloth_V1", parts)

    armature = _build_armature("TC_RIG_AwningCloth", [
        ("root", (0.0, 0.0, top_z), (0.0, 0.3, top_z), None),
        ("edge", (0.0, reach * 0.45, top_z - 0.24), (0.0, reach + 0.3, top_z - 0.5), "root"),
    ])
    _skin(mesh, armature)
    _keyframe_loop(armature, {
        "root": [(1, (0.0, 0.0, 0.0)), (LOOP_FRAMES, (0.0, 0.0, 0.0))],
        "edge": [(1, (0.0, 0.0, 0.0)), (26, (-0.075, 0.0, 0.0)), (52, (0.035, 0.0, 0.0)),
                 (84, (-0.05, 0.0, 0.0)), (LOOP_FRAMES, (0.0, 0.0, 0.0))],
    })


def build_banner_cloth() -> None:
    """A hanging cloth banner swinging from a wall bracket."""
    steel_mat = pbr("TC_MAT_MotionBannerBracket", WORN_STEEL, rough=0.65, metal=0.5)
    cloth_mat = pbr("TC_MAT_MotionBannerCloth", CLOTH_OCHRE, rough=0.94)
    trim_mat = pbr("TC_MAT_MotionBannerTrim", HARDWOOD, rough=0.7)
    parts: list[bpy.types.Object] = []
    top_z = 3.1
    drop = 2.0
    parts.append(_box("wall_plate", (0.2, 0.12, 0.3), (0.0, 0.0, top_z), steel_mat))
    parts.append(_cyl("arm", 0.03, 0.62, (0.0, 0.31, top_z), steel_mat,
                      rot=(math.pi / 2.0, 0.0, 0.0), verts=6))
    parts.append(_cyl("cross_bar", 0.028, 1.0, (0.0, 0.6, top_z - 0.04), trim_mat,
                      rot=(0.0, math.pi / 2.0, 0.0), verts=6))
    parts.append(_box("banner", (0.9, 0.03, drop), (0.0, 0.6, top_z - 0.04 - drop / 2.0), cloth_mat))
    parts.append(_box("banner_hem", (0.94, 0.05, 0.12), (0.0, 0.6, top_z - 0.04 - drop), trim_mat))
    mesh = _join("TC_ENV_BannerCloth_V1", parts)

    armature = _build_armature("TC_RIG_BannerCloth", [
        ("bracket", (0.0, 0.0, top_z), (0.0, 0.6, top_z), None),
        ("banner_upper", (0.0, 0.6, top_z - 0.04), (0.0, 0.6, top_z - drop * 0.55), "bracket"),
        ("banner_lower", (0.0, 0.6, top_z - drop * 0.55), (0.0, 0.6, top_z - drop), "banner_upper"),
    ])
    _skin(mesh, armature)
    # The lower half lags the upper: a hanging cloth trails its own top edge.
    _keyframe_loop(armature, {
        "bracket": [(1, (0.0, 0.0, 0.0)), (LOOP_FRAMES, (0.0, 0.0, 0.0))],
        "banner_upper": [(1, (0.0, 0.0, 0.0)), (32, (0.0, 0.0, 0.07)),
                         (74, (0.0, 0.0, -0.055)), (LOOP_FRAMES, (0.0, 0.0, 0.0))],
        "banner_lower": [(1, (0.0, 0.0, 0.0)), (44, (0.0, 0.0, 0.1)),
                         (88, (0.0, 0.0, -0.07)), (LOOP_FRAMES, (0.0, 0.0, 0.0))],
    })


ASSETS = {
    "TC_ENV_PalmSway_V1": build_palm_sway,
    "TC_ENV_LaundryLine_V1": build_laundry_line,
    "TC_ENV_AwningCloth_V1": build_awning_cloth,
    "TC_ENV_BannerCloth_V1": build_banner_cloth,
}


def main() -> None:
    only = None
    if "--" in sys.argv:
        tail = sys.argv[sys.argv.index("--") + 1:]
        if len(tail) == 2 and tail[0] == "--asset":
            only = tail[1]
        elif tail:
            raise SystemExit("usage: blender --background --python create_mwezi_quarter_motion_kit_v1.py [-- --asset TC_NAME]")
    names = [only] if only else list(ASSETS)
    for name in names:
        if name not in ASSETS:
            raise SystemExit(f"unknown asset: {name}")
        reset_scene()
        ASSETS[name]()
        output = SOURCE_DIR / f"{name}.blend"
        output.parent.mkdir(parents=True, exist_ok=True)
        bpy.ops.wm.save_as_mainfile(filepath=str(output))
        print(f"MOTION_KIT_BLEND_WRITTEN {output}")


if __name__ == "__main__":
    main()
