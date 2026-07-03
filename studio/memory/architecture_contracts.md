# Memory Pack: Architecture Contracts

These indexed atomic memory cards are curated and non-exhaustive. `README.md` remains authoritative.

### MEM-ARCHITECTURE-CONTRACTS-001

- id: MEM-ARCHITECTURE-CONTRACTS-001
- title: Compare against rejected screenshots
- tags: [visual,regression, architecture_contracts]
- applies_when: visual fix claims improvement.
- memory: Visual review must compare with prior rejected screenshots when available.
- avoid: Do not review in isolation.
- required_action: Name differences from rejected baseline.
- evidence_required: Before/after PNGs
- related_agents: [visual_reviewer,art_director]
- related_skills: [screenshot_critique]

### MEM-ARCHITECTURE-CONTRACTS-002

- id: MEM-ARCHITECTURE-CONTRACTS-002
- title: No merge without evidence
- tags: [pr,evidence, architecture_contracts]
- applies_when: PR is ready.
- memory: Merge readiness requires command output, relevant artifacts, and verdict.
- avoid: Do not rely on confidence language.
- required_action: Attach exact evidence and missing checks.
- evidence_required: Command logs and artifacts
- related_agents: [qa_lead,producer]
- related_skills: [evidence_reporting]

### MEM-ARCHITECTURE-CONTRACTS-003

- id: MEM-ARCHITECTURE-CONTRACTS-003
- title: Tests passing does not prove visual quality
- tags: [visual,tests, architecture_contracts]
- applies_when: a PR claims art success because tests passed.
- memory: Runtime tests only prove code paths, not composition, silhouette, scale, material coherence, or route readability.
- avoid: Do not use green tests as visual approval.
- required_action: Open PNGs and name visual failures.
- evidence_required: PNG paths, visual diagnosis, before/after comparison
- related_agents: [art_director,visual_reviewer]
- related_skills: [screenshot_critique]

### MEM-ARCHITECTURE-CONTRACTS-004

- id: MEM-ARCHITECTURE-CONTRACTS-004
- title: Screenshots must be opened
- tags: [visual,evidence, architecture_contracts]
- applies_when: screenshot files are generated.
- memory: A screenshot is evidence only after an agent opens it and records what is visible.
- avoid: Do not cite screenshot existence alone.
- required_action: Open each relevant PNG and describe focal point, route, silhouette, scale, materials.
- evidence_required: Opened image list and critique
- related_agents: [visual_reviewer,qa_lead]
- related_skills: [screenshot_critique]

### MEM-ARCHITECTURE-CONTRACTS-005

- id: MEM-ARCHITECTURE-CONTRACTS-005
- title: Codex must not self approve art
- tags: [visual,approval, architecture_contracts]
- applies_when: agent generated or edited visuals.
- memory: The generating agent cannot be the only approver of visual quality.
- avoid: Do not mark visual PASS without reviewer or human gate.
- required_action: Request visual reviewer or human review.
- evidence_required: Independent review verdict
- related_agents: [art_director,visual_reviewer]
- related_skills: [pull_request_review]
