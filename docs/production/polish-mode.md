# Polish Mode & Fast Lane

**Status:** Active as of 2026-08-14

The Crash Site MVP scope is closed (see `docs/production/mvp-closure-report-2026-07-09.md`).
The project is in a **Polish + Agent-Gated Validation** phase.

**Primary validation chain:** Agent Studio Windows playtest journey replaces the human as the required gate. See `docs/production/agent-playtest-primary-gate.md` and `studio/decisions/quality_benchmark_v2_agent_gate_delegation.md`.

Agent Studio remains the source of truth for high-risk work. For small, low-risk polish work a lighter **Fast Lane** is allowed so agents can iterate without full ceremony.

## When Polish Mode applies

- MVP scope is closed and must not expand.
- Remaining **required** gate is the agent-complete Windows playtest journey (smoke + aesthetic PNG review + validated verdict doc).
- Human playtest notes are optional enrichment only.
- Full Agent Studio preflight + multi-agent packets remain mandatory for:
  - Visual asset production / Stage gates
  - Scene structure or collision contract changes
  - New systems or scope expansion
  - Release / export claims (must cite agent journey evidence)
  - Quality-benchmark axis claims

## Fast Lane — allowed work

Fast Lane may be used for:

- Small gameplay bug fixes inside the existing Crash Site loop
- Combat / movement / feedback / juice tuning (values, messages, shake, FOV, cooldowns)
- HUD text, prompts, and action-feedback adjustments
- Audio cue volume, trigger, or path tweaks (no new audio systems)
- Minor UI polish (existing menus/HUD only)
- Documentation-only updates that do not authorize new gameplay scope

## Fast Lane — forbidden

- Any new feature, enemy type, resource type, map, or system
- Scene hierarchy / collision / navigation mesh structural changes
- Production asset replacement or new Stage A/B/C visual claims
- Scope expansion beyond README Crash Site MVP
- Release readiness claims without a validator-passing agent playtest verdict

## Fast Lane process (agents)

1. Confirm the task is inside Fast Lane allowed work.
2. State the change in 1–3 sentences and list files you will touch.
3. Make the smallest safe change.
4. Run minimum validation:
   - `dotnet build` (or project equivalent)
   - relevant unit / integration tests if behavior changed
5. Report:
   - files changed
   - commands run + result
   - short note if behavior changed
   - verdict: `PASS` or `FAIL_REPO_OWNED`

No full preflight packet is required for pure Fast Lane tasks.

If the task touches visuals, scene structure, assets, or release claims, exit Fast Lane and use the normal Agent Studio route (including agent playtest journey for release).

## Core roles during Polish Mode

1. Gameplay Engineer — movement, combat, feedback, juice
2. Technical Director — scene safety, performance, architecture
3. Visual Reviewer — PNG / aesthetic journey verdicts
4. Producer — stage gates and agent journey coordination
5. QA Lead — test evidence and playtest verdict validation

## Human role

Optional. See `docs/production/human-playtest-checklist.md` only if optional feel prose is desired. **Not required** to clear the primary agent gate.

## Relationship to existing gates

- README.md remains the product source of truth for scope.
- AGENTS.md and studio/ indexes remain binding for non-Fast-Lane work.
- Fast Lane does not weaken visual, asset, or release evidence requirements.
