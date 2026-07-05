# Crash Site Scene Composition Plan

Owner: TitanCraft Project Director / Visual Director  
Version: 1.0  
Date: 2026-07-05  
Review status: Composition blueprint ready for future PR-sized visual tasks; not scene approval

## Composition objective

Compose the locked Crash Site as one readable survival loop: spawn, collect, craft, fight one Galaxabrain Scout, recover component, save, activate beacon, win. The environment should feel like a hostile biomechanical machine-world crash basin while preserving strict route clarity and MVP scope.

## Zone 1: Spawn / orientation zone

- **Player goal:** Understand crash context and identify the first resource/workbench route.
- **Dominant visual landmark:** Crashed ship main hull, crushed nose, and torn spine.
- **Secondary landmarks:** Basalt route markers, ash path edge, nearby debris fields, distant rim.
- **Path readability:** Player should see a safe route out of spawn within one camera turn; orange human accents must suggest the workbench direction without becoming arrows everywhere.
- **Object density:** Medium near ship, low on immediate route floor.
- **Lighting mood:** Broad key on hull planes; shadows define damage but do not hide the exit route.
- **Screenshot angle:** Player-height spawn view with ship first read and workbench/resource route second read.
- **Failure risks:** Toy/capsule hull, flat plate terrain, unclear first objective, background shapes competing with route.

## Zone 2: Resource trail

- **Player goal:** Find metal, biomass, and electronics without a long tutorial.
- **Dominant visual landmark:** Sequence of resource silhouettes staged along shaped ash/basalt path.
- **Secondary landmarks:** Small cargo fragments for metal/electronics, alien contamination for biomass, route basalt shards.
- **Path readability:** Resources should sit on slightly raised/dust-highlighted ground patches and contrast by silhouette first, color second.
- **Object density:** Low-to-medium; no clutter that hides pickups.
- **Lighting mood:** Even readable light with small local accents only where pickups need recognition.
- **Screenshot angle:** Contact sheet from approach distance showing all three pickup types distinguishable.
- **Failure risks:** Generic glowing cubes, color-only differentiation, pickup clutter, path reading as a decorated slab.

## Zone 3: Workbench zone

- **Player goal:** Return and craft Mechanical Arm Mk I.
- **Dominant visual landmark:** Workbench with raised armature and muted orange handles.
- **Secondary landmarks:** Modular crates, repair panels, cable bundles, cargo fragments, safe human lighting.
- **Path readability:** The workbench should face the incoming resource trail and frame the next route to the combat arena after crafting.
- **Object density:** Medium-high controlled industrial cluster; interactive work surface must stay visible.
- **Lighting mood:** Warmer human pocket with subtle orange and off-white highlights; no toy brightness.
- **Screenshot angle:** First-person approach and close interaction framing with prompt visible if UI is modified.
- **Failure risks:** Bench reads as random cube table, orange applied to non-interactive clutter, UI doing all readability work, fantasy forge language.

## Zone 4: Crash hull hero zone

- **Player goal:** Orient using the ship and read the world story.
- **Dominant visual landmark:** Main hull with side cheek plates, breach opening, broken ribs, and buried lower hull.
- **Secondary landmarks:** Rear engine assembly, torn wing panels, loose panels, cable bundles, scorch plates.
- **Path readability:** Hull must frame routes rather than block them; breach/interior should add depth without promising a new playable interior.
- **Object density:** High on ship mass, medium in debris field, low on route centerline.
- **Lighting mood:** Strong side light and rim separation to prove heavy mass and torn silhouette.
- **Screenshot angle:** Three-quarter hero screenshot, side neutral silhouette, low-angle hull-ground contact screenshot.
- **Failure risks:** Capsule/toy read, glossy plastic material, ship floating on ash, decorative details hiding bad silhouette.

## Zone 5: Combat arena

