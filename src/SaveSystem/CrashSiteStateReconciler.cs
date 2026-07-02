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
