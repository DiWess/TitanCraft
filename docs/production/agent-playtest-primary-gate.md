# Agent Playtest Primary Gate

**Status:** Active as of 2026-08-14  
**Authority:** `studio/decisions/quality_benchmark_v2_agent_gate_delegation.md` (APPROVED) + explicit owner directive to replace the human in the validation chain.

## Decision

The **required** ship / release validation chain is **agent-complete**:

1. CI Windows export + measured smoke (`windows-playtest-journey` workflow) → `smoke_report.json`
2. Aesthetic captures + **Visual Reviewer** agent verdict on opened PNGs
3. Integration / MVP smoke suite PASS
4. Verdict document under `docs/production/playtests/` that passes `tools/validate_playtest_evidence.py`

A validated agent journey verdict may record `PASS` / `GO` / `NOT_GO` **without** a human playthrough.

## What is NOT allowed

- Agents inventing feel adjectives ("feels responsive", "combat is satisfying") without a dated `Human note (YYYY-MM-DD)`
- Treating agent consensus without opened PNGs / smoke evidence as aesthetic GO
- Expanding MVP scope to pass a gate

## Human role (optional)

Human Windows play (`docs/production/human-playtest-checklist.md`) is **enrichment**, not a blocker:

- Optional for feel prose on quality-benchmark axes
- Optional for subjective tuning preference
- **Not required** to clear the primary agent gate

## Agent Studio routing

| Task | Route |
|------|--------|
| Run / complete journey | `windows_export` + skill `windows_playtest_journey` |
| Aesthetic verdict on captures | Visual Reviewer |
| Intake optional human note | `human_playtest` + `human_playtest_intake` |
| Value-only tuning after gate | `fast_lane_polish` |

## Producer procedure

1. Dispatch `.github/workflows/windows-playtest-journey.yml`
2. Download artifacts; Visual Reviewer opens every capture PNG
3. Fill verdict draft; leave feel lines empty or marked proxy-only unless human note exists
4. Run `python3 tools/validate_playtest_evidence.py`
5. Commit `docs/production/playtests/<date>-journey.md` only if validation passes
6. Update `docs/production/current-status.md` from that document only

## Relationship to Polish Mode

Polish Mode Fast Lane remains for small fixes. It does not replace this gate for release claims.
