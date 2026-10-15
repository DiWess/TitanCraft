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
  weapon/swing_01.wav          Weapon_Swing     servo swing: rising-falling air, faint whine
  weapon/impact_01.wav         Weapon_Impact    low thud, crunch and short metal ring
  weapon/ready_tone_01.wav     Weapon_Ready     latch click then a short high tone
  ui/select_01.wav             UI_Select        two quick rising blips
  ui/hover_01.wav              UI_Hover         a very short soft tick
  ui/menu_toggle_01.wav        UI_Menu_Toggle   a breath of air and a falling two-note
  ui/craft_complete_01.wav     UI_Craft_Complete ratchet clicks, servo, latch
  state/objective_complete_01  State_Objective  two-note bell chime, a rising fifth
  state/defeat_01.wav          DefeatScreen     low descending drone over dark noise
  state/mission_complete_01    State_Mission_Complete  warm major swell under the beacon
  save/save_complete_01.wav    Save_Complete    three rising blips
  save/load_complete_01.wav    Load_Complete    three blips, falling then settling
  enemy/death_01.wav           Scout_Death      falling screech breaking into a rattle
  pickup/metal_01.wav          Metal pickup     bright scrap clank
  pickup/organic_01.wav        Biomass pickup   soft wet squelch
  pickup/glass_01.wav          Electronics pickup  crystalline ping and blip
  pickup/generic_01.wav        default pickup   short thunk and tick

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


def render_swing() -> list[float]:
    """Servo-driven arm swing: a band of air that rises then falls, plus a faint whine."""
    rng = random.Random(7101)
    low = lower = 0.0
    out = []
    length = 0.3
    for i in range(samples(length)):
        t = i / SAMPLE_RATE
        arc = math.sin(math.pi * t / length)
        white = rng.uniform(-1.0, 1.0)
        cutoff = 400.0 + 2_600.0 * arc
        low += one_pole(cutoff) * (white - low)
        lower += one_pole(cutoff * 0.3) * (low - lower)
        whine = 0.12 * math.sin(sweep_phase(t, 320.0, 640.0, length))
        out.append(((low - lower) * 1.8 + whine) * arc ** 1.5)
    return out


def render_impact() -> list[float]:
    """Arm meets carapace: a dense low thud, a crunch and a short metal ring."""
    rng = random.Random(7203)
    low = 0.0
    modes = [(410.0, 0.3, 0.09), (1130.0, 0.2, 0.06), (2870.0, 0.1, 0.04)]
    out = []
    for i in range(samples(0.35)):
        t = i / SAMPLE_RATE
        white = rng.uniform(-1.0, 1.0)
        low += one_pole(1_800.0) * (white - low)
        crunch = low * envelope(t, 0.0008, 0.08) * 2.2
        body = math.sin(TAU * 68.0 * t) * envelope(t, 0.002, 0.22)
        ring = sum(level * math.sin(TAU * f * t) * envelope(t, 0.001, decay) for f, level, decay in modes)
        out.append(crunch + body * 0.9 + ring)
    return out


def render_ready() -> list[float]:
    """Cooldown over: a latch click then a short high confirmation tone."""
    rng = random.Random(7307)
    out = []
    for i in range(samples(0.2)):
        t = i / SAMPLE_RATE
        click = rng.uniform(-1.0, 1.0) * envelope(t, 0.0003, 0.012)
        tone_t = t - 0.03
        tone = 0.0
        if tone_t >= 0.0:
            tone = (math.sin(TAU * 1_760.0 * tone_t) + 0.3 * math.sin(TAU * 2_640.0 * tone_t)) * envelope(tone_t, 0.004, 0.15)
        out.append(click * 0.8 + tone * 0.5)
    return out


def bell(t: float, modes: list[tuple[float, float, float]]) -> float:
    """Sum of decaying sine partials: (frequency, level, decay seconds)."""
    return sum(level * math.sin(TAU * f * t) * envelope(t, 0.002, decay) for f, level, decay in modes)


def blips(t: float, notes: list[tuple[float, float]], length: float) -> float:
    """Short sine blips: (start seconds, frequency) each lasting `length`."""
    total = 0.0
    for start, f in notes:
        local = t - start
        if 0.0 <= local < length:
            total += math.sin(TAU * f * local) * math.sin(math.pi * local / length)
    return total


