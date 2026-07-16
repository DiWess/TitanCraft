# TC_HullRibOccluder_V1 — Formal Review (Backfill)

**Requested:** 2026-07-16
**Reviewed:** 2026-07-16, same session, by explicit human authorization to act first as Visual Artifact Factory (rendering the missing evidence) and then as Visual Reviewer
**Reviewer role:** Claude Code
**Status:** **COMPLETE — qualified PASS**

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

## Review Evidence — gap closed this session

The original `create_hull_rib_occluder_v1.py` never had a render step (unlike every other Blender Asset Forge kit) — it only wrote mesh-stat/hash text files. Added `tools/blender/render_hull_rib_occluder_v1_reviews.py`, which imports the already-committed OBJ **read-only** (the committed mesh geometry was not touched or regenerated) and renders the standard 3-view set. First attempt used flat/legacy material properties and a bare sun lamp, which produced a harsh black sky and hard-to-read result — not a mesh defect, a lighting/material setup bug in the new script. Fixed by matching `render_mvp_asset_review.py`'s established world-background + 3-point-area-light + Principled-BSDF pattern; re-rendered cleanly. Now three tracked PNGs exist: `front.png`, `back_three_quarter.png`, `hero_three_quarter.png` under `artifacts/asset-review/TC_HullRibOccluder_V1/`.

**Important caveat on material:** the render script assigns a placeholder neutral grey-steel material for visualization, since the raw OBJ carries no material data. The actual production instances in `Main.tscn` apply real `material_override` resources — `TC_HullRibOccluder_Foreground` uses `65_tc_steel_mat`, `TC_HullRibOccluder_Buried` uses `66_tc_graphite_mat` (both presumably shared, already-used materials from elsewhere in the scene). **This review's material judgment does not apply** — only the shape/silhouette below is representative of what ships. No scale reference is present in the renders either, consistent with every other kit in this backfill batch.

## Visual Diagnosis

The mesh is a sparse, low-poly, open arch/skeletal structure — a bent spine with thin blade-like cross braces underneath and jagged, torn-looking terminal points, matching its own `review_metadata.md` description ("bent spacecraft rib... torn feet and asymmetric cross braces"). As a concept it's reasonable for a structural hull rib fragment: skeletal and open is arguably more correct for an exposed "rib" than a solid mass would be.

The honest concern: it's **very thin and sparse** compared to sibling wreckage assets (`TC_HeavyCrashHull_V1`, `TC_ENV_CrashDebris_A_V1`), both of which read as substantial mass from any angle. This one, especially in the `front.png` view, is close to a wireframe silhouette — at gameplay viewing distance it risks reading as abstract linework rather than "wreckage" unless the two scene instances' scale/placement (see transforms in Main.tscn: one scaled ~0.78-1.35× at the CrashWreck cluster) and their real production materials (steel/graphite, not this review's placeholder grey) carry more visual weight than the bare mesh does in isolation. That can't be confirmed without an opened in-scene screenshot, which doesn't exist for this specific pair of instances.

## Verdict

`PASS`, qualified. The shape satisfies its own brief concept (torn, asymmetric structural rib) and contains no rejection-pattern violations (not toy-like, not photoreal, not a route slab). Two real limitations keep this from being an unqualified PASS: (1) the material shown is a review placeholder, not the actual `TC_Steel_Mat`/`TC_Graphite_Mat` production materials, and (2) the mesh's sparseness raises a genuine legibility question at gameplay scale that only an in-scene screenshot could resolve. Recommend a follow-up production-scene capture of the `CrashWreck` cluster before treating this as fully closed — cheaper than redoing this review, since the shape and provenance work here doesn't need to be repeated.
