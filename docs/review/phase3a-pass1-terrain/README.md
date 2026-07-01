# Phase 3A Pass 1 Terrain Visual Review Artifacts

The Phase 3A Pass 1 terrain screenshots are generated in GitHub Actions by the `Phase 3A Visual Review` workflow. PNG review files are temporary workflow evidence and are not committed to the repository.

## How to download the artifact

1. Open the pull request or the repository **Actions** tab.
2. Open the `Phase 3A Visual Review` workflow run for the commit being reviewed.
3. Download the artifact from the run's **Artifacts** section.
4. The expected artifact name is `phase3a-pass1-terrain-<commit-sha>`.

## Expected files

The downloaded artifact contains:

- `terrain_01_spawn_route.png`
- `terrain_02_foreground_midground.png`
- `terrain_03_combat_zone.png`
- `terrain_04_wide_crash_site.png`
- `terrain_contact_sheet.png`
- `manifest.json`
- `runtime-contract-report.json`
- `capture.log`
- `test-summary.txt`

## Verifying hashes

Open `manifest.json` and compare each file's `sha256` value with a local hash command, for example:

```bash
sha256sum terrain_01_spawn_route.png
```

The computed hash must match the manifest entry for the same filename. The original four terrain screenshots are authoritative; the contact sheet is only a review convenience.

## Manual workflow trigger

To regenerate the evidence without opening a new pull request, open **Actions**, select `Phase 3A Visual Review`, choose **Run workflow**, select the target branch, and run `workflow_dispatch`.

Pass 2 remains blocked until a human reviews and approves the Pass 1 terrain visual artifact.

## Terrain asset qualification follow-up

After human review returned `PASS1_TERRAIN_VISUAL_NOT_GO`, the failed `Main/AuthenticatedTerrainVisuals` composition was removed from production. The workflow now also produces a separate asset-qualification artifact named `phase3a-terrain-asset-qualification-<commit-sha>` so reviewers can inspect the audit before any future terrain composition pass. This artifact is diagnostic only and does not unblock Pass 2.
