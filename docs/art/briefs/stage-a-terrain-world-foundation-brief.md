# Stage A Terrain / World Foundation Asset Brief

Owner: TitanCraft Project Director / Visual Director  
Version: 1.0  
Date: 2026-07-05  
Review status: Brief ready for future implementation planning; not asset, scene, or visual approval

## 1. Purpose

The Stage A terrain/world foundation is the visual base for the locked Crash Site MVP. It must make the playable space read as an ash-choked basalt crash basin rather than a flat blockout, while preserving the existing Crash Site loop: spawn near the crashed ship, collect resources, craft at the workbench, fight one Galaxabrain Scout, recover the component, save, activate the beacon, and reach victory.

The terrain/world foundation must support:

- route readability from spawn through resource trail, workbench, combat arena, component recovery, save point, and beacon;
- crash impact believability through basin depression, fractured rims, buried hull contact points, scorch, debris mounds, and ash drift accumulation;
- foreground, midground, and background depth in player-height screenshots;
- visual contrast between human wreckage and the hostile alien basalt/biomechanical world;
- no gameplay collision changes unless a future implementation task explicitly scopes and validates them.

This brief is documentation only. It does not implement assets, edit scenes, modify `Main.tscn`, create OBJ/GLB/Blend files, or approve Stage A visuals.

## 2. Current failure diagnosis

The current terrain/world read should be considered internal placeholder/blockout quality only.

- Current ground reads as flat plates rather than shaped volcanic terrain.
- Current route edges feel like floating cards instead of grounded ash, rock lips, or embedded path boundaries.
- Current scene lacks believable basin/rim composition; the crash site does not yet feel carved by impact.
- Current background is a dark void rather than layered unreachable silhouettes with atmospheric depth.
- Current terrain does not yet sell a volcanic ash/basalt world.
- Current screenshots are internal placeholders only and must not be used as public demo or Stage A approval evidence.
- Pipeline success, import success, or generated artifacts do not prove terrain quality.

## 3. Target design

Stage A terrain should establish a compact, readable crash basin with authored depth and route clarity.

### Ash basin floor

- Broad concave ground form with shallow slope changes, dust drifts, cracked ash plates, and readable walkable lanes.
- Matte ash gray base with dusty highlights along route edges and pickup pads.
- No perfectly rectangular walkable boards; any navigable plane must be visually integrated into ash and basalt forms.

### Raised fractured rims

- Irregular basalt rim pieces frame the small map boundary and prevent the background from reading as empty void.
- Rim silhouettes should step in height around the basin, with broken notches that frame route views but do not imply extra paths.
- Rims must be dark and volcanic, not pure black walls.

### Basalt outcrops

- Angular, low-to-medium poly outcrops act as landmarks, occluders, and route-readable edges.
- Outcrops should use broken layered plates and dusty caps, avoiding random cones, pyramids, or repeated spike props.

### Route-readable debris corridor

- The path from spawn to resources to workbench should read through ash highlights, basalt markers, and low debris mounds.
- The corridor must remain visually clear at first-person height and avoid clutter hiding pickups or prompts.

### Buried hull contact points

- Terrain must visually embed the crashed hull with ash berms, crushed contact mounds, scorch stains, and debris partially swallowed by dust.
- The ship should feel heavy and crashed into terrain, not placed on top of a board.

### Layered background silhouettes

- Background should use unreachable basalt ridges, distant alien arcs, smoke/haze, and dark violet shapes behind the basin.
- Background silhouettes must support mood only; they must not imply new maps, new biomes, or additional enemy types.

### Non-flat horizon breakup

- The horizon should break into jagged basalt, ash ridges, and distant machine-world forms.
- Avoid a dark void, straight horizon line, or flat sky-ground seam.

### Combat arena boundary read

- The Scout arena should have a readable oval/circular boundary created by basalt rocks, alien shard props, ash edge changes, and low wreckage.
- The boundary must not hide the Scout, weak core, component recovery point, or exit path.

### Beacon/save zone grounding

