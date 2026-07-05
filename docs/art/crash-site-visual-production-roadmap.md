# Crash Site Visual Production Roadmap

Owner: TitanCraft Project Director / Visual Director  
Version: 1.0  
Date: 2026-07-05  
Review status: Roadmap ready for future PR planning; not implementation approval

## Roadmap principles

- Each PR must be small enough to review independently.
- Do not modify gameplay code unless a future task explicitly permits a gameplay-safe visual integration need.
- Do not modify `Main.tscn` unless that future PR explicitly scopes scene work and includes gameplay smoke evidence.
- Do not commit generated PNG review artifacts or unapproved binary art outputs.
- Passing checks proves process only, not final visual approval.
- Every visual verdict must be separated from implementation/build verdict.

## PR 1: Terrain/world foundation replacement plan

- **Scope:** Author detailed brief and prototype plan for crash basin ground, ash floor, fractured terrain rim, basalt route markers, and foreground occluders.
- **Files likely touched:** `docs/art/*terrain*`, asset briefs under docs, future Blender source only if explicitly scoped.
- **Forbidden scope:** No gameplay code, no procedural world, no voxel terrain, no multiple maps, no scene replacement without approval.
- **Evidence required:** Terrain brief, route-composition sketch/contact sheet if prototyped, before/after screenshots if scene changes.
- **Validation commands:** `python3 tools/agent_preflight.py "<exact PR task>"`, `python3 tools/validate_agent_studio.py`, `git diff --check`, plus `godot --headless --path . --import` and gameplay smoke if scenes change.
- **Visual gate:** Terrain must read shaped, not plate-like; route readability must improve without UI dependency.

## PR 2: Crash hull hero asset brief + prototype

- **Scope:** Create/complete asset brief for main hull, crushed nose, rear engine assembly, torn wing panels, broken ribs, breach, exposed interior, torn spine, cheek plates, scorch, buried lower hull, loose panels, cables, cargo fragments.
- **Files likely touched:** `docs/pipeline/asset-brief-*`, `docs/art/*crash-hull*`, future `assets/Source/Blender/...` only if explicitly implementing a reviewed candidate.
- **Forbidden scope:** No `Main.tscn`, no gameplay code, no final scene insertion, no capsule/toy silhouette preservation by decals.
- **Evidence required:** Neutral silhouette PNGs, material PNGs, source/provenance/hash, manifest if exported.
- **Validation commands:** Blender validation/export/import/manifest commands when asset is implemented; `git diff --check` for docs-only brief.
- **Visual gate:** Neutral silhouette reads as heavy crashed industrial ship before materials.

## PR 3: Workbench/save/beacon interactable redesign

- **Scope:** Redesign human interactable visual language for workbench, save point, beacon base, and beacon emitter.
- **Files likely touched:** Asset briefs, Blender source/output manifests if scoped, scene files only in an explicit integration PR.
- **Forbidden scope:** No new crafting system, no rocket, no teleporter, no giant launch infrastructure.
- **Evidence required:** Turntables, scale-reference PNGs, inactive/active state screenshots if integrated.
- **Validation commands:** Asset Forge commands for implemented assets; Godot import and gameplay smoke if scene files change.
- **Visual gate:** Objects must be interactive without UI, with orange/cyan used according to palette rules.

## PR 4: Resource pickup redesign

- **Scope:** Redesign metal, biomass, electronics, and Galaxabrain component pickups with silhouette-first readability.
- **Files likely touched:** Asset briefs, pickup asset sources/manifests if implemented, UI docs if icons are scoped.
- **Forbidden scope:** No inventory rewrite, no extra resources, no loot rarity system.
- **Evidence required:** Three-resource comparison PNG, component pickup PNG, source/provenance/hash.
- **Validation commands:** Asset validation/export/import if implemented; gameplay pickup smoke if scene/runtime resources change.
- **Visual gate:** All pickup types are distinguishable at gameplay distance by silhouette and material, not generic glow.

## PR 5: Galaxabrain Scout redesign brief

- **Scope:** Create production-ready brief for the single MVP Scout: body, weak core, brain dome, armor shell, alien shards/fins, disabled state.
- **Files likely touched:** `docs/art/*galaxabrain*`, asset brief docs, future Blender assets only if explicitly scoped.
- **Forbidden scope:** No additional enemy types, boss, new attacks, expanded AI, or gore-heavy style.
- **Evidence required:** Neutral silhouette, material turntable, combat-distance weak-core mockup, disabled-state view.
- **Validation commands:** Docs validation for brief; Asset Forge/Godot checks if implemented.
- **Visual gate:** Scout reads hostile and biomechanical; weak core is readable but not cartoon bullseye.

