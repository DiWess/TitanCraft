# Asset Brief: Pickups V1
## Phase 4 Resources — Collectable Resource Items

**Brief Owner:** TitanCraft Project Director  
**Target Assets:** Four distinct resource pickup types (Metal, Biomass, Electronics, Component)  
**Scope:** Individual interactive collectable props for crafting material supply  
**Date:** 2026-07-05  
**Design Philosophy:** Modular salvage components, instantly recognizable by silhouette and color accent

---

## Visual Thesis

Each pickup type is a **resource material collectible**. All must read as:

- **Distinct by type** — Metal/Biomass/Electronics/Component each have unique silhouettes
- **Immediately recognizable** — player identifies resource type from 5+ meters away
- **Lightweight/portable** — small enough to hold or carry (player-hand-scale)
- **Salvage-derived** — repurposed crash materials, not pristine manufactured goods
- **Color-coded** — visual language distinguishes resource type (gray metal, brown biomass, cyan electronics, orange component)
- **Interactive presence** — glint or subtle glow suggests "pickup here"

The pickups signal **"collectable resource available"** purely through form and color, without text or labels.

---

## Design Reference: Visual Hierarchy

### Metal (TC_PICKUP_Metal_V1)
**Purpose:** Industrial structural material (walls, frames, armor)
- **Silhouette:** Roughly cubic block, sharp edges, metallic sheen
- **Scale:** ~0.3m × 0.3m × 0.25m (hand-sized)
- **Color:** Bright silvery-gray (RGB ~200, 200, 200)
- **Accent:** None (pure metal, no glow)
- **Surface:** Smooth, machined appearance, slight reflection
- **Poly budget:** ~200 polys
- **Visibility:** 5–8m in sunlight

### Biomass (TC_PICKUP_Biomass_V1)
**Purpose:** Organic material (fuel, growth substrate, consumables)
- **Silhouette:** Faceted dark-red crystalline organic cluster with growth spikes
- **Scale:** ~0.35m × 0.3m × 0.4m (distinctive clustered form)
- **Color:** Dark burgundy-red (RGB ~150, 10, 16)
- **Accent:** Red emissive glow (subtle, emissive 0.5)
- **Surface:** Faceted crystal appearance with organic growth spikes
- **Poly budget:** ~250 polys (faceted geometry, spikes)
- **Visibility:** 5–8m, red glow aids detection

### Electronics (TC_PICKUP_Electronics_V1)
**Purpose:** Circuitry and controllers (logic, sensing, automation)
- **Silhouette:** Stacked dark crates/modules with orange lids and cyan indicators
- **Scale:** ~0.32m × 0.26m × 0.38m (composite modular stack)
- **Color:** Dark metal base (RGB ~45, 50, 58), orange lid accent, cyan LED indicators
- **Accent:** Orange paint on lids, cyan emissive on indicator strips (emissive 4.0)
- **Surface:** Boxy modular form, geometric lids, antenna protrusion
- **Poly budget:** ~300 polys
- **Visibility:** 8–10m (cyan glow aids distance visibility, distinctive stacked form)

### Component (TC_PICKUP_Component_V1)
**Purpose:** Alien artifact, mission objective material
- **Silhouette:** Purple crystalline cluster formation on dark rock substrate
- **Scale:** ~0.36m × 0.3m × 0.32m (cluster with satellite shards)
- **Color:** Purple crystal core (RGB ~220, 80, 60) with dark substrate
- **Accent:** Purple emissive glow on crystals (RGB ~220, 80, 255, emissive 1.4)
- **Surface:** Faceted crystal geometry, satellite shards, rock base
- **Poly budget:** ~280 polys
- **Visibility:** 7–10m (purple glow distinguishes from other types, alien aesthetic)

---

## Scope Boundaries

### What This Brief Covers

- **Four distinct asset meshes** (one per resource type)
- **Silhouettes** that differentiate by type at distance
- **Material zones** with color-coding and optional emissive accents (electronics/component)
- **Collision hull** (simple, no physics simulation)
- **No animation** (static pickups, collection handled by Godot)
- **Optional visual polish:** glint reflections, subtle rotation hints in Godot

### What This Brief Does NOT Cover

- Collection animation (pickup disappear handled by Godot)
- Inventory UI or item counters
- Sound (collection sfx is separate Godot layer)
- Particle effects (collect VFX rendered by Godot)
- Storage or drop mechanics
- Multiple size variants (MVP = one size per type)

