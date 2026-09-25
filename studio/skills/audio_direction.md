# Skill: Audio Direction

## purpose

Make the quarter audible and prove it: every cue that should fire does, ambience plays where
the player stands, and nothing fails silently.

## when_to_use

Any task touching ambience, sound cues, `AudioStreamPlayer` nodes, `src/Core/AudioCue.cs`, or
quality axis 7.

## required_inputs

- README §16 cue list and licensing rule.
- The scene's audio player nodes and every `AudioCue.Play` / `Play3D` call site.
- A walked playthrough log when claiming a cue fires in play.

## procedure

1. Build the coverage table: node, stream assigned, trigger (code path or autoplay), heard in play.
2. Treat a node with a stream and no trigger as a finding, not as working audio.
3. Wire the smallest trigger that fits (autoplay for ambience, a call site for events).
4. Add or update a test that proves the trigger, and re-run the walked playthrough.
5. Record licence and source for any new asset before it is committed.

## automatic_failures

- Declaring audio done because the nodes exist in the scene.
- Unknown-licence assets.
- Feel adjectives without a dated human note.

## output_format

- Coverage table (node / stream / trigger / heard in play):
- Changes and tests:
- Findings:
- Verdict: `PASS` | `FAIL_REPO_OWNED` | `NOT_GO`

## evidence_required

- Test output proving each new trigger.
- Walked-playthrough run id or log for in-play claims.
- Licence record for every new source.

## example_good_output

Coverage: 29 players, 29 with streams, 16 triggered by code, 3 ambient loops untriggered
(`AmbientLoop_Wind`, `_Rumble`, `_Machinery`). Set autoplay on all three; integration test asserts
each is playing 2 frames after scene load; walked playthrough re-run clean. Verdict: `PASS`.

## example_bad_output

Audio sounds good now; all the sound nodes are in the scene.
