# Asset Brief — Mwezi Quarter District Kit V1

Project-authored architecture for the single MVP map's built environment, authored in Blender 4.0.2 by `tools/blender/create_badjanani_district_kit_v1.py`. Visual-only and collisionless; gameplay collision is authored separately in the scene as explicit `BoxShape3D` volumes.

## Why this kit exists

Before it, the MVP map was dressed almost entirely in volcanic rock, ship wreckage and camp props: one landmark, no street structure, no architecture, and no interior/exterior transitions. That is the direct cause of the two lowest visual scores in `docs/production/quality-scorecard-log.md` — axis 5 (world / level design) and axis 6 (visual art & presentation).

## Authority and scope

Authorised by an explicit human decision on 2026-09-17, recorded as an amendment to `README.md` sections 3, 14, 15 and the section 34 locked-decision table. The amendment changes **the setting of the single existing zone**, not the MVP's scope: one map, one enemy, the same mission anchors, the same loop. Nothing on the `README.md` section 6 forbidden list is introduced.

## Creative-property contract (README section 67)

- The in-game place is the **Mwezi Quarter** and carries its own name and fiction.
- The architecture draws on the **Swahili-Comorian coral-rag building tradition** of the Indian Ocean coast: lime-washed plaster over a rubble core of basalt, crushed coral and sea sand; pointed arcading on square piers; carved timber doors under heavy lintels, the timber split and bleached grey by age and salt air; flat roofs with parapets; external stairs; harbour retaining walls.
- **No real place, real building, or religious function is named or reproduced.** The arcaded hall is a secular civic ruin — a former harbour assembly hall — never a place of worship.
- The quarter's morphology is an **approximation** derived from architectural tradition. It is not survey data: no maps, aerial imagery, photographs or measurements were available to the authoring environment, and none were used. Any resemblance to a specific real street plan is neither claimed nor intended.

## Shared contract

- Scale: 1 Blender/Godot unit = 1 metre, Z-up authoring, Y-up GLB export.
- Origin: world zero at ground contact for every asset.
- Palette: lime render `(0.859, 0.831, 0.761)`, dressed coral rag `(0.561, 0.510, 0.424)`, weathered stone `(0.353, 0.318, 0.263)`, plaster-loss core `(0.090, 0.086, 0.082)`, bleached timber `(0.435, 0.400, 0.353)`, terracotta cloth, dry palm frond. The last two were corrected on 2026-09-24 against written description of the quarters: broken stone is ink-black basalt, not pale coral rag, and the woodwork is bleached rather than dark. `docs/art/references/comorian-seafront-reference-v1.md` section 3.5 is authoritative for all of them and governs the seafront kit too. Graphite / worn steel / interactive orange are lifted from the existing repo materials so the quarter and the wreck share one tonal family.
- Materials: Principled BSDF node materials only; the tower lamp uses Emission Strength.
- Edge treatment: every asset carries a single 18 mm angle-limited bevel, applied before export. Flat-shaded primitive architecture reads as paper until its edges catch light; this roughly triples raw primitive counts and is the reason the budgets below are higher than a prop's.
- Collision policy: `none` on every mesh (`titancraft_collision` custom property), enforced by `tools/blender/validate_blender_asset.py`.
- Export: GLB via `tools/blender/export_asset.py` to `assets/Production/Generated/MweziQuarter_V1/`, then a committed single-file text `.gltf` (embedded base64 buffer) under `assets/models/mwezi_quarter_v1/` via `tools/blender/glb_to_embedded_gltf.py`, matching the MVP Pack V1 precedent.
- Review: 3 auto-framed PNGs per asset via `tools/blender/render_badjanani_district_kit_reviews.py` — a three-quarter hero, a flat side silhouette, and a player-eye-height view at 1.65 m, because these are pieces the player walks past rather than props inspected from above. Every view carries a render-only 1.8 m scale post.

## Assets

