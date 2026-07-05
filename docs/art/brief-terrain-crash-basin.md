# Asset Brief: Crash Basin Terrain
## Phase 1 Foundation — Shaped World Foundation

**Brief Owner:** TitanCraft Project Director  
**Target Asset:** Crash Site volcanic basin, ash floor, basalt rim, route landmarks  
**Scope:** Single contiguous terrain mesh(es) supporting navigation loop  
**Date:** 2026-07-05  

---

## Visual Thesis

The crash basin must read as a **hostile volcanic world staged for survival recovery**. The survivor lands in an ash-choked depression between basalt ridges, where the terrain itself guides navigation:

- **Ash floor** = primary traversal surface (gray, dusty, worn)
- **Basalt ridges & rocks** = world frame, visual weight, combat arena surround
- **Fractured ground** = environmental storytelling (impact zone, seismic activity)
- **Route markers** = naturally occurring guidance (darker ash paths, ridge shadows, terrain steps)
- **Playable slopes** = no clipping, no floating collision, readable steepness

The terrain must **support the locked MVP loop** without implying a larger explorable world, multiple biomes, or procedural generation.

---

## Scope Boundaries

### What This Brief Covers
- Ash floor (primary walking surface)
- Basalt foreground/midground rock formations
- Rim ridge defining basin edge
- Fractured/broken ground (impact zone aesthetic)
- Natural route landmarks (paths between spawn → resources → workbench → arena → beacon)
- Simple scorch and dust accents

### What This Brief Does NOT Cover
- Procedural terrain generation
- Destructible terrain
- Terrain LOD system
- Dynamic weather or erosion
- Vegetation or alien crystal outcrops (those are Phase 3-4 decoration)
- Large-scale height variation (stay within ~30m elevation range)

### What Remains Placeholder
- Distant background rim silhouettes (can be 2D billboard or simple geometry; Phase 7 composition)
- Atmospheric effects (dust, haze; Phase 8 lighting pass)
- Audio-reactive or interactive terrain (blocked by MVP scope)

---

## Technical Specifications

### Geometry

| Aspect | Target | Rationale |
|--------|--------|-----------|
| Poly Count | ~8,000–12,000 total | Medium-detail, visible from FPS distance, performance-safe |
| Mesh Complexity | Single or 2–3 combined meshes | Import/material control, collision clarity |
| Surface Finish | Authored bevels, chunky planes | Low-to-medium poly; avoid perfect smoothness or smooth-shade noise |
| Scale | 150m × 150m basin depth variant | MVP loop traversable 2–3 minutes on foot |
| Height variation | 0–20m elevation change | Terrain feels shaped, not flat; no excessive climbing |
| Walkability | All playable surfaces ≥30° slope max | Player can navigate without sliding/glitching |

### Material Zones

Define distinct material areas (to be painted/assigned in Blender):

| Zone | Description | Target Material | Poly Budget |
|------|-------------|-----------------|-------------|
| **Ash Floor** | Primary walking surface | Gray ash, dusty worn | ~4,000 |
| **Basalt Foreground** | Player-facing rock formations (left/right framing) | Dark basalt, heavy | ~2,500 |
| **Basalt Midground** | Middle-distance rocks, route markers | Basalt, varied angle | ~2,000 |
| **Fractured Ground** | Central impact zone, broken terrain | Gray stone, sharp breaks | ~1,500 |
| **Ridge Rim** | Basin edge, background frame | Basalt ridge line | ~1,500 |
| **Scorch/Dust** | Environmental story (burn marks, dust accumulation) | Brown scorch, gray dust overlay | ~500 |

### Silhouette Requirements

- **Neutral gray render:** Terrain must be readable in silhouette before materials applied
- **First-person height:** Foreground rocks must frame horizon at 1.7m eye level
- **Route clarity:** Player must identify next waypoint (resource cluster, workbench direction) via terrain shape, not UI
- **No flat platter:** Terrain must have depth, ridge line must contrast sky, ash floor must read as depression floor

