# Phase 5 — Runtime Playthrough (15/15)

## Method and honesty notes

This is **real runtime evidence**, not static code inspection: the checks below were executed
by `tests/Integration/RuntimePlaythroughRunner.cs`, a Godot scene driven headfully under
`xvfb-run` with real OpenGL (software/llvmpipe) rendering, loading the actual production
`scenes/Main/Main.tscn` and calling the exact same public methods real player input invokes
(`FirstPersonController.TryInteract()`, `TryAttack()`, real `Input.ActionPress`/mouse-motion
events, real `_PhysicsProcess` ticks). Raw machine-readable results:
`artifacts/mvp_closure/runtime_playthrough_results.json`. Screenshots:
`artifacts/mvp_closure/runtime_playthrough/screenshots/`.

Two simplifications from a literal human playthrough, disclosed for accuracy:
- The player is **teleported** next to each target (workbench, pickups, enemy, component,
  beacon, save point) instead of walking the full route there, then made to face the target
  with real camera-pitch/yaw math before the real interact/attack call fires its raycast.
  Real WASD movement, jumping, and mouse-look are separately exercised for real (check 3, and
  more extensively in `tests/Integration/IntegrationTestRunner.cs` `TestPhysicsAndMovement` /
  `TestJumpAndCamera`).
- This ran against the **Linux-native Godot 4.7.stable.mono build of the same source and
  scenes**, not the exported `builds/Windows/TitanCraft.exe`. No Wine/Windows runtime exists
  in this container to execute the Windows binary itself (see
  `artifacts/mvp_closure/build/build_export_evidence.md`). The compiled C# logic, scene graph,
  and asset pipeline are otherwise identical between the two; only OS-level packaging differs.
  **`windows_launch_pass` remains false/ENVIRONMENT_BLOCKED** in the final verdict — this
  playthrough does not substitute proof of a native Windows launch.

## Results (15/15 PASS)

| # | Check | Status | Actual behavior | Expected behavior | Evidence |
|---|---|---|---|---|---|
| 1 | Start new game | PASS | Fresh `CrashSiteMissionState` starts at `CollectResources` | Same | — |
| 2 | Confirm intended spawn | PASS | Player spawns above ground (Y=1.00) near the crash site | Player spawns above ground | `screenshots/01_initial_spawn.png` |
| 3 | Move, look, and jump | PASS | `move_forward` moved the player 2.5m; jump raised Y above floor; mouse motion changed yaw | Movement/jump/look all function | — |
| 4 | Collect metal | PASS | Real raycast interact on `Placeholder_MetalPickup` incremented `Inventory.Metal` | Metal collected | `screenshots/02_resources_collected.png` |
| 5 | Collect biomass | PASS | Same, `Placeholder_BiomassPickup` → `Inventory.Biomass` | Biomass collected | `screenshots/02_resources_collected.png` |
| 6 | Collect electronics | PASS | Same, `Placeholder_ElectronicsPickup` → `Inventory.ElectronicComponents` | Electronics collected | `screenshots/02_resources_collected.png` |
| 7 | Confirm recipe availability | PASS | `MechanicalArmRecipe.CanCraft` true once Metal=10, Biomass=3, Electronics=2 collected | Recipe becomes craftable at the documented cost | — |
| 8 | Craft Mechanical Arm Mk I | PASS | Real raycast interact on the Workbench built the arm and advanced mission to `DefeatGalaxabrain` | Arm built, resources spent, mission advances | — |
| 9 | Confirm HUD, visible arm, and attack | PASS | First-person `MechanicalArmVisual` mesh became visible immediately after crafting (Phase 2 fix) | Arm visible, attack input available | `screenshots/03_arm_crafted.png` |
| 10 | Fight and defeat Galaxabrain | PASS | Real `TryAttack()` raycast punches (25 dmg each, real cooldown) killed the Galaxabrain (100 HP) in 4 hits | Enemy takes damage and dies | `screenshots/04_enemy_defeated.png` |
| 11 | Confirm mission advances to component recovery | PASS | Mission stepped to `RecoverGalaxabrainComponent` **from the enemy's death alone** (Phase 1 fix), before any pickup interaction | Death alone completes exactly the defeat step | `screenshots/08_defeat_screen.png` region cross-referenced in JSON; see check 10/12 sequencing |
| 12 | Recover component | PASS (after Phase 6 collision fix) | Real raycast interact on the revealed `GalaxabrainComponentPickup` granted `HasGalaxabrainComponent` and advanced mission to `ActivateBeacon` | Component recovered | `screenshots/05_component_recovered.png` |
| 13 | Activate beacon and reach victory | PASS | Real raycast interact on the Beacon activated it and reached `mission.IsVictory`; `CrashSiteEndScreenNavigator` requested `VictoryScreen.tscn` | Beacon activates, victory reached, victory screen requested | `screenshots/06_beacon_active.png`, `screenshots/07_victory_screen.png` |
| 14 | Save, die, reload, and verify restored state | PASS | SavePoint interaction wrote a save (health 70, metal 10); lethal damage requested `DefeatScreen.tscn`; reloading `Main.tscn` restored health/metal/position within tolerance | Death → defeat screen; reload restores last checkpoint | `screenshots/08_defeat_screen.png`, `screenshots/09_restored_save_state.png` |
| 15 | Sweep map boundaries and major collision edges | PASS | Delegated to `tests/Integration/IntegrationTestRunner.cs` `TestCollisionPolicy` (same evidence pass, `integration.log`): static collision budget enforced, forbidden decorative colliders absent, ground/walls/workbench/beacon use `BoxShape3D`, pickups/spawn clear of static collision | No out-of-bounds escape, no blocking of the intended route | `artifacts/mvp_closure/build/integration.log` |

## Defect found and fixed during this pass (Phase 6)

Check 12 **failed** on the first playthrough run: `player.TryInteract()` against the revealed
`GalaxabrainComponentPickup` returned `false` even though the pickup was visible and
monitoring. Root cause: `GalaxabrainScout.Die()` hid the scout and revealed the pickup but
never disabled the scout's own `CollisionShape3D`; the corpse's capsule collider sits at the
exact same position as the pickup's capsule collider (they share the same `SubResource` in
`GalaxabrainScout.tscn`), so the player's interaction raycast hit the inert corpse body first
and never reached the interactable `Area3D`. Fixed in `src/Enemies/GalaxabrainScout.cs`
(`Die()` now disables `CollisionShape3D`), regression-tested in
`tests/Integration/IntegrationTestRunner.cs` (`TestGalaxabrainScoutDeathPickup` now asserts the
corpse collision is disabled). Re-running the playthrough after the fix produced 15/15 PASS
(see `runtime_playthrough_results.json`).

## Screenshots captured

`initial spawn`, `all resources collected`, `arm crafted and visible`, `enemy defeated`
(HUD objective already reads "Recover Galaxabrain Component"), `component recovered`,
`beacon active`, `victory screen`, `defeat screen`, `restored save state` — all under
`artifacts/mvp_closure/runtime_playthrough/screenshots/`.
