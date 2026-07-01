#!/usr/bin/env python3
from pathlib import Path
import struct
import sys

OUTPUT_DIR = Path(sys.argv[1]) if len(sys.argv) > 1 else Path('artifacts/visual-review/phase3a-production-integration')
REQUIRED_SCREENSHOTS = [
    'production_01_spawn_overview.png',
    'production_02_crashed_ship_hero.png',
    'production_03_ship_rear_engines.png',
    'production_04_resource_workbench_zone.png',
    'production_05_savepoint_beacon_zone.png',
    'production_06_galaxabrain_combat_distance.png',
    'production_07_mechanical_arm_first_person.png',
    'production_08_wide_terrain_composition.png',
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
