using System;
using Godot;
using TitanCraft.Core;
using TitanCraft.Crafting;
using TitanCraft.Missions;
using TitanCraft.Resources;

namespace TitanCraft.World;

public partial class Workbench : StaticBody3D, ICrashSiteInteractable, ILookHighlightTarget
{
    private readonly MechanicalArmRecipe _mechanicalArmRecipe = new();

    [Export] public NodePath CraftAudioPath { get; set; } = "CraftAudio";
    [Export] public NodePath ChassisPath { get; set; } = "VisualBase/WorkbenchChassis";
    [Export] public NodePath UnbuiltArmVisualPath { get; set; } = "VisualBase/V1BetaMechanicalArmUnbuiltVisualRoot";
    [Export] public Material? HighlightMaterial { get; set; }

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

    public bool Interact(MvpInventory inventory, CrashSiteMissionState mission)
    {
        ArgumentNullException.ThrowIfNull(inventory);
        ArgumentNullException.ThrowIfNull(mission);

        if (mission.CurrentStep != CrashSiteMissionStep.BuildMechanicalArm)
        {
            return false;
        }

        if (!_mechanicalArmRecipe.TryCraft(inventory))
        {
            return false;
        }

        bool crafted = mission.TryCompleteMechanicalArmConstruction();
        if (crafted)
        {
            AudioCue.Play(this, CraftAudioPath);
            RestoreArmState(true);
        }

        return crafted;
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
