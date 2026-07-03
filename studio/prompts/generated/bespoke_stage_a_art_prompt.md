# Generated Codex Task Prompt: Bespoke Stage A Crash-Site Art

## Task Packet Summary

- Task description: Create bespoke static Stage A crash-site art for TitanCraft after free assets failed, preserving gameplay and requiring screenshots before approval.
- Routed category: visual_scene_composition
- Evidence category: visual
- Primary agent: art_director
- Secondary agents: visual_reviewer, technical_director, qa_lead
- Required checklists: before_task, before_visual_claim, visual_review, before_pr
- Minimum validation commands: `python3 tools/validate_agent_studio.py`, `git diff --check`

## Scope

Create bespoke static Stage A crash-site art direction for TitanCraft's MVP Crash Site presentation after free assets failed. Keep the work focused on static visual composition and review evidence. Preserve existing gameplay behavior and do not claim approval until screenshot evidence and visual diagnosis are available.

## Forbidden Scope

- Do not modify gameplay code unless explicitly requested and permitted by `README.md`.
- Do not change production scenes, Godot project settings, C# runtime code, tests, assets, or visual content outside the assigned art task.
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

- PNG screenshots
- Visual diagnosis naming focal point, route readability, silhouette, scale, and material coherence
- Before/after comparison
- Human-review or visual-reviewer verdict

## Visual Screenshot Gate

Before claiming visual success or Stage A approval, open and inspect PNG screenshots. The evidence must explicitly diagnose focal point, route readability, silhouette, scale, and material coherence. If screenshots are missing or cannot be opened, stop with an approved blocking verdict instead of claiming progress.

## Gameplay Preservation

Preserve the MVP Crash Site gameplay loop and all existing runtime behavior. Do not change player movement, inventory, mission logic, enemy behavior, save flow, menus, resource rules, C# runtime code, gameplay tests, or Godot project settings while executing this visual task.

## Approved Final Verdicts

Use exactly one of these routed verdicts:

- PASS
- FAIL_REPO_OWNED
- HUMAN_BLOCKED
- ENVIRONMENT_BLOCKED
- INTENTIONAL_GATE
- NOT_GO

Do not use vague verdicts such as `done`, `improved`, `looks good`, `should be fine`, or `tests passed`.
