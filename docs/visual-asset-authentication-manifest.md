# Visual Asset Authentication Manifest — VG-01

Owner: Codex
Version: 1
Date: 2026-07-03
Review status: VG-01_READY_FOR_VISUAL_ONLY_STRUCTURE
Scope: authentication manifest for existing local assets permitted as inputs to visual-only prop/environment structure work.

## Verdict

`PASS` for the assets listed in this manifest to be referenced by isolated visual-only prop scenes. This does not authorize direct production-scene integration, gameplay changes, new asset downloads, or any replacement of `scenes/Main/Main.tscn` visuals.

## Source and licence basis

The selected files come from asset packs already recorded in `THIRD_PARTY_ASSETS.md` with local `LICENSE_CC0.txt` and `SOURCE.txt` files preserved in each pack folder. The intended use remains within the README Crash Site MVP art target: simplified realistic science-fiction with readable polygonal forms.

| Pack | Local licence file | Local source file | Production-use note |
|---|---|---|---|
| Quaternius Stylized Nature MegaKit | `assets/ThirdParty/Quaternius/StylizedNatureMegaKit/LICENSE_CC0.txt` | `assets/ThirdParty/Quaternius/StylizedNatureMegaKit/SOURCE.txt` | Allowed for volcanic rock/route-edge visual modules. |
| Quaternius Ultimate Space Kit | `assets/ThirdParty/Quaternius/UltimateSpaceKit/LICENSE_CC0.txt` | `assets/ThirdParty/Quaternius/UltimateSpaceKit/SOURCE.txt` | Allowed for damaged ship/wreckage visual modules. |
| Quaternius Modular Sci-Fi MegaKit | `assets/ThirdParty/Quaternius/ModularSciFiMegaKit/LICENSE_CC0.txt` | `assets/ThirdParty/Quaternius/ModularSciFiMegaKit/SOURCE.txt` | Allowed for human structure/support visual modules. |
| Quaternius Sci-Fi Essentials Kit | `assets/ThirdParty/Quaternius/SciFiEssentialsKit/LICENSE_CC0.txt` | `assets/ThirdParty/Quaternius/SciFiEssentialsKit/SOURCE.txt` | Allowed for machinery/interactable visual modules. |
| KayKit Space Base Bits | `assets/ThirdParty/KayKit/SpaceBaseBits/LICENSE_CC0.txt` | `assets/ThirdParty/KayKit/SpaceBaseBits/SOURCE.txt` | Allowed as supplemental prop detail only. |

## File authentication table

| Asset file | Bytes | SHA-256 |
|---|---:|---|
| `assets/ThirdParty/Quaternius/StylizedNatureMegaKit/Models/rock_irregular_a.obj` | 340 | `e12747df6f0b72795da33dcaf39d36fa7c95945ab2c6dc6aae89f61483689d68` |
| `assets/ThirdParty/Quaternius/StylizedNatureMegaKit/Models/rock_irregular_b.obj` | 340 | `0f62977d07376487127856944691d598d13b994cd8cd79ae3d9416e0ecd12544` |
| `assets/ThirdParty/Quaternius/StylizedNatureMegaKit/Models/rock_slab_h.obj` | 340 | `7f50b7387dbe286441409414e28d08f4c340d3fe7599051da5028f484efd3c74` |
| `assets/ThirdParty/Quaternius/UltimateSpaceKit/Models/hull_nose_a.obj` | 340 | `e12747df6f0b72795da33dcaf39d36fa7c95945ab2c6dc6aae89f61483689d68` |
| `assets/ThirdParty/Quaternius/UltimateSpaceKit/Models/ship_panel_d.obj` | 340 | `8d4dbac0e98173cb7a1532c74b65b038ebd19cac49bbad24002c6c6e4efb026c` |
| `assets/ThirdParty/Quaternius/UltimateSpaceKit/Models/engine_pod_a.obj` | 340 | `07af55a9074041714a5223f7844e1c0fc691a31497319de54bea555633e6a4aa` |
| `assets/ThirdParty/Quaternius/ModularSciFiMegaKit/Models/support_frame_a.obj` | 340 | `02214a95b758b785a6b20b1a54c3e5fcaca9fd186b451653e94e4dd50fc37460` |
| `assets/ThirdParty/Quaternius/SciFiEssentialsKit/Models/machine_console_a.obj` | 340 | `e12747df6f0b72795da33dcaf39d36fa7c95945ab2c6dc6aae89f61483689d68` |
| `assets/ThirdParty/KayKit/SpaceBaseBits/Models/antenna_a.obj` | 340 | `d1315aafdd78f97335aba816ee686bc6b53209db1ca7708caf8a74863f95e838` |

## Guardrails before production integration

- Use these files first through isolated visual-only prop scenes under `scenes/Props/`.
- Do not add collision, scripts, mission state, save state, or interaction behavior to these visual modules.
- Do not instance these modules into `scenes/Main/Main.tscn` until the next explicit visual implementation slice.
- Re-run Godot import and deterministic visual review when any production scene starts using them.
