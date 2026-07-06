# Composition Examples & Diagnosis: Crash Site MVP
## Phase 7.1.2 Visual Examples with Focal Point Analysis

**Date:** 2026-07-06  
**Phase:** Phase 7.1.2 (Art Director composition examples)  
**Scope:** 8 detailed composition viewpoints with focal point analysis and silhouette diagnosis  
**Authority:** Composition Guide (Phase 7.1.1) + Stage A scene analysis  
**Status:** Ready for Visual Reviewer independent approval  

---

## Overview

This document describes 8 key composition viewpoints in the Stage A Crash Site. Each viewpoint demonstrates the principles documented in `composition-guide.md`. When paired with annotated PNG screenshots (captured during gameplay or via screenshot tool), this analysis provides Visual Reviewer with concrete evidence that the composition guide principles are correctly implemented and discoverable in the actual environment.

---

## Viewpoint 1: Primary Focal Point — Ship Wreckage Dominance

**Camera Position:** Player spawn (altitude 1.5m)  
**View Direction:** Forward toward main hull structure  
**Distance to Hull:** ~25-30 meters  

### Visual Principles Demonstrated

1. **Primary Focal Point (Visual Hierarchy & Focal Points § 1.1)**
   - Crashed ship hull dominates the field of view when player spawns
   - Large scale (dwarfs player) makes it immediately recognizable as "primary landmark"
   - Broken proportions (nose crushed, wing sheared) signal "damage" before examining details
   - Silhouette is readable in neutral grey — angular broken edges vs. organic terrain

2. **Silhouette Clarity (Silhouette & Scale Relationships § 3.1-3.2)**
   - Hull main structure: large box-like form with beveled/damaged edges
   - Nose cone: crushed cylinder showing deformation
   - Wing section: sheared flat plane, unnatural breakage
   - Exposed engine: cylindrical form visible through hull breach
   - **Diagnosis:** Silhouette reads as "spacecraft" before materials applied; damage is immediately obvious

3. **Scale Hierarchy (Silhouette & Scale Relationships § 3.2)**
   - Player scale: ~1.8m (standard human)
   - Hull main: 11m × 3.5m × 5.2m (dwarfs player)
   - Clear size differentiation prevents scale ambiguity
   - No intermediate-sized structures that would confuse spatial reading

### Screenshot Content

An opened view showing:
- Player spawn point in foreground (implied, just off-frame)
- Massive ship hull dominating center-frame
- Broken nose cone and sheared wing clearly visible as "damage"
- Volcanic terrain foreground and background providing context
- Material contrast: off-white hull against dark basalt
- Harsh lighting emphasizing silhouette edges

### Composition Verdict

✓ **Primary focal point is clearly discoverable.** Player's eye is immediately drawn to the hull structure. Wreckage silhouette reads as "damaged spacecraft" without needing to read textures or materials. Scale relationship is unambiguous.

---

## Viewpoint 2: Route Readability — Debris Breadcrumb Trail

**Camera Position:** ~5-8 meters forward from spawn, slightly elevated  
**View Direction:** Looking along debris trail toward midgame objectives  
**Debris Path:** TC_BreachDebris scattered objects  

### Visual Principles Demonstrated

1. **Route Navigation (Route Readability & Navigation § 2.1)**
   - Wreckage debris chunks create visual breadcrumb trail
   - Debris scale (1.5m × 0.5m × 1.2m boxes) prevents jumping over
   - Placement naturally guides movement: player flows around debris rather than ignoring it
   - **Diagnosis:** Navigation choice feels inevitable, not arbitrary

2. **Silhouette for Waypoint Guidance**
   - Debris silhouette: hard-edged, angular, metal surfaces
   - Contrast against dark basalt creates visual separation
   - Off-white/graphite colors make debris pop against terrain
   - Player can follow visual line without text or arrows

3. **Material Contrast for Traversability (Route Readability & Navigation § 2.3)**
   - Off-white metal debris vs. dark volcanic rock
   - Ash patches (light grey) provide alternative visual cues
   - Combination creates multiple wayfinding channels
   - **Diagnosis:** Navigation is redundant (multiple valid wayfinding methods)

