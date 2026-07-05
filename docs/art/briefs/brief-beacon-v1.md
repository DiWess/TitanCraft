# Asset Brief: Beacon V1 (Dormant & Active)
## Phase 3 Interactable — Final Objective, Rescue Signal

**Brief Owner:** TitanCraft Project Director  
**Target Asset:** Dual-state rescue beacon (dormant standby, active transmission)  
**Scope:** Single interactive prop with two distinct material/emission states  
**Date:** 2026-07-05  
**Design Philosophy:** Salvaged communication array, activated as final objective  

---

## Visual Thesis

The beacon is **the final victory objective**. It must read as:

- **Dormant state:** Closed, waiting, visually distinct but not active (red standby LED)
- **Active state:** Opened petals, emissive core, broadcasting signal (purple glowing crystal, suggests transmission)
- **Salvage-derived** — repurposed spacecraft communication hardware, not sci-fi artifact
- **Cyan/purple color language** — differs from orange workbench, signals "power/transmission"
- **Obelisk form** — tall, vertical, draws eye upward (final visual destination)
- **Distance-readable** — player can identify from 20+ meters as "destination beacon"

The beacon transition (dormant → active) is **driven by gameplay** (activation script in Godot, not mesh animation). The two states are **distinct static meshes or material swap**:
- **Dormant:** Petals closed, red LED glow
- **Active:** Petals opened (or mesh swap), purple crystal emissive core

---

## Scope Boundaries

### What This Brief Covers

**Dormant State (`TC_PROP_Beacon_Dormant_V1`):**
- Four-petal obelisk structure (petals furled/closed)
- Sealed external shell (metallic, industrial look)
- Red standby LED indicator (small emissive point, low intensity)
- Dark metal base structure
- Compact silhouette (~1.4m × 1.4m × 2.2m)

**Active State (`TC_PROP_Beacon_Active_V1`):**
- Petals opened/unfurled (4-way radial opening)
- Exposed core: large emissive purple crystal (dominant visual)
- Crystal glows with purple light (high emissive strength)
- Base structure similar to dormant (dark metal mount)
- Larger apparent silhouette (~2.9m × 2.9m × 2.1m) due to petal opening
- Core crystal is the focus (player sees this as "success")

### What This Brief Does NOT Cover

- Transmission beam geometry (VFX rendered by Godot particle system, not mesh)
- Sound/audio (separate Godot audio layer)
- Animation transition (dormant → active mesh swap handled by Godot, not rigged animation in mesh)
- Interior detail (sealed or dark; not explorable)
- Multiple states beyond dormant/active (MVP locked to two states)

### What Remains Placeholder/Simple

- Petal mechanism detail (suggest with geometry, not fully mechanical)
- Crystal internal structure (glow is primary, internal detail minimal)
- Base circuitry/components (suggest, not detailed)
- Signal strength indicator (dormant red LED is enough)

---

## Design Reference: Silhouette

### Dormant State Silhouette

**Avoid:**
- ❌ Generic beacon/pillar (needs character, not just a cylinder)
- ❌ Too small (must be visible from 20+ meters)
- ❌ Overly ornate (distraction from final objective clarity)

**Target:**
- ✅ Four-petal closed form (suggests opening mechanism)
- ✅ Obelisk proportions (vertical emphasis, draws eye up)
- ✅ Clear red standby light (signals "waiting")
- ✅ Solid industrial base (stable, mounted)
- ✅ Sealed appearance (compact, secure, dormant)

### Active State Silhouette

**Must Transform Visually:**
- ✅ Petals unfurl/open (4-way radial, flowers upward or outward)
- ✅ Crystal core exposed and glowing (bright purple dominates)
- ✅ Larger overall profile (opening increases apparent size)
- ✅ Dynamic appearance (suggests power transmission, not inert)
- ✅ Crystal is focal point (largest emissive element)

### Scale & Proportion

