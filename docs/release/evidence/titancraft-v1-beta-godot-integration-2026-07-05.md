# TitanCraft V1 Beta Godot Integration Evidence — 2026-07-05

- Branch: work
- Base commit SHA before this change: 542c3103175b8efe1d49605d6000b5f03915be5c
- Gate target: V1_BETA_VISUAL_PASS_READY
- Task category: visual_scene_composition
- Primary agent: art_director
- Required memories: MEM-VISFAIL-001, MEM-VISFAIL-002, MEM-VISFAIL-004, MEM-VISFAIL-005
- Required skills: screenshot_critique, visual_art_direction, evidence_reporting
- Required evidence: live runtime PNG screenshots, opened-image diagnosis, playthrough proof, save/load proof, command results

## Files changed

- scenes/Main/Main.tscn
- scenes/World/Workbench.tscn
- scenes/World/Beacon.tscn
- scenes/Player/Player.tscn
- scenes/Enemies/GalaxabrainScout.tscn
- scenes/Resources/ResourceDrop.tscn
- tests/Integration/MvpPlaythroughCapture.cs
- artifacts/review/v1_beta_godot_integration_2026-07-05/README.md
- docs/release/evidence/titancraft-v1-beta-godot-integration-2026-07-05.md

## Scene files changed

- Crash Site live scene: scenes/Main/Main.tscn
- Workbench scene: scenes/World/Workbench.tscn
- Beacon scene: scenes/World/Beacon.tscn
- Player scene: scenes/Player/Player.tscn
- Galaxabrain Scout scene: scenes/Enemies/GalaxabrainScout.tscn
- Resource drop scene: scenes/Resources/ResourceDrop.tscn

## Assets integrated

- assets/models/v1_beta/TC_PROP_Workbench_01.gltf — added under Workbench/VisualBase.
- assets/models/v1_beta/TC_PICKUP_Component_01.gltf — added under GalaxabrainComponentPickup/VisualRoot.
- assets/models/v1_beta/TC_PLAYER_MechanicalArm_01.gltf — added under the script-critical MechanicalArmVisual node.
- assets/models/v1_beta/TC_PROP_Beacon_01.gltf — added under Beacon/VisualRoot.
- assets/models/v1_beta/TC_PROP_SavePoint_01.gltf — added under Placeholder_SavePoint/VisualRoot.
- assets/models/v1_beta/TC_CHAR_GalaxabrainScout_01.gltf — added under GalaxabrainScout/V1BetaScoutVisualRoot.
- assets/models/v1_beta/TC_PICKUP_Metal_01.gltf — added to ResourceDrop and enabled for the metal instance.
- assets/models/v1_beta/TC_PICKUP_EnergyCell_01.gltf — added to ResourceDrop and enabled for the electronics instance.
- assets/models/v1_beta/TC_ENV_CrashDebris_A_01.gltf — added as route-side Crash Site landmarks in StageAVisualRoot/RouteOpening.

## Node preservation notes

- Workbench script, StaticBody3D root, CollisionShape3D, InteractionZone, CraftAudio, and highlight mesh remain in place; the V1 model is a visual-only child.
- Component pickup remains the existing GalaxabrainComponentPickup Area3D, hidden and non-monitoring until the scout death path enables it; the V1 component is a child of that gated node.
- MechanicalArmVisual remains the existing MeshInstance3D that FirstPersonController toggles from inventory state; the V1 arm is a child so early hidden, crafted visible, and save/load visible states use the existing script contract.
- Beacon script, ClosedVisual, ActiveVisual, InteractionZone, CollisionShape3D, ActivationAudio, and extraction VFX remain in place; the V1 beacon is a visual-only child.
- SavePoint remains the existing Area3D with SavePoint.cs and Collision_SavePoint; the V1 save point is a visual-only child.
- Galaxabrain Scout remains the existing CharacterBody3D with GalaxabrainScout.cs, capsule collision, mission component path, and PlayerPath; the V1 scout is a visual-only child.
- ResourceDrop script, Area3D interaction collision, static physics body, highlight ring, particles, and audio paths remain in place; per-instance overrides only show the matching V1 metal/energy visuals.

## Collision notes

