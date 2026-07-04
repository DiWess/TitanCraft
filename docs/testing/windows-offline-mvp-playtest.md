# Windows Offline MVP Playtest Checklist

This checklist is the required manual validation procedure before TitanCraft can make any Windows offline MVP readiness claim. It records human playtest evidence for the exported Windows build only; it does not replace automated restore, build, import, or export checks.

## Scope guard

- Validate only the locked Crash Site MVP described in `README.md`.
- Do not treat this checklist as approval for multiplayer, cloud services, telemetry, multiple maps, multiple enemy types, wall running, grappling hooks, voxels, a large mech, or a complete rocket.
- Do not claim Windows readiness from this document alone. A completed evidence record in `docs/testing/windows-playtest-evidence-template.md`, with the actual Windows build artifact and tester findings, is required before any readiness claim can be considered.

## Required test environment

- [ ] Use a clean Windows install or a real Windows VM.
- [ ] Record the exact Windows version and build number.
- [ ] Record whether the run uses physical hardware or a VM.
- [ ] Record CPU, GPU, RAM, display resolution, input devices, and any VM graphics settings.
- [ ] Start from a fresh extracted/exported build folder, not from the Godot editor.
- [ ] Confirm no developer tools or editor runtime are required to launch the game.

## Build artifact and launch

- [ ] Record the exported `TitanCraft.exe` path.
- [ ] Record the build artifact hash before launch.
- [ ] Launch `TitanCraft.exe` directly from File Explorer or PowerShell.
- [ ] Confirm the game reaches the main menu without the Godot editor.
- [ ] Confirm no account login, API key, installer dependency, online launcher, or service prompt appears.

## Offline mode and network independence

- [ ] Disconnect the Windows machine or VM from the Internet before the first playtest launch, or otherwise block network access and record the method used.
- [ ] Confirm the executable launches while offline.
- [ ] Confirm the main menu is usable while offline.
- [ ] Confirm New Game is usable while offline.
- [ ] Confirm Continue is usable while offline when a save exists.
- [ ] Confirm no network error, telemetry prompt, account prompt, or remote dependency blocks the player.

## Full Crash Site victory loop

Run the complete MVP route in one playtest session unless a save/continue milestone explicitly interrupts the run.

- [ ] Spawn near the crashed ship.
- [ ] Confirm the current objective is understandable from the HUD or prompts.
- [ ] Collect the required metal resources.
- [ ] Collect the required biomass resources.
- [ ] Collect the required electronic components.
- [ ] Confirm resource counters update clearly after collection.
- [ ] Return to the workbench or crafting point.
- [ ] Craft the Mechanical Arm Mk I.
- [ ] Confirm resources are consumed as expected.
- [ ] Confirm the HUD or prompt clearly shows the Mechanical Arm Mk I state.
- [ ] Locate the Galaxabrain Scout encounter.
- [ ] Fight the Galaxabrain Scout using the crafted arm.
- [ ] Confirm the player can understand incoming damage and enemy state.
- [ ] Defeat the Galaxabrain Scout.
- [ ] Retrieve the required mission component.
- [ ] Use the save point after retrieving or before beacon activation, according to the current route.
- [ ] Activate the beacon.
- [ ] Confirm the victory screen or victory state appears.
- [ ] Record total playtime for the completed loop.

## Defeat path

- [ ] Start or continue a run from a valid checkpoint.
- [ ] Intentionally allow the player to lose all health.
- [ ] Confirm the defeat screen or death feedback appears.
- [ ] Confirm the game reloads or returns the player to the last save point/checkpoint.
- [ ] Confirm progression is not permanently blocked after defeat.
- [ ] Record whether inventory, arm state, enemy state, component state, and objective state are reasonable after respawn.

## Save, continue, quit, and relaunch path

- [ ] Use the save point and record the objective state at the time of save.
- [ ] Quit to the main menu or desktop through the intended UI.
- [ ] Relaunch `TitanCraft.exe` while still offline.
- [ ] Select Continue when a save exists.
- [ ] Confirm the run resumes from the expected checkpoint or saved progression state.
- [ ] Confirm the player can still complete the remaining Crash Site objective path after continuing.
- [ ] Quit from gameplay or menu.
- [ ] Confirm the process exits cleanly without hanging in Task Manager.

## Feel, readability, and comfort notes

Record observations even when the milestone passes.

- [ ] Mouse look feels controllable and does not feel unexpectedly inverted, delayed, or overly sensitive.
- [ ] Keyboard movement feels responsive for forward, backward, strafe, jump, interact, attack, and pause/menu inputs.
- [ ] FPS comfort is acceptable for the tester, with average and worst observed FPS recorded when available.
- [ ] Camera motion, combat motion, and enemy movement do not cause avoidable discomfort for the tester.
- [ ] HUD resource counters, health, objective text, interaction prompt, arm state, victory, and defeat messages are readable at the tested resolution.
- [ ] UI prompts clearly identify collection, crafting, saving, component pickup, beacon activation, menu, and quit actions.

## Stability, audio, and clean quit

- [ ] Record every crash, freeze, severe hitch, soft lock, or unresponsive input incident.
- [ ] Record audio issues, including missing critical cues, repeated loops, clipping, harsh volume changes, or audio continuing after quit.
- [ ] Confirm pause/menu interactions do not break mouse capture or keyboard control after resume.
- [ ] Confirm clean quit closes the game window and process.
- [ ] Confirm no save corruption or obvious local file error appears after quitting and relaunching.

## Completion criteria

The checklist is complete only when the evidence template records pass/fail for every milestone above, all blocking bugs are listed, and the final verdict is one of:

- `WINDOWS_MVP_PLAYTEST_CHECKLIST_READY`
- `WINDOWS_MVP_PLAYTEST_CHECKLIST_FAILED`
