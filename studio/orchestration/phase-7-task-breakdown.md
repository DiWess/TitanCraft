# Phase 7 Detailed Task Breakdown
## Executable Work Items for Agent Studio

**Document:** Operational task list  
**Date:** 2026-07-05  
**Generated from:** phase-7-agent-coordination.md + phase-7-planning.md

---

## Phase 7.1: Art Director Composition Guide (Weeks 1-2)

### Task 7.1.1: Composition Guide Foundation [Art Director]
**Status:** Ready to execute  
**Effort:** 2-4 hours

**Checklist:**
- [ ] Create `docs/art/composition-guide.md` skeleton
- [ ] Document Stage A Crash Site focal points and visual hierarchy
- [ ] Identify primary sightlines and player navigation routes
- [ ] Analyze approved Stage A asset silhouettes and scale relationships
- [ ] Document material and lighting coherence principles
- [ ] Reference Art Taste Pack visual identity alignment

**Deliverables:**
- `docs/art/composition-guide.md` (draft)
- List of approved Stage A assets to analyze

**Definition of Done:**
- Document skeleton complete
- 3-5 composition principles documented
- References to Stage A assets made

---

### Task 7.1.2: Visual Examples & Diagnosis [Art Director]
**Status:** Ready to execute  
**Effort:** 2-4 hours  
**Prerequisites:** Task 7.1.1 complete

**Checklist:**
- [ ] Capture 5-10 opened PNG screenshots of Crash Site gameplay
- [ ] Annotate each screenshot with:
  - Primary focal point
  - Route readability (where player should go)
  - Silhouette clarity (how assets read)
  - Scale relationships
  - Material coherence
- [ ] Save annotated images to `artifacts/review/composition/` with diagnosis notes
- [ ] Link screenshots into composition guide

**Deliverables:**
- 5-10 annotated PNG screenshots
- Diagnosis notes for each screenshot
- Screenshots linked in composition guide

**Definition of Done:**
- At least 5 screenshots with visual diagnosis
- Each screenshot has focal point identified
- Route readability explained

---

### Task 7.1.3: Composition Guide Review [Visual Reviewer]
**Status:** Ready to execute (after 7.1.1-7.1.2)  
**Effort:** 1-2 hours  
**Prerequisites:** Tasks 7.1.1-7.1.2 complete

**Checklist:**
- [ ] Open and review composition guide document
- [ ] Verify visual examples are opened PNGs with diagnosis
- [ ] Confirm guide aligns with Art Taste Pack principles
- [ ] Check for any scene or asset modifications (should be zero)
- [ ] Approve or provide feedback
- [ ] Publish Visual Reviewer verdict (PASS or NOT_GO)

**Deliverables:**
- Visual Reviewer approval verdict
- Feedback document (if NOT_GO)

**Definition of Done:**
- Verdict published with concrete reasoning
- If PASS: composition guide locked for Phase 7.4
- If NOT_GO: specific blockers listed for Art Director

---

### Task 7.1.4: Composition Guide Integration [Art Director]
**Status:** Ready to execute (after 7.1.3)  
**Effort:** 1 hour  
**Prerequisites:** Visual Reviewer approval

**Checklist:**
- [ ] Finalize composition guide based on Visual Reviewer feedback
- [ ] Commit to `docs/art/composition-guide.md`
- [ ] Ensure no scene or asset changes in commit
- [ ] Mark Phase 7.1 complete

**Deliverables:**
- Final `docs/art/composition-guide.md`
- Clean git commit

**Definition of Done:**
- Document merged and committed
- No unintended file changes in commit
- Phase 7.1 gate satisfied

---

## Phase 7.2: Gameplay Balance Playtesting (Weeks 3-4, Parallel)

### Task 7.2.1: Playtesting Infrastructure Setup [Gameplay Engineer + Tools Engineer]
**Status:** Ready to execute  
**Effort:** 2-4 hours

**Checklist:**
- [ ] Document current gameplay parameters:
  - Player health, damage, attack cooldown
  - Galaxabrain Scout health, damage, detection range
  - Resource spawn locations and quantities
  - Crafting costs
