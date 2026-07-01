# Phase 3A Pass 1B procedural terrain route map

The Pass 1B terrain generator reads these node transforms from the real production `scenes/Main/Main.tscn` at runtime. The existing `Ground/Collision_Ground` remains authoritative gameplay collision; procedural terrain is a visual-only world-space layer. The old flat `Ground/MeshInstance3D` is hidden to avoid z-fighting while `Ground/Collision_Ground` remains unchanged.

| Gameplay target | Node path | World position | Clearance radius | Route group |
|---|---|---:|---:|---|
| Player spawn | `Player` | (0.00, 1.20, 0.00) | 2.50 m | Spawn |
| Metal pickup | `Placeholder_MetalPickup` | (8.00, 0.85, -4.00) | 2.50 m | Resource loop |
| Biomass pickup | `Placeholder_BiomassPickup` | (-8.00, 0.85, -4.00) | 2.50 m | Resource loop |
| Electronics pickup | `Placeholder_ElectronicsPickup` | (0.00, 0.85, -10.00) | 2.50 m | Resource loop |
| Workbench | `Placeholder_Workbench` | (12.00, 0.85, -12.00) | 3.00 m | Crafting |
| Galaxabrain spawn | `Placeholder_GalaxabrainScout` | (20.00, 1.00, -16.00) | 4.00 m | Combat |
| Mission component/drop | `Placeholder_GalaxabrainScout/GalaxabrainComponentPickup` | (20.00, 1.00, -16.00) | 3.00 m | Combat recovery |
| Save point | `Placeholder_SavePoint` | (-12.00, 0.85, -12.00) | 3.00 m | Save route |
| Beacon | `Placeholder_Beacon` | (28.00, 0.85, -20.00) | 3.00 m | Victory route |

## Route construction

Safe corridors use a 5.0 m corridor width, derived from the player capsule, pickup/interactable radius, and extra readability margin. The route graph is:

- spawn → metal;
- spawn → biomass;
- spawn → electronics;
- each resource → workbench;
- workbench → Galaxabrain combat area;
- combat area → component;
- component → save point;
- save point → beacon.

Inside the corridor, procedural visual height is held to 0.03 m above the collision plane, with a 0.06 m maximum allowed route deviation. Outside the corridor, the generator blends into controlled low-poly volcanic shoulders, crater depressions, and outer ridges capped at 4.2 m.

## Pass 1C directed terrain zones

Pass 1C preserves the same runtime route extraction, seed and 5.0 m safe corridor, but replaces the prior distance-field mound look with named deterministic zones: `central_playable_plateau`, `ash_route_main`, `spawn_left_basalt_shelf`, `resource_right_basalt_shelf`, `workbench_midground_ridge`, `combat_midground_ridge`, `crater_northwest`, `crater_southeast`, `beacon_back_shelf`, and `irregular_horizon_ridge`. Bounds, areas, triangle counts, luminance inputs and feature height ranges are emitted into `artifacts/terrain-generation-report.json` by the integration runner.

Material treatment is terrain-only: `scenes/Environment/ProceduralCrashSiteTerrain.tscn` uses a local `StandardMaterial3D` with `vertex_color_use_as_albedo = true`, `albedo_color = Color(1, 1, 1, 1)` and `roughness = 0.92`. Zone vertex colours use lighter volcanic-soil values for the ash route, mid-value basalt for the central plateau and shelves, and darker restrained basalt/scorch values for ridges, craters and the horizon.
