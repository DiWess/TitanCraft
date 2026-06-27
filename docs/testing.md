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
