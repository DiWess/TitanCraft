# Crash Site World-Class Visual Master Plan

Owner: TitanCraft Project Director / Visual Director  
Version: 1.0  
Date: 2026-07-05  
Review status: Plan ready for human review; not visual approval

## 1. Visual thesis

**TitanCraft Crash Site should read as a human survivor rebuilding industrial exploration technology from a hostile biomechanical machine-world crash site, staged in an ash-choked basalt basin where every silhouette explains the locked MVP loop.**

This plan preserves the README-locked Crash Site MVP: one small offline FPS map, resource collection, workbench crafting, Mechanical Arm Mk I, one Galaxabrain Scout fight, component recovery, save point, beacon activation, and victory/defeat/save flow. It does not authorize new gameplay, new scenes, asset implementation, public screenshots, or Stage A approval.

### Non-negotiable art direction

- **Style:** Polygonal Salvage Sci-Fi.
- **Geometry:** low-to-medium poly, authored broad planes, beveled masses, thick crash wreckage, readable silhouettes.
- **Readability:** simplified PBR, clear interaction colors, controlled detail density, strong first/second/third focal hierarchy.
- **Human technology:** industrial, field-repaired, damaged exploration hardware.
- **Alien technology:** hostile biomechanical forms with carapace, neural, ribbed, and asymmetric shapes.
- **World:** volcanic ash, basalt, fractured terrain, dust, scorch, buried wreckage.
- **Glow:** selective functional glow only.

### Rejection boundaries

Reject any future candidate that reads as cartoon toy, voxel/block-based construction, photoreal scan, glossy plastic sci-fi, over-neon cyber set, decorative kitbash without load-bearing logic, or a clean showroom spaceship.

## 2. Current visual diagnosis

The current visual state should be treated as **placeholder/blockout quality** even where pipeline checks run successfully.

- Current visuals are useful for validating route intent, object categories, and visual-review tooling, but the pipeline is not visual approval.
- Current terrain reads too flat and plate-like; navigable surfaces must become shaped ash, basalt, rim, and route landmarks rather than decorated slabs.
- The current ship still risks a capsule/toy read; it needs heavier massing, crushed asymmetry, torn structure, buried lower hull, and stronger industrial logic.
- Cubes, cones, panels, and simple route props still feel primitive; every retained shape needs authored purpose, bevels, material grouping, scale logic, or replacement.
- Lighting/readability is not final; future passes must prove silhouette separation, route readability, combat visibility, and restrained glow in screenshots.
- Generated review artifacts, imports, tests, and manifests can prove process, but they do not prove world-class visual quality.

## 3. Complete object inventory

The complete locked Crash Site visual inventory is maintained in `docs/art/crash-site-object-asset-inventory.md`. Required categories are:

- World / terrain.
- Crashed ship.
- Human tech.
- Resources.
- Alien tech / enemy.
- UI / readability.

No inventory item expands the MVP; each item must support the existing loop only.

## 4. Visual hierarchy

1. **First read:** crashed ship or current route objective depending on spawn angle.
2. **Second read:** workbench and nearby resource trail during the early collection/crafting loop.
3. **Third read:** Galaxabrain combat zone and Scout weak-core threat shape.
4. **Fourth read:** save point and beacon extraction zone after component recovery.
5. **Background read:** alien world silhouettes, fractured rim, distant biomechanical forms, and volcanic atmosphere.

### Hierarchy rules

- The ship must be the largest authored silhouette in the basin.
- The workbench must be visually interactive before UI appears.
- The Scout must contrast with human wreckage through purple/dark organic shapes and a controlled red weak core.
- Beacon glow must not compete with early-loop resource or workbench guidance until it is relevant.
- Background alien shapes must add mood without implying additional enemies, maps, or explorable biomes.

## 5. Material and color system

### Human palette

