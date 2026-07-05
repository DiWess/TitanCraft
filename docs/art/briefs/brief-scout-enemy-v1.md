# Asset Brief: Scout Enemy V1
## Phase 5 Antagonist — Procedural Arena Threat

**Brief Owner:** TitanCraft Project Director  
**Target Asset:** Single enemy type—GalaxaBrain Scout reconnaissance unit  
**Scope:** One functional enemy character for MVP arena gameplay  
**Date:** 2026-07-05  
**Design Philosophy:** Organic-mechanical hybrid threat; smaller, faster, intelligent hunting pattern

---

## Visual Thesis

The Scout is **the primary arena antagonist**. It must read as:

- **Distinct enemy form** — clearly different from player avatar and props
- **Organic-mechanical hybrid** — biomechanical predator, not purely robotic
- **Fast, nimble threat** — silhouette suggests speed and agility (thin, lightweight)
- **Intelligent hunter** — body language implies awareness and tactical behavior
- **Visually menacing** — hostile aesthetic without grotesque gore
- **Distance-readable** — identifiable as "enemy" from 15+ meters away
- **Non-humanoid** — avoids confusing with player or NPC allies (MVP has no allies)

The Scout signals **"hostile threat incoming"** purely through form and stance, without text or visual HUD markers.

---

## Scout Anatomy & Silhouette

### Body Plan

**Overall Form:**
- Central torso (alien, bulbous, biomorphic)
- Four articulated legs (insectoid, rapid movement)
- Two arm-like appendages with sensing/weapon capability
- Head unit with optical sensors (glowing threat indicator)
- Compact, coiled posture (suggests ready-to-leap athleticism)

**Size & Scale:**
- Height: ~1.2–1.4m (smaller than player ~1.8m, but present and menacing)
- Width: ~0.6m at widest (compact, agile)
- Stance: Low center of gravity (four legs on ground, hunched forward)
- Implied speed: Slim, efficient form suggests 40+ km/h sprinting capability

### Design Priorities

**Avoid:**
- ❌ Humanoid form (confuses with player or NPCs)
- ❌ Cute or cartoonish appearance (diminishes threat)
- ❌ Overly ornate or decorative (suggests passive, not active threat)
- ❌ Purely mechanical appearance (disconnects from organic threat language)
- ❌ Transparent or see-through aesthetic (feels unsubstantial)

**Target:**
- ✅ Insectoid or cephalopod-inspired (alien, unsettling)
- ✅ Organic-mechanical integration (armor plating over muscle/chitin)
- ✅ Asymmetrical or irregular form (natural evolution, not designed)
- ✅ Predatory stance (aggressive, hunting posture)
- ✅ Visible threat: optical sensors, weapon-like appendages, armor
- ✅ Mid-scale threat (smaller than boss, larger than swarms)

---

## Structural Breakdown

### 1. Torso/Body Core

- Bulbous central mass (biomorphic, alien)
- Chitinous or armored plating (organic-mechanical plates)
- Visible seams between armor segments (suggests articulation)
- Asymmetrical scarring or battle damage (indicates combat history)
- Slight luminescence in joints or armor gaps (suggests internal energy)

**Poly allocation:** ~800–1,000 polys (substantial core form)

### 2. Leg Assembly (Four Legs)

- Each leg: 3–4 articulated segments (thin, efficient)
- Talon or claw feet (predatory, grip terrain)
- Jointed movement implied by geometry (bent, coiled position)
- Spacing: splayed for stability and ground coverage
- Material: Glossy organic-armor (chitin-like)

**Poly allocation:** ~600–800 polys total (200 per leg)

### 3. Arm/Appendage Assembly (Two)

- Upper appendages with weapon-like or sensing capability
- Possibly tipped with appendages (claws, sensing organs, or energy projector)
- Curved or spiked form (aggressive, not neutral)
- Articulated at shoulder, elbow-equivalent (full movement range)
- Material: Darker armor, possibly with organic accent

**Poly allocation:** ~400–500 polys total (200 per arm)

### 4. Head/Sensor Unit