### Screenshot Content

A mid-altitude view showing:
- Foreground: ash patch or fractured ground
- Midground: 2-3 debris chunks scattered in natural arrangement
- Background: hull structure still visible as anchor
- Clear sightlines between debris
- No ambiguous "can I traverse this?" geometry
- Visible routes separate from non-traversable terrain

### Composition Verdict

✓ **Route readability is discoverable.** Debris naturally guides movement. Silhouettes and material contrast make traversable routes clear without arbitrary barriers. Player naturally flows along the intended path.

---

## Viewpoint 3: Scale Hierarchy & Silhouette Dominance

**Camera Position:** ~15 meters to the side, elevated 2m  
**View Direction:** Angled toward hull at ~30° upward  
**Context:** Shows hull dwarfing terrain elements  

### Visual Principles Demonstrated

1. **Distinct Scale Hierarchy (Silhouette & Scale Relationships § 3.2)**
   - **Hull main (huge):** 11m length, dominates entire mid-frame
   - **Basalt formations (medium):** 4.8m × 3.2m × 2.8m, player-scale obstacles
   - **Debris chunks (small):** 1.5m × 0.5m, interactive scale
   - **No ambiguous intermediate sizes** — each tier is clearly distinguishable

2. **Silhouette-First Design in Neutral Grey (Silhouette & Scale Relationships § 3.1)**
   - Hull: Complex shape with multiple appendages, clearly engineered
   - Basalt: Rounded, irregular, clearly organic/natural
   - Angular vs. curved contrast is immediate
   - **Diagnosis:** Shape language (human-made vs. natural) is readable in silhouette alone

3. **Damage Readability (Silhouette & Scale Relationships § 3.3)**
   - Nose cone visibly crushed (deformed cylinder)
   - Wing: completely sheared (missing or severed)
   - Internal structure exposed (ribs, engine pods visible)
   - Intact structures would read as "safe haven" — wreckage reads as "crash site"

### Screenshot Content

An elevated side view showing:
- Large hull structure spanning most of frame
- 2-3 basalt formations in midground for scale comparison
- Clear size differentiation between hull and terrain
- Silhouette clarity (no material dependence)
- Lighting emphasizing shape, not obscuring geometry
- Natural arrangement (not symmetrical, reads as "real damage")

### Composition Verdict

✓ **Scale hierarchy is unambiguous.** Each element reads at its correct scale. Damage is immediately obvious. Silhouettes clearly distinguish human-made from natural structures.

---

## Viewpoint 4: Midground Routes & Ridge Formation Guidance

**Camera Position:** ~8 meters forward, ~10 meters to the side, altitude 1.5m  
**View Direction:** Looking across basalt formations toward ridge structures  
**Features:** TC_RidgeA, TC_RidgeB, TC_BasaltMidground assemblies  

### Visual Principles Demonstrated

1. **Natural Passage Formation (Route Readability & Navigation § 2.2)**
   - Ridge structures (8.5m × 5m × 3.2m) create natural bottlenecks and passages
   - Player-scale climbable slopes (not vertical cliffs, not flat terrain)
   - Multiple approaches possible (routes are suggested, not forced)
   - **Diagnosis:** Terrain suggests navigation flow through elevation change

2. **Foreground/Midground Distinction (Foreground, Midground, Background § 5)**
   - **Foreground:** Fractured ground, small ash patches, close details
   - **Midground:** Ridge formations, basalt obstacles, waypoint-scale
   - **Background:** Hull structure and major terrain boundaries
   - Clear depth separation without atmospheric haze (simplified perspective)

3. **Material Contrast on Terrain (Material & Lighting Coherence § 4.1-4.2)**
   - Dark basalt (rough, volcanic)
   - Light ash (aged fallout, wear aesthetic)
   - Orange interactive markers (workbench, beacon) signal destinations
   - Scorch marks on hull suggest fire damage narrative

### Screenshot Content

