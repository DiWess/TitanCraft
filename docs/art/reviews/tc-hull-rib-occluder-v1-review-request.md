# TC_HullRibOccluder_V1 — Formal Review Request (Backfill)

**Requested:** 2026-07-16
**Requested by:** Claude Code (Code Reviewer & Architecture Validator) — filed per Producer-approved cleanup action, not a completed review
**Status:** **PENDING** — no visual diagnosis or verdict has been performed. This document scaffolds the required review; a human or the Visual Reviewer agent must complete the Visual Diagnosis and Verdict sections from opened images before this can close.

## Why this exists

`TC_HullRibOccluder_V1` is already live in the production scene (two instances, see Integration below) but never went through a formal `docs/art/reviews/*.md` gate — the only prior record is an untracked local file (`artifacts/asset-review/TC_HullRibOccluder_V1/review_metadata.md`) plus a `context_log.md` narrative entry from 2026-07-07. Unlike `TC_TerrainDioramaKit_V1` and `TC_HeavyCrashHull_V1`, which both have full `docs/art/reviews/*.md` verdicts, this asset skipped the process it should have followed.

## Provenance

- Source recipe (tracked): `tools/blender/create_hull_rib_occluder_v1.py` — project-authored, no third-party assets.
- Production asset (tracked, committed text): `assets/Production/Custom/StageA/TC_HullRibOccluder_V1.obj`
  SHA-256 `1729c0c9eaa6d700e2f1eda7be5a223e7253c978bd8cd7c08c80f58abfa5bd4d`
- Binary policy: `text_obj_committed_no_blend_or_glb_binary` (per `review_metadata.md`) — this asset was authored and shipped as a committed text OBJ, not the `.blend`/GLB pipeline used elsewhere.
- License: project-authored.

## Integration (already live — this is a backfill, not a pre-integration gate)

`scenes/Main/Main.tscn`:
- `StageAVisualRoot/CrashWreck/TC_HullRibOccluder_Foreground` (line 1109)
- `StageAVisualRoot/CrashWreck/TC_HullRibOccluder_Buried` (line 1115)

Collision policy: none (visual-only dressing), consistent with the other Stage A wreckage props.

## Review Evidence — GAP

**No PNG evidence exists for this asset.** `artifacts/asset-review/TC_HullRibOccluder_V1/` contains only `mesh_stats_report.md`, `review_metadata.md`, and `sha256sums.txt` — no rendered images. This is the largest evidence gap of the six items in this backfill pass: even the mechanical mesh-stat/hash step that other candidates have is missing the PNG-render step entirely.

**Required before this can move to Visual Diagnosis:** run the Blender Asset Forge render step (`render_asset_review.py` or equivalent) against `create_hull_rib_occluder_v1.py`'s output to produce neutral + material passes with a scale reference, per the standard established in `docs/art/reviews/heavy-crash-hull-v1-standalone-review.md`.

## Visual Diagnosis

**PENDING** — cannot be completed until PNG evidence above exists. Do not fill this section from the mesh-stats report or from memory of the asset; it must come from opened images.

## Verdict

**PENDING** — no verdict can be issued without the Visual Diagnosis step. This asset remains integrated in the production scene in the interim (removing already-shipped dressing is out of scope for this backfill request), but its formal sign-off is open.
