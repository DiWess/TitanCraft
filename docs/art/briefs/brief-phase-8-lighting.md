# Phase 8 Brief: Lighting & Materials Refinement
## Scene Illumination & Emissive Optimization

**Phase:** 8  
**Owner:** Art Director  
**Scope:** Directional lighting, ambient lighting, material parameter tuning  
**Effort:** ~1.5 hours  
**Status:** `READY_FOR_EXECUTION` (blocked on Phase 7 completion)  
**Next Gate:** `ASSET_IMPLEMENTATION_PASS` (visibility re-validation + stable performance)

---

## Overview

Phase 8 adds lighting to the composed Phase 7 scene. The goal is to illuminate the environment while preserving visual readability established in Phases 1–7.

All emissive materials (orange, red, cyan, purple) must remain visible and menacing at gameplay distance (5–20m+).

---

## Lighting Specification

### Directional Light (Sun)

**Position & Angle:**
- Source: Elevated angle, casting shadows across basin
- Suggested: 45° above horizon, azimuth ~315° (northeast to southwest, late-day light)
- Intensity: 1.0–1.2 (bright day, clear shadows)

**Color:** Neutral white-ish (RGB ~245, 245, 230) — slight warm tone

**Shadows:**
- Enable dynamic shadows (hull, terrain formations, props all cast)
- Shadow resolution: 2048×2048 (balance quality + performance)
- Shadow bias: Standard (minimize acne/peter-panning)

**Effect:** Creates depth, emphasizes terrain topology, suggests crashed environment (late day rescue operation).

### Ambient Light

**Strength:** 0.3–0.4 (fill light, prevents pure black shadows)  
**Color:** Neutral blue-gray (RGB ~180, 190, 200) — suggests reflected sky  
**Purpose:** Illuminates shadow areas without washing out directional contrast

### Optional: Atmospheric Effects

**Fog (Optional, if performance allows):**
- Type: Exponential or linear fog
- Color: Same as ambient (neutral blue-gray)
- Start distance: 30m
- End distance: 80m+
- Effect: Suggests haze over terrain, enhances distance depth cue

**Particle Effects (Optional):**
- Dust motes near Scout (suggests threat activation)
- Not blocking; implement if time permits

---

## Material Parameter Tuning

### Emissive Strength Validation

After lighting is added, re-validate emissive visibility:

| Asset | Color | Target Strength | Test Distance | Must Be Visible |
|-------|-------|-----------------|----------------|-----------------|
| Workbench | Orange | 0.7–1.0 | 10m+ | ✅ |
| Beacon Dormant | Red | 0.5 (dim) | 20m+ | ✅ |
| Beacon Active | Purple | 2.5+ (intense) | 20m+ | ✅ |
| Save Point | Cyan | 1.2+ | 10m+ | ✅ |
| Electronics Pickup | Cyan | 4.0 | 8–10m | ✅ |
| Component Pickup | Purple | 1.4 | 7–10m | ✅ |
| Scout Threat Core | Purple | 7.0 (menacing) | 15m+ | ✅ |
| Scout Leg Glow | Purple | 3.0 (accent) | 15m+ | ✅ |
| Mech Arm Seams | Purple | 5.0 (energy) | FPS POV | ✅ |

**If any emissive is too dim after lighting:**
- Increase emissive strength in material parameter (not brightness slider)
- Re-export asset if source material was modified
- Revalidate distance visibility

### Other Material Parameters

**Metalness Tuning:**
- Metal pickup: 0.8 (reflective, shows directional light well)
- Scout armor: 0.55–0.65 (reflects light realistically)
- Mech arm: Dark metal, 0.65+ (industrial appearance)

**Roughness Tuning:**
- Smooth polished surfaces: 0.3–0.4 (shows specular highlights)
- Weathered/worn surfaces: 0.5–0.7 (matte appearance)
- Organic surfaces (Biomass): 0.8+ (low gloss, natural look)

---

## Godot Implementation Checklist

### Lighting Setup

- [ ] Create DirectionalLight3D node in scene root
- [ ] Set position and rotation for 45° sun angle
- [ ] Configure intensity (1.0–1.2)
- [ ] Enable shadows (resolution 2048×2048)
- [ ] Set color to warm white (RGB ~245, 245, 230)

