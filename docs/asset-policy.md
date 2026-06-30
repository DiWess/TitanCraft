# TitanCraft Asset Policy — Crash Site Visual Slice

Date: 2026-06-30
Phase: Phase 1 documentation policy only.

This policy governs future assets for the TitanCraft Crash Site MVP. No assets are downloaded, imported, generated, or modified by this Phase 1 document.

## 1. Asset classes

| Class | Definition | Phase 1 decision |
|---|---|---|
| Godot-built assets | Scenes made directly in Godot from primitives, CSG used responsibly, `MeshInstance3D`, `ArrayMesh`, `SurfaceTool`, particles, lights, controls, and shared materials. | Preferred production route for MVP visual slice. |
| Existing repository assets | Current files already in `assets/`, `scenes/`, `data/`, `src/`, `tests/`, and `docs/`. | May be referenced and documented; not modified unless phase scope allows. |
| Internally generated textures/SVGs | Original textures, icons, masks, decals, or SVGs created specifically for TitanCraft by the team/agents. | Allowed later if source, generation method, and license ownership are documented. |
| Free external assets | Third-party assets with no purchase cost. | Allowed only after source/license/commercial-use review and attribution plan. |
| Commercial assets | Paid third-party assets. | Avoid by default; require explicit human approval before purchase/import. |
| Fonts | Typeface files used by UI. | Use Godot/default/system-safe fonts unless an external font license is documented. |
| Audio | Sound effects, ambience, music, UI sounds. | No unknown-license audio; temporary sounds still need source/license. |
| Icons | UI/resource/action pictograms. | Prefer original simple geometric icons; external icon packs require documentation. |

## 2. Required metadata for every external asset

Every external asset introduced after Phase 1 must document:

- exact source URL or distribution channel;
- creator/author name;
- license name and version;
- commercial-use permission;
- modification permission;
- attribution requirement and required wording;
- local file path after import;
- replacement feasibility: easy, moderate, or hard;
- review approval and reviewer/date;
- entry in `THIRD_PARTY_ASSETS.md` if the asset is kept in the repository.

## 3. Approval rules

| Asset source | Approval requirement | Notes |
|---|---|---|
| Godot-built original asset | Phase review approval | Must comply with visual bible and budgets. |
| Internally generated asset | Phase review approval plus source/process note | Include prompt/process only when relevant and permitted. |
| Free external asset | Human approval before import | Verify license permits commercial use and modification. |
| Commercial asset | Explicit human approval before purchase/import | Store receipt/license outside repo if needed; document usage rights. |
| Unknown-license asset | Forbidden | Do not import, commit, trace, or derive from it. |
| Asset imitating one commercial game | Forbidden | Inspiration is allowed only at high-level design-principle level. |

## 4. Documentation locations

- `THIRD_PARTY_ASSETS.md`: required later for any third-party asset kept in the project.
- `docs/asset-policy.md`: this policy.
- Future asset-specific docs may live under `docs/assets/` only if a later phase creates that folder.

## 5. Style compliance rules

- Assets must fit **Polygonal Comorian Afro-Futurist Salvage Sci-Fi**.
- External assets must not force a style shift toward generic western sci-fi, cartoon, cyberpunk neon, photorealistic military, fantasy tribal, voxel, or block-based looks.
- Cultural influence must be visible through construction systems, material reuse, climate adaptation, geometric rhythm, and survival infrastructure rather than surface decoration.
- Replacement feasibility must be considered before accepting any external dependency.

## 6. Technical compliance rules

- Prefer shared Godot `.tres` materials over embedded local materials.
- Prefer simple primitive collisions over imported mesh collisions.
- Imported models, if ever approved, must be inspected for scale, pivot, materials, triangle count, and license.
- No asset may require Blender as a mandatory runtime or build step for the MVP visual slice.
- No asset may add networking, telemetry, cloud service, account, or other non-MVP dependency.

## 7. Phase 1 asset decision

Phase 1 introduces documentation only. It does not add, download, import, modify, or create any production asset files outside the authorized documentation set.
