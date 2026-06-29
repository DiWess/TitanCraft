namespace TitanCraft.Missions;

/// <summary>
/// Represents the current step/objective in the crash site mission.
/// </summary>
public enum CrashSiteMissionStep
{
    CollectResources,
    Start = CollectResources,
    BuildMechanicalArm,
    DefeatGalaxabrain,
    RecoverGalaxabrainComponent,
    ComponentRecovery = RecoverGalaxabrainComponent,
    ActivateBeacon,
    Victory
}
