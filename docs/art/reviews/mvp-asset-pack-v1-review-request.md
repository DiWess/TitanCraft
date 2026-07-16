# MVP Asset Pack V1 — Formal Review Request (Backfill)

**Requested:** 2026-07-16
**Requested by:** Claude Code (Code Reviewer & Architecture Validator) — filed per Producer-approved cleanup action, not a completed review
**Status:** **PENDING** — Visual Diagnosis and Verdict must be completed by a human or the Visual Reviewer agent; the brief itself already states review is pending, and integration happened anyway.

## Why this exists — the highest-priority item in this backfill batch

`docs/art/briefs/mvp-asset-pack-v1.md:41` states outright: *"Pending human visual review (`BLENDER_ASSET_FORGE_READY` was reached mechanically... visual approval is a human decision per README)."*

Despite that, all 13 assets in this pack are **already live in the production scene** via their sub-scenes, and several are not background dressing — they are the core gameplay-critical props the player interacts with directly: the Workbench, both Beacon states, the Save Point, both Mechanical Arm states, both Galaxabrain Scout states, all four resource pickups, and the crash debris. This is the single biggest gap in the whole backfill batch: foundational MVP assets shipped ahead of the review their own brief says they're still waiting on.

## Assets in this pack

| Asset | SHA-256 (source) | Integrated via |
|---|---|---|
| `TC_PROP_Workbench_V1` | `9a368483ce28850e...` | `scenes/World/Workbench.tscn` |
| `TC_PROP_Beacon_Active_V1` | `9f1eed0c0d51a128...` | `scenes/World/Beacon.tscn` |
| `TC_PROP_Beacon_Dormant_V1` | `5cb5e2a65ad8b5f9...` | `scenes/World/Beacon.tscn` |
| `TC_PROP_SavePoint_V1` | `228c6b0179c455b9...` | `scenes/World/SavePoint.tscn` (referenced via the pack; confirm exact scene at review time) |
| `TC_PLAYER_MechanicalArm_V1` | `ac184e0e07ad70dd...` | `scenes/Player/Player.tscn` |
| `TC_PLAYER_MechanicalArmUnbuilt_V1` | `f684383cd4e814b5...` | `scenes/Player/Player.tscn` |
| `TC_CHAR_GalaxabrainScout_V1` | `04b533f2ab6775c9...` | `scenes/Enemies/GalaxabrainScout.tscn` |
| `TC_CHAR_GalaxabrainScout_Disabled_V1` | `ac8814674fd7dda0...` | `scenes/Enemies/GalaxabrainScout.tscn` |
| `TC_PICKUP_Metal_V1` | `d2286256984f394b...` | `scenes/Resources/ResourceDrop.tscn` |
| `TC_PICKUP_Biomass_V1` | `f685c70f0cdd235f...` | `scenes/Resources/ResourceDrop.tscn` |
| `TC_PICKUP_Electronics_V1` | `5db69263edc6d55f...` | `scenes/Resources/ResourceDrop.tscn` |
| `TC_PICKUP_Component_V1` | `a063725c94bd709c...` | `scenes/Resources/ResourceDrop.tscn` |
| `TC_ENV_CrashDebris_A_V1` | `3697b1969a1fa85f...` | `scenes/Main/Main.tscn` (dressing) |

Source recipe for all 13: `tools/blender/create_mvp_asset_pack_v1.py`. License: project-authored. Collision policy: per-asset, mixed — several of these (Workbench, SavePoint, Beacon, pickups) are functionally interactive, not just visual, so this review should also confirm no visual candidate silently implies a collision/interaction-shape change versus what Gameplay Engineer already built.

## Review Evidence

Each of the 13 assets has three tracked PNGs under `artifacts/asset-review/<asset_name>/`: `front.png`, `back_three_quarter.png`, `hero_three_quarter.png`.

**Spot-check performed this session (integrity only):** opened `TC_PROP_Workbench_V1/hero_three_quarter.png` — a detailed sci-fi workbench render (monitor, articulated arm, drawers, indicator light, foot pedal), clean, no corruption. Basic sanity confirmed for this one asset; the remaining 12 were not individually opened this session.

## Visual Diagnosis

**PENDING for all 13.** Given these are the assets the player looks at and interacts with for the entire MVP loop, this diagnosis carries more weight than the background-dressing kits in this same backfill batch — recommend treating this as the priority item among the six.

## Verdict

**PENDING** for all 13 assets, consistent with the brief's own stated status. Already integrated and shipping in the interim; this request exists to close the gap between "mechanically ready" and "visually approved" that the brief flagged from the start.
