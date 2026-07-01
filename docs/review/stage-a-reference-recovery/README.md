# TitanCraft Phase 3A Stage A — Reference Recovery Asset Gate

Final verdict: `BLOCKED_BY_FREE_ASSET_SET`

## Scope

This report continues Phase 3A Stage A after the prior `BLOCKED_BY_REFERENCE_IMAGE` result. The reference attachment was not exposed at the literal prompt path, and no PNG/JPG attachment was discoverable in the allowed common attachment locations, so this pass used the human-authorized locked composition blueprint as the reference specification rather than inventing a replacement image.

No production scene files were modified. In particular, `scenes/Main/Main.tscn` was not touched because the free authenticated asset gate did not pass.

## Required pre-production reads

Before asset recovery, the following gameplay and production-safety files were inspected:

- `README.md`
- `AGENTS.md`
- `project.godot`
- `scenes/Main/Main.tscn`
- `docs/architecture/runtime-flow-map.md`
- `docs/architecture/production-scene-contracts.md`
- `docs/phase3a-visual-review.md`
- `tests/Integration/IntegrationTestRunner.cs`
- `artifacts/runtime-contract-report.json`

The relevant safety decision is that gameplay roots, scripts, node paths, collisions, player spawn, pickup locations, workbench, Galaxabrain spawn, save point, beacon, mission progression, victory/defeat, and save compatibility remain authoritative. Visual composition may change only after the asset gate passes.

## Reference handling

- Literal prompt path checked: `<REFERENCE_FILE>` — not present.
- Allowed attachment locations checked: `/mnt/data`, `/tmp`, `/workspace` — no attached PNG/JPG reference found.
- Authorized fallback used: locked Stage A composition blueprint from the task.
- Reference image committed: no.
- Copied reference artifact committed: no.

### Blueprint composition recorded

- Dominant wreck silhouette: damaged heavy industrial exploration vessel in the central-left midground, occupying roughly 30–40% of the player-spawn frame.
- Terrain shape: dark volcanic, readable medium-value facets, irregular authored rocks, no rectangular terrain board.
- Foreground framing: low volcanic rocks that frame the lower image without blocking the gameplay camera.
- Route position: open route begins in the lower centre and bends toward the wreck using negative space and value changes, not slabs/cards/ribbons.
- Background ridge: distant irregular basalt ridge family with sky separation.
- Value hierarchy: dark charcoal/basalt ground, medium ash route, off-white/graphite/worn-steel machinery, localized cyan accent.
- Human palette: off-white, graphite, worn steel, muted orange, tiny warning red only if functional.
- Alien accent: exactly one localized cyan/turquoise breach or contamination point.
- Atmosphere: hostile volcanic island crash site, polygonal salvage sci-fi, not cartoonish, photorealistic, block-based, or showroom-like.

## Official source and licence manifest

| Creator | Pack | Official source URL | Exact licence | Commercial use | Modification | Archive / source file | SHA-256 | UTC download time | Original internal path | Production derivative path |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| Kenney | Space Kit | https://kenney.nl/assets/space-kit | CC0 1.0 Universal | Permitted | Permitted | `kenney_space-kit.zip` | `d5d7cdf2635ed5a43a9187deaf409b6f47484e402321128341d3c3698e9ef4d9` | 2026-07-01T12:31:28Z | `/tmp/tc_asset_recovery/kenney_space/Models/OBJ format/*.obj` | none; gate blocked production import |
| Kenney | Nature Kit | https://kenney.nl/assets/nature-kit | CC0 1.0 Universal | Permitted | Permitted | `kenney_nature-kit.zip` | `fa7974a0d342bfe63c38664ba9f8ec1a4aab8ea25f099bdc56870e33588c4d9d` | 2026-07-01T12:31:28Z | `/tmp/tc_asset_recovery/kenney_nature/Models/OBJ format/*.obj` | none; gate blocked production import |
| Quaternius | Ultimate Spaceships | https://quaternius.com/packs/ultimatespaceships.html | CC0 | Permitted | Permitted | official Google Drive files linked from creator page | see selected-object hashes below | 2026-07-01T12:31:28Z | `/tmp/tc_asset_recovery/quaternius_ultimate_spaceships/*/OBJ/*.obj` | none; gate blocked production import |

Selected Quaternius source-object hashes:

