using Godot;
using TitanCraft.Player;
using TitanCraft.UI;

namespace TitanCraft.SaveSystem;

public partial class CrashSiteSaveCoordinator : Node
{
    [Export] public NodePath PlayerPath { get; set; } = "../Player";
    [Export] public NodePath PauseMenuPath { get; set; } = "../PauseMenu";
    [Export] public string SavePath { get; set; } = LocalSaveGameStore.DefaultSavePath;

    public bool LastSaveSucceeded { get; private set; }
    public bool LastLoadSucceeded { get; private set; }

    private FirstPersonController _player = null!;
    private PauseMenu _pauseMenu = null!;

    public override void _Ready()
    {
        _player = GetNode<FirstPersonController>(PlayerPath);
        _pauseMenu = GetNode<PauseMenu>(PauseMenuPath);
        _pauseMenu.SaveRequested += SaveGame;
        LoadGameIfPresent();
    }

    public override void _ExitTree()
    {
        if (_pauseMenu is not null)
            _pauseMenu.SaveRequested -= SaveGame;
    }

    public void SaveGame()
    {
        var position = _player.GlobalPosition;
        LocalSaveGameStore.Save(new CrashSiteSaveData
        {
            PlayerX = position.X,
            PlayerY = position.Y,
            PlayerZ = position.Z,
            Health = _player.Health.CurrentHealth,
            Metal = _player.Inventory.Metal,
            Biomass = _player.Inventory.Biomass,
            ElectronicComponents = _player.Inventory.ElectronicComponents,
            MechanicalArmBuilt = _player.Inventory.IsMechanicalArmBuilt,
            GalaxabrainComponentCollected = _player.Inventory.HasGalaxabrainComponent,
            MissionStep = _player.Mission.CurrentStep,
        }, SavePath);
        LastSaveSucceeded = true;
    }

    public bool LoadGameIfPresent()
    {
        if (!LocalSaveGameStore.TryLoad(out var saveData, SavePath))
            return false;

        _player.GlobalPosition = new Vector3(saveData.PlayerX, saveData.PlayerY, saveData.PlayerZ);
        _player.Health.Restore(saveData.Health);
        _player.Inventory.Restore(saveData.Metal, saveData.Biomass, saveData.ElectronicComponents, saveData.MechanicalArmBuilt, saveData.GalaxabrainComponentCollected);
        _player.Mission.Restore(saveData.MissionStep);
        LastLoadSucceeded = true;
        return true;
    }
}
