# Phase 3 Visibility Test Plan
## Interactive Prop Distance Validation

**Phase:** 3  
**Test Scope:** Three interactable assets (Workbench, Beacon Dormant/Active, Save Point)  
**Test Duration:** ~45–60 minutes total  
**Environment:** Godot 4 first-person viewer or standalone GLB viewer  
**Success Criteria:** All props readable at gameplay distance, visually distinct from each other

---

## Test Assets Required

Before testing, ensure these GLB files are available:

```
assets/Production/Generated/MVP_Pack_V1/
├── TC_PROP_Workbench_V1.glb          (Phase 3, orange accent)
├── TC_PROP_Beacon_Dormant_V1.glb     (Phase 3, red standby glow)
├── TC_PROP_Beacon_Active_V1.glb      (Phase 3, purple crystal glow)
└── TC_PROP_SavePoint_V1.glb          (Phase 3, cyan glow)
```

---

## Test 1: Workbench V1 Distance Visibility

**Objective:** Confirm orange holographic panel is readable from 10+ meters

**Setup:**
1. Open Godot or GLB viewer
2. Import `TC_PROP_Workbench_V1.glb`
3. Place in scene at origin (0, 0, 0)
4. Create first-person camera at player eye height (~1.7m)
5. Position camera 10m away from workbench (along +Y axis)

**Test Procedure:**

| Distance | Checklist | Pass/Fail |
|----------|-----------|-----------|
| **10m** | ☐ Silhouette reads "industrial workbench" | `__` |
| **10m** | ☐ Orange panel obviously visible | `__` |
| **10m** | ☐ Articulated arm readable (3–4 segments obvious) | `__` |
| **10m** | ☐ Scale appropriate (larger than player, not massive) | `__` |
| **10m** | ☐ Bench base is solid and grounded | `__` |
| **8m** | ☐ Orange material glows or reflects | `__` |
| **8m** | ☐ Beige hull material distinguishable from ground | `__` |
| **6m** | ☐ Functional details visible (panels, segments, structure) | `__` |
| **6m** | ☐ Arm position/pose is clear (menacing assembly stance) | `__` |

**Validation Checklist:**
- [ ] Workbench orange accent visible at 10m (emissive or highly reflective)
- [ ] Arm silhouette readable (not mushy or undefined)
- [ ] Bench structure reads as "craft station" (not sculpture or storage)
- [ ] Material palette coherent (beige hull, dark steel, orange highlights)
- [ ] No visual corruption, texture stretching, or floating geometry
- [ ] Scale feels right in FPS perspective

**If test fails:**
- Document which distance/aspect fails
- Note if issue is glow, geometry, or scale
- Recommend: Increase orange emissive strength, add more edge definition, or adjust scale

**Result:** ✅ **PASS** / ❌ **NOT_READY**

---

## Test 2: Beacon Dormant & Active State Distance Visibility

**Objective:** Confirm both beacon states are distinct and readable from 20+ meters

### Test 2A: Beacon Dormant V1

**Setup:**
1. Import `TC_PROP_Beacon_Dormant_V1.glb`
2. Place at origin (0, 0, 0)
3. First-person camera at 1.7m height
4. Position 20m away from beacon (along +Y axis)

**Test Procedure:**

| Distance | Checklist | Pass/Fail |
|----------|-----------|-----------|
| **20m** | ☐ Silhouette reads "sealed beacon, awaiting activation" | `__` |
| **20m** | ☐ Four-petal closed form obvious | `__` |
| **20m** | ☐ Red standby LED visible on one face | `__` |
| **20m** | ☐ Obelisk proportions (vertical emphasis) clear | `__` |
| **15m** | ☐ Petal seams visible (suggests opening mechanism) | `__` |
| **15m** | ☐ Base structure solid and distinct | `__` |
| **10m** | ☐ Red LED glow noticeable (dim but present) | `__` |
| **10m** | ☐ Sealed appearance obvious (no openings visible) | `__` |

**Validation Checklist:**
- [ ] Dormant beacon reads as "waiting, sealed" (not active, not derelict)
- [ ] Red LED visible and dim (standby indicator, not alarm)
- [ ] Silhouette clearly different from workbench and save point
- [ ] Four-petal form geometrically obvious
- [ ] No visual corruption or material issues

