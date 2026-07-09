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

### 2026-07-07 (third pass same day) — claude/blender-assets-scene-integration-kyt4th (no PR yet)

| # | Axis | Score /10 | Peer target | Δ | Evidence |
|---|---|---:|---:|---|---|
| 1 | Core gameplay loop | 6.0 | 9.0 | = | Unchanged this pass. |
| 2 | Combat & enemy AI | 3.0 | 9.0 | = | Unchanged this pass. |
| 3 | Movement & controls | 3.0 | 9.5 | = | Unchanged this pass. |
| 4 | Crafting & progression | 5.0 | 8.5 | = | Unchanged this pass. |
| 5 | World / level design | 3.0 | 8.5 | +0.5 | `docs/art/crash-site-object-asset-inventory.md`'s "Distant silhouettes" entry calls for three named variants (basalt ridge, alien arc, smoke plume); `scenes/Main/Main.tscn`'s `DistantRock_4..7` background nodes were, until this pass, four plain scaled `BoxMesh` cubes (confirmed by reading the file before touching it) — an unfinished placeholder, not tuned art. Built `tools/blender/create_distant_silhouettes_kit_v1.py` (3 low-poly variants, 84-96 triangles each, all `BLENDER_ASSET_VALID`), rendered review PNGs (opened and visually checked before integrating), converted to embedded `.gltf`, and replaced all four `DistantRock` placeholders in `Main.tscn` (kept prior positions, cleaned the non-uniform placeholder scale to uniform). Still +0.5 not more: only one gap in one axis-5 sub-area closed; axis 5's stated blocker ("layout claims unverified") is untouched, and this is background dressing, not level layout. |
| 6 | Visual art & presentation | 3.0 | 9.0 | = | Held at 3.0 despite the same work as axis 5: still Stage-A-unapproved by `docs/production/known-blockers.md`, still no continuous terrain, still kit-heavy. This pass replaced one small placeholder category, not enough to move a 9.0-target axis; recorded under axis 5 instead since it is level-background completeness, not an art-direction milestone. |
| 7 | Audio & feedback | 2.5 | 8.5 | = | Unchanged this pass. |
| 8 | Technical stability | 7.0 | 8.0 | = | Re-verified after the `Main.tscn` change: `godot --headless --path . --import` 0 errors; a headless GDScript instantiation of `Main.tscn` confirmed all four `DistantRock_N/DistantSilhouetteModel` nodes resolve; `dotnet test` 71/71; `tests/Integration/IntegrationTestRunner.tscn` rerun to `TITANCRAFT_INTEGRATION_TESTS_PASS` (all 11 MVP smoke milestones) after the scene edit, confirming the background swap didn't regress anything the integration suite covers (it doesn't assert on `DistantRock` specifically — grepped `tests/` first to confirm zero references before assuming this was safe). |
| 9 | Content volume / replayability | 2.0 | 9.0 | = | Unchanged this pass. |
| 10 | Process integrity of studio claims | 2.0 | n/a | = | Unchanged this pass. |

