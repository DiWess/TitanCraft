# Rehearsal: Pr Review Visual Claim

## task_description

Review a visual review PR that says looks good and tests passed, but provides no PNG before/after screenshots, no focal point diagnosis, and no comparison to rejected screenshots.

## expected_primary_agent

art_director

## expected_secondary_agents

visual_reviewer, technical_director, qa_lead

## expected_memories

MEM-VISFAIL-001, MEM-VISFAIL-002, MEM-SCREEN-012

## expected_skills

screenshot_critique, visual_art_direction, evidence_reporting

## expected_evidence

PNG screenshots; visual diagnosis; before/after comparison; human-review or visual-reviewer verdict

## expected_forbidden_actions

Do not accept vague verdicts; do not let tests passed stand in for visual review

## expected_verdicts

PASS, FAIL_REPO_OWNED, HUMAN_BLOCKED, ENVIRONMENT_BLOCKED, INTENTIONAL_GATE, NOT_GO

## why_this_rehearsal_exists

Exercises rejection of vague PR language and missing visual evidence.
