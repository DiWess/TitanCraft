#!/usr/bin/env python3
"""Validate a TitanCraft Blender source asset contract."""
from __future__ import annotations

import json
import subprocess
import sys
from pathlib import Path

INSPECT = r'''
import json, sys, bpy
from pathlib import Path
path = sys.argv[sys.argv.index('--') + 1]
bpy.ops.wm.open_mainfile(filepath=path)
meshes = [o for o in bpy.context.scene.objects if o.type == 'MESH']
materials = sorted({slot.material.name for o in meshes for slot in o.material_slots if slot.material})
issues = []
if not meshes: issues.append('no mesh objects')
for obj in meshes:
    if not obj.name.startswith('TC_'):
        issues.append(f'mesh name must start with TC_: {obj.name}')
    if obj.location.length > 0.001:
        issues.append(f'unclean origin/location on {obj.name}: {tuple(round(v,4) for v in obj.location)}')
    if obj.get('titancraft_collision', 'none') != 'none':
        issues.append(f'collision policy must be none on visual test asset: {obj.name}')
    if not obj.material_slots:
        issues.append(f'missing material slot on {obj.name}')
triangles = 0
for obj in meshes:
    depsgraph = bpy.context.evaluated_depsgraph_get()
    mesh = obj.evaluated_get(depsgraph).to_mesh()
    for poly in mesh.polygons:
        triangles += max(1, len(poly.vertices) - 2)
    obj.evaluated_get(depsgraph).to_mesh_clear()
print(json.dumps({'mesh_count': len(meshes), 'materials': materials, 'triangles': triangles, 'issues': issues}, sort_keys=True))
budgets = {
    'TC_HeavyCrashHull_V1.blend': 1500,
    'TC_PROP_Workbench_V1.blend': 3200,
    'TC_PROP_Beacon_Dormant_V1.blend': 2000,
    'TC_PROP_Beacon_Active_V1.blend': 2800,
    'TC_PICKUP_Metal_V1.blend': 900,
    'TC_PICKUP_Biomass_V1.blend': 1400,
    'TC_PICKUP_Electronics_V1.blend': 900,
    'TC_PICKUP_Component_V1.blend': 900,
    'TC_CHAR_GalaxabrainScout_V1.blend': 3200,
    'TC_PLAYER_MechanicalArm_V1.blend': 2600,
    'TC_PROP_SavePoint_V1.blend': 1400,
    'TC_ENV_CrashDebris_A_V1.blend': 1400,
    'TC_LightingReference_V1.blend': 700,
}
max_triangles = budgets.get(Path(path).name, 500)
if triangles > max_triangles:
    issues.append(f'triangle budget exceeded: {triangles} > {max_triangles}')
if issues:
    raise SystemExit(2)
'''


def main() -> None:
    if len(sys.argv) != 2:
        raise SystemExit("usage: python3 tools/blender/validate_blender_asset.py source.blend")
    source = Path(sys.argv[1])
    if not source.exists() or source.suffix.lower() != ".blend":
        raise SystemExit(f"missing .blend source: {source}")
    result = subprocess.run(["blender", "--background", "--python-expr", INSPECT, "--", str(source)], text=True, capture_output=True)
    print(result.stdout.strip())
    if result.returncode != 0:
        print(result.stderr, file=sys.stderr)
        raise SystemExit(result.returncode)
    print(f"BLENDER_ASSET_VALID source={source}")


if __name__ == "__main__":
    main()
