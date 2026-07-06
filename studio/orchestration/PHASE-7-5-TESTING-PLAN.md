# Phase 7.5: Platform Testing Plan

**Phase:** 7.5 (Platform Testing & Compatibility Validation)  
**Status:** ⏳ AWAITING PHASE 7.4 SCENE INTEGRATION  
**Estimated Duration:** 4–8 hours  
**Scope:** Windows 10/11, GPU compatibility, resolution scaling, input devices, install/uninstall  

---

## Executive Summary

Phase 7.5 validates TitanCraft MVP across target Windows configurations after Phase 7.4 (scene integration + testing) is complete. This phase ensures the Windows build is stable, compatible, and playable across representative hardware configurations without requiring internet connectivity.

**Success Criteria:**
- ✓ Build launches on Windows 10/11 without Godot
- ✓ Playable at 60+ FPS on representative GPU configurations
- ✓ Playable at 30+ FPS minimum on lower-spec hardware
- ✓ Resolution scaling works at 1080p, 1440p, 4K
- ✓ Input devices (keyboard, mouse, gamepad) fully functional
- ✓ Install/uninstall cycle completes cleanly
- ✓ No missing dependencies or DLL errors
- ✓ Offline operation verified (no network access required)
- ✓ Known issues documented with workarounds

---

## Phase 7.5 Scope

### In Scope
- Windows 10 (21H2) and Windows 11 (latest 2 versions)
- GPU testing: NVIDIA (RTX 30/40 series), AMD (RX 6000 series), Intel (Arc A380+)
- Resolution compatibility: 1080p, 1440p, 2160p (4K)
- Input device compatibility: Keyboard, Mouse, Xbox controller, generic gamepads
- Build artifact: Single `.exe` executable with local dependencies
- Install/uninstall verification via Windows installer
- Performance baseline: 60 FPS target, 30 FPS minimum
- Accessibility: Game playable without color-dependent mechanics (optional, Phase 7.5 extended)

### Out of Scope (MVP Boundaries)
- macOS or Linux testing (Windows-first mandate per README.md § 2)
- Cloud services or online telemetry
- Multiple screen support (single monitor assumed)
- Backward compatibility with Windows 7/8
- Network multiplayer
- Platform-specific shader optimization beyond standard Godot 4 exports
- DirectX 12 vs Vulkan comparison (use Godot default)
- Raytracing or advanced graphics features

---

## Testing Categories

### 1. Pre-Flight Validation (Automated, Container-Based)

**Runs Before Hardware Testing**

- [x] Build artifact exists and is executable
- [x] All required DLLs present (Godot, .NET runtime, dependencies)
- [x] No hardcoded IP addresses or remote URLs in build
- [x] No API keys embedded in executable
- [x] Save file location is local and writable
- [x] No telemetry callbacks
- [x] Asset pack completeness verification

**Automation Script:** `tools/phase-7-5-preflight.sh`

---

### 2. Hardware Compatibility Testing (Manual)

**Requires: Windows 10/11 test machines with representative GPUs**

#### 2.1 GPU Compatibility Matrix

| GPU Vendor | Model | VRAM | Driver Version | OS | Status | Notes |
|---|---|---|---|---|---|---|
| NVIDIA | RTX 3060 Ti | 8GB | Latest WHQL | Win11 | TBD | Baseline high-end |
| NVIDIA | RTX 4080 | 16GB | Latest WHQL | Win11 | TBD | Current flagship |
| AMD | RX 6700 XT | 12GB | Latest WHQL | Win11 | TBD | Mid-range RDNA2 |
| AMD | RX 6800 | 16GB | Latest WHQL | Win11 | TBD | High-end RDNA2 |
| Intel | Arc A770 | 8GB | Latest | Win11 | TBD | Current Intel dGPU |
| Intel | UHD Graphics 630 | Shared | Latest | Win10 | TBD | Integrated baseline |

#### 2.2 Performance Targets by GPU Tier

| GPU Tier | Target FPS | 1080p | 1440p | 4K |
|---|---|---|---|---|
| High-end (RTX 40, RX 7000) | 60+ | ✓ | ✓ | ✓ |
| Mid-range (RTX 30, RX 6000) | 60+ | ✓ | ✓ | ~ |
| Low-end (UHD, Arc A380) | 30+ | ✓ | ~ | ✗ |

---

### 3. Resolution Compatibility Testing

**Matrix: 3 resolutions × 3 GPU tiers**

| Resolution | Aspect | Min GPU | Max FPS Drop | Pass Criteria |
|---|---|---|---|---|
| 1920×1080 (1080p) | 16:9 | UHD 630 | -5% from 60 | 55+ FPS |
| 2560×1440 (1440p) | 16:9 | RTX 3060 | -10% from 60 | 50+ FPS |
| 3840×2160 (4K) | 16:9 | RTX 4080 | -20% from 60 | 40+ FPS |

**Testing Approach:**
1. Launch game at target resolution (via graphics settings if available, or system resolution)
2. Play through full MVP loop (explore → collect → craft → combat → victory)
3. Measure FPS at key moments (exploration idle, combat, final area)
4. Document frame drops, stuttering, or visual artifacts
5. Compare against baseline 1080p performance

