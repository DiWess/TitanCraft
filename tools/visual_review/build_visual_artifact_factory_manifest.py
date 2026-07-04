#!/usr/bin/env python3
"""Build a combined manifest for Visual Artifact Factory outputs."""
from __future__ import annotations

import hashlib
import json
from datetime import datetime, timezone
from pathlib import Path
from typing import Any

ROOT = Path(__file__).resolve().parents[2]
SCAN_ROOTS = [ROOT / "artifacts/asset-review", ROOT / "artifacts/visual-review"]
EXTRA_PATHS = [
    ROOT / "assets/Production/Generated/asset_manifest.json",
    ROOT / "assets/Production/Generated/Test/TC_TestCrate.glb",
    ROOT / "assets/Production/Generated/CrashWreck/TC_HeavyCrashHull_V1.glb",
]
OUTPUT_DIR = ROOT / "artifacts/visual-artifact-factory"
MANIFEST = OUTPUT_DIR / "visual_artifact_factory_manifest.json"
EXTENSIONS = {".png", ".glb", ".json", ".log", ".md"}


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


def artifact_type(path: Path) -> str:
    rel = path.relative_to(ROOT).as_posix()
    if rel.startswith("artifacts/asset-review/"):
        return "asset_review"
    if rel.startswith("artifacts/visual-review/"):
        return "scene_review"
    if rel.startswith("assets/Production/Generated/"):
        return "asset_review"
    return "unknown"


def generated_by(path: Path) -> str | None:
    text = path.as_posix()
    if text.endswith("asset_manifest.json"):
        return "tools/blender/build_asset_manifest.py"
    if text.endswith(".glb"):
        return "tools/blender/export_asset.py"
    if "TC_HeavyCrashHull_V1" in text:
        return "tools/blender/render_heavy_crash_hull_v1_reviews.py"
    if "test-crate" in text:
        return "tools/blender/render_asset_turntable.py"
    if "visual_artifact_factory_scene_manifest" in text:
        return "tools/visual_review/run_visual_artifact_factory.py"
    if text.endswith(".log"):
        return "captured command log"
    return None


def collect_entries() -> list[dict[str, Any]]:
    entries = []
    for root in SCAN_ROOTS:
        if not root.exists():
            entries.append({
                "artifact_type": "asset_review" if root.name == "asset-review" else "scene_review",
                "path": str(root.relative_to(ROOT)),
                "notes": "missing/skipped scan root",
            })
            continue
        for path in sorted(root.rglob("*")):
            if path.is_file() and path.suffix.lower() in EXTENSIONS:
                entries.append({
                    "artifact_type": artifact_type(path),
                    "path": str(path.relative_to(ROOT)),
                    "sha256": sha256(path),
                    "file_size": path.stat().st_size,
                    "generated_by": generated_by(path),
                    "timestamp": datetime.fromtimestamp(path.stat().st_mtime, timezone.utc).isoformat(),
                    "notes": None,
                })
    for path in EXTRA_PATHS:
        if path.exists() and path.is_file():
            entries.append({
                "artifact_type": artifact_type(path),
                "path": str(path.relative_to(ROOT)),
                "sha256": sha256(path),
                "file_size": path.stat().st_size,
                "generated_by": generated_by(path),
                "timestamp": datetime.fromtimestamp(path.stat().st_mtime, timezone.utc).isoformat(),
                "notes": None,
            })
        else:
            entries.append({
                "artifact_type": artifact_type(path),
                "path": str(path.relative_to(ROOT)),
                "notes": "missing/skipped optional generated asset output",
            })
    return entries


def main() -> None:
    OUTPUT_DIR.mkdir(parents=True, exist_ok=True)
    entries = collect_entries()
    manifest = {
        "schema": "titancraft.visual_artifact_factory.manifest.v1",
        "generated_at": datetime.now(timezone.utc).isoformat(),
        "entries": entries,
    }
    MANIFEST.write_text(json.dumps(manifest, indent=2) + "\n", encoding="utf-8")
    (OUTPUT_DIR / "generated_file_list.txt").write_text("\n".join(entry["path"] for entry in entries) + "\n", encoding="utf-8")
    (OUTPUT_DIR / "generated_sha256.txt").write_text("\n".join(f"{entry.get('sha256', '')}  {entry['path']}" for entry in entries if entry.get("sha256")) + "\n", encoding="utf-8")
    print(f"VISUAL_ARTIFACT_FACTORY_MANIFEST_WRITTEN {MANIFEST} entries={len(entries)}")


if __name__ == "__main__":
    main()
