using System;
using TitanCraft.Missions;

namespace TitanCraft.SaveSystem;

/// <summary>
/// Derives the world flags a load must reconstruct from the persisted mission step.
/// The mission step is authoritative: in every save the game itself writes, flags and
/// step agree (each flag mutation advances the step within the same interaction), so
/// deriving from the step is lossless for legitimate saves and prevents tampered or
/// corrupted files from reconstructing impossible states (e.g. a recovery objective
/// with a living Galaxabrain, or a built arm that cannot attack).
/// </summary>
public static class CrashSiteStateReconciler
{
    public static CrashSiteMissionStep ReconcileLoadedMissionStep(CrashSiteSaveData saveData)
    {
        var step = saveData.MissionStep;
        var armLevel = Math.Max(saveData.ArmLevel, saveData.IsMechanicalArmBuilt ? 1 : 0);

        // A load must never grant progress from a mission step alone when the
        // saved inventory says the player has not earned it yet. Downgrading to
        // the last proven playable objective keeps old or inconsistent saves
        // recoverable without skipping the step that grants the missing item.
        if (step >= CrashSiteMissionStep.Victory && !saveData.IsBeaconActivated)
            step = CrashSiteMissionStep.ActivateBeacon;

        if (step >= CrashSiteMissionStep.ActivateBeacon && !saveData.HasPowerCore)
            step = CrashSiteMissionStep.RecoverPowerCore;

        if (step >= CrashSiteMissionStep.BreachTowerGate && armLevel < 3)
            step = CrashSiteMissionStep.UpgradeArmMk3;

        if (step >= CrashSiteMissionStep.OpenNetYard && armLevel < 2)
            step = CrashSiteMissionStep.UpgradeArmMk2;

        // Between recovery and the Mk II fitting the component is in the pack.
        if (step == CrashSiteMissionStep.UpgradeArmMk2 && !saveData.IsGalaxabrainComponentCollected)
            step = CrashSiteMissionStep.RecoverGalaxabrainComponent;

        if (step >= CrashSiteMissionStep.RecoverGalaxabrainComponent && !saveData.IsGalaxabrainDefeated)
            step = CrashSiteMissionStep.DefeatGalaxabrain;

        if (step >= CrashSiteMissionStep.DefeatGalaxabrain && armLevel < 1)
            step = CrashSiteMissionStep.BuildMechanicalArm;

        return step;
    }

    /// <summary>The arm level a step implies: the module that step was reached with.</summary>
    public static int RequiredArmLevel(CrashSiteMissionStep step) => step switch
    {
        >= CrashSiteMissionStep.BreachTowerGate => 3,
        >= CrashSiteMissionStep.OpenNetYard => 2,
        >= CrashSiteMissionStep.DefeatGalaxabrain => 1,
        _ => 0,
    };

    public static bool RequiresMechanicalArmBuilt(CrashSiteMissionStep step) =>
        step >= CrashSiteMissionStep.DefeatGalaxabrain;

    public static bool RequiresGalaxabrainDefeated(CrashSiteMissionStep step) =>
        step >= CrashSiteMissionStep.RecoverGalaxabrainComponent;

    /// <summary>The component is in the player's pack only until it is fitted as Mk II.</summary>
    public static bool RequiresComponentCollected(CrashSiteMissionStep step) =>
        step == CrashSiteMissionStep.UpgradeArmMk2;

    public static bool IsComponentAvailable(CrashSiteMissionStep step) =>
        step == CrashSiteMissionStep.RecoverGalaxabrainComponent;

    public static bool RequiresNetYardOpen(CrashSiteMissionStep step) =>
        step >= CrashSiteMissionStep.ClearDroneNest;

    public static bool RequiresDroneNestCleared(CrashSiteMissionStep step) =>
        step >= CrashSiteMissionStep.UpgradeArmMk3;

    public static bool RequiresTowerGateBroken(CrashSiteMissionStep step) =>
        step >= CrashSiteMissionStep.DefeatWarden;

    public static bool RequiresWardenDefeated(CrashSiteMissionStep step) =>
        step >= CrashSiteMissionStep.RecoverPowerCore;

    public static bool IsPowerCoreAvailable(CrashSiteMissionStep step) =>
        step == CrashSiteMissionStep.RecoverPowerCore;

    public static bool RequiresPowerCoreCollected(CrashSiteMissionStep step) =>
        step is CrashSiteMissionStep.ActivateBeacon or CrashSiteMissionStep.DefendBeacon;

    public static bool RequiresBeaconActivated(CrashSiteMissionStep step) =>
        step >= CrashSiteMissionStep.Victory;
}

/// <summary>
/// Brings an older save forward to <see cref="CrashSiteSaveData.CurrentSaveVersion"/>.
/// Version 1 (the MVP) ended at the beacon: its step values 0-3 mean the same
/// thing today; 4 (activate beacon) and 5 (victory) both meant "holding the
/// Scout's component", which is now the start of the Mk II upgrade.
/// </summary>
public static class CrashSiteSaveMigration
{
    public static bool TryMigrate(CrashSiteSaveData saveData)
    {
        ArgumentNullException.ThrowIfNull(saveData);

        if (saveData.SaveVersion == CrashSiteSaveData.CurrentSaveVersion)
            return true;

        if (saveData.SaveVersion != 1)
            return false;

        var oldStep = (int)saveData.MissionStep;
        if (oldStep < 0 || oldStep > 5)
            return false;

        saveData.ArmLevel = saveData.IsMechanicalArmBuilt ? 1 : 0;
        saveData.HasPowerCore = false;
        saveData.IsBeaconActivated = false;
        saveData.CollectedPickups ??= new();
        if (oldStep >= 4)
        {
            saveData.MissionStep = CrashSiteMissionStep.UpgradeArmMk2;
            saveData.IsGalaxabrainComponentCollected = true;
        }

        saveData.SaveVersion = CrashSiteSaveData.CurrentSaveVersion;
        return true;
    }
}
