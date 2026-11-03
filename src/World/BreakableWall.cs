using Godot;
using TitanCraft.Combat;
using TitanCraft.Core;
using TitanCraft.Missions;
using TitanCraft.Player;
using TitanCraft.Resources;

namespace TitanCraft.World;

/// <summary>
/// The cracked coral-stone gate of the tower court, sealed by the Galaxabrains
/// with the beacon's power core behind it. Ordinary strikes ring off it; the
/// Mk III breaker coil's charged strike brings it down. Breaking it wakes the
/// Warden inside.
/// </summary>
public partial class BreakableWall : StaticBody3D, IArmTarget, IAimHint, IMissionRestorable
{
    [Export] public NodePath PlayerPath { get; set; } = "../../Player";
    [Export] public NodePath IntactVisualPath { get; set; } = "IntactVisual";
    [Export] public NodePath BrokenVisualPath { get; set; } = "BrokenVisual";
    [Export] public NodePath CollisionPath { get; set; } = "CollisionShape3D";
    [Export] public NodePath BreakAudioPath { get; set; } = "BreakAudio";
    [Export] public NodePath BreakBurstPath { get; set; } = "BreakBurst";

    public bool IsBroken { get; private set; }

    public bool IsStrikeable => !IsBroken;

    public string DeflectFeedback => "The cracked stone holds — hold left click to charge a Mk III strike.";

    public override void _Ready()
    {
        AddToGroup(IMissionRestorable.Group);
        ApplyVisualState();
    }

    public string GetAimHint(MvpInventory inventory)
    {
        if (IsBroken)
            return string.Empty;

        return inventory.HasBreakerCoil
            ? "Hold left click, then release: charged strike breaks the cracked gate"
            : "Cracked stone — a heavier blow than Mk I could break it";
    }

    public StrikeOutcome ReceiveStrike(ArmStrike strike)
    {
        if (IsBroken)
            return StrikeOutcome.Ignored;

        if (!strike.IsCharged)
            return StrikeOutcome.Deflected;

        var mission = GetNodeOrNull<FirstPersonController>(PlayerPath)?.Mission;
        if (mission is not null && mission.CurrentStep != CrashSiteMissionStep.BreachTowerGate)
            return StrikeOutcome.Deflected;

        Break(animate: true);
        mission?.TryCompleteTowerBreach();
        return StrikeOutcome.Killed;
    }

    public void RestoreFromMission(CrashSiteMissionStep step)
    {
        if (SaveSystem.CrashSiteStateReconciler.RequiresTowerGateBroken(step) && !IsBroken)
            Break(animate: false);
    }

    private void Break(bool animate)
    {
        IsBroken = true;
        GetNodeOrNull<CollisionShape3D>(CollisionPath)?.SetDeferred(CollisionShape3D.PropertyName.Disabled, true);
        ApplyVisualState();
        if (!animate)
            return;

        AudioCue.Play3D(this, BreakAudioPath, GlobalPosition);
        if (GetNodeOrNull<GpuParticles3D>(BreakBurstPath) is { } burst)
        {
            burst.Restart();
            burst.Emitting = true;
        }

        if (IsInsideTree())
        {
            foreach (var node in GetTree().GetNodesInGroup(CameraShaker.CameraShakerGroup))
            {
                if (node is CameraShaker shaker)
                    shaker.AddTrauma(0.55f);
            }
        }
    }

    private void ApplyVisualState()
    {
        if (GetNodeOrNull<Node3D>(IntactVisualPath) is { } intact)
            intact.Visible = !IsBroken;
        if (GetNodeOrNull<Node3D>(BrokenVisualPath) is { } broken)
            broken.Visible = IsBroken;
    }
}
