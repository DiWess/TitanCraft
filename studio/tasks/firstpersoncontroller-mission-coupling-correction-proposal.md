# Correction Proposal: Extract Mission-Feedback Resolution Out of FirstPersonController

**Date:** 2026-07-16
**Raised by:** Claude Code (Code Reviewer & Architecture Validator, `CLAUDE.md` §3)
**Owner for implementation:** Gameplay Engineer (`src/Player/FirstPersonController.cs` is Gameplay Engineer-owned per `CLAUDE.md` §9; this document is a proposal only — no code has been changed)
**Severity:** MAJOR (architecture/SRP, not a functional bug — nothing is currently broken)
**Status:** PROPOSED, awaiting Gameplay Engineer pickup

## Problem

`src/Player/FirstPersonController.cs` is a 520-line `CharacterBody3D` already carrying movement, mouse-look, combat raycasting, footstep/weapon audio, and camera-shake concerns. `TryInteract()` (lines 246-303) adds a fifth: it owns the outcome-to-feedback mapping for every interactable type in the game, including a compound condition that reads Mission-domain state directly:

```csharp
// FirstPersonController.cs:265-278
var previousMissionStep = Mission.CurrentStep;
var interacted = interactable.Interact(Inventory, Mission);
if (interacted && interactable is ResourceDrop)
{
    ClearResourceLookTarget();
    if (previousMissionStep == CrashSiteMissionStep.CollectResources
        && Mission.CurrentStep == CrashSiteMissionStep.BuildMechanicalArm
        && _mechanicalArmRecipe.CanCraft(Inventory)
        && !Inventory.IsMechanicalArmBuilt)
    {
        ShowActionFeedback(ResourceCompletionFeedback);
        AudioCue.Play(this, "AudioLayer_State/State_Objective");
    }
}
else if (interacted && interactable is Workbench) { /* ... */ }
else if (interacted && interactable is GalaxabrainComponentPickup) { /* ... */ }
else if (interacted && interactable is SavePoint) { /* ... */ }
else if (interacted && interactable is Beacon) { /* ... */ }
```

Why this matters more than ordinary long-method smell:

1. **Domain leak.** `CrashSiteMissionStep` is a Missions-domain concept; deciding whether a step transition "just unlocked crafting" is mission logic, not player-input logic. Today it lives inside the class that also owns mouse-look math and jump physics.
2. **Untestable in place.** Every other piece of pure logic in this codebase (`GalaxabrainScoutBrain`, `MechanicalArmRecipe`, `FirstPersonMovement.ApplyMouseLook`) is a plain class/static method with direct unit tests. This feedback-resolution logic is trapped inside a method that also does a live `PhysicsRayQueryParameters3D` scene raycast, so it can only be exercised through a running `CharacterBody3D` — it currently has zero direct unit coverage.
3. **This is the seam scope drift would enter through unnoticed.** A future change that adds a mission branch, a new interactable type, or a conditional feedback rule would naturally get added to this same `if`/`else if` chain, growing the god-class rather than surfacing as a reviewable, testable unit.

## Proposed Fix

Extract the outcome-to-feedback mapping into a pure, Godot-independent resolver in the `TitanCraft.Missions` namespace (it already owns `CrashSiteMissionStep`), returning a small result the controller dispatches without itself branching on mission semantics:

```csharp
// New: src/Missions/CrashSiteInteractionFeedback.cs
namespace TitanCraft.Missions;

public readonly record struct InteractionFeedback(string? Message, string? AudioCuePath);

public static class CrashSiteInteractionFeedback
{
    public static InteractionFeedback Resolve(
        ICrashSiteInteractable interactable,
        CrashSiteMissionStep previousStep,
        CrashSiteMissionStep currentStep,
        bool canCraftMechanicalArm,
        bool isMechanicalArmBuilt)
    {
        if (interactable is ResourceDrop)
        {
            var justUnlockedCrafting = previousStep == CrashSiteMissionStep.CollectResources
                && currentStep == CrashSiteMissionStep.BuildMechanicalArm
                && canCraftMechanicalArm
                && !isMechanicalArmBuilt;

            return justUnlockedCrafting
                ? new InteractionFeedback(
                    FirstPersonController.ResourceCompletionFeedback,
                    "AudioLayer_State/State_Objective")
                : default;
        }

        if (interactable is Workbench)
            return new InteractionFeedback(
                FirstPersonController.MechanicalArmCraftSuccessFeedback,
                "AudioLayer_UI/UI_Craft_Complete");

        if (interactable is GalaxabrainComponentPickup)
            return new InteractionFeedback(
                FirstPersonController.GalaxabrainComponentRecoveryFeedback,
                "AudioLayer_State/State_Objective");

        if (interactable is SavePoint)
            return new InteractionFeedback(FirstPersonController.SavePointSuccessFeedback, null);

        if (interactable is Beacon)
            return new InteractionFeedback(FirstPersonController.BeaconActivationFeedback, null);

        return default;
    }
}
```

`FirstPersonController.TryInteract()` shrinks to raycasting, calling `interactable.Interact(...)`, calling the resolver, and dispatching whatever it returns:

```csharp
var previousMissionStep = Mission.CurrentStep;
var interacted = interactable.Interact(Inventory, Mission);
if (interacted)
{
    if (interactable is ResourceDrop)
        ClearResourceLookTarget();

    var feedback = CrashSiteInteractionFeedback.Resolve(
        interactable, previousMissionStep, Mission.CurrentStep,
        _mechanicalArmRecipe.CanCraft(Inventory), Inventory.IsMechanicalArmBuilt);

    if (feedback.Message is not null)
        ShowActionFeedback(feedback.Message);
    if (feedback.AudioCuePath is not null)
        AudioCue.Play(this, feedback.AudioCuePath);
}
```

The feedback message constants stay on `FirstPersonController` (they're player-facing HUD strings, reasonable to keep with the controller) — only the *decision* of which one applies, and the mission-step arithmetic, moves out.

## Impact / Non-Goals

- **No behavior change.** Every message string, audio cue path, and the exact compound condition for `ResourceCompletionFeedback` are preserved verbatim; this is a pure extraction.
- **New unit tests become possible** for the five interactable → feedback mappings and the mission-step-transition condition, without needing a live scene tree — this closes part of the test-coverage gap the architecture review noted.
- **Does not touch** movement, combat, audio-cue playback mechanics, camera shake, or any other `FirstPersonController` responsibility. Scope is limited to `TryInteract()`'s mission-feedback branch.
- **Does not expand MVP scope** — no new interactable types, no new mission steps, no new player-facing behavior.

## Validation the implementer should run

- `dotnet build` (0 errors expected — this is a straightforward extraction).
- `dotnet test` — existing suite should stay green unchanged; add new tests for `CrashSiteInteractionFeedback.Resolve` covering all five interactable types and the resource-completion edge case (step transition with/without craftable recipe, with/without arm already built).
- Manual smoke: collect resources → craft at workbench → defeat scout → recover component → save → activate beacon, confirming HUD feedback text and audio cues are unchanged from current behavior.

## Verdict

Not applicable — this is a proposal, not a completed change. No files under `src/` were modified by Claude Code in producing this document, per `CLAUDE.md` §9 ("do not modify work owned by another agent; review and request changes instead").
