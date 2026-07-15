# Next Five Task Execution Packet — 2026-07-15

Owner: Agent Studio Producer
Version: 1
Date: 2026-07-15
Review status: TASK_PACKET_READY
Final verdict: NOT_GO

## Source-of-truth check

This packet interprets the request “Do the 5 next tasks” against the current production dashboard. The next five production tasks remain the Stage B chain listed in `docs/production/PRODUCTION_PROGRESSION_DASHBOARD.md`: Task #1, Asset Manifest, Task #2, Task #3, and Task #4.

The packet does not authorize multiplayer, grappling hook, wall running, procedural worlds, voxels, large mech, complete rocket, multiple maps, multiple enemy types, cloud services, remote telemetry, or any other out-of-scope MVP feature.

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

## Five next tasks

| # | Task | Owner | Input | Command or action | Required artifact | Verdict |
|---|---|---|---|---|---|---|
| 1 | Generate Blender candidates for the 10 Stage B briefs. | Art Director | Approved Stage A palette, asset briefs, Blender Asset Forge scripts | Run the relevant Blender Asset Forge generation commands for each approved brief. | Candidate source/export files plus per-candidate hash records. | HUMAN_BLOCKED until approved candidate selection or generated files are present. |
| 2 | Document each candidate in the asset manifest. | Asset Librarian | Task #1 candidate files and source records | Record brief reference, source/provenance, licence, format, hash, and intended use before scene integration. | Updated asset manifest/provenance records. | HUMAN_BLOCKED until Task #1 artifacts exist. |
| 3 | Generate PNG bundles from GLB exports. | Visual Artifact Factory | Task #1 exports and Task #2 provenance records | Run the visual artifact workflow or equivalent local factory command. | PNG bundle paths with hashes. | HUMAN_BLOCKED until authenticated GLB exports exist. |
| 4 | Open PNGs and provide visual diagnosis. | Visual Reviewer | Task #3 PNG bundles | Inspect the actual PNGs and score silhouette, scale, route readability, material coherence, and objective readability. | Visual review report with screenshot paths and approved verdict vocabulary. | HUMAN_BLOCKED until PNG bundles exist. |
| 5 | Test GLB imports and performance. | Technical Director | Task #1 exports, Task #2 provenance, Task #3 review bundles | Run Godot import/build/performance checks applicable to isolated candidate scenes before production integration. | Import logs, build output, performance notes, and changed-file list. | HUMAN_BLOCKED until authenticated candidate exports exist. |

## Execution result

No production asset generation, PNG visual approval, GLB import test, gameplay integration, or release-readiness claim was performed in this documentation slice because the required upstream Stage B artifacts are not present in the working tree for Tasks #1–#5. The safe completed work is the routing, dependency ordering, evidence definition, owner assignment, and blocking verdict for the next five tasks.

## Required follow-up

1. Art Director or automation must produce the Task #1 candidate files in a separate asset-generation slice.
2. Asset Librarian must record provenance and hashes before any candidate is used in a scene.
3. Visual Artifact Factory must create PNG evidence bundles before any visual approval claim.
4. Visual Reviewer must open the PNGs before issuing a visual verdict.
5. Technical Director must validate imports and performance before Producer can consider the Stage B gate.

## Manual checks

- README scope checked: Crash Site MVP remains solo, offline-first, Windows-first.
- Forbidden MVP scope checked: no forbidden gameplay or platform feature is introduced.
- Evidence limitation checked: missing Stage B candidate artifacts prevent PASS/GO claims.

## Final verdict

NOT_GO — the next five tasks are routed and evidence-gated, but their production completion remains blocked until the required Stage B asset and PNG evidence exists.
