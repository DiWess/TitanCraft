# Phase 8 Execution Guide: Lighting & Materials Refinement
## Add Directional Lighting, Ambient Fill, & Emissive Tuning

**Phase:** 8  
**Owner:** Art Director  
**Effort:** ~1.5 hours (setup + validation + re-renders)  
**Status:** `READY_FOR_EXECUTION` (blocked on Phase 7 completion)  
**Prerequisites:** Phase 7 scene (`scenes/CrashSite_MVP.tscn`) complete and loaded

---

## Step 1: Create Directional Light (20 min)

**Objective:** Add sun-like directional light casting shadows across the basin

### 1A: Create DirectionalLight3D Node

```gdscript
# In Godot editor:
# Scene root: Main (or CrashSite_MVP)
#   ├── DirectionalLight3D (new, created this step)
#   ├── Terrain
#   ├── Hull
#   ├── Props
#   └── ... other assets

# Steps:
1. Right-click scene root
2. Add child node
3. Select "DirectionalLight3D"
4. Name: "SunLight"
```

### 1B: Configure Sun Angle & Intensity

```gdscript
var sun = DirectionalLight3D.new()

# Angle: ~45° elevation, northeast-to-southwest (late-day light)
sun.rotation_degrees = Vector3(-45, -45, 0)  # Pitch, yaw, roll

# Intensity: Bright daylight
sun.light_energy = 1.1

# Color: Warm white (late afternoon)
sun.light_color = Color(0.96, 0.96, 0.90, 1.0)  # RGB 245, 245, 230

# Add to scene
add_child(sun)
```

### 1C: Enable & Configure Shadows

```gdscript
# In Godot inspector (or code):
sun.shadow_enabled = true
sun.shadow_map_size = 2048  # Balance quality vs performance
sun.shadow_bias = 0.1  # Standard bias to minimize acne
sun.shadow_pancake_size = 0.0  # Default
```

**Validation:**
- [ ] Shadow map appears in viewport (black areas with directional shading)
- [ ] Shadows cast by terrain and hull are smooth and believable
- [ ] No "acne" (flickering black spots) on surfaces
- [ ] Performance still ~60 FPS (check monitor in Godot)

---

## Step 2: Create Ambient Light (15 min)

**Objective:** Add fill light to prevent pure-black shadow areas

### 2A: Create WorldEnvironment Node

```gdscript
# In Godot editor:
# Scene root: Main
#   ├── WorldEnvironment (new, created this step)
#   ├── DirectionalLight3D (from Step 1)
#   └── ... other assets

# Steps:
1. Right-click scene root
2. Add child node
3. Select "WorldEnvironment"
4. Name: "Environment"
```

### 2B: Configure Ambient Light

```gdscript
var env = WorldEnvironment.new()
var env_data = Environment.new()

# Ambient light color: Neutral blue-gray (reflected sky)
env_data.ambient_light_source = Environment.AMBIENT_LIGHT_FIXED
env_data.ambient_light_color = Color(0.70, 0.75, 0.80, 1.0)  # RGB 180, 190, 205
env_data.ambient_light_energy = 0.35  # 35% fill intensity

# Apply environment
env.environment = env_data
add_child(env)
```

**Validation:**
- [ ] Shadow areas are no longer pure black
- [ ] Shadows have visible detail (not washed out)
- [ ] Ambient light strength feels natural (not over-bright)
- [ ] Check: directional shadows still clearly visible against fill

---

## Step 3: Optional — Add Fog (Atmospheric Effect)

**Duration:** ~10 min (optional, skip if time-constrained)

**Objective:** Add subtle fog to enhance distance depth perception

### 3A: Configure Fog in Environment

```gdscript
var env_data = Environment.new()  # From Step 2

# Fog parameters
env_data.fog_enabled = true
env_data.fog_air_density = 0.01  # Exponential fog density
env_data.fog_aerial_perspective = 0.1  # Slight aerial perspective

# OR use linear fog for distance-based control:
# env_data.fog_aerial_perspective = 0.0
# env_data.fog_density = 0.0001  # Linear fog density

# Fog color: Match ambient light
env_data.fog_light_color = Color(0.70, 0.75, 0.80, 1.0)
env_data.fog_sun_scatter = 0.1  # Slight sun scattering
```

**Validation:**
- [ ] Fog is subtle (not obscuring nearby objects)
- [ ] Distant objects fade naturally (beacon, far horizon)
- [ ] Frame rate unchanged or <5% impact
- [ ] Emissive glows (orange, red, cyan, purple) still visible through fog

---

## Step 4: Re-Validate Emissive Visibility (30 min)

**Objective:** Test that all emissive materials remain visible under directional lighting

### 4A: Load Scene & Switch to Lit Viewport

