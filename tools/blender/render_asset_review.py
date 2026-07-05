#!/usr/bin/env python3
"""Render standard review PNGs for a Blender asset.

Run with Blender:
  blender --background --python tools/blender/render_asset_review.py -- source.blend output_dir
"""
from __future__ import annotations

import sys
from pathlib import Path

import bpy
from mathutils import Vector

VIEWS = {
    "front.png": ((0, -13, 5.0), (0, 0, 0.6), 36),
    "back.png": ((0, 13, 5.0), (0, 0, 0.6), 36),
    "left.png": ((-13, 0, 5.0), (0, 0, 0.6), 36),
    "right.png": ((13, 0, 5.0), (0, 0, 0.6), 36),
    "top.png": ((0, 0, 18), (0, 0, 0), 45),
    "hero_three_quarter.png": ((9, -10, 6.0), (0, 0, 0.7), 32),
    "scale_reference.png": ((3.5, -8, 3.0), (1.1, -2.9, 0.8), 42),
    "material_preview.png": ((-6, -7, 3.8), (-1.5, -0.8, 0.45), 48),
}


def args() -> tuple[Path, Path]:
    if "--" not in sys.argv:
        raise SystemExit("usage: blender --background --python render_asset_review.py -- source.blend output_dir")
    tail = sys.argv[sys.argv.index("--") + 1:]
    if len(tail) != 2:
        raise SystemExit("expected source.blend and output_dir")
    return Path(tail[0]), Path(tail[1])


def look_at(obj: bpy.types.Object, target: tuple[float, float, float]) -> None:
    direction = Vector(target) - obj.location
    obj.rotation_euler = direction.to_track_quat("-Z", "Y").to_euler()


def render_view(output: Path, location: tuple[float, float, float], target: tuple[float, float, float], lens: float) -> None:
    bpy.ops.object.camera_add(location=location)
    camera = bpy.context.object
    look_at(camera, target)
    camera.data.lens = lens
    bpy.context.scene.camera = camera
    bpy.context.scene.render.filepath = str(output)
    bpy.ops.render.render(write_still=True)
    bpy.data.objects.remove(camera, do_unlink=True)
    print(f"ASSET_REVIEW_RENDERED {output}")


def write_stats(output_dir: Path) -> None:
    meshes = [o for o in bpy.context.scene.objects if o.type == "MESH"]
    depsgraph = bpy.context.evaluated_depsgraph_get()
    triangles = 0
    materials = sorted({slot.material.name for o in meshes for slot in o.material_slots if slot.material})
    min_x = min_y = min_z = 999999.0
    max_x = max_y = max_z = -999999.0
    for obj in meshes:
        mesh = obj.evaluated_get(depsgraph).to_mesh()
        for poly in mesh.polygons:
            triangles += max(1, len(poly.vertices) - 2)
        for vertex in mesh.vertices:
            world = obj.matrix_world @ vertex.co
            min_x, min_y, min_z = min(min_x, world.x), min(min_y, world.y), min(min_z, world.z)
            max_x, max_y, max_z = max(max_x, world.x), max(max_y, world.y), max(max_z, world.z)
        obj.evaluated_get(depsgraph).to_mesh_clear()
    lines = [
        "# TC_TerrainDioramaKit_V1 Mesh Stats",
        f"mesh_count: {len(meshes)}",
        f"triangle_count: {triangles}",
        f"material_slots: {len(materials)}",
        f"materials: {', '.join(materials)}",
        f"dimensions_xyz_m: {max_x-min_x:.3f}, {max_y-min_y:.3f}, {max_z-min_z:.3f}",
    ]
    (output_dir / "mesh_stats_report.md").write_text("\n".join(lines) + "\n", encoding="utf-8")
    print(f"ASSET_REVIEW_STATS_WRITTEN {output_dir / 'mesh_stats_report.md'}")


def main() -> None:
    source, output_dir = args()
    bpy.ops.wm.open_mainfile(filepath=str(source))
    output_dir.mkdir(parents=True, exist_ok=True)
    bpy.ops.object.light_add(type="AREA", location=(0, -6, 8))
    bpy.context.object.data.energy = 550
    bpy.context.object.data.size = 7
    bpy.ops.object.light_add(type="AREA", location=(-6, 4, 5))
    bpy.context.object.data.energy = 180
    bpy.context.object.data.size = 5
    bpy.context.scene.render.resolution_x = 1280
    bpy.context.scene.render.resolution_y = 720
    if hasattr(bpy.context.scene, "eevee"):
        bpy.context.scene.eevee.taa_render_samples = 32
    bpy.context.scene.world.color = (0.07, 0.075, 0.085)
    for name, (location, target, lens) in VIEWS.items():
        render_view(output_dir / name, location, target, lens)
    write_stats(output_dir)


if __name__ == "__main__":
    main()
