# Windows Playtest Evidence Template

Copy this template for each Windows offline MVP playtest run. This evidence record supports review of a specific exported Windows build; it must not be used to claim Windows readiness unless the recorded run actually passes the required checklist on clean Windows hardware or a real Windows VM.

## Test record

| Field | Evidence |
|---|---|
| Tester name | OpenAI GPT-5.5 agent in Linux CI/container (not a Windows tester) |
| Test date | 2026-07-04 |
| Test start/end time | 2026-07-04 UTC; manual Windows run not started because required artifact was absent in this environment |
| Windows version and build | NOT_TESTED — no real Windows machine or real Windows VM was available in this Linux container |
| Hardware or VM type | NOT_TESTED — current environment is Linux container, not an accepted Windows playtest environment |
| CPU | NOT_TESTED for Windows; Linux container reports x86_64 host architecture |
| GPU / VM graphics adapter | NOT_TESTED |
| RAM | NOT_TESTED |
| Display resolution and refresh rate | NOT_TESTED |
| Input devices | NOT_TESTED |
| Network state during test | NOT_TESTED — no Windows launch occurred; network independence could not be validated |
| Build artifact path | `builds/Windows/TitanCraft.exe` (expected by task, but missing from this repository workspace) |
| Build artifact hash algorithm | SHA-256 |
| Build artifact hash | Expected by task: `43a5787be0e14f15a0af6990ddd2c52cb65d4d34f3f0d531c2b27f298557b4c1`; actual hash NOT_COMPUTED because artifact file was missing |
| Build source commit | NOT_VERIFIED for artifact because artifact was missing |
| Screenshots/video paths or links (optional) | None; no Windows run occurred |

## Milestone results

Use `PASS`, `FAIL`, `BLOCKED`, or `NOT_TESTED` for each milestone. Add concise evidence notes and bug IDs when relevant.

| Milestone | Result | Evidence notes / bug IDs |
|---|---|---|
| Clean Windows install or real Windows VM used | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Exported `TitanCraft.exe` launches without Godot editor | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Offline launch works with no network dependency | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Main menu opens while offline | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| New Game starts Crash Site session | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Spawn near crash site succeeds | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Resource collection works for metal | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Resource collection works for biomass | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Resource collection works for electronic components | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Resource counters update clearly | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Mechanical Arm Mk I can be crafted | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Crafting consumes the expected resources | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Galaxabrain Scout encounter starts | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Combat with crafted arm works | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Galaxabrain Scout can be defeated | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Mission component can be retrieved | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Save point can be used | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Beacon can be activated | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Victory screen/state appears | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Defeat path triggers at zero health | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Defeat reload returns to last save/checkpoint | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Save/continue path resumes expected progress | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Quit and relaunch path works while offline | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Mouse and keyboard feel acceptable | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| FPS comfort acceptable for tester | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| UI prompts are clear | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| No blocking crash/freeze occurs | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| No blocking audio issue occurs | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |
| Clean quit closes window and process | BLOCKED | Blocked before manual playtest: `builds/Windows/TitanCraft.exe` is absent from the Linux workspace and no real Windows machine/VM is available here. |

## Playtest notes

### Mouse and keyboard feel

- Sensitivity / comfort: NOT_TESTED; no Windows launch occurred.
- Movement responsiveness: NOT_TESTED; no Windows launch occurred.
- Interaction clarity: NOT_TESTED; no Windows launch occurred.
- Attack feel: NOT_TESTED; no Windows launch occurred.
- Pause/menu control behavior: NOT_TESTED; no Windows launch occurred.

### FPS comfort

- Average observed FPS: NOT_TESTED.
- Worst observed FPS: NOT_TESTED.
- Measurement method: None; no Windows run occurred.
- Comfort notes: NOT_TESTED; FPS comfort cannot be claimed.

### UI prompt clarity

- Objective clarity: NOT_TESTED.
- Resource counter clarity: NOT_TESTED.
- Crafting prompt clarity: NOT_TESTED.
- Save prompt clarity: NOT_TESTED.
- Beacon prompt clarity: NOT_TESTED.
- Victory/defeat clarity: NOT_TESTED.

### Stability and audio

- Crashes: NOT_TESTED; no launch occurred.
- Freezes or hangs: NOT_TESTED; no launch occurred.
- Severe hitches: NOT_TESTED; no launch occurred.
- Soft locks: NOT_TESTED; no launch occurred.
- Audio issues: NOT_TESTED; no launch occurred.
- Clean quit result: NOT_TESTED; no launch occurred.

## Bugs found

| Bug ID | Severity | Milestone | Reproduction steps | Expected result | Actual result | Evidence path/link | Status |
|---|---|---|---|---|---|---|---|
| WMP-2026-07-04-001 | Blocker | Artifact confirmation / Windows environment | From repo root, check `builds/Windows/TitanCraft.exe`, then attempt to use a real Windows machine or real Windows VM. | Artifact exists and Windows environment is available for playtest. | Artifact is missing in this workspace and current environment is a Linux container, so manual Windows playtest cannot start. | Terminal command output in session: `if [ -e builds/Windows/TitanCraft.exe ]; then ...; else echo MISSING ...; fi; uname -a` | Open |

## Optional media evidence

| Media type | Path or link | What it proves |
|---|---|---|
| Log | Terminal output | Shows artifact missing and Linux container environment. |

## Final verdict

Choose exactly one final verdict:

- `WINDOWS_MVP_PLAYTEST_PASS` — the full MVP loop completed on Windows with no blocker bugs.
- `WINDOWS_MVP_PLAYTEST_FAIL` — Windows launched but one or more required MVP milestones failed.
- `WINDOWS_MVP_PLAYTEST_BLOCKED` — Windows playtest could not be completed due to environment, launch, export, missing dependency, or machine limitation.

Final verdict: `WINDOWS_MVP_PLAYTEST_BLOCKED`

## Reviewer notes

- Evidence reviewed by: OpenAI GPT-5.5 agent
- Review date: 2026-07-04
- Follow-up required: Provide the expected exported artifact in `builds/Windows/TitanCraft.exe` and run the checklist on a real Windows machine or real Windows VM.

## Task packet summary

- Task category: prompt_or_agent_governance; evidence category: documentation.
- Primary agent: producer.
- Secondary agents: qa_lead, technical_director.
- Required memories: MEM-PROMPT-009, MEM-GOV-001, MEM-GOV-002.
- Required skills: prompt_design, pull_request_review, evidence_reporting.
- Required evidence: requested doc objective, files changed, validation command.
- Minimum validation commands: `python3 tools/validate_agent_studio.py`, `git diff --check`.

## Non-claims

- This record does not claim public demo readiness.
- This record does not claim Windows readiness.
- This record does not claim visual approval.
- This record does not claim final gameplay feel.
- This record does not claim the Crash Site loop, defeat/retry, save/continue, or quit/relaunch paths passed.

## Blocker summary

- Artifact hash used: expected `43a5787be0e14f15a0af6990ddd2c52cb65d4d34f3f0d531c2b27f298557b4c1`; actual hash unavailable because `builds/Windows/TitanCraft.exe` was missing.
- Windows environment: unavailable; current execution environment is a Linux container.
- Full loop result: BLOCKED before launch.
- Defeat/retry result: BLOCKED before launch.
- Save/continue result: BLOCKED before launch.
- Quit/relaunch result: BLOCKED before launch.
- Bugs found: WMP-2026-07-04-001 records the missing artifact and unavailable Windows environment as playtest blockers.
