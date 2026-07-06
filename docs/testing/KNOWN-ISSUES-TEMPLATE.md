# Phase 7.5: Known Issues Template

**Purpose:** Standardized format for documenting issues found during Phase 7.5 testing.

**Location:** `docs/testing/known-issues-phase-7-5.md` (populated during testing)

**Status:** Template — ready for use

---

## Issue Template

Use this template for each issue discovered:

```markdown
## Issue Phase7.5-[GPU/CATEGORY]-[NUMBER]: [Title]

**Severity:** [Blocker / Major / Minor / Cosmetic]  
**Reproducibility:** [Always / Often / Sometimes / Rare]  
**Status:** [Investigating / Root Cause Found / Awaiting Fix / Resolved / Deferred]

### Affected Configurations

| Component | Value |
|---|---|
| GPU | [e.g., RTX 3060 Ti, RX 6700 XT, UHD 630] |
| VRAM | [e.g., 8GB, 12GB, shared] |
| Driver Version | [e.g., 531.18, 23.12.1] |
| OS | [Windows 10 21H2 / Windows 11 22H2] |
| OS Build | [e.g., 22621.xxxx] |
| Resolution | [1920×1080 / 2560×1440 / 3840×2160] |
| Refresh Rate | 60Hz / 144Hz / other |
| Input Device | Keyboard/Mouse / Xbox Controller / Generic |

### Steps to Reproduce

1. [Step 1]
2. [Step 2]
3. [Step 3]
4. [...]

### Expected Behavior

[What should happen according to game design]

### Actual Behavior

[What actually happens]

### Impact Analysis

**Gameplay Impact:** 
- Blocks progression? [YES / NO]
- Affects combat? [YES / NO]
- Affects resource collection? [YES / NO]
- Cosmetic only? [YES / NO]

**Player Experience:**
- Frequency encountered in typical playthrough: [Always / Often / Rare]
- Severity from player perspective: [Game-breaking / Frustrating / Minor / Unnoticed]

### Evidence

**Screenshots:**
- [File: screenshot-issue-name-01.png]
- [File: screenshot-issue-name-02.png]

**Video Recording:**
- [File: video-issue-name.mp4 or YouTube link]

**Log File:**
```
[Paste relevant log excerpt, if applicable]
[Godot/game logs usually in AppData\Local\TitanCraft\logs\]
```

**System Info:**
- CPU: [Model, clock speed]
- RAM: [Capacity, type, speed]
- Storage: [SSD/HDD, available space]
- Other running applications: [List any that might interfere]

### Initial Investigation

**Hypothesis:**
[What might be causing this issue?]
- [Possible cause 1]
- [Possible cause 2]
- [Possible cause 3]

**Root Cause (if determined):**
[Actual cause identified, if investigation completed]

**Potential Fix:**
[Suggested fix direction, code location, or configuration change]

### Workaround for Users

**Is there a workaround?** [YES / NO]

**If YES, describe:**
[Steps user can take to avoid or mitigate the issue]

**Example:**
- Reduce resolution to 1080p
- Update GPU driver to version X or newer
- Disable overlay (Discord, GeForce Experience)
- Close background applications

### Resolution Path

**Current Status:** 
- [ ] Under investigation (in-progress diagnostics)
- [ ] Root cause identified, fix in progress
- [ ] Fix developed, awaiting verification
- [ ] Fixed and verified
- [ ] Deferred to post-launch update

**Owner:** [Assigned to: Name or "TBD"]

**ETA for Fix:** [Date if applicable, or "pending investigation"]

**Verification Plan:**
[How will we know the fix works? What test procedure?]

### Notes & Context

[Additional context, related issues, historical info, etc.]

### Attachments

- [If applicable: .zip of save files that reproduce issue]
- [If applicable: .txt of system diagnostics output]
- [If applicable: .log of game session]

---

## Issue Submission Checklist

Before submitting an issue, verify:

- [ ] Issue title is clear and specific
- [ ] Steps to reproduce are clear and complete
- [ ] Expected vs. actual behavior clearly distinguished
- [ ] All affected configurations listed
- [ ] At least one screenshot or video provided
- [ ] Severity and reproducibility accurately assessed
- [ ] Issue is not a duplicate of existing known issue

```

---

## Example: Completed Issue

### Reference Issue

