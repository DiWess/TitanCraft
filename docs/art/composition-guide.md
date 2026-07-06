# Composition Guide: Crash Site MVP
## Visual Composition Principles for Approved Stage A Environment

**Date:** 2026-07-05  
**Phase:** Phase 7.1 (Art Director composition analysis)  
**Scope:** Document visual composition principles for the approved Stage A Crash Site MVP environment  
**Authority:** Art Taste Pack visual identity + Phase 7 planning  
**Status:** Draft for Visual Reviewer approval

---

## Overview

This guide documents the visual composition principles discovered in the approved Stage A Crash Site MVP environment. These principles guide future asset creation, scene layout refinement, and visual consistency decisions.

The Crash Site is built on **Polygonal Salvage Sci-Fi** visual identity: readable, low-to-medium poly, damaged human hardware combined with hostile volcanic terrain.

---

## 1. Visual Hierarchy & Focal Points

### Primary Focal Point: Crashed Ship Wreckage
The crashed ship hull dominates the Crash Site visual hierarchy. It serves as the primary landmark and story anchor.

**Assets involved:**
- `TC_HullMain.obj` (primary hull structure)
- `TC_NoseCrushed.obj` (damaged nose cone)
- `TC_WingSheared.obj` (severed wing section)
- `TC_EngineExposed.obj` (damaged propulsion system)
- `TC_BreachDebris.obj` (hull rupture debris)

**Composition principle:**
The hull wreckage is positioned to be visually dominant when the player spawns. Its silhouette is immediately recognizable as "damaged spacecraft" through:
- Large scale (dwarfs the player)
- Broken proportions (nose crushed, wing sheared)
- Visible internal structure (exposed engine, internal ribs)
- Material contrast (off-white hull against darker terrain)

**Guideline for future artists:**
The primary focal point must be clearly damaged or broken—intact structures read as "safe haven" not "crash site." Wreckage silhouettes must be immediately readable without materials.

### Secondary Focal Points: Objective Markers

Three secondary focal points guide player progression:
1. **Workbench** (resource fabrication)
   - Positioned at functional distance from spawn
   - Elevated slightly on terrain for visibility
   - Visually distinct shape (structured vs. organic)

2. **Beacon** (mission objective)
   - Positioned at farthest reach of playable area
   - Should be visible from multiple sightlines
   - Orange/cyan accents signal interactivity

3. **Enemy Zone** (Galaxabrain Scout encounter)
   - Natural terrain formation that creates arena space
   - Volcanic ridge provides visual separation

**Guideline for future artists:**
Secondary objectives should be spatially separated but visually linked through clear routes. Each should have distinct scale or material characteristics so players can distinguish them from terrain.

---

## 2. Route Readability & Navigation

### Visual Path Language

The Crash Site guides player movement through visual cues rather than arrows or guides:

**Navigation principles:**
1. **Destroyed structure as waypoint**
   - Wreckage debris chunks create breadcrumb trail from spawn toward objectives
   - `TC_BreachDebris.obj` placement suggests "this way"
   - Debris scale prevents jumping over (guides movement naturally)

2. **Terrain elevation changes**
   - Ridge formations (`TC_RidgeA.obj`, `TC_RidgeB.obj`, `TC_RidgeC.obj`) create natural passages
   - Slopes guide flow while remaining climbable
   - Elevation provides vantage points for spatial awareness

3. **Material contrast for traversability**
   - Ash patches (`TC_AshPatch.obj`) on darker basalt create visual separation
   - Off-white metal debris contrasts against volcanic rock
   - Orange interactive markers (workbench, beacon) signal destinations

4. **Silhouette clarity for obstacles**
   - Basalt formations (`TC_BasaltForeground.obj`, `TC_BasaltMidgroundA.obj`, `TC_BasaltMidgroundB.obj`) read as obstacles
   - Their silhouettes clearly show traversable vs. blocked areas
   - No ambiguous "can I jump that?" geometry

**Guideline for future artists:**
Every navigation choice should be readable from a distance. If the player needs to stop and think "where do I go?", the visual design has failed. Routes should feel inevitable, not arbitrary.

---

## 3. Silhouette & Scale Relationships

### Silhouette-First Design

All Stage A assets follow silhouette-first design: each element reads clearly in neutral grey before materials are applied.

