#!/usr/bin/env bash
# Claude Code SessionStart hook: bootstraps the TitanCraft build toolchain
# in Claude Code on the web sessions by delegating to the checked-in,
# reusable setup script so any Agent Studio agent can immediately run
# `dotnet build`, `godot --headless ...`, or `tools/test.sh`.
set -euo pipefail

if [[ "${CLAUDE_CODE_REMOTE:-}" != "true" ]]; then
  exit 0
fi

BIN_DIR="${TITANCRAFT_BIN_DIR:-$HOME/.local/bin}"

"$CLAUDE_PROJECT_DIR/tools/setup_environment.sh"

if [[ -n "${CLAUDE_ENV_FILE:-}" ]]; then
  echo "export PATH=\"$BIN_DIR:\$PATH\"" >> "$CLAUDE_ENV_FILE"
fi
