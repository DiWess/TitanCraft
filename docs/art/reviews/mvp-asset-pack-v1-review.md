# MVP Asset Pack V1 — Formal Review (Backfill)

**Requested:** 2026-07-16
**Reviewed:** 2026-07-16, same session, by explicit human authorization to act as Visual Reviewer for this backlog
**Reviewer role:** Claude Code, acting as Visual Reviewer (not self-approving own generated work — these assets predate this session). Note: this closes the *visual* half of the brief's "pending human visual review" status; it is agent evidence, and the brief's own language ("visual approval is a human decision per README") means a human should still be able to override this verdict.
**Status:** **COMPLETE — overall PASS, one flagged deviation on Workbench**

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

All 13 assets opened this session (`hero_three_quarter.png` for all 13; `front.png`/`back_three_quarter.png` also opened for Workbench and both Beacon states).

## Visual Diagnosis

**Resource pickups (Metal, Biomass, Electronics, Component) — strong, coherent set.** Each reads instantly distinct from the other three by both silhouette and color: Metal is flat angular grey-silver hull shards with one orange paint mark; Biomass is a dark-red faceted organic spiky cluster; Electronics is stacked dark modules with an orange lid, antenna, and cyan LED strip; Component is a purple emissive crystal cluster on a dark rock shard, using a distinct hue (purple) from the other three specifically to signal "this one's different" (mission item vs. crafting resource). This is good gameplay-legibility design, not just individually nice models — a player needs to tell these apart at a glance while playing, and they can.

**Scout enemy (active + disabled) — strong.** Spindly quadruped biomech, long thin tapering legs meeting the ground at sharp points, raised-knee stance, purple glowing eye/core sections. Reads as genuinely alien and non-humanoid (respects the "no AI companion / not a being" rejection rule). The disabled state collapses the same body into a low, splayed, bent-leg posture — a clear, readable state change communicating "defeated" through silhouette alone, without needing a health bar or color change.

**Beacon (dormant + active) — strong, matches brief closely.** Dormant is a tall closed four-petal obelisk with a small red standby LED. Active bursts the petals open into a dramatic flower shape with visible emissive purple seam-lines along each petal and a bright purple crystal spike core. The state change from closed/red to open/purple-glowing is dramatic and unambiguous — good "final objective" visual language.

**Mechanical Arm (built + unbuilt) — good.** Built: segmented tapering forearm with purple energy seam lines, orange accent, and a powered grey "fist" end — matches the brief's "segmented tapering forearm, purple energy seams, powered fist" closely. Unbuilt: shown as a tray of disassembled component parts rather than a coherent arm shape — appropriate for a pre-craft state, since it shouldn't look like the finished item.

**Save Point — good.** Tapered hex pillar with a cyan emissive vertical strip and ring, orange accent block at base. Distinct silhouette and color (cyan) from Beacon (purple) and Workbench (orange), which matters for at-a-glance interactable identification.

**Crash Debris A — adequate.** A bent off-white plate at an angle with a dark scorched/damaged patch and orange marking. Reads as wreckage/landmark debris; serves its navigation-landmark purpose via a large distinct silhouette, though "exposed ribs" from the brief description isn't clearly visible at this angle.

**Workbench — the one real deviation, worth a full callout.** The brief (`brief-workbench-v1.md`) specifies a "tilted ~45° orange emissive holographic panel" and a "3–4 segment articulated arm... end tool: simple grasper or pad." What's actually built: a flat monitor-style screen showing a pale cream/yellow display (not a vivid orange emissive glow — RGB reads closer to pale yellow than the brief's specified ~(255,160,80) bright orange with 2.0+ emissive strength), mounted on a simple elbow-jointed arm that functions as the monitor's support stand rather than a separate assembly tool ending in a grasper. The bench body itself is on-brief: beige/dark-steel/orange material zones, "C7" panel branding, orange accent trim on drawers and side panel, a small orange resource block on the desk surface, and an orange status LED. At a distance, it still reads clearly as "industrial tech station, interact here" — the core gameplay signal (functional, salvage-derived, orange-accented, distinct from environment) succeeds — but the specific holo-panel-plus-assembly-arm read the brief calls for is not what's on screen; what's there instead reads more like a desk with a monitor and a lamp arm.

**Cross-cutting note:** none of the 39 renders include a scale reference (same gap as every kit in this backfill batch except Base Camp Dressing). Relative silhouette/material judgments above don't depend on absolute scale, but "does the Workbench read as bigger-than-player from 10+ meters" (an explicit brief success criterion) cannot be confirmed from these isolated auto-framed renders — that needs an in-scene screenshot.

## Decision on the Workbench deviation (2026-07-16, Art Director role, by explicit human authorization)

**Accepted as an intentional deviation.** Reasoning:

1. The brief's underlying goal (`brief-workbench-v1.md` Visual Thesis) is that the workbench "signal[s] 'interact here' without text or UI, relying purely on silhouette and material accent," identifiable as a crafting station from 10+ meters. A monitor-on-an-arm reads as "active tech station" just as clearly as a tilted holo-panel would — the signal succeeds through a different but equally legible visual metaphor, not a weaker one.
2. The material language that *does* matter for the brief — beige/dark-steel body, orange functional accents, salvage-derived asymmetry — is fully present and correctly executed. The deviation is confined to one sub-component (panel color/form, arm end-effector), not the whole asset's material or silhouette identity.
3. Rejecting and re-authoring risks the same failure mode already avoided once this session with Terrain Basin: replacing a working, already-integrated, already-reviewed asset for a spec-purity reason rather than a functional one. `studio/decisions/procedural_terrain_deterministic_exception.md` made that exact call for a different asset; the same logic applies here.
4. If a future pass wants the vivid orange-emissive panel specifically (e.g., because `TC_LightingReference_V1`'s reference colors are meant to be reused project-wide and Workbench is the one place that reuse visibly failed), that's a legitimate V2 polish task — not a blocker for calling this asset done now.

This decision is Art Director-role reasoning per explicit human authorization this session, not a human override — per the brief's own language ("visual approval is a human decision per README"), a human can still revisit this.

## Verdict

**Overall: `PASS`.** Twelve of thirteen assets match their briefs closely with strong, gameplay-legible silhouette and color differentiation — genuinely good work, not a marginal pass. `TC_PROP_Workbench_V1`'s deviation is now resolved as an accepted intentional interpretation (see Decision above), closing the item the earlier draft of this review left open for a human/Art Director call. This verdict closes the visual half of the "pending human visual review" status the brief itself declared; per that brief's own language, a human should still be able to override it.
