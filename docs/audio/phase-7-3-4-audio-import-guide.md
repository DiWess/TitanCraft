# Phase 7.3.4: Audio File Import & Integration Guide
## Complete Workflow for Sourcing, Verifying, and Wiring Audio Assets

**Date:** 2026-07-07  
**Phase:** Phase 7.3.4 (Audio file import and smoke testing)  
**Status:** Implementation guide ready for developer execution  
**Estimated Time:** 1-2 hours (mostly automated download + interactive Godot testing)

---

## Overview

This guide provides a complete workflow to:
1. Download 33 CC0-licensed audio files from Freesound.org
2. Verify file integrity via SHA-256 hashing
3. Update Godot scene references to use actual audio files
4. Perform smoke testing of all audio playback
5. Validate 3D positional audio and mixing levels

**All files are CC0 public domain (Freesound.org) — legal for commercial use, no attribution required.**

---

## Part 1: Directory Structure (COMPLETED)

Asset directories created at `assets/audio/sources/`:
```
assets/audio/sources/
├── ambient/           (3 files: wind, rumble, machinery hum)
├── footsteps/         (6 files: metal, rock, ash walk+run variants)
├── weapon/            (4 files: swing, impact, ready tone, swing_impact combo)
├── enemy/             (5 files: alert, attack, hurt, idle, death)
├── ui/                (4 files: select, hover, craft complete, menu toggle)
├── pickup/            (4 files: metal, glass, organic, generic)
├── state/             (4 files: objective complete, victory, defeat, mission complete)
└── save/              (3 files: save complete, save progress, load complete)
```

**Status:** ✓ Directory structure created

---

## Part 2: Audio File Download Script

Use the provided Python script to batch-download all 33 files from Freesound.org.

**Prerequisites:**
- Python 3.8+
- Freesound API account (free tier allows downloads)
- Internet access (HTTPS through configured proxy)

**Script location:** `tools/audio_download.py` (see below)

**Usage:**
```bash
# Download all 33 audio files with progress tracking
python tools/audio_download.py --verify-sha256 --output-manifest

# Output:
# - Downloads all files to assets/audio/sources/[category]/
# - Verifies SHA-256 hashes against audio-provenance.md
# - Generates audio_manifest.json with file metadata
```

**Manual Download (if script unavailable):**
1. Visit each Freesound URL from audio-provenance.md
2. Download WAV file to corresponding category folder
3. Rename to exact filename in provenance document
4. Verify SHA-256 hash (see Verification section below)

---

## Part 3: Audio File Manifest & Verification

### Downloaded Files Checklist

All 33 files should be present:

**Ambient (3 files):**
- [ ] ambient/wind_ambience_loop_01.wav (120s, 2.1 MB)
- [ ] ambient/volcanic_rumble_loop_01.wav (90s, 1.6 MB)
- [ ] ambient/machinery_hum_loop_01.wav (60s, 1.0 MB)

**Footsteps (6 files):**
- [ ] footsteps/metal_walk_01.wav (0.7s)
- [ ] footsteps/metal_run_01.wav (0.5s)
- [ ] footsteps/rock_walk_01.wav (0.6s)
- [ ] footsteps/rock_run_01.wav (0.5s)
- [ ] footsteps/ash_walk_01.wav (0.5s)
- [ ] footsteps/ash_run_01.wav (0.4s)

**Weapon (4 files):**
- [ ] weapon/swing_01.wav (0.4s, swing only)
- [ ] weapon/impact_01.wav (0.2s, hit confirmation)
- [ ] weapon/ready_tone_01.wav (0.2s, cooldown complete)
- [ ] weapon/swing_impact_combo_01.wav (0.6s, optional: swing+impact in one file)

**Enemy (5 files):**
- [ ] enemy/alert_01.wav (1.2s, detection)
- [ ] enemy/attack_01.wav (0.4s, strike)
- [ ] enemy/hurt_01.wav (0.5s, damage feedback)
- [ ] enemy/idle_01.wav (2.0s, ambient breathing, optional)
- [ ] enemy/death_01.wav (2.5s, defeat fanfare)

**UI (4 files):**
- [ ] ui/select_01.wav (0.2s, confirmation click)
- [ ] ui/hover_01.wav (0.15s, focus change)
- [ ] ui/craft_complete_01.wav (0.5s, crafting success)
- [ ] ui/menu_toggle_01.wav (0.3s, pause/resume)

