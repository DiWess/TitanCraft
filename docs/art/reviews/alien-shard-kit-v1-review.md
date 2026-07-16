# Alien Shard Kit V1 — Formal Review (Backfill)

**Requested:** 2026-07-16
**Reviewed:** 2026-07-16, same session, by explicit human authorization to act as Visual Reviewer for this backlog
**Reviewer role:** Claude Code, acting as Visual Reviewer (not self-approving own generated work — these assets predate this session)
**Status:** **COMPLETE**

## Why this exists

All three Alien Shard candidates are already live in the production scene (5 instances total) but only have a narrative log entry (`docs/production/quality-scorecard-log.md:252`), not a formal `docs/art/reviews/*.md` verdict.

## Assets in this kit

| Asset | Source `.blend` | SHA-256 (source) | GLB export |
|---|---|---|---|
| `TC_ENV_AlienShard_Small_V1` | `assets/Source/Blender/Production/AlienShard_V1/TC_ENV_AlienShard_Small_V1.blend` | `ce1ff345abc704bde51fa2af55d3fec9397521f98d0be800aa733f0663e83064` | `assets/Production/Generated/AlienShard_V1/TC_ENV_AlienShard_Small_V1.glb` |
| `TC_ENV_AlienShard_Medium_V1` | `.../TC_ENV_AlienShard_Medium_V1.blend` | `077cd1beb6083e0b6b2972f6ae0d351a1a62838d8f98e1d17c455133d9c07e50` | `.../TC_ENV_AlienShard_Medium_V1.glb` |
| `TC_ENV_AlienShard_EmbeddedCluster_V1` | `.../TC_ENV_AlienShard_EmbeddedCluster_V1.blend` | `b1e46e18e6ceee8af748ea415e78caa18d0326fc688900904d658d38b848e6db` | `.../TC_ENV_AlienShard_EmbeddedCluster_V1.glb` |

License: project-authored, all three. Collision policy: none (visual-only).

## Integration (already live)

`scenes/Main/Main.tscn`, five `Node3D` instances:
- `AlienCrystal_1` → Medium (line 825)
- `AlienCrystal_2` → Small (line 829)
- `AlienCrystal_3` → EmbeddedCluster (line 833)
- `AlienCrystal_4` → Small (line 837)
- `AlienCrystal_5` → EmbeddedCluster (line 841)

## Review Evidence

All 9 PNGs opened this session: `front.png`, `back_three_quarter.png`, `hero_three_quarter.png` for each of the three assets, under `artifacts/asset-review/TC_ENV_AlienShard_{Small,Medium,EmbeddedCluster}_V1/`.

## Visual Diagnosis

- **Focal point / silhouette:** Small and Medium are both single tall faceted spires with a notched "V" apex — clean, unambiguous crystal-shard silhouette, no toy-like proportions, no photorealism. EmbeddedCluster breaks from the single-spire language into a three-peaked cluster of varying heights, giving genuine compositional variety within the kit rather than three near-duplicates.
- **Material coherence:** all three share one flat lavender-violet material with a single darker facet per view — consistent, matches the "AlienVioletEmissive" palette referenced in the authoring commit, and reads distinctly "alien" against the beige/steel human-tech palette used elsewhere (Workbench, Base Camp Dressing). No rejection-pattern violations found (not photoreal, not a route slab, not a toy hull).
- **Small vs. Medium differentiation:** in isolated framing these two read as very similar — same silhouette family, same single-spire composition, similar apparent proportions. The manifest names them differently by intended size, but nothing in these auto-framed, scale-less renders confirms that difference is visually meaningful once placed. This matters because `Main.tscn` reuses Small at two positions (`AlienCrystal_2`, `AlienCrystal_4`) and EmbeddedCluster at two positions (`AlienCrystal_3`, `AlienCrystal_5`) — checked the scene transforms directly: per-instance non-uniform scale multipliers (0.7×, 0.8×, 1.0×, 1.2×, 1.5×) are applied on top of the base meshes, so the artist did vary apparent size per placement. That confirms intentional scale variation exists, but does not by itself confirm the *in-scene* composition avoids feeling repetitive — that requires a scene-level screenshot, not these isolated asset renders, and none was captured for this review.
- **Scale reference:** none of the 9 renders include a scale-reference object (unlike the Base Camp Dressing kit's 8-angle set, which does). Per this project's own precedent (`heavy-crash-hull-v1-standalone-review.md` treats a visible scale post as required evidence), absolute real-world scale cannot be independently verified from these images alone. The per-instance transform multipliers above are the closest available substitute, and they at least confirm relative variation was authored on purpose.

## Verdict

`PASS` on silhouette, material coherence, and scope (no rejection-pattern violations, consistent with the alien-material language). **Scale is explicitly unverified**, not assumed good — that would require either a scale-reference re-render or an opened in-scene screenshot showing the 5 placements together, neither of which exists yet. Recommend a follow-up scene-composition screenshot before treating the 5-instance placement as fully closed, but that is a smaller, cheaper gap than the one this review closes.
