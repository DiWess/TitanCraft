using System;
using Godot;
using TitanCraft.Missions;
using TitanCraft.Resources;

namespace TitanCraft.World;

public partial class Beacon : Area3D, ICrashSiteInteractable
{
    [Export] public NodePath ClosedVisualPath { get; set; } = "ClosedVisual";

    [Export] public NodePath ActiveVisualPath { get; set; } = "ActiveVisual";

    public bool IsActivated { get; private set; }

    public override void _Ready() => UpdateVisualState();

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

        IsActivated = mission.TryCompleteBeaconActivation();
        UpdateVisualState();
        return IsActivated;
    }

    /// <summary>
    /// Reconstructs the activation state from a save without mutating mission progression.
    /// </summary>
    public void RestoreActivated(bool isActivated)
    {
        IsActivated = isActivated;
        UpdateVisualState();
    }

    private void UpdateVisualState()
    {
        if (!ClosedVisualPath.IsEmpty && GetNodeOrNull<Node3D>(ClosedVisualPath) is { } closedVisual)
            closedVisual.Visible = !IsActivated;

        if (!ActiveVisualPath.IsEmpty && GetNodeOrNull<Node3D>(ActiveVisualPath) is { } activeVisual)
            activeVisual.Visible = IsActivated;
    }
}
