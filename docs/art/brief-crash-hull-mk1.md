# Asset Brief: Crash Hull Mk1
## Phase 2 Hero — Largest Silhouette, Salvage Foundation

**Brief Owner:** TitanCraft Project Director  
**Target Asset:** Crashed spacecraft, crushed and broken, serves as visual anchor and salvage source  
**Scope:** Single hero asset representing the player's origin point and survival foundation  
**Date:** 2026-07-05  
**Design Philosophy:** Heavy industrial wreckage, not toy capsule  

---

## Visual Thesis

The crashed hull is **the world's largest and most important silhouette**. It must read as:

- **Heavy industrial spacecraft** (not sleek, not capsule-like)
- **Crushed asymmetrically** (impact damage, not cosmetic dents)
- **Torn open and salvageable** (internal structure visible, panels torn, materials exposed)
- **Partially buried** (lower hull embedded in ash, not sitting neatly on terrain)
- **Field-repaired** (human survival activity visible: patched panels, external cables, jury-rigged structures)
- **Source of resources** (metal panels, electronic components, structural material the player salvages)

The hull must **dominate the first read** from spawn but not interfere with wayfinding to resources or workbench.

---

## Scope Boundaries

### What This Brief Covers
- **Main fuselage:** Cargo/personnel compartment, primary hull body
- **Crushed nose cone:** Impact-deformed, asymmetric break
- **Broken wing assembly:** One/both damaged, torn panels
- **Engine pod(s):** Exposed internal turbines/reactors (not functional, visual storytelling)
- **Tail/rear section:** Torn spine, exposed ribs, breaks
- **Internal structure:** Visible ribs, bulkheads, cables where hull is torn
- **Salvage elements:** Detached panels, cargo containers, loose exterior brackets
- **Scorch/damage:** Burns, melting, impact marks (concentrated around breach/crush zone)
- **Field repair accents:** Temporary bracing, salvaged panels patched over holes, visible human intervention

### What This Brief Does NOT Cover
- Landing gear or landing systems
- Ejectable escape pods
- Cockpit interior detail (windows can be simple, cockpit area should not be explorable)
- Full avionics or control systems (if visible, should be nonfunctional/dead)
- Weapons or military systems
- Functioning thrusters or power systems
- Crew figures or remains

### What Remains Placeholder/Simple
- Distant background ship wreckage (larger crashed ships on rim; can be silhouette only in Phase 7)
- Cargo interior detail (if hull is opened, interior can be simple/dark; not explorable)
- Exterior surface weathering (general scorch; detailed texture work is Phase 8 materials pass)

---

## Design Reference: Form Language

### Silhouette Priorities

**Avoid:**
- ❌ Sleek, tapering fuselage (looks spaceship toy)
- ❌ Perfect symmetry (looks designed, not crashed)
- ❌ Smooth, undamaged surfaces (looks pristine)
- ❌ Capsule-like rounded pod (looks too much like space shuttle)
- ❌ Minimal damage (impact alone is boring; add internal structure revelation)

**Target:**
- ✅ Boxy industrial fuselage with heavy structural ribs
- ✅ Crushed nose (one side collapsed inward, asymmetric)
- ✅ Torn wing: one wing at angle, other broken/separated
- ✅ Exposed internal structure where hull is breached
- ✅ Heavy mass: thick hull panels, dense substructure, feels weighty
- ✅ Tilted orientation: not sitting perfectly upright; hull rests at ~15–30° angle suggesting collision direction
- ✅ Partially buried: lower rear hull embedded in ash, front nose raised or tipped

### Scale & Proportion

| Aspect | Target | Rationale |
|--------|--------|-----------|
| **Length** | ~60–80m | Largest object in basin; visible from entire map |
| **Height** | ~25–35m | Dominates 1.7m player view, towers over other objects |
| **Width** | ~15–20m | Substantial footprint; player can walk around full ship |
| **Orientation** | Tilted ~20° | Suggests impact/crash direction; playable surfaces vary |
| **Burial depth** | ~1/3 rear section underground | Suggests impact force, adds visual weight |

### Poly Count & Complexity

| Component | Poly Budget | Detail Level |
|-----------|-------------|--------------|
| **Main fuselage** | ~4,000 | Medium: visible rivets, panel lines, broad structural planes |
| **Crushed nose** | ~1,500 | High: asymmetric deformation, torn edges, internal exposure |
| **Wings (1–2)** | ~2,000 | Medium: torn edges, panel breaks, attitude variation |
| **Engines/pods** | ~1,500 | Medium: visible turbine/core, exterior damage, mount points |
| **Internal structure** | ~1,000 | Low–medium: visible ribs/bulkheads in breaches, not full interior |
| **Salvage/debris** | ~500 | Low: detached panels, containers, loose brackets |
| **Scorch/damage details** | ~500 | Low: burn marks, melting, small deformations |
| **Field repairs** | ~300 | Low: temporary bracing, patched panels (if modeled 3D) |
| **TOTAL** | **~11,300** | Target: ≤13,000 to leave room for detail pass |

---

## Structural Breakdown

### Hull Sections (Recommended Assembly Order)

