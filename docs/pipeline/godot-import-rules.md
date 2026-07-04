# Godot Import Rules for Blender Assets

## Format

Use GLB/glTF for production assets unless OBJ is explicitly justified in the asset brief. OBJ is acceptable only for narrow debug/prototype needs where material slots, node hierarchy, and metadata preservation are not required.

## Required Import Properties

Every imported art asset must preserve or prove:

- correct meter scale relative to Godot units;
- clean origin/pivot for placement;
- named mesh nodes using a stable `TC_` prefix;
- preserved material slots;
- no runtime mesh generation;
- no accidental collision resources;
- no scripts inside imported art;
- import thumbnails or preview renders;
- visual review PNGs before production placement;
- preserved source `.blend` for every generated production export, either from a reviewed GitHub Actions artifact bundle or a human-approved tracked source file.

## Collision Policy

Visual assets are collisionless by default. If gameplay collision is required, it must be authored as a separate gameplay-owned resource with explicit approval from the gameplay owner. Imported art must not silently generate or carry collision into the playable scene.

## Godot Validation Command

After exporting GLB assets, run:

```bash
godot --headless --path . --import
```

Any import warning that changes scale, material slots, node names, or collision state must be resolved before the asset can be promoted from pipeline-test or placeholder status.
