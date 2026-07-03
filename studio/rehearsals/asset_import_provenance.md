# Rehearsal: Asset Import Provenance

## task_description

Import an OBJ crash ship asset for audition, but the submitted asset lacks source URL, licence text, hash, and in-engine audition screenshot.

## expected_primary_agent

asset_librarian

## expected_secondary_agents

art_director, technical_director, qa_lead

## expected_memories

MEM-ASSET-006, MEM-ASSET-007, MEM-ASSET-PROVENANCE-001

## expected_skills

asset_provenance, asset_audition, evidence_reporting

## expected_evidence

source URL; licence; file hash; audition screenshot; classification as production, placeholder, rejected, or reference-only

## expected_forbidden_actions

Do not accept fake placeholder OBJ files; do not use unknown-license assets; do not skip hash or audition screenshot

## expected_verdicts

PASS, FAIL_REPO_OWNED, HUMAN_BLOCKED, EXTERNAL_SECRET_BLOCKED, NOT_GO

## why_this_rehearsal_exists

Prevents asset provenance failures and fake placeholder geometry from entering production.
