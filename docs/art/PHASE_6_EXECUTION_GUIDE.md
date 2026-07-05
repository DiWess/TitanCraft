# Phase 6 Execution Guide: Mechanical Arm V1
## Player Equipment — Wrist-Mount Exoskeleton Tool

**Phase:** 6  
**Target Asset:** TC_PLAYER_MechanicalArm_V1  
**Scope:** Single player-mounted mechanical arm equipment mesh  
**Effort:** ~1.5–2.5 hours (single authoring pass)  
**Status:** `READY_FOR_AUTHORING`  
**Next Gate:** `ASSET_IMPLEMENTATION_PASS` (FPS POV readability validation)

---

## Reference Documents

- **Brief:** `docs/art/briefs/brief-mechanical-arm-v1.md` — full specifications
- **Color Language:** Dark industrial steel, hydraulic accent (orange or cyan), salvage aesthetic
- **Poly Budget:** ~1,800 target, ≤2,200 max
- **Visibility requirement:** Readable and menacing in FPS POV (lower-right corner, melee range)

---

## Execution Paths

Choose one based on Blender expertise and time availability:

### Path A: Guided Blender Authoring (Recommended)

**Duration:** ~1.5–2.5 hours  
**Skill Level:** Intermediate Blender (modeling, mechanical detail, material setup)  
**Advantage:** Full creative control, direct iteration on FPS feel  
**Disadvantage:** Requires Blender expertise, moderate complexity

**Process:**
1. Open Blender 4.0+
2. Create new `.blend`: `assets/Source/Blender/Production/MVP_Pack_V1/TC_PLAYER_MechanicalArm_V1.blend`
3. Model arm components:
   - Wrist cuff (15 min)
   - Forearm extension (15 min)
   - Hydraulic system and lines (15 min)
   - Gripper/fist end-effector (30 min)
   - Accent detail and weathering (20 min)
4. Apply materials and color specification (15 min)
5. Set hydraulic accent color (orange or cyan, emissive if desired)
6. UV unwrap (5–10 min)
7. Validate, scale check, and save (5 min)

**Modeling Breakdown:**

| Component | Duration | Complexity | Key Details |
|-----------|----------|-----------|------------|
| **Wrist Cuff** | 15 min | Medium | Curved/cylindrical body, hinged at wrist, straps or clamps |
| **Forearm Ext.** | 15 min | Low–Medium | Tubular I-beam or box shape, support struts, mounting points |
| **Hydraulics** | 15 min | Medium | Line routing, piston rod geometry, conduit manifolds |
| **Gripper/Fist** | 30 min | Medium–High | Articulated fingers or knuckle-armored fist, menacing tips |
| **Detail & Wear** | 20 min | Low | Bolts, rivets, weathering, battle damage, asymmetry |
| **Materials & UV** | 15 min | Low | Color spec, metallic materials, efficient unwrap |
| **Validation** | 5 min | — | Scale check, manifold validation, naming |

**Modeling Tips:**

- **Cuff proportions:** ~0.35m long (wrist to mid-forearm), ~0.1m diameter (worn on arm)
- **Gripper/fist:** ~0.25m long (menacing, visible in FPS corner)
- **Hydraulics:** Visual impact without excessive geometry; curved lines suggest power
- **Weathering:** Rust, dents, scorch marks add salvage authenticity
- **Articulation:** Show hinged wrist joint and gripper movement hints
- **Color separation:** Dark body + bright hydraulic accent creates visual hierarchy
- **Detail density:** Concentrate detail at gripper/fist (end-effector is focal point)

### Path B: Python Script Generation (If Script Exists)

**Duration:** ~5 min  
**Skill Level:** Minimal  
**Advantage:** Deterministic, reproducible  
**Disadvantage:** Requires pre-built script

**Process:**
```bash
# Verify script exists
grep -l "MechanicalArm" tools/blender/create_mvp_asset_pack_v1.py

# Generate arm
blender --background --python tools/blender/create_mvp_asset_pack_v1.py

# Verify output
ls -la assets/Source/Blender/Production/MVP_Pack_V1/TC_PLAYER_MechanicalArm_V1.blend
```

