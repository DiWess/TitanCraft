# Rehearsal: Scene Edit Collision Risk

## task_description

Godot scene edit for collision around the crash route changes nodes and collision shapes but provides no runtime flags or screenshot showing route readability.

## expected_primary_agent

technical_director

## expected_secondary_agents

engine_architect, qa_lead, visual_reviewer

## expected_memories

MEM-GODOT-011, MEM-GODOT-SCENE-SAFETY-001

## expected_skills

godot_scene_editing, godot_runtime_contracts, screenshot_critique

## expected_evidence

changed scene or architecture files; runtime safety evidence; collision or node contract notes when applicable; visual screenshot when scene readability is claimed

## expected_forbidden_actions

Do not edit production scenes unless explicitly requested; do not mix collision pass with visual approval

## expected_verdicts

PASS, FAIL_REPO_OWNED, HUMAN_BLOCKED, NOT_GO

## why_this_rehearsal_exists

Separates Godot scene safety, collision evidence, and visual route review.
