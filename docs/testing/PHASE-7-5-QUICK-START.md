# Phase 7.5: Quick Start Guide for Test Teams

**Purpose:** Get started with Phase 7.5 testing in 5 minutes.

**Target Audience:** QA engineers, test leads, hardware testers

---

## What Is Phase 7.5?

Phase 7.5 is **Platform Testing & Compatibility Validation** — the final validation before TitanCraft MVP release. We verify the Windows build works across different GPU manufacturers, resolutions, input devices, and configurations.

**Timeline:** ~4–8 hours total (can be parallelized across 3–4 test machines)

---

## Quick Start (5 Min Setup)

### Step 1: Understand Your Role

**Your job:** Test the game on your assigned hardware configuration and report results.

**You are NOT:**
- Fixing code (that's the dev team)
- Making design decisions
- Writing detailed logs (we provide templates)

**You ARE:**
- Running the game through the full MVP loop
- Measuring FPS at key moments
- Documenting any issues you find
- Reporting pass/fail status

### Step 2: Get the Files

**You need these three documents:**

1. **Main Testing Plan:** `studio/orchestration/PHASE-7-5-TESTING-PLAN.md`
   - Overall scope, timelines, success criteria

2. **Your Test Procedures:** `docs/testing/PHASE-7-5-PROCEDURES.md`
   - Step-by-step how to run each test
   - Copy-paste friendly checklists
   - What FPS numbers to expect

3. **Hardware Reference:** `docs/testing/PHASE-7-5-GPU-MATRIX.md`
   - What GPUs are being tested
   - Expected performance by GPU tier
   - Driver version requirements

### Step 3: Know Your Hardware

**Before you start, know:**

- [ ] My GPU model (e.g., RTX 3060 Ti, RX 6700 XT, UHD 630)
- [ ] VRAM amount (e.g., 8GB)
- [ ] GPU driver version (Settings → Device Manager → Display Adapters, right-click → Properties)
- [ ] Windows version (Settings → System → About)
- [ ] Monitor resolution (Settings → Display → Display Resolution)

**Record this:** Use the template in PHASE-7-5-PROCEDURES.md § "Recording FPS Data"

---

## Testing Workflow (30–45 Min Per GPU)

### Phase 1: Pre-Test Checklist (5 Min)

**Before you launch the game:**

- [ ] Close Discord, Twitch, YouTube (disable overlays)
- [ ] Disable any background recording/monitoring software
- [ ] Set monitor to 60 Hz refresh rate
- [ ] Launch TitanCraft.exe

**Expected:** Game starts in <5 seconds, shows main menu at 55+ FPS

### Phase 2: Play Through Full MVP Loop (25 Min)

**Follow this exact sequence (from PHASE-7-5-PROCEDURES.md):**

1. **Launch & Menu (2 min)**
   - Start new game
   - Note menu FPS _____ (should be 55+)

2. **Exploration & Collection (10 min)**
   - Walk around, collect resources (metal, biomass, electronics)
   - Note exploration FPS _____ (should be 50+)

3. **Crafting (5 min)**
   - Approach workbench
   - Build mechanical arm
   - Note crafting FPS _____ (should be 50+)

4. **Combat (8 min)**
   - Trigger scout enemy (move toward spawn)
   - Fight the scout
   - Note combat FPS _____ (should be 40+)

5. **Victory (1 min)**
   - Activate beacon
   - See victory screen
   - Note victory FPS _____ (should be 55+)

### Phase 3: Record Results (5 Min)

**Fill in this template:**

```
GPU: ________________________
Driver: ____________________
OS: ________________________

Menu FPS: _________ ✓ or ✗
Exploration FPS: _________ ✓ or ✗
Combat FPS: _________ ✓ or ✗
Crafting FPS: _________ ✓ or ✗
Victory FPS: _________ ✓ or ✗

Issues Found: [NONE / List below]
- Issue 1: _______________
- Issue 2: _______________

Overall Result: ✓ PASS / ✗ FAIL
```

**Where to submit:** `docs/testing/known-issues-phase-7-5.md` (if issues found)

---

## What To Look For

### Good Results (PASS)

- ✓ Game launches without crashes
- ✓ FPS meets or exceeds targets (see PHASE-7-5-GPU-MATRIX.md)
- ✓ Game is playable (not stuttering, not freezing)
- ✓ Can complete full MVP loop without errors
- ✓ Controls are responsive

### Bad Results (FAIL or ISSUE)

- ✗ Crashes on startup or during gameplay
- ✗ FPS significantly below target (see matrix for acceptable ranges)
- ✗ Severe stuttering or frame drops (normal: 55–60 FPS, bad: <30 FPS)
- ✗ Game freezes for >1 second
- ✗ Controls lag or don't respond
- ✗ Save/load doesn't work
- ✗ Any error messages

---

## Key FPS Targets (By GPU Tier)

**Reference:** NVIDIA RTX 3060 Ti @ 1920×1080

| Scene | Target | Acceptable | Poor |
|---|---|---|---|
| Menu | 60 FPS | 55+ | <50 |
| Exploration | 60 FPS | 50+ | <40 |
| Combat | 50 FPS | 40+ | <30 |

**Note:** Your GPU may be faster or slower than the reference. Use the matrix to see what's expected for YOUR GPU.

---

## Common Issues & Quick Fixes

### "Game won't launch"
- [ ] Verify .exe file exists (builds/Windows/TitanCraft.exe)
- [ ] Update GPU driver to latest WHQL version
- [ ] Check Windows 10/11 is up to date (Settings → Update)
- [ ] Disable Discord/GeForce overlay (if running)
- [ ] Report: File → known-issues-phase-7-5.md with error message

### "FPS is low (30–40 when I expect 60)"
- [ ] Check your GPU model is in the matrix (if not, expected to be lower)
- [ ] Close any background apps (Discord, Chrome, etc.)
- [ ] Verify resolution is set correctly (1080p vs 1440p)
- [ ] Check GPU driver version matches recommended version
- [ ] If expected 60 but getting 30: THIS IS AN ISSUE → Report it

### "Game is stuttering/freezing"
- [ ] Note the exact moment (menu, combat, exploration?)
- [ ] Try restarting game to see if reproducible
- [ ] Report with description in known-issues-phase-7-5.md

### "Controls don't respond"
- [ ] Verify your input device is connected
- [ ] Try a different input device (keyboard instead of controller)
- [ ] Restart game
- [ ] Report the input device type and issue

---

## How to Report Issues

### Option A: Simple Report (Recommended)

**File:** `docs/testing/known-issues-phase-7-5.md`

Add a new section:

```markdown
## Issue Phase7.5-[YOUR-GPU]-XXX: [Title]

**GPU:** [Your GPU model]
**Reproducibility:** Always / Often / Sometimes
**Severity:** Blocker / Major / Minor / Cosmetic

### Steps to Reproduce
1. ...
2. ...

### What Happened
[Description]

### Expected
[What should have happened]

### Screenshot/Video
[Attach or describe]
```

### Option B: Detailed Report

See the full template in `docs/testing/KNOWN-ISSUES-TEMPLATE.md`

---

## Testing Checklist

Use this to track your progress:

### Pre-Flight (Do Once, First Thing)

- [ ] Run: `bash tools/phase-7-5-preflight.sh`
- [ ] All checks pass? (should see green ✓)
- [ ] If fails: Stop, contact dev team

### GPU Tests (Repeat For Each GPU)

- [ ] Deploy TitanCraft.exe to test machine
- [ ] Document hardware specs (GPU, driver, OS)
- [ ] Run Phase 2 (Play through full MVP loop)
- [ ] Record results in template
- [ ] Any issues? File report in known-issues-phase-7-5.md
- [ ] Mark as complete

### After All Tests

- [ ] Consolidate all results
- [ ] Generate final test report (use template)
- [ ] Get sign-off from test lead
- [ ] Archive evidence (screenshots, videos if applicable)

---

## Timing Estimate

**For ONE GPU test:**

- Setup: 5 min
- Pre-flight check: 5 min
- Test execution (full loop): 30 min
- Results recording: 5 min
- **Total: 45 min per GPU**

**For 4 GPUs in parallel (recommended):**

- 4 testers × 45 min = 180 min total (3 hours)
- Plus resolution testing (1 hour) = **4 hours total**

---

## FAQ

### Q: What if I get a different FPS than the matrix says?

**A:** It's normal. The matrix shows expected ranges. As long as you're within ±10% and the game is playable, it's fine. If significantly lower, file an issue.

### Q: Can I test multiple GPUs on the same machine?

**A:** Only if you physically swap GPUs or have multiple GPU slots. If you only have one GPU, you test only that GPU.

### Q: What if the game crashes? Does that mean it fails?

**A:** Yes, crashes are blockers. Report immediately with exact error message.

### Q: How long does the full MVP loop take?

**A:** 10–15 minutes of actual gameplay (plus setup/recording time). We provide a checklist so you don't need to memorize it.

### Q: What if my FPS is lower than the matrix predicts?

**A:** Check:
1. GPU driver is up to date (recommended version in matrix)
2. No background apps running
3. Resolution is correct (1080p vs 1440p)
4. If still lower: This is an issue → Report it

---

## Getting Help

### I'm Stuck On...

| Problem | Solution |
|---|---|
| "Where's the exe?" | `builds/Windows/TitanCraft.exe` (after build completes) |
| "What FPS should I see?" | Check `docs/testing/PHASE-7-5-GPU-MATRIX.md` for your GPU |
| "How do I report an issue?" | Use template in `docs/testing/KNOWN-ISSUES-TEMPLATE.md` |
| "Is stuttering bad?" | If <30 FPS, yes. If 50+ FPS with occasional dips, acceptable |
| "Can I skip any steps?" | No. Full MVP loop required for each GPU. |

### Contact

- **Test Lead:** [Name, contact]
- **Dev Team (for crashes):** [Contact]
- **Documentation:** See files listed in "Step 2" above

---

## Success Criteria

Your testing is complete when:

- [x] All assigned GPUs tested (full MVP loop each)
- [x] Results recorded in template
- [x] Any issues filed in known-issues-phase-7-5.md
- [x] Final report generated
- [x] Test lead has signed off

**Expected timeline:** 4–8 hours total (can be done in parallel with other testers)

---

## Key Documents (Bookmark These)

| Document | Purpose | Location |
|---|---|---|
| **Testing Plan** | Overall scope & timeline | `studio/orchestration/PHASE-7-5-TESTING-PLAN.md` |
| **GPU Matrix** | Expected FPS by GPU | `docs/testing/PHASE-7-5-GPU-MATRIX.md` |
| **Test Procedures** | Step-by-step how-to | `docs/testing/PHASE-7-5-PROCEDURES.md` |
| **Issue Template** | How to report issues | `docs/testing/KNOWN-ISSUES-TEMPLATE.md` |
| **Known Issues** | Active issues list | `docs/testing/known-issues-phase-7-5.md` |

---

## Next Step

1. Get your assigned GPU configuration from test lead
2. Read PHASE-7-5-PROCEDURES.md completely
3. Run pre-flight script: `bash tools/phase-7-5-preflight.sh`
4. Deploy TitanCraft.exe to your test machine
5. Start testing!

**Questions?** Check the FAQ above or contact your test lead.

---

**Good luck! We're counting on you.** ✓

