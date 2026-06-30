# Visual Vertical Slice Phase 0 Baseline

Date: 2026-06-30
Scope: documentation-only baseline for the Crash Site visual vertical slice.

## Phase 0 status

Phase 0 establishes the current state before any final visual work. It does not authorize Phase 1 implementation by itself and does not change gameplay, scenes, collisions, materials, tests, or object positions.

## Commands executed

| Command | Result | Notes |
|---|---|---|
| `dotnet restore` | PASS | Restored `TitanCraft.csproj` and `tests/TitanCraft.Tests.csproj`. |
| `dotnet build` | PASS | Debug build succeeded with 0 warnings and 0 errors. |
| `dotnet test --settings tests/TitanCraft.runsettings` | PASS | 43 tests passed, 0 failed, 0 skipped, total duration 696 ms. |
| `godot --headless --path . --import` | PASS | Godot 4.7 stable mono import completed with exit code 0. |
| `./tools/test.sh` | PASS_WITH_KNOWN_WARNING | Debug and Release builds passed, unit tests passed, integration runner printed `TITANCRAFT_INTEGRATION_TESTS_PASS`, import/export checks completed. One expected warning was emitted by the integration test for intentionally unreadable save data. |
| `git status --short` | PASS | Clean before documentation changes. |

## Environment limitations

- No Windows executable was manually launched in this Linux container, so Windows offline readiness is not validated here.
- No human gameplay-feel pass was performed, so FPS feel, mouse comfort, objective comprehension by a new player, and fun factor remain unvalidated.
- No in-editor graphics profiler capture was taken. Performance baseline is limited to project settings, scene contents, and successful headless/import/export checks.

## Gameplay-critical node paths

These paths are treated as stable contracts until a dedicated migration updates both scenes and tests.

### Main scene contracts

| Path | Type | Contract |
|---|---|---|
| `WorldEnvironment` | `WorldEnvironment` | Single active world environment for Crash Site. |
| `DirectionalLight3D` | `DirectionalLight3D` | Main directional light; integration tests require it to exist. |
| `Ground` | `StaticBody3D` | Primary floor body. |
| `Ground/MeshInstance3D` | `MeshInstance3D` | Ground visual mesh. |
| `Ground/Collision_Ground` | `CollisionShape3D` | Primary floor collision; currently a `BoxShape3D`. |
| `C7_Wall_1/Collision_C7Wall` | `CollisionShape3D` | Critical base wall collision; currently a `BoxShape3D`. |
| `C7_Wall_2/Collision_C7Wall` | `CollisionShape3D` | Critical base wall collision; currently a `BoxShape3D`. |
| `C7_Wall_3/Collision_C7Wall` | `CollisionShape3D` | Critical base wall collision; currently a `BoxShape3D`. |
| `C7_Wall_4/Collision_C7Wall` | `CollisionShape3D` | Critical base wall collision; currently a `BoxShape3D`. |
| `Placeholder_MetalPickup` | `Area3D` | Metal resource interactable, `ResourceKind = 0`, `Quantity = 10`. |
| `Placeholder_MetalPickup/Collision_MetalPickup` | `CollisionShape3D` | Metal pickup interaction/collision volume. |
| `Placeholder_BiomassPickup` | `Area3D` | Biomass interactable, `ResourceKind = 1`, `Quantity = 3`. |
| `Placeholder_BiomassPickup/Collision_BiomassPickup` | `CollisionShape3D` | Biomass pickup interaction/collision volume. |
| `Placeholder_ElectronicsPickup` | `Area3D` | Electronics interactable, `ResourceKind = 2`, `Quantity = 2`. |
| `Placeholder_ElectronicsPickup/Collision_ElectronicsPickup` | `CollisionShape3D` | Electronics pickup interaction/collision volume. |
| `Placeholder_Workbench` | `Area3D` | Crafting interactable for Mechanical Arm Mk I. |
| `Placeholder_Workbench/Collision_Workbench` | `CollisionShape3D` | Workbench interaction/collision volume; integration tests require a `BoxShape3D`. |
| `Placeholder_SavePoint` | `Area3D` | Save point interactable used by `CrashSiteSaveCoordinator`. |
| `Placeholder_SavePoint/Collision_SavePoint` | `CollisionShape3D` | Save point interaction/collision volume. |
| `Placeholder_Beacon` | `Area3D` | Final mission beacon interactable. |
| `Placeholder_Beacon/ClosedVisual` | `MeshInstance3D` | Beacon closed visual path used by `Beacon`. |
| `Placeholder_Beacon/ActiveVisual` | `MeshInstance3D` | Beacon active visual path used by `Beacon`; starts hidden in tests. |
| `Placeholder_Beacon/Collision_BeaconBase` | `CollisionShape3D` | Beacon interaction/collision volume; integration tests require a `BoxShape3D`. |
| `AlienCrystal_1` | `MeshInstance3D` | Route marker checked by integration tests. |
| `Moon` | `MeshInstance3D` | Distant moon/landmark checked by integration tests. |
| `Player` | `CharacterBody3D` instance | Runtime player contract for HUD, save, enemy, and navigation scripts. |
| `HUD` | `CanvasLayer` instance | HUD contract for `CrashSiteHudBinder`. |
| `PauseMenu` | `CanvasLayer` instance | Pause/save contract for `CrashSiteSaveCoordinator`. |
| `HudBinder` | `Node` | Binds player health, inventory, mission, and prompt events to HUD. |
| `EndScreenNavigator` | `Node` | Requests victory or defeat scenes. |
| `SaveCoordinator` | `Node` | Loads and saves local Crash Site state. |
| `Placeholder_GalaxabrainScout` | `GalaxabrainScout` instance | The single MVP enemy instance; `PlayerPath = ../Player`. |

