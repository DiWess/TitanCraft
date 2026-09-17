#!/usr/bin/env python3
"""Render review PNGs for every TC_ENV Mwezi Quarter district kit asset.

Cameras auto-frame from evaluated scene bounds and every view carries a 1.8 m
scale-reference post (render-only, never exported), per the Stage A review
lesson recorded in `docs/art/reviews/heavy-crash-hull-v1-standalone-review.md`:
unframed or scale-less PNGs are not reviewable evidence.

Three views per asset: a three-quarter hero for silhouette, a flat side for
proportion, and a ground-level eye-height view at roughly the player's camera
height, because these are architectural pieces the player walks past rather
than props seen from above.

Run with Blender after source generation:
  blender --background --python tools/blender/render_badjanani_district_kit_reviews.py
  blender --background --python tools/blender/render_badjanani_district_kit_reviews.py -- --asset TC_ENV_ArcadeBay_V1
"""
from __future__ import annotations

import math
import sys
from pathlib import Path

import bpy
from mathutils import Vector

ROOT = Path(__file__).resolve().parents[2]
SOURCE_DIR = ROOT / "assets/Source/Blender/Production/MweziQuarter_V1"
REVIEW_ROOT = ROOT / "artifacts/asset-review"

SCALE_REFERENCE_HEIGHT_M = 1.8
PLAYER_EYE_HEIGHT_M = 1.65

ASSETS = [
    "TC_ENV_CoralWallSegment_V1",
    "TC_ENV_ArcadeBay_V1",
    "TC_ENV_QuarterTower_V1",
    "TC_ENV_CarvedDoorway_V1",
    "TC_ENV_StoneHouse_V1",
    "TC_ENV_StairTerrace_V1",
    "TC_ENV_SeawallRun_V1",
    "TC_ENV_MarketStall_V1",
    "TC_ENV_PalmCluster_V1",
    "TC_ENV_CoralRubble_V1",
]

RESOLUTION_X = 1280
RESOLUTION_Y = 720
SENSOR_WIDTH_MM = 36.0

# view name -> (direction from centre to camera, framing margin, lens mm, eye level)
VIEWS = {
    "hero_three_quarter.png": (Vector((-0.72, -0.78, 0.40)), 1.18, 35, False),
    "side_silhouette.png": (Vector((0.0, -1.0, 0.12)), 1.14, 40, False),
    "player_eye_level.png": (Vector((-0.35, -1.0, 0.0)), 1.45, 30, True),
}


def _framing_distance(low: Vector, high: Vector, lens: float, margin: float) -> float:
    """Distance at which the whole bounding box fits the 16:9 frame.

    A distance taken from bounding-sphere radius alone crops tall, narrow
    assets, because a 16:9 frame is far shorter vertically than it is wide.
    This solves each axis against its own half-angle and takes the larger.
    """
    size = high - low
    sensor_height_mm = SENSOR_WIDTH_MM * RESOLUTION_Y / RESOLUTION_X
    half_h = math.atan((SENSOR_WIDTH_MM / 2.0) / lens)
    half_v = math.atan((sensor_height_mm / 2.0) / lens)
    # Worst-case horizontal extent for an arbitrary yaw is the XY diagonal.
    extent_h = math.hypot(size.x, size.y)
    extent_v = size.z
    return max(
        (extent_h / 2.0) / math.tan(half_h),
        (extent_v / 2.0) / math.tan(half_v),
        1.5,
    ) * margin


def _look_at(obj: bpy.types.Object, target: Vector) -> None:
    direction = target - obj.location
    obj.rotation_euler = direction.to_track_quat("-Z", "Y").to_euler()


def _scene_bounds() -> tuple[Vector, Vector]:
    low = Vector((1e9, 1e9, 1e9))
    high = Vector((-1e9, -1e9, -1e9))
    depsgraph = bpy.context.evaluated_depsgraph_get()
    for obj in bpy.context.scene.objects:
        if obj.type != "MESH":
            continue
        mesh = obj.evaluated_get(depsgraph).to_mesh()
        for vertex in mesh.vertices:
            world = obj.matrix_world @ vertex.co
            low = Vector(map(min, low, world))
            high = Vector(map(max, high, world))
        obj.evaluated_get(depsgraph).to_mesh_clear()
    return low, high


