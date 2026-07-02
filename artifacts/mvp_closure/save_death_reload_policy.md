# Crash Site MVP — Save, Death, and Reload Policy

## Rule (README §17/§18 compliant)

Death leads to the defeat screen. **Reload Last Save** loads `Main.tscn` and restores the
latest valid save-point state. When no save exists, the same action explicitly starts a
new run (button relabelled "Start New Crash Site Run" by `EndScreen._Ready`).

Flow: `PlayerHealth` reaches 0 → `CrashSiteEndScreenNavigator` → `DefeatScreen.tscn` →
`EndScreen.ReloadLastSave()` → `Main.tscn` → `CrashSiteSaveCoordinator._Ready()` →
`LoadGameIfPresent()`.

## Persisted MVP state (decided)

| State | Persisted | Restoration policy |
| --- | --- | --- |
| Player transform | Position (X/Y/Z) | Restored exactly. Look direction not persisted (respawn reorientation acceptable for MVP). |
| Player health | Yes | Restored to the value at save time. Saves with `Health <= 0` are rejected by `LocalSaveGameStore.IsValid`, so a reload can never spawn a dead player. |
| Inventory quantities | Yes | Restored exactly. |
| Mechanical arm ownership | Yes (written) | **Derived from mission step on load** (`step >= DefeatGalaxabrain`). |
| Mission state | Yes (`MissionStep`) | Authoritative field. `CrashSiteMissionState.Restore` infers completed objectives. |
| Enemy defeated | Yes (`IsGalaxabrainDefeated`, written) | **Derived from mission step on load** (`step >= RecoverGalaxabrainComponent`); applied via `GalaxabrainScout.RestoreDefeated`. |
| Component recovered/available | Yes (`GalaxabrainComponentCollected`, written) | Collected derived from `step >= ActivateBeacon`; pickup available only at `RecoverGalaxabrainComponent`. |
| Beacon activated | Yes (`IsBeaconActivated`, written) | Derived from `step >= Victory`; applied via `Beacon.RestoreActivated`. |
| Save-point identity | Yes (`CheckpointId`) | Recorded for diagnostics/reload target. |

## Why load-time flags derive from the mission step

Every flag mutation in the game advances the mission step inside the same interaction
(craft → step, enemy death → step, pickup → step, beacon → step), so in every save the
game writes, flags and step agree — deriving from the step is lossless. For tampered or
corrupted files it guarantees no impossible state can be reconstructed:

- mission says "recover component" while the enemy is alive → impossible (defeat derived true at that step);
- component collected but still interactable → impossible (`IsComponentAvailable` false once collected/step advanced);
- arm in inventory but attack disabled or hidden → impossible (single authority: `MvpInventory.IsMechanicalArmBuilt`, visual bound to its `Changed` event);
- beacon active before component recovery → impossible (derived only at `Victory`).

Files that fail structural validation (bad JSON, wrong version, `Health <= 0`, negative
resources, unknown step) are rejected wholesale by `LocalSaveGameStore.TryLoad` and the
run starts fresh — invalid data is never partially applied.

Implementation: `src/SaveSystem/CrashSiteStateReconciler.cs` (pure, unit-tested in
`tests/Unit/CrashSiteStateReconcilerTests.cs`), applied by `CrashSiteSaveCoordinator`.
