# Seafront Reference V1 — Comorian coral-stone coast

**Status:** active reference. `data/art/comorian_seafront_spec.json` (spec 2.1.0) is the machine-readable form; `tools/blender/create_seafront_kit_v1.py` reads it and builds the assets. Change the spec and regenerate — never hand-edit the generated assets, and never let this document and the spec drift apart.

**Scope:** the seaward edge of the MVP's single map — the harbour front, the civic hall at the water, the volcanic shoreline, the sand pocket and the sea itself. The inland streets are covered separately by `docs/art/briefs/mwezi-quarter-district-v1.md`, but sections 3.5 and 4 below now govern both kits, because the two share a palette.

---

## 1. What this reference is, and what it is not

### No photographs were used, and the spec no longer waits for any

The authoring environment has **no ability to view photographs**. Web search returns titles and URLs; page fetching converts documents to text and discards images. No map, aerial image, survey drawing or photograph of Badjanani, Mtsangani or any other Comorian location was seen while writing this.

Version 1 of this reference handled that by marking the fields a photograph would settle as `needs_photo` and parking a defensible default in each. That was honest but passive: it left the most important values in the spec permanently labelled as guesses waiting on an input that was never going to arrive.

Version 2 replaced those placeholders with values taken from **written descriptions of the two quarters themselves** — descriptions specific enough to contradict things this kit had already built. Every label changed accordingly:

| Label | Meaning |
| --- | --- |
| `text-sourced` | Stated in a general written source about Moroni or Grande Comore |
| `description-sourced` | Stated in a written description of Badjanani, Mtsangani or the hall specifically |
| `assumed` | An authoring choice consistent with the tradition, not attested by a source |
| `unresolved` | No consulted description settles this; the current value is a defensible default |

`needs_photo` is gone as a label. Four fields remain `unresolved`, and they are listed in section 3.4.

**Nothing here should be described as a replica of a real building.** It is a reconstruction of a *building tradition* from written description. That is a much weaker claim than "we copied the photo", and the distinction has to survive into any future review or marketing text.

### What the descriptions changed

Three values in the shipped kits were **wrong** against the written descriptions, not merely unconfirmed. They are recorded here rather than quietly fixed, because "the reference corrected the art" is the only evidence that the reference is doing any work at all:

| What | Was | Now | Why |
| --- | --- | --- | --- |
| Plaster-loss core | Pale coral rag `0.561, 0.510, 0.424` | Ink-black `0.090, 0.086, 0.082` | Where the grey plaster has collapsed, the stones beneath are described as *"noires comme de l'encre"*. The walls are a mix of basalt, crushed coral and sea sand; the basalt is what shows. |
| Quarter woodwork | Dark hardwood `0.224, 0.145, 0.086` | Bleached `0.435, 0.400, 0.353` | The medina's timber is *"d'un bois fendillé, blanchi par l'âge et le climat"* — split and bleached. Dark oiled hardwood is what a **new** door looks like, and nothing in this quarter is new. |
| Hall plinth | Pale coral platform | Dark volcanic base, 1.6 m | The hall is described as standing on *"roches volcaniques sombres à sa base"* beneath *"murs d'un blanc immaculé"*. The first version inverted the single strongest value contrast in the whole composition. |

The overall palette intent the descriptions give is *"un mélange de blancheur et de noirceur"* — a mixture of whiteness and blackness. Version 1's palette had the whiteness and had replaced the blackness with mid-tone beige.

### Creative-property line (README section 67)

The human decision of 2026-09-17 chose *inspired-by, renamed* over faithful recreation, and README section 67 forbids using real names as in-game elements. That still holds, and the descriptions made it sharper rather than looser:

- The in-game place is the **Mwezi Quarter**. No real place name appears in the game.
- The hall is a **secular civic ruin** — a former harbour assembly hall. It is never a place of worship.
- The descriptions name several features that are religious identifiers: a green dome on the minaret, Quranic verses carved into the walls, and a mihrab. These are **deliberately not modelled**. The kit takes only the secular morphology — dark volcanic base, white rendered walls, arcades on slender columns, flat roof, slender corner tower — and omits every religious marker. This omission is recorded in the spec under `provenance.religious_omissions` so it cannot be "fixed" later by someone who thinks detail was missed by accident.
- This document names real places only as *reference*, in the same way an aerospace-salvage asset brief may name a real launch vehicle as reference.

---

## 2. The site

A stone quarter meeting the sea on a volcanic coast.

