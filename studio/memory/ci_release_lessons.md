# Memory Pack: Ci Release Lessons

These indexed atomic memory cards are curated and non-exhaustive. `README.md` remains authoritative.

### MEM-CI-RELEASE-LESSONS-001

- id: MEM-CI-RELEASE-LESSONS-001
- title: Runtime and art are separate gates
- tags: [ci,visual, ci_release_lessons]
- applies_when: build passes for visual work.
- memory: Runtime correctness and art quality are separate approvals.
- avoid: Do not collapse gates.
- required_action: Report both verdicts separately.
- evidence_required: Build log plus visual critique
- related_agents: [technical_director,art_director]
- related_skills: [evidence_reporting]

### MEM-CI-RELEASE-LESSONS-002

- id: MEM-CI-RELEASE-LESSONS-002
- title: Generated geometry may fail visually
- tags: [godot,visual, ci_release_lessons]
- applies_when: procedural or generated geometry is used.
- memory: Generated geometry can satisfy node counts and collisions while looking unusable.
- avoid: Do not rely on structural tests alone.
- required_action: Capture and review in-engine screenshots.
- evidence_required: Scene screenshot and node evidence
- related_agents: [technical_director,visual_reviewer]
- related_skills: [godot_scene_editing]

### MEM-CI-RELEASE-LESSONS-003

- id: MEM-CI-RELEASE-LESSONS-003
- title: Compare against rejected screenshots
- tags: [visual,regression, ci_release_lessons]
- applies_when: visual fix claims improvement.
- memory: Visual review must compare with prior rejected screenshots when available.
- avoid: Do not review in isolation.
- required_action: Name differences from rejected baseline.
- evidence_required: Before/after PNGs
- related_agents: [visual_reviewer,art_director]
- related_skills: [screenshot_critique]

### MEM-CI-RELEASE-LESSONS-004

- id: MEM-CI-RELEASE-LESSONS-004
- title: No merge without evidence
- tags: [pr,evidence, ci_release_lessons]
- applies_when: PR is ready.
- memory: Merge readiness requires command output, relevant artifacts, and verdict.
- avoid: Do not rely on confidence language.
- required_action: Attach exact evidence and missing checks.
- evidence_required: Command logs and artifacts
- related_agents: [qa_lead,producer]
- related_skills: [evidence_reporting]

### MEM-CI-RELEASE-LESSONS-005

- id: MEM-CI-RELEASE-LESSONS-005
- title: Tests passing does not prove visual quality
- tags: [visual,tests, ci_release_lessons]
- applies_when: a PR claims art success because tests passed.
- memory: Runtime tests only prove code paths, not composition, silhouette, scale, material coherence, or route readability.
- avoid: Do not use green tests as visual approval.
- required_action: Open PNGs and name visual failures.
- evidence_required: PNG paths, visual diagnosis, before/after comparison
- related_agents: [art_director,visual_reviewer]
- related_skills: [screenshot_critique]

### MEM-CI-010

- id: MEM-CI-010
- title: CI evidence does not replace domain evidence
- tags: [ci,evidence,validation]
- applies_when: A routed task has automated checks plus visual, gameplay, asset, or release evidence requirements.
- memory: CI can prove command execution and structural checks, but it does not replace required domain evidence such as opened PNG critique, gameplay smoke results, provenance, or release-truth gates.
- avoid: Do not use a green validation command as proof of visual quality, gameplay feel, asset readiness, or release readiness.
- required_action: Report CI output separately from missing or present domain evidence.
- evidence_required: Exact command output and any required domain artifact paths or explicit missing-evidence limitation.
- related_agents: [qa_lead,technical_director,producer]
- related_skills: [ci_cd_validation,evidence_reporting]
- topic: CI evidence boundaries
- atomic_statement: Automated validation proves command status only and must be separated from visual, gameplay, asset, or release evidence.
- source_reference: README.md current MVP status and AGENTS.md sections 8 and 9.
- confidence: high
- last_reviewed: 2026-07-04

### MEM-CI-013

- id: MEM-CI-013
- title: Windows export failures are not production readiness signals
- tags: [ci,windows,export,release]
- applies_when: A workflow routes Windows export, build failure, artifact, or release validation work.
- memory: Fixing or rehearsing a Windows export path may demonstrate pipeline behavior, but it does not establish production readiness without required artifact metadata, signing, device, telemetry, rollback, and go/no-go evidence.
- avoid: Do not convert build repair or dummy artifacts into beta or production GO claims.
- required_action: Classify the evidence lane, list missing release gates, and keep final verdict within the routed approved vocabulary.
- evidence_required: Export/build command log, artifact metadata when available, and explicit release-gate status or blocker list.
- related_agents: [technical_director,qa_lead,producer]
- related_skills: [windows_export_validation,ci_cd_validation,evidence_reporting]
- topic: Windows export and release evidence
- atomic_statement: Windows export/build evidence must not be represented as beta or production readiness without release-truth gates.
- source_reference: README.md current production phase and AGENTS.md release/evidence rules.
- confidence: high
- last_reviewed: 2026-07-04
