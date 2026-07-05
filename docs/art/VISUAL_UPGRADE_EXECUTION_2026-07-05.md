# TitanCraft Visual Upgrade Execution Plan
## Top-Tier Production with Full Studio Workflow

**Project Director Approval:** YES  
**Execution Date:** 2026-07-05  
**Scope:** All-of-the-above + marketing-grade renders  
**Workflow:** Visual Art Direction → Blender Asset Forge → Visual Artifact Factory → Evidence-gated approval  

---

## Phase Overview

### Current State Diagnosis
- **Status:** Placeholder/blockout quality (per master plan §2)
- **Terrain:** Flat plate-like surfaces; needs shaped ash/basalt
- **Crashed Ship:** Reads toy-like capsule; needs heavier mass, crushed asymmetry, torn structure
- **Materials:** Simple; needs coherent PBR palette (worn, industrial, alien contrast)
- **Lighting:** Not final; needs readable silhouettes, hero angles, combat visibility
- **Interactables:** Functional but not visually distinct (workbench, beacon, save)
- **Resources:** Generic geometry; need silhouette-first readability
- **Scout Enemy:** Needs biomechanical contrast (purple/dark alien vs human wreckage)
- **Marketing:** No approval-grade renders yet

### Target State (Top-Tier)
- **Terrain:** Shaped volcanic basin, ash floors, basalt ridges, readable route
- **Crashed Ship:** Heavy industrial wreckage, crushed nose, torn panels, exposed structure
- **Materials:** Worn steel, graphite, off-white, orange accents, alien purple/cyan contrast
- **Lighting:** High-contrast silhouettes, hero-angle optimized, readable combat distance
- **Interactables:** Visually distinct, orange functional accents, readable at gameplay distance
- **Resources:** Distinct silhouettes (metal cube, biomass organic, electronics circuit, component module)
- **Scout:** Biomechanical threat form, asymmetric armor, readable weak core (cyan/red)
- **Marketing:** Approved contact sheets, turntables, hero angles (all validation angles from master plan §6)

---

## Execution Roadmap (10-Phase)

### **PHASE 1: FOUNDATION — Terrain & World** ⚡ START HERE
**Outcome:** Shaped crash basin, ash floor, basalt rim, route landmarks  
**Files to create:** `docs/art/brief-terrain-crash-basin.md`, Blender source  
**Evidence required:** Neutral gray silhouette PNG, before/after scene composition

**Steps:**
1. [ ] Create asset brief: terrain geometry specs, poly budget, material zones
2. [ ] Build Blender prototype: ash floor, fractured ground, basalt ridges, navigable slopes
3. [ ] Export GLB and validate import in Godot
4. [ ] Generate manifest and SHA256 hash
5. [ ] Capture neutral-gray turntable PNG
6. [ ] Validate: no clipping, no excessive poly, route playable
7. [ ] Create `ASSET_IMPLEMENTATION_PASS` report

**Gate check:** Route readability improved? Silhouette readable in neutral gray?  
**Verdict:** `ASSET_BRIEF_READY` → `ASSET_IMPLEMENTATION_PASS` → Ready for composition integration

---

### **PHASE 2: HERO — Crashed Ship** ⚡ HIGH IMPACT
**Outcome:** Industrial crushed spacecraft, mass and broken structure  
**Files to create:** `docs/art/brief-crash-hull-mk1.md`, Blender source  
**Evidence required:** Silhouette PNG, damaged-state turntable, scale reference

**Steps:**
1. [ ] Create detailed brief: hull body, crushed nose, broken wing, engine pods, internal ribs, breach, scorch
2. [ ] Build Blender model: author heavy massing, asymmetric crush, torn panels, field repairs
3. [ ] Material setup: worn steel, graphite, off-white hull, scorch marks, exposed internals
4. [ ] Export GLB, validate poly/import
5. [ ] Capture hero angles: spawn view, three-quarter, damaged-detail close-up
6. [ ] Generate manifest/hash
7. [ ] Test integration in scene composition (non-destructive, review-only)

