#!/usr/bin/env bash
# Idempotent Linux toolchain setup for TitanCraft (Godot 4 .NET + C# + the
# Blender Asset Forge).
#
# Installs, only if missing/wrong version:
#   - .NET SDK (channel matches DOTNET_CHANNEL below)
#   - Godot 4 .NET (mono) headless-capable editor binary
#   - Godot export templates for that same Godot version
#   - Blender, for tools/blender/*.py (docs/pipeline/blender-asset-forge.md)
#
# Then warms the local build: `dotnet restore`, temp audio assets, and a
# headless Godot import pass, so `dotnet build` / `tools/test.sh` can run
# immediately afterwards without extra setup steps.
#
# Versions here must stay in sync with .github/workflows/ci.yml,
# .github/workflows/windows-mvp-export.yml, .github/workflows/blender-asset-forge.yml,
# and tools/test.sh.
set -euo pipefail

DOTNET_CHANNEL="8.0"
GODOT_VERSION="4.7-stable"
GODOT_VERSION_STRING="4.7.stable.mono.official.5b4e0cb0f"
GODOT_TEMPLATE_VERSION="4.7.stable.mono"
BLENDER_VERSION="4.0.2"

TOOLS_DIR="${TITANCRAFT_TOOLS_DIR:-$HOME/.local/share/titancraft-tools}"
BIN_DIR="${TITANCRAFT_BIN_DIR:-$HOME/.local/bin}"
REPO_ROOT="${CLAUDE_PROJECT_DIR:-$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)}"

SUDO=""
if [[ "$(id -u)" -ne 0 ]] && command -v sudo >/dev/null 2>&1; then
  SUDO="sudo"
fi

log() { echo "[setup_environment] $*"; }

mkdir -p "$TOOLS_DIR" "$BIN_DIR"

link_bin() {
  # link_bin <target> <name>
  ln -sf "$1" "$BIN_DIR/$2"
}

install_dotnet() {
  if command -v dotnet >/dev/null 2>&1 && dotnet --list-sdks 2>/dev/null | grep -q "^${DOTNET_CHANNEL%.*}\."; then
    log "dotnet SDK $(dotnet --version) already on PATH, skipping install."
    return
  fi
  if [[ -x "$TOOLS_DIR/dotnet/dotnet" ]] && "$TOOLS_DIR/dotnet/dotnet" --list-sdks 2>/dev/null | grep -q "^${DOTNET_CHANNEL%.*}\."; then
    log "dotnet SDK already installed at $TOOLS_DIR/dotnet, linking."
    link_bin "$TOOLS_DIR/dotnet/dotnet" dotnet
    return
  fi
  log "Installing .NET SDK $DOTNET_CHANNEL into $TOOLS_DIR/dotnet"
  curl -fsSL https://dot.net/v1/dotnet-install.sh -o "$TOOLS_DIR/dotnet-install.sh"
  chmod +x "$TOOLS_DIR/dotnet-install.sh"
  "$TOOLS_DIR/dotnet-install.sh" --channel "$DOTNET_CHANNEL" --install-dir "$TOOLS_DIR/dotnet"
  link_bin "$TOOLS_DIR/dotnet/dotnet" dotnet
}

install_godot() {
  local godot_exe="$TOOLS_DIR/godot-$GODOT_VERSION/Godot_v${GODOT_VERSION}_mono_linux_x86_64/Godot_v${GODOT_VERSION}_mono_linux.x86_64"
  if [[ -x "$godot_exe" ]] && "$godot_exe" --headless --version 2>/dev/null | grep -q "$GODOT_VERSION_STRING"; then
    log "Godot $GODOT_VERSION already installed at $godot_exe, linking."
    link_bin "$godot_exe" godot
    link_bin "$godot_exe" godot4
    return
  fi
  log "Installing Godot $GODOT_VERSION (mono) into $TOOLS_DIR/godot-$GODOT_VERSION"
  local archive="$TOOLS_DIR/Godot_v${GODOT_VERSION}_mono_linux_x86_64.zip"
  curl -fsSL -o "$archive" "https://github.com/godotengine/godot-builds/releases/download/${GODOT_VERSION}/Godot_v${GODOT_VERSION}_mono_linux_x86_64.zip"
  rm -rf "$TOOLS_DIR/godot-$GODOT_VERSION"
  mkdir -p "$TOOLS_DIR/godot-$GODOT_VERSION"
  unzip -q "$archive" -d "$TOOLS_DIR/godot-$GODOT_VERSION"
  rm -f "$archive"
  chmod +x "$godot_exe"
  "$godot_exe" --headless --version | grep -q "$GODOT_VERSION_STRING"
  link_bin "$godot_exe" godot
  link_bin "$godot_exe" godot4
}

install_export_templates() {
  local template_dir="$HOME/.local/share/godot/export_templates/$GODOT_TEMPLATE_VERSION"
  if [[ -f "$template_dir/windows_release_x86_64.exe" ]]; then
    log "Godot export templates already installed at $template_dir, skipping."
    return
  fi
  log "Installing Godot export templates into $template_dir"
  local archive="$TOOLS_DIR/Godot_v${GODOT_VERSION}_mono_export_templates.tpz"
  curl -fsSL -o "$archive" "https://github.com/godotengine/godot-builds/releases/download/${GODOT_VERSION}/Godot_v${GODOT_VERSION}_mono_export_templates.tpz"
  local extract_dir="$TOOLS_DIR/godot-templates-extract"
  rm -rf "$extract_dir"
  unzip -q "$archive" -d "$extract_dir"
  mkdir -p "$template_dir"
  cp -R "$extract_dir/templates/." "$template_dir/"
  rm -rf "$archive" "$extract_dir"
  test -f "$template_dir/windows_release_x86_64.exe"
}

install_blender() {
  if command -v blender >/dev/null 2>&1 && blender --version 2>/dev/null | head -1 | grep -q "$BLENDER_VERSION"; then
    log "Blender $BLENDER_VERSION already installed, skipping."
    return
  fi
  if ! command -v apt-get >/dev/null 2>&1; then
    log "apt-get not available; skipping Blender install. Install Blender $BLENDER_VERSION manually to use tools/blender/*.py."
    return
  fi
  log "Installing Blender via apt (matches .github/workflows/blender-asset-forge.yml)"
  $SUDO apt-get update
  $SUDO apt-get install -y blender python3-numpy libegl1 xvfb
  blender --version | head -1 | grep -q "$BLENDER_VERSION"
}

warm_build() {
  export PATH="$BIN_DIR:$PATH"
  cd "$REPO_ROOT"
  if [[ -f "TitanCraft.sln" ]]; then
    log "Running dotnet restore"
    dotnet restore
  fi
  if [[ -f "tools/prepare_audio_assets.py" ]]; then
    log "Preparing temporary audio assets"
    python3 tools/prepare_audio_assets.py
  fi
  if command -v godot >/dev/null 2>&1 && [[ -f "project.godot" ]]; then
    log "Warming Godot import cache (headless)"
    godot --headless --path . --import --quit || {
      log "Godot import reported a non-zero exit; inspect output above for fatal errors."
      return 1
    }
  fi
}

main() {
  install_dotnet
  install_godot
  install_export_templates
  install_blender
  warm_build
  log "Environment ready. dotnet: $(command -v dotnet || echo missing), godot: $(command -v godot || echo missing), blender: $(command -v blender || echo missing)"
  log "Next: dotnet build, or ./tools/test.sh for the full restore/build/test/import/export pass."
  log "Blender Asset Forge loop: docs/pipeline/blender-asset-forge.md (tools/blender/*.py)."
}

main "$@"
