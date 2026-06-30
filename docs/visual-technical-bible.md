# TitanCraft Visual Technical Bible — Crash Site Vertical Slice

Date: 2026-06-30
Phase: Phase 1 — Visual Technical Bible
Scope: documentation-only visual production rules for the MVP Crash Site vertical slice.

This bible converts `docs/visual-style.md` and the Phase 0 baseline into measurable rules for later Godot-only asset production. It does not create materials, scenes, props, shaders, VFX, audio, animations, UI assets, or gameplay changes.

## 1. Visual identity statement

TitanCraft is a serious first-person sci-fi survival game set in a Comorian volcanic crash site where modular industrial human salvage technology, shaped by African-futurist repair logic, contrasts with invasive biomimetic Galaxabrain machinery through readable polygonal low-to-medium-poly forms.

TitanCraft must not visually become:

- voxel or block-based;
- cartoon, toy-like, or childlike;
- a photorealistic military shooter;
- generic sci-fi assembled from unrelated asset packs;
- stereotypical African decoration pasted onto machines;
- fantasy tribal styling;
- excessive neon cyberpunk;
- a visual imitation of one existing commercial game.

Pass/fail criterion: a screenshot from the main route must read as first-person survival sci-fi salvage in a volcanic environment, with human and alien systems distinguishable without relying on a caption.

## 2. Exact palette

Use these values as production targets. RGB values are normalized 0-1 for Godot `Color(r, g, b, 1)` use. Existing Phase 0 materials are not modified in Phase 1.

### Human palette

| Name | HEX | RGB 0-1 | Intended material role | Allowed usage | Prohibited usage |
|---|---:|---|---|---|---|
| Off-White Hull | `#D4C8AE` | `0.831, 0.784, 0.682` | readable human hull panels and C7 base surfaces | ship hull, wall panels, workbench top, large readable forms | alien shells, organic matter, damage soot, all small detail pieces |
| Graphite | `#252A30` | `0.145, 0.165, 0.188` | dark industrial casing | crates, machine housings, joints, weapon/arm base parts | primary pickup signal color by itself, alien energy, UI warnings |
| Worn Steel | `#6E7478` | `0.431, 0.455, 0.471` | scraped steel edges and exposed structural metal | rails, bolts, hinges, repaired edges, pipe collars | broad terrain, glowing tech, decorative pattern fill |
| Dark Warm Bronze | `#8A633D` | `0.541, 0.388, 0.239` | warm mechanical reinforcement | brackets, bearings, piston housings, reused repair parts | full hull paint, alien carapace, danger signals |
| Muted Interaction Orange | `#E87822` | `0.910, 0.471, 0.133` | human interaction and survival function | handles, workbench panel, interactable trim, emergency equipment, construction zones | random decoration, large terrain patches, alien energy, non-interactive clutter |
| Emergency Red | `#C3362C` | `0.765, 0.212, 0.173` | urgent human warning | low-health UI accents, small emergency LEDs, hazard slashes | normal interaction prompts, alien cores, large surface fills |
| Screen Cyan | `#34C8E8` | `0.204, 0.784, 0.910` | human powered screens and readable electronics | small screen glow, electronics pickup details, arm status UI | alien-only areas where it could confuse energy ownership unless explicitly mixed |
| Soot Burn Black | `#08090A` | `0.031, 0.035, 0.039` | burn scars and crash damage | scorch marks, panel burns, impact edges, exhaust interiors | base albedo for all props, UI text, readable silhouettes against dark terrain |

### Alien palette

| Name | HEX | RGB 0-1 | Intended material role | Allowed usage | Prohibited usage |
|---|---:|---|---|---|---|
| Blue-Black | `#0A0B12` | `0.039, 0.043, 0.071` | Galaxabrain shell and alien mass | scout carapace, invasive plates, alien growth bases | human machines, UI panels, ordinary rocks |
| Deep Violet | `#7A3FF2` | `0.478, 0.247, 0.949` | alien neural core energy | cores, crystal lines, active alien state, beacon only when story-justified | human workbench, normal pickups, all background props |
| Cold Metallic Grey | `#8A96A3` | `0.541, 0.588, 0.639` | alien metal edges | alien plate cuts, exposed cold machinery, broken fragments | warm human repair metal, terrain soil |
| Cyan Emission | `#34C8E8` | `0.204, 0.784, 0.910` | alien powered channels and Galaxabrain state | thin energy lines, scout alert accents, powered alien tech | every alien surface, human emergency signals |
| Turquoise Emission | `#2FE6C8` | `0.184, 0.902, 0.784` | intense alien activation | beacon activation particles, rare core pulses, mission-critical powered state | idle decorative glow, all crystals at all times |
| Controlled Magenta Accent | `#C247B8` | `0.761, 0.278, 0.722` | rare biological/energy stress accent | damage state, unstable core flicker, final death burst | general lighting, UI base color, human props |

