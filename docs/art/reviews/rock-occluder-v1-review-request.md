# TC_ENV_RockOccluder_V1 — Formal Review Request (Backfill)

**Requested:** 2026-07-16
**Requested by:** Claude Code (Code Reviewer & Architecture Validator) — filed per Producer-approved cleanup action, not a completed review
**Status:** **PENDING** — Visual Diagnosis and Verdict must be completed by a human or the Visual Reviewer agent from the opened images below.

## Why this exists

Live in the production scene (3 instances) with only a narrative log entry (`docs/production/quality-scorecard-log.md:219`) — no formal `docs/art/reviews/*.md` verdict exists.

## Provenance

- Source recipe (tracked): `tools/blender/create_rock_occluder_kit_v1.py`
- Source `.blend`: `assets/Source/Blender/Production/TC_ENV_RockOccluder_V1.blend`, SHA-256 `fa3389248a10ba9c54acc56c1ecabce44cebf230fa9f9972e2b44d58168bfca0`
- GLB export: `assets/Production/Generated/Environment/TC_ENV_RockOccluder_V1.glb`
- License: project-authored.

## Integration — important difference from the other five items in this backfill batch

`scenes/Main/Main.tscn`, three `StaticBody3D` instances (not `Node3D`, unlike every other asset in this backfill batch):
- `VolcanicRock_1` (line 673), `VolcanicRock_2` (line 679), `VolcanicRock_3` (line 685)

Each carries a `CollisionShape3D` named `Collision_BlockingRock` (lines 677, 683, 689). **This asset is not visual-only dressing — it participates in player movement/navigation collision**, unlike the Hull Rib Occluder, Alien Shard, Distant Silhouette, and Base Camp Dressing kits, which are all explicitly collisionless. That means this backfill request should route to **Technical Director** as well as Visual Reviewer: a gameplay-blocking collision shape needs a feasibility/navigation check (does it block the intended route in a way that matches level design intent, or accidentally wall off something), not just an aesthetic pass.

## Review Evidence

Three tracked PNGs under `artifacts/asset-review/TC_ENV_RockOccluder_V1/`: `front.png`, `back_three_quarter.png`, `hero_three_quarter.png`.

**Spot-check performed this session (integrity only):** opened `hero_three_quarter.png` — a single low-poly angular rock mass, clean render, no corruption.

## Visual Diagnosis

**PENDING** (Visual Reviewer) — silhouette/material coherence per the standard template.

## Technical Audit

**PENDING** (Technical Director) — confirm the three `Collision_BlockingRock` placements don't obstruct the intended Crash Site route or create unintended navigation dead-ends; this is new scope for this backfill batch since the other five kits carry no collision.

## Verdict

**PENDING** on both fronts. Already integrated in the interim; formal sign-off is open.
