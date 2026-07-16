# Next Five Task Execution Packet — 2026-07-16 (Refresh)

Owner: Agent Studio Producer
Version: 2 (refresh of `next-five-tasks-2026-07-15.md`); see Correction below, added same day
Date: 2026-07-16
Review status: TASK_PACKET_READY
Final verdict: NOT_GO (unchanged)

## Correction (same-day, after initial publish)

The `ENVIRONMENT_BLOCKED` finding below was wrong. It was based on `which blender` returning nothing, not on an actual attempt to make Blender available. Installing it was tried afterward, in the same session: `apt-get install -y blender` succeeded (candidate `4.0.2+dfsg-1ubuntu8`, matching the version this repo's own `docs/release/evidence/titancraft-base-camp-dressing-pass-2026-07-09.md` recorded using previously), and `xvfb-run -a blender --background --python-expr "import bpy; ..."` confirmed headless `bpy` scripting actually works (`xvfb-run` was already installed). Task #1 is therefore **not** environment-blocked — it is `HUMAN_BLOCKED` only, on an Art Director judgment call about what the 8 remaining candidates should look like, which is a creative-authorship decision outside Claude Code's role (`CLAUDE.md` §3/§9: Art Director owns art asset generation; Claude Code reviews it). The rest of this document is left as originally filed, with this correction taking precedence over the environment-capability claims in "Delta since 2026-07-15" and the task table below.

## Correction 2 (same day, after Correction 1): "8 remaining candidates" was also wrong

Before acting on Correction 1's conclusion, an attempt to actually generate the "8 remaining" candidates was stopped after checking the individual asset briefs against what already exists. That check found real, already-generated deliverables for 7 of the 8:

- **Candidate #1 (Crash Hull):** already complete. `TC_HeavyCrashHull_V1` has a full standalone `PASS` review (`docs/art/reviews/heavy-crash-hull-v1-standalone-review.md`, 2026-07-10). Not scene-integrated yet, but that's a separate Stage C task, not a missing candidate.
- **Candidates #3–8 (Scout Enemy, Mechanical Arm, Workbench, Beacon, Resource Pickups, Save Point):** already complete. `docs/art/briefs/brief-{workbench,beacon,mechanical-arm,pickups,save-point,scout-enemy}-v1.md` each specify, in their own Deliverables tables, the exact same asset names and file paths (`TC_PROP_Workbench_V1`, `TC_PROP_Beacon_{Active,Dormant}_V1`, `TC_PLAYER_MechanicalArm_V1`, `TC_PICKUP_*_V1`, `TC_PROP_SavePoint_V1`, `TC_CHAR_GalaxabrainScout_V1`) that `tools/blender/create_mvp_asset_pack_v1.py` already generated, with PNG evidence, and that are already integrated into `scenes/Main/Main.tscn`. These are not placeholders a "real" Stage B pass would replace — they are the deliverables. `docs/art/reviews/mvp-asset-pack-v1-review-request.md` (filed the same session, before this correction) already routes all of them to formal review.
- **Candidate #2 (Terrain Basin):** a candidate (`TC_TERRAIN_CrashBasin_V1`) exists but was deliberately never swapped in — `docs/production/quality-scorecard-log.md` records that decision explicitly: *"that art was deliberately tuned... swapping it for a redundant candidate would be bulk replacement of working art, not a targeted fix."* `studio/decisions/procedural_terrain_deterministic_exception.md` (filed the same session) independently reaches the same conclusion from the architecture side. No generation gap here either.

**Corrected state: the only real gaps left in the entire 10-candidate Stage B list are #9 (`TC_LightingReference_V1`) and #10 (`TC_PolishDetails_V1`) needing a formal `docs/art/reviews/` verdict** — the same conclusion the "Manual checks" and "Required follow-up" sections below should have reached, had this been checked before publishing the original table. Generating 7 new candidates as the original Task #1 describes would have created confusing duplicates of assets already live in the scene. This correction takes precedence over "Delta since 2026-07-15" and the task table below, in addition to Correction 1.

## Source-of-truth check

This is a same-day re-verification of the request "do next 5 tasks" against `docs/production/next-five-tasks-2026-07-15.md`, which routed the identical phrase into the Stage B chain from `docs/production/PRODUCTION_PROGRESSION_DASHBOARD.md` (Task #1 through Task #5) with verdict NOT_GO. This refresh checks whether the underlying blockers moved in the one day since, rather than re-deriving the routing from scratch.

The packet does not authorize multiplayer, grappling hook, wall running, procedural worlds, voxels, large mech, complete rocket, multiple maps, multiple enemy types, cloud services, remote telemetry, or any other out-of-scope MVP feature.

## Delta since 2026-07-15

Checked directly against the working tree rather than assumed:

- `assets/Production/Generated/asset_manifest.json` still contains only 2 of the 10 planned Stage B candidates (`TC_LightingReference_V1` = candidate #9, `TC_PolishDetails_V1` = candidate #10), neither with a completed PNG-evidence + Visual Reviewer + Technical Director chain. Candidates #1–#8 (Crash Hull, Terrain Basin, Scout Enemy, Mechanical Arm, Workbench, Beacon, Resource Pickups, Save Point) have no Stage B manifest entry at all.
- `python3 tools/validate_agent_studio.py` — passes (governance structure itself is intact; this does not certify Stage B asset progress).
- `context_log.md`'s most recent entry before this session was the 2026-07-15 routing of this same packet; nothing was added between then and now.
- **New finding this refresh:** no Blender executable is available in this execution container (`blender` not found on `PATH`). The 2026-07-15 packet marked Task #1 `HUMAN_BLOCKED` (needs Art Director judgment/selection); this session adds that it is also `ENVIRONMENT_BLOCKED` here specifically — even mechanical Blender generation could not be run from this container regardless of authorization. Tasks #2–#5 inherit the same blocker by dependency.

Net: **no material change.** The blocking chain identified yesterday is identical today.

## Task packet summary

| Field | Value |
|---|---|
| Task category | prompt_or_agent_governance |
| Primary agent | producer |
| Secondary agents | qa_lead, technical_director |
| Required memories | MEM-PROMPT-009, MEM-GOV-001, MEM-GOV-002 |
| Required skills | prompt_design, pull_request_review, evidence_reporting |
| Required evidence | requested doc objective, files changed, validation command |
| Minimum validation | `python3 tools/validate_agent_studio.py`, `git diff --check` |
| Scope result | Documentation/governance only; no gameplay, scene, asset, or visual-content edits |

## Five next tasks (unchanged from 2026-07-15)

| # | Task | Owner | Blocked by | Verdict |
|---|---|---|---|---|
| 1 | ~~Generate Blender candidates for the 8 remaining Stage B briefs~~ — superseded by Correction 2: 7 of the 8 already exist under their briefs' own specified names; only Terrain Basin has a deliberately-unused candidate, not a gap. | Art Director | Nothing left to generate. | SUPERSEDED (see Correction 2) |
| 2 | Document each candidate in the asset manifest. | Asset Librarian | Task #1 artifacts. | HUMAN_BLOCKED |
| 3 | Generate PNG bundles from GLB exports. | Visual Artifact Factory | Task #1 exports. | HUMAN_BLOCKED |
| 4 | Open PNGs and provide visual diagnosis. | Visual Reviewer | Task #3 PNG bundles; also requires genuine human/reviewer visual judgment, not a fabricated verdict. | HUMAN_BLOCKED |
| 5 | Test GLB imports and performance. | Technical Director | Tasks #1–#3. | HUMAN_BLOCKED |

## Execution result

No production asset generation, PNG visual approval, GLB import test, gameplay integration, or release-readiness claim was performed, for the same reason as 2026-07-15: the required upstream Stage B artifacts are not present, and this specific container additionally lacks the tooling (Blender) to produce them. The completed work in this refresh is verification that the blocking condition still holds, plus the added environment-capability finding.

## Required follow-up

Unchanged from 2026-07-15:

1. Art Director or automation must produce the Task #1 candidate files in a separate asset-generation slice, run somewhere Blender is actually available.
2. Asset Librarian must record provenance and hashes before any candidate is used in a scene.
3. Visual Artifact Factory must create PNG evidence bundles before any visual approval claim.
4. Visual Reviewer must open the PNGs before issuing a visual verdict.
5. Technical Director must validate imports and performance before Producer can consider the Stage B gate.

## Manual checks

- README scope checked: Crash Site MVP remains solo, offline-first, Windows-first.
- Forbidden MVP scope checked: no forbidden gameplay or platform feature is introduced.
- Evidence limitation checked: missing Stage B candidate artifacts prevent PASS/GO claims; Blender unavailability in this container is a separate, additive limitation.

## Final verdict

NOT_GO for the original five-task framing, but the real remaining work is much smaller than "generate 8 candidates": per Correction 2, review-request stubs already exist or were filed this session for every Stage B candidate except `TC_LightingReference_V1` and `TC_PolishDetails_V1` (both filed immediately after this correction — see `docs/art/reviews/tc-lighting-reference-v1-review-request.md` and `tc-polish-details-v1-review-request.md`). What actually blocks Stage B closure now is a human or the Visual Reviewer agent opening PNGs and issuing real verdicts across the ~10 pending review-request documents in `docs/art/reviews/`, not any further asset generation.
