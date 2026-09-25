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

## example_good_output

Agent journey status: `windows_playtest_journey` `PASS` on workflow run 1234567. Human note:
`docs/production/playtests/2026-09-24-windows-human-playtest.md`. Findings: quoted the note's
dated line "strafe lean makes me queasy after a minute" (2026-09-24) as feel evidence, P1, Fast
Lane; the note also reports the Scout falling through the seawall and becoming unkillable, P0,
which reopens enemy work despite the agent PASS. Verdict: `FAIL_REPO_OWNED` until the P0 is fixed.

## example_bad_output

Human signed off the renders on 2026-09-24, so movement and combat feel great; `PASS`.
