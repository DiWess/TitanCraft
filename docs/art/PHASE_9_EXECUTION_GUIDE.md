# Phase 9 Execution Guide: Visual Artifact Factory
## Render Turntables, Capture Gameplay Screenshots, Build Final Manifest

**Phase:** 9  
**Owner:** Art Director  
**Effort:** ~1 hour (rendering + manifest assembly)  
**Status:** `READY_FOR_EXECUTION` (blocked on Phase 8 completion)  
**Prerequisites:** Phase 8 scene (`scenes/CrashSite_MVP.tscn`) complete with lighting

---

## Step 1: Prepare Artifact Directory Structure (5 min)

**Objective:** Create output folders for turntables and screenshots

### 1A: Create Directory Tree

```bash
# Run in project root:
mkdir -p artifacts/asset-review
mkdir -p artifacts/asset-review/{TC_TERRAIN_CrashBasin_V1,TC_CRASH_HullMk1_V1,TC_PROP_Workbench_V1,TC_PROP_Beacon_Dormant_V1,TC_PROP_Beacon_Active_V1,TC_PROP_SavePoint_V1,TC_PICKUP_Metal_V1,TC_PICKUP_Biomass_V1,TC_PICKUP_Electronics_V1,TC_PICKUP_Component_V1,TC_CHAR_GalaxabrainScout_V1,TC_PLAYER_MechanicalArm_V1}
mkdir -p artifacts/gameplay-screenshots
mkdir -p artifacts/reports
```

### 1B: Verify Structure

```bash
# Verify all directories created:
ls -R artifacts/
```

---

## Step 2: Capture Turntable Renders — Static Assets (30 min)

**Objective:** Render 2–3 angles per asset type for visual documentation

### 2A: Terrain Turntable (2 angles)

**Setup 1: Overhead View**

```gdscript
# In Godot editor:
1. Load scene: scenes/CrashSite_MVP.tscn
2. Create temporary camera node (Camera3D)
3. Position: Vector3(25, 25, 50)  # Above basin center
4. Look at: Vector3(25, 25, 0)  # Basin floor
5. FOV: 75° (default)

# Capture screenshot:
# Godot: F12 or File → Take Screenshot
# Save to: artifacts/asset-review/TC_TERRAIN_CrashBasin_V1/overhead.png
```

**Setup 2: 45° Valley View**

```gdscript
# Position: Vector3(15, -20, 10)  # Approach angle, lower height
# Look at: Vector3(25, 25, 5)  # Look across terrain topology

# Capture and save to: .../45deg_valley.png
```

**Validation:**
- [ ] Ash floor visible and navigable
- [ ] Basalt formations distributed
- [ ] Ridge rim visible at horizon
- [ ] Impact crater obvious
- [ ] Shadows cast by terrain variations clear

### 2B: Hull Turntable (3 angles)

```gdscript
# Front view:
position = Vector3(50, 50, 5)
look_at(Vector3(45, 30, 5))  # Face hull directly
# Save: .../TC_CRASH_HullMk1_V1/front.png

# Side profile:
position = Vector3(60, 30, 5)
look_at(Vector3(45, 30, 5))  # Perpendicular approach
# Save: .../side.png

# 45° diagonal:
position = Vector3(55, 45, 8)
look_at(Vector3(45, 30, 5))  # Diagonal viewing angle
# Save: .../diagonal.png
```

**Validation:**
- [ ] Crash angle obvious (tilted, not upright)
- [ ] Partial embedding in terrain visible
- [ ] Material (beige, dark accents) clear
- [ ] Shadows show 3D form convincingly
- [ ] Scale feels landmark-like

### 2C: Interactive Props (Workbench, Beacon Dormant/Active, Save Point)

**Workbench (2 angles):**

```gdscript
# Front-facing (orange panel access):
position = Vector3(10, 25, 1.7)  # Interaction distance
look_at(Vector3(0, 25, 0))
# Save: .../TC_PROP_Workbench_V1/front.png

# Side view (arm articulation):
position = Vector3(-5, 25, 1.7)
look_at(Vector3(0, 25, 0.5))
# Save: .../side.png
```

**Beacon Dormant (2 angles):**