- [ ] Create playtesting checklist template in `docs/testing/playtesting-template.md`
- [ ] Set up metrics collection: session duration, resource collection rate, combat effectiveness
- [ ] Create playtesting log template (CSV or markdown)
- [ ] Establish baseline performance metrics

**Deliverables:**
- Gameplay parameter documentation
- Playtesting checklist template
- Metrics collection script/spreadsheet

**Definition of Done:**
- All current parameters documented
- Playtesting template ready for QA Lead
- Metrics can be recorded for each session

---

### Task 7.2.2: Playtesting Session Execution [Gameplay Engineer + QA Lead]
**Status:** Ready to execute (after 7.2.1)  
**Effort:** 8-12 hours (across 5+ sessions)

**Checklist:**
- [ ] Execute first complete playthrough with metrics
- [ ] Execute second complete playthrough with metrics
- [ ] Execute third complete playthrough with metrics
- [ ] Execute fourth complete playthrough with metrics
- [ ] Execute fifth complete playthrough with metrics
- [ ] Document observations: difficulty, pacing, fairness, bugs
- [ ] Note any sequence-breaking exploits or unfair mechanics
- [ ] Collect session duration, resource usage, combat effectiveness

**Deliverables:**
- 5+ playtesting session logs
- Aggregate metrics (average session time, fairness assessment)
- Bug list (repo-owned vs. environment-blocked)

**Definition of Done:**
- At least 5 complete sessions documented
- Session durations 10-30 minutes
- Playtesting observations recorded
- Fair difficulty assessed across all sessions

---

### Task 7.2.3: Balance Adjustment & Validation [Gameplay Engineer]
**Status:** Ready to execute (after 7.2.2)  
**Effort:** 4-6 hours

**Checklist:**
- [ ] Analyze playtesting results
- [ ] Identify balance issues (too easy, too hard, tedious, unfair)
- [ ] Propose configuration-only adjustments (no code rewrites)
- [ ] Apply changes to balance configuration (YAML, JSON, etc.)
- [ ] Execute smoke test after each change
- [ ] Validate with 1-2 additional playthroughs
- [ ] Document all changes with rationale
- [ ] Confirm no scope expansion (forbidden features not added)

**Deliverables:**
- Balance adjustment diff with explanation
- Updated gameplay configuration files
- Post-adjustment playtesting evidence

**Definition of Done:**
- All balance changes are configuration-only
- Smoke test passes after changes
- Difficulty feels fair in follow-up playthroughs
- No forbidden MVP features added

---

### Task 7.2.4: QA Validation & Gate Approval [QA Lead]
**Status:** Ready to execute (after 7.2.3)  
**Effort:** 2-3 hours

**Checklist:**
- [ ] Review all playtesting logs
- [ ] Verify 5+ sessions documented
- [ ] Assess fairness across sessions
- [ ] Confirm no sequence-breaking exploits
- [ ] Review balance changes for scope expansion
- [ ] Approve or request re-tuning
- [ ] Publish QA verdict: PASS or NOT_GO
- [ ] Gate: Balance Validation Lock

**Deliverables:**
- QA approval verdict
- Playtesting evidence summary

**Definition of Done:**
- QA verdict published with reasoning
- If PASS: Phase 7.2 gate satisfied, can proceed to 7.4
- If NOT_GO: specific blockers for Gameplay Engineer

---

## Phase 7.3: Audio & Sound Design (Weeks 3-4, Parallel)

### Task 7.3.1: Audio Source Research & Procurement [Creative Director]
**Status:** Ready to execute  
**Effort:** 2-4 hours

**Checklist:**
- [ ] Identify audio sources (Kenney, freesound.org, etc.)
- [ ] Verify CC0 or commercial-use licenses
- [ ] Download audio files for 8+ categories:
  1. Footsteps
  2. Player attack/swing
  3. Enemy approach and attack
  4. Resource pickup
  5. Crafting completion
  6. Damage/hit feedback
  7. Enemy death
  8. Beacon activation
  9. Victory/defeat stings (optional)
- [ ] Compute SHA-256 hashes for each audio file
- [ ] Document source URL, creator, license for each file
- [ ] Verify files work in Godot (WAV, OGG, MP3 supported)

