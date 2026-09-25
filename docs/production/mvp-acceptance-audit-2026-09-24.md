# MVP Acceptance Audit — 2026-09-24

**Question:** is the Crash Site MVP fully done, so that post-MVP work can be unlocked?
**Answer:** **not yet.** Every README §30 acceptance criterion is met on evidence, but README §32
("done") is not: the game's climax throws an unhandled exception and one shipped feature gives
the player a wrong instruction. Two small, repository-owned fixes stand between the current
build and done.

**Verdict:** `FAIL_REPO_OWNED`

Evidence sources:
- Windows runner: workflow run 36070332245 on `main` (`bbfd14c`), exe SHA-256
  `4083688b7e5632d2ed270a19d03961273a646a1d9c0df52f95d378a791482a4f`.
- Walked playthroughs on real input (`tools/visual_review/playthrough_journey.gd`): three valid
  journey runs (takes 1, 4, 5) and one defeat-and-respawn run (`-- --defeat`).
- `docs/production/playtests/2026-09-24-journey.md` — the playtest verdict and its findings.
- `./tools/test.sh` on this branch: build 0 errors, 116/116 unit, integration pass, smoke 11/11.

## README §30 — acceptance criteria

| # | Criterion | Met | Evidence |
|---|---|---|---|
| 1 | The project launches from Godot | yes | Every playthrough loaded `Main.tscn`; `godot --headless --import` exit 0 |
| 2 | The C# project compiles without errors | yes | `./tools/test.sh`: 0 errors; Windows runner build in run 36070332245 |
| 3 | The player can walk | yes | 28 of 30 designed legs walked on real input across three runs |
| 4 | The player can look around | yes | Mouse-look steering drove every leg; Look onboarding step advanced on it |
| 5 | The player can jump | yes | **Measured for the first time:** 1.07 m rise, landed (take 5). The suite only checked the key mapping |
| 6 | Collect three resource types | yes | Metal, Biomass, Electronics collected by `interact` in every run |
| 7 | Quantities are displayed | yes | HUD Resources line tracked each pickup |
| 8 | The workbench can be used | yes | Craft interaction succeeded on the first attempt in every run |
| 9 | The Mechanical Arm can be built | yes | "Mechanical Arm Mk I online" in every run |
| 10 | Resources are consumed correctly | yes | HUD 10/3/2 before crafting, 0/0/0 after (take 4 frames 06→07) |
| 11 | The Galaxabrain detects the player | yes | The Scout closed on the player unprompted (walked 3.75 m of an 8.36 m leg) |
| 12 | The Galaxabrain pursues the player | yes | As 11; the component dropped where the Scout died, not at its home |
| 13 | The Galaxabrain attacks | yes | 4 hits landed on the player in every journey run |
| 14 | The player can take damage | yes | 100 → 60 health in every journey run |
| 15 | The player can attack with the arm | yes | Strikes landed on real `attack` input; "strike landed" feedback |
| 16 | The Galaxabrain can die | yes | Beaten in 7, 9 and 9 swings |
| 17 | The mission component appears | yes | Visible at the death point; objective switched to recovery |
| 18 | The player can recover it | yes | Recovered on the first `interact` attempt |
| 19 | The beacon can be activated | **yes, defective** | The mission completes, but activation throws `NullReferenceException` at `Beacon.cs:119` in 3 of 3 runs; the beam and activation sound never play |
| 20 | The victory screen appears | yes | `VictoryScreen.tscn` reached in 3 of 3 runs |
| 21 | The player respawns after death | yes | **Verified on the real path for the first time** (the suite disables scene changes): died in 7.43 s, defeat screen shown, real click on "Reload Last Save", respawned at the save point (1.44 m) on 100 health; progress before the save kept, progress after it correctly lost |
| 22 | Local save works | yes | Checkpoint written at the save point and read back (criterion 21) |
| 23 | The game can be resumed | yes | Relaunch resumes the checkpoint — observed directly when two early runs resumed a stale save |
| 24 | Works without an Internet connection | yes | No network API anywhere in `src/` |
| 25 | A Windows build launches outside Godot | yes | `TitanCraft.exe` on Windows NT 10.0.26100: exit code 0, 600 frames |
| 26 | The full loop completes in under 30 minutes | yes | 49.77–52.60 s of game time on the optimal route |
| 27 | No known blocking bug prevents finishing | yes | Neither defect below stops the mission; victory reached every run |

**27 of 27 met.**

## README §32 — definition of done

"A task is done when … the main errors are handled; … the feature works in Godot; … no existing
feature is broken." Two defects fail it, both reproduced on every valid run:

1. **The beacon activation, the game's climax, never plays** (finding 1 of the playtest
   verdict). Completing the beacon moves the mission to Victory, which changes scene
   synchronously; Godot 4 removes the current scene from the tree at once, so the rest of
   `ActivateExtraction()` runs with no tree. The exception is unhandled, and an existing feature
   (the beam, camera shake and activation sound) is silently lost. The integration suite cannot
   see it because it disables the scene change in every scenario.
2. **Onboarding tells the player to build before they can** (finding 2). The first pickup
   jumps the tutorial to "Return to the workbench and press E to build" with 1 of 3 resources in
   hand; the Collect prompt is never shown. Introduced by PR #134.

## What unlocking needs

1. Fix both defects, with tests that run the real paths: the victory transition with scene
   changes enabled, and onboarding across all three pickups.
2. Re-run `playthrough_journey.gd` (both modes) and commit a `PASS` playtest verdict.
3. A product-owner decision to amend README §6 and §34. Nothing in the repository defines a
   post-MVP unlock; the long-term vision (§4) states it "is not an authorisation to develop these
   features". Moving past the MVP is therefore an explicit amendment, in the same way as the
   2026-09-17 setting amendment. The proposal is in `docs/production/top-tier-single-scene-plan.md`.

## Also observed, not blocking

- The route from the Scout's death point to the save point runs into a debris block near
  (2.6, −11.3) in 2 of 3 runs; a visible gap lets a player walk round it.
- The save point sits in a pocket that a straight walk from the Biomass pickup cannot reach
  (defeat run); it is reached from the east, as the designed route intends.
- An optimal route finishes the whole MVP in about 50 seconds of game time, against the README
  §5 session target of 10–30 minutes. That is not an acceptance failure (§30 #26 is a ceiling),
  but it is the single most important fact for what comes after the MVP.
