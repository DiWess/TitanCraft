# Windows MVP Export CI Artifact

## Workflow

- Workflow name: `TitanCraft Windows MVP Export`.
- Workflow file: `.github/workflows/windows-mvp-export.yml`.
- Artifact name: `windows-mvp-export`.

This workflow creates a downloadable Windows export artifact for manual Crash Site MVP playtesting. It is CI export evidence only, not readiness evidence.

## Trigger method

The workflow can be started in two ways:

1. `workflow_dispatch` from the GitHub Actions UI.
2. `pull_request` when one of these paths changes:
   - `src/**`
   - `scenes/**`
   - `tests/**`
   - `tools/**`
   - `docs/release/**`
   - `project.godot`
   - `export_presets.cfg`
   - `.github/workflows/windows-mvp-export.yml`

## What the workflow does

The CI job checks out the repository, installs .NET, installs Godot 4.7 .NET with Windows export templates, restores and builds the C# project, prepares generated audio assets, imports Godot assets, exports the `Windows Desktop` preset, verifies that `builds/Windows/TitanCraft.exe` exists, hashes the executable, and uploads the build folder as the `windows-mvp-export` artifact.

## Expected artifact files

The uploaded `windows-mvp-export` artifact is expected to include:

- `builds/Windows/TitanCraft.exe`
- `builds/Windows/export.log`
- `builds/Windows/TitanCraft.exe.sha256.txt`
- `builds/Windows/windows-mvp-export-summary.md`
- any generated `.pck` or sidecar files in `builds/Windows/`, if Godot creates them for the export preset

The current `Windows Desktop` export preset embeds the PCK into the executable, so a separate `.pck` may not be present.

## Download procedure

1. Open the repository on GitHub.
2. Open the **Actions** tab.
3. Select **TitanCraft Windows MVP Export**.
4. Open the completed workflow run.
5. In **Artifacts**, download `windows-mvp-export`.
6. Extract the downloaded artifact archive locally.
7. Confirm the extracted folder contains `builds/Windows/TitanCraft.exe`, `builds/Windows/export.log`, and hash or summary metadata.

## Copying to a Windows machine or VM

1. Copy the extracted `builds/Windows/` folder to a clean Windows machine or real Windows VM.
2. Keep the executable, `export.log`, hash text file, summary markdown, and any sidecar files together in the same folder.
3. Recalculate or compare the executable hash against `builds/Windows/TitanCraft.exe.sha256.txt` before launch.
4. Do not launch from the Godot editor for manual playtest evidence.

## Continue into manual Windows playtesting

After the artifact is copied to Windows, continue with `docs/release/windows-mvp-playtest-runbook.md`, which points to `docs/testing/windows-offline-mvp-playtest.md` and the evidence template.

Manual playtest evidence must record the artifact path, hash, Windows environment, offline state, Crash Site victory loop, defeat path, save/continue behavior, readability notes, stability notes, and the approved playtest checklist verdict.

## Explicit non-claims

Creating a CI Windows export artifact does not prove Windows readiness.
Manual Windows playtest is still required.
Visual approval is separate.
Gameplay feel approval is separate.
Public demo readiness is not claimed.

This workflow also does not claim signing readiness, production readiness, release readiness, or manual playtest success.
