# TitanCraft Production Progression Dashboard
**Date:** 2026-07-06 (see 2026-07-16 correction below — everything from "Current Status" through Task #3 in this file is stale)
**Scope:** Visual Experience Kickoff → MVP Ship  
**Authority:** README.md (MVP definition), AGENTS.md (governance), studio/agents/ (agent missions)

---

## Correction — 2026-07-16 (Claude Code, verified against the working tree)

This file's "Task #1 IN_PROGRESS" framing is wrong and has been since well before this date; it was never updated as work landed. Verified state as of commit `c04728a`:

- **Task #1 (generate candidates):** effectively done. All 10 Stage B candidates have generated deliverables — `TC_HeavyCrashHull_V1` and `TC_TERRAIN_CrashBasin_V1` standalone, the other 8 via `create_mvp_asset_pack_v1.py` and the environment-dressing kits (Alien Shard, Distant Silhouettes, Rock Occluder, Base Camp Dressing, Hull Rib Occluder, Lighting Reference, Polish Details). See `studio/decisions/procedural_terrain_deterministic_exception.md` for why Terrain Basin was never swapped in (deliberate, not a gap).
- **Task #2 (PNG bundles) and Task #3 (Visual Reviewer verdict):** done for every candidate as of `docs/art/reviews/*.md` (commit `8774951`). One real `NOT_GO` (`TC_ENV_DistantSilhouette_SmokePlume_V1`) and one flagged-not-failed deviation (`TC_PROP_Workbench_V1`) came out of that pass — see `studio/tasks/art-director-decision-packet-2026-07-16.md` for the handoff.
- **Task #4 (Tech Director audit):** partially open. `TC_ENV_RockOccluder_V1`'s collision/navigation half is routed to Technical Director in the same decision packet. The GLB import/performance half cannot be run from this container — no Godot binary is available here (apt only has an incompatible 3.5.2; the CI-standard direct download from GitHub is blocked by this session's network egress policy) — so this stays `ENVIRONMENT_BLOCKED` until run somewhere Godot actually exists.
- **Task #5 (Producer gate) through Task #8 (release gate):** genuinely still pending, and correctly so — these need Producer/QA/Build Engineer authority and, for #6/#7, Godot-based before/after evidence this container can't produce. The one real remaining content gap feeding into these is `TC_HeavyCrashHull_V1` scene placement — already has a full standalone review `PASS` (`docs/art/reviews/heavy-crash-hull-v1-standalone-review.md`), just never placed, and its own review already scopes that as "a separate gated task with its own before/after captures and sign-off."

**Current Status (corrected):** Stage A PASS ✓ | Stage B — candidates generated and reviewed, one NOT_GO and one flagged item awaiting Art Director decision, Tech Director audit partially routed/partially environment-blocked | Stage C–Release PENDING, blocked on the above plus Godot availability.

---

## Stage Overview

| Stage | Objective | Primary Agents | Gate Authority | Status |
|-------|-----------|-----------------|-----------------|--------|
| **A** | Lock visual identity & environment direction | Creative Director, Art Director | Producer | ✓ PASS |
| **B** | Generate & validate standalone asset candidates | Art Director, Visual Reviewer, Tech Director | Producer | 🔄 IN_PROGRESS (#1) |
| **C** | Integrate approved assets & validate experience | Level Designer, Gameplay Engineer, QA | Producer | ⏳ PENDING (#6) |
| **Release** | Final export & deployment readiness | Build Engineer, Producer | Human approval | ⏳ PENDING (#8) |

---

## Full Agent Responsibility Map

### Stage A: Direction Lock (COMPLETE ✓)

| Agent | Role | Output | Verdict | Status |
|-------|------|--------|---------|--------|
| **Creative Director** | Maintain narrative identity, tone, brand distinctiveness | Brand guide, naming rules, narrative scope lock | N/A | ✓ Complete |
| **Art Director** | Establish visual style, rejection patterns, environment landmarks | Visual identity spec, material palette, landmark definitions | N/A | ✓ Complete |
| **Technical Director** | Validate feasibility of visual direction in Godot 4 | Pipeline audit, performance targets confirmed | N/A | ✓ Complete |
| **Producer** | Sequence work, hold gates, require evidence | Stage A Art Brief Packet, gate conditions, advance approval | PASS | ✓ PASS |

**Stage A Deliverables:**
- ✓ `docs/art/titancraft-visual-identity.md` (visual identity, rejection patterns)
- ✓ `docs/art/STAGE_A_ART_BRIEF_PACKET.md` (governance packet, evidence gate)
- ✓ 10 asset briefs (crash hull, terrain, scout, arm, workbench, beacon, pickups, save point, lighting, polish)
- ✓ Material palette (locked with albedo/roughness/metalness values)
- ✓ Composition principles (focal point, route, silhouette, scale, coherence)
- ✓ Automatic rejection patterns (9 veto categories)

**Gate Conditions Met:** ✓ Visual identity locked | ✓ Environment direction established | ✓ Briefs ready | ✓ Producer approval recorded

---

### Stage B: Asset Generation & Validation (IN_PROGRESS 🔄)

**Task #1 IN_PROGRESS**

| Agent | Role | Output | Verdict | Status |
|-------|------|--------|---------|--------|
| **Art Director** | Generate standalone Blender candidates per briefs | 10 Blender `.blend` + GLB exports + manifest | Generation (N/A) | 🔄 IN_PROGRESS (#1) |
| **Visual Artifact Factory** | Render PNG evidence bundles | 10 PNG pairs (silhouette + material) per candidate | Automation (N/A) | ⏳ PENDING (#2) |
| **Visual Reviewer** | Independent review of PNG evidence | Visual diagnosis per candidate, PASS/NOT_GO verdict | PASS / NOT_GO | ⏳ PENDING (#3) |
| **Technical Director** | Pipeline & performance audit | Godot import test, draw call estimates, feasibility verdict | PASS / NOT_GO | ⏳ PENDING (#4) |
| **Producer** | Gate Stage B → Stage C or return to rework | Gate verdict (PASS / NOT_GO), advancement decision | PASS / NOT_GO / INTENTIONAL_GATE | ⏳ PENDING (#5) |

**Stage B Deliverables (In Progress):**
- 🔄 10 Blender candidates with materials (Art Director)
- ⏳ Asset manifest with source, brief, hash, material assignments
- ⏳ PNG evidence bundles (silhouette clarity, material coherence, scale reference)
- ⏳ Visual Reviewer diagnosis document (focal point, silhouette, scale, material, glow per candidate)
- ⏳ Technical Director audit log (GLB import test, performance, scale validation per candidate)

**Gate Conditions (Before Stage C):**
- ⏳ All 10 candidates PASS Visual Reviewer verdict
- ⏳ All 10 candidates PASS Technical Director audit
- ⏳ Asset manifest complete and auditable
- ⏳ PNG evidence bundles present and opened
- ⏳ Producer issues PASS verdict

---

### Stage C: Integration & Final Validation (PENDING ⏳)

**Task #6–#7 PENDING**

| Agent | Role | Output | Verdict | Status |
|-------|------|--------|---------|--------|
| **Level Designer** | Integrate candidates into Crash Site scene, verify layout & gameplay | Updated `src/Scenes/CrashSite.tscn` with placed assets | N/A | ⏳ PENDING (#6) |
| **Gameplay Engineer** | Verify gameplay mechanics work with integrated assets (resource pickup, crafting, combat) | Gameplay validation (movement, resource gather, craft, fight, beacon) | PASS / NOT_GO | ⏳ PENDING (#7) |
| **QA Lead** | Visual & gameplay coherence in integrated scene | In-engine screenshots, visual diagnosis, smoke test results | PASS / NOT_GO | ⏳ PENDING (#7) |
| **Visual Reviewer** | Final visual approval of integrated composition | Scene screenshot diagnosis, overall coherence verdict | PASS / NOT_GO | ⏳ PENDING (#7) |
| **Producer** | Gate Stage C → Release or return to rework | Release readiness verdict (GO / INTENTIONAL_GATE / NOT_GO) | GO / INTENTIONAL_GATE / NOT_GO | ⏳ PENDING (#8) |

**Stage C Deliverables (Pending):**
- ⏳ Updated Crash Site scene with integrated, placed assets
- ⏳ In-engine screenshots (scene composition, gameplay actions)
- ⏳ Visual diagnosis of integrated composition (focal point, route, visual coherence)
- ⏳ Gameplay validation (movement, resource gathering, crafting, combat, beacon flow)
- ⏳ QA sign-off (visual polish, collision, performance, no new blockers)

**Gate Conditions (Before Release):**
- ⏳ All gameplay mechanics work with integrated assets
- ⏳ Visual composition is coherent and supports gameplay
- ⏳ No new performance or rendering issues
- ⏳ All evidence documented
- ⏳ Producer issues GO verdict

---

### Release: Export & Deployment Readiness (PENDING ⏳)

**Task #8 PENDING**

| Agent | Role | Output | Verdict | Status |
|-------|------|--------|---------|--------|
| **Build Release Engineer** | Windows export, executable validation, release bundle | Windows `.exe` build artifact, export log, performance verification | PASS / NOT_GO | ⏳ PENDING |
| **Producer** | Final release gate, approval for public deployment | Release gate verdict (GO / HOLD / NOT_GO) | GO / INTENTIONAL_GATE / NOT_GO | ⏳ PENDING (#8) |
| **Human (Producer)** | Final sign-off on visual experience readiness | Approval recorded in issue/PR | GO / NOT_GO | ⏳ Awaits |

**Release Deliverables (Pending):**
- ⏳ Windows executable build artifact
- ⏳ Export validation log (no errors, performance targets met)
- ⏳ Release notes (visual changes, gameplay scope locked to MVP)
- ⏳ Producer release gate verdict

---

## Gate Conditions by Stage

### Stage A → Stage B Gate (PASSED ✓)

**Conditions:**
- ✓ Visual identity is locked and documented
- ✓ Environment direction (landmarks, composition, material palette) is locked
- ✓ Automatic rejection patterns are established
- ✓ 10 asset briefs are ready
- ✓ No scope conflicts with README MVP boundaries

**Verdict:** PASS (2026-07-06)  
**Authority:** Producer (studio/agents/producer.md)

---

### Stage B → Stage C Gate (PENDING ⏳)

**Conditions:**
- ⏳ All 10 candidates PASS Visual Reviewer verdict
- ⏳ All 10 candidates PASS Technical Director audit
- ⏳ Asset manifest complete with hashes and provenance
- ⏳ PNG evidence bundles generated and opened
- ⏳ No new scope conflicts or blockers

**Verdict:** PENDING (awaits Stream 1-5 completion)  
**Authority:** Producer (studio/agents/producer.md)

---

### Stage C → Release Gate (PENDING ⏳)

**Conditions:**
- ⏳ Crash Site scene integrates approved assets
- ⏳ Gameplay mechanics validated (movement, resources, crafting, combat, beacon)
- ⏳ Visual composition is coherent and readable
- ⏳ In-engine screenshots show final visual polish
- ⏳ QA sign-off (visual + gameplay + performance)
- ⏳ No rendering or collision surprises

**Verdict:** PENDING (awaits Stage C completion)  
**Authority:** Producer (studio/agents/producer.md)

---

### Release → Ship Gate (PENDING ⏳)

**Conditions:**
- ⏳ Windows executable builds without errors
- ⏳ Export validation confirms 60 FPS target on Windows
- ⏳ Release artifact is auditable and reproducible
- ⏳ All evidence documented
- ⏳ No gameplay scope expansion (locked to MVP Crash Site)

**Verdict:** PENDING (awaits Build Engineer + Producer)  
**Authority:** Producer + Human approval (studio/agents/producer.md, README.md)

---

## Task & Evidence Tracking

### Current Task List

| Task | Status | Owner | Blocks |
|------|--------|-------|--------|
| #1: Generate Blender candidates | 🔄 IN_PROGRESS | Art Director | #2 |
| #2: PNG evidence bundles | ⏳ PENDING | Visual Artifact Factory | #3, #4 |
| #3: Visual Reviewer verdict | ⏳ PENDING | Visual Reviewer | #5 |
| #4: Tech Director audit | ⏳ PENDING | Technical Director | #5 |
| #5: Producer gate (B→C) | ⏳ PENDING | Producer | #6 |
| #6: Integrate into scene | ⏳ PENDING | Level Designer | #7 |
| #7: Final validation | ⏳ PENDING | QA + Visual Reviewer | #8 |
| #8: Release gate | ⏳ PENDING | Producer + Build Engineer | SHIP |

### Evidence Standards (Per Stage)

**Stage A Evidence:**
- ✓ `docs/art/STAGE_A_ART_BRIEF_PACKET.md` — governance packet with visual identity, briefs, gate conditions
- ✓ `docs/art/titancraft-visual-identity.md` — visual style spec
- ✓ 10 asset briefs (markdown documents)
- ✓ Producer gate verdict recorded

**Stage B Evidence (In Progress):**
- ⏳ Blender candidates + asset manifest (Art Director output)
- ⏳ PNG bundles (Visual Artifact Factory CI output)
- ⏳ Visual Reviewer diagnosis document (per candidate)
- ⏳ Technical Director audit log (per candidate)
- ⏳ Producer gate verdict (when all evidence is in)

**Stage C Evidence (Pending):**
- ⏳ Updated Crash Site scene file (`src/Scenes/CrashSite.tscn`)
- ⏳ In-engine screenshots (composition, gameplay actions)
- ⏳ Visual + gameplay diagnosis document
- ⏳ QA smoke test results
- ⏳ Producer gate verdict

**Release Evidence (Pending):**
- ⏳ Windows build artifact (`.exe`)
- ⏳ Export validation log
- ⏳ Release notes
- ⏳ Producer release gate verdict

---

## Key Policies & Gate Rules

**From AGENTS.md § 3 & studio/memory/:**

1. **Evidence is Mandatory:** No stage advances without concrete, machine-readable proof (PNG files opened, diagnosis named, verdicts recorded).
2. **Independent Review Required:** Art Director cannot approve visual work; Visual Reviewer must provide independent verdict (MEM-VISFAIL-003).
3. **Tests ≠ Approval:** Runtime tests passing does not prove visual quality; PNG evidence and visual diagnosis are required (MEM-VISFAIL-001).
4. **Rejection Patterns are Enforceable:** Toy-like proportions, photorealism, excessive glow, route slabs, toy hulls are automatic vetoes; no subjective back-and-forth (Stage A locked).
5. **Scope is Locked:** No gameplay expansion beyond Crash Site MVP. Briefs reference Stage A direction only.
6. **Stage Gating is Serial:** Stage A failure blocks Stage B (not bypassed). Stage B failure blocks Stage C.
7. **No Self-Approval:** Verdicts must be independent. Art Director routes to Visual Reviewer; Visual Reviewer is not Art Director.

---

## Roadmap to Ship

```
Stage A: Direction Lock
    ├─ Creative Director: brand + narrative
    ├─ Art Director: visual style + rejection patterns
    ├─ Technical Director: feasibility audit
    └─ Producer: GATE APPROVAL (PASS 2026-07-06) ✓
         ↓
Stage B: Asset Generation & Validation (ACTIVE 🔄)
    ├─ Stream 1: Art Director generates 10 Blender candidates (#1 in_progress)
    ├─ Stream 2: Visual Artifact Factory → PNG bundles (#2 pending)
    ├─ Stream 3: Visual Reviewer → visual verdict (#3 pending)
    ├─ Stream 4: Tech Director → feasibility audit (#4 pending)
    └─ Stream 5: Producer → GATE APPROVAL (#5 pending)
         ↓ (upon PASS)
Stage C: Integration & Validation (PENDING ⏳)
    ├─ Level Designer: integrate assets into Crash Site scene (#6 pending)
    ├─ Gameplay Engineer: validate mechanics (#7 pending)
    ├─ QA: visual + gameplay smoke test (#7 pending)
    ├─ Visual Reviewer: final composition approval (#7 pending)
    └─ Producer: GATE APPROVAL (#8 pending)
         ↓ (upon PASS)
Release: Windows Export & Deployment (PENDING ⏳)
    ├─ Build Release Engineer: export & validate artifact
    └─ Producer: FINAL GATE APPROVAL + Human sign-off
         ↓ (upon GO)
SHIP: Deploy MVP to public (awaits all gates)
```

---

## Success Metrics

**Stage A Success:** ✓ ACHIEVED
- Visual identity is locked and enforceable
- Environment direction is documented
- All briefs are ready
- Producer gate approved

**Stage B Success:** 🔄 IN PROGRESS (Target: 7–14 days)
- 10 Blender candidates generated
- PNG evidence bundles created
- Visual Reviewer approves all candidates
- Technical Director confirms feasibility
- Producer gates advancement to Stage C

**Stage C Success:** ⏳ PENDING (Target: 3–7 days after Stage B)
- Assets integrated into scene
- Gameplay validated
- Visual composition coherent
- QA smoke test passes
- Producer gates advancement to Release

**Release Success:** ⏳ PENDING (Target: 1–2 days after Stage C)
- Windows build exports successfully
- Performance targets met (60 FPS)
- Release artifact is auditable
- Producer approves shipment
- Human approves deployment

**Final Success:** MVP Crash Site visual experience ships on Windows with locked scope, validated visual quality, and locked gameplay.

---

## Next Actions (By Priority)

### Immediate (This Week)
1. **Task #1 (Art Director):** Generate Blender candidates for 10 briefs
   - Reference Stage A material palette and asset languages
   - Ensure silhouettes read in neutral grey
   - Export GLB candidates

2. **Asset Manifest:** Document each candidate (brief ref, hash, materials, provenance)

### Short-term (When Task #1 Completes)
3. **Task #2 (Visual Artifact Factory):** Generate PNG bundles from GLB exports
4. **Task #3 (Visual Reviewer):** Open PNGs and provide visual diagnosis
5. **Task #4 (Tech Director):** Test GLB imports and performance

### Medium-term (When Tasks #3 & #4 Complete)
6. **Task #5 (Producer):** Review all evidence and issue gate verdict
   - If PASS → unlock Task #6 (integration)
   - If NOT_GO → return Task #1 to rework

### Long-term (Upon Stage B PASS)
7. **Task #6–#8:** Integration, validation, release

---

## Document Authority & Sign-Off

**Created:** 2026-07-06  
**Prepared By:** Agent Studio Orchestrator  
**Authority References:** README.md, AGENTS.md, studio/agents/, studio/memory/  
**Gate:** Producer (approval of advancement decisions)  
**Status:** ACTIVE — Production in progress

**Current Phase:** Stage B (Task #1 IN_PROGRESS)  
**Next Gate:** Stage B completion (Task #5)  
**Target Ship Date:** Dependent on Stage B + C timelines; estimate 3-4 weeks from Stage A completion

---
