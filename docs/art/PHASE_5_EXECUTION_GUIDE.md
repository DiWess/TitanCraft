# Phase 5 Execution Guide: Scout Enemy V1
## Arena Antagonist — Procedural Threat Asset

**Phase:** 5  
**Target Asset:** TC_CHAR_GalaxabrainScout_V1  
**Scope:** Single alien insectoid enemy character mesh  
**Effort:** ~2–3 hours (single authoring pass)  
**Status:** `READY_FOR_AUTHORING`  
**Next Gate:** `ASSET_IMPLEMENTATION_PASS` (threat visibility testing at 15+ meters)

---

## Reference Documents

- **Brief:** `docs/art/briefs/brief-scout-enemy-v1.md` — full specifications
- **Color Language:** Dark organic-mechanical (brown-gray), accent glow (amber or red), menacing aesthetic
- **Poly Budget:** ~2,650 target, ≤3,200 max
- **Visibility requirement:** Identifiable as "enemy" from 15+ meters in FPS POV

---

## Execution Paths

Choose one based on Blender expertise and team capacity:

### Path A: Guided Blender Authoring (Recommended)

**Duration:** ~2–3 hours  
**Skill Level:** Intermediate–Advanced Blender (organic modeling, material setup, articulation hints)  
**Advantage:** Full creative control, visible iteration, menacing aesthetic easily customized  
**Disadvantage:** Requires Blender expertise, single long authoring pass

**Process:**
1. Open Blender 4.0+
2. Create new `.blend`: `assets/Source/Blender/Production/MVP_Pack_V1/TC_CHAR_GalaxabrainScout_V1.blend`
3. Model Scout geometry following anatomy breakdown:
   - Torso/core (30 min)
   - Four legs with claws (40 min)
   - Arms/appendages with sensory/weapon details (30 min)
   - Head/optical sensors (20 min)
   - Armor plating, seams, and battle damage details (20 min)
4. Apply materials and color specification (20 min)
5. Set optical glow (amber or red emissive, 0.5 min)
6. UV unwrap (5–10 min)
7. Validate and save (5 min)

**Modeling Breakdown:**

| Component | Duration | Complexity | Key Details |
|-----------|----------|-----------|------------|
| **Torso** | 30 min | Medium | Bulbous form, armor plating, seams, asymmetry |
| **Legs (4×)** | 40 min | Medium | 3–4 segments each, jointed, claws, consistent scale |
| **Arms (2×)** | 30 min | Medium | Curved/spiked, articulated, weapon-like or sensory tips |
| **Head** | 20 min | Medium | Distinct from torso, optical sensors prominent, mouthparts optional |
| **Armor & Detail** | 20 min | Low | Edge bevels, rivet/seam lines, battle damage, weathering |
| **Materials & UV** | 20 min | Low | Color spec, emissive setup, efficient unwrap |
| **Validation** | 5 min | — | Check manifold, scale, naming |

**Modeling Tips:**

- **Insectoid form:** Use subdivide + extrude for organic curves; box-model for structural elements (legs)
- **Articulation hints:** Keep leg segments distinct (not merged); show joint geometry
- **Menace:** Asymmetry is key; uneven damage or worn appearance adds threat
- **Claws:** Sharp, tapered geometry; avoid rounded/cute look
- **Optical sensors:** Prominent, glowing, on head unit; amber or red color (not friendly blue)
- **Armor plates:** Segmented, overlapping; avoid smooth blob appearance

### Path B: Python Script Generation (If Script Exists)

**Duration:** ~5 min  
**Skill Level:** Minimal  
**Advantage:** Deterministic, reproducible  
**Disadvantage:** Requires pre-built script; quality depends on script accuracy

**Process:**
```bash
# Verify script exists (check create_mvp_asset_pack_v1.py)
grep -l "GalaxabrainScout" tools/blender/create_mvp_asset_pack_v1.py

# Generate Scout mesh
blender --background --python tools/blender/create_mvp_asset_pack_v1.py

# Verify output
ls -la assets/Source/Blender/Production/MVP_Pack_V1/TC_CHAR_GalaxabrainScout_V1.blend
```

After script completes: Proceed to Path B→Post-Generation

### Path C: Rapid Placeholder (Threat Outline Only)

**Duration:** ~15 min  
**Skill Level:** Minimal  
**Advantage:** Fastest to visibility testing  
**Disadvantage:** Geometry is temporary; requires refinement before final approval

**Process:**
1. Import simple alien/bug mesh from placeholder library
2. Add optical glow material (red or amber, emissive 1.5)
3. Scale to ~1.2m height
4. Rename and save to correct path
5. Proceed to post-export workflow

---

## Authoring Checklist (Path A Only)

During Blender authoring of Scout:

