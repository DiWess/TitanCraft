# Windows MVP Export CI Evidence — 2026-07-04

## Scope

This evidence record verifies CI artifact production only for the GitHub Actions workflow file `.github/workflows/windows-mvp-export.yml`. It does not claim Windows readiness, manual playtest success, public demo readiness, signing readiness, production readiness, gameplay feel approval, or visual approval.

## Preflight summary

- Task category: `build_failure`.
- Evidence category: `build`.
- Primary agent: `build_release_engineer`.
- Required memories reviewed: `MEM-CI-RELEASE-LESSONS-004`, `MEM-CI-013`.
- Required skills reviewed: `ci_cd_validation`, `production_debugging`, `evidence_reporting`.
- Required evidence: exact command output, artifact path, failure class, environment limitation if blocked.

## Workflow inspection

- Workflow name: `TitanCraft Windows MVP Export`.
- Workflow file: `.github/workflows/windows-mvp-export.yml`.
- Expected artifact name: `windows-mvp-export`.
- Expected artifact executable path: `builds/Windows/TitanCraft.exe`.
- Expected export log path: `builds/Windows/export.log`.
- Expected hash metadata path: `builds/Windows/TitanCraft.exe.sha256.txt`.
- Expected summary path: `builds/Windows/windows-mvp-export-summary.md`.

## Real GitHub Actions run evidence

- Run URL or run identifier: unavailable from this environment.
- Trigger type: not triggered or inspected.
- Commit SHA: `412a3e0ed3d7355084c69c34a0acf712df41f708`.
- Real workflow completion status: not confirmed.
- Artifact name found in a real run: not confirmed.
- Artifact downloaded: no.
- Artifact launched on Windows: no.
- Manual playtest run: no.

## Environment limitation

A real GitHub Actions run could not be triggered or inspected from this environment because the local repository has no configured Git remote and the GitHub CLI is not installed or authenticated here.

Command evidence:

```text
$ git config --get-regexp 'remote\\..*\\.url' || true
<no output>

$ gh --version || true
/bin/bash: line 1: gh: command not found

$ gh auth status || true
/bin/bash: line 1: gh: command not found
```

Because no repository URL, GitHub token, or authenticated GitHub CLI context was available, this record must not claim that the real workflow completed successfully or that a downloadable artifact exists in GitHub Actions.

## Artifact verification fields

- Artifact name: `windows-mvp-export` expected by workflow; not verified in a real run.
- Artifact files listed from downloaded artifact: unavailable because no artifact could be downloaded.
- `builds/Windows/TitanCraft.exe` present in downloaded artifact: not verified.
- `builds/Windows/export.log` present in downloaded artifact: not verified.
- `builds/Windows/TitanCraft.exe.sha256.txt` or equivalent metadata present in downloaded artifact: not verified.
- TitanCraft.exe size: unavailable.
- SHA-256 hash: unavailable.
- Export log warning summary: unavailable because `builds/Windows/export.log` was not obtained from a real workflow artifact.
- Workflow summary includes explicit non-claims: the workflow file generates `builds/Windows/windows-mvp-export-summary.md` with explicit non-claims, but the summary was not verified from a real completed run.

## Explicit non-claims

- This evidence record does not claim Windows readiness.
- This evidence record does not claim manual Windows playtest success.
- This evidence record does not claim public demo readiness.
- This evidence record does not claim signing readiness.
- This evidence record does not claim production readiness.
- This evidence record does not claim visual approval.
- This evidence record does not claim gameplay feel approval.

## Forbidden scope confirmation

- No gameplay code was modified.
- `Main.tscn` was not modified.
- Art, asset, scene, and material files were not modified.
- Mission order was not modified.
- Victory condition was not modified.
- Save semantics were not modified.
- `builds/Windows/TitanCraft.exe` was not committed to git.

## Final verdict

WINDOWS_MVP_EXPORT_CI_BLOCKED
