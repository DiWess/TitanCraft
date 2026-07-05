# Visual Completion Checklist

**Status:** Governance document identifying blocking tasks  
**Date:** 2026-07-05  
**Objective:** Close remaining tasks to unblock Stage A visual approval and completion

---

## Current Visual Status

### Completed ✓
- Art Taste Pack / Visual Identity established (docs/art/titancraft-visual-identity.md)
- Blender Asset Forge pipeline complete (tools/blender/, automated GLB export)
- Stage A terrain assets produced (TC_TerrainDioramaKit_V1)
- Visual Artifact Factory CI workflow created (.github/workflows/visual-artifact-factory.yml)
- Stage A asset review workflow created (.github/workflows/stage-a-blender-asset-review.yml)
- MVP asset pack v1 integrated into Main.tscn
- Scene composition guides created (docs/art/, Phase 7 scene skeleton)
- Crash hull and all MVP assets modeled and textured

### Blocked - Awaiting Execution ⏳
- Stage A visual artifact generation (requires Blender runtime)
- Review PNG renders (requires Blender execution)
- Mesh statistics validation (requires Blender execution)
- Visual Reviewer approval verdict (requires PNGs and diagnosis)
- Production scene integration sign-off (requires visual approval)

### Out of Scope (Vision Features)
- Stage B multi-biome environments (post-Phase-6)
- Advanced lighting/volumetrics (post-Phase-6)
- Custom animation/cinematics (vision feature)
- Multiple regions (vision feature)

---

## Blocking Task Analysis

### BLOCKER #1: Stage A Review Artifacts Missing
**Status:** Awaiting execution  
**Prerequisite:** Blender runtime available  
**Solution Path:**
1. Execute `.github/workflows/stage-a-blender-asset-review.yml` in CI or locally
2. Generate review PNGs: front.png, back.png, left.png, right.png, top.png, hero_three_quarter.png, scale_reference.png, material_preview.png
3. Generate mesh_stats_report.md and contact_sheet.png
4. Store in `artifacts/asset-review/TC_TerrainDioramaKit_V1/`

**Evidence Required:**
- 9 PNG renders in artifacts/asset-review/
- mesh_stats_report.md with polygon counts and geometry validation
- contact_sheet.png showing all kit pieces

**Gate:** Stage A Artifact Generation Complete

### BLOCKER #2: Visual Reviewer Approval Missing
**Status:** Awaiting review artifacts  
**Prerequisite:** Review PNGs exist and opened  
**Solution Path:**
1. Visual Reviewer opens all PNG renders from artifacts/
2. Records visual diagnosis: focal point, route readability, silhouette, scale, material coherence
3. Issues verdict: PASS, NOT_GO, or INTENTIONAL_GATE with specific conditions
4. Documents verdict in docs/art/reviews/stage-a-visual-approval-verdict.md

**Evidence Required:**
- Opened PNG screenshots with visual diagnosis
- Independent review verdict (not auto-approved)
- Any conditional gates or required changes documented

**Gate:** Visual Reviewer Approval Verdict

### BLOCKER #3: Production Scene Integration Sign-Off Missing
**Status:** Awaiting visual approval  
**Prerequisite:** Visual Reviewer approval PASS  
**Solution Path:**
1. After visual approval, integrate approved Stage A assets into Main.tscn
2. Run visual regression tests (screenshot comparison)
3. Confirm gameplay collisions not degraded
4. Issue production integration verdict

**Evidence Required:**
- Before/after scene screenshots
- Collision validation (no route blocking)
- Integration test results

**Gate:** Production Scene Integration Approved

### BLOCKER #4: Phase 6 Release Readiness Gate Missing
**Status:** Awaiting phase 5 completion  
**Prerequisite:** All Phase 5 exports validated  
**Solution Path:**
1. Phase 5 (Windows playtest/export) must complete first
2. Validate export, offline launch, save/load, playtest evidence
3. Issue Phase 6 readiness verdict
4. Then proceed to public demo preparation

**Gate:** Phase 6 Release Readiness Approved

---

## Task Closure Sequence

### Immediate Tasks (This Session)
**Status:** Ready to close ✓

- [x] **Task:** Realign documentation to codebase
  - **Status:** COMPLETE (Commit 0f14944)
  - **Evidence:** README.md realigned, 23 discrepancies resolved
  - **Verdict:** PASS

