# Phase 3A terrain asset qualification audit

Human review verdict for the previous production terrain composition: `PASS1_TERRAIN_VISUAL_NOT_GO` with a score of 7/60. The failed production node `Main/AuthenticatedTerrainVisuals` has been removed; the files below are audited only as candidate source meshes and are not reintroduced into `Main.tscn`.

## Source policy

Candidate terrain meshes must be byte-identical to an official source file or remain `INVALID_FOR_PRODUCTION`. The official pack-level source currently available in the repository is:

- Official source pack: Quaternius Stylized Nature MegaKit
- Official source URL: https://quaternius.itch.io/stylized-nature-megakit
- Archive source: `Stylized Nature MegaKit[Standard].zip`
- Archive SHA-256: `298f6732b872e4cf7b30e6e7abf9641c7f6dc6b326df37ac089533ed7e3d58c9`

The local OBJ files all begin with a TitanCraft placeholder header and the repository does not contain an independent per-file official source hash for any of the four candidate OBJ files. Therefore none can be proven byte-identical to an official source file in this task.

## Candidate audit table

| File | Local hash | Renamed | Modified | Byte-identical to official source | Vertices | Faces | Materials | AABB min | AABB max | AABB size | Geometric centre | Origin-to-centre | Lowest Y | Highest Y | Longest dimension | Imported orientation | Classification | Reason |
|---|---|---:|---:|---:|---:|---:|---:|---|---|---|---|---:|---:|---:|---:|---|---|---|
| `rock_cliff_d.obj` | `8d4dbac0e98173cb7a1532c74b65b038ebd19cac49bbad24002c6c6e4efb026c` | No | Yes | No | 8 | 6 | 0 | (-0.40, 0.00, -0.72) | (0.64, 0.70, 0.72) | (1.04, 0.70, 1.44) | (0.12, 0.35, 0.00) | 0.370 | 0.00 | 0.70 | 1.44 | Godot OBJ import uses Y-up; no terrain forward axis is authoritative. | INVALID_FOR_PRODUCTION | Placeholder header; no per-file official source hash. |
| `rock_irregular_c.obj` | `74b6d6569c436eb3b3312de8527f5d7b8255d2ac7984b44934c9e0b96ccc0756` | No | Yes | No | 8 | 6 | 0 | (-0.76, 0.00, -0.50) | (0.92, 0.55, 0.50) | (1.68, 0.55, 1.00) | (0.08, 0.275, 0.00) | 0.286 | 0.00 | 0.55 | 1.68 | Godot OBJ import uses Y-up; no terrain forward axis is authoritative. | INVALID_FOR_PRODUCTION | Placeholder header; no per-file official source hash. |
| `rock_ridge_f.obj` | `02214a95b758b785a6b20b1a54c3e5fcaca9fd186b451653e94e4dd50fc37460` | No | Yes | No | 8 | 6 | 0 | (-0.76, 0.00, -0.72) | (0.76, 0.40, 0.72) | (1.52, 0.40, 1.44) | (0.00, 0.20, 0.00) | 0.200 | 0.00 | 0.40 | 1.52 | Godot OBJ import uses Y-up; no terrain forward axis is authoritative. | INVALID_FOR_PRODUCTION | Placeholder header; no per-file official source hash. |
| `rock_spire_g.obj` | `42ce253ad9fe8c89af4e6bb96581c8bc4231864f20cafb82bfdcc474c0c4ccd9` | No | Yes | No | 8 | 6 | 0 | (-0.40, 0.00, -0.50) | (0.48, 0.55, 0.50) | (0.88, 0.55, 1.00) | (0.04, 0.275, 0.00) | 0.278 | 0.00 | 0.55 | 1.00 | Godot OBJ import uses Y-up; no terrain forward axis is authoritative. | INVALID_FOR_PRODUCTION | Placeholder header; no per-file official source hash. |

## Qualification result

No candidate is currently suitable for production terrain use. The isolated debug gallery remains a non-production review scene; it does not connect to gameplay and does not authorize any mesh for `Main.tscn`.
