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

### MEM-GOV-003

- id: MEM-GOV-003
- title: Ownership decides who may write a file
- tags: [ownership, governance, authority]
- applies_when: An agent is about to create, edit, or delete a repository file.
- memory: `studio/indexes/ownership.yml` assigns each path one accountable owning agent, its required reviewers, and its write right; the most specific matching path wins. Routing decides who reviews a task, ownership decides who may write a file.
- avoid: Do not infer authority from an agent file's prose `Owns:` line, and do not treat an unowned path as permission to edit.
- required_action: Run `python3 tools/agent_ownership.py <path>` before editing. If the path is owned by another agent, request the change from its owner; if it is unowned, route it to the producer and add it to the index first.
- evidence_required: Resolver output for the changed paths, or the preflight packet Ownership Rights section.
- related_agents: [producer, qa_lead, technical_director]
- related_skills: [evidence_reporting, pull_request_review]

### MEM-GOV-004

- id: MEM-GOV-004
- title: Constitutional files are human-owned
- tags: [ownership, governance, scope]
- applies_when: A task would change README.md, AGENTS.md, CLAUDE.md, or PROJECT_DIRECTOR_START_HERE.md.
- memory: These four files carry `human_approval_required` in `studio/indexes/ownership.yml`. No agent may change them without explicit human instruction in the task, and ownership never grants scope authority over README.md.
- avoid: Do not edit a constitutional file as a side effect of another task, and do not weaken a gate by rewriting the document that defines it.
- required_action: Confirm the task text explicitly instructs the change; otherwise stop and return HUMAN_BLOCKED.
- evidence_required: The instructing task text quoted in the final report.
- related_agents: [producer, game_director]
- related_skills: [evidence_reporting]
