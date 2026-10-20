using Godot;
using TitanCraft.Combat;
using TitanCraft.Core;
using TitanCraft.Missions;
using TitanCraft.Player;
using TitanCraft.Resources;

namespace TitanCraft.World;

/// <summary>
/// Hull wreckage jammed across the Net-makers' Yard gate. Too heavy to shift
/// by hand, it is the first thing the Mk II magnet clamp is for: one pull drags
/// it clear and tips it aside, and the yard is open for the rest of the run.
/// The clamp pulls objects to the player, never the player to a point (README
/// section 6 keeps the grappling hook forbidden).
/// </summary>
public partial class PullableBarricade : StaticBody3D, IPullTarget, IArmTarget, IAimHint, IMissionRestorable
{
    [Export] public NodePath PlayerPath { get; set; } = "../../Player";
    [Export] public NodePath VisualPath { get; set; } = "Visual";
    [Export] public NodePath CollisionPath { get; set; } = "CollisionShape3D";

    /// <summary>How far the wreck is dragged toward the player before it tips over.</summary>
    [Export] public float DragMetres { get; set; } = 1.6f;

    /// <summary>Where the wreck comes to rest, relative to its closed position (tipped clear of the gate).</summary>
    [Export] public Vector3 RestOffset { get; set; } = new(-2.7f, -0.2f, 1.1f);

    [Export] public NodePath PullAudioPath { get; set; } = "PullAudio";

    public bool IsOpen { get; private set; }

    public bool CanBePulled => !IsOpen && PlayerHasClamp();

    public Vector3 PullAnchor => GlobalPosition + Vector3.Up * 1.1f;

    public bool IsStrikeable => !IsOpen;

    public string DeflectFeedback => "The wreck is jammed fast — the Mk II clamp could drag it clear.";

    public override void _Ready()
    {
        AddToGroup(FirstPersonController.PullTargetGroup);
        AddToGroup(IMissionRestorable.Group);
    }

    public string GetAimHint(MvpInventory inventory)
    {
        if (IsOpen)
            return string.Empty;

        return inventory.HasMagnetClamp
            ? "Right click to pull the wreck clear (Mk II)"
            : "Wreckage jams the gate — too heavy to move by hand";
    }

    public StrikeOutcome ReceiveStrike(ArmStrike strike) => IsOpen ? StrikeOutcome.Ignored : StrikeOutcome.Deflected;

    public bool Pull(Vector3 pullerPosition)
    {
        if (!CanBePulled)
            return false;

        var mission = Mission();
        if (mission is not null && mission.CurrentStep != CrashSiteMissionStep.OpenNetYard)
            return false;

        Open(animate: true, pullerPosition);
        mission?.TryCompleteNetYardOpening();
        return true;
    }

    public void RestoreFromMission(CrashSiteMissionStep step)
    {
        if (SaveSystem.CrashSiteStateReconciler.RequiresNetYardOpen(step) && !IsOpen)
            Open(animate: false, GlobalPosition);
    }

    private void Open(bool animate, Vector3 pullerPosition)
    {
        IsOpen = true;
        GetNodeOrNull<CollisionShape3D>(CollisionPath)?.SetDeferred(CollisionShape3D.PropertyName.Disabled, true);
        var visual = GetNodeOrNull<Node3D>(VisualPath);
        if (visual is null)
            return;

        var rest = RestOffset;
        var restRotation = new Vector3(0f, 0.55f, 1.45f);
        if (!animate)
        {
            visual.Position = rest;
            visual.Rotation = restRotation;
            return;
        }

        AudioCue.Play3D(this, PullAudioPath, GlobalPosition);
        // Drag toward the clamp, then tip over clear of the gate: a heavy,
        // two-beat motion so the pull reads as force, not a fade.
        var toward = (pullerPosition - GlobalPosition) with { Y = 0f };
        var drag = toward.LengthSquared() > 0.01f ? ToLocal(GlobalPosition + toward.Normalized() * DragMetres) : Vector3.Zero;
        var tween = CreateTween();
        tween.TweenProperty(visual, "position", drag + new Vector3(0f, 0.25f, 0f), 0.32f)
            .SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
        tween.TweenProperty(visual, "position", rest, 0.45f)
            .SetTrans(Tween.TransitionType.Bounce).SetEase(Tween.EaseType.Out);
        tween.Parallel().TweenProperty(visual, "rotation", restRotation, 0.45f)
            .SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.In);
        tween.TweenCallback(Callable.From(() => ShakeCameras(0.35f)));
    }

    private void ShakeCameras(float trauma)
    {
        if (!IsInsideTree())
            return;

        foreach (var node in GetTree().GetNodesInGroup(CameraShaker.CameraShakerGroup))
        {
            if (node is CameraShaker shaker)
                shaker.AddTrauma(trauma);
        }
    }

    private bool PlayerHasClamp() =>
        GetNodeOrNull<FirstPersonController>(PlayerPath) is not { } player || player.Inventory.HasMagnetClamp;

    private CrashSiteMissionState? Mission() => GetNodeOrNull<FirstPersonController>(PlayerPath)?.Mission;
}
