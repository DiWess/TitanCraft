# Phase 2 Execution Guide: Crash Hull Mk1
## Step-by-Step Implementation

**Status:** Ready to Execute  
**Brief Reference:** `docs/art/brief-crash-hull-mk1.md`  
**Estimated Time:** ~3 hours  
**Success Gate:** `ASSET_IMPLEMENTATION_PASS`  

---

## Prerequisites Checklist

Before starting Phase 2, confirm:

- [ ] Phase 1 (Terrain) completed and validated in Godot
- [ ] Blender 4.0+ installed on your machine
- [ ] Godot 4 .NET available for validation
- [ ] ~3 hours of continuous work time
- [ ] Brief specifications thoroughly understood (`docs/art/brief-crash-hull-mk1.md`)

---

## Three Execution Paths

### **Path A: Guided Blender Authoring (Recommended)**

This path walks you through manual Blender modeling using the brief as your guide. Total time: ~3 hours.

#### Step 1: Create New Blender Project

```bash
# Start fresh Blender scene
blender --new-instance
```

1. File → New → General
2. Delete default cube
3. File → Save As → `art/blender/models/TC_CRASH_HullMk1_V1.blend`

#### Step 2: Build Main Fuselage (45 min)

**Geometry:**
- Add UV Sphere, scale to ~60m length (X), ~20m width (Y), ~30m height (Z)
- Press Tab to enter Edit Mode
- Select all faces
- Press Shift+Z to wireframe view for better control
- Box select half the sphere and delete to create open cargo area
- Add loop cuts along the length for panel definition (~every 3m)
- Flatten bottom slightly to show landing (not perfect)

**Material:**
- Create material: `TC_HullMat_OffWhiteWorn` (RGB 200, 190, 180, roughness 0.75)
- Assign to fuselage faces

**Outcome:**
- Boxy industrial main hull
- ~4,000 polys

#### Step 3: Build Crushed Nose (30 min)

**Approach:**
- Add UV Sphere for nose cone
- Scale asymmetrically: left side intact, right side collapsed inward ~40%
- Use proportional editing to push/pull the crushed side
- Extrude and delete faces to expose interior where crushed
- Flatten/sharpen edges (NOT smooth)

**Interior Structure:**
- Add simple cube/planes where interior is exposed
- Create material: `TC_StructuralSteel_Graphite` (RGB 100, 100, 110, roughness 0.7)
- Position so internal ribs/bulkheads are visible in crush zone

**Outcome:**
- Asymmetric crush obvious
- Interior structure visible
- ~1,500 polys

#### Step 4: Build Wing Assembly (30 min)

**Primary Wing:**
- Add cube, scale to ~50m x 3m x 15m (wing planform)
- Position at right angle to fuselage
- Select midspan faces, delete or extrude at severe angle to show tearing
- Expose internal spar structure where wing is torn
- Add loop cuts for panel definition

**Material:**
- Apply off-white hull material to exterior
- Apply steel material to exposed spar

**Optional Secondary Wing:**
- Duplicate wing, rotate/scale differently for asymmetry
- Position partially buried or at different angle

**Outcome:**
- Torn midspan is obvious
- Spar structure visible at tear point
- ~2,000 polys (for both wings)

#### Step 5: Build Engine Pod(s) (30 min)

**Engine Mount:**
- Add cylinder for engine housing (~4m diameter, ~2m depth)
- Position at rear of fuselage
- Scale slightly: not perfectly symmetrical

**Turbine/Core:**
- Add nested cylinders for turbine stages (concentric rings)
- Apply steel/graphite materials to show internal structure
- Add radial planes as vanes (rotor blades)

**Mount Struts:**
- Add rectangular boxes connecting engine to fuselage
- Position at angles suggesting heavy structure

**Material:**
- Engine housing: `TC_StructuralSteel_Graphite` 
- Vanes/blades: darker steel

**Outcome:**
- Turbine stages visible (concentric circles)
- Heavy external mounting structure clear
- ~1,500 polys

#### Step 6: Build Tail Section (20 min)

**Stabilizer Fins:**
- Add flat planes at rear of fuselage
- Damage/bend one or both (rotate/scale asymmetrically)
- Show breaking at root

**Internal Ribs (where tail is torn):**
- Add parallel planes visible through tear zones
- Create depth sensation (not flat cross-section)
- Apply structural steel material

**Scorch/Burn Zone:**
- Add darkened areas around tail
- Material: `TC_Scorch_Charred` (RGB 40, 30, 25, roughness 0.85)
- Concentrate burns around impact zone

**Outcome:**
- Tail is identifiable
- Torn structure shows internal complexity
- ~1,000 polys

#### Step 7: Add Exposed Interior (20 min)

Where hull is breached/torn:

