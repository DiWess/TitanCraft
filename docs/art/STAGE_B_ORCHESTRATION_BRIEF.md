# Stage B Orchestration Brief
**Status:** ACTIVE — All Agents Parallel Engagement  
**Authority:** AGENTS.md § 3 (global agent behavior), studio/agents/art_director.md (primary)  
**Date:** 2026-07-06  
**Gate:** Producer (approval required at Stage B conclusion)

---

## Stage B Objective

Generate, validate, and gate-lock standalone asset candidates from Stage A briefs. Stage B work runs in parallel across four independent agent streams, converging at Producer gate (Task #5).

**Success Condition:** All PNG evidence, Visual Reviewer verdict, and Technical Director feasibility audit are PASS. Producer gates advancement to Stage C (integration).

---

## Parallel Agent Streams

### Stream 1: Asset Candidate Generation (Art Director + Asset Librarian)
**Task:** #1 (in_progress)  
**Duration:** 4-8 hours (staggered across 1-2 weeks)  
**Deliverable:** 10 standalone Blender candidates + asset manifest

#### Scope (In)
- Generate Blender `.blend` files for each Stage A brief
- Reference material palette and asset language (human/alien/terrain)
- Export GLB candidates ready for pipeline testing
- Document each candidate: source, brief reference, asset class, material assignments
- Build asset manifest: filenames, hashes, briefs, provenance notes

#### Scope (Out, Forbidden)
- Do not modify production Crash Site scene
- Do not integrate into scene yet (Stage C task)
- Do not invent scope beyond Stage A briefs
- Do not claim visual approval without review

#### Candidates (10 Total)
1. **Crash Hull** — wreckage silhouette, salvage aesthetic
2. **Terrain Basin** — volcanic rock, readable slopes, shaped terrain
3. **Scout Enemy** — alien threat silhouette, asymmetrical biomorphic form
4. **Mechanical Arm** — human salvage tech, modular, craftable appearance
5. **Workbench** — craft station, industrial human tech aesthetic
6. **Beacon** — victory objective, human salvage, functional marker
7. **Resource Pickups** — three resource types, readable affordances
8. **Save Point** — safe zone marker, human tech aesthetic
9. **Lighting Reference** — functional glow samples (minimal, not decorative)
10. **Polish Details** — surface details, wear textures, material samples

#### Acceptance Criteria (Before Stream 2)
- ✓ Each candidate has `.blend` source file with materials
- ✓ GLB export ready and tested in pipeline
- ✓ Brief reference documented for each candidate
- ✓ Asset manifest complete with hashes and provenance
- ✓ Material assignments follow Stage A palette (albedo, roughness, metalness tables)
- ✓ Silhouettes are readable in neutral grey (no material dependency)
- ✓ No toy-like proportions, photorealism, or scope expansion

---

### Stream 2: PNG Evidence Generation (Visual Artifact Factory)
**Task:** #2 (pending — blocked by #1)  
**Duration:** 1-2 hours (runs in CI after Stream 1 exports)  
**Deliverable:** PNG review bundles for each asset candidate

#### Scope (In)
- Render each GLB candidate as PNG from neutral grey and material-applied angles
- Silhouette clarity proof (neutral grey angle)
- Material coherence proof (textured angle)
- Scale reference proof (comparison with readable human-scale object)
- Batch export as review bundle per brief

#### Scope (Out)
- Do not edit candidates
- Do not generate final scene renders yet (Stage C)
- Do not skip any candidate

#### PNG Evidence Standard (Per Candidate)
| View | Purpose | Standard |
|------|---------|----------|
| Neutral Grey (50m distance) | Silhouette clarity | Readable without material or glow |
| Textured (50m distance) | Material coherence | Palette adherence, no excessive glow |
| Scale Reference | Size confirmation | Human-scale reference object included |
| Damage/Wear Detail (close) | Surface coherence | Repair marks, patching, wear visible |

#### Acceptance Criteria (Before Stream 3+4)
- ✓ PNG bundle generated for each of 10 candidates
- ✓ Each PNG pair (neutral + textured) rendered at same angle
- ✓ File naming convention: `CANDIDATE_[name]_silhouette.png`, `CANDIDATE_[name]_material.png`
- ✓ PNGs accessible in shared evidence directory or artifact factory output
- ✓ No visual post-processing or filtering (raw render output)

---

### Stream 3: Visual Reviewer Independent Verdict
**Task:** #3 (pending — blocked by #2)  
**Duration:** 2-4 hours  
**Deliverable:** Visual verdict on each candidate (PASS / NOT_GO + diagnosis)

#### Scope (In)
- Open each PNG pair and inspect for visual coherence
- Diagnose focal point, route readability (where applicable), silhouette, scale, material, glow
- Compare against Stage A rejection patterns (toy-like, photorealism, excessive glow, etc.)
- Return individual verdict per candidate: PASS or NOT_GO with specific failure reason
- Return summary verdict: X of 10 candidates PASS; list NOT_GO blockers

#### Scope (Out)
- Do not self-approve (review must be independent of Art Director)
- Do not edit candidates
- Do not claim approval without visual diagnosis document

#### Visual Diagnosis Template (Per PNG)
```
## Candidate: [Name]

### Focal Point & Route
- [Description of what draws the eye, whether routes/edges read clearly]

### Silhouette
- [Readable in neutral grey? Proportions correct? Scale appropriate?]

### Material & Scale
- [Palette adherence? Roughness/metalness correct? Scale refs visible?]

### Coherence vs. Rejection Patterns
- [Any toy-like, photorealistic, toy sci-fi, excessive glow, or paper-thin issues?]

### Verdict
- PASS / NOT_GO (and specific blocker if NOT_GO)
```

#### Acceptance Criteria (Before Producer Gate)
- ✓ Diagnosis document exists for each candidate
- ✓ Each diagnosis names focal point, silhouette clarity, scale, material coherence
- ✓ Independent review (not Art Director self-approval)
- ✓ Summary verdict: count PASS vs. NOT_GO
- ✓ Blocker list (if any NOT_GO verdicts)

---

### Stream 4: Technical Director Feasibility Audit
**Task:** #4 (pending — blocked by #2)  
**Duration:** 2-4 hours  
**Deliverable:** Technical verdict on integration readiness (PASS / NOT_GO + blockers)

#### Scope (In)
- Test Blender → GLB export pipeline for each candidate
- Verify GLB loads in Godot 4 .NET without errors
- Audit material import (albedo, roughness, metalness mapping)
- Estimate draw calls and performance impact (target 60 FPS on Windows, baseline ~500m view distance)
- Confirm scale matches brief specifications
- Verify no missing materials, textures, or references in GLB

#### Scope (Out)
- Do not integrate into production scene yet (Stage C)
- Do not optimize or rebuild candidates
- Do not claim gameplay readiness (separate gate)

#### Technical Audit Checklist (Per Candidate)
- ✓ GLB exports without errors
- ✓ Godot import succeeds (no shader errors, missing textures)
- ✓ Material nodes map correctly (PBR standard)
- ✓ Performance estimate acceptable (draw call budget per object)
- ✓ Scale matches brief (measurement validation)
- ✓ No asset or reference warnings in Godot import log

#### Acceptance Criteria (Before Producer Gate)
- ✓ Audit log exists for each candidate (import test, performance estimate, blockers)
- ✓ All 10 candidates pass import test
- ✓ Performance estimates confirm 60 FPS target is achievable
- ✓ Summary verdict: PASS (all candidates ready for integration) or NOT_GO (list specific blockers)

---

## Convergence: Producer Gate (Task #5)

**When All Streams Report:**
- Stream 1: 10 candidates + manifest ready
- Stream 2: PNG bundles generated
- Stream 3: Visual Reviewer verdict (PASS / NOT_GO per candidate)
- Stream 4: Technical audit (PASS / NOT_GO per candidate)

**Producer Reviews:**
1. All 10 candidates have PASS verdicts from both Visual Reviewer and Technical Director
2. No blocking scope conflicts (candidates match Stage A briefs, no forbidden MVP features)
3. Asset manifest is complete and auditable
4. All evidence is documented and traceable

**Producer Issues:**
- **PASS:** Advance to Stage C (integration)
- **NOT_GO:** Return with specific blockers; Stream 1/3/4 return to rework and resubmit

---

## Task Dependencies & Parallel Flow

```
Stage A PASS (locked visual identity + environment)
    ↓
#1: Stream 1 (Art Director) generates Blender candidates [IN_PROGRESS]
    ↓ (upon completion)
#2: Stream 2 (Visual Artifact Factory) renders PNG bundles [PENDING]
    ↓ (upon completion)
    ├─ #3: Stream 3 (Visual Reviewer) reviews PNGs [PENDING]
    └─ #4: Stream 4 (Tech Director) audits feasibility [PENDING]
    ↓ (both #3 and #4 complete)
#5: Producer gate verdict [PENDING]
    ├─ PASS → advance to Stage C
    └─ NOT_GO → return to Stream 1 with blockers
```

---

## Stage B Success Definition

**Stage B is complete when:**
1. ✓ 10 standalone Blender candidates exist with GLB exports
2. ✓ Asset manifest documents all candidates (source, brief, hash, material assignments)
3. ✓ PNG evidence bundles generated (silhouette + material pairs)
4. ✓ Visual Reviewer verdict: all candidates PASS visual coherence check
5. ✓ Technical Director verdict: all candidates PASS Godot import and performance audit
6. ✓ Producer gate: PASS issued, no blockers remaining
7. ✓ No production scene changes (candidates isolated until Stage C)

**Stage B Failure & Return:**
- If any candidate receives NOT_GO (visual or technical), Stream 1 reworks candidate, Stream 2 regenerates PNG, Stream 3 and 4 re-review, and Producer re-gates.
- Return-to-work loop repeats until all verdicts are PASS.

---

## Agent Responsibilities & Verdicts

| Stream | Agent | Verdict Authority | Approval Required |
|--------|-------|-------------------|-------------------|
| 1 | Art Director | AGENTS.md § 5 | N/A (generation) |
| 2 | Visual Artifact Factory | CI automation | N/A (automation) |
| 3 | Visual Reviewer | studio/agents/visual_reviewer.md | Independent review (not Art Director) |
| 4 | Technical Director | studio/agents/technical_director.md | Independent technical audit |
| 5 | Producer | studio/agents/producer.md | Gate verdict (PASS / NOT_GO) |

---

## Evidence & Validation

**Before Producer Gate is Opened (#5):**
- Run `git diff --check` (no formatting violations)
- Validate asset manifest: `python3 tools/blender/build_asset_manifest.py --check`
- Collect all PNG paths and Visual Reviewer verdict document
- Collect all Technical Director audit logs
- Verify no production scene modifications

**Producer Gate Checklist:**
- ✓ Visual Reviewer verdict document exists and names all candidates
- ✓ Technical Director audit logs exist for all 10 candidates
- ✓ Asset manifest is complete and auditable
- ✓ PNG evidence bundles are accessible
- ✓ No scope conflicts detected (briefs align with Stage A identity)

---

## Next Stage (Stage C)

**Upon Producer PASS:**
- Task #6 unlocks: Integrate approved candidates into `src/Scenes/CrashSite.tscn`
- Level Designer and Gameplay Engineer position assets, verify gameplay and visual coherence
- QA conducts final validation (composition, collision, performance, visual polish)

---

## Sign-Off & Authority

**Document Created:** 2026-07-06  
**Prepared By:** Agent Studio Orchestrator (routed from Stage A PASS)  
**Gate Authority:** Producer (Stage B gate verdict required)  
**Status:** Ready for Parallel Agent Engagement

**Task #1 Status:** IN_PROGRESS  
**Task #2–#4:** PENDING (blocked by task dependencies)  
**Task #5:** PENDING (awaits all evidence)

---