- Compact head, distinct from torso
- Prominent optical sensors (glowing eyes or compound sensor array)
- Possibly mandibles or mouth-like aperture
- Sensing organs or antenna-like protrusions
- Glow color: Amber, red, or cyan (menacing, not friendly blue-green)

**Poly allocation:** ~300–400 polys (detailed sensor/face geometry)

### 5. Armor Plating & Detailing

- Segmented armor across body
- Visible seams, rivets, or organic growth lines
- Battle damage: dents, scratches, scorch marks
- Asymmetry (not perfectly bilateral)
- Possible organic growths or bio-luminescent patches

**Poly allocation:** ~200–300 polys (edge and seam detailing)

---

## Poly Count & Complexity

| Component | Target Poly Budget | Detail Level |
|-----------|-------------------|--------------|
| **Torso/Core** | ~900 | Medium: armor plating, seams, articulation hints |
| **Legs (4×)** | ~700 | Low–Medium: clean segments, claws, joints |
| **Arms/Appendages (2×)** | ~450 | Medium: curved form, sensory/weapon tips |
| **Head/Sensors** | ~350 | Medium: optical details, mouthparts |
| **Armor & Detail** | ~250 | Low: edge geometry, damage marks |
| **TOTAL** | **~2,650** | Budget: ≤3,200 (healthy margin for polish) |

---

## Material & Color Specification

### Primary Armor (Organic-Chitin)
- **Albedo:** Dark brown-gray (RGB ~80, 70, 60)
- **Roughness:** ~0.65 (chitinous, matte with slight sheen)
- **Metalness:** 0.2 (organic armor, slight metal content from embedded tech)
- **Coverage:** ~60%
- **Purpose:** Primary body form, menacing dark appearance

### Accent Plating (Industrial Metal)
- **Albedo:** Steel gray (RGB ~120, 120, 130)
- **Roughness:** ~0.6 (oxidized metal, worn)
- **Metalness:** 0.4 (bare/treated steel, armor reinforcement)
- **Coverage:** ~20%
- **Purpose:** Segmented armor, joint reinforcements, weapon-like edges

### Optical Sensors (Threat Glow)
- **Albedo:** Bright amber or red (RGB ~240, 150, 50 or ~220, 80, 60)
- **Emissive Strength:** 1.5–2.0 (obvious threat indicator, visible even in daylight)
- **Roughness:** ~0.2 (lens-like, reflective)
- **Metalness:** 0
- **Coverage:** ~5% (eyes, sensor array)
- **Purpose:** Hostile indicator, draws player attention, suggests awareness

### Shadow/Damage Accent (Scorch)
- **Albedo:** Very dark brown-black (RGB ~30, 25, 20)
- **Roughness:** ~0.85 (charred, scorched)
- **Metalness:** 0
- **Coverage:** ~15% (battle damage, scorch marks)
- **Purpose:** Battle history, menacing aesthetic

---

## Behavior Hints (Visual Language)

The mesh must *suggest* combat capability and intelligence without animation:

- **Stance:** Coiled, forward-leaning (ready to pounce)
- **Limb positioning:** Legs bent, arms slightly raised (active posture)
- **Asymmetry:** Uneven damage or wear (fighting history)
- **Optical presence:** Sensors prominent and obvious (watching, aware)
- **Edges and spikes:** Sharp transitions, pointed features (predatory)

Godot will animate movement; the mesh establishes static ready-pose.

---

## Validation Checklist

**Before export (Blender):**
- [ ] Silhouette reads "hostile alien threat" (not cute, not mechanical, not humanoid)
- [ ] Four legs are visible and distinct (insectoid form clear)
- [ ] Arms/appendages suggest weapon or sensing capability
- [ ] Head unit distinct with prominent optical sensors
- [ ] Optical glow color chosen (amber or red, not blue/cyan)
- [ ] Armor plating visible (not smooth blob)
- [ ] Asymmetry present (battle-worn, not pristine)
- [ ] All material zones defined and named
- [ ] No overlapping faces or internal geometry
- [ ] Poly count ≤ 3,200

