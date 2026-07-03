# Memory Pack: Gameplay Mvp Scope

These indexed atomic memory cards are curated and non-exhaustive. `README.md` remains authoritative.

### MEM-GAMEPLAY-MVP-SCOPE-001

- id: MEM-GAMEPLAY-MVP-SCOPE-001
- title: Do not continue Stage B after Stage A fails
- tags: [stage,gates, gameplay_mvp_scope]
- applies_when: stage advancement requested.
- memory: A failed Stage A blocks Stage B even if code compiles.
- avoid: Do not open follow-up stage PRs to bypass failure.
- required_action: Close Stage A gaps first.
- evidence_required: Gate verdict and blocker list
- related_agents: [producer,qa_lead]
- related_skills: [stage_gate_execution]

### MEM-GAMEPLAY-MVP-SCOPE-002

- id: MEM-GAMEPLAY-MVP-SCOPE-002
- title: No docs-only PR unless requested
- tags: [scope,docs, gameplay_mvp_scope]
- applies_when: task asks implementation or validation.
- memory: Documentation cannot substitute for missing code, screenshots, or tests unless docs are the explicit task.
- avoid: Do not ship explanatory docs as proof of feature completion.
- required_action: Return NOT_GO and list missing artifact.
- evidence_required: Task quote and changed files
- related_agents: [producer,qa_lead]
- related_skills: [pull_request_review]

### MEM-GAMEPLAY-MVP-SCOPE-003

- id: MEM-GAMEPLAY-MVP-SCOPE-003
- title: Runtime and art are separate gates
- tags: [ci,visual, gameplay_mvp_scope]
- applies_when: build passes for visual work.
- memory: Runtime correctness and art quality are separate approvals.
- avoid: Do not collapse gates.
- required_action: Report both verdicts separately.
- evidence_required: Build log plus visual critique
- related_agents: [technical_director,art_director]
- related_skills: [evidence_reporting]

### MEM-GAMEPLAY-MVP-SCOPE-004

- id: MEM-GAMEPLAY-MVP-SCOPE-004
- title: Generated geometry may fail visually
- tags: [godot,visual, gameplay_mvp_scope]
- applies_when: procedural or generated geometry is used.
- memory: Generated geometry can satisfy node counts and collisions while looking unusable.
- avoid: Do not rely on structural tests alone.
- required_action: Capture and review in-engine screenshots.
- evidence_required: Scene screenshot and node evidence
- related_agents: [technical_director,visual_reviewer]
- related_skills: [godot_scene_editing]

### MEM-GAMEPLAY-MVP-SCOPE-005

- id: MEM-GAMEPLAY-MVP-SCOPE-005
- title: Compare against rejected screenshots
- tags: [visual,regression, gameplay_mvp_scope]
- applies_when: visual fix claims improvement.
- memory: Visual review must compare with prior rejected screenshots when available.
- avoid: Do not review in isolation.
- required_action: Name differences from rejected baseline.
- evidence_required: Before/after PNGs
- related_agents: [visual_reviewer,art_director]
- related_skills: [screenshot_critique]