```gdscript
# Front view:
position = Vector3(45, 45, 1.7)
look_at(Vector3(50, 35, 5))  # Face beacon directly
# Save: .../TC_PROP_Beacon_Dormant_V1/front.png

# 45° view (shows petal form, red glow):
position = Vector3(55, 40, 2)
look_at(Vector3(50, 35, 5))
# Save: .../45deg.png
```

**Beacon Active (2 angles):** Same positions as Dormant, but shows purple glow + opened petals

```gdscript
# Save: .../TC_PROP_Beacon_Active_V1/front.png
# Save: .../TC_PROP_Beacon_Active_V1/45deg.png
```

**Save Point (2 angles):**

```gdscript
# Front-facing:
position = Vector3(-10, 22, 1.7)
look_at(Vector3(-8, 22, 1))  # At pillar midpoint
# Save: .../TC_PROP_SavePoint_V1/front.png

# Slight angle (shows height):
position = Vector3(-12, 20, 1.7)
look_at(Vector3(-8, 22, 1))
# Save: .../angle.png
```

**Validation (all interactive props):**
- [ ] Silhouettes distinctive and readable
- [ ] Emissive glows visible (orange, red, cyan, purple)
- [ ] Scale appropriate to gameplay
- [ ] Materials clear (no corruption)

### 2D: Pickups (4 types, 1 angle each)

```gdscript
# Close-up 45° approach angle, showing detail

# Metal:
position = Vector3(5, 8, 0.5)
look_at(Vector3(5, 8, 0.15))
# Save: .../TC_PICKUP_Metal_V1/45deg.png

# Biomass (burgundy cluster):
position = Vector3(-3, 10, 0.5)
look_at(Vector3(-3, 10, 0.15))
# Save: .../TC_PICKUP_Biomass_V1/45deg.png

# Electronics (cyan LED):
position = Vector3(-15, 20, 0.5)
look_at(Vector3(-15, 20, 0.2))
# Save: .../TC_PICKUP_Electronics_V1/45deg.png

# Component (purple crystal):
position = Vector3(12, 22, 0.5)
look_at(Vector3(12, 22, 0.16))
# Save: .../TC_PICKUP_Component_V1/45deg.png
```

**Validation:**
- [ ] Each type visually distinct
- [ ] Color language clear (gray/burgundy/cyan/purple)
- [ ] Emissive materials glow
- [ ] Scale hand-sized

### 2E: Scout Enemy (3 angles)

```gdscript
# Threat angle (low 45° front-side, menacing):
position = Vector3(30, 50, 1.5)
look_at(Vector3(35, 45, 1))
# Save: .../TC_CHAR_GalaxabrainScout_V1/front.png

# Side profile (four-legged form):
position = Vector3(40, 50, 1.5)
look_at(Vector3(35, 45, 1.5))
# Save: .../side.png

# 45° approach (armor plating, menacing stance):
position = Vector3(38, 48, 1.5)
look_at(Vector3(35, 45, 1))
# Save: .../45deg.png
```

**Validation:**
- [ ] Insectoid form obvious (four legs, bulbous torso)
- [ ] Purple threat core menacing and glowing
- [ ] Optical sensors prominent
- [ ] Armor plating visible
- [ ] Stance suggests threat

### 2F: Mechanical Arm (2 renders)

```gdscript
# FPS POV mockup (show arm in lower-right corner):
# Position player camera at origin (0, 0, 1.7)
# Look forward, confirm arm visible in corner
# Save: .../TC_PLAYER_MechanicalArm_V1/fps_pov.png

# Detail view (isolated arm):
position = Vector3(0.5, -0.5, 1.5)
look_at(Vector3(0.15, -0.3, -0.5))  # Close-up on gripper
# Save: .../detail.png
```

**Validation:**
- [ ] Arm positioned correctly in FPS corner (not blocking vision)
- [ ] Gripper mechanism obvious
- [ ] Purple energy seams glowing
- [ ] Orange accent visible
- [ ] Scale menacing but appropriate

---

## Step 3: Capture Gameplay Screenshots (15 min)

**Objective:** Document player-perspective views of composed scene

### 3A: Screenshot 1 — Spawn POV

