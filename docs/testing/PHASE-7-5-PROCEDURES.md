# Phase 7.5: Detailed Testing Procedures

**Purpose:** Step-by-step procedures for executing Phase 7.5 tests.

**Audience:** Test engineers, quality assurance team  
**Duration:** ~4–8 hours total across all test configurations  

---

## Pre-Flight Checklist (Before Hardware Testing)

**Time Required:** 30–45 minutes (once)

### Step 1: Verify Build Artifact

```bash
# On development machine (before packaging for distribution)
cd TitanCraft
ls -lah builds/Windows/TitanCraft.exe

# Expected output:
# -rwxr-xr-x  1 user group  XXMB  timestamp  builds/Windows/TitanCraft.exe
```

**Pass Criteria:**
- [x] File exists and is >10 MB (contains executable + assets)
- [x] Timestamp is recent (within last build)
- [x] File is readable

### Step 2: Verify No Hardcoded Remote URLs

```bash
# Check for suspicious network strings
strings builds/Windows/TitanCraft.exe | grep -iE "(http|https|api|telemetry|cloud)"

# Expected output: (empty or only expected URLs like Godot docs links)
```

**Pass Criteria:**
- [x] No unauthorized remote URLs
- [x] No embedded API keys
- [x] No cloud service references

### Step 3: Verify DLL Dependencies (Windows)

On a test Windows machine:
```powershell
# Launch dependency checker
C:\Windows\System32\dumpbin.exe /depends builds\Windows\TitanCraft.exe

# Or use Dependencies (open-source tool):
# Download from: https://github.com/lucasg/Dependencies/releases
Dependencies.exe builds\Windows\TitanCraft.exe
```

**Pass Criteria:**
- [x] All required DLLs present in build directory
- [x] No "missing DLL" warnings
- [x] Expected DLLs: msvcrt.dll, kernel32.dll, ntdll.dll, godot runtime DLLs

### Step 4: Verify Save File Location (Code Check)

```csharp
// In src/SaveSystem/SaveManager.cs or equivalent, verify:
// Save path should use: Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
// NOT: hardcoded absolute paths or network shares
```

**Pass Criteria:**
- [x] Save files use %APPDATA%\TitanCraft or equivalent
- [x] Save files are NOT in \Program Files\ (immutable on modern Windows)
- [x] Save files are NOT on network shares

### Step 5: Document Baseline Specs

Create file: `docs/testing/phase-7-5-baseline-specs.txt`

```
Test Machine Baseline:
CPU: [Model, Gen, Core/Thread count]
RAM: [Capacity, Type, Speed]
GPU: [Model, VRAM]
OS: [Windows version, build number, patches applied]
Display: [Resolution, refresh rate]
Monitor DPI: [standard or high-DPI]
```

**Pass Criteria:**
- [x] All specs documented
- [x] Baseline matches Phase 7.4 testing machine (if applicable)

---

## GPU Compatibility Testing Procedure

**Time Required Per GPU:** 30–45 minutes

### Hardware Setup

**Equipment Needed:**
- Target GPU installed in Windows 10/11 machine
- Latest GPU driver installed
- FPS counter available (Afterburner, GeForce Experience overlay, or in-game counter)
- Optional: Screen recording software (OBS, NVIDIA ShadowPlay)

### Test Procedure: Full MVP Loop @ 1080p

**Duration:** 30 minutes per GPU

#### Phase 1: Launch & Baseline (5 min)

1. Close all background applications (disable Discord, Steam overlay, etc.)
2. Set display to 1920×1080, 60 Hz
3. Launch `TitanCraft.exe`
4. Note FPS in main menu (should be 60)
5. Take screenshot of menu FPS counter
6. **Pass Criteria:** Game launches without errors, FPS counter visible, 55+ FPS idle

#### Phase 2: Exploration & Collection (10 min)

1. Start new game ("New Game" button)
2. Spawn at starting location
3. Walk around spawn area for 1 minute, note FPS:
   - Standing still: _______ FPS
   - Walking: _______ FPS
   - Camera look: _______ FPS
4. Collect 3–4 metal resources (walk to each, interact, watch FPS)
5. Collect 2–3 biomass resources (same as above)
6. Collect 2–3 electronics resources (same as above)
7. Return to workbench area
8. **Pass Criteria:** FPS stays ≥45 during exploration, no stuttering

#### Phase 3: Crafting & Workbench (5 min)

1. Approach workbench
2. Open workbench UI (interact key)
3. Verify inventory shows collected resources
4. Craft mechanical arm (verify cost in resources met)
5. Confirm crafting starts
6. Note FPS while crafting UI is open
7. **Pass Criteria:** FPS ≥50 during UI interaction, crafting succeeds

