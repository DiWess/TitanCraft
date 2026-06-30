# Visual Pipeline Pivot — Asset-First Proof Scene

Date: 2026-06-30
Scope: isolated visual-direction recovery after failed Phase 3A human screenshot review.

## Why Phase 3A failed visual acceptance

Phase 3A was technically valid, but the visible result remained recognizably built from Godot primitive boxes: C7 walls, panels, frames, rails, and damaged variants read as cuboid panels with decorative trim rather than authored salvage sci-fi assets. The failure is therefore a visual-pipeline failure, not a gameplay failure.

The rejected approach should not become the final art foundation because it optimizes primitive assembly instead of asset selection, silhouette language, material hierarchy, and first-person art review.

## Phase 3A files and production changes inspected

Latest history shows Phase 3A in commits `782c81f` and `811654f`, merged by `746e325`.

Phase 3A files added or changed for the primitive kit:

- `scenes/Props/Human/Beam_A.tscn`
- `scenes/Props/Human/Frame_A.tscn`
- `scenes/Props/Human/Panel_A.tscn`
- `scenes/Props/Human/Panel_Damaged_A.tscn`
- `scenes/Props/Human/Rail_A.tscn`
- `scenes/Main/Main.tscn` visual-only C7 wall instance integration
- `docs/phase-3a-human-structure-kit.md`

Production-scene changes found: the main gameplay scene references Phase 3A C7 wall visual modules around the existing C7 wall roots. No gameplay scripts, mission contracts, player controller, crafting, save system, or enemy AI were intentionally changed by this pivot task.

## Experimental status

The Phase 3A primitive modules are retained as experiments only. They are useful for scale studies, palette tests, scene-contract notes, and collision-safe visual layering, but they are explicitly excluded as the basis for final production art.

## Authoritative assets retained

The following remain authoritative for visual direction unless superseded by a future approved README/documentation update:

- `README.md`
- `docs/visual-style.md`
- `docs/visual-technical-bible.md`
- `docs/asset-policy.md`
- `docs/material-library.md`
- `docs/visual-review-checklist.md`
- `assets/Materials/*.tres`

The TitanCraft master materials remain the project palette source. Imported assets may keep functional embedded materials during proof-scene evaluation, but production integration must converge toward the master palette or a documented approved variant.

## New asset-first pipeline

1. Research candidate asset packs before scene editing.
2. Verify source, creator, license, commercial-use rights, modification rights, attribution requirements, formats, and Godot compatibility.
3. Import only a minimal sample, not an entire library.
4. Build an isolated proof scene under `scenes/Proof/`.
5. Use imported or authored non-cubic meshes for dominant silhouettes.
6. Document every imported file in `THIRD_PARTY_ASSETS.md`.
7. Capture review screenshots before any production-scene integration.

## Proof-scene gate

`scenes/Proof/VisualDirectionProof.tscn` is the only scene created for this pivot. It is not connected to menus, save data, missions, combat, or the production Crash Site route.

The proof scene must pass human screenshot review before any asset-first approach can be integrated into `scenes/Main/Main.tscn`.

## Conditions before production integration

Production integration remains blocked until all of the following are true:

- human review approves the proof-scene screenshots;
- selected third-party assets have complete source and license records;
- no asset requires purchase, credentials, noncommercial terms, or unclear terms;
- production collision and gameplay node contracts are preserved;
- gameplay tests remain unchanged or are expanded only for approved behavior;
- visual changes have before/after screenshots and a manual playtest plan;
- Phase 3B and later primitive-roadmap work remains paused unless explicitly reauthorized.