**Visible Structure:**
- Bulkhead frames (flat rectangular planes at sections)
- Stringers/ribs (thin horizontal elements)
- Cables/conduits (very thin dark strands, optional)

**Goal:** Depth, not detail — player should see "industrial/complex" not "empty"

**Outcome:**
- Interior reads as complex/real
- No confusion about scale or construction
- ~300 polys

#### Step 8: Add Salvage Debris (15 min)

Around/beneath hull:

**Loose Panels:**
- Add rectangular boxes, position scattered around hull
- Some partially buried under hull
- Apply hull/steel materials
- Make varied sizes

**Containers/Equipment:**
- Add boxes for cargo containers (some crushed)
- Add small cylinders for external equipment
- Scale to suggest weight/industrial use

**Outcome:**
- Visual storytelling: impact scattered debris
- Suggests salvage opportunities
- ~300 polys

#### Step 9: Apply Materials & Finalize (15 min)

**Material Setup:**
- Review brief color specs for each zone
- Ensure material zone names match brief (e.g., `TC_MAT_worn_off_white_hull`)
- Create material slots for each zone
- Assign materials to appropriate faces

**UV Unwrapping (if needed):**
- Select all objects
- Mark seams on hard edges
- Smart UV Project for quick unwrap
- Scale UV islands to reasonable proportions

**Final Checks:**
- No overlapping faces (Mesh → Validation → Check Overlapping Faces)
- No internal geometry (wireframe view confirms clean shell)
- Collision-safe surfaces (no sharp internal protrusions)
- Poly count estimate: select all, check face count in properties

**Target poly count:** 8,000–13,000 total

#### Step 10: Export to GLB

**File → Export → glTF 2.0 (.glb/.gltf)**

Settings:
```
Filename: TC_CRASH_HullMk1_V1.glb
Location: assets/Production/Generated/Hero/
Format: Binary (.glb)
Include Animations: OFF
Include Armatures: OFF
Export Materials: ON
Export Textures: ON (if external textures)
Export Normals: ON
Export Tangents: ON
```

**Verify export:**
- File size ~10–20 MB (depending on detail)
- File created successfully at `assets/Production/Generated/Hero/TC_CRASH_HullMk1_V1.glb`

---

### **Path B: Python Generation Script (If Available)**

If a `tools/blender/create_crash_hull_mk1.py` script exists, use it:

```bash
blender --background --python tools/blender/create_crash_hull_mk1.py
```

**What this does:**
- Procedurally generates hull geometry
- Applies materials automatically
- Exports to GLB
- Updates asset manifest
- Reports validation status

**Output:**
- `assets/Production/Generated/Hero/TC_CRASH_HullMk1_V1.glb`
- Updated `assets/Production/Generated/asset_manifest.json` with SHA256 hash

---

### **Path C: Import Existing Heavy Crash Hull (If Approved)**

If the existing `TC_HeavyCrashHull_V1` asset meets brief requirements:

```bash
# Verify it meets poly count and detail spec
python3 tools/blender/validate_blender_asset.py assets/Source/Blender/Production/TC_HeavyCrashHull_V1.blend

# Check if export already exists
ls assets/Production/Generated/CrashWreck/TC_HeavyCrashHull_V1.glb
```

**Considerations:**
- Existing asset has 904 polys (vs ~11,300 spec)
- May need enhancement/re-export if detail is insufficient
- Review brief requirements carefully before approving

---

## Validation in Godot

Once GLB is exported:

```bash
cd /home/user/TitanCraft
godot --headless --path . --import
```

**Check for:**
- ✅ No import errors or warnings
- ✅ No shader/material mismatches
- ✅ GLB loads successfully
- ✅ Silhouette matches brief (heavy, crushed, industrial)
- ✅ Interior structure visible where breached
- ✅ No visual corruption or stretching

**Expected output:**
```
[INFO] Importing scene: res://assets/Production/Generated/Hero/TC_CRASH_HullMk1_V1.glb
[INFO] Successfully imported; asset ready for use
```

---

## Capture Review Artifacts

Once validated in Godot:

### **Neutral Gray Silhouette PNG**

Load hull in Godot test scene with neutral gray material:

```gdscript
# Test scene setup
var hull = preload("res://assets/Production/Generated/Hero/TC_CRASH_HullMk1_V1.glb").instantiate()
add_child(hull)

# Capture screenshot in neutral gray wireframe or simple material
# Save to: artifacts/review/hull-v1/TC_CRASH_HullMk1_silhouette.png
```

**Checklist:**
- [ ] Reads as heavy crushed spacecraft (not sleek toy)
- [ ] Asymmetric crush is obvious in silhouette
- [ ] Nose, wings, engine pods, tail all identifiable
- [ ] Interior structure visible where exposed
- [ ] No obvious clipping or geometry errors

