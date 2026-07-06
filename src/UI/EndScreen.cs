using Godot;
using TitanCraft.SaveSystem;

namespace TitanCraft.UI;

public partial class EndScreen : Control
{
    [Export] public string MainMenuScenePath { get; set; } = "res://scenes/UI/MainMenu.tscn";
    [Export] public string GameScenePath { get; set; } = "res://scenes/Main/Main.tscn";
    [Export] public string SavePath { get; set; } = LocalSaveGameStore.DefaultSavePath;
    [Export] public NodePath BackdropPath { get; set; } = "Backdrop";
    [Export] public NodePath MenuPath { get; set; } = "Menu";
    [Export] public float RevealSeconds { get; set; } = 0.7f;

    public override void _Ready()
    {
        if (LocalSaveGameStore.SaveExists(SavePath))
        {
            PlayReveal();
            return;
        }

        var summary = GetNodeOrNull<Label>("Menu/Summary");
        if (summary is not null)
            summary.Text = "No checkpoint save was found. Start a new Crash Site run to continue.";

        var reloadButton = GetNodeOrNull<Button>("Menu/ReloadButton");
        if (reloadButton is not null)
            reloadButton.Text = "Start New Crash Site Run";

        PlayReveal();
    }

    /// <summary>
    /// Fades the dark backdrop in and the menu content in shortly after, instead
    /// of the end screen just snapping into place -- presentation only, same
    /// content and same buttons, just a directed reveal instead of an instant cut.
    /// </summary>
    private void PlayReveal()
    {
        var backdrop = GetNodeOrNull<CanvasItem>(BackdropPath);
        var menu = GetNodeOrNull<CanvasItem>(MenuPath);
        if (backdrop is null && menu is null)
        {
            return;
        }

        if (backdrop is not null)
        {
            backdrop.Modulate = new Color(1, 1, 1, 0);
        }

        if (menu is not null)
        {
            menu.Modulate = new Color(1, 1, 1, 0);
        }

        Tween tween = CreateTween();
        bool backdropQueued = false;
        if (backdrop is not null)
        {
            tween.TweenProperty(backdrop, "modulate:a", 1.0, RevealSeconds);
            backdropQueued = true;
        }

        if (menu is not null)
        {
            Tween menuTweenTarget = backdropQueued ? tween.Parallel() : tween;
            menuTweenTarget.TweenProperty(menu, "modulate:a", 1.0, RevealSeconds).SetDelay(RevealSeconds * 0.5f);
        }
    }

    public void ReturnToMainMenu() => GetTree().ChangeSceneToFile(MainMenuScenePath);

    // Main.tscn owns the restore decision through CrashSiteSaveCoordinator.LoadGameIfPresent().
    public void ReloadLastSave() => GetTree().ChangeSceneToFile(GameScenePath);
}