**Composite (axes 1–9):** 3.9 / 10 (peer average ≈8.8 / 10)
**Note:** Deliberately did not touch `TC_HeavyCrashHull_V1`/`TC_TERRAIN_CrashBasin_V1` against the existing
StageA hull/terrain in this pass either — same reasoning as the prior session on this branch: that art was
deliberately tuned (see the exposure-rebalance commit in this repo's history) and swapping it for a
redundant candidate would be bulk replacement of working art, not a targeted fix. The distant-silhouette
swap was safe specifically because the placeholder it replaced was unfinished (plain boxes), not tuned.

### Correction — axis 5 transcription error in the entry above

The "third pass" table above lists axis 5's `Δ` as `+0.5` but left the **Score /10** cell at `3.0` instead
of `3.5` — a copy/paste error, not an intentional score. The composite of `3.9` already used the *correct*
value (35.0 summed with 3.5, not 3.0, ÷ 9 = 3.89 ≈ 3.9), so the reported composite number was right; only
the axis-5 score cell in that row was wrong. Recorded here per this log's own rule ("do not edit prior
entries; add a new entry that corrects it and say why") rather than silently editing the table above.

### 2026-07-07 (fourth pass same day) — claude/blender-assets-scene-integration-kyt4th (no PR yet)

| # | Axis | Score /10 | Peer target | Δ | Evidence |
|---|---|---:|---:|---|---|
| 1 | Core gameplay loop | 6.0 | 9.0 | = | Unchanged this pass. |
| 2 | Combat & enemy AI | 3.0 | 9.0 | = | Unchanged this pass. |
| 3 | Movement & controls | 3.0 | 9.5 | = | Unchanged this pass. |
| 4 | Crafting & progression | 5.0 | 8.5 | = | Unchanged this pass. |
| 5 | World / level design | 3.5 | 8.5 | = | Unchanged this pass (corrected score carried forward; see correction note above). |
| 6 | Visual art & presentation | 3.0 | 9.0 | = | Unchanged this pass. |
| 7 | Audio & feedback | 2.5 | 8.5 | = | Unchanged this pass. |
| 8 | Technical stability | 7.5 | 8.0 | +0.5 | Ran the actual documented Windows export procedure end to end for the first time on this branch (`docs/testing.md` "Windows offline MVP build verification"): `python3 tools/prepare_audio_assets.py` (materializes the 7 temp audio cues that every earlier session this branch had been missing, confirmed by the "resource file not found" errors disappearing from `godot --headless --path . --import` afterward), then `godot --headless --path . --export-release "Windows Desktop" builds/Windows/TitanCraft.exe`. First attempt surfaced two real, previously-undiscovered defects via actual ERROR-level export log lines (not assumed): `scenes/World/Phase7_Composition.tscn` referenced a `SubResource("Environment")` before its declaration (Godot's format-3 parser requires declaration-before-use; fixed by reordering the sub_resource block before the referencing node) and `scenes/CrashSite_MVP.tscn` referenced a script, `src/Core/CrashSiteMVPController.cs`, that does not exist anywhere in the repo (confirmed via `find`) — `docs/art/PHASE_7_EXECUTION_GUIDE.md` shows this was one of two scene-naming options considered ("`Main.tscn` or `CrashSite_MVP.tscn`"); `Main.tscn` is the one actually built out, so this was superseded, unreferenced (confirmed via repo-wide grep) dead scaffolding, not a live dependency — deleted rather than patched. Re-ran import (0 errors) and export (0 errors, `file` confirms a valid `PE32+ executable ... for MS Windows, 12 sections`) after both fixes. `dotnet test` (71/71) and `tests/Integration/IntegrationTestRunner.tscn` (`TITANCRAFT_INTEGRATION_TESTS_PASS`, all 11 milestones) re-verified after the scene edits/deletion, since grep confirmed nothing in `tests/` referenced either file. Held at 7.5, not the full 8.0 target: the export itself is now clean and verified, but the executable actually **running** on real Windows hardware remains unverified in this container, per the existing documented blocker (`artifacts/mvp_closure/20260703_windows_manual_validation_blocked.md`) — that gap is real and this pass does not close it. |
| 9 | Content volume / replayability | 2.0 | 9.0 | = | Unchanged this pass. |
| 10 | Process integrity of studio claims | 2.0 | n/a | = | See the correction note above — recorded honestly rather than left uncorrected. |

**Composite (axes 1–9):** 3.9 / 10 (peer average ≈8.8 / 10 — 35.5/9 = 3.94, rounds to 3.9)
**Note:** Asked to push this to 4.5 this pass. Doing the math honestly: 4.5 needs a summed total of 40.5
across 9 axes; the actual total after this pass is 35.5. Axes 2, 3, and 9 cannot move without a human
playtest or a README scope change (both outside what a headless agent pass can produce), which leaves
axes 1, 4, 5, 6, 7, 8 to carry the full +5.0 still needed — and axis 8 alone had only 1.0 of headroom left
before this pass, now largely spent. Reaching 4.5 honestly needs several more independently-verified
passes like this one and the three before it (each has moved the needle roughly +0.05 to +0.1 in the
rounded composite), not one turn. This pass's real, verified contribution: a Windows export that failed
silently-by-omission before (never actually attempted with evidence on this branch) now succeeds cleanly,
and two real, previously-undiscovered scene defects are fixed.

### 2026-07-07 (fifth pass same day) — claude/blender-assets-scene-integration-kyt4th (no PR yet)

| # | Axis | Score /10 | Peer target | Δ | Evidence |
|---|---|---:|---:|---|---|
| 1 | Core gameplay loop | 6.0 | 9.0 | = | Unchanged this pass. |
| 2 | Combat & enemy AI | 3.0 | 9.0 | = | Unchanged this pass. |
| 3 | Movement & controls | 3.0 | 9.5 | = | Unchanged this pass. |
| 4 | Crafting & progression | 5.0 | 8.5 | = | Unchanged this pass. |
| 5 | World / level design | 3.5 | 8.5 | = | Unchanged this pass. |
| 6 | Visual art & presentation | 4.0 | 9.0 | +1.0 | Captured fresh screenshots via `xvfb-run -a godot --path . --script tools/visual_review/capture_phase3a_production_integration.gd` and found the single worst visual defect in the whole scene: `ProceduralCrashSiteTerrain` was `visible = false` in `scenes/Main/Main.tscn` (confirmed by reading the file), so every asset — hull, workbench, save point, beacon, pickups, distant silhouettes — appeared as tiny disconnected debris floating in a flat void with no ground plane at all. Set it `visible = true` and re-captured: this fixed the void/connectivity but revealed a second real defect — the terrain rendered solid unlit black. Ruled out shadow-mapping as sole cause via a disable-shadows diagnostic (partial improvement only, most of the surface stayed black). Read `src/World/ProceduralCrashSiteTerrain.cs`'s `AddColored()`/`AddTriangle()` and found both computed the face normal as `(b - a).Cross(c - a).Normalized()`; hand-checked this against the actual grid-triangulation winding used in `BuildMesh()` (representative vertices a=(0,0,0), b=(1,0,0), c=(1,0,1)) and got a cross product of `(0,-1,0)` — pointing straight down through the terrain, away from the light, explaining the near-zero direct lighting. Flipped both to `(c - a).Cross(b - a).Normalized()`, rebuilt (`dotnet build`, 0 errors), restored the shadow-disable diagnostic back to `shadow_enabled = true` (that was a test, not the fix), re-imported, and re-captured all 8 production screenshots. Opened `production_01_spawn_overview.png`, `production_04_resource_workbench_zone.png`, and `production_08_wide_terrain_composition.png` before/after: terrain now renders as a continuous, connected, properly lit surface with visible directional-light gradient and per-zone albedo tones (brown/ash/basalt), not solid black. New, disclosed limitation found while checking: `production_01` shows a visible hard vertical seam/step where two terrain zone meshes meet in the near-camera foreground (a sharp height discontinuity with a hard shadow edge) — not fixed this pass, this is a separate zone-blending issue, not the normal-inversion bug. Held at 4.0, not higher: this fixes a rendering-breaking defect (the worst-case failure for this axis — a broken void), not the underlying art-direction gaps (still Stage-A-unapproved per `docs/production/known-blockers.md`, still low-poly kit assets, now also has a newly-visible zone-seam artifact). This is the largest single verified jump this axis has had this session because the prior state was not merely "unpolished" but actually broken (no ground plane rendering at all). |
| 7 | Audio & feedback | 2.5 | 8.5 | = | Unchanged this pass. |
| 8 | Technical stability | 7.5 | 8.0 | = | Re-verified after the terrain/scene changes: `dotnet build TitanCraft.sln` 0 errors; `godot --headless --path . --import` 0 errors; `dotnet test` 71/71 passed; `godot --headless --path . tests/Integration/IntegrationTestRunner.tscn` reached `TITANCRAFT_INTEGRATION_TESTS_PASS` with all 11 MVP smoke milestones (the `LocalSaveGameStore` "unreadable save data" line in the log is an expected negative-path warning from that same suite's load-corruption test, not a regression). No change to the score: this pass re-confirmed the existing 7.5, it didn't close the still-open Windows-hardware-execution gap. |
| 9 | Content volume / replayability | 2.0 | 9.0 | = | Unchanged this pass. |
| 10 | Process integrity of studio claims | 2.0 | n/a | = | Unchanged this pass. |

**Composite (axes 1–9):** 4.1 / 10 (peer average ≈8.8 / 10 — 36.5/9 = 4.055, rounds to 4.1)
**Note:** This pass fixed a genuine rendering bug (inverted terrain normals causing solid-black, disconnected
terrain) rather than adding new art assets — the kind of fix that matters most for a first impression, since
a broken void is worse than a merely unpolished level. It does not constitute Stage A visual approval, a feel
claim, or a "10/10" — those remain blocked exactly as stated in every prior entry. The newly-observed
terrain-zone seam in `production_01` is disclosed above rather than cropped out of the evidence.

### 2026-07-07 (sixth pass same day) — claude/blender-assets-scene-integration-kyt4th (no PR yet)

Asked this pass to "push category 5 6 7 to their max." Read that as: close every real,
headlessly-verifiable gap in World/level design, Visual art & presentation, and Audio & feedback that
doesn't require a feel claim or Stage A human sign-off — not literally reach 10/10, which those two
constraints still block regardless of how much work is done in this container.

| # | Axis | Score /10 | Peer target | Δ | Evidence |
|---|---|---:|---:|---|---|
| 1 | Core gameplay loop | 6.0 | 9.0 | = | Unchanged this pass. |
| 2 | Combat & enemy AI | 3.0 | 9.0 | = | Unchanged this pass. |
| 3 | Movement & controls | 3.0 | 9.5 | = | Unchanged this pass. |
| 4 | Crafting & progression | 5.0 | 8.5 | = | Unchanged this pass. |
| 5 | World / level design | 4.0 | 8.5 | +0.5 | `docs/art/crash-site-object-asset-inventory.md`'s "Foreground occluders" entry lists "Rock occluder" as a required variant ("Chunky partial forms near camera edges... Terrain and wreckage palettes... Knee to above-player"). `scenes/Main/Main.tscn`'s three close-range, player-visible `VolcanicRock_1/2/3` blocking props (confirmed via grep before touching them) still used a plain scaled `BoxMesh` (`BoxMesh_rock`), never replaced with anything matching that spec. Built `tools/blender/create_rock_occluder_kit_v1.py` (one chunky, irregular, flat-bottomed low-poly boulder — a jittered icosphere, 20 triangles, `BLENDER_ASSET_VALID`, reusing the exact albedo of `assets/Materials/VolcanicRock.tres` for tone continuity), rendered and opened a review PNG before integrating, exported to GLB, converted to embedded `.gltf`, and instanced it as `RockOccluderModel` under each `VolcanicRock_N` — leaving each prop's separate `Collision_BlockingRock` `CollisionShape3D` sibling completely untouched (mesh-only swap, gameplay-blocking behavior unchanged). Removed the now-orphaned `BoxMesh_rock` sub_resource. Added the new asset to `assets/Production/Generated/asset_manifest.json` and to `.github/workflows/blender-asset-forge.yml`'s build/validate/export/render steps so CI keeps producing it. Still +0.5 not more: this closes one named placeholder gap, not the "hull rib occluder" or "cable occluder" variants the same inventory entry also lists (not built — no scene currently needs them; not manufacturing unused assets speculatively), and it doesn't touch the still-unverified broader layout claims axis 5 remains blocked on. |
| 6 | Visual art & presentation | 5.0 | 9.0 | +1.0 | While verifying the rock-occluder swap with a dedicated close-range camera test (`godot --script`, not part of the official 8-shot capture), found the swap itself was clean but a large, unrelated solid-black cuboid was visible near it regardless of hiding the rock's mesh or its collision shape. Isolated the real cause by hiding candidate nodes one at a time: `ProceduralCrashSiteTerrain/HorizonSegments/HorizonSegment_06` — one of the six background "horizon ridge" terrain prisms built by `BuildHorizonSegmentMeshes()`. Hiding it made the cube disappear; re-enabling it and checking the other five segments from dedicated per-segment cameras showed the same solid-black defect on multiple of them, not just one — a second, distinct inverted-normal bug from the one fixed earlier this session (that fix, to the shared `AddColored`/`AddTriangle` cross-product order, corrected `BuildPrism` shapes built from `IrregularPolygon`'s point ordering, but the horizon segments use a separate, hand-authored fixed polygon array with the opposite winding, so the same shared-function fix left them wrong). Fixed by reversing the point order of that specific polygon array in `BuildHorizonSegmentMeshes()` (isolated to that one call site; does not touch `IrregularPolygon`-based shapes, which stay correct). Rebuilt and re-verified empirically per-segment: segments 1 and 6 both now render with proper directional-light gradient shading instead of solid black. Re-ran the full 8-shot official production capture afterward and confirmed no regression (`production_01`, `production_08` visually compared against the prior pass, unchanged). This is the same class of "broken, not just unpolished" fix as the terrain-visibility bug two passes ago — a full +1.0 rather than the usual +0.5, because six background shapes going from unlit-black to properly shaded is a first-impression-level fix, not incremental polish. Held at 5.0, not higher: still Stage-A-unapproved per `docs/production/known-blockers.md`, still low-poly kit assets throughout, and a still-undiagnosed additional dark wedge shape was spotted near the crashed-ship silhouette during this investigation (not chased further this pass — noted here rather than silently dropped, since it may be a legitimate hard shadow or a third distinct defect requiring separate investigation). |
| 7 | Audio & feedback | 3.0 | 8.5 | +0.5 | Grepped every `AudioCue.Play` call site against `scenes/Main/Main.tscn`'s `AudioLayer_UI` node and found three authored, scene-wired `AudioStreamPlayer` nodes — `UI_Select`, `UI_Hover`, `UI_Menu_Toggle` — that no code anywhere called (confirmed via repo-wide grep for their string paths returning zero code hits before this pass). `src/UI/PauseMenu.cs`'s `TogglePause()`, `Resume()`, `SaveGame()`, `ReturnToMainMenu()`, and `QuitGame()` had zero audio feedback despite the scene already shipping cues for exactly this. Wired `UI_Menu_Toggle` into `TogglePause()`, `UI_Select` into the four button-driven actions, and added a new `PlayHoverSound()` method wired to each of the four `PauseMenu.tscn` buttons' `mouse_entered` signal for `UI_Hover`. `AudioCue.Play`'s scene-root fallback resolves correctly here since `PauseMenu` and `AudioLayer_UI` are both direct children of `Main` (confirmed via grep, not assumed). Re-ran `dotnet test` (71/71) and `IntegrationTestRunner` (`TITANCRAFT_INTEGRATION_TESTS_PASS`, 11/11) after the change. Still +0.5 not more: this closes one specific, fully-verifiable silent-UI gap; the axis's larger blockers (weapon/enemy audio feel, mixing, spatialization quality) are feel claims that stay `HUMAN_BLOCKED` regardless. |
| 8 | Technical stability | 7.5 | 8.0 | = | Re-verified after all changes this pass: `dotnet build` 0 errors, `godot --headless --path . --import` 0 errors, `dotnet test` 71/71, `IntegrationTestRunner` → `TITANCRAFT_INTEGRATION_TESTS_PASS` (11/11 milestones). No change to the score itself; this pass didn't touch the still-open Windows-hardware-execution gap. |
| 9 | Content volume / replayability | 2.0 | 9.0 | = | Unchanged this pass. |
| 10 | Process integrity of studio claims | 2.0 | n/a | = | Unchanged this pass. |

**Composite (axes 1–9):** 4.3 / 10 (peer average ≈8.8 / 10 — 38.5/9 = 4.278, rounds to 4.3)
**Note:** "Max" this pass means "every gap closeable without a feel claim or Stage A sign-off," not 10/10 —
axis 6 stays capped by unapproved Stage A art regardless of bug fixes, and axis 7's larger gaps (mixing,
spatialization, weapon feel) are feel claims that stay `HUMAN_BLOCKED` in this container by the ADR's own
binding rule 2. Disclosed rather than hid the one loose thread from this pass: an unidentified dark wedge
shape near the crashed-ship silhouette, spotted but not chased down, flagged for a future pass.

### 2026-07-07 (seventh pass same day) — claude/blender-assets-scene-integration-kyt4th (no PR yet)

User supplied 8 AAA-cinematic-style reference images (a main menu screen and in-game combat/base shots)
and asked to build "a complete scene like these" then merge to main. Two real conflicts surfaced and were
put to the user rather than silently actioned: (1) the references show a large background mech and a
much bigger/different enemy than the documented single Galaxabrain Scout — both explicitly forbidden by
`README.md`/`CLAUDE.md`'s MVP scope; (2) the references are path-traced cinematic renders with sculpted,
fully-textured models — a different content pipeline entirely from this project's deliberate flat-shaded
low-poly Blender kit style, not a gap closeable by "trying harder" in this container. User chose, explicitly:
use the images as mood/palette/composition reference only (no large mech, no new enemy type), and
confirmed "integrate to live production" means merge this branch into `main`.

| # | Axis | Score /10 | Peer target | Δ | Evidence |
|---|---|---:|---:|---|---|
| 1 | Core gameplay loop | 6.0 | 9.0 | = | Unchanged this pass. |
| 2 | Combat & enemy AI | 3.0 | 9.0 | = | Unchanged this pass. Explicitly did not add the second/larger enemy type shown in the reference images — forbidden by `README.md`. |
| 3 | Movement & controls | 3.0 | 9.5 | = | Unchanged this pass. |
| 4 | Crafting & progression | 5.0 | 8.5 | = | Unchanged this pass. |
| 5 | World / level design | 4.0 | 8.5 | = | Unchanged this pass (the shard swap below is recorded under axis 6, matching how the equivalent rock-occluder swap was categorized last pass). |
| 6 | Visual art & presentation | 5.5 | 9.0 | +0.5 | The reference images' most prominent recurring motif — jagged violet crystal shards scattered around the base/arena — maps directly onto `docs/art/crash-site-object-asset-inventory.md`'s documented "Alien shard props" entry ("Jagged dark violet shards with irregular lean... Required variants: Small, medium, embedded cluster"). `scenes/Main/Main.tscn`'s five `AlienCrystal_1..5` route/arena props (grepped and confirmed before touching) all reused one plain 4-sided `PrismMesh` pyramid at different uniform scales — a placeholder, not the documented jagged/irregular-lean silhouette. Built `tools/blender/create_alien_shard_kit_v1.py` (three variants — small/medium/embedded-cluster, 16/16/48 triangles, all `BLENDER_ASSET_VALID`, reusing `assets/Materials/AlienVioletEmissive.tres`'s exact color for continuity), opened all three variants' review PNGs before integrating, exported to GLB, converted to embedded `.gltf`, and instanced them across the same 5 existing node positions (no increase in instance count — the inventory's own entry explicitly forbids "crystal forest expansion"). Verified placement/scale with a dedicated close-range camera test before trusting the wide production shots; confirmed the shard silhouettes read as broken/angular rather than the old clean-toy-pyramid look, both close-up and at gameplay-representative distance. Added the asset to `asset_manifest.json` and the CI workflow. +0.5, not more: this is one documented placeholder swap in the same spirit as the terrain/horizon/rock-occluder fixes from the last two passes, not a broader art pass — still Stage-A-unapproved, still low-poly throughout. |
| 7 | Audio & feedback | 3.0 | 8.5 | = | Unchanged this pass. |
| 8 | Technical stability | 7.5 | 8.0 | = | Re-verified after the swap: `dotnet build` 0 errors, `godot --headless --path . --import` 0 errors, `dotnet test` 71/71, `IntegrationTestRunner` → `TITANCRAFT_INTEGRATION_TESTS_PASS` (11/11 milestones). |
| 9 | Content volume / replayability | 2.0 | 9.0 | = | Unchanged this pass. |
| 10 | Process integrity of studio claims | 2.0 | n/a | = | Recorded the scope conflict and its resolution above rather than quietly complying with (or quietly refusing) an out-of-scope request. |

