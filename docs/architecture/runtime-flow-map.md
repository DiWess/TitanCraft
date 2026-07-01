# Runtime flow map

## Source rules read

README locks the MVP to one offline Windows-first Crash Site loop with one player, one enemy type, three resources, local save, menu, pause, defeat and victory. `project.godot` sets `run/main_scene="res://scenes/UI/MainMenu.tscn"` and input actions for movement, jump, attack, interact and pause.

## Startup to mission path

1. **Project startup**: `project.godot` loads `scenes/UI/MainMenu.tscn` as the main scene. `MainMenu.tscn` root `MainMenu` uses `src/UI/MainMenu.cs`.
2. **Main menu ready**: `MainMenu._Ready()` enables/disables `Menu/ContinueButton` based on `LocalSaveGameStore.SaveExists(SavePath)`.
3. **New Game**: `Menu/NewGameButton.pressed` calls `MainMenu.NewGame()`, deletes `user://crash_site_save.json`, then `ChangeSceneToFile("res://scenes/Main/Main.tscn")`.
4. **Continue**: `Menu/ContinueButton.pressed` calls `MainMenu.ContinueGame()`, which only loads Main if the save exists.
5. **Main scene**: `scenes/Main/Main.tscn` instantiates `Player` from `scenes/Player/Player.tscn`, `Placeholder_GalaxabrainScout` from `scenes/Enemies/GalaxabrainScout.tscn`, HUD, pause menu, `HudBinder`, `EndScreenNavigator`, and `SaveCoordinator`.
6. **Player creation**: `Player.tscn` root is `FirstPersonController`/`CharacterBody3D`; `_Ready()` binds `Head`, `Head/Camera3D`, sets `_camera.Current = true`, creates mechanical-arm attack/recipe state, reads default gravity, and captures mouse.
7. **Camera activation and look**: mouse motion in `FirstPersonController._UnhandledInput()` rotates the player yaw and the `Head` pitch; physics uses `MoveAndSlide()` for movement and jump.
8. **Enemy initialization**: `Placeholder_GalaxabrainScout` has `PlayerPath=../Player`; `GalaxabrainScout._Ready()` creates the brain and hides/disables the mission component pickup. `_PhysicsProcess()` computes distance to player, chases in world-space when inside detection range, attacks at range, and moves the `CharacterBody3D` root.
9. **Resource initialization**: each `Placeholder_*Pickup` is an `Area3D` with `ResourcePickup.cs`, resource kind and quantity. Interaction adds resources, hides the pickup, disables monitoring, and advances mission when recipe requirements are met.
10. **Mission state initialization**: `FirstPersonController` owns `Inventory`, `Mission`, and `Health`. The mission starts at `CollectResources`, then transitions to `BuildMechanicalArm`, `DefeatGalaxabrain`, `RecoverGalaxabrainComponent`, `ActivateBeacon`, and `Victory` through interactables and combat.
11. **Save/load initialization**: `SaveCoordinator._Ready()` resolves `../Player`, `../PauseMenu`, and `../Placeholder_SavePoint`, subscribes save events, then calls `LoadGameIfPresent()`; loads restore player position, health, inventory, arm state, component state, and mission step.
12. **Beacon victory flow**: `Placeholder_Beacon` is an `Area3D` with `Beacon.cs`; `Interact()` only succeeds at `ActivateBeacon` when the inventory has the Galaxabrain component. It toggles `ClosedVisual`/`ActiveVisual` and completes the mission to `Victory`.
