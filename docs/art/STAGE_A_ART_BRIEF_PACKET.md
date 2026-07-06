# Stage A Art Brief Packet
**Date:** 2026-07-06  
**Primary Agent:** Art Director  
**Gate:** Producer (visual identity approval before Stage B asset production)  
**Status:** Ready for Producer Gate Review  

---

## 1. Task Summary

Establish and gate-lock the visual identity and environment direction for TitanCraft Crash Site MVP. This packet confirms Stage A deliverables are complete and ready for Producer approval before asset candidate generation (Stage B) begins.

**Gate Authority:** README.md § 5-6, AGENTS.md § 3-4, studio/memory/production_stage_gates.md (MEM-STAGE-008)

---

## 2. Visual Identity: LOCKED

### 2.1 Style Definition
**Document:** `docs/art/titancraft-visual-identity.md`  
**Status:** Canonical, approved  
**Style Name:** Polygonal Salvage Sci-Fi

### 2.2 Core Direction (Verified)
- ✓ **Not cartoonish**: no toy-like proportions, oversized shapes, mascot silhouettes
- ✓ **Not photorealistic**: no high-frequency scans, dense film-detail materials
- ✓ **Not block-based**: no voxels, Minecraft-like grids, destructible blocks
- ✓ **Not glossy toy sci-fi**: no plastic shine, spotless panels, showroom ships
- ✓ **Low-to-medium poly**: authored broad planes, bevels, chunky readable forms
- ✓ **Strong silhouettes**: all candidates must read clearly in neutral grey before materials
- ✓ **Simplified PBR**: rough, readable material groups (not texture noise)
- ✓ **Worn and repaired**: visible damage, salvage, field repairs, exposed structure
- ✓ **Selective glow**: emissive accents only for functional signals

### 2.3 Three Asset Language Families

#### Human Tech (Salvage)
- Industrial panels, frames, ribs, clamps, crates, rails, vents, mechanical hinges
- Off-white, graphite, steel, bronze, functional orange accents
- Visible damage, replacement parts, field patches, stripped panels
- Angular but usable silhouettes implying load paths and repair access
- **Forbidden:** luxury spacecraft, sleek fighters, branded hardware, polished toy kits

#### Alien/Galaxabrain (Threat)
- Asymmetrical biomorphic and biomechanical forms
- Faceted organic masses + metallic/neural structures
- Sharp, twisted, ribbed, carapace-like silhouettes
- Dark alien bases with restrained violet, cyan, or red energy accents
- Glow limited to cores, weak points, active energy lines
- **MVP Constraint:** One Scout type only; no faction expansion

#### Volcanic Terrain (Environment)
- Dark volcanic rock, basalt-like silhouettes, ash, fractured ground
- Simplified polygonal rock masses (not noisy photorealism)
- Limited alien vegetation or crystals (guidance and mood, not clutter)
- Clear gameplay paths, landmarks, objective readability
- **Forbidden:** block terrain, procedural world implication, destructible voxels

### 2.4 Automatic Rejection Patterns (Gate Rules)
Reject any candidate showing:
- Cartoon mascot proportions or toy-like materials
- Photoreal scan style conflicting with low-to-medium poly readability
- Block-based, voxel, Minecraft-like, or destructible-grid language
- Glossy plastic sci-fi, clean showroom ships, excessive neon glow
- Silhouettes that only read after materials, outlines, or labels
- Random cubes glued together instead of authored industrial structure
- Paper-thin panels where crash wreckage needs mass and thickness
- Excessive cyan/violet glow overwhelming functional signals
- Visual claims without PNG review artifacts and human/visual-reviewer verdict

---

## 3. Environment Direction: LOCKED

### 3.1 Crash Site Setting
**Document Authority:** README.md § 3 (narrative context), § 5 (Crash Site MVP definition)

**Setting Summary:**
- Location: Hostile alien planet with volcanic geology
- Time: Immediate post-crash (hours to days)
- Player Status: Stranded astronaut, no functional exoskeleton
- Resources: Wreckage, alien biomass, volcanic materials
- Threats: One Galaxabrain Scout (MVP), environmental hazards
- Goals: Resource gathering → crafting → combat → component recovery → beacon activation

### 3.2 Key Landmarks (Environmental Pillars)
Each landmark must be visually readable, tactically significant, and emotionally reinforcing:

