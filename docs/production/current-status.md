# TitanCraft Current Production Status

## Snapshot

- Agent Studio is ready as the repository-local operating system for task routing, memories, skills, checklists, and evidence gates.
- Blender Asset Forge is locally verified as the repeatable path for standalone generated asset candidates and GLB exports.
- Art Taste Pack / visual identity guidance is ready as the visual style reference for asset and scene review.
- Visual Artifact Factory is the reproducible CI path for Blender review PNGs and allowlisted Godot scene captures.
- Stage A terrain-diorama evidence has moved from earlier NOT_GO to `PASS`: `TC_TerrainDioramaKit_V1` has regenerated review PNG evidence with a readable scale-reference view, a Visual Reviewer `PASS`, production integration sign-off, and `StageAVisualRoot` metadata in `scenes/Main/Main.tscn`.
- Stage A generated binary source/export/review artifacts remain local or CI artifacts; tracked production state depends on text mesh resources, sign-off documentation, hashes/manifests, and scene metadata rather than committed `.blend`, `.glb`, or PNG binaries.
- `TC_HullRibOccluder_V1` has been integrated as collisionless Stage A wreckage dressing with provenance/hash evidence and import/build/PNG capture evidence; this does not expand gameplay scope.
- `TC_HeavyCrashHull_V1` still requires its own standalone artifact review and human or visual-reviewer verdict before production use.
- Gameplay MVP work should remain locked to the Crash Site loop and must not expand scope.
- **MVP scope closure — 2026-07-09:** the Crash Site MVP scope is closed per
  `docs/production/mvp-closure-report-2026-07-09.md`: all 27 README §30 acceptance criteria are satisfied
  by repository-owned evidence, re-validated against HEAD `d2a754a` (build 0/0, tests 71/71, integration
  suite PASS, import 0 errors). This is a scope-completion verdict only: human Windows gameplay feel
  validation remains `HUMAN_BLOCKED`, and the quality benchmark composite remains 4.3/10 — closure does
  not claim, and must not be quoted as, a 10/10 quality result.

## Blocked

- Any additional Stage A visual replacement or generated asset integration that lacks standalone review artifacts, hashes/manifests, opened-PNG diagnosis, and human or visual-reviewer verdict.
- Stage B work until a producer re-gate explicitly cites the reconciled Stage A evidence and confirms remaining prerequisites.
- Marketing screenshots or public-facing visual claims.
- Production asset approval without standalone review artifacts, hashes, manifest evidence, and review verdict.

## Unblocked

- Conservative Crash Site gameplay bug fixes.
- Build, validation, and CI maintenance.
- Documentation and production-operation updates requested by a human.
- Agent Studio routing and evidence improvements.
- Blender Asset Forge standalone candidate generation.
- Visual Artifact Factory review-bundle generation.
- Documentation-only reconciliation of Stage A status against `context_log.md`, `docs/art/reviews/stage-a-visual-approval-verdict.md`, `docs/production/stage-a-production-integration-signoff.md`, and `docs/production/visual-completion-checklist.md`.
- Phase 7 planning (composition guide, balance playtesting, audio design, polish, platform testing) pending Phase 6 completion.

## Stage A Reconciliation — 2026-07-07

- Earlier status lines that said Stage A generated art was not approved are superseded for the `TC_TerrainDioramaKit_V1` terrain-diorama slice only. The current evidence chain records regenerated review PNGs, opened-image visual diagnosis, Visual Reviewer `PASS`, production scene integration sign-off, and tracked `StageAVisualRoot` metadata.
- The `TC_HullRibOccluder_V1` follow-up is recorded as collisionless visual dressing with provenance/hash evidence and validation; it does not authorize gameplay changes, collision changes, or broader asset substitution.
- Generated Stage A `.blend`, `.glb`, and PNG binaries remain untracked local/CI evidence artifacts. Future reviewers should use the checked-in review/sign-off documents, manifests, hashes, and scene metadata to locate and regenerate evidence rather than expecting those binaries in git.
- This reconciliation does not mark broader beta, public demo, marketing, Stage B, or production release readiness as `GO`. Export proof, full release gates, human Windows gameplay/playtest evidence, and quality-benchmark evidence remain separate gates.

## Phase 7 Status

- **Phase 7 planning document:** Complete (docs/production/phase-7-planning.md)
- **Phase 7 roadmap entry:** Added to docs/production/production-roadmap.md
- **Phase 7 start condition:** Blocked on Phase 6 completion (visual approval, export proof, release gates)
- **Phase 7 estimated duration:** 6-10 weeks (24-40 hours) across 5 workstreams

## Quality Benchmark (target bar)

- `studio/decisions/quality_benchmark_v1.md` sets the studio's target quality bar: 10 axes, each anchored to a
  named top-tier peer in the same genre lane (Half-Life 2, Titanfall 2, Doom Eternal, Returnal, Subnautica,
  Valheim, Grounded), with a target score and the current gap on each axis.
- Every specialist agent (Producer, QA Lead, Technical Director, Art Director, Gameplay Engineer, Level
  Designer, Visual Reviewer) must cite that ADR's axis, peer anchor, and gap before signing `PASS`/`GO` on
  the corresponding system — agreement between studio agents alone is not sufficient evidence.
- The 2026-07-06 `BETA_READY` verdict (`studio/tasks/PRE_BETA_AUDIT_COMPLETE.md`) predates this ADR and does
  not meet its evidence bar: it states FPS/draw-call/GPU-ms numbers and a full Windows human playthrough with
  no supporting artifact, and it contradicts this file's own "Blocked" list above. Treat that verdict as
  **unverified**, not current, until it is re-issued with cited evidence per the ADR.

## Verification Environment Notes

- Local Codex/checkouts may not have `origin` or a local `main` branch configured. Inability to fetch `origin/main` in that environment is an environment limitation, not a repository production defect.
- Authoritative post-merge verification should run in GitHub Actions or in a full clone with `origin` configured.
- Local production-cockpit verification may use `python3 tools/blender/build_asset_manifest.py --check` to inspect generated asset availability without rewriting `assets/Production/Generated/asset_manifest.json`. Full manifest generation still belongs in GitHub Actions or local environments where generated Blender/GLB artifacts are present.
