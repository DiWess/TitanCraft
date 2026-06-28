#!/usr/bin/env bash
set -euo pipefail

real_godot="${REAL_GODOT_BIN:-}"

if [[ -z "$real_godot" || ! -x "$real_godot" ]]; then
  echo "REAL_GODOT_BIN is missing or invalid: $real_godot" >&2
  exit 1
fi

wrapper_path="$(realpath "$0")"
real_path="$(realpath "$real_godot")"

if [[ "$wrapper_path" == "$real_path" ]]; then
  echo "REAL_GODOT_BIN points to the wrapper itself" >&2
  exit 1
fi

for argument in "$@"; do
  if [[ "$argument" == "--headless" ]]; then
    exec "$real_godot" "$@"
  fi
done

exec "$real_godot" --headless "$@"