- **Player goal:** Fight the single Galaxabrain Scout after crafting the arm.
- **Dominant visual landmark:** Scout silhouette and red weak core inside a darker alien-contaminated arena.
- **Secondary landmarks:** Arena edge markers, alien shard props, basalt boundary, route exit, component recovery spot.
- **Path readability:** Entry, safe movement space, Scout location, and exit must remain readable at FPS height.
- **Object density:** Medium at arena edges, low in core combat lane.
- **Lighting mood:** Higher contrast hostile palette with readable fill; red weak core remains the only strong red focal point.
- **Screenshot angle:** Arena entry before combat, combat-distance weak-core view, defeated Scout/component view.
- **Failure risks:** Weak core too small, glow overload, alien clutter mistaken for resources, arena implying boss-stage expansion.

## Zone 6: Component recovery point

- **Player goal:** Recover the Galaxabrain component after defeating the Scout.
- **Dominant visual landmark:** Unique Galaxabrain component near disabled Scout silhouette.
- **Secondary landmarks:** Dimmed Scout core, broken armor shell, highlighted safe route to beacon/save zone.
- **Path readability:** Component must be visible from post-combat position and point player toward final route after collection.
- **Object density:** Low around pickup; dead Scout shape provides context.
- **Lighting mood:** Reduced combat intensity; small controlled component glow.
- **Screenshot angle:** First-person post-combat view with disabled Scout and component.
- **Failure risks:** Component looks like common biomass, defeated Scout appears alive, route to beacon is unclear.

## Zone 7: Save point

- **Player goal:** Save local progress during or before final activation sequence.
- **Dominant visual landmark:** Rugged save terminal with small state light.
- **Secondary landmarks:** Beacon base nearby or visible down route, human crates/cables, safe terrain patch.
- **Path readability:** Save point should be on the route, not hidden off-path.
- **Object density:** Low-to-medium; station must stand out.
- **Lighting mood:** Calm human safety tone, small cyan/white saved-state accent.
- **Screenshot angle:** Approach view and close prompt/state view.
- **Failure risks:** Reads as crate, shrine, or teleporter; oversized effects compete with beacon.

## Zone 8: Beacon extraction zone

- **Player goal:** Activate rescue beacon and complete MVP victory condition.
- **Dominant visual landmark:** Beacon base and beam emitter.
- **Secondary landmarks:** Stabilizer feet, service panel, cable run, distant ship silhouette behind/aside.
- **Path readability:** After component recovery, the beacon should become the strongest forward objective.
- **Object density:** Medium around equipment, low around activation point.
- **Lighting mood:** Controlled cyan/white activation accent; environment remains visible and not washed out.
- **Screenshot angle:** Inactive approach, activation close-up, active beacon victory framing.
- **Failure risks:** Beacon reads as rocket/launch pad, overbright pillar, final screen claims public-ready visuals.

## Zone 9: Background silhouette zone

- **Player goal:** None; provides atmosphere only.
- **Dominant visual landmark:** Fractured basalt rim and distant alien machine-world silhouettes.
- **Secondary landmarks:** Smoke plume, unreachable arcs, far dark violet forms.
- **Path readability:** Background must never become a false objective.
- **Object density:** Low, layered, atmospheric.
- **Lighting mood:** Hazy, desaturated, lower contrast than gameplay landmarks.
- **Screenshot angle:** Wide hero view with crash ship and route still dominant.
- **Failure risks:** Implies extra maps, multiple biomes, extra enemy types, or climbable/explorable routes.

## Required review contact sheet

Future scene-composition PRs should provide a contact sheet containing:

1. Spawn orientation.
2. Resource trail approach.
3. Workbench approach.
4. Crash hull hero three-quarter.
5. Combat arena entry.
6. Combat weak-core distance.
7. Disabled Scout/component recovery.
8. Save point.
9. Beacon inactive/active comparison.
10. Wide background silhouette view.

## Composition verdict rule

A composition pass may receive `VISUAL_SLICE_GAMEPLAY_SAFE` only after required gameplay/scene validation runs. It may receive `STAGE_A_VISUAL_APPROVED` only after human or assigned visual-review approval of opened screenshots.