| Landmark | Function | Visual Tone | Scale Ref |
|----------|----------|-------------|-----------|
| **Crash Hull** | Shelter, wreckage origin | Salvage sci-fi, torn panels, interior exposed | 2-3 story building |
| **Terrain Basin** | Resource zone, movement route | Volcanic rock, ash deposits, readable slopes | Open courtyard |
| **Workbench/Craft Station** | Safe mechanical assembly | Human tech, organized but weathered | Large desk-like structure |
| **Scout Arena** | Encounter space, tactical | Open floor, elevated vantage points, routes clear | Town square |
| **Beacon Site** | Victory objective, final zone | Salvaged human tech, active glow (minimal) | Single prominent post |

### 3.3 Composition Principles (Stage A Guidance)
**Document:** `docs/art/composition-guide.md` (linked, requires Phase 6 completion)

**Confirmed Principles:**
- **Focal Point:** Player's eyes drawn to mission objective without UI arrows
- **Route Readability:** Navigable slopes and edges are readable at 50m distance
- **Silhouette:** All terrain and objects read clearly in simple grey
- **Scale Contrast:** Human wreckage vs. alien biomass creates visual narrative
- **Material Coherence:** Worn metal ≠ polished panels; alien flesh ≠ organic confusion
- **Lighting Hierarchy:** Functional glow only (hazard, beacon, weak points); no ambient neon

### 3.4 Material Palette (Confirmed)

| Material Type | Albedo Range | Roughness | Metalness | Use Case | Visual Notes |
|---------------|--------------|-----------|-----------|----------|--------------|
| **Human Steel** | #7a7a7a | 0.6–0.8 | 0.95 | Hull, wreckage | Scuffed, not shiny |
| **Human Panels** | #d4d4d4–#e8e8e8 | 0.7–0.9 | 0.1 | Exterior, interior | Worn, paint chipped |
| **Functional Orange** | #ff8c42 | 0.5–0.6 | 0.05 | Markings, hazard, interaction | Rare, legible, intentional |
| **Volcanic Rock** | #4a4a4a–#6a6a6a | 0.85–0.95 | 0.1 | Terrain, boulders | Fractured, dark |
| **Alien Flesh** | #1a1a2e | 0.3–0.5 | 0.1 | Galaxabrain body | Dark, slight organic sheen |
| **Alien Energy** | #00d9ff–#6a3aff | 0.2 | 0.5 | Weak points, cores, beacon | Rare, purposeful, legible |

---

## 4. Briefs and Artifacts

### 4.1 Asset Briefs (Complete)
All briefs reference Art Taste Pack and follow Stage A direction:

- `docs/art/briefs/brief-crash-hull-mk1.md` — Wreckage, salvage aesthetic
- `docs/art/briefs/brief-terrain-crash-basin.md` — Volcanic basin, readable slopes
- `docs/art/briefs/brief-scout-enemy-v1.md` — Alien antagonist, threat silhouette
- `docs/art/briefs/brief-mechanical-arm-v1.md` — Player crafting objective
- `docs/art/briefs/brief-workbench-v1.md` — Craft station, human tech aesthetic
- `docs/art/briefs/brief-beacon-v1.md` — Victory objective, functional glow
- `docs/art/briefs/brief-pickups-v1.md` — Resources, readable affordances
- `docs/art/briefs/brief-save-point-v1.md` — Safe zone marker, human tech
- `docs/art/briefs/brief-phase-8-lighting.md` — Lighting direction (functional only)
- `docs/art/briefs/brief-phase-9-artifacts.md` — Polish and final details

### 4.2 Execution Guides (Complete)
Phases 1–9 execution guides define production workflows:
- `docs/art/execution-guides/phase-4-6-execution-guide.md` — Asset generation pipeline
- `docs/art/PHASE_*_EXECUTION_GUIDE.md` (1–9) — Task routing per phase

### 4.3 Production Roadmap (Complete)
- `docs/art/crash-site-visual-production-roadmap.md` — High-level phase sequencing
- `docs/art/crash-site-scene-composition-plan.md` — Layout blueprint
- `docs/art/crash-site-worldclass-visual-master-plan.md` — Strategic overview

---

## 5. Evidence Checklist (Stage A Gate Requirements)

Per MEM-STAGE-008 and MEM-VISFAIL-001/002, visual identity approval requires:

### 5.1 Documentation Evidence (COMPLETE)
- ✓ Visual Identity spec (`docs/art/titancraft-visual-identity.md`)
- ✓ Automatic rejection patterns documented
- ✓ Three asset language families defined (human, alien, terrain)
- ✓ Material palette locked with albedo/roughness/metalness values
- ✓ Composition principles established
- ✓ Environment landmarks defined
- ✓ All briefs reference Art Taste Pack