- Off-white worn panels.
- Dark graphite frames.
- Worn steel structural members.
- Muted orange handles, clamps, route markings, and interaction affordances.
- Small red warning lights only where danger or malfunction is needed.

### Alien palette

- Black / dark violet shell bases.
- Bruised purple organic volumes.
- Cyan / turquoise energy glow for powered alien technology.
- Organic dark red accents for tissue, weak-core threat, or injury states.

### Terrain palette

- Ash gray floors.
- Basalt black rocks and rims.
- Brown scorch marks.
- Dusty muted highlights on path edges and raised silhouettes.

### Color rules

- **Orange = human interaction / survival equipment.**
- **Cyan = powered technology / energy.**
- **Purple = alien biomechanical contamination.**
- **Red = danger / weak core / warning.**
- Glow must be selective, not everywhere; if every object glows, no object reads as important.

## 6. Lighting plan

- **Key direction:** low angled key from upper-left/front of the initial basin orientation so wreckage planes catch broad readable highlights.
- **Contrast target:** medium-high silhouette contrast with readable shadow interiors; no flat darkness and no overbright toy lighting.
- **Fill:** cool, low-intensity ambient fill to keep navigation safe without erasing form.
- **Rim:** restrained rim highlights on the ship spine, terrain rim, Scout carapace, and beacon mast.
- **Beacon glow:** cyan/white column or emitter accent only at the extraction stage; it should guide but not wash the scene.
- **Combat visibility:** Scout weak core, cover edges, floor hazards, and player retreat path must remain readable from first-person combat height.
- **Screenshot validation angles:** spawn orientation, resource trail toward workbench, three-quarter crash hull hero view, combat arena entry, defeated Scout/component view, save/beacon extraction view, and overhead route-composition debug view.

## 7. Approval gates and verdicts

Future visual work must separate implementation status from visual approval.

- `VISUAL_PLAN_READY`: a planning document defines scope, object briefs, evidence, forbidden patterns, and validation requirements.
- `ASSET_BRIEF_READY`: an individual asset brief is complete enough for a human or Blender artist to author without guessing.
- `ASSET_IMPLEMENTATION_PASS`: source, export, manifest, validation, and review artifacts exist; this is not final art approval.
- `VISUAL_SLICE_GAMEPLAY_SAFE`: scene or asset integration did not violate gameplay scope and passed required smoke/build checks.
- `STAGE_A_VISUAL_NOT_GO`: evidence exists but visual quality, readability, or approval is insufficient.
- `STAGE_A_VISUAL_APPROVED`: human or assigned visual-review authority approves opened PNG evidence for the Stage A target.

Passing tests does not approve visuals. Generating screenshots does not approve visuals. Only human/visual-review approval can approve final Stage A visuals.

## 8. Human-vs-agent production rule

Agent-generated geometry can support prototypes, route markers, proxy massing, internal blockout, and review tooling. World-class hero assets likely require human-authored or strongly art-directed Blender work with deliberate silhouettes, believable construction logic, and curated materials. Do not rely on autonomous procedural OBJ generation alone for final visual quality.

## 9. Evidence requirements for future visual PRs

Every future visual PR must include:

- Before/after screenshots where a scene changes.
- Contact sheet or turntable PNGs for standalone asset review.
- Gameplay smoke evidence if `Main.tscn` or gameplay-critical scene files change.
- Asset source/provenance, license, hash, and audition evidence for external or generated assets.
- Confirmation that no binary policy violation occurred.
- Final visual verdict separated from implementation verdict.
- Explicit note that generated artifacts and passing commands do not equal Stage A approval.

## 10. Forbidden claims

This plan must not be used to claim:

- Crash Site visuals are approved.
- Stage A is visually complete.
- Public demo screenshots are ready.
- Generated assets are already world-class.
- Gameplay tests equal visual quality.

## 11. Final plan verdict

`VISUAL_PLAN_READY` for documentation structure only. Stage A remains unapproved until future asset and scene evidence receives human/visual-review approval.
