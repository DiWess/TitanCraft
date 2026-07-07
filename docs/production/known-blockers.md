# Known Blockers

- Stage A visual art is not approved as a whole for overall art-quality or marketing purposes: quality-benchmark axis 6 remains well below its 9.0 target (`docs/production/quality-scorecard-log.md`). One slice, `TC_TerrainDioramaKit_V1`, has completed its own standalone-review-artifact process and holds a narrow Visual Reviewer PASS plus production integration sign-off for that asset only (`docs/production/current-status.md`'s 2026-07-07 reconciliation, `docs/production/stage-a-production-integration-signoff.md`) — this does not extend to any other Stage A asset or to overall visual-quality/marketing readiness.
- Autonomous mesh generation is exhausted without stronger references and a human gate.
- Production asset approval requires standalone artifact review, including source, GLB, manifest, hashes, and PNG review evidence.
- The GitHub Visual Artifact Factory workflow must be used for review bundles when local visual tooling is unavailable or when CI reproducibility is required.
- No marketing screenshots, public visual claims, Stage B work, or production-scene visual replacement should proceed until visual approval exists.

## Capability correction — 2026-07-06

Prior blocker notes and `studio/decisions/quality_benchmark_v1.md` (`MEM-QUALITY-BENCHMARK-003`)
recorded this execution container as having no display, blocking real screenshot capture. Confirmed
2026-07-06: `godot` and `xvfb-run` are both present in this container, and running the existing
allowlisted capture script **without** `--headless` (headless forces Godot's dummy rendering driver,
which yields a null viewport texture) produces real rendered PNGs — see
`docs/release/evidence/titancraft-visual-axis6-pass-2026-07-06.md` for the exact command and cited
before/after evidence. This does not unblock human feel claims (movement, combat, Windows hardware
performance) — those still require a human on real hardware — but it does mean future visual-diagnosis
tasks in this container are no longer automatically `HUMAN_BLOCKED` for lack of a capture method.
