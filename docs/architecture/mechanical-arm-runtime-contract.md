# Mechanical arm runtime contract

## Actual state

The arm is both a crafted gameplay ability and a first-person visual. Gameplay availability is stored in `FirstPersonController.Inventory.IsMechanicalArmBuilt`; `Workbench.Interact()` crafts the arm and advances the mission. Attacks are controlled by `FirstPersonController.TryAttack()` and `MechanicalArmAttackLogic`; the attack ray starts at `Head/Camera3D` and only damages a living `GalaxabrainScout` collider. Current scene hierarchy is:

```text
Player (CharacterBody3D)
└── Head (Node3D, pitch pivot)
    └── Camera3D (active camera)
        └── MechanicalArmVisual (MeshInstance3D view model)
```

## Invariants

- Only the arm view model may be camera-relative.
- A complete mech body must not be used as the first-person arm.
- The arm visual must have no collision and must not intercept interaction/attack raycasts.
- Arm gameplay remains unavailable until crafting succeeds; attack feedback must block pre-craft attacks.
- View-model bounds must remain compact (documented target: each transformed dimension below 1.5m and origin offset below 1m) unless a future explicit viewmodel renderer changes this contract.
