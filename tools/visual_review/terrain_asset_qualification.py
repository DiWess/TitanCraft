#!/usr/bin/env python3
from __future__ import annotations

import hashlib
import json
import math
import os
import subprocess
import sys
from datetime import datetime, timezone
from pathlib import Path
from typing import Any

from PIL import Image, ImageDraw, ImageFont

OUTPUT_DIR = Path('artifacts/visual-review/phase3a-terrain-asset-qualification')
MESH_ROOT = Path('assets/ThirdParty/Quaternius/StylizedNatureMegaKit/Models')
CANDIDATES = ['rock_cliff_d.obj', 'rock_irregular_c.obj', 'rock_ridge_f.obj', 'rock_spire_g.obj']
SOURCE_ARCHIVE_HASH = '298f6732b872e4cf7b30e6e7abf9641c7f6dc6b326df37ac089533ed7e3d58c9'
SOURCE_ARCHIVE = 'Stylized Nature MegaKit[Standard].zip'
SOURCE_URL = 'https://quaternius.itch.io/stylized-nature-megakit'
SOURCE_PACK = 'Quaternius Stylized Nature MegaKit'
SAFE_PRODUCTION_SCALE = {'min': 0.5, 'max': 6.0}
ORIGIN_TOLERANCE_METRES = 1.0
MAX_CRASH_SITE_DIMENSION_METRES = 20.0


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open('rb') as handle:
        for chunk in iter(lambda: handle.read(1024 * 1024), b''):
            digest.update(chunk)
    return digest.hexdigest()


def parse_obj(path: Path) -> dict[str, Any]:
    vertices: list[tuple[float, float, float]] = []
    face_count = 0
    materials: set[str] = set()
    for line in path.read_text(encoding='utf-8', errors='replace').splitlines():
        if line.startswith('v '):
            _, x, y, z, *_ = line.split()
            vertices.append((float(x), float(y), float(z)))
        elif line.startswith('f '):
            face_count += 1
        elif line.startswith('usemtl '):
            materials.add(line.split(maxsplit=1)[1])
    if not vertices:
        return {'vertex_count': 0, 'face_count': face_count, 'material_count': len(materials), 'empty': True}
    mins = tuple(min(v[i] for v in vertices) for i in range(3))
    maxs = tuple(max(v[i] for v in vertices) for i in range(3))
    size = tuple(maxs[i] - mins[i] for i in range(3))
    centre = tuple((mins[i] + maxs[i]) / 2.0 for i in range(3))
    origin_distance = math.sqrt(sum(c * c for c in centre))
    return {
        'vertex_count': len(vertices),
        'face_count': face_count,
        'material_count': len(materials),
        'aabb_min': mins,
        'aabb_max': maxs,
        'aabb_size': size,
        'geometric_centre': centre,
        'origin_to_centre_distance': origin_distance,
        'lowest_vertex_y': mins[1],
        'highest_vertex_y': maxs[1],
        'longest_dimension': max(size),
        'empty': False,
    }


def classify(path: Path, stats: dict[str, Any]) -> tuple[str, list[str], bool, bool]:
    text = path.read_text(encoding='utf-8', errors='replace')
    reasons = []
    renamed = False
    modified = False
    if text.startswith('# Curated OBJ subset placeholder'):
        reasons.append('LOCAL_PLACEHOLDER_HEADER_NOT_OFFICIAL_SOURCE_BYTES')
        modified = True
    reasons.append('NO_PER_FILE_OFFICIAL_SOURCE_HASH_AVAILABLE_IN_REPO')
    if stats.get('empty'):
        reasons.append('EMPTY_MESH')
    if stats.get('origin_to_centre_distance', 999.0) > ORIGIN_TOLERANCE_METRES:
        reasons.append('ORIGIN_OUTSIDE_TOLERANCE')
    if stats.get('lowest_vertex_y', 999.0) != 0.0:
        reasons.append('LOWEST_VERTEX_NOT_GROUNDED_AT_ZERO')
    return 'INVALID_FOR_PRODUCTION', reasons, renamed, modified


