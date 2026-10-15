# Workstream A — Ending, HUD and Readability Pass (2026-09-27)

**Owners:** UX Designer (HUD, ending), Art Director (grade, route reading); Visual Reviewer
verdict below.
**Plan items:** `docs/production/top-tier-single-scene-plan.md`, workstream A — the in-world
ending shot, a smaller HUD panel, lime-render clipping, eye-level route readability.
**Findings addressed:** `docs/production/playtests/2026-09-24-journey.md`, aesthetic diagnosis
("the ending fails outright", "lime render clips", "route readability — carried by the HUD").

Binary evidence: the PNGs named here are regenerated under the gitignored
`artifacts/visual-review/` by the commands in each section; this document is the tracked record.

## 1. The ending — `d7b2ae7`

Victory used to cut from a button press to small grey text on near-black. Now:

- **In-world shot.** `src/UI/VictoryEndingShot.cs` cuts to a fixed camera on the lit beacon
  against the harbour wall and sea, hides the HUD for a letterbox and a caption, freezes the
  player and pushes the camera 2 m in over the hold, under the mission-complete swell. The
  framing was chosen from eight rendered candidates (`artifacts/visual-review/ending/`,
  candidate D2); the rejected ones were inside a building, behind a palm, or lost the sea.
- **Hold** 3 s → 5 s, to fit the shot.
- **Victory screen over the last frame.** It is a separate scene, so `EndingSnapshot` carries
  the ending's last frame across; the screen shows it under a lighter backdrop and consumes it.
  The first version put the menu over the beacon spire and was illegible; it moved to the lower
  third with outlined text.

Opened: `artifacts/visual-review/playthrough/16_beacon_climax.png` (ending shot) and
`17_99_end_screen.png` (victory screen over it).

Tests: the victory-transition test requires the ending camera current with the beacon in frame,
the HUD hidden, the overlay shown and the player disabled; `TestVictoryScreenShowsTheEndingFrame`
covers the hand-off and a plain visit with no stale frame.

## 2. A smaller HUD panel

| | Before | After |
|---|---|---|
| Panel at start (1280×720) | 488 × 178 px | **400 × 150 px** (−31 % area) |
| Late game (component recovered) | 517 × 178 px — the arm line did not wrap and widened it | 400 × 176 px |
| Lines | health, objective, resources, 2–3 line controls reference, arm progress | health, objective, resources, arm progress |

- The permanent controls line left the panel. The onboarding prompt already teaches controls one
  step at a time; the full reference is now in the pause menu (`Controls`), where it had no
  entry at all before.
- Resources no longer show "Galaxabrain Component: Missing" for the first half of the run; the
  component appears once recovered.
- Arm progress shortened ("Mk I: Metal 4/10 · Biomass 1/3 · Electronics 0/2") so it fits one line.

Tests: `TestPauseMenuListsControls` (every control the old HUD line listed is in the pause menu;
the panel has no controls line) and a 400 × 160 budget in `TestHudPromptsDoNotOverlapThePanel`.

## 3. Lime render no longer clips

The grade used ACES with `tonemap_white = 1.0`, which maps every value above 1.0 to pure white,
so sunlit lime walls lost their plaster detail. Five grades were rendered from the district's
eight eye-level views and measured (pixels with all channels ≥ 250; mean luma):

| White / exposure | Clipped | Mean luma |
|---|---:|---:|
| 1.0 / 1.22 (before) | 0.99 % | 98.0 |
| 3.0 / 1.35 | 0.23 % | 96.6 |
| **4.0 / 1.45 (chosen)** | **0.18 %** | **99.8** |
| 6.0 / 1.55 | 0.12 % | 103.1 |
| 4.0 / 1.30 | 0.08 % | 93.7 |

4.0 / 1.45 removes most clipping at the old brightness. Across the full 13-view district capture
(`tools/visual_review/capture_mwezi_quarter_district.gd`) the mean clipped fraction fell from
**0.76 % to 0.18 %**. Opened: the east-alley wall before/after crop
(`artifacts/visual-review/grade/compare_v3.png`) — the sunlit house front keeps a warm off-white
with shading and the stair edges read, where it was a flat white slab.

## 4. Route reading — lit thresholds

`tools/level/build_mwezi_quarter_district.py` now generates a warm light (2.8 m up, 5.5 m range)
over every objective arrival: the three pickups, workbench, Scout arena, save point and beacon.
The next place to go is the lit one. Visual only; no collision; the layout's clearance asserts
are unchanged. Test: `TestObjectivesHaveThresholdLights`.

Opened: `artifacts/visual-review/workstream_a_visual_before_after.png` (spawn, east alley,
workbench courtyard, beacon; before left, after right). The metal pickup's arrival and the
workbench courtyard now sit in warm pools where the ground was dark. On the beacon plaza the warm
pool competes with the beacon's own violet glow, which is less saturated than before.

The eight world views' mean luma rose 98.0 → 107.6; about 2 points of that is the grade and the
rest is these lights.

**Worn-ground strips were tried and removed.** The ground near the routes is several overlapping
visual layers (procedural terrain; Stage A fractured ground up to 0.64 m; ash patches), so no
single flat strip height lies on the visible surface, and three attempts were not visible at eye
level. A `Decal` is the right tool, but the Compatibility renderer the review captures run on
cannot draw decals, so it could not be verified here. Ground wear moves to workstream D, whose
textured-materials pass is where it belongs. Two generator defects found on the way were fixed
before removal: a variable shadowing that wrote a node name into a colour (the district failed
to load — caught by opening the frames), and a basis written in column order that flipped the
strips face-down.

## Harness

| Mode | Result | Legs | Game time | Exceptions | Findings |
|---|---|---|---|---|---|
| Journey | victory | 10 / 10 | 49.82 s (+2 s: the longer hold) | 0 | none |
| Defeat | respawned at 1.78 m, 100 health | 11 / 11 | 52.85 s | 0 | none |

`dotnet test` 118 / 118; integration suite pass with all new tests; smoke verifier exit 0.

## Aesthetic verdict

Reviewer: Visual Reviewer agent (`studio/decisions/quality_benchmark_v2_agent_gate_delegation.md`).

Opened: `16_beacon_climax.png`, `17_99_end_screen.png`, `workstream_a_visual_before_after.png`,
`grade/compare_v3.png`, `ending/candidates_sheet2.png`.

Diagnosis: the payoff now has a focal point — the lit spire centred against the sea with the
beam rising, framed by the tower — and the victory screen keeps it in view. Lime walls hold
edge and plaster shading in direct light. Objective arrivals are the brightest ground in their
frames. Still weak: the ground between objectives carries no route of its own (ground wear,
deferred to D); the sea is a flat plane (D); materials are flat colour (D).

Verdict: `PASS` for the four items above, on in-game frames. This is not the workstream A exit:
that also needs human playtest note #1.
