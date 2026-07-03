# Memory Pack: Csharp Runtime Safety

These indexed atomic memory cards are curated and non-exhaustive. `README.md` remains authoritative.

### MEM-CSHARP-RUNTIME-SAFETY-001

- id: MEM-CSHARP-RUNTIME-SAFETY-001
- title: Codex must not self approve art
- tags: [visual,approval, csharp_runtime_safety]
- applies_when: agent generated or edited visuals.
- memory: The generating agent cannot be the only approver of visual quality.
- avoid: Do not mark visual PASS without reviewer or human gate.
- required_action: Request visual reviewer or human review.
- evidence_required: Independent review verdict
- related_agents: [art_director,visual_reviewer]
- related_skills: [pull_request_review]

### MEM-CSHARP-RUNTIME-SAFETY-002

- id: MEM-CSHARP-RUNTIME-SAFETY-002
- title: Route slabs are not terrain
- tags: [terrain,level, csharp_runtime_safety]
- applies_when: flat slabs are used as route solution.
- memory: A navigable slab can pass collision tests while failing as believable terrain.
- avoid: Do not decorate a slab and call it terrain.
- required_action: Replace with shaped terrain, landmarks, and readable edges.
- evidence_required: Before/after route screenshot
- related_agents: [level_designer,art_director]
- related_skills: [visual_art_direction]

### MEM-CSHARP-RUNTIME-SAFETY-003

- id: MEM-CSHARP-RUNTIME-SAFETY-003
- title: Toy hulls need replacement
- tags: [assets,visual, csharp_runtime_safety]
- applies_when: ship hull looks toy-like.
- memory: Toy-like proportions must be replaced or remodeled, not hidden with decals.
- avoid: Do not add surface detail to wrong silhouette.
- required_action: Fix silhouette and scale first.
- evidence_required: Silhouette comparison screenshot
- related_agents: [art_director,asset_librarian]
- related_skills: [asset_audition]

### MEM-CSHARP-RUNTIME-SAFETY-004

- id: MEM-CSHARP-RUNTIME-SAFETY-004
- title: Asset provenance must be real
- tags: [asset,license, csharp_runtime_safety]
- applies_when: importing or documenting assets.
- memory: Every asset needs source URL, license, author, date, and hash.
- avoid: Do not cite vague marketplace names.
- required_action: Record provenance before use.
- evidence_required: URL, license text, hash
- related_agents: [asset_librarian]
- related_skills: [asset_provenance]

### MEM-CSHARP-RUNTIME-SAFETY-005

- id: MEM-CSHARP-RUNTIME-SAFETY-005
- title: Fake placeholder OBJ forbidden
- tags: [asset,geometry, csharp_runtime_safety]
- applies_when: OBJ or mesh file appears.
- memory: A fake OBJ that only satisfies a pipeline is not a production asset.
- avoid: Do not create placeholder geometry and label it recovered or sourced.
- required_action: Classify as placeholder or reject.
- evidence_required: File hash and provenance note
- related_agents: [asset_librarian,qa_lead]
- related_skills: [asset_audition]
