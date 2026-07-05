# ADR: Phase 8–9 Art Documentation Authority

**Date:** 2026-07-05  
**Status:** APPROVED  
**Owner:** Art Director  
**Reviewed by:** Claude Code (Architecture Validator)

## Decision

The **Art Director agent** is authorized to own and complete Phases 8 & 9 art documentation (brief + execution guide) once Phase 7 closes without escalation to human review, subject to the constraints below.

## Rationale

- Phases 1–7 establish the visual vocabulary, composition rules, and deliverable format.
- Phase 8–9 follow the same pattern and constraints.
- Art Director has demonstrated mastery of the brief/guide structure.
- Unnecessary human review between phases slows iteration.

## Authority Scope

Art Director **may:**

1. Draft Phase 8 brief once Phase 7 execution guide is approved.
2. Draft Phase 8 execution guide following the Phase 7 template.
3. Draft Phase 9 brief and execution guide once Phase 8 closes.
4. Reference existing visual identity, composition rules, and asset provenance records.

Art Director **must not:**

1. Expand MVP scope beyond `README.md` boundaries.
2. Introduce new asset types without Technical Director approval.
3. Reference unavailable assets in execution guides.
4. Approve their own work; another agent or human must sign off.

## Validation Gates

Before marking phase complete:

1. **Scope check:** All scene elements are in `README.md` § MVP or explicitly planned for.
2. **Dependency check:** All referenced assets exist or are scheduled.
3. **Clarity check:** Execution guide is step-by-step reproducible by another artist/developer.
4. **Architectural check:** Briefs align with existing materials, lighting, and silhouette language.

## Approval Workflow

1. Art Director drafts Phase 8 brief → Claude Code or Technical Director reviews for scope/architecture.
2. If approved, Art Director drafts Phase 8 execution guide.
3. Claude Code validates guide clarity, scope, and asset availability.
4. If approved, Phase 8 closes and Phase 9 planning begins.
5. Same workflow for Phase 9.

## Escalation

Escalate immediately if:

- New asset type required (e.g., new material, new mesh category).
- Scope boundary conflict (e.g., new enemy type, new mechanic).
- Asset unavailable or generation failed.
- Composition conflicts with existing phases.

Escalate to: **Technical Director** (architecture) or **Producer** (scope/schedule).

## Contingency

If Phase 8 or 9 work reveals scope or architectural issues, Art Director:

1. Documents the issue in a decision record.
2. Escalates to Technical Director + Producer.
3. Does not proceed until resolved.

---

**Approved by:** Claude Code (Code Reviewer & Architecture Validator)  
**Next step:** Upon Phase 7 completion, Art Director may proceed with Phase 8 planning.
