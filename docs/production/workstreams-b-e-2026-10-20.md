# Workstreams B–E and the 1.0.0 release candidate — 2026-10-20

**Plan:** `docs/production/top-tier-single-scene-plan.md`
**Scope check:** README §6 amendment of 2026-09-25 (D1–D7). Nothing from the forbidden list was
added: one map, one player, no multiplayer, no cloud or telemetry. The Mk II module pulls
*objects and drones to the player*, never the player to a point, so it is not a grappling hook
(D5). Galaxabrain variants: Scout, Swarm Drone, Warden — three, the D3 cap. At most three
enemies are active at once in every encounter, including the holdout (README §28).

## What was built

### B — the arm becomes the game
- **Mk I strike** (existing): tap, 25 damage.
- **Mk II magnet clamp**: right click / F pulls pullable wreckage and drones (range 16 m,
  0.9 s cooldown, 7° aim assist). Opens the Net-makers' Yard by dragging the wreck barricade
  clear; a pulled drone is stunned for 2.8 s.
- **Mk III breaker coil**: hold ≥ 0.75 s, release for a 70-damage charged strike (movement
  ×0.6 while charging). Breaks the cracked gate into the tower court and the Warden's guard.
- Each module is crafted at the one workbench from what the previous act gives up
  (`data/Recipes/mechanical_arm_mk{1,2,3}.json`); the workbench only offers the craft the
  current mission step asks for.
- The quarter gains two gated courts (yard and tower court) with invisible boundary walls;
  the level generator now emits them, and the integration suite flood-fills physics to prove
  each gate seals its court until opened (`TestGatesSealTheirCourts`).

### C — encounters and the climax
- **Scout** (existing) at the plaza.
- **Drone nest** in the yard: three swarm drones (telegraph 0.7 s, dive 1.3 s); the clamp
  pulls them out of the air.
- **Warden** in the tower court: guard deflects anything but a charged strike; a charged
  strike staggers it for 3.6 s; ground slam (0.95 s wind-up, 3.3 m reach).
- **Beacon holdout**: installing the power core starts a 75 s charge that only runs while the
  player holds the plaza (13 m). Waves at 0 / 14 / 28 / 44 / 58 s, never more than three
  active. The beacon's field repairs 1.5 HP/s while held. When the charge completes the beam
  burns out whatever is still fighting.
- Mission chain extended to 14 steps; save format v2 with a v1 migration; per-pickup and
  per-note persistence; checkpoints at the save point, every craft and the power core.

### D — the place tells its story
- Six optional story notes (notice board, slate, toy boat, flight recorder, stripped machine,
  plaque); the run summary on the victory screen counts them.
- The harbour gets a sea (shader plane, terrain falls away east of x 36.5).
- Materials move from flat colour to runtime triplanar noise albedo + normal per material
  family; paved courts; route-wear decals.

### E — feel and finish (the parts agents can supply)
- Settings: mouse sensitivity, invert Y, FOV, master volume, fullscreen, VSync, reduced camera
  shake, key rebinding; persisted to `user://settings.cfg`.
- 18 new project-authored audio cues (arm, drones, Warden, barricade, gate, holdout, discovery).
- Release packaging: icon, version 1.0.0, recipe data packed into the export (an exported build
  previously could not find its recipes), a `--titancraft-selftest` flag an exported build runs
  on itself, and `.github/workflows/release.yml` (tag `v*` → build, tests, export, self-test,
  zip, SHA-256, **draft** GitHub Release).

## Evidence

| Check | Result |
|---|---|
| Unit tests | 151 / 151 |
| Integration suite | `TITANCRAFT_INTEGRATION_TESTS_PASS`; full playthrough milestones 1–19; HUD budget held at every step |
| Exported Linux build, run away from the repo | `TITANCRAFT_SELFTEST_PASS` (recipes load from the pck) |
| Windows export | produced; mk1–mk3 recipe JSON packed |
| Audio sources | 45 files, 45 audible (`tools/test_audio_sources.py`) |
| Walked playthrough (real input, bot) | see below |

### Walked playthrough

`tools/visual_review/playthrough_journey.gd`, real input through the whole gated route, after
the holdout rebalance (commit `a5e0813`):

| Measure | Result |
|---|---|
| Result | `victory` (victory screen reached) |
| Legs walked | 45 / 45, 0 stuck events, every interaction on attempt 1 |
| Game time, start to victory | 201 s (bot-optimal; no exploration, no notes read) |
| Scout | won in 3.3 s, 9 swings, health 100 → 60 |
| Drone nest | won in 33.3 s, 61 pulls, 21 swings, health 100 → 84 |
| Warden | won in 13.5 s, 3 charged strikes, 13 swings, health 100 → 56 |
| Beacon holdout | won in 79.2 s from full health |
| Damage events | 15; no defeat |
| Render proxy | software renderer under xvfb, mean 212 ms/frame — not a hardware claim |

The previous run (before the rebalance) died in the holdout at 32.5 s. The rebalance made the
Scouts that answer the beacon lighter (75 HP, 7 damage, 1.3 s cooldown), holdout drones hit
for 6, and the beacon's field repairs 1.5 HP/s while the plaza is held.

The `Weapon_Deflect` cue never sounded in this run: the bot only swings at the Warden while it
is staggered, which is the optimal play. Deflection itself is covered by the unit tests in
`tests/Unit/GalaxabrainVariantTests.cs`.

## What this does not establish

- **Plan exits on length are not met.** Workstream B asks for bot-optimal completion of at least
  8 minutes and C for at least 15. The walked bot finishes in 201 s (about 3.4 minutes). A
  human's first run will be longer than the bot's, but that has not been measured.
- **No human playtest notes** #1–#3 exist. Nothing here claims feel; axes 2 and 3 stay where
  they were scored.
- **No rendered performance on real Windows hardware.** README §28's 60 fps target is
  unmeasured.
- **Not code-signed**; SmartScreen will warn. **No controller support.**
- Pre-existing, unrelated: exported builds print "disconnect a nonexistent connection" engine
  errors on exit (identical on `main`).

## Verdict

`HUMAN_BLOCKED` for the plan's B–E exits (human playtests, real-hardware performance). The
release candidate itself is repository-complete: `PASS` on every automated gate above,
pending the product owner's go to publish the draft release.
