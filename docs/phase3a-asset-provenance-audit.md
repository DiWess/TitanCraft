# Phase 3A asset provenance audit — file-level recovery

Final verdict for this recovery slice: `PRODUCTION_VISUAL_SLICE_NOT_GO`.

Reason: the official Quaternius Google Drive folder distribution is now accepted by human decision and selected files from Ultimate Space Kit, Animated Mech Pack, and Animated Robot Pack were authenticated at file level and integrated as visual-only production children. However, this container session did not complete Windows export/playtest or full human visual scoring, so Phase 3B must not begin.

## Authentication basis

The official Quaternius pack page is treated as the authority for creator identity, pack identity, licence, commercial-use permission, modification permission, published formats, and the official Google Drive download entry point. For Google Drive folder distributions, the Drive file ID and original Drive path replace archive filename and archive-internal path.

Licence stated on the official Quaternius pages: CC0 / public domain. Commercial use: permitted. Modification: permitted. Attribution: not required by CC0.

## File-level authenticated Google Drive sources

| Creator | Pack | Official page | Drive folder ID | Drive file ID | Original Drive path | Bytes | SHA-256 | Downloaded UTC | Format | Preserved source path | Renamed | Converted | Conversion command | Converted SHA-256 | Final Godot path |
|---|---|---|---|---|---|---:|---|---|---|---|---|---|---|---|---|
| Quaternius | Ultimate Space Kit | https://quaternius.com/packs/ultimatespacekit.html | `17F8HlI2zPTlo32aieW5YPPwOk78xo-2m` | `1SFhmudLeG3qqoIfT4P8twHXA0K3edv9c` | `Vehicles/OBJ/Spaceship_BarbaraTheBee.obj` | 468496 | `525d9b596e90490fc58ce51bc04f54a1bf82505eddc364857302fa6732cd1730` | 2026-06-30T21:19:55Z | OBJ | `artifacts/source-files/quaternius/Ultimate_Space_Kit/Vehicles/OBJ/Spaceship_BarbaraTheBee.obj` | No | No | N/A | `525d9b596e90490fc58ce51bc04f54a1bf82505eddc364857302fa6732cd1730` | `assets/ThirdParty/Quaternius/UltimateSpaceKit/Models/Spaceship_BarbaraTheBee.obj` |
| Quaternius | Ultimate Space Kit | https://quaternius.com/packs/ultimatespacekit.html | `17F8HlI2zPTlo32aieW5YPPwOk78xo-2m` | `1bLbx7BFkAMwPTFU4livKKMzWRw-8Qms8` | `Vehicles/OBJ/Spaceship_BarbaraTheBee.mtl` | 250 | `6343d6c3926571e682439296fba81acd3ebfdc069f47699465399b3de8e06777` | 2026-06-30T21:19:57Z | MTL | `artifacts/source-files/quaternius/Ultimate_Space_Kit/Vehicles/OBJ/Spaceship_BarbaraTheBee.mtl` | No | No | N/A | `N/A` | `Preserved source only` |
| Quaternius | Ultimate Space Kit | https://quaternius.com/packs/ultimatespacekit.html | `17F8HlI2zPTlo32aieW5YPPwOk78xo-2m` | `1OF0kY7IOqzRer-fRP-TNTXNxc5hQ0-Dj` | `Environment/OBJ/Base_Large.obj` | 199648 | `a907c21b20d88903539c4b64f9142dfce9c3d0b63c87c0d6c938dadfc637f8ee` | 2026-06-30T21:19:59Z | OBJ | `artifacts/source-files/quaternius/Ultimate_Space_Kit/Environment/OBJ/Base_Large.obj` | No | No | N/A | `a907c21b20d88903539c4b64f9142dfce9c3d0b63c87c0d6c938dadfc637f8ee` | `assets/ThirdParty/Quaternius/UltimateSpaceKit/Models/Base_Large.obj` |
| Quaternius | Ultimate Space Kit | https://quaternius.com/packs/ultimatespacekit.html | `17F8HlI2zPTlo32aieW5YPPwOk78xo-2m` | `1Fnf99JDdhaherl4UxLmmQuYzWvdKLE9K` | `Environment/OBJ/Base_Large.mtl` | 237 | `624a1084c38ee600684c28735d9a06509bf175f04bc799761be5f6cd67675a72` | 2026-06-30T21:20:01Z | MTL | `artifacts/source-files/quaternius/Ultimate_Space_Kit/Environment/OBJ/Base_Large.mtl` | No | No | N/A | `N/A` | `Preserved source only` |
| Quaternius | Animated Mech Pack | https://quaternius.com/packs/animatedmech.html | `1sueV_4CGMpZC8y30mWfgKK9UaT3mkHBX` | `1JJ1WPNKHXsvkHyj7u2cZMg2kTbWpJaBJ` | `Textured/OBJ/George.obj` | 594170 | `d34edeb91d6bc0aec2a2402943fe3e46a0bb12a21900436d3348f04e3ba10da5` | 2026-06-30T21:20:03Z | OBJ | `artifacts/source-files/quaternius/Animated_Mech_Pack/Textured/OBJ/George.obj` | No | No | N/A | `d34edeb91d6bc0aec2a2402943fe3e46a0bb12a21900436d3348f04e3ba10da5` | `assets/ThirdParty/Quaternius/AnimatedMechPack/Models/George.obj` |
| Quaternius | Animated Mech Pack | https://quaternius.com/packs/animatedmech.html | `1sueV_4CGMpZC8y30mWfgKK9UaT3mkHBX` | `1MxyeWbPuhRV4ysj7HR4IvwvMEjGwC2Dr` | `Textured/OBJ/George.mtl` | 242 | `af4885447ee42140b2b60bb04e409d834fd955e6e0cc86fc5baa643c0d007bed` | 2026-06-30T21:20:05Z | MTL | `artifacts/source-files/quaternius/Animated_Mech_Pack/Textured/OBJ/George.mtl` | No | No | N/A | `N/A` | `Preserved source only` |
| Quaternius | Animated Mech Pack | https://quaternius.com/packs/animatedmech.html | `1sueV_4CGMpZC8y30mWfgKK9UaT3mkHBX` | `1LBUPT1h3M2EHQYdFwT9xLYAIC_v0T_gR` | `Textured/FBX/George.fbx` | 5139308 | `57c01cdca2d56ace4ddb4b8cdec8828e5b0a78af6db1a2ef1905e8b0b88fda70` | 2026-06-30T21:20:07Z | FBX | `Not committed: binary source authenticated by Drive ID/size/SHA-256 only` | No | No | N/A | `N/A` | `Metadata only (binary not committed)` |
| Quaternius | Animated Robot Pack | https://quaternius.com/packs/animatedrobot.html | `18MU0RtRu9G6SU6uSZ_zMQFmVkRlB4zH5` | `1lg8qzP-l5O5Ghc3lf1W4CpiL-ok_JzcR` | `OBJ/Robot.obj` | 172431 | `ec8b63524aeca072ff7f6b69a911e3ea851e6b33aa9f26edd3786e3d5b9ec51e` | 2026-06-30T21:20:09Z | OBJ | `artifacts/source-files/quaternius/Animated_Robot_Pack/OBJ/Robot.obj` | No | No | N/A | `ec8b63524aeca072ff7f6b69a911e3ea851e6b33aa9f26edd3786e3d5b9ec51e` | `assets/ThirdParty/Quaternius/AnimatedRobotPack/Models/Robot.obj` |
| Quaternius | Animated Robot Pack | https://quaternius.com/packs/animatedrobot.html | `18MU0RtRu9G6SU6uSZ_zMQFmVkRlB4zH5` | `1GIf0GksPtfI4tp4PjEthoPpwlr2F-_tU` | `OBJ/Robot.mtl` | 586 | `34bd55c15149712e52798abc196827ccca56b8bad53c145f9523cca62ade553d` | 2026-06-30T21:20:12Z | MTL | `artifacts/source-files/quaternius/Animated_Robot_Pack/OBJ/Robot.mtl` | No | No | N/A | `N/A` | `Preserved source only` |
| Quaternius | Animated Robot Pack | https://quaternius.com/packs/animatedrobot.html | `18MU0RtRu9G6SU6uSZ_zMQFmVkRlB4zH5` | `1RHFlOum6jk03X_jTsX2OS7I8Vdm4hIwM` | `FBX/Robot.fbx` | 3297804 | `38afb56db7fb17a74d30f0afc8adb5f00441a94e65d6b8ac1958732480f79eb8` | 2026-06-30T21:20:14Z | FBX | `Not committed: binary source authenticated by Drive ID/size/SHA-256 only` | No | No | N/A | `N/A` | `Metadata only (binary not committed)` |