def audit() -> dict[str, Any]:
    meshes = []
    for filename in CANDIDATES:
        path = MESH_ROOT / filename
        stats = parse_obj(path)
        classification, reasons, renamed, modified = classify(path, stats)
        local_hash = sha256(path)
        meshes.append({
            'filename': filename,
            'local_path': str(path),
            'official_source_pack': SOURCE_PACK,
            'official_source_url': SOURCE_URL,
            'archive_or_drive_source': SOURCE_ARCHIVE,
            'source_filename': f'UNKNOWN_ARCHIVE_INTERNAL_PATH/{filename}',
            'source_hash': SOURCE_ARCHIVE_HASH,
            'local_hash': local_hash,
            'renamed': renamed,
            'modified': modified,
            'byte_identical_to_official_source': False,
            'authentication_status': 'INVALID_FOR_PRODUCTION',
            'classification': classification,
            'role_mismatch': False,
            'qualification_reasons': reasons,
            'proposed_production_scale': None,
            'production_scale_safe_interval': SAFE_PRODUCTION_SCALE,
            'origin_tolerance_metres': ORIGIN_TOLERANCE_METRES,
            'max_crash_site_dimension_metres': MAX_CRASH_SITE_DIMENSION_METRES,
            'imported_forward_up_orientation': 'OBJ imported by Godot as Y-up; no terrain forward axis is authoritative.',
            **stats,
        })
    return {
        'generated_utc': datetime.now(timezone.utc).isoformat(),
        'source_policy': 'Candidates must be byte-identical to an official source file or remain INVALID_FOR_PRODUCTION.',
        'candidate_count': len(meshes),
        'authenticated_candidate_count': 0,
        'suitable_candidate_count': 0,
        'verdict': 'NO_SUITABLE_TERRAIN_MESHES',
        'meshes': meshes,
    }


def png_dimensions(path: Path) -> dict[str, int] | None:
    if path.suffix.lower() != '.png':
        return None
    with Image.open(path) as image:
        return {'width': image.width, 'height': image.height}


def create_contact_sheet(output_dir: Path) -> None:
    images = [path for path in output_dir.glob('*.png') if path.name != 'terrain_contact_sheet.png']
    if not images:
        raise SystemExit('No PNGs available for qualification contact sheet')
    cell_width, image_height, label_height = 640, 360, 36
    rows = math.ceil(len(images) / 2)
    sheet = Image.new('RGB', (cell_width * 2, (image_height + label_height) * rows), 'white')
    draw = ImageDraw.Draw(sheet)
    font = ImageFont.load_default()
    for index, path in enumerate(images):
        row, column = divmod(index, 2)
        with Image.open(path) as source:
            image = source.convert('RGB')
            image.thumbnail((cell_width, image_height), Image.Resampling.LANCZOS)
            x = column * cell_width + (cell_width - image.width) // 2
            y = row * (image_height + label_height) + (image_height - image.height) // 2
            sheet.paste(image, (x, y))
        label_y = row * (image_height + label_height) + image_height + 8
        label_width = draw.textlength(path.name, font=font)
        draw.text((column * cell_width + (cell_width - label_width) / 2, label_y), path.name, fill='black', font=font)
    sheet.save(output_dir / 'terrain_contact_sheet.png')


def pr_number() -> str | None:
    event_path = os.environ.get('GITHUB_EVENT_PATH')
    if not event_path or not Path(event_path).exists():
        return None
    with open(event_path, encoding='utf-8') as event_file:
        event = json.load(event_file)
    number = event.get('pull_request', {}).get('number')
    return str(number) if number is not None else None


def godot_version() -> str:
    try:
        return subprocess.check_output(['godot', '--version'], text=True).strip()
    except (OSError, subprocess.CalledProcessError):
        return os.environ.get('GODOT_VERSION', 'unknown')


