#!/usr/bin/env python3
from __future__ import annotations
import hashlib, json, os, statistics, subprocess, sys
from datetime import datetime, timezone
from pathlib import Path
from PIL import Image, ImageDraw, ImageFont

OUT = Path(sys.argv[1]) if len(sys.argv) > 1 else Path('artifacts/visual-review/phase3a-pass1d-terrain-visibility')
DEBUG = ['pass1d_zone_id.png','pass1d_height_gradient.png','pass1d_normals.png','pass1d_wireframe_top.png','pass1d_route_mask.png','pass1d_neutral_lighting.png','pass1d_production_material_terrain_only.png']
PROD = ['pass1d_prod_01_spawn_route.png','pass1d_prod_02_resource_workbench.png','pass1d_prod_03_combat_zone.png','pass1d_prod_04_beacon_route.png']
MASKS = [p.replace('.png','_category_mask.png') for p in PROD]
CONTACTS = ['pass1d_debug_contact_sheet.png','pass1d_production_contact_sheet.png']
REQUIRED = [*DEBUG,*PROD,*MASKS,*CONTACTS,'terrain-visibility-report.json','category-coverage-report.json','runtime-contract-report.json','terrain-generation-report.json','capture.log','test-summary.txt']

COLORS = {'terrain':(0,255,0),'ship':(255,0,0),'player':(0,0,255),'enemy':(255,0,255),'interactables':(255,255,0),'other_props':(0,255,255)}

def sha(path: Path) -> str:
    h=hashlib.sha256(); h.update(path.read_bytes()); return h.hexdigest()

def dims(path: Path):
    if path.suffix.lower() != '.png': return None
    with Image.open(path) as img: return {'width': img.width, 'height': img.height}

def lum(rgb): return (0.2126*rgb[0]+0.7152*rgb[1]+0.0722*rgb[2])/255.0

