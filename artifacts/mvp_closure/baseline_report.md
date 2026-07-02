# TitanCraft Crash Site MVP Closure — Baseline Report (Phase 0)

## Environment

| Item | Value |
| --- | --- |
| Branch | `claude/titancraft-static-qa-wyhc5i` |
| Baseline commit SHA | `3b5a7dc61c04c14e1590fb90430371d077538677` |
| Working tree | clean at baseline |
| OS | Linux 6.18.5 x86_64 (containerized, no display; xvfb available; no Wine) |
| .NET SDK | 8.0.422 |
| Godot | 4.7.stable.mono.official.5b4e0cb0f |
| Export templates | `~/.local/share/godot/export_templates/4.7.stable.mono` present (incl. `windows_release_x86_64.exe`) |
| Export preset | `Windows Desktop` → `builds/Windows/TitanCraft.exe`, embed_pck, x86_64 |

## Baseline gate results (at 3b5a7dc)

| Gate | Result |
| --- | --- |
| `dotnet restore` | PASS |
| `dotnet build` | PASS (0 warnings, 0 errors) |
| `dotnet test` | PASS (44/44) |
| `godot --headless --path . --import` | PASS (exit 0) |

## Confirmed defects

### D1 (P0) — Galaxabrain defeat and component recovery are collapsed into one interaction
- `GalaxabrainScout.Die()` reveals the component but never advances mission state.
- `GalaxabrainComponentPickup.Interact()` runs at step `DefeatGalaxabrain` and calls
  `TryCompleteGalaxabrainDefeat(true)` immediately followed by `TryCompleteComponentRecovery()`.
- Consequence: after the enemy dies the HUD objective still reads "Defeat Galaxabrain";
  the `RecoverGalaxabrainComponent` step is never player-visible; two mission steps are
  completed by a single interaction, violating the required explicit progression.

### D2 (P0) — Enemy defeated state is not persisted
- `CrashSiteSaveData.IsGalaxabrainDefeated` and `IsBeaconActivated` exist but
  `CrashSiteSaveCoordinator.SaveGame()` never writes them and `LoadGameIfPresent()` never
  applies them to the scene.
- Consequence: kill the scout, recover the component, save, reload → mission resumes at
  `ActivateBeacon` but the scout respawns alive and its component pickup is hidden again.
  README §18 requires persisting "état de l'ennemi".

### D3 (P1) — MechanicalArmVisual is never made visible
- `scenes/Player/Player.tscn` has `MechanicalArmVisual` with `visible = false`.
- No code references the node; crafting and save-load leave it hidden forever.
- Consequence: no first-person visual confirmation that the arm exists, and load-time
  visual state disagrees with authoritative `MvpInventory.IsMechanicalArmBuilt`.

## Suspected defects
- None additional at baseline. Beacon, workbench, resource pickups, save store validation
  (`Health > 0`), HUD binding, and end-screen navigation are correctly gated in code and
  covered by the existing integration runner.

## Not defects (verified compliant)
- Death flow: README §17 explicitly specifies "écran de défaite → rechargement du dernier
  point de sauvegarde". The implemented flow (death → DefeatScreen → Reload Last Save →
  Main → `LoadGameIfPresent()`) matches. The no-save fallback ("Start New Crash Site Run")
  is explicit. Kept as-is; documented in Phase 3.
- Scout `PlayerPath` is wired in `Main.tscn` (`NodePath("../Player")`).
- `CrashSiteMissionStep` numeric ordering supports the `>=` restore inference.
- Resource pickups grant exactly the recipe cost (10 metal / 3 biomass / 2 electronics).

## Missing runtime evidence (to be produced)
- Windows export produced from the tested commit with recorded identity (SHA-256, size).
- Windows launch: **environment-blocked** (no Wine in container); CI `windows` job performs
  a native exported smoke on `windows-latest`.
- Full runtime playthrough of the gameplay loop (will be executed via the Godot runtime on
  Linux headless through the integration harness; on-Windows manual pass remains for a human).
- Screenshots (xvfb rendering pass, where the renderer permits).

## Optional polish (out of scope for closure, P2)
- Audio cues (none exist; README: audio non-priority).
- Placeholder node naming (`Placeholder_*`) — functional, marked, and load-bearing for
  NodePaths and integration tests; renaming is deferred deliberately.
- Decorative asset upgrades.

## Planned corrections (smallest correct implementation)
1. `GalaxabrainScout.Die()` advances `TryCompleteGalaxabrainDefeat` via the already-configured
   player reference; `GalaxabrainComponentPickup` gates on `RecoverGalaxabrainComponent` and
   completes only component recovery.
2. `CrashSiteSaveCoordinator` writes and applies enemy-defeated / component-available /
   beacon state, with a pure `CrashSiteStateReconciler` preventing invalid combinations.
3. `FirstPersonController` binds `MechanicalArmVisual.Visible` to `MvpInventory.Changed`.
4. Regression tests for each corrected deterministic defect (unit + integration runner).
