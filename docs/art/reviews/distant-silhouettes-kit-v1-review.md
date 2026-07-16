# Distant Silhouettes Kit V1 — Formal Review (Backfill)

**Requested:** 2026-07-16
**Reviewed:** 2026-07-16, same session, by explicit human authorization to act as Visual Reviewer for this backlog
**Reviewer role:** Claude Code, acting as Visual Reviewer (not self-approving own generated work — these assets predate this session)
**Status:** **COMPLETE — mixed verdict, one asset needs rework**

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

All 9 PNGs opened this session: `front.png`, `back_three_quarter.png`, `hero_three_quarter.png` for each of the three assets.

## Visual Diagnosis

- **`TC_ENV_DistantSilhouette_AlienArc_V1`:** an octagonal ring/arch balanced on two thin tapered legs, muted violet-grey stone material. Distinctive, recognizable silhouette — reads as a natural arch or alien architectural remnant, not a generic rock. Strong background landmark candidate.
- **`TC_ENV_DistantSilhouette_BasaltRidge_V1`:** three faceted rock/column masses of varying height clustered together, mauve-grey stone material. Reads clearly as a rock ridge formation. Clean, coherent silhouette.
- **`TC_ENV_DistantSilhouette_SmokePlume_V1` — does not achieve its own brief.** This is the finding the "Why this exists" section above flagged as worth checking, and it's real: the asset is a hard-edged, faceted, angular geometric stack — visually indistinguishable in style, material, and construction technique from `BasaltRidge`. There is no softness, no billow, no taper suggesting rising particulate matter, nothing that reads as "smoke" rather than "rock totem." A silhouette named "SmokePlume" should be immediately distinguishable from the kit's own rock formations at distant-read scale; instead it reinforces the same hard-surface rock language, just stacked vertically. This is a genuine authoring gap, not a nitpick — the kit's own coherence check (mixing atmospheric and hard-surface silhouettes) fails for this specific asset because it was never actually built as atmospheric.

## Verdict

- `TC_ENV_DistantSilhouette_AlienArc_V1`: `PASS`
- `TC_ENV_DistantSilhouette_BasaltRidge_V1`: `PASS`
- `TC_ENV_DistantSilhouette_SmokePlume_V1`: `NOT_GO` — needs rework to actually read as smoke/atmospheric (softer silhouette, tapering/billowing form, and ideally a translucent or particulate-suggesting material rather than the kit's opaque rock material) before it can be considered visually approved under its current name and concept. Recommend either reworking the geometry or renaming/repurposing it as a fourth rock-formation variant if an atmospheric asset isn't a priority — shipping it as-is under the "SmokePlume" name misrepresents what it actually is.

All three remain integrated in the production scene in the interim (`DistantRock_4` through `DistantRock_7`); this verdict does not require pulling `SmokePlume` from the scene immediately, since at true distant-silhouette viewing range in actual gameplay the failure may read as less severe than in this isolated close-up review render — but it should not be treated as visually closed.