def metrics(path: Path):
    with Image.open(path) as img:
        pix=list(img.convert('RGB').getdata())
    vals=sorted(lum(p) for p in pix)
    n=len(vals); mean=sum(vals)/n
    distinct=len({(p[0]//24,p[1]//24,p[2]//24) for p in pix if max(p)-min(p)>10})
    terrain_pixels=sum(1 for p in pix if max(p)>18)
    route_pixels=sum(1 for p in pix if p[0]>225 and p[1]>225 and p[2]>225)
    rows=[]
    with Image.open(path) as img:
        data=img.convert('RGB')
        for y in range(data.height):
            row=[data.getpixel((x,y)) for x in range(data.width)]
            if any(max(p)>18 for p in row): rows.append(y)
    return {
        'mean_luminance':mean,
        'p05_luminance':vals[int(n*0.05)],
        'p95_luminance':vals[int(n*0.95)],
        'pixels_below_0_03_pct':100*sum(v<0.03 for v in vals)/n,
        'pixels_below_0_08_pct':100*sum(v<0.08 for v in vals)/n,
        'visible_terrain_pixel_pct':100*terrain_pixels/n,
        'route_pixel_pct':100*route_pixels/n,
        'horizon_silhouette_height_px':(max(rows)-min(rows)+1) if rows else 0,
        'visibly_distinct_zone_colours':distinct,
    }

def coverage(path: Path):
    with Image.open(path) as img:
        pix=list(img.convert('RGB').getdata())
    total=len(pix); result={}
    for name, color in COLORS.items():
        result[name+'_coverage_pct']=100*sum(sum(abs(p[i]-color[i]) for i in range(3)) < 36 for p in pix)/total
    result['camera_invalid']=any(result[k] > 35 for k in result if k.endswith('_coverage_pct') and not k.startswith('terrain'))
    return result

def route_luminance_delta(production_path: Path, route_mask_path: Path):
    with Image.open(production_path) as prod, Image.open(route_mask_path) as mask:
        production=list(prod.convert('RGB').getdata())
        route_mask=list(mask.convert('RGB').getdata())
    route=[lum(p) for p,m in zip(production, route_mask) if m[0]>225 and m[1]>225 and m[2]>225]
    basalt=[lum(p) for p,m in zip(production, route_mask) if 18<max(m)<80]
    route_avg=sum(route)/len(route) if route else 0.0
    basalt_avg=sum(basalt)/len(basalt) if basalt else 0.0
    return {
        'route_average_luminance': route_avg,
        'adjacent_basalt_average_luminance': basalt_avg,
        'route_minus_basalt_luminance': route_avg - basalt_avg,
    }

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
    terrain_metrics={name:metrics(OUT/name) for name in DEBUG}
    production_material=terrain_metrics['pass1d_production_material_terrain_only.png']
    route_delta=route_luminance_delta(OUT/'pass1d_production_material_terrain_only.png', OUT/'pass1d_route_mask.png')
    zone_ok=terrain_metrics['pass1d_zone_id.png']['visibly_distinct_zone_colours'] >= 6
    route_ok=terrain_metrics['pass1d_route_mask.png']['route_pixel_pct'] > 1.0
    height_ok=terrain_metrics['pass1d_height_gradient.png']['p95_luminance'] - terrain_metrics['pass1d_height_gradient.png']['p05_luminance'] >= 0.15
    geometry_class='GEOMETRY_VALID_MATERIAL_OR_LIGHTING_FAILURE' if zone_ok and route_ok and height_ok else 'GEOMETRY_VISUALLY_INSUFFICIENT'
    material_ok=production_material['pixels_below_0_08_pct'] < 55 and production_material['p95_luminance'] - production_material['p05_luminance'] >= 0.15 and production_material['visibly_distinct_zone_colours'] >= 3 and route_delta['route_minus_basalt_luminance'] >= 0.07
    material_config={'base_albedo':'Color.WHITE','vertex_color_use_as_albedo':True,'vertex_color_multiplied_by_base_albedo':True,'roughness':0.92,'metallic':0.0,'shading_mode':'per-pixel lit StandardMaterial3D','cull_mode':'back','normal_mode':'flat/faceted mesh normals','ambient_contribution':'existing WorldEnvironment plus capture-only neutral diagnostic lights'}
    report={'geometry_classification':geometry_class,'production_material_requirements_pass':material_ok,'material_configuration':material_config,'production_route_luminance':route_delta,'renders':terrain_metrics}
    (OUT/'terrain-visibility-report.json').write_text(json.dumps(report,indent=2)+'\n')
    cover={name:coverage(OUT/name.replace('.png','_category_mask.png')) for name in PROD}
    (OUT/'category-coverage-report.json').write_text(json.dumps({'production_cameras':cover,'any_camera_invalid':any(v['camera_invalid'] for v in cover.values())},indent=2)+'\n')
    contact(DEBUG,'pass1d_debug_contact_sheet.png',4)
    contact(PROD,'pass1d_production_contact_sheet.png',2)
    (OUT/'test-summary.txt').write_text('\n'.join(['Phase 3A Pass 1D terrain visibility diagnostic',f'UTC timestamp: {datetime.now(timezone.utc).isoformat()}',f'Geometry classification: {geometry_class}',f'Production material requirements pass: {material_ok}',f'Runtime flag count: {os.environ.get("RUNTIME_FLAG_COUNT","unknown")}'])+'\n')
    files=[]
    for name in REQUIRED:
        path=OUT/name
        if not path.exists(): raise SystemExit(f'Missing required artifact file: {path}')
        files.append({'filename':name,'dimensions':dims(path),'byte_size':path.stat().st_size,'sha256':sha(path)})
    manifest={'commit_sha':os.environ.get('GITHUB_SHA',subprocess.getoutput('git rev-parse HEAD')),'workflow_run_id':os.environ.get('GITHUB_RUN_ID'),'utc_timestamp':datetime.now(timezone.utc).isoformat(),'godot_version':godot_version(),'capture_command':os.environ.get('CAPTURE_COMMAND','xvfb-run -a godot --path . --script tools/visual_review/capture_phase3a_pass1d_terrain_visibility.gd'),'runtime_flag_count':os.environ.get('RUNTIME_FLAG_COUNT','unknown'),'artifact_name':os.environ.get('ARTIFACT_NAME','phase3a-pass1d-terrain-visibility-local'),'files':files,'filenames':[f['filename'] for f in files]+['manifest.json']}
    (OUT/'manifest.json').write_text(json.dumps(manifest,indent=2)+'\n')
    print('PASS1D_ARTIFACT_MANIFEST_OK', OUT/'manifest.json')
    print('GEOMETRY_CLASSIFICATION', geometry_class)
    print('PRODUCTION_MATERIAL_REQUIREMENTS_PASS', material_ok)
if __name__ == '__main__': main()
