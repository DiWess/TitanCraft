using Godot;

namespace TitanCraft.UI;

public partial class EndScreen : Control
{
    [Export] public string MainMenuScenePath { get; set; } = "res://scenes/UI/MainMenu.tscn";
    [Export] public string GameScenePath { get; set; } = "res://scenes/Main/Main.tscn";

    public void ReturnToMainMenu() => GetTree().ChangeSceneToFile(MainMenuScenePath);

    public void ReloadLastSave() => GetTree().ChangeSceneToFile(GameScenePath);
}