### Player scene contracts

| Path | Type | Contract |
|---|---|---|
| `Player` | `CharacterBody3D` | Root with `FirstPersonController`. |
| `CollisionShape3D` | `CollisionShape3D` | Player capsule collision. |
| `Head` | `Node3D` | Camera pitch pivot. |
| `Head/Camera3D` | `Camera3D` | Current FPS camera, FOV 75. |

### Enemy scene contracts

| Path | Type | Contract |
|---|---|---|
| `GalaxabrainScout` | `CharacterBody3D` | Root with `GalaxabrainScout` script. |
| `CollisionShape3D` | `CollisionShape3D` | Enemy capsule collision. |
| `Placeholder_GalaxabrainScout` | `MeshInstance3D` | Current enemy placeholder visual. |
| `GalaxabrainComponentPickup` | `Area3D` | Mission component, hidden and not monitoring before enemy death. |
| `GalaxabrainComponentPickup/CollisionShape3D` | `CollisionShape3D` | Component pickup interaction volume. |

### UI scene contracts

| Path | Type | Contract |
|---|---|---|
| `HUD/Panel/Margin/VBox/Health` | `Label` | Health display updated by `CrashSiteHud`. |
| `HUD/Panel/Margin/VBox/Objective` | `Label` | Mission objective display. |
| `HUD/Panel/Margin/VBox/Resources` | `Label` | Metal, Biomass, Electronics counters. |
| `HUD/Panel/Margin/VBox/StartTutorial` | `Label` | Initial short control tutorial. |
| `HUD/Panel/Margin/VBox/InteractionPrompt` | `Label` | Contextual `E: ...` prompt. |
| `HUD/Panel/Margin/VBox/MechanicalArmState` | `Label` | Mechanical arm online/offline state. |
| `MainMenu/Menu/NewGameButton` | `Button` | Starts a new Crash Site run and deletes existing save. |
| `MainMenu/Menu/ContinueButton` | `Button` | Enabled only when local save exists. |
| `MainMenu/Menu/QuitButton` | `Button` | Quits game. |
| `PauseMenu/Panel/Menu/ResumeButton` | `Button` | Resumes game. |
| `PauseMenu/Panel/Menu/SaveButton` | `Button` | Emits save request. |
| `PauseMenu/Panel/Menu/MainMenuButton` | `Button` | Returns to main menu. |
| `PauseMenu/Panel/Menu/QuitButton` | `Button` | Quits game. |
| `VictoryScreen/Menu/MainMenuButton` | `Button` | Returns to main menu. |
| `DefeatScreen/Menu/ReloadButton` | `Button` | Reloads last save by reloading `Main.tscn`. |
| `DefeatScreen/Menu/MainMenuButton` | `Button` | Returns to main menu. |