## Binary-file PR workaround

Binary FBX originals are not committed because the PR host does not support binary artifacts in this review flow. Their official Quaternius page, Drive folder ID, Drive file ID, original filename, byte size, and SHA-256 remain recorded above so reviewers can re-download and verify them without storing binary files in Git. Text OBJ/MTL files needed by the current Godot production integration remain committed.

## Production mesh table

| Local mesh | Pack | Official Drive file | Drive original path | Source hash | Local hash | Authentic | Used in proof | Used in production |
|---|---|---|---|---|---|---|---|---|
| `assets/ThirdParty/Quaternius/UltimateSpaceKit/Models/Spaceship_BarbaraTheBee.obj` | Ultimate Space Kit | `1SFhmudLeG3qqoIfT4P8twHXA0K3edv9c` | `Vehicles/OBJ/Spaceship_BarbaraTheBee.obj` | `525d9b596e90490fc58ce51bc04f54a1bf82505eddc364857302fa6732cd1730` | `525d9b596e90490fc58ce51bc04f54a1bf82505eddc364857302fa6732cd1730` | Yes — official page → Google Drive file | No | Yes |
| `assets/ThirdParty/Quaternius/UltimateSpaceKit/Models/Base_Large.obj` | Ultimate Space Kit | `1OF0kY7IOqzRer-fRP-TNTXNxc5hQ0-Dj` | `Environment/OBJ/Base_Large.obj` | `a907c21b20d88903539c4b64f9142dfce9c3d0b63c87c0d6c938dadfc637f8ee` | `a907c21b20d88903539c4b64f9142dfce9c3d0b63c87c0d6c938dadfc637f8ee` | Yes — official page → Google Drive file | No | Yes |
| `assets/ThirdParty/Quaternius/AnimatedMechPack/Models/George.obj` | Animated Mech Pack | `1JJ1WPNKHXsvkHyj7u2cZMg2kTbWpJaBJ` | `Textured/OBJ/George.obj` | `d34edeb91d6bc0aec2a2402943fe3e46a0bb12a21900436d3348f04e3ba10da5` | `d34edeb91d6bc0aec2a2402943fe3e46a0bb12a21900436d3348f04e3ba10da5` | Yes — official page → Google Drive file | No | Yes |
| `assets/ThirdParty/Quaternius/AnimatedRobotPack/Models/Robot.obj` | Animated Robot Pack | `1lg8qzP-l5O5Ghc3lf1W4CpiL-ok_JzcR` | `OBJ/Robot.obj` | `ec8b63524aeca072ff7f6b69a911e3ea851e6b33aa9f26edd3786e3d5b9ec51e` | `ec8b63524aeca072ff7f6b69a911e3ea851e6b33aa9f26edd3786e3d5b9ec51e` | Yes — official page → Google Drive file | No | Yes |

