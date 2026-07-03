using System;
namespace TitanCraft.Missions;

/// <summary>
/// Manages the state and progression of the crash site mission.
/// Tracks completed objectives and current mission step.
/// </summary>
public sealed class CrashSiteMissionState
{
    private CrashSiteMissionStep _currentStep = CrashSiteMissionStep.CollectResources;
    private bool _resourceCollectionCompleted;
    private bool _mechanicalArmConstructionCompleted;
    private bool _galaxabrainDefeatCompleted;
    private bool _componentRecoveryCompleted;
    private bool _beaconActivationCompleted;

    public event Action<CrashSiteMissionState>? Changed;

    public CrashSiteMissionPhase CurrentPhase => _currentStep switch
    {
        CrashSiteMissionStep.CollectResources => CrashSiteMissionPhase.Scavenge,
        CrashSiteMissionStep.BuildMechanicalArm => CrashSiteMissionPhase.Craft,
        CrashSiteMissionStep.DefeatGalaxabrain => CrashSiteMissionPhase.ExtractConquer,
        CrashSiteMissionStep.RecoverGalaxabrainComponent => CrashSiteMissionPhase.ExtractConquer,
        CrashSiteMissionStep.ActivateBeacon => CrashSiteMissionPhase.ExtractConquer,
        CrashSiteMissionStep.Victory => CrashSiteMissionPhase.Complete,
        _ => CrashSiteMissionPhase.SurviveAndOrient
    };

    public string CurrentPhaseTitle => CurrentPhase switch
    {
        CrashSiteMissionPhase.SurviveAndOrient => "Phase 1 — Survive & Orient",
        CrashSiteMissionPhase.Scavenge => "Phase 2 — Scavenge",
        CrashSiteMissionPhase.Craft => "Phase 3 — Craft",
        CrashSiteMissionPhase.ExtractConquer => "Phase 4 — Extract/Conquer",
        CrashSiteMissionPhase.Complete => "Mission Complete",
        _ => "Crash Site"
    };

    /// <summary>
    /// Gets the current mission step.
    /// </summary>
    public CrashSiteMissionStep CurrentStep
    {
        get => _currentStep;
        private set
        {
            if (_currentStep != value)
            {
                _currentStep = value;
            }
        }
    }

    /// <summary>
    /// Gets the current objective text for the HUD.
    /// </summary>
    public string CurrentObjectiveText => _currentStep switch
    {
        CrashSiteMissionStep.CollectResources => "Collect Metal, Biomass, and Electronics near the debris trail.",
        CrashSiteMissionStep.BuildMechanicalArm => "Return to the lit Workbench and build Mechanical Arm Mk I.",
        CrashSiteMissionStep.ActivateBeacon => "Activate the beacon beam to call for extraction.",
        CrashSiteMissionStep.DefeatGalaxabrain => "Defeat the red-core Galaxabrain Scout.",
        CrashSiteMissionStep.RecoverGalaxabrainComponent => "Recover the Galaxabrain component from the crash arena.",
        CrashSiteMissionStep.Victory => "Mission complete: beacon activated.",
        _ => "Unknown Objective"
    };

    public string HudBreadcrumb => $"{CurrentPhaseTitle}: {CurrentObjectiveText}";

    /// <summary>
    /// Attempts to complete the resource collection objective.
    /// </summary>
    public bool TryCompleteResourceCollection()
    {
        if (_resourceCollectionCompleted || _currentStep != CrashSiteMissionStep.CollectResources)
            return false;

        _resourceCollectionCompleted = true;
        CurrentStep = CrashSiteMissionStep.BuildMechanicalArm;

        Changed?.Invoke(this);
        return true;
    }

    /// <summary>
    /// Attempts to complete the mechanical arm construction objective.
    /// </summary>
    public bool TryCompleteMechanicalArmConstruction()
    {
        if (_mechanicalArmConstructionCompleted || _currentStep != CrashSiteMissionStep.BuildMechanicalArm)
            return false;

        _mechanicalArmConstructionCompleted = true;
        CurrentStep = CrashSiteMissionStep.DefeatGalaxabrain;

        Changed?.Invoke(this);
        return true;
    }

    /// <summary>
    /// Attempts to complete the Galaxabrain defeat objective.
    /// </summary>
    public bool TryCompleteGalaxabrainDefeat(bool isGalaxabrainDefeated = true)
    {
        if (_galaxabrainDefeatCompleted || !isGalaxabrainDefeated || _currentStep != CrashSiteMissionStep.DefeatGalaxabrain)
            return false;

        _galaxabrainDefeatCompleted = true;
        CurrentStep = CrashSiteMissionStep.RecoverGalaxabrainComponent;

        Changed?.Invoke(this);
        return true;
    }

    /// <summary>
    /// Attempts to complete the component recovery objective.
    /// </summary>
    public bool TryCompleteComponentRecovery()
    {
        if (_componentRecoveryCompleted || _currentStep != CrashSiteMissionStep.RecoverGalaxabrainComponent)
            return false;

        _componentRecoveryCompleted = true;
        CurrentStep = CrashSiteMissionStep.ActivateBeacon;

        Changed?.Invoke(this);
        return true;
    }

    /// <summary>
    /// Attempts to complete the beacon activation objective.
    /// </summary>
    public bool TryCompleteBeaconActivation()
    {
        if (_beaconActivationCompleted || _currentStep != CrashSiteMissionStep.ActivateBeacon)
            return false;

        _beaconActivationCompleted = true;
        UpdateVictoryStatus();

        Changed?.Invoke(this);
        return true;
    }

    /// <summary>
    /// Restores the mission state from saved data.
    /// </summary>
    public void Restore(CrashSiteMissionStep savedStep)
    {
        _currentStep = savedStep;

        // Infer completed objectives based on current step
        _resourceCollectionCompleted = savedStep >= CrashSiteMissionStep.BuildMechanicalArm;
        _mechanicalArmConstructionCompleted = savedStep >= CrashSiteMissionStep.DefeatGalaxabrain;
        _galaxabrainDefeatCompleted = savedStep >= CrashSiteMissionStep.RecoverGalaxabrainComponent;
        _componentRecoveryCompleted = savedStep >= CrashSiteMissionStep.ActivateBeacon;
        _beaconActivationCompleted = savedStep >= CrashSiteMissionStep.Victory;

        Changed?.Invoke(this);
    }

    /// <summary>
    /// Checks if all objectives are completed and updates to victory state.
    /// </summary>
    private void UpdateVictoryStatus()
    {
        if (_resourceCollectionCompleted && _mechanicalArmConstructionCompleted &&
            _beaconActivationCompleted && _galaxabrainDefeatCompleted && _componentRecoveryCompleted)
        {
            CurrentStep = CrashSiteMissionStep.Victory;
        }
    }

    /// <summary>
    /// Checks if the mission is complete (victory state reached).
    /// </summary>
    public bool IsVictory => _currentStep == CrashSiteMissionStep.Victory;
}
