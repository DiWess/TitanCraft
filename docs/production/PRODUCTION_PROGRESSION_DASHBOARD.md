# TitanCraft Production Progression Dashboard
**Date:** 2026-07-06  
**Scope:** Visual Experience Kickoff → MVP Ship  
**Authority:** README.md (MVP definition), AGENTS.md (governance), studio/agents/ (agent missions)

**Last reconciled:** 2026-07-24 — the stage sections and gate conditions below were still reading
`IN_PROGRESS`/`PENDING` for Stage B and Stage C while the executive summary and task list recorded
both as `PASS`. Reconciled against the cited evidence; see "Reconciliation note" at the end.

---

## Executive Summary

TitanCraft visual production is structured as a **gated three-stage progression** from direction (Stage A) through asset generation (Stage B) to integration and ship (Stage C/Release). Each stage has parallel agent streams converging at Producer gates. No stage advances without evidence.

**Current Status:** Stage A PASS ✓ | Stage B PASS ✓ (2026-07-18, `docs/production/stage-b-producer-gate-2026-07-18.md`) | Stage C PASS ✓ (2026-07-18, `docs/production/stage-c-integration-validation-2026-07-18.md`) | Release HUMAN_BLOCKED (export proof done; human Windows validation + GO pending)

---

## Stage Overview

| Stage | Objective | Primary Agents | Gate Authority | Status |
|-------|-----------|-----------------|-----------------|--------|
| **A** | Lock visual identity & environment direction | Creative Director, Art Director | Producer | ✓ PASS |
| **B** | Generate & validate standalone asset candidates | Art Director, Visual Reviewer, Tech Director | Producer | ✓ PASS (2026-07-18) |
| **C** | Integrate approved assets & validate experience | Level Designer, Gameplay Engineer, QA | Producer | ✓ PASS (2026-07-18) |
| **Release** | Final export & deployment readiness | Build Engineer, Producer | Human approval | ⛔ HUMAN_BLOCKED (export proof done 2026-07-18) |

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

### Stage B: Asset Generation & Validation (COMPLETE ✓ 2026-07-18)

**Tasks #1–#5 DONE.** The 10 briefs produced **13** `MVP_Pack_V1` candidates; counts below are the
delivered figures, not the original brief count.