**Composite (axes 1–9):** 4.3 / 10 (peer average ≈8.8 / 10 — 39.0/9 = 4.333, rounds to 4.3)
**Note:** The user's actual ask this pass ("a complete scene like these") is not achievable in this container
at the fidelity shown — that is stated plainly above, not worked around. What shipped instead is the one
real, scope-safe, documented gap the reference images pointed at.

### 2026-07-07 (eighth pass same day) — codex/polish-details-production-integration (no PR yet)

User asked to push categories 5/6/7 toward 10/10 with Blender and production import. Blender was not
installed in this container (`blender --version` returned command-not-found), so this pass did not author a
new `.blend` live; it imported the already-authored Blender Asset Forge `TC_PolishDetails_V1` candidate into
production as a conservative, visual-only scene dressing slice.

| # | Axis | Score /10 | Peer target | Δ | Evidence |
|---|---|---:|---:|---|---|
| 1 | Core gameplay loop | 6.0 | 9.0 | = | Unchanged this pass. |
| 2 | Combat & enemy AI | 3.0 | 9.0 | = | Unchanged this pass. |
| 3 | Movement & controls | 3.0 | 9.5 | = | Unchanged this pass. |
| 4 | Crafting & progression | 5.0 | 8.5 | = | Unchanged this pass. |
| 5 | World / level design | 4.0 | 8.5 | = | Added surface-detail dressing at three existing production locations only: crash wreck, workbench approach, and beacon route. This supports landmark readability, but does not change route topology, objectives, collision, encounter layout, or the broader human-playtest blocker, so no axis-5 score change is claimed. |
| 6 | Visual art & presentation | 5.5 | 9.0 | = | Converted the existing Blender-authored `assets/Production/Generated/PolishDetails/TC_PolishDetails_V1.glb` to an embedded Godot `.gltf` and instanced it in `scenes/Main/Main.tscn` as three visual-only detail strips. A headless node-contract check confirmed all three `PolishDetailsModel` instances resolve. No score increase is claimed because PNG screenshot evidence could not be produced in this environment (`xvfb-run` missing; `--headless` capture has an empty dummy viewport), and Stage A visual approval remains blocked. |
| 7 | Audio & feedback | 3.0 | 8.5 | = | Unchanged this pass. |
| 8 | Technical stability | 7.5 | 8.0 | = | Re-verified after scene/import changes: `python3 tools/validate_agent_studio.py`, `git diff --check`, `godot --headless --path . --import`, `dotnet build TitanCraft.sln --nologo`, and a headless node-contract script for the three new detail instances. No change to Windows-hardware execution evidence. |
| 9 | Content volume / replayability | 2.0 | 9.0 | = | Unchanged this pass. |
| 10 | Process integrity of studio claims | 2.0 | n/a | = | Recorded the Blender/screenshot environment limitations rather than claiming 10/10 or visual approval without required evidence. |

