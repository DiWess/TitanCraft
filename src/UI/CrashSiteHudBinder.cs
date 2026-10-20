using Godot;
using TitanCraft.Crafting;
using TitanCraft.Missions;
using TitanCraft.Player;

namespace TitanCraft.UI;

public partial class CrashSiteHudBinder : Node
{
    [Export] public NodePath PlayerPath { get; set; } = "../Player";
    [Export] public NodePath HudPath { get; set; } = "../HUD";

    private FirstPersonController _player = null!;
    private CrashSiteHud _hud = null!;
    private readonly MechanicalArmRecipe _mechanicalArmRecipe = new();
    private bool _armBuiltFeedbackShown;
    private readonly OnboardingTutorialState _onboarding = new();
    private OnboardingStep _lastOnboardingStep = OnboardingStep.Complete;

    public override void _Ready()
    {
        _player = GetNode<FirstPersonController>(PlayerPath);
        _hud = GetNode<CrashSiteHud>(HudPath);
        _player.Health.Changed += UpdateHealth;
        _player.Inventory.Changed += UpdateInventory;
        _player.Mission.Changed += UpdateMission;
        _player.InteractionPromptChanged += _hud.SetInteractionPrompt;
        _player.ActionFeedbackChanged += _hud.SetActionFeedback;
        _player.LookTravelled += _onboarding.ReportLook;
        _player.GroundTravelled += _onboarding.ReportGroundTravel;
        _player.Jumped += _onboarding.ReportJump;
        _player.AttackLanded += _onboarding.ReportAttackLanded;
        RefreshAll();
        PublishOnboardingPrompt(force: true);
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
        _player.LookTravelled -= _onboarding.ReportLook;
        _player.GroundTravelled -= _onboarding.ReportGroundTravel;
        _player.Jumped -= _onboarding.ReportJump;
        _player.AttackLanded -= _onboarding.ReportAttackLanded;
    }

    /// <summary>Exposed for integration tests to drive the onboarding flow.</summary>
    public OnboardingStep OnboardingStep => _onboarding.CurrentStep;

    public override void _Process(double delta)
    {
        if (_onboarding.IsComplete && _lastOnboardingStep == OnboardingStep.Complete)
        {
            return;
        }

        _onboarding.Tick((float)delta);
        PublishOnboardingPrompt(force: false);
    }

    private void PublishOnboardingPrompt(bool force)
    {
        // Only touch the HUD when the step actually changes: the prompt is a
        // label, not an every-frame redraw.
        if (!force && _onboarding.CurrentStep == _lastOnboardingStep)
        {
            return;
        }

        _lastOnboardingStep = _onboarding.CurrentStep;
        _hud.SetOnboardingPrompt(_onboarding.CurrentPrompt);
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
        // Inventory.Changed is the single HUD refresh trigger for both generic
        // resources and the mission component pickup.
        _hud.SetResources(inventory.Metal, inventory.Biomass, inventory.ElectronicComponents, inventory.HasGalaxabrainComponent, inventory.HasPowerCore);
        _hud.SetMechanicalArmProgress(ArmProgressText(inventory));
        AnnounceUpgradeReady(inventory);

        if (inventory.Metal + inventory.Biomass + inventory.ElectronicComponents > 0)
        {
            _onboarding.ReportResourceCollected();
        }

        if (_mechanicalArmRecipe.CanCraft(inventory))
        {
            _onboarding.ReportCraftingResourcesReady();
        }

        if (inventory.IsMechanicalArmBuilt)
        {
            _onboarding.ReportMechanicalArmBuilt();
        }

        PublishOnboardingPrompt(force: false);

        // Replace the startup "not built yet" hint the moment the arm exists,
        // otherwise the stale hint contradicts the actual state.
        if (inventory.IsMechanicalArmBuilt && !_armBuiltFeedbackShown)
        {
            _armBuiltFeedbackShown = true;
            _hud.SetActionFeedback(FirstPersonController.MechanicalArmCraftSuccessFeedback);
        }
    }

    private void UpdateMission(CrashSiteMissionState mission)
    {
        _hud.SetObjective(mission.HudBreadcrumb);
        _hud.SetMechanicalArmProgress(ArmProgressText(_player.Inventory));
        AnnounceUpgradeReady(_player.Inventory);
        if (mission.IsVictory)
            _hud.SetActionFeedback(FirstPersonController.BeaconActivationFeedback);
    }

    /// <summary>
    /// The arm line tracks the module the mission is asking for: the online
    /// line between upgrades, the next module's bill of salvage while the
    /// mission is collecting for it.
    /// </summary>
    private string ArmProgressText(TitanCraft.Resources.MvpInventory inventory)
    {
        var next = MechanicalArmRecipe.NextFor(inventory);
        if (next is null || next.Tier == 1)
            return next is null ? MechanicalArmRecipe.OnlineText(inventory.ArmLevel) : _mechanicalArmRecipe.GetProgressText(inventory);

        return _player.Mission.CurrentStep == World.Workbench.StepForTier(next.Tier)
            ? next.GetProgressText(inventory)
            : MechanicalArmRecipe.OnlineText(inventory.ArmLevel);
    }

    private int _announcedReadyTier;

    /// <summary>One line, once, when the salvage for the next module is in hand.</summary>
    private void AnnounceUpgradeReady(TitanCraft.Resources.MvpInventory inventory)
    {
        var next = MechanicalArmRecipe.NextFor(inventory);
        if (next is null || next.Tier < 2 || _announcedReadyTier >= next.Tier
            || _player.Mission.CurrentStep != World.Workbench.StepForTier(next.Tier)
            || !next.CanCraft(inventory))
            return;

        _announcedReadyTier = next.Tier;
        _hud.SetActionFeedback($"Salvage secured — fit {next.DisplayName} at the workbench.");
    }
}
