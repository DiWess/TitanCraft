# Distant Silhouettes Kit V1 — Formal Review (Backfill)

**Requested:** 2026-07-16
**Reviewed:** 2026-07-16, same session, by explicit human authorization to act as Visual Reviewer for this backlog
**Reviewer role:** Claude Code, acting as Visual Reviewer (not self-approving own generated work — these assets predate this session)
**Reworked and re-reviewed:** 2026-07-16, same session, by explicit human authorization to act as Art Director for the rework
**Status:** **COMPLETE — all three PASS after SmokePlume rework**

## Why this exists

All three candidates are live in the production scene (4 instances) with only a narrative log entry (`docs/production/quality-scorecard-log.md:136`) and a mechanical manifest verdict (`..._REVIEW_ARTIFACTS_READY`, which the manifest's own schema only certifies as "exists and imports," not visually approved) — no formal `docs/art/reviews/*.md` verdict exists.

## Assets in this kit

| Asset | Source `.blend` | SHA-256 (source) |
|---|---|---|
| `TC_ENV_DistantSilhouette_AlienArc_V1` | `assets/Source/Blender/Production/DistantSilhouettes_V1/TC_ENV_DistantSilhouette_AlienArc_V1.blend` | `76a95e16e825b7f5fc9e8c42866337d28f88d341378d54c9e0f3e5d6a6859935` |
| `TC_ENV_DistantSilhouette_BasaltRidge_V1` | `.../TC_ENV_DistantSilhouette_BasaltRidge_V1.blend` | `90266876b6babe56cad0092d481c5ed2ffe38ad50d9e8a26fa5130d88562b3c4` |
| `TC_ENV_DistantSilhouette_SmokePlume_V1` | `.../TC_ENV_DistantSilhouette_SmokePlume_V1.blend` | `11d5d99c3f4958d6e1783f3138c95ac6b0a980b5371abc279ce8881e32dc9ffb` (reworked 2026-07-16; original hash was `69ff1143d7a8699d0695e7468d46ae8a200fd540ada75f9f2a7311fbd1eb3d19`) |

Source recipe: `tools/blender/create_distant_silhouettes_kit_v1.py`. License: project-authored. Collision policy: none.

## Integration (already live)

`scenes/Main/Main.tscn`, four `Node3D` instances:
- `DistantRock_4` → BasaltRidge (line 691)
- `DistantRock_5` → AlienArc (line 695)
- `DistantRock_6` → SmokePlume (line 699)
- `DistantRock_7` → BasaltRidge (line 703, repeats #4's mesh)

## Review Evidence

All 9 PNGs opened this session: `front.png`, `back_three_quarter.png`, `hero_three_quarter.png` for each of the three assets.

## Visual Diagnosis

- **`TC_ENV_DistantSilhouette_AlienArc_V1`:** an octagonal ring/arch balanced on two thin tapered legs, muted violet-grey stone material. Distinctive, recognizable silhouette — reads as a natural arch or alien architectural remnant, not a generic rock. Strong background landmark candidate.
- **`TC_ENV_DistantSilhouette_BasaltRidge_V1`:** three faceted rock/column masses of varying height clustered together, mauve-grey stone material. Reads clearly as a rock ridge formation. Clean, coherent silhouette.
- **`TC_ENV_DistantSilhouette_SmokePlume_V1` (original geometry) — did not achieve its own brief.** The original asset was a hard-edged, faceted, angular geometric stack — visually indistinguishable in style, material, and construction technique from `BasaltRidge`. No softness, no billow, no taper suggesting rising particulate matter. Verdict at that point: `NOT_GO`, detailed below under Rework.

## Rework (2026-07-16, Art Director role, by explicit human authorization)

Rewrote `build_smoke_plume()` in `tools/blender/create_distant_silhouettes_kit_v1.py` with a deliberately different construction technique from the rock kit, not just a different arrangement of the same technique:

- **Shape:** six low-subdivision icosphere "puffs" (rounded, not tapering-cone) stacked upward, each successive puff **larger** than the one below and drifting laterally — real rising smoke disperses and widens, the opposite of a rock spire narrowing to a point. Denser/smaller near the ground, larger/fainter high up.
- **Material:** new `pbr_translucent()` helper — `Alpha` blend mode with per-puff alpha values from 0.85 (ground) down to 0.16 (top), pale grey-white (`DISTANT_SMOKE_VAPOR`), replacing the fully-opaque dark-violet rock material entirely. This is a difference in material *kind*, not just color — the rock kit has no transparency anywhere.
- Re-exported, re-validated (`BLENDER_ASSET_VALID`, 0 issues, 6 meshes, 120 triangles), re-rendered all three review angles, and re-opened them.

**Opened-image result:** the reworked asset reads as a genuine billowing vapor column — rounded overlapping forms widening toward the top, visible translucency where puffs overlap (most visible in `back_three_quarter.png`), pale color clearly distinct from `BasaltRidge`'s dark violet-grey. Silhouette and material are now both unmistakably different from the rock formations. Honest limitation: the translucency, while real and visible, is subtle at this close review-render distance under flat-lit conditions — the asset's actual purpose is distant-silhouette background dressing, where shape read matters more than close-up material subtlety, so this is an acceptable tradeoff rather than a residual defect.

## Verdict

- `TC_ENV_DistantSilhouette_AlienArc_V1`: `PASS`
- `TC_ENV_DistantSilhouette_BasaltRidge_V1`: `PASS`
- `TC_ENV_DistantSilhouette_SmokePlume_V1`: `PASS` (reworked) — now genuinely distinguishable in both shape and material technique from the kit's rock formations; closes the `NOT_GO` from the original geometry.

All three integrated and live in the production scene (`DistantRock_4` through `DistantRock_7`); `DistantRock_6` now references the reworked GLB automatically via the same `ExtResource` path (source `.blend`/`.glb` files were updated in place, node references in `Main.tscn` did not need to change).
