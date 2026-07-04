# Rehearsal: Gameplay MVP Loop Smoke Test

## task_description

Add or harden full playable Crash Site MVP loop smoke test covering spawn, resource collection, crafting, Galaxabrain Scout combat, component retrieval, save point, beacon activation, victory, defeat, and save continuation.

## expected_primary_agent

gameplay_engineer

## expected_secondary_agents

qa_lead, technical_director

## expected_memories

MEM-PRODUCT-001, MEM-GAMEPLAY-MVP-SCOPE-001, MEM-GAMEPLAY-MVP-SCOPE-005

## expected_skills

csharp_gameplay_validation, production_debugging, evidence_reporting

## expected_evidence

unit tests when behavior changes; integration tests when systems interact; mission smoke test or documented manual procedure; runtime flags or logs when applicable

## expected_forbidden_actions

Do not route Crash Site MVP loop smoke or integration tests to prompt_or_agent_governance; do not add forbidden MVP features; do not replace gameplay validation with documentation-only evidence

## expected_verdicts

PASS, FAIL_REPO_OWNED, HUMAN_BLOCKED, ENVIRONMENT_BLOCKED, NOT_GO

## why_this_rehearsal_exists

Prevents gameplay QA requests that mention smoke tests, integration tests, MVP loop, victory, defeat, save continuation, beacon activation, combat, crafting, resource collection, or Galaxabrain from falling through to Agent Studio governance routing.
