# Windows MVP Export Evidence — Audio Preparation and Hash Tool — 2026-07-04

## Scope guard

This record documents Linux headless Windows export evidence only. It does not claim Windows readiness, public demo readiness, signing readiness, production readiness, or manual playtest success. No gameplay code, `Main.tscn`, art assets, scenes, materials, mission order, victory condition, or save semantics were modified for this evidence record.

## Task packet summary

| Field | Value |
|---|---|
| Task description | Fix Windows export evidence tooling and audio preparation before manual playtest |
| Task category | build_failure |
| Evidence category | build |
| Primary agent | build_release_engineer |
| Secondary agents | tools_engineer, qa_lead |
| Required memories | MEM-CI-RELEASE-LESSONS-004, MEM-PRODUCTION-STAGE-GATES-002, MEM-CI-013 |
| Required skills | ci_cd_validation, production_debugging, evidence_reporting |
| Required checklists | before_task, release_readiness, before_pr |
| Required evidence | exact command output, artifact path, failure class, environment limitation if blocked |

## Pre-edit source review

| Requested source | Status |
|---|---|
| `README.md` | Read before editing; confirms Crash Site MVP and Windows offline export target. |
| `docs/release/windows-mvp-export.md` | Missing at task start; added by this change with required audio preparation before direct Godot import/export. |
| `docs/release/windows-mvp-playtest-runbook.md` | Missing at task start; added by this change as manual-playtest handoff guidance. |
| `docs/testing.md` | Read before editing; already documented generated temporary audio cues and now includes audio preparation in the Windows export command sequence. |
| `python3 tools/agent_preflight.py "Fix Windows export evidence tooling and audio preparation before manual playtest"` | Ran before editing and again during validation; routed to `build_release_engineer`. |

## Environment and export metadata

| Field | Value |
|---|---|
| Date/time UTC | 2026-07-04T23:00:43Z |
| Environment | Linux container under `/workspace/TitanCraft` |
| Godot version | `4.7.stable.mono.official.5b4e0cb0f` |
| Export preset name | `Windows Desktop` |
| Temporary audio preparation run before export | Yes: `python3 tools/prepare_audio_assets.py` |
| Export command used | `godot --headless --path . --export-release "Windows Desktop" builds/Windows/TitanCraft.exe 2>&1 \| tee builds/Windows/export.log` |
| Export command exit code | `0` |
| Export log path | `builds/Windows/export.log` |
| Export warning/error summary | No `WARNING` or `ERROR` lines were found in `builds/Windows/export.log` after audio preparation. The previous missing temporary audio resource errors did not recur. |
| Hash tool exists | Yes: `tools/release/hash_artifact.py` |
| Hash command | `python3 tools/release/hash_artifact.py builds/Windows/TitanCraft.exe` |
| Artifact path | `builds/Windows/TitanCraft.exe` |
| Artifact size | 114,580,280 bytes |
| SHA-256 hash | `43a5787be0e14f15a0af6990ddd2c52cb65d4d34f3f0d531c2b27f298557b4c1` |
| Artifact launched on Windows | No |
| Manual Windows playtest run | No |
| Windows readiness claimed | No |
| Public demo readiness claimed | No |

## Command evidence summary

```text
$ python3 tools/prepare_audio_assets.py
Exit code: 0
```

```text
$ godot --headless --path . --import
Godot Engine v4.7.stable.mono.official.5b4e0cb0f - https://godotengine.org
[ DONE ] reimport
Exit code: 0
```

The import command produced existing OBJ ambient-light PBR warnings. Those import warnings are not missing-audio export warnings and were not introduced by this task.

```text
$ mkdir -p builds/Windows
Exit code: 0
```

```text
$ godot --headless --path . --export-release "Windows Desktop" builds/Windows/TitanCraft.exe 2>&1 | tee builds/Windows/export.log
Godot Engine v4.7.stable.mono.official.5b4e0cb0f - https://godotengine.org
[ DONE ] dotnet_publish_project
[ DONE ] savepack
Exit code: 0
```

```text
$ rg -n "WARNING|ERROR" builds/Windows/export.log
No matches.
```

```text
$ python3 tools/release/hash_artifact.py builds/Windows/TitanCraft.exe
path: builds/Windows/TitanCraft.exe
size_bytes: 114580280
sha256: 43a5787be0e14f15a0af6990ddd2c52cb65d4d34f3f0d531c2b27f298557b4c1
```

## Manual checks not performed

- `TitanCraft.exe` was not launched on Windows.
- The Windows offline MVP playtest checklist was not run.
- Signing, installer behavior, telemetry, rollback, public demo, and production release gates were not assessed.

## Final export-evidence verdict

`WINDOWS_EXPORT_EVIDENCE_CLEAN`
