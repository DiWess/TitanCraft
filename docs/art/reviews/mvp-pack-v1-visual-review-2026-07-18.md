# MVP Asset Pack V1 — Stage B Visual Reviewer Verdict

**Date:** 2026-07-18
**Reviewer role:** Visual Reviewer (independent review; routed packet primary agent Producer,
secondary qa_lead / technical_director)
**Brief:** `docs/art/briefs/mvp-asset-pack-v1.md`
**Authority:** `studio/agents/visual_reviewer.md`, MEM-VISFAIL-001/002/003,
`docs/art/titancraft-visual-identity.md` (automatic rejection patterns)
**Quality benchmark axis:** #6 Visual art & presentation per
`studio/decisions/quality_benchmark_v1.md`. This review closes the Stage B standalone-review
gap for the MVP pack; it does not claim the axis target is met.

> Unlike the 2026-07-06 documents marked **NOT REAL — SIMULATION EXERCISE**
> (`VISUAL_REVIEWER_VERDICT_V1.md` and its cluster), every asset, file, hash, and PNG cited
> here exists in the working tree and was actually opened during this review.

## Provenance

- Source recipe (tracked): `tools/blender/create_mvp_asset_pack_v1.py` — project-authored,
  no third-party assets, no external licences.
- GLB exports: `assets/Production/Generated/MVP_Pack_V1/` — 13 files, all listed in
  `assets/Production/Generated/asset_manifest.json` with SHA-256 hashes.
- Hash verification (this review, 2026-07-18): all 29 manifest entries exist on disk and all
  29 `production_sha256` values match recomputed SHA-256 — 0 mismatches, 0 missing.
- Known provenance finding: tracked `.blend` source hashes differ from manifest
  `source_sha256` values because Blender file serialization embeds timestamps and is not
  byte-deterministic across regeneration. The GLB export hashes — which are what production
  consumes — match exactly. Recorded as a MINOR manifest-policy finding; recommend the
  manifest treat `.blend` hashes as informational, or hash a canonicalized export instead.

## Review evidence (opened PNGs)

Review bundles under `artifacts/asset-review/<AssetName>/`, three Cycles-rendered views per
asset (`hero_three_quarter.png`, `front.png`, `back_three_quarter.png`). All hero views for
the 13 pack assets were opened; `front.png` was additionally opened for the three borderline
candidates (CrashDebris_A, MechanicalArmUnbuilt, Biomass).

Scale evidence: the original bundles contain no in-render scale reference, which the
MEM-VISFAIL series treats as non-reviewable on its own. This review compensates two ways:

1. **Numeric bounds audit** — GLB accessor min/max parsed per asset and compared to the
   brief size table (see the Technical Director audit,
   `docs/production/mvp-pack-v1-technical-audit-2026-07-18.md`).
2. **Regenerated `scale_reference.png` view** — `tools/blender/render_mvp_asset_review.py`
   now renders a fourth view per asset with a render-only 1.8 m orange post (never part of
   asset data), matching the `TC_HeavyCrashHull_V1` review precedent.

## Per-candidate diagnosis

Scored against: focal point, silhouette (neutral-read), scale, material coherence,
objective/gameplay readability, and the nine automatic rejection patterns.

### TC_PROP_Workbench_V1 — PASS
Long low bench, articulated robot arm, tilted holo panel, `C7` stencil, orange drawer
accents. Silhouette reads as an authored industrial station, not stacked primitives. Palette
correct (beige hull / dark metal / orange accents, one small red standby LED). Note: the holo
panel renders amber-yellow rather than orange; within palette tolerance.

### TC_PROP_Beacon_Dormant_V1 — PASS
Closed four-petal obelisk on octagonal base; petals read clearly against dark inner gaps;
red standby LED and orange stencil present. Strong landmark silhouette at 2.18 m.

### TC_PROP_Beacon_Active_V1 — PASS
Opened petals with purple emissive core crystal and petal seam lines. Glow is confined to the
core and active energy lines — the allowed emissive pattern; it does not overwhelm the
functional orange/red signals on the base. Clear state-change read against the dormant
variant, which is exactly the beacon's gameplay job.

