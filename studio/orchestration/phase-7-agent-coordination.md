# Phase 7 Multi-Agent Coordination Plan
## Full Studio Orchestra for Extended Production

**Document:** Agent Studio Orchestration Blueprint  
**Date:** 2026-07-05  
**Authority:** AGENTS.md § 3 (global agent behavior), Phase 7 planning document  
**Scope:** Coordinate 18 studio agents to execute Phase 7 (6-10 week sprint)

---

## Executive Summary

This document orchestrates all 18 TitanCraft studio agents into a coordinated pipeline to execute Phase 7: comprehensive gameplay polish, balance, audio design, optimization, and platform validation. The plan respects MVP scope boundaries, stage gates, and governance while leveraging agent specialization for parallel work streams.

**Key Principles:**
- One task per agent per phase; no simultaneous conflicts
- Evidence gates must pass before stage advancement
- All work within locked Crash Site MVP scope
- Parallel workstreams where dependencies allow
- Human approval required at major gates

---

## Pre-Phase-7 Critical Gate: Phase 6 Completion

### Current Blocker Status
- **Visual approval:** Stage A assets pending human/visual-reviewer verdict
- **Windows export:** Phase 6 prerequisite (status: TBD in current container)
- **Gameplay validation:** Phase 6 prerequisite (status: TBD)
- **Release readiness:** Producer gate (status: pending)

### Phase 6 Unblocking Path (Prerequisite to Phase 7)

**If Phase 6 is NOT yet complete:**

1. **Producer** → Verify Phase 6 gate status and publish blockers
2. **Visual Reviewer** → Generate Visual Artifact Factory review bundles (if Stage A assets exist)
3. **Build Release Engineer** → Validate Windows export pipeline
4. **Technical Director** → Audit gameplay loop for Phase 6 readiness
5. **Producer** → Publish Phase 6 verdict and Phase 7 start authorization

**Assumption for this plan:** Phase 6 is assumed COMPLETE and Phase 7 authorization is GRANTED. If not, execute Phase 6 Unblocking path first.

---

## Phase 7 Multi-Agent Orchestration Map

```
Phase 7 Timeline (6-10 weeks total)

Week 1-2:     Phase 7.1: Composition Guide
              [Art Director + Visual Reviewer + Creative Director]

Week 3-4:     Phase 7.2: Gameplay Balance (parallel start)
Week 3-4:     Phase 7.3: Audio & Sound Design (parallel start)
              [Gameplay Engineer + QA Lead] | [Creative Director + SFX specialist]

Week 5-6:     Phase 7.4: Polish & Optimization (gate: 7.1-7.3 complete)
              [Technical Director + Gameplay Engineer + Build Release Engineer]

Week 7-8:     Phase 7.5: Platform Testing (gate: 7.4 complete)
              [Build Release Engineer + Technical Director + QA Lead]

Week 9-10:    Phase 7 Closure & Release Readiness Verdict
              [Producer + All agents]
```

---

## Agent Roles & Responsibilities

### Lead Agents (Primary Ownership)

#### 1. **Art Director** (Phase 7.1: Composition Guide)
**Mission:** Document visual composition principles for approved Stage A Crash Site

**Responsibilities:**
- [ ] Read Phase 7.1 scope and success criteria
- [ ] Access approved Stage A visual assets
- [ ] Analyze focal points, routes, silhouettes, scale relationships
- [ ] Document composition principles in `docs/art/composition-guide.md`
- [ ] Provide opened PNG visual examples with diagnosis
- [ ] Submit for Visual Reviewer independent approval
- [ ] Integration: Final document reviewed and approved

**Success Criteria:**
- Composition guide complete with visual examples
- Visual Reviewer approval recorded
- No scene or asset modifications
- Document ready for future asset creation guidance

**Dependencies:**
- Requires Phase 6 completion: Stage A visuals integrated and approved

---

#### 2. **Gameplay Engineer** (Phase 7.2: Gameplay Balance)
**Mission:** Validate and refine Crash Site gameplay balance through playtesting

