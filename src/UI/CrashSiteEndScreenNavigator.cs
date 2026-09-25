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

    /// <summary>
    /// How long the world stays on screen after victory before the end screen,
    /// so the beacon's beam, camera shake and activation sound are actually seen.
    /// </summary>
    [Export] public float VictoryHoldSeconds { get; set; } = 3.0f;

    public string LastRequestedScenePath { get; private set; } = string.Empty;

    /// <summary>True between an end-screen request and the scene change it schedules.</summary>
    public bool IsSceneChangePending { get; private set; }

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

        if (!EnableSceneChanges)
            return;

        // Never change scene from inside the event that asked for it. This runs
        // from Mission.Changed or Health.Changed, i.e. in the middle of whatever
        // raised them -- Beacon.Interact, or the Scout's attack. Godot 4 removes
        // the current scene from the tree the moment ChangeSceneToFile is
        // called, so the caller's remaining code ran with GetTree() == null: the
        // beacon threw at AddExtractionTrauma and its beam and sound never
        // played (docs/production/playtests/2026-09-24-journey.md, finding 1).
        IsSceneChangePending = true;
        if (scenePath == VictoryScenePath && VictoryHoldSeconds > 0f)
            GetTree().CreateTimer(VictoryHoldSeconds).Timeout += () => ChangeScene(scenePath);
        else
            Callable.From(() => ChangeScene(scenePath)).CallDeferred();
    }

    private void ChangeScene(string scenePath)
    {
        // The scene may have been freed during the hold (tests, quitting).
        if (!IsInstanceValid(this) || !IsInsideTree())
            return;

        IsSceneChangePending = false;
        Input.MouseMode = Input.MouseModeEnum.Visible;
        GetTree().ChangeSceneToFile(scenePath);
    }
}