```markdown
## Issue Phase7.5-AMD-001: Frame stuttering during scout chase @ 1440p on RX 6700 XT

**Severity:** Major  
**Reproducibility:** Always (when testing RX 6700 XT @ 1440p)  
**Status:** Investigating

### Affected Configurations

| Component | Value |
|---|---|
| GPU | AMD Radeon RX 6700 XT |
| VRAM | 12GB |
| Driver Version | 23.12.1 WHQL |
| OS | Windows 11 22H2 |
| OS Build | 22621.2715 |
| Resolution | 2560×1440 |
| Refresh Rate | 60Hz |
| Input Device | Keyboard + Mouse |

### Steps to Reproduce

1. Launch game at 1440p resolution
2. Start new game
3. Collect resources quickly (2 min)
4. Trigger scout encounter (move toward spawn zone)
5. Allow scout to enter chase state (audio alert)
6. Observe FPS counter while scout chases player

### Expected Behavior

FPS should remain steady at 45–55 during chase phase on RX 6700 XT @ 1440p.

### Actual Behavior

Frame rate drops from 55 FPS to 35–40 FPS during scout chase, with noticeable stuttering (frame time spikes to 30ms). Stuttering resolves if player stands still and scout enters attack state.

### Impact Analysis

**Gameplay Impact:**
- Blocks progression? NO
- Affects combat? YES (makes kiting more difficult)
- Affects resource collection? NO
- Cosmetic only? NO

**Player Experience:**
- Frequency: Always during scout chase @ 1440p on RX 6700 XT
- Severity: Frustrating (combat becomes harder to control)

### Evidence

**Screenshots:**
- [File: frametimes-radeonfps-1440p.png - showing frame time graph with spikes]
- [File: overlay-stuttering-scout-chase.png - in-game FPS overlay during chase]

**Video Recording:**
- [File: scout-chase-1440p-stuttering.mp4 - 30-second recording showing stutter pattern]

**Log File:**
```
[2026-07-09T14:32:15] Scout state changed: Idle → Chase
[2026-07-09T14:32:16] Frame time spike: 32ms (target: 16.6ms for 60 FPS)
[2026-07-09T14:32:17] Frame time spike: 31ms
[2026-07-09T14:32:17] Frame time spike: 29ms
[2026-07-09T14:32:18] GPU utilization jumped to 95%
[2026-07-09T14:32:19] VRAM utilization: 9.2GB / 12GB
```

### Initial Investigation

**Hypothesis:**
1. AMD RDNA2 shader compilation stutter (common issue in first execution of complex shaders)
2. VRAM bandwidth saturation at 1440p (scout mesh + environment = high poly count)
3. Godot 4 physics calculations during chase (multiple raycasts for pathfinding)
4. Driver issue with 23.12.1 (AMD Adrenalin December 2023 update)

**Root Cause (if determined):**
[To be updated after investigation]

### Workaround for Users

**Is there a workaround?** YES

**Workaround steps:**
1. Reduce resolution to 1920×1080 (stuttering does not occur)
2. OR update GPU driver to 24.1.1 or newer (AMD may have fixed in latest driver)
3. OR disable any GPU overlay (Discord, Radeon Software overlay)

### Resolution Path

**Current Status:**
- [x] Under investigation (in-progress diagnostics)
- [ ] Root cause identified, fix in progress
- [ ] Fix developed, awaiting verification
- [ ] Fixed and verified
- [ ] Deferred to post-launch update

**Owner:** Assigned to: Engine Architect

**ETA for Fix:** Pending driver investigation (target: 2026-07-15)

**Verification Plan:**
Reproduce issue on RX 6700 XT with updated driver (24.1.1). If resolved, issue is closed with note "Resolved by driver update". If persists, investigate Godot physics/rendering optimization.

### Notes & Context

- This issue appears specific to AMD RDNA2 GPUs
- NVIDIA RTX 3060 Ti does NOT exhibit this stutter @ 1440p
- Intel Arc A770 does NOT exhibit this stutter @ 1440p
- Issue was not present during Phase 7.4 (Phase 7.4 was 1080p-only baseline testing)
- May be related to AMD's RDNA2 architecture and Godot 4's shader compilation strategy
- Related issue: [Phase7.5-AMD-002] (if similar stutter occurs in other areas)

```

---

## Known Issues List Format

Maintain a running list of all issues in order of discovery:

```markdown
# Phase 7.5 Known Issues - Active Tracking

**Last Updated:** 2026-07-XX  
**Total Issues:** N  
**Blockers:** N  
**Resolved:** N  

## Active Issues

| ID | Title | GPU | Severity | Status | Workaround |
|---|---|---|---|---|---|
| Phase7.5-AMD-001 | Frame stutter @ 1440p chase | RX 6700 | Major | Investigating | Use 1080p |
| Phase7.5-NV-001 | High VRAM use @ 4K | RTX 4080 | Minor | Resolved | Driver 530+ |
| Phase7.5-INPUT-001 | Controller detected late | Generic | Minor | Investigating | Reconnect before launch |

## Resolved Issues

| ID | Title | GPU | Fix | Verification Date |
|---|---|---|---|---|
| Phase7.5-NV-001 | ... | ... | ... | 2026-07-XX |

## Deferred Issues

Issues not fixed before release (documented for post-launch updates):

| ID | Title | GPU | Reason | Follow-up Plan |
|---|---|---|---|---|
| Phase7.5-AMD-002 | ... | ... | ... | Patch 1.0.1 candidate |
```

---

## Severity Rating Guide

| Level | Definition | Example | Action |
|---|---|---|---|
| **Blocker** | Game unplayable or progression impossible | Crash on startup, cannot save/load, invincible enemy | Must fix before release |
| **Major** | Significant gameplay degradation or frustration | Combat unresponsive, 20 FPS below target, frequent stutters | Should fix before release |
| **Minor** | Noticeable but workaround exists | UI label misaligned, rare audio glitch, 5% FPS impact | Document + include workaround |
| **Cosmetic** | Visual/audio polish, no gameplay impact | Typo in victory screen, missing particle effect | Document, can defer |

---

## Reproducibility Guide

| Category | Definition | Impact on Testing |
|---|---|---|
| **Always** | Issue happens every single time | Easy to test & verify, high confidence |
| **Often** | Issue happens 70%+ of attempts | Can be reproduced consistently, some variance |
| **Sometimes** | Issue happens 30–70% of attempts | Harder to diagnose, may need multiple attempts |
| **Rare** | Issue happens <30% of attempts | Very hard to reproduce, may be environmental |

---

## Issue ID Naming Convention

Format: `Phase7.5-[CATEGORY]-[NUMBER]`

**Categories:**
- `NV`: NVIDIA GPU-specific
- `AMD`: AMD GPU-specific
- `INTEL`: Intel GPU-specific
- `INPUT`: Input device-related
- `INSTALL`: Install/uninstall process
- `PERF`: Performance regression
- `CRASH`: Crash or hang
- `SAVE`: Save/load system
- `GENERAL`: General/multi-platform

**Examples:**
- `Phase7.5-NV-001`: First NVIDIA-specific issue
- `Phase7.5-INPUT-003`: Third input device issue
- `Phase7.5-CRASH-001`: First crash issue