**After export to GLB:**
- [ ] File size reasonable (~3–5 MB)
- [ ] Godot imports without errors
- [ ] Optical sensor glow exports correctly
- [ ] Silhouette matches brief (alien, menacing, compact)

**In first-person gameplay (15+ meters away):**
- [ ] Player can identify as "enemy" from distance
- [ ] Optical glow is visible and threatening
- [ ] Four-legged form is readable (not confused with props)
- [ ] Scale feels like formidable threat (smaller than boss, not trivial)
- [ ] Stance suggests active threat (not passive furniture)

**Visual readability:**
- [ ] Scout does not read as friendly or neutral
- [ ] Organic-mechanical aesthetic is clear
- [ ] Threat level is obvious without text or UI
- [ ] Silhouette distinct from workbench, beacon, save point, pickups

---

## Success Criteria

✅ **Scout is `ASSET_IMPLEMENTATION_PASS` when:**

1. Asset exported as GLB with valid manifest entry
2. Silhouette reads **alien insectoid predator** (four-legged, organic-mechanical hybrid)
3. Four legs visible and clearly articulated (suggests rapid movement)
4. Arms/appendages recognizable as sensory or weapon (suggests threat capability)
5. Optical sensor glow visible and menacing (amber or red, emissive)
6. Armor plating obvious (not smooth blob)
7. Asymmetrical damage or wear visible (battle-worn aesthetic)
8. Material palette coherent (dark organic armor, gray metal accent, menacing glow)
9. Poly count ≤ 3,200
10. No visual corruption, texture stretching, or floating geometry
11. Optical glow exports and displays correctly
12. Turntable PNG captured from predatory angle (side/front view showing threat stance, 2–3 angles)
13. SHA256 hash recorded in asset manifest
14. **Visibility test passed:** Identifiable as enemy from 15+ meters (first-person view)

❌ **Scout will be `NOT_GO` if:**

- Silhouette reads cute, friendly, humanoid, or purely mechanical
- Legs ambiguous or humanoid (breaks insectoid identity)
- Optical glow insufficient or missing (weakens threat language)
- Armor plating absent (feels unsubstantial)
- Poly count exceeds 3,500
- Player cannot identify as hostile threat at 15m distance
- Material palette contradicts menacing aesthetic

---

## Gameplay Integration (Phase 7 Composition & Later)

The Scout will be placed in arena combat zones after composition. Placement and behavior:

- **Location:** Hidden in arena rock formations, spawn zones out of initial player view
- **Spawn count:** 1–3 scouts active simultaneously (MVP single-enemy combat focus)
- **Behavior:** Godot scripting handles AI (pathfinding, chase, attack patterns)
- **Collision:** Dynamic (Scout moves, collides with terrain and player)
- **Threat level:** Moderate difficulty (not boss-tier, not trivial)
- **Audio:** Separate Godot layer (hiss, roar, impact sounds)
- **Destruction:** Godot handles defeat/despawn (no mesh animation needed)

---

## Deliverables

| Deliverable | Format | Location |
|---|---|---|
| Scout Brief (this doc) | `.md` | `docs/art/briefs/brief-scout-enemy-v1.md` |
| Blender Source | `.blend` | `assets/Source/Blender/Production/MVP_Pack_V1/TC_CHAR_GalaxabrainScout_V1.blend` |
| GLB Export | `.glb` | `assets/Production/Generated/MVP_Pack_V1/TC_CHAR_GalaxabrainScout_V1.glb` |
| Manifest Entry | JSON | `assets/Production/Generated/asset_manifest.json` |
| Review PNG (2–3 angles) | `.png` | `artifacts/asset-review/TC_CHAR_GalaxabrainScout_V1/` |

---

## Approval Gate

**Brief Status:** `ASSET_BRIEF_READY`  
**Next Gate:** `ASSET_IMPLEMENTATION_PASS` (after Blender refinement/export and threat visibility test)  
**Final Gate:** `VISUAL_SLICE_GAMEPLAY_SAFE` (after composition integration and combat behavior validation in Phase 7)  
**Authority:** Project Director + Distance threat visibility test (15m+ FPS) + Alien/menacing aesthetic review