**Responsibilities:**
- [ ] Establish playtesting infrastructure (test harness, metrics collection)
- [ ] Document current gameplay parameters (resource costs, damage values, etc.)
- [ ] Execute 5+ complete playthrough sessions with QA Lead
- [ ] Identify balance issues and repo-owned bugs
- [ ] Implement balance adjustments (config changes, not code rewrites)
- [ ] Validate each change with smoke tests
- [ ] Document playtesting evidence and final balance configuration
- [ ] Provide gameplay smoke test validation for Phase 7.4 gate

**Success Criteria:**
- 5+ complete 10-30 minute playthrough sessions documented
- Balance feels fair and not frustrating
- All identified repo-owned bugs tracked or fixed
- Gameplay smoke test passes with video/log evidence
- Balance changes are configuration-only (no code rewrite)

**Testing Checklist:**
- [ ] Movement and camera responsiveness
- [ ] Resource collection difficulty (neither trivial nor tedious)
- [ ] Crafting cost fairness
- [ ] Combat difficulty (challenging but fair)
- [ ] Enemy AI stability (no stuck states)
- [ ] Victory/defeat flows work
- [ ] Session duration 10-30 minutes consistently
- [ ] No sequence-breaking exploits

**Parallel Work:** Can proceed while Phase 7.1 composition guide is in progress

---

#### 3. **Creative Director** (Phase 7.3: Audio & Sound Design)
**Mission:** Implement production audio feedback for Crash Site gameplay

**Responsibilities:**
- [ ] Audit current audio state (placeholder vs. missing)
- [ ] Identify licensed audio sources (Kenney, freesound.org, etc.)
- [ ] Download and verify audio assets with provenance
- [ ] Compute file hashes and record in `THIRD_PARTY_ASSETS.md`
- [ ] Integrate audio into Godot project (8+ categories):
  - Footsteps
  - Player attack/swing
  - Enemy approach and attack
  - Resource pickup
  - Crafting completion
  - Damage/hit feedback
  - Enemy death
  - Beacon activation
  - Victory/defeat stings
- [ ] Validate audio mixing and levels
- [ ] Perform gameplay smoke test with audio
- [ ] Document audio implementation evidence

**Success Criteria:**
- 8+ audio categories implemented with licensed sources
- All audio assets have provenance in THIRD_PARTY_ASSETS.md
- Audio mixing balanced (not overwhelming, not inaudible)
- Gameplay smoke test shows no crashes or performance regressions
- Works fully offline (no streaming, no network calls)

**Audio Categories Matrix:**
| Category | Source | Status |
|----------|--------|--------|
| Footsteps | Kenney SFX | TBD |
| Attack | Kenney SFX | TBD |
| Enemy Approach | Freesound CC-BY | TBD |
| Pickup | Kenney SFX | TBD |
| Crafting | Custom beep | TBD |
| Damage | Kenney SFX | TBD |
| Death | Kenney SFX | TBD |
| Beacon | Custom synth | TBD |
| Victory | Kenney Music | TBD |

**Parallel Work:** Can proceed simultaneously with Phase 7.2 balance testing

---

#### 4. **Technical Director** (Phase 7.4: Polish & Optimization)
**Mission:** Optimize performance and refine gameplay feel to release quality

**Responsibilities:**
- [ ] Profile rendering pipeline (CPU, GPU, memory)
- [ ] Set performance baseline metrics (FPS, load time, memory)
- [ ] Identify bottlenecks and optimization targets
- [ ] Address visual glitches (clipping, z-fighting, artifacts)
- [ ] Optimize shaders and rendering paths
- [ ] Profile memory usage and identify leaks
- [ ] Optimize physics and collision performance
- [ ] Improve load time and scene transitions
- [ ] Validate build size
- [ ] Execute 30-minute stability playtest
- [ ] Document before/after performance metrics

**Success Criteria:**
- 60 FPS average on target hardware (min 30 FPS)
- Load time < 5 seconds (< 10 seconds acceptable)
- Build size < 500 MB (< 1 GB acceptable)
- No memory leaks in profiler
- No visual glitches on 30-min playthrough
- Gameplay smoke test passes with performance data

