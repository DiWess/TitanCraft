using System;
using Godot;
using TitanCraft.Core;

namespace TitanCraft.UI;

public partial class PauseMenu : CanvasLayer
{
    public event Action? SaveRequested;
    [Export] public string MainMenuScenePath { get; set; } = "res://scenes/UI/MainMenu.tscn";
    [Export] public bool CanSave { get; set; } = true;

    private SettingsPanel? _settings;

    public override void _Ready()
    {
        Visible = false;
        GetNode<Button>("Panel/Menu/SaveButton").Disabled = !CanSave;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!@event.IsActionPressed("pause_menu"))
            return;

        // Esc backs out of the settings screen first, then out of the pause.
        if (_settings is { Visible: true })
        {
            if (!_settings.IsListeningForKey)
                _settings.Close();
            return;
        }

        TogglePause();
    }

    public void OpenSettings()
    {
        AudioCue.Play(this, "AudioLayer_UI/UI_Select");
        if (_settings is null)
        {
            _settings = new SettingsPanel { Name = "SettingsPanel" };
            AddChild(_settings);
            _settings.Closed += () => GetNode<Control>("Panel").Visible = true;
        }

        GetNode<Control>("Panel").Visible = false;
        _settings.Open();
        var viewport = GetViewport().GetVisibleRect().Size;
        _settings.Position = (viewport - _settings.GetCombinedMinimumSize()) / 2f;
    }

    public void TogglePause()
    {
        if (_settings is { Visible: true })
            _settings.Close();
        Visible = !Visible;
        GetTree().Paused = Visible;
        Input.MouseMode = Visible ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
        AudioCue.Play(this, "AudioLayer_UI/UI_Menu_Toggle");
    }

    public void Resume()
    {
        Visible = false;
        GetTree().Paused = false;
        Input.MouseMode = Input.MouseModeEnum.Captured;
        AudioCue.Play(this, "AudioLayer_UI/UI_Select");
    }

    public void SaveGame()
    {
        AudioCue.Play(this, "AudioLayer_UI/UI_Select");
        SaveRequested?.Invoke();
    }

    public void ReturnToMainMenu()
    {
        AudioCue.Play(this, "AudioLayer_UI/UI_Select");
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile(MainMenuScenePath);
    }

    public void QuitGame()
    {
        AudioCue.Play(this, "AudioLayer_UI/UI_Select");
        GetTree().Quit();
    }

    public void PlayHoverSound() => AudioCue.Play(this, "AudioLayer_UI/UI_Hover");
}
