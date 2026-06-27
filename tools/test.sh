#!/usr/bin/env bash
set -euo pipefail

RESULTS="tests/TestResults"
mkdir -p "$RESULTS" builds/Windows

dotnet restore
dotnet build --configuration Debug
dotnet build --configuration Release
dotnet test tests/TitanCraft.Tests.csproj --settings tests/TitanCraft.runsettings --logger "trx;LogFileName=unit.trx" --results-directory "$RESULTS"
godot --headless --path . --import --quit 2>&1 | tee "$RESULTS/import.log"
godot --headless --path . tests/Integration/IntegrationTestRunner.tscn 2>&1 | tee "$RESULTS/integration.log"
godot --headless --path . --quit-after 300 2>&1 | tee "$RESULTS/smoke.log"
godot --headless --path . --export-release "Windows Desktop" builds/Windows/TitanCraft.exe 2>&1 | tee "$RESULTS/export.log"
test -f builds/Windows/TitanCraft.exe

if grep -R -E "SCRIPT ERROR|Unhandled exception|Failed to load|Cannot get node|NullReferenceException" "$RESULTS"; then
  echo "Blocking error found in logs" >&2
  exit 1
fi
