# Phase 3A source archive cache

Date: 2026-06-30

This directory is a local cache for official creator downloads used during Phase 3A asset-authentication recovery. Large archive binaries are intentionally not committed because repository policy ignores `*.zip`.

Downloaded but uncommitted archives present in the working container:

| Creator | Exact pack | Official source URL | Archive filename | Bytes | SHA-256 | Download date | Licence evidence | Formats included | Extraction destination |
|---|---|---|---|---:|---|---|---|---|---|
| Quaternius | Modular Sci-Fi MegaKit | https://quaternius.itch.io/modular-sci-fi-megakit | `Modular SciFi MegaKit[Standard].zip` | 48559316 | `6fae60cf5189e44dff0bd91097f094a765acc6d57d64a85a0cc0dd56e03035e3` | 2026-06-30 | Official page states CC0/free personal, educational, commercial use. | OBJ, FBX, glTF | Not extracted into production assets in this blocked pass. |
| Quaternius | Sci-Fi Essentials Kit | https://quaternius.itch.io/sci-fi-essentials-kit | `Sci-Fi Essentials Kit[Standard].zip` | 166910545 | `a08346d538aa39fbea9fa492e03620d1860fc6214eedd62a4f5db373ac6fca01` | 2026-06-30 | Official page states CC0/free personal, educational, commercial use. | OBJ, FBX, glTF | Not extracted into production assets in this blocked pass. |
| Quaternius | Stylized Nature MegaKit | https://quaternius.itch.io/stylized-nature-megakit | `Stylized Nature MegaKit[Standard].zip` | 104088529 | `298f6732b872e4cf7b30e6e7abf9641c7f6dc6b326df37ac089533ed7e3d58c9` | 2026-06-30 | Official page states CC0/free personal, educational, commercial use. | OBJ, FBX, glTF | Not extracted into production assets in this blocked pass. |
| Kay Lousberg / KayKit | Space Base Bits | https://kaylousberg.itch.io/space-base-bits | `KayKit_SpaceBaseBits_Free.zip` | 5495833 | `4f8d3e2e90a74d9a0d5262e9e09daccac320f1bcfbe4fa2837559a8ad7b98c17` | 2026-06-30 | Official page states CC0/free personal and commercial use, no attribution required. | OBJ, FBX, glTF | Not extracted into production assets in this blocked pass. |

Blocked archive downloads:

| Creator | Exact pack | Official source URL | Blocking reason |
|---|---|---|---|
| Quaternius | Ultimate Space Kit | https://quaternius.com/packs/ultimatespacekit.html | Official page exposes a Google Drive folder, not a single source archive with archive filename/size/SHA-256 suitable for the requested archive proof. |
| Quaternius | Animated Mech Pack | https://quaternius.com/packs/animatedmech.html | Official page exposes a Google Drive folder, not a single source archive with archive filename/size/SHA-256 suitable for the requested archive proof. |
| Quaternius | Animated Robot Pack | https://quaternius.com/packs/animatedrobot.html | Official page exposes a Google Drive folder, not a single source archive with archive filename/size/SHA-256 suitable for the requested archive proof. |
## Phase 3A file-level Quaternius Google Drive recovery — 2026-06-30

Human decision accepted official Quaternius Google Drive folder/file provenance for Ultimate Space Kit, Animated Mech Pack, and Animated Robot Pack when reached from the official Quaternius pack pages. Selected OBJ/MTL/FBX files were downloaded directly from those folders and SHA-256 hashed. Text OBJ/MTL files used by Godot are committed; binary FBX originals are not committed and are represented by Drive IDs, byte sizes, and hashes as a binary-file PR workaround. OBJ files were copied without geometry conversion into `assets/ThirdParty/Quaternius/.../Models/` for production scene use. Current completion verdict: `PRODUCTION_VISUAL_SLICE_NOT_GO` because Windows export/playtest and human visual scoring remain incomplete.
