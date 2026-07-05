# TitanCraft Crash Site V1 Beta Visual Bible

## Visual thesis

The V1 beta pass keeps the Crash Site MVP scope locked while making the proven loop readable for outside testers: orange human salvage forms identify interaction and crafting, cyan alien/rescue light identifies progression-critical objectives, dark biomechanical forms identify threat, and off-white wreckage plus basalt silhouettes identify navigation landmarks.

## Palette

| Function | Color family | Use |
|---|---|---|
| Navigation | off-white hull, ash gray, basalt charcoal | crashed ship, route edges, landmark debris |
| Interaction | muted emergency orange | workbench controls, path cue strips, pickup rings |
| Threat readability | black/violet shell with cyan core | Galaxabrain Scout silhouette and weak-point focus |
| Progression clarity | cyan emission | component drop, beacon core, save point confirmation |
| Reward clarity | worn steel with orange/cyan accents | visible mechanical arm after crafting |

## Shape language

- Human crash materials use asymmetric, sheared panels and low, readable silhouettes.
- Interactables use simple vertical or panel shapes that contrast with terrain.
- Alien/progression objects use triangular or faceted cores with cyan emission.
- Decorative debris remains non-blocking and supports route reading only.

## Asset list

| Asset | Export | Gameplay function |
|---|---|---|
| TC_PROP_Workbench_01 | `assets/models/v1_beta/TC_PROP_Workbench_01.gltf` | Interaction |
| TC_PROP_Beacon_01 | `assets/models/v1_beta/TC_PROP_Beacon_01.gltf` | Progression clarity |
| TC_PLAYER_MechanicalArm_01 | `assets/models/v1_beta/TC_PLAYER_MechanicalArm_01.gltf` | Reward clarity |
| TC_CHAR_GalaxabrainScout_01 | `assets/models/v1_beta/TC_CHAR_GalaxabrainScout_01.gltf` | Threat readability |
| TC_PICKUP_Component_01 | `assets/models/v1_beta/TC_PICKUP_Component_01.gltf` | Progression clarity |
| TC_PICKUP_Metal_01 | `assets/models/v1_beta/TC_PICKUP_Metal_01.gltf` | Interaction |
| TC_PICKUP_EnergyCell_01 | `assets/models/v1_beta/TC_PICKUP_EnergyCell_01.gltf` | Interaction |
| TC_ENV_CrashDebris_A_01 | `assets/models/v1_beta/TC_ENV_CrashDebris_A_01.gltf` | Navigation |
| TC_PROP_SavePoint_01 | `assets/models/v1_beta/TC_PROP_SavePoint_01.gltf` | Interaction |

## Gameplay readability rules

1. The player start must face a visible crash landmark and orange route cue.
2. Pickups must have a small silhouette plus color-coded affordance ring.
3. The workbench must read as a human salvage station before text appears.
4. The Scout must have a dark body, hostile scale, and cyan focal core.
5. The component drop must be visually distinct from normal resources.
6. The mechanical arm must remain controlled by crafted/equipped state and use no collision.
7. The save point must read as checkpoint technology, not loot.
8. The beacon must be the tallest progression silhouette in its zone.
9. Victory feedback should amplify the beacon beam without adding mission steps.

## Deferred items

Cinematics, new enemy types, full character redesign, large terrain remake, complex VFX pipeline, dialogue systems, and a new crafting tree remain deferred for post-MVP decisions.
