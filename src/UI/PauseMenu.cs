using System;
using Godot;

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
    }

    public void Resume()
    {
        Visible = false;
        GetTree().Paused = false;
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public void SaveGame() => SaveRequested?.Invoke();

    public void ReturnToMainMenu()
    {
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile(MainMenuScenePath);
    }

    public void QuitGame() => GetTree().Quit();
}
