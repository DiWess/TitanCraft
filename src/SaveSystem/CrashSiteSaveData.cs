using System.Collections.Generic;
using TitanCraft.Missions;

namespace TitanCraft.SaveSystem;

public sealed class CrashSiteSaveData
{
    // Constant for versioning save files — used to validate compatibility.
    // Version 2 (workstreams B and C): arm level, power core, collected
    // pickups, and the longer mission chain. Version 1 saves are migrated on
    // load (CrashSiteSaveMigration), not discarded.
    public const int CurrentSaveVersion = 2;
    public const int OldestSupportedSaveVersion = 1;

    public int SaveVersion { get; set; } = CurrentSaveVersion;
    public string CheckpointId { get; set; } = string.Empty;
    public float PlayerX { get; set; }
    public float PlayerY { get; set; }
    public float PlayerZ { get; set; }
    public int Health { get; set; }
    public int Metal { get; set; }
    public int Biomass { get; set; }
    public int ElectronicComponents { get; set; }
    public bool MechanicalArmBuilt { get; set; }
    public bool IsMechanicalArmBuilt
    {
        get => MechanicalArmBuilt;
        set => MechanicalArmBuilt = value;
    }

    /// <summary>0 before crafting; 1-3 for Mk I-III.</summary>
    public int ArmLevel { get; set; }
    public bool GalaxabrainComponentCollected { get; set; }
    public bool IsGalaxabrainComponentCollected
    {
        get => GalaxabrainComponentCollected;
        set => GalaxabrainComponentCollected = value;
    }
    public bool IsGalaxabrainDefeated { get; set; }
    public bool HasPowerCore { get; set; }
    public bool IsBeaconActivated { get; set; }
    public CrashSiteMissionStep MissionStep { get; set; }

    /// <summary>Scene names of resource pickups already taken, so a reload cannot hand them out twice.</summary>
    public List<string> CollectedPickups { get; set; } = new();

    /// <summary>Story notes already read (workstream D discoveries), by note id.</summary>
    public List<string> ReadNotes { get; set; } = new();

    /// <summary>Time played on this run, carried across loads, for the victory summary.</summary>
    public float PlaySeconds { get; set; }

    /// <summary>
    /// Factory method to create a new game save with default values.
    /// Used when starting a new game or when save data is invalid.
    /// </summary>
    public static CrashSiteSaveData NewGame()
    {
        return new CrashSiteSaveData
        {
            SaveVersion = CurrentSaveVersion,
            CheckpointId = string.Empty,
            PlayerX = 0f,
            PlayerY = 0f,
            PlayerZ = 0f,
            Health = 100,
            Metal = 0,
            Biomass = 0,
            ElectronicComponents = 0,
            MechanicalArmBuilt = false,
            ArmLevel = 0,
            GalaxabrainComponentCollected = false,
            IsGalaxabrainDefeated = false,
            HasPowerCore = false,
            IsBeaconActivated = false,
            MissionStep = CrashSiteMissionStep.CollectResources,
            CollectedPickups = new List<string>(),
            ReadNotes = new List<string>(),
            PlaySeconds = 0f,
        };
    }
}