**Gate check:** Reads as heavy crushed industrial, not toy capsule? Silhouettes contrast with terrain?  
**Verdict:** `ASSET_BRIEF_READY` → `ASSET_IMPLEMENTATION_PASS` → Composition-ready

---

### **PHASE 3: INTERACTABLES — Workbench, Save, Beacon** ⚡ GAMEPLAY CRITICAL
**Outcome:** Visually distinct, orange/cyan functional accents  
**Files to create:** Asset briefs for each, Blender sources or refined V1 models  
**Evidence required:** Turntables, inactive/active states, gameplay distance screenshots

**Steps:**
1. [ ] Assess current MVP Pack V1 workbench/beacon/save models
2. [ ] Create briefs: functional readability, material/color language, interactive affordances
3. [ ] Refine or replace in Blender: orange accents for workbench handle, cyan for beacon power, save point indicator glow
4. [ ] Test first-person gameplay distance visibility
5. [ ] Capture: all three objects side-by-side, each inactive/active state
6. [ ] Validate: distinct silhouettes, orange/cyan palette consistency
7. [ ] Generate manifests/hashes

**Gate check:** Are objects visually interactive before UI appears?  
**Verdict:** `ASSET_IMPLEMENTATION_PASS` → Scene integration ready

---

### **PHASE 4: PICKUPS — Resources & Component** ⚡ COLLECTIBILITY
**Outcome:** Silhouette-distinct metal/biomass/electronics/component  
**Files to create:** Asset briefs, Blender sources  
**Evidence required:** Three-resource comparison PNG, component close-up, pickup drop animation

**Steps:**
1. [ ] Create briefs: metal (industrial cube/shard), biomass (organic mass), electronics (circuit/module), component (distinctive trophy form)
2. [ ] Assess current V1 models; enhance if blocky/generic
3. [ ] Design silhouettes: each distinct at 5m gameplay distance
4. [ ] Material setup: metal (muted steel), biomass (red/purple organic), electronics (cyan accent), component (hybrid human+alien)
5. [ ] Validate poly budget
6. [ ] Capture: all four side-by-side, gameplay-distance mockup
7. [ ] Generate manifests/hashes

**Gate check:** Can player distinguish all 4 types by silhouette alone?  
**Verdict:** `ASSET_IMPLEMENTATION_PASS` → Gameplay integration ready

---

### **PHASE 5: THREAT — Galaxabrain Scout** ⚡ VISUAL CONTRAST
**Outcome:** Hostile biomechanical form, weak core readable  
**Files to create:** `docs/art/brief-galaxabrain-scout-mk1.md`, Blender source  
**Evidence required:** Neutral silhouette, armor/organic contrast turntable, weak-core close-up, combat-distance mockup

**Steps:**
1. [ ] Create brief: body (asymmetric organic/shell), weak core (cyan or red focal point), armor plates (carapace), weapon/appendages (asymmetric shards)
2. [ ] Assess MVP Pack V1 model; enhance silhouette/weak-core visibility if needed
3. [ ] Material setup: dark purple/black shell, bruised purple organic tissue, cyan/red weak core glow (selective, not overwhelming)
4. [ ] Test first-person combat visibility: weak core readable at 3-5m range
5. [ ] Capture: neutral gray silhouette, material turntable, weak-core close-up, combat entry angle
6. [ ] Validate: no confused identity with player/workbench, strong threat contrast
7. [ ] Generate manifest/hash

**Gate check:** Reads as hostile and biomechanical before glow considered?  
**Verdict:** `ASSET_IMPLEMENTATION_PASS` → Gameplay integration ready

---

### **PHASE 6: PLAYER TOOL — Mechanical Arm First-Person** ⚡ HERO READ
**Outcome:** Salvaged industrial Mk I, first-person visual  
**Files to create:** Asset brief, first-person Blender model if current lacks detail  
**Evidence required:** First-person pose screenshots, action poses, scale reference