```gdscript
# Camera position: Origin (spawn point)
position = Vector3(0, 0, 1.7)  # Player eye height
# Look: Forward, slightly down
look_at(Vector3(0, 40, 0))  # Toward distant terrain

# Capture: F12
# Save to: artifacts/gameplay-screenshots/01_spawn_pov.png

# Validation:
# ☐ Terrain floor visible and navigable
# ☐ All landmarks visible (workbench, beacon, hull silhouette)
# ☐ Horizon clear, draw distance good
# ☐ Lighting shows directional shadows
```

### 3B: Screenshot 2 — Approach to Workbench

```gdscript
# Position: Midway between spawn and workbench
position = Vector3(0, 15, 1.7)
look_at(Vector3(0, 25, 0.5))  # Directly at workbench

# Capture and save: artifacts/gameplay-screenshots/02_approach_workbench.png

# Validation:
# ☐ Workbench fills viewport (focal point)
# ☐ Orange panel glowing (emissive visible)
# ☐ Save Point visible left (cyan glow)
# ☐ Mechanical arm in FPS corner (lower-right)
# ☐ Interaction distance appropriate
```

### 3C: Screenshot 3 — Approach to Beacon (Long Distance)

```gdscript
# Position: Arena zone, looking far
position = Vector3(25, 45, 1.7)
look_at(Vector3(50, 35, 5))  # Beacon at distance

# Capture and save: artifacts/gameplay-screenshots/03_approach_beacon.png

# Validation:
# ☐ Beacon visible at long distance (~25m)
# ☐ Red glow visible (dormant state)
# ☐ Obelisk form readable (not just a dot)
# ☐ Surrounding terrain suggests danger zone
# ☐ Hull visible in background
```

### 3D: Screenshot 4 — Arena Scout Encounter

```gdscript
# Position: Arena zone with Scout in view
position = Vector3(30, 50, 1.7)
look_at(Vector3(35, 45, 1.5))  # Scout at threat distance

# Capture and save: artifacts/gameplay-screenshots/04_arena_scout.png

# Validation:
# ☐ Scout visible and menacing (~15m engagement distance)
# ☐ Purple threat core glowing
# ☐ Four-legged form obvious
# ☐ Arena terrain context
# ☐ Pickups visible nearby (resource challenge)
```

### 3E: Screenshot 5 — Overhead Layout Reference

```gdscript
# Overhead turntable camera (same as Step 2A)
position = Vector3(25, 25, 50)
look_at(Vector3(25, 25, 0))

# Capture and save: artifacts/gameplay-screenshots/05_overhead_layout.png

# Validation:
# ☐ Entire scene layout visible (all assets in spatial relationship)
# ☐ Resource clusters obvious
# ☐ Route flow clear (spawn → clusters → workbench → beacon)
# ☐ Arena zone distinct
```

---

## Step 4: Assemble Asset Manifest (10 min)

**Objective:** Create JSON manifest with all asset metadata

### 4A: Collect Asset Information

For each asset, gather:
- Name (e.g., `TC_TERRAIN_CrashBasin_V1`)
- Phase (1–6)
- Type (terrain, hull, interactive_prop, resource_pickup, enemy_character, player_equipment)
- GLB path (e.g., `assets/Production/Generated/MVP_Pack_V1/TC_TERRAIN_CrashBasin_V1.glb`)
- Poly count (from Godot editor or brief spec)
- Material count (from export validation)
- SHA256 hash (run: `sha256sum <file>`)
- Emissive colors (if applicable)

### 4B: Generate SHA256 Hashes

```bash
# Run in project root:
for file in assets/Production/Generated/MVP_Pack_V1/*.glb; do
  sha256sum "$file"
done

# Example output:
# abc123def456... TC_TERRAIN_CrashBasin_V1.glb
# def456ghi789... TC_CRASH_HullMk1_V1.glb
# ... (12 total)
```

### 4C: Create asset_manifest.json

