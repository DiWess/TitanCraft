# Codex Execution Prompt: Bespoke Stage A Crash-Site Art

## Task Packet Summary

- Task description: Create bespoke static Stage A crash-site art for TitanCraft after free assets failed, preserving gameplay and requiring screenshots before approval.
- Routed category: visual_scene_composition
- Evidence category: visual
- Primary agent: art_director
- Secondary agents: visual_reviewer, technical_director, qa_lead
- Required checklists: before_task, before_visual_claim, visual_review, before_pr
- Minimum validation commands: `python3 tools/validate_agent_studio.py`, `git diff --check`

## Execution Objective

Create bespoke static Stage A crash-site art for the TitanCraft MVP Crash Site and integrate it into `Main.tscn` for production review. This is an execution prompt, not a rehearsal prompt and not a documentation-only task. The work must replace the failed free-asset direction with original, repo-owned static crash-site composition while preserving gameplay behavior.

## Scope

- Create bespoke static mesh art for the Stage A crash site using original geometry appropriate for TitanCraft's readable low-poly science-fiction direction.
- Integrate the static crash-site art into `Main.tscn` as production scene content for Stage A review.
- Preserve the existing MVP Crash Site gameplay loop, player route readability, collision safety, mission flow, and offline-first solo scope.
- Produce and inspect three production PNG screenshots before requesting approval.
- Complete at least one visible correction cycle after screenshot inspection before final verdict.

## Forbidden Scope

- No Stage B work, Stage B preparation, Stage B claims, or Stage B readiness language.
- No gameplay changes: do not change player movement, inventory, mission logic, enemy behavior, save flow, menus, resource rules, C# runtime code, gameplay tests, or Godot project settings.
- No documentation-only PR: the PR must include production scene integration and screenshot evidence, not only prompt or governance edits.
- Do not add forbidden MVP features, including multiplayer, grappling hook, wall running, procedural world, voxels, large mech, complete rocket, multiple maps, multiple enemy types, cloud services, or remote telemetry.
- Do not treat a route slab as terrain or decorate toy-like hulls as a substitute for readable crash-site composition.

## Required Memories

- MEM-VISFAIL-002
- MEM-SCREENSHOT-REVIEW-LESSONS-003
- MEM-CI-010
- MEM-STAGE-008
- MEM-PRODUCTION-STAGE-GATES-001
- MEM-VISFAIL-001
- MEM-VISFAIL-004
- MEM-VISFAIL-005

## Required Skills

- screenshot_critique
- visual_art_direction
- evidence_reporting

## Required Evidence

- Three production PNG screenshots from the integrated `Main.tscn` scene.
- Visual diagnosis for each screenshot naming focal point, route readability, silhouette, scale, and material coherence.
- Before/after comparison showing at least one correction cycle after initial screenshot inspection.
- Human-review or visual-reviewer verdict based on opened screenshots.
- Explicit gameplay preservation note confirming no gameplay logic, C# runtime code, Godot project settings, or gameplay tests changed.

## Visual Screenshot Gate

Before claiming visual success or Stage A readiness, open and inspect three production PNG screenshots captured from the integrated scene. The inspection must explicitly diagnose focal point, route readability, silhouette, scale, and material coherence for each screenshot. If screenshots are missing, cannot be opened, or cannot prove production integration, stop with `BLOCKED_BY_IMAGE_INSPECTION` or `BLOCKED_BY_RENDER_CAPTURE`.

## Correction Cycle Gate

After the first screenshot inspection, make at least one visual correction pass and capture updated evidence. The PR must identify what was corrected and why. Do not request human review until this correction cycle is complete.

## Gameplay Preservation

Preserve the MVP Crash Site gameplay loop and all existing runtime behavior. If any gameplay behavior, route readability, collision contract, mission flow, or runtime safety regresses, stop with `GAMEPLAY_REGRESSION`. Visual art may support gameplay readability, but it must not alter gameplay rules or introduce new mechanics.

## Required Validation Commands

Run only validation commands applicable to this visual Stage A execution task. At minimum, include:

```bash
python3 tools/validate_agent_studio.py
git diff --check
```

Add Godot import or scene validation only if the execution environment supports it and the production scene change requires it. Do not run gameplay tests unless separately requested.

## Approved Final Verdicts

Use exactly one of these task-specific final verdicts:

- STAGE_A_CUSTOM_ART_READY_FOR_HUMAN_REVIEW
- STAGE_A_CUSTOM_ART_NOT_GO
- BLOCKED_BY_IMAGE_INSPECTION
- BLOCKED_BY_RENDER_CAPTURE
- GAMEPLAY_REGRESSION

Do not use vague verdicts such as `done`, `improved`, `looks good`, `should be fine`, or `tests passed`.
