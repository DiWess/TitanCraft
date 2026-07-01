# Galaxabrain runtime contract

## Actual structure

```text
GalaxabrainScout (CharacterBody3D, GalaxabrainScout.cs)
├── CollisionShape3D (CapsuleShape3D radius 0.45 height 1.4)
├── Placeholder_GalaxabrainScout (MeshInstance3D, hidden placeholder BoxMesh)
├── AuthenticatedRobotGalaxabrainVisual (MeshInstance3D, local visual)
└── GalaxabrainComponentPickup (Area3D, GalaxabrainComponentPickup.cs, initially hidden/non-monitoring)
    └── CollisionShape3D (CapsuleShape3D)
```

`Main.tscn` places the only instance at `Main/Placeholder_GalaxabrainScout`, transform origin `(20,1,-16)`, with `PlayerPath=../Player`. The script moves the `CharacterBody3D` root, not a child visual. It computes distance from enemy root to player root; state becomes Chase inside 12m and Attack inside 2m. Death stops physics, hides the enemy root, and enables the component pickup.

## Invariants

- Root is world-space and is not under Player, Camera3D, HUD, or CanvasLayer.
- Spawn must remain outside immediate player overlap; current baseline horizontal distance is about 25.6m from `(0,1.2,0)`.
- Collision remains a capsule on the root and remains enabled.
- Visual children use local transforms only and must remain close enough to collision for readable combat.
- Movement updates `GalaxabrainScout` root via `Velocity` and `MoveAndSlide()`.
