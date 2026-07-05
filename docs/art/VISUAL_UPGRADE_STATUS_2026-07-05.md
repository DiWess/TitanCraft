# TitanCraft Visual Upgrade — Status Report
## Planning & Execution Ready

**Date:** 2026-07-05  
**Phase:** Planning Complete → Execution Ready  
**Authorization:** Project Director (Full Scope)  
**Workflow:** Visual Art Direction + Blender Asset Forge + Visual Artifact Factory  

---

## What Was Completed (Planning)

### ✅ Executive Approvals
- [x] Project Director authorized full-scope upgrade (all-of-the-above + marketing)
- [x] Studio workflow approved (Visual Art Direction skill + Blender Asset Forge + Visual Artifact Factory)
- [x] Approval gates and evidence requirements defined
- [x] Forbidden scope boundaries documented
- [x] Success criteria and validation checkpoints established

### ✅ Planning Documents Created

| Document | Purpose | Status |
|----------|---------|--------|
| `VISUAL_UPGRADE_EXECUTION_2026-07-05.md` | 10-phase execution roadmap with timelines, gates, dependencies | ✅ Ready |
| `brief-terrain-crash-basin.md` | Phase 1 detailed asset brief (terrain/world foundation) | ✅ Ready |
| `brief-crash-hull-mk1.md` | Phase 2 detailed asset brief (hero crashed spacecraft) | ✅ Ready |
| This report | Status and next steps | ✅ Ready |

### ✅ Deliverables Scoped

**Phase 1–10 complete breakdown:**
- [x] Terrain (shaped ash/basalt basin)
- [x] Crashed hull (heavy industrial wreckage)
- [x] Interactables (workbench, beacon, save point)
- [x] Pickups (metal, biomass, electronics, component)
- [x] Scout enemy (biomechanical threat)
- [x] Mechanical arm (first-person salvaged tool)
- [x] Scene composition (all assets integrated)
- [x] Lighting & materials (hero-angle polish)
- [x] Visual artifact factory (marketing-grade renders)
- [x] Approval gate (human/visual-review verdict)

### ✅ Approval Gates Defined

| Gate | Condition | Authority |
|------|-----------|-----------|
| `VISUAL_PLAN_READY` | Planning complete, no ambiguity | ✅ Project Director |
| `ASSET_BRIEF_READY` | Brief complete, specifications clear | Blender team |
| `ASSET_IMPLEMENTATION_PASS` | Asset exported, imported, manifest valid, no regressions | Build system |
| `VISUAL_SLICE_GAMEPLAY_SAFE` | Scene integrated, gameplay unblocked | Gameplay tests |
| `STAGE_A_VISUAL_APPROVED` | Human/visual-review verdict on final renders | Human authority |

---

## What Comes Next (Execution)

### Immediate Actions (Next 24 Hours)

**Step 1: Blender Asset Forge Setup**
```bash
cd /home/user/TitanCraft/art/blender
# Verify environment
ls -la
cat README.md
python3 ../tools/blender/build_asset_manifest.py --check
```

**Step 2: Phase 1 Kick-Off (Terrain)**
- [ ] Open Blender
- [ ] Create new file: `models/TC_TERRAIN_CrashBasin_V1.blend`
- [ ] Build prototype ash floor, basalt ridges per brief spec
- [ ] Export GLB: `assets/models/terrain/TC_TERRAIN_CrashBasin_V1.glb`
- [ ] Validate import in Godot
- [ ] Generate manifest entry and SHA256 hash
- [ ] Capture neutral-gray turntable PNG
- [ ] Report outcome: `ASSET_IMPLEMENTATION_PASS` or blockers

**Step 3: Phase 2 Brief Availability**
- [ ] Phase 2 brief (`brief-crash-hull-mk1.md`) ready for Blender team
- [ ] Can start concurrently once Phase 1 terrain composition locked

**Step 4: CI/Documentation Integration**
- [ ] Commit all planning docs to branch
- [ ] Update `docs/production/current-status.md` with visual upgrade status
- [ ] Run validation:
  ```bash
  git diff --check
  python3 tools/agent_preflight.py "Visual upgrade Phase 1 terrain asset"
  ```

### Timeline Estimate

