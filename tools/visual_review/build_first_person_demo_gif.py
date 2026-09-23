#!/usr/bin/env python3
"""Assemble the first-person demo PNG sequence into an animated GIF.

The capture writes full-resolution frames; this downscales and quantises them
into something small enough to hand to a reviewer inline. It is a review
artifact, not a deliverable asset: the frames it consumes live under
artifacts/visual-review/, which .gitignore already excludes.

Usage:
  python3 tools/visual_review/build_first_person_demo_gif.py [--width 640] [--fps 12]
"""
from __future__ import annotations

import argparse
from pathlib import Path

from PIL import Image

ROOT = Path(__file__).resolve().parents[2]
FRAME_DIR = ROOT / "artifacts/visual-review/first-person-demo"
OUTPUT = FRAME_DIR / "first_person_demo.gif"


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--width", type=int, default=640, help="output width in pixels")
    parser.add_argument("--fps", type=int, default=12, help="playback rate of the capture")
    parser.add_argument("--colors", type=int, default=128, help="palette size")
    return parser.parse_args()


def main() -> None:
    args = parse_args()
    frames = sorted(FRAME_DIR.glob("frame_*.png"))
    if not frames:
        raise SystemExit(f"no frames found in {FRAME_DIR}")

    images: list[Image.Image] = []
    for path in frames:
        image = Image.open(path).convert("RGB")
        if image.width != args.width:
            height = round(image.height * args.width / image.width)
            image = image.resize((args.width, height), Image.LANCZOS)
        # A single adaptive palette per frame keeps the coral/lime tones from
        # banding into mud, which a global web palette does immediately.
        images.append(image.quantize(colors=args.colors, method=Image.MEDIANCUT))

    duration_ms = round(1000 / max(1, args.fps))
    images[0].save(
        OUTPUT,
        save_all=True,
        append_images=images[1:],
        duration=duration_ms,
        loop=0,
        optimize=True,
        disposal=2,
    )
    size_mb = OUTPUT.stat().st_size / (1024 * 1024)
    print(
        f"FIRST_PERSON_DEMO_GIF_WRITTEN {OUTPUT} "
        f"frames={len(images)} width={args.width} fps={args.fps} size={size_mb:.1f}MB"
    )


if __name__ == "__main__":
    main()
