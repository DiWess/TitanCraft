using Godot;
using TitanCraft.SaveSystem;

namespace TitanCraft.UI;

public partial class EndScreen : Control
{
    [Export] public string MainMenuScenePath { get; set; } = "res://scenes/UI/MainMenu.tscn";
    [Export] public string GameScenePath { get; set; } = "res://scenes/Main/Main.tscn";
    [Export] public string SavePath { get; set; } = LocalSaveGameStore.DefaultSavePath;

    public override void _Ready()
    {
        if (LocalSaveGameStore.SaveExists(SavePath))
            return;

        var summary = GetNodeOrNull<Label>("Menu/Summary");
        if (summary is not null)
            summary.Text = "No checkpoint save was found. Start a new Crash Site run to continue.";

        var reloadButton = GetNodeOrNull<Button>("Menu/ReloadButton");
        if (reloadButton is not null)
            reloadButton.Text = "Start New Crash Site Run";
    }

    public void ReturnToMainMenu() => GetTree().ChangeSceneToFile(MainMenuScenePath);

    // Main.tscn owns the restore decision through CrashSiteSaveCoordinator.LoadGameIfPresent().
    public void ReloadLastSave() => GetTree().ChangeSceneToFile(GameScenePath);
}
