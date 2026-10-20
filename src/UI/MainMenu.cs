using Godot;
using TitanCraft.Core;
using TitanCraft.SaveSystem;

namespace TitanCraft.UI;

public partial class MainMenu : Control
{
    [Export] public string GameScenePath { get; set; } = "res://scenes/Main/Main.tscn";
    [Export] public string SavePath { get; set; } = "user://crash_site_save.json";

    private SettingsPanel? _settings;

    public override void _Ready()
    {
        // The game always starts here, so this is where saved settings take
        // effect (window mode, volume, key bindings) before anything is played.
        GameSettings.EnsureLoaded();
        GameSettings.Apply();
        Input.MouseMode = Input.MouseModeEnum.Visible;
        GetNode<Button>("Menu/ContinueButton").Disabled = !LocalSaveGameStore.SaveExists(SavePath);
        var settingsButton = GetNode<Button>("Menu/SettingsButton");
        settingsButton.Disabled = false;
        settingsButton.Pressed += OpenSettings;
        if (GetNodeOrNull<Label>("VersionLabel") is { } version)
            version.Text = $"{ProjectSettings.GetSetting("application/config/name")} {ProjectSettings.GetSetting("application/config/version")}";
    }

    public void NewGame()
    {
        LocalSaveGameStore.DeleteSave(SavePath);
        GetTree().ChangeSceneToFile(GameScenePath);
    }

    public void ContinueGame()
    {
        if (LocalSaveGameStore.SaveExists(SavePath))
            GetTree().ChangeSceneToFile(GameScenePath);
    }

    public void OpenSettings()
    {
        if (_settings is null)
        {
            _settings = new SettingsPanel { Name = "SettingsPanel" };
            _settings.SetAnchorsPreset(LayoutPreset.Center);
            AddChild(_settings);
            _settings.Closed += () => GetNode<Control>("Menu").Visible = true;
        }

        GetNode<Control>("Menu").Visible = false;
        _settings.Open();
        _settings.Position = (Size - _settings.GetCombinedMinimumSize()) / 2f;
    }

    public void QuitGame() => GetTree().Quit();
}
