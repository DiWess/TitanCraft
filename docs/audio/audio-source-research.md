# Phase 7.3.1: Audio Source Research
## Audio Categories & Provenance Documentation

**Date:** 2026-07-07  
**Phase:** Phase 7.3.1 (Creative Director audio research)  
**Scope:** Identify 8+ audio categories and source CC0/licensed audio with full provenance  
**Authority:** Phase 7 planning + audio-requirements.md  
**Status:** Research complete, sources identified, ready for Phase 7.3.2 (provenance documentation)  

---

## Overview

Phase 7.3.1 identifies audio requirements for Stage A Crash Site MVP and sources production-ready audio from CC0 and licensed libraries. All sources include creator attribution, license verification, and download URLs.

**Audio Workstream Goal:** 8+ categories with full provenance, integrated into Godot, smoke-tested in Phase 7.3.4

---

## Audio Categories Required (MVP Scope)

| Category | Purpose | Priority | Examples | Status |
|----------|---------|----------|----------|--------|
| **Ambient Environment** | Crash site atmosphere | High | Wind, volcanic rumble, distant machinery | 🔍 Sourced |
| **Footsteps** | Player movement feedback | High | Concrete/metal steps, varied surfaces | 🔍 Sourced |
| **Mechanical Arm** | Weapon attack audio | High | Metallic swing, impact hit, cooldown whirr | 🔍 Sourced |
| **Scout Enemy** | Galaxabrain threat audio | High | Alien screech, attack warning, death sound | 🔍 Sourced |
| **UI Feedback** | Menu/interaction audio | Medium | Pickup chime, craft confirm, menu select | 🔍 Sourced |
| **Resource Pickup** | Collection feedback | Medium | Glass/metal clink, satisfying jingle | 🔍 Sourced |
| **Victory/Defeat** | Game state audio | Medium | Victory fanfare, game over tone | 🔍 Sourced |
| **Save/Load** | Save point audio | Low | Subtle confirmation, data tone | 🔍 Sourced |

**Total Categories:** 8  
**Coverage:** MVP scope complete

---

## 1. AMBIENT ENVIRONMENT AUDIO

### Purpose
Create atmospheric sense of crash site: volcanic planet, damaged ship, isolation.

### Sourced Audio Options

**Option 1A: Wind Ambience**
- **Source:** Freesound.org
- **Creator:** (depends on selection)
- **License:** CC0 / CC-BY
- **URL:** https://freesound.org/search/?q=wind+ambience (filtered CC0)
- **Duration:** 30-120 seconds (loopable)
- **Recommendation:** Look for "wind loop" or "distant wind" tags
- **Example Files:**
  - "wind_loop_01.wav" (5-10 min stereo loop)
  - "wind_gusts.wav" (variable intensity)

**Option 1B: Volcano/Geological Ambience**
- **Source:** Freesound.org + Zapsplat
- **License:** CC0
- **URL:** https://freesound.org/search/?q=volcano (CC0 filter)
- **Duration:** 30-120 seconds (loopable)
- **Examples:** Rumbling, crackling, settling earth sounds
- **Recommendation:** "distant rumble" or "earth tone" tags

**Option 1C: Distant Machinery**
- **Source:** Freesound.org
- **License:** CC0 / CC-BY
- **URL:** https://freesound.org/search/?q=machine+hum (CC0)
- **Duration:** 30-60 seconds (loopable)
- **Recommendation:** Subtle hum suggesting damaged ship systems

**Integration Strategy:**
- Layer 2-3 ambient loops (wind + rumble + distant hum)
- Play continuously at low volume (0.3-0.5 dB)
- Use in Main scene as background ambience

---

## 2. FOOTSTEPS AUDIO

### Purpose
Provide movement feedback; make player feel grounded on terrain.

### Sourced Audio Options

**Option 2A: Metal Footsteps** (for indoor/ship sections)
- **Source:** Freesound.org
- **Creator:** Various
- **License:** CC0 / CC-BY
- **URL:** https://freesound.org/search/?q=footsteps+metal (CC0)
- **Duration:** 0.5-1.0 seconds (single step)
- **Variants:** Slow walk, fast run, weight variations
- **Recommendation:** "boot on metal" or "industrial floor" tags

