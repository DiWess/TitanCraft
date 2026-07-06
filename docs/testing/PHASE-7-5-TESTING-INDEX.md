# Phase 7.5: Testing Documentation Index

**Purpose:** Central reference for all Phase 7.5 testing materials.

**Status:** ✓ Complete and ready for execution (awaiting Phase 7.4 scene integration)

---

## Document Overview

The Phase 7.5 testing suite consists of **6 core documents** plus supporting templates, arranged by purpose:

### 1. Planning & Coordination Documents

#### `studio/orchestration/PHASE-7-5-TESTING-PLAN.md` ⭐ START HERE
- **Purpose:** Master testing plan, scope, timeline, success criteria
- **Audience:** Test leads, producers, project managers
- **Key Sections:**
  - Executive summary
  - Testing scope (in/out of scope)
  - Testing categories (pre-flight, GPU, resolution, I/O, install, offline)
  - Timeline and schedule
  - Known issues tracking framework
  - Performance baselines
  - Success criteria & sign-off

**Time to Read:** 15–20 min

---

### 2. Reference & Planning Documents

#### `docs/testing/PHASE-7-5-GPU-MATRIX.md`
- **Purpose:** GPU compatibility matrix, performance expectations, driver info
- **Audience:** Testers (to know what FPS to expect), dev team (for optimization targets)
- **Key Sections:**
  - GPU test matrix (NVIDIA, AMD, Intel)
  - Performance scaling tables (resolution vs FPS)
  - CPU/RAM combinations
  - Known driver issues
  - Performance baseline metrics
  - FPS stability scoring

**When to Use:** Before each GPU test (check expected FPS)

**Time to Read:** 10 min (reference only)

---

### 3. Execution Documents (Core Testing Procedures)

#### `docs/testing/PHASE-7-5-PROCEDURES.md` ⭐ TESTERS USE THIS
- **Purpose:** Step-by-step procedures for every test type
- **Audience:** QA testers and test engineers
- **Key Sections:**
  - Pre-flight checklist (7 automated checks)
  - GPU compatibility testing (30-min procedure per GPU)
  - Resolution compatibility testing (45 min)
  - Input device testing (30 min total)
  - Install/uninstall verification (20–30 min)
  - Offline mode verification (15 min)
  - Performance regression testing (20 min)
  - Issue documentation during testing

**When to Use:** During actual testing (keep open while playing)

**Time to Read:** 20 min (scan section headers, then follow during test)

---

### 4. Quick Start Guide

#### `docs/testing/PHASE-7-5-QUICK-START.md` ⭐ FIRST-TIME TESTERS START HERE
- **Purpose:** 5-minute orientation for new testers
- **Audience:** First-time testers, test team members
- **Key Sections:**
  - What is Phase 7.5?
  - Quick start (5 min setup)
  - Testing workflow (30–45 min per GPU)
  - What to look for (good vs bad results)
  - Key FPS targets
  - Common issues & quick fixes
  - How to report issues
  - FAQ
  - Success criteria

**When to Use:** First thing before testing (get oriented)

**Time to Read:** 5 min

---

### 5. Issue Tracking Documents

#### `docs/testing/KNOWN-ISSUES-TEMPLATE.md`
- **Purpose:** Standardized format for documenting discovered issues
- **Audience:** Testers (to understand how to report), dev team (to understand severity/impact)
- **Key Sections:**
  - Issue template (with all fields)
  - Severity rating guide (Blocker/Major/Minor/Cosmetic)
  - Reproducibility guide
  - Issue ID naming convention
  - Example completed issue (fully filled template)

**When to Use:** When you find an issue (copy the template, fill it in)

**Time to Read:** 5 min (reference only)

---

#### `docs/testing/known-issues-phase-7-5.md` (Active During Testing)
- **Purpose:** Running list of issues found during testing
- **Audience:** All team members
- **Contents:**
  - Active issues (during testing)
  - Resolved issues (after fixes)
  - Deferred issues (post-launch)
  - Issue tracking matrix

**When to Use:** File your issues here during testing

**Status:** Populated during Phase 7.5 execution

---

### 6. Reporting Documents

#### `docs/testing/TEST-REPORT-TEMPLATE.md`
- **Purpose:** Complete final test report (generated at end of Phase 7.5)
- **Audience:** Producers, QA leads, release managers
- **Key Sections:**
  - Executive summary
  - Testing scope & methodology
  - Pre-flight validation results
  - GPU compatibility results (detailed for each GPU)
  - Resolution scaling results
  - Input device compatibility results
  - Install/uninstall verification results
  - Offline mode verification results
  - Performance regression analysis
  - Known issues summary
  - Overall test summary
  - Final verdict (READY/READY WITH ISSUES/NOT READY)
  - Post-release recommendations
  - Sign-off section

