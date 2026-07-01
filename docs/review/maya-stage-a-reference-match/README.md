# MAYA Stage A Reference Match Attempt — Asset Feasibility Gate

Date: 2026-07-01

Final verdict: `BLOCKED_BY_ASSET_LIMITATION`

## Scope

This pass stopped before editing `scenes/Main/Main.tscn`. The rejected Stage A visible composition was not incrementally modified. The only generated visual evidence is an ignored asset-audition artifact under `artifacts/visual-review/maya-stage-a-reference-match-local/`.

The requested prior commit `964380b` was not present in this local history. The closest visible Stage A isolation commit in this checkout is `4ace8c1 [kpi:phase3a] isolate stage A visual pass (+0%)`.

## Asset-audition artifact

Ignored local files generated for human review:

- `artifacts/visual-review/maya-stage-a-reference-match-local/asset_audition_contact_sheet.png`
- `artifacts/visual-review/maya-stage-a-reference-match-local/asset_audition_dimensions.json`

The contact sheet was opened and visually inspected locally. It includes filename labels, three-quarter and side wire projections, one-metre scale markers, AABB dimensions, and original/neutral viewing channels. The currently available authenticated meshes were judged by visible geometry, not filenames.

## Candidate classifications

| Mesh | AABB | Classification | Visual reason |
|---|---:|---|---|
| `assets/Production/Environment/CrashSite/ship_hull_body.obj` | 7.34 × 5.70 × 4.96 | TOY_LIKE | Rounded intact cute/elegant ship silhouette; not heavy industrial crash machinery. |
| `assets/Production/Environment/CrashSite/damaged_structural_base.obj` | 8.53 × 4.97 × 8.53 | STYLE_MISMATCH | Dome/base silhouette reads as intact habitat or toy dome, not damaged hull. |
| `assets/ThirdParty/Quaternius/UltimateSpaceKit/Models/Base_Large.obj` | 8.53 × 4.97 × 8.53 | STYLE_MISMATCH | Same dome/base silhouette; unsuitable as hero wreck. |
| `assets/ThirdParty/Quaternius/UltimateSpaceKit/Models/hull_nose_a.obj` | 0.80 × 0.25 × 1.00 | STYLE_MISMATCH | Flat wedge panel; too small/simple to carry separated wreck section. |
| `assets/ThirdParty/Quaternius/UltimateSpaceKit/Models/hull_engine_mount_c.obj` | 1.68 × 0.55 × 1.00 | STYLE_MISMATCH | Rectangular wedge/block; reads as slab-like support, not authored damage. |
| `assets/ThirdParty/Quaternius/UltimateSpaceKit/Models/engine_pod_a.obj` | 1.48 × 0.25 × 1.00 | STYLE_MISMATCH | Thin low-detail wedge; lacks heavy engine/mechanical read. |
| `assets/ThirdParty/Quaternius/UltimateSpaceKit/Models/mechanical_nozzle_b.obj` | 1.52 × 0.40 × 1.44 | STYLE_MISMATCH | Simple wedge/pyramid form; insufficient mechanical detail for exposed engine. |
| `assets/ThirdParty/Quaternius/UltimateSpaceKit/Models/hull_wing_b.obj` | 1.24 × 0.40 × 1.44 | STYLE_MISMATCH | Flat wing wedge; reinforces toy-like intact ship language. |
| `assets/ThirdParty/Quaternius/UltimateSpaceKit/Models/ship_panel_d.obj` | 1.04 × 0.70 × 1.44 | STYLE_MISMATCH | Generic rectangular/boxy panel; risks visible route-slab/panel language. |
| `assets/ThirdParty/Quaternius/UltimateSpaceKit/Models/alien_debris_a.obj` | 0.88 × 0.55 × 1.00 | STYLE_MISMATCH | Box-like debris; not enough structural damage information. |
| `assets/ThirdParty/Quaternius/ModularSciFiMegaKit/Models/support_frame_a.obj` | 1.52 × 0.40 × 1.44 | BACKGROUND_ONLY | May support minor framing, but too simple for hero wreck structure. |
| `assets/ThirdParty/Quaternius/ModularSciFiMegaKit/Models/internal_rib_a.obj` | 1.76 × 0.25 × 1.00 | BACKGROUND_ONLY | Could be small rib detail, but too thin/simple for meaningful Stage A component. |
| `assets/ThirdParty/Quaternius/StylizedNatureMegaKit/Models/rock_irregular_a.obj` | 0.80 × 0.25 × 1.00 | BACKGROUND_ONLY | Small wedge-like rock; usable only as minor filler. |
| `assets/ThirdParty/Quaternius/StylizedNatureMegaKit/Models/rock_irregular_b.obj` | 1.24 × 0.40 × 1.44 | BACKGROUND_ONLY | Simple low-poly rock; can frame but not define credible volcanic terrain. |
| `assets/ThirdParty/Quaternius/StylizedNatureMegaKit/Models/rock_cliff_d.obj` | 1.04 × 0.70 × 1.44 | BACKGROUND_ONLY | Basic cliff block; useful only in a distant family. |
| `assets/ThirdParty/Quaternius/StylizedNatureMegaKit/Models/rock_ridge_f.obj` | 1.52 × 0.40 × 1.44 | BACKGROUND_ONLY | Low-detail ridge wedge; background only. |
| `assets/ThirdParty/Quaternius/StylizedNatureMegaKit/Models/rock_spire_g.obj` | 0.88 × 0.55 × 1.00 | BACKGROUND_ONLY | Small angular spire; not enough for foreground/background volcanic authorship alone. |
| `assets/ThirdParty/Kenney/NatureKit/Models/cliff_large_rock.obj` | 1.00 × 1.00 × 0.42 | BACKGROUND_ONLY | Cliff block has more verticality but remains generic and not authenticated for current production selection. |
| `assets/ThirdParty/Kenney/ModularSpaceKit/Models/cables.obj` | 1.91 × 0.16 × 2.10 | SUITABLE_FOR_SUPPORT | Readable cable bundle; useful support detail, not a hero hull/damage solution. |

## Mandatory feasibility gate

1. Is there a hull candidate that can read as heavy industrial machinery? **No.** The only full hull candidate reads toy-like/intact, and the base/dome alternative reads as an intact habitat dome.
2. Are there at least two compatible damaged or structural components? **No.** Available ship/mechanical components are mostly simple wedges, slabs, or panels; only cables can serve as a support detail.
3. Is there authored volcanic or rocky geometry suitable for foreground and background framing? **No.** There are small low-poly rocks/ridges usable as background filler, but not enough authored volcanic foreground/background terrain to avoid slab/board reads.
4. Can the selected assets share one coherent material language? **Partially, but insufficient.** Materials can be normalized, but material coherence cannot fix the missing hero hull and damaged-structure geometry.
5. Can the reference composition be approximated without visible rectangular slabs or debug-like primitives? **No.** The available geometry would force slab/panel/wedge primitives into the hero wreck and route/terrain language.

## Missing visual roles

- Heavy industrial broken hull mass.
- Two compatible damaged structural components with authored tear/internal detail.
- Exposed engine/mechanical section with real machinery detail.
- Authored volcanic foreground terrain that is not a rectangular plane/slab.
- Coherent background ridge/cliff family with enough silhouette complexity to frame the crash site.
- Accessible approved reference image in the repository/session for direct comparison.

## Decision

`BLOCKED_BY_ASSET_LIMITATION`

No `Main.tscn` edit was made because the mandatory feasibility gate did not pass.
