using Godot;
using TitanCraft.Enemies;
using TitanCraft.Player;
using TitanCraft.UI;
using TitanCraft.World;

namespace TitanCraft.SaveSystem;

public partial class CrashSiteSaveCoordinator : Node
{
    [Export] public NodePath PlayerPath { get; set; } = "../Player";
    [Export] public NodePath PauseMenuPath { get; set; } = "../PauseMenu";
    [Export] public NodePath SavePointPath { get; set; } = "../Placeholder_SavePoint";
    [Export] public NodePath GalaxabrainPath { get; set; } = "../Placeholder_GalaxabrainScout";
    [Export] public NodePath BeaconPath { get; set; } = "../Placeholder_Beacon";
    [Export] public NodePath WorkbenchPath { get; set; } = "../Placeholder_Workbench";
    [Export] public string SavePath { get; set; } = LocalSaveGameStore.DefaultSavePath;

    public bool LastSaveSucceeded { get; private set; }
    public bool LastLoadSucceeded { get; private set; }

    private FirstPersonController _player = null!;
    private PauseMenu _pauseMenu = null!;
    private SavePoint? _savePoint;
    private GalaxabrainScout? _galaxabrain;
    private Beacon? _beacon;
    private Workbench? _workbench;

    public override void _Ready()
    {
        _player = GetNode<FirstPersonController>(PlayerPath);
        _pauseMenu = GetNode<PauseMenu>(PauseMenuPath);
        _savePoint = GetNodeOrNull<SavePoint>(SavePointPath);
        _galaxabrain = GetNodeOrNull<GalaxabrainScout>(GalaxabrainPath);
        _beacon = GetNodeOrNull<Beacon>(BeaconPath);
        _workbench = GetNodeOrNull<Workbench>(WorkbenchPath);
        _pauseMenu.SaveRequested += SaveGame;
        if (_savePoint is not null)
            _savePoint.SaveRequested += SaveGame;
        LoadGameIfPresent();
    }

    public override void _ExitTree()
    {
        if (_pauseMenu is not null)
            _pauseMenu.SaveRequested -= SaveGame;
        if (_savePoint is not null)
            _savePoint.SaveRequested -= SaveGame;
    }

    public void SaveGame()
    {
        var position = _player.GlobalPosition;
        var missionStep = _player.Mission.CurrentStep;
        LastSaveSucceeded = false;
        LocalSaveGameStore.Save(new CrashSiteSaveData
        {
            CheckpointId = _savePoint?.CheckpointId ?? string.Empty,
            PlayerX = position.X,
            PlayerY = position.Y,
            PlayerZ = position.Z,
            Health = _player.Health.CurrentHealth,
            Metal = _player.Inventory.Metal,
            Biomass = _player.Inventory.Biomass,
            ElectronicComponents = _player.Inventory.ElectronicComponents,
            MechanicalArmBuilt = _player.Inventory.IsMechanicalArmBuilt,
            GalaxabrainComponentCollected = _player.Inventory.HasGalaxabrainComponent,
            IsGalaxabrainDefeated = CrashSiteStateReconciler.RequiresGalaxabrainDefeated(missionStep),
            IsBeaconActivated = CrashSiteStateReconciler.RequiresBeaconActivated(missionStep),
            MissionStep = missionStep,
        }, SavePath);
        LastSaveSucceeded = true;
    }

    public bool LoadGameIfPresent()
    {
        LastLoadSucceeded = false;
        if (!LocalSaveGameStore.TryLoad(out var saveData, SavePath))
            return false;

        var missionStep = CrashSiteStateReconciler.ReconcileLoadedMissionStep(saveData);
        bool isMechanicalArmBuilt = saveData.IsMechanicalArmBuilt || CrashSiteStateReconciler.RequiresMechanicalArmBuilt(missionStep);
        _player.GlobalPosition = new Vector3(saveData.PlayerX, saveData.PlayerY, saveData.PlayerZ);
        _player.Health.Restore(saveData.Health);
        _player.Inventory.Restore(
            saveData.Metal,
            saveData.Biomass,
            saveData.ElectronicComponents,
            isMechanicalArmBuilt,
            saveData.IsGalaxabrainComponentCollected && CrashSiteStateReconciler.RequiresComponentCollected(missionStep));
        _player.Mission.Restore(missionStep);

        if (CrashSiteStateReconciler.RequiresGalaxabrainDefeated(missionStep))
            _galaxabrain?.RestoreDefeated(CrashSiteStateReconciler.IsComponentAvailable(missionStep));
        _beacon?.RestoreActivated(CrashSiteStateReconciler.RequiresBeaconActivated(missionStep));
        _workbench?.RestoreArmState(isMechanicalArmBuilt);

        LastLoadSucceeded = true;
        return true;
    }
}
