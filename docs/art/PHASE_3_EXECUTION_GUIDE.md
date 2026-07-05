# Phase 3 Execution Guide: Interactables (Workbench, Save, Beacon)
## Refinement, Visibility Testing & Review Artifacts

**Status:** Ready to Execute  
**Brief References:**
- `docs/art/briefs/brief-workbench-v1.md`
- `docs/art/briefs/brief-beacon-v1.md`
- `docs/art/briefs/brief-save-point-v1.md`

**Estimated Time:** ~1.5–2 hours (assessment + refinement + capture)  
**Success Gate:** `ASSET_IMPLEMENTATION_PASS`  

---

## Prerequisites Checklist

Before starting Phase 3, confirm:

- [ ] Phase 1 (Terrain) completed and validated
- [ ] Phase 2 (Hull) completed and validated
- [ ] All three briefs read and understood:
  - `docs/art/briefs/brief-workbench-v1.md`
  - `docs/art/briefs/brief-beacon-v1.md`
  - `docs/art/briefs/brief-save-point-v1.md`
- [ ] Blender 4.0+ installed (for any refinements)
- [ ] Godot 4 .NET available for visibility testing
- [ ] ~1.5–2 hours available for assessment and refinement

---

## Overview: Three Assets, Two Execution Paths

**Assets under review:**
1. **TC_PROP_Workbench_V1** — Crafting hub (orange accent)
2. **TC_PROP_Beacon_Dormant_V1** — Checkpoint inactive (red LED)
3. **TC_PROP_Beacon_Active_V1** — Checkpoint activated (purple crystal)
4. **TC_PROP_SavePoint_V1** — Save point (cyan glow)

These assets already exist in the MVP Asset Pack V1 manifest. Phase 3 determines if they meet the brief specs or need refinement.

---

## Execution Path A: Quick Validation (If Assets Match Briefs)

If existing MVP Pack V1 assets meet brief specifications, validation is straightforward:

### Step 1: Load Assets in Godot

```bash
cd /home/user/TitanCraft
godot --headless --path . --import
```

Then open a test scene and instantiate all three assets:

```gdscript
# Test scene: load all interactables
var workbench = preload("res://assets/Production/Generated/MVP_Pack_V1/TC_PROP_Workbench_V1.glb").instantiate()
var beacon_dormant = preload("res://assets/Production/Generated/MVP_Pack_V1/TC_PROP_Beacon_Dormant_V1.glb").instantiate()
var beacon_active = preload("res://assets/Production/Generated/MVP_Pack_V1/TC_PROP_Beacon_Active_V1.glb").instantiate()
var save_point = preload("res://assets/Production/Generated/MVP_Pack_V1/TC_PROP_SavePoint_V1.glb").instantiate()

add_child(workbench)
add_child(beacon_dormant)
add_child(beacon_active)
add_child(save_point)

# Position them for visibility testing
workbench.position = Vector3(0, 0, 0)
beacon_dormant.position = Vector3(20, 0, 0)
beacon_active.position = Vector3(40, 0, 0)
save_point.position = Vector3(10, 0, -10)
```

### Step 2: Gameplay Distance Visibility Test

**Objective:** Verify each asset is identifiable from first-person at expected gameplay distances

**Test Setup:**
- Load terrain + all three interactables in a scene
- Player starts at spawn, 20–30m from assets
- Use first-person camera (1.7m eye height)

**Visibility Checklist:**

**Workbench (10+ meters away):**
- [ ] Identifiable as crafting station (not just "structure")
- [ ] Orange panel visible and distinct
- [ ] Arm silhouette readable (suggests assembly)
- [ ] Does NOT read as workstation decoration

**Beacon Dormant (20+ meters away):**
- [ ] Identifiable as beacon/objective marker
- [ ] Red LED visible (signals "waiting")
- [ ] Four-petal closed form readable
- [ ] Does NOT confuse with workbench or save point

