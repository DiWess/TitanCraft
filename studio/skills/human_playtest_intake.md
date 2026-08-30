# Skill: Human Playtest Intake (Optional Enrichment)

## purpose

Process an **optional** human Windows note. Does not replace the primary agent-gated
playtest journey (`windows_playtest_journey`). Human notes may add feel quotes or P0 bug reports.

## when_to_use

Only when a human filed a note under `docs/production/playtests/` or completed the optional
human checklist. For required ship validation, use `windows_playtest_journey` instead.

## required_inputs

- Path to human note (if any)
- Awareness of latest agent journey verdict (if any)

## procedure

1. Confirm primary gate status via latest validated agent playtest doc (if present).
2. Read human note; quote feel lines only with their date.
3. Classify P0/P1/P2; map to Fast Lane or full routes.
4. Human P0 bugs can reopen work even if agent journey previously PASS.
5. Never require human GO to accept an agent journey PASS that passed the validator.

## automatic_failures

- Blocking release solely because no human note exists
- Inventing feel adjectives
- Expanding MVP scope

## output_format

- Agent journey status:
- Human note path (or none):
- Findings / tasks:
- Verdict: `PASS` | `FAIL_REPO_OWNED` | `NOT_GO`

## evidence_required

- Agent journey doc path when making release claims
- Human quotes only when feel language is used