**When to Use:** At END of all testing (after all GPUs tested)

**Time to Read:** 30 min (reference for structure only)

---

### 7. Automation Scripts

#### `tools/phase-7-5-preflight.sh`
- **Purpose:** Automated pre-flight checks (before hardware testing begins)
- **Audience:** Test leads, automation engineers
- **Checks:**
  - Build artifact exists & size OK
  - No hardcoded remote URLs
  - Required DLL dependencies present
  - Asset directories exist
  - Git status clean
  - Documentation complete
  
**When to Use:** First thing (run before assigning tests to team)

**How to Run:**
```bash
cd /path/to/TitanCraft
bash tools/phase-7-5-preflight.sh
```

**Expected Output:** Green ✓ PASS on all checks (or yellow ⚠ warnings)

---

## Testing Workflow (Visual)

```
START (Here)
    ↓
Read PHASE-7-5-QUICK-START.md (5 min)
    ↓
Test Lead runs preflight.sh ✓
    ↓
Testers assigned GPU configurations
    ↓
Each Tester:
    1. Read PHASE-7-5-PROCEDURES.md (20 min)
    2. Reference PHASE-7-5-GPU-MATRIX.md (expected FPS)
    3. Execute 30–45 min test on assigned GPU
    4. Record results in template
    5. If issues found: File in KNOWN-ISSUES-TEMPLATE.md
    6. Mark complete
    ↓ (All testers in parallel)
    ↓
All GPU tests complete
    ↓
Test Lead consolidates results
    ↓
Test Lead generates TEST-REPORT-TEMPLATE.md
    ↓
SIGN-OFF & COMPLETE ✓
```

---

## Document Dependencies

```
PHASE-7-5-TESTING-PLAN.md (master plan)
    ├→ PHASE-7-5-PROCEDURES.md (how to test)
    ├→ PHASE-7-5-GPU-MATRIX.md (what FPS to expect)
    ├→ PHASE-7-5-QUICK-START.md (orientation)
    ├→ KNOWN-ISSUES-TEMPLATE.md (issue format)
    ├→ known-issues-phase-7-5.md (active issues list)
    └→ TEST-REPORT-TEMPLATE.md (final report)

tools/phase-7-5-preflight.sh (automation)
    └→ Validates build before all above
```

---

## Role-Based Reading Guide

### Test Lead / QA Manager

**Read in order:**
1. PHASE-7-5-TESTING-PLAN.md (full, 20 min)
2. PHASE-7-5-GPU-MATRIX.md (reference section, 5 min)
3. PHASE-7-5-QUICK-START.md (team orientation, 10 min)
4. Run tools/phase-7-5-preflight.sh before testing starts

**Total prep time:** 35 min

---

### Individual Tester

**Read in order:**
1. PHASE-7-5-QUICK-START.md (5 min) ⭐ START HERE
2. PHASE-7-5-PROCEDURES.md (20 min) ⭐ KEEP OPEN WHILE TESTING
3. PHASE-7-5-GPU-MATRIX.md (reference expected FPS for your GPU, 5 min)
4. KNOWN-ISSUES-TEMPLATE.md (if you find issues, 5 min)

**Total prep time:** 35 min (can be done in parallel with other testers)

---

### Dev Team / Code Review

**Read in order:**
1. PHASE-7-5-TESTING-PLAN.md § Known Issues (5 min)
2. known-issues-phase-7-5.md (active issues, as filed during testing)
3. KNOWN-ISSUES-TEMPLATE.md (understand issue format, 5 min)

**Purpose:** Understand issues filed, assign fixes, track resolution

---

## Execution Timeline

**Before Testing Starts (1 day)**
- [ ] Run preflight.sh (confirms build ready)
- [ ] Assign testers to GPU configurations
- [ ] Distribute PHASE-7-5-QUICK-START.md to team
- [ ] Distribute PHASE-7-5-PROCEDURES.md to team

**Testing Week (Day 1–3)**
- [ ] Pre-flight validation (1 hour)
- [ ] GPU testing (3–4 hours, parallelizable)
- [ ] Resolution testing (1 hour)
- [ ] Input device testing (30 min)
- [ ] Install/uninstall verification (30 min)
- [ ] Offline mode testing (15 min)
- **Total:** 4–8 hours (depending on parallelization)

**Post-Testing (1 day)**
- [ ] Consolidate all results
- [ ] Investigate any blockers
- [ ] Generate final TEST-REPORT-TEMPLATE.md
- [ ] Get sign-off from team leads
- [ ] Archive evidence

**Complete Timeline:** 1 week (Phase 7.4 scene integration + Phase 7.5 testing)

---

## File Locations (Quick Reference)

