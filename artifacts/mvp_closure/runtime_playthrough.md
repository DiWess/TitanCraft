# Crash Site MVP — Runtime Playthrough Matrix

Tested commit SHA: `eab008ac1efc9931c5f6157d9d6165a4e6af1263`

Execution environment: Godot 4.7.stable.mono on Linux (headless for logic gates,
xvfb + llvmpipe OpenGL for rendered evidence). The exported **Windows** binary could
not be launched in this container (no Wine/Windows host): every check below was
executed against the identical project through the Godot runtime on Linux — the same
engine version and scene/assembly content packed into the export. Windows-native
execution is covered by the CI `windows` job's exported smoke and remains the human
follow-up for feel validation.

Evidence sources:
- `artifacts/mvp_closure/build/integration.log` — headless integration run (all gates, exit 0)
- `artifacts/mvp_closure/playthrough_capture.log` + `artifacts/mvp_closure/playthrough/*.png` — rendered driven playthrough
- `artifacts/mvp_closure/build/integration_prefix_boundary_failure.log` — pre-fix P0 boundary evidence

| # | Check | Result | Actual behavior (expected behavior met unless noted) | Evidence |
| --- | --- | --- | --- | --- |
| 1 | Start new game | PASS | Fresh run boots with no save; MainMenu Continue disabled without save, New Game targets Main.tscn (`TestMainMenuContinueState`); smoke boot exits clean. | integration.log, smoke.log |
| 2 | Confirm intended spawn | PASS | Player spawns above ground near crash site, no static-collision overlap, enemy >8m away (`TestMainScene`, `TestCollisionPolicy`, `TestRuntimeSceneContracts`). | integration.log, playthrough/01_initial_spawn.png |
| 3 | Move, look, jump | PASS | WASD/ZQSD cardinal + diagonal movement, mouse-look yaw/pitch clamp, single jump, no air double-jump (`TestPhysicsAndMovement`, `TestJumpAndCamera`). | integration.log |
| 4 | Collect metal | PASS | Pickup interaction adds 10 metal, hides pickup, not collectable twice (`TestFullMissionPlaythrough`). | integration.log, playthrough/02_resources_collected.png |
| 5 | Collect biomass | PASS | Adds 3 biomass; same idempotence guarantees. | integration.log |
| 6 | Collect electronics | PASS | Adds 2 electronics; third pickup advances objective to Build Mechanical Arm exactly when recipe cost is met. | integration.log |
| 7 | Confirm recipe availability | PASS | Workbench rejects crafting before resources; objective advances only when `MechanicalArmRecipe.CanCraft` is true; workbench prompt exposes cost. | integration.log, unit tests (MechanicalArmRecipeTests) |
| 8 | Craft Mechanical Arm Mk I | PASS | Workbench interaction spends resources, sets `IsMechanicalArmBuilt`, advances to Defeat Galaxabrain, rejects double-craft. | integration.log, playthrough/03_arm_crafted_visible.png |
| 9 | Confirm HUD, visible arm, attack | PASS | HUD arm state flips to "Online", stale startup hint replaced (fixed this pass), first-person arm mesh becomes visible, attack unblocked. | integration.log, playthrough/03_arm_crafted_visible.png |
| 10 | Fight and defeat Galaxabrain | PASS | First hit through the real `TryAttack` camera raycast damages the live scout; four hits kill it; corpse hides; component revealed. | integration.log |
| 11 | Mission advances to component recovery | PASS | **Fixed this pass**: scout death itself advances to Recover Galaxabrain Component (was previously stuck on "Defeat Galaxabrain" until pickup). | integration.log, playthrough/04_enemy_defeated_objective_updated.png |
| 12 | Recover component | PASS | Pickup interactable only at the recovery step, completes only recovery, not collectable twice. | integration.log, playthrough/05_component_recovered.png |
| 13 | Activate beacon and reach victory | PASS | Beacon rejects early activation, activates once with the component, produces Victory and the victory screen request. | integration.log, playthrough/06_beacon_active.png, playthrough/07_victory_screen.png |
| 14 | Save, die, reload, verify restored state | PASS | Checkpoint save at save point; death routes to defeat screen; reload restores position/health/inventory/mission and (fixed this pass) dead scout, component availability, and beacon state across three consecutive reloads. | integration.log (`TestSaveLoadFlow`, `TestDefeatedScoutPersistenceAcrossReload`, `TestEndScreenNavigation`), playthrough/08_restored_save_state.png |
| 15 | Sweep map boundaries and collision edges | PASS | Static collision policy verified (shape budget, no decorative collision, pickups/walls/ground collision present). **Fixed this pass**: leaving the 150×150 slab was an endless fall with no recovery (P0, pre-fix log preserved); falls below `FallDefeatHeight` now route into the standard death flow. | integration.log, build/integration_prefix_boundary_failure.log |

Score: **15 / 15 PASS** on the Linux Godot runtime.
Windows-native execution of these checks: **ENVIRONMENT_BLOCKED** (no Wine/Windows in container).

## Defect severity log from this phase

| Defect | Severity | Status |
| --- | --- | --- |
| Enemy death did not advance mission; pickup collapsed two steps | P0 | Fixed (`69fd5a1`), regression-tested |
| Enemy/component/beacon state not persisted across reload | P0 | Fixed (`65f3d6e`), regression-tested |
| Out-of-bounds fall was unrecoverable (no containment, no kill plane) | P0 | Fixed (`2e90c90`), pre-fix evidence preserved |
| Mechanical arm visual never shown after crafting/load | P1 | Fixed (`7dc1e02`), regression-tested |
| Stale "Mk I not built yet" HUD hint after crafting | P1 | Fixed (`5b80774`), regression-tested |