#### Phase 4: Combat & Enemy Encounter (8 min)

1. Exit workbench area and move toward enemy spawn zone
2. Approach scout (may need to trigger via proximity)
3. When scout detects player (alert audio cues):
   - Note FPS _______ (detection range, idle scout)
4. Scout begins chasing player
   - Note FPS _______ (chase state)
5. Get within attack range (red zone in HUD or game indication)
   - Note FPS _______ (combat state)
6. Attack scout 3–4 times with mechanical arm
   - Note FPS _______ (attack animation)
7. Move to maintain distance, repeat until scout defeated
   - Note FPS _______ (sustained combat)
8. **Pass Criteria:** FPS ≥40 during combat, no freeze frames, combat mechanics responsive

#### Phase 5: Victory & Exit (2 min)

1. After scout defeated, approach beacon
2. Interact with beacon to activate ("activate" or "use")
3. Confirm victory screen appears
4. Note FPS on victory screen
5. Exit to main menu
6. **Pass Criteria:** Victory screen reaches, FPS ≥55, clean exit

### Recording FPS Data

**Fill in Test Result Sheet:**

```
GPU Model: _____________________
Test Date: _____________________
Driver Version: _________________
OS: ____________________________

PHASE 1 (MENU):
  Menu FPS: _________ (Target: 55+)

PHASE 2 (EXPLORATION):
  Idle FPS: _________ (Target: 55+)
  Walking FPS: _______ (Target: 50+)
  Camera Movement FPS: _______ (Target: 50+)
  Average: _________ (Target: 50+)

PHASE 3 (CRAFTING):
  Workbench UI FPS: _________ (Target: 50+)
  Crafting FPS: _________ (Target: 50+)

PHASE 4 (COMBAT):
  Scout Detection FPS: _________ (Target: 55+)
  Chase FPS: _________ (Target: 50+)
  Close Combat FPS: _________ (Target: 40+)
  Minimum FPS During Combat: _________ (Target: 35+)

PHASE 5 (VICTORY):
  Victory Screen FPS: _________ (Target: 55+)

OVERALL RESULT: [ ] PASS  [ ] FAIL  [ ] ISSUES FOUND

Issues Encountered:
_________________________________________________________________

Workarounds Applied:
_________________________________________________________________

Notes:
_________________________________________________________________
```

### Repeat for Each GPU

If testing multiple GPUs (recommended: 2–4 representative models):
1. Uninstall current GPU driver
2. Install next GPU driver
3. Repeat full procedure
4. Compare results across GPUs

---

## Resolution Compatibility Testing

**Time Required:** 45 minutes (one GPU, all resolutions)

### Procedure: Test 1080p, 1440p, 4K on Same Machine

#### Setup

1. Have Phase 4 Combat test results @ 1080p ready
2. Godot may require editing resolution in `project.godot` or in-game settings
3. If in-game settings exist, use those; otherwise modify project.godot

#### Test Sequence

**For Each Resolution (1920×1080, 2560×1440, 3840×2160):**

1. Set system display resolution (or game resolution if in-game setting available)
2. Launch game
3. Perform abbreviated combat test (2 min):
   - Start new game
   - Quick resource collection (30 sec)
   - Trigger scout encounter
   - Engage in 1 min of combat
   - Note average FPS
4. Record results

**Result Sheet:**

```
GPU: ________________________
Resolution Testing Results:

1920×1080:
  Avg FPS: _________ (Target: 50+)
  Min FPS: _________ 
  Status: [ ] PASS [ ] ACCEPTABLE [ ] FAIL

2560×1440:
  Avg FPS: _________ (Target: 45+)
  Min FPS: _________
  Status: [ ] PASS [ ] ACCEPTABLE [ ] FAIL

3840×2160:
  Avg FPS: _________ (Target: 30+)
  Min FPS: _________
  Status: [ ] PASS [ ] ACCEPTABLE [ ] FAIL
```

**Pass Criteria:**
- [x] 1080p: ≥50 FPS average
- [x] 1440p: ≥45 FPS average (10% drop acceptable)
- [x] 4K: ≥30 FPS average (can expect 50% drop)

---

## Input Device Testing

**Time Required:** 30 minutes total (all devices sequential)

### Test 1: Keyboard + Mouse (Primary)

1. Launch game with keyboard + mouse plugged in
2. Complete abbreviated combat flow:
   - Walk around using WASD keys
   - Look around using mouse
   - Jump using SPACE
   - Attack using Mouse Click / Left Button
   - Reload using R key
3. Open pause menu using ESC key
4. Navigate menu using ARROW KEYS
5. Exit using ESC or menu button

