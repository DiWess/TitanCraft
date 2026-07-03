#!/usr/bin/env python3
"""Materialize temporary MVP audio cues for CI and local Godot imports."""
from __future__ import annotations

import math
import struct
import wave
from pathlib import Path

SAMPLE_RATE = 22_050
OUTPUT_DIR = Path("assets/audio/temp")
CUES = {
    "pickup_chime.wav": ([880.0, 1320.0], 0.16),
    "craft_confirm.wav": ([330.0, 660.0, 990.0], 0.28),
    "arm_hit.wav": ([120.0, 80.0], 0.14),
    "player_damage.wav": ([180.0, 90.0], 0.22),
    "galaxabrain_down.wav": ([220.0, 160.0, 100.0], 0.35),
    "beacon_activate.wav": ([440.0, 880.0, 1760.0], 0.45),
    "victory_sting.wav": ([523.25, 659.25, 783.99], 0.60),
}


def write_cue(path: Path, frequencies: list[float], duration_seconds: float) -> None:
    sample_count = int(SAMPLE_RATE * duration_seconds)
    frames = []
    for sample_index in range(sample_count):
        time_seconds = sample_index / SAMPLE_RATE
        segment = min(int(sample_index / (sample_count / len(frequencies))), len(frequencies) - 1)
        envelope = min(1.0, sample_index / (SAMPLE_RATE * 0.01)) * max(0.0, 1.0 - sample_index / sample_count)
        sample = 0.35 * envelope * math.sin(2.0 * math.pi * frequencies[segment] * time_seconds)
        frames.append(struct.pack("<h", int(sample * 32767)))

    with wave.open(str(path), "wb") as output:
        output.setnchannels(1)
        output.setsampwidth(2)
        output.setframerate(SAMPLE_RATE)
        output.writeframes(b"".join(frames))


def main() -> None:
    OUTPUT_DIR.mkdir(parents=True, exist_ok=True)
    for file_name, (frequencies, duration_seconds) in CUES.items():
        write_cue(OUTPUT_DIR / file_name, frequencies, duration_seconds)


if __name__ == "__main__":
    main()