### Environment palette

| Name | HEX | RGB 0-1 | Intended material role | Allowed usage | Prohibited usage |
|---|---:|---|---|---|---|
| Basalt Black | `#15171A` | `0.082, 0.090, 0.102` | volcanic rock mass | rocks, ground plates, cliffs, dark horizon forms | readable UI text, full silhouettes of interactables without contrast |
| Volcanic Soil | `#2B211C` | `0.169, 0.129, 0.110` | compact ash/soil | ground variation, paths, crater floor | human hull panels, alien shells |
| Ash Grey | `#6C6E6A` | `0.424, 0.431, 0.416` | ash dust and weathering | dust, worn edges on terrain, low-contrast ground detail | primary interaction color, alien energy |
| Warm Sunlit Brown | `#9A6E43` | `0.604, 0.431, 0.263` | sunlit volcanic/coastal warmth | exposed rock faces, dry erosion edges, distant ridges | UI warning, alien core, large human panels |
| Sparse Vegetation Green | `#3E5F45` | `0.243, 0.373, 0.271` | rare hardy vegetation | small plant clusters, moss-like erosion pockets | dense biome, pickup confusion, main route markers |
| Ocean-Atmosphere Blue | `#18233D` | `0.094, 0.137, 0.239` | sky/horizon/ocean atmosphere if retained | distant horizon, cool ambient shadows, sky gradients | local emissive objects, interaction markers |

### Color hierarchy rules

| Rule | Pass/fail criterion |
|---|---|
| Orange is reserved primarily for interaction, survival equipment, handles, emergency systems, and construction systems. | PASS if at least 80% of saturated orange surfaces on the main route are functional or interactive. |
| Cyan/turquoise primarily signals alien energy, powered technology, Galaxabrain state, or justified beacon activation. | PASS if cyan/turquoise glow has an explicit powered-state reason and is not applied to ordinary clutter. |
| All objects must not glow. | PASS if emissive materials are limited to mission, state, UI, or powered details and never blanket-fill full props. |
| Human and alien palettes must not collapse into one purple/orange neon scheme. | PASS if human zones read warmer/industrial and alien zones read colder/asymmetric on a desaturated screenshot. |
| Environment colors stay lower saturation than interactables. | PASS if pickups/workbench/beacon remain readable against terrain at gameplay distance. |

## 3. Shape language

### Human shape language

| Rule | Pass/fail criterion |
|---|---|
| Symmetry is medium-to-high: human modules align to bilateral or grid logic unless visibly damaged. | PASS if intact modules have clear centerlines or grid alignment. |
| Dominant primitives are rectangles, cylinders, frames, rails, panels, straight hinges, handles, and bolted plates. | PASS if a human prop can be decomposed into readable industrial primitives. |
| Edges are bevel-implied or layered through extra primitive strips; avoid perfectly toy-like cubes. | PASS if large boxes include edge bands, inset panels, or damage cuts. |
| Panel rhythm uses repeated functional modules with 2:1, 3:1, or 4:1 rectangular ratios. | PASS if repeating panels look replaceable rather than ornamental wallpaper. |
| Repair logic is visible through mismatched plates, exposed fasteners, patch seams, and cable reroutes. | PASS if damage/repair marks explain how the object still functions. |
| Mechanical joints use cylinders, collars, hinge blocks, pistons, and socket plates. | PASS if moving/mechanical elements have plausible pivot hardware. |
| Cables are purposeful and limited: power, data, or emergency bypass. | PASS if each visible cable has two plausible endpoints. |
| Silhouette density is low at large scale and medium near interactables. | PASS if a prop reads in silhouette at 10 m and rewards detail inspection at 2-3 m. |
| Damage language is directional: impact dents, missing panels, soot, exposed ribs, and bent frames. | PASS if crash damage implies force direction and does not look randomly noisy. |

