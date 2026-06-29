using GdUnit4;
using TitanCraft.Missions;
using static GdUnit4.Assertions;

namespace TitanCraft.Tests.Unit;

[TestSuite]
public sealed class CrashSiteMissionStateTests
{
    [TestCase]
    public void NewMissionStartsWithResourceCollectionObjective()
    {
        var mission = new CrashSiteMissionState();

        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.CollectResources);
        AssertThat(mission.CurrentObjectiveText).Contains("Collect");
    }

    [TestCase]
    public void ValidProgressionAdvancesThroughCrashSiteVictorySequence()
    {
        var mission = new CrashSiteMissionState();

        AssertThat(mission.TryCompleteResourceCollection()).IsTrue();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.BuildMechanicalArm);
        AssertThat(mission.TryCompleteMechanicalArmConstruction()).IsTrue();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.DefeatGalaxabrain);
        AssertThat(mission.TryCompleteGalaxabrainDefeat()).IsTrue();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.RecoverGalaxabrainComponent);
        AssertThat(mission.TryCompleteComponentRecovery()).IsTrue();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.ActivateBeacon);
        AssertThat(mission.TryCompleteBeaconActivation()).IsTrue();

        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.Victory);
        AssertThat(mission.CurrentObjectiveText).Contains("Mission complete");
    }

    [TestCase]
    public void OutOfOrderProgressionIsBlockedWithoutChangingCurrentStep()
    {
        var mission = new CrashSiteMissionState();

        AssertThat(mission.TryCompleteMechanicalArmConstruction()).IsFalse();
        AssertThat(mission.TryCompleteGalaxabrainDefeat()).IsFalse();
        AssertThat(mission.TryCompleteComponentRecovery()).IsFalse();
        AssertThat(mission.TryCompleteBeaconActivation()).IsFalse();

        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.CollectResources);
    }

    [TestCase]
    public void VictoryDoesNotAdvancePastFinalStep()
    {
        var mission = new CrashSiteMissionState();
        mission.TryCompleteResourceCollection();
        mission.TryCompleteMechanicalArmConstruction();
        mission.TryCompleteGalaxabrainDefeat();
        mission.TryCompleteComponentRecovery();
        mission.TryCompleteBeaconActivation();

        AssertThat(mission.TryCompleteBeaconActivation()).IsFalse();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.Victory);
    }
}
