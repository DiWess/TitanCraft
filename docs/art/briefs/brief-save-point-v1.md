# Asset Brief: Save Point V1
## Phase 3 Interactable — Checkpoint / Progress Anchor

**Brief Owner:** TitanCraft Project Director  
**Target Asset:** Salvage checkpoint marker with active indicator  
**Scope:** Single interactive prop for save/respawn location  
**Date:** 2026-07-05  
**Design Philosophy:** Minimalist salvage beacon, distinct from other interactables  

---

## Visual Thesis

The save point is a **progress anchor and respawn location**. It must read as:

- **Checkpoint marker** — player returns here if defeated
- **Minimal form** — small, unobtrusive (not larger than player)
- **Active indicator** — cyan emissive glow signals "save active here"
- **Distinct from workbench/beacon** — different shape, cyan accent (not orange/purple)
- **Safe/sanctuary feeling** — not hostile, calm visual presence
- **Distance-readable** — identifiable from ~10 meters as "save here"

The save point must **signal "return point"** without text or UI. Cyan glow differentiates it from orange workbench (interactive) and purple beacon (victory objective).

---

## Scope Boundaries

### What This Brief Covers

- **Core structure:** Hexagonal or cylindrical pillar, vertical emphasis
- **Emissive indicator:** Cyan glowing ring or band around structure
- **Base mounting:** Small stable footprint, salvage-derived appearance
- **Minimal articulation:** No moving parts (save state is instant, no animation needed)
- **Color language:** Dark steel base, cyan accent (power/energy indicator)
- **Functional simplicity:** Player approaches, overlaps collision, save triggered by Godot

### What This Brief Does NOT Cover

- Animation or state transition (save happens instantly, no mesh change)
- UI elements or text (pure visual interaction, no labels)
- Sound (audio layer separate from mesh)
- Interior detail or exploration
- Multiple save states or checkpoints (MVP = single save point)

### What Remains Placeholder/Simple

- Pillar interior structure (suggest, not detailed)
- Cyan light source (emissive material, not Godot light object)
- Base circuitry/components (minimal suggestion)

---

## Design Reference: Silhouette

### Silhouette Priorities

**Avoid:**
- ❌ Confusing with save inventory (different from workbench, beacon)
- ❌ Overly decorative (suggests monument, not utility)
- ❌ Too tall or large (smaller scale, approachable)
- ❌ Ambiguous purpose (must read as "checkpoint" quickly)

**Target:**
- ✅ Geometric pillar form (hexagonal or cylinder, clean, minimal)
- ✅ Vertical emphasis (stands upright, draws eye, distinct proportions)
- ✅ Cyan glowing ring or band (energy indicator, inviting)
- ✅ Compact footprint (~0.85m × 0.85m)
- ✅ Modest height (~1.6m, taller than player torso, not overwhelming)
- ✅ Clean surface detail (fewer rivets/weathering than workbench, less grand than beacon)

### Scale & Proportion

| Aspect | Target | Rationale |
|--------|--------|-----------|
| **Width** | ~0.85m | Small, personal scale (player-sized interaction zone) |
| **Height** | ~1.6m | Taller than seated player, shorter than beacon |
| **Depth** | ~0.85m | Matches width (compact, not sprawling) |
| **Glow ring height** | ~0.3m | Visible, not dominating |
| **Visibility range** | 10+ meters | Medium distance, not distant objective |

### Poly Count & Complexity

| Component | Poly Budget | Detail Level |
|-----------|-------------|--------------|
| **Pillar structure** | ~600 | Low–Medium: clean geometry, minimal seams |
| **Base platform** | ~200 | Low: simple foundation, mounting feet |
| **Glowing ring/band** | ~200 | Low: geometric ring or torus |
| **Surface detail (optional)** | ~200 | Low: subtle ribbing, panel lines |
| **Emissive elements** | ~100 | Low: glow ring geometry |
| **TOTAL** | **~1,300** | Budget: ≤1,600 (generous margin) |

---

## Structural Breakdown

### 1. Pillar Form

**Shape Options (Choose One):**

**Option A: Hexagonal Pillar (Recommended)**
- Six-sided vertical prism
- Clean, geometric, readable from distance
- Faces can suggest internal hexagonal geometry
- Height: ~1.4m (excluding base and glow ring)

**Option B: Cylinder**
- Smooth, minimal, "purified" beacon feel
- Slightly less distinctive than hexagonal
- Height: ~1.4m

**Surface Treatment:**
- Clean, minimal detailing (not weathered like workbench)
- Faint vertical ribbing or panel lines (suggest internal structure)
- Subtle beveled edges (show craftsmanship, salvage origin)
- No aggressive welds or rust (signals "maintained, safe")

**Material:**
- Primary: Dark tech-gray steel (RGB ~100, 110, 120)
- Accent: Cyan ring/band (emissive, below)

### 2. Base Platform

- Small hexagonal or circular pad (~0.6m diameter)
- Simple mounting structure
- Dark steel, matching pillar
- Slight weathering/worn appearance (salvage origin)
- Minimal detail (supports pillar, nothing decorative)

### 3. Glowing Ring / Band

**Form:**
- Cyan emissive ring or horizontal band around pillar
- Positioned ~0.6m above base (middle-height, visible)
- Height: ~0.3m (distinct, not overwhelming)
- Shape: Circular torus or band (matches pillar contours)

**Material:**
- **Albedo:** Cyan paint (RGB ~100, 200, 220)
- **Emissive Strength:** 1.0–1.5 (moderate glow, visible in daylight and night)
- **Roughness:** ~0.3 (smooth, active indicator appearance)
- **Metalness:** 0 (painted/powder-coated, not reflective metal)
- **Purpose:** Signals "save checkpoint active here" without text

