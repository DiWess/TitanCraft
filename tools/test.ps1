$ErrorActionPreference = "Stop"
$results = "tests/TestResults"
$templateVersion = "4.7.stable.mono"
$templateArchive = "Godot_v4.7-stable_mono_export_templates.tpz"
$templateUrl = "https://github.com/godotengine/godot-builds/releases/download/4.7-stable/$templateArchive"
$templateRoot = Join-Path $env:APPDATA "Godot/export_templates/$templateVersion"
if (Test-Path $results) { Remove-Item -Recurse -Force $results }
New-Item -ItemType Directory -Force $results | Out-Null
New-Item -ItemType Directory -Force "builds/Windows" | Out-Null
$env:GODOT_BIN = if ($env:GODOT_BIN) { $env:GODOT_BIN } else { (Get-Command godot).Source }

function Install-ExportTemplates {
    $releaseTemplate = Join-Path $templateRoot "windows_release_x86_64.exe"
    if (Test-Path $releaseTemplate) { return }
    $tmp = New-Item -ItemType Directory -Force (Join-Path ([System.IO.Path]::GetTempPath()) ([System.Guid]::NewGuid().ToString()))
    $archive = Join-Path $tmp.FullName $templateArchive
    Invoke-WebRequest -Uri $templateUrl -OutFile $archive
    Expand-Archive $archive -DestinationPath $tmp.FullName
    New-Item -ItemType Directory -Force $templateRoot | Out-Null
    Copy-Item -Recurse -Force (Join-Path $tmp.FullName "templates/*") $templateRoot
}

Install-ExportTemplates
if (!(Test-Path (Join-Path $templateRoot "windows_release_x86_64.exe"))) { throw "Windows export template missing" }
& $env:GODOT_BIN --headless --version | Select-String '4.7.stable.mono.official.5b4e0cb0f'
$wrapper = (Resolve-Path "tools/godot-headless.sh").Path
$settings = "tests/TitanCraft.runsettings"
$content = Get-Content $settings -Raw
$content = $content -replace "<GODOT_BIN>.*?</GODOT_BIN>", "<GODOT_BIN>$wrapper</GODOT_BIN>"
Set-Content -Path $settings -Value $content

dotnet restore
dotnet build --configuration Debug
dotnet build --configuration Release
dotnet test tests/TitanCraft.Tests.csproj --settings tests/TitanCraft.runsettings --logger "trx;LogFileName=unit.trx" --results-directory $results
& $env:GODOT_BIN --headless --path . --import --quit 2>&1 | Tee-Object "$results/import.log"
& $env:GODOT_BIN --headless --path . tests/Integration/IntegrationTestRunner.tscn 2>&1 | Tee-Object "$results/integration.log"
& $env:GODOT_BIN --headless --path . --quit-after 300 2>&1 | Tee-Object "$results/smoke.log"
& $env:GODOT_BIN --headless --path . --export-release "Windows Desktop" builds/Windows/TitanCraft.exe 2>&1 | Tee-Object "$results/export.log"
if (!(Test-Path "builds/Windows/TitanCraft.exe")) { throw "Windows export missing" }

$matches = Select-String -Path "$results/*" -Pattern "SCRIPT ERROR|Unhandled exception|Failed to load|Cannot get node|NullReferenceException"
if ($matches) { $matches | Format-Table; throw "Blocking error found in logs" }