**Beacon Active (20+ meters away):**
- [ ] Identifiable as activated beacon
- [ ] Purple crystal glow dominant
- [ ] Petals opening/opened visible (if mesh swap)
- [ ] Reads as "transmission active"

**Save Point (10+ meters away):**
- [ ] Identifiable as checkpoint/save location
- [ ] Cyan glow ring visible and distinct
- [ ] Pillar form readable (geometric, minimal)
- [ ] Does NOT confuse with workbench or beacon

### Step 3: Assess Against Brief Requirements

For each asset, verify:

**Workbench Brief Checklist:**
- [ ] Orange emissive material present and glowing
- [ ] Articulated arm identifiable (3–4 segments)
- [ ] Holographic panel tilted ~45°
- [ ] Material palette: beige hull + dark steel + orange accents ✓
- [ ] Poly count reported in manifest ≤ 3,500
- [ ] No visual corruption (clipping, stretching, gaps)

**Beacon Brief Checklist:**
- [ ] **Dormant:** Closed petal form, red LED visible, sealed appearance
- [ ] **Active:** Opened petals (or state change), purple crystal emissive, focal point
- [ ] Material palette: dark base + purple crystal (emissive) + orange accents (edges)
- [ ] Poly counts reported in manifest: Dormant ≤ 2,000, Active ≤ 2,800
- [ ] Both states' silhouettes are distinct and readable
- [ ] Red and purple emissive materials export/display correctly

**Save Point Brief Checklist:**
- [ ] Hexagonal or cylindrical pillar form
- [ ] Cyan glowing ring/band around middle
- [ ] Geometric, minimal design (not decorated)
- [ ] Material palette: dark tech-gray base + cyan accent
- [ ] Poly count reported in manifest ≤ 1,600
- [ ] Cyan emissive material glows correctly

### Step 4: If All Checks Pass

If all visibility tests pass and briefs are met:

**Report:** "All three interactables pass visibility validation. Poly counts and materials align with brief specs."

**Proceed to:** Step 5 (Capture Review Artifacts)

---

## Execution Path B: Refinement Needed

If any asset fails visibility or brief checks:

### Step 1: Identify Issues

**Common problems:**

**Workbench issues:**
- Orange accent too subtle → Increase emissive strength or saturation
- Arm not readable → Simplify arm geometry, increase contrast
- Panel not obvious → Increase tilt, add glow effect

**Beacon issues:**
- Dormant red LED not visible → Increase red emissive strength, enlarge indicator
- Active purple not glowing → Check emissive strength export setting
- Petal opening ambiguous → Increase petal separation, add geometric articulation

**Save Point issues:**
- Cyan ring too subtle → Increase emissive strength to 1.5+
- Pillar confuses with beacon → Reduce height, add more geometry distinction
- Not readable at distance → Increase cyan glow intensity

### Step 2: Refine in Blender

Open the source `.blend` files:

```bash
blender assets/Source/Blender/Production/MVP_Pack_V1/TC_PROP_Workbench_V1.blend
blender assets/Source/Blender/Production/MVP_Pack_V1/TC_PROP_Beacon_Dormant_V1.blend
blender assets/Source/Blender/Production/MVP_Pack_V1/TC_PROP_Beacon_Active_V1.blend
blender assets/Source/Blender/Production/MVP_Pack_V1/TC_PROP_SavePoint_V1.blend
```

**Common refinements:**

**Emissive Material Strength:**
- Workbench orange: Try 0.8–1.0 (bright, not overwhelming)
- Beacon red: Try 0.8–1.2 (visible but standby, not active)
- Beacon purple: Try 2.0–3.0 (bright, dominant when active)
- Save cyan: Try 1.2–1.5 (calm, welcoming glow)

**Geometry Adjustments:**
- Arm segments: Reduce overlaps, increase visibility of joints
- Beacon petals: Increase separation angle when "opened" (more dramatic opening)
- Cyan ring: Increase height/thickness for better visibility at distance

