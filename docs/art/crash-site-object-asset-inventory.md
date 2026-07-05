# Crash Site Object Asset Inventory and Briefs

Owner: TitanCraft Project Director / Visual Director  
Version: 1.0  
Date: 2026-07-05  
Review status: Asset briefs ready for future review; not implementation approval

## Global quality gate for every object

Each object must preserve Polygonal Salvage Sci-Fi, the locked Crash Site MVP loop, low-to-medium poly readable forms, simplified PBR, strong silhouette, and selective glow. Each object requires review evidence before scene use: neutral silhouette PNG, material PNG, scale reference beside player where applicable, provenance/hash if sourced or generated, and a written verdict separated from gameplay/build status.

## WORLD / TERRAIN

### Object: Crash basin ground
- **Gameplay role:** Primary walkable ground for the small Crash Site loop.
- **Visual role:** Establishes ash-choked volcanic crash bowl.
- **Silhouette target:** Broad depressed basin with raised uneven edges, not a flat plate.
- **Shape language:** Layered ash, cracked slabs, embedded basalt, shallow impact depression.
- **Materials:** Ash gray, basalt black, dusty muted highlights, brown scorch marks.
- **Scale relation:** Player-scale surface with meter-wide cracks and readable path widths.
- **Readability distance:** Reads from spawn and overhead review.
- **Required variants:** Main path, arena floor, workbench pad, beacon pad.
- **Forbidden patterns:** Flat route slab, voxel/block terrain, noisy photoreal scan, random grey plane.
- **Quality gate:** Route is readable without UI and terrain reads shaped from all required screenshots.
- **Evidence required:** Before/after basin screenshots, overhead route contact sheet, gameplay smoke if scene changes.

### Object: Ash floor
- **Gameplay role:** Safe traversal base and contrast field for pickups.
- **Visual role:** Softens harsh basalt and sells dust/impact.
- **Silhouette target:** Low rolling planes with subtle drifts.
- **Shape language:** Wind-smoothed ash banks, shallow troughs, dust accumulation against wreckage.
- **Materials:** Matte ash gray, dusty beige-gray highlights.
- **Scale relation:** Foot-level; never obscures pickups.
- **Readability distance:** Medium and close.
- **Required variants:** Clean route ash, scorched ash, alien-contaminated ash.
- **Forbidden patterns:** Snow read, sand beach read, flat painted plane.
- **Quality gate:** Supports navigation while separating props.
- **Evidence required:** Close-up and route-distance PNGs.

### Object: Fractured terrain rim
- **Gameplay role:** Defines map boundary and frames route.
- **Visual role:** Makes the crash site feel contained and dangerous.
- **Silhouette target:** Jagged basalt crescent with broken height variation.
- **Shape language:** Polygonal cliffs, fractured shelves, undercut rocks.
- **Materials:** Basalt black, ash dust caps, scorch-dark creases.
- **Scale relation:** Taller than player; lower than ship hero silhouette where visible.
- **Readability distance:** Background and mid-distance.
- **Required variants:** Low boundary, tall backdrop, broken entry notch.
- **Forbidden patterns:** Vertical box wall, repeated cones, climbable-looking path to new areas.
- **Quality gate:** Blocks/frames without implying extra maps or routes.
- **Evidence required:** Spawn and combat arena boundary screenshots.

### Object: Basalt route markers
- **Gameplay role:** Guide path between resources, workbench, arena, and beacon.
- **Visual role:** Natural wayfinding without extra UI dependence.
- **Silhouette target:** Leaning dark shards with dusty edge highlights.
- **Shape language:** Triangular basalt fins, stacked broken stones, directional fractures.
- **Materials:** Basalt black, ash gray, occasional muted orange human tape only near human zones.
- **Scale relation:** Knee to chest height.
- **Readability distance:** 10-30 meters.
- **Required variants:** Small shard, paired gate, arena threshold.
- **Forbidden patterns:** Neon arrows everywhere, traffic cones, voxel pillars.
- **Quality gate:** Player can infer route in screenshots without labels.
- **Evidence required:** Route contact sheet with focal-point notes.

### Object: Debris fields
- **Gameplay role:** Frame playable routes and imply crash violence.
- **Visual role:** Break up terrain emptiness and connect ship to workbench/resources.
- **Silhouette target:** Low clusters of panels, struts, crates, and ash-covered scrap.
- **Shape language:** Angular wreckage partially buried with load-bearing thickness.
- **Materials:** Worn steel, off-white hull paint, graphite, scorch.
- **Scale relation:** Ankle to waist height; cover-like forms must not imply new combat systems unless already used.
- **Readability distance:** Mid-distance.
- **Required variants:** Human debris, alien shard debris, mixed contamination debris.
- **Forbidden patterns:** Random cube scatter, paper-thin panels, clutter blocking route.
- **Quality gate:** Adds density without hiding pickups or objectives.
- **Evidence required:** Before/after density screenshots and collision/gameplay smoke if placed.