```gdscript
# In Godot:
1. Load scene: scenes/CrashSite_MVP.tscn
2. Switch to "Lit" viewport mode (top-right camera icon)
3. Enable directional shadows if not visible
4. Zoom/pan to test each asset
```

### 4B: Visual Emissive Checklist

Test each asset at gameplay distance with emissive visibility:

| Asset | Color | Distance | Visibility Test |
|-------|-------|----------|-----------------|
| Workbench | Orange | 10m+ | ☐ Glow visible, not dim |
| Beacon Dormant | Red | 20m+ | ☐ Red LED subtle but visible |
| Beacon Active | Purple | 20m+ | ☐ Intense purple glow, menacing |
| Save Point | Cyan | 10m+ | ☐ Cyan band visible, safe feeling |
| Electronics Pickup | Cyan | 8–10m | ☐ Cyan LED glow obvious |
| Component Pickup | Purple | 7–10m | ☐ Purple glow visible |
| Scout Threat Core | Purple | 15m+ | ☐ Intense menacing glow |
| Scout Leg Accent | Purple | 15m+ | ☐ Leg glow path visible |
| Mech Arm Seams | Purple | FPS POV | ☐ Energy seams glow in corner |

**If emissive is too dim after lighting:**
- Increase emissive strength in material parameter
- Example: `material.emission_energy_multiplier = 1.5` (increase from 1.0)
- Re-test distance visibility

### 4C: Scene Screenshot (Proof of Validation)

```bash
# In Godot editor:
1. Arrange viewport to show full scene in good lighting
2. Screenshot: F12 or menu → File → Take Screenshot
3. Save to: artifacts/phase-8-lighting-proof.png
4. Verify: All emissive materials visible, shadows clear
```

---

## Step 5: Material Parameter Tuning (30 min)

**Objective:** Adjust metalness, roughness, emissive strength per material spec

### 5A: Material Adjustment Workflow

For each material, verify parameters match spec:

```gdscript
# Example: Workbench orange accent
var workbench = load("res://assets/glb/MVP_Pack_V1/TC_PROP_Workbench_V1.glb")
var mat = workbench.get_surface_override_material(0)

# Check emissive
mat.emission_energy_multiplier = 1.0  # Spec: 0.7–1.0
mat.emission_color_texture.emission_color = Color(0.94, 0.55, 0.24)  # Orange

# Check metalness
mat.metallic = 0.0  # Not metal (optional accent)
mat.roughness = 0.5
```

### 5B: Material Checklist (Per Spec)

**Workbench:**
- [ ] Orange accent emissive 0.7–1.0 (check: glow visible)
- [ ] Beige hull metalness ~0.4, roughness ~0.6 (check: realistic)

**Beacon Dormant:**
- [ ] Red LED emissive 0.5 (check: subtle glow)
- [ ] Obelisk form metalness ~0.2, roughness ~0.7

**Beacon Active:**
- [ ] Purple crystal emissive 2.5+ (check: intense glow)
- [ ] Crystal faceting roughness ~0.3 (check: light-catching)

**Save Point:**
- [ ] Cyan glow emissive 1.2+ (check: visible at 10m)
- [ ] Pillar metalness ~0.1, roughness ~0.8

**Pickups:**
- [ ] Metal: metalness 0.8, roughness 0.5 (reflective)
- [ ] Biomass: metalness 0.0, roughness 0.8 (matte organic)
- [ ] Electronics: cyan LED emissive 4.0, base metalness 0.65
- [ ] Component: purple emissive 1.4, substrate metalness 0.2

**Scout:**
- [ ] Threat core emissive 7.0 (check: intense menace)
- [ ] Armor metalness 0.55–0.65, roughness 0.38–0.48

**Mech Arm:**
- [ ] Purple seams emissive 5.0 (check: energy feel)
- [ ] Dark metal metalness 0.65+, roughness 0.4–0.5

---

## Step 6: Performance Validation (10 min)

**Objective:** Confirm 60 FPS target maintained with lighting

### 6A: Monitor Frame Rate

```gdscript
# In Godot Debugger:
1. Enable FPS monitor (top-right, Performance tab)
2. Move camera around scene (pan, rotate, zoom)
3. Watch FPS counter
4. Note any drops below 55 FPS (15% margin on 60 FPS target)
```

### 6B: Optimization If Needed

If FPS drops below 55:

```gdscript
# Option 1: Reduce shadow resolution
sun.shadow_map_size = 1024  # From 2048

# Option 2: Disable fog (if enabled)
env_data.fog_enabled = false

# Option 3: Check scene complexity (Settings → Project → Rendering)
# May need LOD culling for distant assets (terrain, hull)
```

**Validation:**
- [ ] FPS ≥ 55 (target 60)
- [ ] No stuttering or frame drops during camera movement
- [ ] Shadows smooth (no flickering)

