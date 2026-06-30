using GdUnit4;
using TitanCraft.Missions;
using TitanCraft.SaveSystem;
using static GdUnit4.Assertions;

namespace TitanCraft.Tests.Unit;

[TestSuite]
public sealed class CrashSiteSaveServiceTests
{
    [TestCase]
    public void SerializeAndDeserializePreservesCrashSiteProgress()
    {
        var saveData = new CrashSiteSaveData
        {
            CheckpointId = "crash_site_save_point",
            PlayerX = 3.0f,
            PlayerY = 2.0f,
            PlayerZ = -7.0f,
            Health = 60,
            Metal = 10,
            Biomass = 3,
            ElectronicComponents = 2,
            IsMechanicalArmBuilt = true,
            IsGalaxabrainDefeated = true,
            IsGalaxabrainComponentCollected = true,
            IsBeaconActivated = true,
            MissionStep = CrashSiteMissionStep.Victory,
        };

        var json = CrashSiteSaveService.Serialize(saveData);
        var loadedSaveData = CrashSiteSaveService.DeserializeOrNewGame(json);

        AssertThat(loadedSaveData.SaveVersion).IsEqual(CrashSiteSaveData.CurrentSaveVersion);
        AssertThat(loadedSaveData.CheckpointId).IsEqual("crash_site_save_point");
        AssertThat(loadedSaveData.PlayerX).IsEqual(3.0f);
        AssertThat(loadedSaveData.PlayerY).IsEqual(2.0f);
        AssertThat(loadedSaveData.PlayerZ).IsEqual(-7.0f);
        AssertThat(loadedSaveData.Health).IsEqual(60);
        AssertThat(loadedSaveData.Metal).IsEqual(10);
        AssertThat(loadedSaveData.Biomass).IsEqual(3);
        AssertThat(loadedSaveData.ElectronicComponents).IsEqual(2);
        AssertThat(loadedSaveData.IsMechanicalArmBuilt).IsTrue();
        AssertThat(loadedSaveData.IsGalaxabrainDefeated).IsTrue();
        AssertThat(loadedSaveData.IsGalaxabrainComponentCollected).IsTrue();
        AssertThat(loadedSaveData.IsBeaconActivated).IsTrue();
        AssertThat(loadedSaveData.MissionStep).IsEqual(CrashSiteMissionStep.Victory);
    }

    [TestCase]
    public void DeserializeMissingSavePayloadFallsBackToNewGame()
    {
        var saveData = CrashSiteSaveService.DeserializeOrNewGame(null);

        AssertThat(saveData.SaveVersion).IsEqual(CrashSiteSaveData.CurrentSaveVersion);
        AssertThat(saveData.CheckpointId).IsEqual(string.Empty);
        AssertThat(saveData.PlayerX).IsEqual(0f);
        AssertThat(saveData.PlayerY).IsEqual(0f);
        AssertThat(saveData.PlayerZ).IsEqual(0f);
        AssertThat(saveData.Health).IsEqual(100);
        AssertThat(saveData.Metal).IsEqual(0);
        AssertThat(saveData.Biomass).IsEqual(0);
        AssertThat(saveData.ElectronicComponents).IsEqual(0);
        AssertThat(saveData.IsMechanicalArmBuilt).IsFalse();
        AssertThat(saveData.IsGalaxabrainDefeated).IsFalse();
        AssertThat(saveData.IsGalaxabrainComponentCollected).IsFalse();
        AssertThat(saveData.IsBeaconActivated).IsFalse();
        AssertThat(saveData.MissionStep).IsEqual(CrashSiteMissionStep.CollectResources);
    }

    [TestCase]
    public void DeserializePartiallyInvalidSaveFallsBackToNewGame()
    {
        const string invalidJson = """
            {
              "SaveVersion": 1,
              "CheckpointId": "crash_site_save_point",
              "Metal": -1,
              "Biomass": 3,
              "ElectronicComponents": 2,
              "MissionStep": "ActivateBeacon"
            }
            """;

        var saveData = CrashSiteSaveService.DeserializeOrNewGame(invalidJson);

        AssertThat(saveData.CheckpointId).IsEqual(string.Empty);
        AssertThat(saveData.PlayerX).IsEqual(0f);
        AssertThat(saveData.PlayerY).IsEqual(0f);
        AssertThat(saveData.PlayerZ).IsEqual(0f);
        AssertThat(saveData.Health).IsEqual(100);
        AssertThat(saveData.Metal).IsEqual(0);
        AssertThat(saveData.MissionStep).IsEqual(CrashSiteMissionStep.CollectResources);
    }

    [TestCase]
    public void DeserializeUnknownVersionFallsBackToNewGame()
    {
        const string unknownVersionJson = """
            {
              "SaveVersion": 999,
              "CheckpointId": "future_checkpoint",
              "Metal": 10,
              "Biomass": 3,
              "ElectronicComponents": 2,
              "MissionStep": "Victory"
            }
            """;

        var saveData = CrashSiteSaveService.DeserializeOrNewGame(unknownVersionJson);

        AssertThat(saveData.SaveVersion).IsEqual(CrashSiteSaveData.CurrentSaveVersion);
        AssertThat(saveData.CheckpointId).IsEqual(string.Empty);
        AssertThat(saveData.MissionStep).IsEqual(CrashSiteMissionStep.CollectResources);
    }
}