| Asset | Role in the map | Approx. size (m) | Triangles | Silhouette target |
| --- | --- | --- | ---: | --- |
| `TC_ENV_CoralWallSegment_V1` | Alley edges, courtyard boundaries | 6.5 × 1.0 × 3.2 | 396 | Rendered wall run with coping ledge, damp-course plinth, two shallow buttress piers, and patches where the lime render has fallen to bare rag |
| `TC_ENV_ArcadeBay_V1` | Civic hall front; west face of the workbench courtyard | 12.0 × 1.4 × 6.3 | 2288 | Three pointed arches on square piers with bases and capitals, spandrel wall, cornice, merlon skyline |
| `TC_ENV_QuarterTower_V1` | Primary navigation landmark | 3.7 × 3.7 × 11.9 | 1460 | Three tapering stages with string courses, recessed slit openings, open belvedere, salvaged signal lamp |
| `TC_ENV_CarvedDoorway_V1` | Eye-level detail along the alleys | 3.6 × 0.9 × 3.5 | 2100 | Heavy stone surround with dentil frieze, recessed dark reveal, twin bleached-timber leaves with brass boss studs |
| `TC_ENV_StoneHouse_V1` | The quarter's residential mass | 7.0 × 6.0 × 6.4 | 1848 | Two storeys, parapet roof, recessed shuttered windows with sills, external stair to the roof terrace |
| `TC_ENV_StairTerrace_V1` | The map's vertical element | 6.3 × 9.0 × 3.0 | 616 | Stepped flight between cheek walls onto a parapeted platform |
| `TC_ENV_SeawallRun_V1` | Harbour edge of the quarter | 10.3 × 1.4 × 2.6 | 956 | Coped retaining wall with tide band, mooring bollards, eroded gaps |
| `TC_ENV_MarketStall_V1` | Human-scale street dressing | 3.1 × 2.1 × 2.3 | 812 | Cloth canopy on timber posts, counter, baskets |
| `TC_ENV_PalmCluster_V1` | Skyline break-up | 4.6 × 3.4 × 6.2 | 2800 | Two leaning trunks, fronds built from three segments of increasing droop |
| `TC_ENV_CoralRubble_V1` | Route edging, collapsed masonry | 4.3 × 3.9 × 1.1 | 748 | Mixed block sizes with chips and protruding roof timbers, mostly weathered core rather than clean rendered faces |

Kit total: ~14.2k triangles across 10 assets. Well inside the README section 28 target of 60 FPS on a mid-range Windows PC, where draw calls and dynamic light count — not triangle count at this scale — are the limiting factor.

## Motion kit (animated dressing)

An empty stone quarter reads as a diorama. Four skinned, looping assets give it
weather and life, authored by `tools/blender/create_mwezi_quarter_motion_kit_v1.py`.

**Why skeletal animation.** The asset contract requires every mesh at a clean
origin, so a cloth or frond animated as a separate child object — sitting at its
own pivot — fails validation outright. An armature keeps one mesh at the origin
and drives the motion through bones. That also survives glTF export and gives
Godot a real `AnimationPlayer`.

**Why a separate exporter.** `tools/blender/export_animated_asset.py` exists
because the static exporter passes `export_apply=True`, which applies modifiers
— including the Armature modifier carrying the skin. That would bake the rest
pose and silently strip the motion, producing a file that looks right and never
moves. The animated exporter refuses a source with no armature or no action, so
the failure is loud rather than silent.

| Asset | Motion | Triangles | Bones |
| --- | --- | ---: | ---: |
| `TC_ENV_PalmSway_V1` | Trunk and crown bend through a slow gust cycle; the lower trunk barely moves and the crown carries the travel | 1400 | 3 |
| `TC_ENV_LaundryLine_V1` | Three sheets swinging on offset beats, because washing on one line never moves in unison | 456 | 4 |
| `TC_ENV_AwningCloth_V1` | The unsecured front edge lifts and settles | 344 | 2 |
| `TC_ENV_BannerCloth_V1` | Hanging cloth swinging, the lower half lagging the upper | 268 | 3 |

All four run a 5-second, 120-frame loop at 24 fps. Every bone track starts and
ends on the same pose; the builder raises an error otherwise, because a
mismatched loop snaps visibly every five seconds in-engine.

**Runtime.** `src/World/EnvironmentMotionPlayer.cs` does two things the glTF
importer cannot: it sets the clip to loop (glTF has no looping concept, so Godot
imports every clip with looping disabled and each prop would sway once then
freeze), and it offsets each instance's start phase and playback rate from a
hash of its own world position — deterministic, so captures and tests
reproduce. Ten motion placements sit on the routes the player actually walks.

## Provenance

