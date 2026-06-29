using TitanCraft.Missions;

namespace TitanCraft.SaveSystem;

public sealed class CrashSiteSaveData
{
    public int SaveVersion { get; set; } = 1;
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
}