### Alien shape language

| Rule | Pass/fail criterion |
|---|---|
| Asymmetry is medium-to-high; mirrored forms are rare and broken by growth or plate offsets. | PASS if the scout/alien cluster is not mistaken for a human machine in silhouette. |
| Dominant forms are arcs, spikes, interlocking plates, ribs, twisted segments, floating shards, and non-human openings. | PASS if at least three alien form families appear on major alien objects. |
| Segment logic resembles invasive adaptive growth, not animal anatomy. | PASS if plates seem grown/fused into terrain or machinery rather than costume armor. |
| Floating elements are rare, close to cores, and imply energy suspension. | PASS if floating shards are within a powered field and do not become clutter. |
| Openings are irregular slits, vents, or cavities with cold inner glow. | PASS if openings do not look like human doors/windows. |
| Energy cores are focal points, not all-over glow. | PASS if a core or line system guides state reading while most shell area stays dark. |
| Silhouette rhythm alternates dense clusters and negative space. | PASS if the enemy remains readable as a threat at 15 m. |
| Damage language is flicker, cracked shell, dimmed core, shed plates, and unstable magenta/cyan stress. | PASS if damage feedback is visible without adding new gameplay states. |

### Environment shape language

| Rule | Pass/fail criterion |
|---|---|
| Basalt formations use angular columns, broken slabs, stepped ridges, and eroded polygonal chunks. | PASS if terrain reads volcanic rather than generic brown hills. |
| Crater forms use compressed rings, scorched centers, radial debris, and tilted slabs. | PASS if the crash direction is visually understandable from nearby landmarks. |
| Coastal/volcanic shapes can appear through distant blue atmosphere, black rock, humid haze, and eroded edges. | PASS if the coastline reference is atmospheric and not a second biome. |
| Vegetation is sparse and hardy, never dense jungle/savanna. | PASS if plants remain accent clusters and do not obscure route or pickups. |
| Horizon composition uses 2-3 large silhouettes, not a cluttered skyline. | PASS if the beacon/crash landmarks remain dominant from spawn. |

### UI shape language

| Rule | Pass/fail criterion |
|---|---|
| Panels are industrial rectangles/trapezoids with clipped corners and clear hierarchy. | PASS if HUD can be parsed in 1 second at 1080p. |
| Corner treatment is small chamfer, bracket, or inset; no ornate frames. | PASS if decoration does not reduce text area. |
| Line weight uses 1-2 px fine lines and 3-4 px emphasis at 1080p. | PASS if lines remain legible but do not dominate labels. |
| Spacing follows 4 px base rhythm, preferably 8/12/16/24 px increments. | PASS if labels and counters do not crowd at 720p. |
| Icons are filled or outline geometric pictograms, not detailed illustrations. | PASS if each icon reads at 24 px. |
| Decorative patterns are functional separators, vents, scanlines, or status codes only. | PASS if no UI motif appears without a readable function. |

## 4. Comorian and African-futurist design logic

Regional identity must be expressed through systems, materials, climate response, repair practice, and spatial rhythm rather than pasted ornament.

| Influence area | Production rule | Pass/fail criterion |
|---|---|---|
| Material reuse | Human technology reuses panels, rails, cargo pieces, and local basalt-like ballast in visible repairs. | PASS if repaired modules show reused parts with plausible function. |
| Modular repair | Human structures show replaceable panels, bolt patterns, removable handles, and emergency bypasses. | PASS if a damaged module looks serviceable by a survivor. |
| Climatic adaptation | Surfaces include vents, raised panels, shade-like overhangs, and heat-aware gaps. | PASS if large human modules look adapted to heat/humidity rather than sealed generic sci-fi. |
| Volcanic geology | Terrain emphasizes black basalt, ash, impact scorch, and angular erosion. | PASS if environment still reads volcanic in greyscale. |
| Coastal atmosphere | Horizon may use cool blue haze and distant moisture without creating a separate beach biome. | PASS if coastal influence supports mood, not scope expansion. |
| Geometric rhythm | Repetition uses functional panel cadence inspired by East African/Indian Ocean geometric rhythm, not copied symbols. | PASS if patterns double as vents, seams, labels, or structural reinforcement. |
| Signaling | Orange/cyan/red signals map to function and state, not decoration. | PASS if color-coded elements answer “what can I use / what is powered / what is dangerous.” |
| Construction logic | Crash-site architecture looks assembled from survival needs: shade, repair, power, storage, communication. | PASS if each major human object has a survival purpose. |
| Survival infrastructure | Workbench, save point, beacon, and ship debris form a believable emergency workflow. | PASS if the player can visually infer collect → craft → fight → activate. |
| Interface design | UI uses restrained geometric panels and status lines echoing industrial signage. | PASS if UI feels local to the world without cultural symbols pasted on top. |

