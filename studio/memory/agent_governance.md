# Memory Pack: Agent Governance

These indexed atomic memory cards are curated and non-exhaustive. `README.md` and root `AGENTS.md` remain authoritative.

### MEM-GOV-001

- id: MEM-GOV-001
- title: Evidence beats confidence
- tags: [evidence, governance]
- applies_when: An agent reports completion, readiness, or review outcome.
- memory: Completion claims require concrete evidence such as commands, artifacts, screenshots, URLs, hashes, or file citations.
- avoid: Do not use confidence language as a substitute for proof.
- required_action: List exact evidence or return NOT_GO.
- evidence_required: Command log, artifact path, or cited file.
- related_agents: [qa_lead, producer]
- related_skills: [evidence_reporting, pull_request_review]

### MEM-GOV-002

- id: MEM-GOV-002
- title: Governance tasks must not touch gameplay
- tags: [scope, gameplay]
- applies_when: The task is documentation, agent studio, process, or knowledge-base work.
- memory: Governance-only changes must not modify gameplay code, production scenes, C# runtime files, tests, assets, or visual content.
- avoid: Do not opportunistically fix gameplay while editing governance.
- required_action: Verify changed files before commit.
- evidence_required: git status or changed-file list.
- related_agents: [producer, qa_lead]
- related_skills: [evidence_reporting]
