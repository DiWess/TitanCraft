# Phase 7 Planning: Extended Production

**Status:** Planning document (governance/documentation work, not implementation)  
**Date:** 2026-07-05  
**Scope:** Define Phase 7 work following Phase 6 (Public demo preparation)  
**Authority:** README.md § 5-6 (MVP), AGENTS.md § 2 (product boundary), studio/memory/product_scope.md

---

## Phase 7 Overview

Phase 7 is **post-Phase 6** work that extends the Crash Site MVP foundation with composition guidance, balance validation, audio implementation, polish optimization, and expanded platform support. This document defines scope boundaries, success criteria, stage gates, and agent ownership for each phase 7 workstream.

### Key Constraints
- **MVP Preservation:** All Phase 7 work operates within locked Crash Site scope. Features listed in README.md § 6 (Forbidden scope) remain forbidden.
- **Windows-First Offline:** Platform target remains Windows PC, fully offline. No cloud, accounts, telemetry, or unsupported platforms.
- **Evidence Gates:** Visual, audio, gameplay, and release claims require independent review and concrete evidence (not confidence language).
- **Stage Gating:** Phase 7 does not begin until Phase 6 passes all gates (visual approval, gameplay validation, export proof, release readiness).

---

## Phase 7 Roadmap

### Phase 7.1: Art Director Composition Guide
**Agent Owner:** Art Director  
**Secondary:** Visual Reviewer, Creative Director  
**Duration Target:** 1-2 weeks (4-8 hours)

#### Objective
Document visual composition principles and style reference for the Crash Site environment to guide future asset creation, scene layout, and visual consistency after approved Stage A visuals are integrated.

#### Scope
- **In Scope:**
  - Focal point and route readability principles for Crash Site layout
  - Silhouette and scale relationships for crash site elements
  - Material and lighting coherence guidelines
  - Foreground, midground, background composition hierarchy
  - Color palette and contrast guidance (aligned with Art Taste Pack)
  - Reference to approved Stage A asset organization
  - Before/after visual examples (opened PNGs with diagnosis)

- **Out of Scope (Forbidden):**
  - Creating new assets
  - Replacing production scene visuals
  - Inventing new style directions beyond Art Taste Pack
  - Multi-stage or multi-biome composition (vision feature, not MVP)

#### Success Criteria
- [ ] Composition guide document exists in `docs/art/composition-guide.md`
- [ ] Document references approved Stage A visuals and established Art Taste Pack
- [ ] Visual examples are opened PNGs with focal point, route, silhouette, scale, material diagnosis
- [ ] Visual Reviewer provides independent approval verdict (not Art Director self-approval)
- [ ] No scene edits or asset modifications included

#### Dependencies
- Requires Phase 6 completion: Stage A visuals approved and integrated

#### Evidence Requirements
- Document file (markdown)
- Opened reference PNG screenshots with visual diagnosis
- Visual Reviewer verdict recorded
- Validation: `git diff --check`, no scene/asset changes

---

### Phase 7.2: Gameplay Balance Playtesting
**Agent Owner:** Gameplay Engineer + QA Lead  
**Secondary:** Technical Director, Producer  
**Duration Target:** 2-3 weeks (8-12 hours)

#### Objective
Validate and refine gameplay balance within the locked Crash Site loop: resource distribution, crafting costs, combat difficulty, enemy behavior, and overall progression pacing. Identify and fix balance-only bugs without expanding scope.

#### Scope
- **In Scope (Balance Adjustments Only):**
  - Resource spawn locations and quantities
  - Mechanical arm crafting resource costs
  - Player health, damage values, attack cooldown
  - Galaxabrain Scout health, damage, behavior timing
  - Enemy detection range, chase speed, attack frequency
  - Overall session pacing (10-30 minute target)
  - Balance configuration updates (not code rewrites)

- **Out of Scope (Forbidden):**
  - New gameplay systems (stamina, hunger, temperature, radiation, etc.)
  - New enemy types or variants
  - New weapons or equipment
  - Grappling hook, wall-running, or movement enhancements
  - Level design changes beyond prop placement
  - Difficulty modes or difficulty selection

#### Success Criteria
- [ ] Playtesting evidence recorded (smoke test results, bug list, balance notes)
- [ ] At least 5 complete playthrough sessions completed and documented
- [ ] Balance changes applied and validated with gameplay smoke tests
- [ ] No scope expansion beyond balance adjustments
- [ ] All repo-owned bugs identified and tracked or fixed

#### Playtesting Checklist
- [ ] Player movement and camera feel responsive
- [ ] Resource collection is neither too easy nor too tedious
- [ ] Crafting cost feels fair for effort invested
- [ ] Combat difficulty is challenging but fair
- [ ] Enemy AI is not frustrating (no stuck states, unreachable positioning)
- [ ] Victory and defeat flows work without crashes
- [ ] Session duration in target 10-30 minute range
- [ ] No sequence-breaking exploits discovered