Explicit prohibitions:

- decorative masks;
- random tribal motifs;
- symbols without functional meaning;
- generic pan-African visual mixing;
- excessive pattern application;
- cultural elements used only as surface decoration.

## 5. Scale and grid standards

Use current gameplay values as constraints; do not change them for visual polish.

| Standard | Value | Constraint source / usage |
|---|---:|---|
| Godot unit convention | `1.0` unit = `1.0` meter | All visual modules use meter scale. |
| Player reference capsule | radius `0.35`, height `1.8` | Existing `Player.tscn`; preserve collision. |
| Player camera/FPS comfort | FOV `75`, head at approximately `0.65` above player root | Existing `Player.tscn`; arm visuals must avoid clipping. |
| Door clear width | `1.4-1.8 m` | Must exceed capsule diameter and allow comfort. |
| Door clear height | `2.2-2.6 m` | Must exceed player capsule height plus clearance. |
| Corridor clear width | minimum `2.0 m`, preferred `2.5-3.0 m` | Supports FPS strafing and enemy readability. |
| Minimum walkable clearance | width `1.2 m`, height `2.1 m` | Never below this on required route. |
| Workbench dimensions | `3.0-3.5 m` wide, `1.2-1.8 m` deep, `0.8-1.4 m` tall | Based on current `3.2 x 0.8 x 1.5` visual footprint. |
| Beacon scale | base diameter `1.0-1.4 m`, height `2.4-4.0 m` | Must be visible as final objective. |
| Pickup size | `0.6-1.2 m` primary silhouette | Must read at 5-10 m but interact at 3 m. |
| Enemy scale | capsule radius `0.45`, height `1.4`; visual max approximately `1.8 m` tall and `1.4 m` wide | Preserve current enemy collision. |
| First-person arm bounds | keep within lower/right 35% of screen at rest | Avoid covering objective or interaction prompt. |
| Crashed-ship module scale | small `2-4 m`, medium `4-8 m`, large `8-16 m` segments | Big enough for landmark, not full rocket scope. |
| Interaction range | `3.0 m` | Existing `FirstPersonController.InteractionRange`; do not change. |
| Interaction visibility distance | pickups `5-10 m`, workbench/beacon `15-30 m`, enemy threat `15 m` | Visual target, not gameplay value change. |
| Modular grid | `0.25 m` detail, `0.5 m` prop, `1.0 m` structural | Supports reuse and snapping. |
| Pivot convention | floor props pivot at bottom center; wall panels pivot at bottom center on grid face; pickups pivot center-bottom | Reduces placement surprises. |
| Orientation convention | forward is Godot `-Z`; up is `+Y`; right is `+X` | Match camera and raycast conventions. |
| Origin convention | gameplay root origin remains stable; visual offsets happen inside `VisualRoot` | Protect save positions, tests, and interaction raycasts. |

## 6. Scene architecture standard

Preferred structure for future visualized gameplay objects:

```text
GameplayObject
├── VisualRoot
│   ├── PrimaryForms
│   ├── SecondaryForms
│   ├── FunctionalDetails
│   ├── EmissiveElements
│   └── DamageElements
├── CollisionRoot
├── InteractionArea
├── VFXRoot
├── AudioRoot
└── ExistingGameplayScript
```

