# Phase 7.4: Expected Visual Results & Verification

**What you should see after each system is integrated and working**

---

## System 1: Combat Tutorial Hint — Expected Visual

### When Scout is First Detected

**What appears on screen:**

```
                    ┌─────────────────────────────┐
                    │   THREAT DETECTED           │
                    │                             │
                    │ Maintain distance and       │
                    │ attack from range           │
                    │                             │
                    │ Move: WASD | Attack:        │
                    │ Mouse Click | Reload: R     │
                    │                             │
                    │ [Hint will auto-dismiss]    │
                    └─────────────────────────────┘
          (centered upper portion of screen)
```

### Appearance Details

- **Position:** Upper quarter of screen, centered horizontally
- **Text:** White color, clearly readable
- **Font Size:** ~24pt (large, not small)
- **Behavior:** 
  - Appears instantly when scout detects player (0 sec)
  - Stays visible for 8 seconds
  - Fades to transparent over 1.5 seconds
  - Never reappears in same session (one-time only)

### Audio Context

- Scout alert sound plays at SAME moment hint appears
- Sound: Distinctive "alert" tone, 3D directional audio

### Test Steps to Verify

1. **Load game, start new game**
2. **Walk toward scout spawn area** (move closer than 12m detection range)
3. **Scout becomes alert** (you'll see/hear the alert)
4. **Hint text should IMMEDIATELY appear** on screen
5. **Watch for ~8 seconds** (text stays visible)
6. **After 8 seconds, text fades out** (over ~1.5 seconds)
7. **Trigger scout again later** (after respawn or new encounter)
8. **Hint should NOT appear** (only shows once per session)

### If You Don't See It

**Problem: No text appears when scout detects you**

Check:
1. Is `CombatTutorialUI` node in scene tree? (UICanvas → CombatTutorialUI)
2. Is `HintLabel` created as child of `CombatTutorialUI`? (named exactly "HintLabel")
3. Is `CombatTutorialHint` script attached to `CombatTutorialUI`?
4. Is `CombatTutorialPath` set on `GalaxabrainScout`? (should be `../UICanvas/CombatTutorialUI`)
5. Console errors? Check Godot output for script errors

---

## System 2: Resource Locator UI — Expected Visual

### When Player is Near Resources

**What appears on screen:**

```
    ┌──────────────────────┐
    │  🟠 METAL NEARBY     │
    └──────────────────────┘
         (top center of screen)
```

**For different resource types:**

```
Metal:       🟠 METAL NEARBY
Biomass:     🟢 BIOMASS NEARBY
Electronics: 🔵 ELECTRONICS NEARBY
Multiple:    Resources located — Proceed to workbench
```

### Appearance Details

- **Position:** Top-center of screen
- **Text:** White, emoji + text, ~18pt font
- **Behavior:**
  - Appears when within 15m of a resource
  - Updates every 0.5 seconds (smooth transitions)
  - Shows closest resource type if multiple nearby
  - Disappears when >20m from all resources
  - Reappears instantly when re-entering range

### Animated Behavior Sequence

```
Timeline (Resource Locator):

0m away:  [Text disappears - too far]
↓ Player walks toward resource
10m away: [Player enters 15m activation range]
↓
14m away: 🟠 METAL NEARBY [Text appears smoothly]
↓
10m away: 🟠 METAL NEARBY [Text visible and stable]
↓ Player walks away
15m away: 🟠 METAL NEARBY [At edge of range]
↓
20m away: [Text disappears - outside 20m deactivation range]
```

### Test Steps to Verify

1. **Load game, start new game**
2. **Walk toward a metal resource pickup**
3. **When within ~15m, indicator should appear:**
   ```
   🟠 METAL NEARBY
   ```
4. **Walk away beyond 20m**
5. **Indicator should disappear**
6. **Walk toward biomass pickup**
7. **Indicator should show:**
   ```
   🟢 BIOMASS NEARBY
   ```
8. **Walk toward electronics pickup**
9. **Indicator should show:**
   ```
   🔵 ELECTRONICS NEARBY
   ```

### If You Don't See It

**Problem: No indicator text appears even when standing on a resource**

Check:
1. Is `ResourceLocatorUI` node in scene tree? (UICanvas → ResourceLocatorUI)
2. Is `IndicatorLabel` created as child of `ResourceLocatorUI`?
3. Is `ResourceLocatorUI` script attached with correct PlayerPath?
4. **CRITICAL:** Are ALL resource nodes tagged with `pickable_resource` group?
   - Select each resource node
   - Inspector → Node → Groups
   - Should show: `pickable_resource`
5. Is `PlayerPath` set correctly? (should be `../FirstPersonController`)
6. Is `IndicatorLabelPath` set correctly? (should be `IndicatorLabel`)
7. Are resource node names readable? (script looks for "metal", "biomass", "electronics" in name)
8. Console errors? Check Godot output

---

## System 3: Interactive Object Highlight — Expected Visual

### When Player is 25m from Workbench/Beacon

**What appears:**

Visual effect on the 3D object itself (not UI):
- **Orange glow** surrounding the workbench/beacon
- **Smooth pulsing** (gets brighter and dimmer, repeating)
- **Visible in 3D space** (not a UI overlay)

### Appearance Details

**Color:** Orange/golden, RGB approximately (255, 165, 50)  
**Effect:** Emission-based glow (material property)  
**Animation:** Smooth sine-wave pulsing at ~1.5 Hz (1.5 pulses per second)  
**Visibility Range:** 0m–25m (visible, intensity decreases with distance)  
**Beyond 25m:** Glow fades/disappears  

### Pulsing Animation Description

```
Emission Intensity Over Time:

100% ████████████████████
  │    ╱╲    ╱╲    ╱╲
  │   ╱  ╲  ╱  ╲  ╱  ╲
  │  ╱    ╲╱    ╲╱    ╲
  │ ╱
  0% ────────────────────
     0s   1s   2s   3s
     (Smooth sine wave)
```

Intensity varies from `BaseEmissionStrength` (1.5) to `HighlightEmissionStrength` (3.0)

### Visual Distance Behavior

```
Distance    Visibility          Description
─────────────────────────────────────────────────
0m–5m       Full bright glow     Workbench/beacon very visible
5m–15m      Bright glow          Still very noticeable
15m–25m     Fading glow          Starting to dim
>25m        No visible glow      Too far to see effect
```

### Test Steps to Verify

1. **Load game**
2. **Teleport or move to ~30m from workbench** (need distance to see range effect)
3. **Walk toward workbench slowly, watching it**
4. **At ~25m, orange glow should START appearing** (faint at first)
5. **Continue walking closer**
6. **Glow should GET BRIGHTER** as you approach
7. **Observe the pulsing:** Glow should smoothly brighten and dim (~1.5 times/sec)
8. **At closest range:** Glow is very bright and pulsing visibly
9. **Walk away to >25m**
10. **Glow should FADE and disappear**
11. **Repeat for beacon** (same expected behavior)

### If Glow Looks Wrong

**Problem: Glow too dim to see**
- In Workbench inspector, increase properties:
  - BaseEmissionStrength: Try 2.5–3.0 (default 1.5)
  - HighlightEmissionStrength: Try 5.0–7.0 (default 3.0)
- Save, reload game
- Glow should be much brighter

**Problem: Glow pulses too fast/slow**
- In Workbench inspector:
  - PulseSpeed: 2.0–3.0 for faster (default 1.5)
  - PulseSpeed: 0.5–1.0 for slower
- Save, reload game

**Problem: Glow visible too far/not far enough**
- In Workbench inspector:
  - HighlightDistance: Increase to 35–40 to see from farther
  - HighlightDistance: Decrease to 15 to see only up close
- Save, reload game

**Problem: No glow at all**
- Check InteractiveObjectHighlight script attached to Workbench
- Check Workbench MeshInstance3D has a StandardMaterial3D
- If no material: Create one (Inspector → Material → New StandardMaterial3D)
- Check PlayerPath set correctly
- Check console for errors

---

## Full Gameplay Integration Test

### Complete MVP Loop with All 3 Systems

Follow this sequence to verify all systems work together:

#### Phase 1: Exploration & Resource Collection (5 min)

```
Expected Events:
1. Start new game
   ✓ No hint text visible yet (scout not detected)
   
2. Walk around exploring
   ✓ No indicator text visible yet (no resources in range)
   
3. Walk toward metal resource
   ✓ When within 15m:  🟠 METAL NEARBY  (appears)
   
4. Collect metal, walk toward biomass
   ✓ When within 15m:  🟢 BIOMASS NEARBY  (appears)
   
5. Collect biomass, walk toward electronics
   ✓ When within 15m:  🔵 ELECTRONICS NEARBY  (appears)
   
6. Collect electronics
   ✓ Walk away → indicators disappear
```

#### Phase 2: Workbench & Crafting (3 min)

```
Expected Events:
1. Walk toward workbench from distance
   ✓ At 25m: Orange glow appears on workbench (faint)
   ✓ As you walk closer: Glow gets brighter
   ✓ Glow is pulsing smoothly
   
2. Reach workbench, open crafting menu
   ✓ Glow continues (no disruption)
   
3. Craft mechanical arm
   ✓ Resources consumed, arm added to inventory
   
4. Close menu
   ✓ Glow still visible
```

#### Phase 3: Scout Combat (5 min)

```
Expected Events:
1. Walk toward scout spawn area
   ✓ No hint text visible yet
   ✓ Scout is in Idle state (no alert)
   
2. Get within 12m of scout
   ✓ Scout alert plays (audio cue)
   ✓ AT EXACT SAME MOMENT:  THREAT DETECTED  hint appears
   ✓ Hint text shows in upper-center of screen
   ✓ Hint is white, 24pt, readable
   
3. Move around (combat occurs)
   ✓ Hint stays visible for ~8 seconds total
   ✓ Hint does NOT interrupt combat
   ✓ Combat mechanics work normally
   
4. Defeat scout
   ✓ Scout dies
   ✓ Component drops
```

#### Phase 4: Beacon & Victory (2 min)

```
Expected Events:
1. Walk toward beacon from distance
   ✓ At 25m: Orange glow appears on beacon (faint)
   ✓ Glow gets brighter as you approach
   
2. Reach beacon, interact with it
   ✓ Glow visible until victory screen
   
3. Victory screen appears
   ✓ Game shows win condition met
```

### Result

✓ **ALL SYSTEMS WORKING TOGETHER** if:
- Combat Tutorial hint appeared once, on first scout detection
- Resource Locator indicators appeared when near resources
- Interactive Highlight glow visible on workbench and beacon
- Full MVP loop completed without errors
- No conflicting visual effects
- All audio/mechanics still working

---

## Performance Expectations

### FPS Impact (If Measurable)

Expect minimal impact from Phase 7.4 systems:

```
Before Phase 7.4:    60 FPS (baseline)
After Phase 7.4:     58–60 FPS (negligible impact)

Each system:
├── Combat Tutorial: <0.1% CPU (only when active)
├── Resource Locator: <0.2% CPU (low update frequency)
└── Interactive Highlight: <0.3% CPU (smooth pulsing)

Total: <0.5% overhead
```

**If you see FPS drop significantly (>10 FPS loss):**
- Check if multiple processes are running
- Verify emission effects aren't overloaded
- Review material settings on workbench/beacon

---

## Verification Checklist

After completing integration, verify each system:

### Combat Tutorial ✓

- [ ] Hint text appears on first scout detection
- [ ] Text is centered, upper portion of screen
- [ ] Text is white and readable (24pt)
- [ ] Text contains all 4 lines of guidance
- [ ] Text stays for ~8 seconds
- [ ] Text fades out smoothly (1.5 sec fade)
- [ ] Hint only appears once per session
- [ ] Audio alert plays at same time as text appears
- [ ] No errors in console

### Resource Locator ✓

- [ ] Indicator appears when within 15m of metal resource
- [ ] Shows: 🟠 METAL NEARBY
- [ ] Indicator appears when within 15m of biomass resource
- [ ] Shows: 🟢 BIOMASS NEARBY
- [ ] Indicator appears when within 15m of electronics resource
- [ ] Shows: 🔵 ELECTRONICS NEARBY
- [ ] Indicator disappears when >20m from all resources
- [ ] Indicator updates smoothly (0.5s interval, not jerky)
- [ ] Multiple resources show generic "Resources located" message
- [ ] No errors in console

### Interactive Highlight ✓

- [ ] Orange glow appears on workbench at ~25m
- [ ] Orange glow appears on beacon at ~25m
- [ ] Glow gets brighter as you approach
- [ ] Glow gets dimmer as you move away
- [ ] Glow pulses smoothly (sine wave, ~1.5 Hz)
- [ ] Glow disappears when >25m away
- [ ] Glow has orange color (not white, not other colors)
- [ ] Glow is visible in 3D space (on the object, not UI)
- [ ] Glow doesn't interfere with gameplay or visibility
- [ ] No errors in console

---

## Next Steps

### If All Systems Working ✓

1. **Save Main.tscn** (Ctrl+S)
2. **Close and reopen Godot** (verify scene loads correctly)
3. **Run full MVP loop again** (final verification)
4. **Commit to git:**
   ```bash
   git add scenes/Main/Main.tscn
   git commit -m "feat: Phase 7.4 scene integration - all three systems active"
   ```
5. **Move to Phase 7.4 Gameplay Testing** (30–60 min)

### If Some Systems Not Working

1. **Check the specific system** using checklist above
2. **Refer to troubleshooting** in Phase 7.4 Scene Integration Guide
3. **Verify all node paths** using Scene Structure Reference
4. **Check console** for error messages
5. **Re-read relevant section** of integration guide
6. **Try again** after fixes

---

## Visual Comparison: Before vs After

### Before Phase 7.4 Integration

```
Game Screen:
┌────────────────────────────┐
│                            │
│  Plain game world          │
│  Player at first person    │
│  No UI overlays (except HUD│
│                            │
│  Scout appears             │
│  (no tutorial hint)        │
│                            │
│  Workbench visible         │
│  (no glow effect)          │
│                            │
└────────────────────────────┘
```

### After Phase 7.4 Integration ✓

```
Game Screen:
┌────────────────────────────┐
│   THREAT DETECTED          │ ← Combat Tutorial (System 1)
│   [Hint text visible]      │
│                            │
│   🟠 METAL NEARBY          │ ← Resource Locator (System 2)
│                            │
│  Game world                │
│  Player at first person    │
│  Scout appears with alert  │
│                            │
│  Workbench GLOWING orange  │ ← Interactive Highlight (System 3)
│  (smooth pulsing effect)   │
│                            │
└────────────────────────────┘
```

---

## Completion Milestone

Phase 7.4 Scene Integration is **COMPLETE** when you can:

1. ✓ See combat tutorial hint appear on scout detection
2. ✓ See resource indicators appear when resources are near
3. ✓ See orange glow on workbench and beacon
4. ✓ Complete full MVP loop without errors
5. ✓ Save and close scene without issues
6. ✓ Reopen scene and all systems still work

**Estimated time:** 45–60 minutes (including setup and testing)

---

**You're ready to begin integration! Open Godot, follow the Scene Integration Guide, verify results against this document, and you'll have all three systems active.** ✓