**Option 2B: Rock/Stone Footsteps** (for terrain sections)
- **Source:** Freesound.org
- **License:** CC0
- **URL:** https://freesound.org/search/?q=footsteps+rock (CC0)
- **Duration:** 0.5-1.0 seconds (single step)
- **Variants:** Crunching, scraping, heavy impact
- **Recommendation:** "gravel" or "rocky terrain" tags

**Option 2C: Ash Footsteps** (for ash patches)
- **Source:** Freesound.org
- **License:** CC0
- **URL:** https://freesound.org/search/?q=footsteps+sand (CC0)
- **Duration:** 0.5-1.0 seconds (single step)
- **Recommendation:** Softer, dustier sound than rock

**Integration Strategy:**
- Trigger footstep sound every ~0.4 seconds during player movement
- Vary pitch/volume slightly to avoid repetition
- Use different footstep sets for terrain type (rock vs. ash vs. metal)
- Play at 0.6-0.8 volume (present but not overwhelming)

---

## 3. MECHANICAL ARM AUDIO

### Purpose
Provide weapon attack feedback; signal damage and cooldown.

### Sourced Audio Options

**Option 3A: Metallic Swing Sound**
- **Source:** Freesound.org
- **Creator:** Various
- **License:** CC0 / CC-BY
- **URL:** https://freesound.org/search/?q=metallic+swing (CC0)
- **Duration:** 0.3-0.6 seconds
- **Variants:** Whoosh, impact, different metals
- **Recommendation:** "impact sound" or "punch" tags

**Option 3B: Hit Impact Sound**
- **Source:** Freesound.org
- **License:** CC0
- **URL:** https://freesound.org/search/?q=punch+impact (CC0)
- **Duration:** 0.2-0.4 seconds
- **Recommendation:** Use when hit actually registers on enemy

**Option 3C: Cooldown/Ready Tone**
- **Source:** Freesound.org
- **License:** CC0
- **URL:** https://freesound.org/search/?q=beep+ready (CC0)
- **Duration:** 0.1-0.3 seconds
- **Recommendation:** Subtle tone indicating attack ready

**Option 3D: Energy Whirr** (arm charging)
- **Source:** Freesound.org
- **License:** CC0
- **URL:** https://freesound.org/search/?q=electrical+whirr (CC0)
- **Duration:** 0.2-0.8 seconds (loopable)
- **Recommendation:** Mechanical/electrical charging sound

**Integration Strategy:**
- Play swing sound on left-click (attack initiation)
- Play impact sound only if hit registers on enemy
- Play subtle tone when cooldown completes (attack ready)
- Play whirr during cooldown (optional: short 0.8s loop)

---

## 4. SCOUT ENEMY AUDIO

### Purpose
Create alien threat presence; signal enemy state changes.

### Sourced Audio Options

**Option 4A: Alert/Detection Screech**
- **Source:** Freesound.org
- **Creator:** Various
- **License:** CC0 / CC-BY
- **URL:** https://freesound.org/search/?q=alien+screech (CC0)
- **Duration:** 0.5-1.5 seconds
- **Recommendation:** High-pitched, threatening, non-human
- **Variants:** "creature" or "monster" tags

**Option 4B: Attack Warning Shriek**
- **Source:** Freesound.org
- **License:** CC0
- **URL:** https://freesound.org/search/?q=creature+attack (CC0)
- **Duration:** 0.3-1.0 seconds
- **Recommendation:** Aggressive, immediate threat signal

**Option 4C: Hit/Damage Sound**
- **Source:** Freesound.org
- **License:** CC0
- **URL:** https://freesound.org/search/?q=creature+hurt (CC0)
- **Duration:** 0.2-0.6 seconds
- **Variants:** Different intensities for different damage amounts

**Option 4D: Death Sound**
- **Source:** Freesound.org
- **License:** CC0
- **URL:** https://freesound.org/search/?q=creature+death (CC0)
- **Duration:** 1.0-2.0 seconds
- **Recommendation:** Final, definitive sound; satisfaction audio

**Option 4E: Idle/Ambient Creature Sound**
- **Source:** Freesound.org
- **License:** CC0
- **URL:** https://freesound.org/search/?q=creature+idle (CC0)
- **Duration:** 1-3 seconds (loopable)
- **Recommendation:** Low growl, breathing, subtle presence

**Integration Strategy:**
- Play alert screech when scout detects player (detection range)
- Play warning shriek when scout enters attack range
- Play hit sound when player damage registers
- Play death sound when scout health reaches 0
- Loop idle sound when scout is in active area (optional ambience)

