# Quality Scorecard Log

Running, append-only history of the 10-axis quality scorecard defined in
`studio/decisions/quality_benchmark_v1.md`. Each PR that touches a benchmarked axis adds one entry below —
this file is the trend line; the ADR is the fixed target.

Do not edit or delete prior entries. If a score was wrong, add a new entry that corrects it and say why.

## How to add an entry

1. Copy the table below.
2. Fill in today's score per axis (0–10) and the peer target from the ADR.
3. Fill `Δ` with the change since the previous entry (`+`, `-`, or `=`).
4. Cite the evidence for every score that changed — a file, log, test run, or screenshot path. No evidence,
   no score change.
5. Append it under "Entries", newest last.

```
### <date> — <PR link or commit sha>

| # | Axis | Score /10 | Peer target | Δ | Evidence |
|---|---|---:|---:|---|---|
| 1 | Core gameplay loop |  | 9.0 |  |  |
| 2 | Combat & enemy AI |  | 9.0 |  |  |
| 3 | Movement & controls |  | 9.5 |  |  |
| 4 | Crafting & progression |  | 8.5 |  |  |
| 5 | World / level design |  | 8.5 |  |  |
| 6 | Visual art & presentation |  | 9.0 |  |  |
| 7 | Audio & feedback |  | 8.5 |  |  |
| 8 | Technical stability |  | 8.0 |  |  |
| 9 | Content volume / replayability |  | 9.0 |  |  |
| 10 | Process integrity of studio claims | | n/a |  |  |

**Composite (axes 1–9):**
**Note:**
```

## Entries

### 2026-07-06 — baseline (repo audit, no PR)

| # | Axis | Score /10 | Peer target | Δ | Evidence |
|---|---|---:|---:|---|---|
| 1 | Core gameplay loop | 6.0 | 9.0 | — | `CrashSiteMissionState` + tests: full collect→craft→fight→recover→beacon path wired and passing. |
| 2 | Combat & enemy AI | 3.0 | 9.0 | — | `GalaxabrainScout` 4-state FSM tested; no human feel pass exists. |
| 3 | Movement & controls | 3.0 | 9.5 | — | `FirstPersonController`/`FirstPersonMovement` tested; `20260703_windows_manual_validation_blocked.md` records feel as BLOCKED. |
| 4 | Crafting & progression | 5.0 | 8.5 | — | One recipe (`MechanicalArmRecipe`), consumes resources correctly per tests. |
| 5 | World / level design | 3.0 | 8.5 | — | One ~150×150 m scene; no verified layout critique on file. |
| 6 | Visual art & presentation | 2.0 | 9.0 | — | `docs/production/known-blockers.md`: Stage A custom art still unapproved; scenes lean on third-party kit assets. |
| 7 | Audio & feedback | 2.0 | 8.5 | — | Non-priority per README; only an `AudioCue` hook exists. |
| 8 | Technical stability | 7.0 | 8.0 | — | `dotnet build`: 0 warnings/errors. `dotnet test`: 71/71 gdUnit4 tests pass. Windows CI smoke-launch exits 0. |
| 9 | Content volume / replayability | 2.0 | 9.0 | — | One map, one enemy, one weapon, ~10–30 min, one ending, by MVP design. |
| 10 | Process integrity of studio claims | 2.0 | n/a | — | `studio/tasks/PRE_BETA_AUDIT_COMPLETE.md` asserts unverifiable FPS/draw-call/GPU numbers and a Windows human playthrough with no supporting artifact; contradicts `known-blockers.md` from the same day. |

**Composite (axes 1–9):** 3.7 / 10 (peer average ≈8.8 / 10)
**Note:** This entry is the baseline `studio/decisions/quality_benchmark_v1.md` was built from. Future entries
track movement from here — the goal is the Δ column trending toward the peer target column, with cited
evidence for every change.

### 2026-07-06 — claude/visual-benchmark-6-10-blomv3 (no PR yet)

| # | Axis | Score /10 | Peer target | Δ | Evidence |
|---|---|---:|---:|---|---|
| 1 | Core gameplay loop | 6.0 | 9.0 | = | Unchanged this pass. |
| 2 | Combat & enemy AI | 3.0 | 9.0 | = | Unchanged this pass. |
| 3 | Movement & controls | 3.0 | 9.5 | = | Unchanged this pass. |
| 4 | Crafting & progression | 5.0 | 8.5 | = | Unchanged this pass. |
| 5 | World / level design | 3.0 | 8.5 | = | Unchanged this pass; still one scene, sparse/disconnected ground patches (see axis 6 evidence). |
| 6 | Visual art & presentation | 3.0 | 9.0 | +1.0 | `docs/release/evidence/titancraft-visual-axis6-pass-2026-07-06.md`: real Godot+Xvfb screenshot capture confirmed working in-container (corrects prior no-display assumption); opened 8 before/after production PNGs; fixed a real floating-geometry defect (orphaned `OrangeVent` accents left visible after their parent wall panels were hidden) and rebalanced blown-highlight exposure / pure-black ambient via `Environment`/`DirectionalLight3D` values in `scenes/Main/Main.tscn`. Still kit/placeholder-heavy, still no continuous terrain, Stage A still formally unapproved per `docs/production/known-blockers.md` — does not reach the 9.0 target or the studio's 6.0 aspirational milestone; +1.0 reflects only the cited, verified defect fixes, not a general art-quality claim. |
| 7 | Audio & feedback | 2.0 | 8.5 | = | Unchanged this pass. |
| 8 | Technical stability | 7.0 | 8.0 | = | Unchanged this pass; `dotnet build`/`dotnet test` (71/71) re-verified during this pass. |
| 9 | Content volume / replayability | 2.0 | 9.0 | = | Unchanged this pass. |
| 10 | Process integrity of studio claims | 2.0 | n/a | = | Unchanged this pass; see capability correction note in axis 6 evidence file — prior "no display in container" assumption (`MEM-QUALITY-BENCHMARK-003`) was disproved this pass for visual capture specifically (does not extend to human feel/hardware claims). |