| Phase | Duration | Blocker Dependencies |
|-------|----------|----------------------|
| 1. Terrain | ~2h | None |
| 2. Hull | ~3h | Phase 1 approved |
| 3. Interactables | ~1.5h | Phase 2 composition plan |
| 4. Pickups | ~1h | Phase 3 tested |
| 5. Scout | ~2h | Phase 4 approved |
| 6. Arm | ~1h | Phase 5 composition ready |
| 7. Composition | ~2h | Phases 1–6 complete |
| 8. Lighting | ~1.5h | Phase 7 integrated |
| 9. Artifacts | ~1h | Phase 8 final lighting |
| 10. Approval | ~1h | All evidence assembled |
| **TOTAL** | **~16h** | Sequential + parallelizable |

**Parallelization opportunity:** Phases 1–2 can start independently. Phases 3–6 can proceed in parallel once Phase 2 composition brief is ready. Phase 7 requires all 1–6 complete.

**Realistic delivery:** ~2–3 working days if Blender team has continuous availability.

---

## What Must NOT Happen

🚫 **Forbidden actions during visual upgrade:**

- ❌ Do not claim visual approval without human/visual-review verdict
- ❌ Do not enable Stage A assets in production scenes until approval gate passes
- ❌ Do not expand MVP scope (no new enemies, weapons, gameplay mechanics)
- ❌ Do not create multiple maps or biomes
- ❌ Do not add procedural generation beyond authored shapes
- ❌ Do not delete v1 beta assets until confirmed as fully obsolete
- ❌ Do not publish marketing screenshots before approval
- ❌ Do not skip any evidence gates (tests, manifests, hashes, screenshots)
- ❌ Do not modify gameplay code without explicit permission in the upgrade task
- ❌ Do not claim "done" without passing all validation checkpoints

---

## Key Documents Reference

| Document | Purpose | Location |
|----------|---------|----------|
| **Visual Upgrade Execution Plan** | 10-phase roadmap with timelines & gates | `docs/art/VISUAL_UPGRADE_EXECUTION_2026-07-05.md` |
| **Terrain Brief** | Phase 1 detailed specs | `docs/art/brief-terrain-crash-basin.md` |
| **Hull Brief** | Phase 2 detailed specs | `docs/art/brief-crash-hull-mk1.md` |
| **World-Class Master Plan** | Visual thesis & diagnosis | `docs/art/crash-site-worldclass-visual-master-plan.md` |
| **Production Roadmap** | 10-PR planning structure | `docs/art/crash-site-visual-production-roadmap.md` |
| **Visual Identity** | Style boundaries & rules | `docs/art/titancraft-visual-identity.md` |
| **Production Status** | Current blocked/unblocked work | `docs/production/current-status.md` |
| **Definition of Done** | Asset/scene/CI completion criteria | `docs/production/definition-of-done.md` |

---

## Tools & Environment Requirements

### Blender Asset Forge
```bash
# Location
art/blender/

# Asset manifest commands
python3 tools/blender/build_asset_manifest.py --check  # Inspect only
python3 tools/blender/build_asset_manifest.py          # Generate (CI use)

# Expected output
assets/Production/Generated/asset_manifest.json
```

### Visual Artifact Factory
```bash
# CI workflow
.github/workflows/visual-artifact-factory.yml

# Purpose
Generate PNG review captures, turntables, contact sheets
Export Godot scene screenshots for visual review
```

### Godot Validation Commands
```bash
dotnet build                                    # C# compilation
dotnet test tests/TitanCraft.Tests.csproj      # Unit tests
godot --headless --path . --import             # Scene import validation
git diff --check                               # No trailing whitespace/CRLF
```

---

## Success Criteria (Final)

### Visual Upgrade is **COMPLETE & TOP-TIER** when:

✅ All 10 phases pass `ASSET_IMPLEMENTATION_PASS` or `VISUAL_SLICE_GAMEPLAY_SAFE`

✅ **Terrain:** Shaped volcanic basin, ash floor, basalt rim, readable routes

✅ **Crashed ship:** Heavy crushed industrial, not toy capsule, internal structure visible

✅ **Materials:** Coherent palette (worn human steel, graphite, off-white, orange accents, alien purple/cyan)

✅ **Visual hierarchy:** All 4 focal levels work (spawn → resources → workbench → combat → component → save → beacon)

✅ **Interactables:** Workbench, beacon, save point visually distinct, readable at gameplay distance

