using System;
namespace TitanCraft.Missions;

/// <summary>
/// Manages the state and progression of the crash site mission.
/// The mission is one linear chain of steps (CrashSiteMissionStep); each
/// completion method only succeeds from its own step and advances to the next,
/// so no interaction can skip ahead or complete an objective twice.
/// </summary>
public sealed class CrashSiteMissionState
{
    private CrashSiteMissionStep _currentStep = CrashSiteMissionStep.CollectResources;
    private int _beaconChargePercent;

    public event Action<CrashSiteMissionState>? Changed;

    public CrashSiteMissionPhase CurrentPhase => _currentStep switch
    {
        CrashSiteMissionStep.CollectResources => CrashSiteMissionPhase.Scavenge,
        CrashSiteMissionStep.BuildMechanicalArm => CrashSiteMissionPhase.Craft,
        CrashSiteMissionStep.DefeatGalaxabrain => CrashSiteMissionPhase.ExtractConquer,
        CrashSiteMissionStep.RecoverGalaxabrainComponent => CrashSiteMissionPhase.ExtractConquer,
        CrashSiteMissionStep.UpgradeArmMk2 => CrashSiteMissionPhase.NetYard,
        CrashSiteMissionStep.OpenNetYard => CrashSiteMissionPhase.NetYard,
        CrashSiteMissionStep.ClearDroneNest => CrashSiteMissionPhase.NetYard,
        CrashSiteMissionStep.UpgradeArmMk3 => CrashSiteMissionPhase.Tower,
        CrashSiteMissionStep.BreachTowerGate => CrashSiteMissionPhase.Tower,
        CrashSiteMissionStep.DefeatWarden => CrashSiteMissionPhase.Tower,
        CrashSiteMissionStep.RecoverPowerCore => CrashSiteMissionPhase.Tower,
        CrashSiteMissionStep.ActivateBeacon => CrashSiteMissionPhase.Beacon,
        CrashSiteMissionStep.DefendBeacon => CrashSiteMissionPhase.Beacon,
        CrashSiteMissionStep.Victory => CrashSiteMissionPhase.Complete,
        _ => CrashSiteMissionPhase.SurviveAndOrient
    };

    public string CurrentPhaseTitle => CurrentPhase switch
    {
        CrashSiteMissionPhase.SurviveAndOrient => "Phase 1 — Survive & Orient",
        CrashSiteMissionPhase.Scavenge => "Phase 2 — Scavenge",
        CrashSiteMissionPhase.Craft => "Phase 3 — Craft",
        CrashSiteMissionPhase.ExtractConquer => "Phase 4 — The Scout",
        CrashSiteMissionPhase.NetYard => "Phase 5 — The Yard",
        CrashSiteMissionPhase.Tower => "Phase 6 — The Tower",
        CrashSiteMissionPhase.Beacon => "Phase 7 — The Beacon",
        CrashSiteMissionPhase.Complete => "Mission Complete",
        _ => "Crash Site"
    };

    /// <summary>
    /// Gets the current mission step.
    /// </summary>
    public CrashSiteMissionStep CurrentStep => _currentStep;

    /// <summary>Beacon charge during the holdout, 0-100.</summary>
    public int BeaconChargePercent => _beaconChargePercent;

    /// <summary>
    /// Gets the current objective text for the HUD.
    /// </summary>
    public string CurrentObjectiveText => _currentStep switch
    {
        CrashSiteMissionStep.CollectResources => "Collect Metal, Biomass, and Electronics near the debris trail.",
        CrashSiteMissionStep.BuildMechanicalArm => "Return to the lit Workbench and build Mechanical Arm Mk I.",
        CrashSiteMissionStep.DefeatGalaxabrain => "Defeat the red-core Galaxabrain Scout.",
        CrashSiteMissionStep.RecoverGalaxabrainComponent => "Recover the Galaxabrain component from the crash arena.",
        CrashSiteMissionStep.UpgradeArmMk2 => "Scavenge salvage, then fit Mk II at the Workbench.",
        CrashSiteMissionStep.OpenNetYard => "Pull the wreck off the yard gate (right click).",
        CrashSiteMissionStep.ClearDroneNest => "Bring down the swarm drones in the yard.",
        CrashSiteMissionStep.UpgradeArmMk3 => "Salvage the yard, then fit Mk III at the Workbench.",
        CrashSiteMissionStep.BreachTowerGate => "Break the tower gate: hold left click, release.",
        CrashSiteMissionStep.DefeatWarden => "Defeat the Warden. Charged strikes break its guard.",
        CrashSiteMissionStep.RecoverPowerCore => "Take the beacon's power core from the nest.",
        CrashSiteMissionStep.ActivateBeacon => "Install the power core in the beacon.",
        CrashSiteMissionStep.DefendBeacon => $"Hold the plaza while the beacon charges: {_beaconChargePercent}%.",
        CrashSiteMissionStep.Victory => "Beacon activated — rescue signal online.",
        _ => "Unknown Objective"
    };