def _add_scale_reference(low: Vector, high: Vector) -> None:
    # The post must sit in the structure's own front plane, close to its
    # near-left corner. Parked off to one side it lands further from the
    # camera in a three-quarter view and perspective shrinks it, which is
    # exactly the unreadable-scale failure the Stage A review caught.
    base_x = low.x + (high.x - low.x) * 0.18
    base_y = low.y - 1.0
    bpy.ops.mesh.primitive_cylinder_add(
        vertices=12, radius=0.22, depth=SCALE_REFERENCE_HEIGHT_M,
        location=(base_x, base_y, SCALE_REFERENCE_HEIGHT_M / 2),
    )
    post = bpy.context.object
    post.name = "TC_ReviewScaleReference_1p8m"
    marker = bpy.data.materials.new("TC_ReviewScaleReferenceOrange")
    marker.use_nodes = True
    marker.node_tree.nodes["Principled BSDF"].inputs["Base Color"].default_value = (0.85, 0.30, 0.05, 1)
    post.data.materials.append(marker)


def _add_ground_plane(low: Vector, high: Vector) -> None:
    # Architecture without a ground plane reads as floating geometry; this
    # plane is render-only and is never part of the exported asset.
    span = max((high - low).length, 6.0) * 2.0
    bpy.ops.mesh.primitive_plane_add(size=span, location=(0.0, 0.0, low.z - 0.01))
    plane = bpy.context.object
    plane.name = "TC_ReviewGroundPlane"
    mat = bpy.data.materials.new("TC_ReviewGround")
    mat.use_nodes = True
    bsdf = mat.node_tree.nodes["Principled BSDF"]
    bsdf.inputs["Base Color"].default_value = (0.30, 0.28, 0.25, 1)
    bsdf.inputs["Roughness"].default_value = 0.95
    plane.data.materials.append(mat)


def _render(asset: str, name: str, direction: Vector, margin: float,
            lens: float, eye_level: bool) -> None:
    source = SOURCE_DIR / f"{asset}.blend"
    bpy.ops.wm.open_mainfile(filepath=str(source))
    low, high = _scene_bounds()
    center = (low + high) / 2
    radius = max((high - low).length / 2, 1.0)
    _add_scale_reference(low, high)
    _add_ground_plane(low, high)

    distance = _framing_distance(low, high, lens, margin)
    camera_location = center + direction.normalized() * distance
    if eye_level:
        # Player-eye views are deliberately not full-object framings: they
        # answer "what does this look like walking past it", so the camera
        # stays at eye height and looks at the lower half of the mass.
        camera_location.z = low.z + PLAYER_EYE_HEIGHT_M
        target = Vector((center.x, center.y, low.z + (high.z - low.z) * 0.35))
    else:
        # No z clamp here: nudging the camera up after the framing distance was
        # solved for this direction re-aims the shot and crops the asset, which
        # is what cut the lintel off the first doorway render.
        target = center

    # Key light from the camera-left, sun-height; fill from the opposite side.
    bpy.ops.object.light_add(type="AREA", location=(center.x - radius * 1.4, center.y - radius * 1.6, center.z + radius * 2.0))
    bpy.context.object.data.energy = 700 * radius
    bpy.context.object.data.size = radius * 1.6
    bpy.ops.object.light_add(type="AREA", location=(center.x + radius * 1.8, center.y + radius * 1.2, center.z + radius * 1.1))
    bpy.context.object.data.energy = 220 * radius
    bpy.context.object.data.size = radius * 1.4

    bpy.ops.object.camera_add(location=camera_location)
    camera = bpy.context.object
    _look_at(camera, target)
    camera.data.lens = lens
    camera.data.sensor_width = SENSOR_WIDTH_MM
    camera.data.clip_end = max(radius, distance) * 30
    bpy.context.scene.camera = camera
    bpy.context.scene.render.resolution_x = RESOLUTION_X
    bpy.context.scene.render.resolution_y = RESOLUTION_Y
    if hasattr(bpy.context.scene, "eevee"):
        bpy.context.scene.eevee.taa_render_samples = 32
    bpy.context.scene.world.color = (0.16, 0.19, 0.23)
    output_dir = REVIEW_ROOT / asset
    output_dir.mkdir(parents=True, exist_ok=True)
    bpy.context.scene.render.filepath = str(output_dir / name)
    bpy.ops.render.render(write_still=True)
    print(f"DISTRICT_KIT_REVIEW_RENDERED {output_dir / name}")


def main() -> None:
    only = None
    if "--" in sys.argv:
        tail = sys.argv[sys.argv.index("--") + 1:]
        if len(tail) == 2 and tail[0] == "--asset":
            only = tail[1]
        elif tail:
            raise SystemExit("usage: blender --background --python render_badjanani_district_kit_reviews.py [-- --asset TC_NAME]")
    names = [only] if only else ASSETS
    for asset in names:
        if not (SOURCE_DIR / f"{asset}.blend").exists():
            raise SystemExit(f"missing generated source blend: {asset}")
        for view_name, (direction, margin, lens, eye_level) in VIEWS.items():
            _render(asset, view_name, direction, margin, lens, eye_level)


if __name__ == "__main__":
    main()
