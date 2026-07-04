# Windows MVP Export Evidence — 2026-07-04

## Scope guard

This record documents export evidence only. It does not claim Windows MVP readiness, public demo readiness, signing readiness, or manual playtest success. No gameplay, `Main.tscn`, art, asset, scene, material, mission-order, victory-condition, or save-semantics changes were made for this evidence record.

## Task packet summary

| Field | Value |
|---|---|
| Task description | Create first Windows MVP export evidence record |
| Task category | build_failure |
| Evidence category | build |
| Primary agent | build_release_engineer |
| Secondary agents | tools_engineer, qa_lead |
| Required memories | MEM-CI-RELEASE-LESSONS-004, MEM-CI-013 |
| Required skills | ci_cd_validation, production_debugging, evidence_reporting |
| Required checklists | before_task, release_readiness, before_pr |
| Required evidence | exact command output, artifact path, failure class, environment limitation if blocked |

## Pre-edit source review

| Requested source | Status |
|---|---|
| `README.md` | Read before editing; confirms Crash Site MVP and Windows offline export target. |
| `PROJECT_DIRECTOR_START_HERE.md` | Read before editing; confirms release/export tasks are fail-closed and require real export evidence. |
| `docs/release/windows-mvp-export.md` | Missing from repository at task time. |
| `docs/release/windows-mvp-playtest-runbook.md` | Missing from repository at task time. |
| `docs/testing/windows-offline-mvp-playtest.md` | Read before editing; manual Windows playtest checklist was not run. |
| `python3 tools/agent_preflight.py "Create first Windows MVP export evidence record"` | Ran before editing and routed the task to build_release_engineer. |

## Environment and export metadata

| Field | Value |
|---|---|
| Date/time UTC | 2026-07-04T21:04:17Z |
| Machine/environment | Linux container `32c3c66c4454`, kernel `6.12.47`, x86_64 GNU/Linux |
| Godot executable | `/root/.local/bin/godot` |
| Godot version | `4.7.stable.mono.official.5b4e0cb0f` |
| Export preset file | `export_presets.cfg` |
| Export preset name | `Windows Desktop` |
| Export preset path | `builds/Windows/TitanCraft.exe` |
| Export command used | `godot --headless --path . --export-release "Windows Desktop" builds/Windows/TitanCraft.exe` |
| Export command exit code | `0` |
| Artifact path | `builds/Windows/TitanCraft.exe` |
| TitanCraft.exe exists | Yes |
| Artifact size | 114,554,496 bytes |
| SHA-256 hash | `d5187c911a57552202fcbfeb4eae40807c6b8d331ffd9dc149a9fab00fa59b2a` |
| Hash command available | `tools/release/hash_artifact.py` was missing, so the required hash tool command could not run. `sha256sum builds/Windows/TitanCraft.exe` was used to record the artifact hash. |
| Artifact launched on Windows | No |
| Manual playtest run | No |
| Signing status | Not assessed in this export-only record. |
| Windows readiness claimed | No |
| Public demo readiness claimed | No |

## Command evidence

```text
$ godot --headless --path . --export-release "Windows Desktop" builds/Windows/TitanCraft.exe
Godot Engine v4.7.stable.mono.official.5b4e0cb0f - https://godotengine.org
[ DONE ] dotnet_publish_project
[ DONE ] savepack
Exit code: 0
```

The export log also reported missing temporary audio resources while packing scenes:

```text
ERROR: Failed loading resource: res://assets/audio/temp/galaxabrain_down.wav.
ERROR: Failed loading resource: res://assets/audio/temp/arm_hit.wav.
ERROR: Failed loading resource: res://assets/audio/temp/player_damage.wav.
ERROR: Failed loading resource: res://assets/audio/temp/pickup_chime.wav.
ERROR: Failed loading resource: res://assets/audio/temp/craft_confirm.wav.
ERROR: Failed loading resource: res://assets/audio/temp/beacon_activate.wav.
ERROR: Failed loading resource: res://assets/audio/temp/victory_sting.wav.
```

```text
$ python3 tools/release/hash_artifact.py builds/Windows/TitanCraft.exe
/root/.pyenv/versions/3.12.13/bin/python3: can't open file '/workspace/TitanCraft/tools/release/hash_artifact.py': [Errno 2] No such file or directory
```

```text
$ sha256sum builds/Windows/TitanCraft.exe
d5187c911a57552202fcbfeb4eae40807c6b8d331ffd9dc149a9fab00fa59b2a  builds/Windows/TitanCraft.exe
```

```text
$ stat -c '%n %s bytes' builds/Windows/TitanCraft.exe
builds/Windows/TitanCraft.exe 114554496 bytes
```

## Manual checks not performed

- `TitanCraft.exe` was not launched on Windows.
- The Windows offline MVP playtest checklist was not run.
- No Windows readiness, MVP readiness, or public demo readiness claim is made by this record.

## Final export-only verdict

`PASS` — Windows export artifact was created at `builds/Windows/TitanCraft.exe` and hashed with `sha256sum`. The repository-required `tools/release/hash_artifact.py` command is blocked because the script is missing, and Windows launch/playtest remain not run.
