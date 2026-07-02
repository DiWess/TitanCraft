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

        // Defeat is completed by the Galaxabrain's death; this pickup only completes recovery.
        if (_isCollected || mission.CurrentStep != CrashSiteMissionStep.RecoverGalaxabrainComponent)
        {
            return false;
        }

        if (!mission.TryCompleteComponentRecovery())
        {
            return false;
        }

        inventory.MarkGalaxabrainComponentCollected();
        Collect();
        return true;
    }

    /// <summary>
    /// Reconstructs the already-collected state from a save without mutating mission progression.
    /// </summary>
    public void RestoreCollected() => Collect();

    private void Collect()
    {
        _isCollected = true;
        Visible = false;
        Monitoring = false;
    }
}
