# TitanCraft Visual Review Checklist

Date: 2026-06-30
Purpose: binary approval checklist for every future visual phase after Phase 1.

Use this checklist before approving any visual PR. A future phase passes only when all applicable required checks are `PASS`, or a non-applicable item is explicitly marked `N/A` with a reason.

## Required review metadata

| Field | Value |
|---|---|
| Phase reviewed |  |
| Reviewer |  |
| Date |  |
| Scenes changed |  |
| Scripts changed |  |
| Materials changed |  |
| New assets |  |
| Screenshots attached | Yes / No |
| Manual playtest performed | Yes / No |

## Binary checklist

| Check | PASS/FAIL/N/A | Evidence required |
|---|---|---|
| Gameplay unchanged |  | Mission order, health, damage, crafting, save, and interaction values unchanged or explicitly approved. |
| Collision unchanged or safely equivalent |  | Critical route collision paths still resolve; player cannot be blocked by decorative geometry. |
| Interaction stable |  | Raycast interactions still work at existing `3.0 m` range for pickups, workbench, save point, beacon, and component. |
| Scale correct |  | Assets respect player capsule, corridor, pickup, enemy, workbench, and beacon scale standards. |
| Pivot correct |  | Floor props pivot bottom-center; panels/modules align to grid; gameplay root origins remain stable. |
| Human/alien distinction |  | Screenshot shows human industrial salvage and alien invasive technology are visually distinct. |
| Palette compliance |  | Colors match the visual bible palette or documented approved variant. |
| Orange usage |  | Orange primarily marks interaction, handles, survival, emergency, or construction systems. |
| Cyan/turquoise usage |  | Cyan/turquoise marks powered tech, alien energy/state, or justified beacon activation. |
| Material count |  | Asset material counts remain within budget and shared `.tres` resources are used. |
| Geometry budget |  | Triangle targets/hard maximums are met or justified for hero assets. |
| Collision complexity |  | No unnecessary dynamic trimesh collision; decorative details normally have no collision. |
| Lighting budget |  | Shadow-casting and local dynamic light counts remain within budget. |
| Glow/fog readability |  | Fog/glow do not hide pickups, enemy, workbench, beacon, or route. |
| VFX budget |  | Particle count, lifetime, visibility, looping/burst mode, and no-shadow policy meet budget. |
| UI readability |  | HUD/menu text readable at 720p and 1080p; critical text meets minimum size/contrast. |
| Cultural integrity |  | No masks, random tribal motifs, symbol dumping, generic pan-African mixing, or ornament-only cultural references. |
| Asset policy |  | Every external asset has source, creator, license, commercial/modification permission, attribution, local path, review approval, and `THIRD_PARTY_ASSETS.md` entry if retained. |
| Performance risk |  | Expected cost is documented; FPS measured when interactive environment is available. |
| Tests run |  | Required build/test/import commands executed and results pasted in PR. |
| Known warnings disclosed |  | Existing or new warnings are not hidden; new warnings require justification or fix. |
| Screenshots required |  | Before/after screenshots from standard positions are attached for visible scene/UI changes. |
| Manual playtest required |  | Main-route walkthrough, collision check, interaction check, combat/checkpoint check completed when applicable. |
| MVP scope preserved |  | No new map, enemy, weapon, multiplayer, procedural world, Blender dependency, or non-MVP system added. |

## Standard screenshot positions

Capture these when a future phase changes visible scenes:

1. Player spawn looking toward the crash/base landmark.
2. Resource pickup cluster from 5-10 m.
3. Workbench from interaction distance and from 10-15 m.
4. Save point from interaction distance.
5. Alien route looking toward crystals/enemy zone.
6. Galaxabrain Scout at threat-read distance before combat.
7. Beacon inactive from 15-30 m.
8. Beacon active after mission completion.
9. HUD at 720p.
10. HUD at 1080p.

## Approval rule

A visual phase is approved only if every applicable binary check is `PASS`, screenshots/manual checks are present when visible changes exist, and no Phase 0 gameplay-critical contract is broken.
