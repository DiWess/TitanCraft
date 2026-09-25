#!/usr/bin/env python3
"""Author the Mwezi Quarter's one-shot cues from code (workstream A, audio).

Replaces silent placeholders under ``assets/audio/sources/`` with
project-authored one-shots. Nothing is downloaded or sampled: each cue is
shaped noise, resonant partials and pitch sweeps generated here, so the source
record is this script and the licence is the project's own.

Output is deterministic (a fixed seed per cue, no set iteration, no clock).
``--check`` regenerates in memory and compares with the committed files within
two LSBs, since math.sin/exp may round differently on another libm.

Cues, by the node that plays them in ``scenes/Main/Main.tscn``:
  footsteps/metal_walk_01.wav  Footsteps_Metal  heel strike on plate, short ring
  footsteps/rock_walk_01.wav   Footsteps_Rock   heel-toe on basalt with grit
  footsteps/ash_walk_01.wav    Footsteps_Ash    soft compressed-ash shuffle
  enemy/alert_01.wav           Scout_Alert      two rising warbled chirps
  enemy/attack_01.wav          Scout_Attack     falling screech into a strike
  enemy/hurt_01.wav            Scout_Hurt       short rough squeal, pitch falling

Usage: python3 tools/audio/synthesize_cues.py [--check]
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
OUTPUT_DIR = Path("assets/audio/sources")
TAU = 2.0 * math.pi


def one_pole(cutoff_hz: float) -> float:
    return 1.0 - math.exp(-TAU * cutoff_hz / SAMPLE_RATE)


def samples(seconds: float) -> int:
    return int(SAMPLE_RATE * seconds)


def envelope(t: float, attack: float, decay: float) -> float:
    """Linear attack, exponential decay (decay = time to -60 dB)."""
    if t < attack:
        return t / attack
    return math.exp(-6.9 * (t - attack) / decay)


def render_metal_step() -> list[float]:
    rng = random.Random(5101)
    modes = [(523.0, 0.5, 0.16), (1337.0, 0.35, 0.11), (2211.0, 0.25, 0.08), (3469.0, 0.15, 0.05)]
    low = 0.0
    out = []
    for i in range(samples(0.28)):
        t = i / SAMPLE_RATE
        ring = sum(level * math.sin(TAU * f * t) * envelope(t, 0.001, decay) for f, level, decay in modes)
        white = rng.uniform(-1.0, 1.0)
        low += one_pole(2_500.0) * (white - low)
        thud = low * envelope(t, 0.0015, 0.05) * 1.6
        out.append(ring * 0.6 + thud)
    return out


def render_rock_step() -> list[float]:
    rng = random.Random(5203)
    low = 0.0
    out = []
    for i in range(samples(0.24)):
        t = i / SAMPLE_RATE
        white = rng.uniform(-1.0, 1.0)
        low += one_pole(1_400.0) * (white - low)
        heel = envelope(t, 0.002, 0.07)
        toe = 0.6 * envelope(t - 0.045, 0.002, 0.06) if t >= 0.045 else 0.0
        # Grit: sparse grains of loose basalt under the sole.
        grain = rng.uniform(-1.0, 1.0) if rng.random() < 0.02 else 0.0
        out.append(low * (heel + toe) * 1.8 + grain * 0.5 * envelope(t, 0.005, 0.12))
    return out


def render_ash_step() -> list[float]:
    rng = random.Random(5307)
    first = low = band = 0.0
    out = []
    for i in range(samples(0.3)):
        t = i / SAMPLE_RATE
        white = rng.uniform(-1.0, 1.0)
        # Two poles: one lets enough top end through to read as hiss, not ash.
        first += one_pole(800.0) * (white - first)
        low += one_pole(800.0) * (first - low)
        band += one_pole(150.0) * (low - band)
        # Slow attack: ash compresses rather than cracks.
        out.append((low - 0.7 * band) * envelope(t, 0.02, 0.2))
    return out


def sweep_phase(t: float, start_hz: float, end_hz: float, length: float) -> float:
    """Phase of an exponential sweep from start_hz to end_hz over length seconds."""
    ratio = end_hz / start_hz
    k = math.log(ratio) / length
    return TAU * start_hz * (math.exp(k * t) - 1.0) / k


def render_alert() -> list[float]:
    out = []
    chirp = 0.22
    gap = 0.08
    for i in range(samples(2 * chirp + gap)):
        t = i / SAMPLE_RATE
        local = t if t < chirp else t - chirp - gap
        if local < 0.0 or local >= chirp:
            out.append(0.0)
            continue
        second = t >= chirp
        start, end = (420.0, 1300.0) if not second else (520.0, 1650.0)
        vibrato = 0.35 * math.sin(TAU * 23.0 * local)
        phase = sweep_phase(local, start, end, chirp) + vibrato
        tone = math.sin(phase) + 0.35 * math.sin(2.0 * phase) + 0.15 * math.sin(3.01 * phase)
        shape = math.sin(math.pi * local / chirp) ** 0.6
        out.append(tone * shape)
    return out


def render_attack() -> list[float]:
    rng = random.Random(6101)
    noise = 0.0
    out = []
    lunge = 0.32
    for i in range(samples(0.5)):
        t = i / SAMPLE_RATE
        white = rng.uniform(-1.0, 1.0)
        if t < lunge:
            phase = sweep_phase(t, 950.0, 240.0, lunge)
            roughness = 0.7 + 0.3 * math.sin(TAU * 37.0 * t)
            noise += one_pole(3_000.0) * (white - noise)
            screech = (math.sin(phase) + 0.4 * math.sin(2.0 * phase)) * roughness
            level = min(1.0, t / 0.03) * (0.6 + 0.4 * t / lunge)
            out.append((screech * 0.8 + noise * 0.5) * level)
        else:
            # The strike: a hard low transient with a short body.
            local = t - lunge
            noise += one_pole(600.0) * (white - noise)
            body = math.sin(TAU * 95.0 * local) * envelope(local, 0.001, 0.12)
            out.append(noise * 2.2 * envelope(local, 0.0008, 0.06) + body * 0.9)
    return out


def render_hurt() -> list[float]:
    rng = random.Random(6203)
    noise = 0.0
    out = []
    length = 0.3
    for i in range(samples(length)):
        t = i / SAMPLE_RATE
        phase = sweep_phase(t, 1250.0, 480.0, length)
        roughness = 0.55 + 0.45 * math.sin(TAU * 41.0 * t)
        white = rng.uniform(-1.0, 1.0)
        noise += one_pole(2_000.0) * (white - noise)
        tone = math.sin(phase) + 0.5 * math.sin(2.0 * phase + 0.3)
        out.append((tone * roughness + noise * 0.4) * envelope(t, 0.008, 0.28))
    return out


# name -> (renderer, peak): the peak sets each cue's level relative to the
# others; the scene's volume_db still sets the mix.
CUES = {
    "footsteps/metal_walk_01.wav": (render_metal_step, 0.5),
    "footsteps/rock_walk_01.wav": (render_rock_step, 0.5),
    "footsteps/ash_walk_01.wav": (render_ash_step, 0.45),
    "enemy/alert_01.wav": (render_alert, 0.7),
    "enemy/attack_01.wav": (render_attack, 0.8),
    "enemy/hurt_01.wav": (render_hurt, 0.7),
}


def finish(raw: list[float], peak: float) -> list[float]:
    # 3 ms fade at each end so no cue starts or stops with a click.
    fade = samples(0.003)
    for i in range(min(fade, len(raw))):
        raw[i] *= i / fade
        raw[-1 - i] *= i / fade
    loudest = max(abs(s) for s in raw) or 1.0
    return [s * peak / loudest for s in raw]


def encode(values: list[float]) -> bytes:
    frames = b"".join(struct.pack("<h", int(round(max(-1.0, min(1.0, v)) * 32767))) for v in values)
    buffer = io.BytesIO()
    with wave.open(buffer, "wb") as output:
        output.setnchannels(1)
        output.setsampwidth(2)
        output.setframerate(SAMPLE_RATE)
        output.writeframes(frames)
    return buffer.getvalue()


def pcm(riff: bytes) -> tuple[int, ...]:
    with wave.open(io.BytesIO(riff)) as source:
        frames = source.readframes(source.getnframes())
    return struct.unpack(f"<{len(frames) // 2}h", frames)


def matches(committed: bytes, generated: bytes, tolerance: int = 2) -> bool:
    if committed == generated:
        return True
    try:
        a, b = pcm(committed), pcm(generated)
    except (wave.Error, EOFError, struct.error):
        return False
    return len(a) == len(b) and all(abs(x - y) <= tolerance for x, y in zip(a, b))


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    parser.add_argument("--check", action="store_true", help="fail if committed files differ")
    args = parser.parse_args()
    mismatched = []
    for name, (render, peak) in CUES.items():
        data = encode(finish(render(), peak))
        path = OUTPUT_DIR / name
        if args.check:
            if not path.exists() or not matches(path.read_bytes(), data):
                mismatched.append(name)
            continue
        path.parent.mkdir(parents=True, exist_ok=True)
        path.write_bytes(data)
        print(f"wrote {path} ({len(data)} bytes)")
    if mismatched:
        print("cues differ from their generator: " + ", ".join(mismatched), file=sys.stderr)
        return 1
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
