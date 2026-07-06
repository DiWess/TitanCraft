# PHASE 7 AUTHORIZATION DOCUMENT
## Extended Production Sprint Activation

**Authorization Date:** 2026-07-05  
**Authorized By:** Human (via explicit Phase 7 authorization)  
**Status:** ✅ **ACTIVE**  
**Timeline:** 6-10 weeks (24-40 hours)  
**Branch:** `claude/workflow-asset-setup-97uizh`

---

## Phase 6 Gate Verification

### ✅ Technical Prerequisites Confirmed
- [x] Windows export presets configured (`export_presets.cfg`)
- [x] Main scene exists (`scenes/Main/Main.tscn`)
- [x] Project builds successfully (dotnet build passes)
- [x] Project configuration complete (`config/name="TitanCraft"`)

### ✅ Git Status
- Latest commits: Asset records + orchestration plan
- Branch: `claude/workflow-asset-setup-97uizh`
- Ready to integrate Phase 7 work

---

## Phase 7 Activation

### Authorization Level
**FULL AUTHORIZATION GRANTED** for all Phase 7.1-7.5 workstreams:
- [ ] Phase 7.1: Art Director Composition Guide
- [ ] Phase 7.2: Gameplay Engineer Balance Testing
- [ ] Phase 7.3: Creative Director Audio Implementation
- [ ] Phase 7.4: Technical Director Optimization
- [ ] Phase 7.5: Build Release Engineer Platform Testing

### Scope Locked
All work operates within MVP Crash Site scope as defined in:
- `README.md` § 5-6 (MVP definition and forbidden features)
- `AGENTS.md` § 2 (product boundary)
- `docs/production/phase-7-planning.md` (detailed scope per phase)

**Forbidden Features (Protected):**
- Multiplayer, new enemy types, new weapons
- Grappling hook, wall running, procedural world
- Cloud services, accounts, telemetry
- Any features in README.md § 6

### Governance Rules In Effect
- **Game Director** monitors scope (escalates if expansion detected)
- **Producer** controls sequencing and gates
- **QA Lead** validates all evidence
- **Evidence gates** prevent advancement without proof
- **No vague verdicts** ("looks good" → NOT_GO automatically)

---

## Week 1 Activation Plan

### Daily Schedule (Starting Immediately)

**Day 1-2: Planning & Documentation**
- [x] Phase 7 authorization document (this file)
- [ ] Distribute orchestration documents to all agents
- [ ] All agents review `phase-7-agent-coordination.md`
- [ ] All agents review `phase-7-task-breakdown.md`

**Day 3-5: Week 1 Task Execution**

**Art Director → Phase 7.1.1: Composition Guide Foundation**
- Create `docs/art/composition-guide.md` skeleton
- Document Stage A Crash Site focal points and visual hierarchy
- Document material and lighting coherence principles
- **Effort:** 2-4 hours
- **Deliverable:** Draft composition guide with structure
- **DUE:** End of Day 5

**Gameplay Engineer → Phase 7.2.1: Playtesting Infrastructure Setup**
- Document current gameplay parameters (health, damage, crafting costs, etc.)
- Create `docs/testing/playtesting-template.md` template
- Set up metrics collection spreadsheet/script
- **Effort:** 2-4 hours
- **Deliverable:** Playtesting infrastructure ready for QA Lead
- **DUE:** End of Day 5

**Tools Engineer → Supporting Setup**
- Verify Godot headless import works (`godot --headless --path . --import`)
- Prepare profiling tools for Phase 7.4
- Set up metrics collection harness for playtesting
- **Effort:** 1-2 hours
- **DUE:** End of Day 5

**Producer → Gate Monitoring**
- Confirm Phase 7 start via this authorization
- Assign Week 1 tasks to Art Director and Gameplay Engineer
- Schedule Weekly Friday status meetings for all agents
- **DUE:** Immediately

### Week 1 Success Criteria
- [ ] Phase 7.1.1 deliverable: Draft composition guide skeleton
- [ ] Phase 7.2.1 deliverable: Playtesting infrastructure template
- [ ] All agents aware of orchestration plan and their roles
- [ ] Producer confirms Friday status meeting setup
- [ ] No blockers preventing Week 2 continuation

---

## Agent Assignments (Week 1-2)

### Phase 7.1: Composition Guide (Weeks 1-2)
**Lead:** 🎨 **Art Director**  
**Secondary:** 👀 **Visual Reviewer**, 🎨 **Creative Director**

**Week 1 Tasks:**
- Task 7.1.1: Composition Foundation (2-4 hrs)
- Task 7.1.2: Visual Examples & Diagnosis (2-4 hrs)

