# ADR: Commit Generated Blender Asset Forge Binaries

## Status

Accepted

## Date

2026-07-06

## Context

Every Blender Asset Forge candidate (`tools/blender/create_*.py`) previously produced
`.blend` sources, `.glb` exports, and review PNGs that were deliberately **not**
committed to the repository. This was documented in `.gitignore`, in
`docs/production/current-status.md`, and in `tools/blender/build_asset_manifest.py`'s
per-asset `binary_delivery` notes ("intentionally not committed for binary-review
compatibility"). Only `assets/Production/Generated/asset_manifest.json` (hashes,
triangle counts, material slots, provenance) was committed; the actual binaries only
ever existed inside a GitHub Actions run (as a downloadable workflow artifact) or a
local Blender session.

The human product owner explicitly requested the opposite: a workflow that, on push,
builds every asset candidate and commits the results back into the branch, so the
binaries live in git history rather than only in ephemeral CI artifacts.

## Decision

`.github/workflows/blender-asset-forge.yml` now runs on `push` (in addition to
`pull_request` and `workflow_dispatch`) and, after building and validating every
candidate, commits and pushes the generated `.blend` sources, `.glb` exports, and
`assets/Production/Generated/asset_manifest.json` back to the branch using a bot
identity, mirroring the existing (previously broken, now also fixed) pattern in
`mvp-phase-4-6-asset-auto-forge.yml`.

Trigger paths are scoped to inputs only (`tools/blender/**`, `docs/pipeline/**`,
`docs/art/briefs/**`, the workflow file itself) — never to the generated output
paths the bot commits to — to avoid the workflow re-triggering itself in a loop.
The job also skips when `github.actor == 'github-actions[bot]'` as a second guard,
and `push` is scoped with `branches-ignore: main` so auto-commits land on feature
branches for review, not directly on `main`.

`.gitignore` and the `binary_delivery` notes in `build_asset_manifest.py` are updated
to state that binaries are now committed, rather than the previous "not committed"
language.

## Consequences

- Positive: candidates and their review evidence are reviewable directly in the PR
  diff and durable in git history, not dependent on a time-limited CI artifact
  download.
- Negative: PR diffs for `tools/blender/**` changes now include binary `.blend`/`.glb`
  diffs, which are not human-reviewable as text. Repo size grows on every run that
  changes generated output, since `.blend` re-saves are **not** byte-identical
  across runs even with unchanged scene content (confirmed empirically: rebuilding
  `TC_LightingReference_V1.blend` twice, with no script changes, produced two different
  SHA-256 hashes, while its `.glb` export hash matched exactly both times). Git LFS
  is not configured for these paths, so this is plain git object growth.
- Neutral: `assets/Production/Generated/asset_manifest.json` remains the
  human/CI-readable source of truth for hashes and provenance; the committed
  binaries are the artifacts those hashes describe.

## Evidence

- `docs/production/current-status.md`, `tools/blender/build_asset_manifest.py`, and
  `.gitignore` previously documented and enforced the "not committed" convention
  (checked before this change).
- Verified locally: `blender --background --python tools/blender/create_lighting_reference_kit_v1.py`
  run twice produced `.blend` SHA-256 `cb21b5c4...` then `616275e7...` (differ) but
  `.glb` SHA-256 `b6ab7ffd...` identical both times.
- `git log --all --diff-filter=A -- 'artifacts/asset-review/**/*.png'` showed zero
  PNGs ever committed under the old convention, confirming the prior policy was
  fully enforced (by convention, not gitignore, for `.blend`/`.glb`; by `.gitignore`
  for PNGs).
- Fixed a pre-existing invalid-YAML bug in `mvp-phase-4-6-asset-auto-forge.yml`
  (multi-line `git commit -m` string and a `python3 <<'PYEOF'` heredoc both violated
  `run: |` block-scalar indentation rules) discovered while building the equivalent
  commit step for `blender-asset-forge.yml`; both workflow files now parse as valid
  YAML (`python3 -c "import yaml; yaml.safe_load(open(...))"`).

## Verdict

`PASS` — decision recorded, workflow implemented and YAML-validated locally;
requires the next real GitHub Actions run to confirm the commit/push step succeeds
with live repository permissions (not verifiable in this sandboxed session).
