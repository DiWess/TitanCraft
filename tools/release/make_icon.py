#!/usr/bin/env python3
"""Draw TitanCraft's application icon from code (no external art).

A lit beacon tower over the sea at dusk, in the game's palette: the violet
beam, the orange human-tech accent, the lime-stone tower. Writes icon.png
(512 px, the Godot project icon) and icon.ico (16-256 px, the Windows exe).

Usage: python3 tools/release/make_icon.py
"""
from pathlib import Path

from PIL import Image, ImageDraw, ImageFilter

ROOT = Path(__file__).resolve().parents[2]
S = 512


def draw() -> Image.Image:
    img = Image.new("RGBA", (S, S), (0, 0, 0, 0))
    bg = Image.new("RGBA", (S, S))
    d = ImageDraw.Draw(bg)
    for y in range(S):  # dusk sky to sea
        t = y / S
        d.line([(0, y), (S, y)], fill=(int(20 + 70 * t), int(18 + 30 * t), int(48 + 20 * t), 255))
    d.rectangle([0, int(S * 0.74), S, S], fill=(14, 52, 64, 255))
    mask = Image.new("L", (S, S), 0)
    ImageDraw.Draw(mask).rounded_rectangle([0, 0, S - 1, S - 1], radius=96, fill=255)
    img.paste(bg, (0, 0), mask)
    glow = Image.new("RGBA", (S, S), (0, 0, 0, 0))
    g = ImageDraw.Draw(glow)
    g.polygon([(236, 150), (276, 150), (330, 0), (182, 0)], fill=(150, 90, 255, 150))
    g.ellipse([196, 112, 316, 192], fill=(190, 140, 255, 220))
    glow = glow.filter(ImageFilter.GaussianBlur(10))
    img.alpha_composite(Image.composite(glow, Image.new("RGBA", (S, S)), mask))
    t = ImageDraw.Draw(img)
    t.polygon([(222, 168), (290, 168), (318, 380), (194, 380)], fill=(219, 212, 194, 255))
    t.rectangle([206, 158, 306, 176], fill=(143, 130, 108, 255))
    t.rectangle([232, 128, 280, 160], fill=(232, 120, 34, 255))
    t.ellipse([238, 132, 274, 156], fill=(255, 236, 255, 255))
    t.rectangle([160, 372, 352, 392], fill=(143, 130, 108, 255))
    t.polygon([(120, 392), (392, 392), (420, 420), (92, 420)], fill=(90, 81, 67, 255))
    for x in range(40, 480, 56):
        t.line([(x, 440), (x + 30, 440)], fill=(120, 200, 210, 160), width=4)
    return img


def main() -> None:
    icon = draw()
    icon.save(ROOT / "icon.png")
    icon.save(ROOT / "icon.ico", sizes=[(16, 16), (32, 32), (48, 48), (64, 64), (128, 128), (256, 256)])
    print("ICON_WRITTEN icon.png icon.ico")


if __name__ == "__main__":
    main()
