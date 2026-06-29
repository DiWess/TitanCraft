using Godot;
using TitanCraft.SaveSystem;

namespace TitanCraft.UI;

public partial class MainMenu : Control
{
    [Export] public string GameScenePath { get; set; } = "res://scenes/Main/Main.tscn";
    [Export] public string SavePath { get; set; } = "user://crash_site_save.json";

    public override void _Ready()
    {
        GetNode<Button>("Menu/ContinueButton").Disabled = !LocalSaveGameStore.SaveExists(SavePath);
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

    public void QuitGame() => GetTree().Quit();
}
