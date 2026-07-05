#!/usr/bin/env python3
"""Build a contact sheet from Blender asset review PNGs."""
from __future__ import annotations

import sys
from pathlib import Path


def main() -> None:
    if len(sys.argv) != 2:
        raise SystemExit("usage: python3 tools/blender/build_asset_review_contact_sheet.py review_dir")
    review_dir = Path(sys.argv[1])
    if not review_dir.exists():
        raise SystemExit(f"missing review directory: {review_dir}")
    pngs = sorted(path for path in review_dir.glob("*.png") if path.name != "contact_sheet.png")
    if not pngs:
        raise SystemExit(f"no PNGs found in {review_dir}")
    from PIL import Image, ImageDraw, ImageFont
    thumb_w, thumb_h = 420, 236
    cols = 2
    rows = (len(pngs) + cols - 1) // cols
    sheet = Image.new("RGB", (cols * thumb_w, rows * (thumb_h + 28)), (20, 22, 24))
    draw = ImageDraw.Draw(sheet)
    font = ImageFont.load_default()
    for idx, path in enumerate(pngs):
        image = Image.open(path).convert("RGB")
        image.thumbnail((thumb_w, thumb_h))
        x = (idx % cols) * thumb_w
        y = (idx // cols) * (thumb_h + 28)
        sheet.paste(image, (x + (thumb_w - image.width) // 2, y))
        draw.text((x + 8, y + thumb_h + 8), path.name, fill=(230, 230, 220), font=font)
    output = review_dir / "contact_sheet.png"
    sheet.save(output)
    print(f"ASSET_REVIEW_CONTACT_SHEET_WRITTEN {output}")


if __name__ == "__main__":
    main()