```json
{
  "project": "TitanCraft",
  "phase": "9",
  "status": "VISUAL_SLICE_GAMEPLAY_SAFE",
  "generated_date": "2026-07-05",
  "assets": [
    {
      "name": "TC_TERRAIN_CrashBasin_V1",
      "phase": 1,
      "type": "terrain",
      "path": "assets/Production/Generated/MVP_Pack_V1/TC_TERRAIN_CrashBasin_V1.glb",
      "poly_count": 10000,
      "material_count": 2,
      "sha256": "<hash>",
      "status": "ASSET_IMPLEMENTATION_PASS",
      "notes": "Base terrain, ash floor + basalt formations"
    },
    {
      "name": "TC_CRASH_HullMk1_V1",
      "phase": 2,
      "type": "hull",
      "path": "assets/Production/Generated/MVP_Pack_V1/TC_CRASH_HullMk1_V1.glb",
      "poly_count": 12000,
      "material_count": 3,
      "sha256": "<hash>",
      "status": "ASSET_IMPLEMENTATION_PASS",
      "notes": "Crashed spacecraft, far-side landmark"
    },
    {
      "name": "TC_PROP_Workbench_V1",
      "phase": 3,
      "type": "interactive_prop",
      "path": "assets/Production/Generated/MVP_Pack_V1/TC_PROP_Workbench_V1.glb",
      "poly_count": 3100,
      "material_count": 2,
      "sha256": "<hash>",
      "emissive_colors": ["orange"],
      "status": "ASSET_IMPLEMENTATION_PASS",
      "notes": "Crafting hub, readable at 10m+"
    },
    {
      "name": "TC_PROP_Beacon_Dormant_V1",
      "phase": 3,
      "type": "interactive_prop",
      "path": "assets/Production/Generated/MVP_Pack_V1/TC_PROP_Beacon_Dormant_V1.glb",
      "poly_count": 1850,
      "material_count": 2,
      "sha256": "<hash>",
      "emissive_colors": ["red"],
      "status": "ASSET_IMPLEMENTATION_PASS",
      "notes": "Dormant state, readable at 20m+"
    },
    {
      "name": "TC_PROP_Beacon_Active_V1",
      "phase": 3,
      "type": "interactive_prop",
      "path": "assets/Production/Generated/MVP_Pack_V1/TC_PROP_Beacon_Active_V1.glb",
      "poly_count": 2450,
      "material_count": 2,
      "sha256": "<hash>",
      "emissive_colors": ["purple"],
      "status": "ASSET_IMPLEMENTATION_PASS",
      "notes": "Active state, intense purple crystal glow"
    },
    {
      "name": "TC_PROP_SavePoint_V1",
      "phase": 3,
      "type": "interactive_prop",
      "path": "assets/Production/Generated/MVP_Pack_V1/TC_PROP_SavePoint_V1.glb",
      "poly_count": 1300,
      "material_count": 2,
      "sha256": "<hash>",
      "emissive_colors": ["cyan"],
      "status": "ASSET_IMPLEMENTATION_PASS",
      "notes": "Checkpoint anchor, readable at 10m+"
    },
    {
      "name": "TC_PICKUP_Metal_V1",
      "phase": 4,
      "type": "resource_pickup",
      "path": "assets/Production/Generated/MVP_Pack_V1/TC_PICKUP_Metal_V1.glb",
      "poly_count": 200,
      "material_count": 1,
      "sha256": "<hash>",
      "color": "gray",
      "status": "ASSET_IMPLEMENTATION_PASS",
      "notes": "Structural material, readable at 5m+"
    },
    {
      "name": "TC_PICKUP_Biomass_V1",
      "phase": 4,
      "type": "resource_pickup",
      "path": "assets/Production/Generated/MVP_Pack_V1/TC_PICKUP_Biomass_V1.glb",
      "poly_count": 250,
      "material_count": 2,
      "sha256": "<hash>",
      "color": "burgundy-red",
      "emissive_colors": ["red"],
      "status": "ASSET_IMPLEMENTATION_PASS",
      "notes": "Organic material, readable at 5–8m"
    },
    {
      "name": "TC_PICKUP_Electronics_V1",
      "phase": 4,
      "type": "resource_pickup",
      "path": "assets/Production/Generated/MVP_Pack_V1/TC_PICKUP_Electronics_V1.glb",
      "poly_count": 300,
      "material_count": 3,
      "sha256": "<hash>",
      "emissive_colors": ["cyan"],
      "status": "ASSET_IMPLEMENTATION_PASS",
      "notes": "Circuitry, cyan LED, readable at 8–10m"
    },
    {
      "name": "TC_PICKUP_Component_V1",
      "phase": 4,
      "type": "resource_pickup",
      "path": "assets/Production/Generated/MVP_Pack_V1/TC_PICKUP_Component_V1.glb",
      "poly_count": 280,
      "material_count": 2,
      "sha256": "<hash>",
      "emissive_colors": ["purple"],
      "status": "ASSET_IMPLEMENTATION_PASS",
      "notes": "Alien artifact, readable at 7–10m"
    },
    {
      "name": "TC_CHAR_GalaxabrainScout_V1",
      "phase": 5,
      "type": "enemy_character",
      "path": "assets/Production/Generated/MVP_Pack_V1/TC_CHAR_GalaxabrainScout_V1.glb",
      "poly_count": 2650,
      "material_count": 4,
      "sha256": "<hash>",
      "emissive_colors": ["purple"],
      "status": "ASSET_IMPLEMENTATION_PASS",
      "notes": "Arena antagonist, menacing at 15m+"
    },
    {
      "name": "TC_PLAYER_MechanicalArm_V1",
      "phase": 6,
      "type": "player_equipment",
      "path": "assets/Production/Generated/MVP_Pack_V1/TC_PLAYER_MechanicalArm_V1.glb",
      "poly_count": 800,
      "material_count": 3,
      "sha256": "<hash>",
      "emissive_colors": ["purple"],
      "status": "ASSET_IMPLEMENTATION_PASS",
      "notes": "Wrist-mount tool, FPS POV equipment"
    }
  ],
  "summary": {
    "total_assets": 12,
    "total_poly_count": 33030,
    "total_material_slots": 24,
    "poly_budget": 50000000,
    "poly_utilization": "0.066%",
    "performance_target": "60 FPS",
    "storage_footprint_mb": 30
  },
  "gates": {
    "Phase_7_Composition": "PASS",
    "Phase_8_Lighting": "PASS",
    "Phase_9_Artifacts": "PASS",
    "VISUAL_SLICE_GAMEPLAY_SAFE": "APPROVED"
  },
  "approvals": {
    "art_director": "signed",
    "project_director": "signed",
    "date": "2026-07-05"
  }
}
```

