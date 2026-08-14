# Human Windows Playtest Checklist — Crash Site MVP (OPTIONAL)

**Status:** Optional enrichment only (as of 2026-08-14).

The **required** validation chain is agent-complete. See:

- `docs/production/agent-playtest-primary-gate.md`
- `studio/decisions/quality_benchmark_v2_agent_gate_delegation.md`
- skill `windows_playtest_journey`

Use this checklist only when a human wants to add dated feel notes or subjective tuning preference. It does **not** block agent `GO` / `PASS` on the primary journey.

**Owner:** Human (optional)  
**Environment:** Target Windows PC, offline, native exported build preferred.

Record date, machine (CPU/GPU/RAM), and editor vs export.

## Setup

- [ ] Fresh save / New Game
- [ ] No internet required
- [ ] Mouse capture works; Esc opens pause

## Core loop (optional full pass)

- [ ] Walk, look, jump, sprint feel notes
- [ ] Collect Metal, Biomass, Electronics — HUD
- [ ] Objectives / prompts clarity
- [ ] Craft Mechanical Arm Mk I
- [ ] First-person arm visual after craft
- [ ] Combat vs Galaxabrain Scout
- [ ] Death → checkpoint
- [ ] Component recovery + beacon + victory
- [ ] Loop under ~30 minutes

## Feel notes (only place agents may quote feel adjectives)

Must be dated. Example header: `Human note (2026-08-14)`

- Movement:
- Combat:
- Enemy:
- Readability:
- Performance:

## Save / resume / export

- [ ] Checkpoint / Continue / death reload
- [ ] Native export offline launch (if tested)

## Optional verdict labels

- `HUMAN_NOTE_ONLY` — enrichment, no claim to supersede agent journey
- `HUMAN_TUNING_REQUEST` — list desired value changes for Fast Lane
- `HUMAN_P0_REPORT` — human found a blocking bug agents should treat as P0

Save under `docs/production/playtests/` with date in the filename if filed.

## Out of scope

- Replacing the agent journey as the primary gate
- Marketing screenshots
- Scope expansion beyond Crash Site MVP
