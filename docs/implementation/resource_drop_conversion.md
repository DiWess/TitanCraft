# ResourceDrop Scene Setup and Placeholder Conversion

## Inspector Setup

- Instance `scenes/Resources/ResourceDrop.tscn` for Metal, Biomass, and Electronics drops.
- Set `Type` to `Metal`, `Biomass`, or `Electronics` and set `Quantity` to the intended pickup amount.
- Keep the root `ResourceDrop` on collision layer 3 (`collision_layer = 4`) and player mask 1.
- Keep `StaticPhysicsBody` on collision layer 2 (`collision_layer = 2`) only for solid scrap; disable its child shape for non-solid pickups.
- Assign a PBR mesh/material to `VisualGroup/ItemMesh`; use metallic `0.35-0.8`, roughness `0.32-0.68`, and a readable albedo by resource type.
- Keep `VisualGroup/HighlightRing` using `HighlightRingPulse.tres`; it is a horizontal additive yellow ring driven by `TIME` with `pulse_speed = 2.4` and `pulse_strength = 0.28`.
- Assign a local pickup stream to `AudioPlayers/SpatialPickupPlayer` once a licensed CC0/local asset is approved.

## Landmark and Debris Layout

```text
Main
└── Environment
    ├── StaticClutter        # MeshInstance3D debris, no collision
    └── CoverElements        # StaticBody3D cover/rocks/scrap with collision
```

`Workbench.tscn` and `Beacon.tscn` should add a visible `SpotLight3D` and/or vertical `GPUParticles3D` beam under a `LandmarkVfx` node so both objectives read as map-scale weenies.

## Automated Converter Hook

Attach `src/Tools/ResourceDropSceneConverter.cs` to a temporary editor-only node, assign `scenes/Resources/ResourceDrop.tscn` to `ResourceDropScene`, then tick `ExecuteConversion`. The tool logs one line per replacement plus a final summary such as `[ResourceDropConverter] Conversion complete. scanned=3 converted=3`.

## Placeholder Swap Protocol

1. Record each old `Placeholder_Metal_01` transform, groups, owner, and exported resource quantity.
2. Instance `ResourceDrop.tscn` at the same parent and copy the transform to the new root `Area3D`.
3. Copy every group from the placeholder to the new `ResourceDrop` root so programmatic queries continue to work.
4. Repoint any exported `NodePath` that referenced the old placeholder to the new root path in the same scene.
5. Set `Type = Metal`, `Biomass`, or `Electronics`; set `Quantity`; then remove the placeholder only after `godot --headless --path . --import` and tests pass.
