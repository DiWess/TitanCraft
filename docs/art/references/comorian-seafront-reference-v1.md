# Seafront Reference V1 — Comorian coral-stone coast

**Status:** active reference. `data/art/comorian_seafront_spec.json` is the machine-readable form; `tools/blender/create_seafront_kit_v1.py` reads it and builds the assets. Change the spec and regenerate — never hand-edit the generated assets, and never let this document and the spec drift apart.

**Scope:** the seaward edge of the MVP's single map — the harbour front, the civic hall at the water, the volcanic shoreline, and the sea itself. The inland streets are covered separately by `docs/art/briefs/mwezi-quarter-district-v1.md`.

---

## 1. What this reference is, and what it is not

### No photographs were used

This is the single most important thing to know before trusting any number below.

The authoring environment has **no ability to view photographs**. Web search returns titles and URLs; page fetching converts documents to text and discards images. No map, aerial image, survey drawing or photograph of Badjanani, Mtsangani or any other Comorian location was seen while writing this.

Every value in the spec therefore comes from one of three places, and the spec labels each one:

| Label | Meaning |
| --- | --- |
| `text-sourced` | Stated in a written source, cited in section 6 |
| `assumed` | An authoring choice consistent with the tradition, not attested by a source |
| `needs_photo` | A photograph would settle this; the current value is a defensible default |

**Nothing here should be described as a replica of a real building.** It is a reconstruction of a *building tradition* from written description. That is a much weaker claim than "we copied the photo", and the distinction has to survive into any future review or marketing text.

### How to upgrade this reference with real pictures

Attach photographs to the working session. Images supplied that way *can* be read directly. For each photograph, the useful things to extract, in rough order of value:

1. **Counts** — arcade bays, tower stages, window openings per face, steps in a stair.
2. **Proportions against a human** — someone standing in frame turns every ratio into metres.
3. **Rooflines** — parapet height relative to wall, whether the roof is flat, and what sits on it.
4. **The waterline** — how the building or quay meets the sea, freeboard above water, whether there is a beach, rock, or a vertical wall.
5. **Colour in daylight** — lime white against coral grey against black lava, and how much the render has failed to bare stone.
6. **Openings** — arch profile (round, pointed, or ogee), door surround depth, shutter style.

Then update the matching fields in `data/art/comorian_seafront_spec.json`, flip their `confidence` to `photo-sourced`, remove them from `needs_photo`, and re-run the builder. **No code change is needed** — that is the reason the spec is a separate data file.

### Creative-property line (README section 67)

The human decision of 2026-09-17 chose *inspired-by, renamed* over faithful recreation, and README section 67 forbids using real names as in-game elements. That still holds here:

- The in-game place is the **Mwezi Quarter**. No real place name appears in the game.
- The hall is a **secular civic ruin** — a former harbour assembly hall. It is never a place of worship, and no religious function, furnishing or inscription is modelled.
- This document names real places only as *reference*, in the same way an aerospace-salvage asset brief may name a real launch vehicle as reference.

---

## 2. The site

A stone quarter meeting the sea on a volcanic coast.

Written sources agree on the arrangement: the old mosque "sits on the harbor adjacent to a shipyard and the medina"; the medina is "a maze of narrow alleys and ancient buildings", "similar to but smaller than the old town of Lamu"; the coastline is "rocky volcanic ... mostly without beaches". Those three facts give the whole composition:

```
            open sea
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        swell, deep blue-green

    ████████  ▓▓▓▓▓▓▓▓   ███████         quay wall + black lava shore,
    quay      lava rock  quay            alternating along the water

       ┌──────────────┐   ▲
       │ civic hall   │   │ tower        hall on a raised sea platform,
       │ ╷╷╷╷╷ arcade │   │              arcaded face toward the water
       └──────────────┘   ┘

    ░░░  ░░░░  ░░  ░░░░░  ░░░            medina: narrow alleys running
      ░░░░  ░░░░░  ░░░  ░░░░             back from the water
```

The quarter is **not** a beach settlement. Where the built edge stops, black lava rock takes over, not sand. That contrast — lime-white stone, black rock, blue-green water — is the entire colour identity of the seafront, and it is the thing most worth protecting in any later art pass.

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

### 3.3 Quay

| Field | Value | Confidence |
| --- | --- | --- |
| Real reference length | 80 m | **text-sourced** |
| Asset tile length | 12 m | assumed |
| Height above water | 1.9 m | assumed |
| Depth | 2.2 m | assumed |
| Bollards per tile | 3 | assumed |
| Landing stairs per tile | 1 | assumed |

The 80 m is one of the few hard figures available: Moroni's port is documented as an "80 metres quay with a draught of 3.5 metres", taking vessels up to 150 m, with the reef ruling out larger ships. The asset is a 12 m **tile** so the level can lay any run and still cite the real figure as the reference the run was sized against.