**Composite (axes 1–9):** 4.3 / 10 (peer average ≈8.8 / 10 — unchanged from the prior rounded composite)
**Note:** This is a safe production integration pass for an existing Blender candidate, not a new art-fidelity
breakthrough. The visual/runtime gates remain separate: runtime import and node resolution pass; visual
approval remains `ENVIRONMENT_BLOCKED`/`HUMAN_BLOCKED` until PNG review and human or visual-reviewer verdict exist.

### 2026-07-09 — claude/agent-studio-mvp-closure-9s383i (MVP scope-closure pass, documentation only)

| # | Axis | Score /10 | Peer target | Δ | Evidence |
|---|---|---:|---:|---|---|
| 1 | Core gameplay loop | 6.0 | 9.0 | = | Unchanged; re-verified 71/71 unit tests + `TITANCRAFT_INTEGRATION_TESTS_PASS` on HEAD `d2a754a`. |
| 2 | Combat & enemy AI | 3.0 | 9.0 | = | Unchanged; feel still `HUMAN_BLOCKED` per binding rule 2. |
| 3 | Movement & controls | 3.0 | 9.5 | = | Unchanged; same constraint. |
| 4 | Crafting & progression | 5.0 | 8.5 | = | Unchanged. |
| 5 | World / level design | 3.5 | 8.5 | = | Unchanged. |
| 6 | Visual art & presentation | 5.5 | 9.0 | = | Unchanged; Stage A overall approval still open per `known-blockers.md`. |
| 7 | Audio & feedback | 3.0 | 8.5 | = | Unchanged. |
| 8 | Technical stability | 7.5 | 8.0 | = | Re-verified on HEAD `d2a754a`: `dotnet build` 0/0, `dotnet test` 71/71, import exit 0, integration suite PASS (log in `docs/production/mvp-closure-report-2026-07-09.md`). Windows-hardware execution gap still open, so no score change. |
| 9 | Content volume / replayability | 2.0 | 9.0 | = | Unchanged. |
| 10 | Process integrity of studio claims | 2.0 | n/a | = | This pass was asked to "push the report to 10/10 MVP." Handled per this log's own precedent: the closure report (`mvp-closure-report-2026-07-09.md`) records 10/10 only for README §30 scope completion (27/27, evidence-cited) and issues an explicit `NOT_GO` + contradiction report for any quality-benchmark 10/10 claim. No axis score was moved without evidence. |

