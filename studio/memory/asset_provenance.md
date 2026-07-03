# Memory Pack: Asset Provenance

These indexed atomic memory cards are curated and non-exhaustive. `README.md` remains authoritative.

### MEM-ASSET-PROVENANCE-001

- id: MEM-ASSET-PROVENANCE-001
- title: Toy hulls need replacement
- tags: [assets,visual, asset_provenance]
- applies_when: ship hull looks toy-like.
- memory: Toy-like proportions must be replaced or remodeled, not hidden with decals.
- avoid: Do not add surface detail to wrong silhouette.
- required_action: Fix silhouette and scale first.
- evidence_required: Silhouette comparison screenshot
- related_agents: [art_director,asset_librarian]
- related_skills: [asset_audition]

### MEM-ASSET-PROVENANCE-002

- id: MEM-ASSET-PROVENANCE-002
- title: Asset provenance must be real
- tags: [asset,license, asset_provenance]
- applies_when: importing or documenting assets.
- memory: Every asset needs source URL, license, author, date, and hash.
- avoid: Do not cite vague marketplace names.
- required_action: Record provenance before use.
- evidence_required: URL, license text, hash
- related_agents: [asset_librarian]
- related_skills: [asset_provenance]

### MEM-ASSET-PROVENANCE-003

- id: MEM-ASSET-PROVENANCE-003
- title: Fake placeholder OBJ forbidden
- tags: [asset,geometry, asset_provenance]
- applies_when: OBJ or mesh file appears.
- memory: A fake OBJ that only satisfies a pipeline is not a production asset.
- avoid: Do not create placeholder geometry and label it recovered or sourced.
- required_action: Classify as placeholder or reject.
- evidence_required: File hash and provenance note
- related_agents: [asset_librarian,qa_lead]
- related_skills: [asset_audition]

### MEM-ASSET-PROVENANCE-004

- id: MEM-ASSET-PROVENANCE-004
- title: Do not continue Stage B after Stage A fails
- tags: [stage,gates, asset_provenance]
- applies_when: stage advancement requested.
- memory: A failed Stage A blocks Stage B even if code compiles.
- avoid: Do not open follow-up stage PRs to bypass failure.
- required_action: Close Stage A gaps first.
- evidence_required: Gate verdict and blocker list
- related_agents: [producer,qa_lead]
- related_skills: [stage_gate_execution]

### MEM-ASSET-PROVENANCE-005

- id: MEM-ASSET-PROVENANCE-005
- title: No docs-only PR unless requested
- tags: [scope,docs, asset_provenance]
- applies_when: task asks implementation or validation.
- memory: Documentation cannot substitute for missing code, screenshots, or tests unless docs are the explicit task.
- avoid: Do not ship explanatory docs as proof of feature completion.
- required_action: Return NOT_GO and list missing artifact.
- evidence_required: Task quote and changed files
- related_agents: [producer,qa_lead]
- related_skills: [pull_request_review]
