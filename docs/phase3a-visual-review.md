# Phase 3A Visual Review

Owner: Codex
Version: 1
Date: 2026-06-30
Review status: NOT_GO after rendered review

## Scope

Phase 3A visual correction only. This review harness captures deterministic screenshots of the real `scenes/Main/Main.tscn` game viewport without changing the project startup scene, gameplay logic, collision, save data, mission flow, enemy AI, UI design, audio, VFX, or Phase 3B scope.

## Rendering method

Use Godot through `xvfb-run` with the OpenGL renderer when no native display is available:

```bash
xvfb-run -a godot --path . --rendering-driver opengl3 --script tools/visual_review/capture_phase3a.gd
```

Direct Godot execution without a display is expected to fail in the container; `xvfb-run` provides the virtual display for rendered game-viewport screenshots.

## Harness files

- `scenes/Debug/VisualReview/Phase3AVisualReview.tscn`
- `tools/visual_review/capture_phase3a.gd`

## Screenshot outputs

Generated screenshots are review artifacts, not gameplay assets. They are written locally under `artifacts/visual-review/` and should not be wired into gameplay.

## Fixed camera views

| View | Position | Target | FOV | Output |
|---|---:|---:|---:|---|
| Spawn overview | `(0.0, 2.0, 4.5)` | `(-3.5, 1.2, -16.0)` | `72` | `phase3a_spawn_overview.png` |
| Ship hero | `(-12.5, 2.6, -14.0)` | `(-2.5, 1.3, -18.5)` | `64` | `phase3a_ship_hero.png` |
| Ship oblique | `(5.5, 2.5, -29.0)` | `(-2.0, 1.2, -18.0)` | `64` | `phase3a_ship_oblique.png` |
| Galaxabrain combat | `(15.0, 2.0, -12.0)` | `(20.0, 1.0, -16.0)` | `50` | `phase3a_galaxabrain_combat.png` |
| Interactables wide | `(2.0, 5.0, 8.0)` | `(2.0, 0.9, -10.5)` | `74` | `phase3a_interactables_wide.png` |

## Review log

To be filled after capture and screenshot inspection.

## Renderer capability investigation

- Direct execution command: `godot --path . --quit --rendering-driver opengl3`
  - Result: failed without a display server. Godot reported `X11 Display is not available`, then Wayland connection failure.
- Virtual display command: `xvfb-run -a godot --path . --quit --rendering-driver opengl3`
  - Result: succeeded after installing the missing runtime display libraries in the container. Godot reported OpenGL 4.5 through Mesa llvmpipe.
- Capture command used for review:

```bash
xvfb-run -a godot --path . --rendering-driver opengl3 --script tools/visual_review/capture_phase3a.gd
```

## First rendered pass diagnosis

Screenshots captured at 1280×720 under `artifacts/visual-review/`.

| View | Diagnosis |
|---|---|
| Spawn overview | HUD and prompt obstructed the screenshot, ship existed but still read as a flat primitive assembly, terrain remained a flat plane, and focal hierarchy was weak. |
| Ship hero | Camera was blocked by a C7 wall/interactable structure, so the view did not prove ship quality. |
| Ship oblique | Revealed depth, but the wreck still read as a large flat box/slab with harsh black material loss. |
| Galaxabrain combat | Enemy was partially hidden by foreground/world objects and still read as a small box-bodied primitive creature. |
| Interactables wide | Interactables were visible, but most depended on color and simple primitives; terrain did not yet feel like a game environment. |

### First-pass scores

| Category | Score |
|---|---:|
| Ship silhouette | 4/10 |
| Galaxabrain silhouette | 4/10 |
| Terrain depth | 3/10 |
| Interactable readability | 5/10 |
| Material coherence | 5/10 |
| Non-cubic appearance | 3/10 |
| Overall presentation | 4/10 |

Overall: 28/70. Gate failed.

## Correction cycle 1

Changes:

- Fixed the capture harness to hide CanvasLayer/Control review obstructions.
- Reframed ship and Galaxabrain cameras away from major occluders.
- Demoted the primitive ship hull box and added existing repo Kenney hull/cable/cliff meshes.
- Hid the enemy cube body and added a larger thorax/mandible hierarchy.

Result: improved screenshot cleanliness and object separation, but ship still presented a dominant flat slab, Galaxabrain was still occluded by alien crystals from the combat camera, and terrain/interactables remained too primitive.

### Cycle 1 scores

| Category | Score |
|---|---:|
| Ship silhouette | 5/10 |
| Galaxabrain silhouette | 5/10 |
| Terrain depth | 4/10 |
| Interactable readability | 5/10 |
| Material coherence | 5/10 |
| Non-cubic appearance | 4/10 |
| Overall presentation | 5/10 |

Overall: 33/70. Gate failed.

## Correction cycle 2

Changes:

