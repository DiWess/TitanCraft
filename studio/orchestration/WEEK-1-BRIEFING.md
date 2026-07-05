# WEEK 1 EXECUTION BRIEFING
## Phase 7 Launch — Art Director & Gameplay Engineer Activation

**Date:** 2026-07-05  
**Duration:** Days 1-5 (2026-07-05 to 2026-07-12)  
**Status:** ✅ **ACTIVE — BEGIN IMMEDIATELY**

---

## EXECUTIVE SUMMARY

**Phase 7 is now AUTHORIZED.** Two lead agents activate parallel work streams:

| Agent | Phase | Task | Effort | Deadline |
|-------|-------|------|--------|----------|
| 🎨 Art Director | 7.1 | Composition Foundation | 2-4 hrs | End Day 5 |
| 🎮 Gameplay Engineer | 7.2 | Playtesting Infrastructure | 2-4 hrs | End Day 5 |

**Week 1 Success = Both deliverables completed by Friday EOD**

---

## 🎨 ART DIRECTOR — WEEK 1 TASK 7.1.1

### Mission
Create the foundation document for Phase 7.1: Composition Guide.

### What You're Building
A new document: **`docs/art/composition-guide.md`**

This guide will document visual composition principles for the approved Stage A Crash Site environment. Future asset creators and level designers will use this guide to ensure visual coherence.

### Task 7.1.1: Composition Foundation (2-4 hours)

**Deliverable:** `docs/art/composition-guide.md` (draft, ~800-1000 words)

**Steps:**

1. **Create the document skeleton** (15 min)
   ```markdown
   # Composition Guide: Crash Site MVP
   
   **Date:** [today's date]
   **Scope:** Visual composition principles for approved Stage A Crash Site
   **Authority:** Art Taste Pack + Phase 7 planning
   
   ## 1. Visual Hierarchy & Focal Points
   ## 2. Route Readability & Navigation
   ## 3. Silhouette & Scale Relationships
   ## 4. Material & Lighting Coherence
   ## 5. Color Palette & Contrast
   ## 6. Foreground, Midground, Background Composition
   ```

2. **Analyze the Approved Stage A Assets** (30 min)
   - Open `scenes/Main/Main.tscn` in Godot
   - Play the Crash Site level and take mental notes on:
     - What's the primary focal point when player spawns?
     - How does the eye flow through the environment?
     - Which visual elements guide the player toward objectives?
     - What visual scale relationships define the space?

3. **Document Focal Points** (30 min)
   - Where is the primary focal point (crashed ship, wreckage, beacon)?
   - What secondary focal points guide player movement?
   - How do lighting and silhouettes direct attention?
   - Document 3-5 specific focal point principles

4. **Document Route Readability** (30 min)
   - How do visual cues guide the player's navigation?
   - What design elements make it clear where to go?
   - Are paths visually distinct from non-traversable areas?
   - Document 3-4 route readability principles

5. **Document Silhouette & Scale** (30 min)
   - Analyze asset silhouettes (are they clear and distinct?)
   - How do scale relationships work in the environment?
   - How do different zones feel spatially?
   - Document scale relationship principles

6. **Document Material & Lighting** (30 min)
   - What materials define the Crash Site aesthetic?
   - How does lighting create mood and guide attention?
   - What's the overall material coherence?
   - Document material and lighting principles

7. **Reference Art Taste Pack** (15 min)
   - Open `docs/art/titancraft-visual-identity.md`
   - Ensure your composition guide aligns with established visual direction
   - Note any visual identity principles that apply to composition

### Success Criteria
- [ ] Document created at `docs/art/composition-guide.md`
- [ ] At least 6 composition principles documented (focal points, routes, silhouette, scale, materials, lighting)
- [ ] Each principle has 2-3 specific examples from Crash Site
- [ ] Document references Art Taste Pack
- [ ] No scene or asset modifications (document only)
- [ ] Ready for Visual Reviewer to provide detailed examples next week

