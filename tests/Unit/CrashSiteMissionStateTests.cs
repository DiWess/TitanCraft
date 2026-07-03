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
        AssertThat(mission.CurrentPhase).IsEqual(CrashSiteMissionPhase.Scavenge);
        AssertThat(mission.HudBreadcrumb).Contains("Phase 2");
    }

    [TestCase]
    public void ValidProgressionAdvancesThroughCrashSiteVictorySequence()
    {
        var mission = new CrashSiteMissionState();

        AssertThat(mission.TryCompleteResourceCollection()).IsTrue();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.BuildMechanicalArm);
        AssertThat(mission.TryCompleteMechanicalArmConstruction()).IsTrue();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.DefeatGalaxabrain);
        AssertThat(mission.CurrentPhase).IsEqual(CrashSiteMissionPhase.ExtractConquer);
        AssertThat(mission.TryCompleteGalaxabrainDefeat(isGalaxabrainDefeated: true)).IsTrue();
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
        AssertThat(mission.TryCompleteGalaxabrainDefeat(isGalaxabrainDefeated: true)).IsFalse();
        AssertThat(mission.TryCompleteComponentRecovery()).IsFalse();
        AssertThat(mission.TryCompleteBeaconActivation()).IsFalse();

        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.CollectResources);
    }


    [TestCase]
    public void EachIntermediateStepBlocksLaterCompletionMethods()
    {
        var mission = new CrashSiteMissionState();

        AssertThat(mission.TryCompleteResourceCollection()).IsTrue();
        AssertThat(mission.TryCompleteGalaxabrainDefeat(isGalaxabrainDefeated: true)).IsFalse();
        AssertThat(mission.TryCompleteComponentRecovery()).IsFalse();
        AssertThat(mission.TryCompleteBeaconActivation()).IsFalse();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.BuildMechanicalArm);

        AssertThat(mission.TryCompleteMechanicalArmConstruction()).IsTrue();
        AssertThat(mission.TryCompleteComponentRecovery()).IsFalse();
        AssertThat(mission.TryCompleteBeaconActivation()).IsFalse();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.DefeatGalaxabrain);

        AssertThat(mission.TryCompleteGalaxabrainDefeat(isGalaxabrainDefeated: true)).IsTrue();
        AssertThat(mission.TryCompleteBeaconActivation()).IsFalse();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.RecoverGalaxabrainComponent);
    }

    [TestCase]
    public void DefeatStepDoesNotAdvanceBeforeGalaxabrainIsDead()
    {
        var mission = new CrashSiteMissionState();
        mission.TryCompleteResourceCollection();
        mission.TryCompleteMechanicalArmConstruction();

        AssertThat(mission.TryCompleteGalaxabrainDefeat(isGalaxabrainDefeated: false)).IsFalse();

        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.DefeatGalaxabrain);
    }

    [TestCase]
    public void EachCompletionMethodSucceedsExactlyOnce()
    {
        var mission = new CrashSiteMissionState();

        AssertThat(mission.TryCompleteResourceCollection()).IsTrue();
        AssertThat(mission.TryCompleteResourceCollection()).IsFalse();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.BuildMechanicalArm);

        AssertThat(mission.TryCompleteMechanicalArmConstruction()).IsTrue();
        AssertThat(mission.TryCompleteMechanicalArmConstruction()).IsFalse();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.DefeatGalaxabrain);

        AssertThat(mission.TryCompleteGalaxabrainDefeat(isGalaxabrainDefeated: true)).IsTrue();
        AssertThat(mission.TryCompleteGalaxabrainDefeat(isGalaxabrainDefeated: true)).IsFalse();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.RecoverGalaxabrainComponent);

        AssertThat(mission.TryCompleteComponentRecovery()).IsTrue();
        AssertThat(mission.TryCompleteComponentRecovery()).IsFalse();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.ActivateBeacon);

        AssertThat(mission.TryCompleteBeaconActivation()).IsTrue();
        AssertThat(mission.TryCompleteBeaconActivation()).IsFalse();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.Victory);
    }

    [TestCase]
    public void GalaxabrainDefeatDoesNotAutomaticallyCompleteComponentRecovery()
    {
        var mission = new CrashSiteMissionState();
        mission.TryCompleteResourceCollection();
        mission.TryCompleteMechanicalArmConstruction();

        AssertThat(mission.TryCompleteGalaxabrainDefeat(isGalaxabrainDefeated: true)).IsTrue();

        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.RecoverGalaxabrainComponent);
        AssertThat(mission.CurrentObjectiveText).Contains("Recover");
        AssertThat(mission.IsVictory).IsFalse();
    }

    [TestCase]
    public void ComponentRecoveryIsBlockedBeforeGalaxabrainDefeat()
    {
        var mission = new CrashSiteMissionState();
        mission.TryCompleteResourceCollection();
        mission.TryCompleteMechanicalArmConstruction();

        AssertThat(mission.TryCompleteComponentRecovery()).IsFalse();

        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.DefeatGalaxabrain);
    }

    [TestCase]
    public void RestoreReconstructsEverySupportedStepWithAdvanceableState()
    {
        foreach (var step in new[]
                 {
                     CrashSiteMissionStep.CollectResources,
                     CrashSiteMissionStep.BuildMechanicalArm,
                     CrashSiteMissionStep.DefeatGalaxabrain,
                     CrashSiteMissionStep.RecoverGalaxabrainComponent,
                     CrashSiteMissionStep.ActivateBeacon,
                     CrashSiteMissionStep.Victory,
                 })
        {
            var mission = new CrashSiteMissionState();
            mission.Restore(step);

            AssertThat(mission.CurrentStep).IsEqual(step);
            AssertThat(mission.CurrentObjectiveText).IsNotEqual("Unknown Objective");
        }

        // A restored intermediate step must remain completable, not softlocked.
        var restoredMission = new CrashSiteMissionState();
        restoredMission.Restore(CrashSiteMissionStep.RecoverGalaxabrainComponent);
        AssertThat(restoredMission.TryCompleteComponentRecovery()).IsTrue();
        AssertThat(restoredMission.TryCompleteBeaconActivation()).IsTrue();
        AssertThat(restoredMission.IsVictory).IsTrue();
    }

    [TestCase]
    public void VictoryDoesNotAdvancePastFinalStep()
    {
        var mission = new CrashSiteMissionState();
        mission.TryCompleteResourceCollection();
        mission.TryCompleteMechanicalArmConstruction();
        mission.TryCompleteGalaxabrainDefeat(isGalaxabrainDefeated: true);
        mission.TryCompleteComponentRecovery();
        mission.TryCompleteBeaconActivation();

        AssertThat(mission.TryCompleteBeaconActivation()).IsFalse();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.Victory);
    }
}