#### **1. Main Cargo Fuselage**
- Primary hull body (cylinder w/ flat bottom, squared edges)
- Heavy visible ribbing on exterior (every ~2m structural beam)
- Rectangular window line (large rectangular ports suggesting cargo/passenger capacity)
- Slight taper toward tail but primarily boxy/industrial
- No taper toward nose (squared-off design)

#### **2. Crushed Nose Cone**
- Asymmetric crush: right side collapsed inward ~40%, left side less damaged
- Interior metal/structure exposed in collapse zone
- Sharp torn edges (not smooth fracture)
- Internal dome structure barely visible (faintly, gives depth)
- Slight scorching around crush zone

#### **3. Wing Assembly (Primary)**
- Large rectangular wing planform
- Torn at midspan: front half torn away or at severe angle, rear half attached
- Exposed internal spar structure where torn
- Panels on remaining section twisted from impact

#### **4. Wing Assembly (Secondary, Optional)**
- If both wings included: one mostly intact but damaged, one heavily torn
- Adds asymmetry and visual interest
- Second wing can be partially buried or leaning

#### **5. Engine Pod(s)**
- Approximately 1–2 large engine pods attached to rear fuselage
- Exposed turbine/compressor stages (concentric circles or blades visible)
- Heavy external structure, not sleek/aerodynamic (functional, not beautiful)
- One or both showing internal damage

#### **6. Tail Section / Rear**
- Stabilizer fins (one or both damaged/bent)
- Exposed internal ribs where fuselage is torn
- Tail cone attached but could show breaking/separating
- Heaviest scarring/burn marks in this region (impact zone assumption)

#### **7. Exposed Interior Details (Where Hull Breaks)**
- Visible bulkheads/frames where hull is torn
- Structural members visible (ribs, stringers, cross-bracing)
- Cables/conduits (dark, thin, suggest internal complexity)
- Not fully detailed interior, but enough to read as industrial/complex

#### **8. Salvage Debris**
- Detached hull panels (various sizes, scattered around/under hull)
- Cargo containers (rectangular boxes, some crushed)
- Loose exterior brackets, external equipment (radar dish, antennae if simplified)
- These can be placed loose around/under hull to suggest impact scatter

#### **9. Environmental Damage**
- Scorch/burn marks concentrated around fuselage breach and tail (impact zone)
- Melting/ablation on surfaces facing impact direction
- Impact crater depression under nose (asteroid/impact side)
- Ash accumulation on buried rear section (suggests age, weathering)

---

## Material & Color Specification

### Primary Hull Material (Off-White Worn)
- **Albedo:** Cream/off-white worn paint (RGB ~200, 190, 180)
- **Roughness:** ~0.75 (weathered, microabrasion from space)
- **Metalness:** 0 (painted surface, not bare metal)
- **Detail:** Subtle panel line shadows, no bold weathering stripes
- **Coverage:** ~60% of visible surfaces

### Structural Steel (Dark Gray / Graphite)
- **Albedo:** Dark structural steel (RGB ~100, 100, 110)
- **Roughness:** ~0.7 (oxide, not polished)
- **Metalness:** 0.3 (bare steel, slight metal response)
- **Detail:** Rivets and fastener details at medium distance
- **Coverage:** ~25% (structural ribs, interior exposed structure)

### Engine/Internal Metal (Steel with Oxidation)
- **Albedo:** Medium gray steel (RGB ~130, 130, 140)
- **Roughness:** ~0.8 (oxidized, weathered)
- **Metalness:** 0.2 (metal core under oxide)
- **Coverage:** ~10% (turbine housings, engine casings)

### Scorch/Burn Zone (Charred)
- **Albedo:** Black-brown (RGB ~40, 30, 25)
- **Roughness:** ~0.85 (charred surface, rough)
- **Metalness:** 0 (organic char, not metal)
- **Location:** Concentrated around breach/impact zone, tail area
- **Coverage:** ~5%

### Interior Cables/Detail (Dark)
- **Albedo:** Very dark gray (RGB ~50, 50, 60)
- **Roughness:** ~0.6 (bundled, slightly reflective)
- **Metalness:** 0.2 (copper/aluminum core under insulation)

---

## Placement & Collision

### Position in Crash Basin
- **Nose elevation:** ~8m above ash floor (tilted position)
- **Tail elevation:** ~–2m below ash floor (buried ~30% of length)
- **Orientation:** Tilted ~20–25° from vertical (tilt axis along one wing direction)
- **Position in basin:** Offset from center; not perfectly symmetrical (suggests directional crash)

### Collision Setup
- Static collision body (no physics, player cannot move ship)
- Hull surfaces: solid collision (player cannot pass through or climb exterior)
- Interior (if exposed): optional simple collision barrier (player cannot jump into cargo hold)
- Loose panels/debris: optional physics (can tumble, but not critical for MVP)
- No ladders or climbable exterior (Crash Site = no climbing in MVP)

### Visual Occlusion
- Hull should block line-of-sight to far world edges
- Hull should NOT block essential routes (resource collection, workbench approach, arena entry)
- Careful positioning ensures player can walk around full hull perimeter

---

## Integration Points (Phase 7 Composition)

