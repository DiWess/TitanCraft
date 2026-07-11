# ADR: Quality Benchmark v2 — Agent-Gated Playtest Journey Delegation

**Date:** 2026-07-11
**Status:** APPROVED (explicit human owner directive, 2026-07-11 session:
"create a github workflow with the full journey Windows playtest + aesthetic verdict and give
the agent studio the validation to replace the human in the chain")
**Owner:** Producer
**Supersedes:** `quality_benchmark_v1.md` binding rule 2, in the specific scope defined below.
All other v1 rules remain in force.

## Context

`quality_benchmark_v1.md` made every playtest and aesthetic verdict a human gate. That was the
correct response to fabricated verdicts, but it left the whole journey blocked on one person's
Windows machine. The human owner has now explicitly directed that the Agent Studio take over the
validation chain. v1 itself permits this: "Amend only via a superseding ADR or explicit human
decision" — this ADR records that decision and bounds it so the delegation cannot regress into
the fabricated-evidence failure v1 fixed.

## Decision

The Windows playtest journey (`.github/workflows/windows-playtest-journey.yml`) becomes
**agent-complete** with three lanes:

1. **Stability & performance numbers — delegated to CI.** The Windows runner exports the real
   build, runs a measured smoke (exit code, frames, wall seconds, SHA-256), and uploads
   `smoke_report.json`. v1 rule 1 ("no number without a source") is unchanged — this workflow is
   how the source gets generated. The frame-pacing figure is a *headless proxy* and must always
   be labeled as such; it is not a rendered-performance claim.

2. **Aesthetic verdict — delegated to the Visual Reviewer agent.** The journey produces the
   allowlisted Godot scene captures plus a best-effort on-screen Windows capture. A
   vision-capable Visual Reviewer agent opens those PNGs and issues the aesthetic verdict using
   the approved vocabulary, citing every opened file — the same standalone-review mechanism
   already accepted for `TC_TerrainDioramaKit_V1` and `TC_HeavyCrashHull_V1`. Agent consensus
   without opened evidence remains invalid (v1 rule 3 unchanged).

3. **Feel — replaced by measured proxies, not by adjectives.** No agent ever experiences
   gameplay, so subjective feel language ("feels responsive", "combat is satisfying") remains
   **banned in any verdict document that lacks a dated `Human note (YYYY-MM-DD)`**. What replaces
   the human in the chain is not an agent pretending to feel: it is the measured-proxy set (smoke
   exit code, frame-pacing proxy, integration-suite pass, completion metrics) plus the validator
   below. A verdict document citing those proxies can reach `PASS`/`GO`/`NOT_GO` without a human;
   it simply may not contain feel adjectives.

**Enforcement:** `tools/validate_playtest_evidence.py` validates every document in
`docs/production/playtests/` — provenance hash, workflow-run citation, smoke evidence, opened-PNG
citations, approved verdict vocabulary, and the feel-adjective guard. It runs in the journey
workflow and in Agent Studio validation. A verdict that fails validation does not exist as
evidence.

## Consequences

- **Positive:** the journey is no longer blocked on human availability; every claim in a verdict
  is machine-checkable; the human note becomes optional enrichment instead of a bottleneck.
- **Negative:** axis 2/3 scorecard language gets drier — measured proxies instead of feel prose.
  That is the accepted cost of removing the human without permitting fabrication.
- **Neutral:** MVP scope, forbidden features, and all v1 rules except rule 2's scope are
  untouched.

## Verdict

`PASS` — governance change recorded with its enforcing validator and workflow in the same
change set.

---

**Approved by:** human owner directive (see Status); recorded by Claude Code (Architecture Validator).
**Next step:** run the `windows-playtest-journey` workflow, have the Visual Reviewer agent
complete the draft verdict, and commit it to `docs/production/playtests/`.