def renderer(output_dir: Path) -> str:
    capture_log = output_dir / 'capture.log'
    if capture_log.exists():
        text = capture_log.read_text(encoding='utf-8', errors='replace')
        for line in text.splitlines():
            if 'OpenGL API' in line or 'Vulkan API' in line:
                return line.strip()
    return os.environ.get('RENDERER', 'OpenGL or Vulkan via Xvfb')


def write_test_summary(output_dir: Path, audit_data: dict[str, Any]) -> None:
    lines = [
        'Phase 3A terrain asset qualification summary',
        f'UTC timestamp: {datetime.now(timezone.utc).isoformat()}',
        f'Verdict: {audit_data["verdict"]}',
        f'Candidate count: {audit_data["candidate_count"]}',
        f'Authenticated candidate count: {audit_data["authenticated_candidate_count"]}',
        f'Suitable candidate count: {audit_data["suitable_candidate_count"]}',
        f'Unit tests: {os.environ.get("UNIT_TEST_RESULT", "unknown")}',
        f'Integration tests: {os.environ.get("INTEGRATION_TEST_RESULT", "unknown")}',
        f'Runtime flag count: {os.environ.get("RUNTIME_FLAG_COUNT", "unknown")}',
    ]
    (output_dir / 'test-summary.txt').write_text('\n'.join(lines) + '\n', encoding='utf-8')


def file_entries(output_dir: Path) -> list[dict[str, Any]]:
    entries = []
    for path in sorted(output_dir.iterdir()):
        if path.is_file() and path.name != 'manifest.json':
            entries.append({
                'filename': path.name,
                'dimensions': png_dimensions(path),
                'byte_size': path.stat().st_size,
                'sha256': sha256(path),
            })
    return entries


def main() -> None:
    output_dir = Path(sys.argv[1]) if len(sys.argv) > 1 else OUTPUT_DIR
    output_dir.mkdir(parents=True, exist_ok=True)
    audit_data = audit()
    (output_dir / 'terrain_asset_audit.json').write_text(json.dumps(audit_data, indent=2) + '\n', encoding='utf-8')
    create_contact_sheet(output_dir)
    write_test_summary(output_dir, audit_data)
    artifact_name = os.environ.get('ARTIFACT_NAME') or f'phase3a-terrain-asset-qualification-{os.environ.get("GITHUB_SHA", "local")}'
    manifest = {
        'commit_sha': os.environ.get('GITHUB_SHA', subprocess.getoutput('git rev-parse HEAD')),
        'pr_number': pr_number(),
        'workflow_run_id': os.environ.get('GITHUB_RUN_ID'),
        'utc_timestamp': datetime.now(timezone.utc).isoformat(),
        'godot_version': godot_version(),
        'renderer': renderer(output_dir),
        'capture_command': os.environ.get('CAPTURE_COMMAND', 'xvfb-run -a godot --path . --script tools/visual_review/capture_phase3a_terrain_asset_qualification.gd'),
        'terrain_qualification_verdict': audit_data['verdict'],
        'unit_test_result': os.environ.get('UNIT_TEST_RESULT', 'unknown'),
        'integration_test_result': os.environ.get('INTEGRATION_TEST_RESULT', 'unknown'),
        'runtime_flag_count': os.environ.get('RUNTIME_FLAG_COUNT', 'unknown'),
        'filenames': [entry['filename'] for entry in file_entries(output_dir)] + ['manifest.json'],
        'files': file_entries(output_dir),
        'artifact_name': artifact_name,
    }
    (output_dir / 'manifest.json').write_text(json.dumps(manifest, indent=2) + '\n', encoding='utf-8')
    print(f'TERRAIN_ASSET_AUDIT_VERDICT {audit_data["verdict"]}')
    print(f'ARTIFACT_MANIFEST_OK {output_dir / "manifest.json"}')


if __name__ == '__main__':
    main()
