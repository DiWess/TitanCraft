namespace TitanCraft.Missions;

/// <summary>
/// Represents the current mission state for the crash site scenario.
/// Tracks which mission step the player is on.
/// </summary>
public sealed class CrashSiteMissionState
{
    // Default mission step when game starts
    public CrashSiteMissionStep CurrentStep { get; set; } = CrashSiteMissionStep.CollectResources;

    /// <summary>
    /// Advances the mission to the next logical step.
    /// </summary>
    public void AdvanceStep()
    {
        CurrentStep = CurrentStep switch
        {
            CrashSiteMissionStep.CollectResources => CrashSiteMissionStep.BuildMechanicalArm,
            CrashSiteMissionStep.BuildMechanicalArm => CrashSiteMissionStep.CollectGalaxabrainComponent,
            CrashSiteMissionStep.CollectGalaxabrainComponent => CrashSiteMissionStep.Escape,
            _ => CrashSiteMissionStep.Escape
        };
    }
}

/// <summary>
/// Enum defining the sequential steps of the crash site mission.
/// </summary>
public enum CrashSiteMissionStep
{
    CollectResources = 0,
    BuildMechanicalArm = 1,
    CollectGalaxabrainComponent = 2,
    Escape = 3
}
