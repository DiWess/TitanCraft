#!/usr/bin/env python3
"""Validate Stage A Blender asset review metadata and source contract."""
from __future__ import annotations

import json
import subprocess
import sys
from pathlib import Path

INSPECT = r'''
import json, sys, bpy
path = sys.argv[sys.argv.index('--') + 1]
bpy.ops.wm.open_mainfile(filepath=path)
meshes = [obj for obj in bpy.context.scene.objects if obj.type == 'MESH']
issues = []
materials = sorted({slot.material.name for obj in meshes for slot in obj.material_slots if slot.material})
triangles = 0
for obj in meshes:
    if not obj.name.startswith('TC_'):
        issues.append(f'mesh name must start with TC_: {obj.name}')
    if obj.get('titancraft_collision', 'none') != 'none':
        issues.append(f'collision policy must be none: {obj.name}')
    if not obj.material_slots:
        issues.append(f'missing material slot: {obj.name}')
    depsgraph = bpy.context.evaluated_depsgraph_get()
    mesh = obj.evaluated_get(depsgraph).to_mesh()
    triangles += sum(max(1, len(poly.vertices) - 2) for poly in mesh.polygons)
    obj.evaluated_get(depsgraph).to_mesh_clear()
if not meshes:
    issues.append('no mesh objects')
print(json.dumps({'mesh_count': len(meshes), 'triangle_count': triangles, 'material_slots': materials, 'issues': issues}, sort_keys=True))
if issues:
    raise SystemExit(2)
'''


def main() -> None:
    if len(sys.argv) != 2:
        raise SystemExit("usage: python3 tools/blender/validate_stage_a_blender_asset.py source.blend")
    source = Path(sys.argv[1])
    if not source.exists() or source.suffix.lower() != ".blend":
        raise SystemExit(f"missing .blend source: {source}")
    result = subprocess.run(["blender", "--background", "--python-expr", INSPECT, "--", str(source)], text=True, capture_output=True)
    print(result.stdout.strip())
    if result.returncode != 0:
        print(result.stderr, file=sys.stderr)
        raise SystemExit(result.returncode)
    print(f"STAGE_A_BLENDER_ASSET_VALID source={source}")


if __name__ == "__main__":
    main()
