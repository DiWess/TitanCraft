using System;
using Godot;
using TitanCraft.Missions;
using TitanCraft.Resources;

namespace TitanCraft.World;

public partial class Beacon : Area3D, ICrashSiteInteractable
{
    public bool IsActivated { get; private set; }

    public bool Interact(MvpInventory inventory, CrashSiteMissionState mission)
    {
        ArgumentNullException.ThrowIfNull(inventory);
        ArgumentNullException.ThrowIfNull(mission);

        if (IsActivated
            || mission.CurrentStep != CrashSiteMissionStep.ActivateBeacon
            || !inventory.HasGalaxabrainComponent)
        {
            return false;
        }

        IsActivated = mission.TryCompleteBeaconActivation();
        return IsActivated;
    }
}
