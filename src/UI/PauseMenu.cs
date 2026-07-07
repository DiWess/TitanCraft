using System;
using Godot;
using TitanCraft.Core;

namespace TitanCraft.UI;

public partial class PauseMenu : CanvasLayer
{
    public event Action? SaveRequested;
    [Export] public string MainMenuScenePath { get; set; } = "res://scenes/UI/MainMenu.tscn";
    [Export] public bool CanSave { get; set; } = true;

    public override void _Ready()
    {
        Visible = false;
        GetNode<Button>("Panel/Menu/SaveButton").Disabled = !CanSave;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("pause_menu"))
            TogglePause();
    }

    public void TogglePause()
    {
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
