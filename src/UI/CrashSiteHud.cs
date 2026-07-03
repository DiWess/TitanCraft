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
        SetResources(0, 0, 0);
        SetInteractionPrompt(string.Empty);
        SetMechanicalArmProgress("Mechanical Arm Mk I: Metal 0/10 | Biomass 0/3 | Electronics 0/2");
        SetStartTutorialVisible(true);
        SetActionFeedback("Left click: Mk I not built yet — craft it at the workbench first.");
    }

    public void SetHealth(int current, int maximum) => _health.Text = $"Health: {current}/{maximum}";

    public void SetObjective(string objective) => _objective.Text = objective;

    public void SetResources(int metal, int biomass, int electronicComponents)
    {
        _resources.Text = $"Metal: {metal}  Biomass: {biomass}  Electronics: {electronicComponents}";
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
        _armState.Text = progressText;
    }

    public void SetActionFeedback(string message)
    {
        // Keep the latest craft/attack result visible so players can confirm whether their input worked.
        _actionFeedback.Visible = !string.IsNullOrWhiteSpace(message);
        _actionFeedback.Text = message;
    }
}