| Aspect | Target | Rationale |
|--------|--------|-----------|
| **Dormant width** | ~1.4m | Compact sentinel form |
| **Dormant height** | ~2.2m | Taller than player, visible from distance |
| **Active width** | ~2.9m | Petals open ~2× footprint |
| **Active height** | ~2.1m | Slightly lower as petals radiate horizontally |
| **Visibility range** | 20+ meters | Last objective, visible from most basin positions |
| **Crystal size** | ~0.8m diameter | Dominates active state (core focal point) |

### Poly Count & Complexity

| Component | Poly Budget | Detail Level |
|-----------|-------------|--------------|
| **Dormant base structure** | ~800 | Medium: panel lines, sealed seams, mounting points |
| **Dormant petals (closed)** | ~600 | Medium: petal geometry, fold lines, edges |
| **Dormant LED indicator** | ~50 | Low: simple emissive point or small dome |
| **Active petals (unfurled)** | ~1,200 | Medium–High: opening geometry, articulation surfaces |
| **Crystal core (active)** | ~800 | Medium: faceted crystal structure, light-catching planes |
| **Shared base (both states)** | ~400 | Low: mounting feet, cable conduits, shared structure |
| **TOTAL Dormant** | **~1,850** | Budget: ≤2,000 |
| **TOTAL Active** | **~2,450** | Budget: ≤2,800 |

---

## Structural Breakdown

### 1. Base Structure (Shared, Both States)

- Rectangular or hexagonal mount pad (~1.5m footprint)
- Heavy dark steel construction (grounded, immobile)
- Visible mounting bolts, weld seams, structural ribs
- Slight asymmetry (worn, salvaged appearance)
- Cable conduits emerging from base (suggest power input)

### 2. Dormant State: Closed Obelisk

**Petal Assembly (Closed/Furled):**
- Four petals arranged radially around vertical axis
- Each petal: flat or slightly curved plane
- Petals sealed/closed vertically (touch or nearly touch at edges)
- Smooth exterior surface (sealed aerospace composite appearance)
- Faint seam lines where petals meet (suggests opening mechanism)

**Internal Chamber (Not Visible, Dormant):**
- Sealed behind petals
- Red LED indicator on one face (small glow, ~5cm diameter, dim)
- Suggest power-down state with minimal light

**Material:**
- Primary: Dark metallic hull (RGB ~80, 80, 90, roughness 0.6)
- Accent: Dark cable/structural elements
- LED: Red emissive (RGB ~200, 50, 50, emissive strength 0.5, dim in dormant state)

### 3. Active State: Opened Petals & Crystal Core

**Petal Assembly (Opened/Unfurled):**
- Four petals rotate/unfold outward and/or upward
- Each petal: exposes interior hinge points
- Petals spread ~90° or more (radial opening, 360° total spread ~= 4 × 45°)
- Interior petal surfaces visible (reveal darker inner faces with tech detail)
- Hinge points/articulation visible (suggest mechanical opening)

**Crystal Core (Revealed, Active):**
- Large faceted crystal (~0.6–0.8m diameter)
- Positioned at center, elevated above base
- Purple emissive material (bright, high-intensity glow)
- **Emissive Strength:** 2.0–3.0 (intense, visible even in daylight)
- Crystal shape: geometric, multi-faceted (not organic, suggests technology)
- Possible internal structure hints (darker planes inside, light refraction)

**Halo/Glow Effect (Mesh-based):**
- Optional: subtle secondary geometry around crystal (slight halo planes) to enhance glow
- Or: rely on material emissive strength for glow without additional mesh

**Material:**
- Crystal: Purple emissive (RGB ~180, 80, 255, emissive strength 2.5+)
- Petals inner surface: Dark steel/tech material (RGB ~100, 100, 110)
- Petal edges/articulations: Orange accent highlights (RGB ~240, 140, 60, emissive 0.8) — signals power flow
- Base: Same as dormant

---

## Material & Color Specification

### Shared (Both States)