| Rule | Pass/fail criterion |
|---|---|
| Gameplay logic may live only on the gameplay root or dedicated controller nodes documented in the scene. | PASS if deleting `VisualRoot` cannot remove core mission/craft/save/combat behavior. |
| `VisualRoot` and its children are visual-only: meshes, decals, static labels, state meshes. | PASS if no critical gameplay state is stored only in a mesh/material. |
| Collisions remain independent from decorative geometry. | PASS if required route collisions are simple `BoxShape3D`/`CapsuleShape3D` or documented simple shapes. |
| Interactable raycasts remain stable by preserving existing `Area3D` roots or explicit interaction proxies. | PASS if `E` prompt and interaction still work at `3.0 m`. |
| Visual states switch through visibility, animation, or material parameters triggered by gameplay events. | PASS if visual state follows mission/combat state but does not decide it. |
| Reusable scenes are instanced under `VisualRoot`, not copied as disconnected node piles. | PASS if repeated assets have one source scene unless intentionally unique. |
| Shared materials are referenced via `res://assets/Materials/...`; local materials need review notes. | PASS if material duplication is justified or absent. |
| Existing gameplay-critical node names remain stable until a dedicated migration updates tests. | PASS if Phase 0 node paths still resolve. |

## 7. Geometry budgets

Triangle counts are practical Godot low-to-medium-poly targets. Hard maximums are per visible asset instance unless documented otherwise.

| Asset type | Preferred triangles | Hard max | Screen importance | Additional geometry justified when |
|---|---:|---:|---|---|
| Small pickup | 80-250 | 400 | high at close range | silhouette improves identification under 2 seconds. |
| Medium prop | 150-600 | 1,000 | medium | it sits near route or interaction. |
| Large prop | 500-1,500 | 2,500 | medium/high | it is a landmark or blocks skyline. |
| Workbench | 800-1,800 | 3,000 | high | details communicate crafting function. |
| Beacon | 800-2,000 | 3,500 | very high | silhouette/active state improves mission completion clarity. |
| First-person mechanical arm | 1,500-3,500 | 5,000 | very high | detail is visible in FPS view and does not obscure screen. |
| Galaxabrain Scout | 1,500-3,500 | 5,000 | very high | silhouette/state readability improves combat. |
| Crashed-ship segment | 1,000-3,000 | 5,000 | high landmark | segment is visible from spawn or route. |
| Distant rock | 40-150 | 250 | low | silhouette affects horizon composition. |
| Foreground rock | 150-600 | 1,000 | medium | rock is near route/combat. |
| Vegetation cluster | 100-500 | 800 | low/medium | sparse accent helps scale or path readability. |

Additional geometry rules:

- Maximum unique materials per small pickup: `2`; medium prop: `3`; large prop/ship segment: `4`; hero arm/enemy/beacon: `5`.
- Transparent surfaces: avoid by default; maximum one transparent material on a hero asset and none on ordinary clutter.
- Emissive surfaces: use as accents only; target under 10% of visible surface area, hard max 20% for powered hero states.
- Collision complexity: use primitive shapes; no dynamic trimesh collisions for player-route objects; decorative detail should usually have no collision.
- CSG is acceptable for editor prototyping or simple static modules, but runtime gameplay should use stable meshes/primitives and simple collisions.

## 8. Material budgets

### Master-material categories

- `Human/HullOffWhite`
- `Human/Graphite`
- `Human/WornSteel`
- `Human/BronzeWarmMetal`
- `Human/OrangeInteractive`
- `Human/EmergencyRed`
- `Human/ScreenCyan`
- `Damage/SootBurn`
- `Environment/BasaltRock`
- `Environment/VolcanicSoil`
- `Environment/AshDust`
- `Alien/BlueBlackShell`
- `Alien/ColdMetal`
- `Alien/VioletEmissive`
- `Alien/CyanEmissive`
- `Alien/TurquoiseEmissive`
- `Alien/MagentaStress`
- `Organic/BiomassDarkRed`

### Existing material plan

| Existing material | Phase 1 role | Future action, not Phase 1 |
|---|---|---|
| `HumanIvory` | maps to Off-White Hull | keep as shared human hull/panel base. |
| `HumanGraphite` | maps to Graphite | keep for crates, casings, dark metal. |
| `HumanBronze` | maps to Dark Warm Bronze | keep for warm reinforcement and mechanical parts. |
| `HumanOrangeInteractive` | maps to Muted Interaction Orange | keep for interactable/function accents. |
| `VolcanicRock` | maps to Basalt Black | keep for baseline rocks/ground until terrain variants exist. |
| `AlienBlack` | maps to Blue-Black | keep for alien shells. |
| `AlienVioletEmissive` | maps to Deep Violet | keep for alien/beacon energy accents. |
| `BiomassRed` | maps to organic dark red | keep for biomass. |

### Material rules

