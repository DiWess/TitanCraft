namespace TitanCraft.Missions;

public enum CrashSiteMissionStep
{
    CollectResources,
    BuildMechanicalArm,
    DefeatGalaxabrain,
    RecoverGalaxabrainComponent,
    ActivateBeacon,
    Victory,
}

public sealed class CrashSiteMissionState
{
    public CrashSiteMissionStep CurrentStep { get; private set; } = CrashSiteMissionStep.CollectResources;

    public string CurrentObjectiveText => CurrentStep switch
    {
        CrashSiteMissionStep.CollectResources => "Collect metal, biomass, and electronic components near the crash site.",
        CrashSiteMissionStep.BuildMechanicalArm => "Return to the workbench and build the Mechanical Arm Mk I.",
        CrashSiteMissionStep.DefeatGalaxabrain => "Use the mechanical arm to defeat the Galaxabrain Scout.",
        CrashSiteMissionStep.RecoverGalaxabrainComponent => "Recover the component dropped by the Galaxabrain Scout.",
        CrashSiteMissionStep.ActivateBeacon => "Activate the rescue beacon.",
        CrashSiteMissionStep.Victory => "Mission complete. The rescue beacon is active.",
        _ => "Continue the Crash Site mission.",
    };

    public bool TryCompleteResourceCollection()
    {
        return TryAdvanceFrom(CrashSiteMissionStep.CollectResources);
    }

    public bool TryCompleteMechanicalArmConstruction()
    {
        return TryAdvanceFrom(CrashSiteMissionStep.BuildMechanicalArm);
    }

    public bool TryCompleteGalaxabrainDefeat(bool isGalaxabrainDefeated)
    {
        return isGalaxabrainDefeated && TryAdvanceFrom(CrashSiteMissionStep.DefeatGalaxabrain);
    }

    public bool TryCompleteComponentRecovery()
    {
        return TryAdvanceFrom(CrashSiteMissionStep.RecoverGalaxabrainComponent);
    }

    public bool TryCompleteBeaconActivation()
    {
        return TryAdvanceFrom(CrashSiteMissionStep.ActivateBeacon);
    }

    private bool TryAdvanceFrom(CrashSiteMissionStep expectedStep)
    {
        if (CurrentStep != expectedStep)
        {
            return false;
        }

        CurrentStep++;
        return true;
    }
}
