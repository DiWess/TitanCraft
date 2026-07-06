using Godot;

namespace TitanCraft.UI;

/// <summary>
/// Brief letterboxed mission title card played once on a fresh Crash Site run.
/// CrashSiteSaveCoordinator only calls Play() when LoadGameIfPresent() found no
/// save, so continuing or reloading a run never replays it. Presentation only:
/// it does not pause gameplay, gate input, or add any new mechanic.
/// </summary>
public partial class IntroTitleCard : CanvasLayer
{
    [Export] public NodePath LetterboxTopPath { get; set; } = "Root/LetterboxTop";
    [Export] public NodePath LetterboxBottomPath { get; set; } = "Root/LetterboxBottom";
    [Export] public NodePath TitleGroupPath { get; set; } = "Root/TitleGroup";
    [Export] public float LetterboxHeight { get; set; } = 90.0f;
    [Export] public float FadeSeconds { get; set; } = 0.9f;
    [Export] public float HoldSeconds { get; set; } = 2.2f;

    /// <summary>
    /// Looks up its child nodes at call time rather than caching them in
    /// _Ready(), since Play() can be invoked from a sibling node's own
    /// _Ready() (CrashSiteSaveCoordinator) and Godot does not guarantee
    /// _Ready() call order across siblings -- GetNodeOrNull only depends on
    /// the node tree existing, which it already does once this node is
    /// instantiated, regardless of whether _Ready() has run yet. The initial
    /// hidden state is the scene file's own "visible = false" (not set here in
    /// code), specifically so there is no _Ready() write to Visible that could
    /// race with and clobber an already-in-progress Play() call.
    /// </summary>
    public void Play()
    {
        Control? letterboxTop = GetNodeOrNull<Control>(LetterboxTopPath);
        Control? letterboxBottom = GetNodeOrNull<Control>(LetterboxBottomPath);
        Control? titleGroup = GetNodeOrNull<Control>(TitleGroupPath);
        if (letterboxTop is null || letterboxBottom is null || titleGroup is null)
        {
            return;
        }

        Visible = true;
        letterboxTop.OffsetBottom = 0f;
        letterboxBottom.OffsetTop = 0f;
        titleGroup.Modulate = new Color(1, 1, 1, 0);

        Tween tween = CreateTween();
        tween.TweenProperty(letterboxTop, "offset_bottom", LetterboxHeight, FadeSeconds);
        tween.Parallel().TweenProperty(letterboxBottom, "offset_top", -LetterboxHeight, FadeSeconds);
        tween.Parallel().TweenProperty(titleGroup, "modulate:a", 1.0, FadeSeconds);
        tween.TweenInterval(HoldSeconds);
        tween.TweenProperty(titleGroup, "modulate:a", 0.0, FadeSeconds);
        tween.Parallel().TweenProperty(letterboxTop, "offset_bottom", 0f, FadeSeconds);
        tween.Parallel().TweenProperty(letterboxBottom, "offset_top", 0f, FadeSeconds);
        tween.TweenCallback(Callable.From(() => Visible = false));
    }
}
