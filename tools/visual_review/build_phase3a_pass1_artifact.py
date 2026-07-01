#!/usr/bin/env python3
from __future__ import annotations

import hashlib
import json
import os
import subprocess
import sys
from datetime import datetime, timezone
from pathlib import Path
from typing import Any

from PIL import Image, ImageDraw, ImageFont

SCREENSHOTS = [
    'terrain_01_spawn_route.png',
    'terrain_02_foreground_midground.png',
    'terrain_03_combat_zone.png',
    'terrain_04_wide_crash_site.png',
]
REQUIRED_FILES = [
    *SCREENSHOTS,
    'terrain_contact_sheet.png',
    'runtime-contract-report.json',
    'capture.log',
    'test-summary.txt',
]


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open('rb') as handle:
        for chunk in iter(lambda: handle.read(1024 * 1024), b''):
            digest.update(chunk)
    return digest.hexdigest()


def png_dimensions(path: Path) -> dict[str, int] | None:
    if path.suffix.lower() != '.png':
        return None
    with Image.open(path) as image:
        return {'width': image.width, 'height': image.height}


def pr_number() -> str | None:
    event_path = os.environ.get('GITHUB_EVENT_PATH')
    if not event_path or not Path(event_path).exists():
        return None
    with open(event_path, encoding='utf-8') as event_file:
        event = json.load(event_file)
    number = event.get('pull_request', {}).get('number')
    return str(number) if number is not None else None


def godot_version() -> str:
    if os.environ.get('GODOT_VERSION_TEXT'):
        return os.environ['GODOT_VERSION_TEXT']
    try:
        return subprocess.check_output(['godot', '--version'], text=True).strip()
    except (OSError, subprocess.CalledProcessError):
        return os.environ.get('GODOT_VERSION', 'unknown')


def runtime_flag_count(output_dir: Path) -> int:
    report_path = output_dir / 'runtime-contract-report.json'
    with report_path.open(encoding='utf-8') as report_file:
        report = json.load(report_file)
    return len(report.get('flags', []))


def renderer(output_dir: Path) -> str:
    configured = os.environ.get('RENDERER')
    if configured:
        return configured
    capture_log = output_dir / 'capture.log'
    if capture_log.exists():
        text = capture_log.read_text(encoding='utf-8', errors='replace')
        for line in text.splitlines():
            if 'OpenGL API' in line or 'Vulkan API' in line:
                return line.strip()
    return 'OpenGL via Xvfb'


def create_contact_sheet(output_dir: Path) -> None:
    images = []
    for filename in SCREENSHOTS:
        path = output_dir / filename
        if not path.exists():
            raise SystemExit(f'Missing screenshot for contact sheet: {path}')
        images.append((filename, Image.open(path).convert('RGB')))

    cell_width, image_height, label_height = 640, 360, 36
    sheet = Image.new('RGB', (cell_width * 2, (image_height + label_height) * 2), 'white')
    draw = ImageDraw.Draw(sheet)
    font = ImageFont.load_default()

    for index, (filename, image) in enumerate(images):
        row, column = divmod(index, 2)
        image.thumbnail((cell_width, image_height), Image.Resampling.LANCZOS)
        x = column * cell_width + (cell_width - image.width) // 2
        y = row * (image_height + label_height) + (image_height - image.height) // 2
        sheet.paste(image, (x, y))
        label_y = row * (image_height + label_height) + image_height + 8
        label_width = draw.textlength(filename, font=font)
        draw.text((column * cell_width + (cell_width - label_width) / 2, label_y), filename, fill='black', font=font)

    sheet.save(output_dir / 'terrain_contact_sheet.png')


def write_test_summary(output_dir: Path, flag_count: int) -> None:
    lines = [
        'Phase 3A Pass 1 terrain visual artifact summary',
        f'UTC timestamp: {datetime.now(timezone.utc).isoformat()}',
        f'Unit tests: {os.environ.get("UNIT_TEST_RESULT", "unknown")}',
        f'Integration tests: {os.environ.get("INTEGRATION_TEST_RESULT", "unknown")}',
        f'Runtime flag count: {flag_count}',
        f'Screenshot validation: {os.environ.get("SCREENSHOT_VALIDATION_RESULT", "unknown")}',
    ]
    (output_dir / 'test-summary.txt').write_text('\n'.join(lines) + '\n', encoding='utf-8')


def file_entries(output_dir: Path) -> list[dict[str, Any]]:
    entries = []
    for filename in REQUIRED_FILES:
        path = output_dir / filename
        if not path.exists():
            raise SystemExit(f'Missing required artifact file: {path}')
        entries.append({
            'filename': filename,
            'dimensions': png_dimensions(path),
            'byte_size': path.stat().st_size,
            'sha256': sha256(path),
        })
    return entries


def main() -> None:
    output_dir = Path(sys.argv[1]) if len(sys.argv) > 1 else Path('artifacts/visual-review/phase3a-pass1-terrain')
    output_dir.mkdir(parents=True, exist_ok=True)
    flag_count = runtime_flag_count(output_dir)
    create_contact_sheet(output_dir)
    write_test_summary(output_dir, flag_count)
    artifact_name = os.environ.get('ARTIFACT_NAME') or f'phase3a-pass1-terrain-{os.environ.get("GITHUB_SHA", "local")}'
    manifest = {
        'commit_sha': os.environ.get('GITHUB_SHA', subprocess.getoutput('git rev-parse HEAD')),
        'pr_number': pr_number(),
        'workflow_run_id': os.environ.get('GITHUB_RUN_ID'),
        'utc_timestamp': datetime.now(timezone.utc).isoformat(),
        'godot_version': godot_version(),
        'renderer': renderer(output_dir),
        'capture_command': os.environ.get('CAPTURE_COMMAND', 'xvfb-run -a godot --path . --script tools/visual_review/capture_phase3a_pass1_terrain.gd'),
        'runtime_flag_count': flag_count,
        'unit_test_result': os.environ.get('UNIT_TEST_RESULT', 'unknown'),
        'integration_test_result': os.environ.get('INTEGRATION_TEST_RESULT', 'unknown'),
        'filenames': REQUIRED_FILES,
        'files': file_entries(output_dir),
        'artifact_name': artifact_name,
    }
    (output_dir / 'manifest.json').write_text(json.dumps(manifest, indent=2) + '\n', encoding='utf-8')
    print(f'ARTIFACT_MANIFEST_OK {output_dir / "manifest.json"}')


if __name__ == '__main__':
    main()
