# Visual Evidence — Reference-Mood Production Pass — 2026-07-09

| Field | Value |
|---|---|
| Task | Bring Main scene lighting/sky/landmarks and main menu styling toward the user-supplied cinematic reference images (mood/palette/composition only) |
| Routed packet | visual_scene_composition / visual evidence; primary agent art_director |
| Branch | `claude/agent-studio-mvp-closure-9s383i` |
| Scope guard | Per the 2026-07-07 user decision recorded in `docs/production/quality-scorecard-log.md` (seventh pass): references are mood/palette/composition guidance only. No large mech, no second enemy type, no scope expansion. Path-traced AAA fidelity is not achievable in this pipeline and is not claimed. |

## What changed (all text-format scene/material/UI edits; no gameplay code, no collision, no new binaries)

1. **Dusk sky grade** — `scenes/Main/Main.tscn` `ProceduralSkyMaterial_sky`: flat navy sky replaced
   with violet-zenith / warm-orange-horizon dusk gradient (`sky_curve = 0.12`).
2. **Atmosphere** — `Environment_main`: fog tinted violet-dusk (`0.17, 0.11, 0.18`, density 0.010),
   ambient shifted violet (`0.24, 0.19, 0.36`, energy 0.7), glow raised (intensity 0.32 / strength 0.6).
   Exposure deliberately untouched (0.85) to avoid regressing the 2026-07-06 blown-highlight fix.
3. **Warm key light** — `DirectionalLight3D` given a low-sun color (`1, 0.76, 0.56`); energy unchanged.
4. **Moon landmark** — the pre-existing but hidden `Moon` node enabled, scaled ×1.9, moved to
   (-46, 34, -92), given new `assets/Materials/Landmarks/MoonGlow.tres` (unshaded warm ivory,
   emission 1.0) so it reads as a glowing moon at dusk, matching the references' dominant sky element.
5. **Signal spire landmark** — new `SignalSpire_Landmark` dressing node at (58, 0, -64): graphite
   tower + steel fins + 52 m emissive orange beam (`assets/Materials/Landmarks/SignalSpireBeam.tres`),
   `cast_shadow` off, **no collision** — matches the references' distant orange beacon-tower beam and
   gives the skyline its vertical orange accent. Decorative only; the gameplay Beacon is untouched.
6. **Warm practical pools** — the four existing `BaseLamp_1..4` omnilights raised 0.55 → 1.15 energy
   (range 9) so the base/workbench area reads as the references' warm-lit hub; `AlienZoneLight`
   raised to 0.95 for the violet counter-pool. No new lights added (perf budget respected: same count).
7. **Main menu restyle** — `scenes/UI/MainMenu.tscn`: dark full-bleed background, "TITANCRAFT" title,
   tricolor tagline "Build. Explore. Endure." (orange/cyan/violet, matching the reference menu),
   orange-bordered dark button styles with hover/pressed/disabled states, version label. All original
   node paths (`Menu/NewGameButton`, `Menu/ContinueButton`, …) preserved — verified against
   `src/UI/MainMenu.cs` and `tests/Integration/IntegrationTestRunner.cs` contracts before editing.
8. **New capture tool** — `tools/visual_review/capture_main_menu.gd` (modeled on the existing
   phase3a capture script) so menu styling has repeatable PNG evidence.

## Evidence (PNGs regenerable; binaries untracked per repo policy)

Regenerate with:

```
python3 tools/prepare_audio_assets.py
xvfb-run -a godot --path . --script tools/visual_review/capture_phase3a_production_integration.gd
xvfb-run -a godot --path . --script tools/visual_review/capture_main_menu.gd
```

Outputs: `artifacts/visual-review/phase3a-production-integration/production_01..08.png`,
`artifacts/visual-review/main-menu/main_menu.png`.

## Opened-image diagnosis (before → after)

**Before** (captured this session, pre-change, same 8 camera views): flat navy void sky with the
terrain edge silhouetted against nothing; no moon; no vertical landmark; base lamps too weak to read
as warm pools; scene cold, monochrome slate/brown; menu was unstyled default-grey Godot controls.

**After, `production_01_spawn_overview.png`:** violet dusk sky with warm horizon; glowing ivory moon
reads as the sky's focal element; crash-site hub sits in a warm pool against violet shadow; route
readability preserved (hull → workbench sight-line unchanged). Known limitation: at this camera's
72° FOV and the moon's frame position, the sphere renders elliptically — acceptable at gameplay FOV,
disclosed rather than cropped.

**After, `production_08_wide_terrain_composition.png`:** the orange spire beam is the dominant
vertical accent on the skyline exactly as in the references; fog grounds the terrain edge (the
before-shot's floating-island-in-void failure is gone); moon anchors the upper-left sky; violet
crystals and cyan breach accents provide the cool counterpoint.

**After, `main_menu.png`:** title/tagline/button hierarchy matches the reference menu's structure;
Continue correctly renders the disabled style when no save exists (state logic untouched).

**Honest gap statement:** the references are path-traced cinematic renders with sculpted characters,
a large mech, and dense micro-detail. Those are out of scope (mech/enemy forbidden by README; render
fidelity out of pipeline reach). This pass closes the *mood/palette/composition* gap only: dusk grade,
moon, vertical orange landmark, warm-vs-violet lighting contrast, menu identity.

## Validation (this session, after all changes)

- `dotnet build TitanCraft.sln` — 0 warnings, 0 errors
- `dotnet test` — 71/71 passed
- `godot --headless --path . --import` — exit 0, no errors
- `IntegrationTestRunner.tscn` — `TITANCRAFT_INTEGRATION_TESTS_PASS` (all MVP smoke milestones; menu
  and pause node contracts included)
- `python3 tools/validate_agent_studio.py` — passed; `git diff --check` — clean

## Verdict

`PASS` for the scoped reference-mood visual pass (repository-owned evidence, opened-PNG diagnosis
above). Human/Art Director aesthetic sign-off remains open — this record is agent evidence, not a
human approval, and does not close the Stage A overall-approval blocker in
`docs/production/known-blockers.md`.