**Deliverables:**
- Audio file collection (8+ categories)
- Provenance spreadsheet with URL, license, hash, creator, date
- List of audio files ready for import

**Definition of Done:**
- At least 8 audio files sourced
- All have documented provenance
- All have clear commercial licenses
- All files are in Godot-compatible format

---

### Task 7.3.2: Audio Provenance Documentation [Creative Director + Asset Librarian]
**Status:** Ready to execute (after 7.3.1)  
**Effort:** 1-2 hours

**Checklist:**
- [ ] Update `THIRD_PARTY_ASSETS.md` with audio section
- [ ] For each audio file, record:
  - Source URL
  - Creator/author
  - License (CC0, CC-BY, etc.)
  - Download date
  - SHA-256 hash
  - Local file path
  - Purpose (footsteps, attack, etc.)
- [ ] Verify license text for each source
- [ ] Store license files if provided by source
- [ ] Create `assets/ThirdParty/Audio/` directory structure

**Deliverables:**
- Updated `THIRD_PARTY_ASSETS.md` with audio entries
- License text files (if provided)
- Organized audio directory structure

**Definition of Done:**
- All audio has provenance entry
- Every entry has URL, license, hash
- Asset Librarian validates entries

---

### Task 7.3.3: Audio Integration into Godot [Gameplay Engineer + Tools Engineer]
**Status:** Ready to execute (after 7.3.2)  
**Effort:** 4-6 hours

**Checklist:**
- [ ] Import audio files into Godot project
- [ ] Create audio bus structure (Master, SFX, Music, UI)
- [ ] Assign audio files to relevant gameplay events:
  - Footstep sound → player movement
  - Attack sound → mechanical arm swing
  - Pickup sound → resource collection
  - Crafting sound → workbench use
  - Damage sound → player/enemy damage
  - Death sound → enemy defeat
  - Beacon sound → beacon activation
  - Victory/defeat stings → end screen
- [ ] Set audio levels (volume mixing)
- [ ] Test audio playback for each category
- [ ] Verify no audio stuttering or sync issues
- [ ] Profile audio memory and CPU usage

**Deliverables:**
- Integrated audio files in Godot project
- Audio bus configuration
- Audio event assignments (code or visual script)
- Audio mixing levels documented

**Definition of Done:**
- All 8+ audio categories have integrated audio
- Audio triggers correctly on gameplay events
- Audio levels are balanced (not overwhelming)
- No audio stuttering or crashes

---

### Task 7.3.4: Audio Gameplay Smoke Test [QA Lead + Gameplay Engineer]
**Status:** Ready to execute (after 7.3.3)  
**Effort:** 2-3 hours

**Checklist:**
- [ ] Execute gameplay smoke test with audio enabled
- [ ] Walk through Crash Site and verify all sounds trigger:
  - Footsteps when moving
  - Attack sound when swinging mechanical arm
  - Pickup sound when collecting resources
  - Crafting sound when building
  - Damage sound when taking/dealing damage
  - Enemy death sound when defeating Scout
  - Beacon sound when activating
  - Victory sting when completing objective
- [ ] Monitor for audio crashes or glitches
- [ ] Check audio doesn't cause performance regression
- [ ] Record playthrough with audio (30-minute session)
- [ ] Publish audio validation evidence

**Deliverables:**
- Gameplay smoke test log with audio validation
- Performance metrics (CPU/memory with audio)
- Recording evidence (if possible)

**Definition of Done:**
- All audio categories trigger correctly
- No audio-related crashes
- No performance regression from audio
- Smoke test passes with video/log evidence

---

### Task 7.3.5: QA Validation & Audio Gate Approval [QA Lead]
**Status:** Ready to execute (after 7.3.4)  
**Effort:** 1-2 hours

**Checklist:**
- [ ] Review audio integration completeness
- [ ] Verify all 8+ categories have audio
- [ ] Check THIRD_PARTY_ASSETS.md entries for completeness
- [ ] Verify audio provenance (source, license, hash)
- [ ] Review smoke test results (no crashes, no regression)
- [ ] Assess audio mixing (not overwhelming, not inaudible)
- [ ] Approve or request adjustments
- [ ] Publish QA verdict: PASS or NOT_GO
- [ ] Gate: Audio Integration Lock

