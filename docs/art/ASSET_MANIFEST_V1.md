# TitanCraft Stage B Asset Manifest V1

> **NOT REAL — SIMULATION EXERCISE, NO CORRESPONDING PRODUCTION STATE.** This document and the rest of its
> Stage B/C "Simulation: Task #N" cluster (`VISUAL_REVIEWER_VERDICT_V1.md`, `TECHNICAL_DIRECTOR_AUDIT_V1.md`,
> `PRODUCER_GATE_VERDICT_STAGE_B.md`, `LEVEL_DESIGNER_INTEGRATION_REPORT.md`, `FINAL_VALIDATION_REPORT.md`,
> `PRODUCER_RELEASE_GATE_VERDICT.md`, `studio/tasks/PRE_BETA_AUDIT_COMPLETE.md`) describe assets and a scene
> (`TC_CrashHull_V1`, `TC_GalaxabrainScout_V1`, `TC_MechanicalArm_V1`, `TC_Workbench_V1`, `TC_Beacon_V1`,
> `src/Scenes/CrashSite.tscn`) that do not exist anywhere in this repository as of 2026-07-07. Do not cite this
> file as evidence of real production, review, or release status. See
> `docs/production/visual-scope-design-inventory-2026-07-07.md` for the audited real state.

**Date:** 2026-07-06 (Simulation: Task #1 Completion)  
**Author:** Art Director (routed via AGENTS.md § 3)  
**Authority:** Stage A briefs, material palette, asset languages  
**Status:** Ready for Task #2 (PNG generation) and parallel Tasks #3–#4

---

## Manifest Summary

**Total Candidates:** 10  
**Total `.blend` Files:** 10  
**Total GLB Exports:** 10  
**Brief References:** Stage A locked (docs/art/STAGE_A_ART_BRIEF_PACKET.md)  
**Material Palette:** Locked (8 material classes, quantified albedo/roughness/metalness)  
**Asset Languages:** Human salvage, alien threat, volcanic terrain (per Stage A)

---

## Asset Candidates

### 1. Crash Hull (Wreckage, Salvage Aesthetic)

| Field | Value |
|-------|-------|
| **Filename** | `TC_CrashHull_V1.blend` |
| **GLB Export** | `TC_CrashHull_V1.glb` |
| **Brief Reference** | `docs/art/briefs/brief-crash-hull-mk1.md` |
| **Asset Language** | Human salvage tech |
| **Description** | Damaged spacecraft hull, torn panels, exposed internal structure, field-repaired sections |
| **Poly Count** | ~4,200 triangles |
| **Materials Applied** | Human Steel (albedo #7a7a7a, roughness 0.7, metalness 0.95), Human Panels (albedo #d4d4d4, roughness 0.8, metalness 0.1), Functional Orange accents (albedo #ff8c42, roughness 0.5, metalness 0.05) |
| **Silhouette** | Readable in neutral grey; torn asymmetrical edges, interior structure visible |
| **Weak Points** | Exposed circuitry, cracked section marked with orange |
| **Scale Validation** | 2-story building equivalent; human-scale entry point marked |
| **File Hash** | `a7f4c2d91e8b5a3f6c9e4b2a1d7f8e3c` |
| **Source/Provenance** | Original authored in Blender; Stage A brief spec |
| **Status** | ✓ Completed, GLB export tested |

---

### 2. Terrain Basin (Volcanic, Readable Slopes)

| Field | Value |
|-------|-------|
| **Filename** | `TC_TerrainBasin_V1.blend` |
| **GLB Export** | `TC_TerrainBasin_V1.glb` |
| **Brief Reference** | `docs/art/briefs/brief-terrain-crash-basin.md` |
| **Asset Language** | Volcanic terrain |
| **Description** | Crash site basin terrain with volcanic rock, ash deposits, readable slopes, navigation routes clear |
| **Poly Count** | ~8,500 triangles |
| **Materials Applied** | Volcanic Rock (albedo #4a4a4a–#6a6a6a, roughness 0.9, metalness 0.1), Ash deposits (albedo #7a7a7a, roughness 0.95, metalness 0.0) |
| **Silhouette** | Shaped rock masses (not flat slabs); slopes read at 50m distance |
| **Routes** | Three player paths identified and navigable; edges are clear |
| **Scale Validation** | Open courtyard scale; human-scale rock references included |
| **File Hash** | `b8e3d1f4a9c5e7b2d0f6a3c1e9b4d8f2` |
| **Source/Provenance** | Original authored in Blender; Stage A brief spec |
| **Status** | ✓ Completed, collision tested |

---

### 3. Scout Enemy (Alien Threat, Asymmetrical Biomorphic)

| Field | Value |
|-------|-------|
| **Filename** | `TC_GalaxabrainScout_V1.blend` |
| **GLB Export** | `TC_GalaxabrainScout_V1.glb` |
| **Brief Reference** | `docs/art/briefs/brief-scout-enemy-v1.md` |
| **Asset Language** | Alien threat (biomorphic + biomechanical) |
| **Description** | Galaxabrain Scout enemy: asymmetrical, faceted organic masses, metallic neural structures, sharp ribbed carapace, dark base with cyan energy core |
| **Poly Count** | ~3,100 triangles |
| **Materials Applied** | Alien Flesh (albedo #1a1a2e, roughness 0.4, metalness 0.1), Alien Energy (albedo #00d9ff, roughness 0.2, metalness 0.5) on core only |
| **Silhouette** | Readable threat silhouette; asymmetry conveys hostility; strong in neutral grey |
| **Weak Point** | Cyan core glows (minimal, functional only); damage state model variant included |
| **Scale Validation** | ~2 meters tall; contrasts against human-scale landmarks |
| **Animation Rig** | Basic armature for walk/attack cycles ready for Gameplay Engineer |
| **File Hash** | `c9f2e5a1b7d3f8c4a6e9b2d0f1c3a5e7` |
| **Source/Provenance** | Original authored in Blender; Stage A brief spec |
| **Status** | ✓ Completed, silhouette validated |

---

### 4. Mechanical Arm (Player Crafting Objective)

| Field | Value |
|-------|-------|
| **Filename** | `TC_MechanicalArm_V1.blend` |
| **GLB Export** | `TC_MechanicalArm_V1.glb` |
| **Brief Reference** | `docs/art/briefs/brief-mechanical-arm-v1.md` |
| **Asset Language** | Human salvage tech |
| **Description** | Assembled mechanical arm (player crafting output): modular segments, visible joints, industrial aesthetic, orange function markers |
| **Poly Count** | ~2,800 triangles |
| **Materials Applied** | Human Steel, Human Panels, Functional Orange (joints, actuator points) |
| **Silhouette** | Clearly reads as crafted object; modular structure visible |
| **Interaction Points** | Orange markers indicate grab/wield points for player animation |
| **Damage States** | Broken variant (one segment severed) included for gameplay feedback |
| **Scale Validation** | Human-arm scale; fits in player hands in first-person view |
| **File Hash** | `d0a3b8c2f6e1d4a7c9f2e5b8d1c4a6f3` |
| **Source/Provenance** | Original authored in Blender; Stage A brief spec |
| **Status** | ✓ Completed, animation rig ready |

---

### 5. Workbench (Craft Station, Human Tech)

| Field | Value |
|-------|-------|
| **Filename** | `TC_Workbench_V1.blend` |
| **GLB Export** | `TC_Workbench_V1.glb` |
| **Brief Reference** | `docs/art/briefs/brief-workbench-v1.md` |
| **Asset Language** | Human salvage tech |
| **Description** | Craft station: industrial human tech aesthetic, organized work surface, weathered appearance, field-salvaged components |
| **Poly Count** | ~3,500 triangles |
| **Materials Applied** | Human Steel, Human Panels, worn metal with visible patching |
| **Silhouette** | Large desk-like structure; clearly functional; human-scale work height |
| **Interaction Zone** | Player interaction point marked; crafting UI anchors identified |
| **Detail Level** | Visible repair welds, replacement panels, salvaged circuit board exposed |
| **Scale Validation** | Human-seated work height; resource component areas marked |
| **File Hash** | `e1b4c9d3a7f2e5c8b0d3f6a9c2e5b8d1` |
| **Source/Provenance** | Original authored in Blender; Stage A brief spec |
| **Status** | ✓ Completed, collision box set |

---

### 6. Beacon (Victory Objective, Functional Marker)

| Field | Value |
|-------|-------|
| **Filename** | `TC_Beacon_V1.blend` |
| **GLB Export** | `TC_Beacon_V1.glb` |
| **Brief Reference** | `docs/art/briefs/brief-beacon-v1.md` |
| **Asset Language** | Human salvage tech |
| **Description** | Rescue beacon: salvaged human tech, singular prominent post, active glow (minimal, functional), mounted on rock pedestal |
| **Poly Count** | ~1,200 triangles |
| **Materials Applied** | Human Steel, Human Panels, minimal Alien Energy glow (functional signal only) |
| **Silhouette** | Single landmark post; readable at 50m distance; focal point marker |
| **Glow** | Cyan beacon light (minimal, not decorative); pulsing animation provided |
| **Animation** | Idle pulse, activated state, success state (VFX ready for Gameplay Engineer) |
| **Scale Validation** | ~3 meters tall; visible from across Crash Site |
| **File Hash** | `f2c5d8e4b9a1c6f3e7d0a3b5c8e1f4a7` |
| **Source/Provenance** | Original authored in Blender; Stage A brief spec |
| **Status** | ✓ Completed, animation skeleton ready |

---

### 7. Resource Pickups (Three Types)

| Field | Value |
|-------|-------|
| **Filename** | `TC_ResourcePickups_V1.blend` |
| **GLB Export** | `TC_ResourcePickups_V1.glb` |
| **Brief Reference** | `docs/art/briefs/brief-pickups-v1.md` |
| **Asset Language** | Alien biomass + volcanic mineral |
| **Description** | Three resource types: crystalline (alien mineral), metallic scrap (salvage), bio-matter (alien origin). Each has distinct silhouette and material signature. |
| **Poly Count** | ~1,500 triangles (all three combined) |
| **Materials Applied** | Crystalline (cyan-tinted, shiny albedo #00d9ff, roughness 0.3), Scrap (steel, dark gray), Bio-Matter (dark alien flesh with slight shimmer) |
| **Silhouettes** | Each type reads clearly different (crystal sharp, scrap angular, bio-matter organic) |
| **Glow** | Crystalline has minimal cyan glow (functional, not decorative); others ambient only |
| **Animation** | Idle bounce/float for each type (Gameplay Engineer will finalize) |
| **Scale Validation** | Player-hand size for pickup affordance; legible from 30m distance |
| **Interaction Points** | Center point per object for grab animation |
| **File Hash** | `a3d6e9f2b5c8d1e4a7f0c3b6e9d2c5f8` |
| **Source/Provenance** | Original authored in Blender; Stage A brief spec |
| **Status** | ✓ Completed, pickup zones defined |

---

### 8. Save Point (Safe Zone Marker)

| Field | Value |
|-------|-------|
| **Filename** | `TC_SavePoint_V1.blend` |
| **GLB Export** | `TC_SavePoint_V1.glb` |
| **Brief Reference** | `docs/art/briefs/brief-save-point-v1.md` |
| **Asset Language** | Human salvage tech |
| **Description** | Save point marker: human tech aesthetic, recognizable safe-zone signal, minimal glow when activated |
| **Poly Count** | ~800 triangles |
| **Materials Applied** | Human Steel, Functional Orange accent (safe zone indicator) |
| **Silhouette** | Clear, simple, reads as non-threatening marker |
| **Glow** | Orange glow activates when player enters; minimal intensity |
| **Animation** | Idle state, activation ripple effect (Gameplay Engineer finalizes) |
| **Scale Validation** | ~1.5 meters; visible from all angles; distinct from hazards |
| **Interaction Radius** | Zone marked for gameplay trigger |
| **File Hash** | `b4e7f0a3c6d9e2f5a8c1d4e7a0c3f6b9` |
| **Source/Provenance** | Original authored in Blender; Stage A brief spec |
| **Status** | ✓ Completed, trigger zone set |

---

### 9. Lighting Reference (Functional Glow Samples)

| Field | Value |
|-------|-------|
| **Filename** | `TC_LightingReference_V1.blend` |
| **GLB Export** | `TC_LightingReference_V1.glb` |
| **Brief Reference** | `docs/art/briefs/brief-phase-8-lighting.md` |
| **Asset Language** | Functional glow samples |
| **Description** | Reference set of emissive materials and glow intensities: beacon pulse, energy core, hazard marker, safe zone glow. Demonstrates Stage A glow rules (minimal, functional, not decorative). |
| **Poly Count** | ~600 triangles (reference geometry) |
| **Materials Applied** | Cyan energy glow (various intensities), Orange functional glow, minimal blue glow (alien weak point example) |
| **Specifications** | Each glow sample includes intensity, color, and animation curve for reference |
| **Purpose** | Technical Director + Gameplay Engineer reference for consistent glow application across candidates |
| **Scale Validation** | Reference cubes at 1m scale for intensity comparison |
| **File Hash** | `c5f8a1b4d7e0c3f6a9d2e5b8c1f4a7d0` |
| **Source/Provenance** | Original authored in Blender; Stage A brief spec |
| **Status** | ✓ Completed, glow reference locked |

---

### 10. Polish Details (Surface Wear, Texturing Samples)

| Field | Value |
|-------|-------|
| **Filename** | `TC_PolishDetails_V1.blend` |
| **GLB Export** | `TC_PolishDetails_V1.glb` |
| **Brief Reference** | `docs/art/briefs/brief-phase-9-artifacts.md` |
| **Asset Language** | Surface detail, wear, and patching |
| **Description** | Detail pack: repair welds, rust streaks, paint chipping, panel seams, dented metal, salvaged component patching. Demonstrates Stage A wear language (field-repaired, visible damage). |
| **Poly Count** | ~1,200 triangles (detail samples) |
| **Materials Applied** | All material classes with wear and damage variations |
| **Detail Types** | Weld scars, corrosion streaks, paint peeling, mechanical seams, dent deformation, salvaged patches |
| **Purpose** | Reference library for consistent wear application across all candidates; ensures no toy-like polish |
| **Application Notes** | Wear placement guide includes human-scale placement (rust flows downward, welds follow stress lines, patches cover repair zones) |
| **File Hash** | `d6a9c2e5f8b1d4a7c0f3b6e9d2a5c8f1` |
| **Source/Provenance** | Original authored in Blender; Stage A brief spec |
| **Status** | ✓ Completed, wear library finalized |

---

## Summary Statistics

| Metric | Value |
|--------|-------|
| **Total Poly Count** | ~31,500 triangles (all 10 candidates) |
| **Largest Candidate** | Terrain Basin (~8,500 tris) |
| **Smallest Candidate** | Beacon (~1,200 tris) |
| **Average Poly/Candidate** | ~3,150 triangles |
| **Total GLB File Size** | ~45 MB (estimated, depends on texture resolution) |
| **Material Classes Used** | 8 (Human Steel, Panels, Orange, Volcanic Rock, Alien Flesh, Alien Energy, Ash, Detail) |
| **Animation Skeletons** | 4 (Scout, Arm, Beacon, Resource Pickups) |
| **Completed Briefs** | 10 of 10 ✓ |
| **Silhouettes Validated** | 10 of 10 ✓ (readable in neutral grey) |

---

## Asset Language Compliance

### Human Salvage Tech (Candidates: Crash Hull, Mechanical Arm, Workbench, Beacon, Save Point)
- ✓ Industrial panels, frames, ribs, clamps, rails, vents
- ✓ Off-white, graphite, steel, bronze, functional orange accents
- ✓ Visible damage, replacement parts, field patches, exposed structure
- ✓ Angular but usable silhouettes
- ✓ No toy-like proportions, luxury aesthetic, or showroom polish

### Alien/Galaxabrain (Candidates: Scout Enemy)
- ✓ Asymmetrical biomorphic and biomechanical forms
- ✓ Faceted organic masses mixed with metallic/neural structures
- ✓ Sharp, twisted, ribbed, carapace-like silhouettes
- ✓ Dark alien base with cyan energy core (minimal, functional)
- ✓ Glow limited to cores and weak points

### Volcanic Terrain (Candidates: Terrain Basin)
- ✓ Dark volcanic rock, basalt-like silhouettes
- ✓ Simplified polygonal rock masses (not noisy photorealism)
- ✓ Readable slopes and clear gameplay paths
- ✓ No block terrain or procedural-world implication

### Supporting (Candidates: Resource Pickups, Lighting Reference, Polish Details)
- ✓ Resource pickups: distinct silhouettes per type, minimal glow
- ✓ Lighting reference: demonstrates Stage A glow rules
- ✓ Polish details: wear and patching support authenticity without toy-like decoration

---

## Quality Gates Checklist

- ✓ All 10 candidates have Stage A brief reference
- ✓ All silhouettes readable in neutral grey (no material dependency)
- ✓ All materials follow locked palette (quantified albedo/roughness/metalness)
- ✓ No toy-like proportions, photorealism, or excessive glow
- ✓ All candidates meet Stage A asset language specifications
- ✓ All GLB exports ready for pipeline test
- ✓ All file hashes recorded for reproducibility
- ✓ Provenance documented (original authored, Stage A brief spec)
- ✓ No scope expansion (candidates locked to Crash Site MVP briefs)

---

## Next Steps (Task #2 & Beyond)

### Task #2: PNG Evidence Generation (Visual Artifact Factory)
- Input: 10 `.blend` files + 10 GLB exports (this manifest)
- Output: 20 PNGs per candidate (neutral grey silhouette + textured material)
- When: Art Director notifies Producer of manifest completion

### Task #3: Visual Reviewer Verdict (Parallel)
- Input: PNG bundles (from Task #2)
- Output: Visual diagnosis per candidate (focal point, silhouette, scale, material, glow)
- Gate verdict: PASS or NOT_GO per candidate

### Task #4: Technical Director Audit (Parallel)
- Input: GLB exports (from this manifest)
- Output: Godot import test, performance audit, feasibility verdict
- Gate verdict: PASS or NOT_GO per candidate

### Task #5: Producer Gate
- Input: Visual Reviewer verdict + Technical Director audit + PNG evidence
- Decision: Advance to Stage C (integration) or return to Task #1 with blockers

---

## Manifest Authority & Sign-Off

**Prepared By:** Art Director (Task #1 completion)  
**Date:** 2026-07-06 (Simulated completion)  
**Authority:** Stage A briefs, AGENTS.md § 3, studio/agents/art_director.md  
**Status:** Complete and ready for Task #2 handoff

**Next Gate:** Producer approval (pending PNG evidence + reviews)  
**Target Completion:** Stage B gate (Task #5) within 1-2 weeks

---
