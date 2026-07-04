# Windows Playtest Evidence Template

Copy this template for each Windows offline MVP playtest run. This evidence record supports review of a specific exported Windows build; it must not be used to claim Windows readiness unless the recorded run actually passes the required checklist on clean Windows hardware or a real Windows VM.

## Test record

| Field | Evidence |
|---|---|
| Tester name |  |
| Test date |  |
| Test start/end time |  |
| Windows version and build |  |
| Hardware or VM type |  |
| CPU |  |
| GPU / VM graphics adapter |  |
| RAM |  |
| Display resolution and refresh rate |  |
| Input devices |  |
| Network state during test | Offline / blocked by:  |
| Build artifact path |  |
| Build artifact hash algorithm | SHA-256 |
| Build artifact hash |  |
| Build source commit |  |
| Screenshots/video paths or links (optional) |  |

## Milestone results

Use `PASS`, `FAIL`, `BLOCKED`, or `NOT_TESTED` for each milestone. Add concise evidence notes and bug IDs when relevant.

| Milestone | Result | Evidence notes / bug IDs |
|---|---|---|
| Clean Windows install or real Windows VM used |  |  |
| Exported `TitanCraft.exe` launches without Godot editor |  |  |
| Offline launch works with no network dependency |  |  |
| Main menu opens while offline |  |  |
| New Game starts Crash Site session |  |  |
| Spawn near crash site succeeds |  |  |
| Resource collection works for metal |  |  |
| Resource collection works for biomass |  |  |
| Resource collection works for electronic components |  |  |
| Resource counters update clearly |  |  |
| Mechanical Arm Mk I can be crafted |  |  |
| Crafting consumes the expected resources |  |  |
| Galaxabrain Scout encounter starts |  |  |
| Combat with crafted arm works |  |  |
| Galaxabrain Scout can be defeated |  |  |
| Mission component can be retrieved |  |  |
| Save point can be used |  |  |
| Beacon can be activated |  |  |
| Victory screen/state appears |  |  |
| Defeat path triggers at zero health |  |  |
| Defeat reload returns to last save/checkpoint |  |  |
| Save/continue path resumes expected progress |  |  |
| Quit and relaunch path works while offline |  |  |
| Mouse and keyboard feel acceptable |  |  |
| FPS comfort acceptable for tester |  |  |
| UI prompts are clear |  |  |
| No blocking crash/freeze occurs |  |  |
| No blocking audio issue occurs |  |  |
| Clean quit closes window and process |  |  |

## Playtest notes

### Mouse and keyboard feel

- Sensitivity / comfort:
- Movement responsiveness:
- Interaction clarity:
- Attack feel:
- Pause/menu control behavior:

### FPS comfort

- Average observed FPS:
- Worst observed FPS:
- Measurement method:
- Comfort notes:

### UI prompt clarity

- Objective clarity:
- Resource counter clarity:
- Crafting prompt clarity:
- Save prompt clarity:
- Beacon prompt clarity:
- Victory/defeat clarity:

### Stability and audio

- Crashes:
- Freezes or hangs:
- Severe hitches:
- Soft locks:
- Audio issues:
- Clean quit result:

## Bugs found

| Bug ID | Severity | Milestone | Reproduction steps | Expected result | Actual result | Evidence path/link | Status |
|---|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |  |

## Optional media evidence

| Media type | Path or link | What it proves |
|---|---|---|
| Screenshot |  |  |
| Video |  |  |
| Log |  |  |

## Final verdict

Choose exactly one final verdict:

- `WINDOWS_MVP_PLAYTEST_CHECKLIST_READY` — every required milestone passed on a clean Windows install or real Windows VM, no blocking bugs remain, and the evidence fields above are complete.
- `WINDOWS_MVP_PLAYTEST_CHECKLIST_FAILED` — any required milestone failed, was blocked, was not tested, lacks evidence, or revealed a blocking bug.

Final verdict: 

## Reviewer notes

- Evidence reviewed by:
- Review date:
- Follow-up required:
