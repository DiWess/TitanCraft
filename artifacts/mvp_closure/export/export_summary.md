# Phase 4 — Windows Export Summary

See `artifacts/mvp_closure/build/build_export_evidence.md` for the full command list, gate results, and Windows launch evidence/blocker.

| Field | Value |
|---|---|
| Command | `godot --headless --path . --export-release "Windows Desktop" builds/Windows/TitanCraft.exe` |
| Result | PASS |
| Executable path | `builds/Windows/TitanCraft.exe` |
| File size | 114,594,984 bytes |
| SHA-256 | `bbe37ebc86c71dc592f780938f7e03aea47bb6b1cb68acfd36618725914a0cae` |
| Godot version | 4.7.stable.mono.official.5b4e0cb0f |
| .NET version | 8.0.422 |
| Export template version | 4.7.stable.mono |
| Required adjacent files | `builds/Windows/data_TitanCraft_windows_x86_64/` (Mono runtime + GodotSharp assemblies, 187 files, ~77 MB) |
| Native Windows launch | ENVIRONMENT_BLOCKED — no Wine/Windows runtime in this container; see `build_export_evidence.md` |

The raw Godot export command output (`tests/TestResults/export.log`) is reproducible via `tools/test.sh` but is not committed — the repository's `.gitignore` excludes `*.log`.
