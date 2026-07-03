# Memory Pack: Titancraft Visual Identity

These indexed atomic memory cards are curated and non-exhaustive. `README.md` remains authoritative.

### MEM-TC-VISUAL-IDENTITY-001

- id: MEM-TC-VISUAL-IDENTITY-001
- title: Asset provenance must be real
- tags: [asset,license, titanCraft_visual_identity]
- applies_when: importing or documenting assets.
- memory: Every asset needs source URL, license, author, date, and hash.
- avoid: Do not cite vague marketplace names.
- required_action: Record provenance before use.
- evidence_required: URL, license text, hash
- related_agents: [asset_librarian]
- related_skills: [asset_provenance]

### MEM-TC-VISUAL-IDENTITY-002

- id: MEM-TC-VISUAL-IDENTITY-002
- title: Fake placeholder OBJ forbidden
- tags: [asset,geometry, titanCraft_visual_identity]
- applies_when: OBJ or mesh file appears.
- memory: A fake OBJ that only satisfies a pipeline is not a production asset.
- avoid: Do not create placeholder geometry and label it recovered or sourced.
- required_action: Classify as placeholder or reject.
- evidence_required: File hash and provenance note
- related_agents: [asset_librarian,qa_lead]
- related_skills: [asset_audition]

### MEM-TC-VISUAL-IDENTITY-003

- id: MEM-TC-VISUAL-IDENTITY-003
- title: Do not continue Stage B after Stage A fails
- tags: [stage,gates, titanCraft_visual_identity]
- applies_when: stage advancement requested.
- memory: A failed Stage A blocks Stage B even if code compiles.
- avoid: Do not open follow-up stage PRs to bypass failure.
- required_action: Close Stage A gaps first.
- evidence_required: Gate verdict and blocker list
- related_agents: [producer,qa_lead]
- related_skills: [stage_gate_execution]

### MEM-TC-VISUAL-IDENTITY-004

- id: MEM-TC-VISUAL-IDENTITY-004
- title: No docs-only PR unless requested
- tags: [scope,docs, titanCraft_visual_identity]
- applies_when: task asks implementation or validation.
- memory: Documentation cannot substitute for missing code, screenshots, or tests unless docs are the explicit task.
- avoid: Do not ship explanatory docs as proof of feature completion.
- required_action: Return NOT_GO and list missing artifact.
- evidence_required: Task quote and changed files
- related_agents: [producer,qa_lead]
- related_skills: [pull_request_review]

### MEM-TC-VISUAL-IDENTITY-005

- id: MEM-TC-VISUAL-IDENTITY-005
- title: Runtime and art are separate gates
- tags: [ci,visual, titanCraft_visual_identity]
- applies_when: build passes for visual work.
- memory: Runtime correctness and art quality are separate approvals.
- avoid: Do not collapse gates.
- required_action: Report both verdicts separately.
- evidence_required: Build log plus visual critique
- related_agents: [technical_director,art_director]
- related_skills: [evidence_reporting]
