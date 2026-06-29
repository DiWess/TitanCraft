using TitanCraft.Missions;

namespace TitanCraft.SaveSystem;

public sealed class CrashSiteSaveData
{
    // Constant for versioning save files — used to validate compatibility
    public const int CurrentSaveVersion = 1;

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
    public bool GalaxabrainComponentCollected { get; set; }
    public bool IsGalaxabrainComponentCollected
    {
        get => GalaxabrainComponentCollected;
        set => GalaxabrainComponentCollected = value;
    }
    public bool IsGalaxabrainDefeated { get; set; }
    public bool IsBeaconActivated { get; set; }
    public CrashSiteMissionStep MissionStep { get; set; }

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
            GalaxabrainComponentCollected = false,
            IsGalaxabrainDefeated = false,
            IsBeaconActivated = false,
            MissionStep = CrashSiteMissionStep.CollectResources
        };
    }
}
