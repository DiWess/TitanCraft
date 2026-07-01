# Phase 4 — Build and Export Evidence

## Commands run (via `tools/test.sh`, matching `.github/workflows/ci.yml` linux job)

```bash
dotnet restore
dotnet build --configuration Debug
dotnet build --configuration Release
dotnet test tests/TitanCraft.Tests.csproj --settings tests/TitanCraft.runsettings --logger "trx;LogFileName=unit.trx" --results-directory tests/TestResults
godot --headless --path . --import --quit
godot --headless --path . tests/Integration/IntegrationTestRunner.tscn
godot --headless --path . --quit-after 300
godot --headless --path . --export-release "Windows Desktop" builds/Windows/TitanCraft.exe
```

## Results

Raw logs (`tests/TestResults/*.log`, `*.trx`) are excluded from version control by the
repository's own `.gitignore` (`*.log`, `*.trx`) and are not committed here; they are fully
reproducible by running `tools/test.sh` locally, which is exactly what produced the results
below (last run against commit `85cbd40c227d4f489834efce14ac7cadd2208738`).

| Gate | Result | Evidence |
|---|---|---|
| `dotnet restore` | PASS | (no errors) |
| `dotnet build` (Debug + Release) | PASS, 0 warnings / 0 errors | (no errors) |
| Unit tests (GdUnit4, `dotnet test`) | PASS — 49/49 | `tests/TestResults/unit.trx` (reproducible, not committed) |
| Godot headless import | PASS | `tests/TestResults/import.log` (reproducible, not committed) |
| Integration/runtime scene tests (`IntegrationTestRunner.tscn`) | PASS — ends `TITANCRAFT_INTEGRATION_TESTS_PASS` | `tests/TestResults/integration.log` (reproducible, not committed) |
| General smoke run (`--quit-after 300`) | PASS — no errors | `tests/TestResults/smoke.log` (reproducible, not committed) |
| Windows export | PASS | `tests/TestResults/export.log` / `artifacts/mvp_closure/export/export_summary.md` |
| Log sweep for `SCRIPT ERROR\|Unhandled exception\|Failed to load\|Cannot get node\|NullReferenceException` | PASS — no matches | (checked across all logs above) |

## Windows artifact identity

| Field | Value |
|---|---|
| Executable path | `builds/Windows/TitanCraft.exe` |
| Adjacent required files | `builds/Windows/data_TitanCraft_windows_x86_64/` (187 files, ~77 MB — Mono/.NET runtime + `GodotSharp.dll` assemblies required by the Mono export template; must ship alongside the `.exe`) |
| File size (`.exe`) | 114,594,984 bytes (~109 MiB) |
| SHA-256 (`.exe`) | `bbe37ebc86c71dc592f780938f7e03aea47bb6b1cb68acfd36618725914a0cae` |
| File type | `PE32+ executable (GUI) x86-64, for MS Windows` (confirmed via `file`) |
| Godot version | 4.7.stable.mono.official.5b4e0cb0f |
| .NET SDK version | 8.0.422 |
| Export template version | `4.7.stable.mono` (matches Godot version; templates present at `~/.local/share/godot/export_templates/4.7.stable.mono`) |
| Export preset | `Windows Desktop` (`export_presets.cfg`, `preset.0`), architecture `x86_64`, `embed_pck=true` |
| Source commit SHA | see `artifacts/mvp_closure/final_mvp_verdict.json` (`source_commit_sha`) |

## Windows launch result

**ENVIRONMENT_BLOCKED.** This container is Linux-only with no Wine/Windows runtime available (`which wine wine64` → not found), so `builds/Windows/TitanCraft.exe` cannot itself be executed here to prove a native Windows launch. This matches the project's own CI design: `.github/workflows/ci.yml` only launches the exported `.exe` natively on the `windows-latest` runner job (`Native exported smoke` step), never on Linux.

What **was** proven in this environment, and does not substitute for the above:
- The same Godot engine version (4.7.stable.mono) ran the same compiled game logic and the same `Main.tscn` scene graph headlessly (`tests/TestResults/smoke.log`) and under real (Xvfb/OpenGL, software-rendered) display (see `artifacts/mvp_closure/runtime_playthrough_results.json` and screenshots) without script errors, exceptions, or missing-node failures.
- The export step itself completed without error and produced a well-formed Windows PE32+ binary of plausible size with all adjacent runtime files.

Remaining manual action: run `builds/Windows/TitanCraft.exe` on an actual Windows machine (or via Wine) and confirm it launches, matches the "Windows offline export checklist" in README §27, and exits cleanly.
