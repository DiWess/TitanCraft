# Stage A Terrain Diorama Kit V1 Review

Owner: TitanCraft Project Director / Visual Director  
Version: 1.0  
Date: 2026-07-05  
Review status: CI artifact workflow added; local visual review remains blocked by missing Blender runtime in the current environment; not Stage A approval

## Asset summary

- Asset name: `TC_TerrainDioramaKit_V1`
- Purpose: Standalone Stage A terrain asset review package for a volcanic ash basin, fractured basalt crash-site kit, and player scale reference.
- Scope: Asset production/review only; no `Main.tscn` integration, no production scene replacement, no gameplay code, no collision resources, no Stage A visual approval claim.
- Source recipe: `tools/blender/create_stage_a_terrain_diorama_kit_v1.py`
- Intended source path: `assets/Source/Blender/StageA/TC_TerrainDioramaKit_V1.blend`
- Intended generated/export path: `assets/Production/Generated/StageA/TC_TerrainDioramaKit_V1.glb`
- Intended review output directory: `artifacts/asset-review/TC_TerrainDioramaKit_V1/`
- CI workflow: `.github/workflows/stage-a-blender-asset-review.yml`
- CI artifact name: `stage-a-terrain-diorama-kit-v1-review`

## Required kit pieces represented in source recipe

1. Main concave ash basin: `TC_TerrainDioramaKit_V1_main_concave_ash_basin`.
2. Raised fractured rim segments: `TC_TerrainDioramaKit_V1_raised_fractured_rim_segment_*`.
3. Basalt outcrops: `TC_TerrainDioramaKit_V1_basalt_outcrop_*`.
4. Ash drift mounds: `TC_TerrainDioramaKit_V1_ash_drift_mound_*`.
5. Hull burial/contact mounds: `TC_TerrainDioramaKit_V1_hull_burial_contact_mound_*` plus a non-production embedded hull proxy scale marker.
6. Route-edge grounded markers: `TC_TerrainDioramaKit_V1_route_edge_grounded_marker_*`.
7. Distant basalt silhouette pieces: `TC_TerrainDioramaKit_V1_distant_basalt_silhouette_piece_*`.
8. Player capsule scale reference: `TC_TerrainDioramaKit_V1_player_capsule_scale_reference_*`.

## Render artifact paths

Expected paths after Blender execution:

- Front render: `artifacts/asset-review/TC_TerrainDioramaKit_V1/front.png`
- Back render: `artifacts/asset-review/TC_TerrainDioramaKit_V1/back.png`
- Left render: `artifacts/asset-review/TC_TerrainDioramaKit_V1/left.png`
- Right render: `artifacts/asset-review/TC_TerrainDioramaKit_V1/right.png`
- Top render: `artifacts/asset-review/TC_TerrainDioramaKit_V1/top.png`
- 3/4 hero render: `artifacts/asset-review/TC_TerrainDioramaKit_V1/hero_three_quarter.png`
- Scale reference render with player capsule: `artifacts/asset-review/TC_TerrainDioramaKit_V1/scale_reference.png`
- Material preview render: `artifacts/asset-review/TC_TerrainDioramaKit_V1/material_preview.png`
- Mesh stats report: `artifacts/asset-review/TC_TerrainDioramaKit_V1/mesh_stats_report.md`
- Contact sheet: `artifacts/asset-review/TC_TerrainDioramaKit_V1/contact_sheet.png`

## Mesh stats

Blender was unavailable in this environment (`blender: command not found`), so generated `.blend`, `.glb`, render PNGs, contact sheet, and measured mesh stats were not produced locally.

Expected validation source for mesh stats after Blender is available:

```bash
blender --background --python tools/blender/render_asset_review.py -- assets/Source/Blender/StageA/TC_TerrainDioramaKit_V1.blend artifacts/asset-review/TC_TerrainDioramaKit_V1
```

