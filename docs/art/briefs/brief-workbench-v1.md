# Asset Brief: Workbench V1
## Phase 3 Interactable — Crafting Interaction Hub

**Brief Owner:** TitanCraft Project Director  
**Target Asset:** Functional crafting station with articulated robotic arm and holographic interface  
**Scope:** Single interactive prop for resource-to-equipment conversion  
**Date:** 2026-07-05  
**Design Philosophy:** Industrial salvage workbench meets functional arcade aesthetic  

---

## Visual Thesis

The workbench is the **player's primary hub for crafting**. It must read as:

- **Functional (not decorative)** — every element suggests purpose
- **Salvage-derived** — built from crash debris and repurposed materials
- **Interactive from distance** — player can identify it as "craft here" from 10+ meters away
- **Orange-dominant color language** — differentiates from environment and signals functionality
- **Armature-driven** — articulated robot arm suggests assembly capability
- **Holographic interface** — tilted orange panel suggests active control surface

The workbench must **signal "interact here"** without text or UI, relying purely on silhouette and material accent.

---

## Scope Boundaries

### What This Brief Covers

- **Bench structure:** Sturdy base platform, repurposed hull materials, industrial framing
- **Articulated arm:** 3–4 segments, capable of assembly motion, dark metal construction
- **Holographic interface panel:** Tilted ~45°, orange emissive glow, suggests data/control
- **Color language:** Beige/off-white hull base, dark steel structural elements, orange accents (arm joints, panel surround, control highlights)
- **Functional accents:** Visible resource slots (where materials are placed), assembly area (where items are built)

### What This Brief Does NOT Cover

- Full interior detail (if opened, interior can be simple/dark)
- Animated rigging (arm can be posed; animation is Godot runtime, not mesh)
- UI elements or text (all interaction is visual/spatial, not written)
- Sound-producing components (audio is separate Godot layer)
- Multiple crafting recipes display (only one recipe per MVP)

### What Remains Placeholder/Simple

- Arm interior hydraulics (suggest with simple geometry, not detailed)
- Panel data display (blank orange glow; no screens/readouts)
- Electrical connections (suggest with dark conduits, not fully traced)

---

## Design Reference: Silhouette

### Silhouette Priorities

**Avoid:**
- ❌ Sleek, futuristic appearance (suggests high-tech, not salvage)
- ❌ Perfectly clean/symmetrical (looks manufactured, not repurposed)
- ❌ Robot humanoid form (no AI companions; arm is tool, not being)
- ❌ Overly complex detailing that reads as "decorative"

**Target:**
- ✅ Industrial workbench aesthetic (salvage/heavy/functional)
- ✅ Single articulated arm (suggest 3–4 segment chain)
- ✅ Tilted orange panel (45° angle, emissive glow)
- ✅ Heavy base (clearly stable, weighted, immobile)
- ✅ Rough texture/panel lines (worn materials, welded joints, salvage appearance)
- ✅ Clear material zones (beige body, dark arm, orange accents)

### Scale & Proportion

| Aspect | Target | Rationale |
|--------|--------|-----------|
| **Width** | ~3.3m | Larger than player (1.8m), signifies importance |
| **Depth** | ~1.1m | Modest footprint; doesn't block navigation |
| **Height** | ~1.7m | Arm at work height (1.2–1.5m), panel at eye level when approached |
| **Arm reach** | ~1.5m | Can assemble objects on central platform |
| **Panel tilt** | ~45° | Inviting angle, readable from player approach angle |

### Poly Count & Complexity

| Component | Poly Budget | Detail Level |
|-----------|-------------|--------------|
| **Bench base/platform** | ~800 | Medium: visible panel lines, rivets, welded seams |
| **Articulated arm** | ~1,200 | Medium: segment joints, actuator hints, 3–4 bones |
| **Holographic panel** | ~600 | Low: simple flat plane with orange emissive material |
| **Functional detail** | ~200 | Low: resource slots, assembly area markers |
| **Material accents** | ~300 | Low: orange trim, connecting brackets, fasteners |
| **TOTAL** | **~3,100** | Target: ≤3,500 to leave detail margin |

---

## Structural Breakdown

### 1. Base Platform (Bench Body)

- Rectangular boxy structure (salvage hull origin)
- Visible frame/ribbing on sides (structural, not decorative)
- Heavy footprint; no wheels or moving base (immobile anchor)
- Off-white/beige primary material (hull origin)
- Dark steel secondary material for internal framing
- Slight asymmetry (worn appearance, impact history visible)

### 2. Articulated Arm Assembly

**Arm Structure:**
- 3–4 segments, each narrowing toward end (tapering)
- Heavy at base (motor/actuator), lighter at end (precision end-effector)
- Joints visible (swivels, rotations, bends)
- Dark steel construction (functional, not ornamental)

**Segment Detailing:**
- Each segment: ~30cm diameter pipes/tubes
- Visible hydraulic lines or conduits connecting segments
- Slight surface detail (bolts, seams, wear marks)
- End tool: simple grasper or pad (not anthropomorphic hand)

**Positioning:**
- Mounted on left or right rear of bench
- Rotates downward to work surface
- Can reach center-to-front of bench area (~1.5m reach)
- Suggest motion without animation (posed bent position)

### 3. Holographic Interface Panel

**Form:**
- Flat rectangular plane (~1.2m wide, ~0.8m tall)
- Tilted ~45° from vertical
- Framed by dark metal surround (structural bracket, not ornamental)
- Origin at work surface level, readable from standing player (1.7m eye height)

**Material:**
- Primary: Orange emissive material (glow visible in dark)
- Surround: Dark steel bracket/mounting
- Suggest data without detail (blank glowing panel, not screen-like)

