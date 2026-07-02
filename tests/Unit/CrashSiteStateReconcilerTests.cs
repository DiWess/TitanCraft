using GdUnit4;
using TitanCraft.Missions;
using TitanCraft.SaveSystem;
using static GdUnit4.Assertions;

namespace TitanCraft.Tests.Unit;

[TestSuite]
public sealed class CrashSiteStateReconcilerTests
{
    [TestCase]
    public void EarlyStepsReconstructALivingWorldWithoutArmOrComponent()
    {
        foreach (var step in new[] { CrashSiteMissionStep.CollectResources, CrashSiteMissionStep.BuildMechanicalArm })
        {
            AssertThat(CrashSiteStateReconciler.RequiresMechanicalArmBuilt(step)).IsFalse();
            AssertThat(CrashSiteStateReconciler.RequiresGalaxabrainDefeated(step)).IsFalse();
            AssertThat(CrashSiteStateReconciler.RequiresComponentCollected(step)).IsFalse();
            AssertThat(CrashSiteStateReconciler.IsComponentAvailable(step)).IsFalse();
            AssertThat(CrashSiteStateReconciler.RequiresBeaconActivated(step)).IsFalse();
        }
    }

    [TestCase]
    public void DefeatStepRequiresArmButKeepsGalaxabrainAlive()
    {
        const CrashSiteMissionStep step = CrashSiteMissionStep.DefeatGalaxabrain;

        AssertThat(CrashSiteStateReconciler.RequiresMechanicalArmBuilt(step)).IsTrue();
        AssertThat(CrashSiteStateReconciler.RequiresGalaxabrainDefeated(step)).IsFalse();
        AssertThat(CrashSiteStateReconciler.IsComponentAvailable(step)).IsFalse();
    }

    [TestCase]
    public void RecoveryStepRequiresDeadGalaxabrainWithAvailableComponent()
    {
        const CrashSiteMissionStep step = CrashSiteMissionStep.RecoverGalaxabrainComponent;

        AssertThat(CrashSiteStateReconciler.RequiresMechanicalArmBuilt(step)).IsTrue();
        AssertThat(CrashSiteStateReconciler.RequiresGalaxabrainDefeated(step)).IsTrue();
        AssertThat(CrashSiteStateReconciler.RequiresComponentCollected(step)).IsFalse();
        AssertThat(CrashSiteStateReconciler.IsComponentAvailable(step)).IsTrue();
        AssertThat(CrashSiteStateReconciler.RequiresBeaconActivated(step)).IsFalse();
    }

    [TestCase]
    public void BeaconStepRequiresCollectedComponentAndInertPickup()
    {
        const CrashSiteMissionStep step = CrashSiteMissionStep.ActivateBeacon;

        AssertThat(CrashSiteStateReconciler.RequiresGalaxabrainDefeated(step)).IsTrue();
        AssertThat(CrashSiteStateReconciler.RequiresComponentCollected(step)).IsTrue();
        AssertThat(CrashSiteStateReconciler.IsComponentAvailable(step)).IsFalse();
        AssertThat(CrashSiteStateReconciler.RequiresBeaconActivated(step)).IsFalse();
    }

    [TestCase]
    public void VictoryStepRequiresEverythingIncludingActiveBeacon()
    {
        const CrashSiteMissionStep step = CrashSiteMissionStep.Victory;

        AssertThat(CrashSiteStateReconciler.RequiresMechanicalArmBuilt(step)).IsTrue();
        AssertThat(CrashSiteStateReconciler.RequiresGalaxabrainDefeated(step)).IsTrue();
        AssertThat(CrashSiteStateReconciler.RequiresComponentCollected(step)).IsTrue();
        AssertThat(CrashSiteStateReconciler.IsComponentAvailable(step)).IsFalse();
        AssertThat(CrashSiteStateReconciler.RequiresBeaconActivated(step)).IsTrue();
    }

    [TestCase]
    public void TamperedFlagsCannotProduceImpossibleCombinations()
    {
        // A save claiming a dead Galaxabrain (or collected component) while the step
        // still precedes recovery must reconstruct a living enemy, otherwise the
        // DefeatGalaxabrain objective can never be completed again.
        var tampered = new CrashSiteSaveData
        {
            MissionStep = CrashSiteMissionStep.DefeatGalaxabrain,
            IsGalaxabrainDefeated = true,
            GalaxabrainComponentCollected = true,
            IsBeaconActivated = true,
            MechanicalArmBuilt = false,
        };

        AssertThat(CrashSiteStateReconciler.RequiresGalaxabrainDefeated(tampered.MissionStep)).IsFalse();
        AssertThat(CrashSiteStateReconciler.RequiresComponentCollected(tampered.MissionStep)).IsFalse();
        AssertThat(CrashSiteStateReconciler.RequiresBeaconActivated(tampered.MissionStep)).IsFalse();
        // And the arm must exist at this step even if the flag was stripped,
        // otherwise the attack is disabled and the fight is unwinnable.
        AssertThat(CrashSiteStateReconciler.RequiresMechanicalArmBuilt(tampered.MissionStep)).IsTrue();
    }
}
