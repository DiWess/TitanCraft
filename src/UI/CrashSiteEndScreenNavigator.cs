using Godot;
using TitanCraft.Missions;
using TitanCraft.Player;

namespace TitanCraft.UI;

public partial class CrashSiteEndScreenNavigator : Node
{
    [Export] public NodePath PlayerPath { get; set; } = "../Player";
    [Export] public string VictoryScenePath { get; set; } = "res://scenes/UI/VictoryScreen.tscn";
    [Export] public string DefeatScenePath { get; set; } = "res://scenes/UI/DefeatScreen.tscn";
    [Export] public bool EnableSceneChanges { get; set; } = true;

    public string LastRequestedScenePath { get; private set; } = string.Empty;

    private FirstPersonController _player = null!;
    private bool _hasRequestedEndScreen;

    public override void _Ready()
    {
        _player = GetNode<FirstPersonController>(PlayerPath);
        _player.Mission.Changed += OnMissionChanged;
        _player.Health.Changed += OnHealthChanged;
    }

    public override void _ExitTree()
    {
        if (_player is null)
            return;

        _player.Mission.Changed -= OnMissionChanged;
        _player.Health.Changed -= OnHealthChanged;
    }

    private void OnMissionChanged(CrashSiteMissionState mission)
    {
        if (mission.CurrentStep == CrashSiteMissionStep.Victory)
            RequestEndScreen(VictoryScenePath);
    }

    private void OnHealthChanged(PlayerHealth health)
    {
        if (health.IsDead)
            RequestEndScreen(DefeatScenePath);
    }

    private void RequestEndScreen(string scenePath)
    {
        if (_hasRequestedEndScreen)
            return;

        _hasRequestedEndScreen = true;
        LastRequestedScenePath = scenePath;
        GetTree().Paused = false;
        Input.MouseMode = Input.MouseModeEnum.Visible;

        if (EnableSceneChanges)
            GetTree().ChangeSceneToFile(scenePath);
    }
}
