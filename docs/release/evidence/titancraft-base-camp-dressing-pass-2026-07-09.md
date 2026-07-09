# Visual Evidence — Base Camp Dressing Kit V1 (Blender) — 2026-07-09

| Field | Value |
|---|---|
| Task | Install Blender in-container and push visual design/assets/scene further toward the approved reference mood |
| Routed packet | visual_scene_composition / visual evidence; primary agent art_director |
| Branch | `claude/agent-studio-mvp-closure-9s383i` |
| Toolchain | Blender 4.0.2 (installed via apt this session, with python3-numpy + libegl1 for glTF export and EEVEE review renders), Godot headless + xvfb captures |

## Gap filled (documented, not invented)

- `docs/art/crash-site-object-asset-inventory.md` "Foreground occluders" requires a **cable occluder**
  variant ("cropped rocks, wreckage ribs, cable loops") that had never been built (rock and hull-rib
  variants exist).
- The same inventory's base-pad shape language calls for "Rectangular pads, crates, cables, tool
  frames"; the approved reference-mood direction shows the camp as a lived-in hub (orange awning,
  supply crates, pole work lights). Before this pass the production scene had **four bare
  OmniLight3D sources with no visible fixture** and no camp canopy/crate dressing at all.

## Assets authored (Blender Asset Forge pipeline, all `BLENDER_ASSET_VALID`)

`tools/blender/create_base_camp_dressing_kit_v1.py` →
`assets/Source/Blender/Production/TC_ENV_{CampAwning,SupplyCrateStack,LightPole,CableOccluder}_V1.blend`
→ `assets/Production/Generated/BaseCampDressing/*.glb` → embedded
`assets/models/mvp_pack_v1/*.gltf`. Palette matched byte-for-byte to existing repo materials
(HumanGraphite / HumanWornSteel / HumanOrangeInteractive tones). Manifest regenerated
(`asset_manifest.json`, 25 → 29 entries); CI workflow extended with create/validate/export/render
steps and bundle paths for the new kit.

**Review-render defect caught before integration:** the first render pass showed floating crate lids
and a canopy not reaching its poles — a real authoring bug (box parts built at half their intended
size from scaling a unit cube by size/2). Fixed in the kit script, regenerated, re-validated, and
re-reviewed before any scene integration. Review PNGs (silhouette/material/scale views per asset)
under `artifacts/asset-review/TC_ENV_*_V1/` were opened and checked.

## Production integration (visual-only, collisionless)

New `BaseCampDressing` Node3D in `scenes/Main/Main.tscn`: awning + crate stack at the workbench hub,
second crate stack at the crash hull, **light-pole fixtures at the four existing BaseLamp positions**
(the light pools now have visible sources; no lights added or removed), and two cable runs. No
gameplay, collision, NodePath, or light-count changes.

## Opened-image diagnosis (production captures, same 8 cameras)

- `production_04_resource_workbench_zone.png`: the workbench hub now reads as a lived-in camp —
  orange awning over the bench, crate stacks with hazard stripes, lamp fixture, cables — against the
  moon/spire skyline; focal point and route readability preserved.
- `production_01_spawn_overview.png`: pole fixtures punctuate the route and visually explain the
  warm pools; silhouettes read at distance.
- `production_05_savepoint_beacon_zone.png` / `production_08_wide_terrain_composition.png`: no
  regression to beacon-zone readability or the wide composition. Disclosed nit: at close range the
  lamp housing visually floats a few centimeters off its support arm — cosmetic, queued for a V2
  tweak, not hidden.

## Validation (after integration)

- `dotnet build` 0 warnings / 0 errors; `dotnet test` 71/71
- `godot --headless --path . --import` exit 0; `IntegrationTestRunner.tscn` →
  `TITANCRAFT_INTEGRATION_TESTS_PASS`
- `python3 tools/validate_agent_studio.py` passed; `build_asset_manifest.py --check` clean (29
  entries); `git diff --check` clean

## Verdict

`PASS` for the Base Camp Dressing Kit V1 slice (authored, validated, reviewed, integrated with
evidence). Human/Art Director aesthetic sign-off remains open, as does the axis-6 quality gap to its
9.0 peer target — this record is agent evidence, not a human approval.
