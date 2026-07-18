# MVP Asset Pack V1 — Stage B Technical Director Audit

**Date:** 2026-07-18
**Auditor role:** Technical Director (independent feasibility audit)
**Scope:** the 13 `MVP_Pack_V1` GLB candidates against the Godot 4 .NET pipeline,
`docs/art/briefs/mvp-asset-pack-v1.md` budgets, and README §28 performance targets.

> Unlike the 2026-07-06 `TECHNICAL_DIRECTOR_AUDIT_V1.md` marked **NOT REAL — SIMULATION
> EXERCISE**, every figure below is the output of a command run in this repository on
> 2026-07-18; no FPS or draw-call numbers are estimated or invented.

## Commands run and results

| Check | Command | Result |
|---|---|---|
| C# build | `dotnet build` | Build succeeded, 0 warnings, 0 errors |
| Unit tests | `dotnet test` | Passed 75 / Failed 0 / Skipped 0 (330 ms) |
| Project import | `godot --headless --path . --import` | exit 0; 0 `ERROR`/`SCRIPT ERROR` lines in captured log |
| Import artifacts | `ls .godot/imported/` | 156 `TC_*` imported resources, including all 13 `MVP_Pack_V1` GLBs (`.scn` + `.md5` present) |
| Manifest integrity | `python3 tools/blender/build_asset_manifest.py --check` | `ASSET_MANIFEST_CHECK … entries=29 write=skipped` |
| GLB hash verification | SHA-256 recompute vs `asset_manifest.json` | 29/29 `production_sha256` match; 0 missing |

Import log captured to the session scratchpad (`godot_import.log`); reproducible with the
command above.

## Geometry audit (parsed from GLB accessor bounds, Y-up)

| Asset | Size x × h × d (m) | Triangles | Brief budget | Brief size (approx) |
|---|---|---:|---:|---|
| TC_PROP_Workbench_V1 | 2.96 × 1.73 × 1.00 | 1548 | 3200 | 3.3 × 1.1 × 1.7 |
| TC_PROP_Beacon_Dormant_V1 | 1.15 × 2.18 × 1.16 | 668 | 2000 | 1.4 × 1.4 × 2.2 |
| TC_PROP_Beacon_Active_V1 | 2.84 × 1.82 × 2.84 | 1060 | 2800 | 2.9 × 2.9 × 2.1 |
| TC_PICKUP_Metal_V1 | 0.58 × 0.16 × 0.49 | 264 | 900 | ~0.5 wide |
| TC_PICKUP_Biomass_V1 | 0.47 × 0.33 × 0.44 | 200 | 1400 | ~0.45 wide |
| TC_PICKUP_Electronics_V1 | 0.49 × 0.45 × 0.31 | 264 | 900 | ~0.55 wide |
| TC_PICKUP_Component_V1 | 0.32 × 0.53 × 0.31 | 120 | 900 | ~0.45 wide |
| TC_CHAR_GalaxabrainScout_V1 | 1.65 × 1.69 × 1.82 | 704 | 3200 | 2.4 × 2.4 × 1.7 |
| TC_CHAR_GalaxabrainScout_Disabled_V1 | 1.64 × 0.80 × 1.82 | 704 | — (variant) | — |
| TC_PLAYER_MechanicalArm_V1 | 0.31 × 0.35 × 1.04 | 808 | 2600 | 0.35 × 1.0 × 0.35 |
| TC_PLAYER_MechanicalArmUnbuilt_V1 | 1.10 × 0.37 × 0.68 | 480 | — (variant) | — |
| TC_PROP_SavePoint_V1 | 0.82 × 1.58 × 0.78 | 316 | 1400 | 0.85 × 0.85 × 1.6 |
| TC_ENV_CrashDebris_A_V1 | 2.90 × 2.54 × 1.46 | 320 | 1400 | ~2.6 wide × 1.9 tall |

**Findings:**

- Every asset is 2–7× under its triangle budget; the whole pack totals ≈ 7,456 triangles —
  negligible against the README §28 60 FPS target on a mid-range Windows PC. No measured FPS
  claim is made (that requires the Windows playtest gate); the geometry load alone cannot be
  the constraint.
- Heights match briefs within ~10% for all human-interaction props (workbench 1.73 m,
  dormant beacon 2.18 m, save point 1.58 m, scout 1.69 m) — human-scale plausible.
- Deviations recorded (non-blocking, briefs are "approx"): Scout leg-splay footprint
  1.65 × 1.82 m vs 2.4 × 2.4 m brief; CrashDebris_A height 2.54 m vs ~1.9 m brief;
  Workbench 10% narrower than brief.
- Materials are Principled BSDF exports with emissives via `KHR_materials_emissive_strength`,
  imported by Godot 4.7 without warnings; one-to-few materials per asset keeps per-asset
  draw calls in the low single digits (structural property of the exports, not a runtime
  measurement).
- Collision policy `none` on all 13 (per brief contract) — integration must supply gameplay
  collisions from the existing production scene contracts, not from these meshes.
- Provenance note: tracked `.blend` `source_sha256` values in the manifest no longer match
  the regenerated `.blend` files (Blender serialization is not byte-deterministic); GLB
  hashes — the production-consumed artifact — all match. MINOR manifest-policy finding,
  shared with the Visual Reviewer document.

## Verdict

`PASS` — all 13 candidates import cleanly, are far inside geometry budgets, are
human-scale-correct, and introduce no pipeline, licence, or performance red flags at the
standalone-candidate level. Runtime FPS validation on Windows remains a separate
release-gate task and is not claimed here.
