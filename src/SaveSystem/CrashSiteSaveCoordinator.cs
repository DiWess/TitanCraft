using System.Collections.Generic;
using System.Linq;
using Godot;
using TitanCraft.Core;
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
    [Export] public NodePath IntroTitleCardPath { get; set; } = "../IntroTitleCard";
    [Export] public string SavePath { get; set; } = LocalSaveGameStore.DefaultSavePath;

    /// <summary>Scene group of nodes that ask for an automatic checkpoint (workbench, power core).</summary>
    public const string CheckpointGroup = "checkpoint_sources";

    public bool LastSaveSucceeded { get; private set; }
    public bool LastLoadSucceeded { get; private set; }

    private FirstPersonController _player = null!;
    private PauseMenu _pauseMenu = null!;
    private SavePoint? _savePoint;
    private GalaxabrainScout? _galaxabrain;
    private Beacon? _beacon;
    private Workbench? _workbench;
    private IntroTitleCard? _introTitleCard;
    private PowerCorePickup? _powerCore;
    private readonly HashSet<string> _collectedPickups = new();
    private readonly HashSet<string> _readNotes = new();

    /// <summary>Time played on this run (this session plus what the save carried).</summary>
    public float PlaySeconds { get; private set; }

    public override void _Process(double delta)
    {
        // Paused time (the pause menu) does not count: this node pauses with the tree.
        PlaySeconds += (float)delta;
    }

    public (int Read, int Total) Discoveries()
    {
        var notes = StoryNotes().ToList();
        return (notes.Count(note => note.HasBeenRead), notes.Count);
    }

    private IEnumerable<StoryNote> StoryNotes()
    {
        var root = GetParent();
        return root is null
            ? Enumerable.Empty<StoryNote>()
            : root.FindChildren("*", "", true, false).OfType<StoryNote>();
    }

    public override void _Ready()
    {
        _player = GetNode<FirstPersonController>(PlayerPath);
        _pauseMenu = GetNode<PauseMenu>(PauseMenuPath);
        _savePoint = GetNodeOrNull<SavePoint>(SavePointPath);
        _galaxabrain = GetNodeOrNull<GalaxabrainScout>(GalaxabrainPath);
        _beacon = GetNodeOrNull<Beacon>(BeaconPath);
        _workbench = GetNodeOrNull<Workbench>(WorkbenchPath);
        _introTitleCard = GetNodeOrNull<IntroTitleCard>(IntroTitleCardPath);
        _pauseMenu.SaveRequested += SaveGame;
        if (_savePoint is not null)
            _savePoint.SaveRequested += SaveCheckpoint;

        foreach (var drop in ResourceDrops())
            drop.ResourceCollected += _ => _collectedPickups.Add(drop.Name);

        // Finished crafts and the power core are automatic checkpoints.
        if (_workbench is not null)
            _workbench.Crafted += OnCrafted;
        _powerCore = GetParent()?.FindChildren("*", "", true, false).OfType<PowerCorePickup>().FirstOrDefault();
        if (_powerCore is not null)
            _powerCore.Collected += SaveCheckpoint;

        // Fresh run (no save to restore): play the title card once. A loaded
        // run should never replay it, so this only fires on the false branch.
        if (!LoadGameIfPresent())
        {
            _introTitleCard?.Play();
        }
    }

    public override void _ExitTree()
    {
        if (_pauseMenu is not null)
            _pauseMenu.SaveRequested -= SaveGame;
        if (_savePoint is not null)
            _savePoint.SaveRequested -= SaveCheckpoint;
        if (_workbench is not null)
            _workbench.Crafted -= OnCrafted;
        if (_powerCore is not null)
            _powerCore.Collected -= SaveCheckpoint;
    }

    private void OnCrafted(int tier) => SaveCheckpoint();

    /// <summary>
    /// Automatic checkpoint (a finished craft, the power core): the suit is
    /// repaired and the run saved where the player stands, so a death in the
    /// next encounter costs that encounter, not the act before it.
    /// </summary>
    public void SaveCheckpoint()
    {
        _player.Health.RestoreToFull();
        SaveGame();
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
            ArmLevel = _player.Inventory.ArmLevel,
            GalaxabrainComponentCollected = _player.Inventory.HasGalaxabrainComponent,
            IsGalaxabrainDefeated = CrashSiteStateReconciler.RequiresGalaxabrainDefeated(missionStep),
            HasPowerCore = _player.Inventory.HasPowerCore,
            IsBeaconActivated = CrashSiteStateReconciler.RequiresBeaconActivated(missionStep),
            MissionStep = missionStep,
            CollectedPickups = _collectedPickups.OrderBy(name => name).ToList(),
            ReadNotes = StoryNotes().Where(note => note.HasBeenRead).Select(note => note.NoteId).OrderBy(id => id).ToList(),
            PlaySeconds = PlaySeconds,
        }, SavePath);
        LastSaveSucceeded = true;
        AudioCue.Play(this, "AudioLayer_Save/Save_Complete");
    }

    public bool LoadGameIfPresent()
    {
        LastLoadSucceeded = false;
        if (!LocalSaveGameStore.TryLoad(out var saveData, SavePath))
            return false;

        var missionStep = CrashSiteStateReconciler.ReconcileLoadedMissionStep(saveData);
        var armLevel = System.Math.Max(
            System.Math.Max(saveData.ArmLevel, saveData.IsMechanicalArmBuilt ? 1 : 0),
            CrashSiteStateReconciler.RequiredArmLevel(missionStep));
        _player.GlobalPosition = new Vector3(saveData.PlayerX, saveData.PlayerY, saveData.PlayerZ);
        _player.Health.Restore(saveData.Health);
        _player.Inventory.Restore(
            saveData.Metal,
            saveData.Biomass,
            saveData.ElectronicComponents,
            armLevel,
            CrashSiteStateReconciler.RequiresComponentCollected(missionStep),
            CrashSiteStateReconciler.RequiresPowerCoreCollected(missionStep));
        _player.Mission.Restore(missionStep);

        if (CrashSiteStateReconciler.RequiresGalaxabrainDefeated(missionStep))
            _galaxabrain?.RestoreDefeated(CrashSiteStateReconciler.IsComponentAvailable(missionStep));
        _beacon?.RestoreActivated(CrashSiteStateReconciler.RequiresBeaconActivated(missionStep));
        _workbench?.RestoreArmState(armLevel >= 1);

        foreach (var name in saveData.CollectedPickups ?? new List<string>())
            _collectedPickups.Add(name);
        foreach (var id in saveData.ReadNotes ?? new List<string>())
            _readNotes.Add(id);
        PlaySeconds = float.IsFinite(saveData.PlaySeconds) ? System.Math.Max(0f, saveData.PlaySeconds) : 0f;

        // Deferred so every world object has run _Ready first: a node that
        // sets itself up after being restored would undo the restore.
        Callable.From(() => RestoreWorld(missionStep)).CallDeferred();

        LastLoadSucceeded = true;
        AudioCue.Play(this, "AudioLayer_Save/Load_Complete");
        return true;
    }

    private void RestoreWorld(Missions.CrashSiteMissionStep missionStep)
    {
        if (!IsInsideTree())
            return;

        foreach (var drop in ResourceDrops().Where(drop => _collectedPickups.Contains(drop.Name)))
            drop.RestoreCollected();
        foreach (var note in StoryNotes().Where(note => _readNotes.Contains(note.NoteId)))
            note.RestoreRead();

        var root = GetParent();
        foreach (var node in GetTree().GetNodesInGroup(IMissionRestorable.Group))
        {
            // Only this game scene's objects: tests hold several at once.
            if (node is IMissionRestorable restorable && root.IsAncestorOf(node))
                restorable.RestoreFromMission(missionStep);
        }
    }

    private IEnumerable<ResourceDrop> ResourceDrops()
    {
        var root = GetParent();
        return root is null
            ? Enumerable.Empty<ResourceDrop>()
            : root.FindChildren("*", "", true, false).OfType<ResourceDrop>();
    }
}