| Asset | Triangles | Source `.blend` SHA-256 | GLB SHA-256 | Review PNGs |
| --- | ---: | --- | --- | ---: |
| `TC_ENV_ArcadeBay_V1` | 2288 | `3d6033e016ae…` | `247e93309351…` | 3 |
| `TC_ENV_CarvedDoorway_V1` | 2100 | `87b25017a545…` | `5df073016f29…` | 3 |
| `TC_ENV_CoralRubble_V1` | 748 | `3992c2ddb84f…` | `3e56b2d0ab1b…` | 3 |
| `TC_ENV_CoralWallSegment_V1` | 396 | `0fe7c812f67f…` | `c5c8d7ebadc3…` | 3 |
| `TC_ENV_MarketStall_V1` | 812 | `5f1c9ab60d6d…` | `baa0ce557457…` | 3 |
| `TC_ENV_PalmCluster_V1` | 2800 | `e9f8b1aa01a1…` | `4c693b105eb3…` | 3 |
| `TC_ENV_QuarterTower_V1` | 1460 | `533146acf6f4…` | `f9402f185179…` | 3 |
| `TC_ENV_SeawallRun_V1` | 956 | `5b2ed6ebf5bb…` | `685c3a32401c…` | 3 |
| `TC_ENV_StairTerrace_V1` | 616 | `0e86ed767ef6…` | `8903de6088b5…` | 3 |
| `TC_ENV_StoneHouse_V1` | 1848 | `b2b8b218204e…` | `04dec3004422…` | 3 |

Full hashes are in `assets/Production/Generated/asset_manifest.json`. Licence: project-authored, no third-party source, no external download, no texture maps — every surface is geometry and material colour.

## Layout

The district is composed by `tools/level/build_mwezi_quarter_district.py`, which emits `scenes/Environment/MweziQuarterDistrict.tscn` from a declared layout table. The generator is the authoring surface; the scene file is generated output and must not be hand-edited.

It solves two things that hand-placement cannot hold stable:

1. **Ground height.** Each placement's Y is solved with the same formula `src/World/ProceduralCrashSiteTerrain.cs` uses, so building bases meet the visual terrain instead of floating over or sinking into it.
2. **Gameplay clearance.** Before writing the scene it asserts, against each collision box's *surface* (not its centre):
   - every mission anchor — spawn, three pickups, workbench, save point, beacon, Scout spawn — has more than 1.2 m of clearance;
   - every mission route segment keeps more than 0.9 m of walkable gap either side of its centre line;
   - nothing solid comes within 6.5 m of the Scout arena centre, because the Scout steers directly at the player with no navmesh and geometry inside the arena would trap it;
   - any structure marked permeable (the arcade, whose arches are walk-through) has an opening of at least 1.6 m.

   The check refuses to write the scene on failure. It caught four real layout faults during authoring: arcade piers standing inside the Scout arena, a house swallowing the metal→workbench walk, a seawall 0.13 m from the beacon, and a house a metre off the save point.

## Motion provenance

| Asset | Triangles | GLB SHA-256 | Motion review PNGs |
| --- | ---: | --- | ---: |
| `TC_ENV_PalmSway_V1` | 1400 | `436a8fbb7582…` | 4 |
| `TC_ENV_LaundryLine_V1` | 456 | `f8a908370996…` | 4 |
| `TC_ENV_AwningCloth_V1` | 344 | `c03a9fdee142…` | 4 |
| `TC_ENV_BannerCloth_V1` | 268 | `709f101b4f86…` | 4 |

A still cannot distinguish a playing rig from a frozen one, so each animated
asset is rendered at four frames across its loop from one fixed camera
(`tools/blender/render_mwezi_quarter_motion_reviews.py`): the displacement
between frames is the evidence. In-engine, two capture frames 1.5 s apart
(`district_12`/`district_13`) show the same measurable change.

## Known limitations

- The market stall's canopy is oversized relative to its posts and the counter reads thin. Cosmetic; left as-is rather than widening this change.
- The palm crown is dense and stylised at close range; it is authored to read at mid and long distance.
- The harbour has no water body. The procedural terrain rises to 4.2 m at the map bounds, so any water plane placed beyond the seawall would be occluded by the ridge behind it. The seawall is therefore a retaining wall over a dry tidal flat, which also keeps the existing volcanic-ash ground language intact.
- Aesthetic verdicts on this kit are candidate-level only. Integration into the production scene is evidenced separately in `docs/art/reviews/mwezi-quarter-district-v1-review.md`.
