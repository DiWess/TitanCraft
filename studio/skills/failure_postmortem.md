# Skill: Failure Postmortem

## purpose

Execute a repeatable Failure Postmortem workflow that produces evidence, blockers, and an approved verdict without vague progress claims.

## when_to_use

Use when a task mentions `failure postmortem` or when routing indexes require this workflow.

## required_inputs

- User objective and forbidden scope.
- Relevant README and AGENTS sections.
- Changed files or artifact paths.
- Required memories from `studio/indexes/memory_routing.yml`.

## procedure

1. Confirm the task is allowed by `README.md`.
2. Load routed memory cards and name them in notes.
3. Inspect the actual files or artifacts; for visual work, open PNGs before judging.
4. Run or request the minimum applicable command evidence.
5. Separate runtime, visual, asset, and release verdicts instead of merging them.
6. Return blockers first, then fixes, then final verdict.

## automatic_failures

- Missing required input artifact.
- Vague verdict such as done, improved, looks good, should be fine, or tests passed.
- Evidence claims without exact command, file, screenshot, URL, hash, or review note.
- Scope expansion beyond Crash Site MVP.

## output_format

- Objective checked:
- Memories used:
- Evidence inspected:
- Findings:
- Required fixes:
- Verdict:

## evidence_required

- Exact file paths or artifact paths.
- Exact commands and result when commands apply.
- Before/after screenshot paths for visual tasks.
- Source URL, license, and hash for asset tasks.

## example_good_output

Objective checked: visual scene composition. Evidence inspected: `artifacts/review/before.png` and `artifacts/review/after.png`; first focal point, route readability, silhouette, scale, and material coherence were named. Verdict: `NOT_GO` until route slab is replaced.

## example_bad_output

Looks good; tests passed; should be fine to merge.
