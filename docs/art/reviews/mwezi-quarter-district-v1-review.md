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

**Material coherence (revised 2026-09-24).** The quarter's lime render, dressed coral rag, ink-black plaster-loss core and weathered stone form one family, and the graphite/worn-steel/orange tones are lifted directly from the existing repo materials, so the wreck and the camp dressing do not read as a different game. The alien cyan and violet emissives still stand out as the only saturated hues in frame, which preserves the interactive-vs-decor contrast README section 15 asks for.

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

## Second pass — 2026-09-23 (motion, onboarding, and three defects it exposed)

### Added

- **Motion kit**: four skinned, looping animated assets (palm sway, laundry
  line, awning cloth, banner cloth), ten placements on the walked routes.
  Verified in-engine: all 10 `EnvironmentMotionPlayer` nodes playing, looping,
  with distinct start phases and playback rates. Motion evidence is four frames
  across each asset's loop plus two in-engine frames 1.5 s apart.
- **Onboarding**: a six-step, action-driven flow (look → move → jump → collect
  → craft → attack) showing one short line at a time and retiring itself. It
  advances only on real gameplay, and a player who ignores it and simply plays
  is pulled forward rather than left behind. Deliberately additive: the existing
  controls-reference line is unchanged, because README section 7 forbids leaning
  on a long text tutorial and the reference line is a reminder, not a teacher.

### Defects this pass found and fixed

1. **Environment motion would have shipped frozen.** glTF carries no looping
   flag, so Godot imported every clip with looping disabled. Without
   `EnvironmentMotionPlayer` setting it, each prop swayed once and then stood
   still for the rest of the session — and would have rendered identically in
   any still screenshot. The integration suite now asserts loop mode, playback,
   and distinct phases.
2. **Near-identical motion seeds.** The first per-instance hash was a single
   weighted sum; two awnings 21 m apart produced seeds 0.7257 and 0.7253, so
   they swayed in visible lockstep. Replaced with an FNV-1a plus avalanche mix,
   now asserted to keep every district placement more than 0.02 apart.
3. **The tone curve was crushing every dark prop to pure black.** Chasing a
   black box in a capture, measurement on the live scene showed the grade's
   response: albedo 1.0 rendered at 0.494, 0.5 at 0.133, and 0.235 at 0.008.
   The cargo crate two metres from the player was rendering `(0, 0, 0)` even
   with the sun at 12 energy and shadows disabled — it was not a shadow, a
   material fault or a normals fault, but an `adjustment_contrast = 1.08` boost
   stacked on ACES. Contrast is now neutral, with exposure and ambient carrying
   the image; the crate reads as a dark object again. This affected every dark
   "tech" surface in the game, not just the crate.
4. **The ash route was shaded as if it faced the ground.** The route ribbon's
   left/right edges swap sides wherever the route doubles back, which gave the
   generic triangle builder mixed winding: 42 of 48 vertices carried downward
   normals. The brightest surface in the map and the player's main navigation
   aid (README section 7) was being lit as a downward face, with parts
   backface-culled from above. The ribbon is horizontal by construction, so it
   now gets an explicit upward normal and consistent winding — 48 of 48 up.

Defects 3 and 4 were pre-existing in the first pass and are exactly the kind of
fault that a still screenshot review does not catch: both produce an image that
looks deliberate.

## Known limitations

- The market stall canopy is oversized relative to its posts; cosmetic, not fixed.
- `district_04_workbench_courtyard` is partly occluded by a foreground timber. The camera position, not the layout, is at fault; left as captured rather than choosing a flattering angle.
- The harbour has no water. The procedural terrain rises to 4.2 m at the map bounds and would occlude any water plane behind the seawall, so the seawall is a retaining wall over a dry tidal flat.
- The wide composition view still shows the ground plate's hard boundary edge against the void. Pre-existing, outside this change.
- The motion kit's palm is a single straight trunk while the static palm cluster
  has two leaning trunks, so the two read as different plants at close range.
- Volumetric fog was deliberately not enabled. Light shafts through the arcade would be the single strongest remaining image, but README section 28 forbids expensive visual effects without a measured frame budget, and no frame budget can be measured from this headless container.

## Addendum, 2026-09-24 — description-sourced material correction

Written descriptions of Badjanani and Mtsangani replaced the spec's photo-gated
placeholders. Three of the values in the shipped kits were not merely
unconfirmed, they were **wrong**, and are recorded here because a reference that
never corrects the art is not doing any work:

| What | Was | Now |
| --- | --- | --- |
| Plaster-loss core | pale coral rag `0.561, 0.510, 0.424` | ink-black `0.090, 0.086, 0.082` |
| Quarter woodwork | dark hardwood `0.224, 0.145, 0.086` | bleached `0.435, 0.400, 0.353` |
| Civic hall plinth | pale coral platform | dark volcanic base, 1.6 m |

Recolouring the wall patches was only half of the first fix, and the review
renders were what proved it. Three attempts, each opened and diagnosed:

1. Patch slab proud of the wall face — read as dark panels hung on a white wall,
   the same protruding-void failure caught earlier on the window openings.
2. Patch slab recessed into the wall — the patches disappeared entirely. A
   recess buried inside a solid render box is not a hole, it is hidden geometry.
3. Rubble core carrying a plaster skin, the skin boolean-cut at each patch —
   correct. A fourth render then caught z-fighting where the core and skin
   shared a coplanar end face, fixed by insetting the core on X and Z as well.

`TC_ENV_CoralWallSegment_V1` rose 396 → 992 triangles and its budget 460 → 1060
as a result. `TC_ENV_BeachPocket_V1` is new, and took two failed attempts of its
own before the sand became a displaced grid rather than box strips reading as
white decking.

Full detail, including the contested dating of the reference building and the
religious features deliberately omitted under README section 67, is in
`docs/art/references/comorian-seafront-reference-v1.md`.

**Human verdict, 2026-09-24.** A human viewed three of these renders --
`TC_ENV_CoralWallSegment_V1/player_eye_level.png`,
`TC_ENV_CivicHallSeafront_V1/hero_three_quarter.png` and
`TC_ENV_BeachPocket_V1/hero_three_quarter.png` -- and validated them. That is
the first human visual sign-off on this pass and it is recorded, with its
limits, in `docs/art/reviews/2026-09-24-human-render-verdict.md`. It covers
three images. It is not a playtest, it does not approve the ~46 assets that
were not shown, and the feel axes stay `HUMAN_BLOCKED` under
`studio/decisions/quality_benchmark_v1.md` rule 2.

## Verdict

`PASS` — as a **production asset candidate set with scene integration evidence**, on the terms below.

This verdict covers: the ten assets meet their contract (`BLENDER_ASSET_VALID` on all ten, collisionless, clean origins, within budget, provenance hashed in `assets/Production/Generated/asset_manifest.json`); the integration preserves every gameplay contract (build 0/0, unit 95/95, integration suite `TITANCRAFT_INTEGRATION_TESTS_PASS`, MVP smoke 11/11, import 0 errors, Windows export produced); and the composition is materially better than what it replaced, on opened before/after PNGs.

This verdict does **not** cover, and must not be quoted as: aesthetic sign-off by a human; any claim about how the quarter feels to walk through; or any marketing or public-facing visual claim. Per `studio/decisions/quality_benchmark_v1.md` rule 2, feel claims require a dated human playtest note, and none exists. Axis 6 remains open on overall quality and human sign-off.

## Next step

A human pulls the branch, runs the Windows build, walks the quarter, and records a dated aesthetic and feel verdict. That is the only remaining unlock for axes 2, 3, 5 and 6.
