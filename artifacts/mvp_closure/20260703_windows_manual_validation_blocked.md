# Windows Manual Validation Evidence — Crash Site MVP

| Field | Value |
| --- | --- |
| Date | 2026-07-03 |
| Evidence owner | Codex session |
| Requested target | Real Windows PC, exported TitanCraft Crash Site build from current target commit |
| Target commit from request note | `6bbe9388ba6280253581aaf21b60f5011ba5db89` |
| Actual execution environment | Linux container (`/workspace/TitanCraft`) |
| Overall verdict | `BLOCKED` |

## Scope requested

The requested procedure in `artifacts/mvp_closure/20260703_windows_manual_request_note.md` requires a native Windows PC playthrough of the exported build:

1. Launch `TitanCraft.exe`.
2. Start New Game.
3. Complete collect → craft → defeat → recover → activate beacon → victory.
4. Save at `Placeholder_SavePoint`.
5. Verify Continue reload behavior.
6. Record notes for movement feel, mouse look, combat readability, objective clarity, interactable readability, launch behavior, and perceived performance.

## Result

`BLOCKED`: this Codex execution ran inside a Linux container, not on a real Windows PC. I cannot truthfully launch `TitanCraft.exe` on target Windows hardware, perform a human feel/readability playthrough, or observe Windows-specific performance/SmartScreen/antivirus behavior from this environment.

No PASS is claimed for native Windows manual gameplay validation in this artifact.

## Completed manual validation table

| Check | PASS / FAIL / BLOCKED | Notes |
| --- | --- | --- |
| Windows launch behavior | BLOCKED | Requires launching `TitanCraft.exe` on a real Windows PC; this session only has a Linux container. |
| New Game from Main Menu | BLOCKED | Requires interactive Windows execution of the exported build. |
| Spawn near crashed ship | BLOCKED | Requires visual/manual confirmation in the Windows build. |
| Movement feel | BLOCKED | Human feel assessment cannot be performed headlessly or from Linux-only execution. |
| Mouse look | BLOCKED | Human mouse-look comfort and responsiveness require a real Windows play session. |
| Resource collection clarity | BLOCKED | Requires observing prompts and object readability at normal play distance in the Windows build. |
| Workbench / arm crafting clarity | BLOCKED | Requires interactive crafting at `Placeholder_Workbench` in the Windows build. |
| Combat readability | BLOCKED | Requires human assessment of attack range, hit feedback, enemy state, and damage readability. |
| Component recovery clarity | BLOCKED | Requires completing the enemy defeat/recovery step in the Windows build. |
| Beacon activation clarity | BLOCKED | Requires activating `Placeholder_Beacon` interactively on Windows. |
| Victory screen appears | BLOCKED | Requires completing the native Windows playthrough to `VictoryScreen.tscn`. |
| Continue after save point | BLOCKED | Requires saving at `Placeholder_SavePoint`, returning to menu/relaunching, and using Continue on Windows. |
| Interactables readable at normal distance | BLOCKED | Requires real-time visual inspection on target hardware/display. |
| Windows performance | BLOCKED | Perceived FPS/stutter/input lag and Windows-specific launch friction cannot be measured in this container. |

## Closure impact

The manual Windows validation row must remain open in `artifacts/mvp_closure/closeout_index.md` and `docs/mvp_review.md` because the requested target-hardware procedure did not pass. Existing automated/runtime and CI launch evidence still stand, but they do not replace this human Windows gameplay validation.

## Manual test still required

Run the procedure from `artifacts/mvp_closure/20260703_windows_manual_request_note.md` on a real Windows PC using the exported build from the target commit. If every row passes, add a new PASS evidence artifact and then update `artifacts/mvp_closure/closeout_index.md` plus `docs/mvp_review.md` to move the manual validation item out of the open list.