### Object: Distant silhouettes
- **Gameplay role:** Non-playable backdrop only.
- **Visual role:** Suggest hostile machine-world without expanding map.
- **Silhouette target:** Far basalt teeth, broken towers, biomechanical arcs.
- **Shape language:** Simple dark layered forms with atmospheric fade.
- **Materials:** Basalt black, dark violet accents, dusty haze.
- **Scale relation:** Huge and unreachable.
- **Readability distance:** Background.
- **Required variants:** Basalt ridge, alien arc, smoke plume.
- **Forbidden patterns:** Visible playable buildings, multiple biomes, enemy silhouettes implying extra enemy types.
- **Quality gate:** Enhances mood but never redirects objective route.
- **Evidence required:** Wide screenshot with route still dominant.

### Object: Foreground occluders
- **Gameplay role:** Compose screenshots and guide first-person framing.
- **Visual role:** Add depth at spawn, hull, arena, and beacon views.
- **Silhouette target:** Cropped rocks, wreckage ribs, cable loops.
- **Shape language:** Chunky partial forms near camera edges.
- **Materials:** Terrain and wreckage palettes.
- **Scale relation:** Knee to above-player depending shot.
- **Readability distance:** Foreground only.
- **Required variants:** Rock occluder, hull rib occluder, cable occluder.
- **Forbidden patterns:** Visual clutter covering UI or objectives.
- **Quality gate:** Improves composition without impeding gameplay.
- **Evidence required:** Screenshot set from player-height cameras.

### Object: Workbench route zone
- **Gameplay role:** Guides collection return and crafting.
- **Visual role:** Human survival pocket near wreckage.
- **Silhouette target:** Warm industrial cluster with orange handles and flat work surface.
- **Shape language:** Rectangular pads, crates, cables, tool frames.
- **Materials:** Graphite, worn steel, off-white, muted orange.
- **Scale relation:** Player waist to head height.
- **Readability distance:** Spawn route and close-up.
- **Required variants:** Entry marker, bench pad, supporting crates.
- **Forbidden patterns:** Fantasy forge, toy worktable, unexplained neon machine.
- **Quality gate:** Clearly interactive before UI.
- **Evidence required:** Spawn-to-workbench route PNG and close interaction PNG.

### Object: Combat arena zone
- **Gameplay role:** Supports one Scout encounter.
- **Visual role:** More hostile, contaminated basin pocket.
- **Silhouette target:** Circular/oval readable fight space framed by alien shards and basalt.
- **Shape language:** Broken human debris invaded by asymmetric alien growth.
- **Materials:** Ash, basalt, dark violet, bruised purple, controlled cyan/red.
- **Scale relation:** Wide enough for FPS movement; landmarks at player height.
- **Readability distance:** Entry and combat ranges.
- **Required variants:** Entry threshold, arena edge, component pickup spot.
- **Forbidden patterns:** Boss-arena grandeur implying extra stage, hidden hazards not in gameplay.
- **Quality gate:** Scout, weak core, exit, and component point remain readable.
- **Evidence required:** Combat visibility screenshots and gameplay smoke when scene changes.

### Object: Beacon/save zone
- **Gameplay role:** Save and final beacon activation destination.
- **Visual role:** End-of-loop human rescue signal against hostile world.
- **Silhouette target:** Vertical beacon mast plus grounded save station.
- **Shape language:** Tripod/base plates, antenna, emitter dish, service panel.
- **Materials:** Human palette with cyan/white active beacon accent.
- **Scale relation:** Beacon taller than player; save station waist height.
- **Readability distance:** Long route and close-up.
- **Required variants:** Inactive, powered/active, saved-state accent.
- **Forbidden patterns:** Full rocket, launch pad, giant sci-fi tower, excessive glow.
- **Quality gate:** Final objective reads as beacon, not spacecraft launch system.
- **Evidence required:** Inactive/active screenshots and UI/objective state notes.

## CRASHED SHIP

