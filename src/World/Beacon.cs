using System;
using Godot;
using TitanCraft.Core;
using TitanCraft.Missions;
using TitanCraft.Player;
using TitanCraft.Resources;

namespace TitanCraft.World;

public partial class Beacon : StaticBody3D, ICrashSiteInteractable, ILookHighlightTarget
{
    [Export] public NodePath ClosedVisualPath { get; set; } = "ClosedVisual";
    [Export] public NodePath ActiveVisualPath { get; set; } = "ActiveVisual";
    [Export] public NodePath ActivationPillarPath { get; set; } = "LandmarkVfx/ExtractionPillar";
    [Export] public NodePath ActivationAudioPath { get; set; } = "ActivationAudio";
    [Export] public Material? DormantMaterial { get; set; }
    [Export] public Material? ActiveMaterial { get; set; }
    [Export] public Material? HighlightMaterial { get; set; }

    public bool IsActivated { get; private set; }

    private MeshInstance3D? _closedVisual;
    private MeshInstance3D? _activeVisual;
    private GpuParticles3D? _activationPillar;
    private Material? _baseDormantMaterial;

    public override void _Ready()
    {
        CollisionLayer = 1u << 1;
        CollisionMask = 1u;
        _closedVisual = GetNodeOrNull<MeshInstance3D>(ClosedVisualPath);
        _activeVisual = GetNodeOrNull<MeshInstance3D>(ActiveVisualPath);
        _activationPillar = GetNodeOrNull<GpuParticles3D>(ActivationPillarPath);
        _baseDormantMaterial = _closedVisual?.MaterialOverride ?? DormantMaterial;
        UpdateVisualState();
    }

    public bool Interact(MvpInventory inventory, CrashSiteMissionState mission)
    {
        ArgumentNullException.ThrowIfNull(inventory);
        ArgumentNullException.ThrowIfNull(mission);

        if (IsActivated
            || mission.CurrentStep != CrashSiteMissionStep.ActivateBeacon
            || !inventory.HasGalaxabrainComponent)
        {
            return false;
        }

        if (!mission.TryCompleteBeaconActivation())
        {
            return false;
        }

        ActivateExtraction();
        return true;
    }

    public void ActivateExtraction()
    {
        if (IsActivated)
        {
            return;
        }

        IsActivated = true;
        UpdateVisualState();
        _activationPillar?.Restart();
        AddExtractionTrauma();
        AudioCue.Play(this, ActivationAudioPath);
    }

    /// <summary>
    /// Reconstructs the activation state from a save without mutating mission progression.
    /// </summary>
    public void RestoreActivated(bool isActivated)
    {
        IsActivated = isActivated;
        UpdateVisualState();
    }

    public void SetHighlighted(bool isHighlighted)
    {
        if (_closedVisual is null || IsActivated)
        {
            return;
        }

        _closedVisual.MaterialOverride = isHighlighted && HighlightMaterial is not null
            ? HighlightMaterial
            : _baseDormantMaterial;
    }

    private void AddExtractionTrauma()
    {
        foreach (var node in GetTree().GetNodesInGroup(CameraShaker.CameraShakerGroup))
        {
            if (node is CameraShaker cameraShaker)
            {
                cameraShaker.AddTrauma(0.8f);
            }
        }
    }

    private void UpdateVisualState()
    {
        if (_closedVisual is not null)
        {
            _closedVisual.Visible = !IsActivated;
            _closedVisual.MaterialOverride = IsActivated ? ActiveMaterial : _baseDormantMaterial;
        }

        if (_activeVisual is not null)
        {
            _activeVisual.Visible = IsActivated;
            _activeVisual.MaterialOverride = ActiveMaterial;
        }

        if (_activationPillar is not null)
        {
            _activationPillar.Emitting = IsActivated;
        }
    }
}