**Performance Targets:**
| Metric | Target | Minimum |
|--------|--------|---------|
| Frame Rate | 60 FPS | 30 FPS stable |
| Load Time | < 5 sec | < 10 sec |
| Build Size | < 500 MB | < 1 GB |
| RAM Usage (Peak) | < 2 GB | < 4 GB |

**Gate Requirement:** Phase 7.1-7.3 must be integrated before optimization begins

---

#### 5. **Build Release Engineer** (Phase 7.5: Platform Testing)
**Mission:** Validate Crash Site on Windows across configurations and prepare release

**Responsibilities:**
- [ ] Document test matrix (OS versions, GPU variants, resolutions)
- [ ] Test Windows 10 on multiple GPU configurations (NVIDIA, AMD, Intel)
- [ ] Test Windows 11 on multiple GPU configurations
- [ ] Test multiple resolutions (1080p, 1440p, 4K, ultrawide)
- [ ] Test input devices (keyboard, mouse, gamepad if supported)
- [ ] Validate install/uninstall process
- [ ] Test clean save/load across platform variants
- [ ] Performance validation on minimum-spec hardware
- [ ] Document known issues or limitations
- [ ] Publish platform compatibility matrix and release readiness verdict

**Success Criteria:**
- No crashes on Windows 10 or 11
- Tested on NVIDIA, AMD, Intel GPUs (recent 3-generation drivers)
- Performance acceptable on minimum-spec hardware
- Clean install/uninstall works
- Resolution scaling works (no UI breakage)
- Known issues documented (if any)
- Release readiness verdict from Build Release Engineer

**Platform Test Matrix:**
| Config | Status | Driver | FPS | Notes |
|--------|--------|--------|-----|-------|
| Win10 + NVIDIA | TBD | Latest | TBD | - |
| Win10 + AMD | TBD | Latest | TBD | - |
| Win10 + Intel iGPU | TBD | Latest | TBD | - |
| Win11 + NVIDIA | TBD | Latest | TBD | - |
| Win11 + AMD | TBD | Latest | TBD | - |
| Win11 + Intel iGPU | TBD | Latest | TBD | - |
| 1080p | TBD | - | TBD | - |
| 1440p | TBD | - | TBD | - |
| 4K | TBD | - | TBD | - |
| Ultrawide | TBD | - | TBD | - |

**Gate Requirement:** Phase 7.4 must be complete before platform testing begins

---

### Supporting Agents (Cross-Cutting)

#### **QA Lead**
**Primary Role:** Validate all Phase 7 work and own gameplay/audio/platform gates

**Responsibilities:**
- [ ] Phase 7.2 Gate: Oversee balance playtesting and approve gameplay lock
- [ ] Phase 7.3 Gate: Validate audio integration and approve audio lock
- [ ] Phase 7.5 Gate: Oversee platform testing and compilation of results
- [ ] Execute all gameplay smoke tests with evidence collection
- [ ] Document bug lists and repo-owned vs. blocked classification
- [ ] Approve all gates with concrete evidence (not confidence language)

**Gate Authority:**
- Balance Validation Lock (Phase 7.2 → 7.4)
- Audio Integration Lock (Phase 7.3 → 7.4)
- Platform Compatibility Lock (Phase 7.5 complete)

---

#### **Producer**
**Primary Role:** Sequence work, manage dependencies, and publish verdicts

**Responsibilities:**
- [ ] Confirm Phase 6 completion and authorize Phase 7 start
- [ ] Verify Phase 7.1 Gate: Composition guide approval
- [ ] Monitor parallel workstreams (7.2, 7.3) for dependencies
- [ ] Approve Phase 7.4 start condition (7.1-7.3 complete)
- [ ] Approve Phase 7.5 start condition (7.4 complete)
- [ ] Publish final Phase 7 completion verdict
- [ ] Report risks or blockers to human
- [ ] Document timeline adherence (6-10 week estimate)

**Gate Authority:**
- Phase 6 → Phase 7 authorization
- Phase 7.1-7.3 → Phase 7.4 sequencing
- Phase 7.4 → Phase 7.5 sequencing
- Phase 7 completion verdict (PASS/NOT_GO)