- Replaced the dominant flat ship slab with a faceted cylindrical fuselage and nose-cone hierarchy.
- Reduced the old primitive nose/wing dominance while keeping asymmetric damage and engine forms visible.
- Reframed the Galaxabrain combat camera to show the enemy directly at combat distance.
- Enlarged the Galaxabrain thorax, brain dome, sensor, and legs without changing gameplay root/collision contracts.
- Lowered terrain ridge masses to reduce block-wall appearance.

### Final rendered diagnosis

| View | Diagnosis |
|---|---|
| Spawn overview | Clean screenshot with no HUD/pause UI. Major human structures, ship, interactable cluster, and terrain context are visible. The route is readable, but the ground still reads too flat and sparse. |
| Ship hero | Ship has clearer cylindrical body/engine language and less HUD obstruction, but it still feels assembled from visible primitive chunks rather than authored spacecraft wreckage. |
| Ship oblique | Oblique view proves depth and asymmetry. Cylinders, nose cone, engines, and torn parts are visible; however, material contrast is harsh and forms are still prototype-quality. |
| Galaxabrain combat | Enemy is now readable at combat distance with body mass, legs, organic dome, armor/sensor hierarchy, and no dominant cube body. Some nearby props/crystals still compete with the silhouette. |
| Interactables wide | Pickups, workbench, save point, beacon, terrain ridges, enemy route, and ship are visible. Interactables remain distinct but still rely heavily on primitive shape/color language. |

### Final scores

| Category | Score |
|---|---:|
| Ship silhouette | 6/10 |
| Galaxabrain silhouette | 6/10 |
| Terrain depth | 5/10 |
| Interactable readability | 6/10 |
| Material coherence | 6/10 |
| Non-cubic appearance | 5/10 |
| Overall presentation | 6/10 |

Overall: 40/70. Gate failed.

## Final Phase 3A visual verdict

`NOT_GO`: Phase 3A is not visually accepted yet. Rendering is available and screenshots were inspected, but after two bounded correction cycles the scene still reads too much like a sparse editor/test level. The largest remaining issues are terrain flatness, primitive interactable language, and ship material/form cohesion. Further progress likely requires a dedicated art-direction pass or a more asset-led composition slice, not additional blind primitive iteration in Phase 3A.

## Asset-based recovery after primitive-cycle NOT_GO

Decision date: 2026-06-30
Status: `NOT_GO`

### Imported assets used

Only already documented Kenney CC0 assets were used:

- `assets/ThirdParty/Kenney/ModularSpaceKit/Models/corridor-end.obj`
- `assets/ThirdParty/Kenney/ModularSpaceKit/Models/corridor-corner.obj`
- `assets/ThirdParty/Kenney/ModularSpaceKit/Models/cables.obj`
- `assets/ThirdParty/Kenney/ModularSpaceKit/Models/template-wall-detail-a.obj`
- `assets/ThirdParty/Kenney/NatureKit/Models/cliff_large_rock.obj`
- `assets/ThirdParty/Kenney/NatureKit/Models/cliff_topDiagonal_rock.obj`
- `assets/ThirdParty/Kenney/NatureKit/Models/cliff_blockSlope_rock.obj`
- `assets/ThirdParty/Kenney/NatureKit/Models/cliff_cornerInner_rock.obj`
- `assets/ThirdParty/Kenney/NatureKit/Models/cliff_half_rock.obj`

No new asset pack was downloaded. No Quaternius or deferred pack was used.

### Asset recovery changes

- Terrain: added imported foreground, route-edge, crash-lip, hero-frame and background rock/cliff visual-only meshes. These have no added collision.
- Interactables: hid dominant primitive visual children under the existing pickup/workbench/save/beacon roots and added imported visual-only module/cable/cliff children. Roots, scripts, quantities, colliders and gameplay values were preserved.
- Crashed ship: hid the dominant primitive ship visuals and added imported modular sci-fi pieces as the dominant wreck forms with asymmetric orientations and buried terrain context.
- Capture framing: adjusted the ship hero and oblique cameras to show the imported wreck composition more consistently.

### Asset-based comparison table

| View | Previous failure | New change | Final score |
|------|------------------|------------|-------------|
| Spawn overview | Flat plane dominated the composition; ship and interactables looked primitive. | Added route-edge rocks, background ridges, imported interactable wrappers and imported ship modules. | 6/10 |
| Ship hero | Ship read as primitive cylinders/slabs and the hero angle over-emphasized one large flat form. | Reframed camera, reduced primitive ship visibility, made imported oriented hull pieces dominant. | 6/10 |
| Ship oblique | Oblique view proved depth but still looked like primitive chunks. | Added imported hull/nose/engine/interior pieces and asymmetric layout. | 6/10 |
| Galaxabrain combat | Enemy was acceptable but competed with surrounding primitives and beacon clutter. | Left Galaxabrain unchanged; distant beacon/interactable context changed. | 6/10 |
| Interactables wide | Workbench/save/beacon/pickups depended heavily on primitive shape and color. | Added imported module/cable/cliff wrappers under existing roots and enlarged wrappers in pass 2. | 6/10 |