### What Remains Placeholder/Simple

- Interior component detail (suggest mechanically, not fully detailed)
- Material variation within type (use one PBR material per type)
- Wear/damage markings (minimal, salvage appearance sufficient)

---

## Material & Color Specification

### Metal (TC_PICKUP_Metal_V1)

**Primary Material (Industrial Metal)**
- **Albedo:** Bright silvery-gray (RGB ~200, 200, 200)
- **Roughness:** ~0.5 (polished metal, moderate reflection)
- **Metalness:** 0.8 (bare steel/aluminum)
- **Coverage:** 100% (pure metal pickup)
- **Purpose:** Signals structural/industrial material, reflects light

### Biomass (TC_PICKUP_Biomass_V1)

**Primary Organic Crystal (Dark Red)**
- **Albedo:** Dark burgundy-red (RGB ~150, 10, 16)
- **Roughness:** ~0.32 (faceted crystal, some gloss)
- **Metalness:** 0 (organic, non-metallic)
- **Emissive:** Red glow (emissive strength 0.5)
- **Coverage:** ~80% (crystal cluster)

**Substrate (Dark Metal)**
- **Albedo:** Very dark metal (RGB ~45, 50, 58)
- **Roughness:** ~0.7 (matte, oxidized)
- **Metalness:** 0.65 (bare metal)
- **Coverage:** ~20% (rock base)

### Electronics (TC_PICKUP_Electronics_V1)

**Dark Crate Substrate (Industrial Dark)**
- **Albedo:** Very dark metal (RGB ~45, 50, 58)
- **Roughness:** ~0.48 (matte)
- **Metalness:** 0.65 (bare metal)
- **Coverage:** ~60% (main crate body)

**Orange Lid Paint (Functional Accent)**
- **Albedo:** Orange paint (RGB ~240, 140, 60)
- **Roughness:** ~0.42 (painted finish)
- **Metalness:** 0
- **Coverage:** ~20% (lid covers)

**Cyan LED Indicators (Glowing)**
- **Albedo:** Very dark cyan (RGB ~0, 20, 20)
- **Emissive:** Bright cyan glow (RGB ~100, 220, 255, emissive 4.0)
- **Roughness:** ~0.3 (LED appearance)
- **Coverage:** ~10% (indicator lights, antenna)
- **Purpose:** Signals active electronics, high visibility

**Mid-Tone Metal (Modules)**
- **Albedo:** Medium steel (RGB ~180, 190, 205)
- **Roughness:** ~0.45 (polished metal)
- **Metalness:** 0.75 (bare metal)
- **Coverage:** ~10% (secondary modules)

### Component (TC_PICKUP_Component_V1)

**Purple Crystal (Alien Artifact)**
- **Albedo:** Purple (RGB ~220, 80, 255)
- **Roughness:** ~0.22 (faceted, light-catching)
- **Metalness:** 0 (crystal, non-metallic)
- **Emissive:** Purple glow (RGB ~220, 80, 255, emissive 1.4)
- **Coverage:** ~60% (crystal cluster and satellites)

**Dark Substrate (Rock/Metal)**
- **Albedo:** Very dark (RGB ~45, 50, 58)
- **Roughness:** ~0.7 (matte, oxidized)
- **Metalness:** 0.2 (dirty metal/rock)
- **Coverage:** ~40% (base rock, substrate)
- **Purpose:** Alien artifact material, exotic color language

---

## Poly Count & Complexity

| Asset | Target Poly Budget | Detail Level |
|-------|-------------------|--------------|
| **Metal** | ~200 | Low: clean geometric block, minimal seams |
| **Biomass** | ~250 | Low–Medium: organic lumpy form, surface variation |
| **Electronics** | ~300 | Medium: board detail, component geometry |
| **Component** | ~280 | Medium: hybrid form, bolts and connectors |
| **TOTAL** | **~1,030** | Budget: ≤1,200 (modest detail margin) |

---

## Interaction Flow (Godot Integration)

Pickup collection is **instant, non-visual mesh change**:

**Player approaches:**
1. Enters collision trigger zone (~1m radius around pickup)
2. Godot registers proximity (no mesh change)
3. Player presses interact key (E or F) or auto-collects
4. Godot increments resource counter (inventory system)
5. Mesh disappears (Godot node removed or visibility toggle)
6. Optional: Play collection SFX and particle effect

**Implication:** Pickups are pure visual markers. All collection logic lives in Godot, not the mesh.

---

## Validation Checklist

