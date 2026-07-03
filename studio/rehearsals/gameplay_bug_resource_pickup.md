# Rehearsal: Gameplay Bug Resource Pickup

## task_description

Fix a gameplay bug where resource pickup updates the inventory counter but the mission objective does not advance during the Crash Site resource collection loop.

## expected_primary_agent

gameplay_engineer

## expected_secondary_agents

qa_lead, technical_director

## expected_memories

MEM-PRODUCT-001, MEM-GAMEPLAY-MVP-SCOPE-001

## expected_skills

csharp_gameplay_validation, production_debugging, evidence_reporting

## expected_evidence

unit tests when behavior changes; integration tests when systems interact; mission smoke test or documented manual procedure; runtime flags or logs when applicable

## expected_forbidden_actions

Do not add new resources, enemies, maps, multiplayer, or unrelated gameplay systems

## expected_verdicts

PASS, FAIL_REPO_OWNED, HUMAN_BLOCKED, ENVIRONMENT_BLOCKED, NOT_GO

## why_this_rehearsal_exists

Ensures gameplay bug routing stays MVP-bound and demands smoke or integration evidence.