**Key silhouette principles:**

1. **Ship Wreckage vs. Terrain**
   - Wreckage has angular, broken edges
   - Terrain has rounded, natural curves
   - Contrast is immediate and clear

2. **Distinct Scale Hierarchy**
   - Hull main (huge, dwarfs player) = primary landmark
   - Basalt formations (medium, player-scale)
   - Debris chunks (small, interactive scale)
   - No ambiguous intermediate sizes that confuse spatial reading

3. **Readable Damage States**
   - Crushed nose, sheared wing = immediate "this is broken"
   - No intact structures (would read as intact, not crashed)
   - Exposed internal structure (ribs, engine) = storytelling through silhouette

4. **Volcanic Terrain Silhouette**
   - Dark basalt (dark silhouette) vs. light ash (light silhouette)
   - Creates visual paths without requiring materials
   - Fractured ground shows degradation through geometry, not texture

**Guideline for future artists:**
Before applying any material, ask: "Does this silhouette read clearly at 100m distance?" If the answer is no, fix the geometry first. Materials should enhance, not rescue, weak silhouettes.

---

## 4. Material & Lighting Coherence

### Material Palette

The Crash Site uses a tight, intentional material palette that supports readability:

**Human technology materials:**
- **Off-white hull** (TC_HullOffWhite) — primary structural element, aged and worn
- **Graphite/dark metal** (TC_Graphite, TC_WornSteel) — mechanical components, damage
- **Muted orange** (interactive accents) — functional marking, hazard warning
- **Scorch/burn marks** (TC_Scorch) — fire damage, energy damage, aging

**Terrain materials:**
- **Basalt dark rock** (TC_Basalt) — primary terrain, volcanic
- **Ash light** (TC_Ash) — fallout, aged ash layer
- **Fractured ground** (TC_FracturedGround) — geological damage

**Alien materials:**
- **Cyan emissive** (TC_CyanBreach) — alien energy, focal accent
- **Alien black** — alien biomass contrast

**Material coherence principle:**
Every material choice supports the story: "This ship crashed here, was damaged by impact and fire, sits on a hostile volcanic planet." No clean, showroom-like surfaces. No bright neon that contradicts damage narrative.

### Lighting Strategy

Stage A lighting emphasizes:
1. **Harsh contrasts** — damaged ship and alien landscape are not subtle
2. **Readable silhouettes** — shadows enhance, not obscure, shape recognition
3. **Functional glows** — cyan breach, orange markers are rare and signaling
4. **No romantic lighting** — this is survival, not vacation

**Guideline for future artists:**
Lighting should make gameplay elements more readable, not more dramatic. If dramatic lighting obscures a player's ability to judge distance or traverse safely, reduce it. Function over mood.

---

## 5. Foreground, Midground, Background Composition

### Spatial Layering

The Crash Site uses depth layering to create spatial coherence:

**Foreground (player immediate area):**
- Fractured ground debris
- Small ash patches
- Resource pickups (mineral deposits, biomass)
- Interactive elements (within reach)

**Midground (active play area):**
- Basalt ridge formations
- Workbench (crafting station)
- Galaxabrain Scout spawn zone
- Mid-scale debris chunks

**Background (visual anchor/boundary):**
- Primary ship hull wreckage
- Major terrain ridges
- Beacon (visible but distant)
- Environmental boundary (cliffs, impassable terrain)

**Depth composition principle:**
Clear foreground → distinct midground → readable background. Each layer is visually separate but compositionally connected. No layer should be ambiguously sized (is that rock close or far?).

**Guideline for future artists:**
Use material contrast, scale, and positioning to create depth. Distant elements should use fewer details, slightly cooler colors (if atmospheric perspective applies), and simpler geometry. Close elements can have more detail and contrast.

---

## 6. Color Palette & Contrast

### Intentional Color Language

The Crash Site palette is limited and purposeful:

**Primary colors (80% of scene):**
- Dark greys/blacks (basalt terrain)
- Light greys/whites (metal, ash)
- Muted earth tones (burn marks, wear)

**Accent colors (20% of scene, functional):**
- Orange (interaction, hazard marking)
- Cyan (alien energy, focal accent)
- Violet (enemy/alien contrast)