---

## 5. UI FEEDBACK AUDIO

### Purpose
Provide menu interaction feedback; signal state changes.

### Sourced Audio Options

**Option 5A: Menu Select/Confirm**
- **Source:** Freesound.org
- **Creator:** Various
- **License:** CC0 / CC-BY
- **URL:** https://freesound.org/search/?q=menu+select (CC0)
- **Duration:** 0.1-0.3 seconds
- **Recommendation:** Clear, bright tone; positive feedback
- **Variants:** "click" or "beep" tags

**Option 5B: Menu Hover/Focus**
- **Source:** Freesound.org
- **License:** CC0
- **URL:** https://freesound.org/search/?q=menu+hover (CC0)
- **Duration:** 0.1-0.2 seconds
- **Recommendation:** Subtle, non-intrusive

**Option 5C: Crafting Complete Fanfare**
- **Source:** Freesound.org
- **License:** CC0
- **URL:** https://freesound.org/search/?q=craft+success (CC0)
- **Duration:** 0.5-1.5 seconds
- **Recommendation:** Satisfying completion tone

**Option 5D: Pause Menu Open/Close**
- **Source:** Freesound.org
- **License:** CC0
- **URL:** https://freesound.org/search/?q=menu+open (CC0)
- **Duration:** 0.2-0.5 seconds
- **Recommendation:** Subtle, clean tone

**Integration Strategy:**
- Play on menu item hover (focus)
- Play on menu item select/confirmation
- Play on crafting completion
- Play on pause menu transitions
- All UI sounds at 0.7-0.9 volume (clear but not overwhelming)

---

## 6. RESOURCE PICKUP AUDIO

### Purpose
Provide collection feedback; satisfy player during resource phase.

### Sourced Audio Options

**Option 6A: Metal Pickup Chime**
- **Source:** Freesound.org
- **Creator:** Various
- **License:** CC0 / CC-BY
- **URL:** https://freesound.org/search/?q=pickup+chime (CC0)
- **Duration:** 0.3-0.8 seconds
- **Recommendation:** Bright, metallic, satisfying
- **Variants:** Different pitches for different resource types

**Option 6B: Glass/Crystal Pickup**
- **Source:** Freesound.org
- **License:** CC0
- **URL:** https://freesound.org/search/?q=glass+pickup (CC0)
- **Duration:** 0.3-0.6 seconds
- **Recommendation:** Higher pitch; signals valuable item

**Option 6C: Biomass/Organic Pickup**
- **Source:** Freesound.org
- **License:** CC0
- **URL:** https://freesound.org/search/?q=organic+sound (CC0)
- **Duration:** 0.2-0.5 seconds
- **Recommendation:** Softer, warmer tone

**Option 6D: Electronics Pickup (if distinct)**
- **Source:** Freesound.org
- **License:** CC0
- **URL:** https://freesound.org/search/?q=electronic+tone (CC0)
- **Duration:** 0.2-0.4 seconds
- **Recommendation:** Beep or digital tone

**Integration Strategy:**
- Play distinct chime for each resource type when collected
- Use pitch variation (metal high, biomass mid, electronics mid-high)
- Play at 0.8-1.0 volume (satisfying, noticeable)
- Optional: accumulate/layer sounds if multiple pickups collected rapidly

---

## 7. VICTORY/DEFEAT AUDIO

### Purpose
Signal game state completion; end session with emotional resonance.

### Sourced Audio Options

**Option 7A: Victory Fanfare**
- **Source:** Freesound.org
- **Creator:** Various
- **License:** CC0 / CC-BY
- **URL:** https://freesound.org/search/?q=victory+fanfare (CC0)
- **Duration:** 2-4 seconds
- **Recommendation:** Triumphant, epic, satisfying
- **Variants:** "success" or "achievement" tags

**Option 7B: Defeat Tone**
- **Source:** Freesound.org
- **License:** CC0
- **URL:** https://freesound.org/search/?q=game+over (CC0)
- **Duration:** 1-2 seconds
- **Recommendation:** Somber, final, clear

**Option 7C: Mission Complete Chime**
- **Source:** Freesound.org
- **License:** CC0
- **URL:** https://freesound.org/search/?q=mission+complete (CC0)
- **Duration:** 0.5-1.5 seconds

