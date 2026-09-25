# Top-Tier Single-Scene Plan — "One scene, one player, one experience"

**Status:** ACTIVE since 2026-09-25. Gate 0 met (`docs/production/playtests/2026-09-25-journey.md`,
`PASS`); decisions D1–D7 approved by the product owner and recorded in README §6 (amendment of
2026-09-25), `studio/decisions/quality_benchmark_v3_single_scene_axis9.md` (D6) and
`studio/agents/audio_director.md` (D7). Workstream A is next.
**Date:** 2026-09-25
**Owner:** Producer (orchestration), Game Director (experience), Creative Director (vision)
**Relationship to `phase-7-planning.md`:** keeps its audio, balance and polish workstreams and
their gates; replaces its sequencing, because Phase 7 was written to extend a finished MVP
sideways, while this plan deepens a single scene into a complete experience.

---

## 1. The direction, stated as a measurable target

The product owner set the post-MVP direction on 2026-09-25: **one scene, one player, one
experience**, made top tier. That rules breadth out on purpose. Top tier here means what the
best single-level work in the genre achieves: every minute authored, one core mechanic explored
completely, a pacing arc with a real climax, and a place that tells its own story.

Reference anchors for this direction (craft references, not content targets):

| What | Anchor | The lesson for TitanCraft |
|---|---|---|
| One mechanic, fully explored | Titanfall 2 — *Effect and Cause*; Portal | The Mechanical Arm is the game. Teach it, test it, twist it, master it — inside one space |
| A known space re-read by new ability | Half-Life 2 — gravity gun; Metroid-style gating | Each arm module reopens the same quarter differently |
| Encounter pacing and a climax | Doom Eternal arenas; Half-Life 2 set pieces | Escalating fights that end in an event, not a button press |
| Place as story | What Remains of Edith Finch; Half-Life 2 | The Mwezi Quarter explains itself without dialogue |
| Atmosphere through sound | Subnautica; Doom Eternal | Audio is half of "being there" |

**Headline gap, measured:** an optimal route finishes the whole MVP in **about 50 seconds** of
game time (three walked playthroughs, 2026-09-24). README §5 targets 10–30 minutes. Everything
else in this plan matters less than turning 50 seconds into 20–30 minutes of *authored* play —
density, not size.

**Honest scale note:** the peers above were built by large teams over years; README §34 budgets
this project at five hours a weekend. "Top tier" is therefore a bar for craft *within this
scope*, judged by the measurable exits below and by human playtests — not a claim of parity with
AAA production values. Workstreams are ordered by quality gained per hour so that stopping at any
gate still leaves the best possible game.

## 2. Decisions the product owner must make first

Nothing below starts until these are recorded as a README amendment.

| # | Decision | Recommendation |
|---|---|---|
| D1 | Keep forbidden: multiplayer, multiple maps, procedural world, voxels, cloud, telemetry, large mech, complete rocket | **Keep.** They are what "one scene, one player" means |
| D2 | Arm progression: Mk I → Mk II → Mk III modules | **Unlock.** Already a locked decision ("bras mécanique évoluant vers un exosquelette", §34); this makes it the spine |
| D3 | Enemy variety: behaviour variants within the Galaxabrain family | **Unlock, capped at 3 variants.** One species, different jobs (Scout, a heavier Warden, a swarm drone) — depth, not a bestiary |
| D4 | A climax encounter at the beacon (holdout or boss) | **Unlock.** §4 lists bosses; the current climax is a button press |
| D5 | Grappling / wall running | **Keep forbidden** unless they arrive as arm modules that serve D2. Movement verbs dilute a single scene fast |
| D6 | Re-anchor quality-benchmark axis 9 | **Amend** `quality_benchmark_v1.md`: axis 9's anchors (Valheim, Subnautica) measure breadth this direction rules out. Re-anchor to single-level density: authored minutes, encounters, secrets, replay reasons |
| D7 | Add an audio owner to the studio | **Add `audio_director`.** Axis 7 has the second-largest gap (3.5 → 8.5) and no agent owns it |

## 3. Where the game stands

Recorded scores (`docs/production/quality-scorecard-log.md`, composite 5.4):

| # | Axis | Score | Target | What the 2026-09-24 playtest adds |
|---|---|---:|---:|---|
| 1 | Core loop | 6.0 | 9.0 | Complete and walkable; ~50 s long |
| 2 | Combat & AI | 4.5 | 9.0 | One fight, 7–9 swings; the Scout reads clearly |
| 3 | Movement | 4.0 | 9.5 | Walk, look, jump (1.07 m) verified; no movement verbs beyond these |
| 4 | Crafting | 5.5 | 8.5 | One recipe, one craft |
| 5 | World / level | 6.5 | 8.5 | Routes open; navigation carried by HUD text, not the street |
| 6 | Visual | 8.5 | 9.0 | Scored on asset renders. In play: blank climax frame, flat victory screen, lime walls clipping, HUD overlap. Expect an in-game rescore below 8.5 |
| 7 | Audio | 3.5 | 8.5 | Three ambient loops are wired with sounds and never started — the quarter is silent |
| 8 | Technical | 7.5 | 8.0 | Windows export and smoke pass; rendered performance on real hardware has never been measured |
| 9 | Content / replay | 2.0 | 9.0 | See D6 |
| 10 | Process integrity | 4.0 | n/a | Evidence gates working; the playthrough harness caught what the suite could not |

## 4. Workstreams, in order