---

#### **Visual Reviewer**
**Primary Role:** Independent approval of visual and artistic work

**Responsibilities:**
- [ ] Phase 7.1 Gate: Independent review of composition guide
  - Verify visual examples are opened PNGs with diagnosis
  - Confirm guide aligns with Art Taste Pack
  - Approve or reject with feedback
- [ ] Phase 7.4 Validation: Confirm no visual regressions during optimization
- [ ] Provide visual diagnosis for any glitches or issues

**Gate Authority:**
- Composition Guide Approval (Phase 7.1 complete)
- Visual Polish Validation (Phase 7.4 complete)

---

#### **Game Director**
**Primary Role:** Protect MVP scope and validate win/loss loop integrity

**Responsibilities:**
- [ ] Phase 7.2 Validation: Confirm balance changes don't expand scope
- [ ] Phase 7.4 Validation: Confirm optimization doesn't change gameplay intent
- [ ] Phase 7.5 Validation: Confirm platform expansion doesn't alter Crash Site loop
- [ ] Escalate any scope drift or MVP boundary violations

**Gate Authority:**
- Gameplay Balance Scope Lock (Phase 7.2)
- Optimization Scope Lock (Phase 7.4)
- Platform Testing Scope Lock (Phase 7.5)

---

#### **Engine Architect**
**Primary Role:** Validate technical decisions and architecture coherence

**Responsibilities:**
- [ ] Phase 7.3 Audit: Verify audio integration doesn't introduce dependencies or tighten architecture
- [ ] Phase 7.4 Audit: Confirm optimization changes maintain code health
- [ ] Phase 7.5 Audit: Validate platform expansion doesn't introduce platform-specific hacks

**Gate Authority:**
- Technical Risk Assessment (all phases)

---

#### **Tools Engineer**
**Primary Role:** Prepare and maintain pipelines for evidence collection

**Responsibilities:**
- [ ] Phase 7.2: Establish gameplay profiling and metrics collection tools
- [ ] Phase 7.3: Validate Godot audio integration pipeline
- [ ] Phase 7.4: Set up performance profiling and monitoring
- [ ] Phase 7.5: Establish platform testing automation and result aggregation
- [ ] All phases: Provide evidence collection scripts and documentation

**Deliverables:**
- Playtesting metrics harness
- Audio validation tools
- Performance profiler setup
- Platform test result aggregator

---

### Minor Supporting Agents

#### **Level Designer** (Standby)
- Review balance changes for spatial implications
- Validate resource placement post-balance

#### **UX Designer** (Standby)
- Audit UI during optimization (Phase 7.4)
- Validate platform testing results for input/UX issues

#### **Reviewer** (Standby)
- Independent code review of optimization and audio integration changes
- Validate commit quality and test coverage

#### **Validator** (Standby)
- Final validation pass before release readiness
- Confirm all gates are satisfied

#### **Creative Director** (Secondary)
- Provide creative direction for audio and composition work
- Mentor Art Director and Creative Director/audio specialist

#### **Orchestrator** (Meta-Coordination)
- Monitor agent compliance with governance
- Escalate gate failures and blockers
- Publish weekly status and risk reports

---

## Phase 7 Dependency Graph

```
Phase 6 Complete ✓
│
├─→ Phase 7.1: Composition Guide (weeks 1-2)
│   Owner: Art Director
│   Gate: Visual Reviewer approval
│   Output: docs/art/composition-guide.md
│
├─→ Phase 7.2: Balance (weeks 3-4, parallel)
│   Owner: Gameplay Engineer + QA Lead
│   Gate: QA validation of 5+ playthroughs
│   Output: playtesting evidence + balance config changes
│
├─→ Phase 7.3: Audio (weeks 3-4, parallel)
│   Owner: Creative Director
│   Gate: QA validation of 8+ audio categories
│   Output: integrated audio + THIRD_PARTY_ASSETS.md entries
│
├─→ Phase 7.4: Polish (weeks 5-6, after 7.1-7.3)
│   Gate condition: 7.1 ✓ AND 7.2 ✓ AND 7.3 ✓
│   Owner: Technical Director
│   Gate: 60 FPS, < 500 MB, no visual glitches
│   Output: performance metrics + optimized builds
│
└─→ Phase 7.5: Platform (weeks 7-8, after 7.4)
    Gate condition: 7.4 ✓
    Owner: Build Release Engineer
    Gate: Windows 10/11 + NVIDIA/AMD/Intel tested
    Output: platform compatibility matrix + release verdict
```