### Object: Main hull
- **Gameplay role:** Spawn landmark and central crash-site orientation object.
- **Visual role:** Hero wreckage proving human industrial exploration tech.
- **Silhouette target:** Heavy asymmetric broken fuselage with broad planes, thick ribs, and collapsed mass.
- **Shape language:** Industrial cargo/exploration hull, not fighter or capsule toy.
- **Materials:** Off-white worn panels, graphite underframe, worn steel, scorch, ash burial.
- **Scale relation:** Several player-heights tall and long enough to dominate basin.
- **Readability distance:** First focal point from spawn.
- **Required variants:** Intact broad hull plates, damaged edge modules, buried lower section.
- **Forbidden patterns:** Capsule toy, glossy plastic ship, paper-thin shell, random cubes.
- **Quality gate:** Reads as crashed heavy ship in neutral silhouette before materials.
- **Evidence required:** Neutral side silhouette, front three-quarter, material hero PNGs, source/provenance/hash.

### Object: Crushed nose
- **Gameplay role:** Directional crash indicator and foreground landmark.
- **Visual role:** Sells impact direction and damage.
- **Silhouette target:** Flattened, buckled wedge buried in ash.
- **Shape language:** Folded armor, exposed frame, bent sensor housings.
- **Materials:** Scorched off-white, exposed steel, ash packed into seams.
- **Scale relation:** Taller than player but lower than main hull spine.
- **Readability distance:** Spawn/mid.
- **Required variants:** Primary nose and loose nose shards.
- **Forbidden patterns:** Rounded capsule front, clean cockpit toy window.
- **Quality gate:** Damage visible without decals.
- **Evidence required:** Nose close-up and spawn view PNGs.

### Object: Rear engine assembly
- **Gameplay role:** Secondary ship landmark and silhouette anchor.
- **Visual role:** Industrial propulsion mass.
- **Silhouette target:** Clustered heavy nozzles, broken rings, offset engine pods.
- **Shape language:** Cylinders with bevels, braces, vents, torn mounts.
- **Materials:** Dark graphite, heat-stained steel, brown scorch, faint dead red warning light if needed.
- **Scale relation:** Engine openings near player height to taller.
- **Readability distance:** Mid/background.
- **Required variants:** Dead nozzle, cracked nozzle, torn mount.
- **Forbidden patterns:** Clean rocket ready to launch, glossy turbines, full functional spacecraft.
- **Quality gate:** Reads wrecked and inert.
- **Evidence required:** Rear-engine PNG and neutral silhouette.

### Object: Torn wing panels
- **Gameplay role:** Route framing and wreckage scale.
- **Visual role:** Broad broken planes that imply crash spread.
- **Silhouette target:** Thick angular slabs with fractured ends.
- **Shape language:** Reinforced panels, spars, hinges, curled metal.
- **Materials:** Off-white worn paint, exposed graphite/steel, scorch.
- **Scale relation:** Player-height edges; large enough for occlusion.
- **Readability distance:** Mid.
- **Required variants:** Upright shard, buried panel, loose panel.
- **Forbidden patterns:** Paper planes, random rectangles, sleek fighter wings.
- **Quality gate:** Thickness and structure visible.
- **Evidence required:** Route composition PNG.

### Object: Broken ribs
- **Gameplay role:** Interior/exterior readability and possible foreground framing.
- **Visual role:** Shows ship skeleton.
- **Silhouette target:** Repeating but irregular curved/angled frame members.
- **Shape language:** Heavy structural ribs, torn braces, bolt blocks.
- **Materials:** Worn steel, graphite, ash.
- **Scale relation:** Larger than player torso.
- **Readability distance:** Close/mid.
- **Required variants:** Standing rib, collapsed rib, interior rib.
- **Forbidden patterns:** Thin wires, decorative arches, perfect repetition.
- **Quality gate:** Feels load-bearing.
- **Evidence required:** Interior/breach PNG.

### Object: Breach opening
- **Gameplay role:** Visual story point; may frame route if already scoped.
- **Visual role:** Clear torn hole into exposed interior.
- **Silhouette target:** Irregular jagged opening with thickness.
- **Shape language:** Peeled panels, rib cross-sections, cable shadows.
- **Materials:** Off-white exterior, dark graphite interior, steel, scorch.
- **Scale relation:** Human-scale entrance read, not necessarily traversable unless scene permits.
- **Readability distance:** Mid/close.
- **Required variants:** Main breach, small side tears.
- **Forbidden patterns:** Perfect doorway, polished hatch, unscoped interior level promise.
- **Quality gate:** Reads as crash damage without implying extra playable map.
- **Evidence required:** Breach screenshot and route note.

### Object: Exposed interior
- **Gameplay role:** Storytelling and resource context.
- **Visual role:** Human industrial cabin/cargo fragments.
- **Silhouette target:** Layered dark recesses with recognizable frames.
- **Shape language:** Cargo racks, conduits, floor grates, broken consoles.
- **Materials:** Graphite, steel, off-white fragments, muted orange emergency handles.
- **Scale relation:** Human usable modules.
- **Readability distance:** Close/mid.
- **Required variants:** Dark recess, cargo bay fragment, console fragment.
- **Forbidden patterns:** Full explorable interior expansion, clean cockpit luxury.
- **Quality gate:** Adds depth but stays MVP background/support.
- **Evidence required:** Close-up material PNG.

