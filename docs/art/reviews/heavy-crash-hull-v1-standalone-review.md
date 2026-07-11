# TC_HeavyCrashHull_V1 — Standalone Artifact Review

**Date:** 2026-07-10
**Reviewer role:** Visual Reviewer (routed packet: `visual_scene_composition`, primary agent Art Director)
**Brief:** `docs/art/briefs/TC_HeavyCrashHull_V1.md`
**Quality benchmark axis:** #6 Visual art & presentation — target 9.0, peer anchors per
`studio/decisions/quality_benchmark_v1.md`; current axis status 2.0. This review closes the
"standalone artifact review" gap named in `docs/production/current-status.md`; it does not claim
the axis target is met.

## Provenance

- Source recipe (tracked): `tools/blender/create_heavy_crash_hull_v1.py` — project-authored,
  no third-party assets, no external licences.
- Tracked Blender source: `assets/Source/Blender/Production/TC_HeavyCrashHull_V1.blend`
  SHA-256 `0d6193fd32f58d02a42fffbf949a5f6357da8e87dc0b789494902bdb0b59dae9`
- Tracked GLB export: `assets/Production/Generated/CrashWreck/TC_HeavyCrashHull_V1.glb`
  SHA-256 `69b77c1dc737f23221da6cdf0a7a5676babc2ec69ce8a7132bdbb12e54d120f5`
- Validation: `python3 tools/blender/validate_blender_asset.py` → `BLENDER_ASSET_VALID`,
  0 issues, 60 meshes, 904 triangles, 5 materials
  (`TC_MAT_worn_off_white_hull`, `TC_MAT_worn_steel_ribs_panels`,
  `TC_MAT_graphite_underside_interior`, `TC_MAT_muted_orange_markings`,
  `TC_MAT_localized_cyan_breach_slot`).
- Render environment: Blender 4.2.22 LTS as the `bpy` Python module (headless Linux container,
  EEVEE, software GL). The tracked `.blend` binary was **not** modified; renders open the
  committed source read-only.

## Review Evidence (opened PNGs)

Six renders in `artifacts/asset-review/TC_HeavyCrashHull_V1/`, 1280×720, neutral-grey and
material passes of three auto-framed views. Cameras frame from evaluated scene bounds and every
view includes a render-only 1.8 m orange scale post (never part of the asset data), per the
Stage A terrain-diorama lesson that unframed or scale-less review PNGs are not reviewable
(MEM-VISFAIL series).

| View | Neutral | Material |
|---|---|---|
| Front three-quarter | `neutral_front_three_quarter.png` | `material_front_three_quarter.png` |
| Side silhouette | `neutral_side_silhouette.png` | `material_side_silhouette.png` |
| Rear engine | `neutral_rear_engine.png` | `material_rear_engine.png` |

## Visual Diagnosis (opened-image findings)

- **Focal point:** the torn side breach with exposed internal ribs is the dominant read in the
  front three-quarter and side views; the rear engine ring with fin debris is the secondary
  focal at the stern. The single cyan breach slot reads as a localized accent, not glow spam.
- **Silhouette:** full hull reads as a broad, flattened, asymmetric crashed transport — not a
  capsule, not a toy rocket. Crushed bow, burial-line underside, and scattered hull-shard
  debris satisfy the brief's silhouette target in the neutral pass without material help.
- **Scale:** against the 1.8 m reference post, the hull reads roughly 13–14 m long and 4–5 m
  tall, inside the brief's 10–14 m × 3–5 m envelope — genuinely "heavy" against player height.
- **Material coherence:** worn off-white hull, steel ribs/panels, graphite interior, muted
  orange markings, single cyan slot — consistent with the MVP Asset Pack palette and the Art
  Taste Pack contrast rule (interactive/landmark accents against desaturated structure).
- **Residual weaknesses (honest gaps vs axis-6 anchors):** panel seams are modifier-bevel only,
  there is no surface wear texturing, and interior rib spacing is regular enough to read
  slightly architectural in the direct side view. Acceptable for a low-poly production
  candidate; noted for any future V2 pass.

## Verdict

`PASS` — as a **standalone production asset candidate review bundle**. The brief's readability
requirements are met in opened PNGs with scale reference, provenance, hashes, and validation
output recorded above.

This verdict does **not** integrate the asset into `Main.tscn`, does not claim Stage A scene
replacement, and does not claim axis-6 target quality. Production scene placement remains a
separate gated task requiring its own before/after scene captures and sign-off per
`docs/production/current-status.md`.
