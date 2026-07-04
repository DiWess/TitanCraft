#!/usr/bin/env python3
"""Print SHA-256 metadata for a supplied release artifact."""

from __future__ import annotations

import argparse
import hashlib
from datetime import datetime, timezone
from pathlib import Path


def sha256_file(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as artifact:
        for chunk in iter(lambda: artifact.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


def main() -> int:
    parser = argparse.ArgumentParser(
        description="Print SHA-256, size, and UTC timestamp for a build artifact."
    )
    parser.add_argument("artifact", type=Path, help="Path to the artifact to hash")
    args = parser.parse_args()

    artifact = args.artifact
    if not artifact.is_file():
        parser.error(f"artifact not found or not a file: {artifact}")

    stat = artifact.stat()
    timestamp = datetime.fromtimestamp(stat.st_mtime, timezone.utc).isoformat()
    print(f"artifact_path={artifact}")
    print(f"artifact_name={artifact.name}")
    print(f"artifact_size_bytes={stat.st_size}")
    print("hash_algorithm=SHA-256")
    print(f"sha256={sha256_file(artifact)}")
    print(f"modified_utc={timestamp}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