### Object: Torn spine
- **Gameplay role:** Long directional landmark.
- **Visual role:** Defines ship top silhouette.
- **Silhouette target:** Broken keel/spine line with missing chunks.
- **Shape language:** Angular beams, armor plates, support struts.
- **Materials:** Worn steel, graphite, off-white plate remnants.
- **Scale relation:** Highest ship ridge.
- **Readability distance:** Long.
- **Required variants:** Raised spine, collapsed spine segment.
- **Forbidden patterns:** Smooth toy ridge, decorative fin.
- **Quality gate:** Improves ship silhouette first.
- **Evidence required:** Neutral side silhouette.

### Object: Side cheek plates
- **Gameplay role:** Ship massing and orientation.
- **Visual role:** Prevents capsule read with angular industrial side mass.
- **Silhouette target:** Offset armor cheeks, vents, and cargo bulges.
- **Shape language:** Layered side armor, brackets, service panels.
- **Materials:** Off-white, graphite, worn steel.
- **Scale relation:** Multi-player-height.
- **Readability distance:** Spawn/mid.
- **Required variants:** Left/right asymmetrical plates.
- **Forbidden patterns:** Symmetric capsule, smooth pod.
- **Quality gate:** Breaks toy/capsule read.
- **Evidence required:** Front three-quarter PNG.

### Object: Scorch plates
- **Gameplay role:** Impact and danger storytelling.
- **Visual role:** Adds crash history without clutter.
- **Silhouette target:** Mostly material detail on thick plates.
- **Shape language:** Burned panel edges, heat streaks, soot.
- **Materials:** Brown/black scorch over human palette.
- **Scale relation:** Panel-scale.
- **Readability distance:** Close/mid.
- **Required variants:** Heavy burn, edge soot, blast patch.
- **Forbidden patterns:** Decal-only coverup of bad silhouette.
- **Quality gate:** Supports already-good shape; never hides toy form.
- **Evidence required:** Material PNG.

### Object: Buried lower hull
- **Gameplay role:** Grounds ship physically in basin.
- **Visual role:** Prevents floating prop read.
- **Silhouette target:** Hull disappearing into ash berms.
- **Shape language:** Ash buildup, crushed underside, embedded debris.
- **Materials:** Ash gray, off-white fragments, dark underside.
- **Scale relation:** Player foot to waist height around hull.
- **Readability distance:** Spawn/mid.
- **Required variants:** Nose burial, side burial, engine burial.
- **Forbidden patterns:** Ship sitting cleanly on top of plate.
- **Quality gate:** Contact shadows/geometry sell weight.
- **Evidence required:** Low-angle hull-ground PNG.

### Object: Loose panels
- **Gameplay role:** Debris scatter and path framing.
- **Visual role:** Human wreckage detail.
- **Silhouette target:** Thick bent plates with bolts and torn corners.
- **Shape language:** Trapezoids, triangles, curled edges.
- **Materials:** Off-white, graphite backfaces, scorch.
- **Scale relation:** Knee to player height.
- **Readability distance:** Close/mid.
- **Required variants:** Small, medium, large.
- **Forbidden patterns:** Paper-thin rectangles, random cubes.
- **Quality gate:** Each piece has thickness and orientation purpose.
- **Evidence required:** Debris field PNG.

### Object: Cable bundles
- **Gameplay role:** Visual guidance and wreckage detail.
- **Visual role:** Exposed systems and foreground depth.
- **Silhouette target:** Thick looping bundles, not hair-thin wires.
- **Shape language:** Grouped hoses, clamps, torn ends.
- **Materials:** Dark graphite rubber, steel clamps, small orange tags.
- **Scale relation:** Wrist to arm thickness.
- **Readability distance:** Close/mid.
- **Required variants:** Hanging, ground, torn bundle.
- **Forbidden patterns:** Spaghetti clutter, rope fantasy, excessive emissive lines.
- **Quality gate:** Enhances industrial read without hiding route.
- **Evidence required:** Close-up PNG.

