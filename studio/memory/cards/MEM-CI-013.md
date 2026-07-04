# Memory Card: MEM-CI-013

These indexed atomic memory cards are curated and non-exhaustive. `README.md` remains authoritative.

### MEM-CI-013

- id: MEM-CI-013
- stable_id: MEM-CI-013
- title: CI artifact bundle discipline
- topic: CI artifact discipline
- tags: [ci, artifacts, evidence]
- applies_when: a workflow claims generated artifacts are available for review.
- memory: CI verification must confirm the named artifact bundle contains every review artifact required by the task, including logs when logs are part of the evidence contract.
- atomic_statement: A workflow artifact is review-usable only when its bundle contents match the documented evidence contract.
- source_reference: `.github/workflows/blender-asset-forge.yml`; `docs/pipeline/blender-asset-forge-verification.md`
- confidence: high
- last_reviewed: 2026-07-04
- avoid: Do not infer artifact usability from a green workflow name alone.
- required_action: Inspect or enumerate the artifact bundle paths and report missing files as blockers.
- evidence_required: Workflow run identifier or local workflow-command log plus artifact path list.
- related_agents: [build_release_engineer, qa_lead]
- related_skills: [ci_cd_validation, evidence_reporting]