## Invalid and unused local substitutes

The pre-existing manually generated or unauthenticated OBJ placeholders under `assets/ThirdParty/Quaternius/` and `assets/ThirdParty/KayKit/` remain invalid for visual proof unless separately matched to an official archive or Drive file. They were not used as dominant visible meshes in the updated production integration. The isolated `FreeAssetDirectionProof.tscn` still references earlier proof-only placeholders and remains non-authoritative until that scene is separately remapped or retired.

## Production integration notes

- `Main.tscn` now references authenticated Quaternius meshes as visual-only children while preserving gameplay roots, collision nodes, resource quantities, workbench, save point, beacon, player spawn, and Galaxabrain instance paths.
- `GalaxabrainScout.tscn` hides the cube visual and adds an authenticated Animated Robot Pack mesh as a visual-only child; AI, collision, health, attack, speed, and mission component paths are unchanged.
- `Player.tscn` adds an authenticated Animated Mech Pack mesh under `Head/Camera3D` as a visual-only first-person arm; movement, FOV, collision, interaction, and attack logic are unchanged.

## Remaining gate status

- Linux import: passed with OBJ material ambient-light warnings only.
- Real Main screenshot capture: passed; review PNGs are under `docs/review/phase3a-production-integration/`.
- Windows export: completed with `godot --headless --path . --export-release "Windows Desktop" builds/Windows/TitanCraft.exe`.
- Windows playtest/FPS measurement: not completed because the container cannot run the Windows executable interactively.
- Human visual score: not self-approved.

Final verdict: `PRODUCTION_VISUAL_SLICE_NOT_GO`.