## PR 6: Mechanical Arm first-person visual pass

- **Scope:** Brief and/or implement a visual pass for the crafted Mechanical Arm Mk I first-person read.
- **Files likely touched:** First-person arm asset docs/assets/scenes only if explicitly scoped.
- **Forbidden scope:** No grappling hook, no wall run, no large mech, no extra weapons, no gameplay changes unless explicitly requested.
- **Evidence required:** First-person screenshots before/after, action pose, scale/material notes.
- **Validation commands:** Godot import/build/gameplay smoke if integrated; relevant tests if gameplay-facing references change.
- **Visual gate:** Reads as salvaged industrial Mk I tool attached to the survivor suit.

## PR 7: Scene composition / route readability pass

- **Scope:** Integrate approved visual pieces or proxy composition improvements to clarify spawn, resource trail, workbench, hull, arena, component, save, beacon, and background.
- **Files likely touched:** Scene composition docs; scene files only with explicit permission.
- **Forbidden scope:** No new map, new biome, new routes beyond locked loop, or unreviewed production art drop.
- **Evidence required:** Before/after contact sheet, opened PNG critique naming focal point/route/silhouette/scale/material coherence, gameplay smoke if `Main.tscn` or gameplay scene files change.
- **Validation commands:** `godot --headless --path . --import`, gameplay smoke/test commands relevant to touched files, `git diff --check`.
- **Visual gate:** First/second/third/fourth hierarchy works from player-height screenshots.

## PR 8: Lighting/material pass

- **Scope:** Tune key/fill/rim direction, exposure, material roughness, palette discipline, and glow restraint.
- **Files likely touched:** Lighting docs, material docs, scene/environment resources only if explicitly scoped.
- **Forbidden scope:** No over-neon style, no photoreal scan material pivot, no glossy plastic rewrite.
- **Evidence required:** Before/after screenshots from validation angles, exposure notes, palette audit.
- **Validation commands:** Godot import and scene smoke if scene/resources change; `git diff --check`.
- **Visual gate:** Readable silhouettes with no flat darkness and no overbright toy lighting.

## PR 9: Screenshot review artifact pass

- **Scope:** Improve or document visual artifact factory captures/contact sheets for Crash Site review.
- **Files likely touched:** Visual review tooling/docs and allowlisted capture scripts if scoped.
- **Forbidden scope:** No claiming screenshot generation equals approval, no committed PNG artifacts, no arbitrary script execution.
- **Evidence required:** Downloadable artifact bundle paths, manifest, opened screenshot critique.
- **Validation commands:** `python3 tools/visual_review/run_visual_artifact_factory.py` and manifest command where environment supports Godot; otherwise document environment blocker.
- **Visual gate:** Contact sheet covers required review angles and can support human/visual-review verdicts.

## PR 10: Stage A visual approval gate

- **Scope:** Assemble evidence for human/visual-review decision on Stage A target.
- **Files likely touched:** Review report docs, manifest references, no gameplay changes unless explicitly scoped elsewhere.
- **Forbidden scope:** No self-approval, no public demo screenshot claim, no Stage B advancement without approval.
- **Evidence required:** Complete before/after contact sheet, opened PNG critique, asset provenance/manifests/hashes, gameplay-safe evidence for any integrated scene changes, human/visual-review verdict.
- **Validation commands:** Agent Studio validation, Godot import/build/smoke where applicable, asset manifest checks, `git diff --check`.
- **Visual gate:** Verdict is either `STAGE_A_VISUAL_NOT_GO` or human/visual-review-issued `STAGE_A_VISUAL_APPROVED`.

## Roadmap summary

1. Foundation: terrain and route forms.
2. Hero: crash hull silhouette and wreckage mass.
3. Interactables: workbench, save, beacon.
4. Pickups: resource and component clarity.
5. Threat: single Galaxabrain Scout.
6. Player tool: Mechanical Arm first-person read.
7. Composition: zone hierarchy and route readability.
8. Lighting/materials: palette, contrast, restrained glow.
9. Evidence: repeatable visual artifacts.
10. Approval: human/visual-review Stage A gate.

## Final roadmap verdict

`VISUAL_PLAN_READY` for production planning only. Stage A remains visually unapproved until future evidence and human/visual-review approval satisfy the gate.