**Test After Refinement:**
1. Export to GLB
2. Re-import in Godot
3. Re-test visibility from first-person at expected distances
4. Verify emissive materials display correctly

### Step 3: If Still Failing

If refinement doesn't fix issues:

**Document:** Write brief note on what failed and why

**Options:**
1. Escalate for human review (may require larger redesign)
2. Accept current state as "good enough for MVP" (if functionally acceptable)
3. Request different brief or approach

---

## Step 5: Capture Review Artifacts

Once all three assets pass visibility validation:

### Capture 1: Side-by-Side Comparison (All Three)

In Godot, position all three assets in a row, capture neutral screenshot:

```gdscript
# Position for side-by-side comparison
workbench.position = Vector3(-10, 0, 0)
save_point.position = Vector3(0, 0, 0)
beacon_active.position = Vector3(10, 0, 0)  # Use active for full visual

# Capture from ~15m distance, first-person perspective
get_viewport().get_texture().get_image().save_png("artifacts/review/phase-3/all-interactables-comparison.png")
```

**Output:** `artifacts/review/phase-3/all-interactables-comparison.png`
**Purpose:** Shows scale comparison, material/color distinction between assets

### Capture 2: Workbench Detail (3 angles)

From first-person at gameplay distance (~10m):

1. **Front approach:** Head-on view, orange panel obvious
2. **Side view:** Arm articulation visible, bench profile clear
3. **Overhead perspective:** Whole bench silhouette

**Angles:** 0°, 90°, 180° rotation around asset

**Output:**
- `artifacts/review/phase-3/TC_PROP_Workbench_front.png`
- `artifacts/review/phase-3/TC_PROP_Workbench_side.png`
- `artifacts/review/phase-3/TC_PROP_Workbench_detail.png`

**Purpose:** Verify orange accent is visible, arm is readable, silhouette is functional

### Capture 3: Beacon Dormant (3 angles)

From first-person at gameplay distance (~20m):

1. **Front approach:** Red LED visible, sealed form clear
2. **Side view:** Petal seams visible, four-part structure readable
3. **Close-up:** Red LED detail

**Angles:** 0°, 90°, 180°

**Output:**
- `artifacts/review/phase-3/TC_PROP_Beacon_Dormant_front.png`
- `artifacts/review/phase-3/TC_PROP_Beacon_Dormant_side.png`
- `artifacts/review/phase-3/TC_PROP_Beacon_Dormant_led.png`

**Purpose:** Confirm dormant state is distinct, red LED is visible, petal structure is clear

### Capture 4: Beacon Active (3 angles)

From first-person at gameplay distance (~20m):

1. **Petals opened:** Full opening visible, crystal core exposed
2. **Detail on crystal:** Purple glow, faceted structure
3. **Full beacon:** Whole activated appearance

**Angles:** 0°, 90°, Crystal close-up

**Output:**
- `artifacts/review/phase-3/TC_PROP_Beacon_Active_petals.png`
- `artifacts/review/phase-3/TC_PROP_Beacon_Active_crystal.png`
- `artifacts/review/phase-3/TC_PROP_Beacon_Active_full.png`

**Purpose:** Confirm active state is visually distinct, crystal glow is obvious, petals are opened

### Capture 5: Save Point (3 angles)

From first-person at gameplay distance (~10m):

1. **Front:** Cyan ring visible, pillar proportions clear
2. **Side:** Height and glow band obvious
3. **Detail:** Cyan emissive glow close-up

**Angles:** 0°, 90°, Glow detail

**Output:**
- `artifacts/review/phase-3/TC_PROP_SavePoint_front.png`
- `artifacts/review/phase-3/TC_PROP_SavePoint_side.png`
- `artifacts/review/phase-3/TC_PROP_SavePoint_glow.png`