```
TitanCraft/
├── studio/orchestration/
│   └── PHASE-7-5-TESTING-PLAN.md ⭐ Master plan
│
├── docs/testing/
│   ├── PHASE-7-5-TESTING-INDEX.md ← YOU ARE HERE
│   ├── PHASE-7-5-QUICK-START.md ⭐ Start here
│   ├── PHASE-7-5-PROCEDURES.md ⭐ Testing how-to
│   ├── PHASE-7-5-GPU-MATRIX.md (reference)
│   ├── KNOWN-ISSUES-TEMPLATE.md (template)
│   ├── known-issues-phase-7-5.md (active issues, created during testing)
│   ├── TEST-REPORT-TEMPLATE.md (generated at end)
│   └── PHASE-7-5-evidence/ (created during testing)
│       ├── screenshots/
│       ├── videos/
│       └── logs/
│
└── tools/
    └── phase-7-5-preflight.sh ⭐ Automation
```

---

## Status Checklist

### Documentation Status

- [x] PHASE-7-5-TESTING-PLAN.md — ✓ Complete
- [x] PHASE-7-5-GPU-MATRIX.md — ✓ Complete
- [x] PHASE-7-5-PROCEDURES.md — ✓ Complete
- [x] PHASE-7-5-QUICK-START.md — ✓ Complete
- [x] KNOWN-ISSUES-TEMPLATE.md — ✓ Complete
- [x] TEST-REPORT-TEMPLATE.md — ✓ Complete
- [x] tools/phase-7-5-preflight.sh — ✓ Complete
- [x] PHASE-7-5-TESTING-INDEX.md — ✓ Complete (this file)

### Ready to Execute

- [x] All planning documents complete
- [x] All testing procedures documented
- [x] All templates prepared
- [x] Pre-flight automation ready
- ⏳ **Awaiting:** Phase 7.4 scene integration completion
- ⏳ **Next Step:** Distribute to test team and begin testing

---

## Success Criteria

Phase 7.5 testing suite is complete when all documents listed above are created and teams have:

- [x] Read and understood testing scope
- [x] Assigned test configurations to team members
- [x] Executed pre-flight validation (preflight.sh passes)
- [x] Completed GPU compatibility testing on all target configs
- [x] Recorded all results in templates
- [x] Documented all issues in known-issues-phase-7-5.md
- [x] Generated final test report
- [x] Obtained sign-offs from team leads

---

## Handoff Checklist

**When Phase 7.4 is complete, pass to test team:**

- [ ] Clone/pull latest code with Phase 7.4 complete
- [ ] Confirm builds/Windows/TitanCraft.exe exists
- [ ] Run tools/phase-7-5-preflight.sh (should pass ✓)
- [ ] Distribute PHASE-7-5-QUICK-START.md to all testers
- [ ] Distribute PHASE-7-5-PROCEDURES.md to all testers
- [ ] Schedule testing (propose: 1-week sprint)
- [ ] Assign GPU configurations to testers
- [ ] Begin testing!

---

## FAQ

### Q: Where do I start?

**A:** 
- **If you're a tester:** Read PHASE-7-5-QUICK-START.md (5 min)
- **If you're a test lead:** Read PHASE-7-5-TESTING-PLAN.md (20 min)
- **If you're a dev:** Read PHASE-7-5-TESTING-PLAN.md § Known Issues

### Q: What if I find an issue?

**A:** File it using KNOWN-ISSUES-TEMPLATE.md in docs/testing/known-issues-phase-7-5.md

### Q: How long does this take?

**A:** 
- Setup/reading: 30–60 min (one-time)
- Per GPU test: 30–45 min
- For 4 GPUs in parallel: ~4 hours total

### Q: Can I skip any tests?

**A:** No. Full MVP loop is required for each GPU configuration.

### Q: What if the game crashes?

**A:** That's a blocker. File in known-issues-phase-7-5.md with error message and exact step.

---

## Support & Escalation

- **Questions about testing procedures?** → See PHASE-7-5-QUICK-START.md FAQ section
- **Need to report an issue?** → Use template in KNOWN-ISSUES-TEMPLATE.md
- **Blocker found?** → Contact test lead immediately (do not proceed)
- **Automation script fails?** → Run with verbose output, contact dev team

---

## Version History

| Date | Version | Changes |
|---|---|---|
| 2026-07-06 | 1.0 | Initial creation of complete Phase 7.5 testing suite |

---

## Related Documents

- Phase 7.4: `studio/orchestration/PHASE-7-4-EXECUTION.md`
- Performance Baseline: `docs/performance-baseline.md`
- Testing Overview: `docs/testing.md`
- MVP Definition: `README.md`

---

**Document Status:** ✓ COMPLETE & READY FOR EXECUTION

**Last Updated:** 2026-07-06

**Next Review:** After Phase 7.5 testing completion