The landing steps down the seaward face matter more than they look: they are what makes a wall read as a *working* harbour rather than a barrier.

### 3.4 Civic hall

| Field | Value | Confidence |
| --- | --- | --- |
| Footprint | 16 × 11 m | `needs_photo` |
| Wall height | 5.2 m | assumed |
| Parapet height | 0.9 m | assumed |
| Arcade bays | 5 | `needs_photo` |
| Arcade span | 2.4 m | assumed |
| Pier width | 0.72 m | assumed |
| Springing height | 2.3 m | assumed |
| Arch point ratio | 0.22 | assumed |
| Tower base | 3.4 m | assumed |
| Tower height | 13.5 m | `needs_photo` |
| Tower stages | 3 | `needs_photo` |
| Sea platform height | 1.2 m | assumed |

What *is* firmly sourced is history, not geometry: the building dates to 1427, and its tower was a much later addition in 1921. That matters for the model — the tower should read as a different campaign of work from the hall, not as one design. The stage-and-string-course treatment is how that is expressed.

No source consulted gives a single dimension, bay count or height. Those four `needs_photo` fields are the highest-value things a photograph would fix, and they are exactly the fields that determine whether the building reads correctly at a distance.

### 3.5 Materials

| Surface | Colour | Shared with |
| --- | --- | --- |
| Lime render | `0.859, 0.831, 0.761` | Mwezi Quarter district kit |
| Coral rag | `0.561, 0.510, 0.424` | Mwezi Quarter district kit |
| Weathered stone | `0.353, 0.318, 0.263` | Mwezi Quarter district kit |
| Carved hardwood | `0.224, 0.145, 0.086` | Mwezi Quarter district kit |
| Lava rock | `0.086, 0.082, 0.078` | seafront only |

The first four are **identical** to the inland kit by design. The seafront and the streets behind it must read as one settlement, and shared material values are what guarantee that regardless of how either is lit.

Sources consistently describe coral stone, whitewashed walls, and carved wooden doors, set against dark volcanic rock — which is what this palette encodes.

---

## 4. Built assets

All four are visual-only and collisionless. Gameplay collision is authored in the scene as explicit `BoxShape3D` volumes, per the contract the integration suite asserts.

| Asset | Triangles | Budget | Builder |
| --- | ---: | ---: | --- |
| `TC_ENV_SeaWallQuay_V1` | 904 | 1400 | `quay` |
| `TC_ENV_CivicHallSeafront_V1` | 4796 | 5200 | `civic_hall` |
| `TC_ENV_LavaShoreline_V1` | 1144 | 1800 | `shoreline` |
| `TC_ENV_HarbourSea_V1` | 2160 | 2400 | `sea` |

All four pass `tools/blender/validate_blender_asset.py`. Edge bevelling matches the inland kit so the two catch light identically.

---

## 5. Open items

- **The four `needs_photo` fields** on the civic hall. Highest value, lowest effort to fix.
- **Lava rock silhouette** reads as boxes; needs a different generation approach to read as broken basalt.
- **Scene integration is not done.** These assets exist and validate; placing them in `scenes/Main/Main.tscn` is a separate gated step, and it collides with a known problem: the procedural terrain rises to 4.2 m at the map bounds and will occlude a water plane placed beyond the seawall. Putting real water in the map means addressing that terrain boundary first. Until then the existing brief's statement — that the seawall fronts a dry tidal flat — remains the shipped truth.
- **No human has reviewed any of this visually.** Asset renders were opened and diagnosed during authoring, but aesthetic sign-off remains outstanding, as it does for the whole art pass.

---

## 6. Sources

Written sources only. None supplied images that could be viewed.

- [Moroni, Comoros — Wikipedia](https://en.wikipedia.org/wiki/Moroni,_Comoros) — port dimensions (80 m quay, 3.5 m draught), medina description, volcanic coastline, climate, mosque dates.
- [Grand Comore — Wikivoyage](https://en.wikivoyage.org/wiki/Grand_Comore) — medina and harbour relationship, shipyard adjacency, beaches and lava coast, nearby settlements.
- [Badjanani Mosque — Madain Project](https://madainproject.com/badjanani_mosque_\(moroni\)) — construction date 1427, position on the harbour at the edge of the stone medina.
- [Friday Mosque Moroni — Archnet](https://www.archnet.org/sites/15056) — listed for completeness; the page returned HTTP 429 and could not be read, so **nothing in this document derives from it**.

A search for **Mtsangani** specifically returned no usable architectural or topographic description; results covered other Grande Comore settlements instead. The name is Comorian for a sandy place, and several localities share it, which is likely why it did not resolve. If a specific Mtsangani is meant, a photograph or a precise location would settle it — nothing in this reference currently claims to represent it.
