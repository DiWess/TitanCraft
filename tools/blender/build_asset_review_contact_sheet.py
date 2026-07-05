#!/usr/bin/env python3
"""Build a contact sheet for TitanCraft Blender asset review PNGs."""
from __future__ import annotations

import sys
from pathlib import Path
from PIL import Image, ImageDraw

ORDER = [
    "front.png",
    "back.png",
    "left.png",
    "right.png",
    "top.png",
    "hero_three_quarter.png",
    "scale_reference_player_capsule.png",
    "material_preview.png",
]


def main() -> None:
    if len(sys.argv) not in {2, 3}:
        raise SystemExit("usage: python3 build_asset_review_contact_sheet.py review_dir [output.png]")
    review_dir = Path(sys.argv[1])
    output = Path(sys.argv[2]) if len(sys.argv) == 3 else review_dir / "contact_sheet.png"
    images = [review_dir / name for name in ORDER if (review_dir / name).exists()]
    if not images:
        raise SystemExit(f"no review PNGs found in {review_dir}")
    thumb_w, thumb_h, label_h, cols = 320, 180, 24, 2
    rows = (len(images) + cols - 1) // cols
    sheet = Image.new("RGB", (cols * thumb_w, rows * (thumb_h + label_h)), (12, 12, 18))
    draw = ImageDraw.Draw(sheet)
    for index, path in enumerate(images):
        image = Image.open(path).convert("RGB")
        image.thumbnail((thumb_w, thumb_h))
        x = (index % cols) * thumb_w
        y = (index // cols) * (thumb_h + label_h)
        draw.text((x + 6, y + 5), path.stem, fill=(235, 230, 210))
        sheet.paste(image, (x, y + label_h))
    output.parent.mkdir(parents=True, exist_ok=True)
    sheet.save(output)
    print(f"ASSET_REVIEW_CONTACT_SHEET_WRITTEN {output}")


if __name__ == "__main__":
    main()
