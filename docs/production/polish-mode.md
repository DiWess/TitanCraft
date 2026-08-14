# Polish Mode & Fast Lane

**Status:** Active as of 2026-08-14

The Crash Site MVP scope is closed (see `docs/production/mvp-closure-report-2026-07-09.md`).
The project is now in a **Polish + Human Validation** phase.

Agent Studio remains the source of truth for high-risk work. For small, low-risk polish work a lighter **Fast Lane** is allowed so agents and humans can iterate on feel without full ceremony.

## When Polish Mode applies

- MVP scope is closed and must not expand.
- Remaining gates are human Windows playthrough, feel tuning, and selective visual polish.
- Full Agent Studio preflight + multi-agent packets remain mandatory for:
  - Visual asset production / Stage gates
  - Scene structure or collision contract changes
  - New systems or scope expansion
  - Release / export claims
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
- Release readiness or "ship it" claims without human GO

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
   - short manual check note (if gameplay feel is involved)
   - verdict: `PASS` or `FAIL_REPO_OWNED`

No full preflight packet, multi-agent secondary review, or formal memory/skill loading is required for pure Fast Lane tasks.

If the task touches visuals, scene structure, assets, or release claims, exit Fast Lane and use the normal Agent Studio route.

## Core roles during Polish Mode

Prefer these roles; treat others as reference only unless the task clearly needs them:

1. Gameplay Engineer — movement, combat, feedback, juice
2. Technical Director — scene safety, performance, architecture
3. Visual Reviewer — only when PNGs / visual claims are involved
4. Producer — stage gates and human GO coordination
5. QA Lead — test evidence and playtest procedures

## Human gate (still required)

Human Windows playthrough and GO remain `HUMAN_BLOCKED`.
Use `docs/production/human-playtest-checklist.md` for the required procedure and notes.

## Relationship to existing gates

- README.md remains the product source of truth.
- AGENTS.md and studio/ indexes remain binding for non-Fast-Lane work.
- Fast Lane does not weaken visual, asset, or release evidence requirements.