def render_ui_select() -> list[float]:
    return [blips(i / SAMPLE_RATE, [(0.0, 880.0), (0.05, 1320.0)], 0.06) for i in range(samples(0.12))]


def render_ui_hover() -> list[float]:
    return [math.sin(TAU * 1_050.0 * (i / SAMPLE_RATE)) * envelope(i / SAMPLE_RATE, 0.002, 0.03)
            for i in range(samples(0.05))]


def render_ui_menu_toggle() -> list[float]:
    rng = random.Random(8101)
    low = 0.0
    out = []
    for i in range(samples(0.26)):
        t = i / SAMPLE_RATE
        white = rng.uniform(-1.0, 1.0)
        low += one_pole(1_200.0) * (white - low)
        air = low * math.sin(math.pi * t / 0.26) * 0.8
        out.append(air + blips(t, [(0.02, 440.0), (0.11, 330.0)], 0.12) * 0.6)
    return out


def render_craft_complete() -> list[float]:
    """Assembly: three ratchet clicks, then a servo settling into a latch."""
    rng = random.Random(8203)
    out = []
    for i in range(samples(0.55)):
        t = i / SAMPLE_RATE
        clicks = sum(rng.uniform(-1.0, 1.0) * envelope(t - c, 0.0004, 0.02)
                     for c in (0.0, 0.07, 0.14) if t >= c)
        servo = 0.0
        if 0.2 <= t < 0.45:
            local = t - 0.2
            servo = 0.3 * math.sin(sweep_phase(local, 260.0, 520.0, 0.25)) * math.sin(math.pi * local / 0.25)
        latch = bell(t - 0.45, [(620.0, 0.5, 0.08), (1_540.0, 0.3, 0.05)]) if t >= 0.45 else 0.0
        out.append(clicks * 0.7 + servo + latch)
    return out


def render_objective() -> list[float]:
    """Two-note bell chime, a rising fifth: an objective is done."""
    out = []
    for i in range(samples(0.8)):
        t = i / SAMPLE_RATE
        first = bell(t, [(660.0, 0.6, 0.5), (1_320.0, 0.2, 0.3), (1_985.0, 0.1, 0.2)])
        second = bell(t - 0.14, [(990.0, 0.6, 0.6), (1_980.0, 0.2, 0.35)]) if t >= 0.14 else 0.0
        out.append(first + second)
    return out


def render_save_complete() -> list[float]:
    return [blips(i / SAMPLE_RATE, [(0.0, 740.0), (0.07, 988.0), (0.14, 1_480.0)], 0.08)
            for i in range(samples(0.26))]


def render_load_complete() -> list[float]:
    return [blips(i / SAMPLE_RATE, [(0.0, 1_480.0), (0.07, 988.0), (0.14, 1_245.0)], 0.08)
            for i in range(samples(0.26))]


def render_defeat() -> list[float]:
    """A low descending drone with a dark noise bed."""
    rng = random.Random(8307)
    low = 0.0
    length = 2.0
    out = []
    for i in range(samples(length)):
        t = i / SAMPLE_RATE
        phase = sweep_phase(t, 220.0, 98.0, length)
        white = rng.uniform(-1.0, 1.0)
        low += one_pole(300.0) * (white - low)
        tone = math.sin(phase) + 0.4 * math.sin(2.0 * phase) + 0.2 * math.sin(3.0 * phase)
        shape = min(1.0, t / 0.08) * math.exp(-1.6 * t / length)
        out.append((tone * 0.7 + low * 1.5) * shape)
    return out


def render_mission_complete() -> list[float]:
    """A warm major swell that rises under the beacon and settles."""
    chord = [220.0, 277.18, 329.63, 440.0, 554.37]
    length = 3.0
    out = []
    for i in range(samples(length)):
        t = i / SAMPLE_RATE
        swell = min(1.0, t / 1.2) * (1.0 if t < 1.8 else math.exp(-3.0 * (t - 1.8)))
        shimmer = 1.0 + 0.08 * math.sin(TAU * 5.0 * t)
        tone = sum(math.sin(TAU * f * t + k) / (k + 1) for k, f in enumerate(chord))
        out.append(tone * swell * shimmer)
    return out


