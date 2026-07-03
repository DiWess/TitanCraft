# Crash Site Vertical Slice Implementation Plan

## Godot Node Structures

### Mission Manager

```text
Main
└── CrashSiteMissionManager (Node, CrashSiteMissionState owner)
    ├── MissionUpdateAudio (AudioStreamPlayer, UI bus)
    ├── ObjectiveChangedParticles (GPUParticles3D, optional world ping source)
    └── GoldenPathMarkers (Node3D)
        ├── WorkbenchLandmark (Node3D)
        │   ├── Beam (MeshInstance3D, yellow/orange unshaded transparent material)
        │   └── Particles (GPUParticles3D)
        └── BeaconLandmark (Node3D)
            ├── Beam (MeshInstance3D, blue/white activation material)
            └── Particles (GPUParticles3D)
```

`CrashSiteMissionState` exposes the player-facing phase title and breadcrumb text used by the HUD. The binder should play `MissionUpdateAudio` whenever `HudBreadcrumb` changes.

### Visual Indicators

```text
Interactable.tscn (Area3D + ICrashSiteInteractable script)
├── VisualRoot (Node3D)
│   ├── Mesh_LOD0 (MeshInstance3D, casts shadow)
│   ├── Mesh_LOD1 (MeshInstance3D, simplified fallback)
│   └── RimLight (OmniLight3D, yellow/orange, low energy)
├── InteractionArea (CollisionShape3D)
├── PromptAnchor (Marker3D)
└── PickupParticles (GPUParticles3D, one-shot)
```

```text
ResourceDrop.tscn (inherits Interactable.tscn)
├── ResourceColorBand (MeshInstance3D)
├── ShadowCaster (MeshInstance3D or enabled geometry shadow)
└── ResourcePickupAudio (AudioStreamPlayer3D)
```

Resources use high-contrast yellow/orange rim accents. Hostiles use a red/purple emissive core. Objective landmarks use vertical beams and particles so the player can read the route without a minimap.

## Golden Path Phases

1. **Survive & Orient**: spawn near the wreck, show movement prompt, place health pickup in direct view.
2. **Scavenge**: debris trail leads through Metal, Biomass, and Electronics drops; HUD tracks recipe counts.
3. **Craft**: workbench beam switches to high-intensity yellow; HUD says to return and build Mk I.
4. **Extract/Conquer**: Scout arena activates, then component and beacon breadcrumbs take over.

## Free Asset Slots

- **Terrain PBR**: PolyHaven or ambientCG rocky ground, packed as `assets/Textures/Terrain/CrashSite/*` with albedo, normal, roughness, and license entry in `THIRD_PARTY_ASSETS.md`.
- **Crash debris and props**: Kenney space/sci-fi kit meshes in `assets/Models/CrashSite/`, renamed by role (`ShipHull_Nose_A`, `DebrisTrail_Panel_A`, `Workbench_Frame_A`).
- **Audio**: CC0 UI clicks/mission stingers in `assets/Audio/UI/`, spatial enemy grunts in `assets/Audio/Enemies/GalaxabrainScout/`, pickup/craft/beacon sounds in `assets/Audio/World/`.
- **VFX**: Godot-native `GPUParticles3D` scenes under `scenes/VFX/` for pickup sparkle, arm impact sparks, Scout death burst, and beacon activation beam.

## Placeholder Conversion Method

1. Use Godot scene unique names or exported `NodePath` fields before renaming any `Placeholder_*` node.
2. Run a script that parses `.tscn` files, builds a mapping from placeholder node names to final names, and rewrites both `name = "Placeholder_*"` and matching exported `NodePath` strings in the same scene.
3. Reopen the project with `godot --headless --path . --import` to force scene validation.
4. Run `dotnet build`, tests, and a manual playthrough before deleting any old placeholder asset files.

## C# Integration Targets

- `CrashSiteMissionState`: owns phases and HUD breadcrumb text.
- `CrashSiteHudBinder`: listens to mission and inventory changes, updates the corner objective, and should play the UI mission update cue when the breadcrumb changes.
- `MechanicalArmRecipe`: exposes recipe progress text so the HUD can show Metal/Biomass/Electronics sub-objectives using the same data as crafting.
- `Workbench`: remains the single crafting gate; when crafting succeeds it advances the mission and plays craft feedback.
