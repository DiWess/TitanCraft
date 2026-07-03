# Memory Pack: Product Scope

These indexed atomic memory cards are curated and non-exhaustive. `README.md` remains authoritative.

### MEM-PRODUCT-001

- id: MEM-PRODUCT-001
- title: Crash Site is the only MVP
- tags: [mvp, scope]
- applies_when: Any task proposes gameplay, levels, enemies, or progression.
- memory: The current MVP is Crash Site, a short single-player offline loop.
- avoid: Do not treat long-term vision features as current scope.
- required_action: Check README sections 5 and 6 before approving scope.
- evidence_required: README citation and changed-file list.
- related_agents: [game_director, producer]
- related_skills: [stage_gate_execution, pull_request_review]

### MEM-PRODUCT-002

- id: MEM-PRODUCT-002
- title: Windows-first offline-first remains binding
- tags: [platform, offline]
- applies_when: A task mentions networking, cloud, telemetry, accounts, or export targets.
- memory: MVP platform is Windows PC and the initial mode is solo fully offline.
- avoid: Do not add services, accounts, cloud saves, remote telemetry, or unsupported platforms.
- required_action: Reject or escalate any online dependency.
- evidence_required: README citation and dependency/config diff.
- related_agents: [technical_director, build_release_engineer]
- related_skills: [ci_cd_validation, windows_export_validation]
