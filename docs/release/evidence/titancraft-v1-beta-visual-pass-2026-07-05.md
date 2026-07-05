# TitanCraft Crash Site V1 Beta Visual Pass Evidence — 2026-07-05

## Branch

work

## Commit SHA

Final commit SHA is reported in the pull request and final response because amending this evidence file changes the commit identifier.

## Scope verdict

The pass stays within the Crash Site MVP visual clarity boundary. It adds no enemies, levels, mission steps, victory changes, save semantic changes, combat rebalancing, cloud services, telemetry, procedural world work, grappling hook, wall running, or large mech features.

## Files changed

- `art/blender/README.md`
- `art/blender/v1_beta_asset_manifest.json`
- `assets/models/v1_beta/*.gltf`
- `docs/art/titancraft-v1-beta-visual-bible.md`
- `docs/release/evidence/titancraft-v1-beta-visual-pass-2026-07-05.md`
- `artifacts/review/v1_beta_visual_pass_2026-07-05/visual-evidence-workaround.svg`
- `artifacts/review/v1_beta_visual_pass_2026-07-05/visual-evidence-workaround.md`

## Blender files created

No binary `.blend` files were created because Blender is not installed in this execution container. The Blender source folder was created with an explicit source note and deterministic repo-owned GLTF blockout exports for later Blender authoring.

## Exported assets

See `art/blender/v1_beta_asset_manifest.json` for source, license, path, and SHA-256 provenance for each exported GLTF blockout candidate.

## Godot scenes changed

No Godot scene node hierarchy was changed in this pass. Existing runtime scenes already contain the required readable workbench, beacon, pickup rings, save point core, Scout, terrain landmarks, and mechanical arm visibility contract; this pass documents and adds V1 beta asset candidates without risking script node references.

## Tests run

- `python3 tools/agent_preflight.py "TitanCraft Blender V1 beta visual pass"` — PASS.
- `python3 tools/validate_agent_studio.py` — PASS.
- `python3 tools/test_agent_task_router.py` — PASS.
- `python3 tools/test_agent_preflight.py` — PASS.
- `dotnet test tests/TitanCraft.Tests.csproj` — PASS, 71 tests.
- `./tools/test.sh` — PASS, includes Godot import, integration smoke through victory, and export smoke.
- `git diff --check` — PASS.
- `git status --short --untracked-files=all` — PASS as inspection command before commit.
- Binary evidence workaround validation — PASS: binary PNG placeholders removed; diffable SVG and Markdown evidence notes added.

## Visual evidence path

`artifacts/review/v1_beta_visual_pass_2026-07-05/`

Diffable workaround files:

- `visual-evidence-workaround.svg`
- `visual-evidence-workaround.md`

Visual diagnosis: the binary PNG placeholders were removed because this PR needs a binary-file-not-supported workaround. The SVG contact sheet is text-diffable and documents the intended focal point and route/readability functions. Focal point is the cyan objective marker; route readability is the orange path stripe; silhouette separation is high for beacon/crash landmark in the schematic; scale is readable as blockout; materials are simple flat colors with no complex shader risk. This workaround does not satisfy the README PNG evidence gate for final visual approval.

## Performance notes

The GLTF assets are primitive embedded-buffer blockout meshes with simple PBR material factors. They should not create a material or shader complexity regression.

## Gameplay loop result

Automated validation passed: Agent Studio checks, .NET unit tests, Godot import, integration smoke, export smoke, diff whitespace check, and git status inspection were run. Manual V1 Beta playthrough is incomplete in this non-interactive container, so the release verdict cannot be `V1_BETA_VISUAL_PASS_READY`.

## Known limitations

- Blender binary authoring was unavailable in the container.
- Live Godot screenshot capture and human/visual-reviewer approval were not completed.
- No Windows beta executable was exported in this pass.
- Binary PNG placeholders were removed for review compatibility; live Godot PNG evidence is still required before any visual readiness claim.

## Final verdict

V1_BETA_VISUAL_PASS_PARTIAL
