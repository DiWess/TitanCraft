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

        // A load must never grant the Galaxabrain component, beacon, or arm from
        // a mission step alone when the saved progression flags say the player
        // has not completed the required trigger yet. Downgrading to the last
        // proven playable objective keeps old or inconsistent saves recoverable
        // without skipping the Scout/component pickup loop.
        if (step >= CrashSiteMissionStep.Victory && !saveData.IsBeaconActivated)
        {
            step = CrashSiteMissionStep.ActivateBeacon;
        }

        if (step >= CrashSiteMissionStep.ActivateBeacon && !saveData.IsGalaxabrainComponentCollected)
        {
            step = CrashSiteMissionStep.RecoverGalaxabrainComponent;
        }

        if (step >= CrashSiteMissionStep.RecoverGalaxabrainComponent && !saveData.IsGalaxabrainDefeated)
        {
            step = CrashSiteMissionStep.DefeatGalaxabrain;
        }

        if (step >= CrashSiteMissionStep.DefeatGalaxabrain && !saveData.IsMechanicalArmBuilt)
        {
            step = CrashSiteMissionStep.BuildMechanicalArm;
        }

        return step;
    }

    public static bool RequiresMechanicalArmBuilt(CrashSiteMissionStep step) =>
        step >= CrashSiteMissionStep.DefeatGalaxabrain;

    public static bool RequiresGalaxabrainDefeated(CrashSiteMissionStep step) =>
        step >= CrashSiteMissionStep.RecoverGalaxabrainComponent;

    public static bool RequiresComponentCollected(CrashSiteMissionStep step) =>
        step >= CrashSiteMissionStep.ActivateBeacon;

    public static bool IsComponentAvailable(CrashSiteMissionStep step) =>
        step == CrashSiteMissionStep.RecoverGalaxabrainComponent;

    public static bool RequiresBeaconActivated(CrashSiteMissionStep step) =>
        step >= CrashSiteMissionStep.Victory;
}
