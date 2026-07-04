# Memory Pack: Prompt Failure Patterns

These indexed atomic memory cards are curated and non-exhaustive. `README.md` remains authoritative.

### MEM-PROMPT-FAILURE-PATTERNS-001

- id: MEM-PROMPT-FAILURE-PATTERNS-001
- title: Screenshots must be opened
- tags: [visual,evidence, prompt_failure_patterns]
- applies_when: screenshot files are generated.
- memory: A screenshot is evidence only after an agent opens it and records what is visible.
- avoid: Do not cite screenshot existence alone.
- required_action: Open each relevant PNG and describe focal point, route, silhouette, scale, materials.
- evidence_required: Opened image list and critique
- related_agents: [visual_reviewer,qa_lead]
- related_skills: [screenshot_critique]

### MEM-PROMPT-FAILURE-PATTERNS-002

- id: MEM-PROMPT-FAILURE-PATTERNS-002
- title: Codex must not self approve art
- tags: [visual,approval, prompt_failure_patterns]
- applies_when: agent generated or edited visuals.
- memory: The generating agent cannot be the only approver of visual quality.
- avoid: Do not mark visual PASS without reviewer or human gate.
- required_action: Request visual reviewer or human review.
- evidence_required: Independent review verdict
- related_agents: [art_director,visual_reviewer]
- related_skills: [pull_request_review]

### MEM-PROMPT-FAILURE-PATTERNS-003

- id: MEM-PROMPT-FAILURE-PATTERNS-003
- title: Route slabs are not terrain
- tags: [terrain,level, prompt_failure_patterns]
- applies_when: flat slabs are used as route solution.
- memory: A navigable slab can pass collision tests while failing as believable terrain.
- avoid: Do not decorate a slab and call it terrain.
- required_action: Replace with shaped terrain, landmarks, and readable edges.
- evidence_required: Before/after route screenshot
- related_agents: [level_designer,art_director]
- related_skills: [visual_art_direction]

### MEM-PROMPT-FAILURE-PATTERNS-004

- id: MEM-PROMPT-FAILURE-PATTERNS-004
- title: Toy hulls need replacement
- tags: [assets,visual, prompt_failure_patterns]
- applies_when: ship hull looks toy-like.
- memory: Toy-like proportions must be replaced or remodeled, not hidden with decals.
- avoid: Do not add surface detail to wrong silhouette.
- required_action: Fix silhouette and scale first.
- evidence_required: Silhouette comparison screenshot
- related_agents: [art_director,asset_librarian]
- related_skills: [asset_audition]

### MEM-PROMPT-FAILURE-PATTERNS-005

- id: MEM-PROMPT-FAILURE-PATTERNS-005
- title: Asset provenance must be real
- tags: [asset,license, prompt_failure_patterns]
- applies_when: importing or documenting assets.
- memory: Every asset needs source URL, license, author, date, and hash.
- avoid: Do not cite vague marketplace names.
- required_action: Record provenance before use.
- evidence_required: URL, license text, hash
- related_agents: [asset_librarian]
- related_skills: [asset_provenance]

### MEM-PROMPT-009

- id: MEM-PROMPT-009
- title: Prompt discipline preserves requested scope
- tags: [prompt,scope,planning,governance]
- applies_when: A task prompt asks for governance, planning, review, documentation, or Agent Studio routing changes.
- memory: Task prompts must preserve the requested scope; planning tasks must not silently become implementation, final reports must distinguish requested docs or planning work from production claims, and missing evidence must be reported instead of hidden.
- avoid: Do not expand a prompt into gameplay, scene, asset, CI, release, or production-readiness work unless the user explicitly requested that scope and the README permits it.
- required_action: Restate the requested scope, keep edits limited to the routed governance/planning objective, and mark absent evidence as a limitation or blocking condition rather than implying readiness.
- evidence_required: Task quote or packet summary, changed-file list, validation command output, and explicit distinction between documentation/planning evidence and production evidence.
- related_agents: [producer,qa_lead,technical_director]
- related_skills: [prompt_design,evidence_reporting,pull_request_review]
- topic: Prompt discipline and scope control
- atomic_statement: Planning and governance prompts preserve scope and report missing evidence rather than converting into implementation or production claims.
- source_reference: README.md "How to start any task" and AGENTS.md sections 3 and 8.
- confidence: high
- last_reviewed: 2026-07-04
