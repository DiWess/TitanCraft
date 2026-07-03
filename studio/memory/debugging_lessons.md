# Memory Pack: Debugging Lessons

These indexed atomic memory cards are curated and non-exhaustive. `README.md` remains authoritative.

### MEM-DEBUGGING-LESSONS-001

- id: MEM-DEBUGGING-LESSONS-001
- title: Route slabs are not terrain
- tags: [terrain,level, debugging_lessons]
- applies_when: flat slabs are used as route solution.
- memory: A navigable slab can pass collision tests while failing as believable terrain.
- avoid: Do not decorate a slab and call it terrain.
- required_action: Replace with shaped terrain, landmarks, and readable edges.
- evidence_required: Before/after route screenshot
- related_agents: [level_designer,art_director]
- related_skills: [visual_art_direction]

### MEM-DEBUGGING-LESSONS-002

- id: MEM-DEBUGGING-LESSONS-002
- title: Toy hulls need replacement
- tags: [assets,visual, debugging_lessons]
- applies_when: ship hull looks toy-like.
- memory: Toy-like proportions must be replaced or remodeled, not hidden with decals.
- avoid: Do not add surface detail to wrong silhouette.
- required_action: Fix silhouette and scale first.
- evidence_required: Silhouette comparison screenshot
- related_agents: [art_director,asset_librarian]
- related_skills: [asset_audition]

### MEM-DEBUGGING-LESSONS-003

- id: MEM-DEBUGGING-LESSONS-003
- title: Asset provenance must be real
- tags: [asset,license, debugging_lessons]
- applies_when: importing or documenting assets.
- memory: Every asset needs source URL, license, author, date, and hash.
- avoid: Do not cite vague marketplace names.
- required_action: Record provenance before use.
- evidence_required: URL, license text, hash
- related_agents: [asset_librarian]
- related_skills: [asset_provenance]

### MEM-DEBUGGING-LESSONS-004

- id: MEM-DEBUGGING-LESSONS-004
- title: Fake placeholder OBJ forbidden
- tags: [asset,geometry, debugging_lessons]
- applies_when: OBJ or mesh file appears.
- memory: A fake OBJ that only satisfies a pipeline is not a production asset.
- avoid: Do not create placeholder geometry and label it recovered or sourced.
- required_action: Classify as placeholder or reject.
- evidence_required: File hash and provenance note
- related_agents: [asset_librarian,qa_lead]
- related_skills: [asset_audition]

### MEM-DEBUGGING-LESSONS-005

- id: MEM-DEBUGGING-LESSONS-005
- title: Do not continue Stage B after Stage A fails
- tags: [stage,gates, debugging_lessons]
- applies_when: stage advancement requested.
- memory: A failed Stage A blocks Stage B even if code compiles.
- avoid: Do not open follow-up stage PRs to bypass failure.
- required_action: Close Stage A gaps first.
- evidence_required: Gate verdict and blocker list
- related_agents: [producer,qa_lead]
- related_skills: [stage_gate_execution]