- No new CollisionShape3D or physics bodies were added for GLTF visual models.
- Existing gameplay collision and trigger nodes remain the authority for interaction, movement blocking, and pickup behavior.
- Crash debris V1 landmarks were placed beside the workbench/beacon route rather than on the player traversal line.

## Import warnings

- Godot import completed for V1 GLTF assets.
- Import/runtime environment warnings observed: X11 reported a missing libxkbcommon message but Vulkan llvmpipe rendering continued; audio fell back to dummy driver in xvfb. These did not block scene load, PNG capture, or playthrough completion.
- Legacy OBJ ambient-light PBR warnings appeared during import; no V1 GLTF import failures were observed.

## Screenshots path

artifacts/review/v1_beta_godot_integration_2026-07-05/

The PR diff now commits `README.md` in this directory instead of binary PNG files because the review surface reported binary-file support limitations. The manifest records the locally generated live PNG filenames, dimensions, byte sizes, SHA-256 hashes, capture command, and expected runtime marker. Required live runtime PNGs captured locally:

1. start-area-live.png
2. workbench-live.png
3. scout-encounter-live.png
4. component-pickup-live.png
5. mechanical-arm-visible-live.png
6. save-load-arm-persist-live.png
7. beacon-activation-live.png
8. victory-live.png

## Opened-image visual diagnosis

- start-area-live.png: first-person live scene shows the Crash Site route, workbench/beacon silhouettes in the distance, HUD objective state, and integrated V1 props visible in the actual runtime scene.
- workbench-live.png: the workbench area uses a readable low-poly V1 workbench silhouette with cyan/orange accents; the route remains open and existing collision blocks are still visible.
- scout-encounter-live.png: the Galaxabrain Scout is a large readable humanoid/mechanical silhouette; the gated component is visible after defeat and the route behind it remains navigable.
- component-pickup-live.png: HUD confirms the component is recovered after the pickup path; the component pickup was not granted from mission text alone.
- mechanical-arm-visible-live.png: V1 mechanical arm appears in first person after crafting; it was hidden before crafting and does not block the workbench interaction proof.
- save-load-arm-persist-live.png: V1 arm remains visible after continuation/reload, with HUD showing the recovered component and activate-beacon state.
- beacon-activation-live.png: beacon activation is visible with live VFX/HUD mission-complete state; beacon remains an orienting final objective.
- victory-live.png: runtime victory screen is displayed after beacon activation.

## Test results

- python3 tools/agent_preflight.py "TitanCraft close V1 beta visual integration gate" — PASS; routed visual_scene_composition with PNG evidence requirement.
- dotnet build — PASS before runtime capture.
- godot --headless --path . --import — PASS; V1 GLTF imports completed.
- xvfb-run -a godot --path . --scene res://tests/Integration/MvpPlaythroughCapture.tscn — PASS; emitted MVP_PLAYTHROUGH_CAPTURE_PASS and generated all required PNGs locally. Binary PNGs are represented in git by the diffable artifact manifest because binary files are not supported in the PR review surface.

Additional required commands are recorded in the final response.

## Playthrough result

PASS. The integration playthrough loaded the real Main scene, collected resources, crafted the mechanical arm, defeated the Galaxabrain Scout, recovered the component, saved, reloaded, activated the beacon, and displayed victory.

## Save/load result

PASS. The continuation/reload step restored the saved state and confirmed the mechanical arm visual remained visible from inventory/save state rather than being visible early from scene load alone.

## Performance notes

- V1 assets are static visual children with no added gameplay scripts, collision, heavy shaders, or dynamic material systems.
- Runtime capture completed under software Vulkan llvmpipe in xvfb, which is a conservative environment for a simple visual integration check.

## Known limitations

- Audio used the dummy driver during xvfb capture; audio is not part of this visual gate.
- The visual diagnosis is agent-side evidence, not a human art approval.
- The committed artifact is a hash manifest rather than binary PNG blobs due to the binary-file review limitation; rerun the capture command to regenerate exact local PNGs.
- Existing placeholder collision/primitive readability geometry is still present where it owns gameplay clarity.

## Final verdict

V1_BETA_VISUAL_PASS_READY
