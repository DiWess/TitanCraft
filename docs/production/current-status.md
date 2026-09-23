# TitanCraft Current Production Status

## Snapshot

- Agent Studio is ready as the repository-local operating system for task routing, memories, skills, checklists, and evidence gates.
- Blender Asset Forge is locally verified as the repeatable path for standalone generated asset candidates and GLB exports.
- Art Taste Pack / visual identity guidance is ready as the visual style reference for asset and scene review.
- Visual Artifact Factory is the reproducible CI path for Blender review PNGs and allowlisted Godot scene captures.
- Stage A terrain-diorama evidence has moved from earlier NOT_GO to `PASS`: `TC_TerrainDioramaKit_V1` has regenerated review PNG evidence with a readable scale-reference view, a Visual Reviewer `PASS`, production integration sign-off, and `StageAVisualRoot` metadata in `scenes/Main/Main.tscn`.
- Stage A generated binary source/export/review artifacts remain local or CI artifacts; tracked production state depends on text mesh resources, sign-off documentation, hashes/manifests, and scene metadata rather than committed `.blend`, `.glb`, or PNG binaries.
- `TC_HullRibOccluder_V1` has been integrated as collisionless Stage A wreckage dressing with provenance/hash evidence and import/build/PNG capture evidence; this does not expand gameplay scope.
- `TC_HeavyCrashHull_V1` has its standalone artifact review with a Visual Reviewer `PASS` as a production asset candidate (`docs/art/reviews/heavy-crash-hull-v1-standalone-review.md`, 2026-07-10: six opened review PNGs with scale reference, provenance hashes, `BLENDER_ASSET_VALID`). Production scene placement remains a separate gated task with its own before/after captures and sign-off.
- Gameplay MVP work should remain locked to the Crash Site loop and must not expand scope.
- **MVP scope closure — 2026-07-09:** the Crash Site MVP scope is closed per
  `docs/production/mvp-closure-report-2026-07-09.md`: all 27 README §30 acceptance criteria are satisfied
  by repository-owned evidence, re-validated against HEAD `d2a754a` (build 0/0, tests 71/71, integration
  suite PASS, import 0 errors). This is a scope-completion verdict only: human Windows gameplay feel
  validation remains `HUMAN_BLOCKED`, and the quality benchmark composite remains 4.3/10 — closure does
  not claim, and must not be quoted as, a 10/10 quality result.

## Mwezi Quarter District — 2026-09-17

- Per explicit human direction, the single MVP map's **setting** was amended from empty volcanic terrain to
  a built coastal coral-stone quarter, the **Mwezi Quarter** (`README.md` sections 3/14/15 and the section 34
  locked-decision table amended). MVP scope is unchanged: one map, one enemy, the same mission anchors, the
  same loop, and nothing added from the section 6 forbidden list.
- Ten project-authored Blender assets (`docs/art/briefs/mwezi-quarter-district-v1.md`), all
  `BLENDER_ASSET_VALID`, hashed in `assets/Production/Generated/asset_manifest.json`, with 30 tracked
  opened review PNGs under `artifacts/asset-review/TC_ENV_*_V1/`.
- District layout is generated, not hand-placed: `tools/level/build_mwezi_quarter_district.py` solves ground
  height against the procedural terrain and refuses to emit a scene that violates mission-anchor, route,
  Scout-arena or passage clearance. It rejected four faulty layouts during authoring.
- Lighting/post reworked (ACES, sky-sourced ambient, SSAO/SSIL, glow, aerial haze, grade, re-aimed key
  light) and the walkable ground palette recoloured to coral sand, with the distant ridge kept dark
  volcanic rock. Three lighting faults were caught by opening captures and fixed.
- Game-feel layer added: stride-locked view bob, strafe lean, landing impact, weapon kick, viewmodel sway,
  plus a procedurally drawn crosshair / hit marker / damage-direction overlay. 20 new unit tests.
- Validation: build Debug+Release 0/0, unit 95/95, integration `TITANCRAFT_INTEGRATION_TESTS_PASS`,
  MVP smoke 11/11, import 0 errors, Windows export produced.
- Verdict `PASS` as a candidate set with integration evidence
  (`docs/art/reviews/mwezi-quarter-district-v1-review.md`). **Not** an aesthetic or feel sign-off: the
  quality composite moved 4.5 → 5.1, not to 10/10, and axes 2/3/6 remain `HUMAN_BLOCKED` on a dated human
  Windows playtest.
- Known pre-existing failure, untouched and not caused by this slice: `python3 tools/validate_agent_studio.py`
  fails on `studio/skills/human_playtest_intake.md` (missing `example_good_output` / `example_bad_output`
  headings). Verified identical on a clean tree; owned by the Studio governance surface, not this change.

## Mwezi Quarter — Motion, Onboarding and Grade — 2026-09-23

- Blender-authored environment motion: four skinned, looping assets (palm sway, laundry line, awning
  cloth, banner cloth), ten placements on the walked routes, each phase- and rate-offset per instance by
  `src/World/EnvironmentMotionPlayer.cs`. glTF carries no looping flag, so without that node every prop
  would sway once and freeze — the integration suite now asserts loop mode, playback and phase spread.
- Animated assets use armatures because the asset contract requires meshes at a clean origin, and ship
  through `tools/blender/export_animated_asset.py`: the static exporter applies modifiers and would have
  stripped every skin, producing files that look correct and never move.