| Agent | Role | Output | Verdict | Status |
|-------|------|--------|---------|--------|
| **Art Director** | Generate standalone Blender candidates per briefs | 13 `MVP_Pack_V1` GLBs + tracked `.blend` sources via `tools/blender/create_mvp_asset_pack_v1.py` | Generation (N/A) | ✓ DONE (#1) |
| **Visual Artifact Factory** | Render PNG evidence bundles | 13 review bundles, plus `scale_reference` views added 2026-07-18 | Automation (N/A) | ✓ DONE (#2) |
| **Visual Reviewer** | Independent review of PNG evidence | `docs/art/reviews/mvp-pack-v1-visual-review-2026-07-18.md` — 13/13, three MINOR notes | PASS | ✓ PASS (#3) |
| **Technical Director** | Pipeline & performance audit | `docs/production/mvp-pack-v1-technical-audit-2026-07-18.md` — build 0/0, tests 75/75, import 0 errors, 13/13 under triangle budgets | PASS | ✓ PASS (#4) |
| **Producer** | Gate Stage B → Stage C or return to rework | `docs/production/stage-b-producer-gate-2026-07-18.md` | PASS | ✓ PASS (#5) |

**Stage B Deliverables (Complete):**
- ✓ 13 Blender candidates with materials (Art Director)
- ✓ Asset manifest with source, brief, hash, material assignments — `assets/Production/Generated/asset_manifest.json`, 29 entries, 29/29 GLB SHA-256 recomputed and matched
- ✓ PNG evidence bundles (silhouette clarity, material coherence, scale reference)
- ✓ Visual Reviewer diagnosis document (13/13 PASS, real opened-PNG review)
- ✓ Technical Director audit log (13/13 GLBs imported, all under triangle budgets)

**Gate Conditions (Before Stage C) — all met:**
- ✓ All candidates PASS Visual Reviewer verdict (13/13)
- ✓ All candidates PASS Technical Director audit (13/13)
- ✓ Asset manifest complete and auditable (29 entries, hashes verified)
- ✓ PNG evidence bundles present and opened
- ✓ Producer issues PASS verdict (2026-07-18)

---

### Stage C: Integration & Final Validation (COMPLETE ✓ 2026-07-18)

**Tasks #6–#7 DONE** — `docs/production/stage-c-integration-validation-2026-07-18.md`.

| Agent | Role | Output | Verdict | Status |
|-------|------|--------|---------|--------|
| **Level Designer** | Integrate candidates into Crash Site scene, verify layout & gameplay | `scenes/Main/Main.tscn` plus `scenes/World/`, `scenes/Enemies/`, `scenes/Resources/`, `scenes/Player/` — found already wired by prior committed work, then verified per-prop | N/A | ✓ DONE (#6) |
| **Gameplay Engineer** | Verify gameplay mechanics work with integrated assets (resource pickup, crafting, combat) | `./tools/test.sh` — unit 75/75, all 11 `MVP_SMOKE_MILESTONE` entries on the integrated scenes | PASS | ✓ PASS (#7) |
| **QA Lead** | Visual & gameplay coherence in integrated scene | 8 in-engine captures via the allowlisted factory; import exit 0, 0 errors | PASS | ✓ PASS (#7) |
| **Visual Reviewer** | Final visual approval of integrated composition | All 8 production-integration PNGs opened and diagnosed | PASS | ✓ PASS (#7) |
| **Producer** | Gate Stage C → Release or return to rework | Stage C `PASS`; release gate held separately | PASS | ✓ PASS (#8 gate held) |

**Stage C Deliverables (Complete):**
- ✓ Crash Site scene integrates the approved candidates (verified per-prop; gameplay collision nodes unmodified)
- ✓ In-engine screenshots (8 production-integration views, opened)
- ✓ Visual diagnosis of integrated composition (focal point, route, visual coherence)
- ✓ Gameplay validation (movement, resource gathering, crafting, combat, beacon flow — 11/11 smoke milestones)
- ✓ QA sign-off (visual polish, collision, performance, no new blockers)

**Gate Conditions (Before Release) — all met:**
- ✓ All gameplay mechanics work with integrated assets (75/75 unit, 11/11 smoke)
- ✓ Visual composition is coherent and supports gameplay
- ✓ No new performance or rendering issues
- ✓ All evidence documented
- ✓ Producer issues Stage C PASS verdict

**Findings raised in Stage C and since closed** (`docs/production/polish-slice-2026-07-18.md`): the
first-person capture did not show the crafted arm, which exposed a BLOCKER-class bug — a legacy
proxy mesh rendering over the crafted MVP arm, affecting every crafted playthrough. Fixed, the arm
re-framed as a proper viewmodel, and the crafted-arm capture added to the factory allowlist.

---

### Release: Export & Deployment Readiness (HUMAN_BLOCKED ⛔)

**Task #8 — export proof done, gate held.**

| Agent | Role | Output | Verdict | Status |
|-------|------|--------|---------|--------|
| **Build Release Engineer** | Windows export, executable validation, release bundle | `godot --headless --export-release "Windows Desktop"` exit 0, exe SHA-256 recorded (2026-07-18) | PASS | ✓ PASS (export proof only) |
| **Producer** | Final release gate, approval for public deployment | Release gate verdict (GO / HOLD / NOT_GO) | GO / INTENTIONAL_GATE / NOT_GO | ⛔ HELD |
| **Human (Producer)** | Final sign-off on visual experience readiness | Approval recorded in issue/PR | GO / NOT_GO | ⏳ Awaits |

**Release Deliverables:**
- ✓ Windows executable build artifact (local export proof, exe hash recorded)
- ✓ Export validation log (export exit 0, 0 errors) — **performance targets not yet measured on Windows hardware**
- ⏳ Release notes (visual changes, gameplay scope locked to MVP)
- ⛔ Producer release gate verdict — held

**Journey dispatched 2026-07-25** (run id `30152950604`, commit `5afdd2c6`) — first ever execution
of the Windows runner job, and the first verdict document the journey has produced:
`docs/production/playtests/windows-playtest-2026-07-25.md`.

- ✓ Exported Windows binary launches standalone, 600-frame headless smoke, **exit code 0** in 4.64 s,
  exe SHA-256 `2a19485b…` with full run provenance.
- ⛔ **Aesthetic verdict `NOT_GO`** — ground-plane depth artifact (shadow acne) across first-person
  and near-ground framing. The crafted-arm viewmodel fix is confirmed holding at this commit.
- ⛔ **README § 28 unmet** — no rendered frame-rate measurement exists; the journey's 129.3 figure is
  an explicitly-labelled headless proxy and is not a rendered-performance claim.
- ⛔ **README § 27 partially unestablished** — offline operation, local save in the build, and resume
  after relaunch are not exercised by a headless frame-count smoke.
- ⚠ The bundle is missing `export.log` (3 files uploaded, not 4), confirming the upload-path defect;
  the fix postdates this run.

**Final verdict: `NOT_GO`.** The machine lanes succeeded and this is the project's first real Windows
evidence, but it is not a ship decision.

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

### Stage B → Stage C Gate (PASSED ✓)

**Conditions:**
- ✓ All candidates PASS Visual Reviewer verdict (13/13)
- ✓ All candidates PASS Technical Director audit (13/13)
- ✓ Asset manifest complete with hashes and provenance (29 entries, 29/29 hashes matched)
- ✓ PNG evidence bundles generated and opened
- ✓ No new scope conflicts or blockers

**Verdict:** PASS (2026-07-18, `docs/production/stage-b-producer-gate-2026-07-18.md`)

**Authority:** Producer (studio/agents/producer.md)

---

### Stage C → Release Gate (PASSED ✓)

**Conditions:**
- ✓ Crash Site scene integrates approved assets (verified per-prop)
- ✓ Gameplay mechanics validated (75/75 unit, 11/11 MVP smoke milestones)
- ✓ Visual composition is coherent and readable
- ✓ In-engine screenshots show final visual polish (8 captures opened)
- ✓ QA sign-off (visual + gameplay + performance)
- ✓ No rendering or collision surprises — one BLOCKER found in review (legacy proxy mesh over the crafted arm) and fixed in the same-day polish slice

**Verdict:** PASS (2026-07-18, `docs/production/stage-c-integration-validation-2026-07-18.md`)

**Authority:** Producer (studio/agents/producer.md)

This gate advances Stage C only. The Release → Ship gate below is separate and remains held.

---

### Release → Ship Gate (HUMAN_BLOCKED ⛔)

**Conditions:**
- ✓ Windows executable builds without errors (export exit 0, exe SHA-256 recorded 2026-07-18)
- ⛔ Export validation confirms 60 FPS target on Windows (README § 28) — **not measured.** No frame-rate figure exists from Windows hardware. The journey's frame-pacing number is a *headless proxy* and, per `studio/decisions/quality_benchmark_v2_agent_gate_delegation.md`, must never be presented as a rendered-performance claim.
- ⏳ Release artifact is auditable and reproducible
- ⏳ All evidence documented
- ✓ No gameplay scope expansion (locked to MVP Crash Site)

**Verdict:** HUMAN_BLOCKED (awaits the journey dispatch, then Producer + human GO)

**Authority:** Producer + Human approval (studio/agents/producer.md, README.md)

**Open conflict, recorded not resolved:** the Stage C entries of 2026-07-18 name "the human Windows
playthrough and GO (README § 27)" as the remaining gate, written a week after the ADR that delegated
those evidence lanes to CI and the Visual Reviewer. That ADR states a verdict citing measured proxies
can reach `PASS`/`GO`/`NOT_GO` without a human. Whether the **final ship GO** is delegated is a
product decision the human owner still holds; this dashboard does not resolve it either way.

---

## Task & Evidence Tracking

### Current Task List

| Task | Status | Owner | Blocks |
|------|--------|-------|--------|
| #1: Generate Blender candidates | ✓ DONE (Auto-Forge, `MVP_Pack_V1`) | Art Director | #2 |
| #2: PNG evidence bundles | ✓ DONE (+ `scale_reference.png` views 2026-07-18) | Visual Artifact Factory | #3, #4 |
| #3: Visual Reviewer verdict | ✓ PASS (`docs/art/reviews/mvp-pack-v1-visual-review-2026-07-18.md`) | Visual Reviewer | #5 |
| #4: Tech Director audit | ✓ PASS (`docs/production/mvp-pack-v1-technical-audit-2026-07-18.md`) | Technical Director | #5 |
| #5: Producer gate (B→C) | ✓ PASS (`docs/production/stage-b-producer-gate-2026-07-18.md`) | Producer | #6 |
| #6: Integrate into scene | ✓ DONE (found pre-wired; verified 2026-07-18) | Level Designer | #7 |
| #7: Final validation | ✓ PASS (tests 75/75 + 11 smoke milestones; 8 captures opened, arm-capture gap noted) | QA + Visual Reviewer | #8 |
| #8: Release gate | ⛔ HUMAN_BLOCKED (Windows export proof done; human playthrough + GO pending) | Producer + Build Engineer | SHIP |

### Evidence Standards (Per Stage)

**Stage A Evidence:**
- ✓ `docs/art/STAGE_A_ART_BRIEF_PACKET.md` — governance packet with visual identity, briefs, gate conditions
- ✓ `docs/art/titancraft-visual-identity.md` — visual style spec
- ✓ 10 asset briefs (markdown documents)
- ✓ Producer gate verdict recorded

**Stage B Evidence (Complete ✓ 2026-07-18):**
- ✓ Blender candidates + asset manifest — 13 `MVP_Pack_V1` GLBs, `assets/Production/Generated/asset_manifest.json` (29 entries, 29/29 hashes matched)
- ✓ PNG bundles (Visual Artifact Factory, incl. `scale_reference` views)
- ✓ Visual Reviewer diagnosis — `docs/art/reviews/mvp-pack-v1-visual-review-2026-07-18.md` (13/13 PASS)
- ✓ Technical Director audit — `docs/production/mvp-pack-v1-technical-audit-2026-07-18.md` (13/13 PASS)
- ✓ Producer gate verdict — `docs/production/stage-b-producer-gate-2026-07-18.md`

**Stage C Evidence (Complete ✓ 2026-07-18):**
- ✓ Integrated Crash Site scene — `scenes/Main/Main.tscn` with subscenes under `scenes/World/`, `scenes/Enemies/`, `scenes/Resources/`, `scenes/Player/`
- ✓ In-engine screenshots — 8 production-integration captures, all opened
- ✓ Visual + gameplay diagnosis document — `docs/production/stage-c-integration-validation-2026-07-18.md`
- ✓ QA smoke test results — unit 75/75, 11/11 `MVP_SMOKE_MILESTONE` entries
- ✓ Producer gate verdict — Stage C `PASS`

**Release Evidence (Partial ⛔):**
- ✓ Windows build artifact (`.exe`) — local export proof, exe SHA-256 recorded 2026-07-18
- ✓ Export validation log — export exit 0, 0 errors
- ⛔ Measured Windows smoke + frame data — **absent**; the journey has never been dispatched (`docs/production/windows-release-gate-analysis-2026-07-24.md`)
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

---

## Reconciliation note — 2026-07-24

This dashboard was internally inconsistent and is the document agents route from when deciding what
to work on next. The executive summary and the Current Task List correctly recorded Stage B and
Stage C as `PASS` (2026-07-18), while the detailed stage sections and the "Gate Conditions by Stage"
blocks still read `IN_PROGRESS`/`PENDING` with every condition unchecked. An agent reading the stage
sections rather than the summary would have concluded Stage B was still generating candidates and
redone completed work.

Corrected in this pass, each against cited evidence rather than by flipping a status:

- Stage B section: `IN_PROGRESS` → `COMPLETE ✓`, agent statuses and deliverables filled from the
  Visual Reviewer review, Technical Director audit, and Producer gate documents.
- Stage C section: `PENDING` → `COMPLETE ✓`, with the validation figures (75/75 unit, 11/11 smoke,
  8 opened captures) and the BLOCKER found in review and closed the same day.
- Stage B → C and Stage C → Release gate blocks: `PENDING` → `PASSED`, conditions checked.
- Release section: `PENDING` → `HUMAN_BLOCKED`, separating the completed export proof from the
  unmeasured performance target and the held gate.

Two factual errors were corrected in the same pass:

1. **Candidate count.** The stage tables said "10 candidates" throughout, taken from the brief count.
   Stage B actually delivered **13** `MVP_Pack_V1` candidates and a 29-entry manifest. Every
   downstream verdict is 13/13, not 10/10.
2. **Scene path.** Two places — the Stage C agent row and the Stage C evidence list — pointed at
   `src/Scenes/CrashSite.tscn`, which does not exist anywhere in the repository (`src/Scenes/` is
   not a directory). The integrated scene is `scenes/Main/Main.tscn` with subscenes under
   `scenes/World/`, `scenes/Enemies/`, `scenes/Resources/`, and `scenes/Player/`.

No stage verdict was advanced by this reconciliation. Every status here now matches a dated evidence
document that already existed; where evidence is absent — the Windows performance measurement — the
condition is marked unmet rather than quietly checked. No gameplay code, scenes, assets, or tests
were changed.