**Pickup (4 files):**
- [ ] pickup/metal_01.wav (0.3s, resource collection)
- [ ] pickup/glass_01.wav (0.3s, electronics)
- [ ] pickup/organic_01.wav (0.35s, biomass)
- [ ] pickup/generic_01.wav (0.25s, fallback)

**State (4 files):**
- [ ] state/objective_complete_01.wav (0.8s, phase transition)
- [ ] state/victory_01.wav (3.2s, game win)
- [ ] state/defeat_01.wav (1.5s, game loss)
- [ ] state/mission_complete_01.wav (2.0s, full mission success)

**Save (3 files):**
- [ ] save/save_complete_01.wav (0.4s, checkpoint saved)
- [ ] save/save_progress_01.wav (0.2s, saving indicator)
- [ ] save/load_complete_01.wav (0.4s, game restored)

### SHA-256 Verification

Verify file integrity after download:

```bash
# Verify all files against audio-provenance.md hashes
cd /home/user/TitanCraft/assets/audio/sources
for category in ambient footsteps weapon enemy ui pickup state save; do
  for file in $category/*.wav; do
    sha256sum "$file"
  done
done > /tmp/audio_hashes.txt

# Compare against audio-provenance.md — all hashes must match
# If mismatches found, re-download the affected file
```

---

## Part 4: Godot Scene Integration

Update `scenes/Main/Main.tscn` to reference actual audio files instead of placeholders.

### Current State (Placeholder References)

All 31 AudioStreamPlayer nodes in Main.tscn currently reference:
- ExtResource("40_pickup_audio") — temporary placeholder
- ExtResource("41_craft_audio") — temporary placeholder  
- ExtResource("42_beacon_audio") — temporary placeholder

### Integration Steps

**Step 1: Add Audio Resources to Main.tscn**

After line 28 (last ExtResource), add:

```
[ext_resource type="AudioStream" path="res://assets/audio/sources/ambient/wind_ambience_loop_01.wav" id="51_wind_ambient"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/ambient/volcanic_rumble_loop_01.wav" id="52_rumble_ambient"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/ambient/machinery_hum_loop_01.wav" id="53_machinery_hum"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/footsteps/metal_walk_01.wav" id="54_footsteps_metal"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/footsteps/rock_walk_01.wav" id="55_footsteps_rock"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/footsteps/ash_walk_01.wav" id="56_footsteps_ash"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/weapon/swing_01.wav" id="57_weapon_swing"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/weapon/impact_01.wav" id="58_weapon_impact"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/weapon/ready_tone_01.wav" id="59_weapon_ready"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/enemy/alert_01.wav" id="60_enemy_alert"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/enemy/attack_01.wav" id="61_enemy_attack"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/enemy/hurt_01.wav" id="62_enemy_hurt"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/enemy/death_01.wav" id="63_enemy_death"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/ui/select_01.wav" id="64_ui_select"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/ui/hover_01.wav" id="65_ui_hover"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/ui/craft_complete_01.wav" id="66_ui_craft"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/ui/menu_toggle_01.wav" id="67_ui_menu"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/pickup/metal_01.wav" id="68_pickup_metal"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/pickup/glass_01.wav" id="69_pickup_glass"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/pickup/organic_01.wav" id="70_pickup_organic"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/pickup/generic_01.wav" id="71_pickup_generic"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/state/objective_complete_01.wav" id="72_state_objective"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/state/victory_01.wav" id="73_state_victory"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/state/defeat_01.wav" id="74_state_defeat"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/state/mission_complete_01.wav" id="75_state_mission"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/save/save_complete_01.wav" id="76_save_complete"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/save/save_progress_01.wav" id="77_save_progress"]
[ext_resource type="AudioStream" path="res://assets/audio/sources/save/load_complete_01.wav" id="78_load_complete"]
```

**Step 2: Update AudioStreamPlayer Nodes**

For each AudioLayer, update the `stream` property to reference the new ExtResource:

Example (wind ambience):
```
[node name="AmbientLoop_Wind" type="AudioStreamPlayer3D" parent="AudioLayer_Ambient"]
stream = ExtResource("51_wind_ambient")  # Changed from ("40_pickup_audio")
volume_db = -12.0
```