After script completes: Proceed to Path B→Post-Generation

### Path C: Rapid Placeholder (Equipment Outline Only)

**Duration:** ~15 min  
**Skill Level:** Minimal  
**Advantage:** Fastest to FPS testing  
**Disadvantage:** Geometry is temporary; requires refinement

**Process:**
1. Import simple gripper/arm placeholder
2. Scale to 0.35–0.5m overall length (wrist mount)
3. Apply dark steel + orange/cyan accent materials
4. Rename and save to correct path
5. Proceed to post-export

---

## Authoring Checklist (Path A Only)

During Blender authoring of Mechanical Arm:

**Geometry Phase:**
- [ ] Cuff is curved/cylindrical (wraps around arm-sized form)
- [ ] Cuff hinge visible at wrist joint (suggests articulation)
- [ ] Forearm extension distinct from cuff (separate mesh volume)
- [ ] Forearm is tubular (I-beam or box shape, structural)
- [ ] Support struts or ribbing visible (suggests load-bearing)
- [ ] Gripper or fist is large and menacing relative to arm
- [ ] Gripper has articulation hints (jointed fingers or knuckle lines)
- [ ] Claws or impact face is sharp/threatening (not blunt or cute)
- [ ] Overall length ~0.6m (cuff + forearm + gripper combined)
- [ ] Proportions fit human arm-mount (not oversized, not tiny)
- [ ] No internal faces or manifold errors

**Hydraulic System Phase:**
- [ ] Main hydraulic lines run full arm length (visual prominence)
- [ ] Piston rod geometry visible (suggests mechanical actuator)
- [ ] Conduit manifolds at cuff and gripper (connection points)
- [ ] Line routing follows arm form (not random, appears functional)
- [ ] Tubing color chosen: orange (RGB ~240, 140, 60) or cyan (RGB ~100, 220, 255)
- [ ] Accent color distinct from body (bright vs. dark contrast)

**Material Phase:**
- [ ] Primary dark steel: RGB ~90, 90, 100, Metalness 0.4, Roughness 0.75
- [ ] Gripper/fist dark material: RGB ~60, 60, 70, Metalness 0.3, Roughness 0.85
- [ ] Hydraulic accent: bright color, metallic (0.6 metalness), smooth (0.4 roughness)
- [ ] Rivets/fasteners: bright steel (RGB ~180, 180, 190), metallic (0.8)
- [ ] Weathering/scorch: very dark (RGB ~40, 35, 30), rough (0.9)
- [ ] All materials assigned and visible in Material Preview
- [ ] No texture stretching or undefined zones

**Articulation Phase:**
- [ ] Wrist hinge geometrically obvious (bent joint)
- [ ] Gripper/fist articulation hints visible (separated fingers or knuckle geometry)
- [ ] Movement range suggested without full animation

**Validation Phase (In Blender):**
- [ ] Switch to Material Preview: colors read correctly, hydraulic accent is bright
- [ ] Silhouette reads "salvage tool/weapon" (not sleek, not cosmetic)
- [ ] Cuff proportions fit arm-mount (not oversized)
- [ ] Gripper/fist is focal point (detailed, menacing)
- [ ] Weathering/patina obvious (salvage aesthetic)
- [ ] Hydraulic system visually prominent (suggests function)
- [ ] No visual defects, black spots, or rendering errors
- [ ] Object name is `TC_PLAYER_MechanicalArm_V1`
- [ ] Single mesh object in scene

**Scale Validation:**
- [ ] Total length ~0.6m (measure in Blender units; 0.35m cuff + 0.25m gripper)
- [ ] Cuff diameter ~0.1m (fits human arm when worn)
- [ ] Gripper/fist ~0.25m (visible and menacing in FPS POV)

**Save:**
- [ ] File name: `TC_PLAYER_MechanicalArm_V1.blend`
- [ ] Location: `assets/Source/Blender/Production/MVP_Pack_V1/`
- [ ] Blender 4.0+ format