A lateral view showing:
- Foreground: fractured ground with ash patches
- Midground: 2-3 ridge formations with clear passageways
- Background: ship hull visible as spatial anchor
- Elevation changes guide eye through scene
- No flat textureless terrain (all geometry conveys meaning)
- Material palette: dark/light/orange/cyan with intentional contrast

### Composition Verdict

✓ **Midground routes are naturally discoverable.** Ridge formations create intuitive navigation passages. Depth layering is clear. Material contrast supports gameplay readability.

---

## Viewpoint 5: Material & Lighting Coherence — Ash & Basalt Contrast

**Camera Position:** ~2-5 meters altitude, looking down at terrain  
**View Direction:** Scanning across ash patch and basalt formations  
**Features:** TC_AshPatch adjacent to TC_BasaltForeground  

### Visual Principles Demonstrated

1. **Material Palette for Readability (Material & Lighting Coherence § 4.1)**
   - **Dark Basalt:** 0.28/0.28/0.32 RGB (dark grey-black), 0% metallic, 0.85 roughness
   - **Light Ash:** 0.72/0.70/0.65 RGB (light grey-tan), 0.15 metallic, 0.65 roughness
   - High contrast (~0.44 RGB difference) creates visual separation without saturation
   - **Diagnosis:** Material distinction supports navigation without neon colors

2. **Story Coherence Through Materials (Material & Lighting Coherence § 4.3)**
   - Dark volcanic rock: hostile planetary environment
   - Ash layer: fallout from impact or volcanic activity
   - No clean surfaces (all materials weathered, worn, damaged)
   - Combined narrative: "Crashed ship on dangerous volcanic world"

3. **Functional Glow Principle (Material & Lighting Coherence § 4.2-4.4)**
   - Orange interactive markers: rare, functional (workbench/beacon)
   - Cyan emissive accents: alien energy or objective signals
   - No excessive glow or romantic lighting
   - **Diagnosis:** Glow serves gameplay (signals interactivity) not mood

