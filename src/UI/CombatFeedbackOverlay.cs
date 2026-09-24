using Godot;
using TitanCraft.Player;

namespace TitanCraft.UI;

/// <summary>
/// Crosshair, hit marker and damage-direction indicator, drawn procedurally.
///
/// Drawn rather than composed from texture assets so the overlay carries no
/// art dependency and no licence surface: it is pure geometry sized from the
/// viewport, which also keeps it crisp at any resolution.
///
/// This closes three gaps the MVP HUD had: there was no aiming reference, no
/// confirmation that a strike connected beyond a text line, and no way to tell
/// where damage came from.
/// </summary>
public partial class CombatFeedbackOverlay : Control
{
    /// <summary>
    /// Group the player controller resolves this overlay through, so the HUD
    /// and the player stay independent of each other's ready ordering.
    /// </summary>
    public const string OverlayGroup = "combat_feedback_overlay";

    [Export] public Color CrosshairColor { get; set; } = new(0.92f, 0.90f, 0.84f, 0.85f);
    [Export] public Color HitMarkerColor { get; set; } = new(1.0f, 0.98f, 0.92f, 1.0f);
    [Export] public Color LethalHitMarkerColor { get; set; } = new(1.0f, 0.36f, 0.22f, 1.0f);
    [Export] public Color DamageColor { get; set; } = new(0.95f, 0.18f, 0.14f, 1.0f);
    [Export(PropertyHint.Range, "2,24,0.5")] public float CrosshairGap { get; set; } = 5.0f;
    [Export(PropertyHint.Range, "2,24,0.5")] public float CrosshairLength { get; set; } = 7.0f;
    [Export(PropertyHint.Range, "1,6,0.5")] public float CrosshairThickness { get; set; } = 2.0f;

    private readonly HitMarkerState _hitMarker = new();
    private readonly DamageIndicatorState _damageIndicator = new();

    public bool IsHitMarkerVisible => _hitMarker.IsVisible;

    public bool IsDamageIndicatorVisible => _damageIndicator.IsVisible;

    public override void _Ready()
    {
        // The overlay is a full-rect passthrough: it must never intercept the
        // clicks that drive attacking and interaction.
        MouseFilter = MouseFilterEnum.Ignore;
        SetAnchorsPreset(LayoutPreset.FullRect);
        AddToGroup(OverlayGroup);
    }

    public void ShowHitMarker(bool lethal)
    {
        _hitMarker.Trigger(lethal);
        QueueRedraw();
    }

    public void ShowDamageFrom(Vector3 playerPosition, float playerYawRadians, Vector3 damageSource)
    {
        _damageIndicator.Trigger(
            DamageIndicatorState.ScreenAngleFor(playerPosition, playerYawRadians, damageSource));
        QueueRedraw();
    }

    public override void _Process(double delta)
    {
        var wasActive = _hitMarker.IsVisible || _damageIndicator.IsVisible;
        _hitMarker.Tick((float)delta);
        _damageIndicator.Tick((float)delta);
        if (wasActive)
        {
            // Only redraw while something is animating; a static crosshair does
            // not need a repaint every frame.
            QueueRedraw();
        }
    }

    public override void _Draw()
    {
        var center = Size / 2.0f;
        DrawCrosshair(center);
        if (_hitMarker.IsVisible)
        {
            DrawHitMarker(center);
        }

        if (_damageIndicator.IsVisible)
        {
            DrawDamageIndicator(center);
        }
    }

    private void DrawCrosshair(Vector2 center)
    {
        foreach (var direction in new[] { Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right })
        {
            DrawLine(
                center + direction * CrosshairGap,
                center + direction * (CrosshairGap + CrosshairLength),
                CrosshairColor,
                CrosshairThickness);
        }

        DrawRect(new Rect2(center - Vector2.One, Vector2.One * 2.0f), CrosshairColor);
    }

    private void DrawHitMarker(Vector2 center)
    {
        var color = _hitMarker.IsLethal ? LethalHitMarkerColor : HitMarkerColor;
        color.A *= _hitMarker.Opacity;
        // Four diagonal ticks: the shape reads as confirmation at a glance and
        // cannot be confused with the crosshair's axis-aligned lines.
        var inner = CrosshairGap + 2.0f;
        var outer = inner + (_hitMarker.IsLethal ? 9.0f : 6.0f);
        foreach (var direction in new[]
                 {
                     new Vector2(-1, -1), new Vector2(1, -1),
                     new Vector2(-1, 1), new Vector2(1, 1),
                 })
        {
            var unit = direction.Normalized();
            DrawLine(center + unit * inner, center + unit * outer, color, CrosshairThickness);
        }
    }

    private void DrawDamageIndicator(Vector2 center)
    {
        var color = DamageColor;
        color.A *= _damageIndicator.Opacity;
        var radius = Mathf.Min(Size.X, Size.Y) * 0.22f;
        var angle = _damageIndicator.AngleRadians;
        // Screen space: angle 0 is straight up (dead ahead), growing clockwise.
        var direction = new Vector2(Mathf.Sin(angle), -Mathf.Cos(angle));
        var tangent = new Vector2(-direction.Y, direction.X);
        var tip = center + direction * (radius + 26.0f);
        var baseCenter = center + direction * radius;
        DrawPolygon(
            new[] { tip, baseCenter + tangent * 22.0f, baseCenter - tangent * 22.0f },
            new[] { color, color with { A = color.A * 0.25f }, color with { A = color.A * 0.25f } });
    }
}
