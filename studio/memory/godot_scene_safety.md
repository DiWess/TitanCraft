# Memory Pack: Godot Scene Safety

These indexed atomic memory cards are curated and non-exhaustive. `README.md` remains authoritative.

### MEM-GODOT-SCENE-SAFETY-001

- id: MEM-GODOT-SCENE-SAFETY-001
- title: Generated geometry may fail visually
- tags: [godot,visual, godot_scene_safety]
- applies_when: procedural or generated geometry is used.
- memory: Generated geometry can satisfy node counts and collisions while looking unusable.
- avoid: Do not rely on structural tests alone.
- required_action: Capture and review in-engine screenshots.
- evidence_required: Scene screenshot and node evidence
- related_agents: [technical_director,visual_reviewer]
- related_skills: [godot_scene_editing]

### MEM-GODOT-SCENE-SAFETY-002

- id: MEM-GODOT-SCENE-SAFETY-002
- title: Compare against rejected screenshots
- tags: [visual,regression, godot_scene_safety]
- applies_when: visual fix claims improvement.
- memory: Visual review must compare with prior rejected screenshots when available.
- avoid: Do not review in isolation.
- required_action: Name differences from rejected baseline.
- evidence_required: Before/after PNGs
- related_agents: [visual_reviewer,art_director]
- related_skills: [screenshot_critique]

### MEM-GODOT-SCENE-SAFETY-003

- id: MEM-GODOT-SCENE-SAFETY-003
- title: No merge without evidence
- tags: [pr,evidence, godot_scene_safety]
- applies_when: PR is ready.
- memory: Merge readiness requires command output, relevant artifacts, and verdict.
- avoid: Do not rely on confidence language.
- required_action: Attach exact evidence and missing checks.
- evidence_required: Command logs and artifacts
- related_agents: [qa_lead,producer]
- related_skills: [evidence_reporting]

### MEM-GODOT-SCENE-SAFETY-004

- id: MEM-GODOT-SCENE-SAFETY-004
- title: Tests passing does not prove visual quality
- tags: [visual,tests, godot_scene_safety]
- applies_when: a PR claims art success because tests passed.
- memory: Runtime tests only prove code paths, not composition, silhouette, scale, material coherence, or route readability.
- avoid: Do not use green tests as visual approval.
- required_action: Open PNGs and name visual failures.
- evidence_required: PNG paths, visual diagnosis, before/after comparison
- related_agents: [art_director,visual_reviewer]
- related_skills: [screenshot_critique]

### MEM-GODOT-SCENE-SAFETY-005

- id: MEM-GODOT-SCENE-SAFETY-005
- title: Screenshots must be opened
- tags: [visual,evidence, godot_scene_safety]
- applies_when: screenshot files are generated.
- memory: A screenshot is evidence only after an agent opens it and records what is visible.
- avoid: Do not cite screenshot existence alone.
- required_action: Open each relevant PNG and describe focal point, route, silhouette, scale, materials.
- evidence_required: Opened image list and critique
- related_agents: [visual_reviewer,qa_lead]
- related_skills: [screenshot_critique]