**Week 2 Tasks:**
- Task 7.1.3: Visual Reviewer Approval (1-2 hrs)
- Task 7.1.4: Integration (1 hr)

**Gate:** Visual Reviewer publishes PASS verdict

---

### Phase 7.2: Balance Testing (Weeks 3-4, Parallel Start Week 1)
**Lead:** 🎮 **Gameplay Engineer** + 📋 **QA Lead**  
**Secondary:** ⚙️ **Technical Director**, 🎯 **Producer**

**Week 1 Tasks:**
- Task 7.2.1: Infrastructure Setup (2-4 hrs)

**Week 2 Tasks:**
- Task 7.2.2: First 2-3 playtesting sessions

**Week 3-4 Tasks:**
- Complete 5+ sessions total
- Balance adjustments
- QA validation

**Gate:** QA Lead approves 5+ playthroughs with fair difficulty

---

### Phase 7.3: Audio (Weeks 3-4, Parallel Start Week 1)
**Lead:** 🔊 **Creative Director**  
**Secondary:** 🎮 **Gameplay Engineer**, 📋 **QA Lead**

**Week 1 Tasks:**
- Task 7.3.1: Audio Source Research (2-4 hrs)

**Week 2 Tasks:**
- Task 7.3.2: Provenance Documentation
- Task 7.3.3: Godot Integration

**Week 3-4 Tasks:**
- Audio smoke testing
- Mixing validation
- QA approval

**Gate:** QA Lead approves 8+ categories with valid provenance

---

### Phase 7.4: Optimization (Weeks 5-6, Starts After 7.1-7.3)
**Lead:** ⚙️ **Technical Director**  
**Secondary:** 🎮 **Gameplay Engineer**, 📋 **QA Lead**, 📦 **Build Release Engineer**

**Gate Condition:** Phase 7.1 ✓ AND Phase 7.2 ✓ AND Phase 7.3 ✓

**Tasks:**
- Performance baseline
- Visual glitch fixes
- Rendering optimization
- Memory & load time optimization
- Build size optimization

**Gate:** 60 FPS, <500MB, no visual glitches, no leaks

---

### Phase 7.5: Platform Testing (Weeks 7-8, Starts After 7.4)
**Lead:** 📦 **Build Release Engineer**  
**Secondary:** ⚙️ **Technical Director**, 📋 **QA Lead**

**Gate Condition:** Phase 7.4 ✓

**Tasks:**
- Platform test matrix setup
- Windows 10 testing (NVIDIA, AMD, Intel)
- Windows 11 testing (NVIDIA, AMD, Intel)
- Resolution & input testing
- Install/uninstall testing
- Known issues documentation

**Gate:** Windows 10/11 + 3 GPU variants tested, no critical crashes

---

## Status Reporting Protocol

### Weekly Status Submission (Fridays)
All agents submit status using this format:

```markdown
## [Agent Name] — Phase 7.X Status Report (Week #)

**Phase:** 7.X [Phase Name]  
**Owner:** [Agent Name]  
**Status:** In Progress / Complete / Blocked

### Completed This Week
- [x] Task 7.X.Y: [Task Name]
- [x] Task 7.X.Z: [Task Name]

### Current Task
- [ ] Task 7.X.W: [Task Name] — [%] complete

### Blockers
- [Blocker name] — Owner: [Agent], ETA: [date]

### Evidence Collected
- File path or commit hash
- Metrics or deliverable location
- Test results (if applicable)

### Next Week
- [ ] Task 7.X.V: [Task Name]
- [ ] Task 7.X.U: [Task Name]

### Gate Status
- Prerequisites met: YES / NO
- Evidence collected: YES / NO
- Ready for gate review: YES / NO
```

### Gate Review Protocol

**When agent completes phase:**
1. Agent publishes evidence (files, commits, screenshots, metrics)
2. Gate Owner (QA Lead, Visual Reviewer, Technical Director, Build Release Engineer) reviews
3. Gate Owner publishes verdict:
   - **PASS:** All criteria met, proceed to next phase
   - **NOT_GO:** Specific blockers listed, agent addresses
4. Producer monitors gate progression and adjusts timeline if needed

---

## Risk Escalation

### Automatic Escalations to Producer
- Gate fails (NOT_GO verdict published)
- Task effort exceeds estimate by >50%
- Timeline risk (behind schedule by >1 week)
- Scope expansion detected (forbidden feature appears)
- Critical blocker impacts multiple phases

### Producer Escalations to Human
- Phase 6 status unclear
- Stage A visual approval missing
- Timeline at risk of >2 week slip
- Scope change requested
- Release readiness gate fails

---

## Commit & Push Strategy

