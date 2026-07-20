# Stage C Integration Validation & Release-Gate Evidence — 2026-07-18

**Roles executed:** QA Lead / Gameplay validation (Task #7a), Visual Reviewer (Task #7b),
Build Release Engineer (Task #8 export proof), Producer (gate summary)
**Prerequisite:** Stage B → C `PASS` (`docs/production/stage-b-producer-gate-2026-07-18.md`)
**Routed packet:** build_release_engineer primary, tools_engineer / qa_lead secondary

## Task #6 finding — integration already present

The Level Designer integration the Stage B gate routed as "next" was found **already wired**
in the production scenes by prior committed work (v1-beta/auto-forge slices). This slice
therefore validated the existing integration instead of re-integrating:

| Approved candidate | Production wiring (verified 2026-07-18) |
|---|---|
| TC_PROP_Workbench_V1 | `scenes/World/Workbench.tscn` → `V1BetaWorkbenchModel` instance |
| TC_PLAYER_MechanicalArmUnbuilt_V1 | `scenes/World/Workbench.tscn` unbuilt-arm visual root |
| TC_PROP_Beacon_Dormant_V1 / _Active_V1 | `scenes/World/Beacon.tscn` closed/active visuals (state swap) |
| TC_CHAR_GalaxabrainScout_V1 / _Disabled_V1 | `scenes/Enemies/GalaxabrainScout.tscn` alive/disabled visual roots |
| TC_PICKUP_Component_V1 | Scout scene `GalaxabrainComponentPickup/VisualRoot` |
| TC_PICKUP_Metal_V1 / _Electronics_V1 / _Biomass_V1 | `scenes/Resources/ResourceDrop.tscn` VisualGroup; per-pickup visibility overrides verified in `Main.tscn` (correct model visible, placeholder `ItemMesh` hidden, per type) |
| TC_PLAYER_MechanicalArm_V1 | `scenes/Player/Player.tscn` first-person arm resource |
| TC_PROP_SavePoint_V1 | `Main.tscn` `Placeholder_SavePoint/VisualRoot` |
| TC_ENV_CrashDebris_A_V1 | `Main.tscn` route-opening debris instances |

All references use the committed text `.gltf` deliverables under `assets/models/mvp_pack_v1/`
per the pack brief's scene-delivery contract. Gameplay collision nodes were not modified.

## Task #7a — integration validation (real command output)

| Check | Command | Result |
|---|---|---|
| Unit tests | `./tools/test.sh` (dotnet test stage) | Passed 75 / Failed 0 / Skipped 0 |
| Godot integration + MVP smoke | `./tools/test.sh` (headless Godot stage) | exit 0; all 11 `MVP_SMOKE_MILESTONE` entries verified in `tests/TestResults/integration.log`: spawn, three resource pickups, crafting, combat, component recovery, save point, beacon activation, victory, defeat path, save continuation |
| Import | `godot --headless --path . --import` | exit 0, 0 errors |

The smoke run exercises the real integrated scenes (`Main.tscn`, Workbench, Beacon, Scout,
ResourceDrop) end-to-end, which validates that the visual integration did not break any
gameplay contract.

## Task #6b/#7b — in-engine captures and Visual Reviewer diagnosis

Captures generated locally with the allowlisted factory
(`python3 tools/visual_review/run_visual_artifact_factory.py`, xvfb; exit 0). Eight
production-integration views under `artifacts/visual-review/phase3a-production-integration/`
with SHA-256 hashes in `artifacts/visual-review/visual_artifact_factory_scene_manifest.json`
(directory is a local/CI artifact per repo binary policy; regenerate with the command above).

All eight PNGs were opened for this review:

- **Spawn overview / crashed-ship hero / wide composition:** camp reads as a coherent
  destination (hull mass, light poles, awning, crate stacks); beacon sky-beam is a clear
  objective marker; volcanic silhouettes and alien crystals frame the route; palette holds
  (desaturated structure + orange functional accents + confined cyan/violet emissives).
- **Resource/workbench zone:** workbench with robot arm and holo panel reads as the
  crafting hub; crate stacks and awning give it mass; biomass (red cluster) and electronics
  pickups are identifiable at gameplay distance; savepoint cyan is visible and confined.
- **Galaxabrain combat distance:** scout silhouette (spindly quadruped) is identifiable
  against the terrain at combat range; crystals and beacon beam preserve orientation.
- **Ship rear engines:** hull volume and lighting read correctly; no paper-thin panels.

**Findings (honest gaps):**

1. **`production_07_mechanical_arm_first_person.png` does not show the mechanical arm** —
   the capture frames the scene at ground level without the crafted-arm state. The arm's
   integration is validated by scene wiring (`Player.tscn` reference) and the crafting smoke
   milestone, but not by an opened in-engine image. Follow-up: extend the capture script
   with a crafted-state first-person view. (MAJOR gap for marketing/visual-approval claims;
   MINOR for integration validation since the wiring and runtime path are test-covered.)
2. Foreground ground planes are large and empty in several views — polish/composition
   backlog, consistent with the known 4.3/10 quality-benchmark composite; no regression.
3. The wide shot shows the map's island edges — expected for an isolated capture; not a
   player-visible defect inside the playable bounds.

**Visual verdict:** `PASS` for integrated-composition coherence at MVP placeholder-tolerant
quality (no rejection patterns; readability preserved), with finding #1 recorded as an
explicit evidence gap that blocks any *visual-approval* claim for the first-person arm.

## Task #8 — Windows export proof (Build Release Engineer)

| Step | Command | Result |
|---|---|---|
| Audio prep | `python3 tools/prepare_audio_assets.py` | exit 0 |
| Export | `godot --headless --path . --export-release "Windows Desktop" builds/Windows/TitanCraft.exe` | exit 0, 0 ERROR lines |
| Artifact | `builds/Windows/TitanCraft.exe` | 122,501,896 bytes + `data_TitanCraft_windows_x86_64/`; SHA-256 `1acf91dd855a8f82538aacca23196eae00f36e031437eeec8335ff0514c8fd40` |

`builds/` stays local per policy (gitignored). The `Export Windows MVP artifact` CI job on
PR #129 provides the reproducible CI copy of this proof.

## Producer gate summary

- **Stage C (integration + validation): `PASS`** — integration verified, gameplay loop
  green end-to-end on the integrated scenes, captures opened and diagnosed.
- **Release gate: `HUMAN_BLOCKED`** — export proof exists, but README §27/§29 requires
  manual Windows validation (launch outside Godot, offline loop completion, save/quit/
  resume) and the studio requires a human GO for deployment. The first-person-arm capture
  gap (finding #1) must also be closed before public visual claims.

**Next actions:** (1) human Windows playthrough per `docs/testing.md` § Required Windows
manual validation; (2) crafted-arm first-person capture added to the factory allowlist;
(3) Biomass V2 albedo/glow note from the Stage B review, addressable in the same polish
slice.
