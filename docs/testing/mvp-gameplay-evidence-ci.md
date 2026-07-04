# TitanCraft MVP Gameplay Evidence CI

## Purpose

The **TitanCraft MVP Gameplay Evidence** workflow produces downloadable gameplay QA evidence for the locked Crash Site MVP loop. It runs the existing automated test path, verifies the required `MVP_SMOKE_MILESTONE_##` markers, and uploads the generated logs and test results as a GitHub Actions artifact.

This workflow is an evidence bundle for CI review. It does not change the MVP rules, expand gameplay scope, or approve any gameplay, visual, asset, or release milestone by itself.

## When it runs

The workflow runs on:

- `pull_request` events that touch the guarded gameplay, testing, scene, or workflow paths;
- manual `workflow_dispatch` runs from the GitHub Actions UI.

The pull request path filters include:

- `src/**`
- `tests/**`
- `tools/test.sh`
- `tools/testing/**`
- `scenes/Main/**`
- `.github/workflows/mvp-gameplay-evidence.yml`

## Artifact name

The uploaded artifact is named:

```text
mvp-gameplay-evidence
```

When present, the bundle includes:

- `tests/TestResults/unit.trx`
- `tests/TestResults/integration.log`
- `tests/TestResults/import.log`
- `tests/TestResults/smoke.log`
- `tests/TestResults/export.log`
- other `.log`, `.trx`, and `.txt` files produced under `tests/TestResults/` by `./tools/test.sh`

## Required milestone markers

The workflow fails if `tests/TestResults/integration.log` is missing any of these markers:

- `MVP_SMOKE_MILESTONE_01`
- `MVP_SMOKE_MILESTONE_02`
- `MVP_SMOKE_MILESTONE_03`
- `MVP_SMOKE_MILESTONE_04`
- `MVP_SMOKE_MILESTONE_05`
- `MVP_SMOKE_MILESTONE_06`
- `MVP_SMOKE_MILESTONE_07`
- `MVP_SMOKE_MILESTONE_08`
- `MVP_SMOKE_MILESTONE_09`
- `MVP_SMOKE_MILESTONE_10`
- `MVP_SMOKE_MILESTONE_11`

The markers are checked by `python3 tools/testing/verify_mvp_smoke_log.py tests/TestResults/integration.log`. The workflow does not fake or synthesize milestone output.

## How to download evidence

1. Open the repository on GitHub.
2. Select the **Actions** tab.
3. Open a run of **TitanCraft MVP Gameplay Evidence**.
4. After the run completes, scroll to **Artifacts**.
5. Download **mvp-gameplay-evidence**.
6. Inspect `integration.log`, `unit.trx`, `import.log`, `smoke.log`, `export.log`, and any other included test logs.

## What it proves

A successful run proves that, in the GitHub Actions Linux CI environment:

- .NET packages restored;
- the Godot .NET project built;
- the unit test command completed;
- Godot headless import completed;
- the integration runner completed;
- every required Crash Site MVP smoke milestone marker appeared in `integration.log`;
- the existing `./tools/test.sh` automated path completed;
- `git diff --check` found no patch whitespace errors.

## What it does not prove

This workflow has deliberate limits:

- it does **not** claim Windows readiness;
- it does **not** replace manual gameplay feel review;
- it does **not** approve visuals;
- it does **not** prove final release readiness;
- it does **not** approve art, assets, materials, scene composition, readability, or visual direction;
- it does **not** replace human review of FPS feel, pacing, mouse comfort, or Crash Site route clarity.