### Object: Cargo fragments
- **Gameplay role:** Source context for resources/workbench.
- **Visual role:** Human survival salvage.
- **Silhouette target:** Broken crates, module racks, canisters.
- **Shape language:** Industrial containers with handles and latches.
- **Materials:** Graphite, off-white, steel, muted orange handles.
- **Scale relation:** Knee to chest height.
- **Readability distance:** Close/mid.
- **Required variants:** Crate fragment, canister, rack.
- **Forbidden patterns:** Loot-box toy, fantasy chest, branded real-world items.
- **Quality gate:** Supports resource collection without becoming new inventory system.
- **Evidence required:** Workbench/resource route PNG.

## HUMAN TECH

### Object: Workbench
- **Gameplay role:** Crafts Mechanical Arm Mk I.
- **Visual role:** Human industrial survival station.
- **Silhouette target:** Rectangular table, raised armature, orange handles, tool modules.
- **Shape language:** Heavy table, clamps, tool rail, service lights, salvage parts.
- **Materials:** Worn steel, graphite, off-white panels, muted orange interaction points.
- **Scale relation:** Waist-high table with above-head armature max.
- **Readability distance:** Spawn route and close-up.
- **Required variants:** Inactive/available, crafting feedback accent.
- **Forbidden patterns:** Random cube table, toy block, neon machine, fantasy forge.
- **Quality gate:** Readable from spawn route and close-up; clearly interactive without UI.
- **Evidence required:** Neutral/material PNGs, player scale view, interaction-state screenshot.

### Object: Save point
- **Gameplay role:** Supports local save continuation.
- **Visual role:** Compact human safety marker.
- **Silhouette target:** Waist-high station with grounded base and small light stack.
- **Shape language:** Rugged terminal, handhold, power cell, status light.
- **Materials:** Graphite, off-white, muted orange, small cyan/white state light.
- **Scale relation:** Waist to chest height.
- **Readability distance:** 10-25 meters.
- **Required variants:** Unsaved, saved confirmation.
- **Forbidden patterns:** Magical shrine, oversized teleporter, full checkpoint gate.
- **Quality gate:** Clearly a save terminal, not a weapon or crate.
- **Evidence required:** State screenshots and UI prompt check.

### Object: Beacon base
- **Gameplay role:** Final activation location.
- **Visual role:** Rescue hardware grounded in salvage.
- **Silhouette target:** Low triangular/rectangular base with stabilizers.
- **Shape language:** Tripod feet, bolted plates, cable drum, service panel.
- **Materials:** Worn steel, graphite, off-white, orange latches.
- **Scale relation:** Waist height base.
- **Readability distance:** Medium/long.
- **Required variants:** Inactive, powered.
- **Forbidden patterns:** Rocket pad, giant portal base.
- **Quality gate:** Supports beacon mast and final objective clarity.
- **Evidence required:** Beacon zone screenshots.

### Object: Beacon beam emitter
- **Gameplay role:** Visualizes rescue beacon activation.
- **Visual role:** Vertical final-goal signal.
- **Silhouette target:** Mast, dish, emitter prongs, controlled beam source.
- **Shape language:** Industrial antenna, ring clamps, lens housing.
- **Materials:** Graphite/steel with cyan/white active light.
- **Scale relation:** Taller than player but not monumental tower.
- **Readability distance:** Long.
- **Required variants:** Inactive, charging, active.
- **Forbidden patterns:** Full launch vehicle, giant neon pillar always on.
- **Quality gate:** Final objective visible without washing out scene.
- **Evidence required:** Inactive/active comparison PNGs.

### Object: Mechanical arm
- **Gameplay role:** Crafted first combat/progression tool.
- **Visual role:** Player’s salvaged industrial upgrade.
- **Silhouette target:** Chunky forearm rig with clamps, piston, claw/striker profile.
- **Shape language:** Assembled salvage plates, hydraulic lines, orange maintenance points.
- **Materials:** Graphite, worn steel, off-white patches, muted orange, tiny red warning if damaged.
- **Scale relation:** First-person arm scale; should feel attached to suit.
- **Readability distance:** First-person close view.
- **Required variants:** Unbuilt hint, equipped first-person, action pose.
- **Forbidden patterns:** Large mech arm, grappling hook, sleek superhero gauntlet.
- **Quality gate:** Communicates crafted Mk I utility without adding mechanics.
- **Evidence required:** First-person screenshots and gameplay smoke if integrated.

### Object: Player suit silhouette
- **Gameplay role:** First-person body/hand context where visible.
- **Visual role:** Human survivor identity.
- **Silhouette target:** Practical astronaut survival suit with damaged gloves/forearm.
- **Shape language:** Layered fabric/armor panels, seals, utility fasteners.
- **Materials:** Off-white suit, graphite undersuit, worn steel buckles, muted orange tabs.
- **Scale relation:** Player body.
- **Readability distance:** First-person/close.
- **Required variants:** Hands, mechanical-arm equipped view.
- **Forbidden patterns:** Superhero armor, giant mech, branded real-world suit.
- **Quality gate:** Supports FPS identity without scope expansion.
- **Evidence required:** First-person view PNG.