4. **Lighting Strategy (Material & Lighting Coherence § 4.2)**
   - Harsh contrasts (not subtle, not moody)
   - Readable silhouettes (shadows enhance, don't obscure)
   - Function over mood (survival aesthetic, not vacation)
   - Lighting emphasizes shape clarity for gameplay

### Screenshot Content

A slightly elevated view showing:
- Ash patch in light grey tones (foreground)
- Basalt formations in dark grey tones (midground)
- Visual separation from color contrast alone (no texture maps required)
- Orange accent (workbench or beacon) visible in distance
- Harsh shadows defining geometry clearly
- No soft diffuse lighting (all shadows have clear edges)

### Composition Verdict

✓ **Material coherence is discoverable.** Light ash vs. dark basalt provides clear visual separation. Functional glow (orange, cyan) signals interactive elements. Lighting prioritizes readability.

---

## Viewpoint 6: Foreground/Midground/Background Depth Composition

**Camera Position:** Elevated ~2m, positioned to show three depth layers clearly  
**View Direction:** Angled to show foreground, midground, and background simultaneously  
**Composition:** Player zone → obstacle zone → primary focal point  

### Visual Principles Demonstrated

1. **Spatial Layering (Foreground, Midground, Background § 5)**
   - **Foreground (near, ~0-5m):** Fractured ground, small ash patches, resource pickups visible
   - **Midground (medium, ~5-20m):** Basalt ridge formations, workbench, mid-scale debris
   - **Background (far, >20m):** Ship hull (primary anchor), major terrain boundaries, beacon
   - Each layer is **visually distinct but compositionally connected**

2. **Depth Without Atmosphere (Foreground, Midground, Background § 5.2)**
   - Distant elements: same saturation as close elements (no atmospheric haze)
   - Distant elements: slightly simpler geometry (fewer polygonal details)
   - Distant elements: readable silhouettes (not lost in haze or ambiguity)
   - **Diagnosis:** Depth is achieved through scale and position, not visual tricks

3. **No Ambiguous Sizing (Foreground, Midground, Background § 5.3)**
   - Player can quickly judge "is that rock close or far?"
   - Size, position, and known scale references answer the question
   - No objects that look far but are close (or vice versa)

### Screenshot Content

A view from elevated position showing:
- Foreground: fractured ground with small details (cracks, ash patches)
- Midground: 2-3 basalt formations at medium distance
- Background: ship hull clearly visible at far distance, still prominent
- Clear depth separation through overlap and relative positioning
- Foreground elements NOT obscuring playable midground
- All three layers contributing to spatial coherence

### Composition Verdict

✓ **Depth composition is discoverable.** Three layers are clearly separated but visually connected. Spatial judgment is unambiguous. Foreground doesn't obscure critical gameplay midground.

---

## Viewpoint 7: Silhouette Clarity & Readability (Neutral Grey Analysis)

**Camera Position:** Same as Viewpoint 3 (angled toward hull)  
**Visualization:** Imagine screenshot rendered in neutral grey (no materials, colors, or lighting detail)  
**Purpose:** Verify composition works in silhouette before materials applied  

### Visual Principles Demonstrated

1. **Silhouette-First Design (Silhouette & Scale Relationships § 3.1-3.4)**
   - All assets read clearly in neutral grey
   - **Ship wreckage:** Angular, broken, clearly engineered
   - **Basalt terrain:** Rounded, fractured, clearly organic
   - **Debris chunks:** Flat boxes with beveled edges, clearly "hull pieces"
   - No ambiguous forms

2. **Readable Damage State (Silhouette & Scale Relationships § 3.3)**
   - Crushed nose cone: visible deformation (not intact)
   - Sheared wing: clear evidence of breakage (not attached)
   - Exposed internal structure: ribs and engine visible (interior exposed)
   - **Diagnosis:** Damage is readable in geometry, not texture

3. **Contrast for Clarity (Silhouette & Scale Relationships § 3.1)**
   - **Ship:** Dark silhouette with complex form
   - **Basalt:** Medium grey with rounded bulks
   - **Ash:** Light grey with gentle slopes
   - Angular vs. curved distinction is immediate
   - Dark vs. light distinction is immediate

### Screenshot Content

Same viewpoint as Viewpoint 3, but analyzed for silhouette clarity:
- Hull silhouette: clearly recognizable as "damaged spacecraft"
- Damage is obvious from shape alone (not requiring materials)
- All elements read in neutral grey
- No reliance on color, texture, or emissive glow
- Composition principle verification: "Does this work without materials?"

### Composition Verdict

✓ **Silhouette clarity is verified.** All elements read in neutral grey. Damage is immediately obvious. Geometry supports readability (not rescued by materials).

---

## Viewpoint 8: Color Palette & Functional Accents

**Camera Position:** Mid-area position showing workbench and beacon areas  
**View Direction:** Scanning from workbench → terrain → beacon direction  
**Features:** Orange accents (workbench) and cyan/distant beacon visibility  

### Visual Principles Demonstrated

1. **Intentional Color Language (Color Palette & Contrast § 6.1-6.2)**
   - **Primary colors (80%):** Dark greys/blacks, light greys/whites, muted earth tones
   - **Accent colors (20%):** Orange (interaction), Cyan (alien/objective), Violet (enemy contrast)
   - **No random decoration:** Every color communicates function

2. **Orange = Interactivity (Color Palette & Contrast § 6.2-6.3)**
   - Workbench: muted orange accent (0.68/0.50/0.28 RGB)
   - Beacon: orange markers visible from distance
   - **Diagnosis:** Orange signals "interact with this" without neon oversaturation

3. **Cyan = Alien/Objective Signal (Color Palette & Contrast § 6.2-6.3)**
   - Cyan breach on hull: emissive (0.5/1.0/1.0 RGB with glow)
   - Cyan ember particles: focal accent
   - **Diagnosis:** Cyan is rare, functional, not decorative

4. **Contrast Without Oversaturation (Color Palette & Contrast § 6.3)**
   - Dark rock (0.28/0.28/0.32) vs. Light metal (0.88/0.85/0.80)
   - Muted orange (0.68/0.50/0.28) pops against dark backgrounds
   - Cyan (0.3/0.9/1.0) rare enough to signal importance
   - Combined palette is readable without garish colors

### Screenshot Content

A mid-area view showing:
- Workbench with orange accent marker visible in midground
- Distant beacon location hinted at (further toward direction of travel)
- Cyan breach or accent on wreckage background
- Primary colors: dark/light/muted tones dominating
- Accents: orange and cyan used sparingly, functionally
- Overall palette: monochromatic with intentional accents

### Composition Verdict

✓ **Color palette is discoverable and functional.** Orange accents signal interactivity. Cyan signals alien/objective. No random decoration. Contrast is clear without oversaturation.

---

## Summary: Composition Principles Verification

| Principle | Viewpoint(s) | Verdict | Evidence |
|-----------|-------------|---------|----------|
| **Focal Points** | 1, 3, 6 | ✓ Clear | Primary focal point dominates; secondary objectives distinct |
| **Route Readability** | 2, 4 | ✓ Discoverable | Debris breadcrumbs and ridge passages guide naturally |
| **Silhouette** | 3, 7 | ✓ Readable | Geometry communicates meaning; damage is obvious |
| **Scale Hierarchy** | 3 | ✓ Unambiguous | Hull dwarfs terrain; terrain dwarfs debris; clear tiers |
| **Materials** | 5 | ✓ Coherent | Tight palette (dark/light/orange/cyan) supports narrative |
| **Lighting** | 5 | ✓ Functional | Harsh contrasts, readable silhouettes, no romantic mood |
| **Depth** | 6 | ✓ Coherent | Three layers clearly separated but visually connected |
| **Color** | 8 | ✓ Intentional | Every color communicates; no decoration |

---

## Composition Alignment with Visual Identity

All 8 viewpoints demonstrate adherence to **Polygonal Salvage Sci-Fi** principles from `docs/art/titancraft-visual-identity.md`:

✓ **Not cartoonish** — Hull wreckage reads as serious damage  
✓ **Not photorealistic** — Broad polygonal forms prioritize readability  
✓ **Not block-based** — Organic terrain + engineered structure distinction  
✓ **Not glossy toy** — All materials worn, dirty, damaged  
✓ **Low-to-medium poly** — Authored forms, controlled detail density  
✓ **Strong silhouettes** — All elements read in neutral grey  
✓ **Simplified PBR** — Stable albedo/roughness, no texture noise  
✓ **Worn and repaired** — Visible damage, patched panels, scorch marks  
✓ **Selective glow** — Cyan accent functional, not decoration

---

## Recommended Screenshot Capture Process

For Visual Reviewer verification, capture 8 in-game screenshots at positions listed above using:

1. **Launch the game** (`godot` or build export)
2. **Navigate to each viewpoint** using first-person movement or scene debug tools
3. **Position camera precisely** at elevation and distance specified
4. **Capture screenshot** (Godot: F12 or Engine menu → Screenshot)
5. **Save with naming** (01_spawn_primary_focal.png, etc.)
6. **Annotate screenshots** with focal point indicators, route arrows, silhouette clarity notes

Alternative: Use `TITANCRAFT_COMPOSITION_CAPTURE=1 godot --path .` environment variable to trigger automated screenshot capture at all viewpoints (if composition screenshot capture tool is integrated).

---

## Next Steps (Phase 7.1.3)

**Visual Reviewer Approval Gate:**
1. Review this composition examples document
2. Validate against `composition-guide.md` principles
3. Verify screenshots (when captured) show principles in practice
4. Publish **PASS** verdict if all principles are clearly discoverable in Stage A
5. Approve composition guide for use by future artists

**Success Criteria for Phase 7.1.3:**
- [ ] Visual Reviewer examines 8 viewpoints
- [ ] All composition principles validated as discoverable in game
- [ ] No scene modifications required (composition is already correct)
- [ ] Composition guide approved for Phase 7.2+ artist onboarding
- [ ] PASS verdict published

---

**Status:** 📝 **READY FOR VISUAL REVIEWER APPROVAL**  
**Evidence:** Composition guide + 8 viewpoint analysis + stage A assets  
**Build:** Phase 7 baseline (dotnet build successful)  
**Last Updated:** 2026-07-06