    public string HudBreadcrumb => $"{CurrentPhaseTitle}: {CurrentObjectiveText}";

    public bool TryCompleteResourceCollection() => TryAdvanceFrom(CrashSiteMissionStep.CollectResources);

    public bool TryCompleteMechanicalArmConstruction() => TryAdvanceFrom(CrashSiteMissionStep.BuildMechanicalArm);

    public bool TryCompleteGalaxabrainDefeat(bool isGalaxabrainDefeated = true)
        => isGalaxabrainDefeated && TryAdvanceFrom(CrashSiteMissionStep.DefeatGalaxabrain);

    public bool TryCompleteComponentRecovery() => TryAdvanceFrom(CrashSiteMissionStep.RecoverGalaxabrainComponent);

    public bool TryCompleteMk2Upgrade() => TryAdvanceFrom(CrashSiteMissionStep.UpgradeArmMk2);

    public bool TryCompleteNetYardOpening() => TryAdvanceFrom(CrashSiteMissionStep.OpenNetYard);

    public bool TryCompleteDroneNest() => TryAdvanceFrom(CrashSiteMissionStep.ClearDroneNest);

    public bool TryCompleteMk3Upgrade() => TryAdvanceFrom(CrashSiteMissionStep.UpgradeArmMk3);

    public bool TryCompleteTowerBreach() => TryAdvanceFrom(CrashSiteMissionStep.BreachTowerGate);

    public bool TryCompleteWardenDefeat() => TryAdvanceFrom(CrashSiteMissionStep.DefeatWarden);

    public bool TryCompletePowerCoreRecovery() => TryAdvanceFrom(CrashSiteMissionStep.RecoverPowerCore);

    /// <summary>Activating the beacon starts its charge: the holdout, not yet victory.</summary>
    public bool TryCompleteBeaconActivation()
    {
        if (!TryAdvanceFrom(CrashSiteMissionStep.ActivateBeacon, raise: false))
            return false;

        _beaconChargePercent = 0;
        Changed?.Invoke(this);
        return true;
    }

    public bool TryCompleteBeaconDefence()
    {
        if (_currentStep != CrashSiteMissionStep.DefendBeacon)
            return false;

        _beaconChargePercent = 100;
        return TryAdvanceFrom(CrashSiteMissionStep.DefendBeacon);
    }

    /// <summary>
    /// Reports holdout progress; raises Changed only when the whole percent
    /// moves, so the HUD is not rewritten every frame.
    /// </summary>
    public void SetBeaconCharge(float fraction)
    {
        if (_currentStep != CrashSiteMissionStep.DefendBeacon)
            return;

        var percent = Math.Clamp((int)MathF.Floor(fraction * 100f), 0, 99);
        if (percent == _beaconChargePercent)
            return;

        _beaconChargePercent = percent;
        Changed?.Invoke(this);
    }

    /// <summary>
    /// Restores the mission state from saved data. A save taken mid-holdout
    /// resumes at activation: the charge itself is not persisted.
    /// </summary>
    public void Restore(CrashSiteMissionStep savedStep)
    {
        _currentStep = savedStep == CrashSiteMissionStep.DefendBeacon
            ? CrashSiteMissionStep.ActivateBeacon
            : savedStep;
        _beaconChargePercent = 0;

        Changed?.Invoke(this);
    }

    /// <summary>
    /// Checks if the mission is complete (victory state reached).
    /// </summary>
    public bool IsVictory => _currentStep == CrashSiteMissionStep.Victory;

    private bool TryAdvanceFrom(CrashSiteMissionStep step, bool raise = true)
    {
        if (_currentStep != step || step >= CrashSiteMissionStep.Victory)
            return false;

        _currentStep = step + 1;
        if (raise)
            Changed?.Invoke(this);
        return true;
    }
}
