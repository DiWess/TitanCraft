# TitanCraft Material Library — Phase 2 Master Materials

Date: 2026-06-30
Scope: Crash Site MVP master material resources under `assets/Materials/`.

Phase 2 creates and normalizes reusable `StandardMaterial3D` resources only. No shaders, textures, gameplay scripts, scenes, layout, VFX, audio, or external assets are introduced.

## Shared-resource policy

- Use shared `.tres` resources from `assets/Materials/` for production scenes.
- Do not create local scene material overrides unless a future review documents why a shared resource cannot meet the need.
- Keep existing material filenames stable so current scene `ExtResource` references remain valid.
- Prefer conservative roughness, metallic, and emission values; do not maximize shine or glow by default.

## Shader policy

No shader is introduced in Phase 2. `StandardMaterial3D` satisfies the current color, metallic, roughness, emission, and limited glass transparency needs. A future shader requires a documented purpose, exposed parameters, fallback behavior, no gameplay logic, and a visual-review approval.

## Emission hierarchy

1. Non-powered hull, terrain, soot, vegetation, and ordinary metal: no emission.
2. Human powered/status surfaces: weak emission, target multiplier `0.35`.
3. Human interaction orange: restrained emission, multiplier `0.35`, because it signals function but must not replace lighting.
4. Alien violet/cyan/turquoise: stronger but controlled emission, multiplier `1.0-1.2`.
5. Magenta: minor stress/accent only, multiplier `0.7`.
6. Inactive powered objects: no emission by default.

## Orange and cyan usage rules

- `HumanOrangeInteractive.tres` is reserved for interaction, handles, survival equipment, emergency/construction systems, and clear functional trim.
- `HumanScreenCyan.tres` is reserved for human screens, electronics, and mechanical-arm status; it should not be used as general alien energy.
- `AlienCyanEmissive.tres` and `AlienTurquoiseEmissive.tres` are reserved for powered alien systems, Galaxabrain state, and justified beacon activation.
- Do not make every object glow; emissive materials must remain accents or state indicators.

## Future Phase 3 usage expectations

Phase 3 human kit modules should use `HumanIvory`, `HumanGraphite`, `HumanWornSteel`, `HumanBronze`, `HumanOrangeInteractive`, `HumanEmergencyRed`, `HumanScreenCyan`, `IndustrialGlass`, `DamageScorch`, and `InactiveEmission` as shared resources. Environment and alien materials should remain distinct from human graphite so basalt and Galaxabrain technology do not read as the same surface.

## Material inventory