**Composite (axes 1–9):** 3.8 / 10 (peer average ≈8.8 / 10)
**Note:** Axis 6 moved 2.0 → 3.0 on cited, opened before/after PNG evidence for two specific, verified
defect fixes (see evidence file). This explicitly does **not** claim the 6/10 milestone the branch name
targets — reaching 6.0 needs continuous terrain (not sparse patches), replacement of placeholder standee
assets, and a human/Art Director aesthetic sign-off, none of which this pass attempted (out of scope for
a single lighting/bugfix pass; see `CLAUDE.md` §9 agent handoff).

### 2026-07-07 — claude/blender-assets-scene-integration-kyt4th (no PR yet)

| # | Axis | Score /10 | Peer target | Δ | Evidence |
|---|---|---:|---:|---|---|
| 1 | Core gameplay loop | 6.0 | 9.0 | = | Unchanged this pass. |
| 2 | Combat & enemy AI | 3.0 | 9.0 | = | `src/Player/FirstPersonController.cs` now plays a distinct `Weapon_Ready` cue when the Mechanical Arm's cooldown ends (previously that audio asset was produced but never triggered by any code — confirmed via repo-wide grep before the change). This is a functional completeness fix, not a feel improvement: per `quality_benchmark_v1.md` binding rule 2, "combat is satisfying" and equivalents require a dated human playtest in a non-headless environment, which this session cannot produce. Score held at 3.0 (logic tested, feel still unverified) rather than raised. |
| 3 | Movement & controls | 3.0 | 9.5 | = | `FirstPersonController` now triggers footstep audio (cycling the three already-produced but previously unused `Footsteps_Metal/Rock/Ash` clips) on a walk/sprint timer while grounded and moving. This is not real surface-material detection (no such tagging exists on ground meshes) — it is a fixed rotation, documented as such in code. Same binding-rule-2 constraint as axis 2: functional wiring verified, "feels responsive" is not claimed and needs a human pass. Score held at 3.0. |
| 4 | Crafting & progression | 5.0 | 8.5 | = | Unchanged this pass. |
| 5 | World / level design | 3.0 | 8.5 | = | Unchanged this pass. |
| 6 | Visual art & presentation | 3.0 | 9.0 | = | Unchanged this pass (prior session on this branch added Blender Asset Forge candidates and cinematic-polish UI reveals; see branch history, not re-scored here). |
| 7 | Audio & feedback | 2.0 | 8.5 | +0.5 | Same footstep/weapon-ready wiring as axes 2–3 above: two previously-silent, already-produced audio layers (`AudioLayer_Player/Footsteps_*`, `AudioLayer_Player/Weapon_Ready`) are now actually triggered, confirmed present at their expected NodePaths via a headless GDScript instantiation of `Main.tscn` (`AudioLayer_Player/Footsteps_Metal` etc. resolved to real `AudioStreamPlayer3D` nodes). Still placeholder-sourced audio per README's non-priority stance on audio; +0.5 reflects closing a concrete "asset exists, never plays" gap, not a mix/design quality claim. |
| 8 | Technical stability | 7.0 | 8.0 | = | Re-verified: `dotnet build` 0 warnings/errors; `dotnet test` 71/71 gdUnit4 passed; `godot --headless --path . --import` 0 errors. Additionally ran `tests/Integration/IntegrationTestRunner.tscn` headlessly (a full Godot-scene integration suite this session had not previously run) and found it failing on stale assertions from an earlier commit on this same branch: three `Require` calls in `TestGalaxabrainScoutDeathPickup()` and `TestDefeatedScoutPersistenceAcrossReload()` still asserted `!scout.Visible` after death, left over from before `GalaxabrainScout.Die()`/`RestoreDefeated()` were changed (this branch, prior commit) to keep the corpse visible via a visual-root swap instead of hiding the whole body. Fixed the assertions to check the new alive/disabled visual-root swap instead of reverting the behavior; reran to a clean `TITANCRAFT_INTEGRATION_TESTS_PASS` with all 11 MVP smoke milestones passing. This is the process-integrity axis 10 evidence for this entry, not scored here as a stability gain — it's a correction of this session's own prior gap (didn't run this suite before declaring the earlier commit done). |
| 9 | Content volume / replayability | 2.0 | 9.0 | = | Unchanged this pass. |
| 10 | Process integrity of studio claims | 2.0 | n/a | = | See axis 8 evidence: a prior commit on this branch was declared verified (`dotnet build`/`dotnet test`/scene-load checks) without running `tests/Integration/IntegrationTestRunner.tscn`, which would have caught the stale-assertion regression immediately. Recorded here rather than silently fixed, per this axis's own purpose. |

**Composite (axes 1–9):** 3.8 / 10 (peer average ≈8.8 / 10, unchanged from prior entry — the +0.5 on axis 7
does not move the rounded composite)
**Note:** This pass explicitly does not claim a "10/10 MVP." Per `quality_benchmark_v1.md` binding rule 2,
no agent in a headless/no-display environment may assert that combat, movement, or level flow "feels"
better — axes 2 and 3 remain at 3.0 (logic/wiring verified, feel `HUMAN_BLOCKED`) despite the additions
above. A genuine push toward 10/10 on this benchmark needs, at minimum: a dated human playtest for axes
2/3/9, Stage A visual approval for axis 6, and the Stage B→C review chain this branch has been operating
under an explicit human override of (see prior commits) resolved properly rather than bypassed.

### 2026-07-07 (second pass same day) — claude/blender-assets-scene-integration-kyt4th (no PR yet)

| # | Axis | Score /10 | Peer target | Δ | Evidence |
|---|---|---:|---:|---|---|
| 1 | Core gameplay loop | 6.0 | 9.0 | = | The 6-step progression (collect → build arm → defeat Scout → recover component → save → activate beacon) now has an audio beat at 3 of its intermediate transitions (see axis 7). This closes a completeness gap in how the loop communicates its own progress, not a loop-structure change; score held, since the axis's stated gap ("single path") is about content breadth, not feedback, and is unaffected. |
| 2 | Combat & enemy AI | 3.0 | 9.0 | = | Unchanged this pass; see prior entry's binding-rule-2 constraint. |
| 3 | Movement & controls | 3.0 | 9.5 | = | Unchanged this pass. |
| 4 | Crafting & progression | 5.0 | 8.5 | = | Workbench crafting success now plays `UI_Craft_Complete` (previously produced, never triggered — confirmed via grep before this change). Held at 5.0: this is audio-feedback completeness on an already-scored system, not a change to the recipe/progression design itself. |
| 5 | World / level design | 3.0 | 8.5 | = | Unchanged this pass. |
| 6 | Visual art & presentation | 3.0 | 9.0 | = | Unchanged this pass. |
| 7 | Audio & feedback | 2.5 | 8.5 | +0.5 | Wired four more previously-silent, already-produced audio cues (confirmed via grep showing zero call sites before this change, and a headless GDScript instantiation of `Main.tscn` confirming all four resolve to real `AudioStreamPlayer` nodes at their expected paths afterward): `Save_Complete` on `CrashSiteSaveCoordinator.SaveGame()` success, `Load_Complete` on `LoadGameIfPresent()` success, `State_Objective` on the resource-collection-complete, component-recovery, and Scout-defeat transitions, and `UI_Craft_Complete` on Workbench crafting success. Deliberately left unwired: `Save_Progress` (no clean semantic moment for a synchronous, near-instant save — wiring it would mean firing it and `Save_Complete` in the same frame, which is contrived, not a real "in progress" state); `UI_Select`/`UI_Hover`/`UI_Menu_Toggle` and the Beacon→Victory transition's own `State_Objective` (Main.tscn's audio layers are unreachable once `MainMenu.tscn`/`VictoryScreen.tscn`/`DefeatScreen.tscn` load as standalone scene roots — Main unloads first — so wiring those would need new local `AudioStreamPlayer` nodes added to four separate scene files, a materially larger change than this pass scoped; Beacon activation already gets its own distinct Victory sting from the end screen, so adding `State_Objective` there would just be redundant, overlapping audio). Same binding-rule-2 caveat as before: this is wiring completeness, not a "sounds good" claim. |
| 8 | Technical stability | 7.0 | 8.0 | = | Re-verified: `dotnet build` 0 warnings/errors; `dotnet test` 71/71; `tests/Integration/IntegrationTestRunner.tscn` rerun to `TITANCRAFT_INTEGRATION_TESTS_PASS` with all 11 MVP smoke milestones (including the save/load persistence test that now also exercises the new Save/Load audio calls) after the audio wiring above. |
| 9 | Content volume / replayability | 2.0 | 9.0 | = | Unchanged this pass. |
| 10 | Process integrity of studio claims | 2.0 | n/a | = | Unchanged this pass — no new claim requiring correction found. |

**Composite (axes 1–9):** 3.8 / 10 (peer average ≈8.8 / 10; the +0.5 on axis 7 does not move the rounded
composite)
**Note:** Same constraint as the prior entry: no claim of "10/10" or of improved feel. This pass closes
audio-completeness gaps only (assets that existed and were silent now play at their intended moments);
axes 2/3/9 remain gated on a human playtest, axis 6 on Stage A approval, and axis 5 on unverified layout
claims — none of which this pass touched.
