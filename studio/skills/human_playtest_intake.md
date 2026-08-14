# Skill: Human Playtest Intake

## purpose

Turn a completed human Windows playtest note into actionable Agent Studio work:
classify findings, preserve feel language as human-dated evidence, and open only
the minimum Fast Lane or full-route tasks justified by the note.

## when_to_use

Use when a human has filled `docs/production/human-playtest-checklist.md` (or a
dated note under `docs/production/playtests/`) and agents must react to the
verdict (`HUMAN_GO`, `HUMAN_BLOCKED`, or `NEEDS_TUNING`).

## required_inputs

- Path to the dated human playtest note
- Explicit human verdict line
- Machine / build context if present (editor vs export, hardware)

## procedure

1. Read the entire note. Do not invent feel claims the human did not write.
2. Classify each finding:
   - **P0** — blocks completing the Crash Site loop or causes soft-lock / crash
   - **P1** — serious feel or readability issue that should block ship GO
   - **P2** — polish / tuning preference (Fast Lane candidates)
3. Map findings to owners:
   - Movement / combat / feedback values → Gameplay Engineer (Fast Lane if value-only)
   - Save / mission / soft-lock → Gameplay Engineer + QA Lead
   - Export / offline launch → Build Release Engineer
   - Visual readability only → Visual Reviewer (full visual route if PNG claims)
4. If verdict is `HUMAN_GO` and no P0/P1: update production status; no code required.
5. If `NEEDS_TUNING`: open one Fast Lane task per clear value change; keep changes minimal.
6. If `HUMAN_BLOCKED`: open the smallest set of tasks that clear P0/P1 only.
7. Quote the human’s dated feel lines in any later quality-benchmark or release note;
   never rewrite them as agent opinion.

## automatic_failures

- Treating agent CI smoke as a substitute for human feel notes
- Expanding scope beyond Crash Site MVP to “fix” a playtest complaint
- Claiming `HUMAN_GO` without a real dated human document
- Vague verdicts (done, feels better, should be fine)

## output_format

- Note path and human verdict:
- Finding table (id, severity, owner, Fast Lane?):
- Proposed tasks (max 3 unless P0 flood):
- Status impact (what changes in current-status.md):
- Final agent verdict: `PASS` | `FAIL_REPO_OWNED` | `HUMAN_BLOCKED`

## evidence_required

- Link or path to the human note
- Quoted human lines for any feel claim used downstream
- List of files/tasks opened as a result

## example_good_output

Note `docs/production/playtests/2026-08-14-windows-human-playtest.md` verdict
`NEEDS_TUNING`. Human: “arm cooldown too long”. One Fast Lane task: reduce
`AttackCooldownSeconds` export default; build + unit tests; no scene changes.

## example_bad_output

Playtest said combat feels off so we should add a second enemy and a grappling hook.