**Geometry Phase:**
- [ ] Torso is bulbous, non-humanoid form (alien, not robot)
- [ ] Four distinct legs visible, properly jointed (insectoid stance, coiled/ready pose)
- [ ] Legs tapered toward claws (efficient, menacing)
- [ ] Two arms/appendages with sensory or weapon-like tips
- [ ] Head unit distinct from torso, separate geometry (not merged blob)
- [ ] Optical sensors prominent, positioned on head (glowing threat indicator)
- [ ] Armor segmentation visible (seams, plates, overlapping geometry)
- [ ] Asymmetry or battle damage evident (dents, scratches, scorch marks)
- [ ] Scale approximately 1.2–1.4m height (smaller than 1.8m player, still menacing)
- [ ] Stance is coiled and forward-leaning (suggests ready to leap)
- [ ] No internal faces or manifold errors

**Material Phase:**
- [ ] Optical sensor color chosen: amber (RGB ~240, 150, 50) or red (RGB ~220, 80, 60)
- [ ] Optical sensor emissive strength: 1.5–2.0 (obvious threat glow)
- [ ] Primary armor material: dark brown-gray (RGB ~80, 70, 60)
- [ ] Accent plating: steel gray (RGB ~120, 120, 130)
- [ ] Damage/scorch material: very dark (RGB ~30, 25, 20)
- [ ] All materials assigned to correct geometry
- [ ] Optical glow visible in Material Preview mode
- [ ] No texture stretching or undefined zones

**Articulation Phase:**
- [ ] Leg segments separated/distinct (not one smooth mesh)
- [ ] Leg joints geometrically obvious (bent, coiled)
- [ ] Arm articulation hints visible (shoulder/elbow-equivalent joints)
- [ ] Gripper or weapon tips distinct from arm body

**Validation Phase (In Blender):**
- [ ] Switch to Material Preview: optical glow is obvious, materials read correctly
- [ ] Optical glow color is menacing (amber or red, not friendly)
- [ ] Silhouette reads "hostile alien threat" (not cute, not mechanical blob, not humanoid)
- [ ] Four legs visible and distinct
- [ ] Armor plating obvious (not smooth blob)
- [ ] No visual defects, black spots, or rendering errors
- [ ] Object name is `TC_CHAR_GalaxabrainScout_V1`
- [ ] Single mesh object in scene (clean)

**Save:**
- [ ] File name: `TC_CHAR_GalaxabrainScout_V1.blend`
- [ ] Location: `assets/Source/Blender/Production/MVP_Pack_V1/`
- [ ] Blender 4.0+ format

---

## Post-Authoring Workflow (All Paths)

### Step 1: Validate Source `.blend` File

```bash
# Verify file exists
ls -la assets/Source/Blender/Production/MVP_Pack_V1/TC_CHAR_GalaxabrainScout_V1.blend
```

### Step 2: Export to GLB

```bash
blender --background --python tools/blender/export_asset.py -- \
  assets/Source/Blender/Production/MVP_Pack_V1/TC_CHAR_GalaxabrainScout_V1.blend \
  assets/Production/Generated/MVP_Pack_V1/TC_CHAR_GalaxabrainScout_V1.glb

# Verify export
ls -lh assets/Production/Generated/MVP_Pack_V1/TC_CHAR_GalaxabrainScout_V1.glb
```

### Step 3: Validate Export

```bash
python3 tools/blender/validate_blender_asset.py \
  assets/Source/Blender/Production/MVP_Pack_V1/TC_CHAR_GalaxabrainScout_V1.blend
```

Expected output:
```
validation_status: PASS
triangle_count: ~2650
material_slots: [dark armor, gray accent, optical sensor, etc.]
```

### Step 4: Rebuild Asset Manifest

```bash
python3 tools/blender/build_asset_manifest.py
```

Verify Scout entry added:
```
ASSET_MANIFEST_WRITTEN assets/Production/Generated/asset_manifest.json entries=<N>
```

### Step 5: Capture Review Artifacts

```bash
# Create review directory
mkdir -p artifacts/asset-review/TC_CHAR_GalaxabrainScout_V1

# Render turntables (predatory angle: side view and 45° front view)
blender --background --python tools/blender/render_mvp_asset_review.py -- \
  assets/Source/Blender/Production/MVP_Pack_V1/TC_CHAR_GalaxabrainScout_V1.blend \
  artifacts/asset-review/TC_CHAR_GalaxabrainScout_V1
```

---

## Threat Visibility Testing (In Godot or Standalone)

### Test Procedure: 15+ Meters First-Person Validation

**Goal:** Confirm Scout is identifiable as "hostile threat" from combat distance

**Setup:**
1. Import Scout GLB into Godot
2. Create test scene: place Scout at 15m distance
3. View from first-person camera
4. Assess threat readability

