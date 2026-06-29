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
              "save_version": 1,
              "checkpoint_id": "crash_site_save_point",
              "metal": -1,
              "biomass": 3,
              "electronic_components": 2,
              "mission_step": "ActivateBeacon"
            }
            """;

        var saveData = CrashSiteSaveService.DeserializeOrNewGame(invalidJson);

        AssertThat(saveData.CheckpointId).IsEqual(string.Empty);
        AssertThat(saveData.Metal).IsEqual(0);
        AssertThat(saveData.MissionStep).IsEqual(CrashSiteMissionStep.CollectResources);
    }

    [TestCase]
    public void DeserializeUnknownVersionFallsBackToNewGame()
    {
        const string unknownVersionJson = """
            {
              "save_version": 999,
              "checkpoint_id": "future_checkpoint",
              "metal": 10,
              "biomass": 3,
              "electronic_components": 2,
              "mission_step": "Victory"
            }
            """;

        var saveData = CrashSiteSaveService.DeserializeOrNewGame(unknownVersionJson);

        AssertThat(saveData.SaveVersion).IsEqual(CrashSiteSaveData.CurrentSaveVersion);
        AssertThat(saveData.CheckpointId).IsEqual(string.Empty);
        AssertThat(saveData.MissionStep).IsEqual(CrashSiteMissionStep.CollectResources);
    }
}