| Rule | Standard |
|---|---|
| Naming convention | `CategoryPurpose[_Variant].tres`, PascalCase, e.g. `HumanWornSteel_Damaged.tres`. |
| Allowed variants | `_Clean`, `_Worn`, `_Damaged`, `_Emissive`, `_Inactive`, `_Active`; create only when used. |
| Reuse | Prefer shared `.tres` resources in `assets/Materials/`; avoid local material overrides. |
| Local material permission | Only for one-off UI/test experiments or scene-specific generated colors with review note. |
| Shader justification | Only for clear need: pulse emission, scanline screen, controlled dissolve/hit flash, or procedural wear; never for hidden gameplay logic. |
| Transparency prohibited | On large terrain, ordinary props, most pickups, and collision-critical visual surfaces. |
| Emission limits | Human orange multiplier target `0.2-0.6`; alien energy target `0.5-1.5`; avoid area-filling emission. |
| Roughness ranges | hull/panels `0.55-0.85`; worn metal `0.35-0.65`; rock/soil `0.75-0.95`; alien shell `0.45-0.75`. |
| Metallic ranges | human graphite/steel/bronze `0.2-0.75`; hull paint `0.0-0.15`; rock/organic `0.0-0.1`; alien shell/metal `0.2-0.55`. |
| Texture policy | Prefer flat colors/procedural masks; 512 px max for small props, 1024 px for hero props, 2048 px only for large unique hero surfaces with approval. |
| Procedural policy | Noise/gradients may support soot, ash, wear, and emission masks if stored or documented and reproducible. |

## 9. Lighting budget

Phase 0 reference: Forward Plus, one shadowed `DirectionalLight3D`, four orange `OmniLight3D` base lamps, one violet `OmniLight3D` alien light, glow enabled, fog enabled.

| Budget rule | Standard |
|---|---|
| Shadow-casting lights | Maximum `1` primary shadow-casting directional light in Crash Site unless a measured need is approved. |
| Local dynamic lights | Target `0-2` per gameplay zone; hard max `3` active local lights visible in one small zone. |
| Emission versus light | Use emissive material for small LEDs, screens, cable glows, and crystals; use real light only if it changes gameplay readability. |
| Human light color | warm orange/amber around survival systems; avoid pure saturated neon. |
| Alien light color | cold violet/cyan/turquoise near Galaxabrain energy; keep magenta rare for stress/damage. |
| Fog | low density for depth; must not hide pickups, enemy silhouette, or beacon. |
| Glow | restrained readability accent; no bloom blanket across scene. |
| Shadow distance | prioritize player route, workbench, enemy zone, and beacon; distant horizon can be unshadowed. |
| Exposure | preserve readable midtones on basalt terrain and off-white panels. |
| Fallback quality | reduce or disable glow/fog first, then reduce local lights, then disable non-critical shadows; never remove objective readability. |

## 10. VFX budget

No VFX are created in Phase 1. These are future budgets.

| VFX category | Particle count | Lifetime | Visibility range | Loop/burst | Shadow policy | Transparency constraint | Gameplay/ambience justification |
|---|---:|---:|---:|---|---|---|---|
| Smoke | 20-60 | 2-6 s | 20-40 m | looping, low rate | no particle shadows | soft alpha, low overdraw, large slow particles | crash damage and engine residue. |
| Dust/ash | 30-80 | 1-4 s | 10-25 m | looping or localized burst | no shadows | low opacity, terrain-colored | volcanic atmosphere and footsteps/impact ambience if used. |
| Sparks | 8-24 | 0.2-0.8 s | 8-15 m | burst | no shadows | additive or alpha, tiny area | damaged human machinery or hit impact. |
| Alien wisps | 10-30 | 1-3 s | 10-20 m | looping near powered alien objects | no shadows | cyan/violet low opacity | danger/powered alien presence. |
| Hit effects | 6-20 | 0.1-0.5 s | 5-12 m | burst only | no shadows | small, short-lived | confirms real combat hit. |
| Death effects | 20-50 | 0.5-2 s | 10-20 m | burst only | no shadows | avoid full-screen bloom | confirms enemy death and reveals mission component. |
| Beacon activation | 40-100 | 1-5 s | 20-50 m | burst plus controlled loop | no shadows | vertical particles, not screen-filling | signals mission completion state. |
| Environmental particles | 20-60 per local emitter | 2-5 s | 15-30 m | low-rate loop | no shadows | low alpha and capped emitters | wind, ash, light atmosphere. |