### Per-Phase Commits
Each phase produces clean, focused commits:

```bash
# Example: Phase 7.1 completion
git commit -m "feat: add art director composition guide

- Visual composition principles for Crash Site
- 5-10 opened PNG examples with focal point analysis
- Route readability and silhouette guidance
- Alignment with Art Taste Pack visual identity
- Visual Reviewer independent approval

Co-Authored-By: [Agent] <noreply@anthropic.com>
Claude-Session: [session]"
```

### Branch Strategy
- Work on `claude/workflow-asset-setup-97uizh`
- Push commits after each phase gate passes
- PR to main after Phase 7 complete (final Producer sign-off)

---

## Success Definition (Phase 7 Complete)

Phase 7 is **DONE** when ALL of these are TRUE:

**Week 1-2:**
- [ ] Phase 7.1 composition guide approved by Visual Reviewer

**Week 3-4:**
- [ ] Phase 7.2 balance validated with 5+ complete playthroughs
- [ ] Phase 7.3 audio implemented with 8+ categories and provenance

**Week 5-6:**
- [ ] Phase 7.4 performance meets 60 FPS target (30 FPS minimum)
- [ ] No visual glitches on 30-minute session
- [ ] Build size < 500 MB

**Week 7-8:**
- [ ] Phase 7.5 platform testing complete
- [ ] Windows 10 & 11 tested on NVIDIA, AMD, Intel
- [ ] No critical crashes

**Overall:**
- [ ] All gates have PASS verdicts
- [ ] Timeline within 6-10 weeks
- [ ] Zero scope expansion (no forbidden MVP features)
- [ ] Producer publishes final PASS verdict

---

## Authority & Validation

**Source of Truth:**
- `README.md` § 5-6 (MVP definition)
- `AGENTS.md` § 2-3 (agent workflow)
- `docs/production/phase-7-planning.md` (scope details)
- `studio/orchestration/phase-7-agent-coordination.md` (coordination)
- `studio/orchestration/phase-7-task-breakdown.md` (tasks)

**Validation Commands (All Phases):**
```bash
python3 tools/validate_agent_studio.py
git diff --check
```

**Before Release:**
```bash
dotnet restore && dotnet build
godot --headless --path . --import
```

---

## Document Signatures

**Authorization:**
```
Phase 7 authorization granted by: Human (explicit authorization on 2026-07-05)
Producer acknowledgment: [Pending first status report]
Game Director scope lock: [Pending]
QA Lead gate readiness: [Pending]
```

**Timeline Start:** Immediately (2026-07-05)  
**Week 1 Deadline:** 2026-07-12  
**Week 2 Deadline:** 2026-07-19  
**Phase 7 Target Completion:** 2026-08-23 to 2026-09-06 (6-10 weeks)

---

## Contingency Plans

### If Phase 7.1 (Composition) Slips
- Move to blocking Phase 7.4 optimization
- Composition guide can be completed in parallel (non-blocking)
- Game Director confirms scope impact

### If Phase 7.2-7.3 (Balance/Audio) Slips
- Parallel work continues independently
- If both slip >1 week: escalate to Producer
- Can accept degraded features (fewer audio categories, less playtesting) per Producer call

### If Phase 7.4 (Optimization) Slips
- Blocks Phase 7.5 (platform testing)
- If slip >2 weeks: escalate Phase 7.5 timeline risk
- Minimum FPS target (30 FPS) cannot be relaxed per README

### If Phase 7.5 (Platform) Fails
- Document known issues rather than block release
- Build Release Engineer and Producer decide release readiness
- Critical crashes (not cosmetic issues) block release

---

## Next Steps (Immediate)

**Day 1 (Today):**
1. ✅ Distribute this authorization document to all agents
2. ✅ Post Phase 7 activation in studio comms
3. ✅ Art Director reviews Phase 7.1 scope
4. ✅ Gameplay Engineer reviews Phase 7.2 scope
5. ✅ Producer confirms task assignments

**Day 2:**
6. Art Director begins Task 7.1.1
7. Gameplay Engineer begins Task 7.2.1
8. Tools Engineer prepares infrastructure

**Day 3-5:**
9. Deliverables collected and verified
10. Producer reviews Week 1 progress
11. Plan Week 2 continuation

---

**STATUS: 🚀 PHASE 7 ACTIVE**  
**ACTIVATION TIME: 2026-07-05 14:00 UTC**  
**WEEK 1 DEADLINE: 2026-07-12**  
**PHASE 7 TARGET: 2026-08-23 to 2026-09-06**

---

*This authorization supersedes any prior Phase 7 status and activates all orchestration documents. All agents should treat this document as the operational mandate for Phase 7 execution.*
