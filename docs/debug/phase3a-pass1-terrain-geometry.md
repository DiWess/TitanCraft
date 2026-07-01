# Phase 3A Pass 1 terrain geometry measurements

All nodes are visual-only `MeshInstance3D` children under `Main/AuthenticatedTerrainVisuals`; no collision shapes, gameplay scripts, interaction masks, or gameplay positions are changed.

| Node | Source asset | Source AABB size | Longest source dimension | Local scale approx | World position | Distance from player spawn | Collision | Render layer |
|---|---|---|---:|---|---|---:|---|---|
| ForegroundRockFrame_Left | `rock_cliff_d.obj` | (1.04, 0.70, 1.44) | 1.44 | (5.56, 1.80, 5.56) | (-18, 0.05, 2) | 18.11 | none | default 1 |
| ForegroundRockFrame_Right | `rock_irregular_c.obj` | (1.68, 0.55, 1.00) | 1.68 | (4.84, 1.60, 4.84) | (16, 0.05, 1) | 16.03 | none | default 1 |
| RouteEdgeRock_MetalApproach | `rock_spire_g.obj` | (0.88, 0.55, 1.00) | 1.00 | (2.21, 0.90, 2.21) | (5, 0.05, -1) | 5.10 | none | default 1 |
| RouteEdgeRock_WorkbenchApproach | `rock_irregular_c.obj` | (1.68, 0.55, 1.00) | 1.68 | (2.63, 0.80, 2.63) | (14, 0.05, -8) | 16.12 | none | default 1 |
| MidgroundRidge_CrashLeft | `rock_ridge_f.obj` | (1.52, 0.40, 1.44) | 1.52 | (6.05, 1.40, 6.05) | (-22, 0.10, -21) | 30.41 | none | default 1 |
| MidgroundRidge_CrashRight | `rock_ridge_f.obj` | (1.52, 0.40, 1.44) | 1.52 | (5.54, 1.20, 5.54) | (8, 0.10, -25) | 26.25 | none | default 1 |
| CombatZoneBackRidge | `rock_cliff_d.obj` | (1.04, 0.70, 1.44) | 1.44 | (6.51, 1.50, 6.51) | (26, 0.10, -31) | 40.46 | none | default 1 |
| BackgroundSilhouette_Left | `rock_cliff_d.obj` | (1.04, 0.70, 1.44) | 1.44 | (8.06, 2.80, 8.06) | (-38, 0.20, -48) | 61.22 | none | default 1 |
| BackgroundSilhouette_Center | `rock_ridge_f.obj` | (1.52, 0.40, 1.44) | 1.52 | (7.54, 2.40, 7.54) | (0, 0.20, -54) | 54.00 | none | default 1 |
| BackgroundSilhouette_Right | `rock_irregular_c.obj` | (1.68, 0.55, 1.00) | 1.68 | (8.05, 2.50, 8.05) | (39, 0.20, -47) | 61.07 | none | default 1 |

## Rejection policy

No source mesh has an off-centre origin that requires blind compensation for this pass, and no local scale exceeds the integration ceiling used by the terrain contract test. Future terrain additions must extend this table before scene wiring.
