# Visual Review — Mwezi Quarter District V1 (standalone kit + scene integration)

**Date:** 2026-09-17
**Reviewer:** Claude Code (Architecture Validator), acting on opened PNG evidence
**Subject:** `tools/blender/create_badjanani_district_kit_v1.py` kit (10 assets) and its integration into `scenes/Main/Main.tscn` via `scenes/Environment/MweziQuarterDistrict.tscn`
**Authority:** Explicit human decision 2026-09-17 to set the single MVP map in a Badjanani-inspired, renamed coastal stone quarter, recorded as a `README.md` amendment (sections 3, 14, 15, 34).

## Evidence opened

Every PNG below was rendered and **opened and looked at** during this pass, not merely produced.

**Standalone kit** — `artifacts/asset-review/TC_ENV_*_V1/` (30 PNGs: 3 views × 10 assets, each auto-framed with a render-only 1.8 m scale post):
- `hero_three_quarter.png`, `side_silhouette.png`, `player_eye_level.png` per asset.

**Scene integration** — `artifacts/visual-review/mwezi-quarter-district/` (11 PNGs, `tools/visual_review/capture_mwezi_quarter_district.gd`, allowlisted in the Visual Artifact Factory):
- `district_01_spawn_eye_level` … `district_08_terrace_overlook` (player eye height, 1.65 m, along the mission route).
- `district_09_first_person_hud`, `district_10_hit_marker_and_damage_direction`, `district_11_lethal_hit_marker`.

**Prior composition set** — `artifacts/visual-review/phase3a-production-integration/` (8 PNGs), re-rendered after each lighting change and used as the before/after comparison surface.

## Diagnosis

**Focal point.** Each route beat now has one. The spawn square reads as a breach between two stone houses; the workbench courtyard is bounded by the arcaded hall; the harbour plaza is closed by the tower. Before this pass the map had exactly one landmark (`SignalSpire_Landmark`) and no other structure competing to anchor a view.

**Route readability.** The alleys read as streets rather than as open ground with props scattered on it. `district_01`, `district_02` and `district_03` each show a walled corridor pointing at the next objective. The generator asserts more than 0.9 m of walkable clearance either side of every mission route line, so readability is not bought at the cost of a snag.

**Silhouette.** The tower carries the skyline: three tapering stages, string courses, an open belvedere. It is legible at distance in `district_06` and from the beacon front in `district_07`. The arcade's merlon parapet gives the civic hall a second recognisable roofline (`district_05`).

**Scale.** Correct and checkable. Doorways are 2.6 m, house floor bands sit at 2.9 m, the arcade springs at 2.2 m. In `district_09` the first-person view sits below the door heads and window sills, which is the read that matters.

**Material coherence.** The quarter's lime render, coral rag and weathered stone form one family, and the graphite/worn-steel/orange tones are lifted directly from the existing repo materials, so the wreck and the camp dressing do not read as a different game. The alien cyan and violet emissives still stand out as the only saturated hues in frame, which preserves the interactive-vs-decor contrast README section 15 asks for.

## Defects found and fixed during this pass

These are recorded because the first render of each was wrong, not because the process was clean.

1. **Flat-paper edges.** The first kit render showed razor-sharp, light-free edges. Fixed by applying an angle-limited 18 mm bevel to every asset before export; triangle budgets were raised to honest measured values rather than the bevel being dropped.
2. **Protruding voids.** House windows and tower slits were modelled as dark boxes centred on the wall plane, so they stuck *out* of the render. Fixed by pushing each void into the wall and adding a stone reveal just proud of it.
3. **Unreadable scale post.** The review renderer parked the 1.8 m post off to one side, where a three-quarter view put it further from the camera and perspective shrank it — the exact Stage A failure mode. Fixed by placing it in the structure's own front plane.
4. **Cropped tall assets.** The renderer solved camera distance from bounding-sphere radius, which crops tall narrow assets in a 16:9 frame; the tower lost its top and the doorway lost its lintel. Fixed by solving each axis against its own half-angle, and by removing a z-clamp that re-aimed the shot after the distance had been solved.
5. **Rubble as clean white boxes.** The first pile read as crates. Fixed by inverting the material ratio toward weathered core stone and adding chips and protruding roof timbers.
6. **Flat star palm fronds.** Fixed by rebuilding each frond from three segments of increasing droop.
7. **Four layout faults**, caught by the generator's clearance asserts rather than by eye: arcade piers inside the Scout arena, a house standing on the metal→workbench walk, a seawall 0.13 m from the beacon, a house a metre off the save point.
8. **Three lighting faults**, each caught by opening the capture: a western sun put the entire spawn square inside a 12 m building shadow; at 32° the entry houses' shadows still merged into one dark field; and the procedural sky's saturated red ground hemisphere was bouncing red onto every horizontal surface in the quarter.

## Known limitations

- The market stall canopy is oversized relative to its posts; cosmetic, not fixed.
- `district_04_workbench_courtyard` is partly occluded by a foreground timber. The camera position, not the layout, is at fault; left as captured rather than choosing a flattering angle.
- The harbour has no water. The procedural terrain rises to 4.2 m at the map bounds and would occlude any water plane behind the seawall, so the seawall is a retaining wall over a dry tidal flat.
- The wide composition view still shows the ground plate's hard boundary edge against the void. Pre-existing, outside this change.
- Volumetric fog was deliberately not enabled. Light shafts through the arcade would be the single strongest remaining image, but README section 28 forbids expensive visual effects without a measured frame budget, and no frame budget can be measured from this headless container.

## Verdict

`PASS` — as a **production asset candidate set with scene integration evidence**, on the terms below.

This verdict covers: the ten assets meet their contract (`BLENDER_ASSET_VALID` on all ten, collisionless, clean origins, within budget, provenance hashed in `assets/Production/Generated/asset_manifest.json`); the integration preserves every gameplay contract (build 0/0, unit 95/95, integration suite `TITANCRAFT_INTEGRATION_TESTS_PASS`, MVP smoke 11/11, import 0 errors, Windows export produced); and the composition is materially better than what it replaced, on opened before/after PNGs.

This verdict does **not** cover, and must not be quoted as: aesthetic sign-off by a human; any claim about how the quarter feels to walk through; or any marketing or public-facing visual claim. Per `studio/decisions/quality_benchmark_v1.md` rule 2, feel claims require a dated human playtest note, and none exists. Axis 6 remains open on overall quality and human sign-off.

## Next step

A human pulls the branch, runs the Windows build, walks the quarter, and records a dated aesthetic and feel verdict. That is the only remaining unlock for axes 2, 3, 5 and 6.