### **Turntable 360° Views**

Capture 4 angles (0°, 90°, 180°, 270°):

```bash
# From Blender command line:
blender --background assets/Production/Generated/Hero/TC_CRASH_HullMk1_V1.glb \
  --render-frame 1 \
  --render-output "artifacts/review/hull-v1/TC_CRASH_HullMk1_turntable_"
```

**Angles to capture:**
1. **0° (Spawn view):** Show fuselage, nose, one wing
2. **90°:** Show profile, engine pods, tail
3. **180°:** Opposite fuselage side, tail from rear
4. **270°:** Other profile, emphasize crushed nose from this angle

**Output files:**
- `artifacts/review/hull-v1/TC_CRASH_HullMk1_turntable_0.png`
- `artifacts/review/hull-v1/TC_CRASH_HullMk1_turntable_90.png`
- `artifacts/review/hull-v1/TC_CRASH_HullMk1_turntable_180.png`
- `artifacts/review/hull-v1/TC_CRASH_HullMk1_turntable_270.png`

### **Damage Detail Close-up**

Capture high-detail view of:
- Crushed nose (asymmetry, interior exposure)
- Torn wing (spar structure, panel breaks)
- Engine pod (turbine stages, mounts)
- Breach zone (internal ribs, scorch marks)

**Output:**
- `artifacts/review/hull-v1/TC_CRASH_HullMk1_detail_nose.png`
- `artifacts/review/hull-v1/TC_CRASH_HullMk1_detail_wing_tear.png`
- `artifacts/review/hull-v1/TC_CRASH_HullMk1_detail_engine.png`
- `artifacts/review/hull-v1/TC_CRASH_HullMk1_detail_breach.png`

---

## Manifest & Hashing

Once GLB is created and validated:

```bash
python3 tools/blender/build_asset_manifest.py
```

This will:
- Compute SHA256 hash of GLB
- Generate manifest entry with metadata
- Update `assets/Production/Generated/asset_manifest.json`

**Verify entry:**
```bash
grep -A 20 "TC_CRASH_HullMk1_V1" assets/Production/Generated/asset_manifest.json
```

**Expected manifest entry:**
```json
{
  "asset_name": "TC_CRASH_HullMk1_V1",
  "classification": "production-candidate",
  "source_blend": "art/blender/models/TC_CRASH_HullMk1_V1.blend",
  "production_export": "assets/Production/Generated/Hero/TC_CRASH_HullMk1_V1.glb",
  "production_sha256": "<hash>",
  "validation_status": "PASS",
  "triangle_count": 11300,
  "review_artifacts": [
    "artifacts/review/hull-v1/TC_CRASH_HullMk1_silhouette.png",
    "artifacts/review/hull-v1/TC_CRASH_HullMk1_turntable_0.png",
    ...
  ],
  "verdict": "TC_CRASH_HULL_MK1_V1_REVIEW_ARTIFACTS_READY"
}
```

---

## Validation Checklist

Mark as you complete each section:

**Geometry & Structure:**
- [ ] Silhouette reads heavy crushed spacecraft ✓
- [ ] Asymmetric crush is obvious ✓
- [ ] Interior structure visible where breached ✓
- [ ] Nose, wings, engines, tail all identifiable ✓
- [ ] Poly count within budget (8,000–13,000) ✓
- [ ] No overlapping/internal geometry ✓
- [ ] Collision-clean surfaces ✓

**Materials & Colors:**
- [ ] Worn off-white hull paint (RGB ~200, 190, 180) ✓
- [ ] Dark structural steel visible (RGB ~100, 100, 110) ✓
- [ ] Engine metal zones distinct ✓
- [ ] Scorch/burn marks present (~5% coverage) ✓
- [ ] Material names match brief spec ✓

**Godot Import:**
- [ ] GLB imports without errors ✓
- [ ] Textures/materials resolve correctly ✓
- [ ] No visual corruption or stretching ✓
- [ ] Silhouette matches brief visually ✓

**Review Artifacts:**
- [ ] Neutral gray silhouette PNG captured ✓
- [ ] 4-angle turntable sequence captured ✓
- [ ] Detail close-ups captured (nose, wing, engine, breach) ✓
- [ ] Artifact folder organized and named correctly ✓

**Asset Management:**
- [ ] GLB exported to correct path ✓
- [ ] Manifest entry created with SHA256 ✓
- [ ] Source .blend saved (if authoring manually) ✓
- [ ] All review PNGs referenced in manifest ✓

---

## Success Criteria

✅ **Phase 2 is `ASSET_IMPLEMENTATION_PASS` when:**

