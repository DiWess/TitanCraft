# Phase 3A code-aware visual integration plan

No new visual assets are integrated by this plan. Every pass is atomic and may proceed only after contract tests pass.

| Pass | Authorized nodes | Forbidden nodes | Expected parent | Transform-space rules | AABB range | Collision rules | Runtime tests | Rollback condition | Screenshot |
|---|---|---|---|---|---|---|---|---|---|
| 1 terrain visual-only children | `Ground`, decorative top-level terrain containers | Player, Camera3D, HUD, pickups, enemy | Main/world | world local only | visual extents inside play area, no spawn overlap | no new collision unless intentional | scene contract + collision policy | player spawn/interactions blocked | only after tests pass |
| 2 crashed ship world visual | `AuthenticatedCrashSiteVisuals` or dedicated ship Node3D | Player/Head/Camera/HUD | Main/world | world transform, origin documented | size compatible with crash site, not camera-sized | visual-only | ship world-space test | view obstruction or interaction blocked | only after tests pass |
| 3 resource pickups | pickup visual child meshes | scripts/collisions/root names | each pickup root | local visual transforms only | close to pickup collision | preserve Area3D collision | pickup script/collision behavior tests | pickup cannot be collected | only after tests pass |
| 4 workbench/save/beacon | local visual children and beacon state visuals | exported paths/scripts/collisions | interactable roots | local transforms only | readable, no raycast obstruction | preserve Area3D collision | craft/save/victory tests | interact fails | only after tests pass |
| 5 Galaxabrain visual | enemy local visual child | root script, PlayerPath, camera, player | Galaxabrain root | local-only child; root remains gameplay | visual/capsule ratio bounded | preserve capsule | spawn/distance/move/visual-ratio tests | enemy stuck on camera or cannot move | only after tests pass |
| 6 first-person arm view model | `Head/Camera3D/MechanicalArmVisual` or future `FirstPersonVisualRoot` | world nodes, full mech body | active Camera3D | camera-local compact viewmodel only | each dimension < 1.5m target | no collision | arm parent/no-collision/bounds tests | pre-craft/craft/attack raycast broken | only after tests pass |
