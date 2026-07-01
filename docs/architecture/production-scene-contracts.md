# Production scene contracts

| Scene | Node path | Type | Script | Runtime purpose | Owns collision | Owns gameplay | Owns visuals | Transform space | Safe visual child location | Forbidden changes |
|---|---|---|---|---|---|---|---|---|---|---|
| Main | `Main` | Node3D | none | World root | No | Owns composition | No | World | Top-level world-only decorative nodes | Do not parent world objects under player/camera/HUD |
| Main | `Ground` | StaticBody3D | none | Play floor | Yes | Yes | Has hidden placeholder visual | World | `Ground/MeshInstance3D` only | Do not remove `Collision_Ground` |
| Main | `Player` | CharacterBody3D instance | FirstPersonController | Player pawn | Yes | Yes | Camera-relative arm only | World root with camera child | `Head/Camera3D/MechanicalArmVisual` | Do not move enemy/world visuals under Player/Head/Camera |
| Main | `Placeholder_GalaxabrainScout` | CharacterBody3D instance | GalaxabrainScout | Only enemy | Yes | Yes | Yes | World | direct local visual child of enemy root | Do not change PlayerPath, collision, or root position by visual guesses |
| Main | `Placeholder_MetalPickup`/`Biomass`/`Electronics` | Area3D | ResourcePickup | Collectables | Yes | Yes | Yes | World | local child mesh below pickup root | Do not remove script/collision or make camera-relative |
| Main | `Placeholder_Workbench` | Area3D | Workbench | Craft arm | Yes | Yes | Yes | World | child meshes that do not affect collision | Do not change script or collision path |
| Main | `Placeholder_SavePoint` | Area3D | SavePoint | Local checkpoint | Yes | Yes | Yes | World | child meshes that do not affect collision | Do not break SaveCoordinator path |
| Main | `Placeholder_Beacon` | Area3D | Beacon | Victory interactable | Yes | Yes | State visuals | World | `ClosedVisual`, `ActiveVisual`, optional visual children | Do not rename ClosedVisual/ActiveVisual without export update |
| Player | `Head` | Node3D | none | Pitch pivot | No | Camera transform | No | Player-local rotating | only camera | Do not attach world models |
| Player | `Head/Camera3D` | Camera3D | none | Active FPS camera | No | View/raycast origin | No, except viewmodel | Camera-local | `MechanicalArmVisual` or future `FirstPersonVisualRoot` | Do not attach full mech/world assets |
| Player | `Head/Camera3D/MechanicalArmVisual` | MeshInstance3D | none | First-person view model | No | No | Yes | Camera-local | itself/future viewmodel children | No collision, no world layer role, no full mech body |
| HUD | `HUD` | CanvasLayer | CrashSiteHud | UI overlay | No | Displays state | UI only | Canvas | labels under HUD | Do not parent world objects |
