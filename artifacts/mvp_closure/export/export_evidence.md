# Windows Offline Export Evidence

| Item | Value |
| --- | --- |
| Command | `godot --headless --path . --export-release "Windows Desktop" builds/Windows/TitanCraft.exe` (via `tools/test.sh`) |
| Result | Success (exit 0, no export errors; full log: `artifacts/mvp_closure/build/export.log`) |
| Executable path | `builds/Windows/TitanCraft.exe` (gitignored; identified by hash below) |
| File size | 114 946 208 bytes |
| SHA-256 | `84d47d8668a06159c4b305c5efa919b6b09b8eb3b3ecace919b271d785a0bacf` |
| PE header | Valid (`MZ`) |
| Source commit SHA | `eab008ac1efc9931c5f6157d9d6165a4e6af1263` |
| Godot version | 4.7.stable.mono.official.5b4e0cb0f |
| .NET SDK | 8.0.422 (target `net8.0`) |
| Export template version | 4.7.stable.mono (`windows_release_x86_64.exe` present) |
| Export preset | `Windows Desktop`, x86_64, `embed_pck=true` |
| Required adjacent files | `builds/Windows/data_TitanCraft_windows_x86_64/` (187 files: TitanCraft.dll, GodotSharp.dll, .NET runtime assemblies) — must ship next to the exe |
| Launch result | **ENVIRONMENT_BLOCKED** in this Linux container (no Wine, no Windows host). CI `windows` job (`.github/workflows/ci.yml`) performs a native exported smoke (`TitanCraft.exe --headless --quit-after 120`) on `windows-latest`. |

Offline property: the project contains no network services; the export requires no
internet access at runtime (verified by code inspection and the offline container run
of the identical project through the Godot runtime).