| Original internal path | SHA-256 |
| --- | --- |
| `Bob/OBJ/Bob.obj` | `381dfe713287a295b487e98400824ea938a0abb2e5a747212deae8e381389d07` |
| `Challenger/OBJ/Challenger.obj` | `b2ebff8cc25cac95cb9443a5f71c7a05f37b832878ceb4cb47b6e0aa80aa52b9` |
| `Dispatcher/OBJ/Dispatcher.obj` | `86d4ea43b670a95a6cae4c1d7face9d4128fbfeb16f57e51ea9eab2bb00010b1` |
| `Executioner/OBJ/Executioner.obj` | `74ff008ff89f50b83c873d9c0e10fd3ff8e7b64091c3abb7208abb25b1f573a6` |
| `Imperial/OBJ/Imperial.obj` | `711dc83eca1c3d9a490f829dede9ac9029af7e34ccdcd457c848b37f7bdf81f4` |

## Ignored review artifacts

Generated PNGs are intentionally under `artifacts/visual-review/` and were not committed:

- `artifacts/visual-review/stage-a-reference-recovery/hull_components_contact_sheet.png`
- `artifacts/visual-review/stage-a-reference-recovery/damaged_structures_contact_sheet.png`
- `artifacts/visual-review/stage-a-reference-recovery/engines_mechanics_contact_sheet.png`
- `artifacts/visual-review/stage-a-reference-recovery/rocks_cliffs_contact_sheet.png`
- `artifacts/visual-review/stage-a-reference-recovery/background_ridges_contact_sheet.png`
- `artifacts/visual-review/stage-a-reference-recovery/quaternius_spaceships_contact_sheet.png`
- `artifacts/visual-review/stage-a-reference-recovery/audition_dimensions.json`
- `artifacts/visual-review/stage-a-reference-recovery/quaternius_spaceships_dimensions.json`

## Visual audition classifications

Classifications are based on rendered contact sheets containing three-quarter, side, elevated, material-proxy/neutral views, one-metre scale markers, and AABB dimensions. They are not filename-based.

### Hull / ship candidates

| Mesh | AABB evidence | Visible read | Classification | Decision |
| --- | --- | --- | --- | --- |
| Kenney `craft_cargoA.obj`, `craft_cargoB.obj`, `craft_miner.obj`, `craft_racer.obj`, `craft_speederA.obj` | small craft proportions in contact sheet | compact, cute/elegant low-poly spacecraft; unsuitable as the dominant crash-site hero | `TOY_LIKE` | rejected for hero use |
| Kenney `hangar_largeA.obj`, `hangar_largeB.obj` | broad but architectural | reads as a hangar/building module, not a crashed exploration vessel hull | `STYLE_MISMATCH` | rejected for hero use |
| Kenney `structure_detailed.obj` | cube-like frame | useful only as possible support frame; not a vessel hull | `SUPPORT_ONLY` | not sufficient for hero hull |
| Quaternius `Bob.obj` | `10.50 x 2.04 x 5.61m` | small fighter with wing/fuselage toy silhouette | `TOY_LIKE` | rejected for hero use |
| Quaternius `Challenger.obj` | `10.50 x 3.02 x 10.18m` | fighter-like, elegant and intact-looking | `TOY_LIKE` | rejected for hero use |
| Quaternius `Dispatcher.obj` | `4.90 x 3.09 x 10.26m` | narrow pointed ship; intact elegant silhouette persists | `TOY_LIKE` | rejected for hero use |
| Quaternius `Executioner.obj` | `10.02 x 1.85 x 9.20m` | low-profile fighter with side engine pods; more mechanical but still intact toy/fighter read | `TOY_LIKE` | rejected for hero use |
| Quaternius `Imperial.obj` | `7.73 x 3.30 x 18.55m` | the heaviest available spaceship form, but still reads as an intact stylized vessel rather than broken industrial machinery | `HULL_COMPONENT` | support only; not a proven `HERO_HULL` |

### Damaged structural and mechanical candidates

| Mesh family | Visible read | Classification | Decision |
| --- | --- | --- | --- |
| Kenney corridor/support/ring/pipe pieces | compatible low-poly sci-fi support elements; some rectangular and showroom-like if dominant | `DAMAGED_STRUCTURE` or `SUPPORT_ONLY` depending on piece | could support a valid hull but cannot replace one |
| Kenney generators, rocket bases, barrels, pipe modules | credible small mechanical props; scale and silhouette are too minor for a dominant exposed engine | `ENGINE_MECHANICAL` for selected generators/rocket bases; `SUPPORT_ONLY` for barrels/pipes | one exposed mechanical component is plausible, but not enough to overcome hull/ridge failures |