**Contrast principle:**
Dark rock vs. light metal vs. orange accents creates visual clarity without oversaturation. The alien cyan is rare and signals "danger" or "objective." No random color used for decoration.

**Guideline for future artists:**
If you're adding color to the Crash Site, first answer: "What does this color communicate?" Orange = interact. Cyan = alien/danger. If the color doesn't communicate, it's decoration and should be removed.

---

## 7. Visual Identity Alignment

All Stage A composition adheres to **Polygonal Salvage Sci-Fi** principles from `docs/art/titancraft-visual-identity.md`:

✓ **Not cartoonish** — Hull wreckage reads as serious damage, not toy-like  
✓ **Not photorealistic** — Broad polygonal forms prioritize readability  
✓ **Not block-based** — Organic terrain + engineered structure distinction  
✓ **Not glossy toy** — Worn, dirty, damaged materials throughout  
✓ **Low-to-medium poly** — Authored forms, controlled detail density  
✓ **Strong silhouettes** — All elements read in neutral grey  
✓ **Simplified PBR** — Stable albedo/roughness, no texture noise chaos  
✓ **Worn and repaired** — Visible damage, patched panels, scorch marks  
✓ **Selective glow** — Cyan accent is functional, not decoration

---

## Composition Principles Summary

| Principle | Application in Crash Site | Guideline for Future Work |
|-----------|---------------------------|--------------------------|
| **Focal Points** | Crashed hull primary, objectives secondary | Each focal point must be visually distinct and serve narrative |
| **Route Readability** | Debris → ridges → objectives form natural paths | Navigation should feel inevitable, not arbitrary |
| **Silhouette** | All elements read in grey, damage is readable | Silhouette first; materials enhance, don't rescue |
| **Materials** | Tight palette: worn metals, dark rock, accents | Every material choice supports the crash narrative |
| **Lighting** | Harsh contrasts, readable silhouettes, functional glow | Prioritize gameplay readability over dramatic mood |
| **Depth** | Clear foreground/midground/background layering | Each layer distinct but compositionally connected |
| **Color** | Dark/light/orange/cyan with intentional meaning | Color communicates function; decoration is removed |
| **Identity** | Polygonal Salvage Sci-Fi throughout | Damaged, readable, worn, modular, accessible |

---

## Key Assets & Their Composition Role

| Asset | Composition Role | Visual Function |
|-------|------------------|-----------------|
| `TC_HullMain` | Primary focal point | Story anchor, scale reference |
| `TC_NoseCrushed` | Damage narrative | Immediate "this is broken" signal |
| `TC_WingSheared` | Damage narrative | Large-scale destruction visible |
| `TC_BasaltForeground/Midground` | Terrain foundation | Readable navigation substrate |
| `TC_RidgeA/B/C` | Route guidance | Natural passage formation |
| `TC_BreachDebris` | Waypoint trail | Breadcrumb navigation |
| `TC_AshPatch` | Material contrast | Visual separation on terrain |
| `TC_Graphite/WornSteel` | Material language | Technological wear aesthetic |
| `TC_CyanBreach` | Accent glow | Alien/objective signal |

---

## For Future Asset Creators

When adding to or replacing elements in the Crash Site:

1. **Start with silhouette** — does it read clearly in grey?
2. **Respect scale hierarchy** — where does this sit in the foreground/mid/background?
3. **Align with palette** — does this color communicate or decorate?
4. **Support navigation** — does this guide or confuse player movement?
5. **Maintain identity** — is this Polygonal Salvage Sci-Fi or something else?
6. **Tell the story** — does this look like a crashed ship on a volcanic planet?

If you can answer all six questions affirmatively, the asset likely maintains Crash Site composition coherence.

---

## Next Steps (Phase 7.1.2)

Week 2 will include:
- 5-10 opened PNG screenshots of Crash Site gameplay
- Visual annotations showing focal points, routes, silhouettes, depth layering
- Specific examples of each composition principle in context
- Visual Reviewer independent approval of full composition guide

---

**Status:** 📝 **DRAFT — Ready for Visual Reviewer Detailed Examples (Week 2)**  
**Document created by:** Art Director (Phase 7.1.1)  
**Next gate:** Visual Reviewer approval (Phase 7.1.3)