---

## Phase 7 Evidence & Gate Validation

### Gate Structure

Each Phase 7 subphase has explicit:
1. **Pass Criteria** (what must be true to advance)
2. **Blocking Conditions** (what stops advancement)
3. **Required Evidence** (concrete deliverables)
4. **Gate Owner** (who approves)
5. **Approved Verdicts** (PASS, NOT_GO, etc.)

### Evidence Requirements Summary

| Phase | Evidence | Gate Owner |
|-------|----------|-----------|
| 7.1 | Composition guide + Visual examples + Visual Reviewer verdict | Visual Reviewer |
| 7.2 | 5+ playthrough logs + balance config changes + smoke test | QA Lead |
| 7.3 | Audio files + THIRD_PARTY_ASSETS.md entries + smoke test | QA Lead |
| 7.4 | Performance metrics (before/after) + no regressions | Technical Director |
| 7.5 | Platform test matrix + known issues list | Build Release Engineer |

### Vague Verdict Prohibition

**FORBIDDEN verdicts** (automatic NOT_GO):
- "looks good"
- "should work"
- "improved"
- "tests passed" (without specific evidence)
- "ready to ship" (without gate approval)

**APPROVED verdicts** (use always):
- `PASS` (evidence satisfied all gate criteria)
- `NOT_GO` (blocking condition found, list specific blockers)
- `HUMAN_BLOCKED` (awaiting human decision)
- `ENVIRONMENT_BLOCKED` (container limitation)
- `INTENTIONAL_GATE` (awaiting future work)

---

## Parallel Work Scheduling

### Non-Blocking Parallelism

**Phase 7.2 (Balance) and Phase 7.3 (Audio) can run in parallel:**
- Gameplay Engineer + Gameplay + QA focus on balance
- Creative Director + audio specialist focus on sound design
- No file conflicts or circular dependencies
- Can integrate in parallel during weeks 3-4

**Estimated Efficiency Gain:** 1 week saved by parallelization

### Sequencing Requirements

**Phase 7.4 (Polish) MUST wait for Phase 7.1-7.3 integration:**
- Composition guide establishes visual intent
- Balance and audio are dependencies for smoke testing
- Technical Director needs stable feature set to optimize
- Cannot optimize a moving target

**Phase 7.5 (Platform) MUST wait for Phase 7.4 completion:**
- Must have final optimized build to test on platforms
- Cannot test moving baselines
- Final polish may fix platform-specific issues

---

## Agent Communication Protocol

### Weekly Status Cadence

Each agent publishes status at phase boundaries:

**Format:**
```markdown
## [Agent Name] — Phase 7.X Status

**Owner:** [Name]  
**Status:** In Progress / Complete / Blocked  
**Gate:** [Phase gate name]  
**Evidence collected:**
- [ ] Deliverable 1 (path/status)
- [ ] Deliverable 2 (path/status)
- [ ] Deliverable 3 (path/status)

**Blockers (if any):**
- Blocker A (owner, ETA to resolve)

**Next steps:**
- Next action (ETA)
```

### Escalation Protocol

**Gate failures escalate to Producer immediately:**
1. Agent discovers blocking condition
2. Agent publishes NOT_GO verdict with specific blockers
3. Producer assesses impact on timeline
4. Producer escalates to human if timeline risk > 1 week
5. Human makes decision; producer re-routes work

---

## Validation & Submission

### Per-Phase Validation

Before each phase completion:

```bash
# Agent validates own work
python3 tools/validate_agent_studio.py
git diff --check
git log --oneline -5  # Verify commits

# Gate owner validates deliverables
# (Manual evidence review per phase)

# Producer publishes gate verdict
```

### Phase 7 Completion Validation

**Final Release Readiness Gate:**

```bash
# Full suite validation
dotnet restore && dotnet build
godot --headless --path . --import
# Gameplay smoke test (manual or automated)
# Performance profiler run
# Platform test matrix review
```

**Gate Owner:** Producer + Build Release Engineer  
**Verdict Publication:** Final Phase 7 completion document

---

## Risk Mitigation

### Known Risks & Mitigation

| Risk | Impact | Mitigation |
|------|--------|-----------|
| Phase 6 delay | Phase 7 cannot start | Producer monitors Phase 6 gates weekly; escalate if at risk |
| Balance tuning → infinite loop | Timeline slip | Limit balance changes to 2-3 iterations per playtesting cycle |
| Audio licensing issues | Phase 7.3 blocked | Pre-approve all audio sources before integration begins |
| Performance regression from audio | Phase 7.4 rework | Profile audio integration early (week 2) before optimization |
| Platform GPU incompatibility | Phase 7.5 blocked | Document incompatibilities as "known issues" rather than blocking |

### Scope Drift Prevention

**Game Director + Producer actively monitor:**
- No new features disguised as "polish"
- No scope expansion beyond Phase 7 objectives
- No dependencies on post-Phase-7 work

**Automatic escalation if:**
- Code rewrite suggested for "balance"
- New audio category added beyond 8 core categories
- Platform support requested beyond Windows
- Any forbidden MVP feature appears

---

## Success Definition

**Phase 7 is COMPLETE when ALL of the following are TRUE:**

- [ ] **7.1 Composition Guide:** Document in `docs/art/composition-guide.md` with visual examples and Visual Reviewer approval
- [ ] **7.2 Balance:** 5+ playthroughs documented, fair difficulty, all repo-owned bugs tracked/fixed
- [ ] **7.3 Audio:** 8+ categories implemented with licensed sources in THIRD_PARTY_ASSETS.md
- [ ] **7.4 Optimization:** 60 FPS average (30 FPS min), < 500 MB build, no visual glitches
- [ ] **7.5 Platform:** Windows 10/11 tested on NVIDIA/AMD/Intel, clean install/uninstall works
- [ ] **All gates:** PASS verdicts with concrete evidence (no vague claims)
- [ ] **Timeline:** Completed within 6-10 week estimate
- [ ] **No scope creep:** Zero forbidden MVP features or post-Phase-7 dependencies
- [ ] **Release readiness:** Producer + Build Release Engineer sign-off on MVP closure

---

## Post-Phase-7 Considerations

After Phase 7 completion, future work may address:
- Multiple regions/maps (requires scope change)
- Additional enemy types (requires scope change)
- Advanced equipment/upgrades (requires scope change)
- Campaign continuation (requires scope change)
- Platform ports (requires scope change)

**All post-Phase-7 work requires explicit human authorization and README updates.**

---

## Orchestration Authority

**Source of Truth:**
- `README.md` § 5 (MVP definition), § 6 (forbidden scope)
- `AGENTS.md` § 2 (product boundary), § 3 (agent workflow)
- `docs/production/phase-7-planning.md` (detailed scope & gates)
- `studio/memory/production_stage_gates.md` (gate definitions)

**This Document:**
- Maps Phase 7 planning to agent roles
- Defines parallel execution strategy
- Establishes evidence gates and validation protocol
- Provides weekly status template and escalation rules

**Approval Required From:**
- Human or Producer before Phase 7 execution begins
- Visual Reviewer for Phase 7.1 gate
- QA Lead for Phase 7.2 and 7.3 gates
- Technical Director for Phase 7.4 gate
- Build Release Engineer for Phase 7.5 gate
- Producer for final Phase 7 completion verdict

---

**Status:** ORCHESTRATION BLUEPRINT (awaiting human approval and Phase 6 completion)  
**Next Action:** Human review, Phase 6 status verification, and authorization to begin Phase 7 execution