### Ambient Light

- [ ] Create WorldEnvironment node (if not exists)
- [ ] Configure ambient light color (neutral blue-gray)
- [ ] Set ambient light strength (0.3–0.4)
- [ ] Test in editor: shadows not pure black

### Optional Fog

- [ ] Add fog to WorldEnvironment (if time permits)
- [ ] Configure start/end distances (30m–80m+)
- [ ] Set color to match ambient
- [ ] Validate performance impact (fps target 60+)

### Material Re-Validation

- [ ] Load scene with all assets
- [ ] Switch to lit viewport (not unlit)
- [ ] Visually check: all emissive materials glow
- [ ] Confirm: orange, red, cyan, purple all visible
- [ ] Verify: directional shadows are smooth and believable

---

## Distance Visibility Re-Validation

After lighting is complete, run quick FPS POV tests:

### Test 1: Workbench from Spawn (10m)
- [ ] Orange panel visible and glowing
- [ ] Arm silhouette readable despite shadows
- [ ] Bench base clearly grounded

### Test 2: Beacon from Workbench (20m)
- [ ] Red dormant glow visible at distance
- [ ] Obelisk form clear (not lost in shadow)
- [ ] Ready for activation trigger

### Test 3: Save Point from Spawn (8m)
- [ ] Cyan glow obvious
- [ ] Pillar form clear
- [ ] Signals "safety checkpoint"

### Test 4: Scout in Arena (15m)
- [ ] Purple threat core visible and menacing
- [ ] Four-legged form readable
- [ ] Menacing aesthetic not diminished by shadows

### Test 5: Pickups in Resource Clusters (5–10m)
- [ ] All four types identifiable
- [ ] Cyan (Electronics) and orange (Component) accents glow
- [ ] Color language clear despite shadows

---

## Performance Checklist

- [ ] DirectionalLight enabled (dynamic shadows)
- [ ] Performance target: 60 FPS on target hardware
- [ ] No frame drops when moving camera
- [ ] Fog (if used) does not degrade FPS by >5%
- [ ] Scene file size reasonable (~10–20 MB)

---

## Success Criteria

✅ **Phase 8 is `ASSET_IMPLEMENTATION_PASS` when:**

1. Directional lighting configured and casting shadows
2. Ambient light prevents pure black shadows
3. All emissive materials glow visibly (orange, red, cyan, purple all glowing)
4. Distance visibility tests re-run and passed (all props readable at gameplay distance)
5. Material parameters optimized (emissive, metalness, roughness tuned per spec)
6. Optional fog/atmosphere added (if time permits; not blocking)
7. Performance stable (60 FPS target maintained)
8. Scene screenshot captured showing full lighting setup

---

## Troubleshooting

| Issue | Symptom | Solution |
|-------|---------|----------|
| **Emissive too dim** | Glow barely visible after lighting | Increase emissive strength in material (not global brightness) |
| **Shadows too dark** | Black areas in scene, low visibility | Increase ambient light strength (0.4–0.5) |
| **Shadows too soft** | Loss of detail in shadow areas | Increase shadow resolution (2048→4096) |
| **Performance drop** | FPS below 60 | Disable fog; reduce shadow resolution; check scene complexity |
| **Emissive washed out** | Glows disappear in bright areas | Increase emissive strength; check bloom post-processing |

---

## Next Steps

After Phase 8 `ASSET_IMPLEMENTATION_PASS`:

1. **Phase 9: Visual Artifact Factory** (~1 hour)
   - Render turntables for all assets
   - Capture gameplay screenshots
   - Build asset manifest report
   - Final gate approval

2. **Final Gate:** `VISUAL_SLICE_GAMEPLAY_SAFE` — Scene ready for gameplay integration

---

## Gate Transition

| Gate | Status | Blocker | Approval |
|------|--------|---------|----------|
| `ASSET_IMPLEMENTATION_PASS` (Phase 8) | Pending | Lighting + visibility validation | Art Director |
| `VISUAL_SLICE_GAMEPLAY_SAFE` | Pending | Phase 9 artifacts | Project Director |
