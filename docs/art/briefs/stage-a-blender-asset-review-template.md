# Stage A Blender Asset Review Template

Owner: TitanCraft Visual Director  
Version: 1.0  
Date: 2026-07-05  
Review status: Template ready; not asset approval

## Asset identity

- Asset name:
- Stage A category:
- Gameplay role in locked Crash Site MVP:
- Visual role:
- Authoring method: `hand-authored Blender` / `script-authored Blender` / `hybrid`
- Source path:
- Export path:
- Review artifact path:

## Scope constraints

- No gameplay code.
- No gameplay collision unless explicitly scoped in a future task.
- No new map area, biome, resource, enemy, recipe, weapon, mission step, or system.
- No `Main.tscn` integration before standalone review passes.
- No final visual approval claim from generated PNGs alone.

## Required source contract

- Mesh object names start with `TC_`.
- Collision metadata is `none`.
- Material slots are named and assigned.
- Origin and dimensions are intentional.
- Triangle count is documented.
- Source/export/review hashes are recorded.

## Required review renders

- front
- back
- left
- right
- top
- 3/4 hero
- scale reference with player capsule
- material preview
- wireframe or mesh-stat report
- contact sheet

## Visual scorecard

| Category | Score 0-5 | Notes |
|---|---:|---|
| Silhouette readability |  |  |
| Non-toy industrial/alien read |  |  |
| Material coherence |  |  |
| Scale |  |  |
| Gameplay readability |  |  |
| Crash Site palette integration |  |  |
| Forbidden-shape avoidance |  |  |
| Screenshot value |  |  |

## Metadata record

- Asset name:
- Source file:
- Exported file:
- Source hash:
- Export hash:
- Review PNG hashes:
- Triangle count:
- Material slots:
- Dimensions:
- Binary committed: `yes/no`
- Binary artifact-only: `yes/no`
- Approval verdict:

## Verdict

Use one:

- `BLENDER_ASSET_REVIEW_READY`
- `BLENDER_ASSET_IMPLEMENTATION_PASS`
- `BLENDER_ASSET_NOT_GO`
- `BLENDER_ASSET_APPROVED`