**Before export (Blender):**
- [ ] Metal reads as structural industrial material (clean, reflective)
- [ ] Biomass reads as organic matter (lumpy, matte, warm)
- [ ] Electronics reads as circuitry (cyan accents, board detail)
- [ ] Component reads as modular tool (orange accent, hybrid form)
- [ ] All four pickups are clearly distinct at distance (silhouettes differ)
- [ ] Cyan and orange accents are emissive-ready
- [ ] All material zones defined and named
- [ ] No overlapping faces or internal geometry
- [ ] UV unwrapped efficiently (can be simple/tiled)

**After export to GLB:**
- [ ] File sizes reasonable (~1–2 MB total for 4 assets)
- [ ] Godot imports without errors
- [ ] Emissive materials (cyan, orange) glow correctly
- [ ] Silhouettes match brief (distinct, recognizable)

**In first-person gameplay (5–10 meters away):**
- [ ] Player can identify resource type from distance
- [ ] Metal appears silvery and structural
- [ ] Biomass appears organic and brown
- [ ] Electronics cyan glow is visible
- [ ] Component orange glow is visible and distinct
- [ ] All four types are clearly distinguishable

**Visual readability:**
- [ ] No confusion between pickup types
- [ ] Color coding and silhouettes are clear
- [ ] Emissive accents enhance visibility
- [ ] Forms suggest their resource purpose semantically

---

## Success Criteria

✅ **Pickups are `ASSET_IMPLEMENTATION_PASS` when:**

1. All four assets exported as GLB with valid manifest entries
2. **Metal** silhouette reads "structural material" (cubic, clean, reflective)
3. **Biomass** silhouette reads "organic matter" (lumpy, rough, warm)
4. **Electronics** silhouette reads "circuitry" (board detail, cyan accent visible)
5. **Component** silhouette reads "modular tool" (hybrid form, orange accent visible)
6. Cyan (electronics) and orange (component) emissive materials glow correctly
7. Material palettes coherent (metal gray, biomass brown, electronics cyan, component orange)
8. Poly count ≤ 1,200 total
9. No visual corruption, texture stretching, or floating geometry
10. Turntable PNGs captured for all four types (1–2 angles each is sufficient for pickups)
11. SHA256 hashes recorded in asset manifest for all four
12. **Visibility test passed:** All four types identifiable from 5–10 meters (first-person view)

❌ **Pickups will be `NOT_GO` if:**

- Any type confuses with another or with other interactive props
- Cyan or orange accents insufficient or not visually emissive
- Poly count exceeds 1,400
- Any pickup form is ambiguous about its material type
- Materials do not export/display correctly
- Emissive accents do not glow or are barely visible
- Player cannot distinguish types at 5+ meter distance

---

## Integration Points (Phase 7 Composition)

Pickups will be scattered throughout the arena after composition. Placement guidelines:

- **Location:** Near resource clusters, accessible but not on direct paths
- **Density:** ~5–8 total pickups per arena (balanced across all four types)
- **Grouping:** Resources of same type clustered (suggests source location)
- **Orientation:** Natural resting orientation, Z-axis vertical for most
- **Collision:** Static, non-passable (player navigates around, doesn't pass through)
- **Visual dominance:** Smallest interactive silhouettes (after workbench, beacon, save point)
- **Line of sight:** Scattered for exploration, not all visible from spawn

---

## Deliverables

| Deliverable | Format | Location |
|---|---|---|
| Pickup Brief (this doc) | `.md` | `docs/art/briefs/brief-pickups-v1.md` |
| Blender Sources | `.blend` | `assets/Source/Blender/Production/MVP_Pack_V1/TC_PICKUP_*.blend` (4 files) |
| GLB Exports | `.glb` | `assets/Production/Generated/MVP_Pack_V1/TC_PICKUP_*.glb` (4 files) |
| Manifest Entries | JSON | `assets/Production/Generated/asset_manifest.json` |
| Review PNGs | `.png` | `artifacts/asset-review/TC_PICKUP_*/` (4 folders) |

---

## Approval Gate

**Brief Status:** `ASSET_BRIEF_READY`  
**Next Gate:** `ASSET_IMPLEMENTATION_PASS` (after Blender refinement/export and visibility test)  
**Final Gate:** `VISUAL_SLICE_GAMEPLAY_SAFE` (after composition integration in Phase 7)  
**Authority:** Project Director + Distance visibility test (5–10m FPS) + Resource type distinction review
