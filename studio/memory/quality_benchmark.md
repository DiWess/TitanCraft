# Memory Pack: Quality Benchmark

These indexed atomic memory cards are curated and non-exhaustive. `README.md` remains authoritative.
Source decision: `studio/decisions/quality_benchmark_v1.md`.

### MEM-QUALITY-BENCHMARK-001

- id: MEM-QUALITY-BENCHMARK-001
- title: No number without a cited evidence artifact
- tags: [quality,evidence,release, quality_benchmark]
- applies_when: a verdict states an FPS, draw-call, GPU-ms, or similar performance figure.
- memory: A performance number is only as good as the log or capture it came from. The 2026-07-06 `BETA_READY` verdict stated 60 FPS, 45–50 draw calls, and 4–5 ms GPU time with no supporting artifact anywhere in the repo.
- avoid: Do not restate a number from a prior agent report as fact without locating its source artifact.
- required_action: Cite the exact benchmark log or profiler capture path, or return `HUMAN_BLOCKED` / `ENVIRONMENT_BLOCKED`.
- evidence_required: Benchmark log path or capture artifact path.
- related_agents: [producer,qa_lead,technical_director]
- related_skills: [evidence_reporting,ci_cd_validation]

### MEM-QUALITY-BENCHMARK-002

- id: MEM-QUALITY-BENCHMARK-002
- title: Quality target is peer-anchored, not self-referential
- tags: [quality,peer,gate, quality_benchmark]
- applies_when: an agent claims a system (combat, movement, art, world, crafting, audio) is good or ready.
- memory: `studio/decisions/quality_benchmark_v1.md` names a top-tier peer anchor and target score for each axis. A PASS that only cites another studio agent's PASS is circular, not evidence.
- avoid: Do not treat agreement between studio agents as an external quality check.
- required_action: Name the axis, the peer anchor, the target score, and the specific gap before signing PASS.
- evidence_required: Reference to the relevant row in `studio/decisions/quality_benchmark_v1.md`.
- related_agents: [producer,qa_lead,art_director,gameplay_engineer,level_designer,technical_director,visual_reviewer]
- related_skills: [evidence_reporting,stage_gate_execution]

### MEM-QUALITY-BENCHMARK-003

- id: MEM-QUALITY-BENCHMARK-003
- title: Human- and hardware-gated axes cannot be marked PASS from a container
- tags: [quality,human,windows, quality_benchmark]
- applies_when: movement feel, combat feel, Windows performance, or human playtest claims are requested.
- memory: This execution environment has no display and no Windows hardware. `artifacts/mvp_closure/20260703_windows_manual_validation_blocked.md` recorded this correctly as BLOCKED; the 2026-07-06 audit ignored it and claimed PASS anyway.
- avoid: Do not repeat the 2026-07-06 pattern of asserting a human/hardware result from a headless container.
- required_action: Return `HUMAN_BLOCKED`, name the exact human step required, and stop.
- evidence_required: None available in-container; escalation note naming the blocked step.
- related_agents: [producer,qa_lead,technical_director,gameplay_engineer]
- related_skills: [evidence_reporting]

### MEM-QUALITY-BENCHMARK-004

- id: MEM-QUALITY-BENCHMARK-004
- title: Scorecard progress is tracked per PR, not only at big audits
- tags: [quality,pr,tracking, quality_benchmark]
- applies_when: a PR or commit changes a system on one of the 10 axes in `studio/decisions/quality_benchmark_v1.md`.
- memory: The benchmark is only useful if progress toward it is visible over time. `docs/production/quality-scorecard-log.md` is the running history; the PR template's Quality Scorecard section is the per-PR snapshot.
- avoid: Do not wait for a full studio-wide audit to record a score change; do not edit or delete prior log entries.
- required_action: Fill the PR template's Quality Scorecard table for every axis touched, and append a matching dated entry to `docs/production/quality-scorecard-log.md` with cited evidence per changed score.
- evidence_required: New log entry plus the source evidence (test run, screenshot, playtest note) for each changed score.
- related_agents: [producer,qa_lead]
- related_skills: [evidence_reporting,pull_request_review]