---

## Step 7: Final Render & Documentation (15 min)

**Objective:** Capture proof screenshot for Phase 8 completion

### 7A: Arrange Scene View

```gdscript
# Position camera for good lighting proof:
1. Place camera at spawn (0, 0, 1.7)
2. Look toward distant landmarks (workbench, beacon)
3. Verify: All emissive materials visible, shadows clear
4. Screenshot (F12 or menu → File → Take Screenshot)
5. Save to: artifacts/phase-8-lighting-final.png
```

### 7B: Document Results

Create brief text file summarizing Phase 8:

```markdown
# Phase 8 Completion Report

**Date:** 2026-07-05  
**Lighting Status:** COMPLETE

## Setup
- Directional light: 45° sun angle, intensity 1.1
- Ambient light: Neutral blue-gray fill, strength 0.35
- Fog: [Enabled / Disabled] (optional)

## Emissive Validation
- Workbench orange: ✅ Visible at 10m+
- Beacon red: ✅ Visible at 20m+
- Beacon purple: ✅ Visible at 20m+
- Save Point cyan: ✅ Visible at 10m+
- Scout threat: ✅ Visible at 15m+
- All others: ✅ Visible at gameplay distance

## Performance
- FPS target (60): ✅ Maintained
- No stuttering: ✅ Confirmed
- Shadow quality: ✅ Good

## Artifacts
- Screenshot: artifacts/phase-8-lighting-final.png
- Scene: scenes/CrashSite_MVP.tscn (updated)

## Status
Phase 8: **ASSET_IMPLEMENTATION_PASS**

Next: Phase 9 (Visual Artifact Factory)
```

---

## Troubleshooting Guide

| Issue | Symptom | Solution |
|-------|---------|----------|
| **Emissive too dim** | Glow barely visible | Increase `emission_energy_multiplier` in material (e.g., 1.0 → 1.5) |
| **Shadows too dark** | Black areas, low visibility | Increase ambient light strength (0.35 → 0.45) |
| **Shadows too soft** | Loss of detail in shadow areas | Increase shadow map resolution (1024 → 2048) |
| **FPS drops** | Below 55 FPS during movement | Reduce shadow map size or disable fog |
| **Emissive washed out** | Glows disappear in bright areas | Check bloom post-processing; may need engine setting adjustment |
| **Light too warm/cool** | Color tone feels off | Adjust directional light color (RGB tuple) |

---

## Checklist: Phase 8 Complete

- [ ] DirectionalLight3D created and configured (45° angle, intensity 1.1)
- [ ] Shadows enabled (2048 resolution) and visible
- [ ] WorldEnvironment created with ambient fill (0.35 strength)
- [ ] Emissive visibility validated (all glows visible at gameplay distance)
- [ ] Material parameters checked and adjusted per spec (emissive, metalness, roughness)
- [ ] Optional fog added (if time permits)
- [ ] Performance validated (60 FPS target maintained)
- [ ] Scene screenshot captured (proof of lighting)
- [ ] Scene saved: `scenes/CrashSite_MVP.tscn`
- [ ] Completion report written

---

## Next Steps

After Phase 8 `ASSET_IMPLEMENTATION_PASS`:

1. **Phase 9: Visual Artifact Factory** (~1 hour)
   - Render turntables for all 9 asset types
   - Capture gameplay screenshots (5 views)
   - Build asset manifest JSON
   - Final gate approval

2. **Final Gate:** `VISUAL_SLICE_GAMEPLAY_SAFE` — Ready for gameplay integration

---

## Success Criteria

✅ **Phase 8 is `ASSET_IMPLEMENTATION_PASS` when:**

1. Directional lighting configured and casting shadows
2. Ambient light prevents pure-black shadows
3. **All emissive materials glow visibly:** Orange, red, cyan, purple all glowing at gameplay distance
4. Distance visibility tests re-run and passed
5. Material parameters optimized (emissive, metalness, roughness)
6. Optional fog/atmosphere added (if time permits)
7. Performance stable (60 FPS target maintained)
8. Scene screenshot captured showing full lighting
9. Scene saved to `scenes/CrashSite_MVP.tscn`

---

## Timeline

| Step | Duration | Cumulative |
|------|----------|-----------|
| 1: Directional Light | 20 min | 20 min |
| 2: Ambient Light | 15 min | 35 min |
| 3: Fog (optional) | 10 min | 45 min |
| 4: Emissive Validation | 30 min | 75 min |
| 5: Material Tuning | 30 min | 105 min |
| 6: Performance Check | 10 min | 115 min |
| 7: Final Render | 15 min | 130 min |
| **TOTAL** | — | **~2.2 hours (accounts for iteration/troubleshooting)** |

**Recommended:** Schedule 1.5–2 hours uninterrupted time to complete Phase 8.
