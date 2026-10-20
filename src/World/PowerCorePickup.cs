using System;
using Godot;
using TitanCraft.Missions;
using TitanCraft.Resources;

namespace TitanCraft.World;

/// <summary>
/// The beacon's power cell, carried off into the tower court's nest. It is only
/// within reach once the Warden is down; taking it is a checkpoint.
/// </summary>
public partial class PowerCorePickup : Area3D, ICrashSiteInteractable, IMissionRestorable
{
    [Export] public NodePath VisualPath { get; set; } = "Visual";

    /// <summary>Raised when the core is taken (the save coordinator checkpoints on it).</summary>
    public event Action? Collected;

    private bool _isCollected;
    private float _bobSeconds;

    public string InteractionPrompt => "Press E to take the beacon's power core";

    public bool IsInteractionAvailable => !_isCollected;

    public override void _Ready()
    {
        AddToGroup(IMissionRestorable.Group);
    }

    public override void _Process(double delta)
    {
        if (_isCollected || GetNodeOrNull<Node3D>(VisualPath) is not { } visual)
            return;

        _bobSeconds += (float)delta;
        visual.RotateY(0.8f * (float)delta);
        visual.Position = visual.Position with { Y = 0.08f * Mathf.Sin(_bobSeconds * 2.0f) };
    }

    public bool Interact(MvpInventory inventory, CrashSiteMissionState mission)
    {
        ArgumentNullException.ThrowIfNull(inventory);
        ArgumentNullException.ThrowIfNull(mission);

        if (_isCollected || mission.CurrentStep != CrashSiteMissionStep.RecoverPowerCore)
            return false;

        if (!mission.TryCompletePowerCoreRecovery())
            return false;

        inventory.MarkPowerCoreCollected();
        Collect();
        Collected?.Invoke();
        return true;
    }

    public void RestoreFromMission(CrashSiteMissionStep step)
    {
        if (step > CrashSiteMissionStep.RecoverPowerCore)
            Collect();
    }

    private void Collect()
    {
        _isCollected = true;
        Visible = false;
        Monitoring = false;
        Monitorable = false;
        CollisionLayer = 0;
    }
}
