# ADR: Quality Benchmark v3 — Re-anchor Axis 9 for a Single-Scene Experience

**Date:** 2026-09-25
**Status:** APPROVED (product-owner decision D6, `docs/production/top-tier-single-scene-plan.md`;
approval recorded in README §6, amendment of 2026-09-25)
**Owner:** Producer
**Supersedes:** axis 9's peer anchors and measurement in `quality_benchmark_v1.md`. Every other v1
rule and axis, and the v2 delegation ADR, remain in force.

## Context

`quality_benchmark_v1.md` anchors axis 9, "Content volume / replayability", to Valheim,
Subnautica and Grounded — games judged by breadth: many biomes, many enemies, dozens of hours.
The post-MVP direction set on 2026-09-25 is **one scene, one player, one experience**. Breadth is
ruled out on purpose, so those anchors could never be met, and every scorecard since 2026-07-06
has recorded axis 9 at 2.0 as "structurally capped" — a score that measured scope policy, not
quality.

A single scene can still be thin or dense. The measured problem is density: on 2026-09-24 an
optimal walked route finished the whole MVP in about 50 seconds against a README §5 session
target of 10–30 minutes.

## Decision

Axis 9 is renamed **"Content density / replayability"** and re-anchored to top single-level and
single-space work:

| Anchor | What it demonstrates |
|---|---|
| Titanfall 2 — *Effect and Cause* | One level built entirely around one mechanic, no minute wasted |
| Portal (2007) | One space, one tool, a complete arc |
| Half-Life 2 — a single chapter | Authored pacing and set pieces inside one area |
| What Remains of Edith Finch | A single place whose every room carries meaning |

Target: **9.0 / 10** (unchanged). Axis 9 is scored from these measures, each traced to evidence:

1. **Optimal completion time** — game time for the walked playthrough harness
   (`tools/visual_review/playthrough_journey.gd`) on its optimal route.
2. **Human session length** — median from dated human playtest notes (README §5: 10–30 minutes).
3. **Authored encounters** — distinct designed fights or set pieces.
4. **Optional discoveries** — content off the critical path, each reachable by the harness.
5. **Replay reasons** — a stated reason to play again (routes, mastery, discoveries missed).

Calibration points, so scores are not agents agreeing with themselves:

| Score | Density |
|---:|---|
| 2 | Under 2 minutes optimal; one encounter; nothing optional (the MVP on 2026-09-24) |
| 5 | About 8 minutes optimal; three encounters; a handful of discoveries |
| 7 | About 15 minutes optimal; five encounters with a climax; discoveries reward exploration |
| 9 | 20–30 minute human sessions; every minute authored; players describe a reason to replay |

## Consequences

- Axis 9 stops being a permanent cap and becomes the plan's headline measure.
- The current score stays **2.0** until evidence under these measures moves it; re-anchoring
  changes what is measured, not the result.
- Composite scores before and after this ADR are not directly comparable on axis 9; the
  scorecard log notes the change at its first use.

## Verdict

`PASS` — governance change recorded with its measures and calibration in the same change set.