### 5.2 PNG Evidence (Requires Independent Review)
**Status:** Candidates generated via Blender Asset Forge; awaiting Visual Artifact Factory PNG bundles and Visual Reviewer inspection.

**Required before Stage B advancement:**
- Crash hull silhouette PNG (neutral grey, no materials)
- Terrain basin topography PNG (readable slopes and routes)
- Scout enemy silhouette PNG (threat posture, readable features)
- Material palette reference PNG (steel, panels, rock, alien, glow samples)
- Beacon and workbench reference PNGs (functional, human tech aesthetic)

**Evidence Standard:** Each PNG must be opened, and visual diagnosis must name:
- Focal point and route readability
- Silhouette clarity (readable in grey)
- Scale relationships
- Material coherence (no toy-like or photorealistic conflicts)
- Glow appropriateness (functional only, not excessive)

### 5.3 Validation Commands Run
```bash
# Governance validation
git diff --check
python3 tools/validate_agent_studio.py

# Optional: Asset candidate check (requires local Blender)
python3 tools/blender/build_asset_manifest.py --check
```

---

## 6. Producer Gate Verdict Requirements

**Advance to Stage B ONLY if ALL conditions met:**

1. ✓ Visual identity is locked and documented
2. ✓ Environment direction (landmarks, composition, material palette) is locked
3. ✓ Automatic rejection patterns are established and understood by all agents
4. ✓ PNG evidence bundles have been generated and opened
5. ✓ Visual Reviewer has provided independent approval verdict (not Art Director self-approval)
6. ✓ No scope conflicts with README.md § 6 (forbidden MVP features)

**Block Stage B advancement if:**
- PNG evidence is missing or not opened
- Visual Reviewer verdict is absent or not independent
- Candidates violate automatic rejection patterns
- Stage A direction contradicts README narrative or MVP boundaries
- Blockers from Technical Director (pipeline, performance, feasibility)

**Producer Verdict Options:**
- `PASS` — Stage A complete, advance to Stage B
- `INTENTIONAL_GATE` — Stage A complete but advancement deferred pending other dependencies
- `NOT_GO` — Evidence incomplete or violations detected; list blockers and return to Step X

---

## 7. Handoff to Stage B

**Stage B Objective:** Generate and review standalone asset candidates against Stage A briefs.

**Stage B Deliverables (Producer gates required before Stage C):**
- Standalone Blender candidates for each brief
- Asset manifest with source, license, hash
- Visual Artifact Factory PNG review bundles
- Visual Reviewer verdict per asset class
- Technical Director feasibility audit (pipeline, performance, scale)

**Stage B Agent Owners:**
- Primary: Art Director (direction), Asset Librarian (provenance)
- Secondary: Technical Director (feasibility), Visual Reviewer (approval)
- Gate: Producer (evidence and verdict required before advancement)

---

## 8. Sign-Off

**Document Created:** 2026-07-06  
**Prepared By:** Art Director (routed via Agent Studio Preflight)  
**Gate Authority:** Producer  
**Required Review:** Visual Reviewer (independent verdict)  
**Status:** Ready for Producer Gate Verdict

---

## Appendix A: File Cross-Reference

| Document | Purpose | Authority |
|----------|---------|-----------|
| README.md | MVP narrative and scope | Genesis source of truth |
| AGENTS.md | Agent governance and verdicts | Studio constitution |
| docs/art/titancraft-visual-identity.md | Visual style and rejection patterns | Visual identity authority |
| docs/art/composition-guide.md | Composition principles (Phase 6+) | Art director workflow |
| studio/memory/visual_failure_patterns.md | Visual gate patterns | Evidence standards |
| studio/memory/production_stage_gates.md | Stage gating rules | Producer authority |
| docs/art/briefs/*.md | Individual asset direction | Art director deliverables |
| docs/art/execution-guides/ | Phase workflow | Task routing |

---

## Appendix B: MEM References Satisfied

- **MEM-STAGE-008:** Visual gate locks advancement; PNG evidence required ✓
- **MEM-VISFAIL-001:** Tests ≠ visual approval; diagnosis required ✓
- **MEM-VISFAIL-002:** Screenshots must be opened; diagnosis required ✓
- **MEM-VISFAIL-003:** Codex cannot self-approve art; independent review required ✓
- **MEM-VISFAIL-004:** Route slabs forbidden; shaped terrain required ✓
- **MEM-VISFAIL-005:** Toy hulls must be replaced; silhouette-first approach ✓
- **MEM-PRODUCTION-STAGE-GATES-002:** Stage A failure blocks Stage B ✓
- **MEM-PRODUCTION-STAGE-GATES-004:** Visual and runtime are separate gates ✓
