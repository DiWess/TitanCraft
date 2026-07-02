using Godot;
using TitanCraft.Missions;
using TitanCraft.Player;

namespace TitanCraft.UI;

public partial class CrashSiteHudBinder : Node
{
    [Export] public NodePath PlayerPath { get; set; } = "../Player";
    [Export] public NodePath HudPath { get; set; } = "../HUD";

    private FirstPersonController _player = null!;
    private CrashSiteHud _hud = null!;
    private bool _startTutorialDismissed;
    private bool _armBuiltFeedbackShown;

    public override void _Ready()
    {
        _player = GetNode<FirstPersonController>(PlayerPath);
        _hud = GetNode<CrashSiteHud>(HudPath);
        _player.Health.Changed += UpdateHealth;
        _player.Inventory.Changed += UpdateInventory;
        _player.Mission.Changed += UpdateMission;
        _player.InteractionPromptChanged += _hud.SetInteractionPrompt;
        _player.ActionFeedbackChanged += _hud.SetActionFeedback;
        RefreshAll();
    }

    public override void _ExitTree()
    {
        if (_player is null)
            return;

        _player.Health.Changed -= UpdateHealth;
        _player.Inventory.Changed -= UpdateInventory;
        _player.Mission.Changed -= UpdateMission;
        _player.InteractionPromptChanged -= _hud.SetInteractionPrompt;
        _player.ActionFeedbackChanged -= _hud.SetActionFeedback;
    }

    private void RefreshAll()
    {
        UpdateHealth(_player.Health);
        UpdateInventory(_player.Inventory);
        UpdateMission(_player.Mission);
        _hud.SetInteractionPrompt(string.Empty);
    }

    private void UpdateHealth(PlayerHealth health) => _hud.SetHealth(health.CurrentHealth, health.MaxHealth);

    private void UpdateInventory(TitanCraft.Resources.MvpInventory inventory)
    {
        _hud.SetResources(inventory.Metal, inventory.Biomass, inventory.ElectronicComponents);
        _hud.SetMechanicalArmBuilt(inventory.IsMechanicalArmBuilt);

        // Replace the startup "not built yet" hint the moment the arm exists,
        // otherwise the stale hint contradicts the actual state.
        if (inventory.IsMechanicalArmBuilt && !_armBuiltFeedbackShown)
        {
            _armBuiltFeedbackShown = true;
            _hud.SetActionFeedback("Mechanical Arm Mk I online — left click punches the Galaxabrain.");
        }
    }

    private void UpdateMission(CrashSiteMissionState mission)
    {
        _hud.SetObjective(mission.CurrentObjectiveText);

        if (_startTutorialDismissed || mission.CurrentStep == CrashSiteMissionStep.CollectResources)
            return;

        _startTutorialDismissed = true;
        _hud.SetStartTutorialVisible(false);
    }
}
