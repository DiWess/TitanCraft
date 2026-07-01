# Phase 3A playability baseline

Baseline verdict after reverting broken production wiring: `PLAYABLE_BASELINE_RESTORED`.

## Commands and results

To be refreshed on every future visual pass:

- `dotnet restore` — PASS as part of `./tools/test.sh`.
- `dotnet build` — PASS in Debug and Release as part of `./tools/test.sh`; standalone `dotnet build` also passed.
- `godot --headless --path . --import` — PASS with OBJ PBR ambient-light warnings only.
- `godot --headless --path . tests/Integration/IntegrationTestRunner.tscn` — PASS; output contained `TITANCRAFT_INTEGRATION_TESTS_PASS`. `./tools/test.sh` — PASS including unit tests (44), integration runner, headless smoke, and Windows export.

## Programmatic coverage

The integration contract tests validate main scene load, player/camera ownership, enemy world-space ownership, enemy movement, collision preservation, pickup/workbench/save/beacon scripts and collisions, arm camera-local ownership without collision, and a deterministic mission progression path.

## Manual test still required outside container

A human must still play in Godot/Windows to confirm feel: main menu, New Game, move, jump, collect all resources, craft at workbench, defeat Galaxabrain, collect component, save, activate beacon, victory, defeat/reload, and Continue.
