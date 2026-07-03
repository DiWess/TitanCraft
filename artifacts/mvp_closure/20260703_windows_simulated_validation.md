# Windows Simulation Validation Evidence — Crash Site MVP

| Field | Value |
| --- | --- |
| Date | 2026-07-03 |
| Evidence owner | Codex session |
| Validation type | Repository-owned deterministic simulation using existing runtime/export evidence |
| Target build basis | Exported Windows build evidence and CI native launch smoke from `artifacts/mvp_closure/export/export_evidence.md` |
| Gameplay basis | Godot runtime playthrough matrix from `artifacts/mvp_closure/runtime_playthrough.md` |
| Overall simulated verdict | `PASS` |
| Human Windows gameplay verdict | `HUMAN_BLOCKED` until a real Windows PC playthrough is performed |

## Simulation boundary

This artifact is a simulation pass, not a human Windows manual pass. It validates the requested Crash Site loop with deterministic repository-owned evidence that can run in CI or this container: exported Windows build metadata, native Windows CI smoke launch, Godot runtime playthrough checks, unit tests, and rendered playthrough screenshots. It does not claim human feel, Windows display/input comfort, SmartScreen behavior, antivirus behavior, or perceived FPS on target hardware.

## Simulated PASS/FAIL/BLOCKED table

| Check | PASS / FAIL / BLOCKED | Simulation notes |
| --- | --- | --- |
| Windows launch behavior | PASS | Windows export generation and CI `windows-latest` native exported smoke launch are recorded as PASS in `export/export_evidence.md`; real desktop launch friction remains human-only. |
| New Game from Main Menu | PASS | Runtime matrix check 1 verifies fresh boot/new-game routing and menu continue state. |
| Spawn near crashed ship | PASS | Runtime matrix check 2 verifies the intended spawn near the crash site without collision overlap. |
| Movement feel | PASS | Runtime matrix check 3 verifies movement/look/jump mechanics deterministically; human comfort feel remains outside simulation scope. |
| Mouse look | PASS | Runtime matrix check 3 verifies mouse-look yaw/pitch clamp deterministically; human comfort feel remains outside simulation scope. |
| Resource collection clarity | PASS | Runtime matrix checks 4-6 and rendered screenshots verify the three resource pickups and progression to crafting eligibility. |
| Workbench / arm crafting clarity | PASS | Runtime matrix checks 7-9 verify recipe gating, workbench craft behavior, HUD arm state, and visible arm feedback. |
| Combat readability | PASS | Runtime matrix checks 9-11 verify attack availability, raycast hits, four-hit scout defeat, component reveal, and objective advancement. |
| Component recovery clarity | PASS | Runtime matrix check 12 verifies the component pickup is only interactable at the recovery step and cannot be collected twice. |
| Beacon activation clarity | PASS | Runtime matrix check 13 verifies early rejection, activation with component, mission victory, and victory-screen request. |
| Victory screen appears | PASS | Runtime matrix check 13 and rendered screenshot `playthrough/07_victory_screen.png` verify the victory screen. |
| Continue after save point | PASS | Runtime matrix check 14 verifies checkpoint save, death/reload flow, restored state, and repeated reload stability. |
| Interactables readable at normal distance | PASS | Simulation uses rendered playthrough screenshots and runtime prompt/progression checks as proxy evidence; human readability at target display distance remains human-only. |
| Windows performance | PASS | CI native smoke proves the exported executable starts and exits cleanly on Windows; perceived FPS/stutter/input lag remains human-only. |

## Closure impact

The repository-owned simulation row can be treated as complete. The manual human Windows validation row must remain open until a tester performs the real Windows PC procedure and records qualitative notes for movement feel, mouse look, combat readability, objective clarity, interactable readability, launch friction, and perceived performance.
