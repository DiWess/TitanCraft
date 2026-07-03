# Visual Greybox Closure Tasks — Crash Site MVP

Owner: Codex
Version: 1
Date: 2026-07-03
Review status: TASK_BACKLOG_READY
Scope: documentation-only task backlog for closing the remaining visible greybox gaps in the single Crash Site MVP map.

## Purpose

This backlog translates the current visual-review findings into implementation tasks that can move the playable Crash Site scene from greybox/prototype presentation toward the README art target: simplified realistic science fiction, readable polygonal forms, coherent materials, and clear contrast between interactable and decorative elements.

The backlog does not authorize new gameplay features, new maps, new enemies, network services, procedural worlds, voxels, grappling, wall running, vehicles, grand mechs, or a complete rocket. Each task must preserve the existing mission order, collisions, save contracts, player controller, enemy behavior, HUD flow, and local offline MVP scope unless a separate human-approved gameplay task updates the README and tests.

## Working constraints for every visual task

- Keep `scenes/Main/Main.tscn` as the single playable Crash Site map.
- Add visual children under existing gameplay roots before renaming or replacing root nodes.
- Separate decorative geometry from gameplay collision.
- Preserve the existing interactable `Area3D` roots for pickups, workbench, save point, beacon, and mission component.
- Preserve the single `GalaxabrainScout` enemy archetype.
- Treat orange as the functional interaction color; keep cyan/violet alien emission rare and objective-readable.
- Use only project-owned or documented third-party assets with licence/source records in `THIRD_PARTY_ASSETS.md` before integration.
- Run the deterministic visual review capture after each visible scene slice and score the screenshots before continuing.

## Definition of done for “not a greybox”

Crash Site can be considered out of greybox only when all conditions below are met:

1. The rendered visual-review total is at least 48/70 with no category below 6/10.
2. Primary route screenshots show an authored crash site rather than flat floor plus primitives.
3. The ship wreck reads as damaged spacecraft debris from spawn and hero views.
4. Terrain has visible depth, route edges, foreground rocks, background ridges, and believable burial around wreckage/interactables.
5. Workbench, save point, beacon, resource pickups, and mission component have distinct silhouettes readable without relying only on color.
6. The Galaxabrain Scout has a biomechanical silhouette that does not read as a cube/capsule placeholder.
7. The first-person mechanical arm has an in-view visual when built or attacking, without adding forbidden abilities.
8. HUD and objective readability remain intact during gameplay.
9. `dotnet build`, applicable tests, Godot import, and `git status --short` have been run and reported.
10. A short human manual playtest confirms the route, objectives, interactables, combat readability, and victory flow remain understandable.

## Task backlog

