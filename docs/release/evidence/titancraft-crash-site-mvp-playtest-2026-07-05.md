# TitanCraft Crash Site MVP Playtest Evidence — 2026-07-05

## Scope

- Task: Close Crash Site MVP gameplay blockers without adding features, enemies, mission redesign, art direction changes, map expansion, or unrelated system edits.
- Evidence category: gameplay.
- README scope check: Crash Site MVP allows gameplay bug fixes for the single-player loop that collects resources, crafts Mechanical Arm Mk I, defeats one Galaxabrain Scout, recovers the component, activates the beacon, and reaches victory.

## Build tested

- Commit SHA at evidence authoring: pending final commit for this PR; final committed SHA is reported in the PR/final response.
- Build tested: automated C# behavior tests, Godot headless Crash Site MVP smoke milestones, and Linux-triggered Windows export generation in the repository validation script; the exported Windows executable was not manually launched in this environment.
- OS/environment: Linux container, .NET 8 SDK, non-interactive headless environment.
- Start scene: `scenes/Main/Main.tscn` documented as the Crash Site scene path for validation.

## Gameplay loop evidence

| Required checkpoint | Evidence recorded | Result |
|---|---|---|
| Launch game | `./tools/test.sh` completed Godot headless validation and generated `builds/Windows/TitanCraft.exe`; Windows manual launch was not performed. | Partial |
| Start Crash Site scene | Godot smoke milestone log reported `MVP_SMOKE_MILESTONE_01: player spawned`. | Completed by automated smoke |
| Move, jump, look, interact, attack | Unit/integration coverage ran through `./tools/test.sh`; manual FPS feel was not replayed here. | Partial |
| Collect required resources | Godot smoke milestone log reported `MVP_SMOKE_MILESTONE_02: resources collected`. | Completed by automated smoke |
| Encounter Galaxabrain Scout | Godot smoke milestone log reported `MVP_SMOKE_MILESTONE_04: Galaxabrain Scout engaged`. | Completed by automated smoke |
| Defeat Scout | Godot smoke milestone log reported `MVP_SMOKE_MILESTONE_05: Galaxabrain Scout defeated`. | Completed by automated smoke |
| Unlock/collect component only after intended condition | New reconciliation tests cover inconsistent loaded saves not granting component before pickup. | Completed by automated regression |
| Use workbench | Godot smoke milestone log reported `MVP_SMOKE_MILESTONE_03: Mechanical Arm Mk I crafted`. | Completed by automated smoke |
| Craft mechanical arm | Existing recipe/inventory tests cover crafted state and event emission. | Completed by automated regression |
| See mechanical arm visual become visible/equipped | Smoke artifact import listed `03_arm_crafted_visible.png`; inventory event tests verify the visual-binding state changes after craft/restore. | Completed by automated/screenshot artifact |
| Activate beacon/final objective | Godot smoke milestone log reported `MVP_SMOKE_MILESTONE_08: beacon activated`. | Completed by automated smoke |
| Reach victory screen | Godot smoke milestone log reported `MVP_SMOKE_MILESTONE_09: victory reached`. | Completed by automated smoke |
| Save/load state | Godot smoke milestone log reported `MVP_SMOKE_MILESTONE_11: save continuation verified`; new reconciliation tests cover inconsistent loaded mission state. | Completed by automated smoke/regression |

## Manual playtest details

- Resources collected: Automated Godot smoke milestone verified resources collected.
- Scout defeated: Automated Godot smoke milestone verified Scout defeated.
- Component acquired: Automated Godot smoke milestone verified component retrieved.
- Arm crafted: Automated Godot smoke milestone verified Mechanical Arm Mk I crafted.
- Arm visible: Automated import/export output included the existing `artifacts/mvp_closure/playthrough/03_arm_crafted_visible.png` evidence asset, but no new interactive screenshot was captured in this container.
- Beacon/victory reached: Automated Godot smoke milestones verified beacon activated and victory reached.
- Save/load result: Automated Godot smoke milestone verified save continuation; regression tests verified load reconciliation.
- Screenshots: Existing imported smoke artifacts were packaged by export; no new screenshot was captured manually.
- Failures: Full exported Windows end-to-end manual playtest could not be completed here; final verdict must not be `CRASH_SITE_MVP_PLAYABLE_PASS`.

## Deterministic manual checklist for next Windows/export pass

1. Launch `builds/Windows/TitanCraft.exe` on Windows while offline.
2. Start a new Crash Site run from the main menu.
3. Confirm FPS controls: move, jump, mouse look, interact, attack block before craft.
4. Collect the exact recipe resources: 10 Metal, 3 Biomass, and 2 Electronics.
5. Use the workbench and confirm Mechanical Arm Mk I crafts once.
6. Confirm first-person mechanical arm visual appears only after craft.
7. Defeat the Galaxabrain Scout with the crafted arm.
8. Confirm the mission component becomes visible/interactive only after Scout defeat.
9. Pick up the component and confirm the objective advances to beacon activation.
10. Save, reload/continue, and confirm arm crafted/equipped state and component/Scout state remain consistent.
11. Activate the beacon and confirm the victory screen appears.
12. Record screenshots for arm visible, component available after defeat, beacon active, and victory.

## Final evidence verdict

CRASH_SITE_MVP_PARTIAL_PASS
