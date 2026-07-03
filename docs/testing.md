# Testing TitanCraft

TitanCraft uses a minimal layered test setup.

## Structure

- `tests/Unit/`: GdUnit4Net tests that run through `dotnet test` without a Godot runtime.
- `tests/Integration/`: Godot headless scene tests that load real `.tscn` scenes, instantiate nodes, run physics frames, simulate input, and fail with a non-zero exit code.
- `tests/TestResults/`: generated logs and TRX files ignored by Git.
- `tools/test.sh` and `tools/test.ps1`: local smoke-test entry points.

## Linux commands

```bash
./tools/test.sh
```

## Windows PowerShell commands

```powershell
./tools/test.ps1
```

## What is covered

Unit tests cover FPS movement math: pitch clamp, normalized directions, diagonal-speed limits, and parameter validity.
Integration tests cover the real Godot scenes, collision nodes, light, player capsule/camera, physics landing, movement actions, jump, camera clamp, and InputMap configuration.

## Manual control check

Movement keeps both keyboard layouts active for local playtesting:

- QWERTY/WASD: `W` forward, `A` left, `S` backward, `D` right.
- AZERTY/ZQSD: `Z` forward, `Q` left, `S` backward, `D` right.

`Q` is reserved for left movement in the AZERTY/ZQSD layout and must not trigger critical gameplay actions such as `quit_game`.


## Manual placeholder readability check

Use this check after visual changes to `scenes/Main/Main.tscn`:

1. Launch the Crash Site scene in Godot and stand near the player spawn.
2. Look toward each `Placeholder_*` interactive from several meters away without collecting it first.
3. Confirm metal is readable as a grey metallic block, biomass as a green rounded object, electronics as a blue cylinder, the workbench as an orange bench-like block, the save point as a purple low cylinder, and the beacon as a tall yellow marker.
4. Walk around each object and confirm the simple interaction collisions do not behave like decorative walls or block the route through the crash site.



## Generated temporary audio cues

The MVP keeps temporary WAV cues as generated artifacts instead of tracked binary assets. Before opening scenes directly in the Godot editor or running any direct Godot import command outside `tools/test.sh`, materialize the local cues from the repository root:

```bash
python3 tools/prepare_audio_assets.py
```

`tools/test.sh` and `tools/test.ps1` run this setup automatically for normal validation. Do not commit `assets/audio/temp/*.wav` unless a human explicitly decides to replace the generated-audio approach with tracked WAV assets and records their license/source notes.

## Reusable visual validation procedure

Use this procedure after any future phase that changes visible scenes, UI, materials, VFX, lighting, props, or environment composition. It complements automated tests; it does not replace gameplay review.

1. Run `godot --headless --path . --import` and confirm the project imports without fatal errors.
2. Validate every `assets/Materials/*.tres` resource loads as a `StandardMaterial3D` before reviewing scene visuals.
3. Launch the Crash Site scene in an interactive Godot environment and watch the output log for new runtime errors or warnings.
4. Walk the main mission route: spawn, resources, workbench, save point, Galaxabrain zone, component pickup, beacon, victory/defeat flow when applicable.
5. Confirm gameplay collisions remain predictable: no decorative mesh blocks the required route, pickup access, workbench access, enemy combat space, or beacon access.
6. Confirm interaction raycasts still work at the existing interaction range for pickups, workbench, save point, mission component, and beacon.
7. Capture before/after screenshots when available from these positions: spawn toward crash/base, pickup cluster, workbench, save point, alien route, enemy threat distance, inactive beacon, active beacon, HUD at 720p, and HUD at 1080p.
8. Check 1280x720 readability for HUD text, objective, resource counters, prompt, mechanical-arm state, menus, victory, and defeat screens.
9. Check 1920x1080 readability for the same UI elements and verify decorative patterns do not compete with text.
10. Compare screenshots against `docs/visual-technical-bible.md` and `docs/visual-review-checklist.md` for palette, human/alien distinction, orange/cyan usage, cultural integrity, geometry density, lighting, and VFX budgets.
11. Measure FPS in an interactive environment when available and record hardware, resolution, render settings, average FPS, and worst observed FPS. If no interactive graphics environment is available, state that limitation explicitly.
12. Defer Windows offline validation until a real Windows machine or real Windows VM with the exported build is available; Linux headless checks are not a substitute for Windows readiness.


