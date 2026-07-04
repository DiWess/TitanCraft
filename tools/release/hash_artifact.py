#!/usr/bin/env python3
"""Print deterministic SHA-256 evidence for a build artifact."""
from __future__ import annotations

import argparse
import hashlib
from pathlib import Path


def sha256_file(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as artifact:
        for chunk in iter(lambda: artifact.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


def main() -> int:
    parser = argparse.ArgumentParser(description="Hash a release artifact with SHA-256.")
    parser.add_argument("artifact", type=Path, help="Path to the artifact to hash.")
    args = parser.parse_args()

    artifact = args.artifact
    if not artifact.is_file():
        parser.error(f"artifact does not exist or is not a file: {artifact}")

    size = artifact.stat().st_size
    digest = sha256_file(artifact)
    print(f"path: {artifact}")
    print(f"size_bytes: {size}")
    print(f"sha256: {digest}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
