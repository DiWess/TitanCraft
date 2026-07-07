# TC_HeavyCrashHull_V1 Standalone Review

Owner: Art Director / Visual Reviewer
Version: 1.0
Date: 2026-07-07
Review status: Visual Reviewer verdict recorded as `PASS` after two geometry/camera/lighting correction rounds; asset brief verdict is `TC_HEAVY_CRASH_HULL_V1_READY_FOR_HUMAN_REVIEW`

## Asset summary

- Asset name: `TC_HeavyCrashHull_V1`
- Brief: `docs/art/briefs/TC_HeavyCrashHull_V1.md`
- Purpose: standalone visual-only crash-site hero wreck module candidate. Not Stage A scene art, not integrated into `Main.tscn`, no collision.
- Source recipe: `tools/blender/create_heavy_crash_hull_v1.py`
- Source path: `assets/Source/Blender/Production/TC_HeavyCrashHull_V1.blend` (tracked in git)
- Production export path: `assets/Production/Generated/CrashWreck/TC_HeavyCrashHull_V1.glb` (tracked in git)
- Review render script: `tools/blender/render_heavy_crash_hull_v1_reviews.py`
- Review output directory: `artifacts/asset-review/TC_HeavyCrashHull_V1/` (tracked in git)

This task closed the outstanding item noted in `docs/production/current-status.md`: "`TC_HeavyCrashHull_V1` still requires its own standalone artifact review and human or visual-reviewer verdict before production use."

## Round 1 — initial render, NOT_GO

Rendered the six brief-required PNGs (`neutral_front_three_quarter`, `neutral_side_silhouette`, `neutral_rear_engine`, and their `material_*` counterparts) from the already-committed source blend using the existing camera rig in `render_heavy_crash_hull_v1_reviews.py`.

Opened all six PNGs. Findings:

- The delivered `*_side_silhouette` shots did not show the hull's silhouette at all — the camera was aimed almost straight at the near breach/rib wall (`y ≈ -2.5` to `-3.0`) rather than the full ~13 m length, so the brief's explicit requirement ("heavy length, flattened underside, and non-capsule silhouette must read without materials") had no supporting evidence.
- Where the rib area was visible, it read as a picket-fence/scaffold of thin, evenly spaced parallel bars rather than integrated exposed ribs with visible thickness — close to the brief's forbidden "random cubes glued together" and "blockout-looking primitive silhouette."
- Several crushed-front and torn-panel fragments were rotated at steep angles (up to 35°) far enough from the hull mass that they read as debris floating in empty space rather than attached structure.
- The rear-engine read (circular mount, concentric rings, radial vanes) was acceptable on its own.

Verdict: **NOT_GO**. Reason: missing silhouette evidence required by the brief, and the visible geometry risked the forbidden "random cubes glued together" shape language.

## Round 2 — camera pull-back, still NOT_GO

Pulled all three cameras back to frame the full hull bounding box (~13 m x 6 m x 3.2 m) and reduced the rotation extremity on the loose front/breach fragments in `create_heavy_crash_hull_v1.py`.

This confirmed the geometry problem was real, not just a framing problem: with the full hull in frame, the middle third of the ship was dominated by the rib lattice, and the picket-fence read remained the dominant silhouette feature from the breach-facing side.

Verdict: **NOT_GO**. Reason: the "side/silhouette" shot, viewed from the same broadside face as the rib breach, could not simultaneously satisfy "torn breach must read" (front three-quarter's job) and "heavy length / flattened underside / non-capsule silhouette must read without materials" (the side view's job per the brief) — the breach geometry overwhelmed the pure-profile read.

## Round 3 — corrected framing and lighting, PASS

Two further changes:

1. Re-aimed the `side_silhouette` camera at the intact `+Y` hull face (the one carrying `manufactured_hull_panel` / `worn_steel_panel_seam` geometry) instead of the `-Y` breach face, so this shot does the job the brief asks of it: a clean profile read of length, flattened underside, and non-capsule silhouette, while the front-three-quarter shot continues to carry the breach/rib read.
2. Fixed a real lighting bug in `render_heavy_crash_hull_v1_reviews.py`: the area lights were created at a position but never aimed at the subject (`_look_at` was only ever called on the camera), so flipping the side camera left the visible face almost unlit. Both lights are now aimed at the same `target` as the camera.

Re-rendered all six PNGs and re-opened them:

- `neutral_side_silhouette.png` / `material_side_silhouette.png`: now show a continuous, broad, asymmetric hull mass with a visibly flattened underside, recessed panel seams, and a non-capsule silhouette, with the crumpled front and rear engine mount visible at the ends as grounded detail rather than floating debris. This satisfies the brief's silhouette requirement.
- `neutral_front_three_quarter.png` / `material_front_three_quarter.png`: broad damaged front and torn side breach read clearly, with the internal ribs now sitting inside a visible shadowed cavity instead of floating loose bars.
- `neutral_rear_engine.png` / `material_rear_engine.png`: circular engine mount, concentric rings, and radial mounting struts read as structured industrial hardware.
- Material pass: off-white hull, graphite underside/interior voids, worn-steel ribs and seams, a single muted-orange marking stripe, and a small, non-excessive localized cyan breach slot are all present and distinguishable without looking like glossy toy sci-fi.

Verdict: **PASS**. No remaining forbidden-shape read; all three required camera angles now supply the evidence the brief asks for.

## Validation

- `python3 tools/blender/validate_blender_asset.py assets/Source/Blender/Production/TC_HeavyCrashHull_V1.blend` — `BLENDER_ASSET_VALID`, 0 issues, 904 triangles (budget 1500), 5 material slots, all `TC_` mesh names, no collision.
- `dotnet build TitanCraft.sln` — succeeded, 0 warnings, 0 errors.
- `godot --headless --path . --import` (via `xvfb-run`) — completed without error (this asset is not placed in any scene; import was run defensively since the GLB changed).

## Hashes

Recorded in `assets/Production/Generated/asset_manifest.json` and `artifacts/asset-review/TC_HeavyCrashHull_V1/sha256sums.txt`.

- `assets/Source/Blender/Production/TC_HeavyCrashHull_V1.blend`: `eaa887ca888305198f96c4d2eb543abe85d054d5c5fc306fb035dd02190e7bbc`
- `assets/Production/Generated/CrashWreck/TC_HeavyCrashHull_V1.glb`: `b3ebdd087bea4330ac0aa4a7f5710579e573a67ebe1dab48be51147bb39f9f9a`

## Scope

- No gameplay code, collision, or scene changes were made.
- This asset remains a standalone candidate: it is not integrated into `Main.tscn` or any Stage A scene. Production integration is a separate, later decision per the brief ("not approved Stage A scene art").
- Files touched: `tools/blender/create_heavy_crash_hull_v1.py` (geometry), `tools/blender/render_heavy_crash_hull_v1_reviews.py` (camera + lighting), the regenerated `.blend`/`.glb` binaries, `assets/Production/Generated/asset_manifest.json` (hash refresh only, for this asset's entry), and the six review PNGs under `artifacts/asset-review/TC_HeavyCrashHull_V1/`.

## Implementation verdict

`TC_HEAVY_CRASH_HULL_V1_READY_FOR_HUMAN_REVIEW`

Reason: the asset now has standalone review artifacts, a passing mesh-contract validation, a recorded Visual Reviewer `PASS` against the brief's shape-language and per-angle evidence requirements, and green build/import validation. Per the brief, this candidate still requires a human (or future producer re-gate) decision before any production/Stage A integration is authorized — this task does not grant that integration.

## Visual verdict

`PASS`

Reason: the Visual Reviewer opened all six regenerated PNGs across three correction rounds and diagnosed the forbidden-shape risk, the missing silhouette evidence, and the lighting defect that were blocking approval, then confirmed each was resolved in the final render set.