1. Hull GLB exported and validated in Godot
2. Silhouette reads **heavy industrial crushed spacecraft**
3. Asymmetric crush is visually obvious
4. Interior structure visible in breached areas (depth, not flat)
5. Nose, wings, engines, tail all identifiable
6. Material palette coherent (worn paint, structural steel, engine metal, char)
7. Poly count ≤ 13,000 total
8. No visual corruption, stretching, or floating geometry
9. Turntable PNGs captured (minimum 4 angles, hero coverage)
10. Detail close-ups captured (damage, internal structure)
11. SHA256 hash recorded in asset manifest
12. All review artifacts referenced in manifest

**Gate verdict:** `TC_CRASH_HULL_MK1_V1_READY_FOR_COMPOSITION`

---

## Troubleshooting

### Problem: Silhouette reads "toy-like" or sleek

**Cause:** Hull is too smooth, taper is too pronounced, crush is too subtle

**Solution:**
1. Make fuselage more boxy (square off nose, less taper)
2. Increase crush asymmetry (one side collapsed much more)
3. Add heavy structural ribbing on exterior
4. Roughen edges (no smooth shade at breaks)
5. Re-export and review in neutral gray

### Problem: Interior structure not visible

**Cause:** Breaches are closed or interior is not modeled

**Solution:**
1. Delete/remove faces to expose interior where hull is torn
2. Add simple planes for bulkheads/ribs inside
3. Ensure interior surfaces have structural steel material applied
4. In Godot, wireframe view should show depth
5. Re-capture silhouette PNG to verify

### Problem: Poly count exceeds budget

**Cause:** Too many subdivisions, overly detailed components

**Solution:**
1. Reduce loop cuts on large surfaces
2. Combine smaller geometry elements into single mesh
3. Use lower-res cylinders for engine turbines
4. Remove detail from areas that aren't visually critical
5. Target: reduce to 10,000–12,000 polys

### Problem: Materials not exporting correctly

**Cause:** Complex node graphs, baking not applied, texture paths broken

**Solution:**
1. Use only Principled BSDF in material nodes
2. Avoid custom procedural textures (bake if needed)
3. Keep texture references simple (relative paths)
4. Test export with simple material first
5. In Godot, check material inspector for missing assets

### Problem: Collision doesn't work (player falls through hull)

**Cause:** No collision shape assigned; mesh is too complex for auto-collision

**Solution:**
1. In Godot, add StaticBody3D node
2. Add CollisionShape3D as child
3. Assign `mesh.create_trimesh_shape()` to collision
4. For optimization: create simplified collision hull (convex shape)
5. Test player can walk around full hull perimeter

---

## Next Steps (After Phase 2 Complete)

1. Commit Phase 2 assets and manifest update
2. Push to branch; let GitHub Actions validate
3. Review turntable PNGs and detail artifacts
4. Obtain human visual review verdict
5. **If approved:** Proceed to Phase 3 (Interactables)
6. **If remediation needed:** Return to Blender, address feedback, re-export

**Timeline:** Phase 2 complete → Phase 3 start (after composition brief finalized)

---

## Support & Questions

If you hit blockers:

1. **Brief confusion:** Review `docs/art/brief-crash-hull-mk1.md` sections on silhouette, poly budget, material spec
2. **Godot import issues:** Check `docs/pipeline/blender-asset-forge.md` troubleshooting
3. **Blender modeling questions:** Consult Visual Identity guide (`docs/art/titancraft-visual-identity.md`) for style direction
4. **Composition questions:** Phase 3 brief will clarify placement constraints

---

## Commit & Push

Once Phase 2 is complete:

```bash
git status
git diff --stat

# Stage all changes (GLB, manifest, turntable PNGs)
git add assets/Production/Generated/
git add artifacts/review/hull-v1/
git add -A

# Commit with clear message
git commit -m "feat: Complete Phase 2 Crash Hull Mk1 asset

- Hull mesh: 11,300 polys across main fuselage, crushed nose, wings, engines, tail
- Material zones: worn off-white hull, structural steel, engine metal, scorch marks
- Interior structure: visible in breach zones, reads as industrial/complex
- Generated via: Blender authoring (guided steps) or create_crash_hull_mk1.py
- Export: assets/Production/Generated/Hero/TC_CRASH_HullMk1_V1.glb
- Manifest: Updated with SHA256 hash
- Review artifacts: Silhouette + turntable (4 angles) + detail close-ups
- Validation: Godot import successful, silhouette matches brief
- Status: ASSET_IMPLEMENTATION_PASS

Next: Phase 3 (Interactables - Workbench, Save, Beacon)

Co-Authored-By: Claude Haiku 4.5 <noreply@anthropic.com>"

git push origin claude/phase-1-blender-export-vrmnfb
```

Good luck with Phase 2! Report completion once:
- GLB exported and validated
- Review artifacts captured
- Manifest updated
- Commit pushed

