#!/usr/bin/env python3
"""Generate TitanCraft's TC_HullRibOccluder_V1 as a text OBJ via Blender.

This Blender-only authoring script creates a low-poly, collisionless wreckage rib
that can be committed as text and instanced in the Crash Site scene without
adding gameplay mechanics or binary source/export files.
"""
from __future__ import annotations

import hashlib
from pathlib import Path

import bpy

ROOT = Path(__file__).resolve().parents[2]
OUT_OBJ = ROOT / "assets/Production/Custom/StageA/TC_HullRibOccluder_V1.obj"
REVIEW_DIR = ROOT / "artifacts/asset-review/TC_HullRibOccluder_V1"


def _reset_scene() -> None:
    bpy.ops.object.select_all(action="SELECT")
    bpy.ops.object.delete()


def _make_mesh() -> tuple[list[tuple[float, float, float]], list[tuple[int, int, int]]]:
    # The mesh is a bent spacecraft rib: an arch-like spine with torn feet and
    # asymmetric cross braces, kept deliberately low-poly for Crash Site clarity.
    verts = [
        (-1.55, 0.00, -0.28), (-1.15, 0.62, -0.18), (-0.55, 1.08, -0.10),
        (0.15, 1.22, 0.00), (0.82, 0.92, 0.10), (1.30, 0.35, 0.18),
        (1.62, 0.00, 0.25), (-1.55, 0.00, 0.28), (-1.15, 0.62, 0.18),
        (-0.55, 1.08, 0.10), (0.15, 1.22, 0.02), (0.82, 0.92, -0.08),
        (1.30, 0.35, -0.18), (1.62, 0.00, -0.25), (-1.88, -0.18, -0.45),
        (-1.36, 0.12, -0.32), (-1.92, -0.18, 0.40), (-1.32, 0.10, 0.30),
        (1.92, -0.16, 0.44), (1.42, 0.12, 0.30), (1.86, -0.14, -0.42),
        (1.38, 0.10, -0.30), (-0.78, 0.28, -0.22), (0.70, 0.78, 0.18),
        (-0.62, 0.36, 0.22), (0.54, 0.72, -0.18), (-0.08, 1.34, -0.04),
        (0.36, 1.06, 0.16), (0.48, 0.18, 0.34), (0.78, 0.32, -0.24),
    ]
    faces = [
        (1, 2, 9), (1, 9, 8), (2, 3, 10), (2, 10, 9), (3, 4, 11),
        (3, 11, 10), (4, 5, 12), (4, 12, 11), (5, 6, 13), (5, 13, 12),
        (6, 7, 14), (6, 14, 13), (15, 16, 18), (15, 18, 17),
        (19, 20, 22), (19, 22, 21), (23, 24, 25), (24, 26, 25),
        (27, 28, 11), (28, 12, 11), (29, 30, 20), (30, 22, 20),
    ]
    return verts, faces


def _write_obj(verts: list[tuple[float, float, float]], faces: list[tuple[int, int, int]]) -> None:
    OUT_OBJ.parent.mkdir(parents=True, exist_ok=True)
    with OUT_OBJ.open("w", encoding="utf-8") as fh:
        fh.write("# TitanCraft custom Stage A static mesh: TC_HullRibOccluder_V1\n")
        fh.write("# Source: tools/blender/create_hull_rib_occluder_v1.py\n")
        fh.write("# License: project-authored; visual-only; collisionless; Crash Site MVP scope\n")
        fh.write("o TC_HullRibOccluder_V1\n")
        for x, y, z in verts:
            fh.write(f"v {x:.4f} {y:.4f} {z:.4f}\n")
        for a, b, c in faces:
            fh.write(f"f {a} {b} {c}\n")


def _write_review_files() -> None:
    REVIEW_DIR.mkdir(parents=True, exist_ok=True)
    digest = hashlib.sha256(OUT_OBJ.read_bytes()).hexdigest()
    (REVIEW_DIR / "mesh_stats_report.md").write_text(
        "# TC_HullRibOccluder_V1 Mesh Stats\n\n"
        "- Mesh count: 1\n"
        "- Vertex count: 30\n"
        "- Triangle count: 22\n"
        "- Production path: `assets/Production/Custom/StageA/TC_HullRibOccluder_V1.obj`\n",
        encoding="utf-8",
    )
    (REVIEW_DIR / "review_metadata.md").write_text(
        "review_date: 2026-07-07\n"
        "asset: TC_HullRibOccluder_V1\n"
        "source_script: tools/blender/create_hull_rib_occluder_v1.py\n"
        "production_asset: assets/Production/Custom/StageA/TC_HullRibOccluder_V1.obj\n"
        "license: project-authored\n"
        "scope: Crash Site MVP visual-only hull rib occluder\n"
        "forbidden_scope_added: none\n"
        "binary_policy: text_obj_committed_no_blend_or_glb_binary\n",
        encoding="utf-8",
    )
    (REVIEW_DIR / "sha256sums.txt").write_text(
        f"{digest}  assets/Production/Custom/StageA/TC_HullRibOccluder_V1.obj\n",
        encoding="utf-8",
    )


def main() -> None:
    _reset_scene()
    verts, faces = _make_mesh()
    mesh = bpy.data.meshes.new("TC_HullRibOccluder_V1_Mesh")
    mesh.from_pydata(verts, [], [(a - 1, b - 1, c - 1) for a, b, c in faces])
    mesh.update()
    obj = bpy.data.objects.new("TC_HullRibOccluder_V1", mesh)
    bpy.context.collection.objects.link(obj)
    _write_obj(verts, faces)
    _write_review_files()
    print(f"TC_HULL_RIB_OCCLUDER_WRITTEN {OUT_OBJ}")


if __name__ == "__main__":
    main()
