# Visual Placeholder Inventory — Crash Site Main Route

Date: 2026-07-03
Scope: current visible placeholder and primitive inventory before final visual vertical slice work.

## Summary

The Crash Site mission route is playable, but the visible presentation is still mostly composed from Godot primitives and placeholder nodes. The names and paths below are gameplay-sensitive until a dedicated migration updates tests and scene contracts.

## Main route placeholders and primitive visuals

| Path | Current visual | Current gameplay role | Stability note | Phase candidate |
|---|---|---|---|---|
| `Ground` | Single flat `BoxMesh` using volcanic rock material | Primary traversable ground | Keep `Ground/Collision_Ground` stable. | Phase 4A |
| `C7_Wall_1` | Rectangular panel box with orange strip | Human base/crash-side visual boundary | Keep `Collision_C7Wall` stable. | Phase 3A / 5 |
| `C7_Wall_2` | Rectangular panel box with orange strip | Human base/crash-side visual boundary | Keep `Collision_C7Wall` stable. | Phase 3A / 5 |
| `C7_Wall_3` | Rectangular panel box with orange strip | Human base/crash-side visual boundary | Keep `Collision_C7Wall` stable. | Phase 3A / 5 |
| `C7_Wall_4` | Rectangular panel box with orange strip | Human base/crash-side visual boundary | Keep `Collision_C7Wall` stable. | Phase 3A / 5 |
| `Placeholder_Crate1` | Graphite cube/box | Decorative salvage crate / route dressing | Root name is visibly marked placeholder; collision can block path. | Phase 3B |
| `Placeholder_Crate2` | Graphite cube/box | Decorative salvage crate / route dressing | Root name is visibly marked placeholder; collision can block path. | Phase 3B |
| `Placeholder_Crate3` | Graphite cube/box | Decorative salvage crate / route dressing | Root name is visibly marked placeholder; collision can block path. | Phase 3B |
| `Placeholder_MetalPickup` | Grey/graphite rectangular block | Required resource pickup, Metal 10 | Keep root Area3D, `ResourceKind`, `Quantity`, and collision stable. | Phase 6A |
| `Placeholder_BiomassPickup` | Low-poly red sphere | Required resource pickup, Biomass 3 | Keep root Area3D, `ResourceKind`, `Quantity`, and collision stable. | Phase 6A |
| `Placeholder_ElectronicsPickup` | Low-poly graphite cylinder | Required resource pickup, Electronics 2 | Keep root Area3D, `ResourceKind`, `Quantity`, and collision stable. | Phase 6A |
| `Placeholder_Workbench` | Ivory bench with orange strip plus small orange pylon/marker readability meshes | Required crafting interactable | Keep root Area3D and `Collision_Workbench` stable; new marker meshes are visual-only children. | Phase 6B |
| `Placeholder_SavePoint` | Low orange cylinder plus cyan core and orange marker | Optional local save interactable | Keep root Area3D and save signal contract stable; new marker meshes are visual-only children. | Phase 6B |
| `Placeholder_Beacon` | Graphite cylinder with hidden/active violet prism plus orange antenna wings and violet crown marker | Required final beacon interactable | Keep `ClosedVisual` and `ActiveVisual` paths unless tests/scripts migrate; new marker meshes are visual-only children. | Phase 6B / 11B |
| `Placeholder_GalaxabrainScout` in `Main.tscn` | Instanced enemy root at alien zone | Single required MVP enemy | Keep `PlayerPath`, root instance, and mission role stable. | Phase 8 |
| `Placeholder_GalaxabrainScout` in enemy scene | Hidden box mesh plus robot visual, red organic core, and cyan eye marker | Enemy visible body | Keep hidden placeholder mesh/root, collision, component pickup, and brain contracts stable. | Phase 8A |
| `GalaxabrainComponentPickup` | Hidden until enemy death; no distinct final visual yet | Required mission component after enemy death | Must remain accessible after enemy death. | Phase 9B |
| `VolcanicRock_1` | Stretched/tilted box rock | Blocking terrain element | Keep collision predictable; avoid blocking route. | Phase 4A |
| `VolcanicRock_2` | Stretched/tilted box rock | Blocking terrain element | Keep collision predictable; avoid blocking route. | Phase 4A |
| `VolcanicRock_3` | Stretched/tilted box rock | Blocking terrain element | Keep collision predictable; avoid blocking route. | Phase 4A |
| `DistantRock_4` | Box rock without collision | Distant environment silhouette | Visual-only; can be replaced by horizon modules. | Phase 4B |
| `DistantRock_5` | Box rock without collision | Distant environment silhouette | Visual-only; can be replaced by horizon modules. | Phase 4B |
| `DistantRock_6` | Box rock without collision | Distant environment silhouette | Visual-only; can be replaced by horizon modules. | Phase 4B |
| `DistantRock_7` | Box rock without collision | Distant environment silhouette | Visual-only; can be replaced by horizon modules. | Phase 4B |
| `AlienCrystal_1` | Violet `PrismMesh` | Alien route marker | Integration tests require this node to exist. | Phase 11B / 14 |
| `AlienCrystal_2` | Violet `PrismMesh` | Alien route marker | Keep route readability. | Phase 11B / 14 |
| `AlienCrystal_3` | Violet `PrismMesh` | Alien route marker | Keep route readability. | Phase 11B / 14 |
| `AlienCrystal_4` | Violet `PrismMesh` | Alien route marker | Keep route readability. | Phase 11B / 14 |
| `AlienCrystal_5` | Violet `PrismMesh` | Alien route marker | Keep route readability. | Phase 11B / 14 |
| `Moon` | Low-poly sphere | Distant landmark / atmosphere | Integration tests require this node to exist. | Phase 4B / 10 |
| `Player` | No visible FPS arm/body mesh | Player controller and camera | Add visuals only as child composition; keep root/head/camera stable. | Phase 7 |

## Missing final-presentation elements observed

- No visible first-person mechanical arm yet.
- No dedicated `VisualRoot`, `CollisionRoot`, `VFXRoot`, or `AudioRoot` structure on gameplay objects yet.
- No `GPUParticles3D` environmental or combat VFX observed.
- No `AnimationPlayer`-driven combat or idle animation observed.
- No decals or scorch/damage system observed.
- No modular human prop library observed under `scenes/Props`.
- No terrain module library observed under `scenes/Environment`.

## Replacement guardrails for future phases

- Add visual children under existing gameplay roots before renaming root nodes.
- Keep simple gameplay collision shapes separate from decorative geometry.
- Do not use decorative mesh origin or material state as gameplay state.
- Preserve interactable Area3D roots for raycast interaction.
- Preserve mission, save, crafting, and combat scripts unless a separate gameplay task is approved.
- Treat orange as interaction/function color and avoid overusing it on non-interactive props.
- Treat violet/cyan alien emission as rare and controlled to avoid hiding objectives.