- Action-driven onboarding (look → move → jump → collect → craft → attack), one short line at a time,
  advancing only on real gameplay and retiring itself. Additive: the existing controls-reference line and
  its assertions are unchanged, per README section 7's ban on a long text tutorial.
- Two pre-existing presentation defects fixed, both found by measurement rather than by looking: the
  grade's `adjustment_contrast = 1.08` on top of ACES was crushing every dark prop to pure black
  (albedo 0.5 rendered at 0.133, 0.235 at 0.008), and the ash route ribbon carried 42 of 48 inverted
  normals so the map's main navigation aid was shaded as a downward face.
- Validation: build Debug+Release 0/0, unit 116/116, integration `TITANCRAFT_INTEGRATION_TESTS_PASS`,
  MVP smoke 11/11, import 0 errors, Windows export produced.
- Quality composite 5.1 → 5.3. Still not a 10/10 and still not an aesthetic or feel sign-off; the
  structural caps on axes 9 and 2/3 are unchanged.

## Blocked

- Any additional Stage A visual replacement or generated asset integration that lacks standalone review artifacts, hashes/manifests, opened-PNG diagnosis, and human or visual-reviewer verdict.
- Stage C production-scene integration of `MVP_Pack_V1` assets without its own before/after captures, preserved gameplay collision contracts, and integration sign-off (Stage B approval is candidate-level only).
- Marketing screenshots or public-facing visual claims.
- Production asset approval without standalone review artifacts, hashes, manifest evidence, and review verdict.

## Unblocked

- Conservative Crash Site gameplay bug fixes.
- Build, validation, and CI maintenance.
- Documentation and production-operation updates requested by a human.
- Agent Studio routing and evidence improvements.
- Blender Asset Forge standalone candidate generation.
- Visual Artifact Factory review-bundle generation.
- Agent-gated Windows playtest journey runs (`.github/workflows/windows-playtest-journey.yml`) and
  verdict documents in `docs/production/playtests/` validated by `tools/validate_playtest_evidence.py`,
  per `studio/decisions/quality_benchmark_v2_agent_gate_delegation.md` (aesthetic verdicts delegated to
  the Visual Reviewer agent on opened CI captures; feel adjectives still require a dated human note).
- Documentation-only reconciliation of Stage A status against `context_log.md`, `docs/art/reviews/stage-a-visual-approval-verdict.md`, `docs/production/stage-a-production-integration-signoff.md`, and `docs/production/visual-completion-checklist.md`.
- Phase 7 planning (composition guide, balance playtesting, audio design, polish, platform testing) pending Phase 6 completion.
- Stage C Task #6 (Level Designer): visual-only integration of the approved `MVP_Pack_V1` candidates into the Crash Site scene, per `docs/production/stage-b-producer-gate-2026-07-18.md`.

## Stage B Completion — 2026-07-18

- The producer re-gate required above was issued in `docs/production/stage-b-producer-gate-2026-07-18.md`, citing the reconciled Stage A evidence (terrain diorama PASS, hull rib occluder integration, heavy crash hull standalone PASS).
- Stage B is `PASS` for MVP Asset Pack V1: 13 GLB candidates with verified manifest hashes, tracked PNG review bundles (regenerated with a `scale_reference.png` view containing a render-only 1.8 m post), a real opened-PNG Visual Reviewer verdict (13/13 PASS with three MINOR corrective notes — `docs/art/reviews/mvp-pack-v1-visual-review-2026-07-18.md`), and a real Technical Director audit (build 0/0, tests 75/75, import 0 errors — `docs/production/mvp-pack-v1-technical-audit-2026-07-18.md`).
- The 2026-07-06 simulation cluster (`PRODUCER_GATE_VERDICT_STAGE_B.md` etc.) remains NOT REAL and was not cited as evidence.

## Stage C Validation & Release Evidence — 2026-07-18

- Stage C is `PASS` per `docs/production/stage-c-integration-validation-2026-07-18.md`: the approved `MVP_Pack_V1` candidates were found already wired into the production scenes via committed text `.gltf` deliverables; the wiring was verified per-prop, the full gameplay loop passed (`./tools/test.sh`: unit 75/75 plus all 11 MVP smoke milestones on the integrated scenes), and eight in-engine captures were generated with the allowlisted factory and opened for visual diagnosis.
- Known evidence gap: the first-person mechanical-arm capture does not show the crafted arm; wiring and runtime path are test-covered, but a crafted-state capture is required before any visual-approval claim for the arm.
- Release: Windows export proof exists (`godot --headless --export-release "Windows Desktop"`, exit 0, exe SHA-256 recorded); the release gate itself remains `HUMAN_BLOCKED` on the README §27 manual Windows validation and human GO.

## Polish Slice — 2026-07-18 (later same day)

- Per `docs/production/polish-slice-2026-07-18.md`: fixed a BLOCKER-class first-person visual bug (legacy Quaternius George proxy mesh rendered over the crafted MVP arm — every crafted playthrough was affected), re-tuned the arm as a proper viewmodel, closed the crafted-arm capture gap with a new allowlisted capture script (both Stage C findings now closed), added collisionless foreground dressing off-route, and resolved the Stage B Biomass color note as a review-lighting artifact (asset data matches the brief; no change needed).
- Validation after polish: build 0 errors, unit 75/75, MVP smoke 11/11, import clean, capture factory green with all new/changed views opened.
- Remaining gate to ship: the human Windows playthrough and GO (README §27); gameplay feel tuning (provisional README §11 combat values) is deliberately deferred to that same human validation.

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
