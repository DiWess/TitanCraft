#!/usr/bin/env python3
"""Validate the Stage A terrain diorama Blender asset contract."""
from __future__ import annotations

import json
import subprocess
import sys
from pathlib import Path

INSPECT = r'''
import json, sys, bpy
path = sys.argv[sys.argv.index('--') + 1]
bpy.ops.wm.open_mainfile(filepath=path)
meshes = [o for o in bpy.context.scene.objects if o.type == 'MESH']
required = ['main_concave_ash_basin','raised_fractured_rim_segment','basalt_outcrop','ash_drift_mound','hull_burial_contact_mound','route_edge_grounded_marker','distant_basalt_silhouette_piece','player_capsule_scale_reference']
names = [o.name for o in meshes]
materials = sorted({slot.material.name for o in meshes for slot in o.material_slots if slot.material})
issues = []
for req in required:
    if not any(req in name for name in names): issues.append(f'missing required kit piece: {req}')
for obj in meshes:
    if not obj.name.startswith('TC_TerrainDioramaKit_V1_'): issues.append(f'bad mesh prefix: {obj.name}')
    if obj.location.length > 0.001: issues.append(f'unclean origin/location: {obj.name}')
    if obj.get('titancraft_collision', 'none') != 'none': issues.append(f'collision metadata is not none: {obj.name}')
    if not obj.material_slots: issues.append(f'missing material slot: {obj.name}')
triangles = 0
min_x = min_y = min_z = 999999
max_x = max_y = max_z = -999999
depsgraph = bpy.context.evaluated_depsgraph_get()
for obj in meshes:
    mesh = obj.evaluated_get(depsgraph).to_mesh()
    for poly in mesh.polygons: triangles += max(1, len(poly.vertices) - 2)
    for v in mesh.vertices:
        world = obj.matrix_world @ v.co
        min_x, min_y, min_z = min(min_x, world.x), min(min_y, world.y), min(min_z, world.z)
        max_x, max_y, max_z = max(max_x, world.x), max(max_y, world.y), max(max_z, world.z)
    obj.evaluated_get(depsgraph).to_mesh_clear()
if triangles > 9000: issues.append(f'triangle budget exceeded: {triangles} > 9000')
print(json.dumps({'mesh_count': len(meshes), 'materials': materials, 'material_count': len(materials), 'triangles': triangles, 'dimensions': [round(max_x-min_x, 3), round(max_y-min_y, 3), round(max_z-min_z, 3)], 'issues': issues}, sort_keys=True))
if issues: raise SystemExit(2)
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
        print(result.stderr.strip(), file=sys.stderr)
        raise SystemExit(result.returncode)
    print(f"STAGE_A_BLENDER_ASSET_VALID source={source}")


if __name__ == "__main__":
    main()
