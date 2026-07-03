- 2026-06-29: Added first Crash Site playable interaction slice with resource pickups, workbench crafting, Galaxabrain component recovery, beacon activation, and mission sequence regression coverage.

- 2026-06-29: Added the first MVP Galaxabrain Scout enemy slice with deterministic state, placeholder scene, mission component reveal, and unit coverage.

- 2026-06-29: Added the minimal Crash Site combat path with mechanical arm attack gating, enemy-to-player damage, input binding, and combat progression tests.

- 2026-06-29: Made the MVP mechanical arm recipe data-backed from a single JSON recipe while preserving focused crafting behavior and unit coverage.

- 2026-06-29: Added README-required minimal UI screens for HUD, main menu, pause menu, victory, and defeat with integration scene-loading coverage.

- 2026-06-29: Connected the Crash Site HUD to player health, inventory, mission objective, interaction targeting, and mechanical arm state changes without moving gameplay decisions into UI scripts.

- 2026-06-29: Wired Crash Site victory and defeat events to end-screen navigation through a UI coordinator with integration coverage for both transition requests.

- 2026-06-29: Added local Crash Site save/load support for Continue, pause Save, and defeat reload using a versioned offline JSON save file.

- 2026-06-29: Hardened local save file writes and deletion for CI parity across Linux and Windows while preserving the MVP save/load flow.

- 2026-06-29: Fixed Crash Site mission progression ordering, mission change notifications, and save DTO compatibility so CI build and regression tests pass.

- 2026-06-29: Removed the gameplay quit action from Q, kept Escape pause as the clean exit path, and added integration coverage so left movement cannot trigger quit_game.

- 2026-06-29: Added the short Crash Site start tutorial HUD prompt with mission-progression dismissal and integration coverage.

- 2026-06-30: Routed mechanical arm hits through the GalaxabrainScout node damage path and covered mission component exposure on enemy death.

- 2026-06-30: Moved Galaxabrain Scout pickup exposure coverage out of runtime-dependent unit tests and into the Godot integration runner so CI unit tests stay runtime-free.

- 2026-06-30: Made the main menu the startup scene while preserving Crash Site loading and covering Continue save-state behavior in integration tests.

- 2026-06-30: Wired SavePoint interaction to the local Crash Site save coordinator and covered checkpoint serialization and restore behavior.

- 2026-06-30: Clarified defeat-screen reload fallback and hardened Crash Site checkpoint save loading for missing or invalid save data.

- 2026-06-30: Aligned resource pickup mission progression with the data-backed Mechanical Arm Mk I recipe cost and covered partial collection order regressions.

- 2026-06-30: Documented the local Windows offline MVP build export and manual validation process without claiming Windows readiness.

- 2026-06-30: Corrected Phase 3A C7 wall visuals with layered, readable industrial assemblies while preserving gameplay collisions.

- 2026-06-30: Reworked Phase 3A human wall panels away from flat box-only silhouettes with canted hull plates, cutaways, service conduits, and integration coverage for production C7 wall visual roots.

- 2026-06-30: Added direct HUD craft guidance and Mk I attack feedback so players can see why attacks are blocked, missed, or landed.

- 2026-06-30: Stabilized FPS mouse look with deterministic yaw/pitch math and softened Crash Site cubic placeholders using low-poly silhouettes and accent details.
- 2026-06-30: Added Phase 3A rendered screenshot harness and stopped after two visual correction cycles because final screenshots remained NOT_GO at 40/70.
- 2026-06-30: Ran two asset-based Phase 3A recovery passes with verified Kenney meshes; final screenshots stayed NOT_GO at 41/70 against the required 48/70 target.
- 2026-06-30: Terminated Phase 3A implementation work, rolled failed visual experiments out of production scenes, preserved the screenshot harness, and prepared asset-selection documents for human approval.
- 2026-07-01: Restored Phase 3A playable scene resource baseline, documented runtime/visual contracts, and added scene-contract diagnostics/tests before any future visual integration.
- 2026-07-01: Accepted the restored Phase 3A baseline, disabled the invalid complete-mech first-person visual, and completed terrain-only Pass 1 with contract tests and rendered review screenshots.
- 2026-07-01: Tightened Phase 3A runtime diagnostics to emit fail-closed contract flags and report the Galaxabrain baseline visual transform before controlled movement simulation.

- 2026-07-01: Added the Phase 3A Pass 1 GitHub Actions visual-artifact pipeline so terrain screenshots are regenerated and distributed as workflow artifacts instead of tracked PNG binaries.

- 2026-07-01: Removed the failed Phase 3A Pass 1 terrain production composition and added a terrain asset qualification audit/gallery workflow that marks the current rock placeholders invalid for production.

- 2026-07-01: Implemented Phase 3A Pass 1B deterministic procedural volcanic terrain around real Crash Site mission-route nodes with fail-closed terrain contracts and CI artifact capture support.

- 2026-07-01: Reworked Phase 3A Pass 1C procedural terrain from uniform noise into directed volcanic zones with route readability, terrain-isolation captures, and artifact workflow support.

- 2026-07-01: Added Phase 3A Pass 1D terrain visibility diagnostics to separate geometry, material, lighting, vertex-colour and production-camera obstruction failures without changing gameplay ownership.

- 2026-07-01: Replaced the visually insufficient Phase 3A terrain height field with semantic visual-only low-poly landform meshes and Pass 1E artifact diagnostics while preserving gameplay contracts.
- 2026-07-03: Added self-created temporary MVP audio cues for collection, crafting, combat, death, beacon activation, and victory feedback.
- 2026-07-03: Moved temporary MVP audio WAV binaries out of git and into deterministic CI/local asset preparation.

- 2026-07-03: Improved Crash Site interactable readability with visual-only marker meshes while preserving gameplay node paths and validation checks.

- 2026-07-03: Refined MVP pickup material contrast and added scene-contract coverage for visual-only readability markers without renaming gameplay nodes.
- 2026-07-03: Décision MVP explicite — la sauvegarde Crash Site persiste la position du joueur mais pas le yaw/pitch caméra; aucune modification du contrat de données nécessaire.

- 2026-07-03: Added repository-owned simulated Windows validation evidence while keeping human Windows gameplay feel validation explicitly open.
- 2026-07-03: Authenticated the first visual-only Crash Site asset subset and added isolated prop/environment structure scenes for future greybox closure without touching production gameplay scenes.
- 2026-07-03: Instanced the authenticated visual-only rock and wreckage props into the production Crash Site scene, preserved gameplay collision contracts, and captured deterministic Phase 3A review screenshots.
- 2026-07-03: Fixed the visual prop CI failure by replacing rejected terrain OBJ references in the production-instanced rock cluster with authenticated non-rejected rock meshes and re-running CI plus Phase 3A screenshot validation.