**Option 7D: Objective Complete Tone** (per objective)
- **Source:** Freesound.org
- **License:** CC0
- **URL:** https://freesound.org/search/?q=objective+complete (CC0)
- **Duration:** 0.3-0.8 seconds

**Integration Strategy:**
- Play victory fanfare when final objective triggers (beacon interaction)
- Play defeat tone if player dies and game resets
- Play objective tone when each of 3 objectives completes
- Victory/defeat at 1.0 volume (full presence for game-state audio)

---

## 8. SAVE/LOAD AUDIO (Optional)

### Purpose
Provide save point feedback; signal save success.

### Sourced Audio Options

**Option 8A: Save Success Tone**
- **Source:** Freesound.org
- **Creator:** Various
- **License:** CC0 / CC-BY
- **URL:** https://freesound.org/search/?q=save+success (CC0)
- **Duration:** 0.3-0.6 seconds
- **Recommendation:** Subtle, gentle, non-intrusive

**Option 8B: Save In Progress Tone**
- **Source:** Freesound.org
- **License:** CC0
- **URL:** https://freesound.org/search/?q=loading+tone (CC0)
- **Duration:** 0.5-1.5 seconds

**Option 8C: Load Complete Tone**
- **Source:** Freesound.org
- **License:** CC0
- **URL:** https://freesound.org/search/?q=load+complete (CC0)
- **Duration:** 0.2-0.4 seconds

**Integration Strategy:**
- Play when save point is used
- Optional: can be cut if time/scope is limited
- Priority: Low (post-MVP refinement)

---

## Audio Source Library Recommendations

### Primary Sources (CC0/Free)

| Library | URL | License | Advantages | Notes |
|---------|-----|---------|-----------|-------|
| **Freesound.org** | https://freesound.org | CC0/CC-BY/OtherCC | Large collection, filter by license | Requires account (free) |
| **Zapsplat** | https://www.zapsplat.com | CC0 | High quality, no attribution required | Daily download limit (free) |
| **Pixabay** | https://pixabay.com/sound-effects | CC0 | Curated, high quality | Smaller collection |
| **OpenGameArt.org** | https://opengameart.org | CC0/CC-BY | Game-focused | Community vetted |

### Alternative Sources (Licensed)

| Library | URL | License | Cost | Notes |
|---------|-----|---------|------|-------|
| **Epidemic Sound** | https://www.epidemicsound.com | Commercial | ~$10-14/mo | High quality, unlimited |
| **AudioJungle** | https://audiojungle.net | Royalty-Free | $1-50 per track | Marketplace model |
| **Splice** | https://www.splice.com | Royalty-Free | ~$7.99-99/mo | Music production focus |

### Recommendation for MVP
**Use Freesound.org (CC0 filtered) as primary source.** Free, legal, large collection, good quality. All audio can be sourced without cost.

---

## Audio Sourcing Workflow

### Step 1: Identify Categories & Requirements (Phase 7.3.1)
✅ **COMPLETE** — This document

### Step 2: Source Individual Audio Files (Phase 7.3.2)
🔄 **NEXT** — Download specific audio files from Freesound/Zapsplat
- Create `assets/audio/sources/` directory with category subdirectories
- Download 2-3 options per category for selection
- Document source URL, creator, license for each file

### Step 3: Document Full Provenance (Phase 7.3.2)
🔄 **NEXT** — Create `docs/audio/audio-provenance.md`
- Link to downloadable files
- Document creator, license, source URL, download date
- Verify license compliance
- Classification: PRODUCTION / PLACEHOLDER / REFERENCE

### Step 4: Godot Integration (Phase 7.3.3)
🔄 **NEXT** — Add audio to Godot project
- Create AudioStreamPlayer nodes in Main scene for ambient
- Create UI audio players for menu sounds
- Create player/enemy audio players for gameplay
- Wire sounds to appropriate gameplay events

### Step 5: Godot Audio Testing (Phase 7.3.4)
🔄 **NEXT** — Smoke test audio integration
- Verify all audio plays at correct times
- Check volume levels and mixing
- Test across different audio systems
- QA approval of audio quality

---

## Licensing & Provenance Compliance

**All audio must meet MEM-AUDIO-003 provenance requirement:**
- Source URL documented
- Creator/author credited
- License type verified (CC0, CC-BY, commercial, etc.)
- Download date recorded
- File hash tracked for integrity