**Composite (axes 1–9):** 4.3 / 10 (peer average ≈8.8 / 10 — unchanged; documentation-only pass)
**Note:** This entry closes the Crash Site MVP **scope** (README §30: 27/27 criteria, repository-owned
evidence, re-validated on today's HEAD). Scope completion and quality are different axes, per
`quality_benchmark_v1.md`'s own Context section — this entry moves neither the composite nor any axis.

### 2026-07-09 (second entry) — claude/agent-studio-mvp-closure-9s383i (reference-mood visual pass)

User supplied the same class of cinematic reference images as the 2026-07-07 seventh pass and asked to
"build the all scene assets and visual and apply to prod." Handled under the standing 2026-07-07 user
decision: mood/palette/composition reference only — no large mech, no second enemy, no fidelity claim.
Blender is not installed in this container, so this pass is scene-level art direction (text-format scene,
material, and UI edits only), with before/after PNGs captured via xvfb and opened for diagnosis.

| # | Axis | Score /10 | Peer target | Δ | Evidence |
|---|---|---:|---:|---|---|
| 1 | Core gameplay loop | 6.0 | 9.0 | = | Unchanged this pass. |
| 2 | Combat & enemy AI | 3.0 | 9.0 | = | Unchanged this pass. |
| 3 | Movement & controls | 3.0 | 9.5 | = | Unchanged this pass. |
| 4 | Crafting & progression | 5.0 | 8.5 | = | Unchanged this pass. |
| 5 | World / level design | 4.0 | 8.5 | = | Unchanged (the new SignalSpire/Moon are sky dressing recorded under axis 6, per the rock-occluder precedent). |
| 6 | Visual art & presentation | 6.0 | 9.0 | +0.5 | `docs/release/evidence/titancraft-visual-reference-mood-pass-2026-07-09.md`: before-state flat navy void sky (no moon, no landmark, unreadably weak practicals, default-grey main menu) replaced with a dusk violet/orange grade, glowing moon, 52 m emissive signal-spire skyline landmark, warm base pools vs violet counter-pool, and a styled main menu matching the reference structure — all verified by opened before/after PNGs from the same 8 production cameras plus a new menu capture. Two conditions the 2026-07-06 entry attached to reaching 6.0 (continuous terrain; placeholder standee swaps) were closed in intervening passes; the third (human/Art Director aesthetic sign-off) remains open and is stated in the evidence file — this raise reflects the verified closure of the sky/landmark/menu gaps, not an aesthetic approval, and 6.0 remains an agent score pending human confirmation. Disclosed limitation: the moon renders elliptically at the widest review-camera FOV. |
| 7 | Audio & feedback | 3.0 | 8.5 | = | Unchanged this pass. |
| 8 | Technical stability | 7.5 | 8.0 | = | Re-verified after all scene/UI edits: `dotnet build` 0/0, `dotnet test` 71/71, import exit 0, `TITANCRAFT_INTEGRATION_TESTS_PASS` (menu node contracts intact — original `Menu/*` paths deliberately preserved). |
| 9 | Content volume / replayability | 2.0 | 9.0 | = | Unchanged this pass. |
| 10 | Process integrity of studio claims | 2.0 | n/a | = | The forbidden elements in the references (large mech, second enemy) were again not built; the fidelity gap is stated in the evidence file rather than papered over. |

**Composite (axes 1–9):** 4.4 / 10 (peer average ≈8.8 / 10 — 39.5/9 = 4.39, rounds to 4.4)
**Note:** No 10/10 claim. Axis 6's remaining gap to its 9.0 target is art-direction depth (sculpted
assets, texture work, human sign-off), not bug closure — the classes of work this container can verify
are increasingly exhausted; the next real moves on this axis need Blender-authored assets and a human
aesthetic pass.

### Clarification — Stage A reconciliation scope vs. axis 6 (2026-07-07)

`docs/production/current-status.md`'s "Stage A Reconciliation — 2026-07-07" section grants a PASS for
`TC_TerrainDioramaKit_V1` specifically (a standalone-review-artifact process gate for one asset: PNG
diagnosis, Visual Reviewer sign-off, production integration sign-off). That is a different question from this
axis's quality-bar gate, which scores the overall visual presentation against a 9.0 peer target and remains at
5.5 (kit/placeholder-heavy throughout, no marketing-grade art pass). Both are correct simultaneously: one
asset's process gate can close while the axis-level quality gate stays open. `docs/production/known-blockers.md`
has been updated the same day to state this distinction explicitly, so future entries should keep citing axis 6
as blocked on overall quality, not imply that the terrain-diorama reconciliation raises this axis.