### Object: Modular crates
- **Gameplay role:** World dressing and resource context.
- **Visual role:** Human salvage modules.
- **Silhouette target:** Stackable rugged cases with handles and bevels.
- **Shape language:** Industrial corners, latches, reinforced strips.
- **Materials:** Graphite, off-white, worn steel, orange handles.
- **Scale relation:** Knee/waist/chest variants.
- **Readability distance:** Close/mid.
- **Required variants:** Small, medium, broken.
- **Forbidden patterns:** Toy blocks, loot crates with gamey glow.
- **Quality gate:** Adds industrial logic without hiding pickups.
- **Evidence required:** Workbench zone PNG.

### Object: Repair panels
- **Gameplay role:** Indicates salvage/repair context.
- **Visual role:** Human maintenance language.
- **Silhouette target:** Flat but thick service panels with handles.
- **Shape language:** Bolted plates, access hatches, warning strips.
- **Materials:** Off-white, graphite seams, steel, orange handles.
- **Scale relation:** Hand to torso sized.
- **Readability distance:** Close/mid.
- **Required variants:** Loose, attached, open.
- **Forbidden patterns:** Random colored rectangles, glossy sci-fi panels.
- **Quality gate:** Clearly serviceable industrial parts.
- **Evidence required:** Close material PNG.

### Object: Orange handles
- **Gameplay role:** Interaction affordance.
- **Visual role:** Consistent human survival touchpoints.
- **Silhouette target:** Small raised bars/loops.
- **Shape language:** Rounded-rect metal grips, pull tabs, latch levers.
- **Materials:** Muted orange worn paint over steel.
- **Scale relation:** Hand-sized.
- **Readability distance:** Close/mid on important objects.
- **Required variants:** Handle, latch, grip stripe.
- **Forbidden patterns:** Orange everywhere, toy plastic, non-interactive false positives.
- **Quality gate:** Orange usage maps to interaction/survival equipment.
- **Evidence required:** Palette review screenshot.

### Object: Warning lights
- **Gameplay role:** Danger, malfunction, or state feedback.
- **Visual role:** Small functional red accents.
- **Silhouette target:** Tiny inset lights, not decorative glow strips.
- **Shape language:** Rugged lenses, cages, panel indicators.
- **Materials:** Dark housing, red emissive lens.
- **Scale relation:** Hand-sized or smaller.
- **Readability distance:** Close/mid.
- **Required variants:** Off, blink/on, broken.
- **Forbidden patterns:** Full red neon scene, ambiguous objective glow.
- **Quality gate:** Red remains danger/weak core/warning only.
- **Evidence required:** Night/normal exposure screenshot if lit.

## RESOURCES

### Object: Metal pickup
- **Gameplay role:** Collectible resource for crafting.
- **Visual role:** Recognizable salvage metal bundle.
- **Silhouette target:** Compact angular scrap cluster with steel glint.
- **Shape language:** Bent plates, bolts, short rods, clamp band.
- **Materials:** Worn steel, graphite, tiny orange tag if needed.
- **Scale relation:** Small pickup at foot/knee scale.
- **Readability distance:** 5-15 meters.
- **Required variants:** Single pickup, small cluster.
- **Forbidden patterns:** Generic cube, coin, shiny loot gem.
- **Quality gate:** Distinct from electronics and biomass at pickup distance.
- **Evidence required:** Three-resource comparison PNG.

### Object: Biomass pickup
- **Gameplay role:** Collectible biological resource.
- **Visual role:** Alien organic contrast to human scrap.
- **Silhouette target:** Lumpy dark organic pod/shard.
- **Shape language:** Faceted tissue, ribbed membranes, small purple veins.
- **Materials:** Bruised purple, organic dark red, dark violet shell.
- **Scale relation:** Small pickup at foot/knee scale.
- **Readability distance:** 5-15 meters.
- **Required variants:** Pod, torn tissue sample.
- **Forbidden patterns:** Cute plant, fantasy mushroom, gore overload.
- **Quality gate:** Alien but readable and age-appropriate.
- **Evidence required:** Three-resource comparison PNG.