**Step 3: Set Audio Bus Properties (Optional but Recommended)**

Add audio bus configuration to Main.tscn for professional mixing control:

```
[node name="AudioBus" type="AudioBusLayout" parent="."]
metadata/bus_name = "Master"
```

---

## Part 5: Smoke Testing Checklist

### Manual Testing in Godot Editor

**Launch:** Open Godot editor → Load scenes/Main/Main.tscn

**Test 1: Scene Load & Ambient Loops (5 min)**
- [ ] Scene loads without audio errors
- [ ] Wind ambience plays automatically on scene load
- [ ] Rumble and machinery hum play simultaneously
- [ ] All three ambient sounds layerable (no clipping)
- [ ] Ambient loops restart seamlessly at loop points
- [ ] Expected result: Natural atmospheric background at 0.3 volume

**Test 2: Footstep Triggers (5 min)**
- [ ] Move player with WASD → hear footstep audio
- [ ] Footsteps play on grounded surfaces only (not while jumping)
- [ ] Metal footsteps play when on metal surfaces
- [ ] Rock/ash footsteps play on corresponding terrain
- [ ] Footstep timing matches player movement speed
- [ ] Footsteps do not overlap (one step finishes before next begins)

**Test 3: Weapon Audio (3 min)**
- [ ] Left-click attack → swing audio plays
- [ ] Hit confirmation plays ONLY when raycast hits scout
- [ ] Missed attacks play swing only (no impact audio)
- [ ] Ready tone plays when cooldown completes
- [ ] Weapon sounds don't interfere with enemy audio

**Test 4: Scout Enemy Audio (5 min)**
- [ ] Approach scout within 12m → alert audio triggers
- [ ] Scout enters attack range (2m) → attack audio plays
- [ ] Damage scout → hurt audio plays
- [ ] Defeat scout (0 HP) → death fanfare plays
- [ ] Scout sounds originate from scout position (3D positioning)
- [ ] Scout audio does not cut off other sounds

**Test 5: UI Audio (3 min)**
- [ ] Menu hover → subtle hover audio
- [ ] Menu selection → confirmation click
- [ ] Crafting completion → craft success audio
- [ ] Pause/resume → menu toggle audio
- [ ] UI sounds are non-positional (same volume from all angles)

**Test 6: State & Resource Audio (5 min)**
- [ ] Resource pickup → distinct pickup audio for each resource type
- [ ] Objective completion → objective audio
- [ ] Victory condition → victory fanfare
- [ ] Defeat condition → defeat audio
- [ ] Save checkpoint → save confirmation audio

**Test 7: Audio Mixing & Levels (5 min)**
- [ ] No clipping or distortion at any volume level
- [ ] Dialogue/UI audio not masked by gameplay sounds
- [ ] Ambient sounds not drowning out critical audio cues
- [ ] All audio clear and distinguishable
- [ ] Expected output: Professional-quality audio mix

### Automated Verification (Optional)

Check audio file properties in command line:

```bash
# Check all audio files for proper formatting
ffprobe -v quiet -show_entries format=duration,sample_rate,channels \
  assets/audio/sources/*/*.wav > audio_verification.json

# Verify all files are valid WAV format (not corrupt)
find assets/audio/sources -name "*.wav" -exec sh -c '
  if ! ffprobe -v quiet "$1" >/dev/null 2>&1; then
    echo "CORRUPT: $1"
  fi
' sh {} \;
```

---

## Part 6: Audio Mixing Reference Levels

Reference volume levels documented in integration plan. Adjust in Main.tscn if needed:

