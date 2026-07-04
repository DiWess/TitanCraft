# Memory Pack: Production Stage Gates

These indexed atomic memory cards are curated and non-exhaustive. `README.md` remains authoritative.

### MEM-STAGE-008

- id: MEM-STAGE-008
- title: Stage A visual gate blocks stage advance
- tags: [stage,visual,gate, production_stage_gates]
- applies_when: Stage A, visual scene composition, or crash-site art is reviewed.
- memory: Stage A cannot advance or be treated as visually ready until required PNG evidence exists, has been opened, and the visual diagnosis is recorded.
- avoid: Do not merge, approve, or continue to later stage work from code checks alone.
- required_action: Generate and inspect required review screenshots; return a blocking or NOT_GO verdict if evidence is missing or visual failures remain.
- evidence_required: PNG screenshot paths, opened-image diagnosis, validation commands, and gate verdict.
- related_agents: [art_director,visual_reviewer,qa_lead]
- related_skills: [screenshot_critique,visual_art_direction,evidence_reporting]

### MEM-PRODUCTION-STAGE-GATES-001

- id: MEM-PRODUCTION-STAGE-GATES-001
- title: Fake placeholder OBJ forbidden
- tags: [asset,geometry, production_stage_gates]
- applies_when: OBJ or mesh file appears.
- memory: A fake OBJ that only satisfies a pipeline is not a production asset.
- avoid: Do not create placeholder geometry and label it recovered or sourced.
- required_action: Classify as placeholder or reject.
- evidence_required: File hash and provenance note
- related_agents: [asset_librarian,qa_lead]
- related_skills: [asset_audition]

### MEM-PRODUCTION-STAGE-GATES-002

- id: MEM-PRODUCTION-STAGE-GATES-002
- title: Do not continue Stage B after Stage A fails
- tags: [stage,gates, production_stage_gates]
- applies_when: stage advancement requested.
- memory: A failed Stage A blocks Stage B even if code compiles.
- avoid: Do not open follow-up stage PRs to bypass failure.
- required_action: Close Stage A gaps first.
- evidence_required: Gate verdict and blocker list
- related_agents: [producer,qa_lead]
- related_skills: [stage_gate_execution]

### MEM-PRODUCTION-STAGE-GATES-003

- id: MEM-PRODUCTION-STAGE-GATES-003
- title: No docs-only PR unless requested
- tags: [scope,docs, production_stage_gates]
- applies_when: task asks implementation or validation.
- memory: Documentation cannot substitute for missing code, screenshots, or tests unless docs are the explicit task.
- avoid: Do not ship explanatory docs as proof of feature completion.
- required_action: Return NOT_GO and list missing artifact.
- evidence_required: Task quote and changed files
- related_agents: [producer,qa_lead]
- related_skills: [pull_request_review]

### MEM-PRODUCTION-STAGE-GATES-004

- id: MEM-PRODUCTION-STAGE-GATES-004
- title: Runtime and art are separate gates
- tags: [ci,visual, production_stage_gates]
- applies_when: build passes for visual work.
- memory: Runtime correctness and art quality are separate approvals.
- avoid: Do not collapse gates.
- required_action: Report both verdicts separately.
- evidence_required: Build log plus visual critique
- related_agents: [technical_director,art_director]
- related_skills: [evidence_reporting]

### MEM-PRODUCTION-STAGE-GATES-005

- id: MEM-PRODUCTION-STAGE-GATES-005
- title: Generated geometry may fail visually
- tags: [godot,visual, production_stage_gates]
- applies_when: procedural or generated geometry is used.
- memory: Generated geometry can satisfy node counts and collisions while looking unusable.
- avoid: Do not rely on structural tests alone.
- required_action: Capture and review in-engine screenshots.
- evidence_required: Scene screenshot and node evidence
- related_agents: [technical_director,visual_reviewer]
- related_skills: [godot_scene_editing]
