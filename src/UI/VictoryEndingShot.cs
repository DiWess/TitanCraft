using Godot;

namespace TitanCraft.UI;

/// <summary>
/// The in-world ending. When the beacon completes the mission, the view cuts
/// from the player to a fixed camera framing the lit beacon against the
/// harbour, the HUD steps aside for a letterbox and a caption, and the camera
/// pushes slowly in while the mission-complete swell plays. The end-screen
/// navigator holds the world for the length of this shot, then hands its last
/// frame to the victory screen (<see cref="EndingSnapshot"/>).
/// </summary>
public partial class VictoryEndingShot : Node3D
{
    [Export] public NodePath CameraPath { get; set; } = "EndingCamera";
    [Export] public NodePath OverlayPath { get; set; } = "Overlay";
    [Export] public NodePath CaptionPath { get; set; } = "Overlay/Caption";
    [Export] public NodePath HudPath { get; set; } = "../HUD";
    [Export] public NodePath PlayerPath { get; set; } = "../Player";

    /// <summary>World point the camera looks at: the beacon's crown.</summary>
    [Export] public Vector3 LookTarget { get; set; } = new(30f, 4f, -21f);

    /// <summary>How far the camera travels toward the target over the shot.</summary>
    [Export] public float PushMetres { get; set; } = 2.0f;
    [Export] public float ShotSeconds { get; set; } = 5.0f;

    public bool IsPlaying { get; private set; }

    public void Play()
    {
        if (IsPlaying)
            return;

        IsPlaying = true;
        var camera = GetNodeOrNull<Camera3D>(CameraPath);
        if (camera is null)
            return;

        camera.LookAt(LookTarget, Vector3.Up);
        camera.Current = true;

        // The player stops being a player: no input, no movement, no view bob
        // fighting the shot. The HUD's objective text would restate what the
        // caption says, so it steps aside too.
        var player = GetNodeOrNull<Node>(PlayerPath);
        if (player is not null)
            player.ProcessMode = ProcessModeEnum.Disabled;

        if (GetNodeOrNull<CanvasLayer>(HudPath) is { } hud)
            hud.Visible = false;

        if (GetNodeOrNull<CanvasLayer>(OverlayPath) is { } overlay)
            overlay.Visible = true;

        var towardTarget = (LookTarget - camera.GlobalPosition).Normalized() * PushMetres;
        var tween = CreateTween();
        tween.TweenProperty(camera, "global_position", camera.GlobalPosition + towardTarget, ShotSeconds)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.Out);

        if (GetNodeOrNull<CanvasItem>(CaptionPath) is { } caption)
        {
            caption.Modulate = new Color(1, 1, 1, 0);
            tween.Parallel().TweenProperty(caption, "modulate:a", 1.0, 1.2).SetDelay(0.8);
        }
    }
}