---

## Post-Authoring Workflow (All Paths)

### Step 1: Validate Source `.blend` File

```bash
ls -la assets/Source/Blender/Production/MVP_Pack_V1/TC_PLAYER_MechanicalArm_V1.blend
```

### Step 2: Export to GLB

```bash
blender --background --python tools/blender/export_asset.py -- \
  assets/Source/Blender/Production/MVP_Pack_V1/TC_PLAYER_MechanicalArm_V1.blend \
  assets/Production/Generated/MVP_Pack_V1/TC_PLAYER_MechanicalArm_V1.glb

# Verify export
ls -lh assets/Production/Generated/MVP_Pack_V1/TC_PLAYER_MechanicalArm_V1.glb
```

### Step 3: Validate Export

```bash
python3 tools/blender/validate_blender_asset.py \
  assets/Source/Blender/Production/MVP_Pack_V1/TC_PLAYER_MechanicalArm_V1.blend
```

Expected output:
```
validation_status: PASS
triangle_count: ~1800
material_slots: [dark_steel, gripper, hydraulic_accent, etc.]
```

### Step 4: Rebuild Asset Manifest

```bash
python3 tools/blender/build_asset_manifest.py
```

Verify arm entry added:
```
ASSET_MANIFEST_WRITTEN assets/Production/Generated/asset_manifest.json entries=<N>
```

### Step 5: Capture Review Artifacts

```bash
# Create review directory
mkdir -p artifacts/asset-review/TC_PLAYER_MechanicalArm_V1

# Render turntables (show gripper/fist and side-mount detail)
blender --background --python tools/blender/render_mvp_asset_review.py -- \
  assets/Source/Blender/Production/MVP_Pack_V1/TC_PLAYER_MechanicalArm_V1.blend \
  artifacts/asset-review/TC_PLAYER_MechanicalArm_V1
```

---

## FPS POV Readability Testing (In Godot or Viewer)

### Test Procedure: First-Person Arm Validation

**Goal:** Confirm arm is readable, menacing, and appropriate in FPS lower-right POV

**Setup:**
1. Import arm GLB into Godot
2. Attach to player avatar (right wrist position)
3. Test in first-person view
4. Check readability in lower-right corner
5. Assess melee interaction range visibility

**Validation Checklist:**

**Visual Readability:**
- [ ] Arm visible and clear in lower-right POV corner
- [ ] Cuff proportions fit wrist-mount (not oversized, not tiny)
- [ ] Gripper/fist is focal point and menacing
- [ ] Hydraulic lines visible and distinct (bright accent color)
- [ ] Weathering and detail visible (not mushy, not featureless)
- [ ] Overall silhouette reads "salvage tool/weapon"

**Technical:**
- [ ] Mesh imports without errors
- [ ] Materials display correctly (dark body, bright accent)
- [ ] Hydraulic accent color is obvious and distinct
- [ ] No visual corruption, clipping, or geometry issues
- [ ] Scale appropriate for wrist-mount (not blocking view, not tiny)

**Functional Feel:**
- [ ] Arm suggests griping/grasping or striking capability
- [ ] Presence is empowering to player
- [ ] Threatens to enemies (menacing, not harmless)
- [ ] Implies melee range (~0.5m reach)

**If test fails:** Mark `NOT_READY` and refine:
- Increase hydraulic accent brightness or saturation
- Add more detail to gripper/fist (knuckles, joints, spikes)
- Adjust scale if arm appears oversized or undersized
- Enhance weathering visibility for salvage aesthetic

---

## Success Criteria

✅ **Phase 6 is `ASSET_IMPLEMENTATION_PASS` when:**