---

## Route & Landmark Structure

### Spawn Zone (Entry Point)
- Player appears near crashed ship debris (Phase 2 asset placement)
- Terrain slopes gently downward toward basin floor
- First visual read: crashed ship is largest silhouette; terrain frames it
- Collision: safe to land, walk forward, no immediate obstacles

### Resource Cluster Trail (Collection Phase)
- Ash floor path with subtle height variation (±1m) guides toward resources
- Basalt rocks mark resource zones (visual waypoints)
- Foreground basalt creates route-edge framing on left/right
- Travel time: ~30 seconds walking

### Workbench Approach
- Route opens into slight depression (workbench placement zone)
- Ridge or rock formation on far side provides backdrop
- Terrain at workbench is level, collision-clean, safe interaction distance

### Combat Arena Entrance
- Route narrows between basalt formations (arena entry point)
- Interior space (~20m × 20m) for Scout encounter
- Arena floor is level ash, arena walls are tall basalt rocks
- Ridge rim visible in distance (framing, no escape expectation)

### Component Recovery & Save/Beacon Zone
- Routes from arena toward save point and beacon
- Terrain elevation slight rise (visual signal: "ascend to transmit")
- Save point and beacon are placed on elevated terrain plateau
- Clear line of sight from player to beacon for final activation

### Background (Distant Rim)
- Basalt ridge line forms horizon, defines basin edge
- No climbing/escape implied; rim is visual frame, not traversable
- Distant silhouettes can include procedural or 2D billboard rim complexity (not poly-detailed)

---

## Material & Color System

### Ash Floor Palette
- **Albedo:** Dusty gray (RGB ~140, 140, 140)
- **Roughness:** High (~0.8); weathered dust
- **Metalness:** 0; no reflection
- **Detail:** Subtle noise, no pattern; worn surface feel

### Basalt Rock Palette
- **Albedo:** Dark gray-black (RGB ~80, 80, 90)
- **Roughness:** Medium (~0.7); rough volcanic surface
- **Metalness:** 0
- **Detail:** Faceted planes emphasize angular mass, avoid photo-scanned noise

### Scorch/Burn Marks
- **Albedo:** Brown-black (RGB ~60, 50, 40)
- **Location:** Scattered on ash and basalt near impact zone
- **Roughness:** High (~0.85); charred appearance
- **Extent:** ~10–15% of visible terrain; storytelling only, not overwhelming

### Dust Accents
- **Albedo:** Light gray (RGB ~180, 180, 175)
- **Location:** Edges of terrain steps, ridge tops, path boundaries
- **Effect:** Highlights form without being glowing or unrealistic

---

## Validation Checklist

**Before export (Blender):**
- [ ] Mesh(es) UV-unwrapped (unique or tiled, no overlap)
- [ ] No overlapping faces or internal geometry
- [ ] Collision-clean walkable surfaces (no floating geometry)
- [ ] Material zones defined and named
- [ ] Neutral gray screenshot captured (wireframe mode or simple material)

**After export to GLB:**
- [ ] File size reasonable (~3–8 MB depending on poly count)
- [ ] Godot imports without errors or warnings
- [ ] Collision bodies auto-generated or manually assigned
- [ ] Texture paths resolve correctly
- [ ] Silhouette matches brief (readable, shaped, not flat)

**In Godot scene:**
- [ ] Terrain loads without rendering artifacts
- [ ] Player can walk all playable surfaces (no clipping)
- [ ] Camera height (1.7m eye level) provides correct view angles
- [ ] Route landmarks are visually distinct at walking distance
- [ ] Spawn area, resource trail, workbench approach, arena entry, beacon zone are all accessible

