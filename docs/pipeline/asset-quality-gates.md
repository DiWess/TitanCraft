# Asset Quality Gates

## Gate 1 — Brief Completeness

The asset brief must define gameplay purpose, visual purpose, scale, review angles, silhouette target, forbidden shapes, material slots, origin rules, collision policy, export format, poly budget, screenshot requirements, and final verdicts.

## Gate 2 — Source Validation

`validate_blender_asset.py` must pass on the `.blend` source. A passing source has named `TC_` mesh nodes, material slots, clean object origins, no collision metadata other than `none`, and a reasonable triangle count for its budget.

## Gate 3 — Export Validation

`export_asset.py` must produce GLB/glTF from the `.blend` source. Production assets prefer GLB unless a documented exception exists.

## Gate 4 — Godot Import

`godot --headless --path . --import` must complete after export. Imported art must not create scripts, runtime mesh generation, or accidental collision resources.

## Gate 5 — Visual Evidence

Each reviewed asset needs neutral and material PNG review evidence. The PNGs are generated or downloaded as GitHub Actions artifacts and are not committed. Review must inspect the images directly before claiming an asset is ready.

## Gate 6 — Manifest Evidence

`build_asset_manifest.py` must emit `assets/Production/Generated/asset_manifest.json` with source path, source hash, production path, production hash, format, classification, license, and collision policy. For binary outputs that GitHub cannot review inline, the manifest is generated inside CI and uploaded in the same artifact bundle as the `.blend` and `.glb`.

## Final Pipeline Verdicts

Use only:

- `BLENDER_ASSET_FORGE_READY`
- `BLENDER_ASSET_FORGE_VALIDATION_FAILED`
- `BLOCKED_BY_BLENDER_TOOLING`
