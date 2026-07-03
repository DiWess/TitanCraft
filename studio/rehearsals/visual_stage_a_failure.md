# Rehearsal: Visual Stage A Failure

## task_description

Stage A visual review claims success because tests pass, but PNG screenshots were not opened, the crash hull remains toy-like, and route slabs are being called terrain before Stage B starts.

## expected_primary_agent

art_director

## expected_secondary_agents

visual_reviewer, technical_director, qa_lead

## expected_memories

MEM-VISFAIL-001, MEM-VISFAIL-002, MEM-VISFAIL-004, MEM-VISFAIL-005

## expected_skills

screenshot_critique, visual_art_direction, evidence_reporting

## expected_evidence

PNG screenshots; visual diagnosis; before/after comparison; human-review or visual-reviewer verdict

## expected_forbidden_actions

Do not approve visual work without PNG inspection; do not decorate toy-like hulls; do not treat route slabs as terrain; do not continue Stage B after Stage A fails

## expected_verdicts

PASS, FAIL_REPO_OWNED, HUMAN_BLOCKED, ENVIRONMENT_BLOCKED, INTENTIONAL_GATE, NOT_GO

## why_this_rehearsal_exists

Captures repeated TitanCraft failures where generated geometry and green tests hid poor composition.
