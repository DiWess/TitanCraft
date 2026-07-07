# Visual Scope & Design Inventory — 2026-07-07

## Purpose

A human asked the Agent Studio to enumerate all remaining visual scope and design work. Per `CLAUDE.md` §3 and §9, Claude Code's role here is reviewer/architecture-validator, not the Art Director agent that owns visual production — so this document is an **evidence-audited inventory**, not a plan to execute. It cross-checks every visual-production claim under `docs/art/`, `docs/production/`, and `studio/` against what actually exists in the repository (scenes, assets, manifests, hashes), and organizes the result into what a Producer can act on next.

Status labels below (`VERIFIED`, `CLAIMED-ONLY`, `PENDING`, `BLOCKED`) are descriptive categories for this audit. They are not the approved Agent Studio verdict vocabulary (`PASS` / `GO` / `NOT_GO` / etc. — see `AGENTS.md` §5) and must not be quoted as if they were.

## Top finding: a fabricated "Stage B / Ship Ready" document cluster

Eight files tell an internally consistent story that goes from Stage A lock straight through Windows-export/public-deployment approval, all self-labeled `(Simulation: Task #N Completion)`:

- `docs/art/ASSET_MANIFEST_V1.md` — invents 10 Stage B candidates (`TC_CrashHull_V1`, `TC_GalaxabrainScout_V1`, `TC_MechanicalArm_V1`, `TC_Workbench_V1`, `TC_Beacon_V1`, etc.)
- `docs/art/VISUAL_REVIEWER_VERDICT_V1.md` — "all 10 PASS"
- `docs/art/TECHNICAL_DIRECTOR_AUDIT_V1.md` — invented FPS/draw-call/GPU-ms numbers, no cited artifact
- `docs/art/PRODUCER_GATE_VERDICT_STAGE_B.md` — "PASS — ADVANCE TO STAGE C"
- `docs/art/LEVEL_DESIGNER_INTEGRATION_REPORT.md` and `docs/art/FINAL_VALIDATION_REPORT.md` — both reference `src/Scenes/CrashSite.tscn`
- `docs/art/PRODUCER_RELEASE_GATE_VERDICT.md` — "GO — APPROVED FOR WINDOWS EXPORT & PUBLIC DEPLOYMENT," dated 2026-07-08
- `studio/tasks/PRE_BETA_AUDIT_COMPLETE.md` — "BETA_READY — ZERO DRIFT DETECTED"

**Independently verified against the actual repository (2026-07-07):**
- `src/Scenes/CrashSite.tscn` does not exist anywhere in the repo. The only real main scene is `scenes/Main/Main.tscn`.
- None of the five named Stage B candidate files (`TC_CrashHull_V1`, `TC_GalaxabrainScout_V1`, `TC_MechanicalArm_V1`, `TC_Workbench_V1`, `TC_Beacon_V1`, any extension) exist anywhere in the repo.

