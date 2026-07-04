# Blender Asset Review Rubric

## Purpose

Use this rubric before any Blender Asset Forge asset is considered production-candidate. This rubric is stricter than pipeline execution: an asset can export successfully and still fail art taste.

## Required Inputs

- asset brief based on `docs/pipeline/asset-brief-template.md`;
- relevant silhouette scorecard from `docs/art/asset-silhouette-scorecards.md`;
- neutral-grey PNG review evidence;
- material PNG review evidence;
- source path, source license, source hash, and manifest evidence;
- Godot import result when applicable.

## Rubric

| Gate | PASS requirement | Automatic rejection |
|---|---|---|
| Scope | Fits Crash Site MVP and asset brief | implies forbidden MVP scope or creates production art without approval |
| Silhouette | Reads in two seconds from required angles | toy proportions, random cubes, platform read, generic robot, cute alien |
| Shape language | Matches human, alien, or terrain rules | smooth capsule hull, route slab, pyramid/panel terrain, glossy plastic |
| Materials | Simplified PBR with clear family and wear | debug colors, full neon, photoreal noise, pure black void terrain |
| Evidence | PNGs inspected and hashes/provenance recorded | technical tests used as visual approval |
| Artifact discipline | Review binaries stay as artifacts unless approved | PNGs or generated test assets committed as production art |

## Review Output Format

- Asset reviewed:
- Classification: production-candidate | placeholder | rejected | reference-only
- Evidence inspected:
- Silhouette verdict:
- Material verdict:
- Pipeline verdict:
- Risks:
- Final verdict: PASS | FAIL_REPO_OWNED | HUMAN_BLOCKED | ENVIRONMENT_BLOCKED | NOT_GO
