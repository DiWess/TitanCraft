# Windows Playtest Journey Verdict — 2026-07-25

**Journey run:** `windows-playtest-journey` run id `30152950604` (run #8, `workflow_dispatch`, branch `main`)
**Executed under:** `studio/decisions/quality_benchmark_v2_agent_gate_delegation.md`
**Roles:** Build Release Engineer (smoke lane), Visual Reviewer agent (aesthetic lane), Producer (final gate)

This is the **first** verdict document the journey has ever produced. The workflow was added
2026-07-11 but never dispatched until 2026-07-25; every prior run was a `push`/`pull_request` event
that executed only the validation job. See
`docs/production/windows-release-gate-analysis-2026-07-24.md`.

## Build Provenance

| Field | Value |
|---|---|
| Executable | `builds/Windows/TitanCraft.exe` |
| SHA-256 | `2a19485b93d711d3c82427c3c242ae7eca959dca0211ba1bce3e4e1af65ce6b1` |
| Commit | `5afdd2c67a65134216cc5cf2e7d491d4c2817205` (branch `main`) |
| Workflow run id | `30152950604` |
| Runner | `windows-latest`, Microsoft Windows NT 10.0.26100.0 |
| Godot | 4.7.stable.mono.official.5b4e0cb0f |
| Uploaded artifact | `windows-playtest-bundle` (id 8618195541, 43,346,016 bytes, zip digest `c856f7beba5b363729548f359683df85a295f9054e537f0753e08aa114c60c95`) |

The build was produced by `./tools/test.ps1` on the Windows runner (build, unit tests, import,
integration, export), then exported to `builds/Windows/TitanCraft.exe` and hashed in the same job.

**Evidence completeness gap.** The uploaded bundle contains **3 files**, not 4: the workflow's upload
step listed `builds/Windows/export.log`, but `tools/test.ps1` writes the export log to
`tests/TestResults/export.log`. The runner log states "With the provided path, there will be 3 files
uploaded", confirming the export log is absent from this bundle. Because the other upload patterns
matched real files, `if-no-files-found: error` did not fire and the run stayed green. Fixed on
`claude/agent-studio-ownership-rights-ou8s4q`; this run predates that fix, so the export log for this
particular build exists only in the job log, not in the artifact.

## Windows Smoke Evidence

Source: `smoke_report.json` from the `windows-playtest-bundle` artifact, as printed verbatim by the
"Measured Windows smoke run" step of run `30152950604`.

```json
{
  "executable": "builds/Windows/TitanCraft.exe",
  "sha256": "2a19485b93d711d3c82427c3c242ae7eca959dca0211ba1bce3e4e1af65ce6b1",
  "sha": "5afdd2c67a65134216cc5cf2e7d491d4c2817205",
  "run_id": "30152950604",
  "workflow": "windows-playtest-journey",
  "runner_os": "windows-latest",
  "runner_os_version": "Microsoft Windows NT 10.0.26100.0",
  "smoke_exit_code": 0,
  "smoke_frames": 600,
  "smoke_wall_seconds": 4.64,
  "frames_per_second_proxy": 129.3,
  "windows_capture_png": null
}
```

- **exit code 0** from the exported Windows binary — it launched outside the editor on real Windows
  and ran 600 frames to a clean exit in 4.64 wall seconds.
- `frames_per_second_proxy` of 129.3 is a **headless frame-pacing proxy**, not a rendered frame rate.
  Per the governing ADR it must never be presented as a rendered-performance claim, and it does
  **not** satisfy the README § 28 target of 60 FPS on a Windows test machine. No rendered frame-rate
  figure exists for this build.
- `windows_capture_png` reads `null` above because the report was printed before the best-effort
  on-screen capture step patched it. That step reported success and wrote
  `artifacts/playtest-journey/windows_onscreen_capture.png` into the artifact. Its **contents were
  not opened for this verdict** and no claim is made about them.

### README § 27 criteria — what this smoke does and does not establish

| Criterion | Status |
|---|---|
| Launches without the IDE | ✓ established — the exported exe ran standalone on the runner |
| Build directory identifiable | ✓ established — `builds/Windows/TitanCraft.exe` |
| No account, API key, or authentication | ✓ no such prompt is reachable in a 600-frame headless run, and none exists in the MVP |
| Runs with no Internet connection | ✗ not established — the runner had network access; offline was never enforced |
| Local save works in the build | ✗ not established — a headless frame-count run does not exercise save |
| Resume from a save after relaunch | ✗ not established — the binary was launched once |
| Clean shutdown | ✓ partially — exit code 0 after `--quit-after`, which is not the same as quitting via the in-game menu |

## Aesthetic Verdict

Issued by the **Visual Reviewer** agent role on opened PNGs.

**Capture provenance — read this before citing the verdict.** The `aesthetic-capture-bundle`
artifact (id 8618184893) could not be downloaded from this session: artifact download requires the
GitHub API, which this environment's proxy blocks, and no MCP artifact-download tool exists. The
PNGs judged below were therefore **regenerated locally from the identical commit**
`5afdd2c67a65134216cc5cf2e7d491d4c2817205`, using the same allowlisted factory
(`python3 tools/visual_review/run_visual_artifact_factory.py`, exit 0, 28 PNGs) and the same Godot
4.7 build that CI used. They are equivalent in content but are **not** the CI artifact bytes. Any
claim requiring the CI artifact specifically is not satisfied by this document.

Files opened for this review:

- `artifacts/visual-review/phase3a-production-integration/production_01_spawn_overview.png`
- `artifacts/visual-review/phase3a-production-integration/production_02_crashed_ship_hero.png`
- `artifacts/visual-review/phase3a-production-integration/production_04_resource_workbench_zone.png`
- `artifacts/visual-review/phase3a-production-integration/production_06_galaxabrain_combat_distance.png`
- `artifacts/visual-review/phase3a-production-integration/production_07_mechanical_arm_first_person.png`
- `artifacts/visual-review/crafted-arm-first-person/crafted_arm_01_camp_backdrop.png`

**What reads correctly.** Composition holds across the elevated views: the crashed hull is the focal
mass, light poles and the yellow beacon beam give vertical structure and a legible objective marker,
and the palette stays within the locked identity (desaturated hull and terrain, orange functional
accents, confined cyan and violet emissives). Silhouettes are distinguishable — the Galaxabrain
Scout reads as a spindly quadruped against the terrain at combat distance in
`production_06_galaxabrain_combat_distance.png`, and the workbench reads as the crafting hub in
`production_04_resource_workbench_zone.png`. Resource pickups are identifiable by colour at
gameplay distance. No route slab is presented as terrain, and no toy-like decorated hull appears.

**Confirmed fix.** `crafted_arm_01_camp_backdrop.png` shows the crafted mechanical arm rendering as
a genuine first-person viewmodel in the lower right, with no legacy proxy mesh over it. The
BLOCKER-class regression recorded in `docs/production/polish-slice-2026-07-18.md` holds fixed at
this commit.

**MAJOR — ground-plane shadow acne at gameplay framing.** `crafted_arm_01_camp_backdrop.png` and
`production_04_resource_workbench_zone.png` both show dense horizontal light/dark banding across the
ground plane, covering a large share of the visible floor. It is a depth-precision or shadow-bias
artifact at grazing camera angles, not a material or texture choice. It is **absent** from the
elevated overview `production_01_spawn_overview.png`, which is why earlier elevated-camera reviews
did not surface it. This was checked against the same view rendered from an older tree and is
**not** a regression introduced by the recent auto-forge asset commits — the elevated view is
unchanged between the two. The problem is that the affected angles are first-person and near-ground
framing, which is what a player sees for most of the session.

**MINOR.** `production_07_mechanical_arm_first_person.png` still does not show a first-person arm —
it frames a cylinder resting on the ground. This is the Stage C capture gap; the dedicated
`crafted-arm-first-person/` set supersedes it, and the stale view should be retired from the
production-integration set to stop it being re-reported.

Verdict: `NOT_GO`

The scene direction is sound and the arm fix is confirmed, but a depth artifact covering the ground
plane in the primary gameplay camera is not a defect a release build should carry, and it must be
diagnosed before any visual approval or marketing claim.

## Feel Evidence

No human has played this build. No subjective claim about handling, responsiveness, or combat is
made anywhere in this document, per ADR lane 3.

Measured proxies standing in for feel, all from this run or the repo-standard suite at the same
commit:

- Smoke exit code 0 from the exported Windows binary.
- 600 frames rendered headless in 4.64 wall seconds (frame-pacing proxy 129.3 frames/second — a
  proxy metric, not a rendered frame rate).
- Unit suite 75/75 and all 11 `MVP_SMOKE_MILESTONE` entries passing on the integrated scenes
  (`./tools/test.ps1` on the Windows runner, same job).
- Godot import exit 0 with 0 errors.

These establish that the loop runs to completion without crashing on Windows. They establish nothing
about how the game handles under a player, and README § 11 combat tuning remains provisional.

## Final Verdict

Verdict: `NOT_GO`

The machine lanes of the journey succeeded and this is the first real Windows evidence the project
has: a genuine Windows-runner build that launches standalone and completes a 600-frame smoke with
exit code 0, with full hash and run provenance. That is a real advance on the release gate.

It is not a ship decision. Three things block `GO`:

1. **Aesthetic `NOT_GO`** — ground-plane shadow acne across first-person and near-ground framing.
2. **README § 28 unmet** — no rendered frame-rate measurement exists on Windows hardware; the only
   number available is an explicitly-labelled headless proxy.
3. **README § 27 partially unestablished** — offline operation, local save in the build, and resume
   after relaunch are not exercised by a headless frame-count smoke.

Recommended next actions, in order: diagnose the depth artifact; extend the measured smoke to cover
save, relaunch, and resume in the exported build; and add a rendered frame-rate measurement path or
record explicitly that README § 28 requires a human on real hardware. The export-log upload fix is
already committed on `claude/agent-studio-ownership-rights-ou8s4q` and will apply to the next run.