**Deliverables:**
- QA approval verdict
- Audio validation summary

**Definition of Done:**
- QA verdict published
- If PASS: Phase 7.3 gate satisfied, can proceed to 7.4
- If NOT_GO: specific blockers for Creative Director

---

## Phase 7.4: Polish & Optimization (Weeks 5-6, After 7.1-7.3)

### Task 7.4.1: Performance Baseline & Profiling [Technical Director]
**Status:** Ready to execute (after Phase 7.2-7.3 gates pass)  
**Effort:** 3-4 hours

**Checklist:**
- [ ] Profile current rendering (CPU time, GPU time, draw calls)
- [ ] Profile memory usage (heap, textures, audio)
- [ ] Measure current frame rate on target hardware (30 min session)
- [ ] Measure load time (cold start from disk)
- [ ] Measure build size
- [ ] Document baseline metrics
- [ ] Identify bottlenecks (CPU-bound, GPU-bound, memory-limited?)
- [ ] Set optimization targets based on gaps from 60 FPS, < 500 MB, < 5 sec load

**Deliverables:**
- Baseline performance report (CPU, GPU, memory, FPS, load time, build size)
- Optimization priority list
- Target metrics for each subsystem

**Definition of Done:**
- Baseline metrics documented
- Bottlenecks identified
- Optimization targets clear

---

### Task 7.4.2: Visual Glitch Fixes [Technical Director + Visual Reviewer]
**Status:** Ready to execute (after 7.4.1)  
**Effort:** 2-4 hours

**Checklist:**
- [ ] Execute 30-minute full Crash Site session
- [ ] Document any visual glitches (clipping, z-fighting, texture artifacts)
- [ ] Prioritize by visibility and impact
- [ ] Fix clipping issues in collision geometry
- [ ] Fix z-fighting with material/depth offset adjustments
- [ ] Address texture artifacts (mipmapping, UV issues)
- [ ] Verify fixes with gameplay session
- [ ] Visual Reviewer approves glitch fixes

**Deliverables:**
- Visual glitch fix log
- Code commits for fixes
- Post-fix gameplay session evidence

**Definition of Done:**
- No visual glitches observable in 30-min session
- All fixes validated
- Visual Reviewer approval obtained

---

### Task 7.4.3: Rendering Optimization [Technical Director]
**Status:** Ready to execute (after 7.4.1)  
**Effort:** 4-6 hours

**Checklist:**
- [ ] Optimize draw call count (batching, object pooling)
- [ ] Optimize shader complexity
- [ ] Enable LOD (if applicable) or reduce geometry
- [ ] Optimize lighting (bake shadows where possible)
- [ ] Reduce texture resolution where not visible
- [ ] Profile after each change
- [ ] Target: 60 FPS on target hardware
- [ ] Document all rendering changes

**Deliverables:**
- Rendering optimization commits
- Performance metrics after each change
- Final FPS measurement (target 60, minimum 30)

**Definition of Done:**
- 60 FPS average on target hardware
- At minimum 30 FPS stable
- Frame rate improvement documented

---

### Task 7.4.4: Memory & Load Time Optimization [Technical Director]
**Status:** Ready to execute (parallel with 7.4.3)  
**Effort:** 3-4 hours

**Checklist:**
- [ ] Profile memory allocations (peak usage)
- [ ] Identify large assets (textures, meshes, audio)
- [ ] Optimize asset sizes (compress where possible)
- [ ] Implement asset streaming if needed
- [ ] Reduce load time (target < 5 sec)
- [ ] Profile load time breakdown (disk I/O, Godot import, scene instantiation)
- [ ] Optimize bottleneck (usually disk I/O or scene setup)
- [ ] Verify memory stays < 2 GB peak

**Deliverables:**
- Memory optimization commits
- Load time optimization commits
- Before/after memory and load time metrics

**Definition of Done:**
- Load time < 5 seconds (< 10 acceptable)
- Peak memory < 2 GB (< 4 GB acceptable)
- Memory improvements documented

---