| Filename | Category | Exact base color | Roughness | Metallic | Emission | Transparency | Intended use | Prohibited use | Existing/new | Performance considerations |
|---|---|---:|---:|---:|---|---|---|---|---|---|
| `HumanIvory.tres` | Human | `#D4C8AE` | `0.72` | `0.0` | none | opaque | off-white hull panels, C7 base surfaces, workbench top | alien shells, organic matter, soot | existing normalized | cheap opaque material |
| `HumanGraphite.tres` | Human | `#252A30` | `0.58` | `0.2` | none | opaque | structural metal, casings, crates, dark machine parts | terrain basalt, alien energy | existing normalized | cheap opaque material |
| `HumanWornSteel.tres` | Human | `#6E7478` | `0.52` | `0.6` | none | opaque | exposed steel, rails, hinges, bolts, scraped edges | broad terrain, energy glow | new | moderate metallic only; no transparency |
| `HumanBronze.tres` | Human | `#8A633D` | `0.48` | `0.55` | none | opaque | warm mechanical reinforcement, brackets, bearings | alien carapace, full hull paint | existing normalized | moderate metallic only; no transparency |
| `HumanOrangeInteractive.tres` | Human | `#E87822` | `0.42` | `0.15` | `#E87822`, multiplier `0.35` | opaque | interactables, handles, survival and construction systems | random decoration, alien energy, terrain | existing normalized | restrained emission; avoid overuse |
| `HumanEmergencyRed.tres` | Human | `#C3362C` | `0.58` | `0.05` | none | opaque | rare emergency LEDs/signage, danger strips, low-health future UI accents | normal interaction, alien cores | new | cheap opaque material |
| `HumanScreenCyan.tres` | Human | `#34C8E8` | `0.28` | `0.05` | `#34C8E8`, multiplier `0.35` | opaque | human screens, electronics, arm status indicators | general alien energy or all props | new | weak human emission; keep small surfaces |
| `HumanSootBurn.tres` | Damage | `#08090A` | `0.92` | `0.0` | none | opaque | soot, burn marks, exhaust interiors | whole-prop base albedo, readable UI | new | very cheap matte material |
| `AlienBlack.tres` | Alien | `#0A0B12` | `0.62` | `0.35` | none | opaque | Galaxabrain shell, invasive alien plates | human machinery, ordinary rocks | existing normalized | dark material needs contrast accents |
| `AlienVioletEmissive.tres` | Alien | `#7A3FF2` | `0.35` | `0.1` | `#7A3FF2`, multiplier `1.2` | opaque | alien cores, crystal lines, active alien state | human workbench, normal pickups | existing normalized | stronger emission; restrict surface area |
| `AlienColdMetal.tres` | Alien | `#8A96A3` | `0.5` | `0.45` | none | opaque | cold alien plate edges, broken fragments | human warm repair metal, terrain soil | new | moderate metallic only |
| `AlienCyanEmissive.tres` | Alien | `#34C8E8` | `0.35` | `0.1` | `#34C8E8`, multiplier `1.0` | opaque | powered alien channels, Galaxabrain state | every alien surface, human emergency | new | controlled emission; keep to accents |
| `AlienTurquoiseEmissive.tres` | Alien | `#2FE6C8` | `0.32` | `0.1` | `#2FE6C8`, multiplier `1.2` | opaque | intense alien activation, beacon activation where justified | idle decoration, all crystals always | new | strongest emission; use sparingly |
| `AlienMagentaAccent.tres` | Alien | `#C247B8` | `0.42` | `0.05` | `#C247B8`, multiplier `0.7` | opaque | rare alien stress, unstable core flicker, death burst accent | human props, general lighting | new | minor accent emission only |
| `VolcanicRock.tres` | Environment | `#15171A` | `0.9` | `0.05` | none | opaque | basalt rock mass, current ground and rocks | machinery graphite replacement | existing normalized | cheap matte terrain material |
| `VolcanicSoil.tres` | Environment | `#2B211C` | `0.92` | `0.02` | none | opaque | compact ash soil, paths, crater floor | human hull, alien shell | new | cheap matte material |
| `VolcanicAsh.tres` | Environment | `#6C6E6A` | `0.94` | `0.0` | none | opaque | ash dust, weathering, low-contrast terrain variation | interaction color, alien energy | new | cheap matte material |
| `VolcanicSunlitRock.tres` | Environment | `#9A6E43` | `0.88` | `0.03` | none | opaque | sunlit volcanic edges, distant ridge warmth | UI warning, alien core | new | cheap matte material |
| `SparseVegetation.tres` | Environment | `#3E5F45` | `0.82` | `0.0` | none | opaque | sparse hardy plant clusters, moss-like pockets | dense biome, pickup marker | new | cheap opaque material |
| `OceanAtmosphere.tres` | Environment | `#18233D` | `0.75` | `0.0` | none | opaque | distant ocean/sky atmosphere surfaces if retained | local emissive markers | new | cheap opaque material |
| `IndustrialGlass.tres` | Special | `#34C8E8` alpha `0.35` | `0.18` | `0.0` | none | alpha transparency | industrial glass/screen overlay layers | ordinary panels, rocks, hull, dirt | new | transparent; use sparingly to avoid overdraw |
| `BiomassRed.tres` | Organic | `#641E2B` | `0.86` | `0.05` | none | opaque | biomass pickups, organic Galaxabrain remnants | human panels, terrain base | existing normalized | cheap opaque material |
| `DamageScorch.tres` | Damage | `#08090A` | `0.96` | `0.0` | none | opaque | scorch decals/patches, impact burns | whole-prop base material | new | cheap matte material |
| `InactiveEmission.tres` | Special | `#252A30` | `0.7` | `0.05` | none | opaque | powered-down screens/cores/LED panels | active state, interaction highlight | new | no emission; cheap inactive fallback |

## Phase 2 validation notes

- All materials use `StandardMaterial3D`.
- No external textures or assets are introduced.
- No shader resources are introduced.
- Only `IndustrialGlass.tres` uses transparency, and it is documented as a special-purpose material.
- Existing scene references remain stable because existing material filenames are preserved.