The medina is formed by **two historic quarters**: Mtsangani to the north and Badjanani to the south, the latter holding the principal mosque and the main market. Two main commercial streets link the upper and lower town; everything else is alleys, sometimes *"aussi minces que des failles rocheuses"* — as narrow as rock fissures. Buildings run one to three storeys, rarely four, with flat roofs.

The coast is *"rocky volcanic ... mostly without beaches"* — but Mtsangani's beaches are separately described as among the finest in Moroni. **Both are true**, and the shore that results is the interesting one: lava rock interrupted by sand, not one or the other. That is why `TC_ENV_BeachPocket_V1` exists as its own asset.

```
            open sea
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        swell, deep blue-green

    ████████  ▓▓▓▓▓▓  ░░░░░  ███████     quay wall, black lava shore,
    quay      lava    sand   quay        one sand pocket, quay again

       ┌──────────────┐   ▲
       │ civic hall   │   │ tower        white walls on a DARK volcanic
       │ ╷╷╷╷╷ arcade │   │              base; arcade toward the water
       ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓   ┘

    ░░░  ░░░░  ░░  ░░░░░  ░░░            medina: 1-3 storeys, flat roofs,
      ░░░░  ░░░░░  ░░░  ░░░░             alleys running back from the water
```

The colour identity is lime-white render, **ink-black basalt**, and blue-green water — and the black is the half that is easiest to lose, because it only appears where something has broken or where the ground shows.

---

## 3. The elements

### 3.1 Sea

| Field | Value | Confidence |
| --- | --- | --- |
| Water level | −0.35 m relative to ground datum | assumed |
| Extent | 140 × 120 m | assumed |
| Deep colour | `0.043, 0.129, 0.184` | assumed |
| Shallow colour | `0.110, 0.325, 0.365` | assumed |
| Roughness | 0.08 | assumed |
| Swell amplitude | 0.09 m | assumed |
| Swell wavelength | 12.0 m | assumed |

The surface is a displaced grid, not a flat plane. A dead-flat water plane reads as glass exactly at the grazing angle the player sees a harbour from, which is why the swell exists at all.

The wavelength is a **long ocean swell**, not wind chop, and that is a sampling decision as much as an artistic one: across 140 m on a 36-column grid the sample spacing is about 3.9 m, so a short wavelength aliases into noise instead of reading as a wave. The first spec value of 5.5 m was wrong for this reason and was corrected.

### 3.2 Shoreline

| Field | Value | Confidence |
| --- | --- | --- |
| Rock colour | `0.086, 0.082, 0.078` | text-sourced |
| Tide-stain colour | `0.160, 0.149, 0.129` | assumed |
| Block count | 26 | assumed |
| Block size range | 0.5 – 2.4 m | assumed |
| Shore band depth | 6.0 m | assumed |

Near-black basalt. Sources call the island's first impression a "volcanic island of harsh black rock", and the dark band is the strongest contrast available against lime-washed stone.

The scatter is deterministic — a hash of the block index, not a random seed — so the shore regenerates byte-comparably for provenance hashing.

**Known weakness:** the blocks read as boxes even when rotated. Real lava is far more jagged. Fixing it properly means a different generation approach (cut prisms, or a displaced surface), which is a larger change than this pass; noted rather than hidden.

### 3.3 Beach pocket

| Field | Value | Confidence |
| --- | --- | --- |
| Present | yes | description-sourced |
| Width | 18 m | assumed |
| Dry sand colour | `0.780, 0.733, 0.639` | assumed |
| Wet sand colour | `0.545, 0.509, 0.447` | assumed |
| Wet fraction | 0.30 of the band | assumed |

A separate asset rather than a variant of the lava tile, because at 18 m it is wider than most of the 22 m lava run — folding it in would make every shore tile part beach. The level lays lava runs and interrupts them with one pocket.

The sand is a **22 × 12 displaced grid**, the same technique `build_sea()` uses, and that took two failed attempts to arrive at. Five sand strips across the band read as white decking, because every seam between strips caught the key light as a plank edge. Merging them into three tilted slabs just produced three larger boards. A beach is a graded surface and box primitives cannot make one. The profile is concave (`u^1.35`), which is what makes the berm read at the top of the swash.

The grid is joined with **bevel disabled** — a bevel on a displaced grid rounds every quad and turns sand into quilting — so the rocks are bevelled separately before the final join.

**Known weakness:** the sand sheet has no thickness, so its outer edge reads as paper from a low grazing angle. In-scene it sits on terrain and the edge is buried, but the asset in isolation shows it.

