#!/usr/bin/env python3
"""Author the Mwezi Quarter ambience loops from code (workstream A, audio).

Replaces the silent placeholders under ``assets/audio/sources/ambient/`` with
project-authored, seamlessly looping ambience. Nothing is downloaded or sampled:
every file is shaped noise and sine partials generated here, so the source
record is this script and the licence is the project's own.

Output is deterministic -- a fixed seed per loop, no set iteration, no clock --
so re-running the script reproduces the committed files and CI can check it
(within two LSBs, since math.sin/exp may round differently on another libm).

Loops:
  wind_ambience_loop_01.wav    gusting band-limited noise; the quarter-wide bed
  sea_surf_loop_01.wav         three swells a loop breaking on the seawall
  volcanic_rumble_loop_01.wav  low filtered noise with slow swells (Karthala)
  machinery_hum_loop_01.wav    the wreck's residual hum with a breathing tremolo

Seamless looping: every modulation has a period that divides the loop length
and every partial completes a whole number of cycles, so the only
discontinuity left is the noise itself; that is removed by rendering a tail
past the loop end and cross-fading it into the head with equal power.

Usage: python3 tools/audio/synthesize_ambience.py [--check]
  --check  regenerate in memory and fail if the committed files differ.
"""
from __future__ import annotations

import argparse
import io
import math
import random
import struct
import sys
import wave
from pathlib import Path

SAMPLE_RATE = 32_000
LOOP_SECONDS = 24
CROSSFADE_SECONDS = 1.5
PEAK = 0.7  # -3.1 dBFS; the scene's volume_db sets the mix, not the file.
OUTPUT_DIR = Path("assets/audio/sources/ambient")

TAU = 2.0 * math.pi


def one_pole_coefficient(cutoff_hz: float) -> float:
    return 1.0 - math.exp(-TAU * cutoff_hz / SAMPLE_RATE)


def loop_phase(index: int, cycles_per_loop: int) -> float:
    """Phase in radians of a modulator that completes whole cycles per loop."""
    return TAU * cycles_per_loop * index / (SAMPLE_RATE * LOOP_SECONDS)


def render_wind(count: int) -> list[float]:
    rng = random.Random(1101)
    low = band = 0.0
    out = []
    for i in range(count):
        # Gusts: three slow modulators whose periods divide the loop.
        gust = (0.55
                + 0.25 * math.sin(loop_phase(i, 2) + 0.7)
                + 0.15 * math.sin(loop_phase(i, 5) + 2.1)
                + 0.05 * math.sin(loop_phase(i, 11) + 4.0))
        cutoff = 250.0 + 900.0 * gust * gust
        white = rng.uniform(-1.0, 1.0)
        low += one_pole_coefficient(cutoff) * (white - low)
        band += one_pole_coefficient(cutoff * 0.25) * (low - band)
        # Low minus lower leaves a moving band: the whistle rides the gust.
        out.append((low - 0.6 * band) * gust)
    return out


def render_sea(count: int) -> list[float]:
    rng = random.Random(2203)
    rumble = hiss = 0.0
    swell_count = 3  # an 8 s swell period, typical of a harbour wall
    swell_level = [0.85, 1.0, 0.7]
    out = []
    for i in range(count):
        position = (i % (SAMPLE_RATE * LOOP_SECONDS)) / (SAMPLE_RATE * LOOP_SECONDS) * swell_count
        swell = int(position) % swell_count
        t = position - int(position)  # 0..1 through one swell
        # Build slowly, break at 55 %, then wash back and settle.
        if t < 0.55:
            body = 0.25 + 0.45 * (t / 0.55) ** 2
            wash = 0.0
        else:
            decay = (t - 0.55) / 0.45
            body = 0.25 + 0.75 * math.exp(-4.0 * decay)
            wash = math.exp(-2.5 * decay) * min(1.0, decay * 12.0)
        # Close the envelope across the swell boundary so levels do not jump.
        edge = min(1.0, t / 0.04)
        level = swell_level[swell] * (body * edge + 0.25 * (1.0 - edge))
        white = rng.uniform(-1.0, 1.0)
        rumble += one_pole_coefficient(180.0 + 500.0 * body) * (white - rumble)
        hiss += one_pole_coefficient(3_500.0) * (white - hiss)
        out.append(level * rumble + 0.35 * swell_level[swell] * wash * (hiss - rumble))
    return out


def render_rumble(count: int) -> list[float]:
    rng = random.Random(3307)
    first = second = 0.0
    out = []
    for i in range(count):
        swell = 0.6 + 0.3 * math.sin(loop_phase(i, 1) + 1.3) + 0.1 * math.sin(loop_phase(i, 3))
        white = rng.uniform(-1.0, 1.0)
        first += one_pole_coefficient(90.0) * (white - first)
        second += one_pole_coefficient(45.0) * (first - second)
        # A 41.25 Hz body (990 whole cycles per loop) keeps it from reading as hiss.
        body = 0.25 * math.sin(loop_phase(i, 990))
        out.append(swell * (second * 6.0 + body * 0.4))
    return out


