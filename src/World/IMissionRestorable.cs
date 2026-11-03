using TitanCraft.Missions;

namespace TitanCraft.World;

/// <summary>
/// A world object whose state follows from mission progress alone (a gate that
/// is open once its step is passed, an enemy that is dead once its step is
/// passed). After a load the save coordinator hands every member of
/// <see cref="Group"/> the restored step, so each object rebuilds itself
/// without the coordinator knowing what it is.
/// </summary>
public interface IMissionRestorable
{
    public const string Group = "mission_restorable";

    void RestoreFromMission(CrashSiteMissionStep step);
}
