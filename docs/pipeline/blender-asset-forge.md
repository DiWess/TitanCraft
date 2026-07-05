# TitanCraft Blender Asset Forge

## Purpose

The Blender Asset Forge is TitanCraft's repeatable offline DCC pipeline for creating, reviewing, exporting, importing, and validating static visual assets before they enter Godot production scenes.

This pipeline is for authored assets only. It must not add gameplay code, runtime mesh generation, collision resources, or scripts inside imported art files.

## Tooling Record

- Verified Blender version: Blender 4.0.2
- Preferred production interchange format: GLB/glTF.
- Source root: `assets/Source/Blender/`.
- Generated production root: `assets/Production/Generated/`.
- Local review render root: `artifacts/asset-review/`.
- Downloadable CI artifact bundle: `blender-asset-forge-test-crate` from `.github/workflows/blender-asset-forge.yml`.

## Standard Asset Loop

1. Create an asset brief from `docs/pipeline/asset-brief-template.md`.
2. Author the source `.blend` under `assets/Source/Blender/`; binary `.blend` files are generated/downloaded as workflow artifacts unless a human explicitly approves committing a specific source binary.
3. Validate the source with `python3 tools/blender/validate_blender_asset.py <source.blend>`.
4. Export GLB with `blender --background --python tools/blender/export_asset.py -- <source.blend> <output.glb>`.
5. Render neutral/material turntables with `tools/blender/render_asset_turntable.py`.
6. Import into Godot with `godot --headless --path . --import`.
7. Build/update `assets/Production/Generated/asset_manifest.json`.
8. Download the generated `.blend`, `.glb`, manifest, and PNG evidence from the GitHub Actions artifact bundle; do not commit binary review/build outputs.

## Production Asset Candidates

## MVP Asset Pack V1

The eleven Crash Site MVP assets (workbench, dormant/active beacon, metal/biomass/electronics/component pickups, Galaxabrain Scout, mechanical arm, save point, crash debris landmark) are authored by `tools/blender/create_mvp_asset_pack_v1.py` under `assets/Source/Blender/Production/MVP_Pack_V1/`, exported to `assets/Production/Generated/MVP_Pack_V1/`, and reviewed with `tools/blender/render_mvp_asset_review.py` (Cycles CPU, headless-safe). Brief: `docs/art/briefs/mvp-asset-pack-v1.md`. Binaries are delivered through the `blender-asset-forge-assets` workflow artifact, not committed.

# Non-Production Test Asset

The repository contains exactly one pipeline test asset:

- Generated source: `assets/Source/Blender/_templates/TC_TestCrate.blend`
- Generated export: `assets/Production/Generated/Test/TC_TestCrate.glb`
- GitHub Actions artifact: `blender-asset-forge-test-crate`

This crate proves source creation, validation, GLB export, Godot import, thumbnail/turntable review, manifest entry, and asset-evidence discipline. It is not final TitanCraft art and must not be placed in production gameplay scenes.
