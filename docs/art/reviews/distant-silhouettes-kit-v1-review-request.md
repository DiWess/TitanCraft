# Distant Silhouettes Kit V1 — Formal Review Request (Backfill)

**Requested:** 2026-07-16
**Requested by:** Claude Code (Code Reviewer & Architecture Validator) — filed per Producer-approved cleanup action, not a completed review
**Status:** **PENDING** — Visual Diagnosis and Verdict must be completed by a human or the Visual Reviewer agent from the opened images below.

## Why this exists

All three candidates are live in the production scene (4 instances) with only a narrative log entry (`docs/production/quality-scorecard-log.md:136`) and a mechanical manifest verdict (`..._REVIEW_ARTIFACTS_READY`, which the manifest's own schema only certifies as "exists and imports," not visually approved) — no formal `docs/art/reviews/*.md` verdict exists.

## Assets in this kit

| Asset | Source `.blend` | SHA-256 (source) |
|---|---|---|
| `TC_ENV_DistantSilhouette_AlienArc_V1` | `assets/Source/Blender/Production/DistantSilhouettes_V1/TC_ENV_DistantSilhouette_AlienArc_V1.blend` | `76a95e16e825b7f5fc9e8c42866337d28f88d341378d54c9e0f3e5d6a6859935` |
| `TC_ENV_DistantSilhouette_BasaltRidge_V1` | `.../TC_ENV_DistantSilhouette_BasaltRidge_V1.blend` | `90266876b6babe56cad0092d481c5ed2ffe38ad50d9e8a26fa5130d88562b3c4` |
| `TC_ENV_DistantSilhouette_SmokePlume_V1` | `.../TC_ENV_DistantSilhouette_SmokePlume_V1.blend` | `69ff1143d7a8699d0695e7468d46ae8a200fd540ada75f9f2a7311fbd1eb3d19` |

Source recipe: `tools/blender/create_distant_silhouettes_kit_v1.py`. License: project-authored. Collision policy: none.

## Integration (already live)

`scenes/Main/Main.tscn`, four `Node3D` instances:
- `DistantRock_4` → BasaltRidge (line 691)
- `DistantRock_5` → AlienArc (line 695)
- `DistantRock_6` → SmokePlume (line 699)
- `DistantRock_7` → BasaltRidge (line 703, repeats #4's mesh)

## Review Evidence

Each asset has three tracked PNGs under `artifacts/asset-review/TC_ENV_DistantSilhouette_{AlienArc,BasaltRidge,SmokePlume}_V1/`: `front.png`, `back_three_quarter.png`, `hero_three_quarter.png`.

**Spot-check performed this session (integrity only):** opened `TC_ENV_DistantSilhouette_BasaltRidge_V1/hero_three_quarter.png` — shows three faceted rock-arch masses of varying height on a neutral ground, clean render, no corruption. Basic sanity confirmed; not a substitute for full diagnosis.

## Visual Diagnosis

**PENDING.** These are background/distant-read silhouettes by design (per the kit name) — the diagnosis should specifically evaluate silhouette clarity at intended viewing distance, not close-up material detail, and should check whether "SmokePlume" reads distinctly from the two rock-formation candidates (mixing a particulate/atmospheric silhouette with hard rock silhouettes in the same kit is worth a specific coherence check).

## Verdict

**PENDING** for all three assets. Already integrated in the interim; formal sign-off is open.