**Pass Criteria:**
- [x] All keys responsive (no input lag >50ms)
- [x] Mouse look smooth (no acceleration artifacts)
- [x] Menu navigation works
- [x] Combat commands execute immediately

**Result:**
```
Keyboard + Mouse Test: [ ] PASS [ ] FAIL
Issues:
_______________________________________________________
```

### Test 2: Xbox Controller

1. Connect Xbox controller (Xbox One or Xbox Series X|S controller)
2. Verify Windows detects controller (Settings → Devices → Controllers)
3. Launch game
4. Complete abbreviated combat flow using controller:
   - Move using Left Stick
   - Look around using Right Stick
   - Jump using Button A (Xbox)
   - Attack using Button X or Right Trigger
   - Reload using Button Y or Left Trigger
5. Pause and navigate menu using button Y or START button

**Pass Criteria:**
- [x] Controller is detected
- [x] All inputs responsive
- [x] Analog sticks smooth (no dead zone issues)
- [x] Menu navigation works with D-Pad or right stick

**Result:**
```
Xbox Controller Test: [ ] PASS [ ] FAIL
Issues:
_______________________________________________________
```

### Test 3: Generic Gamepad (Optional)

If testing a non-Xbox controller (e.g., 8BitDo, third-party brand):

1. Connect generic gamepad
2. Verify Windows detects it (Settings → Devices → Controllers)
3. Perform 5-minute test:
   - Movement on left stick
   - Camera on right stick
   - Basic combat (attack button)
   - Menu navigation
4. Document controller model and any compatibility issues

**Result:**
```
Generic Gamepad Test (Model: _________): [ ] PASS [ ] FAIL
Issues:
_______________________________________________________
```

---

## Install/Uninstall Verification

**Time Required:** 20–30 minutes

### Prerequisite: Prepare Installer

- Have Windows installer (.exe or .msi) ready
- Installer should NOT be on the same machine (simulate download)

### Fresh Install Test

1. Use clean Windows machine or fresh user account
2. Close all applications
3. Download/locate installer
4. Run installer (accept UAC prompt if required)
5. Accept EULA if present
6. Choose install location (note where game will be installed: `_________________`)
7. Complete installation
8. Verify Start Menu shortcut created
9. Verify Desktop shortcut created (if installer offers)
10. Launch game from Start Menu shortcut
11. Play through full MVP loop:
    - Start new game
    - Collect resources (~2 min)
    - Craft arm (~1 min)
    - Trigger and defeat scout (~3 min)
    - Activate beacon and finish (~1 min)
12. Save game
13. Close game normally
14. Verify save file exists in expected location:
    - Path: `%APPDATA%\TitanCraft\` (or configured save path)
    - File should contain recent timestamp

**Pass Criteria Install:**
- [x] Installer runs without errors
- [x] Game launches after install
- [x] Full MVP loop completable
- [x] Save file created in expected location
- [x] No pop-up errors or warnings

### Uninstall Test

1. Open Windows Settings → Apps → Apps & features
2. Find "TitanCraft" in list
3. Click "Uninstall"
4. Follow uninstaller prompts
5. Confirm removal when prompted
6. Verify game folder is removed from install location
7. Verify Start Menu shortcut no longer exists
8. Verify save files still exist (NOT deleted):
   - Check: `%APPDATA%\TitanCraft\` directory should still exist with save data

**Pass Criteria Uninstall:**
- [x] Uninstaller completes without errors
- [x] Game files removed from install location
- [x] Start Menu shortcut removed
- [x] Save files preserved in AppData
- [x] No stray files left behind

### Re-Install Test

1. Run installer again (on same machine)
2. Install to same location
3. Launch game, verify old save still loads
4. Verify no conflicts or errors

**Pass Criteria Re-Install:**
- [x] Second install succeeds
- [x] Old saves still accessible
- [x] No "file already exists" errors

**Result Sheet:**
```
Install/Uninstall Test:

Fresh Install: [ ] PASS [ ] FAIL
Uninstall: [ ] PASS [ ] FAIL
Re-Install: [ ] PASS [ ] FAIL

