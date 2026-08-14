# Comorian Visual Ceiling & Crash Site Gameplay Lock

**Date:** 2026-08-14  
**Authority:** Project Director / Agent Studio  
**Status:** READY FOR FINAL AGENT GATE

## Purpose

Push the Crash Site MVP to its visual ceiling **with a clear Comorian feel**, then lock the gameplay loop so the experience stays finished and fantastic under the agent primary gate.

This document does **not** expand MVP scope, add new assets, or authorize Stage A/B/C re-runs.

## Visual direction reminder

Canonical identity: **Polygonal Comorian Afro-Futurist Salvage Sci-Fi** (`docs/art/titancraft-visual-identity.md`, `docs/visual-style.md`).

Comorian / Indian Ocean character is expressed only through:

- Heat of the human salvage base (warm functional orange lamps, warm ivory / bronze / graphite industrial language)
- Mineral volcanic island palette (basalt, ash, scorch, sunlit rock edges)
- Strong moon + distant horizon / atmosphere
- Geometric modular human forms vs asymmetrical alien contrast
- Restrained emission hierarchy (no neon spam)

**Forbidden:** generic savanna, copied real cultural symbols, photorealism, glossy toy sci-fi, scope expansion.

## Visual ceiling applied (2026-08-14)

Within existing meshes and materials only:

1. **Sky / atmosphere** — warmer, more saturated horizon (Indian Ocean dusk / volcanic island heat) against a deeper indigo upper sky so the base reads as a warm salvage camp under an alien sky.
2. **Ambient + fog** — cooler indigo ambient fill + warmer mineral fog so human heat and alien cyan/violet accents both read clearly.
3. **Key light + base lamps** — slightly stronger warm directional key and base-camp lamps to emphasize the "heat of the base" without changing light count or collision.
4. **Emission** — left under material-library rules (alien violet/cyan controlled; human orange restrained).

These are visual-only value tweaks in `scenes/Main/Main.tscn` (Environment + DirectionalLight3D + BaseLamp_*). No new meshes, no scene hierarchy changes, no collision changes, no gameplay code changes.

**Exact values locked:**
- `sky_top_color = Color(0.05, 0.04, 0.16, 1)`
- `sky_horizon_color = Color(0.58, 0.28, 0.14, 1)`
- `ambient_light_color = Color(0.20, 0.16, 0.34, 1)`
- `fog_light_color = Color(0.22, 0.13, 0.12, 1)`
- DirectionalLight: `Color(1.0, 0.72, 0.48, 1)`, energy `1.22`
- BaseLamp_*: `Color(1.0, 0.52, 0.22, 1)`, energy `1.35`

## Gameplay lock (Crash Site MVP)

The Crash Site loop is **locked**:

1. Spawn → collect Metal / Biomass / Electronics  
2. Craft Mechanical Arm at Workbench  
3. Defeat Galaxabrain Scout  
4. Recover Component  
5. Activate Beacon → Victory  
6. Save / restore remains functional

**Locked:** core values (damage, cooldowns, resource quantities, mission FSM transitions, enemy AI states, interaction ranges) unless an explicit exception is recorded.

**Allowed under Fast Lane only:** bug fixes, juice/feedback (shake, FOV, prompts, volumes), HUD text, documentation that does not authorize new scope.

**Requires full Agent Studio route + agent playtest journey:** any structural, collision, new system, new asset stage claim, or release claim.

**Feel adjectives** still require a dated human note if used in any verdict (per ADR).

## Agent gate

Primary ship/release gate remains the agent-gated Windows playtest journey (`windows-playtest-journey.yml` + Visual Reviewer + validator). Human playtest is optional enrichment only.

## Success criteria for this slice

- [x] Comorian atmospheric direction applied within style bible  
- [x] Visual-only (no gameplay / collision / light-count change)  
- [x] Gameplay lock documented and active  
- [ ] Full Main.tscn restored on main (assemble the two chunks on this branch — see RESTORE_FROM_CHUNKS.md)  
- [ ] Fresh agent playtest journey captures under the new ceiling  
- [ ] Visual Reviewer opens new PNGs and records aesthetic verdict  

## Relationship to prior polish

Builds on Stage A/B/C sign-off, polish-slice-2026-07-18, and polish-mode.md. Does not reopen closed MVP acceptance criteria.

**Next action to close MVP 100%:**
1. Assemble the two chunks into `scenes/Main/Main.tscn`
2. Merge this branch to main
3. Re-trigger `windows-playtest-journey`
4. Record the final aesthetic verdict from the new captures