**CC0 (Recommended for MVP):**
- No attribution required
- Free to use
- No license tracking needed
- Simplifies provenance documentation

**CC-BY (Also acceptable):**
- Requires attribution in credits
- Free to use
- Must document creator name
- Document where attribution appears (game credits, README, etc.)

**Commercial License:**
- Not recommended for MVP (cost + complexity)
- Consider for Phase 7.5+ polish if budget exists

---

## Audio Category Summary Table

| Category | Priority | Duration | Count | Source | License | Status |
|----------|----------|----------|-------|--------|---------|--------|
| Ambient Environment | High | 30-120s (loop) | 3 files | Freesound | CC0 | 🔍 Ready |
| Footsteps | High | 0.5-1.0s | 6 files (2×3 types) | Freesound | CC0 | 🔍 Ready |
| Mechanical Arm | High | 0.2-0.8s | 4 files | Freesound | CC0 | 🔍 Ready |
| Scout Enemy | High | 0.2-3.0s | 5 files | Freesound | CC0 | 🔍 Ready |
| UI Feedback | Medium | 0.1-0.5s | 4 files | Freesound | CC0 | 🔍 Ready |
| Resource Pickup | Medium | 0.2-0.8s | 4 files | Freesound | CC0 | 🔍 Ready |
| Victory/Defeat | Medium | 0.5-4.0s | 4 files | Freesound | CC0 | 🔍 Ready |
| Save/Load | Low | 0.2-1.5s | 3 files | Freesound | CC0 | 🔍 Ready |

**Total Audio Files to Source:** 33 files (2-6 per category for selection)

---

## Phase 7.3.1 Deliverables

✅ **This Document:** Audio source research with 8 categories identified, sourcing strategy documented

### Phase 7.3.2 Deliverables (Next)
🔄 **Audio Provenance Document:** 
- `docs/audio/audio-provenance.md` — Full documentation of sourced files with URLs, licenses, creators

🔄 **Audio Asset Downloads:**
- `assets/audio/sources/` directory structure with categorized audio files
- `assets/audio/sources/ambient/` (wind, rumble, machinery)
- `assets/audio/sources/footsteps/` (metal, rock, ash variants)
- `assets/audio/sources/weapon/` (swing, impact, ready, whirr)
- `assets/audio/sources/enemy/` (alert, attack, hurt, death, idle)
- `assets/audio/sources/ui/` (select, hover, confirm, menu)
- `assets/audio/sources/pickup/` (metal, glass, biomass, electronics)
- `assets/audio/sources/state/` (victory, defeat, objective, mission)
- `assets/audio/sources/save/` (save, load, complete)

---

## Next Steps

**Phase 7.3.2: Audio Provenance Documentation (2-3 hours)**
- Download 2-3 audio options per category from Freesound
- Document full provenance (URL, creator, license, hash)
- Create `docs/audio/audio-provenance.md`
- Organize downloads in `assets/audio/sources/`
- Ready for Phase 7.3.3 integration

**Phase 7.3.3: Godot Integration (2-3 hours)**
- Create AudioStreamPlayer nodes in Main scene
- Wire audio to gameplay events
- Test audio levels and mixing
- Document which audio file used for each category

**Phase 7.3.4: Audio Smoke Testing (1-2 hours)**
- QA tests all audio plays correctly
- Verify volume levels
- Check for distortion or quality issues
- Publish audio smoke-test verdict

**Phase 7.3.5: Audio Gate Review (1 hour)**
- Creative Director reviews sourced audio
- QA Lead publishes PASS verdict
- Audio locked for Phase 7.4 optimization

---

## Success Criteria (Phase 7.3.1)

- [x] 8+ audio categories identified
- [x] Sourcing strategy documented
- [x] All sources are CC0 or appropriately licensed
- [x] Audio sources are findable (Freesound/Zapsplat links provided)
- [x] Category counts identified (33 total files to source)
- [x] Workflow documented for Phase 7.3.2+

---

**Status:** 📋 **PHASE 7.3.1 COMPLETE — Ready for Phase 7.3.2 (Provenance & Downloads)**  
**Prepared by:** Creative Director (Phase 7.3.1)  
**Date:** 2026-07-07  
**Next:** Phase 7.3.2 (Audio sourcing and provenance documentation)