Save File Location (Verified): _______________________
Orphaned Files Found (if any): _______________________
Issues Encountered:
_________________________________________________________
```

---

## Offline Mode Verification

**Time Required:** 15 minutes

### Network Isolation Test

1. **Disable Network:**
   - Right-click network icon in system tray
   - Select "Network & Internet settings"
   - Or: Settings → Network & Internet → Status
   - Disable network adapter or disconnect from WiFi
   - Verify no network connectivity: Open browser, should fail to load page

2. **Launch Game Offline:**
   - Start TitanCraft.exe with network disabled
   - Note startup time (should NOT increase significantly)
   - Verify no "checking connection..." messages

3. **Play Through MVP Loop:**
   - Complete abbreviated 5-minute game loop
   - Play, save game, exit
   - Verify no network-related error messages

4. **Re-Enable Network:**
   - Restore network connectivity

**Pass Criteria:**
- [x] Game launches within normal startup time (< 5 sec)
- [x] No "network unavailable" error messages
- [x] No telemetry or update checks blocking gameplay
- [x] Game playable for full 5 minutes without network
- [x] Save/load works offline

**Result:**
```
Offline Mode Test: [ ] PASS [ ] FAIL
Startup Time (Offline): _________ seconds
Issues Encountered:
_________________________________________________________
```

---

## Performance Regression Testing

**Time Required:** 20 minutes (Phase 7.4 vs. 7.5 comparison)

**Prerequisite:** Must have Phase 7.4 baseline FPS data (before scene integration)

### Comparative Test

1. Have Phase 7.4 benchmark FPS results available
2. Run Phase 7.5 full MVP loop test
3. Compare FPS at each stage:

**Comparison Table:**
```
Metric                      Phase 7.4 FPS   Phase 7.5 FPS   Variance
Menu Idle                   _________       _________       _________
Exploration                 _________       _________       _________
Combat (Scout Chase)        _________       _________       _________
Combat (Close Range)        _________       _________       _________
Workbench UI                _________       _________       _________
Victory Screen              _________       _________       _________

Acceptable Variance: ±5% or -3 FPS (whichever is larger)
```

**Pass Criteria:**
- [x] No performance regression >5% on any stage
- [x] No new stuttering or frame pacing issues
- [x] If regression found, investigate and document cause

---

## Issue Documentation During Testing

### For Each Issue Found

**Create an entry in `docs/testing/known-issues-phase-7-5.md`:**

```markdown
## Issue #NV-XXX: [Title]

**Reproducibility:** [Always / Often / Sometimes / Rare]

**GPU/Config:** [GPU Model, OS, Driver Version, Resolution]

**Steps to Reproduce:**
1. ...
2. ...

**Expected Behavior:** [What should happen]

**Actual Behavior:** [What actually happens]

**Impact:** [Blocker / Major / Minor / Cosmetic]

**Evidence:**
- Screenshot: [filename]
- Video: [filename or link]
- Log excerpt: [if applicable]

**Workaround:** [User-facing workaround, if any]

**Investigation:**
- Hypothesis: [Initial assessment]
- Root cause: [If determined]
- Resolution: [Fix applied or pending]

**Status:** Investigating / Resolved / Pending Fix
```

---

## Final Test Report Assembly

### After All Tests Complete

Create `docs/testing/TEST-REPORT-PHASE-7-5-FINAL.md`:

1. Summarize all test results (pass/fail counts)
2. GPU compatibility matrix with results
3. Performance data (FPS tables)
4. Input device compatibility status
5. Install/uninstall verification status
6. Known issues list (if any)
7. Overall verdict: **READY FOR RELEASE** / **READY WITH KNOWN ISSUES** / **NOT READY**

---

## Sign-Off Template

```
PHASE 7.5 TESTING SIGN-OFF

Tested By: ________________________
Date Started: ________________________
Date Completed: ________________________

Tests Completed:
[ ] Pre-Flight Checks
[ ] GPU Compatibility (N test machines)
[ ] Resolution Scaling
[ ] Input Device Testing
[ ] Install/Uninstall Verification
[ ] Offline Mode Verification

Result Summary:
  Total Tests Run: ___
  Passed: ___
  Failed: ___
  Known Issues Documented: ___

Blockers Found: [ ] YES [ ] NO

If YES, describe:
_________________________________________________________

Final Verdict:
[ ] READY FOR RELEASE
[ ] READY WITH KNOWN ISSUES (document in known-issues list)
[ ] NOT READY (blockers present)

Signature: ________________________
```

---

## Testing Checklist (Quick Reference)

Use this checklist to ensure all procedures completed:

- [ ] Pre-flight checks passed
- [ ] GPU #1 tested (full 30-min loop)
- [ ] GPU #2 tested (if applicable)
- [ ] GPU #3 tested (if applicable)
- [ ] Resolution scaling tested (1080p, 1440p, 4K)
- [ ] Keyboard + mouse tested
- [ ] Xbox controller tested
- [ ] Generic gamepad tested (optional)
- [ ] Fresh install completed successfully
- [ ] Uninstall completed successfully
- [ ] Re-install verified
- [ ] Offline mode tested
- [ ] All results documented
- [ ] Known issues list completed
- [ ] Final test report generated
- [ ] Team sign-off collected

