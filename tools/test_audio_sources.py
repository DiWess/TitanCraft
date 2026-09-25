#!/usr/bin/env python3
"""Guard the audio library against silent files and unrecorded sources.

The 2026-09-25 playtest (finding 7) found 28 of the game's committed audio
files were digital silence while their provenance record credited real
recordings. This test is a ratchet on that finding:

* a silent file that is not on KNOWN_SILENT fails -- no new placeholders;
* a file on KNOWN_SILENT that now makes sound fails -- remove it from the
  list, so the list only ever shrinks;
* project-authored audio must match its generator (within two LSBs), so
  the committed files are exactly what the source record describes.

Usage: python3 tools/test_audio_sources.py
"""
from __future__ import annotations

import struct
import subprocess
import sys
import wave
from pathlib import Path

SOURCES = Path("assets/audio/sources")

# Silent placeholders still awaiting replacement (workstream A, audio).
KNOWN_SILENT = {
    "enemy/death_01.wav",
    "pickup/generic_01.wav",
    "pickup/glass_01.wav",
    "pickup/metal_01.wav",
    "pickup/organic_01.wav",
    "save/load_complete_01.wav",
    "save/save_complete_01.wav",
    "save/save_progress_01.wav",
    "state/defeat_01.wav",
    "state/mission_complete_01.wav",
    "state/objective_complete_01.wav",
    "state/victory_01.wav",
    "ui/craft_complete_01.wav",
    "ui/hover_01.wav",
    "ui/menu_toggle_01.wav",
    "ui/select_01.wav",
    "weapon/impact_01.wav",
    "weapon/ready_tone_01.wav",
    "weapon/swing_01.wav",
}

# Project-authored audio: the committed files must be what these produce.
GENERATORS = ("tools/audio/synthesize_ambience.py", "tools/audio/synthesize_cues.py")

# -60 dBFS: anything quieter at its loudest sample is not a usable cue.
SILENCE_PEAK = 32


def peak(path: Path) -> int:
    with wave.open(str(path)) as source:
        if source.getsampwidth() != 2:
            raise AssertionError(f"{path}: expected 16-bit PCM, got {8 * source.getsampwidth()}-bit")
        frames = source.readframes(source.getnframes())
    samples = struct.unpack(f"<{len(frames) // 2}h", frames)
    return max((abs(sample) for sample in samples), default=0)


def main() -> int:
    failures = []
    seen = set()
    for path in sorted(SOURCES.rglob("*.wav")):
        name = path.relative_to(SOURCES).as_posix()
        seen.add(name)
        silent = peak(path) < SILENCE_PEAK
        if silent and name not in KNOWN_SILENT:
            failures.append(f"{name} is silent and not a known placeholder")
        if not silent and name in KNOWN_SILENT:
            failures.append(f"{name} now makes sound: remove it from KNOWN_SILENT")
    for name in sorted(KNOWN_SILENT - seen):
        failures.append(f"{name} is listed as silent but does not exist: remove it from KNOWN_SILENT")

    for script in GENERATORS:
        generator = subprocess.run([sys.executable, script, "--check"],
                                   capture_output=True, text=True, check=False)
        if generator.returncode != 0:
            failures.append(f"files do not match {script}: " + generator.stderr.strip())

    for failure in failures:
        print(f"FAIL {failure}", file=sys.stderr)
    audible = len(seen) - len(KNOWN_SILENT & seen)
    print(f"audio sources: {len(seen)} files, {audible} audible, "
          f"{len(KNOWN_SILENT & seen)} known silent placeholders")
    return 1 if failures else 0


if __name__ == "__main__":
    raise SystemExit(main())