- [x] **Task:** Create Phase 7 planning documentation
  - **Status:** COMPLETE (Commit e2cb24e, PR #111)
  - **Evidence:** Phase 7 planning doc with 5 workstreams, stage gates, success criteria
  - **Verdict:** PASS
  - **Action:** Merge PR #111

### Pre-Visual Approval Tasks
**Status:** Prerequisites for visual work ⏳

- [ ] **Task:** Generate Stage A review artifacts
  - **Owner:** Build Release Engineer / CI
  - **Prerequisite:** Blender runtime available
  - **Acceptance:** 9 PNGs + mesh_stats_report.md in artifacts/asset-review/
  - **Blocking:** Visual Reviewer Approval
  - **Estimated:** 1 task (automated CI execution)

- [ ] **Task:** Visual Reviewer approval of Stage A assets
  - **Owner:** Visual Reviewer (art_director or designated)
  - **Prerequisite:** Review PNGs exist and opened
  - **Acceptance:** Visual diagnosis + independent verdict (PASS/NOT_GO)
  - **Blocking:** Production Scene Integration
  - **Estimated:** 1 task (review + diagnosis + verdict)

- [ ] **Task:** Production scene integration and sign-off
  - **Owner:** Technical Director / Level Designer
  - **Prerequisite:** Visual approval PASS
  - **Acceptance:** Assets integrated, collisions validated, tests pass
  - **Blocking:** Phase 6 Completion
  - **Estimated:** 1-2 tasks (integration + regression testing)

### Phase 6 Tasks
**Status:** Follows visual approval ⏳

- [ ] **Task:** Phase 5 Windows playtest/export completion
  - **Owner:** Build Release Engineer
  - **Prerequisite:** Phase 4 completion
  - **Acceptance:** Windows export artifact, playtest evidence
  - **Blocking:** Phase 6
  - **Estimated:** 2-3 tasks

- [ ] **Task:** Phase 6 public demo preparation
  - **Owner:** Producer
  - **Prerequisite:** Phase 5 completion
  - **Acceptance:** Demo materials, release readiness verdict
  - **Blocking:** Visual completion
  - **Estimated:** 1-2 tasks

---

## Visual Completion Path

```
Current State (Phase 4-6 governance ready)
    ↓
PHASE 1: Generate Review Artifacts (CI execution)
    ├─ Execute stage-a-blender-asset-review.yml
    ├─ Collect 9 PNG renders + mesh stats
    └─ Store in artifacts/asset-review/
    ↓
PHASE 2: Visual Reviewer Approval (1 task)
    ├─ Open and diagnose PNGs
    ├─ Record visual assessment
    └─ Issue verdict: PASS / NOT_GO
    ↓
PHASE 3: Production Integration (1-2 tasks)
    ├─ Integrate approved assets into Main.tscn
    ├─ Validate collisions and route
    └─ Issue integration verdict
    ↓
PHASE 4: Phase 6 Release Gates (2-3 tasks)
    ├─ Phase 5: Playtest/export validation
    └─ Phase 6: Public demo materials
    ↓
VISUAL COMPLETION ✓
```

---

## Task Priority & Dependencies

| Task | Priority | Blocker | Duration | Owner |
|------|----------|---------|----------|-------|
| Generate Stage A review artifacts | CRITICAL | Visual Reviewer | 30 min | Build Release Engineer |
| Visual Reviewer approval | CRITICAL | Integration | 1-2 hrs | Visual Reviewer |
| Production scene integration | HIGH | Phase 6 | 1-2 hrs | Technical Director |
| Phase 5 playtest/export | HIGH | Phase 6 | 3-4 hrs | Build Release Engineer |
| Phase 6 demo preparation | MEDIUM | Release | 2-3 hrs | Producer |

---

## Evidence Gates Required Before Visual Completion Claim

### Gate 1: Artifact Generation
- [ ] 9 PNG renders exist in artifacts/asset-review/TC_TerrainDioramaKit_V1/
- [ ] mesh_stats_report.md documents polygon counts
- [ ] contact_sheet.png shows all kit geometry
- [ ] CI workflow execution log recorded

### Gate 2: Visual Approval
- [ ] All PNGs opened and reviewed (not cited, analyzed)
- [ ] Visual diagnosis recorded: focal point, route, silhouette, scale, materials
- [ ] Verdict is PASS (not vague approval)
- [ ] Independent review (not self-approved)

### Gate 3: Production Integration
- [ ] Assets imported into Main.tscn
- [ ] Before/after scene screenshots exist
- [ ] Route collisions not degraded
- [ ] Integration verdict recorded

### Gate 4: Phase 6 Completion
- [ ] Phase 5 (Windows export) validated
- [ ] Playtest evidence documented
- [ ] Release readiness gates pass
- [ ] Public demo materials ready

---

## Authority & Validation

**Source of Truth:** README.md § 5-6 (MVP scope), AGENTS.md § 2-3 (governance)  
**Stage Gates:** docs/production/definition-of-done.md, studio/memory/production_stage_gates.md  
**Visual Review:** studio/skills/screenshot_critique.md, studio/memory/visual_failure_patterns.md  

---

## Final Status

**Current Blocker:** Stage A review artifacts generation (awaiting Blender runtime)  
**Next Action:** Execute CI workflow to generate review PNGs  
**Visual Completion Timeline:** 1-2 weeks after artifact generation (pending review + integration)  
**Final Verdict Eligibility:** PASS (once all gates pass)

---

**Document Status:** TASK CLOSURE CHECKLIST (governance work)  
**Created:** 2026-07-05  
**Next Review:** Upon visual approval completion
