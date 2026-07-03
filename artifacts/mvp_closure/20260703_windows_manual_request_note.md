# Windows Manual Playthrough Request Note — Crash Site MVP

| Field | Value |
| --- | --- |
| Date | 2026-07-03 |
| Requested target | Windows PC, exported TitanCraft Crash Site build from the current commit |
| Current commit | `6bbe9388ba6280253581aaf21b60f5011ba5db89` |
| Verdict vocabulary | `ENVIRONMENT_BLOCKED` for this container-run note; manual Windows verdict still required |
| Evidence owner | Codex session |

## Scope requested

The requested validation is a native Windows playthrough of the exported Crash Site build from the current commit, covering the README §5 victory condition and README §7 gameplay loop:

1. Start a new game from `scenes/UI/MainMenu.tscn`.
2. Confirm spawn near the crashed ship.
3. Collect metal, biomass, and electronic components.
4. Craft the Mechanical Arm Mk I at `Placeholder_Workbench`.
5. Defeat the single `GalaxabrainScout`.
6. Recover the Galaxabrain component.
7. Activate `Placeholder_Beacon`.
8. Confirm `VictoryScreen.tscn` appears.
9. Repeat once using Continue after saving at `Placeholder_SavePoint`.

Qualitative focus areas: movement feel, mouse look, combat readability, objective clarity, interactable readability at normal play distance, Windows performance, and launch behavior.

## Result

`ENVIRONMENT_BLOCKED`: this execution environment is a Linux container, not a Windows PC. I cannot truthfully run or manually feel-test the exported Windows build here, and no Wine/Windows host is available in the container session.

No manual Windows gameplay result is claimed by this note.

## Evidence reviewed from the repository

Existing MVP closure evidence already documents automated/runtime coverage of the same Crash Site loop and a prior Windows CI launch smoke:

- `artifacts/mvp_closure/runtime_playthrough.md` records a 15/15 PASS Godot-runtime playthrough matrix for the README §5 / §7 loop on Linux runtime evidence, while explicitly marking Windows-native manual execution as environment-blocked in the container.
- `artifacts/mvp_closure/export/export_evidence.md` records a successful Windows export and Windows CI native smoke launch of `TitanCraft.exe --headless --quit-after 120`.
- `artifacts/mvp_closure/final_mvp_verdict.json` records `windows_launch_pass: true` from CI smoke evidence and keeps manual Windows playthrough as remaining non-blocking work.

## Manual Windows procedure still required

Run this on a real Windows PC using an exported build produced from the current commit:

1. Launch `TitanCraft.exe` from the exported Windows folder, keeping the adjacent `data_TitanCraft_windows_x86_64/` folder next to the executable.
2. From `MainMenu.tscn`, start **New Game**.
3. Verify the player spawns close to the crashed ship and cannot immediately see confusing alternate paths.
4. Move with keyboard controls, look with the mouse, and jump; record whether movement and camera feel responsive and comfortable.
5. At normal play distance, locate and collect the metal, biomass, and electronic component sources; record whether prompts and object contrast are understandable.
6. Return to `Placeholder_Workbench`, craft Mechanical Arm Mk I, and confirm HUD/objective/arm feedback is clear.
7. Fight the single `GalaxabrainScout`; record whether attack range, hit feedback, enemy state, and damage readability are understandable.
8. Recover the Galaxabrain component and confirm the next objective is obvious.
9. Activate `Placeholder_Beacon` and confirm `VictoryScreen.tscn` appears.
10. Relaunch or return to the menu, use **Continue** after saving at `Placeholder_SavePoint`, and repeat the end-to-end loop once.
11. Capture Windows performance and launch notes: startup success/failure, crashes, average perceived FPS/stutter, input lag, and any antivirus/SmartScreen friction.

## Suggested evidence format for the human Windows pass

| Check | PASS / FAIL / BLOCKED | Notes |
| --- | --- | --- |
| Windows launch behavior | TBD |  |
| New Game from Main Menu | TBD |  |
| Spawn near crashed ship | TBD |  |
| Movement feel | TBD |  |
| Mouse look | TBD |  |
| Resource collection clarity | TBD |  |
| Workbench / arm crafting clarity | TBD |  |
| Combat readability | TBD |  |
| Component recovery clarity | TBD |  |
| Beacon activation clarity | TBD |  |
| Victory screen appears | TBD |  |
| Continue after save point | TBD |  |
| Interactables readable at normal distance | TBD |  |
| Windows performance | TBD |  |

## Closure impact

This note preserves the closure record without overstating evidence. Automated and CI evidence remain useful, but they do not replace the requested human Windows PC playthrough for gameplay feel and readability.