**Result (Dormant):** ✅ **PASS** / ❌ **NOT_READY**

### Test 2B: Beacon Active V1

**Setup:**
1. Import `TC_PROP_Beacon_Active_V1.glb`
2. Place at origin (0, 0, 0)
3. Same camera position as dormant (20m away, 1.7m height)

**Test Procedure:**

| Distance | Checklist | Pass/Fail |
|----------|-----------|-----------|
| **20m** | ☐ Silhouette reads "activated beacon, transmitting" | `__` |
| **20m** | ☐ Petals opened (4-way radial opening obvious) | `__` |
| **20m** | ☐ Purple crystal core glows and dominates | `__` |
| **20m** | ☐ Larger apparent silhouette than dormant | `__` |
| **15m** | ☐ Crystal is focal point (largest emissive element) | `__` |
| **15m** | ☐ Petal opening mechanism geometrically obvious | `__` |
| **10m** | ☐ Purple glow is intense and visible | `__` |
| **10m** | ☐ Crystal faceting detail visible | `__` |

**Validation Checklist:**
- [ ] Active beacon reads as "power on, transmission ready" (not dormant)
- [ ] Purple crystal dominates visual (victory signal)
- [ ] Petals opened (not just color change, actual geometry)
- [ ] Glow is intense and obvious (emissive ≥2.5)
- [ ] Transformation from dormant is clear and distinct

**Result (Active):** ✅ **PASS** / ❌ **NOT_READY**

### Test 2C: Dormant vs. Active Distinction

**Comparison Test (both models in view):**
1. Import both dormant and active in same scene (offset 5m apart)
2. View from 20m away
3. Confirm states are visually distinct

**Validation:**
- [ ] Dormant and active are clearly different (not subtle color change)
- [ ] Can identify state from distance without text/labels
- [ ] Transformation logic is semantically obvious (petals = activation)

**Result (Distinction):** ✅ **PASS** / ❌ **NOT_READY**

---

## Test 3: Save Point V1 Distance Visibility

**Objective:** Confirm cyan glow and pillar form readable from 10+ meters

**Setup:**
1. Import `TC_PROP_SavePoint_V1.glb`
2. Place at origin (0, 0, 0)
3. First-person camera at 1.7m height
4. Position 10m away from save point (along +Y axis)

**Test Procedure:**

| Distance | Checklist | Pass/Fail |
|----------|-----------|-----------|
| **10m** | ☐ Silhouette reads "checkpoint marker" (geometric pillar) | `__` |
| **10m** | ☐ Cyan glow ring/band obvious | `__` |
| **10m** | ☐ Height appropriate (taller than player, modest) | `__` |
| **10m** | ☐ Hexagonal or cylindrical form clear | `__` |
| **8m** | ☐ Cyan emissive material glows (visible in daylight) | `__` |
| **8m** | ☐ Base is solid and grounded (not floating) | `__` |
| **6m** | ☐ Pillar interior structure suggested (panel lines, details) | `__` |
| **6m** | ☐ Glow ring position (middle of pillar) obvious | `__` |

**Validation Checklist:**
- [ ] Save point reads as "checkpoint/progress anchor" (not decoration)
- [ ] Cyan glow visible and distinct (emissive ≥1.2)
- [ ] Geometric pillar form clear (not blob)
- [ ] Scale feels personal and approachable (not monumental)
- [ ] Silhouette clearly different from workbench and beacon
- [ ] No visual corruption or material issues

**Result:** ✅ **PASS** / ❌ **NOT_READY**

---

## Test 4: Cross-Type Distinction (All Three Together)

**Objective:** Confirm all three interactable types are visually distinct and identifiable

**Setup:**
1. Create test scene with all three prop types
2. Space 10–15m apart in a triangle or line
3. First-person camera at 1.7m height, viewing all three from ~12m distance

**Test Procedure:**

| Aspect | Checklist | Pass/Fail |
|--------|-----------|-----------|
| **Workbench** | ☐ Orange accent immediately obvious | `__` |
| **Workbench** | ☐ Arm silhouette distinguishes from save point and beacon | `__` |
| **Beacon Dormant** | ☐ Vertical obelisk form stands out | `__` |
| **Beacon Dormant** | ☐ Red glow differs from cyan and orange | `__` |
| **Save Point** | ☐ Cyan glow immediately recognizable | `__` |
| **Save Point** | ☐ Pillar form different from bench and beacon | `__` |
| **All Three** | ☐ No confusion between any pair at 12m distance | `__` |
| **All Three** | ☐ Color language supports distinction (orange ≠ red ≠ cyan) | `__` |

