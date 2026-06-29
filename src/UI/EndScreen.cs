using Godot;

namespace TitanCraft.UI;

public partial class EndScreen : Control
{
    [Export] public string MainMenuScenePath { get; set; } = "res://scenes/UI/MainMenu.tscn";

    public void ReturnToMainMenu() => GetTree().ChangeSceneToFile(MainMenuScenePath);
}
