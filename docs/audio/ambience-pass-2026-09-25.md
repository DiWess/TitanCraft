# Ambience Pass — 2026-09-25 (workstream A, audio item 1)

**Owner:** Audio Director (D7), with the Gameplay Engineer for wiring and tests.
**Plan item:** `docs/production/top-tier-single-scene-plan.md`, workstream A — "replace the silent
library with project-authored audio, starting with ambience".
**Finding addressed:** playtest finding 7 (`docs/production/playtests/2026-09-25-journey.md`),
ambient part only.

## What was wrong

The quarter had no ambience. There were two separate faults:

1. The three ambient loops in `scenes/Main/Main.tscn` were never started: no `autoplay`, and no
   code calls them.
2. Their files were digital silence (peak sample 0), so starting them would have changed nothing.

## What changed

| Change | Where |
|---|---|
| Four loops authored in code, no third-party material: gusting wind, sea swell on the seawall, low volcanic rumble, the wreck's residual hum | `tools/audio/synthesize_ambience.py` → `assets/audio/sources/ambient/*.wav` |
| Loops declared inside each WAV (`smpl` chunk), because `.import` files are not committed here and Godot's default loop mode reads the chunk | same |
| Loops start with the scene (`autoplay`) | `scenes/Main/Main.tscn`, `AudioLayer_Ambient` |
| Layered by place: wind and rumble are a non-positional bed; the sea is placed seaward of the harbour plaza (40, −24), the hum at the buried hull (−4.7, −13.4) | same |
| A new sea loop and player node (`AmbientLoop_Sea`) | same |
| False Freesound metadata on the ambient nodes replaced with the real source | same |
| Integration test: every loop plays from load, loops forward, and each placed source is nearer its own landmark | `tests/Integration/IntegrationTestRunner.cs`, `TestAmbienceLoopsPlay` |
| Ratchet test: no new silent file; files leaving the silent list must be removed from it; ambience must match its generator | `tools/test_audio_sources.py`, run by `tools/test.sh` and `tools/test.ps1` |
| Harness measures each ambient loop per leg and records a finding if none is audible | `tools/visual_review/playthrough_journey.gd` |
| Probe that measures ambience by place, one full loop per station | `tools/visual_review/ambience_map.gd` |

The integration runner now waits 250 ms of real time before quitting. Without that, the audio
mix thread sometimes had not dropped the freed players' playbacks, and the engine reported the
four streams as leaked at exit (4 of 10 runs before, 0 of 5 after).

## Evidence

Measured with the engine's dummy audio driver, which mixes in real time: every ambient player is
routed to its own bus for the run (harness only; the game's bus layout is unchanged) and the
bus peak is read every frame. Values are the mean of those per-frame peaks, in dBFS, with −80 as
the floor.

### By place — `ambience_map.gd`, 24 s (one full loop) per station

| Station | Wind | Rumble | Sea | Machinery | Before (commit `69c3716`) |
|---|---:|---:|---:|---:|---|
| Spawn (0, 0) | −28.0 | −32.1 | −35.4 | −32.9 | all −80.0 |
| Workbench | −28.0 | −32.1 | −32.0 | −32.4 | all −80.0 |
| Wreck / save point | −28.0 | −32.4 | −36.4 | **−25.0** | all −80.0 |
| Scout arena | −27.7 | −32.1 | −28.4 | −36.5 | all −80.0 |
| Harbour / beacon | −27.9 | −32.3 | **−25.0** | −38.5 | all −80.0 |

What this shows: the bed (wind, rumble) holds level everywhere; the sea rises 10.4 dB from spawn
to the harbour and is the loudest layer there; the hum is the loudest layer at the wreck and
falls 13.5 dB by the harbour. Before this change every loop read the floor at every station (the
sea loop did not exist).

### On real input — `playthrough_journey.gd`

| Mode | Result | Legs | Game time | Exceptions | Findings | Loudest ambient loop per leg (walk mean) |
|---|---|---|---|---|---|---|
| Journey | victory | 10 / 10 | 47.8 s (47.82 s before) | 0 | none | −30.3 to −22.8 dB |
| Defeat (`-- --defeat`) | respawned at 1.78 m, 100 health | 11 / 11 | 52.85 s | 0 | none | −30.2 to −16.6 dB |

No regression in completion, time, stuck events or defeat recovery. The harness would have
recorded a finding for any leg with no loop above −60 dB; none was recorded.

### Tests

- `dotnet test`: 118 / 118.
- Integration suite: pass, including `TestAmbienceLoopsPlay`; smoke verifier exit 0.
- `tools/test_audio_sources.py`: 29 files, 4 audible, 25 known silent placeholders. Shown to fail
  when a loop is replaced with silence.
- `tools/audio/synthesize_ambience.py --check`: committed files match the generator.

## Limits

- **No one has listened to this.** These are measurements of the digital mix, not of speakers or
  of a player's experience. Whether the ambience sounds right is a human judgement, owed at the
  workstream A human playtest note (plan §5). Nothing here claims how it feels.
- The dummy driver mixes at 44.1 kHz stereo; a Windows machine's device may differ in rate and
  channel layout. The loops are 32 kHz mono, which Godot resamples.
- The loops are 24 s long. A player who stands still for minutes may notice the repeat.
- Sub-bass in the rumble will not reproduce on laptop speakers; the wind carries the bed there.
- The other 25 silent files (footsteps, Scout, weapon, UI, pickups, save, state) are untouched.
  README §16's seven cues are unaffected: they play the temporary tones in `assets/audio/temp/`.

## Verdict

`PASS` for this item: the ambience starts, loops, is audible on every leg of a walked journey,
and is layered by place, with tests on the real path and no regression in the harness gate.
Workstream A's audio exit ("ambient audio audible at every leg") is met on measurement; its human
playtest note is still outstanding.
