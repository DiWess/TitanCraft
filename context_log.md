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