`docs/production/current-status.md` already flags `studio/tasks/PRE_BETA_AUDIT_COMPLETE.md` alone as unverified. It does not flag the other seven files that share the same fabricated storyline and reference the same nonexistent scene/assets. **Recommendation: the Producer should treat all eight files above as non-authoritative**, and decide whether to delete them, or move and clearly re-label them under `studio/rehearsals/` (the repo's real mechanism for intentional practice/simulation exercises, per `studio/README.md` "How to Add a Rehearsal") so they can't be mistaken for real production status again. This document does not modify or remove them — that's a call for a human/Producer.

## Verified (real evidence, cross-checked against disk)

| Item | Evidence | Caveat |
|---|---|---|
| `TC_TerrainDioramaKit_V1` (Stage A terrain) | `docs/art/reviews/stage-a-visual-approval-verdict.md` (PASS, 2026-07-07), `docs/production/stage-a-production-integration-signoff.md`, hashes in `artifacts/asset-review/TC_TerrainDioramaKit_V1/sha256sums.txt`; `StageAVisualRoot` node confirmed at `scenes/Main/Main.tscn:1037` | The review PNGs and source `.blend`/`.glb` are intentionally untracked local/CI artifacts (`.gitignore`); only the text manifests/hashes/sign-off docs are committed, so the PASS can't be independently re-rendered without regenerating those binaries. |
| `TC_HullRibOccluder_V1` | `assets/Production/Custom/StageA/TC_HullRibOccluder_V1.obj` (committed as text OBJ), `artifacts/asset-review/TC_HullRibOccluder_V1/review_metadata.md` | Independently verifiable right now — the OBJ itself is tracked, unlike most Stage A binaries. |
| MVP Pack V1 gameplay assets (Workbench, Beacon dormant/active, Save Point, Mechanical Arm, Galaxabrain Scout, 4 pickups) | `docs/art/execution-guides/phase-4-6-execution-guide.md`, `docs/art/status/phase-4-6-asset-status.md`; `.gltf` files under `assets/models/mvp_pack_v1/`, `.glb` under `assets/Production/Generated/MVP_Pack_V1/`; scenes `scenes/World/Workbench.tscn`, `scenes/World/Beacon.tscn`, `scenes/Player/Player.tscn`, `scenes/Enemies/GalaxabrainScout.tscn` all exist | This is the real, gameplay-integrated asset layer — distinct from both the Stage A Blender-Forge terrain work and the fictional Stage B cluster above. |
| Technical stability (quality-benchmark axis 8) | `docs/production/quality-scorecard-log.md`: `dotnet build` 0 errors, `dotnet test` 71/71, integration milestones passing | Most trustworthy document in the corpus — explicitly declines to claim visual/feel quality without human evidence, and self-corrected a prior transcription error. Independently re-confirmed in this session: `dotnet build` 0 warnings/errors, `dotnet test` 71/71 passed. |

## Claimed-only / unverified (real docs, but evidence is missing, broken, or admittedly weak)

- `TC_HeavyCrashHull_V1` — brief exists (`docs/art/briefs/TC_HeavyCrashHull_V1.md`); manifest entry shows technical/import validation only (`TC_HEAVY_CRASH_HULL_V1_REVIEW_ARTIFACTS_READY`), explicitly not human/visual-reviewer approved. `current-status.md` and `known-blockers.md` agree it still needs standalone review — consistent, just unresolved.
- V1 Beta visual pass (`docs/art/titancraft-v1-beta-visual-bible.md`) — its "evidence" is an SVG + Markdown text workaround, not real PNGs, because PNG capture wasn't available at the time. The doc says so itself; not hidden, but weak.
- `docs/art/reviews/agent-studio-visual-valuation-2026-07-07.md` is worth naming as the opposite of the fabricated cluster: it opens real PNGs (`artifacts/mvp_closure/playthrough/*.png`) and issues an honest **NOT_GO** on final environment art quality (flat slabs, blockout walls, prototype materials), passing only a narrower "mission-state readability" claim.

## Pending / not started

- Phase 8–9 (lighting polish, marketing-grade artifact factory) — `docs/art/PHASE_8_EXECUTION_GUIDE.md`, `PHASE_9_EXECUTION_GUIDE.md`, status `READY_FOR_EXECUTION`, blocked on Phase 7. Both reference `scenes/CrashSite_MVP.tscn`, which also does not exist on disk.
- Phase 7 (composition/balance/audio/polish/platform testing) — `docs/production/phase-7-planning.md`, blocked on Phase 6 completion.
- Windows hardware playtest/export proof — `artifacts/mvp_closure/20260703_windows_manual_validation_blocked.md`, explicitly `HUMAN_BLOCKED`/`ENVIRONMENT_BLOCKED`.

## Blocked (explicit, current)

- Real Stage B orchestration (`docs/art/STAGE_B_ORCHESTRATION_BRIEF.md`, tracked honestly in `docs/production/PRODUCTION_PROGRESSION_DASHBOARD.md` as "IN_PROGRESS") — blocked pending a Producer re-gate per `current-status.md`.
- Marketing screenshots / public visual-readiness claims.
- Any further Stage A visual replacement lacking standalone review artifacts.

## Contradictions to reconcile

1. **Fabricated Stage B/Ship-Ready cluster vs. the real dashboard** — see Top Finding above.
2. **Same-day (2026-07-07) disagreement**: `docs/production/quality-scorecard-log.md`'s latest entry still scores axis 6 (visual art) at 5.5/10 citing "Stage A visual approval remains blocked," while `current-status.md`'s "Stage A Reconciliation — 2026-07-07" section grants PASS for the `TC_TerrainDioramaKit_V1` slice specifically. These two same-day documents haven't been cross-referenced with each other yet.
3. **Stale blocker line**: `docs/production/known-blockers.md:3` reads "Stage A visual art is not approved" — superseded (for the terrain-diorama slice only) by `current-status.md`'s reconciliation note, but the line itself hasn't been narrowed or dated.
4. **Ambition vs. locked README scope**: `docs/art/VISUAL_UPGRADE_EXECUTION_2026-07-05.md`, `titancraft-visual-identity.md`, and `STAGE_A_ART_BRIEF_PACKET.md` describe a marketing-grade, 10-axis quality bar with quantified material science. `README.md` §15 only requires "science-fiction réaliste simplifiée" and explicitly permits clearly-labeled placeholder primitives, saying not to delay gameplay for final assets. Not forbidden, but worth flagging as scope-creep risk per `CLAUDE.md` §6.
5. **Naming drift across three asset generations** with no single index: `v1_beta` (`TC_PROP_Workbench_01`), `mvp_pack_v1` (`TC_PROP_Workbench_V1`), and Stage-A/B Blender-Forge (`TC_TerrainDioramaKit_V1`, `TC_HeavyCrashHull_V1`, `TC_LightingReference_V1`). `docs/art/status/phase-4-6-asset-status.md` "Issue 1" notes the first two don't interfere, but a reader skimming `docs/art/` without that note can easily misattribute evidence between generations — exactly the ambiguity the fabricated cluster's asset names exploit.

## Recommended next actions for the Producer

1. Decide the disposition of the eight fabricated-cluster files (delete, or relabel/relocate to `studio/rehearsals/`).
2. Reconcile `known-blockers.md:3` and the 2026-07-07 scorecard-vs-reconciliation gap so there is one current answer to "is Stage A approved."
3. Only then consider a Stage B re-gate decision — it should cite the real dashboard/orchestration brief, not the fabricated cluster.
4. `TC_HeavyCrashHull_V1` needs its own standalone human/Visual-Reviewer review before production use — next concrete, boundable art task.

## Explicitly out of scope for this document

No new assets were generated, no scenes were edited, and no existing `docs/art/` or `studio/` files were modified, moved, or deleted. This is a read-only audit produced at a human's request, consistent with `docs/production/current-status.md`'s "Unblocked" list ("Documentation and production-operation updates requested by a human").
