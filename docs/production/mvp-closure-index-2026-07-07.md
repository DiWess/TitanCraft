# MVP Closure Index — 2026-07-07

## Objective

Route the request "Now close all tasks left to full MVP" through the repository evidence gates and convert it into an actionable closure index without expanding the locked Crash Site MVP scope.

## Scope Guard

- README.md remains the source of truth for the Crash Site MVP: one solo, offline-first, Windows-first playable loop.
- This pass does not add gameplay, visual assets, production scene changes, release materials, or new features.
- Forbidden MVP work remains forbidden: multiplayer, grappling hook, wall running, procedural worlds, voxels, a large mech, a complete rocket, multiple maps, multiple enemy types, cloud services, and remote telemetry.

## Task Packet Summary

| Field | Value |
|---|---|
| Task category | prompt_or_agent_governance |
| Evidence category | documentation |
| Primary agent | producer |
| Secondary agents | qa_lead, technical_director |
| Required memories | MEM-PROMPT-009, MEM-GOV-001, MEM-GOV-002 |
| Required skills | prompt_design, pull_request_review, evidence_reporting |
| Required checklists | before_task, before_pr |
| Required evidence | requested doc objective, files changed, validation command |
| Minimum validation | `python3 tools/validate_agent_studio.py`, `git diff --check` |

## Current Closure State

| Area | Evidence inspected | Status | Closure action |
|---|---|---|---|
| MVP scope definition | README.md and `docs/production/mvp-scope.md` define Crash Site boundaries. | PASS | Keep future closure work inside the locked MVP. |
| Governance and routing | Agent Studio preflight generated a packet for this request. | PASS | Use packet requirements for any follow-up implementation slice. |
| Gameplay loop automation | Existing release evidence records automated MVP playthrough coverage historically, but this pass did not rerun gameplay tests because the routed packet is documentation/governance. | INTENTIONAL_GATE | Run gameplay validation only in a separate routed gameplay/build slice. |
| Visual production approval | `docs/production/current-status.md` still blocks Stage A visual replacement and public visual claims until approved PNG review evidence exists. | NOT_GO | Generate standalone review bundles and obtain human or visual-reviewer verdicts before scene replacement or marketing screenshots. |
| Windows runtime proof | Prior Windows manual MVP playtest records remain blocked without real Windows launch evidence. | ENVIRONMENT_BLOCKED | Run the exported executable on a real Windows machine or equivalent authorized environment and record the full manual playtest verdict. |
| Public demo preparation | Phase 6 requires visual approval, gameplay validation, export proof, and release gates before demo materials. | NOT_GO | Do not prepare public demo materials until all Phase 6 gates pass. |
| Phase 7 work | Phase 7 planning exists, but Phase 7 start is blocked on Phase 6 completion. | INTENTIONAL_GATE | Keep Phase 7 parked until Phase 6 passes. |

## Closeable Autonomous Tasks Remaining

These can be attempted by agents as separate, evidence-backed slices, each with its own preflight packet:

1. Refresh generated validation evidence for the current branch: `dotnet build`, `dotnet test`, Godot import, and the integration smoke suite.
2. Produce or refresh Windows export artifacts in an environment with Godot export templates available.
3. Generate visual review bundles for approved standalone asset candidates without replacing production scenes.
4. Update release evidence records with exact artifact paths, hashes, screenshots, and command logs.
5. Repair repo-owned validation failures discovered by the commands above, provided fixes remain inside Crash Site MVP scope.

## Human- or Environment-Blocked Tasks Remaining

These cannot be closed truthfully by this Linux container session alone:

1. Human or independent visual-reviewer approval of visual quality from opened PNG evidence.
2. Real Windows executable launch and full manual MVP playthrough proof.
3. Public demo or marketing approval after visual, gameplay, export, and release gates pass.
4. Any claim of broader beta or production readiness without the required machine-readable evidence and approved gate verdicts.

## Manual Checks

- Confirmed the request is broader than one safe implementation slice.
- Confirmed README.md permits conservative Crash Site MVP fixes but does not permit scope expansion.
- Confirmed this closure index is documentation-only and does not modify gameplay code, scenes, assets, or tests.

## Final Verdict

NOT_GO for full MVP closure in this session. PASS for converting the broad request into an evidence-backed closure index that identifies remaining autonomous, human-blocked, and environment-blocked work.
