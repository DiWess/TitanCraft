# Pre-Beta Audit & Studio-Wide Alignment Brief
**Authority:** Producer + Orchestrator (AGENTS.md § 3)  
**Date:** 2026-07-08  
**Scope:** Verify all production documentation aligns with executed code; fix any drift  
**Gate:** Beta Testing Readiness  
**Status:** AGENT ENGAGEMENT REQUIRED

---

## Executive Brief

**All agents:** You are instructed to audit the entire production chain (Phase 1 → Release) and identify/fix any drift between documentation and actual codebase state.

**This is NOT a new production phase. This is a verification and alignment pass before beta testers receive the build.**

---

## What We're Checking

### Documentation Chain
1. **README.md** — MVP scope definition (authoritative)
2. **AGENTS.md** — Governance structure (authoritative)
3. **Stage A: Art Brief Packet** — Visual direction locked
4. **Stage B: Asset Manifest** — 10 candidates documented
5. **Stage B: Visual/Tech Verdicts** — Independent reviews
6. **Stage C: Integration Report** — Scene assembly
7. **Stage C: Final Validation** — All tests PASS
8. **Release Gate Verdict** — Approved for ship

### Codebase Reality
- `src/Scenes/CrashSite.tscn` — Scene file (actual state)
- `src/Scripts/` — Gameplay code (actual implementation)
- Asset imports (actual GLB files)
- Performance profile (actual metrics)
- Any uncommitted changes or anomalies

---

## Audit Tasks by Agent

### 1. Creative Director
**Task:** Verify narrative and naming alignment

**Checklist:**
- [ ] Verify no banned brand names used (Jarvis, SpaceX, Titanfall, Minecraft)
- [ ] Confirm original naming throughout (NOVA placeholder for AI, etc.)
- [ ] Verify Crash Site MVP narrative is locked (no campaign expansion)
- [ ] Confirm no lore bloat beyond "stranded astronaut, Galaxabrains, mechanical arm"
- [ ] Check all UI text matches approved tone
- [ ] Verify victory/defeat/save messaging is on-brand

**Report to:** Producer (findings + fixes applied)  
**Deadline:** Today

---

### 2. Art Director
**Task:** Verify visual assets match Stage A direction

**Checklist:**
- [ ] Confirm all 10 candidates are integrated in CrashSite.tscn
- [ ] Verify each candidate's silhouette still reads in neutral grey
- [ ] Check material assignments: albedo/roughness/metalness values match palette
- [ ] Confirm no excessive glow (beacon, energy core, pickup signals only)
- [ ] Verify wear language visible (damage, patches, field repair)
- [ ] Check for any toy-like proportions or photorealism creep
- [ ] Validate all asset languages (human/alien/terrain) distinct
- [ ] Confirm lighting reference materials applied consistently

**If Issues Found:**
- Reapply materials per Stage A palette
- Adjust glow intensity to minimal/functional only
- Fix silhouette clarity (neutral grey test)
- Re-export GLB if geometry changed

**Report to:** Producer (summary of findings + fixes)  
**Deadline:** Today

---

### 3. Technical Director
**Task:** Verify pipeline and performance

**Checklist:**
- [ ] Run `dotnet build` — confirm no compiler errors
- [ ] Test GLB imports in Godot: all 10 candidates load cleanly
- [ ] Verify material assignments in Godot PBR standard
- [ ] Profile performance: confirm 60 FPS stable on Windows (baseline 1440p)
- [ ] Check draw calls: confirm 45-50 DC target met
- [ ] Verify GPU time: confirm 4-5 ms assets acceptable
- [ ] Check memory: confirm ~80 MB scene footprint comfortable
- [ ] Test scene load time: confirm <2 seconds
- [ ] Verify no shader compilation errors
- [ ] Check for missing texture references or broken material nodes

**If Issues Found:**
- Rerun GLB pipeline (Blender → GLB → Godot)
- Adjust material imports if PBR mapping incorrect
- Optimize draw calls via batching if needed
- Verify Windows export build cleanliness

**Report to:** Producer (performance metrics + any regressions)  
**Deadline:** Today

---

### 4. Level Designer
**Task:** Verify scene layout and gameplay flow

