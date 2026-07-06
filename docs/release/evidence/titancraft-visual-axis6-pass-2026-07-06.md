# TitanCraft Visual Benchmark Pass — Axis 6 (Visual Art & Presentation) — 2026-07-06

## Branch

`claude/visual-benchmark-6-10-blomv3`

## Scope verdict

This pass stays inside the Crash Site MVP boundary. It changes only `scenes/Main/Main.tscn`
(WorldEnvironment/lighting values and four pre-existing node `visible` flags). It adds no enemies,
levels, mission steps, save-semantic changes, combat rebalancing, cloud services, telemetry,
procedural world work, grappling hook, wall running, or large mech features, and no new art assets.

## Capability correction (important, affects prior BLOCKED verdicts)

Prior evidence in this repo (`artifacts/mvp_closure/20260703_windows_manual_validation_blocked.md`,
`studio/decisions/quality_benchmark_v1.md` MEM-QUALITY-BENCHMARK-003) recorded this execution
environment as having **no display**, blocking any real screenshot capture. That is no longer
accurate for this container: `godot` (4.7 mono) and `xvfb-run`/`Xvfb` are both present, and running
the existing allowlisted capture script **without** `--headless` (headless forces Godot's dummy
rendering driver, which returns a null viewport texture) produces real, non-empty rendered PNGs:

```bash
dotnet build TitanCraft.csproj -c Debug
xvfb-run -a godot --path . --script tools/visual_review/capture_phase3a_production_integration.gd
```

This does **not** unblock human/feel claims (movement, combat feel, Windows hardware performance
still require a human on real hardware per MEM-QUALITY-BENCHMARK-003) but it does mean **visual**
diagnosis claims for this scene no longer need to return `HUMAN_BLOCKED` for lack of a capture
method — a real PNG pipeline exists and was used for this pass.

## Method

1. Captured 8 baseline production views of `res://scenes/Main/Main.tscn` with the existing
   allowlisted script `tools/visual_review/capture_phase3a_production_integration.gd`
   (unmodified, already in the CI allowlist in `tools/visual_review/run_visual_artifact_factory.py`).
2. Opened and diagnosed all 8 baseline PNGs (focal point, route readability, silhouette, scale,
   materials — per `studio/checklists/before_visual_claim.md`).
3. Identified and fixed two concrete, evidence-backed defects (below).
4. Rebuilt the C# assembly, re-ran the same capture script, and re-diagnosed all 8 PNGs.
5. Compared before/after per view.

Local evidence (gitignored per `artifacts/visual-review/` policy in `.gitignore`; not committed —
regenerate with the commands above, or see the PNGs delivered directly to the requester):

- `artifacts/visual-review/axis6-2026-07-06/before/*.png` (8 files)
- `artifacts/visual-review/axis6-2026-07-06/after/*.png` (8 files)
- `artifacts/visual-review/axis6-2026-07-06/before_sha256.txt`
- `artifacts/visual-review/axis6-2026-07-06/after_sha256.txt`
- `artifacts/visual-review/axis6-2026-07-06/capture_after.log`

## Baseline diagnosis (before)

- **Focal point:** the off-white crashed hull reads as the intended focal point, but its lit face is
  blown out to near-pure white with a hard black terminator — no midtone gradation.
- **Route readability:** the ash/basalt ground patches are legible near the hull but the rest of the
  frame (roughly 60-70% of every shot) is flat, empty, near-black void — there is no continuous
  terrain surface (the real terrain instance, `ProceduralCrashSiteTerrain`, is explicitly
  `visible = false`; only three small decorative ground patches under `StageAVisualRoot/GroundVisuals`
  render).
- **Silhouette / coherence defect (bug, not style):** thin horizontal orange bars float unattached in
  open space in most views, with no wall or structure behind them. Root cause: `C7_Wall_1`..`C7_Wall_4`
  have their panel `MeshInstance3D` set `visible = false` (superseded by the Stage A custom kit) but
  their child `OrangeVent` accent mesh was left `visible = true`, so the accent renders with nothing
  behind it. This reads as broken/glitched geometry, not intentional art.
