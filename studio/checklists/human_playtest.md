# Human Playtest Checklist (Agent Studio)

For **humans** running the game, use `docs/production/human-playtest-checklist.md`.

This checklist is for **agents** after a human note exists.

## Intake

- [ ] Dated note exists under `docs/production/playtests/`
- [ ] Verdict is exactly one of: `HUMAN_GO` | `HUMAN_BLOCKED` | `NEEDS_TUNING`
- [ ] Machine / editor-vs-export context recorded if provided

## Classification

- [ ] Each issue has severity P0 / P1 / P2
- [ ] Feel language is quoted from the human, not invented
- [ ] No scope expansion proposed

## Response

- [ ] P0/P1 mapped to minimal tasks
- [ ] Value-only tuning uses Fast Lane (`docs/production/polish-mode.md`)
- [ ] Visual claims still require full visual evidence route
- [ ] `docs/production/current-status.md` updated only with evidence-backed lines

## Forbidden

- [ ] No “agents say feel is fine” without human note
- [ ] No release `GO` without `HUMAN_GO` (or explicit human waiver documented)
