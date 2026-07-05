# TitanCraft Windows Manual MVP Playtest Evidence — 2026-07-05

## Scope guard

This record is the requested Windows manual playtest closeout attempt for the Crash Site MVP. It does not modify gameplay code, add features, polish visuals, rebalance combat, change mission order, or change save semantics. The README scope permits Windows-first offline Crash Site MVP validation, but a playable-pass verdict requires a real Windows launch and full manual completion of the exported executable.

## Task packet summary

| Field | Value |
|---|---|
| Task description | Close Windows manual MVP playtest |
| Task category | prompt_or_agent_governance |
| Evidence category | documentation |
| Primary agent | producer |
| Secondary agents | qa_lead, technical_director |
| Required memories | MEM-PROMPT-009, MEM-GOV-001, MEM-GOV-002 |
| Required skills | prompt_design, pull_request_review, evidence_reporting |
| Required evidence | requested doc objective, files changed, validation command |
| Minimum packet validation | `python3 tools/validate_agent_studio.py`, `git diff --check` |

## Windows environment

| Field | Evidence |
|---|---|
| Required environment | Real Windows environment or Windows runner capable of launching `TitanCraft.exe` |
| Available environment | Linux container only |
| Host command evidence | `uname -a` returned `Linux 761b45b689dc 6.12.47 #1 SMP Mon Oct 27 10:01:15 UTC 2025 x86_64 x86_64 x86_64 GNU/Linux` |
| Windows version | NOT_AVAILABLE — no real Windows machine, Windows VM, or Windows runner was available in this execution environment |
| Windows runner launch capability | BLOCKED — `wine` and `pwsh` were not available on PATH, and Wine would not satisfy the requested real Windows environment requirement anyway |

## Build source and artifact metadata

| Field | Evidence |
|---|---|
| Requested starting commit | `bbab9bce60aa77e4319831a1bea5de5603368a4e` |
| Repository HEAD inspected before evidence edit | `98050a853ece94b38b83726eff8d272b7a4c8112` |
| Branch | `work` |
| Latest checked-in export evidence source | `docs/release/evidence/windows-mvp-export-2026-07-04.md` |
| Workflow run URL | NOT_AVAILABLE — no workflow run URL was present in the inspected local evidence records |
| Local export path inspected | `builds/Windows/TitanCraft.exe` |
| Artifact name | `TitanCraft.exe` |
| Artifact presence in current workspace | MISSING — `find builds artifacts docs -name 'TitanCraft.exe' -o -name '*.exe'` found no executable artifact; `builds/` was also absent |
| `TitanCraft.exe` size | NOT_COMPUTED for current workspace because the executable artifact is missing |
| SHA-256 hash | NOT_COMPUTED for current workspace because the executable artifact is missing |
| Prior export-only evidence size | `114,554,496 bytes` recorded in `docs/release/evidence/windows-mvp-export-2026-07-04.md` |
| Prior export-only evidence SHA-256 | `d5187c911a57552202fcbfeb4eae40807c6b8d331ffd9dc149a9fab00fa59b2a` recorded in `docs/release/evidence/windows-mvp-export-2026-07-04.md` |

## Launch result

| Check | Result | Evidence notes |
|---|---|---|
| Launch `TitanCraft.exe` | BLOCKED | No real Windows environment was available and no local `TitanCraft.exe` artifact was present in the workspace. |
| Reach main menu | NOT_TESTED | Blocked before launch. |
| Crash/log status | NOT_TESTED | No Windows process launched, so no Windows crash log or runtime log exists for this attempt. |
| FPS/performance notes | NOT_OBSERVED | No Windows run occurred. |

## Required manual playtest milestones

| # | Milestone | Result | Evidence notes |
|---:|---|---|---|
| 1 | Launch `TitanCraft.exe` | BLOCKED | No real Windows runner and no current local executable artifact. |
| 2 | Reach main menu | NOT_TESTED | Blocked before launch. |
| 3 | Start the Crash Site scene | NOT_TESTED | Blocked before launch. |
| 4 | Move, jump, look, interact, and attack | NOT_TESTED | Blocked before launch. |
| 5 | Collect required resources | NOT_TESTED | Blocked before launch. |
| 6 | Engage the Galaxabrain Scout | NOT_TESTED | Blocked before launch. |
| 7 | Defeat the Scout | NOT_TESTED | Blocked before launch. |
| 8 | Retrieve the required component | NOT_TESTED | Blocked before launch. |
| 9 | Use the workbench | NOT_TESTED | Blocked before launch. |
| 10 | Craft the mechanical arm | NOT_TESTED | Blocked before launch. |
| 11 | Confirm the mechanical arm is visible/equipped | NOT_TESTED | Blocked before launch; prior automated screenshot artifact exists but does not satisfy this manual Windows proof. |
| 12 | Use save point | NOT_TESTED | Blocked before launch. |
| 13 | Reload or continue from save | NOT_TESTED | Blocked before launch. |
| 14 | Confirm crafted/equipped state persists | NOT_TESTED | Blocked before launch. |
| 15 | Activate the beacon | NOT_TESTED | Blocked before launch. |
| 16 | Reach victory screen | NOT_TESTED | Blocked before launch. |
| 17 | Verify defeat path if practical without blocking the main proof | NOT_TESTED | Blocked before launch. |

## Screenshots or video references

No new Windows manual screenshot or video was captured because the launch was blocked before any Windows runtime started.

Existing non-manual, non-Windows MVP closure references that remain useful as historical automated evidence only:

| Reference | What it shows | Manual Windows proof status |
|---|---|---|
| `artifacts/mvp_closure/playthrough/03_arm_crafted_visible.png` | Prior mechanical-arm visibility artifact | Does not satisfy this requested manual Windows playtest |
| `artifacts/mvp_closure/playthrough/07_victory_screen.png` | Prior victory-screen artifact | Does not satisfy this requested manual Windows playtest |
| `artifacts/mvp_closure/playthrough/08_restored_save_state.png` | Prior restored-save-state artifact | Does not satisfy this requested manual Windows playtest |

## Required follow-up for playable pass

1. Obtain the latest valid Windows export artifact from the release workflow or regenerate `builds/Windows/TitanCraft.exe` from the intended commit.
2. Record the workflow run URL or local export command/path.
3. Compute and record `stat` size and SHA-256 hash for the exact executable under test.
4. Launch the executable on real Windows hardware, a real Windows VM, or an accepted Windows runner.
5. Complete every required Crash Site MVP milestone manually, including mechanical arm visibility, save/load continuation, and victory.
6. Attach screenshots or video references for the arm visible/equipped state, save continuation, and victory screen.

## Final manual verdict

`CRASH_SITE_MVP_WINDOWS_RUNTIME_BLOCKED`

The executable could not be launched due to environment limitations. The current environment is a Linux container without a real Windows runner, and the expected local Windows executable artifact is absent from the workspace. This evidence file is committed as a blocker record, not as a playable-pass claim.