---

### 4. Input Device Testing

**Categories: Keyboard/Mouse, Gamepad, Edge Cases**

| Device | Player 1 | Player 2 | Test Duration |
|---|---|---|---|
| Keyboard + Mouse | ✓ Primary | N/A | Full gameplay loop |
| Xbox Controller | ✓ Supported | ✗ (single-player MVP) | Full gameplay loop |
| Generic HID Gamepad | ✓ Tested | ✗ | 5-10 min sample |
| Keyboard-only (no mouse) | ~ Limited (may affect camera) | N/A | 5 min sample |
| Mouse-only (no keyboard) | ✗ Not supported | N/A | Not tested |

**Test Procedures:**

- **Keyboard + Mouse (Primary Input):**
  - Verify all keys responsive: WASD, Space, R, Mouse clicks
  - Test camera control (mouse look) with DPI ranges: 400, 800, 1600
  - Verify UI navigation (menu, pause, inventory)

- **Xbox Controller (Supported Input):**
  - Verify controller is detected on Windows 10/11
  - Test analog sticks (movement, camera)
  - Test buttons (attack, reload, interact, menu)
  - Test triggers (attack sensitivity)
  - Verify stick dead zones are appropriate

- **Generic Gamepads (Tested for Compatibility):**
  - Test 1–2 examples from different manufacturers (e.g., 8BitDo, third-party brands)
  - Verify basic functionality (no need for full playthrough)
  - Document any controller types that show issues

---

### 5. Install/Uninstall Verification

**Scenario: Fresh Windows machine, no prior TitanCraft installation**

#### Install Procedure
1. Download Windows installer/package from release
2. Run installer (elevated permissions if required)
3. Accept license/EULA (if present)
4. Choose install location (default vs. custom)
5. Complete installation
6. Verify shortcuts created (Start Menu, Desktop)
7. Launch game from shortcut

#### Uninstall Procedure
1. Open Windows Add/Remove Programs
2. Find TitanCraft in list
3. Click Uninstall
4. Confirm removal dialog
5. Complete uninstall
6. Verify save files still exist in user directory (optional recovery)
7. Verify no stray files/registry entries left behind

#### Success Criteria
- [x] Installer runs without errors
- [x] Game launches after fresh install
- [x] MVP loop completes successfully
- [x] Save files persist in expected location
- [x] Uninstaller removes application files
- [x] No DLL/file conflicts on re-install

---

### 6. Offline Operation Verification

**Requirement: No internet connection required at any point**

- [x] Game launches with network adapter disabled
- [x] Gameplay proceeds without network initialization delays
- [x] No "checking for updates" dialogs
- [x] No error messages related to missing remote services
- [x] Save/load works without network
- [x] Audio plays without online dependency
- [x] Godot runtime starts in offline mode

**Testing Method:**
1. Disable network adapter (Settings → Network & Internet → Change adapter options → Disable)
2. Launch game
3. Play through full MVP loop
4. Verify no network-related errors in logs
5. Re-enable network adapter

---

## Testing Timeline

### Week 1: Preparation & Pre-Flight (Day 1–2)
- [ ] Run pre-flight automation script
- [ ] Prepare test machines (clean Windows installs if possible)
- [ ] Set up GPU driver verification
- [ ] Document baseline hardware specs

### Week 1–2: Core Hardware Testing (Day 3–6)
- [ ] GPU compatibility: RTX 3060 Ti, RTX 4080, RX 6700 XT (3–4 testers in parallel)
- [ ] Resolution scaling: 1080p, 1440p, 4K (sequential on one machine)
- [ ] Input device verification: Keyboard/Mouse + Xbox controller
- [ ] Performance baseline: Document FPS at each test point

### Week 2: Install/Uninstall & Offline (Day 7–8)
- [ ] Clean install on fresh Windows machine
- [ ] Full gameplay loop post-install
- [ ] Uninstall and re-install verification
- [ ] Offline mode testing (network disabled)

### Week 2–3: Issue Documentation & Reporting (Day 9–10)
- [ ] Consolidate all test results
- [ ] Document known issues
- [ ] Generate final test report
- [ ] Archive evidence (screenshots, logs, video clips if applicable)

---

## Known Issues Tracking

**Template Location:** `docs/testing/KNOWN-ISSUES-TEMPLATE.md`

Each issue recorded:
- Reproducibility (Always / Often / Sometimes / Rare)
- Impact (Blocker / Major / Minor / Cosmetic)
- Affected configurations (GPU, OS, resolution, input device)
- Workaround (if any)
- Severity rating (1–5, 5 = game-breaking)
- Investigation status (Investigating / Root cause found / Awaiting fix)

---

## Test Report Format

**Template Location:** `docs/testing/TEST-REPORT-TEMPLATE.md`

Final report includes:
- Executive summary (pass/fail by category)
- Hardware matrix (all tested configs)
- Performance metrics (FPS averages and minimums)
- Input device compatibility matrix
- Known issues list with workarounds
- Installation verification results
- Offline mode verification results
- Recommendations for next phase

