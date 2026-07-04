# TC_HeavyCrashHull_V1 Asset Brief

## Asset Identity

- Asset name: TC_HeavyCrashHull_V1
- Owner: TitanCraft Blender Asset Forge
- Status: production-candidate
- Source recipe: `tools/blender/create_heavy_crash_hull_v1.py`
- Generated source path: `assets/Source/Blender/Production/TC_HeavyCrashHull_V1.blend` (downloadable CI artifact; not committed because GitHub diff review does not support binary files)
- Production export path: `assets/Production/Generated/CrashWreck/TC_HeavyCrashHull_V1.glb`

## Purpose

- Gameplay purpose: static visual crash-site hero wreck module.
- Visual purpose: heavy broken industrial exploration vessel hull.
- MVP scope note: standalone art candidate only; not approved Stage A scene art and not integrated into `Main.tscn`.

## Scale and Placement

- Scale in meters: roughly 10–14 meters long, 4–6 meters wide, 3–5 meters tall.
- Origin/pivot rules: centered at the ground contact/burial line for terrain placement auditions.
- Godot placement assumptions: visual-only crash-site module; lower edge is suitable for partial terrain burial.

## Camera Review Angles

- Neutral front/three-quarter: broad damaged front and torn side breach must read in neutral grey.
- Neutral side/silhouette: heavy length, flattened underside, and non-capsule silhouette must read without materials.
- Rear/engine view: mechanical connection/engine mount must read as structured industrial hardware.
- Neutral top/scale: top-down review confirms 10–14 m by 4–6 m footprint and asymmetry.
- Material review: off-white hull, graphite underside/interior, worn steel ribs/panels, muted orange markings, and optional localized cyan breach slot.
- Godot import preview: import-only validation; no production scene placement.

## Shape Language

- Silhouette target: broad angular hull section with flattened underside, crushed front, torn side breach, visible thickness, exposed internal ribs, asymmetry, panel seams, and structured rear engine/mechanical connection.
- Forbidden shapes: smooth capsule spaceship, toy rocket, elegant fighter ship, random cubes glued together, paper-thin panels, glossy plastic sci-fi, pure cylinder engine with no structure, excessive cyan glow, and blockout-looking primitive silhouette.
- Readability risks: neutral-grey side view must still show industrial mass, breach, ribs, and panel thickness; material pass must not hide weak geometry.

## Materials

- Material slots:
  - `TC_MAT_worn_off_white_hull`
  - `TC_MAT_graphite_underside_interior`
  - `TC_MAT_worn_steel_ribs_panels`
  - `TC_MAT_muted_orange_markings`
  - `TC_MAT_localized_cyan_breach_slot`
- Material naming rules: all authored material slots use stable `TC_MAT_` prefixes.
- Texture policy: no external textures; project-authored procedural material colors only.

## Technical Contract

- Collision policy: no collision generated.
- Export format: GLB/glTF.
- Poly budget: low-to-medium poly, strong silhouette over detail.
- LOD requirement: none for this candidate.
- Runtime mesh generation allowed: no.
- Scripts inside imported art allowed: no.

## Screenshot Requirements

- Neutral front three-quarter PNG: `artifacts/asset-review/TC_HeavyCrashHull_V1/neutral_front_three_quarter.png`
- Neutral side silhouette PNG: `artifacts/asset-review/TC_HeavyCrashHull_V1/neutral_side_silhouette.png`
- Neutral rear engine PNG: `artifacts/asset-review/TC_HeavyCrashHull_V1/neutral_rear_engine.png`
- Material front three-quarter PNG: `artifacts/asset-review/TC_HeavyCrashHull_V1/material_front_three_quarter.png`
- Material side silhouette PNG: `artifacts/asset-review/TC_HeavyCrashHull_V1/material_side_silhouette.png`
- Material rear engine PNG: `artifacts/asset-review/TC_HeavyCrashHull_V1/material_rear_engine.png`
- Any production-scene screenshots: none; this asset must not be integrated into Stage A or `Main.tscn`.

## Evidence

- Source/licence: project-authored in Blender by TitanCraft Asset Forge from the text recipe `tools/blender/create_heavy_crash_hull_v1.py`; no third-party source assets.
- Source hash: recorded in `assets/Production/Generated/asset_manifest.json` for the generated downloadable `.blend` artifact.
- Export hash: recorded in `assets/Production/Generated/asset_manifest.json`.
- Manifest entry: `assets/Production/Generated/asset_manifest.json`.
- Godot import command result: `godot --headless --path . --import` validation required before review handoff.

## Final Verdicts

Use one of:

- TC_HEAVY_CRASH_HULL_V1_READY_FOR_HUMAN_REVIEW
- TC_HEAVY_CRASH_HULL_V1_NOT_GO
- BLOCKED_BY_BLENDER_TOOLING
- BLOCKED_BY_ASSET_FORGE_VALIDATION
