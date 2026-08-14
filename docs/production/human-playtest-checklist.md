# Human Windows Playtest Checklist — Crash Site MVP

**Purpose:** Close the remaining `HUMAN_BLOCKED` gate for gameplay feel, readability, and offline Windows validation (README §27 / §30).

**Owner:** Human (parent or designated tester)
**Environment:** Target Windows PC, offline, native exported build preferred (or Godot editor play if export unavailable).

Record date, machine (CPU/GPU/RAM), and whether you used the exported build or editor.

## Setup

- [ ] Fresh save / New Game
- [ ] No internet required (confirm game does not demand network)
- [ ] Mouse capture works; Esc opens pause

## Core loop (must complete once)

- [ ] Walk, look, jump, sprint feel acceptable (note any floatiness, sluggishness, or camera issues)
- [ ] Collect Metal, Biomass, Electronics — counters update on HUD
- [ ] Objectives / prompts are understandable without a long tutorial
- [ ] Craft Mechanical Arm Mk I at workbench — resources consumed correctly
- [ ] First-person arm visual switches from bare to mechanical arm after craft
- [ ] Attack Galaxabrain Scout with arm (cooldown, hit feedback, damage readable)
- [ ] Scout detects, chases, and attacks; player can die and reload from checkpoint
- [ ] Scout dies; component appears and can be recovered
- [ ] Beacon activates only after component; victory screen appears
- [ ] Full loop finishes in under ~30 minutes

## Feel notes (required for any "feel" claim)

Write short dated notes. Examples:

- Movement: too slow / too fast / ok
- Combat: arm feels weak / strong / cooldown too long
- Enemy: too aggressive / too passive / ok
- Readability: objectives clear / confusing
- Performance: stable 60 / dips / unplayable on this machine

## Save / resume

- [ ] Save at checkpoint works
- [ ] Quit and Continue restores progress correctly
- [ ] Death returns to last save without soft-lock

## Export / offline (if testing native build)

- [ ] Executable launches without Godot editor
- [ ] No account / login / online requirement
- [ ] Quit is clean

## Verdict

After the playthrough, record one of:

- `HUMAN_GO` — ready to treat remaining work as polish only; no P0/P1 blockers found
- `HUMAN_BLOCKED` — list blocking issues (P0/P1) that must be fixed before GO
- `NEEDS_TUNING` — playable but specific feel values should be adjusted (list them)

Attach this note under `docs/production/playtests/` with date in the filename, e.g. `2026-08-14-windows-human-playtest.md`.

## Out of scope for this checklist

- Marketing screenshots
- New features beyond Crash Site MVP
- Claiming quality-benchmark 10/10 without peer-anchored evidence