| ID | Task | Target files / assets | Acceptance evidence | Dependencies | Scope guard |
|---|---|---|---|---|---|
| VG-01 | Authenticate the visual asset library selected for production use. | `THIRD_PARTY_ASSETS.md`, `artifacts/source-archives/README.md`, selected files under `assets/ThirdParty/` | Licence/source/format/hash record exists before scene use. | Human selection or already approved source. | No undocumented asset import. |
| VG-02 | Create a reusable visual scene structure for asset-led props. | New `scenes/Props/` child scenes or visual-only children under current roots | Godot import passes; no gameplay root renamed. | VG-01 | No internal framework; only simple reusable prop scenes where duplication would be risky. |
| VG-03 | Replace flat-ground dominance with a composed volcanic crash-site floor pass. | `scenes/Main/Main.tscn`, possible `scenes/Environment/` visual modules | Spawn and interactables-wide screenshots show route edges, slopes/rocks, and reduced flat plane visibility. | VG-01 | Keep `Ground/Collision_Ground` stable unless a separate collision task is approved. |
| VG-04 | Add background ridges and horizon blockers to remove empty test-map edges. | `scenes/Main/Main.tscn`, environment visual modules | Spawn overview and ship views show bounded terrain instead of sparse void/plane edges. | VG-03 | Visual-only; no new map or procedural terrain. |
| VG-05 | Build an authored crashed-ship wreck composition. | `scenes/Main/Main.tscn`, selected hull/engine/wreckage assets | Ship hero and oblique screenshots score at least 7/10 for ship silhouette. | VG-01, VG-03 | Wreck remains set dressing for the existing Crash Site loop; no flyable ship or complete rocket. |
| VG-06 | Dress the human crash-side boundary and C7 wall area. | `scenes/Main/Main.tscn`, wall/support/cable assets | Spawn overview reads as human crash infrastructure, not rectangular panels only. | VG-05 | Preserve `C7_Wall_*` collision contracts. |
| VG-07 | Replace decorative crate placeholders with salvage props. | `scenes/Main/Main.tscn`, prop scenes/assets | Crates no longer expose `Placeholder_` greybox presentation in screenshots. | VG-02 | Decorative only; do not add inventory/storage behavior. |
| VG-08 | Give resource pickups distinct authored silhouettes. | `scenes/Main/Main.tscn`, pickup visual children | Metal, biomass, and electronics are distinguishable by silhouette and color in first-person range. | VG-02 | Preserve `ResourceKind`, `Quantity`, root `Area3D`, and pickup collisions. |
| VG-09 | Give the workbench a production-readable crafting station silhouette. | `scenes/Main/Main.tscn` or `scenes/World/Workbench.tscn` | Workbench reads as a functional fabrication point from route distance. | VG-02 | Still only crafts Mechanical Arm Mk I. |
| VG-10 | Give the save point a readable local data/safe station silhouette. | `scenes/Main/Main.tscn` or a save-point visual scene | Save point is visually distinct from workbench and beacon without text-only dependence. | VG-02 | Preserve local save-only behavior. |
| VG-11 | Give the beacon a staged inactive/active rescue-communications silhouette. | `scenes/World/Beacon.tscn`, `scenes/Main/Main.tscn` | Beacon inactive/active states are visually distinct and final objective is clear. | VG-02 | No complete rocket, no remote/cloud service, no extra mission. |
| VG-12 | Add a visible Galaxabrain mission component pickup. | `scenes/Enemies/GalaxabrainScout.tscn` or component pickup scene | Component reads as special loot after enemy death and does not blend into generic electronics. | VG-02 | Preserve reveal-after-death and mission-component inventory contract. |
| VG-13 | Replace the Galaxabrain Scout placeholder body with an authored biomechanical visual assembly. | `scenes/Enemies/GalaxabrainScout.tscn`, enemy visual assets | Combat screenshot scores at least 7/10 for enemy silhouette; root, collision, AI, and stats unchanged. | VG-01 | Still one enemy type only. |
| VG-14 | Add minimal enemy damage/death visual feedback. | `scenes/Enemies/GalaxabrainScout.tscn`, existing enemy script only if needed | Hit/death states are readable in manual playtest and covered by applicable tests if behavior changes. | VG-13 | Visual feedback only; no new attack patterns or status effects. |
| VG-15 | Add first-person mechanical arm visuals. | `scenes/Player/Player.tscn`, possible arm visual scene/assets | Arm is visible in first-person when appropriate and supports basic attack readability. | VG-01 | No grappling, propulsion, shield, wall-run, or advanced construction ability. |
| VG-16 | Add minimal arm attack animation or visual cue. | `scenes/Player/Player.tscn`, player visual script only if needed | Attack timing remains 0.8s cooldown while the visual cue communicates the strike. | VG-15 | Do not move damage authority out of `MechanicalArmAttackLogic`. |
| VG-17 | Add focused environmental and interaction VFX. | New visual-only particle/material resources as needed | Collection, craft-ready, beacon active, and enemy hit moments are visually clearer in playtest. | VG-08 to VG-16 | No unknown-license VFX assets; no gameplay state driven by particles. |
| VG-18 | Harmonize materials and palette across human, alien, organic, and terrain assets. | `assets/Materials/`, relevant scene material overrides | Screenshots show coherent material families and reduced harsh/random contrast. | VG-03 to VG-17 | Preserve orange/cyan/violet readability rules. |
| VG-19 | Improve lighting and atmosphere for the single Crash Site map. | `scenes/Main/Main.tscn`, `WorldEnvironment`, light nodes | Screenshots have readable silhouettes, no excessive darkness, and stronger planet atmosphere. | VG-18 | Do not hide objectives with fog or post-processing. |
| VG-20 | Add deterministic visual-review scoring for the final pass. | `docs/phase3a-visual-review.md`, visual review artifacts | Updated score table reaches at least 48/70, with screenshot paths and verdict recorded. | VG-03 to VG-19 | Review artifacts are evidence only, not gameplay assets. |
| VG-21 | Update placeholder inventory after replacements. | `docs/visual-placeholder-inventory.md` | Remaining placeholders are documented honestly, or removed from the inventory if replaced. | Any replacement task | Do not claim final assets where placeholders remain. |
| VG-22 | Add or update tests for any changed behavior contracts. | `tests/`, affected scripts/scenes | Unit/integration tests pass for any altered node path, state, save, combat, or interaction behavior. | Any task that changes behavior or contracts | Pure visual-only additions may not need new tests, but must still run applicable tests. |
| VG-23 | Perform manual MVP route visual playtest. | `artifacts/mvp_closure/` or visual review doc | Human checklist confirms spawn, collect, craft, fight, loot, beacon, victory, and death/reload remain readable. | VG-20, VG-22 | Container cannot replace human feel/art review. |
| VG-24 | Final greybox closure verdict. | `docs/phase3a-visual-review.md`, `docs/visual-greybox-closure-tasks.md` | Verdict changes from backlog to `VISUAL_GREYBOX_CLOSED` only after all acceptance criteria are met. | VG-20 to VG-23 | Do not mark broader MVP, beta, or production GO. |

## Recommended execution order

1. Complete asset authentication and production-use approval: VG-01.
2. Establish visual-only prop/environment structure: VG-02.
3. Fix the scene foundation first: VG-03, VG-04, VG-05, VG-06.
4. Replace interactable placeholders: VG-07 through VG-12.
5. Improve hero gameplay silhouettes: VG-13 through VG-16.
6. Add polish passes: VG-17 through VG-19.
7. Close evidence and documentation: VG-20 through VG-24.

## Manual test procedure required after implementation slices

1. Start a new game from the main menu.
2. Confirm spawn view clearly communicates crash site, route direction, interactable cluster, and terrain boundaries.
3. Collect metal, biomass, and electronics; verify each pickup is visually distinct before interaction.
4. Craft Mechanical Arm Mk I at the workbench; verify the workbench and arm state are visually understandable.
5. Fight the Galaxabrain Scout; verify enemy silhouette, hit feedback, and death state are readable.
6. Pick up the Galaxabrain component; verify it is visually special and accessible.
7. Activate the beacon; verify inactive/active visuals communicate final objective completion.
8. Confirm victory screen appears.
9. Repeat a defeat/reload path if combat or save visuals were changed.

## Current status

This document is a task backlog only. No production visuals, gameplay behavior, collisions, asset imports, tests, or scene node contracts were changed by creating it.
