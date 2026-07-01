# TitanCraft Crash Site MVP — Baseline Report (Phase 0)

## Environment

| Item | Value |
|---|---|
| Branch | `claude/titancraft-mvp-closure-hua2nu` |
| Baseline commit SHA | `107dfd642b787c5a63923060e1e1fa6ca2566b02` |
| Working tree | clean at baseline |
| OS | Linux 6.18.5 (container), x86_64 |
| .NET SDK | 8.0.422 |
| Godot | 4.7.stable.mono.official.5b4e0cb0f (headless, `.NET`/Mono build) |
| Export templates | `4.7.stable.mono` present, including `windows_release_x86_64.exe` / `windows_release_x86_64_console.exe` |

## Gate results at baseline

| Gate | Command | Result |
|---|---|---|
| Restore | `dotnet restore` | PASS |
| Build | `dotnet build` | PASS (0 warnings, 0 errors) |
| Unit tests | `dotnet test tests/TitanCraft.Tests.csproj --settings tests/TitanCraft.runsettings` | PASS — 44/44 |
| Godot headless import | `godot --headless --path . --import` | PASS |
| Integration/runtime scene tests | not yet run at baseline (see Phase 4/5) — driven by `tests/Integration/IntegrationTestRunner.tscn` via `tools/test.sh` |
| Windows export | not yet run at baseline (see Phase 4) |

The repository already ships a CI pipeline (`.github/workflows/ci.yml`, `tools/test.sh`) that runs restore → build (Debug+Release) → unit tests → Godot import → the `IntegrationTestRunner` scene (a real headless run of the Main scene exercising movement, mission wiring, HUD, save/load, collision, and procedural terrain contracts) → a general smoke run → a Windows export, then greps logs for script errors/exceptions. This is the mechanism used for Phase 4/5 evidence.

## Findings

### BLOCKER — Mission step "DefeatGalaxabrain" is completed by component pickup, not by enemy death

- **File:** `src/World/GalaxabrainComponentPickup.cs` (`Interact`), `src/Enemies/GalaxabrainScout.cs` (`Die`)
- **Behavior:** `GalaxabrainScout.Die()` only makes the mission-component pickup visible/monitoring; it never touches `CrashSiteMissionState`. `GalaxabrainComponentPickup.Interact()` calls **both** `mission.TryCompleteGalaxabrainDefeat(true)` **and** `mission.TryCompleteComponentRecovery()` in a single player interaction.
- **Impact:** Violates the required semantics ("Galaxabrain death completes only the enemy-defeat step", "component interaction completes only component recovery"). Practically: after the player kills the Galaxabrain, the HUD objective still reads "Defeat Galaxabrain" instead of "Recover Galaxabrain Component" until the player also interacts with the corpse — misleading real-time feedback, and the two steps are not independently observable/idempotent as required. No existing test (unit or integration) exercises this path end-to-end; `TestEndScreenNavigation` in `IntegrationTestRunner.cs` calls `mission.TryCompleteGalaxabrainDefeat`/`TryCompleteComponentRecovery` directly, bypassing the actual gameplay wiring, so the bug is not caught.
- **Fix (Phase 1):** Move `TryCompleteGalaxabrainDefeat` into `GalaxabrainScout.Die()`, and restrict `GalaxabrainComponentPickup.Interact()` to `TryCompleteComponentRecovery()` only.

### BLOCKER (P1 per task priority model) — Mechanical arm first-person visual is never toggled

- **File:** `scenes/Player/Player.tscn` (`MechanicalArmVisual` node, `Head/Camera3D/MechanicalArmVisual`), `src/Player/FirstPersonController.cs`
- **Behavior:** The scene defines a `MeshInstance3D` named `MechanicalArmVisual` with `visible = false` hardcoded. No C# file references this node name (`grep -r MechanicalArmVisual src/` = no matches). `MvpInventory.IsMechanicalArmBuilt` is never connected to it.
- **Impact:** After crafting the Mechanical Arm Mk I, the first-person view never shows the arm — required visual feedback from Phase 2 is entirely unimplemented, independent of attack availability (which is correctly gated on `Inventory.IsMechanicalArmBuilt` inside `MechanicalArmAttackLogic`/`FirstPersonController.TryAttack`).
- **Fix (Phase 2):** Bind `MechanicalArmVisual.Visible` to `Inventory.IsMechanicalArmBuilt` via the existing `Inventory.Changed` event in `FirstPersonController`, refreshed on `_Ready` (covers both crafting and load-restore).

### MINOR — Dead/duplicate save validation code

- **File:** `src/SaveSystem/CrashSiteSaveService.cs` (+ `tests/Unit/CrashSiteSaveServiceTests.cs`)
- **Behavior:** `CrashSiteSaveService` is a fully-formed alternate save/load path with its own `IsValid` (which, unlike `LocalSaveGameStore.IsValid`, does not require `Health > 0`). It is not referenced by `CrashSiteSaveCoordinator`, any scene, or any other production code — only by its own unit tests.
- **Impact:** Not a runtime defect (unreachable), but a maintenance/drift risk: two independently-evolving save-validation implementations. Left unmodified per the "single feature per task / no unnecessary refactor" rule; flagged for a future decision rather than fixed now.

### SUSPECTED — Save cross-field consistency is not enforced

- **File:** `src/SaveSystem/LocalSaveGameStore.cs` (`IsValid`)
- **Behavior:** `IsValid` checks version, health, non-negative resources, and a defined `MissionStep`, but never checks that `MechanicalArmBuilt`/`GalaxabrainComponentCollected` are consistent with `MissionStep` (e.g., a hand-edited or corrupted save could set `MissionStep = ActivateBeacon` while `GalaxabrainComponentCollected = false`).
- **Impact:** `CrashSiteMissionState.Restore` derives all step-completion flags purely from `MissionStep` (safe), and `Beacon`/`GalaxabrainComponentPickup` gate on both mission step and inventory flags (fails closed — a mismatched save cannot skip a step or double-trigger victory), so this does not currently corrupt gameplay. It can, however, leave the player permanently unable to progress (e.g., beacon ungated by mission step but `HasGalaxabrainComponent` false forever). Addressed in Phase 3 with an explicit cross-field validation rule (reject and fall back to a clean state, matching the existing "invalid → start new run" pattern) plus a regression test.

### Optional polish (P2, out of scope for this closure pass)

- OBJ import warnings ("Ambient light ... ignored in PBR") during `--import` are cosmetic importer warnings from third-party assets, not blocking.
- `docs/debug/phase3a-broken-integration-analysis.md` and related phase3a docs describe prior visual-integration issues already resolved by later commits; not re-litigated here.

## Scope confirmation

No new maps, enemies, resources, crafting recipes, or systems are proposed. All fixes above are targeted corrections to existing MVP-scoped code (mission sequencing, arm visual feedback, save validation), consistent with README §5/§6 and CLAUDE.md §2/§6.
