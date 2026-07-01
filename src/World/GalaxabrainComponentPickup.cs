using System;
using Godot;
using TitanCraft.Missions;
using TitanCraft.Resources;

namespace TitanCraft.World;

public partial class GalaxabrainComponentPickup : Area3D, ICrashSiteInteractable
{
    private bool _isCollected;

    public bool Interact(MvpInventory inventory, CrashSiteMissionState mission)
    {
        ArgumentNullException.ThrowIfNull(inventory);
        ArgumentNullException.ThrowIfNull(mission);

        if (_isCollected
            || mission.CurrentStep != CrashSiteMissionStep.RecoverGalaxabrainComponent
            || !inventory.IsMechanicalArmBuilt)
        {
            return false;
        }

        if (!mission.TryCompleteComponentRecovery())
        {
            return false;
        }

        inventory.MarkGalaxabrainComponentCollected();
        _isCollected = true;
        Visible = false;
        Monitoring = false;
        return true;
    }
}
