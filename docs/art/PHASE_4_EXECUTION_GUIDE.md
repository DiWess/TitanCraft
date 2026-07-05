# Phase 4 Execution Guide: Pickups V1
## Resource Collection Assets — Four Pickup Types

**Phase:** 4  
**Target Assets:** TC_PICKUP_Metal_V1, TC_PICKUP_Biomass_V1, TC_PICKUP_Electronics_V1, TC_PICKUP_Component_V1  
**Scope:** Four distinct modular resource pickup meshes  
**Effort:** ~2–3 hours (parallel authoring, if team available)  
**Status:** `READY_FOR_AUTHORING`  
**Next Gate:** `ASSET_IMPLEMENTATION_PASS` (visibility testing at 5–10m range)

---

## Reference Documents

- **Brief:** `docs/art/briefs/brief-pickups-v1.md` — full specifications
- **Color Language:** Workbench (orange), Beacon (purple/red), Save Point (cyan), Pickups (material-specific)
- **Poly Budget:** ~200–300 per asset, ~1,030 total, ≤1,200 max

---

## Execution Paths

Choose one based on team capacity and Blender proficiency:

### Path A: Guided Blender Authoring (Recommended for Team)

**Duration:** ~2–3 hours total (if team divides work: 40–45 min per asset)  
**Skill Level:** Intermediate Blender (modeling, materials, UV)  
**Advantage:** Full creative control, visibility into quality, easy iteration  
**Disadvantage:** Requires Blender expertise, four separate authoring passes

**Process:**
1. Open Blender 4.0+
2. Create four `.blend` files in `assets/Source/Blender/Production/MVP_Pack_V1/`:
   - `TC_PICKUP_Metal_V1.blend`
   - `TC_PICKUP_Biomass_V1.blend`
   - `TC_PICKUP_Electronics_V1.blend`
   - `TC_PICKUP_Component_V1.blend`
3. For each asset:
   - Model core geometry (6–15 min depending on complexity)
   - Apply materials from color specification (5 min)
   - UV unwrap efficiently (3–5 min)
   - Validate in material preview (2 min)
4. Save and close
5. Run export and validation (Path A→Post-Export)

**Modeling Breakdown per Asset:**

| Asset | Geometry | Duration | Notes |
|-------|----------|----------|-------|
| **Metal** | Cubic block with bevels and wear | 8–12 min | Clean, sharp edges; smooth shading |
| **Biomass** | Lumpy organic form (sculpt or manual geometry) | 12–15 min | Use subdivide + displacement or manual vertex tweaking |
| **Electronics** | Circuit board with component protrusions | 10–12 min | Flat base + geometric components (capacitors, chips) |
| **Component** | Hybrid tool form (wrench or connector shape) | 10–12 min | Combination of cylindrical and box shapes with bolts |

### Path B: Python Script Generation (If Script Exists)

**Duration:** ~5–10 min (only if `tools/blender/create_mvp_asset_pack_v1.py` is complete)  
**Skill Level:** Minimal (run command, verify output)  
**Advantage:** Deterministic, reproducible, parallel-friendly  
**Disadvantage:** Script must already exist; quality depends on script accuracy

**Process:**
```bash
# Ensure script exists and is ready
ls tools/blender/create_mvp_asset_pack_v1.py

# Generate all four pickups at once
blender --background --python tools/blender/create_mvp_asset_pack_v1.py

# Verify outputs
ls assets/Source/Blender/Production/MVP_Pack_V1/TC_PICKUP_*.blend
```

**After script completes:**
- Proceed directly to Path B→Post-Generation

### Path C: Import Placeholder Meshes (If Rapid Validation Only)

**Duration:** ~10 min  
**Skill Level:** Minimal (Blender import, basic material swap)  
**Advantage:** Fastest route to visibility testing  
**Disadvantage:** Geometry is rough; requires refinement before final approval

**Process:**
1. Import or reference temporary geometry from prior projects
2. Apply color materials from specification (5 min per asset)
3. Save to correct paths
4. Proceed to validation (Path C→Post-Import)

---

## Authoring Checklist (Path A Only)

For each pickup asset during Blender authoring:

**Geometry Phase:**
- [ ] Core form matches silhouette in brief (Metal cubic, Biomass lumpy, Electronics board, Component hybrid)
- [ ] Scale approximately correct (0.25–0.4m, hand-sized)
- [ ] No internal faces or manifold errors (`Mesh → Validate Mesh` in Blender)
- [ ] Sharp edges beveled or smoothed (contextual per asset)
- [ ] Smooth shading applied (all objects)
- [ ] Asymmetry or wear marks added (except Metal, which is clean)

