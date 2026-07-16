# Base Camp Dressing Kit V1 — Formal Review (Promoted)

**Requested:** 2026-07-16
**Reviewed:** 2026-07-16, same session, by explicit human authorization to act as Visual Reviewer for this backlog
**Reviewer role:** Claude Code, acting as Visual Reviewer — adding an independent pass on top of the existing 2026-07-09 agent evidence, per this project's "no self-approval" principle (a second agent looking is not the same as the original author self-certifying, though both fall short of the human/Art Director sign-off still explicitly required below).
**Status:** **PASS carried forward and independently corroborated. Human/Art Director aesthetic sign-off still explicitly open — not something I can close.**

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

Each of the 4 assets has 8 tracked PNGs under `artifacts/asset-review/TC_ENV_{CampAwning,SupplyCrateStack,LightPole,CableOccluder}_V1/` (front/back/left/right/top/hero/material_preview/scale_reference) — the most complete standalone evidence set of any kit in this backfill batch. The 2026-07-09 document's own diagnosis is based on separate **production-scene** captures (4 named camera angles inside `Main.tscn`), which is stronger evidence than isolated asset renders since it shows actual in-game composition and scale.

**Opened this session:** `hero_three_quarter.png` for all four assets (CampAwning, SupplyCrateStack, LightPole, CableOccluder) plus `TC_ENV_CampAwning_V1/scale_reference.png`.

**New finding — the standalone review renders for this kit have a real framing problem, distinct from the assets themselves:** unlike the auto-framed renders used for the MVP Asset Pack, Alien Shard, and other kits (which fill the frame based on computed scene bounds), this kit's `hero_three_quarter.png` renders show the subject very small in a large empty grey frame — SupplyCrateStack, LightPole, and CableOccluder are all difficult to evaluate in detail at that size. Worse, `TC_ENV_CampAwning_V1/scale_reference.png` is essentially non-diagnostic — the subject is almost entirely clipped out of frame, leaving only two leg fragments visible in the top-left corner. This is a camera-setup defect in `render_asset_review.py`'s fixed-position views for this kit's actual proportions, not a defect in the assets — the production-scene screenshots in the 2026-07-09 doc don't have this problem, which is why that document's diagnosis is trustworthy despite this. Still, the standalone evidence set should be re-rendered with corrected framing at some point so it's independently useful without relying on the production capture.

**Independent confirmation from what is visible:** CampAwning's hero shot (small in frame but legible) shows an orange canopy over a dark support frame, consistent with the 2026-07-09 description. Nothing in the visible portions of the other three contradicts that document's account.

## Art Director Opinion (2026-07-16) — not the human sign-off

By explicit human authorization, adding a considered opinion in the Art Director role, on top of the Visual Reviewer pass above. **This does not satisfy the "human/Art Director aesthetic sign-off" the record calls for** — it is a second agent's judgment, not a human's, and the 2026-07-09 document's own language draws that line deliberately. Recording it anyway because a reasoned opinion is more useful sitting here than not existing, as long as it isn't mistaken for the thing that's actually missing.

Checked against `docs/art/titancraft-visual-identity.md`'s Automatic Visual Rejection Patterns:
- Not cartoon/toy-like, not glossy/showroom, not voxel/block-based — the awning, crates, and light poles read as authored industrial structure (hazard-striped crates, a canopy over a support frame, pole fixtures), not primitives glued together.
- Not excessive glow — the kit's job is functional light fixtures, and per the 2026-07-09 doc it fills four previously-bare `OmniLight3D` sources with visible housings rather than adding new light sources; that's the opposite of glow overuse.
- The one disclosed nit (lamp housing floating slightly off its support arm) is a real but minor fit-and-finish issue, not a rejection-pattern violation.

Opinion: this kit does what base-camp dressing should — it turns a functionally bare hub (bare light sources, empty crafting area) into a "lived-in" space without competing with the Workbench/Beacon/SavePoint focal points it surrounds, which matches Art Director's stated review question ("what is the first focal point?" — the answer stays the gameplay props, not the dressing). I'd sign off on this if I were the deciding role. I am not that role here.

## Verdict

`PASS`, carried forward from 2026-07-09 and independently corroborated by this session as far as the (partially broken) standalone evidence allows — no contradiction found, and now with a considered Art Director-role opinion in favor. **Human/Art Director aesthetic sign-off remains explicitly open** — two agent passes agreeing is still not the human approval the record calls for. Additionally recommend the standalone review PNGs for this specific kit be re-rendered with corrected camera framing before anyone relies on them in isolation from the production-scene captures.