**Base Structure Metal (Dark Industrial)**
- **Albedo:** Dark steel (RGB ~80, 80, 90)
- **Roughness:** ~0.65 (oxide surface, salvage appearance)
- **Metalness:** 0.3 (bare/treated metal)
- **Coverage:** ~40% (mounting, structure)

**Cable/Conduit (Very Dark)**
- **Albedo:** Very dark gray (RGB ~40, 40, 50)
- **Roughness:** ~0.7 (rubber insulation, metal core)
- **Metalness:** 0.1 (copper/aluminum beneath insulation)
- **Coverage:** ~10%

### Dormant State

**Sealed Hull (Industrial Composite)**
- **Albedo:** Dark gray (RGB ~100, 100, 110)
- **Roughness:** ~0.7 (matte industrial finish)
- **Metalness:** 0 (composite, not metal)
- **Coverage:** ~50% (outer petal surfaces)

**Dormant Red Indicator**
- **Albedo:** Red (RGB ~200, 50, 50)
- **Emissive Strength:** 0.5 (dim standby glow, just visible)
- **Roughness:** ~0.3 (lens-like, slightly reflective)
- **Size:** ~5cm diameter dome or point light
- **Purpose:** Signals "waiting" without overwhelming presence

### Active State

**Crystal Core (Emissive Purple)**
- **Albedo:** Bright purple (RGB ~200, 100, 255)
- **Emissive Strength:** 2.5–3.0 (dominates visual, strong glow)
- **Roughness:** ~0.2 (smooth, faceted, light-catching)
- **Metalness:** 0 (crystal, not metal)
- **Coverage:** ~20% (focal point)
- **Purpose:** Victory signal; draws eye, signals objective achieved

**Petal Inner Surface (Tech Detail)**
- **Albedo:** Slightly lighter than base (RGB ~120, 120, 130)
- **Roughness:** ~0.65 (brushed metal/tech surface)
- **Metalness:** 0.2
- **Coverage:** ~20% (revealed when petals open)

**Petal Edges/Hinge Accents (Orange Power)**
- **Albedo:** Orange paint (RGB ~240, 140, 60)
- **Emissive Strength:** 0.8–1.0 (glows, suggests power activation)
- **Roughness:** ~0.5 (some gloss, active state highlight)
- **Coverage:** ~10% (articulation points, edge highlights)
- **Purpose:** Signals energy activation, matches interactive accent language

---

## State Transition (Godot Integration)

This brief covers **static mesh geometry only**. The state transition is **handled by Godot script**:

**Dormant → Active transition:**
1. Player activates beacon (collision trigger, Godot script)
2. Godot instantiates active mesh in place of dormant mesh
3. Godot plays particle effect (optional VFX for transmission beam)
4. Godot plays audio cue (activation sound, separate layer)
5. Victory condition triggered (mission complete)

**Mesh requirement:** Two separate GLB files (dormant + active) or single mesh with material swaps (simpler approach: material emissive strength change + petal visibility state).

**Recommended approach:** Two distinct meshes (dormant closed, active opened), loaded as separate assets. Simplifies Godot state management.

---

## Validation Checklist

**Before export (Blender):**
- [ ] Dormant state reads "sealed, waiting beacon" (closed form, clear)
- [ ] Active state reads "power on, transmission ready" (opened, emissive core)
- [ ] Petals are distinct geometry (not just color change)
- [ ] Crystal core is focal point in active state
- [ ] Red LED visible on dormant state
- [ ] Purple crystal emissive setup correct (material exported with emissive strength)
- [ ] Orange petal accents consistent with workbench orange language
- [ ] All material zones defined and named
- [ ] No overlapping faces or internal geometry
- [ ] Collision hull prepared (if needed)

**After export to GLB:**
- [ ] Dormant file: ~3–5 MB
- [ ] Active file: ~5–8 MB
- [ ] Godot imports without errors
- [ ] Purple emissive material glows correctly
- [ ] Red LED visible on dormant state
- [ ] Both states' silhouettes are distinct and readable

