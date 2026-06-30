# Phase 3A Asset Requirements

Owner: Codex
Version: 1
Date: 2026-06-30
Review status: READY_FOR_ASSET_SELECTION

## Purpose

Phase 3A implementation work is terminated until a human selects a stronger asset direction. The primitive kit and the currently imported Kenney subset both reached `NOT_GO`; future visible work must keep the mandatory loop: edit → render → capture → inspect → score → correct.

This document defines the art-acquisition target only. It authorizes no purchase, download, import, scene integration, gameplay change, Phase 3B work, or production-scene visual composition.

## Global acceptance constraints

- Assets must support TitanCraft's README direction: simplified realistic science fiction with readable polygonal forms.
- License must allow commercial use and modification; attribution and redistribution limits must be recorded before import.
- Godot 4 integration must be feasible through native files, FBX/glTF/OBJ import, or a Blender conversion step.
- Assets must be readable from first-person gameplay distance, not just in store renders.
- Packs that require heavy shader stacks, photogrammetry pipelines, or dense PBR realism are lower priority unless they include low-poly/stylized variants.
- New assets must be selected by a human before download, purchase, import, or integration.

## A. Volcanic environment kit

Must include:

- Irregular terrain pieces.
- Cliffs.
- Basalt formations.
- Slopes.
- Crater or impact geometry.
- Foreground rocks.
- Background ridges.
- Natural transitions between pieces.
- First-person scale suitability.

Reject packs dominated by:

- Cubes, grid blocks, voxel forms, clean grass landscapes, bright cartoon geology, or photorealistic scanned assets that conflict with the stylized target.

## B. Modular industrial sci-fi kit

Must include:

- Angled wall modules.
- Hull panels.
- Frames.
- Doors.
- Floors.
- Structural supports.
- Maintenance details.
- Damaged variants.
- Machinery.
- Cables or pipes.
- Modular snapping.

Reject packs dominated by:

- Straight rectangular corridors only, pristine white-space-station interiors, cyberpunk neon, toy proportions, or high-detail military realism.

## C. Wreckage and ship components

Must include:

- Damaged hull fragments.
- Broken engines.
- Torn panels.
- Exposed internal structure.
- Asymmetric pieces.
- Wreckage suitable for partial burial.

A complete intact spaceship alone is not sufficient.

## D. Industrial props and interactables

Must support:

- Workbench.
- Save/data station.
- Communications beacon.
- Resource containers.
- Electronic modules.
- Salvage components.

Objects must be readable by silhouette from first-person gameplay distance, not only by material color.

## E. Biomechanical enemy kit

Must support:

- Non-humanoid silhouette.
- Mechanical limbs.
- Organic or synthetic core.
- Sensor elements.
- Asymmetric armor.
- Animation-ready or separable components.

A generic humanoid robot pack is insufficient.

## F. First-person mechanical arm

Must support:

- First-person framing.
- Multiple joints.
- Pistons.
- Articulated plates.
- Attack animation potential.
- Visual compatibility with industrial human technology.

## Replacement priorities

1. Volcanic terrain and crash framing.
2. Modular industrial/wreckage kit for the central ship and human structures.
3. Workbench/save/beacon/resource prop silhouettes.
4. Enemy silhouette only if a suitable biomechanical kit is selected.
5. First-person arm only when the environment and hero interaction vocabulary are coherent.

## Stop condition for future implementation

If selected assets cannot produce at least 48/70 in the rendered Phase 3A review, stop and request human art direction before adding additional packs.