**Material Phase:**
- [ ] Correct material template created (check color spec)
  - Metal: RGB ~200,200,200, Metalness 0.8, Roughness 0.5
  - Biomass: RGB ~160,120,80, Metalness 0.0, Roughness 0.8
  - Electronics: Dark base (RGB ~60,60,70) + Cyan accent (RGB ~100,220,255, emissive 0.6)
  - Component: Dark base (RGB ~100,100,110) + Orange accent (RGB ~240,140,60, emissive 0.7)
- [ ] Material is assigned to all geometry
- [ ] Emissive (if applicable) configured and visible in Material Preview mode
- [ ] No texture stretching or obvious seams

**UV Phase:**
- [ ] All faces included in UV map
- [ ] No overlapping islands (unless intentional tiling)
- [ ] Seams are hidden or minimal
- [ ] Density consistent (not grossly wasted space)
- [ ] Can be simple/efficient (pickups don't need complex UV)

**Validation Phase (In Blender):**
- [ ] Switch to Material Preview: geometry reads correct color and metalness
- [ ] If emissive: glow is visible in preview
- [ ] No visual defects, black spots, or material errors
- [ ] Object name matches file name (TC_PICKUP_Metal_V1, etc.)
- [ ] Only one mesh object per file (clean scene)

**Save:**
- [ ] File name: `TC_PICKUP_<Type>_V1.blend`
- [ ] Location: `assets/Source/Blender/Production/MVP_Pack_V1/`
- [ ] Save in Blender 4.0+ format (.blend)

---

## Post-Authoring Workflow (All Paths)

### Step 1: Validate Source `.blend` Files

```bash
# Check that all four files exist
ls -la assets/Source/Blender/Production/MVP_Pack_V1/TC_PICKUP_*.blend
```

Expected output:
```
TC_PICKUP_Metal_V1.blend
TC_PICKUP_Biomass_V1.blend
TC_PICKUP_Electronics_V1.blend
TC_PICKUP_Component_V1.blend
```

### Step 2: Export to GLB

```bash
# Export all four pickups
for blend in assets/Source/Blender/Production/MVP_Pack_V1/TC_PICKUP_*.blend; do
  name="$(basename "$blend" .blend)"
  blender --background --python tools/blender/export_asset.py -- "$blend" "assets/Production/Generated/MVP_Pack_V1/$name.glb"
  echo "✓ Exported $name.glb"
done

# Verify GLB files exist
ls assets/Production/Generated/MVP_Pack_V1/TC_PICKUP_*.glb
```

### Step 3: Validate Exports

```bash
# Run validation on each .blend
for blend in assets/Source/Blender/Production/MVP_Pack_V1/TC_PICKUP_*.blend; do
  python3 tools/blender/validate_blender_asset.py "$blend"
  echo "---"
done
```

Expected per asset: `validation_status: PASS`, `triangle_count: <value>`, `material_slots: [...]`

### Step 4: Rebuild Asset Manifest

```bash
python3 tools/blender/build_asset_manifest.py
```

Verify output includes all four pickups:
```
ASSET_MANIFEST_WRITTEN assets/Production/Generated/asset_manifest.json entries=<N>
```

### Step 5: Capture Review Artifacts

For each pickup, capture 1–2 turntable angles (side-view, 45° view):

```bash
# Create review directories
mkdir -p artifacts/asset-review/TC_PICKUP_{Metal,Biomass,Electronics,Component}_V1

# Render turntables (if render script available)
for blend in assets/Source/Blender/Production/MVP_Pack_V1/TC_PICKUP_*.blend; do
  name="$(basename "$blend" .blend)"
  blender --background --python tools/blender/render_mvp_asset_review.py -- "$blend" "artifacts/asset-review/$name"
done
```

---

## Visibility Testing (In Godot or Standalone Viewer)

### Test Procedure: 5–10m First-Person Distance Validation

**Goal:** Confirm all four pickup types are visually distinct and identifiable from gameplay distance

**Setup:**
1. Import all four GLB files into Godot (Assets → Import)
2. Create test scene with four pickup instances at ~7m distance
3. View from first-person camera
4. Assess readability per resource type

**Validation Checklist:**

**Metal (Silvery-Gray):**
- [ ] Silhouette reads "cubic, metallic, hard material"
- [ ] Shine/metalness is obvious
- [ ] Distinct from Biomass and Electronics

**Biomass (Brown):**
- [ ] Silhouette reads "lumpy, organic, warm"
- [ ] Matte (non-reflective) appearance obvious
- [ ] Distinct from Metal and Electronics

**Electronics (Dark + Cyan):**
- [ ] Circuit board or module form obvious
- [ ] Cyan glow visible and distinct
- [ ] Clearly different from Component (which is orange)

**Component (Dark + Orange):**
- [ ] Tool or connector form obvious
- [ ] Orange glow visible and distinct
- [ ] Clearly different from Electronics (which is cyan)

**Cross-type validation:**
- [ ] All four types visually distinct at 7m
- [ ] No confusion between any pair
- [ ] Types identifiable without text labels
- [ ] Color language consistent (gray=metal, brown=organic, cyan=electronics, orange=component)

**If any type fails:** Mark as `NOT_READY` and refine geometry/materials until readable

---

## Success Criteria

✅ **Phase 4 is `ASSET_IMPLEMENTATION_PASS` when:**

1. All four `.blend` files exist in `assets/Source/Blender/Production/MVP_Pack_V1/`
2. All four GLB exports successful in `assets/Production/Generated/MVP_Pack_V1/`
3. Validation reports no errors for any asset
4. Asset manifest updated with all four pickups
5. Silhouettes match brief (Metal cubic, Biomass lumpy, Electronics board, Component hybrid)
6. All material zones defined and colors correct
7. Emissive accents (cyan and orange) glow correctly and are visible
8. All four types visually distinct at 5–10m first-person POV
9. Turntable PNGs captured for all four
10. Poly count check passes (total ≤ 1,200)
11. No visual corruption or texture issues

**Expected manifest entries:**
- `TC_PICKUP_Metal_V1` — ~200 polys, PASS
- `TC_PICKUP_Biomass_V1` — ~250 polys, PASS
- `TC_PICKUP_Electronics_V1` — ~300 polys, PASS (cyan glow visible)
- `TC_PICKUP_Component_V1` — ~280 polys, PASS (orange glow visible)

---

## Troubleshooting

| Issue | Symptom | Solution |
|-------|---------|----------|
| **Material doesn't glow** | Emissive (cyan/orange) not visible in preview | Check emissive strength is ≥0.6; ensure glow object has emissive material assigned |
| **Pickup color wrong** | Material appears different than brief spec | Check RGB values in Material Properties; re-apply spec values |
| **Geometry reads as blob** | Silhouette indistinct at distance | Add more edge definition, bevels, or asymmetry; validate against brief silhouette |
| **Scale feels wrong** | Pickup too large or too small | Check dimensions vs. brief target (0.25–0.4m); adjust and re-export |
| **Export fails** | GLB file not created | Check Blender output in console; ensure mesh is valid (no self-intersections) |
| **Poly count exceeds budget** | Total > 1,200 | Reduce geometry density; remove internal detail; consider mesh decimation |
| **Types not distinct** | Can't tell Metal from Component at distance | Increase color saturation or metalness difference; add asymmetry to differentiate silhouettes |

---

## Deployment Checklist

Before closing Phase 4:

- [ ] All four source `.blend` files committed to branch
- [ ] All four GLB exports in artifact bundle
- [ ] Asset manifest updated with all four entries
- [ ] Visibility test passed (5–10m, all types distinct)
- [ ] Review PNG artifacts captured
- [ ] Brief document finalized
- [ ] No outstanding TODOs or FIXME notes

---

## Effort Summary

**Path A (Guided Blender):** ~2–3 hours (40–50 min per asset + post-export)  
**Path B (Script):** ~10 minutes (if script ready)  
**Path C (Rapid Placeholder):** ~10 minutes + later refinement

**Recommended:** Path A if team available; Path B if script is production-ready

---

## Next Steps

After Phase 4 `ASSET_IMPLEMENTATION_PASS`:

1. Proceed to **Phase 5: Scout Enemy** (brief ready, same workflow)
2. Proceed to **Phase 6: Mechanical Arm** (brief ready, same workflow)
3. After Phase 5 & 6 complete, begin **Phase 7: Scene Composition** (terrain, hull, pickups, enemies, interactive props in one scene)

Phase 4 is **parallelizable** with Phase 3 visibility testing and Phases 5 & 6 authoring.
