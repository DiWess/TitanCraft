#!/usr/bin/env python3
from pathlib import Path
import struct
import sys

if len(sys.argv) > 1 and sys.argv[1] == '--qualification':
    OUTPUT_DIR = Path(sys.argv[2]) if len(sys.argv) > 2 else Path('artifacts/visual-review/phase3a-terrain-asset-qualification')
    REQUIRED_SCREENSHOTS = ['qualification_summary.png']
elif len(sys.argv) > 1 and sys.argv[1] == '--pass1d':
    OUTPUT_DIR = Path(sys.argv[2]) if len(sys.argv) > 2 else Path('artifacts/visual-review/phase3a-pass1d-terrain-visibility')
    REQUIRED_SCREENSHOTS = [
        'pass1d_zone_id.png',
        'pass1d_height_gradient.png',
        'pass1d_normals.png',
        'pass1d_wireframe_top.png',
        'pass1d_route_mask.png',
        'pass1d_neutral_lighting.png',
        'pass1d_production_material_terrain_only.png',
        'pass1d_prod_01_spawn_route.png',
        'pass1d_prod_02_resource_workbench.png',
        'pass1d_prod_03_combat_zone.png',
        'pass1d_prod_04_beacon_route.png',
    ]
elif len(sys.argv) > 1 and sys.argv[1] == '--pass1e':
    OUTPUT_DIR = Path(sys.argv[2]) if len(sys.argv) > 2 else Path('artifacts/visual-review/phase3a-pass1e-semantic-terrain')
    REQUIRED_SCREENSHOTS = [
        'pass1e_zone_id.png',
        'pass1e_route_topology.png',
        'pass1e_plateau_and_shelves.png',
        'pass1e_ridges_and_beacon_shelf.png',
        'pass1e_craters.png',
        'pass1e_horizon_segments.png',
        'pass1e_neutral_lighting.png',
        'pass1e_wireframe_wide.png',
        'pass1e_01_spawn_route_terrain_only.png',
        'pass1e_02_resource_workbench_terrain_only.png',
        'pass1e_03_combat_ridge_terrain_only.png',
        'pass1e_04_beacon_shelf_terrain_only.png',
        'pass1e_05_crater_view.png',
        'pass1e_06_wide_semantic_terrain.png',
    ]
elif len(sys.argv) > 1 and sys.argv[1] == '--pass1c':
    OUTPUT_DIR = Path(sys.argv[2]) if len(sys.argv) > 2 else Path('artifacts/visual-review/phase3a-pass1c-directed-terrain')
    REQUIRED_SCREENSHOTS = [
        'pass1c_01_player_spawn_route.png',
        'pass1c_02_player_resource_workbench_route.png',
        'pass1c_03_player_combat_zone.png',
        'pass1c_04_player_beacon_route.png',
        'pass1c_05_wide_production_context.png',
        'pass1c_diag_01_terrain_route_only.png',
        'pass1c_diag_02_terrain_zones_only.png',
        'pass1c_diag_03_terrain_wide_only.png',
    ]
elif len(sys.argv) > 1 and sys.argv[1] == '--pass1b':
    OUTPUT_DIR = Path(sys.argv[2]) if len(sys.argv) > 2 else Path('artifacts/visual-review/phase3a-pass1b-procedural-terrain')
    REQUIRED_SCREENSHOTS = [
        'procedural_terrain_01_spawn_route.png',
        'procedural_terrain_02_resource_workbench_route.png',
        'procedural_terrain_03_combat_zone.png',
        'procedural_terrain_04_beacon_route.png',
        'procedural_terrain_05_wide_crash_site.png',
    ]
else:
    OUTPUT_DIR = Path(sys.argv[1]) if len(sys.argv) > 1 else Path('artifacts/visual-review/phase3a-pass1-terrain')
    REQUIRED_SCREENSHOTS = [
        'terrain_01_spawn_route.png',
        'terrain_02_foreground_midground.png',
        'terrain_03_combat_zone.png',
        'terrain_04_wide_crash_site.png',
    ]

for filename in REQUIRED_SCREENSHOTS:
    path = OUTPUT_DIR / filename
    data = path.read_bytes() if path.exists() else b''
    if not data.startswith(b'\x89PNG\r\n\x1a\n'):
        raise SystemExit(f'Missing or invalid PNG: {path}')
    if len(data) < 1024:
        raise SystemExit(f'PNG too small / likely dummy: {path}')
    width, height = struct.unpack('>II', data[16:24])
    if (width, height) != (1280, 720):
        raise SystemExit(f'Unexpected size for {path}: {(width, height)}')
    if b'IDAT' not in data:
        raise SystemExit(f'PNG has no image data: {path}')
    print(f'PNG_OK {path} size={width}x{height} bytes={len(data)}')
