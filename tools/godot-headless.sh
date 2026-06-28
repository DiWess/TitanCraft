#!/usr/bin/env bash
set -euo pipefail
real_godot="${REAL_GODOT_BIN:-$(command -v godot)}"
has_headless=0
for arg in "$@"; do
  if [[ "$arg" == "--headless" ]]; then
    has_headless=1
    break
  fi
done
if [[ "$has_headless" == "1" ]]; then
  exec "$real_godot" "$@"
fi
exec "$real_godot" --headless "$@"