- **Scale / materials:** flat purple placeholder standee shapes (Quaternius kit humanoid stand-ins)
  and a few grey box primitives remain visible and read clearly as unfinished kit placeholders.
- **Compared with rejected baseline:** consistent with `docs/production/known-blockers.md`
  ("Stage A visual art is not approved... scenes lean on third-party kit assets") and the 2026-07-06
  scorecard baseline description ("kit-asset heavy").

## Fixes applied (both reversible, single-file, no new assets)

`scenes/Main/Main.tscn` only:

1. **Orphaned geometry bug:** added `visible = false` to the four `OrangeVent` `MeshInstance3D`
   nodes under `C7_Wall_1`..`C7_Wall_4`, matching their already-hidden parent panel. Removes the
   floating unattached bars.
2. **Exposure/ambient rebalance** on the existing `Environment_main` resource and
   `DirectionalLight3D` (values only, no new nodes):
   - `ambient_light_energy`: 0.45 → 0.6 (lifts pure-black shadow areas so geometry stays readable)
   - `tonemap_exposure`: unset (1.0) → 0.85 (added)
   - `ssao_enabled`: unset (false) → true (added, cheap depth cue, no asset cost)
   - `DirectionalLight3D.light_energy`: 1.35 → 1.05 (reduces the blown-highlight hotspot on the hull)
   - `DirectionalLight3D.shadow_bias` / `shadow_normal_bias`: added (0.25 / 3.0) — attempted fix for a
     banding artifact on one ground patch (see Known unresolved issue below; did not resolve it).

## After diagnosis

- Hull highlight now shows real midtone gradation instead of a blown white patch; still the
  brightest element in frame by design, but no longer pure-white clipped.
- Shadow-side detail (workbench desk, lamp, cabling) is now visible instead of near-black silhouette.
- The floating orphaned orange bars are gone in all 8 views — confirmed by direct visual comparison,
  not inferred.
- Void/empty-frame problem is **unchanged** — this pass did not add or resize terrain geometry, since
  that is asset/level-design work (`ProceduralCrashSiteTerrain` visibility, ground patch layout) beyond
  a lighting/bugfix pass and belongs with Art Director / Level Designer scope per `CLAUDE.md` §9.
- Purple placeholder standees are **unchanged** — replacing them is asset work, not in scope here.

## Known unresolved issue (flagged, not fixed)

A horizontal banding/scanline artifact is visible on one ground patch (near the workbench zone) in
both before and after captures. Adjusting `shadow_bias`/`shadow_normal_bias` did not change it, so it
is likely not shadow acne. It may be a real mesh/material defect, or it may be an artifact specific to
this container's software rasterizer (`llvmpipe` via `xvfb-run`, no GPU) rather than something that
would reproduce on real Windows GPU hardware. Root cause is **not confirmed** — flagging rather than
guessing further fixes, per the evidence rules in `studio/decisions/quality_benchmark_v1.md`.

## Tests run

- `dotnet build TitanCraft.csproj -c Debug` — PASS, 0 warnings/errors.
- `dotnet test tests/TitanCraft.Tests.csproj` — PASS, 71/71.
- `xvfb-run -a godot --headless --path . --import` — PASS.
- `python3 tools/agent_preflight.py "..."` — PASS (packet generated, scope warnings reviewed).
- `python3 tools/validate_agent_studio.py` — PASS.
- `git diff --check` — PASS (no whitespace errors).

## Scope verdict

No gameplay code, tests, mission logic, or scope boundaries touched. Single scene file, values-only
diff plus four pre-existing-node visibility flags. No forbidden features. No new third-party assets.

## Final verdict

`PASS` for the two fixes described above, each independently backed by opened before/after PNG
evidence. This is **not** a claim that Axis 6 (Visual art & presentation) reaches the ADR's 6/10
target — see the scorecard log entry for the honest score and the stated gap to close it further.