### Object: Electronics pickup
- **Gameplay role:** Collectible resource for crafting.
- **Visual role:** Small technological salvage.
- **Silhouette target:** Circuit module/cable spool/battery pack cluster.
- **Shape language:** Rectangular board, connectors, coiled wire, protective casing.
- **Materials:** Graphite, worn steel, tiny cyan powered dot if necessary.
- **Scale relation:** Small pickup.
- **Readability distance:** 5-15 meters.
- **Required variants:** Circuit pack, cable pack.
- **Forbidden patterns:** Generic glowing cube, full computer terminal.
- **Quality gate:** Distinct from metal by shapes, not just color.
- **Evidence required:** Three-resource comparison PNG.

### Object: Galaxabrain component
- **Gameplay role:** Specific post-Scout objective required for beacon activation.
- **Visual role:** Trophy-like biomechanical component.
- **Silhouette target:** Unique alien core fragment in broken shell cradle.
- **Shape language:** Carapace shard, neural node, restrained cyan/red pulse.
- **Materials:** Dark violet shell, bruised purple tissue, cyan energy, red inner core accent.
- **Scale relation:** Larger than common pickups; hand to forearm sized.
- **Readability distance:** Arena distance after combat.
- **Required variants:** Dropped, carried/collected UI representation.
- **Forbidden patterns:** Generic resource reuse, boss loot chest, magic orb.
- **Quality gate:** Clearly special without implying extra enemy systems.
- **Evidence required:** Defeated Scout/component screenshot.

## ALIEN TECH / ENEMY

### Object: Galaxabrain Scout
- **Gameplay role:** The one MVP enemy encounter.
- **Visual role:** Hostile biomechanical threat and contrast to human tech.
- **Silhouette target:** Low/medium asymmetric creature-machine with brain dome and armor shell.
- **Shape language:** Faceted organic mass, carapace plates, metal ribs, energy fins.
- **Materials:** Dark violet/black shell, bruised purple tissue, cyan powered accents, red weak core.
- **Scale relation:** Threatening but not boss-scale; roughly player chest/head height depending posture.
- **Readability distance:** Arena entry and combat range.
- **Required variants:** Alive, damaged/weak-core exposed, disabled/dead.
- **Forbidden patterns:** Multiple enemy types, cute mascot, giant boss, humanoid copyrighted likeness.
- **Quality gate:** Weak core and attack-facing direction readable in motion screenshots.
- **Evidence required:** Neutral silhouette, material turntable, combat screenshot, disabled-state screenshot.

### Object: Scout weak core
- **Gameplay role:** Combat readability / damage focus.
- **Visual role:** Red danger focal point.
- **Silhouette target:** Protected central lens/organ visible within shell opening.
- **Shape language:** Inset orb/organ with armored rim.
- **Materials:** Red emissive core, dark rim, organic dark red tissue.
- **Scale relation:** Small but targetable at combat distance.
- **Readability distance:** 8-20 meters.
- **Required variants:** Covered, exposed, defeated dim state.
- **Forbidden patterns:** Huge neon bullseye, hidden tiny dot, unrelated red decoration elsewhere.
- **Quality gate:** Red language is unique and readable.
- **Evidence required:** Combat-distance PNG.

### Object: Organic brain dome
- **Gameplay role:** Identifies Galaxabrain biology.
- **Visual role:** Memorable alien form.
- **Silhouette target:** Faceted semi-organic dome protected by shell.
- **Shape language:** Lobed polygonal brain mass, membrane ridges, partial casing.
- **Materials:** Bruised purple, dark red, translucent only if simplified and readable.
- **Scale relation:** Creature head/body focal area.
- **Readability distance:** Mid/close.
- **Required variants:** Intact, cracked/disabled.
- **Forbidden patterns:** Realistic gore, cartoon brain, comedy prop.
- **Quality gate:** Alien without tonal mismatch.
- **Evidence required:** Material close-up PNG.

### Object: Armor shell
- **Gameplay role:** Enemy silhouette and damage-state contrast.
- **Visual role:** Biomechanical carapace.
- **Silhouette target:** Sharp asymmetric plates wrapping organic core.
- **Shape language:** Ribbed shell, overlapping shards, metal-organic seams.
- **Materials:** Black/dark violet, graphite metal, ash dust.
- **Scale relation:** Creature body.
- **Readability distance:** Arena entry.
- **Required variants:** Intact, broken, dead.
- **Forbidden patterns:** Smooth toy shell, fantasy armor, clean robot.
- **Quality gate:** Hostile silhouette before glow.
- **Evidence required:** Neutral silhouette PNG.

### Object: Alien shard props
- **Gameplay role:** Arena/route dressing and contamination marker.
- **Visual role:** Shows alien technology invading crash site.
- **Silhouette target:** Jagged dark violet shards with irregular lean.
- **Shape language:** Carapace splinters, embedded fins, rib fragments.
- **Materials:** Dark violet, black, bruised purple, ash dust.
- **Scale relation:** Knee to player height.
- **Readability distance:** Mid.
- **Required variants:** Small, medium, embedded cluster.
- **Forbidden patterns:** Crystal forest expansion, collectible confusion, voxel spikes.
- **Quality gate:** Mood only; not mistaken for resources unless intended.
- **Evidence required:** Arena composition PNG.

