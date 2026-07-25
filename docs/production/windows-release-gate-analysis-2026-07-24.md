# Windows Release Gate — Blocker Analysis (2026-07-24)

**Routed packet:** `release_readiness` → build_release_engineer primary, producer / qa_lead secondary
**Owner of this document:** producer (`studio/indexes/ownership.yml`)
**Status:** `ENVIRONMENT_BLOCKED` for the agent; one human action unblocks the delegated chain.

## Finding 1 — the delegated journey has never been dispatched

`.github/workflows/windows-playtest-journey.yml` was added 2026-07-11 to implement
`studio/decisions/quality_benchmark_v2_agent_gate_delegation.md`. It has three runs on record, all
`conclusion: success`:

| Run | Event | Date | What actually executed |
|---|---|---|---|
| 1 | `push` | 2026-07-11 | `validate-committed-verdicts` only |
| 2 | `pull_request` | 2026-07-11 | `validate-committed-verdicts` only |
| 3 | `push` (merge of #124) | 2026-07-11 | `validate-committed-verdicts` only |

The `windows-export-smoke` job carries `if: github.event_name == 'workflow_dispatch'`. None of the
three runs was a `workflow_dispatch`, so **the Windows runner job has never executed**. The three
green checks certify only that zero committed verdict documents contain zero errors.

`docs/production/playtests/` does not exist. `tools/validate_playtest_evidence.py` reports this
honestly rather than faking a pass — it prints "no committed verdict documents yet" and exits 0 —
so nothing in the repository is claiming evidence that does not exist. The gap is real but it has
not been papered over.

**Consequence:** the pipeline that the 2026-07-11 ADR built to replace the human in the chain has
produced no evidence at all. This is why `docs/production/quality-scorecard-log.md` still reads
"Axis 10 stays 2.0 until the journey produces its first validated verdict document."

## Finding 2 — the stated blocker predates the delegation it ignores

`docs/production/current-status.md` cites the v2 ADR at line 38 for aesthetic verdicts, but the
Stage C entries from 2026-07-18 (a week *after* the ADR) still record the remaining gate as
"the human Windows playthrough and GO (README §27)" and the release gate as `HUMAN_BLOCKED` on
"the README §27 manual Windows validation."

These are not obviously reconcilable, and the distinction matters:

- The ADR delegates the **evidence lanes** — stability/performance numbers to the Windows runner,
  the aesthetic verdict to the Visual Reviewer agent on opened PNGs — and states that a verdict
  document citing measured proxies "can reach `PASS`/`GO`/`NOT_GO` without a human."
- Several README §27 criteria are directly machine-checkable on a Windows runner: launches without
  the IDE, runs with no network, no account or API key, local save works, clean exit, resume from
  save.
- Whether the **final ship GO** is delegated is a separate product decision. The ADR's language
  permits an agent `GO`; the Stage C documents assume the human retains it.

This document does not resolve that on the human's behalf. It records the conflict per
`CLAUDE.md` § 1 rather than picking the reading that would let the gate close faster.

## Finding 3 — what remains genuinely human-only

Unchanged by anything above, per ADR lane 3: subjective feel language ("feels responsive", "combat
is satisfying") is rejected in any verdict document lacking a dated `Human note (YYYY-MM-DD)`.
`tools/validate_playtest_evidence.py` enforces this with `BANNED_FEEL_PHRASES`. No agent may assert
an experience it never had, and the provisional README § 11 combat-feel tuning still depends on a
human at the controls.

## Finding 4 — two defects in the never-executed job, now fixed

Code that has never run is where bugs accumulate. Auditing `windows-export-smoke` before the first
dispatch found two, both fixed in this change set so the single manual run produces complete
evidence rather than needing a second attempt:

1. **The export log was uploaded from a path that never exists.** `tools/test.ps1` line 78 writes
   the export log to `tests/TestResults/export.log`, but the upload step listed
   `builds/Windows/export.log`. Because the other upload patterns match real files,
   `if-no-files-found: error` would not fire — the run would go green while silently dropping the
   export log that ADR rule 1 ("no number without a source") relies on. The upload now points at
   the real path and also collects `tests/TestResults/import.log`.
2. **The Godot version pin was decorative.** The check piped `--version` into `Select-String` and
   then tested `$LASTEXITCODE`. `Select-String` is a cmdlet and never sets `$LASTEXITCODE`, so the
   variable held Godot's own exit code (`0`) and a version *mismatch* passed silently. The pin is
   now asserted against the captured output string.

Residual risk not fixed, because it cannot be verified from this container: the measured smoke runs
`--headless --quit-after 600` against the *exported* binary rather than the editor binary.
`tools/test.ps1` proves those flags work on the editor build; whether the release export template
honours them is unverified until the first dispatch. If the smoke step fails on argument handling,
that is the first thing to check.

## Finding 5 — the aesthetic lane is verified working; only the Windows lane needs the dispatch

The other two `workflow_dispatch`-gated jobs were audited the same way. Both are sound as written:

- `aesthetic-captures` runs `tools/visual_review/run_visual_artifact_factory.py` with no `xvfb-run`
  prefix, which looks wrong against the 2026-07-06 capability note — but the script wraps
  `xvfb-run -a` internally (line 62), and the job installs `xvfb`. Correct as written.
- `verdict-scaffold` consumes the downloaded capture bundle via
  `captures_dir.rglob("*.png")` (`tools/build_playtest_verdict_draft.py` line 28), so artifact
  nesting cannot break it, and a zero-capture result emits an explicit
  "capture job must be investigated before any PASS" line rather than a silent pass.

The aesthetic lane runs on `ubuntu-latest`, so it was **executed in this container rather than only
read**: `python3 tools/visual_review/run_visual_artifact_factory.py` exited 0 and produced 28 PNGs
(160–245 KB each) across `phase3a-production-integration`, `crafted-arm-first-person`,
`stage-a-custom-final`, and `stage-a-custom-audition`, with a scene manifest. One was opened to
confirm real rendered content rather than the null-viewport black frame that `--headless` produces:
`production_01_spawn_overview.png` shows the crash hull, light poles, crate stacks, red biomass and
cyan pickups, terrain, and distant crystal silhouettes.

This is a pipeline verification, not a Visual Reviewer verdict — issuing that verdict remains the
routed Visual Reviewer's task on the artifacts from a real dispatched run.

Captures are gitignored (`.gitignore` line 77) and were not committed, per the repo binary policy.

**Net effect on the blocker:** of the journey's three evidence lanes, the aesthetic lane is proven
to work, the smoke lane's two defects are fixed, and only the Windows-runner measurement genuinely
requires the manual dispatch — it cannot be produced from a Linux container at all.

## Required action — one human step

The agent cannot dispatch the workflow. `POST .../workflows/windows-playtest-journey.yml/dispatches`
returns `403 Resource not accessible by integration`: this session's GitHub App lacks `actions:
write`. (The same session also cannot open pull requests — `POST /pulls` returns `500` on four
attempts — so both GitHub write paths are unavailable here.)

To unblock, the repository owner runs the journey once:

1. Open <https://github.com/DiWess/TitanCraft/actions/workflows/windows-playtest-journey.yml>
2. **Run workflow** → branch `main` → **Run workflow**.
3. The Windows runner exports the build, runs the measured smoke (exit code, frames, wall seconds,
   exe SHA-256), captures the allowlisted scene PNGs, and uploads `smoke_report.json` plus the
   verdict draft scaffold as run artifacts.

Then the delegated chain can complete agent-side: a vision-capable Visual Reviewer opens the
captures, fills the aesthetic verdict citing every opened file, and commits the completed document
to `docs/production/playtests/`, where `validate_playtest_evidence.py` gates it on push.

Alternatively, granting this session's GitHub App `actions: write` would let the agent dispatch the
run directly and drive the rest of the chain without the manual step.

## Verdict

`ENVIRONMENT_BLOCKED` — the Windows release gate cannot advance from this container. The blocker is
not missing engineering work: the pipeline exists, is wired, and validates. It is that the journey
has never been triggered, and the trigger requires a permission this session does not hold.

No gameplay code, scenes, assets, or tests were changed by this analysis.