The hull will be placed in Phase 7 after terrain is validated. Placement guidelines:

- **Spawn view:** Hull is first/primary silhouette visible from spawn orientation
- **Resource access:** Players can walk around hull to reach resource clusters without climbing
- **Workbench approach:** Routes near hull but do not require hugging wreckage
- **Arena clearance:** Combat zone is clear of hull wreckage (distant backdrop only)
- **Beacon approach:** Beacon placement is on opposite side of basin from hull (visual destination)

---

## Validation Checklist

**Before export (Blender):**
- [ ] Silhouette reads heavy crushed ship (not toy capsule) in neutral gray
- [ ] Asymmetric crush visible (not perfect or symmetric)
- [ ] Interior structure visible where hull is torn (depth, not flat)
- [ ] Nose, wing, engine, tail all distinguishable
- [ ] All material zones defined and named
- [ ] No overlapping faces or internal geometry
- [ ] Collision hull prepared (clean, convex where needed)
- [ ] UV unwrapping complete and efficient

**After export to GLB:**
- [ ] File size reasonable (~8–15 MB)
- [ ] Godot imports without errors
- [ ] Collision bodies auto-generated or properly assigned
- [ ] Material assignments visible
- [ ] Silhouette matches brief (heavy, crushed, industrial)

**In Godot scene (Phase 7 integration):**
- [ ] Hull loads without rendering artifacts
- [ ] Player can walk around full perimeter
- [ ] Hull does not clip terrain or resources
- [ ] Player cannot climb exterior or reach interior
- [ ] Visual dominance: hull is largest silhouette
- [ ] No gameplay blocking issues

**Visual readability:**
- [ ] Crushpoint is obvious (asymmetric deformation visible)
- [ ] Internal structure reads as industrial/complex
- [ ] Contrast with terrain is clear (ship vs. basalt vs. ash)
- [ ] Environmental story is readable (crash impact, damage, repair)

---

## Success Criteria

✅ **Phase 2 is `ASSET_IMPLEMENTATION_PASS` when:**

1. Hull mesh exported as GLB with valid manifest entry
2. Silhouette reads **heavy industrial crushed spacecraft** (not toy capsule)
3. Asymmetric crush is obvious (internal structure exposed)
4. Nose, wings, engines, and tail are all identifiable
5. Interior structure visible in torn areas (depth, not flat)
6. Material palette coherent (worn paint, structural steel, engine metal, char)
7. Poly count ≤ 13,000 total
8. No visual corruption, texture stretching, or floating geometry
9. Neutral gray turntable PNG captured from multiple angles (hero 3–4 angle coverage)
10. SHA256 hash recorded in asset manifest
11. Collision geometry is clean and safe
12. Gameplay smoke test passed (no movement regressions around hull)

❌ **Phase 2 will be `NOT_GO` if:**

- Silhouette reads toy-like, sleek, or undamaged (too pristine)
- Crush is symmetric or subtle (no obvious impact)
- Interior structure is not visible or reads flat
- Hull feels lightweight or hollow (visual mass missing)
- Poly count exceeds 15,000
- Material palette is incoherent (glossy paint, photorealistic, over-detailed)
- Collision issues prevent player access or cause clipping

---

## Deliverables

| Deliverable | Format | Location | Owner |
|-------------|--------|----------|-------|
| Asset Brief (this doc) | `.md` | `docs/art/brief-crash-hull-mk1.md` | Project Director |
| Blender Source | `.blend` | `art/blender/models/TC_CRASH_HullMk1_V1.blend` | Blender/Asset Forge |
| GLB Export | `.glb` | `assets/models/hero/TC_CRASH_HullMk1_V1.glb` | Asset Forge |
| Manifest Entry | JSON | `assets/Production/Generated/asset_manifest.json` | Asset Forge |
| Silhouette PNG (neutral gray) | `.png` | `artifacts/review/hull-v1/TC_CRASH_HullMk1_silhouette.png` | Visual Artifact Factory |
| Turntable PNG (3–4 angles) | `.png` | `artifacts/review/hull-v1/TC_CRASH_HullMk1_turntable.png` | Visual Artifact Factory |
| Damage/detail close-up | `.png` | `artifacts/review/hull-v1/TC_CRASH_HullMk1_detail.png` | Visual Artifact Factory |

---

## References

- **Master Plan:** `docs/art/crash-site-worldclass-visual-master-plan.md`
- **Visual Identity:** `docs/art/titancraft-visual-identity.md`
- **Production Roadmap:** `docs/art/crash-site-visual-production-roadmap.md`
- **Execution Plan:** `docs/art/VISUAL_UPGRADE_EXECUTION_2026-07-05.md`
- **Terrain Brief:** `docs/art/brief-terrain-crash-basin.md`

---

## Approval Gate

**Brief Status:** `ASSET_BRIEF_READY`  
**Next Gate:** `ASSET_IMPLEMENTATION_PASS` (after Blender export and validation)  
**Final Gate:** `VISUAL_SLICE_GAMEPLAY_SAFE` (after composition integration in Phase 7)  
**Authority:** Project Director + Visual readability review + Gameplay safety