#### Dependencies
- Requires Phase 6 completion: Windows export functional and playable

#### Evidence Requirements
- Playtesting session reports (manual procedure execution logs)
- Balance adjustment diff with explanation
- Gameplay smoke test validation output
- Bug list with repo-owned/environment-blocked/blocked classification

---

### Phase 7.3: Audio & Sound Design
**Agent Owner:** Creative Director (or designated audio specialist)  
**Secondary:** Gameplay Engineer, QA Lead  
**Duration Target:** 2-3 weeks (8-12 hours)

#### Objective
Implement audio feedback and sound design for Crash Site gameplay to enhance immersion while respecting the offline-first constraint and license requirements. Move from silence/placeholder sounds to production audio where licensed assets are available.

#### Scope
- **In Scope:**
  - Footstep sounds (walking, running, jumping)
  - Player attack/swing audio feedback
  - Enemy approach and attack sounds
  - Resource pickup audio
  - Crafting completion audio
  - Damage/hit feedback sounds
  - Enemy death audio
  - Beacon activation audio
  - Victory and defeat audio stings
  - Ambient environmental sounds (optional: wind, distant ambient)
  - UI interaction sounds (menu, button clicks)
  - Licensed audio sources (Kenney, freesound.org with CC, etc.)

- **Out of Scope (Forbidden):**
  - Full orchestral score or complex composition (vision feature)
  - Voice acting or dialogue (vision feature)
  - Procedural audio generation
  - Network streaming audio
  - Audio that requires internet connection

#### Audio Asset Policy
- All audio must have documented provenance: source URL, license, author, date, file hash
- Licensing must be free (CC0, CC-BY, MIT equivalent) or legally cleared for commercial use
- Third-party audio goes in `assets/ThirdParty/` with manifest entry
- Custom audio goes in `assets/Production/Audio/` with recipe documentation

#### Success Criteria
- [ ] Audio implementation complete in at least 8 gameplay categories (footsteps, attack, pickup, crafting, damage, death, beacon, victory/defeat)
- [ ] All audio assets have provenance documentation in `THIRD_PARTY_ASSETS.md`
- [ ] No licensing disputes or unknown-source audio
- [ ] Gameplay smoke test confirms no audio crashes or performance regressions
- [ ] Audio level mixing is reasonable (not overwhelming, not inaudible)
- [ ] Works offline (no streaming, no network calls)

#### Audio Integration Checklist
- [ ] Footstep SFX audible during player movement
- [ ] Attack audio triggers on mechanical arm swing
- [ ] Resource pickup has audio feedback
- [ ] Crafting completion has distinct audio
- [ ] Enemy approach/detection has audio warning
- [ ] Combat damage audio is distinct from movement
- [ ] Death audio plays on Scout defeat
- [ ] Beacon activation has audio
- [ ] Victory/defeat stings are distinct and satisfying
- [ ] Menu navigation has optional click feedback
- [ ] No audio stuttering or sync issues with animation

#### Dependencies
- Requires Phase 5 completion: Windows export working
- Asset Forge or licensed audio source identified

#### Evidence Requirements
- Audio asset files imported into Godot
- Provenance entries in `THIRD_PARTY_ASSETS.md` with URLs and hashes
- Gameplay smoke test with audio validation
- QA sign-off on audio mixing and no regressions

---

### Phase 7.4: Polish & Optimization
**Agent Owner:** Technical Director  
**Secondary:** Gameplay Engineer, QA Lead, Build Release Engineer  
**Duration Target:** 2-3 weeks (8-12 hours)

#### Objective
Refine visual polish, gameplay responsiveness, performance, and build quality to achieve a polished MVP release candidate. Address frame rate, load times, visual glitches, and gameplay feel without expanding scope.

#### Scope
- **In Scope (Polish & Optimization):**
  - Rendering performance: target 60 FPS on target hardware, min 30 FPS stable
  - Load time optimization
  - Memory usage profiling and reduction
  - Visual glitch fixes (clipping, z-fighting, texture artifacts)
  - UI responsiveness and clarity
  - Animation and transition smoothing
  - Input latency reduction
  - Shader optimization
  - Physics and collision polish
  - Save/load performance
  - Build size optimization

- **Out of Scope (Forbidden):**
  - New visual effects beyond existing scope
  - New shaders or materials not needed for bugfixes
  - LOD (Level of Detail) systems for multiple maps
  - Advanced graphics features (reflection probes, volumetric fog, etc.)

