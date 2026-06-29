using TitanCraft.Missions;

namespace TitanCraft.SaveSystem;

public sealed class CrashSiteSaveData
{
    // Constant for versioning save files — used to validate compatibility
    public const int CurrentSaveVersion = 1;

    public int SaveVersion { get; set; } = CurrentSaveVersion;
    public float PlayerX { get; set; }
    public float PlayerY { get; set; }
    public float PlayerZ { get; set; }
    public int Health { get; set; }
    public int Metal { get; set; }
    public int Biomass { get; set; }
    public int ElectronicComponents { get; set; }
    public bool MechanicalArmBuilt { get; set; }
    public bool GalaxabrainComponentCollected { get; set; }
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
            PlayerX = 0f,
            PlayerY = 0f,
            PlayerZ = 0f,
            Health = 100,
            Metal = 0,
            Biomass = 0,
            ElectronicComponents = 0,
            MechanicalArmBuilt = false,
            GalaxabrainComponentCollected = false,
            // Initialize to the first mission step, consistent with CrashSiteMissionState default
            MissionStep = CrashSiteMissionStep.CollectResources
        };
    }
}
