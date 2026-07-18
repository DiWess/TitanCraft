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
    [Export] public NodePath SkyBeamPath { get; set; } = "LandmarkVfx/SkyBeam";
    [Export] public NodePath ActivationAudioPath { get; set; } = "ActivationAudio";
    [Export] public Material? DormantMaterial { get; set; }
    [Export] public Material? ActiveMaterial { get; set; }
    [Export] public Material? HighlightMaterial { get; set; }

    public bool IsActivated { get; private set; }

    private MeshInstance3D? _closedVisual;
    private MeshInstance3D? _activeVisual;
    private GpuParticles3D? _activationPillar;
    private Material? _baseDormantMaterial;

    private const float BeamPulseRadiansPerSecond = 2.4f;
    private const float BeamPulseDepth = 0.25f;
    private SpotLight3D? _skyBeam;
    private float _skyBeamBaseEnergy;
    private float _pulseSeconds;

    public override void _Ready()
    {
        CollisionLayer = 1u << 1;
        CollisionMask = 1u;
        _closedVisual = GetNodeOrNull<MeshInstance3D>(ClosedVisualPath);
        _activeVisual = GetNodeOrNull<MeshInstance3D>(ActiveVisualPath);
        _activationPillar = GetNodeOrNull<GpuParticles3D>(ActivationPillarPath);
        _skyBeam = GetNodeOrNull<SpotLight3D>(SkyBeamPath);
        _skyBeamBaseEnergy = _skyBeam?.LightEnergy ?? 0f;
        _baseDormantMaterial = _closedVisual?.MaterialOverride ?? DormantMaterial;
        UpdateVisualState();
    }

    public override void _Process(double delta)
    {
        // Activated-state heartbeat: the sky beam breathes so the completed
        // objective reads as live from anywhere on the map.
        if (!IsActivated || _skyBeam is null)
        {
            return;
        }

        _pulseSeconds += (float)delta;
        _skyBeam.LightEnergy = _skyBeamBaseEnergy
            * (1f + BeamPulseDepth * Mathf.Sin(_pulseSeconds * BeamPulseRadiansPerSecond));
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