### Task 7.4.5: Build Size Optimization [Build Release Engineer]
**Status:** Ready to execute (parallel with 7.4.3-7.4.4)  
**Effort:** 2-3 hours

**Checklist:**
- [ ] Measure current build size
- [ ] Remove unused assets (if any)
- [ ] Compress textures (if not already)
- [ ] Optimize audio compression
- [ ] Remove debug symbols (for release build)
- [ ] Document build configuration
- [ ] Target: < 500 MB
- [ ] Document reasons if unable to meet target

**Deliverables:**
- Build optimization commits
- Final build size measurement
- Build size justification

**Definition of Done:**
- Build size < 500 MB (or documented reason)
- Build optimization documented

---

### Task 7.4.6: Technical Director Validation & Gate Approval [Technical Director]
**Status:** Ready to execute (after 7.4.1-7.4.5)  
**Effort:** 2-3 hours

**Checklist:**
- [ ] Aggregate all performance metrics
- [ ] Verify 60 FPS target met (or document reason)
- [ ] Verify load time < 5 sec (or document reason)
- [ ] Verify build size < 500 MB (or document reason)
- [ ] Execute final 30-minute stability test
- [ ] Verify no visual glitches
- [ ] Verify no regressions from optimization
- [ ] Publish Technical Director verdict: PASS or NOT_GO
- [ ] Gate: Polish & Optimization Complete

**Deliverables:**
- Final performance report
- Technical Director approval verdict

**Definition of Done:**
- All performance metrics documented
- Technical Director verdict published
- If PASS: Phase 7.4 gate satisfied, can proceed to 7.5
- If NOT_GO: specific blockers identified

---

## Phase 7.5: Platform Testing & Expansion (Weeks 7-8, After 7.4)

### Task 7.5.1: Platform Test Matrix Setup [Build Release Engineer]
**Status:** Ready to execute (after Phase 7.4 gate passes)  
**Effort:** 2-3 hours

**Checklist:**
- [ ] Document target Windows versions (10, 11)
- [ ] Document target GPU vendors (NVIDIA, AMD, Intel)
- [ ] Document target resolutions (1080p, 1440p, 4K, ultrawide)
- [ ] Create platform test matrix spreadsheet
- [ ] Identify test machines/configurations
- [ ] Document driver versions to test
- [ ] Create test execution checklist for each configuration

**Deliverables:**
- Platform test matrix (spreadsheet or markdown table)
- Test execution checklist
- Test environment documentation

**Definition of Done:**
- All test configurations documented
- Test matrix clear and actionable
- Execution checklist ready for QA

---

### Task 7.5.2: Windows 10 Testing [QA Lead + Build Release Engineer]
**Status:** Ready to execute (after 7.5.1)  
**Effort:** 3-4 hours

**Checklist:**
- [ ] Test on Windows 10 + NVIDIA GPU
  - [ ] Launch and load Crash Site
  - [ ] Play full 30-minute session
  - [ ] Test resolution scaling (1080p, 1440p, 4K)
  - [ ] Test alt+tab and window focus recovery
  - [ ] Measure FPS
- [ ] Test on Windows 10 + AMD GPU
  - [ ] Same test suite
- [ ] Test on Windows 10 + Intel Integrated GPU
  - [ ] Same test suite
- [ ] Document all pass/fail results
- [ ] Document any crashes or issues
- [ ] Document FPS on each configuration

**Deliverables:**
- Windows 10 test results (matrix filled in)
- Issue log (if any problems found)
- FPS measurements per configuration

**Definition of Done:**
- Windows 10 testing complete on 3 GPU variants
- No crashes on any configuration
- FPS documented for each

---

### Task 7.5.3: Windows 11 Testing [QA Lead + Build Release Engineer]
**Status:** Ready to execute (after 7.5.2)  
**Effort:** 3-4 hours

**Checklist:**
- [ ] Repeat Windows 10 testing on Windows 11
- [ ] Test on Windows 11 + NVIDIA GPU
- [ ] Test on Windows 11 + AMD GPU
- [ ] Test on Windows 11 + Intel Integrated GPU
- [ ] Document all pass/fail results
- [ ] Test Windows 11 specific features (DirectStorage if applicable, DPI scaling, etc.)
- [ ] Document any OS-specific issues