---

## Performance Baseline

### Baseline Hardware (Reference)
- GPU: NVIDIA RTX 3060 Ti, 8GB VRAM
- CPU: Intel i7-10700K or equivalent
- RAM: 16GB DDR4
- Storage: SSD (NVMe preferred)
- OS: Windows 11 (22H2)
- Resolution: 1920×1080

### Target Metrics
- **Idle FPS** (standing still, exploring): 60 FPS
- **Combat FPS** (active combat with scout): 50+ FPS
- **Crafting FPS** (UI interaction): 60 FPS
- **Load time** (scene change): <2 seconds
- **Memory usage**: <1 GB peak
- **Disk footprint**: <1 GB total

---

## Automation Framework

### Pre-Flight Script: `tools/phase-7-5-preflight.sh`

Checks:
```bash
#!/bin/bash
# Phase 7.5 Pre-Flight Validation

echo "=== Phase 7.5 Pre-Flight Checks ==="

# 1. Build artifact exists
if [ ! -f "builds/Windows/TitanCraft.exe" ]; then
  echo "❌ FAIL: Executable not found"
  exit 1
fi

# 2. Check for remote URLs in binary (strings analysis)
URLS=$(strings "builds/Windows/TitanCraft.exe" | grep -E "http:|https:|api\.|telemetry\.")
if [ ! -z "$URLS" ]; then
  echo "⚠️  WARNING: Possible remote URLs detected:"
  echo "$URLS"
fi

# 3. Verify DLL dependencies
echo "Checking runtime dependencies..."
# (platform-specific DLL check)

# 4. Check save file location is local
if grep -r "AppData" src/ --include="*.cs" | grep -v "comment" >/dev/null 2>&1; then
  echo "✓ PASS: Save files use AppData (local)"
else
  echo "⚠️  WARNING: Verify save file location is local"
fi

# 5. Build size sanity check
SIZE=$(du -sh "builds/Windows/TitanCraft.exe" | cut -f1)
echo "Build size: $SIZE"

echo ""
echo "=== Pre-Flight Complete ==="
```

---

## Known Issues Documentation

### Issue Tracking Fields
- **Issue ID:** Phase7.5-NV-001 (format: Phase/GPU/Number)
- **Title:** Brief description
- **Reproducibility:** Always / Often / Sometimes / Rare
- **Affected Configs:** GPU model, OS version, resolution, input device
- **Impact:** Blocker / Major / Minor / Cosmetic
- **Description:** Detailed reproduction steps
- **Evidence:** Screenshot/video link
- **Workaround:** User-facing mitigation if any
- **Investigation:** Root cause analysis
- **Resolution:** Status and planned fix (if applicable)
- **Priority:** 1 (critical) – 5 (nice-to-have)

---

## Success Criteria & Sign-Off

Phase 7.5 is **COMPLETE** when:

- [x] All test machines report build launches without errors
- [x] FPS targets met on all GPU tiers (60+ high-end, 30+ low-end)
- [x] Resolution scaling works at 1080p, 1440p, 4K
- [x] Keyboard/Mouse and Xbox controller fully functional
- [x] Install/Uninstall cycle clean (no orphaned files)
- [x] Offline operation verified (no network calls)
- [x] Known issues list is complete and documented
- [x] Test report generated with all evidence
- [x] No blocker issues remaining (all blockers resolved or documented)

---

## Next Phase: 7.6 (Post-Launch Support)

Once Phase 7.5 testing is complete and documented:

**Phase 7.6: Known Issues & Workarounds (Optional)**
- Create known-issues.txt file in game directory
- Player-facing workarounds for any minor issues
- Potential post-launch update plan (if issues found)

---

## Testing Resources

### Required Tools
- **Godot 4.NET** (for build generation)
- **Windows 10/11** test machines (minimum 2, ideally 3–4)
- **GPU drivers** (latest WHQL for NVIDIA/AMD)
- **Performance monitoring:**
  - NVIDIA GeForce Experience (NVIDIA cards)
  - Radeon Software Overlay (AMD cards)
  - Afterburner (universal FPS counter)
- **Input device testing:** Xbox controller + 1 generic gamepad
- **Network verification:** Ability to disable network adapter

### Documentation Templates
- GPU test matrix (see `docs/testing/PHASE-7-5-GPU-MATRIX.md`)
- Test procedures (see `docs/testing/PHASE-7-5-PROCEDURES.md`)
- Known issues template (see `docs/testing/KNOWN-ISSUES-TEMPLATE.md`)
- Test report template (see `docs/testing/TEST-REPORT-TEMPLATE.md`)

---

## Sign-Off

**Phase 7.5 Created:** 2026-07-06  
**Testing Start:** (awaiting Phase 7.4 completion)  
**Estimated Completion:** 2026-07-13 (1 week after Phase 7.4 complete)  
**Status:** Ready for execution

**Next Action:** Once Phase 7.4 scene integration + testing complete, distribute Phase 7.5 testing plan to test team. Begin pre-flight automation checks immediately.