## Windows offline MVP build verification

Use this process before describing a local Windows MVP build as validated. These commands assume the Godot 4 .NET executable is available as `godot` on `PATH`; if the local executable has a versioned name, substitute that exact executable without changing the arguments.

### Automated local checks before export

Run from the repository root:

```bash
dotnet restore
dotnet build
godot --headless --path . --import
mkdir -p builds/Windows
godot --headless --path . --export-release "Windows Desktop" builds/Windows/TitanCraft.exe
```

Expected automated result:

- `dotnet restore` completes without dependency errors.
- `dotnet build` completes without compilation errors.
- `godot --headless --path . --import` imports the project without fatal errors.
- `mkdir -p builds/Windows` ensures the ignored local export directory exists before export.
- The Godot Windows export writes `builds/Windows/TitanCraft.exe` using the `Windows Desktop` export preset.

If export templates are missing, install the matching Godot export templates locally and rerun the export command. Do not commit the generated `builds/` output unless a human explicitly changes the project policy.

### Required Windows manual validation

After generating the executable, validate on a Windows PC without using the Godot editor:

1. Disconnect from the Internet or otherwise confirm the game has no active network requirement.
2. Launch `builds/Windows/TitanCraft.exe` directly from File Explorer or PowerShell.
3. Confirm the main menu opens.
4. Select **New Game** and confirm the Crash Site session starts.
5. Complete the Crash Site victory loop: collect the required resources, craft the Mechanical Arm Mk I, defeat the Galaxabrain Scout, collect the mission component, activate the beacon, and reach the victory screen.
6. Start or reload a run, trigger player defeat, and confirm the defeat reload returns to the last save/checkpoint instead of blocking progress.
7. Relaunch the executable while still offline and confirm local save/continue behavior works when a save exists.
8. Quit the application and confirm it closes cleanly.

### Pass criteria and readiness wording

The Windows offline MVP build passes local verification only when all of these are true on an actual Windows machine:

- the standalone executable opens without the Godot editor;
- the main menu opens;
- **New Game** works;
- the full Crash Site victory loop can be completed;
- defeat reload works;
- local save/continue behavior works when applicable;
- the app works offline with no account, server, key, or Internet connection;
- quitting closes the app cleanly.

Do not claim Windows readiness, Windows validation, or Windows release suitability until the generated `builds/Windows/TitanCraft.exe` has been run through the manual Windows checklist above on real Windows hardware or a real Windows VM. Linux headless import/export checks can support the build process, but they are not a substitute for the Windows run.

## CI

GitHub Actions should run the same restore, Debug/Release builds, unit tests, Godot headless integration test, import, export, log scan, and artifact upload on Linux and Windows.

## Interpreting failures

- `dotnet test` failures indicate pure C# behavior regressions.
- Integration log failures indicate scene, node, physics, or input regressions.
- Export failures indicate missing templates, invalid export settings, or build errors.

## Adding tests

Add pure logic tests under `tests/Unit/`. Add runtime scene behavior checks to `tests/Integration/IntegrationTestRunner.cs` only when a real Godot scene or physics behavior must be exercised.

## Headless limits

Headless tests validate loading, physics, input simulation, and finite transforms. They do not replace human gameplay review for FPS feel, mouse comfort, or Windows build UX before distribution.

## GdUnit analyzer compatibility note

`tests/TitanCraft.Tests.csproj` keeps `gdUnit4.analyzers` enabled and temporarily suppresses `CS9057` only. Local verification without that suppression shows `gdUnit4.analyzers` 1.0.0 is loaded, but the analyzer assembly references Roslyn compiler 4.14 while the current Godot .NET 8 build path runs compiler 4.11. A NuGet check on 2026-06-28 found no newer `gdUnit4.analyzers` package than 1.0.0, so the suppression remains targeted and temporary instead of disabling GdUnit diagnostics. Remove it once the Godot SDK/compiler toolchain or analyzer package versions are compatible.