**Steps:**
1. [ ] Assess current MVP Pack V1 first-person arm model
2. [ ] Create brief: articulated industrial arm (pistons, plates, joints), salvaged appearance, Mk I marking
3. [ ] Enhance first-person read: visible mechanisms, industrial aesthetics, asymmetric worn panels
4. [ ] Material setup: muted steel, graphite joints, orange control markings
5. [ ] Test FPS aiming pose, attack animation frame
6. [ ] Capture: idle pose, attack pose, close-up joint detail
7. [ ] Generate manifest/hash

**Gate check:** Reads as salvaged industrial tool, not sleek weapon?  
**Verdict:** `ASSET_IMPLEMENTATION_PASS` → Integration ready

---

### **PHASE 7: COMPOSITION — Scene Integration & Route Readability** ⚡ HIERARCHY PROOF
**Outcome:** All assets composed, visual hierarchy validated  
**Files to create:** Scene composition brief, before/after contact sheet  
**Evidence required:** Six validation angles (spawn, resource trail, hull hero, arena entry, component/save, beacon extraction)

**Steps:**
1. [ ] Create scene composition brief: spawn angle, resource trail landmarks, hull focus, arena entry, extraction zone
2. [ ] Integrate Phase 1-6 assets into Main.tscn (non-destructive staging)
3. [ ] Validate visual hierarchy: first/second/third/fourth reads work
4. [ ] Capture six angles (from master plan §6):
   - Spawn orientation (ship or objective first)
   - Resource trail toward workbench
   - Three-quarter crash hull hero view
   - Combat arena entry
   - Defeated Scout/component retrieval
   - Save/beacon extraction zone
5. [ ] Generate contact sheet PNG (6 angles, labeled)
6. [ ] Gameplay smoke test: no collision regressions, movement/interaction unblocked
7. [ ] Document any visual issues or necessary adjustments

**Gate check:** All hierarchy levels work? Routes readable?  
**Verdict:** `VISUAL_SLICE_GAMEPLAY_SAFE` → Lighting pass ready

---

### **PHASE 8: LIGHTING & MATERIALS — Final Polish** ⚡ PHOTO QUALITY
**Outcome:** High-contrast silhouettes, hero-angle optimized, palette discipline  
**Files to create:** Lighting/material technical notes, tuning screenshots  
**Evidence required:** Before/after angles, exposure notes, palette audit

**Steps:**
1. [ ] Tune key direction: low angled from upper-left/front for broad readable highlights
2. [ ] Optimize ambient fill: cool, low-intensity, keeps navigation safe
3. [ ] Validate silhouette contrast: medium-high, no flat darkness, no overbright toy read
4. [ ] Material audit: worn steel, graphite, off-white, orange, purple, cyan palette consistency
5. [ ] Glow restraint: selective beacon/weak-core glow only, no overwhelming decoration
6. [ ] Capture: all six validation angles with final lighting
7. [ ] Exposure notes: exposure value, bloom settings, contrast target
8. [ ] Validate combat visibility: weak core, cover edges, player retreat path all readable from FPS height

**Gate check:** Readable silhouettes with hero-angle polish?  
**Verdict:** `ASSET_IMPLEMENTATION_PASS` → Evidence capture ready

---

### **PHASE 9: VISUAL ARTIFACT FACTORY — Marketing-Grade Renders** ⚡ PUBLIC READY
**Outcome:** Approved contact sheets, turntables, hero angles for marketing  
**Files to create:** Artifact factory captures, manifest  
**Evidence required:** Full contact sheet, individual hero angle PNGs, artifact metadata

**Steps:**
1. [ ] Run Visual Artifact Factory CI workflow for all 6 validation angles
2. [ ] Generate turntables for: crashed ship, Galaxabrain Scout, mechanical arm, resource trio
3. [ ] Create contact sheet PNG: all angles labeled, quality consistent
4. [ ] Capture hero angles: best spawn view, best hull view, best combat setup
5. [ ] Generate manifest: asset metadata, capture settings, lighting specs
6. [ ] Validate PNGs: no corruption, consistent resolution, labeled correctly
7. [ ] Store in `artifacts/review/stage-a-visual-upgrade/` with timestamp
8. [ ] Document: lighting setup, camera distance, exposure used