### 3.4 Civic hall

| Field | Value | Confidence |
| --- | --- | --- |
| Footprint | 16 × 11 m | **unresolved** |
| Wall height | 5.2 m | assumed |
| Parapet height | 0.9 m | assumed |
| Volcanic base height | 1.6 m | description-sourced |
| Wall finish | white lime render over dark volcanic rock | description-sourced |
| Arcade bays | 5 | **unresolved** |
| Arcade span | 2.4 m | assumed |
| Pier width | 0.52 m | description-sourced |
| Springing height | 2.3 m | assumed |
| Arch point ratio | 0.22 | assumed |
| Tower base | 3.4 m | assumed |
| Tower height | 13.5 m | **unresolved** |
| Tower stages | 3 | **unresolved** |
| Floor / ceiling | polished stone / local timber | description-sourced |

The descriptions give the building's *character* precisely and its *dimensions* not at all. What they give: it stands above the shallow port overlooking the ocean; dark volcanic rock at its base under immaculate white walls; elegant arches on refined columns; a polished stone floor and local timber ceilings; and a slender tower, visible across the city, added as a later campaign of work than the hall.

Two of those corrected the model. The base is covered in section 1. The pier width dropped 0.72 m → 0.52 m because 0.72 m square piers on a 2.4 m span are *piers*, and the description says *"colonnes raffinées"*. `platform_height_m` was deleted outright rather than left beside `volcanic_base_height_m`: the plinth **is** the dark base, and two fields for one height is how a corrected value quietly comes back.

The four `unresolved` fields are the ones no consulted description measures. They are exactly the fields that determine whether the building reads correctly at a distance, and they remain the highest-value thing a measured drawing would fix.

**Dating is contested and the model should not assert it.** Most sources give a construction date of 1427 with the tower added in 1921. French-language Wikipedia instead places the building in the early 19th century, noting that Pobéguin's 1899 photographs do not show it. This reference does not adjudicate that. It matters only that the tower reads as a **different campaign of work** from the hall — which every account agrees on — and the stage-and-string-course treatment is how that is expressed.

### 3.5 Materials

This palette governs **both** kits. The values are duplicated in `tools/blender/create_badjanani_district_kit_v1.py`; change them there and in the spec together, or the seafront and the streets behind it stop reading as one settlement.

| Surface | Colour | Confidence | Used by |
| --- | --- | --- | --- |
| Lime render | `0.859, 0.831, 0.761` | text-sourced | both kits |
| Coral rag (dressed: copings, jambs, trim) | `0.561, 0.510, 0.424` | text-sourced | both kits |
| Weathered stone | `0.353, 0.318, 0.263` | assumed | both kits |
| **Plaster-loss core** | `0.090, 0.086, 0.082` | **description-sourced** | both kits |
| **Weathered timber** | `0.435, 0.400, 0.353` | **description-sourced** | both kits |
| Lava rock | `0.086, 0.082, 0.078` | text-sourced | seafront + hall base |

`carved_hardwood` was **removed** from the spec in 2.0.0 rather than left in place beside its replacement. Two timber values in one palette is exactly how a corrected colour comes back.

Note the split in the coral rag role. It is still the right colour for **dressed** stone — cut copings, door jambs, lintels, string courses, parapet caps. It was the wrong colour for **broken** stone, which is the core, which is black.

#### Plaster loss is geometry, not just colour

Recolouring the wall patches was half the fix and the review renders proved it. Three attempts:

1. **Slab proud of the wall face.** Read as three dark panels hung on a white wall — the same protruding-void failure caught earlier on the window openings.
2. **Slab recessed inside the wall.** The patches vanished completely. A recess buried inside a solid render box is not a hole, it is hidden geometry.
3. **Rubble core with a plaster skin cut away.** Correct. The wall is now modelled the way it is built: a full-thickness dark core carrying a 0.04 m plaster skin, with the skin boolean-cut at each patch so the core behind it is genuinely exposed, plus loose stones standing proud of the core inside each opening.

The core is inset by the plaster thickness on X and Z as well as Y, so the skin wraps it on every face — sharing a coplanar end face z-fights, and attempt 3 showed exactly that as a hatched stripe down the end of the wall before it was fixed.

---

## 4. Built assets

All five are visual-only and collisionless. Gameplay collision is authored in the scene as explicit `BoxShape3D` volumes, per the contract the integration suite asserts.