def render_machinery(count: int) -> list[float]:
    rng = random.Random(4409)
    noise = 0.0
    # Whole cycles per loop: 1320 -> 55 Hz fundamental; 1332 beats against
    # 1320 twelve times a loop, the slow "breathing" of a failing regulator.
    partials = [(1320, 0.55), (2640, 0.3), (3960, 0.18), (1332, 0.2), (7920, 0.05)]
    out = []
    for i in range(count):
        tremolo = 0.8 + 0.2 * math.sin(loop_phase(i, 6) + 0.4)
        tone = sum(level * math.sin(loop_phase(i, cycles)) for cycles, level in partials)
        white = rng.uniform(-1.0, 1.0)
        noise += one_pole_coefficient(600.0) * (white - noise)
        out.append(tremolo * tone + 0.6 * noise)
    return out


LOOPS = {
    "wind_ambience_loop_01.wav": render_wind,
    "sea_surf_loop_01.wav": render_sea,
    "volcanic_rumble_loop_01.wav": render_rumble,
    "machinery_hum_loop_01.wav": render_machinery,
}


def make_loop(render) -> list[float]:
    length = SAMPLE_RATE * LOOP_SECONDS
    fade = int(SAMPLE_RATE * CROSSFADE_SECONDS)
    raw = render(length + fade)
    loop = raw[:length]
    # The head becomes the continuation of the tail: sample length-1 is
    # followed in the source by sample `length`, which is where the head now
    # starts, so the wrap-around point is continuous by construction.
    for i in range(fade):
        x = i / fade
        loop[i] = raw[length + i] * math.cos(0.5 * math.pi * x) + raw[i] * math.sin(0.5 * math.pi * x)
    peak = max(abs(sample) for sample in loop)
    return [sample * PEAK / peak for sample in loop]


def encode(samples: list[float]) -> bytes:
    frames = b"".join(struct.pack("<h", int(round(max(-1.0, min(1.0, s)) * 32767))) for s in samples)
    buffer = io.BytesIO()
    with wave.open(buffer, "wb") as output:
        output.setnchannels(1)
        output.setsampwidth(2)
        output.setframerate(SAMPLE_RATE)
        output.writeframes(frames)
    return with_loop_chunk(buffer.getvalue(), len(samples))


def with_loop_chunk(riff: bytes, sample_count: int) -> bytes:
    """Append a `smpl` chunk declaring one forward loop over the whole file.

    `.import` files are not committed in this repository, so the loop cannot
    live in import settings; Godot's default "Detect From WAV" loop mode reads
    this chunk on every machine that imports the file.
    """
    header = struct.pack("<9I", 0, 0, 1_000_000_000 // SAMPLE_RATE, 60, 0, 0, 0, 1, 0)
    loop = struct.pack("<6I", 0, 0, 0, sample_count - 1, 0, 0)  # id, forward, start, end (inclusive)
    chunk = b"smpl" + struct.pack("<I", len(header) + len(loop)) + header + loop
    body = riff[8:] + chunk
    return b"RIFF" + struct.pack("<I", len(body)) + body


def pcm_samples(riff: bytes) -> tuple[int, ...]:
    with wave.open(io.BytesIO(riff)) as source:
        frames = source.readframes(source.getnframes())
    return struct.unpack(f"<{len(frames) // 2}h", frames)


def matches(committed: bytes, generated: bytes, tolerance: int = 2) -> bool:
    """Equal within a couple of LSBs: libm rounding differs across platforms."""
    if committed == generated:
        return True
    try:
        a, b = pcm_samples(committed), pcm_samples(generated)
    except (wave.Error, EOFError, struct.error):
        return False
    return len(a) == len(b) and all(abs(x - y) <= tolerance for x, y in zip(a, b))


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    parser.add_argument("--check", action="store_true", help="fail if committed files differ")
    args = parser.parse_args()
    mismatched = []
    OUTPUT_DIR.mkdir(parents=True, exist_ok=True)
    for name, render in LOOPS.items():
        data = encode(make_loop(render))
        path = OUTPUT_DIR / name
        if args.check:
            if not path.exists() or not matches(path.read_bytes(), data):
                mismatched.append(name)
            continue
        path.write_bytes(data)
        print(f"wrote {path} ({len(data)} bytes)")
    if mismatched:
        print("ambience differs from its generator: " + ", ".join(mismatched), file=sys.stderr)
        return 1
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