**Checklist:**
- [ ] Confirm all 10 candidates are positioned in CrashSite.tscn
- [ ] Verify navigation routes: Crash Hull → Terrain Basin → Workbench → Beacon
- [ ] Check collision meshes: all active and no unexpected clipping
- [ ] Verify resource pickup zones: distributed logically across map
- [ ] Confirm Scout arena: encounter space has clear approach/retreat
- [ ] Check workbench: interaction zone positioned for crafting flow
- [ ] Verify beacon: positioned prominently as victory objective
- [ ] Confirm save points: located in safe zones, not in combat areas
- [ ] Validate visual composition: focal points readable without UI

**If Issues Found:**
- Reposition assets if layout breaks intended flow
- Adjust collision meshes if clipping detected
- Move resources if distribution seems unfair or unclear
- Test player paths if navigation feels wrong

**Report to:** Producer (layout validation + any repositioning needed)  
**Deadline:** Today

---

### 5. Gameplay Engineer
**Task:** Verify all mechanics are implemented and working

**Checklist:**
- [ ] Test movement: fluid, responsive, no stuck spots
- [ ] Test resource gathering: all 3 types collectible
- [ ] Test crafting: mechanical arm recipe works, UI anchors correctly
- [ ] Test combat: Scout AI functional, weak point identifiable, damage feedback works
- [ ] Test victory flow: beacon activation → win state → main menu
- [ ] Test save system: checkpoints save/load correctly
- [ ] Verify camera behavior: no clipping with objects or terrain
- [ ] Check animations: all skeleton animations play correctly
- [ ] Verify all input bindings: movement, interact, attack responsive
- [ ] Test edge cases: resource overflow, crafting with insufficient materials, death handling

**If Issues Found:**
- Fix animation state machine issues if clips/breaks
- Adjust combat AI if too easy/hard
- Fix crafting UI if doesn't anchor correctly
- Debug save system if doesn't persist state

**Report to:** Producer (gameplay validation + any fixes applied)  
**Deadline:** Today

---

### 6. Visual Reviewer
**Task:** Final in-engine visual coherence check

**Checklist:**
- [ ] Open CrashSite.tscn in editor; verify visually
- [ ] Screenshot all major views: Crash Hull, Terrain Basin, Workbench, Beacon, Scout arena
- [ ] Verify focal points: eye naturally drawn to objectives
- [ ] Check routes: all main paths visually readable at 50m+
- [ ] Verify silhouettes: all assets distinct in neutral grey (if possible in-engine)
- [ ] Confirm materials: palette applied, no conflicts
- [ ] Check glow effects: minimal, functional only (no neon saturation)
- [ ] Verify overall tone: Polygonal Salvage Sci-Fi consistent
- [ ] Compare to Stage A brief: any deviations from approved direction?
- [ ] Spot-check composition: does it support gameplay without UI dependency?

**If Issues Found:**
- Flag material conflicts (report to Art Director)
- Note glow excess (report to Art Director)
- Flag composition problems (report to Level Designer)
- Document silhouette clarity issues (report to Art Director)

**Report to:** Producer (visual audit + flagged items for rework)  
**Deadline:** Today

---

### 7. QA Lead
**Task:** Pre-beta smoke test and stability audit

**Checklist:**
- [ ] Build and launch game on Windows (or target platform)
- [ ] Test full gameplay loop: spawn → gather → craft → fight → win
- [ ] Verify no crashes, hard locks, or soft locks
- [ ] Check all UI: menus, inventory, crafting, objectives display correctly
- [ ] Test performance: frame rate stable at 60 FPS
- [ ] Verify audio: all sound effects play at correct times
- [ ] Test save/load: checkpoint system works end-to-end
- [ ] Check error logs: no warnings or unexpected debug output
- [ ] Verify all visual feedback: damage, resource pickup, crafting confirmation
- [ ] Test edge cases: running out of resources, taking fatal damage, winning

**If Issues Found:**
- Document crash logs and reproduction steps
- Note any performance drops or frame stutters
- Flag UI display issues
- Report audio/visual feedback mismatches
- Identify progression blockers

**Report to:** Producer (smoke test results + any blockers)  
**Deadline:** Today

---

### 8. Asset Librarian
**Task:** Verify provenance audit trail and manifest accuracy

**Checklist:**
- [ ] Confirm Asset Manifest V1 lists all 10 candidates with hashes
- [ ] Verify each candidate has source documentation (brief reference)
- [ ] Check material assignments are recorded (albedo/roughness/metalness)
- [ ] Confirm all GLB exports are logged with file hashes
- [ ] Verify no fake placeholder assets (all original authored or approved sourced)
- [ ] Check for any unlicensed or undocumented assets
- [ ] Confirm all Stage B candidates have provenance records
- [ ] Verify no assets changed since final validation without re-audit

