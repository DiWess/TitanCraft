# Windows MVP Export Workflow

This document defines the repeatable workflow for producing a TitanCraft Crash Site MVP Windows export artifact. It prepares export evidence only; it does not claim Windows MVP readiness.

## Scope and non-readiness disclaimer

Use this workflow only for the locked Crash Site MVP described in `README.md` and `docs/production/mvp-scope.md`.

Exact non-readiness disclaimer:

> Creating a Windows export does not prove Windows readiness. Linux/headless CI does not prove Windows readiness. Manual Windows playtest is required. Visual approval is separate. Gameplay feel approval is separate. Public demo readiness is not claimed.

Do not use this workflow to authorize multiplayer, cloud services, telemetry, multiple maps, multiple enemy types, wall running, grappling hooks, voxels, a large mech, a complete rocket, or any other forbidden MVP expansion.

## Canonical export preset and artifact path

The canonical Godot export preset is:

- Preset name: `Windows Desktop`
- Preset file: `export_presets.cfg`
- Expected local artifact path: `builds/Windows/TitanCraft.exe`
- Expected GitHub Actions Windows artifact bundle: `windows-build-and-test-results`
- Expected GitHub Actions Linux validation artifact bundle: `linux-test-results`

The exported executable embeds the `.pck` according to the current preset, so the primary artifact to hash for the MVP playtest record is `builds/Windows/TitanCraft.exe` unless a later preset intentionally changes the packaging model.

## Prerequisites

Use the same toolchain contract as CI unless a release lead documents a deliberate exception:

1. Godot 4.7 .NET / Mono matching the CI version check.
2. .NET SDK 8.x.
3. Godot 4.7 Mono export templates installed for `4.7.stable.mono`.
4. Repository checkout at the source commit being tested.
5. No uncommitted gameplay, scene, art, asset, mission-order, victory-condition, or save-semantics changes.

## Produce the Windows export artifact locally

From the repository root, run the full validation/export helper when possible:

```bash
./tools/test.sh
```

That helper restores/builds/tests/imports/smokes and exports the Windows preset to:

```text
builds/Windows/TitanCraft.exe
```

If a release engineer needs to run only the export step after prerequisites and validation have already been handled, use the same preset and output path:

```bash
godot --headless --path . --export-release "Windows Desktop" builds/Windows/TitanCraft.exe
```

A standalone export command is export evidence only. It does not replace automated tests, import checks, manual Windows playtest evidence, visual approval, or gameplay feel approval.

## Produce or retrieve the CI artifact

The CI workflow uploads exported files under the `windows-build-and-test-results` artifact from the Windows job and `linux-test-results` from the Linux job. For a manual Windows offline playtest, prefer the Windows job artifact when available, then extract it into a fresh folder on the Windows test machine or VM before launch.

Record the workflow run URL, commit SHA, artifact bundle name, and extracted executable path in the playtest evidence record.

## Record artifact name, timestamp, and SHA-256 hash

Use UTC timestamps for artifact records. Record the artifact before launching it.

On Linux/macOS or Git Bash:

```bash
python3 tools/release/hash_artifact.py builds/Windows/TitanCraft.exe
```

Equivalent direct command:

```bash
sha256sum builds/Windows/TitanCraft.exe
```

On Windows PowerShell:

```powershell
Get-FileHash -Algorithm SHA256 .\builds\Windows\TitanCraft.exe
```

Record at minimum:

| Field | Required value |
|---|---|
| Artifact name | `TitanCraft.exe` or the downloaded CI artifact bundle name plus extracted executable name |
| Artifact path | Absolute or repository-relative path to the exact tested executable |
| Timestamp | UTC time when the artifact was created, downloaded, or hashed |
| Hash algorithm | `SHA-256` |
| Hash | Full SHA-256 digest |
| Source commit | `git rev-parse HEAD` for the exported checkout or CI commit SHA |
| Export command or CI workflow | Exact command or workflow run URL |

## PASS / FAIL / BLOCKED for export evidence

Use these meanings only for the export workflow, not for Windows readiness:

- `PASS`: the export command completed, `builds/Windows/TitanCraft.exe` exists, its SHA-256 hash was recorded, and the source commit/artifact timestamp were recorded.
- `FAIL`: repo-owned export, preset, build, import, or validation behavior prevented the artifact from being produced or hashed.
- `BLOCKED`: external environment prevented the workflow from running or proving the artifact metadata, such as missing Godot, missing export templates, unavailable CI artifact download, or no access to the intended Windows test machine.

## What this proves and does not prove

This workflow can prove that a specific command or CI job produced a named Windows executable and that the executable hash was recorded.

It does not prove:

- Windows MVP readiness;
- offline launch on a clean Windows install or real Windows VM;
- completion of the Crash Site victory, defeat, save, continue, quit, and relaunch paths;
- visual approval;
- gameplay feel approval;
- public demo readiness;
- signing, storefront, installer, rollback, telemetry, beta, or production readiness.

Continue with `docs/release/windows-mvp-playtest-runbook.md` before any Windows offline MVP readiness claim is considered.
