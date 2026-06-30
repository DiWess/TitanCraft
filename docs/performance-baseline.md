# Performance Baseline — Crash Site Phase 0

Date: 2026-06-30
Scope: current rendering and performance-relevant state before visual vertical slice work.

## Command results

| Command | Result | Performance relevance |
|---|---|---|
| `dotnet restore` | PASS | Dependency graph restores successfully. |
| `dotnet build` | PASS | Debug build completed with 0 warnings and 0 errors. |
| `dotnet test --settings tests/TitanCraft.runsettings` | PASS | 43 unit tests passed. |
| `godot --headless --path . --import` | PASS | Project imports in Godot 4.7 stable mono. |
| `./tools/test.sh` | PASS_WITH_KNOWN_WARNING | Debug/Release builds, unit tests, integration scene runner, import/export checks completed. Known save warning is intentionally triggered by an integration test. |
| `git status --short` | PASS | Clean before documentation changes. |

## Rendering configuration

| Setting | Current value |
|---|---|
| Godot version observed | 4.7.stable.mono.official.5b4e0cb0f |
| Renderer | Forward Plus |
| Main scene | `res://scenes/UI/MainMenu.tscn` |
| Viewport width | 1280 |
| Viewport height | 720 |
| Stretch mode | `viewport` |
| Stretch aspect | `keep` |
| Default clear color | `Color(0.04, 0.06, 0.09, 1)` |
| Default filter | nearest mipmap filter enabled |
| MSAA | No explicit project setting found in Phase 0 inspection. |
| SSAO | No explicit project setting found in Phase 0 inspection. |
| Volumetric fog | No explicit volumetric fog setting found; scene uses environment fog. |

## Main scene environment

| Element | Current state |
|---|---|
| `WorldEnvironment` | Present in `scenes/Main/Main.tscn`. |
| Sky | `ProceduralSkyMaterial` with dark blue/grey sky and ground colors. |
| Ambient light | Source 3, color `Color(0.18, 0.23, 0.32, 1)`, energy `0.45`. |
| Tonemap | `tonemap_mode = 2`. |
| Glow | Enabled, intensity `0.18`, strength `0.45`. |
| Fog | Enabled, color `Color(0.14, 0.17, 0.22, 1)`, density `0.006`. |

## Current lights

| Node | Type | Current use | Cost note |
|---|---|---|---|
| `DirectionalLight3D` | `DirectionalLight3D` | Primary scene light, shadows enabled, energy `1.05`. | Main shadow cost source. |
| `BaseLamp_1` | `OmniLight3D` | Orange human/base light, range `8.0`, energy `0.55`. | Dynamic local light. |
| `BaseLamp_2` | `OmniLight3D` | Orange human/base light, range `8.0`, energy `0.55`. | Dynamic local light. |
| `BaseLamp_3` | `OmniLight3D` | Orange human/base light, range `8.0`, energy `0.55`. | Dynamic local light. |
| `BaseLamp_4` | `OmniLight3D` | Orange human/base light, range `8.0`, energy `0.55`. | Dynamic local light. |
| `AlienZoneLight` | `OmniLight3D` | Violet alien-zone light, range `10.0`, energy `0.45`. | Dynamic local light. |

## Current materials

| Material | Current role |
|---|---|
| `assets/Materials/AlienBlack.tres` | Alien dark material resource. |
| `assets/Materials/AlienVioletEmissive.tres` | Violet emissive alien/beacon/crystal material. |
| `assets/Materials/BiomassRed.tres` | Biomass pickup material. |
| `assets/Materials/HumanBronze.tres` | Human bronze material resource. |
| `assets/Materials/HumanGraphite.tres` | Graphite human/metal/crate material. |
| `assets/Materials/HumanIvory.tres` | Ivory human panel/moon material. |
| `assets/Materials/HumanOrangeInteractive.tres` | Orange interaction/function material. |
| `assets/Materials/VolcanicRock.tres` | Ground and rock material. |

## Current mesh and collision profile

- Visual geometry is almost entirely built from built-in primitives: `BoxMesh`, `SphereMesh`, `CylinderMesh`, and `PrismMesh`.
- Collision is currently simple and predictable: `BoxShape3D` for ground, walls, crates, pickups, workbench, save point, beacon, and rocks; `CapsuleShape3D` for player and enemy.
- No dynamic trimesh collisions were observed.
- No `MultiMeshInstance3D` or LOD setup was observed.
- No imported external 3D model files were observed in the scoped asset folders.

## Current VFX, animations, and audio

| Category | Current state |
|---|---|
| Particles | No `GPUParticles3D` or `CPUParticles3D` nodes observed in inspected scenes. |
| Decals | No decal nodes observed in inspected scenes. |
| Animation | No `AnimationPlayer` nodes observed in inspected scenes. |
| Tweens | No scene-declared tweens observed; no final visual animation system documented yet. |
| Audio | No `AudioStreamPlayer` or `AudioStreamPlayer3D` nodes observed in inspected scenes. |
| Combat feedback | No hit flash, sparks, decals, screen shake, or enemy death VFX observed. |
| Environmental VFX | No smoke, dust, ash, steam, sparks, wind particles, or beacon particles observed. |

## Runtime script/process profile

| Script | Frame/process behavior | Baseline risk |
|---|---|---|
| `FirstPersonController` | Uses `_PhysicsProcess` for prompt update, movement, gravity, jump, and `MoveAndSlide`; uses raycasts for attack and interaction. | Future visual layers must not add global node searches or resource creation per frame. |
| `GalaxabrainScout` | Uses `_PhysicsProcess` for distance checks, state tick, chase movement, and attack attempts. | Future visual state must read existing state without changing AI timing. |
| `PauseMenu` | Uses `_UnhandledInput` for pause handling. | UI polish must preserve pause responsiveness. |
| `CrashSiteHudBinder` | Event-driven updates from health, inventory, mission, and prompt events. | HUD visual changes should keep event binding paths stable. |
| `CrashSiteEndScreenNavigator` | Event-driven victory/defeat scene requests. | End-screen polish must preserve scene paths and unpause behavior. |

## Performance measurements available in this environment

- Headless import completed successfully with exit code 0.
- Debug build completed in approximately 9.22 seconds in the first direct `dotnet build` run.
- Unit test suite completed with 43 passed tests in approximately 696 ms in the direct `dotnet test` run.
- `./tools/test.sh` completed successfully, including Debug build, Release build, unit tests, Godot integration runner, import, and export check.
- A precise interactive FPS value was not captured because this container did not run an interactive Godot graphics session or Windows build.

## Phase 0 performance risks for future visual work

- The scene already uses Forward Plus, fog, glow, one shadowed directional light, and five dynamic omni lights.
- Adding more dynamic lights for every small emissive detail would likely become expensive quickly.
- Future particles and transparent effects must be budgeted to avoid overdraw in the crash, beacon, and alien zones.
- Decorative meshes must not replace simple gameplay collision with dynamic or complex trimesh collisions.
- Materials should remain shared `.tres` resources to avoid unnecessary duplication.
- Future visual controllers must avoid creating new materials, meshes, packed scenes, or raycast queries repeatedly in `_Process` or `_PhysicsProcess` unless measured and justified.

## Phase 0 performance gate

Phase 1 may proceed from a performance-baseline perspective only if the team accepts that current numeric FPS is not measured in this container and must be captured later on the target Windows machine or an interactive Godot environment before final optimization claims.