---

## Material & Color Specification

### Pillar Base (Dark Industrial)
- **Albedo:** Dark tech-gray (RGB ~100, 110, 120)
- **Roughness:** ~0.7 (matte, industrial, worn)
- **Metalness:** 0.2 (bare/treated steel, slight metal response)
- **Coverage:** ~85% (primary structure)

### Cyan Indicator (Emissive)
- **Albedo:** Bright cyan (RGB ~120, 220, 255)
- **Emissive Strength:** 1.2 (moderate glow, readable in daylight)
- **Roughness:** ~0.3 (smooth, high-tech feel)
- **Metalness:** 0 (glowing paint, not reflective)
- **Coverage:** ~15% (ring/band around middle)
- **Purpose:** Differentiates from orange (workbench) and purple (beacon); signals safe/neutral

### Optional Shadow/Accent (Very Dark)
- **Albedo:** Very dark gray (RGB ~50, 50, 60)
- **Purpose:** Shadow lines, edge definition
- **Coverage:** ~5% (subtle, not dominant)

---

## Interaction Flow (Godot Integration)

Save point activation is **instant, non-visual**. Mesh is static:

**Player approaches:**
1. Enters collision trigger zone (~2m radius around base)
2. Godot registers proximity (no mesh change)
3. Player can manually save (input key, e.g., F or E) or auto-save (on enter)
4. Save system writes data (Godot logic, not visual)
5. Player sees save confirmation in UI only (not mesh feedback)

**Respawn (if defeated):**
1. Player respawns at save point location
2. Mesh presence confirms "this is the respawn point"
3. No animation or state change needed

**Implication:** The save point is purely visual marker + collision trigger. All interaction logic lives in Godot, not the mesh.

---

## Validation Checklist

**Before export (Blender):**
- [ ] Pillar reads "checkpoint" (geometric, minimal, distinct)
- [ ] Cyan ring/band is obvious and emissive-ready
- [ ] Shape is clean and readable (hexagonal or cylinder)
- [ ] Base is stable and grounded (no floating elements)
- [ ] Height is appropriate (1.6m range)
- [ ] Material zones defined and named
- [ ] No overlapping faces or internal geometry
- [ ] UV unwrapped efficiently (minimal, can be simple)

**After export to GLB:**
- [ ] File size reasonable (~2–4 MB)
- [ ] Godot imports without errors
- [ ] Cyan material emissive strength exports correctly
- [ ] Silhouette matches brief (geometric, minimal, cyan glow)

**In first-person gameplay (10+ meters away):**
- [ ] Player can identify as "save point" from distance
- [ ] Cyan glow is visible and distinct
- [ ] Silhouette is clearly different from workbench/beacon
- [ ] Scale feels right (personal, approachable)

**Visual readability:**
- [ ] Save point does not confuse with workbench or beacon
- [ ] Cyan accent is clear and calming (not aggressive)
- [ ] Form suggests "checkpoint" or "anchor" semantically
- [ ] Emissive material displays correctly

---

## Success Criteria

✅ **Save Point is `ASSET_IMPLEMENTATION_PASS` when:**

1. Asset exported as GLB with valid manifest entry
2. Silhouette reads **geometric checkpoint marker** (minimal, clean)
3. Cyan glowing ring is visible and distinct
4. Pillar shape is readily identifiable (hexagonal or cylinder)
5. Material palette coherent (dark tech-gray base, cyan accent)
6. Poly count ≤ 1,600 total
7. No visual corruption, texture stretching, or floating geometry
8. Cyan emissive material exports and displays correctly
9. Turntable PNG captured from player approach angle (3–4 angles)
10. SHA256 hash recorded in asset manifest
11. **Visibility test passed:** Identifiable as checkpoint from 10+ meters

❌ **Save Point will be `NOT_GO` if:**

- Silhouette confuses with workbench or beacon
- Cyan accent is insufficient or not visually emissive
- Poly count exceeds 1,800
- Pillar form is ambiguous or overly decorative
- Material palette is incoherent or contradicts cyan safety language
- Player cannot distinguish from other interactive objects at distance

---

## Integration Points (Phase 7 Composition)

The save point will be placed in Phase 7 after terrain/hull are composed. Placement guidelines:

- **Location:** Safe zone near workbench or resource clusters, away from arena
- **Orientation:** Vertical (Z-axis up), stable on terrain
- **Collision:** Static, non-passable (collision trigger is larger than visible mesh)
- **Visual weight:** Minimal (should not dominate, is support structure, not focal point)
- **Line of sight:** Accessible without traversing dangerous terrain

---

## Deliverables

| Deliverable | Format | Location |
|---|---|---|
| Asset Brief (this doc) | `.md` | `docs/art/briefs/brief-save-point-v1.md` |
| Blender Source | `.blend` | `assets/Source/Blender/Production/MVP_Pack_V1/TC_PROP_SavePoint_V1.blend` |
| GLB Export | `.glb` | `assets/Production/Generated/MVP_Pack_V1/TC_PROP_SavePoint_V1.glb` |
| Manifest Entry | JSON | `assets/Production/Generated/asset_manifest.json` |
| Review PNGs (3 angles) | `.png` | `artifacts/asset-review/TC_PROP_SavePoint_V1/` |

---

## Approval Gate

**Brief Status:** `ASSET_BRIEF_READY`  
**Next Gate:** `ASSET_IMPLEMENTATION_PASS` (after Blender refinement/export and visibility test)  
**Final Gate:** `VISUAL_SLICE_GAMEPLAY_SAFE` (after composition integration in Phase 7)  
**Authority:** Project Director + Gameplay distance visibility test + Visual distinction review
