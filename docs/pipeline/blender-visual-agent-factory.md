# Blender Visual Agent Factory — Stage A

Owner: TitanCraft Project Director / Visual Director  
Version: 1.0  
Date: 2026-07-05  
Review status: Factory workflow ready; not asset, scene, or visual approval

## Purpose

The Blender Visual Agent Factory is the rapid asset-iteration layer between Stage A art briefs and future Godot scene integration. It exists to move Crash Site assets beyond procedural text OBJ blockout quality into authored Blender source assets with repeatable review renders, metadata, revision notes, export gates, and import evidence.

This factory does not authorize gameplay changes, scene replacement, new collision, new map areas, or Stage A visual approval.

## Agent roles

| Role | Authority | Required output |
|---|---|---|
| `visual_director` | Owns art thesis, scorecard, approval boundaries, and visual blockers. | Brief acceptance, scorecard verdict, revision notes. |
| `blender_asset_artist` | Authors or scripts Blender source assets within the brief. | `.blend` source or creation script, material slots, dimensions, mesh stats. |
| `scene_compositor` | Plans later placement and screenshot angles without editing production scenes before approval. | Integration notes, scale reference, camera list. |
| `qa_lead` | Verifies gates, hashes, manifests, and non-approval language. | Evidence checklist, command results, blocker classification. |
| `technical_director` | Owns export/import safety and no-runtime/no-collision policy. | Export/import verdict, binary policy confirmation. |

## Asset lifecycle

1. **Brief** — create a Stage A asset brief from `docs/art/briefs/stage-a-blender-asset-review-template.md`.
2. **Blender source/script** — author under `assets/Source/Blender/` or via a checked-in creation script under `tools/blender/`.
3. **Review renders** — generate required PNGs with `tools/blender/render_asset_review.py`.
4. **Visual scorecard** — score the asset and record blockers.
5. **Revision** — revise source/script until scorecard reaches review-ready quality.
6. **Export** — export GLB with `tools/blender/export_asset.py` only after review-ready status.
7. **Godot import test** — run `godot --headless --path . --import` and verify no scripts, runtime mesh generation, or accidental collision.
8. **Optional scene integration** — only after asset review; do not edit `Main.tscn` during standalone asset review.
9. **Gameplay smoke if scene changed** — full MVP smoke is mandatory if any production scene changes.

## Required review renders

Every asset review package must contain:

- `front.png`
- `back.png`
- `left.png`
- `right.png`
- `top.png`
- `hero_three_quarter.png`
- `scale_reference_player_capsule.png`
- `material_preview.png`
- `wireframe_or_mesh_stats.json`
- `contact_sheet.png`

Use `tools/blender/build_asset_review_contact_sheet.py` to combine review PNGs after rendering.

## Visual scorecard

Score each category from 0 to 5 and include a note for any score below 4.

| Category | Required question |
|---|---|
| Silhouette readability | Is the form readable in neutral view without decals? |
| Non-toy industrial/alien read | Does it avoid toy, capsule, showroom, or random primitive reads? |
| Material coherence | Does it follow the Crash Site palette and rough simplified PBR direction? |
| Scale | Does it read correctly beside the player capsule and route widths? |
| Gameplay readability | Does it support the locked MVP objective/route/combat read without adding mechanics? |
| Crash Site palette integration | Does it fit ash gray, basalt black, worn human tech, or restrained alien purple/cyan? |
| Forbidden-shape avoidance | Does it avoid flat boards, floating cards, cones, default cubes, voxel/grid, and noisy scans? |
| Screenshot value | Does it improve foreground/midground/background composition in review renders? |

## Required metadata

Each asset review folder must record:

- asset name;
- source file path;
- generated/exported file paths;
- SHA256 hash for source/export/review artifacts when generated;
- triangle count;
- material slots;
- dimensions;
- authoring method (`hand-authored Blender`, `script-authored Blender`, or `hybrid`);
- whether binary files are committed or only generated as artifacts;
- approval verdict.

## Verdicts

Use only these asset and slice verdicts:

- `BLENDER_ASSET_REVIEW_READY`
- `BLENDER_ASSET_IMPLEMENTATION_PASS`
- `BLENDER_ASSET_NOT_GO`
- `BLENDER_ASSET_APPROVED`
- `GODOT_IMPORT_PASS`
- `VISUAL_SLICE_GAMEPLAY_SAFE`
- `STAGE_A_VISUAL_NOT_GO`
- `STAGE_A_VISUAL_APPROVED`

`BLENDER_ASSET_APPROVED` and `STAGE_A_VISUAL_APPROVED` require human or assigned visual-review authority after opened PNG inspection.

## Binary/source policy

| Artifact | Policy |
|---|---|
| `.blend` source | Store under `assets/Source/Blender/`; do not commit new production binary sources unless a human explicitly approves the specific binary. Prefer reproducible creation scripts and CI artifacts for review. |
| `.glb` exports | Generate under `assets/Production/Generated/`; do not integrate into production scenes until review gates pass. Commit only if existing repo policy and human review allow it. |
| Review PNGs | Generated under `artifacts/asset-review/<asset>/`; keep out of Git and upload as CI/workflow artifacts. |
| Manifests/hashes | Commit text manifests when they are source-of-truth review records; CI-generated manifests may also be uploaded with artifact bundles. |
| Collision | Standalone Stage A visual assets must set collision policy to `none` unless a future explicit gameplay task scopes collision and runs MVP smoke validation. |

## First target asset

The first factory target is `Stage A Terrain Diorama Kit V1`, defined in `docs/art/briefs/stage-a-terrain-diorama-kit-v1-brief.md`.

## Hard visual target

The factory must specifically defeat:

- flat board terrain;
- floating card route edges;
- weak hull burial;
- debug primitive silhouettes;
- sparse dark void background;
- toy/blockout read.

## Validation commands

```bash
python3 tools/blender/validate_stage_a_blender_asset.py assets/Source/Blender/Production/<asset>.blend
blender --background --python tools/blender/render_asset_review.py -- assets/Source/Blender/Production/<asset>.blend artifacts/asset-review/<asset>
python3 tools/blender/build_asset_review_contact_sheet.py artifacts/asset-review/<asset>
blender --background --python tools/blender/export_asset.py -- assets/Source/Blender/Production/<asset>.blend assets/Production/Generated/<category>/<asset>.glb
godot --headless --path . --import
```

## Final factory verdict

`BLENDER_VISUAL_AGENT_FACTORY_READY` for workflow readiness only. No Stage A visual approval is implied.
