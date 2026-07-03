# Memory Pack: Visual Failure Patterns

These indexed atomic memory cards are curated and non-exhaustive. `README.md` remains authoritative.

### MEM-VISFAIL-001

- id: MEM-VISFAIL-001
- title: Tests passing does not prove visual quality
- tags: [visual,tests, visual_failure_patterns]
- applies_when: a PR claims art success because tests passed.
- memory: Runtime tests only prove code paths, not composition, silhouette, scale, material coherence, or route readability.
- avoid: Do not use green tests as visual approval.
- required_action: Open PNGs and name visual failures.
- evidence_required: PNG paths, visual diagnosis, before/after comparison
- related_agents: [art_director,visual_reviewer]
- related_skills: [screenshot_critique]

### MEM-VISFAIL-002

- id: MEM-VISFAIL-002
- title: Screenshots must be opened
- tags: [visual,evidence, visual_failure_patterns]
- applies_when: screenshot files are generated.
- memory: A screenshot is evidence only after an agent opens it and records what is visible.
- avoid: Do not cite screenshot existence alone.
- required_action: Open each relevant PNG and describe focal point, route, silhouette, scale, materials.
- evidence_required: Opened image list and critique
- related_agents: [visual_reviewer,qa_lead]
- related_skills: [screenshot_critique]

### MEM-VISFAIL-003

- id: MEM-VISFAIL-003
- title: Codex must not self approve art
- tags: [visual,approval, visual_failure_patterns]
- applies_when: agent generated or edited visuals.
- memory: The generating agent cannot be the only approver of visual quality.
- avoid: Do not mark visual PASS without reviewer or human gate.
- required_action: Request visual reviewer or human review.
- evidence_required: Independent review verdict
- related_agents: [art_director,visual_reviewer]
- related_skills: [pull_request_review]

### MEM-VISFAIL-004

- id: MEM-VISFAIL-004
- title: Route slabs are not terrain
- tags: [terrain,level, visual_failure_patterns]
- applies_when: flat slabs are used as route solution.
- memory: A navigable slab can pass collision tests while failing as believable terrain.
- avoid: Do not decorate a slab and call it terrain.
- required_action: Replace with shaped terrain, landmarks, and readable edges.
- evidence_required: Before/after route screenshot
- related_agents: [level_designer,art_director]
- related_skills: [visual_art_direction]

### MEM-VISFAIL-005

- id: MEM-VISFAIL-005
- title: Toy hulls need replacement
- tags: [assets,visual, visual_failure_patterns]
- applies_when: ship hull looks toy-like.
- memory: Toy-like proportions must be replaced or remodeled, not hidden with decals.
- avoid: Do not add surface detail to wrong silhouette.
- required_action: Fix silhouette and scale first.
- evidence_required: Silhouette comparison screenshot
- related_agents: [art_director,asset_librarian]
- related_skills: [asset_audition]