**Gate check:** Are renders marketing-grade quality? Do they tell the survival/craft/fight story?  
**Verdict:** `ASSET_IMPLEMENTATION_PASS` → Human/visual-review approval ready

---

### **PHASE 10: APPROVAL GATE — Stage A Visual Verdict** ⚡ FINAL GATE
**Outcome:** Human/visual-review approval or explicit NOT_GO with remediation  
**Files to create:** Stage A approval report, final verdict  
**Evidence required:** Complete before/after contact sheet, opened PNG critique, manifest, provenance

**Steps:**
1. [ ] Assemble complete evidence:
   - Before photos (current blockout state)
   - After photos (Phase 8 final state, all 6 angles)
   - Turntables (ship, Scout, arm, resources)
   - Manifest (all assets, hashes, sources)
   - Gameplay smoke evidence (no regressions)
   - Palette audit (human/alien/terrain colors named)
   - Readability proof (silhouettes, hierarchy, weak core, route)

2. [ ] Create Stage A approval report:
   - Visual brief recap
   - Evidence locations
   - Palette compliance checklist
   - Readability validation (all hierarchy levels)
   - Combat/gameplay safety proof
   - Marketing readiness assessment

3. [ ] Open evidence to human/visual-review authority:
   - Present contact sheets
   - Ask specific questions: Polygonal Salvage Sci-Fi compliant? Readable at gameplay distance? Marketing-ready?
   - Collect structured feedback

4. [ ] Verdict options:
   - **`STAGE_A_VISUAL_APPROVED`** → Proceed to production integration, public release
   - **`STAGE_A_VISUAL_NOT_GO` + remediation** → Document specific issues, remediation plan, re-audit

5. [ ] If approved:
   - Merge Phase 1-7 into main
   - Update README §30 acceptance criteria
   - Prepare Windows export/marketing materials
   - Archive evidence with approval date

**Gate check:** Does visual quality meet Crash Site thesis? Is it marketing-ready?  
**Verdict:** Final `STAGE_A_VISUAL_APPROVED` or explicit NOT_GO with remediation plan

---

## Execution Sequencing

| Phase | Name | Depends On | Timeline | Owner | Verdict |
|-------|------|-----------|----------|-------|---------|
| 1 | Terrain Foundation | None | ~2h | Blender + Asset Forge | `ASSET_IMPLEMENTATION_PASS` |
| 2 | Crash Hull Hero | Phase 1 terrain approved | ~3h | Blender + Asset Forge | `ASSET_IMPLEMENTATION_PASS` |
| 3 | Interactables | Phase 2 composition plan | ~1.5h | Asset review + refinement | `ASSET_IMPLEMENTATION_PASS` |
| 4 | Pickups | Phase 3 tested | ~1h | Asset review + refinement | `ASSET_IMPLEMENTATION_PASS` |
| 5 | Scout Enemy | Phase 4 pickups approved | ~2h | Blender + Asset Forge | `ASSET_IMPLEMENTATION_PASS` |
| 6 | Player Arm | Phase 5 compositio ready | ~1h | Asset review + refinement | `ASSET_IMPLEMENTATION_PASS` |
| 7 | Scene Composition | Phases 1-6 all done | ~2h | Scene integration + smoke test | `VISUAL_SLICE_GAMEPLAY_SAFE` |
| 8 | Lighting/Materials | Phase 7 integrated | ~1.5h | Godot tuning | `ASSET_IMPLEMENTATION_PASS` |
| 9 | Artifact Factory | Phase 8 final lighting | ~1h | CI capture workflow | Renders approved |
| 10 | Approval Gate | All evidence complete | ~1h | Human review | `STAGE_A_VISUAL_APPROVED` or NOT_GO |

**Total estimated time:** ~16 hours  
**Can be parallelized:** Phases 1-2 can start, then 3-6 in parallel once composition brief done  
**Actual timeline depends on:** Blender environment availability, human review speed, remediation needs

---

## Required Tools & Environment

### Blender Asset Forge
- Source: `art/blender/`
- Commands: `python3 tools/blender/build_asset_manifest.py`
- Output: GLB exports, manifest, SHA256 hashes

