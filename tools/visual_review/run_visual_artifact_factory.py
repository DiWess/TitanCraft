#!/usr/bin/env python3
"""Run allowlisted Godot visual-review capture scripts for CI artifacts."""
from __future__ import annotations

import hashlib
import json
import os
import subprocess
import sys
from datetime import datetime, timezone
from pathlib import Path
from typing import Any

ROOT = Path(__file__).resolve().parents[2]
OUTPUT_ROOT = ROOT / "artifacts/visual-review"
LOG_DIR = OUTPUT_ROOT / "logs"
MANIFEST = OUTPUT_ROOT / "visual_artifact_factory_scene_manifest.json"
ALLOWLIST = [
    "tools/visual_review/capture_phase3a_production_integration.gd",
    "tools/visual_review/capture_crafted_arm_first_person.gd",
    "tools/visual_review/capture_stage_a_custom_audition.gd",
    "tools/visual_review/capture_stage_a_custom_final.gd",
    "tools/visual_review/capture_stage_a_custom_final_v2.gd",
    "tools/visual_review/capture_stage_a_v3_audition.gd",
    "tools/visual_review/capture_stage_a_v3_final.gd",
    "tools/visual_review/capture_stage_a_v4_neutral.gd",
    "tools/visual_review/capture_stage_a_v4_isolated.gd",
    "tools/visual_review/capture_stage_a_v4_final.gd",
    "tools/visual_review/capture_stage_a_v5_neutral.gd",
    "tools/visual_review/capture_stage_a_v5_isolated.gd",
    "tools/visual_review/capture_stage_a_v5_final.gd",
]


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


def png_entries(paths: list[Path]) -> list[dict[str, Any]]:
    entries = []
    for path in sorted(paths):
        if path.exists() and path.suffix.lower() == ".png":
            entries.append({
                "path": str(path.relative_to(ROOT)),
                "sha256": sha256(path),
                "file_size": path.stat().st_size,
            })
    return entries


def run_capture(script: str) -> dict[str, Any]:
    script_path = ROOT / script
    if not script_path.exists():
        return {"script": script, "status": "skipped", "notes": "missing allowlisted script", "pngs": []}

    LOG_DIR.mkdir(parents=True, exist_ok=True)
    before = {path: path.stat().st_mtime_ns for path in OUTPUT_ROOT.rglob("*.png") if path.is_file()}
    command = ["xvfb-run", "-a", os.environ.get("GODOT_BIN", "godot"), "--path", str(ROOT), "--script", script]
    log_path = LOG_DIR / f"{Path(script).stem}.log"
    started_at = datetime.now(timezone.utc).isoformat()
    with log_path.open("w", encoding="utf-8") as log:
        log.write(f"command: {' '.join(command)}\nstarted_at: {started_at}\n")
        result = subprocess.run(command, cwd=ROOT, text=True, stdout=subprocess.PIPE, stderr=subprocess.STDOUT)
        log.write(result.stdout)
        log.write(f"\nreturncode: {result.returncode}\n")
    if result.returncode != 0:
        return {"script": script, "status": "failed", "command": command, "log": str(log_path.relative_to(ROOT)), "returncode": result.returncode, "pngs": []}

    after = {path: path.stat().st_mtime_ns for path in OUTPUT_ROOT.rglob("*.png") if path.is_file()}
    produced = sorted(path for path, mtime in after.items() if path not in before or before[path] != mtime)
    if not produced:
        return {"script": script, "status": "failed", "command": command, "log": str(log_path.relative_to(ROOT)), "notes": "script existed but produced no new PNGs", "pngs": []}
    return {"script": script, "status": "passed", "command": command, "log": str(log_path.relative_to(ROOT)), "pngs": png_entries(produced)}


def main() -> None:
    OUTPUT_ROOT.mkdir(parents=True, exist_ok=True)
    results = [run_capture(script) for script in ALLOWLIST]
    manifest = {
        "schema": "titancraft.visual_artifact_factory.scene_manifest.v1",
        "generated_at": datetime.now(timezone.utc).isoformat(),
        "allowlist": ALLOWLIST,
        "results": results,
        "pngs": png_entries([path for path in OUTPUT_ROOT.rglob("*.png") if path.is_file()]),
    }
    MANIFEST.write_text(json.dumps(manifest, indent=2) + "\n", encoding="utf-8")
    print(f"VISUAL_SCENE_MANIFEST_WRITTEN {MANIFEST}")
    failed = [entry for entry in results if entry["status"] == "failed"]
    if failed:
        print(json.dumps({"failed": failed}, indent=2), file=sys.stderr)
        raise SystemExit(1)


if __name__ == "__main__":
    main()
