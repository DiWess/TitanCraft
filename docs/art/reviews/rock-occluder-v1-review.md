# TC_ENV_RockOccluder_V1 — Formal Review (Backfill)

**Requested:** 2026-07-16
**Reviewed:** 2026-07-16, same session, by explicit human authorization to act as Visual Reviewer for this backlog
**Reviewer role:** Claude Code, acting as Visual Reviewer for the visual half; for the Technical Audit, providing a best-effort **static** analysis only, by explicit human authorization, since real in-engine navigation testing needs Godot and this container confirmed has none available
**Status:** **Visual: COMPLETE. Technical Audit: static analysis complete, caveated — full in-engine confirmation still genuinely PENDING.**

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

## Technical Audit (static analysis, not in-engine testing)

**What real in-engine testing would confirm that this can't:** actual player-capsule navigation around the three rocks, diagonal/exploratory movement, and any interaction with other dynamic collision. None of that can be produced without Godot, which is unavailable in this container. What follows is a static read of the scene file's transforms — evidence-based, but explicitly not a substitute for that test.

**Key finding: the collision geometry did not change.** Each instance's own integration metadata states it directly: *"replaces a plain scaled BoxMesh placeholder... Collision_BlockingRock is untouched."* The `BoxShape3D_blocking_rock` (`size = Vector3(3, 2, 2.3)`, Main.tscn:255) predates this session's visual work and was already the accepted collision volume at each position before the rock mesh was swapped in. Whatever navigation behavior these three boxes produce, this asset's integration did not introduce it — it inherited an already-placed blocking volume unchanged. That significantly narrows what this specific review needs to worry about: it's not "did adding this rock create a new obstacle," it's "was the pre-existing box placeholder ever validated," which is a pre-existing question this asset swap didn't create.

**Position check against key gameplay nodes** (`Main.tscn` transforms):

| Node | Position (X, Z) |
|---|---|
| Player spawn | (0, 0) |
| Save Point | (-12, -12) |
| Workbench | (12, -12) |
| Beacon | (28, -20) |
| `VolcanicRock_1` | (-20, -18) |
| `VolcanicRock_2` | (22, -30) |
| `VolcanicRock_3` | (-25, -35) |

All three rocks sit at larger-magnitude coordinates than every gameplay node they're near — `VolcanicRock_1` is beyond Save Point (not between spawn and Save Point on a direct line), `VolcanicRock_2` is beyond Beacon along the Z axis rather than sitting between Workbench and Beacon, and `VolcanicRock_3` is the furthest-out point in the whole set. Each collision box (after scale, roughly 3–4.5 units across) reads as peripheral dressing near the edge of the walkable area rather than an obstacle straddling a direct path between two gameplay nodes. This is consistent with the asset's own role as a "foreground occluder."

## Verdict

Visual: `PASS`. Technical Audit: static analysis found **no evidence of a routing problem** — unchanged pre-existing collision, peripheral positioning relative to the direct paths between gameplay nodes — but this is a caveated read, not a real in-engine confirmation, and per Technical Director's own forbidden actions ("waiving tests, mixing visual approval with runtime pass") this should not be recorded as a full Technical Director `PASS`. Recommend a quick in-engine walk-around once Godot is available somewhere, but this is now low-priority given the static evidence above, not an open question with no supporting information behind it.