### Visual Artifact Factory
- Workflow: `.github/workflows/visual-artifact-factory.yml`
- Output: PNG contact sheets, turntables, scene captures
- Requirement: Godot headless mode working

### Godot Validation
```bash
godot --headless --path . --import
dotnet build
dotnet test tests/TitanCraft.Tests.csproj
```

### Git Evidence Tracking
```bash
git diff --check
git log --oneline
git status --short
```

---

## Approval Gates Summary

| Gate | Condition | Authority |
|------|-----------|-----------|
| `ASSET_BRIEF_READY` | Brief complete, no ambiguity | Project Director (you) |
| `ASSET_IMPLEMENTATION_PASS` | Asset exported, manifest/hash valid, import successful, no regression | Build system + gameplay smoke |
| `VISUAL_SLICE_GAMEPLAY_SAFE` | Scene integration passed, gameplay unblocked, no collision regressions | Gameplay tests |
| `STAGE_A_VISUAL_NOT_GO` | Evidence exists but quality/readability insufficient | Human/visual-review authority |
| `STAGE_A_VISUAL_APPROVED` | Human/visual-review authority approves Stage A | Human/visual-review authority |

**Critical rule:** Passing tests ≠ visual approval. Generating screenshots ≠ visual approval. Only human/visual-review authority can approve final visuals.

---

## Forbidden Scope Checklist

- ❌ No new gameplay mechanics (crafting, weapons, abilities)
- ❌ No procedural world generation
- ❌ No multiple maps or biomes
- ❌ No voxel/block-based construction
- ❌ No additional Galaxabrain types or boss encounters
- ❌ No grappling hook, wall-run, or large mech
- ❌ No public marketing screenshots before approval
- ❌ No claims of visual approval without human verdict
- ❌ No Stage A replacement without explicit approval gate passing

---

## Success Criteria (Top-Tier Visual)

✅ **All of the following must be true:**

1. All 10 phases complete with `ASSET_IMPLEMENTATION_PASS` or `VISUAL_SLICE_GAMEPLAY_SAFE`
2. Terrain reads shaped (ash/basalt), not flat plates
3. Crashed ship reads heavy/crushed, not toy capsule
4. Material palette coherent (worn human tech, alien threat, volcanic world)
5. Visual hierarchy works: spawn → resources → workbench → combat → component → save/beacon
6. Interactables (workbench, beacon, save) visually distinct, readable at gameplay distance
7. Pickups distinguishable by silhouette (metal/biomass/electronics/component)
8. Scout reads biomechanical threat with readable weak core
9. First-person arm reads salvaged industrial Mk I
10. Lighting: high-contrast silhouettes, no flat darkness, no overbright toy read
11. All 6 validation angles captured and labeled
12. Manifest complete with all assets, hashes, sources
13. No gameplay regressions (movement, interaction, collision)
14. No binary policy violations (all assets licensed/sourced)
15. **Human/visual-review authority issues `STAGE_A_VISUAL_APPROVED` verdict**

---

## Next Steps (Start Immediately)

**PHASE 1 KICKOFF:**

1. [ ] Create `docs/art/brief-terrain-crash-basin.md` with specs (poly budget, material zones, route landmarks)
2. [ ] Launch Blender, open `art/blender/` project
3. [ ] Build prototype: ash floor, fractured ground, basalt ridges
4. [ ] Export first GLB candidate
5. [ ] Validate: import to Godot, check poly count, verify silhouette readability
6. [ ] Capture neutral-gray turntable PNG
7. [ ] Generate manifest entry and SHA256 hash
8. [ ] Report Phase 1 outcome: `ASSET_BRIEF_READY` or blockers

**Ready to begin?** Confirm Phase 1 start and I will guide each step.

---

**Execution Plan Status:** READY FOR EXECUTION  
**Authorization:** Project Director approved  
**Workflow:** Visual Art Direction + Blender Asset Forge + Visual Artifact Factory  
**Final Verdict Pending:** Human/visual-review `STAGE_A_VISUAL_APPROVED`
