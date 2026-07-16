# Decision Packet — Handoff from Visual Reviewer Pass, 2026-07-16

**Filed by:** Claude Code (Code Reviewer & Architecture Validator)
**Filed for:** Art Director (3 items), Technical Director (1 item, routed here per `art_director.md`'s own escalation rule: "Escalate to the Technical Director for runtime or architecture risk" — this one is not a composition/material call)
**Source:** This session's Visual Reviewer pass (`docs/art/reviews/*.md`, commit `8774951`)
**Why a packet instead of leaving it in the review docs:** these four items are decisions and validations that require the named role's authority, not just evidence — Claude Code's role here is routing, not resolving them (`CLAUDE.md` §9: does not modify work owned by another agent).

---

## 1. `TC_ENV_DistantSilhouette_SmokePlume_V1` — rework or repurpose decision

**Owner:** Art Director
**Source:** `docs/art/reviews/distant-silhouettes-kit-v1-review.md`, verdict `NOT_GO`

**Finding:** the asset is a hard-edged, faceted, angular geometric stack — visually indistinguishable in material and construction technique from the kit's own `BasaltRidge` rock formation. Nothing about it reads as smoke, atmosphere, or particulate matter. It is currently integrated and live at `DistantRock_6` in `Main.tscn`.

**Decision needed (pick one):**
- (a) Rework the geometry/material to actually read as atmospheric — softer silhouette, tapering/billowing form, translucent or particulate-suggesting material, distinct from the kit's rock-formation language.
- (b) Repurpose it as a fourth rock-formation variant and rename it out of the "SmokePlume" identity, if an atmospheric asset isn't a current priority.
- (c) Something else the Art Director determines the brief actually needs.

**Required evidence before closing:** new or revised opened-PNG review following the standard template (`docs/art/reviews/heavy-crash-hull-v1-standalone-review.md` format), with a Visual Reviewer verdict recorded. Per `art_director.md`'s forbidden actions, Art Director may not self-approve — route the final verdict through an independent reviewer.

**Scope guard:** whatever is decided, this stays visual-only Stage A/B dressing — no gameplay, collision, or MVP scope change.

---

## 2. `TC_PROP_Workbench_V1` — brief-deviation decision

**Owner:** Art Director
**Source:** `docs/art/reviews/mvp-asset-pack-v1-review.md`, flagged (not failed) within an overall `PASS`

**Finding:** `docs/art/briefs/brief-workbench-v1.md` specifies a "tilted ~45° orange emissive holographic panel" (RGB ~(255,160,80), Emissive Strength 2.0+) and a "3–4 segment articulated arm... end tool: simple grasper or pad." What's built: a flat monitor-style screen showing a pale cream/yellow display (not the specified vivid orange emissive), mounted on a simple elbow-jointed arm that functions as the monitor's support stand rather than a separate assembly tool ending in a grasper. The rest of the asset (bench body, material zones, orange accent trim, "C7" branding) is on-brief. At distance the asset still functionally signals "interact here," which is why this wasn't scored `NOT_GO`.

**Decision needed (pick one):**
- (a) Accept as a valid interpretation of the brief — document why (e.g., "monitor + support arm" reads as functional tech and satisfies the gameplay signaling goal even without literally matching the holo-panel/grasper-arm description) and close the deviation as intentional.
- (b) Request a revision pass to bring the panel color/emissive and arm end-effector in line with the brief.

**Required evidence before closing:** if (b), a re-render and re-review following the same standard; if (a), a short note in `docs/art/reviews/mvp-asset-pack-v1-review.md` or a linked ADR recording the accepted-deviation decision, since `art_director.md` requires execution guides to be "free of scope creep" — an undocumented brief deviation should not just be silently accepted.

**Note:** `TC_LightingReference_V1`'s reference colors (already reviewed, `PASS`) are what the Workbench panel is supposed to reuse — the mismatch is on the Workbench side, not a defect in the lighting reference.

---

## 3. Base Camp Dressing Kit V1 — outstanding human/Art Director aesthetic sign-off

**Owner:** Art Director (human sign-off specifically, per the existing record's own language)
**Source:** `docs/art/reviews/base-camp-dressing-kit-v1-review.md` (promoted this session) and the original `docs/release/evidence/titancraft-base-camp-dressing-pass-2026-07-09.md`

**Finding:** this kit already has real evidence — production-scene opened-image diagnosis, provenance, hashes, full validation — and an agent-level `PASS`, independently corroborated this session. What has never happened is the explicit sign-off both documents call for: *"Human/Art Director aesthetic sign-off remains open... this record is agent evidence, not a human approval."* This is not a gap in evidence quality, it's a gap in who has actually looked and said yes.

**Decision needed:** Art Director (or a human) reviews the existing evidence and either signs off or requests changes. No new investigation should be needed — the evidence is already there.

**Secondary, lower-priority item also flagged in the same review:** the standalone per-asset review PNGs for this kit (distinct from the production-scene captures used in the 2026-07-09 diagnosis) have a camera-framing defect — subjects render tiny in frame, and `TC_ENV_CampAwning_V1/scale_reference.png` is nearly empty. Worth a re-render at some point so the standalone evidence set is independently useful, but this does not block the sign-off above since the production-scene captures don't have this problem.

---

## 4. `TC_ENV_RockOccluder_V1` — collision/navigation audit

**Owner:** Technical Director (not Art Director — see header note)
**Source:** `docs/art/reviews/rock-occluder-v1-review.md`, visual half `PASS`, technical half explicitly left `PENDING`

**Finding:** unlike every other asset in this backfill batch, this one carries real gameplay collision — three `StaticBody3D` instances (`VolcanicRock_1..3` in `Main.tscn`) each with a `Collision_BlockingRock` shape. The visual review is done; nobody has checked whether these three placements block the intended Crash Site route in a way level design intends, or accidentally wall off something.

**Decision needed:** Technical Director confirms (or requests changes to) the three collision placements against the intended route/navigation, per `technical_director.md`'s owned scope ("technical risk, architecture").

**Required evidence before closing:** in-engine navigation check or level-layout review. This explicitly cannot be fabricated from the static asset renders already reviewed.

---

## Verdict vocabulary reminder (both roles)

Per `AGENTS.md` §5 / each agent's `Approved Verdicts`: `PASS`, `FAIL_REPO_OWNED`, `HUMAN_BLOCKED`, `ENVIRONMENT_BLOCKED`, `INTENTIONAL_GATE`, `NOT_GO`. No vague verdicts ("looks good," "done," "should be fine").
