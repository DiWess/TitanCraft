# Windows MVP Playtest Runbook

This runbook connects a specific Windows export artifact to the manual offline MVP playtest checklist and evidence template. It prepares evidence for review; it does not claim Windows readiness unless the real Windows playtest was actually completed and passed.

## Scope and non-readiness disclaimer

Use this runbook only for the locked Crash Site MVP in `README.md` and `docs/production/mvp-scope.md`.

Exact non-readiness disclaimer:

> Creating a Windows export does not prove Windows readiness. Linux/headless CI does not prove Windows readiness. Manual Windows playtest is required. Visual approval is separate. Gameplay feel approval is separate. Public demo readiness is not claimed.

Manual Windows playtest evidence is required because automated restore, build, import, headless smoke, Linux export, and Windows CI smoke do not prove the full offline player experience on a clean Windows install or real Windows VM.

## Relationship to the checklist and evidence template

Use these files together:

1. `docs/release/windows-mvp-export.md` explains how to produce or retrieve the Windows export artifact and record its metadata.
2. `docs/testing/windows-offline-mvp-playtest.md` is the checklist the tester follows during the manual Windows offline run.
3. `docs/testing/windows-playtest-evidence-template.md` is copied for each specific playtest run and filled with artifact metadata, environment details, milestone results, notes, bugs, optional media, and final verdict.

Do not edit the template in place for a test run. Copy it to a dated evidence record path chosen by the release lead, for example:

```text
docs/testing/evidence/windows-mvp-playtest-YYYYMMDD-<short-commit>.md
```

If evidence records should remain outside the repository for privacy or file-size reasons, record the external storage location or issue/PR link in the PR body instead of committing private media.

## Before the playtest

1. Produce or download the Windows export artifact using `docs/release/windows-mvp-export.md`.
2. Record artifact name, timestamp, SHA-256 hash, source commit, export command or CI workflow run URL, and extracted executable path.
3. Prepare a clean Windows install or real Windows VM.
4. Disconnect from the Internet or block network access before the first playtest launch, then record the method.
5. Copy `docs/testing/windows-playtest-evidence-template.md` into a per-run evidence record.
6. Fill all environment and artifact fields before starting the main checklist.

## Run the offline Windows MVP playtest checklist

Follow `docs/testing/windows-offline-mvp-playtest.md` from top to bottom against the exported `TitanCraft.exe`, not from the Godot editor.

Required coverage includes:

- clean Windows or real Windows VM launch environment;
- direct `TitanCraft.exe` launch without Godot editor or developer tools;
- offline main menu, New Game, Continue when a save exists, and no network/account dependency;
- complete Crash Site victory loop: spawn, objective clarity, three resources, workbench, Mechanical Arm Mk I, Galaxabrain Scout fight, component retrieval, save point, beacon, victory;
- defeat path and checkpoint/respawn behavior;
- save, continue, quit, relaunch, and clean process exit;
- mouse/keyboard feel, FPS comfort, prompt readability, stability, audio, and clean quit notes.

Record observations even when a milestone passes. Gameplay feel approval remains separate from checklist completion and must not be self-approved by automation.

## Fill the evidence template

For every template field:

1. Enter the exact Windows version/build, hardware or VM details, display, inputs, and network state.
2. Enter the exact build artifact path, `SHA-256` digest, source commit, and timestamp from the export workflow.
3. For each milestone, use only `PASS`, `FAIL`, `BLOCKED`, or `NOT_TESTED`.
4. Add concise evidence notes and bug IDs for any milestone that is not a clear pass.
5. List all crashes, freezes, hangs, soft locks, audio issues, save/continue issues, and clean-quit problems.
6. Link optional screenshots, video, logs, or issue records when available.
7. Choose exactly one template final verdict: `WINDOWS_MVP_PLAYTEST_CHECKLIST_READY` or `WINDOWS_MVP_PLAYTEST_CHECKLIST_FAILED`.

## PASS / FAIL / BLOCKED meanings

Use these meanings for milestone rows and runbook review:

- `PASS`: the tester actually performed the step on the recorded Windows artifact, the observed result matched the checklist expectation, and evidence notes are sufficient for review.
- `FAIL`: the step was performed and exposed a repo-owned defect, incorrect behavior, crash, soft lock, unclear blocker, broken save/continue behavior, offline dependency, or other issue that prevents accepting that milestone.
- `BLOCKED`: the step could not be performed because of an environment or external limitation, such as no clean Windows machine/VM, inaccessible artifact, missing permissions, broken VM graphics, or unavailable tester hardware.
- `NOT_TESTED`: the step was skipped or no evidence was recorded. Any required `NOT_TESTED` milestone makes the playtest checklist final verdict `WINDOWS_MVP_PLAYTEST_CHECKLIST_FAILED`.

A checklist can use `WINDOWS_MVP_PLAYTEST_CHECKLIST_READY` only when every required milestone passed on clean Windows hardware or a real Windows VM, evidence fields are complete, and no blocking bugs remain.

## What this does not prove

A completed runbook and evidence template do not prove every release gate. They do not prove:

- public demo readiness;
- visual approval, which requires the separate visual review process and PNG evidence;
- gameplay feel approval beyond the named tester notes;
- beta or production readiness;
- code signing, installer, storefront, rollback, telemetry, privacy, or protected-environment readiness;
- support for platforms other than Windows PC;
- any feature outside the locked Crash Site MVP.

If the real Windows playtest was not run, report `BLOCKED` or `WINDOWS_MVP_PLAYTEST_CHECKLIST_FAILED` rather than claiming readiness.
