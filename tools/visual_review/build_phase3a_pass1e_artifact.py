#!/usr/bin/env python3
from __future__ import annotations
import hashlib, json, os, subprocess, sys
from datetime import datetime, timezone
from pathlib import Path
from PIL import Image, ImageDraw, ImageFont

OUT = Path(sys.argv[1]) if len(sys.argv) > 1 else Path('artifacts/visual-review/phase3a-pass1e-semantic-terrain')
DEBUG = ['pass1e_zone_id.png','pass1e_route_topology.png','pass1e_plateau_and_shelves.png','pass1e_ridges_and_beacon_shelf.png','pass1e_craters.png','pass1e_horizon_segments.png','pass1e_neutral_lighting.png','pass1e_wireframe_wide.png']
REVIEW = ['pass1e_01_spawn_route_terrain_only.png','pass1e_02_resource_workbench_terrain_only.png','pass1e_03_combat_ridge_terrain_only.png','pass1e_04_beacon_shelf_terrain_only.png','pass1e_05_crater_view.png','pass1e_06_wide_semantic_terrain.png']
CONTACTS = ['pass1e_debug_contact_sheet.png','pass1e_terrain_review_contact_sheet.png']
REQUIRED = [*DEBUG,*REVIEW,*CONTACTS,'semantic-terrain-report.json','image-space-metrics.json','runtime-contract-report.json','terrain-generation-report.json','capture.log','test-summary.txt']

def sha(path: Path) -> str:
    h=hashlib.sha256(); h.update(path.read_bytes()); return h.hexdigest()

def dims(path: Path):
    if path.suffix.lower() != '.png': return None
    with Image.open(path) as img: return {'width':img.width,'height':img.height}

def lum(p): return (0.2126*p[0]+0.7152*p[1]+0.0722*p[2])/255.0

def metrics(name: str):
    with Image.open(OUT/name) as img: pix=list(img.convert('RGB').getdata())
    vals=sorted(lum(p) for p in pix); n=len(vals)
    terrain=sum(1 for p in pix if max(p)>18)
    route=sum(1 for p in pix if (p[0]>220 and p[1]>200 and p[2]<100) or (p[0]>220 and p[1]>220 and p[2]>220))
    distinct=len({(p[0]//32,p[1]//32,p[2]//32) for p in pix if max(p)-min(p)>12})
    return {'mean_luminance':sum(vals)/n,'p05_luminance':vals[int(n*.05)],'p95_luminance':vals[int(n*.95)],'near_black_terrain_pct':100*sum(v<.08 for v in vals)/n,'terrain_pixel_pct':100*terrain/n,'route_pixel_pct':100*route/n,'distinct_semantic_colours':distinct}

def contact(names, output, rows):
    cell_w,img_h,label_h=640,360,36
    sheet=Image.new('RGB',(cell_w*2,(img_h+label_h)*rows),'white')
    draw=ImageDraw.Draw(sheet); font=ImageFont.load_default()
    for i,name in enumerate(names):
        with Image.open(OUT/name) as src:
            img=src.convert('RGB'); img.thumbnail((cell_w,img_h), Image.Resampling.LANCZOS)
            r,c=divmod(i,2); x=c*cell_w+(cell_w-img.width)//2; y=r*(img_h+label_h)+(img_h-img.height)//2
            sheet.paste(img,(x,y)); tw=draw.textlength(name,font=font); draw.text((c*cell_w+(cell_w-tw)/2,r*(img_h+label_h)+img_h+8),name,fill='black',font=font)
    sheet.save(OUT/output)

def godot_version():
    try: return subprocess.check_output(['godot','--version'], text=True).strip()
    except Exception: return os.environ.get('GODOT_VERSION','unknown')

def main():
    terrain_report=json.loads((OUT/'terrain-generation-report.json').read_text())
    image_metrics={name:metrics(name) for name in [*DEBUG,*REVIEW]}
    semantic_names={m['name'] for m in terrain_report.get('semanticMeshes',[])}
    horizon_segments=terrain_report.get('horizonSegments',[])
    semantic_ok=all(name in semantic_names for name in ['RouteSurface','CentralPlateau','SpawnBasaltShelf','ResourceBasaltShelf','WorkbenchRidge','CombatRidge','BeaconShelf','CraterNorthwest','CraterSoutheast']) and len(horizon_segments)>=5
    route_visible=image_metrics['pass1e_route_topology.png']['route_pixel_pct']>0.5
    debug_bands=image_metrics['pass1e_zone_id.png']['distinct_semantic_colours']>=5
    review_light=image_metrics['pass1e_neutral_lighting.png']['near_black_terrain_pct']<50
    decision='PASS1E_SEMANTIC_TERRAIN_READY_FOR_HUMAN_REVIEW' if semantic_ok and route_visible and debug_bands and review_light else 'PASS1E_SEMANTIC_TERRAIN_GEOMETRY_NOT_GO'
    semantic={'decision':decision,'semantic_meshes':terrain_report.get('semanticMeshes',[]),'horizon_segments':horizon_segments,'route_connected':route_visible,'major_semantic_forms_visible':debug_bands,'neutral_lighting_readable':review_light}
    (OUT/'semantic-terrain-report.json').write_text(json.dumps(semantic,indent=2)+'\n')
    (OUT/'image-space-metrics.json').write_text(json.dumps(image_metrics,indent=2)+'\n')
    contact(DEBUG,'pass1e_debug_contact_sheet.png',4)
    contact(REVIEW,'pass1e_terrain_review_contact_sheet.png',3)
    (OUT/'test-summary.txt').write_text('\n'.join(['Phase 3A Pass 1E semantic terrain artifact',f'UTC timestamp: {datetime.now(timezone.utc).isoformat()}',f'Decision: {decision}',f'Runtime flag count: {os.environ.get("RUNTIME_FLAG_COUNT","unknown")}'])+'\n')
    files=[]
    for name in REQUIRED:
        path=OUT/name
        if not path.exists(): raise SystemExit(f'Missing required artifact file: {path}')
        files.append({'filename':name,'dimensions':dims(path),'byte_size':path.stat().st_size,'sha256':sha(path)})
    manifest={'commit_sha':os.environ.get('GITHUB_SHA',subprocess.getoutput('git rev-parse HEAD')),'workflow_run_id':os.environ.get('GITHUB_RUN_ID'),'utc_timestamp':datetime.now(timezone.utc).isoformat(),'godot_version':godot_version(),'capture_command':os.environ.get('CAPTURE_COMMAND','xvfb-run -a godot --path . --script tools/visual_review/capture_phase3a_pass1e_semantic_terrain.gd'),'runtime_flag_count':os.environ.get('RUNTIME_FLAG_COUNT','unknown'),'artifact_name':os.environ.get('ARTIFACT_NAME','phase3a-pass1e-semantic-terrain-local'),'files':files,'filenames':[f['filename'] for f in files]+['manifest.json']}
    (OUT/'manifest.json').write_text(json.dumps(manifest,indent=2)+'\n')
    print('PASS1E_ARTIFACT_MANIFEST_OK', OUT/'manifest.json')
    print('PASS1E_DECISION', decision)
if __name__ == '__main__': main()