| Audio Category | Type | Volume (dB) | Purpose |
|---|---|---|---|
| Wind Ambience | 3D Loop | -12.0 (-0.3) | Background atmosphere |
| Rumble Ambience | 3D Loop | -14.0 (-0.2) | Distant geological activity |
| Machinery Hum | 3D Loop | -16.0 (-0.15) | Ship system hum |
| Footsteps (all) | 3D Positional | -8.0 (-0.6) | Player movement feedback |
| Weapon Swing | 3D Positional | -6.0 (-0.5) | Attack initiation |
| Weapon Impact | 3D Positional | -5.0 (-0.56) | Hit confirmation (high priority) |
| Weapon Ready | 3D Positional | -10.0 (-0.31) | Cooldown complete |
| Scout Alert | 3D Positional | -7.0 (-0.45) | Detection alert |
| Scout Attack | 3D Positional | -6.0 (-0.5) | Melee strike |
| Scout Hurt | 3D Positional | -8.0 (-0.6) | Damage feedback |
| Scout Death | 3D Positional | -4.0 (-0.63) | Defeat fanfare (high priority) |
| UI Select | 2D Non-positional | -10.0 (-0.31) | Menu confirmation |
| UI Hover | 2D Non-positional | -12.0 (-0.25) | Menu focus |
| UI Craft | 2D Non-positional | -7.0 (-0.45) | Crafting success |
| UI Menu Toggle | 2D Non-positional | -9.0 (-0.35) | Pause/resume |
| Pickup Audio (all) | 3D Positional | -6.0 (-0.5) | Resource feedback |
| State Objective | 2D Non-positional | -8.0 (-0.4) | Phase transition |
| State Victory | 2D Non-positional | -6.0 (-0.5) | Win condition |
| State Defeat | 2D Non-positional | -5.0 (-0.56) | Loss condition |
| State Mission | 2D Non-positional | -7.0 (-0.45) | Full completion |

**Adjustment:** If audio is too loud or too quiet, adjust volume_db in AudioStreamPlayer nodes.

---

## Part 7: Completion Checklist

- [ ] All 33 audio files downloaded to assets/audio/sources/
- [ ] SHA-256 verification passed for all files
- [ ] Main.tscn ExtResource entries added (27 audio files)
- [ ] All 31 AudioStreamPlayer nodes updated with actual audio references
- [ ] Scene loads without audio errors or missing resource warnings
- [ ] All 7 smoke test categories pass (ambient, footsteps, weapon, scout, UI, state, mixing)
- [ ] Audio positioning verified (3D positional sounds originate from correct world position)
- [ ] No audio clipping or distortion at any point
- [ ] Audio mixing professional quality and non-intrusive
- [ ] Performance impact acceptable (60+ FPS maintained)

---

## Part 8: Known Issues & Workarounds

### Issue: Audio file not found

**Symptom:** Console warnings "Cannot load resource 'res://assets/audio/sources/...'

**Cause:** File path mismatch or file not in assets/ directory

**Fix:** 
1. Verify file exists at path: `ls assets/audio/sources/[category]/[filename].wav`
2. Check file extension is .wav (not .mp3 or other format)
3. Verify filename matches exactly (case-sensitive on Linux)
4. Reload Godot project (Project → Reimport Assets)

### Issue: Audio plays but sounds distorted or muffled

**Symptom:** Audio playback has distortion, reverb, or sounds wrong

**Cause:** Godot audio processor mismatch or corrupted file

**Fix:**
1. Re-download file from Freesound.org
2. Verify SHA-256 hash matches audio-provenance.md
3. Check volume level isn't clipping (-3dB or lower for safety)
4. Verify file is valid: `ffprobe assets/audio/sources/[category]/[filename].wav`

### Issue: Footstep audio doesn't trigger during movement

**Symptom:** Player moves but no footstep audio

**Cause:** Footstep integration not yet wired in FirstPersonMovement.cs

**Fix:** Footstep triggering is scheduled for Phase 7.4.1. For Phase 7.3.4 smoke testing, manually trigger with:
```csharp
AudioCue.Play3D(this, "AudioLayer_Player/Footsteps_Metal", GlobalPosition);
```

---

## Summary

Phase 7.3.4 transforms the audio integration from code-ready to fully functional:

**Deliverables:**
- 33 CC0-licensed audio files sourced and verified
- 27 new ExtResource entries in Main.tscn (audio files)
- 31 AudioStreamPlayer nodes wired to actual audio
- Smoke test validation across all 7 audio categories
- Professional audio mix with proper level balancing

**Next Step:** Phase 7.3.5 (Creative Director Gate Review)
- Creative Director reviews implementation completeness
- Verifies audio coherence with gameplay and visual design
- Approves for Phase 7.4 polish cycle

**Estimated Time:** 1-2 hours (mostly automated download + 30 min interactive testing)

**Status:** Ready for developer implementation

---

**Prepared by:** Phase 7.3.3 Audio Integration System  
**Date:** 2026-07-07  
**Authority:** audio-integration-plan.md + audio-provenance.md
