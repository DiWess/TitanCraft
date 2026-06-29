using Godot;

namespace TitanCraft.UI;

public partial class CrashSiteHud : CanvasLayer
{
    private Label _health = null!;
    private Label _objective = null!;
    private Label _resources = null!;
    private const string StartTutorialText = "ZQSD/WASD: move | Mouse: look | Space: jump | E: interact | Left click: attack | Esc: pause";

    private Label _interaction = null!;
    private Label _armState = null!;
    private Label _startTutorial = null!;

    public override void _Ready()
    {
        _health = GetNode<Label>("Panel/Margin/VBox/Health");
        _objective = GetNode<Label>("Panel/Margin/VBox/Objective");
        _resources = GetNode<Label>("Panel/Margin/VBox/Resources");
        _interaction = GetNode<Label>("Panel/Margin/VBox/InteractionPrompt");
        _armState = GetNode<Label>("Panel/Margin/VBox/MechanicalArmState");
        _startTutorial = GetNode<Label>("Panel/Margin/VBox/StartTutorial");
        SetHealth(100, 100);
        SetObjective("Collect resources near the crash site.");
        SetResources(0, 0, 0);
        SetInteractionPrompt(string.Empty);
        SetMechanicalArmBuilt(false);
        SetStartTutorialVisible(true);
    }

    public void SetHealth(int current, int maximum) => _health.Text = $"Health: {current}/{maximum}";

    public void SetObjective(string objective) => _objective.Text = $"Objective: {objective}";

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

    public void SetMechanicalArmBuilt(bool isBuilt)
    {
        _armState.Text = isBuilt ? "Mechanical Arm: Online" : "Mechanical Arm: Not built";
    }
}