**If Issues Found:**
- Update manifest with correct hashes if GLB files regenerated
- Document any asset changes with new provenance notes
- Flag any undocumented assets (need sourcing or rejection)
- Update license matrix if assets added/removed

**Report to:** Producer (manifest accuracy + provenance audit complete)  
**Deadline:** Today

---

### 9. Producer
**Task:** Convergence and final approval for beta

**Responsibility:** Collect all agent reports and verify no drift

**Checklist:**
- [ ] Receive audit reports from all 8 agents
- [ ] Verify no scope expansion detected
- [ ] Confirm all documented decisions are reflected in code
- [ ] Check for any regressions from Stage A → Release
- [ ] Verify README.md boundaries maintained throughout
- [ ] Confirm no forbidden MVP features added
- [ ] Sign off on any fixes agents applied
- [ ] Issue final beta readiness verdict

**Gate Conditions:**
1. ✅ All agent audits complete
2. ✅ No scope violations found
3. ✅ All identified drift has been fixed
4. ✅ Performance targets maintained
5. ✅ No new blockers introduced

**If All Clear:**
- Issue BETA_READY verdict
- Unlock beta testing phase
- Notify QA/users that build is ready

**If Issues Remain:**
- Return to responsible agent with rework request
- Document blocker
- Reschedule audit after fixes applied

**Report:** Final Pre-Beta Audit Summary (to all agents + stakeholders)  
**Deadline:** Today

---

## How to Report Findings

### For Each Agent
**Format:** Create a brief comment or note in your task section:

```
AGENT: [Name]
TASK: [Brief title]
FINDINGS:
- [Finding 1: Description + location in codebase]
- [Finding 2: Description + location in codebase]
- [Finding 3: etc.]

FIXES APPLIED:
- [Fix 1: What was changed and why]
- [Fix 2: What was changed and why]

SIGN-OFF:
Status: ✅ PASS (no drift) or ⚠️ DRIFT FOUND & FIXED (describe)
Date: 2026-07-08
```

### For Producer
**Convergence Report Format:**

```
PRODUCER CONVERGENCE AUDIT — PRE-BETA READINESS

Agent Reports Received:
1. Creative Director: [Status]
2. Art Director: [Status]
3. Technical Director: [Status]
4. Level Designer: [Status]
5. Gameplay Engineer: [Status]
6. Visual Reviewer: [Status]
7. QA Lead: [Status]
8. Asset Librarian: [Status]

OVERALL STATUS:
- Drift detected: [Yes/No]
- Drift fixed: [Yes/No]
- Scope violations: [0]
- Regressions: [0]

FINAL VERDICT: ✅ BETA_READY or ⚠️ HOLD (explain)
```

---

## Timeline

**This Audit MUST Complete Today (2026-07-08)**

| Time | Agent | Task |
|------|-------|------|
| Now | All | Read this brief; begin audit |
| Next 2 hours | Individual agents | Run checklists; identify drift |
| Next 4 hours | Responsible agents | Apply fixes for drift found |
| End of day | All | Report findings to Producer |
| EOD | Producer | Issue final Pre-Beta Verdict |

---

## What Happens After This Audit

### If Verdict: ✅ BETA_READY
- Build is released to beta testers
- Production moves to post-launch phase
- Bug tracking and minor fixes handled separately

### If Verdict: ⚠️ HOLD
- Document blockers
- Assign rework tasks to responsible agents
- Reschedule audit for next day
- No beta until drift is resolved

---

## Authority & Governance

**This audit is mandatory per AGENTS.md § 3:**
- All agents must verify their work against documentation
- Drift must be fixed before beta release
- No stage skips due to documentation misalignment
- Evidence must match execution

**Producer gate is final authority** for beta readiness verdict.

---

## Questions?

If any agent is unclear on their audit scope or checklist, ask the Producer immediately. This audit must be comprehensive and honest—finding and fixing drift NOW prevents beta test failures and user confusion later.

**We are 4 days from Stage A to Release-Ready. This final verification ensures we ship with integrity.**

---

## Sign-Off

**Orchestrator/Producer:** All agents are engaged and empowered to audit, identify, and fix drift.

**All agents:** You are authorized to modify code, assets, and documentation as needed to align execution with documented intent.

**Schedule:** Audit complete by EOD 2026-07-08. Beta readiness verdict issued same day.

**Status:** ✅ AUDIT IN PROGRESS

---