- Save and beacon equipment should sit on grounded ash/rock pads with subtle service clearings, cable burial, and stabilizer contact marks.
- The final beacon area should feel like human survival hardware planted into hostile terrain, not a floating sci-fi platform or rocket pad.

## 4. Object list

| Object | Purpose | Required read | Evidence target |
|---|---|---|---|
| Main crash basin mesh | Primary visual ground base | Concave ash/basalt basin with route lanes | Overhead and spawn screenshots |
| Fractured rim pieces | Map boundary and depth | Jagged, layered basalt frame | Wide hero and boundary screenshots |
| Basalt route markers | Natural wayfinding | Knee-to-chest-height dark shards | Route contact sheet |
| Ash drift patches | Dust accumulation and route softness | Low matte drifts around debris/hull | Close and mid-distance PNGs |
| Buried debris contact mounds | Wreckage grounding | Ash/scorch mounds embedding hull and panels | Hull-ground contact PNG |
| Distant alien basalt silhouettes | Background mood | Unreachable alien machine-world shapes | Wide background screenshot |
| Foreground occluder rocks | Screenshot depth and player framing | Cropped basalt/ash forms near camera | Player-height composition PNGs |
| Combat arena boundary rocks | Scout arena readability | Oval boundary without clutter | Arena entry and combat screenshots |
| Beacon platform grounding | Final objective grounding | Ash pad, stabilizer marks, cable burial | Beacon inactive/active PNGs |
| Workbench route edge grounding | Early-loop route readability | Ash lip/debris edge leading to workbench | Spawn-to-workbench route PNG |

## 5. Shape language

Use:

- angular basalt masses with faceted planes and dusty worn edges;
- broken layered plates that feel geological, fractured, and crash-disturbed;
- non-rectangular terrain edges with bevel-like erosion and ash buildup;
- raised lips around the impact zone and localized depressions under hull/debris;
- cracks that read as fractured volcanic crust, not voxel/block grid seams;
- broad low-to-medium poly forms that read clearly before material detail.

Avoid:

- pyramids, random cones, default cubes, flat rectangles, floating slabs, and card-like route edges;
- voxel, Minecraft-like, destructible-block, or grid terrain language;
- noisy photoreal scanned rocks that fight the simplified PBR direction;
- decorative clutter that hides route direction, pickups, combat visibility, or interactables;
- artificial black walls or void backgrounds.

## 6. Material plan

- **Ash gray base:** matte, dusty, low-saturation ground with subtle value variation for walkable lanes.
- **Basalt black raised forms:** dark volcanic rocks with readable plane highlights; never pure black void.
- **Muted brown scorch staining:** impact streaks, hull contact burns, debris blast marks, and arena stress marks.
- **Dusty highlight edges:** route edges, rock lips, cracked plate tops, and ash drift crests.
- **Alien contamination accents:** restrained dark violet/purple only near combat or alien shard areas.
- **No glossy terrain:** roughness should stay high; wet or plastic-looking ground is forbidden.
- **No noisy photoreal textures:** use simplified PBR groups and broad authored values, not dense scan detail.
- **No excessive glow:** terrain itself should not glow; cyan/purple/red remain functional tech/alien signals.

## 7. Scale and composition rules

- **Player-height relation:** route rocks should range from ankle to chest height; boundary rim pieces can exceed player height but must not look climbable into new areas.
- **Hull burial depth:** the lower hull should appear buried by roughly knee-to-waist-high ash mounds at contact areas, with deeper burial at the nose/impact side and lighter accumulation at raised wreckage.
- **Route width:** main traversal lanes should feel wide enough for current FPS movement and resource collection without adding new traversal mechanics; visual narrowing can be done with low edges, not collision promises.
- **Arena width:** the combat arena should read as a clear oval/circular space sized for the existing single Scout fight, with edge markers outside the main combat lane.
- **Landmark sightlines:** spawn should read ship first and early route/workbench second; combat entry should read Scout/arena first; post-combat should read component then beacon/save direction.
- **Foreground/midground/background:** every required screenshot should include a foreground occluder or edge, a midground objective/route, and background rim/silhouette depth where possible.
- **Screenshot camera expectations:** future implementation must capture spawn orientation, resource route, workbench route, hull-ground contact, combat arena entry, combat-distance view, component recovery, save/beacon zone, and wide background silhouette.

