# Base Camp Dressing Kit V1 — Formal Review Promotion Request (Backfill)

**Requested:** 2026-07-16
**Requested by:** Claude Code (Code Reviewer & Architecture Validator) — filed per Producer-approved cleanup action, not a completed review
**Status:** **PENDING** — this kit already has real evidence and an opened-image diagnosis; what's missing is promotion into `docs/art/reviews/` and the explicit human aesthetic sign-off the existing document itself says is still open.

## Why this exists

Unlike the other five items in this backfill batch, this kit is *not* starting from a bare narrative log entry. `docs/release/evidence/titancraft-base-camp-dressing-pass-2026-07-09.md` already contains: provenance/hashes for all 4 assets, a documented authoring bug caught and fixed before integration, opened production-scene screenshot diagnosis (4 named capture angles), and full validation output (`dotnet build`/`test`, Godot import, `validate_agent_studio.py`, manifest check, `git diff --check`). It even records a `PASS` for the slice.

The gap is process, not evidence quality: that document lives under `docs/release/evidence/`, not `docs/art/reviews/` where this project's other formal asset verdicts live, and it says explicitly: *"Human/Art Director aesthetic sign-off remains open... this record is agent evidence, not a human approval."* That sentence has never been closed out.

## Assets in this kit

| Asset | SHA-256 (source) | Scene instances |
|---|---|---|
| `TC_ENV_CampAwning_V1` | `29adac4dcc283c2b43eb260233671d99884b5d6b3af5651ad58154234997d2a8` | `BaseCampDressing/CampAwning` (line 1210) |
| `TC_ENV_SupplyCrateStack_V1` | `e9ed6aa931f0f347e24aa051dd16d8ac8a5eb8992227ec6e2bf6af2d8608ea2f` | `CrateStack_Workbench`, `CrateStack_Hull` (lines 1212, 1214) |
| `TC_ENV_LightPole_V1` | `4f72a571660f6df0ab89e49857691100d1682c27fe9c790864cf03242e119a35` | `LightPole_1..4` (lines 1216–1222) |
| `TC_ENV_CableOccluder_V1` | `6ec6b74e10b853df12ad3aad7bb66d29914b09e4563f13e192d6d5e260039631` | `CableRun_Workbench`, `CableRun_Hull` (lines 1224, 1226) |

Source recipe: `tools/blender/create_base_camp_dressing_kit_v1.py`. License: project-authored. Collision policy: none — `LightPole_1..4` visually dress the four existing `BaseLamp` positions without changing light count, per the 2026-07-09 evidence doc.

## Review Evidence

Each of the 4 assets has 8 tracked PNGs under `artifacts/asset-review/TC_ENV_{CampAwning,SupplyCrateStack,LightPole,CableOccluder}_V1/` (front/back/left/right/top/hero/material_preview/scale_reference) — the most complete evidence set of any kit in this backfill batch.

**Spot-check performed this session (integrity only):** opened `TC_ENV_CampAwning_V1/hero_three_quarter.png` — a clean render of an orange canopy over a support frame with dark metal legs, no corruption, matches the material description in the 2026-07-09 doc.

## What's actually needed

Not a new diagnosis — the 2026-07-09 document's diagnosis is real and already opened real images. What's needed:

1. Move or duplicate the verdict into `docs/art/reviews/` (or have the Art Director/Visual Reviewer explicitly cross-reference it from there), so this kit is discoverable through the same path as every other formal review.
2. Close the specific open item the document names itself: **human/Art Director aesthetic sign-off.** This is not something Claude Code can provide — it requires the named role.
3. Optionally address the disclosed cosmetic nit (lamp housing floating slightly off its support arm) if a V2 pass happens before that sign-off.

## Verdict

Carried forward as `PASS` (agent evidence) per the 2026-07-09 document, but **not closed** until human/Art Director sign-off lands. This request does not re-open or dispute that PASS — it flags that the sign-off step was never completed and the record was never filed where the rest of the review corpus lives.
