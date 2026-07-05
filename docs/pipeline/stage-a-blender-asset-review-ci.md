# Stage A Blender Asset Review CI

Owner: TitanCraft Project Director / Visual Director  
Version: 1.0  
Date: 2026-07-05  
Review status: CI review workflow documentation; not visual approval and not game integration

## Purpose

This document explains how the Stage A terrain diorama Blender review package is generated in GitHub Actions. The workflow creates downloadable review evidence for `TC_TerrainDioramaKit_V1` without committing binary `.blend`, `.glb`, or PNG outputs and without integrating the asset into the Godot game.

## Workflow

- Workflow file: `.github/workflows/stage-a-blender-asset-review.yml`
- Workflow name: `TitanCraft Stage A Blender Asset Review`
- Job purpose: Run the Stage A terrain diorama Blender recipe in an Ubuntu CI runner with Blender installed, validate the generated `.blend`, render review PNGs, build a contact sheet, write hashes/metadata, and upload one artifact bundle.

## Trigger method

The workflow runs from:

- `workflow_dispatch` for manual review-artifact generation.
- `pull_request` when relevant Blender tooling, Stage A review documentation, CI documentation, or the workflow file changes.

## Artifact name

`stage-a-terrain-diorama-kit-v1-review`

## Expected artifact contents

The uploaded artifact should include:

- `artifacts/asset-review/TC_TerrainDioramaKit_V1/front.png`
- `artifacts/asset-review/TC_TerrainDioramaKit_V1/back.png`
- `artifacts/asset-review/TC_TerrainDioramaKit_V1/left.png`
- `artifacts/asset-review/TC_TerrainDioramaKit_V1/right.png`
- `artifacts/asset-review/TC_TerrainDioramaKit_V1/top.png`
- `artifacts/asset-review/TC_TerrainDioramaKit_V1/hero_three_quarter.png`
- `artifacts/asset-review/TC_TerrainDioramaKit_V1/scale_reference.png`
- `artifacts/asset-review/TC_TerrainDioramaKit_V1/material_preview.png`
- `artifacts/asset-review/TC_TerrainDioramaKit_V1/contact_sheet.png`
- `artifacts/asset-review/TC_TerrainDioramaKit_V1/mesh_stats_report.md`
- `artifacts/asset-review/TC_TerrainDioramaKit_V1/review_metadata.md`
- `artifacts/asset-review/TC_TerrainDioramaKit_V1/sha256sums.txt`
- `artifacts/asset-review/TC_TerrainDioramaKit_V1/generated_file_list.txt`
- `assets/Source/Blender/StageA/TC_TerrainDioramaKit_V1.blend`
- `assets/Production/Generated/StageA/TC_TerrainDioramaKit_V1.glb`
- `artifacts/asset-review/logs/blender-version.log`

Binary `.blend` and `.glb` files are uploaded as workflow artifacts for review/provenance only. They are not committed to the repository by this workflow.

## How to download the contact sheet

1. Open the GitHub Actions run for `TitanCraft Stage A Blender Asset Review`.
2. Download the artifact named `stage-a-terrain-diorama-kit-v1-review` from the run's Artifacts section.
3. Extract the artifact archive locally.
4. Open `artifacts/asset-review/TC_TerrainDioramaKit_V1/contact_sheet.png`.

## Opened-image critique procedure

After downloading and opening the contact sheet and individual PNGs, record critique in `docs/art/reviews/stage-a-terrain-diorama-kit-v1-review.md` or the PR review using these required dimensions:

1. Focal point: Name the first-read shape in the front, hero, and scale-reference views.
2. Route readability: Judge whether route-edge markers feel grounded and whether they avoid floating-card or decorated-slab reads.
3. Silhouette: Judge the basin rim, basalt outcrops, and distant silhouettes against the flat-board/debug-primitive failure modes.
4. Scale: Compare the player capsule to ash drifts, route markers, basalt outcrops, rim height, and hull-contact mounds.
5. Material coherence: Judge whether matte ash, basalt, scorch, dusty highlight, and violet silhouettes fit the Crash Site palette without glossy toy sci-fi.
6. Contact support: Judge whether hull burial/contact mounds read as weight-bearing terrain support rather than decorative piles.
7. Verdict separation: Keep implementation/build results separate from visual readiness.

## Non-claims

This workflow and its artifact must not be used to claim:

- The asset is visually approved.
- Stage A is visually complete.
- Generated PNGs equal visual approval.
- Godot integration is complete.
- `Main.tscn` has been updated.
- Production scenes have been modified.
- Gameplay collision or gameplay scope has changed.

## Final CI documentation verdict

`BLENDER_ASSET_REVIEW_CI_READY` for CI artifact generation workflow documentation only. Visual verdict remains blocked until a reviewer opens the generated PNGs and records a visual critique.
