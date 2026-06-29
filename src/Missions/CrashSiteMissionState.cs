using System;
using Godot;

namespace TitanCraft.Missions;

/// <summary>
/// Manages the state and progression of the crash site mission.
/// Tracks completed objectives and current mission step.
/// </summary>
public partial class CrashSiteMissionState : Resource
{
    private CrashSiteMissionStep _currentStep = CrashSiteMissionStep.Start;
    private bool _resourceCollectionCompleted;
    private bool _mechanicalArmConstructionCompleted;
    private bool _galaxabrainDefeatCompleted;
    private bool _componentRecoveryCompleted;
    private bool _beaconActivationCompleted;

    public event EventHandler? Changed;

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
                EmitSignal(SignalName.Changed);
            }
        }
    }

    /// <summary>
    /// Gets the current objective text for the HUD.
    /// </summary>
    public string CurrentObjectiveText => _currentStep switch
    {
        CrashSiteMissionStep.Start => "Collect Resources",
        CrashSiteMissionStep.CollectResources => "Collect Resources",
        CrashSiteMissionStep.BuildMechanicalArm => "Build Mechanical Arm Mk I",
        CrashSiteMissionStep.ActivateBeacon => "Activate Beacon",
        CrashSiteMissionStep.DefeatGalaxabrain => "Defeat Galaxabrain",
        CrashSiteMissionStep.ComponentRecovery => "Recover Components",
        CrashSiteMissionStep.Victory => "Victory!",
        _ => "Unknown Objective"
    };

    /// <summary>
    /// Attempts to complete the resource collection objective.
    /// </summary>
    public bool TryCompleteResourceCollection()
    {
        if (_resourceCollectionCompleted)
            return false;

        _resourceCollectionCompleted = true;
        if (_currentStep == CrashSiteMissionStep.Start || _currentStep == CrashSiteMissionStep.CollectResources)
            CurrentStep = CrashSiteMissionStep.BuildMechanicalArm;

        Changed?.Invoke(this, EventArgs.Empty);
        return true;
    }

    /// <summary>
    /// Attempts to complete the mechanical arm construction objective.
    /// </summary>
    public bool TryCompleteMechanicalArmConstruction()
    {
        if (_mechanicalArmConstructionCompleted)
            return false;

        _mechanicalArmConstructionCompleted = true;
        if (_currentStep == CrashSiteMissionStep.BuildMechanicalArm)
            CurrentStep = CrashSiteMissionStep.ActivateBeacon;

        Changed?.Invoke(this, EventArgs.Empty);
        return true;
    }

    /// <summary>
    /// Attempts to complete the Galaxabrain defeat objective.
    /// </summary>
    public bool TryCompleteGalaxabrainDefeat(bool defeated = true)
    {
        if (_galaxabrainDefeatCompleted)
            return false;

        _galaxabrainDefeatCompleted = defeated;
        if (defeated && _currentStep == CrashSiteMissionStep.DefeatGalaxabrain)
            CurrentStep = CrashSiteMissionStep.ComponentRecovery;

        Changed?.Invoke(this, EventArgs.Empty);
        return true;
    }

    /// <summary>
    /// Attempts to complete the component recovery objective.
    /// </summary>
    public bool TryCompleteComponentRecovery()
    {
        if (_componentRecoveryCompleted)
            return false;

        _componentRecoveryCompleted = true;
        if (_currentStep == CrashSiteMissionStep.ComponentRecovery)
            UpdateVictoryStatus();

        Changed?.Invoke(this, EventArgs.Empty);
        return true;
    }

    /// <summary>
    /// Attempts to complete the beacon activation objective.
    /// </summary>
    public bool TryCompleteBeaconActivation()
    {
        if (_beaconActivationCompleted)
            return false;

        _beaconActivationCompleted = true;
        if (_currentStep == CrashSiteMissionStep.ActivateBeacon)
            CurrentStep = CrashSiteMissionStep.DefeatGalaxabrain;

        Changed?.Invoke(this, EventArgs.Empty);
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
        _mechanicalArmConstructionCompleted = savedStep >= CrashSiteMissionStep.ActivateBeacon;
        _beaconActivationCompleted = savedStep >= CrashSiteMissionStep.DefeatGalaxabrain;
        _galaxabrainDefeatCompleted = savedStep >= CrashSiteMissionStep.ComponentRecovery;
        _componentRecoveryCompleted = savedStep >= CrashSiteMissionStep.Victory;

        Changed?.Invoke(this, EventArgs.Empty);
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
