# Camera and render contract

- Active camera path: `Main/Player/Head/Camera3D`.
- Parent chain: `Main` → `Player` (`CharacterBody3D`) → `Head` (`Node3D` pitch pivot) → `Camera3D`.
- Activation: `FirstPersonController._Ready()` sets `Head/Camera3D.Current = true`.
- Mouse look: `FirstPersonController._UnhandledInput()` applies yaw to `Player.Rotation` and pitch to `Head.Rotation`.
- Camera cull mask: default scene value unless explicitly changed later.
- World render layers: default `VisualInstance3D` layers for terrain, ship, pickups, workbench, save point, beacon, enemy.
- First-person layer: currently the same default render layer; only `MechanicalArmVisual` is allowed under the camera.
- UI layer: `HUD` and pause/end screens are `CanvasLayer`/Control nodes.
- SubViewport: no project scene currently contains a SubViewport in the inspected runtime path.

## Invariant

Only first-person view-model geometry may be parented under or transformed relative to the active `Camera3D`. World enemies, crashed ships, terrain, pickups, workbench, save point and beacon must never be descendants of `Camera3D`, the rotating `Head`, `CanvasLayer`, HUD, or a first-person visual root.
