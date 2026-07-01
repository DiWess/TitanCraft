#!/usr/bin/env python3
from pathlib import Path
import struct
import sys

if len(sys.argv) > 1 and sys.argv[1] == '--qualification':
    OUTPUT_DIR = Path(sys.argv[2]) if len(sys.argv) > 2 else Path('artifacts/visual-review/phase3a-terrain-asset-qualification')
    REQUIRED_SCREENSHOTS = ['qualification_summary.png']
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
