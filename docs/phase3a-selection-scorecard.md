# Phase 3A Asset Selection Scorecard

Owner: Codex
Version: 1
Date: 2026-06-30
Review status: READY_FOR_ASSET_SELECTION

## Scoring model

Scores are 0–10. Weighted score uses:

- Visual coherence: 20%.
- Non-cubic geometry: 15%.
- Industrial/salvage suitability: 15%.
- Gameplay readability: 10%.
- Godot integration: 10%.
- Material adaptability: 10%.
- Licensing clarity: 10%.
- Performance suitability: 10%.

Automatic rejection applies when any candidate has licensing clarity below 8/10, visual coherence below 7/10, or non-cubic geometry below 7/10. Unknown license facts are scored conservatively.

## Candidate scores

| Candidate | Category fit | Non-cubic | Visual coherence | Industrial/salvage | Gameplay readability | Godot integration | Material adaptability | Licensing clarity | Performance | Weighted | Gate verdict | Rationale |
|---|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---|---|
| Kenney Nature Kit | Environment | 7 | 6 | 3 | 6 | 8 | 7 | 10 | 9 | 6.70 | REJECT | License and performance are excellent, but visual coherence and volcanic/salvage fit are below target. |
| LMHPOLY Low Poly Rocks Pack | Environment supplement | 8 | 7 | 3 | 7 | 5 | 7 | 5 | 8 | 6.35 | REJECT | Shape variety may help, but license/export clarity is not yet high enough. |
| TANDA Low Poly Rock Asset Pack | Environment supplement | 8 | 6 | 2 | 6 | 6 | 6 | 8 | 8 | 6.10 | REJECT | CC-BY is clear enough with attribution, but coherence and full-environment coverage are insufficient. |
| k0rveen LowPoly Environment Pack | Environment | 6 | 5 | 2 | 5 | 4 | 5 | 3 | 7 | 4.70 | REJECT | Exact license/formats are unknown and art direction appears generic rather than volcanic. |
| Kenney Modular Space Kit | Architecture support | 6 | 6 | 5 | 6 | 9 | 7 | 10 | 9 | 6.85 | REJECT | Legally clean and easy, but already failed as dominant Phase 3A production art. |
| Kenney Space Station Kit | Architecture/props support | 6 | 6 | 5 | 6 | 8 | 7 | 10 | 9 | 6.75 | REJECT | Useful fallback, but rectangular/clean risk keeps it below visual-coherence gate. |
| Synty POLYGON Sci-Fi Worlds | Architecture/props | 8 | 8 | 8 | 8 | 5 | 8 | 7 | 8 | 7.65 | REJECT_PENDING_LICENSE | Visual fit looks strong, but licensing/export clarity must reach 8/10 before selection. |
| Synty POLYGON Sci-Fi City | Architecture/props | 8 | 7 | 7 | 7 | 5 | 7 | 7 | 8 | 7.10 | REJECT_PENDING_LICENSE | Potentially useful but city/neon tone and license/export clarity need review. |
| Synty POLYGON Sci-Fi Space | Wreckage/ship | 8 | 8 | 8 | 8 | 5 | 8 | 7 | 8 | 7.65 | REJECT_PENDING_LICENSE | Best researched wreckage/ship-family candidate, blocked until license/export/content are verified. |
| Kenney Space Kit | Wreckage/free ship parts | 6 | 6 | 5 | 6 | 8 | 7 | 10 | 9 | 6.75 | REJECT | Legally clear but not enough damaged asymmetric wreckage for Phase 3A by itself. |
| CGTrader Modular Sci-Fi Station candidate | Wreckage/interior | 7 | 6 | 7 | 6 | 4 | 7 | 3 | 6 | 5.85 | REJECT | Exact listing facts, license and price were not verified. |
| KayKit Space Base Bits | Props/architecture | 7 | 7 | 6 | 7 | 4 | 7 | 3 | 7 | 6.05 | REJECT | Needs exact page/license/formats before it can be trusted. |
| Quaternius sci-fi/robot candidates | Enemy/arm | 7 | 6 | 5 | 6 | 6 | 6 | 3 | 8 | 5.85 | REJECT | User explicitly requires exact asset/source/license verification before use. |
| CGTrader alien/robot candidate | Enemy | 8 | 5 | 5 | 6 | 4 | 6 | 3 | 5 | 5.25 | REJECT | Too fragmented and legally uncertain without exact model-level review. |
| Current Galaxabrain placeholder | Enemy baseline | 4 | 5 | 4 | 6 | 10 | 8 | 10 | 10 | 6.05 | REJECT | Gameplay safe but visually below target and remains a placeholder. |
| Kenney Space Kit character/weapon pieces | Mechanical arm placeholder | 5 | 5 | 4 | 5 | 8 | 7 | 10 | 9 | 6.05 | REJECT | Legally clean placeholder source, but likely too simple for first-person mechanical arm. |
| Synty character/mech components | Enemy/arm | 8 | 8 | 8 | 8 | 5 | 8 | 7 | 8 | 7.65 | REJECT_PENDING_LICENSE | Coherent-family candidate if contents contain suitable parts; license/export/content verification needed. |

## Interpretation

No candidate is approved for import yet. The strongest visual direction is Synty-led, but every Synty entry is `REJECT_PENDING_LICENSE` until a human confirms acquisition path, exact pack contents, source-file access, license terms, and export workflow. Free-only candidates are useful for prototypes and background filler but are unlikely to reach the Phase 3A visual bar after the measured Kenney recovery failure.

## Recommended selection path

1. Human chooses one of the three budget options in `docs/phase3a-asset-shortlist.md`.
2. Before import, verify exact license, file formats, source-file access outside Unity-only prefabs, and modification rights for each chosen pack.
3. Import only the chosen packs and update `THIRD_PARTY_ASSETS.md` with exact source, license, and local files.
4. Re-enter Phase 3A only with the mandatory screenshot review loop.
