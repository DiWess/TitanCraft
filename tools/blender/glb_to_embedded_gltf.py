#!/usr/bin/env python3
"""Convert a binary GLB into a single-file text .gltf with an embedded buffer.

Blender 4.0 removed the GLTF_EMBEDDED export option; this tool restores that
delivery format so production candidates can live in the repository as
reviewable text, matching the committed v1_beta model precedent.

Usage:
  python3 tools/blender/glb_to_embedded_gltf.py input.glb output.gltf
"""
from __future__ import annotations

import base64
import json
import struct
import sys
from pathlib import Path


def convert(source: Path, dest: Path) -> None:
    data = source.read_bytes()
    magic, _version, _length = struct.unpack_from("<III", data, 0)
    if magic != 0x46546C67:
        raise SystemExit(f"not a GLB file: {source}")
    offset = 12
    gltf_json: dict | None = None
    binary: bytes = b""
    while offset < len(data):
        chunk_length, chunk_type = struct.unpack_from("<II", data, offset)
        chunk = data[offset + 8: offset + 8 + chunk_length]
        if chunk_type == 0x4E4F534A:  # JSON
            gltf_json = json.loads(chunk.decode("utf-8"))
        elif chunk_type == 0x004E4942:  # BIN
            binary = chunk
        offset += 8 + chunk_length
    if gltf_json is None:
        raise SystemExit(f"missing JSON chunk in {source}")
    if binary and gltf_json.get("buffers"):
        gltf_json["buffers"][0]["uri"] = (
            "data:application/octet-stream;base64," + base64.b64encode(binary).decode("ascii")
        )
    dest.parent.mkdir(parents=True, exist_ok=True)
    dest.write_text(json.dumps(gltf_json, separators=(",", ":")) + "\n", encoding="utf-8")
    print(f"EMBEDDED_GLTF_WRITTEN {dest}")


def main() -> None:
    if len(sys.argv) != 3:
        raise SystemExit(__doc__)
    convert(Path(sys.argv[1]), Path(sys.argv[2]))


if __name__ == "__main__":
    main()
