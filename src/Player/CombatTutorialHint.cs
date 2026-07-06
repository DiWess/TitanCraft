using System;
using Godot;

namespace TitanCraft.Player;

/// <summary>
/// Combat Tutorial Hint System - Phase 7.4 Priority 1
/// Provides on-screen guidance for new players on first scout encounter.
/// Triggers when scout is first detected, displays kiting mechanic hint.
/// </summary>
public partial class CombatTutorialHint : Control
{
    [Export] public float DisplayDurationSeconds { get; set; } = 8.0f;
    [Export] public float FadeOutDurationSeconds { get; set; } = 1.5f;

    private Label? _hintLabel;
    private float _displayTimeRemaining;
    private bool _isDisplaying;
    private bool _hasBeenShown;

    private const string KitingHint = "THREAT DETECTED\n" +
        "Maintain distance and attack from range\n" +
        "Move: WASD | Attack: Mouse Click | Reload: R\n" +
        "[Hint will auto-dismiss]";

    public override void _Ready()
    {
        // Initialize hint label
        _hintLabel = GetNode<Label>("HintLabel");
        _hintLabel.Text = KitingHint;
        _hintLabel.Modulate = new Color(1, 1, 1, 0); // Start invisible

        _displayTimeRemaining = 0;
        _isDisplaying = false;
        _hasBeenShown = false;
    }

    public override void _Process(double delta)
    {
        if (!_isDisplaying || _hintLabel == null)
        {
            return;
        }

        _displayTimeRemaining -= (float)delta;

        if (_displayTimeRemaining <= 0)
        {
            // Fade out
            float fadeProgress = Math.Min(1.0f, (-_displayTimeRemaining) / FadeOutDurationSeconds);
            _hintLabel.Modulate = new Color(1, 1, 1, 1.0f - fadeProgress);

            if (fadeProgress >= 1.0f)
            {
                _isDisplaying = false;
            }
        }
    }

    /// <summary>
    /// Show combat tutorial hint on first scout detection.
    /// Called by scout when transitioning Idle→Chase on first encounter.
    /// </summary>
    public void ShowCombatTutorial()
    {
        if (_hasBeenShown || _hintLabel == null)
        {
            return;
        }

        _hasBeenShown = true;
        _displayTimeRemaining = DisplayDurationSeconds;
        _isDisplaying = true;
        _hintLabel.Modulate = new Color(1, 1, 1, 1.0f); // Fade in
    }

    /// <summary>
    /// Reset tutorial display (for testing/respawn scenarios).
    /// </summary>
    public void Reset()
    {
        _hasBeenShown = false;
        _isDisplaying = false;
        _displayTimeRemaining = 0;
        if (_hintLabel != null)
        {
            _hintLabel.Modulate = new Color(1, 1, 1, 0);
        }
    }
}