- Triangle count: `ENVIRONMENT_BLOCKED`
- Material slots: `ENVIRONMENT_BLOCKED`
- Dimensions: `ENVIRONMENT_BLOCKED`
- Mesh count: `ENVIRONMENT_BLOCKED`

## Hashes

No generated binary or PNG artifacts exist in this environment because Blender is unavailable.

- `assets/Source/Blender/StageA/TC_TerrainDioramaKit_V1.blend`: `ENVIRONMENT_BLOCKED`
- `assets/Production/Generated/StageA/TC_TerrainDioramaKit_V1.glb`: `ENVIRONMENT_BLOCKED`
- Review PNG hashes: `ENVIRONMENT_BLOCKED`

## Binary/source policy decision

- Commit policy: Commit the text Blender generation script and review documentation only.
- Do not commit `.blend` or `.glb` binaries unless explicitly allowed by current repo policy or human approval.
- If Blender is available locally or in CI, generated `.blend`, `.glb`, PNGs, mesh stats, and contact sheet should remain local/workflow artifacts unless a human explicitly approves committing binaries.

## Opened-image critique

No PNGs could be opened because the current environment lacks Blender and therefore could not generate the required review images.

Expected critique dimensions once images are generated and opened:

- Focal point: Verify the main basin/rim silhouette leads the eye before small markers.
- Route readability: Verify route-edge markers feel embedded and not floating card edges.
- Silhouette: Verify rim, outcrop, and distant silhouette pieces defeat dark void and debug primitive reads.
- Scale: Verify player capsule communicates knee/chest/rim scale relationships.
- Material coherence: Verify matte ash, dark basalt, dusty highlights, muted scorch, and restrained violet fit Crash Site palette without glossy toy sci-fi.

## Scorecard

Scorecard status: `PROVISIONAL_SOURCE_RECIPE_ONLY`

Final visual scoring status: `BLOCKED_PENDING_BLENDER_RENDERS`

These scores are source-recipe triage only. They are not visual approval and must be replaced or confirmed only after the CI artifact PNGs are downloaded, opened, and critiqued.

| Criterion | Score | Rationale |
|---|---:|---|
| Silhouette readability | 6 | Source recipe includes varied rim/outcrop/background silhouettes, but PNG evidence is unavailable. |
| Terrain basin read | 6 | Source recipe builds a concave multi-ring ash basin, but render inspection is blocked. |
| Hull burial/contact support | 5 | Contact mounds and an embedded proxy are scripted, but actual hull contact read is unverified. |
| Route-edge grounding | 5 | Grounded marker meshes are scripted, but floating/card risks cannot be judged without opened PNGs. |
| Non-toy read | 5 | Matte low-poly materials and irregular forms are scripted; visual toy/blockout risk remains unreviewed. |
| Material coherence | 6 | Material slots align to ash/basalt/scorch/violet palette, but no material preview render exists. |
| Crash Site palette fit | 6 | Palette follows Crash Site art documents; evidence is script-level only. |
| Foreground/midground/background usefulness | 5 | Basin, outcrops, and distant pieces are represented; composition is unproven without renders. |
| Forbidden-shape avoidance | 5 | Script avoids voxels and route slabs, but uses simple low-poly primitives that need visual review. |
| Screenshot value | 4 | Render tooling is present, but screenshot output is blocked in this environment. |

## Implementation verdict

`ENVIRONMENT_BLOCKED`

Reason: The source recipe, validation helper, review renderer, contact-sheet builder, and review record are implemented as text files, but required Blender generation/render validation cannot run because `blender` is not installed in the execution environment.

## Visual verdict

`BLENDER_ASSET_NOT_GO`

Reason: Generated PNGs were not produced or opened locally. CI is now configured to generate review artifacts, but final visual scoring remains `BLOCKED_PENDING_BLENDER_RENDERS` until the artifact PNGs are downloaded, opened, and critiqued. This is not Stage A visual approval and not a claim that the terrain kit is visually ready.