## 8. Implementation options

### Option A — Text-generated OBJ continuation

- **Description:** Continue using autonomous text/script-generated OBJ-style terrain candidates.
- **Strengths:** Fast iteration, easy deterministic regeneration, useful for rough route markers and internal blockout.
- **Risks:** High risk of flat boards, floating card edges, fake placeholder geometry, repeated primitives, poor silhouette authorship, weak material logic, and misleading pipeline success.
- **Best use:** Internal massing tests only, explicitly labeled as blockout/prototype.
- **Verdict:** Not recommended for final Stage A terrain/world quality.

### Option B — Blender-authored terrain source

- **Description:** Author terrain as deliberate Blender source assets with named mesh components, material slots, turntable/review renders, export/import validation, and manifest evidence.
- **Strengths:** Best control over basin depression, rim composition, burial contacts, silhouette rhythm, scale, and material grouping.
- **Risks:** Requires more human/art direction time and careful scene-integration planning.
- **Best use:** Final Stage A terrain/world foundation assets after this brief is approved.
- **Verdict:** Recommended for world-class quality.

### Option C — Hybrid blockout plus human-authored final

- **Description:** Use simple generated or in-editor blockout to validate composition and screenshot angles, then replace with Blender-authored production terrain pieces.
- **Strengths:** Preserves fast layout iteration while preventing blockout geometry from becoming final art.
- **Risks:** Requires strict labels and gates so prototypes are not mistaken for approved assets.
- **Best use:** Best practical production path: block out route/composition, then author final in Blender.
- **Verdict:** Recommended route: use hybrid for planning, with Blender-authored terrain as the final visual target.

## 9. Quality gate

Future implementation cannot pass visual review until opened screenshots answer these checks:

1. Does it still look like a board from spawn, overhead, and route screenshots?
2. Does the hull feel embedded by ash, mounds, scorch, and terrain contact?
3. Can the player read the route from spawn to resources, workbench, arena, component, save, and beacon?
4. Are foreground, midground, and background separated by silhouette, value, and depth?
5. Does the terrain avoid toy/blockout primitives, random cones, flat rectangles, floating slabs, and voxel/block reads?
6. Are screenshots improved from all required angles without relying on UI labels or test success as visual proof?
7. Does the background avoid pure black void while remaining non-playable and non-distracting?
8. Does the combat arena boundary support visibility of the single Scout, weak core, component, and exit?
9. Are materials matte, simplified, and aligned to ash gray, basalt black, brown scorch, and dusty highlight rules?
10. Is the final visual verdict separate from implementation/build verdict?

## 10. Evidence required for future implementation PR

A future implementation PR must provide:

- before/after PNGs for every changed scene angle;
- a contact sheet covering spawn, route, workbench, hull-ground contact, arena, component, save/beacon, and background;
- opened-image critique naming focal point, route readability, silhouette, scale, and material coherence;
- gameplay smoke evidence if `Main.tscn` or any gameplay-relevant scene changes;
- Blender source/provenance, GLB/export evidence, hashes, and manifest entry if assets are authored/exported;
- confirmation that generated PNGs and binary outputs are not committed in violation of policy;
- no binary policy violation;
- final visual verdict separated from implementation verdict;
- human or assigned visual-review approval before any Stage A visual approval claim.

## 11. Forbidden scope

This brief forbids:

- new gameplay collision unless a future implementation task explicitly scopes and validates it;
- new biomes;
- new map area;
- new resources;
- new enemies;
- new mission steps;
- Stage B expansion;
- procedural world generation;
- voxel/block terrain;
- destructible terrain;
- public-demo or marketing screenshot claims;
- visual approval claims without screenshot review and human/visual-review verdict.

## 12. Final brief verdict

`STAGE_A_TERRAIN_BRIEF_READY` for planning documentation only. Stage A terrain/world visuals remain unimplemented and unapproved until future evidence satisfies the required gates.