## 11. UI visual rules

| UI element | Rule | Pass/fail criterion |
|---|---|---|
| HUD hierarchy | Objective and interaction prompt are highest priority after health. | PASS if objective/prompt readable within 1 second. |
| Objective prominence | Keep concise mission text, framed or highlighted without covering center aim. | PASS if objective remains visible at 720p. |
| Health display | Red/ivory contrast; health changes must be clear but not horror-styled. | PASS if low health is distinguishable without ambiguous orange. |
| Resource display | Three resources remain counters; icons optional later but must be readable at 24 px. | PASS if Metal/Biomass/Electronics counts are not visually confused. |
| Interaction prompt | Orange accent allowed; prompt must be visually linked to interactables. | PASS if `E` prompt stands out from tutorial text. |
| Mechanical-arm status | Cyan/screen color can indicate online powered human tech. | PASS if arm status does not look like alien threat. |
| Danger indicators | Emergency red for player danger only; alien threat can use cyan/violet in-world. | PASS if red is not used for normal interaction. |
| Victory/defeat | Serious mission report tone; no cartoon celebration. | PASS if screen communicates clear outcome and next action. |
| 720p readability | Minimum body text 14 px, preferred 16 px; no critical text below 14 px. | PASS if all HUD text is legible at 1280x720. |
| 1080p readability | Body text 16-20 px, headings 22-32 px when redesigned. | PASS if text is crisp and uncluttered. |
| Contrast target | Aim for at least 4.5:1 for small text, 3:1 for large/status graphics. | PASS if visual review can identify low-contrast violations. |
| Decorative pattern limit | Decorative pattern area under 15% of a UI panel and must act as divider/status texture. | PASS if patterns never compete with text. |
| Animation limits | UI motion under 0.3 s for feedback; no looping distraction near crosshair. | PASS if HUD remains stable during combat. |

## 12. Naming conventions

Do not rename existing gameplay-critical nodes during Phase 1 or visual-only phases unless a dedicated migration updates tests.

| Asset type | Pattern | Example |
|---|---|---|
| Scenes | `PascalCase_Role_Variant.tscn` where helpful | `HumanPanel_Wall_A.tscn` |
| Props | `HumanPropName_Variant` / `AlienPropName_Variant` | `HumanConsole_A`, `AlienRibCluster_B` |
| Environment pieces | `EnvMaterialType_Form_Variant` | `EnvBasaltRock_Foreground_A` |
| Interactables | Keep root gameplay name; visual child `ThingVisual` | `Placeholder_Workbench/VisualRoot/WorkbenchVisual_A` |
| Materials | `CategoryPurpose[_Variant].tres` | `HumanWornSteel_Damaged.tres` |
| Shaders | `ShaderPurpose.shader` | `AlienPulseEmission.shader` |
| VFX | `VFX_Category_Purpose_Quality.tscn` | `VFX_Combat_HitSpark_Low.tscn` |
| Audio | `Audio_Category_CueName_Variant.ext` | `Audio_Combat_ArmHit_A.wav` |
| Visual controllers | `ObjectVisualController.cs` | `GalaxabrainScoutVisualController.cs` |
| Variants | suffix `_A`, `_B`, `_C`; state variants `_Active`, `_Inactive`; damage `_Damaged` | `BeaconVisual_Active`, `ShipPanel_Damaged_A` |
| Node containers | `VisualRoot`, `PrimaryForms`, `SecondaryForms`, `FunctionalDetails`, `EmissiveElements`, `DamageElements`, `CollisionRoot`, `InteractionArea`, `VFXRoot`, `AudioRoot` | stable container names for review. |

## Phase 1 acceptance checklist

- Visual identity and exclusions are explicit.
- Palette includes HEX, normalized RGB, role, allowed usage, and prohibited usage.
- Shape language includes pass/fail criteria.
- Comorian/African-futurist logic is systemic and prohibits decorative clichés.
- Scale/grid standards preserve current player capsule and interaction range.
- Scene architecture separates visuals, collisions, interactions, VFX, audio, and gameplay logic.
- Geometry, material, lighting, VFX, UI, and naming budgets are measurable.
- No Phase 2 asset creation is included.