### Object: Alien energy fins
- **Gameplay role:** Powered alien visual accent where relevant.
- **Visual role:** Selective cyan contrast.
- **Silhouette target:** Thin but readable fin/prong shapes attached to alien tech.
- **Shape language:** Curved/angular fins, embedded conduits.
- **Materials:** Dark shell with cyan/turquoise emissive edge.
- **Scale relation:** Hand to torso sized.
- **Readability distance:** Mid/close.
- **Required variants:** Off, active, broken.
- **Forbidden patterns:** Neon everywhere, sci-fi rave lighting, decorative fins on all props.
- **Quality gate:** Cyan means powered technology and remains restrained.
- **Evidence required:** Exposure-controlled material PNG.

### Object: Disabled/dead Scout read
- **Gameplay role:** Confirms combat completion and component recovery.
- **Visual role:** Clear defeated state without gore excess.
- **Silhouette target:** Collapsed shell, dimmed core, exposed component location.
- **Shape language:** Broken carapace, slumped brain dome, severed energy fins.
- **Materials:** Dark violet, dim cyan, dark red, ash dust.
- **Scale relation:** Same as Scout.
- **Readability distance:** Arena distance.
- **Required variants:** Recently defeated, component removed if needed.
- **Forbidden patterns:** Ambiguous alive pose, excessive gore, multiple corpse types.
- **Quality gate:** Player understands the fight is over and where to recover component.
- **Evidence required:** Post-combat screenshot.

## UI / READABILITY

### Object: HUD objective style
- **Gameplay role:** Communicates current objective.
- **Visual role:** Minimal survival-suit HUD compatible with world palette.
- **Silhouette target:** Clean text blocks and small icon accents.
- **Shape language:** Industrial brackets, off-white text, muted orange current objective marker.
- **Materials:** UI colors only; no excessive glow.
- **Scale relation:** Screen-space readable.
- **Readability distance:** N/A.
- **Required variants:** Resource collection, craft, fight, component, beacon, victory.
- **Forbidden patterns:** Lore-heavy UI, mobile-game clutter, neon cyber overload.
- **Quality gate:** Objective is short and route-readable.
- **Evidence required:** Screenshot for each objective state when UI changes.

### Object: Action feedback style
- **Gameplay role:** Confirms pickup, craft, hit, damage, save, activation.
- **Visual role:** Tactile survival equipment feedback.
- **Silhouette target:** Brief restrained flashes, icon pulses, small status messages.
- **Shape language:** Brackets, bars, small indicators.
- **Materials:** Orange for interaction, red danger, cyan powered states.
- **Scale relation:** Screen-space.
- **Readability distance:** N/A.
- **Required variants:** Pickup, craft success, hit, damage, component recovered, beacon active.
- **Forbidden patterns:** Fireworks, excessive particles, cartoon popups.
- **Quality gate:** Feedback readable but not visually noisy.
- **Evidence required:** State screenshots/video if available.

### Object: Interaction prompt style
- **Gameplay role:** Shows usable objects.
- **Visual role:** Minimal assistant-like suit prompt.
- **Silhouette target:** Small anchored prompt with icon + verb.
- **Shape language:** Simple bracket panel, no large tutorial box.
- **Materials:** Off-white text, muted orange key/action accent.
- **Scale relation:** Screen-space.
- **Readability distance:** N/A.
- **Required variants:** Workbench, save, beacon, pickup.
- **Forbidden patterns:** Text wall, fantasy scroll, glossy mobile button.
- **Quality gate:** Supports object readability without replacing it.
- **Evidence required:** Prompt screenshot per interactable if UI changes.

### Object: Victory/defeat presentation
- **Gameplay role:** End-state communication.
- **Visual role:** Clear mission result without expanding story.
- **Silhouette target:** Simple screen overlay with Crash Site result language.
- **Shape language:** Industrial HUD frame, restrained background fade.
- **Materials:** Off-white, graphite overlay, orange/cyan for victory, red for defeat.
- **Scale relation:** Screen-space.
- **Readability distance:** N/A.
- **Required variants:** Victory, defeat, saved-state continuation note if present.
- **Forbidden patterns:** Cinematic expansion, trailer-ready claims, lore dump.
- **Quality gate:** End state clear and aligned to MVP.
- **Evidence required:** End-state screenshots when UI changes.
