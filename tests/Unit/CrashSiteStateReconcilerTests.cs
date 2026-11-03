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
    public void TheComponentIsCarriedOnlyUntilItIsFittedAsMk2()
    {
        AssertThat(CrashSiteStateReconciler.RequiresComponentCollected(CrashSiteMissionStep.UpgradeArmMk2)).IsTrue();
        AssertThat(CrashSiteStateReconciler.IsComponentAvailable(CrashSiteMissionStep.UpgradeArmMk2)).IsFalse();
        foreach (var step in new[] { CrashSiteMissionStep.OpenNetYard, CrashSiteMissionStep.ActivateBeacon, CrashSiteMissionStep.Victory })
        {
            AssertThat(CrashSiteStateReconciler.RequiresComponentCollected(step)).IsFalse();
            AssertThat(CrashSiteStateReconciler.IsComponentAvailable(step)).IsFalse();
        }
    }

    [TestCase]
    public void BeaconStepRequiresThePowerCoreAndInertPickups()
    {
        const CrashSiteMissionStep step = CrashSiteMissionStep.ActivateBeacon;

        AssertThat(CrashSiteStateReconciler.RequiresGalaxabrainDefeated(step)).IsTrue();
        AssertThat(CrashSiteStateReconciler.RequiresWardenDefeated(step)).IsTrue();
        AssertThat(CrashSiteStateReconciler.RequiresPowerCoreCollected(step)).IsTrue();
        AssertThat(CrashSiteStateReconciler.IsPowerCoreAvailable(step)).IsFalse();
        AssertThat(CrashSiteStateReconciler.RequiredArmLevel(step)).IsEqual(3);
        AssertThat(CrashSiteStateReconciler.RequiresBeaconActivated(step)).IsFalse();
    }

    [TestCase]
    public void VictoryStepRequiresEverythingIncludingActiveBeacon()
    {
        const CrashSiteMissionStep step = CrashSiteMissionStep.Victory;

        AssertThat(CrashSiteStateReconciler.RequiresMechanicalArmBuilt(step)).IsTrue();
        AssertThat(CrashSiteStateReconciler.RequiresGalaxabrainDefeated(step)).IsTrue();
        AssertThat(CrashSiteStateReconciler.RequiresNetYardOpen(step)).IsTrue();
        AssertThat(CrashSiteStateReconciler.RequiresDroneNestCleared(step)).IsTrue();
        AssertThat(CrashSiteStateReconciler.RequiresTowerGateBroken(step)).IsTrue();
        AssertThat(CrashSiteStateReconciler.RequiresWardenDefeated(step)).IsTrue();
        AssertThat(CrashSiteStateReconciler.IsComponentAvailable(step)).IsFalse();
        AssertThat(CrashSiteStateReconciler.RequiresBeaconActivated(step)).IsTrue();
    }

    [TestCase]
    public void GatesAndEncountersFollowTheirOwnSteps()
    {
        AssertThat(CrashSiteStateReconciler.RequiresNetYardOpen(CrashSiteMissionStep.OpenNetYard)).IsFalse();
        AssertThat(CrashSiteStateReconciler.RequiresNetYardOpen(CrashSiteMissionStep.ClearDroneNest)).IsTrue();
        AssertThat(CrashSiteStateReconciler.RequiresDroneNestCleared(CrashSiteMissionStep.ClearDroneNest)).IsFalse();
        AssertThat(CrashSiteStateReconciler.RequiresDroneNestCleared(CrashSiteMissionStep.UpgradeArmMk3)).IsTrue();
        AssertThat(CrashSiteStateReconciler.RequiresTowerGateBroken(CrashSiteMissionStep.BreachTowerGate)).IsFalse();
        AssertThat(CrashSiteStateReconciler.RequiresTowerGateBroken(CrashSiteMissionStep.DefeatWarden)).IsTrue();
        AssertThat(CrashSiteStateReconciler.RequiresWardenDefeated(CrashSiteMissionStep.DefeatWarden)).IsFalse();
        AssertThat(CrashSiteStateReconciler.IsPowerCoreAvailable(CrashSiteMissionStep.RecoverPowerCore)).IsTrue();
        AssertThat(CrashSiteStateReconciler.RequiredArmLevel(CrashSiteMissionStep.UpgradeArmMk2)).IsEqual(1);
        AssertThat(CrashSiteStateReconciler.RequiredArmLevel(CrashSiteMissionStep.OpenNetYard)).IsEqual(2);
        AssertThat(CrashSiteStateReconciler.RequiredArmLevel(CrashSiteMissionStep.UpgradeArmMk3)).IsEqual(2);
        AssertThat(CrashSiteStateReconciler.RequiredArmLevel(CrashSiteMissionStep.BreachTowerGate)).IsEqual(3);
    }

    [TestCase]
    public void LoadReconciliationDoesNotSkipAModuleTheSaveNeverBuilt()
    {
        var saveData = new CrashSiteSaveData
        {
            ArmLevel = 2,
            IsMechanicalArmBuilt = true,
            IsGalaxabrainDefeated = true,
            HasPowerCore = true,
            MissionStep = CrashSiteMissionStep.ActivateBeacon,
        };

        AssertThat(CrashSiteStateReconciler.ReconcileLoadedMissionStep(saveData)).IsEqual(CrashSiteMissionStep.UpgradeArmMk3);
    }

    [TestCase]
    public void LoadReconciliationDoesNotGrantThePowerCoreFromAStepAlone()
    {
        var saveData = new CrashSiteSaveData
        {
            ArmLevel = 3,
            IsMechanicalArmBuilt = true,
            IsGalaxabrainDefeated = true,
            HasPowerCore = false,
            MissionStep = CrashSiteMissionStep.ActivateBeacon,
        };

        AssertThat(CrashSiteStateReconciler.ReconcileLoadedMissionStep(saveData)).IsEqual(CrashSiteMissionStep.RecoverPowerCore);
    }

    [TestCase]
    public void Version1SavesAreMigratedNotDiscarded()
    {
        var oldBeaconSave = new CrashSiteSaveData
        {
            SaveVersion = 1,
            MechanicalArmBuilt = true,
            GalaxabrainComponentCollected = true,
            IsGalaxabrainDefeated = true,
            MissionStep = (CrashSiteMissionStep)4,
        };

        AssertThat(CrashSiteSaveMigration.TryMigrate(oldBeaconSave)).IsTrue();
        AssertThat(oldBeaconSave.SaveVersion).IsEqual(CrashSiteSaveData.CurrentSaveVersion);
        AssertThat(oldBeaconSave.ArmLevel).IsEqual(1);
        AssertThat(oldBeaconSave.MissionStep).IsEqual(CrashSiteMissionStep.UpgradeArmMk2);
        AssertThat(CrashSiteStateReconciler.ReconcileLoadedMissionStep(oldBeaconSave)).IsEqual(CrashSiteMissionStep.UpgradeArmMk2);

        var oldEarlySave = new CrashSiteSaveData { SaveVersion = 1, MissionStep = CrashSiteMissionStep.BuildMechanicalArm };
        AssertThat(CrashSiteSaveMigration.TryMigrate(oldEarlySave)).IsTrue();
        AssertThat(oldEarlySave.MissionStep).IsEqual(CrashSiteMissionStep.BuildMechanicalArm);

        AssertThat(CrashSiteSaveMigration.TryMigrate(new CrashSiteSaveData { SaveVersion = 99 })).IsFalse();
        AssertThat(CrashSiteSaveMigration.TryMigrate(new CrashSiteSaveData { SaveVersion = 1, MissionStep = (CrashSiteMissionStep)9 })).IsFalse();
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

    [TestCase]
    public void LoadReconciliationDoesNotGrantComponentBeforePickupFlag()
    {
        var saveData = new CrashSiteSaveData
        {
            IsMechanicalArmBuilt = true,
            IsGalaxabrainDefeated = true,
            IsGalaxabrainComponentCollected = false,
            MissionStep = CrashSiteMissionStep.ActivateBeacon,
        };

        var reconciledStep = CrashSiteStateReconciler.ReconcileLoadedMissionStep(saveData);

        AssertThat(reconciledStep).IsEqual(CrashSiteMissionStep.RecoverGalaxabrainComponent);
    }

    [TestCase]
    public void LoadReconciliationDoesNotSkipScoutDefeatFromInconsistentSave()
    {
        var saveData = new CrashSiteSaveData
        {
            IsMechanicalArmBuilt = true,
            IsGalaxabrainDefeated = false,
            IsGalaxabrainComponentCollected = true,
            MissionStep = CrashSiteMissionStep.ActivateBeacon,
        };

        var reconciledStep = CrashSiteStateReconciler.ReconcileLoadedMissionStep(saveData);

        AssertThat(reconciledStep).IsEqual(CrashSiteMissionStep.DefeatGalaxabrain);
    }

    [TestCase]
    public void LoadReconciliationPreservesCraftedArmBeforeScoutFight()
    {
        var saveData = new CrashSiteSaveData
        {
            IsMechanicalArmBuilt = true,
            IsGalaxabrainDefeated = false,
            IsGalaxabrainComponentCollected = false,
            MissionStep = CrashSiteMissionStep.DefeatGalaxabrain,
        };

        var reconciledStep = CrashSiteStateReconciler.ReconcileLoadedMissionStep(saveData);

        AssertThat(reconciledStep).IsEqual(CrashSiteMissionStep.DefeatGalaxabrain);
        AssertThat(saveData.IsMechanicalArmBuilt).IsTrue();
    }

    [TestCase]
    public void LoadReconciliationDowngradesMissingMechanicalArmBeforeCombat()
    {
        var saveData = new CrashSiteSaveData
        {
            IsMechanicalArmBuilt = false,
            IsGalaxabrainDefeated = true,
            IsGalaxabrainComponentCollected = true,
            MissionStep = CrashSiteMissionStep.ActivateBeacon,
        };

        var reconciledStep = CrashSiteStateReconciler.ReconcileLoadedMissionStep(saveData);

        AssertThat(reconciledStep).IsEqual(CrashSiteMissionStep.BuildMechanicalArm);
    }
}
