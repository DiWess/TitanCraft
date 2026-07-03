# Skill: Review Workflow

## purpose

Run the Review Workflow with concrete evidence and fail-closed verdicts.

## when_to_use

Use when a task routes to `review_workflow` or when validating Agent Studio structure.

## required_inputs

- Task objective.
- Relevant files.
- Applicable indexes.
- Evidence artifacts.

## procedure

1. Read README and AGENTS constraints.
2. Load routed memories and agent contracts.
3. Inspect actual files and artifacts.
4. Run applicable checks or identify why they are not applicable.
5. Report missing evidence before any approval.

## automatic_failures

- Missing required evidence.
- Vague verdict language.
- Scope expansion.

## output_format

- Scope:
- Evidence:
- Findings:
- Verdict:

## evidence_required

- Exact file paths and commands.
- Artifact paths for screenshots or assets when relevant.

## example_good_output

Evidence inspected: README, AGENTS, changed files, and command output. Verdict: `PASS` for docs-only validation.

## example_bad_output

Done, looks good, tests passed.