**Visual readability:**
- [ ] Terrain does not read flat or plate-like
- [ ] Basalt rocks provide framing and depth
- [ ] Ash floor clearly distinct from basalt
- [ ] Foreground rocks frame player view naturally
- [ ] Ridge rim visible but not climable (visual horizon)

---

## Success Criteria

✅ **Phase 1 is `ASSET_IMPLEMENTATION_PASS` when:**

1. Terrain mesh exported as GLB with valid manifest entry
2. Silhouette reads shaped volcanic basin (not flat)
3. Ash floor texture distinct from basalt rocks
4. Route landmarks (resource zones, workbench, arena, beacon) all identifiable via terrain form
5. Player can traverse entire MVP loop without collision issues
6. Poly count ≤ 12,000 total
7. No visual corruption, stretching, or floating geometry
8. Neutral gray turntable PNG captured and labeled
9. SHA256 hash recorded in asset manifest
10. Gameplay smoke test passed (no movement/collision regressions)

❌ **Phase 1 will be `NOT_GO` if:**

- Terrain reads flat or boring (plate-like visual)
- Routes are ambiguous (player unsure where to go via terrain shape)
- Poly count exceeds 15,000 or causes frame rate drop
- Basalt rocks feel decorative rather than structural framing
- Collision geometry is broken or unsafe
- Import fails or causes shader/material errors in Godot

---

## Deliverables

| Deliverable | Format | Location | Owner |
|-------------|--------|----------|-------|
| Asset Brief (this doc) | `.md` | `docs/art/brief-terrain-crash-basin.md` | Project Director |
| Blender Source | `.blend` | `art/blender/models/TC_TERRAIN_CrashBasin_V1.blend` | Blender/Asset Forge |
| GLB Export | `.glb` | `assets/models/terrain/TC_TERRAIN_CrashBasin_V1.glb` | Asset Forge |
| Manifest Entry | JSON | `assets/Production/Generated/asset_manifest.json` | Asset Forge |
| Turntable PNG | `.png` | `artifacts/review/terrain-v1/TC_TERRAIN_CrashBasin_turntable.png` | Visual Artifact Factory |
| Neutral Gray Reference | `.png` | `artifacts/review/terrain-v1/TC_TERRAIN_CrashBasin_silhouette.png` | Visual Artifact Factory |

---

## Known Constraints & Decisions

**Why not procedural terrain?**
- Procedural terrain can read flat and generic
- Authored terrain gives clear route guidance
- Controlled poly count is easier to validate
- Better for collision safety and gameplay readability

**Why ash + basalt only?**
- Volcanic world requires two dominant materials for contrast
- Ash (light) and basalt (dark) provide silhouette clarity
- Scorch marks add environmental storytelling without expanding scope

**Why medium poly, not high-detail?**
- Lower poly keeps performance safe for offline Windows export
- Low-to-medium poly matches brief style (Polygonal Salvage Sci-Fi)
- Detail is reserved for hero assets (ship, Scout, arm)
- Environment is supporting player experience, not the focus

**Why single connected basin, not multiple zones?**
- MVP locked to one Crash Site
- Single terrain mesh simplifies collision and LOD
- No implication of larger explorable world
- Easier to validate route clarity and readability

---

## Approval Gate

**Brief Status:** `ASSET_BRIEF_READY`  
**Next Gate:** `ASSET_IMPLEMENTATION_PASS` (after Blender export and Godot validation)  
**Final Gate:** `VISUAL_SLICE_GAMEPLAY_SAFE` (after composition integration in Phase 7)  
**Authority:** Project Director + Gameplay smoke test + Visual readability review

---

## References

- **Master Plan:** `docs/art/crash-site-worldclass-visual-master-plan.md`
- **Visual Identity:** `docs/art/titancraft-visual-identity.md`
- **Production Roadmap:** `docs/art/crash-site-visual-production-roadmap.md`
- **Technical Bible:** `docs/visual-technical-bible.md`
- **MVP Scope:** `docs/production/mvp-scope.md`
