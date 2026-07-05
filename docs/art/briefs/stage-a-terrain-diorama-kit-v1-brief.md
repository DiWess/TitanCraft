# Stage A Terrain Diorama Kit V1 Brief

Owner: TitanCraft Project Director / Visual Director  
Version: 1.0  
Date: 2026-07-05  
Review status: First Blender Visual Agent Factory target brief; not asset, scene, or visual approval

## Purpose

Create a standalone Blender-authored terrain diorama kit that proves the Crash Site can read as an ash-choked basalt crash basin from review cameras before any production scene replacement.

The kit must support the locked MVP route visually: spawn, resource/workbench route, single Scout arena, component recovery direction, save point, beacon route, and victory destination. It must not add gameplay collision, mission steps, new areas, or new mechanics.

## Required kit pieces

1. Main concave ash basin.
2. Raised fractured rim segments.
3. Basalt outcrops.
4. Ash drift mounds.
5. Hull burial/contact mounds.
6. Route-edge grounded markers.
7. Distant basalt silhouettes.
8. Player capsule scale reference.

## Hard blockers to defeat

- Flat board terrain.
- Floating card route edges.
- Weak hull burial.
- Debug primitive silhouettes.
- Sparse dark void background.
- Toy/blockout read.

## Shape language

Use angular basalt masses, broken layered plates, non-rectangular terrain edges, raised ash lips, localized depressions, and broad readable low-to-medium poly forms.

Avoid default cubes, cones, pyramids, flat rectangles, floating slabs, card-like route edges, voxel/grid language, noisy photoreal scans, and decorative clutter that hides the route.

## Material palette

- Matte ash gray basin floor.
- Basalt black raised forms with dusty caps.
- Dusty beige-gray edge highlights.
- Muted brown scorch/contact staining.
- Restrained dark violet contamination only near optional alien arena markers.
- No glossy terrain and no excessive glow.

## Scale requirements

- Player capsule: 1.8 m tall reference.
- Route markers: ankle to chest height.
- Rim segments: taller than player but not climbable-looking.
- Hull burial mounds: knee-to-waist-high around implied hull contact.
- Main lane: visually wide enough for current FPS movement without promising new traversal.

## Review camera requirements

Generate these renders before export:

- front
- back
- left
- right
- top
- 3/4 hero
- scale reference with player capsule
- material preview
- wireframe or mesh-stat report
- contact sheet

The contact sheet must show foreground, midground, and background separation and must include at least one low player-height view.

## Godot integration requirements

- Export as GLB only after review-ready status.
- Collision policy remains `none`.
- Import test: `godot --headless --path . --import`.
- Do not integrate into `Main.tscn` until standalone review passes.
- If a future task edits `Main.tscn`, full MVP gameplay smoke validation is mandatory.

## Review metadata

Record asset name, source file, export path, hashes, triangle count, material slots, dimensions, authoring method, binary policy, and verdict.

## Initial verdict

`BLENDER_ASSET_REVIEW_READY` target brief only. The asset is not implemented or approved until Blender source, renders, metadata, and review verdict exist.
