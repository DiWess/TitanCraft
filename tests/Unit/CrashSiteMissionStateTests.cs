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
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.UpgradeArmMk2);
        AssertThat(mission.CurrentPhase).IsEqual(CrashSiteMissionPhase.NetYard);
        AssertThat(mission.TryCompleteMk2Upgrade()).IsTrue();
        AssertThat(mission.TryCompleteNetYardOpening()).IsTrue();
        AssertThat(mission.TryCompleteDroneNest()).IsTrue();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.UpgradeArmMk3);
        AssertThat(mission.CurrentPhase).IsEqual(CrashSiteMissionPhase.Tower);
        AssertThat(mission.TryCompleteMk3Upgrade()).IsTrue();
        AssertThat(mission.TryCompleteTowerBreach()).IsTrue();
        AssertThat(mission.TryCompleteWardenDefeat()).IsTrue();
        AssertThat(mission.TryCompletePowerCoreRecovery()).IsTrue();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.ActivateBeacon);
        AssertThat(mission.CurrentPhase).IsEqual(CrashSiteMissionPhase.Beacon);

        // Activation starts the holdout; it is not yet the win.
        AssertThat(mission.TryCompleteBeaconActivation()).IsTrue();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.DefendBeacon);
        AssertThat(mission.IsVictory).IsFalse();
        AssertThat(mission.TryCompleteBeaconDefence()).IsTrue();

        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.Victory);
        AssertThat(mission.CurrentObjectiveText).Contains("Beacon activated");
        // The phase title already reads "Mission Complete"; the breadcrumb used to
        // repeat it ("Mission Complete: Mission complete: beacon activated.").
        AssertThat(mission.HudBreadcrumb.ToLowerInvariant().Split("mission complete").Length - 1).IsEqual(1);
    }

    [TestCase]
    public void EveryStepHasPlayerFacingObjectiveText()
    {
        foreach (var step in System.Enum.GetValues<CrashSiteMissionStep>())
        {
            var mission = new CrashSiteMissionState();
            mission.Restore(step);
            AssertThat(mission.CurrentObjectiveText).IsNotEqual("Unknown Objective");
            AssertThat(mission.CurrentPhaseTitle).StartsWith(step == CrashSiteMissionStep.Victory ? "Mission" : "Phase");
        }
    }

    [TestCase]
    public void BeaconChargeShowsInTheObjectiveOnlyWhileDefending()
    {
        var mission = CompletedThrough(CrashSiteMissionStep.ActivateBeacon);
        mission.SetBeaconCharge(0.5f);
        AssertThat(mission.BeaconChargePercent).IsEqual(0);

        mission.TryCompleteBeaconActivation();
        var changes = 0;
        mission.Changed += _ => changes++;
        mission.SetBeaconCharge(0.421f);
        mission.SetBeaconCharge(0.425f);
        AssertThat(mission.BeaconChargePercent).IsEqual(42);
        AssertThat(changes).IsEqual(1);
        AssertThat(mission.CurrentObjectiveText).Contains("42%");

        // Charging never reports 100% on its own: only the defence completing does.
        mission.SetBeaconCharge(1f);
        AssertThat(mission.BeaconChargePercent).IsEqual(99);
        AssertThat(mission.IsVictory).IsFalse();
    }

    [TestCase]
    public void ARestoredHoldoutResumesAtActivation()
    {
        var mission = new CrashSiteMissionState();
        mission.Restore(CrashSiteMissionStep.DefendBeacon);

        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.ActivateBeacon);
        AssertThat(mission.TryCompleteBeaconActivation()).IsTrue();
    }

    internal static CrashSiteMissionState CompletedThrough(CrashSiteMissionStep step)
    {
        var mission = new CrashSiteMissionState();
        mission.Restore(step);
        return mission;
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
        var completions = new System.Func<bool>[]
        {
            mission.TryCompleteResourceCollection,
            mission.TryCompleteMechanicalArmConstruction,
            () => mission.TryCompleteGalaxabrainDefeat(isGalaxabrainDefeated: true),
            mission.TryCompleteComponentRecovery,
            mission.TryCompleteMk2Upgrade,
            mission.TryCompleteNetYardOpening,
            mission.TryCompleteDroneNest,
            mission.TryCompleteMk3Upgrade,
            mission.TryCompleteTowerBreach,
            mission.TryCompleteWardenDefeat,
            mission.TryCompletePowerCoreRecovery,
            mission.TryCompleteBeaconActivation,
            mission.TryCompleteBeaconDefence,
        };

        for (var i = 0; i < completions.Length; i++)
        {
            // Every later completion is refused before its turn.
            for (var later = i + 1; later < completions.Length; later++)
                AssertThat(completions[later]()).IsFalse();

            AssertThat(completions[i]()).IsTrue();
            AssertThat(completions[i]()).IsFalse();
            AssertThat((int)mission.CurrentStep).IsEqual(i + 1);
        }

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
        foreach (var step in System.Enum.GetValues<CrashSiteMissionStep>())
        {
            var mission = new CrashSiteMissionState();
            mission.Restore(step);

            var expected = step == CrashSiteMissionStep.DefendBeacon ? CrashSiteMissionStep.ActivateBeacon : step;
            AssertThat(mission.CurrentStep).IsEqual(expected);
            AssertThat(mission.CurrentObjectiveText).IsNotEqual("Unknown Objective");
        }

        // A restored intermediate step must remain completable, not softlocked.
        var restoredMission = new CrashSiteMissionState();
        restoredMission.Restore(CrashSiteMissionStep.RecoverGalaxabrainComponent);
        AssertThat(restoredMission.TryCompleteComponentRecovery()).IsTrue();
        AssertThat(restoredMission.CurrentStep).IsEqual(CrashSiteMissionStep.UpgradeArmMk2);
        AssertThat(restoredMission.TryCompleteMk2Upgrade()).IsTrue();
    }

    [TestCase]
    public void VictoryDoesNotAdvancePastFinalStep()
    {
        var mission = new CrashSiteMissionState();
        mission.Restore(CrashSiteMissionStep.ActivateBeacon);
        mission.TryCompleteBeaconActivation();
        mission.TryCompleteBeaconDefence();

        AssertThat(mission.TryCompleteBeaconActivation()).IsFalse();
        AssertThat(mission.TryCompleteBeaconDefence()).IsFalse();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.Victory);
    }
}