**Validation Checklist:**

**Visual Reading:**
- [ ] Silhouette reads "alien predator" (not humanoid, not cute, not mechanical blob)
- [ ] Four legs visible and menacing
- [ ] Head with optical sensors distinguishable
- [ ] Optical glow (amber or red) obvious at distance
- [ ] Armor plating evident (not smooth)
- [ ] Threat level obvious without text/UI

**Technical:**
- [ ] Mesh imports without errors
- [ ] Optical sensor glow visible and correct color
- [ ] Scale appropriate (smaller than player, not tiny)
- [ ] Silhouette matches brief (insectoid, coiled, predatory)
- [ ] No visual corruption or geometry issues

**If test fails:** Mark `NOT_READY` and refine:
- Increase optical glow intensity (emissive strength to 2.0)
- Add more edge definition to distinguish silhouette
- Increase armor plating or battle damage visibility
- Adjust color saturation for more obvious threat language

---

## Success Criteria

✅ **Phase 5 is `ASSET_IMPLEMENTATION_PASS` when:**

1. Source `.blend` file exists: `assets/Source/Blender/Production/MVP_Pack_V1/TC_CHAR_GalaxabrainScout_V1.blend`
2. GLB export successful: `assets/Production/Generated/MVP_Pack_V1/TC_CHAR_GalaxabrainScout_V1.glb`
3. Validation reports PASS, ~2,650 triangles
4. Asset manifest entry created with all metadata
5. Silhouette reads **alien insectoid predator** (not humanoid, not cute, not mechanical)
6. Four legs visible, distinct, and menacing
7. Optical sensor glow bright and obvious (amber or red, emissive ≥1.5)
8. Armor plating evident and asymmetrical
9. Battle damage or wear marks visible (suggests combat history)
10. Material palette coherent (dark brown-gray body, gray accent, menacing glow)
11. Poly count ≤ 3,200
12. Turntable PNGs captured (2–3 angles showing threat stance)
13. Threat visibility test passed: identifiable as enemy from 15+ meters (first-person POV)

**Expected manifest entry:**
```json
{
  "asset_name": "TC_CHAR_GalaxabrainScout_V1",
  "classification": "production-candidate",
  "triangle_count": ~2650,
  "validation_status": "PASS",
  "material_slots": ["dark_armor", "gray_accent", "optical_sensor", "damage_scorch"]
}
```

---

## Troubleshooting

| Issue | Symptom | Solution |
|-------|---------|----------|
| **Looks humanoid** | Scout appears biped or human-like | Emphasize four-leg stance; add insectoid features (mandibles, compound eyes, asymmetry) |
| **Optical glow not visible** | Sensor does not glow in preview | Check emissive strength ≥1.5; ensure material is applied correctly |
| **Threat not obvious at distance** | Silhouette mushy or indistinct | Add more edge definition, armor segmentation, asymmetry, or color contrast |
| **Scale feels off** | Scout too large or too small | Target is 1.2–1.4m; adjust and re-export; compare to 1.8m player reference |
| **Armor looks smooth** | No segmentation or plating visible | Add explicit seams, overlapping plates, rivet geometry, edge bevels |
| **Legs confused or merged** | Four legs not visible as separate | Ensure legs are distinct geometry; add joint/articulation hints |
| **Export fails** | GLB not created | Check console for Blender errors; validate mesh manifold (no self-intersections) |
| **Poly count too high** | > 3,200 triangles | Reduce segment count in legs/arms; remove interior detail; consider mesh decimation |

---

## Deployment Checklist

Before closing Phase 5:

- [ ] Source `.blend` file committed
- [ ] GLB export in artifact bundle
- [ ] Asset manifest updated
- [ ] Threat visibility test passed (15+m, identifies as enemy)
- [ ] Review PNG artifacts captured
- [ ] Brief document finalized
- [ ] No outstanding TODOs or FIXME

---

## Effort Summary

**Path A (Guided Blender):** ~2–3 hours (single authoring pass, moderate complexity)  
**Path B (Script):** ~5 minutes (if script ready)  
**Path C (Rapid Placeholder):** ~15 minutes + later refinement

**Recommended:** Path A for best menacing aesthetic; Path B if script is production-ready

---

## Next Steps

After Phase 5 `ASSET_IMPLEMENTATION_PASS`:

1. Proceed to **Phase 6: Mechanical Arm** (brief ready, same workflow)
2. After Phase 6 complete, begin **Phase 7: Scene Composition** with all assets
3. Integrate Scout behavior and AI (Godot scripting, separate from mesh authoring)

Phase 5 is **parallelizable** with Phase 4 (Pickups) and Phase 6 (Mechanical Arm) authoring.
