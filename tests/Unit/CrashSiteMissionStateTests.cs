using System;
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

    [TestCase]
    public void GalaxabrainDefeatCompletesExactlyOnceAndDoesNotAutoRecoverComponent()
    {
        var mission = new CrashSiteMissionState();
        mission.TryCompleteResourceCollection();
        mission.TryCompleteMechanicalArmConstruction();

        AssertThat(mission.TryCompleteGalaxabrainDefeat(isGalaxabrainDefeated: true)).IsTrue();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.RecoverGalaxabrainComponent);

        // Repeated "enemy death" notifications (e.g. duplicate signals) must not re-trigger progression.
        AssertThat(mission.TryCompleteGalaxabrainDefeat(isGalaxabrainDefeated: true)).IsFalse();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.RecoverGalaxabrainComponent);

        // Defeating the enemy alone must never complete component recovery or later steps.
        AssertThat(mission.TryCompleteBeaconActivation()).IsFalse();
        AssertThat(mission.IsVictory).IsFalse();
    }

    [TestCase]
    public void ComponentRecoveryCompletesExactlyOnce()
    {
        var mission = new CrashSiteMissionState();
        mission.TryCompleteResourceCollection();
        mission.TryCompleteMechanicalArmConstruction();
        mission.TryCompleteGalaxabrainDefeat(isGalaxabrainDefeated: true);

        AssertThat(mission.TryCompleteComponentRecovery()).IsTrue();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.ActivateBeacon);

        AssertThat(mission.TryCompleteComponentRecovery()).IsFalse();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.ActivateBeacon);
    }

    [TestCase]
    public void BeaconActivationProducesVictoryExactlyOnce()
    {
        var mission = new CrashSiteMissionState();
        mission.TryCompleteResourceCollection();
        mission.TryCompleteMechanicalArmConstruction();
        mission.TryCompleteGalaxabrainDefeat(isGalaxabrainDefeated: true);
        mission.TryCompleteComponentRecovery();

        AssertThat(mission.TryCompleteBeaconActivation()).IsTrue();
        AssertThat(mission.IsVictory).IsTrue();

        AssertThat(mission.TryCompleteBeaconActivation()).IsFalse();
        AssertThat(mission.IsVictory).IsTrue();
    }

    [TestCase]
    public void RepeatedInteractionsRemainIdempotentAtEveryStep()
    {
        var mission = new CrashSiteMissionState();

        AssertThat(mission.TryCompleteResourceCollection()).IsTrue();
        AssertThat(mission.TryCompleteResourceCollection()).IsFalse();

        AssertThat(mission.TryCompleteMechanicalArmConstruction()).IsTrue();
        AssertThat(mission.TryCompleteMechanicalArmConstruction()).IsFalse();

        AssertThat(mission.TryCompleteGalaxabrainDefeat(isGalaxabrainDefeated: true)).IsTrue();
        AssertThat(mission.TryCompleteGalaxabrainDefeat(isGalaxabrainDefeated: true)).IsFalse();

        AssertThat(mission.TryCompleteComponentRecovery()).IsTrue();
        AssertThat(mission.TryCompleteComponentRecovery()).IsFalse();

        AssertThat(mission.TryCompleteBeaconActivation()).IsTrue();
        AssertThat(mission.TryCompleteBeaconActivation()).IsFalse();
    }

    [TestCase]
    public void RestoringEachSupportedMissionStepReconstructsAPlayableWorld()
    {
        foreach (CrashSiteMissionStep step in Enum.GetValues<CrashSiteMissionStep>())
        {
            var mission = new CrashSiteMissionState();
            mission.Restore(step);

            AssertThat(mission.CurrentStep).IsEqual(step);

            // From any restored step, exactly the single matching "next" transition must be
            // available; earlier transitions must be blocked (already completed) and later
            // ones must be blocked (not yet reachable).
            switch (step)
            {
                case CrashSiteMissionStep.CollectResources:
                    AssertThat(mission.TryCompleteResourceCollection()).IsTrue();
                    break;
                case CrashSiteMissionStep.BuildMechanicalArm:
                    AssertThat(mission.TryCompleteResourceCollection()).IsFalse();
                    AssertThat(mission.TryCompleteMechanicalArmConstruction()).IsTrue();
                    break;
                case CrashSiteMissionStep.DefeatGalaxabrain:
                    AssertThat(mission.TryCompleteMechanicalArmConstruction()).IsFalse();
                    AssertThat(mission.TryCompleteGalaxabrainDefeat(isGalaxabrainDefeated: true)).IsTrue();
                    break;
                case CrashSiteMissionStep.RecoverGalaxabrainComponent:
                    AssertThat(mission.TryCompleteGalaxabrainDefeat(isGalaxabrainDefeated: true)).IsFalse();
                    AssertThat(mission.TryCompleteComponentRecovery()).IsTrue();
                    break;
                case CrashSiteMissionStep.ActivateBeacon:
                    AssertThat(mission.TryCompleteComponentRecovery()).IsFalse();
                    AssertThat(mission.TryCompleteBeaconActivation()).IsTrue();
                    break;
                case CrashSiteMissionStep.Victory:
                    AssertThat(mission.TryCompleteBeaconActivation()).IsFalse();
                    AssertThat(mission.IsVictory).IsTrue();
                    break;
            }
        }
    }
}