#### Performance Targets
| Metric | Target | Minimum |
|--------|--------|---------|
| Frame Rate (Crash Site) | 60 FPS | 30 FPS stable |
| Load Time | < 5 seconds | < 10 seconds |
| Build Size (Windows) | < 500 MB | < 1 GB |
| RAM Usage (Peak) | < 2 GB | < 4 GB |

#### Polish Checklist
- [ ] No visual glitches in Crash Site playthrough
- [ ] UI buttons and menus respond immediately (no lag)
- [ ] Animations are smooth (no stuttering)
- [ ] Transitions between scenes are fast
- [ ] Save/load completes without freezing
- [ ] Performance is stable during 30-minute play session
- [ ] No memory leaks detected in profiler
- [ ] Build size is reasonable for distribution

#### Dependencies
- Requires Phase 6 completion and Phase 7.1-7.3 content integration

#### Evidence Requirements
- Performance profiler output (CPU, GPU, memory usage)
- Frame rate capture showing 60 FPS average on target hardware
- Before/after optimization metrics
- QA validation: no regressions in gameplay or visuals

---

### Phase 7.5: Platform Testing & Expansion
**Agent Owner:** Build Release Engineer  
**Secondary:** Technical Director, QA Lead  
**Duration Target:** 1-2 weeks (4-8 hours)

#### Objective
Validate Crash Site MVP on Windows across machine configurations and expand offline export support if feasible. Ensure release candidate is robust and playable on diverse Windows PCs.

#### Scope
- **In Scope:**
  - Windows 10/11 compatibility validation on multiple test machines
  - Input device testing (mouse, keyboard, gamepad if supported)
  - Display resolution testing (1080p, 1440p, 4K)
  - GPU driver compatibility (NVIDIA, AMD, Intel integrated)
  - DPI and scaling validation
  - Keyboard layout testing (QWERTY, AZERTY, etc.)
  - Performance on minimum spec and high-end machines
  - Install/uninstall process validation
  - Registry/permission requirements documentation

- **Out of Scope (Forbidden):**
  - Linux or macOS support (not guaranteed by README)
  - Console ports (PS5, Xbox, Switch)
  - Mobile/WebGL targets
  - Cloud gaming or streaming support
  - Mod support or workshop integration

#### Platform Configuration Matrix
| Configuration | Test Status | Notes |
|---------------|-------------|-------|
| Windows 10 (NVIDIA) | Target | Standard test machine |
| Windows 11 (AMD) | Target | Modern GPU variant |
| Windows 11 (Intel Integrated) | Target | Integrated GPU variant |
| Ultrawide Display (3440x1440) | Validation | UX compatibility |
| 4K Display (3840x2160) | Validation | Performance headroom |

#### Platform Testing Checklist
- [ ] Game launches without errors on Windows 10
- [ ] Game launches without errors on Windows 11
- [ ] NVIDIA GPU drivers (recent 3-gen) supported
- [ ] AMD GPU drivers (recent 3-gen) supported
- [ ] Intel integrated graphics supported (iGPU)
- [ ] Resolution scaling works (no UI breakage)
- [ ] Keyboard and mouse input responsive
- [ ] Save/load works across platform variants
- [ ] No permission/admin requirement for gameplay
- [ ] Clean uninstall removes all local files
- [ ] Performance acceptable on minimum spec machine

#### Dependencies
- Requires Phase 7.1-7.4 completion (composition, balance, audio, polish)
- Requires Phase 6 completion (export functional)

#### Evidence Requirements
- Platform test matrix with pass/fail results
- Tested Windows versions and GPU drivers documented
- Performance metrics on minimum spec hardware
- Known issues or limitations list (if any)
- Release readiness verdict

---

## Phase 7 Stage Gates

### Pre-Phase-7 Gate: Phase 6 Completion
**Gate Name:** Demo Readiness  
**Owner:** Producer  

**Blocking Conditions:**
- Stage A visuals not approved
- Windows export fails
- Gameplay loop has unresolved repo-owned blockers
- Release readiness verdict is NOT_GO

**Unblock Criteria:**
- Visual Reviewer approval of Stage A integration
- Successful Windows export of MVP
- All Phase 6 scope items completed
- Producer sign-off on release readiness

---

### Phase 7.1 Gate: Composition Guide Approval
**Gate Name:** Art Direction Lock  
**Owner:** Visual Reviewer  

**Blocking Conditions:**
- Composition guide references non-existent assets
- Visual examples not opened and diagnosed
- Guide contradicts Art Taste Pack
- Art Director self-approved (requires independent review)

**Pass Criteria:**
- Document complete with focal point, route, silhouette guidance
- Visual examples opened and analyzed
- Visual Reviewer provides independent approval verdict
- No scene or asset modifications included