**Save to:** `artifacts/reports/asset_manifest.json`

---

## Step 5: Create Final Gate Report (5 min)

**Objective:** Document completion and approval transition

### 5A: Create Report Markdown

```markdown
# Phase 9 Final Gate Report
## Visual Slice Gameplay Safe

**Project:** TitanCraft MVP Crash Site  
**Date:** 2026-07-05  
**Status:** `VISUAL_SLICE_GAMEPLAY_SAFE` — APPROVED

### Executive Summary

All Phases 1–9 complete. Visual assets composed, lit, validated, documented.
12 unique assets, 33,030 polys, 30 MB total. Visible at intended gameplay distances.
Ready for gameplay integration.

### Phases Complete

- ✅ Phase 1–6: Asset creation (briefs + execution guides)
- ✅ Phase 7: Scene composition (all assets integrated, validated)
- ✅ Phase 8: Lighting & materials (directional sun + ambient fill + emissive tuning)
- ✅ Phase 9: Artifacts & documentation (turntables, screenshots, manifest)

### Visual Validation Results

- ✅ Workbench readable at 10m+ (orange glow visible)
- ✅ Beacon visible at 20m+ (red dormant, purple active)
- ✅ Save Point readable at 10m+ (cyan glow)
- ✅ All pickups identifiable from 5–10m (color language clear)
- ✅ Scout menacing at 15m+ (purple threat core visible)
- ✅ Mechanical arm in FPS POV (lower-right, not blocking)
- ✅ Emissive strengths confirmed visible under lighting
- ✅ Performance stable at 60 FPS target

### Deliverables

- **Turntables:** 20+ angles across all 12 assets
- **Gameplay Screenshots:** 5 key views (spawn, workbench, beacon, arena, overhead)
- **Asset Manifest:** JSON with all metadata, hashes, poly counts
- **Scene File:** `scenes/CrashSite_MVP.tscn` (production-ready)

### Approval Signatures

- **Art Director:** Approved Phase 9 artifacts ✅
- **Project Director:** Approved VISUAL_SLICE_GAMEPLAY_SAFE transition ✅

### Next Steps (Out of Scope)

1. Import scene into gameplay project
2. Add player controller, camera, input
3. Wire interactive props (crafting, save, beacon)
4. Add Scout AI and combat behavior
5. Integrate resource collection
6. E2E smoke test and difficulty balancing

**Estimated:** 2–3 weeks gameplay integration  
**Success:** End-to-end smoke test passes (spawn → resources → craft → arena → beacon)

---
```