**Purpose:** Confirm cyan glow is visible, pillar is minimal/geometric, distinctly different from workbench/beacon

### Capture 6: Gameplay Distance Test

**Screenshot showing all three together** at expected in-game distances:

```
Spawn → 30m away: Terrain + Workbench + Beacon + Save Point
```

Show:
- Player can see all three simultaneously (if positioned correctly)
- Each asset is distinguishable by silhouette and color
- Orange (workbench) ≠ Cyan (save) ≠ Purple (beacon) is obvious

**Output:** `artifacts/review/phase-3/gameplay-distance-all-visible.png`

**Purpose:** Validate gameplay composition, all interactables readable at distance

---

## Step 6: Update Manifest with Review Artifacts

Once all PNGs are captured, update asset manifest to reference review artifacts:

```bash
python3 tools/blender/build_asset_manifest.py
```

Then manually verify manifest entries include `review_artifacts` field:

```json
{
  "asset_name": "TC_PROP_Workbench_V1",
  "review_artifacts": [
    "artifacts/review/phase-3/TC_PROP_Workbench_front.png",
    "artifacts/review/phase-3/TC_PROP_Workbench_side.png",
    "artifacts/review/phase-3/TC_PROP_Workbench_detail.png"
  ],
  ...
}
```

---

## Validation Checklist

**Visibility Testing:**
- [ ] Workbench identifiable from 10+ meters ✓
- [ ] Beacon dormant identifiable from 20+ meters ✓
- [ ] Beacon active identifiable from 20+ meters (state distinct) ✓
- [ ] Save point identifiable from 10+ meters ✓
- [ ] All three are visually distinct from each other ✓

**Brief Conformance:**
- [ ] Workbench: Orange panel visible, arm readable, bench form clear ✓
- [ ] Beacon dormant: Red LED visible, sealed form, four-petal structure ✓
- [ ] Beacon active: Purple crystal glowing, petals opened/distinct, focal point ✓
- [ ] Save point: Cyan glow ring, geometric pillar, minimal aesthetic ✓

**Material & Color:**
- [ ] Orange (workbench) emissive ✓
- [ ] Red (beacon dormant) emissive ✓
- [ ] Purple (beacon active) strongly emissive ✓
- [ ] Cyan (save point) emissive ✓
- [ ] All colors distinct and appropriate ✓

**Review Artifacts:**
- [ ] Workbench 3-angle captures ✓
- [ ] Beacon dormant 3-angle captures ✓
- [ ] Beacon active 3-angle captures ✓
- [ ] Save point 3-angle captures ✓
- [ ] All-together comparison ✓
- [ ] Gameplay distance validation ✓
- [ ] All PNGs referenced in manifest ✓

**Asset Management:**
- [ ] All three GLBs in asset manifest ✓
- [ ] Poly counts recorded and ≤ budget ✓
- [ ] Material zones named correctly ✓
- [ ] Review artifacts linked in manifest ✓
- [ ] SHA256 hashes verified ✓

---

## Success Criteria

✅ **Phase 3 is `ASSET_IMPLEMENTATION_PASS` when:**

1. All three interactables loaded and validated in Godot
2. **Workbench** identifiable as crafting hub from 10+ meters
   - Orange panel visible and distinct
   - Arm silhouette readable
   - Bench structure clear
3. **Beacon Dormant** identifiable from 20+ meters
   - Red LED visible
   - Sealed form obvious
   - Four-petal structure readable
4. **Beacon Active** identifiable from 20+ meters
   - Purple crystal glowing (distinct from dormant)
   - Petals opened/separated
   - Reads as "activated transmission"
5. **Save Point** identifiable from 10+ meters
   - Cyan glow ring visible
   - Geometric pillar form clear
   - Distinct from workbench and beacon
6. Material/color language coherent across all three
   - Orange = interactive craft
   - Cyan = safe checkpoint
   - Purple = victory objective