**Deliverables:**
- Windows 11 test results (matrix filled in)
- Issue log (if any OS-specific problems)
- FPS measurements per configuration

**Definition of Done:**
- Windows 11 testing complete on 3 GPU variants
- No crashes on Windows 11
- Windows 11 specific issues documented

---

### Task 7.5.4: Resolution & Input Testing [QA Lead]
**Status:** Ready to execute (parallel with 7.5.2-7.5.3)  
**Effort:** 2-3 hours

**Checklist:**
- [ ] Test resolution scaling (1080p, 1440p, 4K, ultrawide)
- [ ] Verify UI doesn't break at any resolution
- [ ] Test mouse and keyboard responsiveness at each resolution
- [ ] Test gamepad input (if supported)
- [ ] Test DPI scaling (Windows scaling settings)
- [ ] Document any resolution-specific issues
- [ ] Verify game is playable at all tested resolutions

**Deliverables:**
- Resolution compatibility log
- Input device compatibility log
- Any scaling/UI issues documented

**Definition of Done:**
- All tested resolutions work
- No UI breakage
- Input responsive at all resolutions

---

### Task 7.5.5: Install/Uninstall & Clean Testing [Build Release Engineer]
**Status:** Ready to execute (parallel with resolution testing)  
**Effort:** 1-2 hours

**Checklist:**
- [ ] Clean Windows 10 machine (or VM)
- [ ] Test fresh install from build artifact
- [ ] Verify game launches after clean install
- [ ] Execute full Crash Site session
- [ ] Test save/load on clean machine
- [ ] Uninstall game
- [ ] Verify clean uninstall (no leftover files in Program Files or AppData)
- [ ] Document any installation issues

**Deliverables:**
- Install/uninstall test log
- Any permission or prerequisite issues documented
- Clean uninstall verification

**Definition of Done:**
- Fresh install works
- Game is playable on clean machine
- Uninstall is clean

---

### Task 7.5.6: Known Issues Documentation [Build Release Engineer + QA Lead]
**Status:** Ready to execute (after 7.5.2-7.5.5)  
**Effort:** 1-2 hours

**Checklist:**
- [ ] Compile all issues found during platform testing
- [ ] Classify each issue:
  - Critical (crash, unplayable)
  - Major (significant impact)
  - Minor (cosmetic, rare edge case)
- [ ] Document workarounds (if any)
- [ ] Document which configurations are affected
- [ ] If critical issues found: escalate to Technical Director for fix
- [ ] If only minor issues: document and proceed to release

**Deliverables:**
- Known Issues list (formatted for user documentation)
- Platform compatibility summary

**Definition of Done:**
- All issues documented
- Classification clear
- Workarounds documented

---

### Task 7.5.7: Build Release Engineer Validation & Gate Approval [Build Release Engineer]
**Status:** Ready to execute (after 7.5.1-7.5.6)  
**Effort:** 1-2 hours

**Checklist:**
- [ ] Review all platform test results
- [ ] Verify Windows 10 and 11 both pass
- [ ] Verify testing on NVIDIA, AMD, Intel GPUs
- [ ] Verify no critical crashes
- [ ] Verify performance acceptable on minimum-spec hardware
- [ ] Review known issues list
- [ ] Publish Build Release Engineer verdict: PASS or NOT_GO
- [ ] Gate: Windows Compatibility Certified

**Deliverables:**
- Build Release Engineer approval verdict
- Final platform compatibility report
- Release candidate status

**Definition of Done:**
- All testing complete
- Build Release Engineer verdict published
- If PASS: Platform gate satisfied, MVP ready for release
- If NOT_GO: specific blockers for Technical Director

---

## Phase 7 Completion: Final Verdict [Producer]

### Task 7.FINAL: Phase 7 Completion & Release Readiness Verdict
**Status:** Ready to execute (after all Phase 7.1-7.5 gates pass)  
**Effort:** 1-2 hours

