#!/usr/bin/env python3
from __future__ import annotations
import hashlib, json, os, subprocess, sys
from datetime import datetime, timezone
from pathlib import Path
from PIL import Image, ImageDraw, ImageFont

SCREENSHOTS = [
 'procedural_terrain_01_spawn_route.png',
 'procedural_terrain_02_resource_workbench_route.png',
 'procedural_terrain_03_combat_zone.png',
 'procedural_terrain_04_beacon_route.png',
 'procedural_terrain_05_wide_crash_site.png',
]
REQUIRED = [*SCREENSHOTS, 'procedural_terrain_contact_sheet.png', 'runtime-contract-report.json', 'terrain-generation-report.json', 'capture.log', 'test-summary.txt']

def sha(path: Path) -> str:
    h=hashlib.sha256(); h.update(path.read_bytes()); return h.hexdigest()

def dims(path: Path):
    if path.suffix.lower() != '.png': return None
    with Image.open(path) as img: return {'width': img.width, 'height': img.height}

def contact(out: Path) -> None:
    cell_w, img_h, label_h = 640, 360, 36
    rows = 3
    sheet = Image.new('RGB', (cell_w*2, (img_h+label_h)*rows), 'white')
    draw = ImageDraw.Draw(sheet); font = ImageFont.load_default()
    for i,name in enumerate(SCREENSHOTS):
        with Image.open(out/name) as src:
            img=src.convert('RGB'); img.thumbnail((cell_w,img_h), Image.Resampling.LANCZOS)
            r,c=divmod(i,2); x=c*cell_w+(cell_w-img.width)//2; y=r*(img_h+label_h)+(img_h-img.height)//2
            sheet.paste(img,(x,y)); tw=draw.textlength(name,font=font); draw.text((c*cell_w+(cell_w-tw)/2,r*(img_h+label_h)+img_h+8),name,fill='black',font=font)
    sheet.save(out/'procedural_terrain_contact_sheet.png')

def godot_version():
    try: return subprocess.check_output(['godot','--version'], text=True).strip()
    except Exception: return os.environ.get('GODOT_VERSION','unknown')

def renderer(out: Path):
    log=out/'capture.log'
    if log.exists():
        for line in log.read_text(errors='replace').splitlines():
            if 'OpenGL API' in line or 'Vulkan API' in line: return line.strip()
    return os.environ.get('RENDERER','OpenGL or Vulkan via Xvfb')

def pr_number():
    p=os.environ.get('GITHUB_EVENT_PATH')
    if p and Path(p).exists():
        return str(json.loads(Path(p).read_text()).get('pull_request',{}).get('number','')) or None
    return None

def main():
    out=Path(sys.argv[1]) if len(sys.argv)>1 else Path('artifacts/visual-review/phase3a-pass1b-procedural-terrain')
    contact(out)
    (out/'test-summary.txt').write_text('\n'.join([
        'Phase 3A Pass 1B procedural terrain artifact summary',
        f'UTC timestamp: {datetime.now(timezone.utc).isoformat()}',
        f'Unit tests: {os.environ.get("UNIT_TEST_RESULT","unknown")}',
        f'Integration tests: {os.environ.get("INTEGRATION_TEST_RESULT","unknown")}',
        f'Runtime flag count: {os.environ.get("RUNTIME_FLAG_COUNT","unknown")}',
        f'Screenshot validation: {os.environ.get("SCREENSHOT_VALIDATION_RESULT","unknown")}',
    ])+'\n')
    files=[]
    for name in REQUIRED:
        path=out/name
        if not path.exists(): raise SystemExit(f'Missing required artifact file: {path}')
        files.append({'filename':name,'dimensions':dims(path),'byte_size':path.stat().st_size,'sha256':sha(path)})
    manifest={
        'commit_sha': os.environ.get('GITHUB_SHA', subprocess.getoutput('git rev-parse HEAD')),
        'pr_number': pr_number(),
        'workflow_run_id': os.environ.get('GITHUB_RUN_ID'),
        'utc_timestamp': datetime.now(timezone.utc).isoformat(),
        'godot_version': godot_version(),
        'renderer': renderer(out),
        'capture_command': os.environ.get('CAPTURE_COMMAND','xvfb-run -a godot --path . --script tools/visual_review/capture_phase3a_pass1b_procedural_terrain.gd'),
        'runtime_flag_count': os.environ.get('RUNTIME_FLAG_COUNT','unknown'),
        'unit_test_result': os.environ.get('UNIT_TEST_RESULT','unknown'),
        'integration_test_result': os.environ.get('INTEGRATION_TEST_RESULT','unknown'),
        'filenames':[f['filename'] for f in files]+['manifest.json'],
        'files':files,
        'artifact_name':os.environ.get('ARTIFACT_NAME','phase3a-pass1b-procedural-terrain-local')
    }
    (out/'manifest.json').write_text(json.dumps(manifest,indent=2)+'\n')
    print(f'PASS1B_ARTIFACT_MANIFEST_OK {out / "manifest.json"}')
if __name__ == '__main__': main()