7. Poly counts within budget (verified in manifest)
   - Workbench ≤ 3,500
   - Beacon dormant ≤ 2,000
   - Beacon active ≤ 2,800
   - Save point ≤ 1,600
8. No visual corruption, clipping, or material errors
9. Review artifacts captured and referenced in manifest (15+ PNGs total)
10. All SHA256 hashes recorded and verified

❌ **Phase 3 will be `NOT_GO` if:**

- Any asset fails visibility test at expected distance
- Color/emissive language is confusing (hard to distinguish orange/cyan/purple)
- Workbench arm is ambiguous or unreadable
- Beacon states (dormant/active) are not visually distinct
- Save point confuses with other interactables
- Poly counts exceed budget
- Emissive materials do not export/display correctly
- Review artifacts are missing or insufficient

---

## Next Steps (After Phase 3 Complete)

1. Commit Phase 3 manifest updates and all review artifacts
2. Push to branch; GitHub Actions validates
3. Obtain human visual review verdict
4. **If approved:** Proceed to Phase 4 (Pickups)
5. **If remediation needed:** Return to Blender, address feedback, re-capture and re-validate

**Timeline:** Phase 3 complete → Phase 4 start (resource pickups)

---

## Summary: What Phase 3 Does

**Input:** Three pre-existing MVP interactable assets
**Process:** Assess visibility at gameplay distance, validate against briefs, capture review artifacts
**Output:** Validated interactables with visibility evidence and review artifact documentation

**Key deliverables:**
- Visibility test evidence (screenshots)
- Updated manifest with review artifact references
- Approval verdict: `ASSET_IMPLEMENTATION_PASS` or remediation needed

**Effort:** ~1.5–2 hours (mostly testing and screenshot capture)

---

## Troubleshooting

### Problem: Asset not visible from expected distance

**Cause:** Emissive material too dim, silhouette too small, or position unclear

**Solution:**
1. Increase emissive strength (0.5→1.2 range)
2. Check asset scale (measure in Godot, compare to brief)
3. Position asset in higher-contrast area (against terrain)
4. Re-test visibility, re-capture artifact

### Problem: Assets confuse with each other

**Cause:** Color or silhouette too similar, emissive language ambiguous

**Solution:**
1. Increase emissive strength differences (orange 0.8, cyan 1.2, purple 2.5)
2. Adjust saturation if needed (orange brighter, cyan cooler, purple deeper)
3. Ensure silhouettes are distinct (workbench wide, save minimal, beacon tall)
4. Side-by-side comparison test, re-capture

### Problem: Emissive material not glowing in Godot

**Cause:** Export setting missing, material not properly configured

**Solution:**
1. In Blender, ensure Principled BSDF uses Emission Strength (not just Emissive)
2. Export with `--export-emission-strength` flag (if using custom export)
3. Re-import in Godot, check material inspector
4. Verify emissive is rendering (may need Godot material override)

### Problem: Manifest update fails or review artifacts not linked

**Cause:** PNG paths incorrect, manifest schema mismatch

**Solution:**
1. Verify PNG files exist: `ls artifacts/review/phase-3/*.png`
2. Update manifest manually, add `review_artifacts` array to each entry
3. Use `python3 tools/blender/build_asset_manifest.py --check` to validate
4. Commit manifest and all PNG files

---

## Support & Questions

If you hit blockers:

1. **Brief confusion:** Review respective briefs again (workbench/beacon/save point)
2. **Godot visibility issues:** Ensure camera is at 1.7m eye height, assets properly positioned
3. **Material/emissive problems:** Check Blender Principled BSDF emissive strength, re-export
4. **Visibility test failure:** May indicate asset redesign needed; escalate for human review

Good luck with Phase 3! Report completion once:
- All visibility tests pass
- Review artifacts captured
- Manifest updated
- Approval verdict: PASS or remediation needed
