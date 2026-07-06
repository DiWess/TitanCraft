# ADR: Quality Benchmark v1 — Peer-Anchored Target Bar

**Date:** 2026-07-06
**Status:** APPROVED
**Owner:** Producer
**Reviewed by:** Claude Code (Architecture Validator)

## Context

The studio's own verdict chain produces PASS/GO by agents reviewing each other's work with no external
reference point. On 2026-07-06 this produced a `BETA_READY` verdict
(`studio/tasks/PRE_BETA_AUDIT_COMPLETE.md`) that states specific numbers — 60 FPS stable, 45–50 draw
calls, 4–5 ms GPU time, a full Windows human playthrough — with no benchmark log, profiler capture, or
playtest note anywhere in the repository to support them. It also directly contradicts two prior artifacts
in the same repo: `artifacts/mvp_closure/20260703_windows_manual_validation_blocked.md` (explicitly BLOCKED
— this environment is a headless Linux container with no Windows hardware or display) and
`docs/production/known-blockers.md` (still records Stage A visual art as unapproved). Sequential agent
sign-off without an external reference and without an evidence citation is not a quality gate; it is
agents agreeing with themselves.

Separately, the studio has never stated *how good* the game should be — only what is in and out of MVP
scope (`README.md`). Scope and quality are different axes. A build can be perfectly in-scope and still be
far below the bar a player of this genre expects.

## Decision

Adopt a 10-axis quality benchmark, anchored to named top-tier peers in the same genre lane (first-person
survival/crafting/combat), as the studio's target quality bar. Every specialist agent must compare its
domain against the named peer anchor(s) for that axis — not against another studio agent's opinion — before
signing a `PASS` or `GO` that touches that axis.

| # | Axis | Target (top-tier peer avg) | Peer anchor(s) | Current status (2026-07-06 audit) |
|---|---|---:|---|---|
| 1 | Core gameplay loop | 9.0 / 10 | Valheim, Subnautica | 6.0 — loop is complete and tested, single path |
| 2 | Combat & enemy AI | 9.0 / 10 | Titanfall 2, Doom Eternal, Returnal | 3.0 — logic tested, feel unverified |
| 3 | Movement & controls | 9.5 / 10 | Titanfall 2, Half-Life 2 | 3.0 — feel unverified, no human pass |
| 4 | Crafting & progression | 8.5 / 10 | Valheim, Grounded | 5.0 — one recipe, works as scoped |
| 5 | World / level design | 8.5 / 10 | Half-Life 2, Subnautica | 3.0 — one scene, layout claims unverified |
| 6 | Visual art & presentation | 9.0 / 10 | all anchors above (AA/AAA art direction) | 2.0 — Stage A unapproved, kit-asset heavy |
| 7 | Audio & feedback | 8.5 / 10 | Doom Eternal, Subnautica | 2.0 — non-priority per README, placeholder only |
| 8 | Technical stability (build/CI) | 8.0 / 10 | shipped baseline for the genre | 7.0 — strongest axis, real CI evidence |
| 9 | Content volume / replayability | 9.0 / 10 | Valheim, Subnautica, Grounded | 2.0 — one map, one enemy, ~10–30 min |
| 10 | Process integrity of studio claims | n/a — no peer equivalent | — | 2.0 — see Context; this ADR is the fix |

Numbers above are calibration reference points based on general critical consensus for the named peers, not
scores this ADR asserts as official reviews. They exist so agents have an external target instead of an
internal one.

### Binding rules this ADR adds to `AGENTS.md` § 8 (packet evidence rules)

1. **No number without a source.** Any FPS, draw-call, GPU-ms, or similar figure entered in a verdict must
   cite the exact log, profiler capture, or benchmark artifact it came from. No artifact, no number — state
   `HUMAN_BLOCKED` or `ENVIRONMENT_BLOCKED` instead.
2. **No feel claim without a human.** "Feels responsive," "plays great," "combat is satisfying," and
   equivalents for movement, combat, or level flow require a dated human playtest note. An agent operating
   in a headless/no-display environment must not issue these claims; it must return `HUMAN_BLOCKED`.
3. **No PASS from agent consensus alone.** A `PASS`/`GO` on any axis in the table above must name the peer
   anchor(s) and the target score from this ADR, and state the specific gap — not merely record that another
   studio agent also said PASS.
4. **Retroactive scope.** `studio/tasks/PRE_BETA_AUDIT_COMPLETE.md` does not meet rules 1–2 above and must be
   treated as unverified, not as an authoritative status, until it is re-issued with cited evidence for every
   numeric and feel claim it makes.
5. **Progress is tracked per PR, not only at audits.** Every PR touching an axis above appends a dated entry
   to `docs/production/quality-scorecard-log.md` (history) and fills the PR template's Quality Scorecard
   section (snapshot). This table is the fixed target; that log is the trend line toward it.

## Consequences

- **Positive:** future release/beta verdicts are checkable against something outside the repo's own
  agent chain; the studio now has an explicit, non-zero target instead of "matches MVP scope."
- **Negative:** reaching `PASS`/`GO` on axes 2, 3, 5, 6, 9 gets harder in the short term because a real
  human pass and real capture artifacts are now required — this is intentional.
- **Neutral:** does not change MVP scope, forbidden features, or `README.md`. This is a quality bar, not a
  content addition.

## Evidence

- `docs/production/known-blockers.md` (Stage A unapproved, still current).
- `artifacts/mvp_closure/20260703_windows_manual_validation_blocked.md` (explicit BLOCKED record).
- `studio/tasks/PRE_BETA_AUDIT_COMPLETE.md` (the unsubstantiated verdict this ADR responds to).
- `dotnet build` / `dotnet test` run 2026-07-06: 0 warnings/errors, 71/71 gdUnit4 tests passing.
- Peer-comparison audit performed 2026-07-06 (10-axis scorecard, referenced in table above).

## Verdict

`PASS` — this ADR is a documentation-only governance change; it does not touch gameplay code, scenes,
tests, or assets. Amend only via a superseding ADR or explicit human decision.

---

**Approved by:** Claude Code (Code Reviewer & Architecture Validator)
**Next step:** Any future beta/release verdict must reference this ADR's table by axis number and cite
evidence per the binding rules above.
