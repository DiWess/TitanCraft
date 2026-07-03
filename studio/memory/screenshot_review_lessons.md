# Memory Pack: Screenshot Review Lessons

These indexed atomic memory cards are curated and non-exhaustive. `README.md` remains authoritative.

### MEM-SCREENSHOT-REVIEW-LESSONS-001

- id: MEM-SCREENSHOT-REVIEW-LESSONS-001
- title: No merge without evidence
- tags: [pr,evidence, screenshot_review_lessons]
- applies_when: PR is ready.
- memory: Merge readiness requires command output, relevant artifacts, and verdict.
- avoid: Do not rely on confidence language.
- required_action: Attach exact evidence and missing checks.
- evidence_required: Command logs and artifacts
- related_agents: [qa_lead,producer]
- related_skills: [evidence_reporting]

### MEM-SCREENSHOT-REVIEW-LESSONS-002

- id: MEM-SCREENSHOT-REVIEW-LESSONS-002
- title: Tests passing does not prove visual quality
- tags: [visual,tests, screenshot_review_lessons]
- applies_when: a PR claims art success because tests passed.
- memory: Runtime tests only prove code paths, not composition, silhouette, scale, material coherence, or route readability.
- avoid: Do not use green tests as visual approval.
- required_action: Open PNGs and name visual failures.
- evidence_required: PNG paths, visual diagnosis, before/after comparison
- related_agents: [art_director,visual_reviewer]
- related_skills: [screenshot_critique]

### MEM-SCREENSHOT-REVIEW-LESSONS-003

- id: MEM-SCREENSHOT-REVIEW-LESSONS-003
- title: Screenshots must be opened
- tags: [visual,evidence, screenshot_review_lessons]
- applies_when: screenshot files are generated.
- memory: A screenshot is evidence only after an agent opens it and records what is visible.
- avoid: Do not cite screenshot existence alone.
- required_action: Open each relevant PNG and describe focal point, route, silhouette, scale, materials.
- evidence_required: Opened image list and critique
- related_agents: [visual_reviewer,qa_lead]
- related_skills: [screenshot_critique]

### MEM-SCREENSHOT-REVIEW-LESSONS-004

- id: MEM-SCREENSHOT-REVIEW-LESSONS-004
- title: Codex must not self approve art
- tags: [visual,approval, screenshot_review_lessons]
- applies_when: agent generated or edited visuals.
- memory: The generating agent cannot be the only approver of visual quality.
- avoid: Do not mark visual PASS without reviewer or human gate.
- required_action: Request visual reviewer or human review.
- evidence_required: Independent review verdict
- related_agents: [art_director,visual_reviewer]
- related_skills: [pull_request_review]

### MEM-SCREENSHOT-REVIEW-LESSONS-005

- id: MEM-SCREENSHOT-REVIEW-LESSONS-005
- title: Route slabs are not terrain
- tags: [terrain,level, screenshot_review_lessons]
- applies_when: flat slabs are used as route solution.
- memory: A navigable slab can pass collision tests while failing as believable terrain.
- avoid: Do not decorate a slab and call it terrain.
- required_action: Replace with shaped terrain, landmarks, and readable edges.
- evidence_required: Before/after route screenshot
- related_agents: [level_designer,art_director]
- related_skills: [visual_art_direction]