**In first-person gameplay (20+ meters away):**
- [ ] Dormant beacon is visible and readable (awaiting activation)
- [ ] Active beacon crystal glow is obvious (victory signal)
- [ ] Beacon stands out from terrain/hull (distinct silhouette)
- [ ] Crystal is focal point (player knows "success = crystal glow")

**Visual readability:**
- [ ] Dormant state does not confuse with other props
- [ ] Active state transformation is clear (petals open, crystal glows)
- [ ] Purple color differentiates from orange workbench (interactive accent language)
- [ ] Emissive materials export and display correctly in Godot

---

## Success Criteria

✅ **Beacon (Dormant + Active) is `ASSET_IMPLEMENTATION_PASS` when:**

1. Both states exported as GLB with valid manifest entries
2. **Dormant** silhouette reads "sealed beacon, awaiting activation" (closed, clear)
3. **Active** silhouette reads "activated transmission beacon" (opened, glowing core)
4. Purple crystal is dominant focal point in active state
5. Red standby LED visible in dormant state
6. Petal opening mechanism is geometrically obvious (not just color)
7. Material palette coherent (dark base, purple crystal, orange accent, red LED)
8. Poly count ≤ 2,000 (dormant), ≤ 2,800 (active)
9. No visual corruption, texture stretching, or floating geometry
10. Emissive materials export and display correctly
11. Turntable PNGs captured for both states (hero 3–4 angles each)
12. SHA256 hashes recorded in asset manifest for both states
13. **Visibility test passed:** Identifiable from 20+ meters (first-person view)

❌ **Beacon will be `NOT_GO` if:**

- Dormant state reads active or confused with other props
- Active state crystal does not glow or is insufficiently emissive
- Petal opening is ambiguous (just a color change, not geometric opening)
- Poly count exceeds 3,000 (dormant) or 3,500 (active)
- Purple and red materials do not export/display correctly
- Player cannot distinguish from other interactive objects at distance

---

## Integration Points (Phase 7 Composition)

The beacon will be placed in Phase 7 after terrain/hull are composed. Placement guidelines:

- **Location:** Elevated area, opposite far side of basin from hull (visual destination)
- **Orientation:** Vertical (Z-axis up), stable on terrain
- **Collision:** Static, non-passable at base (player can approach but not pass through pedestal)
- **Visual dominance:** Should be third or fourth largest silhouette in scene (after hull, workbench)
- **Line of sight:** Clear view from player spawn (first impression: "this is the goal")

---

## Deliverables

| Deliverable | Format | Location |
|---|---|---|
| Dormant Brief (this doc) | `.md` | `docs/art/briefs/brief-beacon-v1.md` |
| Dormant Source | `.blend` | `assets/Source/Blender/Production/MVP_Pack_V1/TC_PROP_Beacon_Dormant_V1.blend` |
| Dormant GLB | `.glb` | `assets/Production/Generated/MVP_Pack_V1/TC_PROP_Beacon_Dormant_V1.glb` |
| Active Source | `.blend` | `assets/Source/Blender/Production/MVP_Pack_V1/TC_PROP_Beacon_Active_V1.blend` |
| Active GLB | `.glb` | `assets/Production/Generated/MVP_Pack_V1/TC_PROP_Beacon_Active_V1.glb` |
| Manifest Entries | JSON | `assets/Production/Generated/asset_manifest.json` |
| Review PNGs (3 angles × 2 states) | `.png` | `artifacts/asset-review/TC_PROP_Beacon_Dormant_V1/`, `TC_PROP_Beacon_Active_V1/` |

---

## Approval Gate

**Brief Status:** `ASSET_BRIEF_READY`  
**Next Gate:** `ASSET_IMPLEMENTATION_PASS` (after Blender refinement/export and dual-state visibility test)  
**Final Gate:** `VISUAL_SLICE_GAMEPLAY_SAFE` (after composition integration in Phase 7)  
**Authority:** Project Director + Dual-state visibility test + Victory signaling clarity review
