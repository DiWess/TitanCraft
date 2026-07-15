# Skill: Crash Site Gameplay Slice

## purpose

Execute a repeatable A-to-Z workflow for authorized Crash Site MVP gameplay slices without expanding README scope.

## when_to_use

Use when a task asks for gameplay implementation, gameplay bug fixes, mission-loop smoke coverage, or Agent Studio gameplay workflow improvements.

## required_inputs

- README Crash Site MVP scope and forbidden MVP feature list.
- Agent Studio preflight packet for the exact task.
- Routed memories, especially product scope and gameplay MVP scope cards.
- Assigned gameplay slice, changed files or intended file list, and required evidence plan.

## procedure

1. Confirm the slice is inside the README Crash Site MVP loop and rejects forbidden features before planning code.
2. State the one player-facing behavior, affected systems, public API risk, and expected mission checkpoint impact.
3. Select evidence: unit tests for isolated logic, integration tests for interacting systems, and a mission smoke procedure for playable flow claims.
4. Implement only the assigned slice, preserving offline Windows-first behavior and existing public APIs unless a human-approved scope change exists.
5. Verify objective/HUD clarity when mission progression changes, save/respawn when persisted state changes, and victory/defeat when end states change.
6. Record exact commands, artifacts, runtime flags or logs when applicable, and any manual Godot smoke steps that remain.
7. Return blockers first, then fixes, evidence, risks, and an approved final verdict.

## automatic_failures

- The slice adds multiplayer, cloud services, remote telemetry, procedural worlds, voxels, grappling hook, wall running, multiple maps, multiple enemy types, or other README-forbidden MVP scope.
- The change claims gameplay correctness without required test or smoke evidence.
- The change claims movement, combat, level readability, or game feel without a dated human playtest note or documented manual procedure.
- The slice mixes multiple unrelated gameplay features or edits visual/assets/release scope without routing evidence.

## output_format

- Slice checked:
- Scope gate:
- Memories used:
- Systems touched:
- Evidence plan:
- Commands/artifacts:
- Manual smoke path:
- Risks:
- Verdict:

## evidence_required

- README scope citation or section reference for the allowed Crash Site behavior.
- Changed gameplay or governance file list.
- Unit, integration, or mission smoke evidence matching the changed behavior.
- Manual Godot smoke steps for subjective movement, combat, objective clarity, or mission-flow claims.

## example_good_output

Slice checked: Crash Site resource pickup objective advancement. Scope gate: README MVP resources and mission loop only. Evidence: integration test for inventory-to-mission transition plus manual smoke path from spawn to workbench. Verdict: `PASS`.

## example_bad_output

Added grappling hook and two enemy variants because gameplay is more fun; looks good.
