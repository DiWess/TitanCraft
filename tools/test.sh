#!/usr/bin/env bash
set -euo pipefail

RESULTS="tests/TestResults"
TEMPLATE_VERSION="4.7.stable.mono"
TEMPLATE_DIR="${XDG_DATA_HOME:-$HOME/.local/share}/godot/export_templates/$TEMPLATE_VERSION"
TEMPLATE_ARCHIVE="Godot_v4.7-stable_mono_export_templates.tpz"
TEMPLATE_URL="https://github.com/godotengine/godot-builds/releases/download/4.7-stable/$TEMPLATE_ARCHIVE"
rm -rf "$RESULTS"
mkdir -p "$RESULTS" builds/Windows
export GODOT_BIN="${GODOT_BIN:-$(command -v godot)}"

install_export_templates() {
  if [[ -f "$TEMPLATE_DIR/windows_release_x86_64.exe" ]]; then
    return
  fi
  tmp_dir="$(mktemp -d)"
  curl -L -o "$tmp_dir/$TEMPLATE_ARCHIVE" "$TEMPLATE_URL"
  unzip -q "$tmp_dir/$TEMPLATE_ARCHIVE" -d "$tmp_dir"
  mkdir -p "$TEMPLATE_DIR"
  cp -R "$tmp_dir/templates/." "$TEMPLATE_DIR/"
}

install_export_templates
test -f "$TEMPLATE_DIR/windows_release_x86_64.exe"
"$GODOT_BIN" --headless --version | grep '4.7.stable.mono.official.5b4e0cb0f'
python3 - <<PY
from pathlib import Path
p = Path("tests/TitanCraft.runsettings")
s = p.read_text()
start = s.find("<GODOT_BIN>") + len("<GODOT_BIN>")
end = s.find("</GODOT_BIN>")
s = s[:start] + str(Path("tools/godot-headless.sh").resolve()) + s[end:]
p.write_text(s)
PY

dotnet restore
dotnet build --configuration Debug
dotnet build --configuration Release
dotnet test tests/TitanCraft.Tests.csproj --settings tests/TitanCraft.runsettings --logger "trx;LogFileName=unit.trx" --results-directory "$RESULTS"
"$GODOT_BIN" --headless --path . --import --quit 2>&1 | tee "$RESULTS/import.log"
"$GODOT_BIN" --headless --path . tests/Integration/IntegrationTestRunner.tscn 2>&1 | tee "$RESULTS/integration.log"
"$GODOT_BIN" --headless --path . --quit-after 300 2>&1 | tee "$RESULTS/smoke.log"
"$GODOT_BIN" --headless --path . --export-release "Windows Desktop" builds/Windows/TitanCraft.exe 2>&1 | tee "$RESULTS/export.log"
test -f builds/Windows/TitanCraft.exe

if grep -R -E "SCRIPT ERROR|Unhandled exception|Failed to load|Cannot get node|NullReferenceException" "$RESULTS"; then
  echo "Blocking error found in logs" >&2
  exit 1
fi
