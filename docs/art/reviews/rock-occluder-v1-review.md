# TC_ENV_RockOccluder_V1 — Formal Review (Backfill)

**Requested:** 2026-07-16
**Reviewed:** 2026-07-16, same session, by explicit human authorization to act as Visual Reviewer for this backlog
**Reviewer role:** Claude Code, acting as Visual Reviewer only — the Technical Audit below remains genuinely PENDING, since collision/navigation feasibility is Technical Director's call, not something I can self-certify as the same agent doing the visual pass
**Status:** **Visual: COMPLETE. Technical Audit: still PENDING.**

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

All three tracked PNGs opened this session: `front.png`, `back_three_quarter.png`, `hero_three_quarter.png` under `artifacts/asset-review/TC_ENV_RockOccluder_V1/`.

## Visual Diagnosis

A single low-poly angular rock mass, grey stone material, clean asymmetric faceting. Silhouette reads unambiguously as a boulder — functional, not decorative, no toy-like proportions, no rejection-pattern violations. Simple enough that repeated use at three positions (`VolcanicRock_1..3`) is reasonable for a blocking/occluding element rather than a hero prop; no scale reference in the renders, but a rock occluder's exact scale matters less than its role as a collision volume, which is the Technical Director's question below, not a visual one.

## Technical Audit

Still **PENDING** — genuinely so. Confirming whether the three `Collision_BlockingRock` placements route/block the Crash Site path as level design intends requires either in-engine navigation testing or a level-layout review, neither of which is something to fabricate from a static asset render. This is explicitly left for Technical Director.

## Verdict

Visual: `PASS`. Technical Audit: **PENDING** — do not treat this asset as fully closed until that half is done, since it's the only kit in this backfill batch with real gameplay collision.
