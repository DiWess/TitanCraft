$ErrorActionPreference = "Stop"
$results = "tests/TestResults"
$templateVersion = "4.7.stable.mono"
$templateArchive = "Godot_v4.7-stable_mono_export_templates.tpz"
$templateUrl = "https://github.com/godotengine/godot-builds/releases/download/4.7-stable/$templateArchive"
$templateRoot = Join-Path $env:APPDATA "Godot/export_templates/$templateVersion"
if (Test-Path $results) { Remove-Item -Recurse -Force $results }
New-Item -ItemType Directory -Force $results | Out-Null
New-Item -ItemType Directory -Force "builds/Windows" | Out-Null

function Assert-LastExitCode {
    param([string]$Step)

    if ($LASTEXITCODE -ne 0) {
        throw "$Step failed with exit code $LASTEXITCODE"
    }
}

if ($env:GODOT_BIN) {
    if (Test-Path $env:GODOT_BIN) {
        $godotExe = (Resolve-Path $env:GODOT_BIN).Path
    } else {
        $godotCommand = Get-Command $env:GODOT_BIN -ErrorAction SilentlyContinue

        if (-not $godotCommand) {
            throw "GODOT_BIN cannot be resolved: $env:GODOT_BIN"
        }

        $godotExe = $godotCommand.Source
    }
} else {
    $godotExe = (Get-Command godot -ErrorAction Stop).Source
}

if (-not (Test-Path $godotExe)) {
    throw "Godot executable does not exist: $godotExe"
}

if ([System.IO.Path]::GetExtension($godotExe) -ne ".exe") {
    throw "Windows GODOT_BIN must point to a .exe: $godotExe"
}

$env:GODOT_BIN = $godotExe

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
& $godotExe --headless --version | Select-String '4.7.stable.mono.official.5b4e0cb0f'
Assert-LastExitCode "Godot version check"

dotnet restore
Assert-LastExitCode "dotnet restore"
dotnet build --configuration Debug
Assert-LastExitCode "dotnet build Debug"
dotnet build --configuration Release
Assert-LastExitCode "dotnet build Release"
dotnet test tests/TitanCraft.Tests.csproj --settings tests/TitanCraft.runsettings --logger "trx;LogFileName=unit.trx" --results-directory $results
Assert-LastExitCode "dotnet test"
& $godotExe --headless --path . --import --quit 2>&1 | Tee-Object "$results/import.log"
Assert-LastExitCode "Godot import"
& $godotExe --headless --path . tests/Integration/IntegrationTestRunner.tscn 2>&1 | Tee-Object "$results/integration.log"
Assert-LastExitCode "Godot integration"
& $godotExe --headless --path . --quit-after 300 2>&1 | Tee-Object "$results/smoke.log"
Assert-LastExitCode "Godot smoke"
& $godotExe --headless --path . --export-release "Windows Desktop" builds/Windows/TitanCraft.exe 2>&1 | Tee-Object "$results/export.log"
Assert-LastExitCode "Windows export"
if (!(Test-Path "builds/Windows/TitanCraft.exe")) { throw "Windows export missing" }

$matches = Select-String -Path "$results/*" -Pattern "SCRIPT ERROR|Unhandled exception|Failed to load|Cannot get node|NullReferenceException"
if ($matches) { $matches | Format-Table; throw "Blocking error found in logs" }