Each has an owner, a measurable exit, and a gate. The playthrough harness
(`tools/visual_review/playthrough_journey.gd`) re-runs at **every** gate: completion, time,
stuck events and defeat recovery must never regress.

### Gate 0 — MVP done (prerequisite)
- **Owner:** Gameplay Engineer; QA Lead verifies.
- Fix the beacon climax (defer the end-screen scene change; test the real transition with scene
  changes enabled) and the onboarding collect step.
- **Exit:** both harness modes pass; playtest verdict `PASS`; README §6/§34 amendment recorded.

### A — Make what exists land (highest return per hour)
- **Owners:** UX Designer, Audio Director (D7), Art Director.
- Start the three ambient loops and layer them by position (sea, wind, quarter).
- Make the climax an event: beam, shake, sound, a held beat before the victory screen; replace
  the flat victory screen with an in-world ending shot.
- HUD: remove the overlap, shrink the panel, player-facing names for every prompt, reword the
  startup hint.
- Fix lime-render clipping in the grade; add eye-level route readability (ground wear, lit
  thresholds) so the street, not the HUD, shows the way.
- **Exit:** visual `PASS` on in-game frames (not asset renders); no HUD overlap in any harness
  frame; ambient audio audible at every leg; human playtest note #1 (section 5).

### B — The arm becomes the game (D2)
- **Owners:** Game Director (design), Gameplay Engineer, Level Designer.
- Mk I strike (exists) → Mk II pull/grab (move debris, open blocked alleys, disarm hazards) →
  Mk III charged strike or shield (breaks the Warden's guard). Each module is crafted from what
  the previous one unlocks.
- Rework the quarter into layers the modules open in turn — the same streets re-read three
  times. The existing clearance-asserting level generator extends to gated routes.
- **Exit:** each module is taught without text, tested in isolation, then combined; harness
  walks the full gated route; bot-optimal completion at least 8 minutes.

### C — Encounters and the climax (D3, D4)
- **Owners:** Gameplay Engineer, Level Designer; Game Director owns the pacing curve.
- Three to five authored encounters escalating across the quarter, each built around one arm
  module and one Galaxabrain variant. The beacon becomes a holdout set piece in the harbour
  plaza: the first time every module is needed at once.
- A written intensity curve (calm → discovery → pressure → climax → release) that every
  encounter is placed against.
- **Exit:** harness completes every encounter on real input; bot-optimal completion at least 15
  minutes; defeat recovery verified inside each encounter; human playtest note #2.

### D — The place tells its story
- **Owners:** Creative Director, Art Director, Level Designer.
- Environmental storytelling for why the quarter is empty and what the Galaxabrains are doing
  here; optional discoveries that reward looking (feeds re-anchored axis 9).
- The harbour gets water (resolve the terrain boundary that occludes it) and the seafront kit
  is placed; materials move from flat colour to textured.
- **Exit:** visual `PASS` on a full in-game capture set; every optional discovery reachable by
  the harness; human median session 20–30 minutes.

### E — Feel and finish
- **Owners:** Gameplay Engineer, UX Designer, QA Lead, Build & Release Engineer.
- Tuning passes driven by human playtest notes; rendered performance measured on a real
  Windows machine against README §28 (60 fps); accessibility basics (remapping, sensitivity,
  subtitles for any voiced line).
- **Exit:** rendered frame-time capture on target hardware meets §28; human playtest note #3
  with no P0/P1 feel findings; composite score with every axis at or above its target minus 1.0,
  each backed by evidence.

## 5. What agents cannot supply

Under the v2 delegation ADR the studio can gate stability, aesthetics and completion without a
human. It cannot claim feel, and top tier is largely feel. This plan therefore schedules **three
dated human playtest notes** (after A, C and E) in `docs/production/playtests/`, using
`TEMPLATE-windows-human-playtest.md`. They are not a formal gate for the journey verdict; they
are the only evidence that can move axes 2 and 3 toward their targets, and without them the
plan cannot claim to have reached them.

Rendered performance on real hardware (workstream E) likewise needs a human's machine or a GPU
runner; the CI Windows runner has no GPU and its figure is a headless proxy.

## 6. Studio operating rules for this plan

- One workstream item per task, through `tools/agent_preflight.py`, per `AGENTS.md` §3.
- Every gameplay change ships with a test on the real path — the beacon defect survived because
  the suite disabled the scene change it depended on.
- The harness is a gate, not a demo: a regression in completion, time or defeat recovery blocks
  the gate.
- Scores move only with cited evidence (`quality_benchmark_v1.md` rules 1 and 3), and feel
  language only with a dated human note.

## 7. Verdict

`PASS` — active. Gate 0 is met and D1–D7 are recorded.

**Added to workstream A on 2026-09-25:** 28 of the game's 35 audio files are digital silence
(playtest finding 7). "Start the three ambient loops" becomes "replace the silent library with
project-authored audio, starting with ambience" — starting silent loops would change nothing the
player hears.

## 8. Progress

| Date | Workstream item | Evidence | Verdict |
|---|---|---|---|
| 2026-09-25 | A — HUD: overlap removed, player-facing prompts, startup hint reworded (panel not yet shrunk) | `docs/production/playtests/2026-09-25-journey.md`, addendum | `PASS` |
| 2026-09-25 | A — ambience authored, looped, started and layered by place; audible at every walked leg | `docs/audio/ambience-pass-2026-09-25.md` | `PASS` |

Still open in A: the remaining 25 silent files (cue audio), the in-world ending shot, lime-render
clipping, eye-level route readability, a smaller HUD panel, and human playtest note #1.