def render_scout_death() -> list[float]:
    """The Scout's last cry: a long falling screech breaking into a wet rattle."""
    rng = random.Random(8401)
    noise = 0.0
    length = 0.9
    out = []
    for i in range(samples(length)):
        t = i / SAMPLE_RATE
        phase = sweep_phase(t, 1_500.0, 150.0, length)
        roughness = 0.5 + 0.5 * math.sin(TAU * (30.0 + 40.0 * t) * t)
        white = rng.uniform(-1.0, 1.0)
        noise += one_pole(900.0) * (white - noise)
        rattle = noise * (0.5 + 0.5 * math.sin(TAU * 18.0 * t)) * min(1.0, t / 0.4)
        cry = (math.sin(phase) + 0.4 * math.sin(2.0 * phase)) * roughness * math.exp(-2.0 * t)
        out.append((cry + rattle * 1.2) * envelope(t, 0.01, length))
    return out


def render_pickup_metal() -> list[float]:
    """Scrap lifted: a bright clank."""
    rng = random.Random(8501)
    out = []
    for i in range(samples(0.3)):
        t = i / SAMPLE_RATE
        hit = rng.uniform(-1.0, 1.0) * envelope(t, 0.0005, 0.02)
        out.append(hit * 0.6 + bell(t, [(812.0, 0.5, 0.18), (2_133.0, 0.35, 0.12), (3_307.0, 0.2, 0.08)]))
    return out


def render_pickup_organic() -> list[float]:
    """Biomass pulled free: a soft wet squelch."""
    rng = random.Random(8603)
    low = 0.0
    length = 0.3
    out = []
    for i in range(samples(length)):
        t = i / SAMPLE_RATE
        white = rng.uniform(-1.0, 1.0)
        cutoff = 1_400.0 - 1_100.0 * t / length
        low += one_pole(cutoff) * (white - low)
        body = math.sin(sweep_phase(t, 180.0, 90.0, length)) * 0.4
        out.append((low * 1.6 + body) * envelope(t, 0.01, 0.25))
    return out


def render_pickup_glass() -> list[float]:
    """Electronics picked up: a crystalline ping with a digital blip."""
    out = []
    for i in range(samples(0.4)):
        t = i / SAMPLE_RATE
        ping = bell(t, [(2_390.0, 0.5, 0.3), (3_587.0, 0.3, 0.2), (5_210.0, 0.1, 0.1)])
        out.append(ping + blips(t, [(0.05, 1_760.0)], 0.04) * 0.4)
    return out


def render_pickup_generic() -> list[float]:
    """Any other item: a short thunk and a tick."""
    rng = random.Random(8707)
    low = 0.0
    out = []
    for i in range(samples(0.2)):
        t = i / SAMPLE_RATE
        white = rng.uniform(-1.0, 1.0)
        low += one_pole(700.0) * (white - low)
        thunk = (low * 1.8 + math.sin(TAU * 140.0 * t) * 0.6) * envelope(t, 0.002, 0.12)
        out.append(thunk + blips(t, [(0.06, 1_320.0)], 0.03) * 0.3)
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
    "weapon/swing_01.wav": (render_swing, 0.6),
    "weapon/impact_01.wav": (render_impact, 0.8),
    "weapon/ready_tone_01.wav": (render_ready, 0.5),
    "ui/select_01.wav": (render_ui_select, 0.5),
    "ui/hover_01.wav": (render_ui_hover, 0.3),
    "ui/menu_toggle_01.wav": (render_ui_menu_toggle, 0.5),
    "ui/craft_complete_01.wav": (render_craft_complete, 0.6),
    "state/objective_complete_01.wav": (render_objective, 0.6),
    "state/defeat_01.wav": (render_defeat, 0.7),
    "state/mission_complete_01.wav": (render_mission_complete, 0.6),
    "save/save_complete_01.wav": (render_save_complete, 0.5),
    "save/load_complete_01.wav": (render_load_complete, 0.5),
    "enemy/death_01.wav": (render_scout_death, 0.8),
    "pickup/metal_01.wav": (render_pickup_metal, 0.6),
    "pickup/organic_01.wav": (render_pickup_organic, 0.6),
    "pickup/glass_01.wav": (render_pickup_glass, 0.6),
    "pickup/generic_01.wav": (render_pickup_generic, 0.6),
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
