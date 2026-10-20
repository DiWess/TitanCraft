using System;
using Godot;
using TitanCraft.Core;
using TitanCraft.Crafting;
using TitanCraft.Missions;
using TitanCraft.Resources;

namespace TitanCraft.World;

/// <summary>
/// The quarter's one fabrication point. It fits the arm's modules in order:
/// Mk I from scavenged salvage, Mk II around the Scout's component, Mk III
/// from what the Net-makers' Yard gives up. Each craft is only offered at the
/// mission step that asks for it, so the workbench cannot run the story ahead.
/// </summary>
public partial class Workbench : StaticBody3D, ICrashSiteInteractable, ILookHighlightTarget
{
    private readonly MechanicalArmRecipe _mechanicalArmRecipe = new();

    [Export] public NodePath CraftAudioPath { get; set; } = "CraftAudio";
    [Export] public NodePath ChassisPath { get; set; } = "VisualBase/WorkbenchChassis";
    [Export] public NodePath UnbuiltArmVisualPath { get; set; } = "VisualBase/V1BetaMechanicalArmUnbuiltVisualRoot";
    [Export] public Material? HighlightMaterial { get; set; }

    /// <summary>Raised with the module tier after a successful craft (the save coordinator checkpoints on it).</summary>
    public event Action<int>? Crafted;

    private MeshInstance3D? _chassis;
    private Material? _baseMaterial;
    private Node3D? _unbuiltArmVisual;

    public override void _Ready()
    {
        CollisionLayer = 1u << 1;
        CollisionMask = 1u;
        _chassis = GetNodeOrNull<MeshInstance3D>(ChassisPath);
        _baseMaterial = _chassis?.MaterialOverride;
        _unbuiltArmVisual = GetNodeOrNull<Node3D>(UnbuiltArmVisualPath);
        SetHighlighted(false);
    }

    /// <summary>The mission step at which a module tier may be crafted.</summary>
    public static CrashSiteMissionStep StepForTier(int tier) => tier switch
    {
        1 => CrashSiteMissionStep.BuildMechanicalArm,
        2 => CrashSiteMissionStep.UpgradeArmMk2,
        3 => CrashSiteMissionStep.UpgradeArmMk3,
        _ => throw new ArgumentOutOfRangeException(nameof(tier), tier, "Arm tiers run 1-3."),
    };

    public bool Interact(MvpInventory inventory, CrashSiteMissionState mission)
    {
        ArgumentNullException.ThrowIfNull(inventory);
        ArgumentNullException.ThrowIfNull(mission);

        var recipe = inventory.ArmLevel == 0 ? _mechanicalArmRecipe : MechanicalArmRecipe.NextFor(inventory);
        if (recipe is null || mission.CurrentStep != StepForTier(recipe.Tier))
        {
            return false;
        }

        if (!recipe.TryCraft(inventory))
        {
            return false;
        }

        var advanced = recipe.Tier switch
        {
            1 => mission.TryCompleteMechanicalArmConstruction(),
            2 => mission.TryCompleteMk2Upgrade(),
            _ => mission.TryCompleteMk3Upgrade(),
        };

        if (advanced)
        {
            AudioCue.Play(this, CraftAudioPath);
            RestoreArmState(true);
            Crafted?.Invoke(recipe.Tier);
        }

        return advanced;
    }

    /// <summary>
    /// Hides the unbuilt-parts hint model once the arm is built. The Interact()
    /// success path calls this directly; CrashSiteSaveCoordinator also calls it
    /// after a load so a save with the arm already built doesn't leave stray
    /// parts visible at the workbench before the player revisits it.
    /// </summary>
    public void RestoreArmState(bool isMechanicalArmBuilt)
    {
        if (_unbuiltArmVisual is not null)
        {
            _unbuiltArmVisual.Visible = !isMechanicalArmBuilt;
        }
    }

    public void SetHighlighted(bool isHighlighted)
    {
        if (_chassis is null)
        {
            return;
        }

        _chassis.MaterialOverride = isHighlighted && HighlightMaterial is not null
            ? HighlightMaterial
            : _baseMaterial;
    }
}
