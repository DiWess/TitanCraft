# Skill: Windows Playtest Journey

## purpose

Execute the agent-gated Windows playtest journey end to end: generate the measured Windows smoke
evidence and aesthetic capture bundle via CI, complete the verdict draft as Visual Reviewer, and
commit a validator-passing verdict document — without fabricating feel claims.

## when_to_use

Use when a task mentions `windows playtest`, `aesthetic verdict`, or `playtest journey`, or when
a scorecard axis (2, 3, or 6) needs verdict evidence per
`studio/decisions/quality_benchmark_v2_agent_gate_delegation.md`.

## required_inputs

- A completed `windows-playtest-journey` workflow run (dispatch it if none exists).
- Downloaded artifacts: `windows-playtest-bundle`, `aesthetic-capture-bundle`, `playtest-verdict-draft`.
- The delegation ADR and `quality_benchmark_v1.md` binding rules.

## procedure

1. Dispatch `.github/workflows/windows-playtest-journey.yml` and wait for all jobs to pass.
2. Download the three artifacts; confirm `smoke_report.json` shows exit code 0 and carries the
   executable SHA-256 and run id.
3. Open every capture PNG in the bundle. Judge focal point, route readability, silhouette,
   scale, and material coherence against the Art Taste Pack.
4. Fill the draft's Aesthetic Verdict with the diagnosis and an approved-vocabulary verdict,
   citing each opened PNG path.
5. Leave Feel Evidence as measured proxies only; add feel adjectives only inside a dated
   `Human note (YYYY-MM-DD)` supplied by a human.
6. Set the Final Verdict, save the document to `docs/production/playtests/<date>-journey.md`,
   and run `python3 tools/validate_playtest_evidence.py` locally.
7. Commit only when validation passes; the journey workflow re-validates on push.

## automatic_failures

- Any verdict field left `PENDING` in a committed document.
- Subjective feel language without a dated human note.
- Aesthetic verdict issued without opening the PNGs or without citing their paths.
- Numbers (FPS, frames, seconds) that do not trace to `smoke_report.json` or workflow logs.
- Vague verdict such as done, improved, looks good, should be fine, or tests passed.

## output_format

- Workflow run id and artifact names:
- Smoke evidence summary (exit code, frames, wall seconds, SHA-256):
- Opened captures:
- Aesthetic diagnosis:
- Aesthetic verdict:
- Final verdict:

## evidence_required

- `smoke_report.json` values quoted with the run id that produced them.
- Exact paths of every opened capture PNG.
- The committed verdict document path passing `tools/validate_playtest_evidence.py`.

## example_good_output

Workflow run 1234567 produced `windows-playtest-bundle` (exit_code 0, 600 frames, 10.2 s,
sha256 `ab12...`). Opened all 9 PNGs in `aesthetic-capture-bundle`; focal point and route
readability hold, save-point silhouette is weak at distance. Aesthetic verdict: `NOT_GO` until
the save-point read is fixed. Final verdict: `FAIL_REPO_OWNED` with the silhouette finding.

## example_bad_output

Playtest passed, game feels great and looks good; shipping it.
