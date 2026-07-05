# Asset Brief: Mechanical Arm V1
## Phase 6 Player Equipment — Salvage Wrist-Mount Tool

**Brief Owner:** TitanCraft Project Director  
**Target Asset:** Right-arm-mounted mechanical tool/manipulator for player avatar  
**Scope:** Single player equipment asset for environmental interaction and combat enhancement  
**Date:** 2026-07-05  
**Design Philosophy:** Industrial salvage exoskeleton, functional and worn, arms player with environmental utility

---

## Visual Thesis

The Mechanical Arm is **player equipment**. It must read as:

- **Functional tool** — not cosmetic, suggests gripping, manipulating, or impacting
- **Right-arm-mounted** — visible in first-person POV (lower-right screen corner and world)
- **Salvage-derived** — repurposed industrial/spacecraft actuator, weathered
- **Wrist-mounted exoskeleton** — worn on player's arm, moves with arm gestures
- **Instantly recognizable** — player knows "this is my tool" from first glance
- **Modest scale** — not oversized, fits player proportions (not a mecha arm)
- **Visually menacing** — enhanced threat capacity with gripper or impact face
- **Material honesty** — dark metal, rivets, hydraulic lines suggest real functionality

The arm signals **"enhanced player capability"** purely through form and detail, without text or UI callouts.

---

## Arm Design & Function

### Primary Form: Exoskeleton Cuff + Manipulator

