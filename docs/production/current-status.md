# TitanCraft Current Production Status

## Snapshot

- Agent Studio is ready as the repository-local operating system for task routing, memories, skills, checklists, and evidence gates.
- Blender Asset Forge is locally verified as the repeatable path for standalone generated asset candidates and GLB exports.
- Art Taste Pack / visual identity guidance is ready as the visual style reference for asset and scene review.
- Visual Artifact Factory is the reproducible CI path for Blender review PNGs and allowlisted Godot scene captures.
- Stage A autonomous generated art is not approved.
- `TC_HeavyCrashHull_V1` has a revised recipe and review workflow path, but production use still depends on standalone artifact review and a human or visual-reviewer verdict.
- Gameplay MVP work should remain locked to the Crash Site loop and must not expand scope.

## Blocked

- Stage A visual replacement in production scenes.
- Stage B work.
- Marketing screenshots or public-facing visual claims.
- Production asset approval without standalone review artifacts, hashes, manifest evidence, and review verdict.

## Unblocked

- Conservative Crash Site gameplay bug fixes.
- Build, validation, and CI maintenance.
- Documentation and production-operation updates requested by a human.
- Agent Studio routing and evidence improvements.
- Blender Asset Forge standalone candidate generation.
- Visual Artifact Factory review-bundle generation.
- Phase 7 planning (composition guide, balance playtesting, audio design, polish, platform testing) pending Phase 6 completion.

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
