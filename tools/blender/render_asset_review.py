#!/usr/bin/env python3
"""Render TitanCraft Stage A Blender asset review PNGs.

Run with Blender:
  blender --background --python tools/blender/render_asset_review.py -- source.blend artifacts/asset-review/AssetName
"""
from __future__ import annotations

import json
import sys
from pathlib import Path
import bpy
from mathutils import Vector

VIEWS = {
    "front": ((0, -6, 2.2), (0, 0, 0.6)),
    "back": ((0, 6, 2.2), (0, 0, 0.6)),
    "left": ((-6, 0, 2.2), (0, 0, 0.6)),
    "right": ((6, 0, 2.2), (0, 0, 0.6)),
    "top": ((0, 0, 8), (0, 0, 0)),
    "hero_three_quarter": ((4.5, -5.5, 3.0), (0, 0, 0.6)),
    "scale_reference_player_capsule": ((3.6, -5.0, 2.3), (0, 0, 0.8)),
    "material_preview": ((4.0, -4.6, 2.6), (0, 0, 0.7)),
}


def _args() -> tuple[Path, Path]:
    if "--" not in sys.argv:
        raise SystemExit("usage: blender --background --python render_asset_review.py -- source.blend output_dir")
    tail = sys.argv[sys.argv.index("--") + 1:]
    if len(tail) != 2:
        raise SystemExit("expected source.blend output_dir")
    return Path(tail[0]), Path(tail[1])


def _look_at(obj: bpy.types.Object, target: Vector) -> None:
    direction = target - obj.location
    obj.rotation_euler = direction.to_track_quat("-Z", "Y").to_euler()


def _ensure_light() -> None:
    bpy.ops.object.light_add(type="AREA", location=(2.5, -3.5, 5.0))
    light = bpy.context.object
    light.name = "TC_Review_KeyLight"
    light.data.energy = 550
    light.data.size = 5


def _add_player_capsule() -> bpy.types.Object:
    bpy.ops.mesh.primitive_uv_sphere_add(segments=16, ring_count=8, location=(-1.5, 0, 1.55), scale=(0.32, 0.32, 0.32))
    head = bpy.context.object
    head.name = "TC_PlayerScaleCapsule_Head"
    bpy.ops.mesh.primitive_cylinder_add(vertices=16, radius=0.32, depth=1.45, location=(-1.5, 0, 0.75))
    body = bpy.context.object
    body.name = "TC_PlayerScaleCapsule_Body"
    mat = bpy.data.materials.new("TC_PlayerScale_Orange")
    mat.diffuse_color = (1.0, 0.42, 0.08, 1)
    head.data.materials.append(mat)
    body.data.materials.append(mat)
    return body


def _mesh_stats() -> dict[str, object]:
    depsgraph = bpy.context.evaluated_depsgraph_get()
    meshes = [obj for obj in bpy.context.scene.objects if obj.type == "MESH" and not obj.name.startswith("TC_PlayerScaleCapsule")]
    triangles = 0
    materials = sorted({slot.material.name for obj in meshes for slot in obj.material_slots if slot.material})
    dimensions = {}
    for obj in meshes:
        mesh = obj.evaluated_get(depsgraph).to_mesh()
        triangles += sum(max(1, len(poly.vertices) - 2) for poly in mesh.polygons)
        obj.evaluated_get(depsgraph).to_mesh_clear()
        dimensions[obj.name] = [round(v, 4) for v in obj.dimensions]
    return {"mesh_count": len(meshes), "triangle_count": triangles, "material_slots": materials, "dimensions": dimensions}


def main() -> None:
    source, output_dir = _args()
    bpy.ops.wm.open_mainfile(filepath=str(source))
    output_dir.mkdir(parents=True, exist_ok=True)
    _ensure_light()
    _add_player_capsule()
    bpy.context.scene.render.resolution_x = 1280
    bpy.context.scene.render.resolution_y = 720
    bpy.context.scene.world.color = (0.08, 0.10, 0.12)
    bpy.ops.object.camera_add(location=(4, -5, 3))
    camera = bpy.context.object
    camera.name = "TC_ReviewCamera"
    camera.data.lens = 55
    bpy.context.scene.camera = camera
    for name, (position, target) in VIEWS.items():
        camera.location = position
        _look_at(camera, Vector(target))
        bpy.context.scene.render.filepath = str(output_dir / f"{name}.png")
        bpy.ops.render.render(write_still=True)
        print(f"BLENDER_ASSET_REVIEW_RENDERED {output_dir / f'{name}.png'}")
    stats_path = output_dir / "wireframe_or_mesh_stats.json"
    stats_path.write_text(json.dumps(_mesh_stats(), indent=2) + "\n", encoding="utf-8")
    print(f"BLENDER_ASSET_REVIEW_STATS {stats_path}")


if __name__ == "__main__":
    main()