**Wrist Cuff (Worn on Player's Arm):**
- Mounted on player's right wrist and forearm
- Fitted around player's arm (0.08–0.1m diameter for human arm)
- Hinged or segmented at wrist joint (follows arm rotation)
- Dark metal construction with visible hydraulic lines
- Articulated straps or clamps holding cuff to arm
- Approximately 0.35m long (cuff + mid-forearm)

**Hand/Gripper (End-Effector):**
- Large mechanical gripper or pincer hand
- Two opposed grasping surfaces (like industrial robot gripper)
- OR impact-focused fist-like end (for combat enhancement)
- Articulated fingers or jaws (suggest gripping motion)
- Approximately 0.25m long (gripper dimensions)
- Much larger than human hand (intimidating, tool-like)

**Hydraulic System (Visual Detail):**
- Visible hydraulic lines running from cuff to gripper
- Piston rod or actuator arm visible (suggests mechanical movement)
- Conduits and flexible hoses routed along arm exterior
- Accent color: orange or cyan tubing (suggests power/hydraulic fluid)

### Size & Scale

| Dimension | Target | Rationale |
|-----------|--------|-----------|
| **Cuff length** | ~0.35m | Fits forearm segment without obstruction |
| **Gripper/fist length** | ~0.25m | Menacing scale, visible in FPS POV |
| **Cuff diameter** | ~0.1m | Worn around arm, not bulky |
| **Gripper spread** | ~0.2m | Wide grip reach, suggests grasping power |
| **Visible reach** | In FPS: lower-right corner; melee: 0.5m+ | Interaction and combat range hints |

---

## Design Priorities

**Avoid:**
- ❌ Sleek sci-fi arm (suggests high-tech, not salvage)
- ❌ Humanoid hand proportions (too lifelike, confuses identity)
- ❌ Tiny or delicate appearance (diminishes tool/weapon feel)
- ❌ Clean, pristine condition (looks manufactured, not salvaged)
- ❌ Overly complex detailing (readability suffers in FPS POV)

**Target:**
- ✅ Industrial hydraulic gripper (functional, purposeful form)
- ✅ Dark weathered metal (salvage materials, worn patina)
- ✅ Visible mechanical systems (hydraulic lines, pistons, articulation)
- ✅ Asymmetrical or irregular surfaces (repurposed equipment)
- ✅ Compact wrist-mount (doesn't obstruct vision, movable with arm)
- ✅ Intimidating end-effector (gripper or impact face, clearly weapon/tool)
- ✅ Material honesty (rivets, bolts, welds, conduits all visible)

---

## Structural Breakdown

### 1. Wrist Cuff Assembly

- Curved or cylindrical cuff body (wraps around player arm)
- Hinged or segmented at wrist (allows arm rotation)
- Inner mounting points (straps, clamps, padding)
- Outer plating (dark metal, riveted)
- Joint articulation visible (allows flexion at wrist)

**Poly allocation:** ~400–500 polys (detailed cuff, segmentation)

### 2. Forearm Extension

- Structural arm from cuff to gripper (0.25–0.3m long)
- I-beam or box-tube construction (structural, industrial)
- Visible support struts or ribbing (no solid mass)
- Dark steel with possible accent trim
- Mounting points for hydraulic lines

**Poly allocation:** ~300–400 polys (tubular form, strut detailing)

### 3. Hydraulic System

- Main hydraulic lines running full arm length
- Piston rod from cuff to gripper (suggests actuator)
- Conduit manifolds at cuff and gripper
- Tubing with accent color (orange or cyan)
- Flexible hose details (not rigid pipes)

**Poly allocation:** ~200–300 polys (line routing, connector geometry)

### 4. Gripper/End-Effector (Gripper Design)

**If Gripper Form:**
- Two opposed metal fingers or jaws
- Articulated at knuckle joint (can open/close)
- Textured grip surfaces (grooved for traction)
- Possibly magnetized or sticky pad hints
- Threat language: menacing pincer shape

**If Fist Form:**
- Knuckle-reinforced impact face
- Armor plating over fist volume
- Spike or knuckle duster accents
- Smaller articulation at wrist (rotation only)
- Threat language: melee weapon appearance

**Poly allocation:** ~400–600 polys (detailed gripper jaws or armored fist)

### 5. Accent & Detail

- Weathering and oxidation (rust, patina)
- Battle damage (dents, scratches, burn marks)
- Visible bolts, rivets, weld seams
- Asymmetry (repurposed/salvaged appearance)
- Possible glint or slight reflection (suggests metal)

**Poly allocation:** ~200–300 polys (edge geometry, damage marks)

---

## Poly Count & Complexity

| Component | Target Poly Budget | Detail Level |
|-----------|-------------------|--------------|
| **Wrist Cuff** | ~450 | Medium: curved body, segmentation, straps |
| **Forearm Extension** | ~350 | Medium: tubular form, struts, joints |
| **Hydraulic System** | ~250 | Low–Medium: line routing, pistons, conduits |
| **Gripper/Fist** | ~500 | Medium–High: fingers/knuckles, articulation |
| **Accent & Detail** | ~250 | Low: weathering, bolts, damage marks |
| **TOTAL** | **~1,800** | Budget: ≤2,200 (healthy polish margin) |

---

## Material & Color Specification

### Primary Structure (Industrial Metal)
- **Albedo:** Dark steel (RGB ~90, 90, 100)
- **Roughness:** ~0.75 (oxidized, weathered)
- **Metalness:** 0.4 (bare/treated steel)
- **Coverage:** ~60%
- **Purpose:** Main arm structure, salvage aesthetic

### Gripper/Fist Surfaces (High-Friction)
- **Albedo:** Darker steel (RGB ~60, 60, 70)
- **Roughness:** ~0.85 (matte, friction-optimized)
- **Metalness:** 0.3 (bare metal, grip texture)
- **Coverage:** ~20%
- **Purpose:** Gripping surfaces, menacing dark appearance

### Hydraulic Accent (Fluid Lines)
- **Albedo:** Orange or cyan (RGB ~240, 140, 60 or ~100, 220, 255)
- **Roughness:** ~0.4 (smooth, shiny tubing)
- **Metalness:** 0.6 (copper/aluminum tubing)
- **Coverage:** ~10%
- **Purpose:** Highlights functional system, suggests power/energy flow

### Rivets & Bolts (Metallic Detail)
- **Albedo:** Bright steel (RGB ~180, 180, 190)
- **Roughness:** ~0.5 (polished heads, worn shafts)
- **Metalness:** 0.8 (actual metal fasteners)
- **Coverage:** ~5%
- **Purpose:** Shows construction/assembly, industrial detail

### Weathering & Scorch (Patina)
- **Albedo:** Brown-black (RGB ~40, 35, 30)
- **Roughness:** ~0.9 (heavily corroded, matte)
- **Metalness:** 0.1 (oxide layer)
- **Coverage:** ~5%
- **Purpose:** Battle wear, salvage history

---

## Interaction Model (First-Person POV)

In gameplay, the arm appears in lower-right corner of screen:

- **Idle pose:** Resting at side, ready stance
- **Interaction pose:** Gripper extends toward object (Godot animation)
- **Combat pose:** Fist clenches or gripper opens in threat display
- **Impact feedback:** Visual squash/stretch on hit (Godot animation, not mesh change)

The mesh provides **static geometry** for idle state; Godot handles all animation and interaction feedback.

---

## Validation Checklist

**Before export (Blender):**
- [ ] Silhouette reads "salvage tool/weapon" (not fashionable, not cute)
- [ ] Cuff fits arm-mount proportions (wrist/forearm scale appropriate)
- [ ] Gripper or fist is menacing and functional-looking
- [ ] Hydraulic lines visible (system detail obvious)
- [ ] Weathering and patina present (salvage/worn appearance)
- [ ] Articulation hints visible (cuff joint, gripper/fist articulation)
- [ ] All material zones defined and named
- [ ] No overlapping faces or internal geometry
- [ ] Poly count ≤ 2,200

**After export to GLB:**
- [ ] File size reasonable (~2–3 MB)
- [ ] Godot imports without errors
- [ ] Arm silhouette matches brief (functional, industrial, menacing)
- [ ] Hydraulic accent color and metallic details visible

**In first-person gameplay:**
- [ ] Arm visible and readable in lower-right corner
- [ ] Cuff proportions fit arm-mount (not oversized, not tiny)
- [ ] Gripper/fist is clearly functional and threatening
- [ ] Hydraulic lines and detail visible (not mushy or undefined)
- [ ] Scale feels empowering to player (suggests genuine capability)

**Visual readability:**
- [ ] Arm does not read as cosmetic or decorative
- [ ] Industrial/salvage aesthetic is clear
- [ ] Mechanical systems obvious (hydraulics, articulation)
- [ ] Weathering suggests combat history and reliability
- [ ] Material palette coherent (dark metal, colored hydraulics, worn details)

---

## Success Criteria

✅ **Mechanical Arm is `ASSET_IMPLEMENTATION_PASS` when:**

1. Asset exported as GLB with valid manifest entry
2. Silhouette reads **salvage wrist-mount exoskeleton** (functional, tool-like, menacing)
3. Wrist cuff clearly articulated and arm-mounted (fits player forearm scale)
4. Gripper or fist end-effector menacing and functional (suggests gripping or striking)
5. Hydraulic system obvious (lines, pistons, conduits visible)
6. Weathering and material patina evident (salvage/worn aesthetic)
7. Material palette coherent (dark steel, hydraulic accent, rivet detail, weathering)
8. Poly count ≤ 2,200
9. No visual corruption, texture stretching, or floating geometry
10. Hydraulic accent color visible (orange or cyan tubing, shiny against matte body)
11. Turntable PNG captured showing gripper/fist and side-mount arrangement (2–3 angles)
12. SHA256 hash recorded in asset manifest
13. **Readability test passed:** In FPS POV, arm readable and menacing (not mushy, not oversized)

❌ **Mechanical Arm will be `NOT_GO` if:**

- Silhouette reads cosmetic, fashionable, or sleek (breaks salvage identity)
- Cuff proportions wrong for arm-mount (too large, too small, awkward angle)
- Gripper/fist ambiguous or non-threatening (weakens equipment identity)
- Hydraulic system insufficient or invisible (breaks functional language)
- Poly count exceeds 2,400
- Weathering absent or minimal (looks new/pristine, not salvaged)
- Material palette incoherent or contradicts industrial aesthetic
- First-person arm readability poor (detail mushy, scale off, visibility issues)

---

## Integration Points (Avatar Assembly)

The arm will be composed as part of player avatar in Phase 7:

- **Attachment:** Mounted on player's right wrist/forearm
- **Visibility:** Always visible in FPS POV (lower-right corner, melee interaction range)
- **Animation control:** Godot script drives arm animation (interaction, combat, idle)
- **Collision:** Gripper/fist collision handling (combat impacts, environmental grabs)
- **Audio:** Separate Godot layer (hydraulic hiss, mechanical click, impact sounds)
- **Material swapping:** Godot can tint or modify materials for damage feedback (optional)

---

## Deliverables

| Deliverable | Format | Location |
|---|---|---|
| Mechanical Arm Brief (this doc) | `.md` | `docs/art/briefs/brief-mechanical-arm-v1.md` |
| Blender Source | `.blend` | `assets/Source/Blender/Production/MVP_Pack_V1/TC_PLAYER_MechanicalArm_V1.blend` |
| GLB Export | `.glb` | `assets/Production/Generated/MVP_Pack_V1/TC_PLAYER_MechanicalArm_V1.glb` |
| Manifest Entry | JSON | `assets/Production/Generated/asset_manifest.json` |
| Review PNG (2–3 angles) | `.png` | `artifacts/asset-review/TC_PLAYER_MechanicalArm_V1/` |

---

## Approval Gate

**Brief Status:** `ASSET_BRIEF_READY`  
**Next Gate:** `ASSET_IMPLEMENTATION_PASS` (after Blender refinement/export and FPS POV readability test)  
**Final Gate:** `VISUAL_SLICE_GAMEPLAY_SAFE` (after avatar assembly and interaction animation integration in Phase 7)  
**Authority:** Project Director + FPS POV readability test + Equipment functionality/threat language review