**Checklist:**
- [ ] Verify Phase 7.1 gate: PASS
- [ ] Verify Phase 7.2 gate: PASS
- [ ] Verify Phase 7.3 gate: PASS
- [ ] Verify Phase 7.4 gate: PASS
- [ ] Verify Phase 7.5 gate: PASS
- [ ] Verify timeline within 6-10 week estimate
- [ ] Verify no scope creep (forbidden features absent)
- [ ] Aggregate all evidence and verdicts
- [ ] Publish Phase 7 completion verdict
- [ ] Publish final MVP readiness assessment
- [ ] Document next steps (if release approved)

**Deliverables:**
- Phase 7 completion report
- Final MVP readiness verdict
- Release recommendation or blockers

**Definition of Done:**
- Phase 7 PASS or NOT_GO verdict published
- If PASS: MVP is ready for release (or marketing)
- If NOT_GO: specific blockers listed
- All evidence archived in artifacts/

---

## Task Execution Summary

### Quick Start Checklist (Week 1)

**Day 1-2: Planning & Setup**
- [ ] Producer confirms Phase 6 complete and authorizes Phase 7 start
- [ ] All agents review phase-7-agent-coordination.md and phase-7-planning.md
- [ ] Art Director begins Phase 7.1.1 (composition guide foundation)
- [ ] Gameplay Engineer begins Phase 7.2.1 (playtesting infrastructure)
- [ ] Tools Engineer sets up playtesting metrics harness

**Day 3-5: Parallel Phase 7.1 & 7.2 Ramp-Up**
- [ ] Art Director completes Phase 7.1.1-7.1.2 (draft + examples)
- [ ] Gameplay Engineer executes first playtesting session (Phase 7.2.2)
- [ ] Creative Director begins Phase 7.3.1 (audio source research)
- [ ] Visual Reviewer prepares for Phase 7.1 review

**Week 2: Gate Evaluation**
- [ ] Visual Reviewer completes Phase 7.1.3 (composition review)
- [ ] Gameplay Engineer executes sessions 2-3 (Phase 7.2.2)
- [ ] Creative Director completes Phase 7.3.1-7.3.2 (audio + provenance)
- [ ] Technical Director begins Phase 7.4.1 prep (performance baseline planning)

**Weeks 3-10: Remaining Phases**
- [ ] Each phase follows task sequence
- [ ] Parallel workstreams (7.2, 7.3) proceed independently
- [ ] Phase 7.4 begins after 7.1-7.3 gates pass
- [ ] Phase 7.5 begins after 7.4 gate passes
- [ ] Producer monitors gates and timeline

---

## Status Reporting Template

Each agent uses this format for weekly status:

```markdown
## [Agent Name] — Phase 7.X Status Report

**Week:** [Week #]  
**Phase:** 7.X [Phase Name]  
**Owner:** [Agent Name]  
**Status:** [In Progress / Complete / Blocked]

### Completed Tasks (This Week)
- [x] Task 7.X.Y: [Task Name]
- [x] Task 7.X.Z: [Task Name]

### Current Task
- [ ] Task 7.X.W: [Task Name] — [%] complete

### Blockers (if any)
- [Blocker name] (Owner: [Agent], ETA: [date])

### Next Steps
- [ ] Task 7.X.V: [Task Name] (ETA: [date])

### Evidence Collected
- [docs/path/filename]
- [artifacts/path/filename]
- [git commit hash]

### Gate Status
- [ ] Gate prerequisites met: YES / NO
- [ ] Gate evidence collected: YES / NO
- [ ] Ready for gate review: YES / NO
```

---

## Success Metrics per Phase

| Phase | Success Metric | Target |
|-------|---|---|
| 7.1 | Composition guide approved by Visual Reviewer | Week 2 |
| 7.2 | 5+ playthroughs completed, balance fair | Week 4 |
| 7.3 | 8+ audio categories with provenance | Week 4 |
| 7.4 | 60 FPS, < 500 MB, no visual glitches | Week 6 |
| 7.5 | Windows 10/11 + NVIDIA/AMD/Intel tested | Week 8 |
| Overall | All gates PASS, timeline ≤ 10 weeks | Week 10 |

---

**Status:** EXECUTABLE TASK BREAKDOWN (ready to assign to agents)  
**Next Action:** Producer authorizes Phase 7 start and assigns tasks to agents