1. Source `.blend` file exists: `assets/Source/Blender/Production/MVP_Pack_V1/TC_PLAYER_MechanicalArm_V1.blend`
2. GLB export successful: `assets/Production/Generated/MVP_Pack_V1/TC_PLAYER_MechanicalArm_V1.glb`
3. Validation reports PASS, ~1,800 triangles
4. Asset manifest entry created with all metadata
5. Silhouette reads **salvage wrist-mount exoskeleton** (functional, tool-like, menacing)
6. Cuff proportions correct for arm-mount (0.35m length, 0.1m diameter)
7. Gripper/fist is large, detailed, and menacing (focal point)
8. Hydraulic system obvious (lines, manifolds, color accent visible)
9. Weathering and material patina evident (salvage aesthetic)
10. Material palette coherent (dark steel, bright hydraulic accent, rivet detail)
11. Poly count ≤ 2,200
12. Turntable PNGs captured (2–3 angles showing gripper/fist detail)
13. FPS POV readability test passed: arm readable and menacing in lower-right corner

**Expected manifest entry:**
```json
{
  "asset_name": "TC_PLAYER_MechanicalArm_V1",
  "classification": "production-candidate",
  "triangle_count": ~1800,
  "validation_status": "PASS",
  "material_slots": ["dark_steel", "gripper_surface", "hydraulic_accent", "rivet_detail", "weathering"]
}
```

---

## Troubleshooting

| Issue | Symptom | Solution |
|-------|---------|----------|
| **Looks cosmetic** | Arm appears decorative, not functional | Add mechanical detail, hydraulic lines, articulation hints; emphasize tool/weapon feel |
| **Cuff too large** | Wrist-mount blocks hand/movement | Reduce cuff diameter to ~0.1m; ensure forearm extension doesn't extend past elbow |
| **Gripper/fist not menacing** | End-effector appears weak or cute | Add sharp edges, knuckle reinforcement, spikes, or threatening geometry |
| **Hydraulic not visible** | Accent color too dark or small | Increase hydraulic line prominence, size, and color saturation; ensure RGB ~240,140,60 or ~100,220,255 |
| **Scale feels wrong in FPS** | Arm too large or too small | Target is 0.6m total (cuff 0.35m, gripper 0.25m); test with player avatar reference |
| **Weathering absent** | Looks pristine/new, not salvaged | Add rust, dents, scratches, scorch marks, patina; use very dark materials (~RGB 40,35,30) |
| **Poly count too high** | > 2,200 triangles | Reduce line routing complexity, detail in cuff segments, or finger geometry |
| **Materials don't export** | Color looks wrong in GLB | Check material assignment in Blender; verify Principled BSDF setup; re-export |
| **Export fails** | GLB not created | Check Blender console for errors; validate mesh (no self-intersections); ensure valid object |

---

## Deployment Checklist

Before closing Phase 6:

- [ ] Source `.blend` file committed
- [ ] GLB export in artifact bundle
- [ ] Asset manifest updated
- [ ] FPS POV readability test passed
- [ ] Review PNG artifacts captured
- [ ] Brief document finalized
- [ ] No outstanding TODOs or FIXME

---

## Effort Summary

**Path A (Guided Blender):** ~1.5–2.5 hours (moderate complexity, functional detail)  
**Path B (Script):** ~5 minutes (if script ready)  
**Path C (Rapid Placeholder):** ~15 minutes + later refinement

**Recommended:** Path A for best equipment feel; Path B if script production-ready

---

## Next Steps

After Phase 6 `ASSET_IMPLEMENTATION_PASS`:

1. All MVP assets complete (Phases 1–6)
2. Begin **Phase 7: Scene Composition** (integrate all assets into single scene)
3. Validate distance visibility for all interactive props
4. Prepare for gameplay testing

Phases 4–6 are **fully parallelizable**; teams can author Pickups, Scout, and Arm simultaneously.

---

## Summary: Phase 4–6 Parallel Effort

**If team divides work:**
- Person A: Phase 4 Pickups (2–3 hours)
- Person B: Phase 5 Scout Enemy (2–3 hours)
- Person C: Phase 6 Mechanical Arm (1.5–2.5 hours)
- **Total wall time:** ~2.5–3 hours (parallel), vs. ~5.5–8.5 hours (sequential)

**Combined assets ready for Phase 7 composition after all three complete PASS validation.**