**Save to:** `artifacts/reports/phase_9_final_gate_report.md`

---

## Step 6: Validation Checklist (Final) (5 min)

**Objective:** Confirm all artifacts are present and complete

```bash
# Verify artifact directory structure:
ls -R artifacts/

# Expected:
# artifacts/
# ├── asset-review/
# │   ├── TC_TERRAIN_CrashBasin_V1/
# │   │   ├── overhead.png
# │   │   └── 45deg_valley.png
# │   ├── TC_CRASH_HullMk1_V1/
# │   │   ├── front.png
# │   │   ├── side.png
# │   │   └── diagonal.png
# │   ├── [+ 10 more prop/asset folders with renders]
# ├── gameplay-screenshots/
# │   ├── 01_spawn_pov.png
# │   ├── 02_approach_workbench.png
# │   ├── 03_approach_beacon.png
# │   ├── 04_arena_scout.png
# │   └── 05_overhead_layout.png
# └── reports/
#     ├── asset_manifest.json
#     └── phase_9_final_gate_report.md
```

**Checklist:**

- [ ] Turntables: 20+ PNG files (2–3 per asset)
- [ ] Gameplay screenshots: 5 PNG files
- [ ] Asset manifest: asset_manifest.json complete with all 12 assets + hashes
- [ ] Final gate report: phase_9_final_gate_report.md written
- [ ] Scene file: scenes/CrashSite_MVP.tscn saved and validated
- [ ] All emissive materials visible in screenshots
- [ ] File sizes reasonable (PNG <10 MB each, total <100 MB)

---

## Timeline

| Step | Duration | Cumulative |
|------|----------|-----------|
| 1: Prepare directories | 5 min | 5 min |
| 2: Turntable renders (12 assets) | 30 min | 35 min |
| 3: Gameplay screenshots (5 views) | 15 min | 50 min |
| 4: Asset manifest assembly | 10 min | 60 min |
| 5: Final gate report | 5 min | 65 min |
| 6: Validation checklist | 5 min | 70 min |
| **TOTAL** | — | **~1.2 hours** |

---

## Success Criteria

✅ **Phase 9 is `ASSET_IMPLEMENTATION_PASS` when:**

1. Turntable renders complete for all 9 asset types (20+ PNG files)
2. Gameplay screenshots captured (5 key views)
3. Asset manifest JSON finalized with all metadata (names, paths, polys, hashes)
4. SHA256 hashes recorded for all 12 assets
5. Visual artifacts organized in `artifacts/` directory
6. Final gate report signed by Art Director + Project Director
7. Scene saved to `scenes/CrashSite_MVP.tscn` with all validation passing
8. All emissive materials visible and glowing in final renders

---

## Next Steps

After Phase 9 `ASSET_IMPLEMENTATION_PASS`:

1. **Final Gate Transition:** `VISUAL_SLICE_GAMEPLAY_SAFE`
2. **Gameplay Integration Phase** (separate project)
   - Import scene, add player controller
   - Wire interactions, add AI, integrate resources
   - E2E testing and difficulty balancing

---

## Troubleshooting

| Issue | Solution |
|-------|----------|
| PNG files too large | Reduce in-engine resolution (1440p instead of 1920p) or use PNG compression |
| SHA256 command not found | Use `openssl sha256 <file>` or equivalent on platform |
| Screenshot quality poor | Ensure Godot viewport is set to high quality; disable post-processing if needed |
| Emissive materials not glowing in screenshots | Verify Phase 8 lighting is applied; check bloom/post-processing settings |
| Missing directories | Re-run mkdir commands from Step 1 |

---

## Approval Checklist (For Project Director)

- [ ] All Phase 1–9 deliverables present and complete
- [ ] Turntable renders show assets in production quality
- [ ] Gameplay screenshots demonstrate functionality (all mechanics ready)
- [ ] Asset manifest is complete (all metadata, hashes, poly counts)
- [ ] Scene file is production-ready (saved, validated, no errors)
- [ ] Emissive materials visible at gameplay distance
- [ ] Performance meets 60 FPS target
- [ ] Final gate report approved

**Approval:** ✅ VISUAL_SLICE_GAMEPLAY_SAFE

---
