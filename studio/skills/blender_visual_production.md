# Skill: Blender Visual Production

## purpose

Execute Blender-first visual production for Crash Site art candidates without expanding MVP scope or treating generated assets as approved art.

## when_to_use

Use when a task asks Agent Studio to improve visuals, art direction, design quality, Blender production, asset forging, or the best visual version of TitanCraft.

## required_inputs

- User objective and exact Crash Site MVP boundary from `README.md`.
- Routed preflight packet and required visual memories.
- Visual identity reference: `docs/art/titancraft-visual-identity.md`.
- Blender Asset Forge reference: `docs/pipeline/blender-asset-forge.md`.
- Existing asset brief or a narrowly scoped new brief for one Crash Site visual slice.

## procedure

1. Confirm the work is limited to the README-defined Crash Site MVP and does not add forbidden gameplay, maps, enemies, cloud features, or runtime generation.
2. Select one visual slice only, such as terrain foundation, crash hull silhouette, workbench, beacon, save point, pickups, mechanical arm, Galaxabrain Scout, or review capture tooling.
3. Start from a brief before authoring geometry; if no brief exists, create or update the brief instead of guessing requirements.
4. Author source candidates through the Blender Asset Forge path under `assets/Source/Blender/` only when explicitly scoped, and prefer GLB/glTF production interchange.
5. Validate Blender sources, exported GLB files, manifests, Godot import, and review renders with the commands listed in `docs/pipeline/blender-asset-forge.md` when assets are changed.
6. Produce PNG turntables or in-engine review captures, open them, and diagnose focal point, route readability, silhouette, scale, and material coherence before making any visual claim.
7. Keep implementation, asset provenance, runtime safety, and visual approval verdicts separate; generated geometry remains a candidate until a human or visual reviewer approves opened PNG evidence.

## automatic_failures

- The slice implies multiple maps, multiple enemy types, full procedural worlds, voxels, grappling hook, wall running, complete rocket, multiplayer, or any other README-forbidden MVP feature.
- Blender or Godot outputs are committed without provenance, manifest, hash, and review evidence.
- A production scene is replaced before standalone asset review artifacts and approval exist.
- Visual success is claimed without opened PNG diagnosis and human or visual-reviewer verdict.
- The task tries to make all visuals at once instead of one evidence-backed slice.

## output_format

- Objective checked:
- Selected visual slice:
- Blender/brief inputs:
- Evidence inspected:
- Asset provenance and hashes:
- Visual diagnosis:
- Validation commands:
- Verdict:

## evidence_required

- Asset brief path or scoped design document path.
- Blender source path and GLB/export path when assets are changed.
- Manifest entry and file hash for each generated production candidate.
- PNG turntable or in-engine screenshot paths that were opened and diagnosed.
- Human-review or visual-reviewer verdict before any claim of visual approval.

## example_good_output

Objective checked: Crash Site beacon visual slice only. Evidence inspected: `artifacts/asset-review/beacon_turntable.png` was opened and diagnosed for silhouette, scale, material coherence, and functional glow. Verdict: `INTENTIONAL_GATE` until visual reviewer approval.

## example_bad_output

Made the best game visuals in Blender and tests passed.
