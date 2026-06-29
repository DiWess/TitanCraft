using System.Text.Json.Serialization;
using TitanCraft.Missions;

namespace TitanCraft.SaveSystem;

public sealed class CrashSiteSaveData
{
    public const int CurrentSaveVersion = 1;

    [JsonPropertyName("save_version")]
    public int SaveVersion { get; init; } = CurrentSaveVersion;

    [JsonPropertyName("checkpoint_id")]
    public string CheckpointId { get; init; } = string.Empty;

    [JsonPropertyName("metal")]
    public int Metal { get; init; }

    [JsonPropertyName("biomass")]
    public int Biomass { get; init; }

    [JsonPropertyName("electronic_components")]
    public int ElectronicComponents { get; init; }

    [JsonPropertyName("mechanical_arm_built")]
    public bool IsMechanicalArmBuilt { get; init; }

    [JsonPropertyName("galaxabrain_defeated")]
    public bool IsGalaxabrainDefeated { get; init; }

    [JsonPropertyName("galaxabrain_component_collected")]
    public bool IsGalaxabrainComponentCollected { get; init; }

    [JsonPropertyName("beacon_activated")]
    public bool IsBeaconActivated { get; init; }

    [JsonPropertyName("mission_step")]
    [JsonConverter(typeof(JsonStringEnumConverter<CrashSiteMissionStep>))]
    public CrashSiteMissionStep MissionStep { get; init; } = CrashSiteMissionStep.CollectResources;

    public static CrashSiteSaveData NewGame()
    {
        return new CrashSiteSaveData();
    }
}
