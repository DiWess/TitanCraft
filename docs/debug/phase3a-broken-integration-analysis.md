# Phase 3A broken production integration analysis

Baseline note: the user referenced `636bf7a`, but that revision is not present in this clone. The matching commit by subject is `df9be8a4006161fe9846bc86a298bfb687472493` (`[kpi:phase3a] integrate authenticated production assets (+20%)`).

## Production scenes changed

- `scenes/Main/Main.tscn`
- `scenes/Player/Player.tscn`
- `scenes/Enemies/GalaxabrainScout.tscn`

## External resource paths replaced

| Scene | Resource id | Before | After |
|---|---|---|---|
| Main | `21_ship` | `res://assets/ThirdParty/Quaternius/UltimateSpaceKit/Models/Spaceship_BarbaraTheBee.obj` | `res://assets/Production/Environment/CrashSite/ship_hull_body.obj` |
| Main | `22_base_large` | `res://assets/ThirdParty/Quaternius/UltimateSpaceKit/Models/Base_Large.obj` | `res://assets/Production/Environment/CrashSite/damaged_structural_base.obj` |
| Main | `23_mech_george` | `res://assets/ThirdParty/Quaternius/AnimatedMechPack/Models/George.obj` | `res://assets/Production/Player/MechanicalArm/george_mech_source_arm.obj` |
| Main | `24_robot` | `res://assets/ThirdParty/Quaternius/AnimatedRobotPack/Models/Robot.obj` | `res://assets/Production/Enemies/Galaxabrain/robot_body_component.obj` |
| Player | `2_mech_george` | `res://assets/ThirdParty/Quaternius/AnimatedMechPack/Models/George.obj` | `res://assets/Production/Player/MechanicalArm/george_mech_source_arm.obj` |
| GalaxabrainScout | `3_robot` | `res://assets/ThirdParty/Quaternius/AnimatedRobotPack/Models/Robot.obj` | `res://assets/Production/Enemies/Galaxabrain/robot_body_component.obj` |

## Nodes whose mesh changed and transforms involved

The changed paths were file-level resource replacements, so these existing mesh nodes changed without local transform changes: `AuthenticatedCrashSiteVisuals/CrashedShip_AuthenticUltimateSpaceKit` (`Transform3D(-2.8,0,0.7,0,1.15,0,-0.45,0,-3.6,-4,1.0,-13)`), `AuthenticatedCrashSiteVisuals/BuriedHullMass_BaseLarge` (`Transform3D(2.4,0,0.35,0,0.75,0,-0.35,0,2.4,-9,0.35,-16)`), `AuthenticatedCrashSiteVisuals/ForegroundVolcanicRidge_Left` (`Transform3D(3.0,0,0.4,0,0.9,0,-0.4,0,3.0,-18,0.15,-7)`), `AuthenticatedCrashSiteVisuals/ForegroundVolcanicRidge_Right` (`Transform3D(2.4,0,-0.7,0,0.8,0,0.7,0,2.4,17,0.1,-9)`), `Placeholder_Workbench/WorkbenchAuthenticMechVisual` (`Transform3D(0.55,0,0,0,0.55,0,0,0,0.55,0,0.45,0)`), `Placeholder_SavePoint/SavePointAuthenticModule` (`Transform3D(0.45,0,0,0,0.45,0,0,0,0.45,0,0.2,0)`), `Placeholder_Beacon/BeaconAuthenticMast` (`Transform3D(0.35,0,0,0,0.75,0,0,0,0.35,0,1.35,0)`), `Player/Head/Camera3D/MechanicalArmVisual` (`Transform3D(0.18,0,0,0,0.18,0,0,0,0.18,0.55,-0.45,-0.9)`), and `GalaxabrainScout/AuthenticatedRobotGalaxabrainVisual` (`Transform3D(0.75,0,0,0,0.75,0,0,0,0.75,0,0,0)`).

## Imported mesh AABB, origin and scale

| Mesh | Vertex count | Face count | AABB min | AABB max | Size | Geometric centre | Origin distance | Scene scale |
|---|---:|---:|---|---|---|---|---:|---|
| `ship_hull_body.obj` | 3153 | 3222 | (-3.668,-2.182,-3.094) | (3.668,3.522,1.864) | (7.337,5.703,4.958) | (0.000,0.670,-0.615) | 0.910 | mixed 2.8/1.15/3.6 in ship node |
| `damaged_structural_base.obj` | 1457 | 1313 | (-4.265,0.004,-4.265) | (4.265,4.975,4.265) | (8.530,4.971,8.530) | (0.000,2.490,0.000) | 2.490 | mixed 2.4/0.75/2.4 and variants |
| `robot_body_component.obj` | 1822 | 2010 | (-1.521,-0.020,-1.282) | (1.579,4.477,1.346) | (3.099,4.497,2.628) | (0.029,2.228,0.032) | 2.229 | 0.75 on Galaxabrain, 0.45 on save point |
| `george_mech_source_arm.obj` | 4124 | 3816 | (-2.079,-0.033,-1.153) | (2.080,6.057,1.678) | (4.159,6.090,2.831) | (0.001,3.012,0.262) | 3.023 | 0.18 under camera, 0.55 on workbench |

## Gameplay tests added and why they missed the failure

The commit added only four load/path assertions: two `Main.tscn` mesh-not-null assertions, one `Player.tscn` arm mesh-not-null assertion, and one `GalaxabrainScout.tscn` robot mesh-not-null assertion. They did not simulate camera rendering, enemy spawn distance, camera-relative bounds, collision-to-visual ratios, movement, interaction raycasts, or world-vs-viewmodel parentage. A full mech body could therefore be accepted as a first-person arm and a large robot mesh could be accepted as an enemy visual even if its origin, scale, or first-person placement made the game unplayable.

## Root cause

The production integration treated authenticated file provenance as sufficient runtime compatibility. It replaced scene mesh resources without enforcing contracts for model role, AABB, origin, scale, transform space, camera-relative ownership, collision ownership, or behavior. The most dangerous specific mismatch is `george_mech_source_arm.obj`: numerically it is a complete mech-sized body (6.09 units tall, origin 3.02 units from centre), but it was mounted directly under `Head/Camera3D/MechanicalArmVisual` as a first-person arm view model. The Galaxabrain visual also uses a 4.50-unit-tall full robot against a 1.4-unit capsule, so its visual/collision mismatch is large unless intentionally scaled and tested.