### Terrain / ridge candidates

| Mesh family | Visible read | Classification | Decision |
| --- | --- | --- | --- |
| Kenney rocks, stones, meteors, craters | authored low-poly rocks with useful foreground/midground shapes; craters are low flat rings | `FOREGROUND_TERRAIN` / `MIDGROUND_TERRAIN` | four compatible foreground/midground pieces are plausible |
| Kenney cliff pieces | blocky vertical cliff segments with rectangular panel reads in side/front views | `STYLE_MISMATCH` for required ridge role | rejected as coherent background ridge family |

## Feasibility gate answers

1. Does the selected hull look heavy and industrial? **No.** The best available candidate, Quaternius `Imperial.obj`, is heavier than the other ships but still reads as an intact stylized spacecraft, while the remaining ship candidates are toy-like fighters or small craft.
2. Can the hull be broken without retaining an intact toy silhouette? **No.** The available hull forms keep strong intact fighter/ship profiles even in side and elevated views, so breaking them would risk decorating an unsuitable asset.
3. Are at least two compatible damaged structures available? **Yes.** Kenney corridor/support/ring/pipe pieces can support a damaged sci-fi wreck.
4. Is there a convincing exposed engine or machine component? **Partial.** Kenney generator/rocket-base pieces provide mechanical detail, but they are small support parts rather than a strong exposed industrial engine mass.
5. Are four compatible foreground/midground terrain pieces available? **Yes.** Kenney rocks/stones/meteors can support foreground and midground volcanic framing with material overrides.
6. Is there a coherent background ridge family? **No.** The available cliff pieces read as blocky/rectangular panels rather than irregular basalt ridges.
7. Can all selected assets share one TitanCraft material language? **Partial.** Materials could be overridden into off-white/graphite/worn-steel and basalt greys, but material unification cannot solve the hero-hull and ridge-shape failures.
8. Can the scene be built without visible route slabs or terrain boards? **Yes in principle,** but not with the required hero wreck and coherent ridge family from this free asset set.

## Decision

The authenticated free asset subset does not satisfy the required Stage A visual roles. The blocker is not reference availability and not download/authentication; the blocker is the visible geometry of the available free assets:

- No rendered candidate qualifies as a `HERO_HULL` that reads as heavy industrial machinery.
- The best hull candidate is only a `HULL_COMPONENT`, and the rest are `TOY_LIKE` for the required hero role.
- No coherent irregular basalt `BACKGROUND_RIDGE` family was proven; the available ridge/cliff candidates resemble rectangular panels.
- Mechanical and structural support pieces exist, but they cannot substitute for a dominant hero hull or a background ridge family.

Therefore `scenes/Main/Main.tscn` remains unchanged and the final verdict for this pass is:

`BLOCKED_BY_FREE_ASSET_SET`

## Code simplicity report

- New runtime scripts: 0.
- Lines of runtime visual code: 0.
- New production materials: 0.
- Production nodes added: 0.
- Imported production assets used: 0.
- Imported assets rejected before production: Kenney Space Kit ship/support subset, Kenney Nature Kit rock/cliff subset, and Quaternius Ultimate Spaceships subset listed above.
- Simpler scene-only solution exists: no, because the gate failed before any scene-only production arrangement could legally begin.

## Validation summary

- `dotnet restore`: passed.
- `dotnet build`: passed.
- `godot --headless --path . --import`: passed.
- `dotnet test tests/TitanCraft.Tests.csproj`: passed, 44 tests.
- `godot --headless --path . tests/Integration/IntegrationTestRunner.tscn`: passed with `TITANCRAFT_INTEGRATION_TESTS_PASS`.
- `python3 -m json.tool artifacts/runtime-contract-report.json`: passed as runtime-diagnostic JSON validation.
- `xvfb-run -a godot --path . --rendering-driver opengl3 --script tools/visual_review/capture_phase3a.gd`: not available in this container because `xvfb-run` is missing.
- `godot --headless --path . --rendering-driver opengl3 --script tools/visual_review/capture_phase3a.gd`: failed with an empty viewport image under the headless dummy renderer; no Stage A production capture was used as evidence because the asset gate blocked production scene edits.
- `./tools/test.sh`: passed, including restore/build/unit/integration/import/export workflow.
- `git diff --check`: passed.
- `godot --headless --path . --export-release "Windows Desktop" builds/Windows/TitanCraft.exe`: passed; Windows runtime execution was not performed inside the Linux container.
