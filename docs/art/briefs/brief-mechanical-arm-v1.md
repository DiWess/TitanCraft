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

### 1. Elbow Joint Assembly

- Dark metal base joint (stationary attachment to player arm)
- Hinged articulation point (allows forearm rotation)
- Clean cylindrical form, minimal detail
- ~0.1m length, substantial diameter

**Poly allocation:** ~100 polys (simple joint)

### 2. Segmented Forearm (3 Segments)

- Three cylindrical segments in alternating beige and steel
- Each segment ~0.15–0.25m long, tapered diameter
- Segments decrease in diameter toward hand (tapering form)
- Clean geometric cylinders, smooth finish
- No internal detail, structural simplicity

**Poly allocation:** ~250 polys (three segments, geometric precision)

### 3. Energy Seams (Between Segments)

- Purple glowing band between each segment
- Thin cylindrical glowing element (~0.02m tall)
- Suggests power flow through arm structure
- High emissive strength (5.0) for visible energy manifestation

**Poly allocation:** ~100 polys (three seams, glowing geometry)

### 4. Hand Assembly

**Wrist Joint:**
- Dark metal cylindrical base (rotation point)
- ~0.1m length, proportional to segments
- Clean geometry

**Palm & Fingers:**
- Broad palm platform (dark metal, ~0.2m wide)
- Three fingers extending from palm (geometric boxes, menacing)
- Single thumb (angled, off to side)
- All dark metal, armored appearance
- Articulation hints but no full joints

**Knuckle Glow:**
- Purple emissive line across knuckle ridge
- Suggests concentrated energy at impact point
- High threat indicator

**Poly allocation:** ~400 polys (palm, three fingers, thumb, glow)

### 5. Accent & Detail

**Orange Stripe:**
- Single orange paint stripe on upper forearm
- ~0.05m × 0.16m × ~0.05m placement
- Functional accent, suggests active system

**Top Plate:**
- Dark metal mounting plate above segments
- Stabilization element, slightly raised

**Poly allocation:** ~150 polys (accent stripe, top plate, rivet details)

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

### Forearm Segments (Alternating Beige/Steel)
**Segment A (Beige Hull Material):**
- **Albedo:** Off-white beige (RGB ~220, 200, 180)
- **Roughness:** ~0.62 (worn hull material)
- **Metalness:** 0.12 (salvage hull origin)
- **Coverage:** ~30% (alternating segments)

**Segment B (Mid-Tone Metal):**
- **Albedo:** Medium steel (RGB ~180, 190, 205)
- **Roughness:** ~0.45 (polished metal)
- **Metalness:** 0.75 (bare metal)
- **Coverage:** ~25% (alternating segments)

### Energy Seams (Purple Glowing)
- **Albedo:** Very dark purple (RGB ~10, 0, 30)
- **Emissive:** Purple glow (RGB ~220, 80, 255, emissive 5.0)
- **Roughness:** ~0.3 (energy flow appearance)
- **Metalness:** 0 (pure energy glow)
- **Coverage:** ~5% (seams between segments)
- **Purpose:** Suggests powered energy flow through arm seams

### Wrist/Palm/Fingers (Dark Metal)
- **Albedo:** Very dark gray (RGB ~30, 32, 40)
- **Roughness:** ~0.48 (matte, worn)
- **Metalness:** 0.65 (treated metal)
- **Coverage:** ~20% (grip surfaces, finger detail)

### Accent Stripe (Orange Paint)
- **Albedo:** Orange paint (RGB ~240, 140, 60)
- **Roughness:** ~0.42 (painted finish)
- **Metalness:** 0
- **Coverage:** ~5% (accent stripe on upper arm)
- **Purpose:** Functional highlight, suggests active system

### Knuckle Glow (Purple Energy Accent)
- **Albedo:** Very dark purple (RGB ~10, 0, 30)
- **Emissive:** Purple glow (RGB ~220, 80, 255, emissive 3.0)
- **Roughness:** ~0.3 (energy manifestation)
- **Coverage:** ~3% (fist knuckle line)
- **Purpose:** Threat indicator at impact point

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