### Definition of Done
- `docs/art/composition-guide.md` exists with skeleton + documented principles
- Principles are actionable (future designers can use them)
- Document links to Stage A assets by name/location
- No images required yet (that's next week, Task 7.1.2)

### Evidence to Collect
- Final document (path: `docs/art/composition-guide.md`)
- Git commit hash
- Proof you reviewed Stage A in Godot (brief note of observations)

### Next Week Preview (Task 7.1.2)
Next week you'll take 5-10 opened screenshots of Crash Site and annotate them with visual diagnosis. This week's foundation principles will guide which screenshots to capture.

---

## 🎮 GAMEPLAY ENGINEER — WEEK 1 TASK 7.2.1

### Mission
Set up the playtesting infrastructure for Phase 7.2: Balance validation.

### What You're Building
1. **`docs/testing/playtesting-template.md`** — Test execution checklist
2. **Gameplay Parameters Document** — Current balance values (health, damage, costs, etc.)
3. **Metrics Collection Setup** — Spreadsheet or script for recording playtesting data

### Task 7.2.1: Playtesting Infrastructure Setup (2-4 hours)

**Deliverables:**
- `docs/testing/playtesting-template.md`
- Gameplay parameters documented (in same file or separate)
- Metrics spreadsheet ready for QA Lead to fill in during sessions

**Steps:**

1. **Create Playtesting Template** (45 min)
   ```markdown
   # Playtesting Session Log — Template
   
   **Session Date:** [date]
   **Tester:** [name]
   **Session Duration:** [mins]
   **Build Version:** [git hash or build #]
   
   ## Pre-Session Setup
   - [ ] Build compiled
   - [ ] Game launches without errors
   - [ ] Main scene loads in < 10 seconds
   
   ## Gameplay Progression Checklist
   - [ ] Player spawns correctly
   - [ ] Movement feels responsive
   - [ ] Camera looks smooth
   - [ ] Jumping works as expected
   - [ ] Resource collection works (all 3 types)
   - [ ] Inventory displays correctly
   - [ ] Crafting opens and functions
   - [ ] Mechanical arm crafts successfully
   - [ ] Combat feels fair and responsive
   - [ ] Enemy appears and behaves correctly
   - [ ] Victory condition triggers
   - [ ] Game completes without crashes
   
   ## Difficulty Assessment
   - [ ] Resource collection: Too Easy / Fair / Too Hard
   - [ ] Crafting cost feels: Too Cheap / Fair / Too Expensive
   - [ ] Combat difficulty: Too Easy / Fair / Too Hard / Frustrating
   - [ ] Session duration: [minutes] (target: 10-30)
   
   ## Issues Encountered
   - [List any bugs, glitches, or balance problems]
   
   ## Metrics
   - Session duration: __ minutes
   - Resources collected: Metal __, Biomass __, Electronics __
   - Combat effectiveness: __ hits to defeat Scout
   - Player damage taken: __ HP
   
   ## Overall Assessment
   [Tester's subjective fairness/fun assessment]
   ```

2. **Document Current Gameplay Parameters** (45 min)
   
   Read the following C# files and document current values:
   - `src/Player/` — Player health, damage, attack cooldown
   - `src/Enemies/` — Scout health, damage, detection range, chase speed
   - `src/Crafting/` — Mechanical arm recipe costs
   - `src/Resources/` — Resource spawn locations and quantities
   
   Create parameter document:
   ```markdown
   # Current Gameplay Parameters
   
   ## Player
   - Health: [value] HP
   - Attack Cooldown: [value] seconds
   - Mechanical Arm Damage: [value] per hit
   
   ## Galaxabrain Scout
   - Health: [value] HP
   - Damage: [value] per hit
   - Detection Range: [value] meters
   - Chase Speed: [value] m/s
   
   ## Crafting
   - Mechanical Arm Cost:
     - Metal: [value]
     - Biomass: [value]
     - Electronics: [value]
   
   ## Resources
   - [List spawn locations and quantities for each type]
   ```

3. **Create Metrics Collection Spreadsheet** (45 min)
   
   Set up simple CSV or spreadsheet:
   ```
   Session #,Date,Duration,Resource Count,Combat Fairness,Difficulty,Issues,Tester Notes
   1,2026-07-XX,15,3,Fair,Fair,,None reported
   2,2026-07-XX,18,3,Fair,Hard,,Damage too high?
   ...
   ```

4. **Verify Godot Build Works** (30 min)
   - [ ] `dotnet build` succeeds
   - [ ] Launch game with `godot`
   - [ ] Main scene loads
   - [ ] Can play through to end without crashes
   - [ ] Note build time
   - [ ] Note startup time

### Success Criteria
- [ ] `docs/testing/playtesting-template.md` created
- [ ] Template has all required checklist items
- [ ] Current gameplay parameters documented
- [ ] Metrics spreadsheet ready for data entry
- [ ] Godot build verified working
- [ ] All files in git

### Definition of Done
- Playtesting infrastructure is ready for QA Lead to use in Week 2
- Parameters are documented so balance changes can be tracked
- Build is confirmed working for playtesting

### Evidence to Collect
- Files: `docs/testing/playtesting-template.md`, parameters doc, metrics spreadsheet
- Git commit hash
- Screenshot of successful game launch

### Next Week Preview (Task 7.2.2)
Next week you and QA Lead will execute first playtesting sessions using this template and spreadsheet. You'll gather 5+ complete playthroughs and document balance issues.

---

## 📋 PRODUCER — WEEK 1 COORDINATION

### Your Role (Day 1-5)

1. **Day 1: Activation**
   - [ ] Distribute this briefing to Art Director and Gameplay Engineer
   - [ ] Confirm both agents acknowledge receipt
   - [ ] Post "Phase 7 Active" status to team

2. **Days 2-5: Monitoring**
   - [ ] Check in with Art Director daily (brief progress check)
   - [ ] Check in with Gameplay Engineer daily (brief progress check)
   - [ ] Log any blockers or questions
   - [ ] Verify deliverables meet success criteria

3. **Friday (Day 5): Status Submission**
   - [ ] Collect status from both agents
   - [ ] Verify both deliverables completed
   - [ ] Assess readiness for Week 2
   - [ ] Identify any risks or schedule impacts

### Success Criteria
- [ ] Both agents complete deliverables by EOD Friday
- [ ] No blockers impeding Week 2 start
- [ ] Producer confirms Week 2 readiness

---

## 🛠️ TOOLS ENGINEER — WEEK 1 SUPPORT

### Your Role (Day 1-5)

1. **Prepare Profiling Infrastructure** (2 hours)
   - Set up Godot profiler for Phase 7.4 optimization
   - Create performance baseline script
   - Document how to capture FPS metrics
   - Prepare memory profiler for Phase 7.4

2. **Verify Metrics Collection Works** (1 hour)
   - Test CSV/spreadsheet setup for playtesting
   - Verify data entry process is simple
   - Provide sample data for testing
   - Document data export process

3. **Support as Needed**
   - Answer any tool questions from Gameplay Engineer or Art Director
   - Troubleshoot Godot build issues if they arise
   - Prepare Week 2 testing automation (if applicable)

### Success Criteria
- [ ] Profiling infrastructure ready for Phase 7.4
- [ ] Metrics spreadsheet tested and working
- [ ] No tool blockers for Art Director or Gameplay Engineer

---

## DAILY STANDUP FORMAT (Async)

**Each Day at 17:00 UTC (in your respective time zone):**

Post brief update to team:

```
[Agent Name] — Day [#] Standup

Task: 7.X.Y [Task Name]
Progress: [%] complete
Blockers: [None / specific blocker]
On Track: [Yes / No]
Next: [What you're doing tomorrow]
```

Example:
```
Art Director — Day 3 Standup

Task: 7.1.1 Composition Foundation
Progress: 60% complete (principles documented, reviewing Stage A)
Blockers: None
On Track: Yes
Next: Finish material/lighting principles tomorrow, finalize draft
```

---

## FRIDAY DELIVERABLE SUBMISSION (EOD Day 5)

**Art Director → Submit:**
- `docs/art/composition-guide.md` (final draft, 800-1000 words)
- Git commit hash
- Brief note: "Which Stage A assets inspired which principles?"

**Gameplay Engineer → Submit:**
- `docs/testing/playtesting-template.md` (ready for use)
- `docs/testing/gameplay-parameters.md` (or similar)
- Metrics spreadsheet (.csv or link)
- Git commit hash
- Brief note: "Build verified working for playtesting"

**Producer → Publish:**
- Week 1 status report (both deliverables, risks, Week 2 readiness)
- Schedule Week 2 daily standups
- Confirm Phase 7.2 and 7.3 agents ready to launch

---

## TOOLS & RESOURCES

### For Art Director
- Open `docs/art/titancraft-visual-identity.md` for reference
- Review `docs/production/phase-7-planning.md` § Phase 7.1 for scope
- Open `scenes/Main/Main.tscn` in Godot to analyze Stage A
- Refer to `studio/orchestration/phase-7-task-breakdown.md` for full task context

### For Gameplay Engineer
- Review `docs/production/phase-7-planning.md` § Phase 7.2 for scope
- Open `src/` directory to find current parameter values
- Refer to `studio/orchestration/phase-7-task-breakdown.md` for full task context
- Run `dotnet build` to verify project builds

### For Tools Engineer
- Godot profiler documentation (built-in Tools menu)
- C# .NET profiling tools (if needed)
- CSV/spreadsheet format (simple text)

---

## COMMUNICATION

**Daily Standup:** Async (post in team channel)  
**Blockers:** Immediate escalation to Producer  
**Questions:** DM Producer or relevant support agent  
**Status Submission:** Friday EOD to Producer

---

## SUCCESS = WEEK 2 LAUNCH

**If both deliverables are complete by Friday EOD:**
- Art Director continues to Task 7.1.2 (Visual Examples)
- Gameplay Engineer + QA Lead begin Task 7.2.2 (First Playtesting Sessions)
- Creative Director + Audio Specialist launch Phase 7.3 (Audio Research)

**If any deliverable incomplete:**
- Producer assesses impact
- Adjusts Week 2 schedule accordingly
- No Phase 7 gate advancement until ready

---

## WHAT SUCCESS LOOKS LIKE

✅ **Art Director's Week 1 Success:**
- Document with 6+ documented principles
- Principles are specific and actionable
- References Stage A assets by name
- Ready for Visual Reviewer to add detailed examples

✅ **Gameplay Engineer's Week 1 Success:**
- Playtesting template ready for QA Lead to use
- Current parameters documented for comparison
- Metrics spreadsheet ready for data collection
- Build confirmed working for playtesting

✅ **Producer's Week 1 Success:**
- Both agents complete deliverables
- No blockers impeding Week 2
- Team ready to launch parallel Phase 7.2 and 7.3

---

## PHASE 7.1 → 7.5 OVERVIEW (For Context)

Your Week 1 deliverables are the foundation for:

**Weeks 1-2:**
- Art Director finishes composition guide
- Visual Reviewer approves it
- → Locked visual direction for future artists

**Weeks 3-4 (Parallel):**
- Gameplay Engineer + QA Lead run 5+ playtesting sessions
- Adjust balance based on evidence
- Creative Director integrates 8+ audio categories

**Weeks 5-6:**
- Technical Director optimizes performance
- Relies on your Week 1-2 work being complete

**Weeks 7-8:**
- Build Release Engineer tests on platforms
- Relies on Week 5-6 optimization being complete

**Week 9-10:**
- Producer publishes final Phase 7 verdict
- MVP ready for release

---

## GO LIVE 🚀

**Status:** ✅ **ACTIVE NOW**  
**Week 1 Deadline:** 2026-07-12 EOD  
**Phase 7 Target Completion:** 2026-08-23 to 2026-09-06

**Begin immediately.**

Art Director: Start with Stage A analysis.  
Gameplay Engineer: Start with parameters documentation.  
Producer: Monitor and escalate blockers.  
Tools Engineer: Prepare infrastructure.

---

*This briefing is the operational mandate for Week 1. Questions or blockers: escalate immediately to Producer.*