### TC_PICKUP_Metal_V1 — PASS
Pile of flat angular hull shards, beige/dark-metal mix, single orange paint mark. Reads as
salvage debris; low flat silhouette is appropriate for a ground pickup.

### TC_PICKUP_Biomass_V1 — PASS (with corrective note)
Faceted organic cluster with spikes; silhouette clearly organic against the pack's
rectilinear props; no rejection pattern triggered. **Corrective note (MINOR):** the brief
specifies "dark-red … faint inner glow"; the render reads bright crimson with no visible
inner glow. Recommend a V2 albedo darkening + weak emission pass; non-blocking for a
placeholder-tolerant MVP because pickup readability is preserved (arguably improved).

### TC_PICKUP_Electronics_V1 — PASS
Stacked dark modules, orange lids, cyan indicator LED, antenna. Reads as technology at a
glance; cyan is a small indicator, not glow spam.

### TC_PICKUP_Component_V1 — PASS
Purple emissive crystal cluster on dark rock shard. Distinct from all three resources;
correct "mission-critical alien" coding per the visual identity (violet accents on dark
alien base).

### TC_CHAR_GalaxabrainScout_V1 — PASS
Spindly raised-knee quadruped biomech; purple core and head glow limited to weak-point
areas. Matches the identity spec (asymmetrical biomechanical form, sharp twisted silhouette,
dark base, restrained violet). Height 1.69 m matches brief; leg-splay footprint 1.65 × 1.82 m
is under the brief's 2.4 × 2.4 m approximation — noted, non-blocking (see scale audit).

### TC_CHAR_GalaxabrainScout_Disabled_V1 — PASS
Same body language collapsed: legs splayed, body dropped to 0.80 m, emissives dimmed.
Reads immediately as the defeated state of the same creature.

### TC_PLAYER_MechanicalArm_V1 — PASS
Segmented tapering forearm with powered fist, purple energy seams, orange accent. Glow
confined to seams. First-person plausible proportions (0.31 × 0.35 × 1.04 m).

### TC_PLAYER_MechanicalArmUnbuilt_V1 — PASS (with note)
Ordered parts tray: octagonal segments and fist block on a rimmed tray with orange edge
strip. Deliberate kit composition — not the "random cubes" rejection pattern. Note: weakest
standalone silhouette of the set; relies on workbench context to read as "arm parts", which
is where the MVP places it.

### TC_PROP_SavePoint_V1 — PASS
Hex pillar with cyan emissive ring, strip, and top emitter. Cyan is function-coded (save
interaction) and spatially confined; silhouette reads at 1.58 m as an interaction pillar.

### TC_ENV_CrashDebris_A_V1 — PASS (with note)
Bent off-white hull plate with scorched dark corner, orange stencil, exposed ribs. Front
view confirms real edge thickness on the plate — not the paper-thin-panel rejection
pattern. Notes: the two exposed ribs read thin/wiry from some angles; measured height
2.54 m exceeds the brief's ~1.9 m target (see scale audit) — acceptable for a navigation
landmark, recorded for the integration pass.

## Rejection-pattern sweep

All nine automatic rejection patterns checked per asset on opened images: no cartoon/toy
proportions, no photoreal conflict, no voxel language, no glossy showroom plastic, no
material-dependent silhouettes, no random-primitive assemblies, no paper-thin wreckage, no
excessive cyan/violet glow (all emissives are core/seam/state-confined), and this document
itself supplies the PNG-review evidence the ninth pattern requires.

## Verdict

`PASS` — all 13 MVP Asset Pack V1 candidates are approved as **standalone production asset
candidates**, with three MINOR corrective notes (Biomass albedo/glow V2, ArmUnbuilt context
dependence, CrashDebris rib thinness/height) and one MINOR manifest-policy finding
(non-deterministic `.blend` hashes).

This verdict does not integrate any asset into production scenes, does not claim Stage C or
release readiness, and does not claim quality-benchmark axis targets. Production scene
placement remains a separately gated Stage C task requiring its own before/after captures
and sign-off.