✅ **Pickups:** Metal/biomass/electronics/component distinguishable by silhouette

✅ **Scout:** Biomechanical threat, weak core readable, distinct from human wreckage

✅ **First-person arm:** Salvaged industrial Mk I, visible mechanisms

✅ **Lighting:** High-contrast silhouettes, readable combat distance, restrained glow

✅ **Validation angles:** All 6 screenshot angles captured and labeled (spawn, resource trail, hull hero, arena entry, component, beacon)

✅ **Manifest:** Complete asset inventory with hashes and sources

✅ **Gameplay safety:** No regressions in movement, collision, or interaction

✅ **Marketing-ready:** Approved contact sheets, turntables, and hero angles

✅ **Human verdict:** `STAGE_A_VISUAL_APPROVED` issued by visual-review authority

---

## What This Enables (Post-Approval)

Once `STAGE_A_VISUAL_APPROVED` is issued:

1. ✅ Merge all Phase 1–7 work into main branch
2. ✅ Update `docs/production/current-status.md` (Stage A now approved)
3. ✅ Prepare Windows export candidate with approved visuals
4. ✅ Begin marketing asset preparation (hero screenshots, turntables, demo reel)
5. ✅ Schedule Phase 1 gameplay bug-fix cycle with new visuals
6. ✅ Prepare public demo materials (if authorized)

---

## Status Summary

| Category | Status | Notes |
|----------|--------|-------|
| **Planning** | ✅ Complete | All briefs, gates, and timelines documented |
| **Authorization** | ✅ Approved | Project Director full scope |
| **Workflow** | ✅ Defined | Visual Art Direction + Blender Asset Forge + Visual Artifact Factory |
| **Blender Setup** | ⏳ Ready | Awaiting Phase 1 start |
| **Phase 1 Specs** | ✅ Complete | Terrain brief finalized, ready for implementation |
| **Phase 2 Specs** | ✅ Complete | Hull brief finalized, ready for implementation |
| **Phases 3–10 Specs** | 🔄 Pending | Will be created as each phase begins (leverages lessons from 1–2) |
| **CI/Artifact Factory** | ⏳ Ready | Workflows exist, awaiting first asset export |
| **Approval Gate** | 🔄 Pending | Human/visual-review verdict to be issued after Phase 9 |

---

## Risks & Mitigation

| Risk | Impact | Mitigation |
|------|--------|-----------|
| Blender environment unavailable | 16h blocked | Pre-check Blender Forge setup now; use local Blender if needed |
| Asset export fails (manifest, GLB) | Blockers in Phase 1 | Validate export tooling before starting Phase 1 |
| Godot import errors (UID, material mismatch) | Gameplay blocker | Test import on sample asset before Phase 1 completion |
| Visual feedback takes too long | Delays Phase 8 | Capture screenshots early (Phase 1), get feedback ASAP |
| Scope creep (expanding beyond MVP) | Timeline risk | Brief explicitly forbids new mechanics; review each phase against boundaries |
| Human approval delayed | Final gate blocker | Identify visual-review authority NOW; schedule review windows |
| Poly budget exceeded (Phase 1 or 2) | Performance risk | Monitor poly count during Blender work; validate in Godot early |

---

## Next Checkpoint

**Target:** Phase 1 terrain asset `ASSET_IMPLEMENTATION_PASS` (first asset ready for Godot)

**Checkpoints:**
- [ ] Terrain brief understood and questions resolved
- [ ] Blender Forge environment confirmed working
- [ ] Phase 1 Blender work started
- [ ] Intermediate terrain silhouette checked (readable shape, not flat)
- [ ] GLB export successful
- [ ] Godot import successful
- [ ] Manifest entry created with SHA256
- [ ] Turntable PNG captured

**Target completion:** ~2 hours after Phase 1 start

---

## Questions? Blockers?

Before proceeding, confirm:

1. **Blender environment ready?** (version, asset paths, export tooling verified)
2. **Who is visual-review authority for approval gate?** (human decision-maker identified)
3. **Timeline realistic?** (16h across ~2–3 days feasible for your team)
4. **Any scope clarifications needed?** (brief boundaries clear?)

---

**Status:** 🟢 **READY FOR EXECUTION**

All planning complete. Phase 1 (terrain) awaiting kickoff.

Proceed to Phase 1, or request clarifications/adjustments to plan first?