**Validation Checklist:**
- [ ] Each prop type is immediately identifiable by silhouette
- [ ] Color language (orange workbench, red beacon, cyan save point) is clear
- [ ] No two types read as the same or similar
- [ ] Visual hierarchy makes sense (workbench approachable, beacon monumental, save point minimal)
- [ ] Material accents (emissive glows) are all visible and distinct

**Result:** ✅ **PASS** / ❌ **NOT_READY**

---

## Summary Results Table

Fill in after all tests complete:

| Test | Asset | Distance | Result | Notes |
|------|-------|----------|--------|-------|
| 1 | Workbench V1 | 10m+ | ✅/❌ | |
| 2A | Beacon Dormant V1 | 20m+ | ✅/❌ | |
| 2B | Beacon Active V1 | 20m+ | ✅/❌ | |
| 2C | Dormant vs. Active | Side-by-side | ✅/❌ | |
| 3 | Save Point V1 | 10m+ | ✅/❌ | |
| 4 | All Three Together | 12m | ✅/❌ | |

---

## Troubleshooting Guide

| Issue | Symptom | Solution |
|-------|---------|----------|
| **Glow not visible** | Emissive material appears dark | Increase emissive strength in material; check if material is assigned correctly |
| **Silhouette mushy** | Edges unclear, form undefined | Add bevels or edge detail; increase geometric clarity; check lighting |
| **Scale feels wrong** | Prop too large or too small | Compare to 1.8m player reference; adjust scale uniformly |
| **Color indistinct** | Emissive color hard to read | Increase saturation; boost emissive strength; check viewer color space |
| **Material corruption** | Black spots, stretching, or artifacts | Check UV mapping; validate in Blender Material Preview; re-export GLB |
| **Can't distinguish types** | Props look too similar | Emphasize color differences; add more silhouette variety; boost glow intensity |

---

## Test Environment Options

### Option A: Godot 4 First-Person Viewer (Recommended)
**Advantages:**
- Native FPS perspective
- Real-time emissive material preview
- Easy camera control (move/rotate to test distances)
- Matches final gameplay view

**Steps:**
1. Create new Godot scene
2. Import GLB as Node3D
3. Create Player (Camera3D at 1.7m height)
4. Add simple WASD/mouse control
5. Position props at known distances
6. Test from FPS view

### Option B: Blender Material Preview Mode
**Advantages:**
- Native to asset creation
- Material preview shows glows accurately
- Can rotate camera around asset

**Disadvantages:**
- Not first-person perspective
- Less intuitive for gameplay distance testing

### Option C: Standalone GLB Viewer (babylon.js, three.js)
**Advantages:**
- No Godot/Blender dependency
- Web-based, cross-platform
- Easy to import multiple GLBs

**Disadvantages:**
- May not match final rendering exactly
- FPS camera angle requires setup

---

## Approval Workflow

**Phase 3 is `ASSET_IMPLEMENTATION_PASS` when:**

1. ✅ Workbench readable and identifiable at 10+ meters
2. ✅ Beacon Dormant sealed appearance obvious at 20+ meters
3. ✅ Beacon Active crystal glow and petal opening obvious at 20+ meters
4. ✅ Save Point cyan glow and pillar form readable at 10+ meters
5. ✅ All three types visually distinct from each other
6. ✅ No visual corruption or material errors in any prop
7. ✅ Color language (orange, red, cyan) supports distinction
8. ✅ Emissive materials glow correctly and are visible at distance

**Gate:** `VISUAL_SLICE_GAMEPLAY_SAFE` — Props ready for scene composition

---

## Next Steps

After Phase 3 `ASSET_IMPLEMENTATION_PASS`:

1. Phase 4–6 assets complete (auto-generated)
2. **Phase 7: Scene Composition** — Integrate all 1–6 assets into single diorama scene
3. Phase 8: Lighting/Materials refinement
4. Phase 9: Visual artifact factory

**Estimated completion:** Phase 3 tests ~1 hour; Phase 4–6 automation ~15 min; Phase 7 begins immediately after both complete.