---

### Phase 7.2 Gate: Balance Validation
**Gate Name:** Gameplay Balance Lock  
**Owner:** QA Lead  

**Blocking Conditions:**
- Less than 5 complete playthroughs documented
- Sequence-breaking exploits discovered
- Combat frustratingly unfair or trivial
- Balance changes exceed configuration scope (code rewrites)

**Pass Criteria:**
- Playtesting reports show consistent 10-30 minute sessions
- Balance feels fair across all tested runs
- All repo-owned bugs tracked or fixed
- Gameplay smoke test passes

---

### Phase 7.3 Gate: Audio Integration
**Gate Name:** Audio Implementation Complete  
**Owner:** QA Lead  

**Blocking Conditions:**
- Audio sources lack provenance (unknown licenses)
- Audio causes performance regression
- Audio assets not in manifest
- Audio stuttering or sync issues detected

**Pass Criteria:**
- 8+ audio categories implemented
- All assets have provenance in THIRD_PARTY_ASSETS.md
- Smoke test shows no audio crashes
- Mixing is balanced and not overwhelming

---

### Phase 7.4 Gate: Performance Targets Met
**Gate Name:** Polish & Optimization Complete  
**Owner:** Technical Director  

**Blocking Conditions:**
- Frame rate below 30 FPS on target hardware
- Build size exceeds 1 GB
- Memory leaks detected in profiler
- Visual glitches remain

**Pass Criteria:**
- 60 FPS average on target machine
- < 500 MB build size (or documented reason)
- Load time < 5 seconds
- No visual glitches on 30-minute playthrough

---

### Phase 7.5 Gate: Platform Validation Complete
**Gate Name:** Windows Compatibility Certified  
**Owner:** Build Release Engineer  

**Blocking Conditions:**
- Crash on Windows 10
- Crash on Windows 11
- Performance inadequate on minimum spec
- Unrecognized GPU driver issues

**Pass Criteria:**
- Tested on Windows 10 and 11
- Tested on NVIDIA, AMD, Intel GPUs
- Performance acceptable on target specs
- Clean install/uninstall works
- Known issues documented (if any)

---

## Phase 7 Summary & Integration

### Timeline
| Phase | Duration | Start Condition | End Condition |
|-------|----------|-----------------|---------------|
| 7.1 | 1-2 weeks | Phase 6 complete | Composition guide approved |
| 7.2 | 2-3 weeks | Phase 6 complete | Balance validated |
| 7.3 | 2-3 weeks | Phase 6 complete (parallel) | Audio integrated |
| 7.4 | 2-3 weeks | Phase 7.1-7.3 integrated | Performance targets met |
| 7.5 | 1-2 weeks | Phase 7.4 complete | Platform validation pass |

**Total Phase 7 Estimate:** 6-10 weeks (24-40 hours)

### Success Definition
Phase 7 is complete when:
- [ ] Composition guide documents visual direction with opened PNG examples
- [ ] Gameplay balance is validated through 5+ complete playthroughs
- [ ] Audio is implemented in 8+ categories with provenance
- [ ] Performance meets 60 FPS target (30 FPS minimum)
- [ ] Windows 10 and 11 tested on multiple GPU configurations
- [ ] All gates pass with approved verdicts
- [ ] No scope expansion beyond Phase 7 objectives
- [ ] Release readiness is formally assessed

### Post-Phase-7 Considerations
After Phase 7 completion, future work may address:
- Multiple regions/maps (vision feature, requires scope change)
- Additional enemy types (vision feature)
- Advanced equipment/upgrades (vision feature)
- Campaign continuation beyond Crash Site
- Ports to other platforms (requires explicit scope authorization)

All post-Phase-7 work requires explicit human authorization and README updates before agent work begins.

---

## Authority & Validation

**Source of Truth:** README.md § 5 (MVP definition), § 6 (forbidden scope)  
**Governance:** AGENTS.md § 2 (product boundary), § 3 (agent workflow)  
**Stage Gates:** studio/memory/production_stage_gates.md  
**Definition of Done:** docs/production/definition-of-done.md  

**Validation Commands:**
```bash
python3 tools/validate_agent_studio.py
git diff --check
```

**Evidence Gating:**
- Phase 7 work must not begin until Phase 6 passes all gates
- Each Phase 7 subphase has explicit blocking conditions and pass criteria
- All verdicts use approved vocabulary: PASS, NOT_GO, INTENTIONAL_GATE, HUMAN_BLOCKED, ENVIRONMENT_BLOCKED
- Vague verdicts ("looks good", "should work", "improved") are forbidden

---

**Document Status:** PLANNING (governance/documentation work)  
**Next Action:** Human review and approval before Phase 7 agent work begins
