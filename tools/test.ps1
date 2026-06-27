$ErrorActionPreference = "Stop"
$results = "tests/TestResults"
New-Item -ItemType Directory -Force $results | Out-Null
New-Item -ItemType Directory -Force "builds/Windows" | Out-Null

dotnet restore
dotnet build --configuration Debug
dotnet build --configuration Release
dotnet test tests/TitanCraft.Tests.csproj --settings tests/TitanCraft.runsettings --logger "trx;LogFileName=unit.trx" --results-directory $results
godot --headless --path . --import --quit 2>&1 | Tee-Object "$results/import.log"
godot --headless --path . tests/Integration/IntegrationTestRunner.tscn 2>&1 | Tee-Object "$results/integration.log"
godot --headless --path . --quit-after 300 2>&1 | Tee-Object "$results/smoke.log"
godot --headless --path . --export-release "Windows Desktop" builds/Windows/TitanCraft.exe 2>&1 | Tee-Object "$results/export.log"
if (!(Test-Path "builds/Windows/TitanCraft.exe")) { throw "Windows export missing" }

$matches = Select-String -Path "$results/*" -Pattern "SCRIPT ERROR|Unhandled exception|Failed to load|Cannot get node|NullReferenceException"
if ($matches) { $matches | Format-Table; throw "Blocking error found in logs" }