**Optional Detail:**
- Minimal indicator lights (small orange/cyan points at edges)
- Suggest interfaces with no texture/text

### 4. Functional Work Surface

- Flat top platform (where materials are placed, where items are assembled)
- Slightly recessed or marked area (visual container for assembly)
- May include simple holders or guides (low-poly structures)
- Resource slots: two or three small indents/trays for incoming materials

---

## Material & Color Specification

### Primary Hull (Off-White Worn)
- **Albedo:** Beige worn paint (RGB ~210, 200, 190)
- **Roughness:** ~0.75 (weathered, microabrasion)
- **Metalness:** 0 (painted surface)
- **Coverage:** ~50% of visible surfaces (bench body)

### Structural Steel (Dark Gray)
- **Albedo:** Structural steel (RGB ~90, 90, 100)
- **Roughness:** ~0.7 (oxide, worn)
- **Metalness:** 0.3 (bare steel)
- **Coverage:** ~30% (framing, arm, support brackets)

### Orange Accent (Functional Highlight)
- **Albedo:** Bright orange paint (RGB ~240, 140, 60)
- **Roughness:** ~0.6 (painted, some gloss)
- **Metalness:** 0
- **Emissive Strength:** 0.8+ (glows in ambient light)
- **Coverage:** ~15% (panel, arm joints, control highlights)
- **Purpose:** Signals "interactive" without text; matches beacon cyan language

### Orange Holographic (Emissive)
- **Albedo:** Bright orange (RGB ~255, 160, 80)
- **Emissive Strength:** 2.0+ (strong glow, visible even in daylight)
- **Roughness:** ~0.3 (smooth, glass-like, active display feel)
- **Metalness:** 0
- **Coverage:** Panel surface (~1 sq meter visible)

---

## Validation Checklist

**Before export (Blender):**
- [ ] Silhouette reads "workbench, not sculpture" (functional, not art)
- [ ] Arm is clearly articulated (3–4 visible segments)
- [ ] Orange panel is obvious and tilted (~45°)
- [ ] Base is solid and heavy (no floating elements)
- [ ] All material zones defined and named
- [ ] No overlapping faces or internal geometry
- [ ] UV unwrapped efficiently

**After export to GLB:**
- [ ] File size reasonable (~5–10 MB)
- [ ] Godot imports without errors
- [ ] Orange material glows (emissive exported correctly)
- [ ] Silhouette matches brief (functional, industrial)

**In first-person gameplay (10+ meters away):**
- [ ] Player can identify as "craft here" from distance
- [ ] Orange panel is visible and distinct
- [ ] Arm silhouette is readable (not mushy or confusing)
- [ ] Scale feels right (larger than player, approachable)

**Visual readability:**
- [ ] Workbench does not read as decoration or storage
- [ ] Orange accents signal "interactive" clearly
- [ ] Material contrast (beige body, dark arm, orange highlights) is clear
- [ ] No material zones are ambiguous or mismatched

---

## Success Criteria

✅ **Workbench is `ASSET_IMPLEMENTATION_PASS` when:**

1. Asset exported as GLB with valid manifest entry
2. Silhouette reads **industrial salvage workbench** (functional, heavy)
3. Articulated arm is clearly identifiable (3–4 segments)
4. Orange holographic panel is visible and emissive
5. Material palette coherent (beige hull, dark steel, orange functional accents)
6. Poly count ≤ 3,500 total
7. No visual corruption, texture stretching, or floating geometry
8. Orange emissive material exports and displays correctly
9. Turntable PNG captured from gameplay approach angle (hero 3–4 angles)
10. SHA256 hash recorded in asset manifest
11. **Visibility test passed:** Identifiable as crafting station from 10+ meters (first-person view)

❌ **Workbench will be `NOT_GO` if:**

- Silhouette reads decorative, sleek, or robot-like
- Arm is ambiguous or doesn't suggest assembly capability
- Orange accent is insufficient or not visually emissive
- Poly count exceeds 4,000
- Player cannot distinguish from other interactive objects at distance
- Material palette is incoherent or contradicts brief

---

## Integration Points (Phase 7 Composition)

The workbench will be placed in Phase 7 after terrain/hull are composed. Placement guidelines:

- **Location:** Central basin area, ~30m from spawn, near resource clusters
- **Orientation:** Face toward expected player approach angle (usually from spawn/resources direction)
- **Collision:** Static, non-passable (player walks around, not through)
- **Visual dominance:** Should be second or third largest silhouette in scene (after hull, before pickups)

---

## Deliverables

| Deliverable | Format | Location |
|---|---|---|
| Asset Brief (this doc) | `.md` | `docs/art/briefs/brief-workbench-v1.md` |
| Blender Source | `.blend` | `assets/Source/Blender/Production/MVP_Pack_V1/TC_PROP_Workbench_V1.blend` |
| GLB Export | `.glb` | `assets/Production/Generated/MVP_Pack_V1/TC_PROP_Workbench_V1.glb` |
| Manifest Entry | JSON | `assets/Production/Generated/asset_manifest.json` |
| Review PNGs (3 angles) | `.png` | `artifacts/asset-review/TC_PROP_Workbench_V1/` |

---

## Approval Gate

**Brief Status:** `ASSET_BRIEF_READY`  
**Next Gate:** `ASSET_IMPLEMENTATION_PASS` (after Blender refinement/export and gameplay visibility test)  
**Final Gate:** `VISUAL_SLICE_GAMEPLAY_SAFE` (after composition integration in Phase 7)  
**Authority:** Project Director + Gameplay distance visibility test + Visual readability review
