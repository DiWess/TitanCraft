# TitanCraft Windows Playable Proof — 2026-07-05

## Task packet summary

| Field | Value |
|---|---|
| Task | Close Windows runtime proof |
| Task category | `prompt_or_agent_governance` |
| Primary agent | `producer` |
| Required memories | `MEM-PROMPT-009`, `MEM-GOV-001`, `MEM-GOV-002` |
| Required skills | `prompt_design`, `pull_request_review`, `evidence_reporting` |
| Required evidence | Requested document objective, files changed, validation command |
| Forbidden scope observed | No gameplay code, mission logic, save semantics, features, visual polish, or balance changes were made. |

## Windows runtime environment

| Field | Value |
|---|---|
| Windows version | `UNAVAILABLE` |
| Machine type | `UNAVAILABLE` — this session ran in a Linux container, not an Azure Windows VM, physical Windows PC, or interactive Windows runner. |
| Local host evidence | `Linux d447d0768a7e 6.12.47 #1 SMP Mon Oct 27 10:01:15 UTC 2025 x86_64 x86_64 x86_64 GNU/Linux` |
| Runtime access verdict | `CRASH_SITE_MVP_WINDOWS_RUNTIME_BLOCKED` |

## Commit and artifact verification

| Field | Value |
|---|---|
| Requested gameplay commit | `bbab9bce60aa77e4319831a1bea5de5603368a4e` |
| Evidence-document commit basis | Current workspace commit before this evidence update: `665e739a710da113b7fddd209f8ad84a29dafe18` |
| Workflow run URL or artifact source | `UNAVAILABLE` — `gh` is not installed in this container and no remote/artifact URL was available from the workspace. |
| Artifact name | `TitanCraft.exe` |
| Expected local path inspected | `builds/Windows/TitanCraft.exe` |
| Extracted folder path | `UNAVAILABLE` — no artifact archive was available to extract in this workspace. |
| Artifact presence | `MISSING` — `find builds artifacts docs -name 'TitanCraft.exe' -o -name '*.exe'` found no executable artifact; `builds/` was absent. |
| `TitanCraft.exe` file size | `UNAVAILABLE` — executable was missing. |
| SHA-256 hash | `UNAVAILABLE` — executable was missing. |
| Export log summary | `UNAVAILABLE` — no extracted export log was available in this workspace for the required artifact. |

## Screenshot or video references

| Evidence type | Path or URL | Status |
|---|---|---|
| Screenshot | `UNAVAILABLE` | Not captured because no Windows runtime was available and no `TitanCraft.exe` artifact was present locally. |
| Video | `UNAVAILABLE` | Not captured because no Windows runtime was available and no `TitanCraft.exe` artifact was present locally. |

## Required Windows manual playtest checklist

| # | Milestone | Status | Evidence note |
|---:|---|---|---|
| 1 | Main menu loads. | `BLOCKED` | No Windows runtime and no local executable artifact. |
| 2 | Crash Site starts. | `BLOCKED` | No Windows runtime and no local executable artifact. |
| 3 | Player can move, jump, look, interact, and attack. | `BLOCKED` | No Windows runtime and no local executable artifact. |
| 4 | Required resources are collected. | `BLOCKED` | No Windows runtime and no local executable artifact. |
| 5 | Galaxabrain Scout is engaged. | `BLOCKED` | No Windows runtime and no local executable artifact. |
| 6 | Scout is defeated. | `BLOCKED` | No Windows runtime and no local executable artifact. |
| 7 | Required component is retrieved. | `BLOCKED` | No Windows runtime and no local executable artifact. |
| 8 | Workbench is used. | `BLOCKED` | No Windows runtime and no local executable artifact. |
| 9 | Mechanical arm is crafted. | `BLOCKED` | No Windows runtime and no local executable artifact. |
| 10 | Mechanical arm becomes visible/equipped. | `BLOCKED` | No Windows runtime and no local executable artifact. |
| 11 | Save point is used. | `BLOCKED` | No Windows runtime and no local executable artifact. |
| 12 | Continue/reload preserves valid state. | `BLOCKED` | No Windows runtime and no local executable artifact. |
| 13 | Beacon is activated. | `BLOCKED` | No Windows runtime and no local executable artifact. |
| 14 | Victory screen is reached. | `BLOCKED` | No Windows runtime and no local executable artifact. |

## Crash and log notes

No Windows crash log, Godot runtime log, or Windows Event Viewer entry was produced because `TitanCraft.exe` could not be launched in this Linux-only session.

## Manual verification limitation

The requested proof specifically requires a real Windows runtime. This environment is Linux-only and does not provide an Azure Windows VM, physical Windows PC, or interactive Windows runner. Therefore, the playable pass verdict cannot be claimed.

## Final manual verdict

`CRASH_SITE_MVP_WINDOWS_RUNTIME_BLOCKED`
