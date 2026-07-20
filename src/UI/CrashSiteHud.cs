using Godot;

namespace TitanCraft.UI;

public partial class CrashSiteHud : CanvasLayer
{
    private Label _health = null!;
    private Label _objective = null!;
    private Label _resources = null!;
    private const string StartTutorialText = "ZQSD/WASD: move | Mouse: look | Space: jump | E: interact/craft at workbench | Left click: attack after Mk I is built | Esc: pause";

    private Label _interaction = null!;
    private Label _armState = null!;
    private Label _startTutorial = null!;
    private Label _actionFeedback = null!;

    public override void _Ready()
    {
        _health = GetNode<Label>("Panel/Margin/VBox/Health");
        _objective = GetNode<Label>("Panel/Margin/VBox/Objective");
        _resources = GetNode<Label>("Panel/Margin/VBox/Resources");
        _interaction = GetNode<Label>("Panel/Margin/VBox/InteractionPrompt");
        _armState = GetNode<Label>("Panel/Margin/VBox/MechanicalArmState");
        _startTutorial = GetNode<Label>("Panel/Margin/VBox/StartTutorial");
        _actionFeedback = GetNode<Label>("ActionFeedback");
        SetHealth(100, 100);
        SetObjective("Collect resources near the crash site.");
        SetResources(0, 0, 0, false);
        SetInteractionPrompt(string.Empty);
        SetMechanicalArmProgress("Mechanical Arm Mk I: Metal 0/10 | Biomass 0/3 | Electronics 0/2");
        SetStartTutorialVisible(true);
        SetActionFeedback("Left click: Mk I not built yet — craft it at the workbench first.");
    }

    private static readonly Color GainAccent = new(1.0f, 0.62f, 0.18f);
    private static readonly Color DamageAccent = new(1.0f, 0.28f, 0.22f);
    private int _lastHealth = -1;
    private (int Metal, int Biomass, int Electronics, bool Component) _lastResources = (-1, -1, -1, false);
    private string _lastArmProgress = string.Empty;

    public void SetHealth(int current, int maximum)
    {
        _health.Text = $"Health: {current}/{maximum}";
        if (_lastHealth >= 0 && current < _lastHealth)
        {
            PulseLabel(_health, DamageAccent);
        }
        _lastHealth = current;
    }

    public void SetObjective(string objective) => _objective.Text = objective;

    public void SetResources(int metal, int biomass, int electronicComponents, bool hasGalaxabrainComponent)
    {
        // Surface both craft electronics and the later mission component so the HUD
        // confirms every pickup required by the Crash Site loop.
        var missionComponent = hasGalaxabrainComponent ? "Recovered" : "Missing";
        _resources.Text = $"Metal: {metal}  Biomass: {biomass}  Electronics: {electronicComponents}  Galaxabrain Component: {missionComponent}";

        var current = (metal, biomass, electronicComponents, hasGalaxabrainComponent);
        var gained = _lastResources.Metal >= 0
            && (metal > _lastResources.Metal
                || biomass > _lastResources.Biomass
                || electronicComponents > _lastResources.Electronics
                || (hasGalaxabrainComponent && !_lastResources.Component));
        if (gained)
        {
            PulseLabel(_resources, GainAccent);
        }
        _lastResources = current;
    }

    private void PulseLabel(Label label, Color accent)
    {
        // One tween per label; a new pulse replaces the previous one mid-flight.
        if (label.HasMeta("pulse_tween") && label.GetMeta("pulse_tween").As<Tween>() is { } previous && previous.IsValid())
        {
            previous.Kill();
        }

        label.Modulate = accent;
        var tween = CreateTween();
        tween.TweenProperty(label, "modulate", Colors.White, 0.45f)
            .SetTrans(Tween.TransitionType.Quad)
            .SetEase(Tween.EaseType.Out);
        label.SetMeta("pulse_tween", tween);
    }

    public void SetInteractionPrompt(string prompt)
    {
        _interaction.Visible = !string.IsNullOrWhiteSpace(prompt);
        _interaction.Text = prompt;
    }

    public void SetStartTutorialVisible(bool visible)
    {
        _startTutorial.Text = StartTutorialText;
        _startTutorial.Visible = visible;
    }

    public void SetMechanicalArmProgress(string progressText)
    {
        if (_lastArmProgress.Length > 0 && progressText != _lastArmProgress)
        {
            PulseLabel(_armState, GainAccent);
        }
        _lastArmProgress = progressText;
        _armState.Text = progressText;
    }

    public void SetActionFeedback(string message)
    {
        // Keep the latest craft/attack result visible so players can confirm whether their input worked.
        _actionFeedback.Visible = !string.IsNullOrWhiteSpace(message);
        _actionFeedback.Text = message;
    }
}
