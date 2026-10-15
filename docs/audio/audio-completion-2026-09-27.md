# Audio Completion — 2026-09-27 (workstream A, audio item 3)

**Owner:** Audio Director (D7), with the Gameplay Engineer for wiring and tests.
**Plan item:** `docs/production/top-tier-single-scene-plan.md`, workstream A — replace the silent
library with project-authored audio. This closes playtest finding 7
(`docs/production/playtests/2026-09-25-journey.md`): **no committed audio file is silent.**

## What was left, and what happened to each

| File | Played by | Resolution |
|---|---|---|
| `ui/select_01`, `ui/hover_01`, `ui/menu_toggle_01` | `PauseMenu` (`UI_Select`, `UI_Hover`, `UI_Menu_Toggle`) | authored; see the pause defect below |
| `ui/craft_complete_01` | `FirstPersonController` on crafting (`UI_Craft_Complete`) | authored as an assembly layer (clicks, servo, latch) under the workbench's README §16 craft tone |
| `state/objective_complete_01` | `FirstPersonController` (`State_Objective`) | authored |
| `save/save_complete_01`, `save/load_complete_01` | `CrashSiteSaveCoordinator` | authored |
| `state/defeat_01` | `DefeatScreen` (`DefeatAudio`, autoplay) | authored. Losing was silent: this file played on every defeat |
| `state/mission_complete_01` | nothing | authored and **wired**: `CrashSiteEndScreenNavigator` plays it when victory is requested, so it swells under the lit beacon during the 3 s hold |
| `enemy/death_01` | nothing | authored and **wired**: `GalaxabrainScout.Die` plays it over the Scout's README §16 death tone |
| `pickup/metal_01`, `organic_01`, `glass_01` | nothing (four `AudioLayer_Pickup` nodes nobody played) | authored; each placed pickup's `SpatialPickupPlayer` now plays its own type instead of the shared temporary chime; the unused nodes are removed |
| `pickup/generic_01` | nothing | authored; the default stream of `scenes/Resources/ResourceDrop.tscn` for any drop without an override |
| `state/victory_01` | nothing (the victory screen plays the README §16 sting) | **deleted** with its node |
| `save/save_progress_01` | nothing | **deleted** with its node |

Also removed from `Main.tscn`, as duplicates nothing played: `State_Defeat` (the defeat screen
has its own player) and `Scout_Idle` (a pickup chime labelled as Scout breathing).

README §16's seven temporary tones are unchanged except the collect chime, which the three
placed pickups no longer use: each now plays an authored per-type sound, which still meets §16's
"son pour collecte".

## A second defect the silent files had hidden

`PauseMenu` sets `GetTree().Paused = true` and then plays its toggle; select and hover play
while paused. `AudioLayer_UI` inherited the default pausable process mode, so every pause-menu
sound was paused with the tree and could never be heard. `AudioLayer_UI` now always processes.
New test `TestPauseMenuAudioPlaysWhilePaused` failed on the old setting ("UI_Menu_Toggle is
paused with the tree, so the pause menu cannot be heard") and passes on the fix.

The Scout-death player and the three pickup players joined `TestCloseCuesKeepTheirVolume`, the
`max_db` clamp check from the cue pass.

## Evidence — walked playthrough (captured audio, onsets and peak dBFS)

| Cue | Journey | Defeat run |
|---|---|---|
| Metal pickup | 1, −15.5 | 0 (route does not collect Metal) |
| Biomass pickup | 1, −13.5 | 1, −13.5 |
| Electronics pickup | 1, −15.5 | 1, −15.5 |
| Save_Complete | 1, −16.0 | 1, −16.0 |
| State_Objective | 3, −14.5 | 0 |
| UI_Craft_Complete | 4, −13.5 | 0 (arm never built) |
| Scout_Death | 2, −7.0 | 0 (Scout survives) |
| State_Mission_Complete | 1, −12.4 | 0 |
| DefeatAudio (defeat screen) | — | 1, −10.3 |

Onsets count 10 ms blocks rising above −60 dBFS, so a cue with internal gaps (the craft
assembly's three clicks, the death cry's rattle) reads more than once per play. The pause-menu
cues are not exercised by the harness (it never pauses); the pause test covers them.

| Mode | Result | Legs | Game time | Exceptions | Findings |
|---|---|---|---|---|---|
| Journey | victory | 10 / 10 | 47.82 s | 0 | none |
| Defeat | respawned at 1.78 m, 100 health | 11 / 11 | 52.85 s | 0 | none |

No regression against the weapon-cue pass (47.82 s; 52.85 s). The harness now counts the 90
frames it waits on the defeat screen as game time, as it did before it sampled audio there.

## Tests

- `tools/test_audio_sources.py`: 27 files, 27 audible, 0 known silent. `KNOWN_SILENT` is empty,
  so any silent file is now a failure.
- Integration suite: pass, including `TestPauseMenuAudioPlaysWhilePaused` and the extended
  `TestCloseCuesKeepTheirVolume`; smoke verifier exit 0.

## Limits

- **No one has listened to any of this.** Levels are measured on the digital mix and set to
  peak targets, not by ear. Human playtest note #1 is where that is judged.
- Every cue has one variant.
- `AudioCue` finds scene-level players through `GetTree().Root.GetChild(0)`. That is the game
  scene in play, but in the integration runner it is the runner, so the suite cannot hear
  scene-level cues; the harness can. Worth replacing with `CurrentScene` in a later task.

## Verdict

`PASS`: the audio library has no silent file, every authored file has a trigger or a default
use, two useless placeholders are gone, and a pause defect is fixed with a test.