| Asset | Triangles | Budget | Builder |
| --- | ---: | ---: | --- |
| `TC_ENV_SeaWallQuay_V1` | 904 | 1400 | `quay` |
| `TC_ENV_CivicHallSeafront_V1` | 4796 | 5200 | `civic_hall` |
| `TC_ENV_LavaShoreline_V1` | 1144 | 1800 | `shoreline` |
| `TC_ENV_BeachPocket_V1` | 1188 | 1400 | `beach_pocket` |
| `TC_ENV_HarbourSea_V1` | 2160 | 2400 | `sea` |

`TC_ENV_CoralWallSegment_V1` in the district kit rose 396 → 992 triangles in this pass, and its budget 460 → 1060, because plaster loss became real openings: a cut skin plus a back core plus four loose stones per patch, three patches, all bevelled. That is a large relative increase on one small asset and it is the single best-spent geometry in either kit.

All pass `tools/blender/validate_blender_asset.py`. Edge bevelling matches across both kits so the two catch light identically — except the beach sand grid, for the reason in section 3.3.

---

## 5. Open items

- **The four `unresolved` civic hall fields.** No written description measures them. A drawing or a dimensioned survey would; a photograph would help but is not available to this environment.
- **Lava rock silhouette** reads as boxes; needs a different generation approach to read as broken basalt. The beach pocket's edge rocks share the weakness.
- **Beach sand has no thickness** — see section 3.3.
- **Scene integration is not done.** These assets exist and validate; placing them in `scenes/Main/Main.tscn` is a separate gated step, and it collides with a known problem: the procedural terrain rises to 4.2 m at the map bounds and will occlude a water plane placed beyond the seawall. Putting real water in the map means addressing that terrain boundary first. Until then the existing brief's statement — that the seawall fronts a dry tidal flat — remains the shipped truth, and the beach pocket has nothing to be a pocket **in**.
- **No human has reviewed any of this visually.** Asset renders were opened and diagnosed during authoring, and that is what produced every correction recorded above, but aesthetic sign-off remains outstanding, as it does for the whole art pass.

---

## 6. Sources

Written sources only. None supplied images that could be viewed. The descriptions in sections 1, 2 and 3.4 are translated and paraphrased from the French-language sources below; quoted fragments are reproduced as written.

- [Moroni, Comoros — Wikipedia](https://en.wikipedia.org/wiki/Moroni,_Comoros) — port dimensions (80 m quay, 3.5 m draught), medina description, volcanic coastline, climate, mosque dates.
- [Grand Comore — Wikivoyage](https://en.wikivoyage.org/wiki/Grand_Comore) — medina and harbour relationship, shipyard adjacency, beaches and lava coast, nearby settlements.
- [Badjanani Mosque — Madain Project](https://madainproject.com/badjanani_mosque_\(moroni\)) — construction date 1427, position on the harbour at the edge of the stone medina.
- [Mosquée de Moroni — Wikipédia (fr)](https://fr.wikipedia.org/wiki/Mosqu%C3%A9e_de_Moroni) — the dissenting early-19th-century dating and the 1899 Pobéguin photographs.
- [La médina sans voile — Lettres de l'Océan](https://www.lettres-ocean.net/blog/2017/1/31/la-mdina-sans-voile) — **the most load-bearing source here.** Quarter names and positions, storey counts, alley widths, flat roofs, the two commercial streets, the basalt/crushed-coral/sea-sand wall mix, the ink-black stone behind collapsed plaster, and the split age-bleached timber.
- [Grande Mosquée de Paris — mosques of the world, no. 45: Moroni](https://www.grandemosqueedeparis.fr/post/lumiere-et-lieux-saints-de-l-islam-la-decouverte-des-mosquees-du-monde-n-45-moroni) — the hall's dark volcanic base under white walls, arches and refined columns, polished stone floor, timber ceilings, slender tower.
- [Quartiers de Moroni — Maskane](https://maskane.com/quartiers-exclusifs-moroni-expatries) — Mtsangani as the northern quarter and its beaches.
- [Friday Mosque Moroni — Archnet](https://www.archnet.org/sites/15056) — listed for completeness; the page returned HTTP 429 and could not be read, so **nothing in this document derives from it**.

Version 1 of this document recorded that a search for **Mtsangani** returned nothing usable. That is no longer true and the correction is worth keeping visible: the name resolves once it is searched alongside Moroni and Badjanani rather than alone, because several localities share it. It is the northern of the medina's two quarters.