### Asset-based scores

| Category | Before asset recovery | Final asset recovery |
|---|---:|---:|
| Ship silhouette | 6/10 | 6/10 |
| Galaxabrain silhouette | 6/10 | 6/10 |
| Terrain depth | 5/10 | 5/10 |
| Interactable readability | 6/10 | 6/10 |
| Material coherence | 6/10 | 6/10 |
| Non-cubic appearance | 5/10 | 6/10 |
| Overall presentation | 6/10 | 6/10 |

Final total: 41/70. Required total: 48/70.

### Final asset-based diagnosis

The recovery did not meet the required visual target. The imported Kenney subset is verified and useful, but in this specific production scene it still reads as a small set of repeated slab-like modules once texture maps are absent and the existing flat ground remains visible. The terrain masks do not sufficiently bury the flat plane in the required camera views, and the interactable wrappers remain visually subordinate to the old rectangular C7 structures. Further progress should not continue by adding more primitives or more random module scattering. It needs either a dedicated asset-led layout pass with a broader verified environment/industrial prop set, or human art direction selecting a smaller number of purpose-built placeholder assets.

Final verdict: `NOT_GO` due to insufficient asset match for the Phase 3A target, not due to rendering failure.


## Phase 3A production rollback and asset-selection gate

Decision date: 2026-06-30
Status: `READY_FOR_ASSET_SELECTION`

### Cleanup decision

Phase 3A implementation work is terminated. The primitive recovery and the Kenney asset recovery both remained `NOT_GO`; no third primitive-only or asset-subset composition pass is authorized. The screenshot harness, deterministic cameras, review command, proof documentation, asset policy and third-party asset records remain mandatory infrastructure for future visible work.

### Production rollback proof

The live production scenes were surgically restored to the last stable pre-Phase-3A gameplay presentation instead of using a blind hard reset.

Removed from `scenes/Main/Main.tscn`:

- Primitive C7 `VisualRoot` wall-kit assemblies, canted panels, conduits and added hull plates.
- Primitive crashed-ship composition nodes and terrain-ridge visual-only additions.
- Primitive interactable wrappers for workbench, save point, beacon and pickups.
- Kenney imported mesh wrappers added directly to production terrain, interactables and wreck composition.
- Imported terrain masks/framing meshes and hidden/demoted primitive visual overrides.

Removed from `scenes/Enemies/GalaxabrainScout.tscn`:

- Primitive multi-part Galaxabrain visual assembly, enlarged brain-dome/thorax details, non-gameplay visual children and extra visual-only materials.

Preserved production nodes and contracts in `scenes/Main/Main.tscn`:

- `Ground` and `Ground/Collision_Ground`.
- `Player` instance and player transform.
- `C7_Wall_1` through `C7_Wall_4`, including `MeshInstance3D`, `OrangeVent` and `Collision_C7Wall` children.
- `VolcanicRock_1` through `VolcanicRock_3` and each `Collision_BlockingRock`.
- `Placeholder_Crate1` through `Placeholder_Crate3` and each `Collision_Crate`.
- `Placeholder_MetalPickup`, `Placeholder_BiomassPickup`, `Placeholder_ElectronicsPickup`, their `ResourcePickup` scripts, visual mesh children and collision children.
- `Placeholder_Workbench`, its `Workbench` script, `ControlPanel` child and `Collision_Workbench`.
- `Placeholder_SavePoint`, its `SavePoint` script and `Collision_SavePoint`.
- `Placeholder_Beacon`, its `Beacon` script, `ClosedVisual`, `ActiveVisual` and `Collision_BeaconBase`.
- `Placeholder_GalaxabrainScout` instance transform.
- HUD binder, end navigation and save coordinator nodes/scripts.

Preserved production nodes and contracts in `scenes/Enemies/GalaxabrainScout.tscn`:

- Root `CharacterBody3D` named `GalaxabrainScout`.
- `src/Enemies/GalaxabrainScout.cs` script reference.
- Root collision shape and configured combat/export values.
- `Placeholder_GalaxabrainScout` visual mesh child.
- `GalaxabrainComponentPickup` Area3D, `src/World/GalaxabrainComponentPickup.cs` script and pickup collision child.
- `MissionComponentPath = NodePath("GalaxabrainComponentPickup")`.

### Baseline/reset screenshot evidence

After rollback, the screenshot harness must still be run with:

```bash
xvfb-run -a godot --path . --rendering-driver opengl3 --script tools/visual_review/capture_phase3a.gd
```

The generated images under `artifacts/visual-review/` are labeled baseline/reset evidence only. They are not final art evidence and do not claim Phase 3A visual acceptance.

### Asset-selection handoff

Asset selection now lives in:

- `docs/phase3a-asset-requirements.md`
- `docs/phase3a-asset-shortlist.md`
- `docs/phase3a-selection-scorecard.md`

No new assets may be downloaded, purchased, imported or integrated until a human explicitly selects an acquisition option.