## Scene and script contracts that must remain stable

### Scenes

- `project.godot` must keep `run/main_scene="res://scenes/UI/MainMenu.tscn"` unless a dedicated menu migration is reviewed.
- `scenes/Main/Main.tscn` remains the single playable MVP map.
- `scenes/Player/Player.tscn` remains a first-person-only player scene.
- `scenes/Enemies/GalaxabrainScout.tscn` remains the only MVP enemy scene.
- `scenes/UI/HUD.tscn`, `PauseMenu.tscn`, `MainMenu.tscn`, `VictoryScreen.tscn`, and `DefeatScreen.tscn` must preserve their button and label contracts.

### Gameplay scripts

- `FirstPersonController` owns FPS movement, mouse look, attack raycast, interaction raycast, inventory, health, and mission state.
- `MechanicalArmAttackLogic` owns arm damage and cooldown rules; visuals must not decide damage.
- `GalaxabrainScoutBrain` owns enemy states `Idle`, `Chase`, `Attack`, `Dead`.
- `GalaxabrainScout` owns enemy movement, attack, damage, death, and mission component reveal.
- `CrashSiteMissionState` owns the mission sequence and `Changed` event.
- `MvpInventory` owns the three resources, arm-built flag, and Galaxabrain component flag.
- `MechanicalArmRecipe` loads `data/Recipes/mechanical_arm_mk1.json` and must remain the single source of the crafting cost.
- `ResourcePickup`, `Workbench`, `SavePoint`, `Beacon`, and `GalaxabrainComponentPickup` implement the interaction contracts.
- `CrashSiteSaveCoordinator`, `LocalSaveGameStore`, `CrashSiteSaveService`, and `CrashSiteSaveData` own local save/load behavior and save compatibility.
- `CrashSiteHud`, `CrashSiteHudBinder`, `MainMenu`, `PauseMenu`, `EndScreen`, and `CrashSiteEndScreenNavigator` own current UI flow.

## MVP gameplay contracts

- The MVP remains single-player, offline-first, Windows-first, and Crash Site only.
- No multiplayer, cloud service, accounts, voxel terrain, procedural infinite world, new map, new enemy type, grappling hook, wall run, vehicle, grand mech, or full rocket may be added during visual work.
- Mission order remains: collect resources, build Mechanical Arm Mk I, defeat Galaxabrain, recover component, activate beacon, victory.
- The recipe remains Metal 10, Biomass 3, Electronic Components 2 unless README and tests are explicitly updated by human-approved scope.
- Player health remains 100 baseline; defeat occurs at zero health.
- Galaxabrain Scout health remains 100, attack damage 10, attack range 2.0, detection range 12.0, cooldown 0.8, chase speed 3.0 unless a separate gameplay tuning task is approved.
- Mechanical arm attack remains 25 damage with 0.8 second cooldown and requires the arm to be built for full damage.
- Interaction range remains 3.0 unless a dedicated interaction task proves otherwise.
- Save path remains `user://crash_site_save.json` and save version remains governed by `CrashSiteSaveData`.

## Baseline acceptance criteria

| Criterion | Status | Evidence |
|---|---|---|
| Build and applicable tests documented | PASS | Command table above. |
| Critical node paths listed | PASS | Node path tables above. |
| Placeholder inventory documented | PASS | See `docs/visual-placeholder-inventory.md`. |
| Rendering/performance baseline documented | PASS_WITH_LIMITATIONS | See `docs/performance-baseline.md`; no live Windows/GPU profiler capture in container. |
| No gameplay scripts changed | PASS | Documentation-only Phase 0. |
| No scene visuals, collisions, materials, tests, object positions, or save data changed | PASS | Documentation-only Phase 0. |

## Phase 1 gate

Phase 1 is authorized only after human review of this baseline confirms that the documentation is sufficient. This Phase 0 output does not start Phase 1 and does not introduce final visual changes.
