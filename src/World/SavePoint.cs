using System;
using Godot;
using TitanCraft.Missions;
using TitanCraft.Resources;

namespace TitanCraft.World;

public partial class SavePoint : Area3D, ICrashSiteInteractable
{
    public event Action? SaveRequested;

    [Export] public string CheckpointId { get; set; } = "crash_site_save_point";

    public bool HasSavedCheckpoint { get; private set; }

    public bool Interact(MvpInventory inventory, CrashSiteMissionState mission)
    {
        ArgumentNullException.ThrowIfNull(inventory);
        ArgumentNullException.ThrowIfNull(mission);

        HasSavedCheckpoint = true;
        SaveRequested?.Invoke();
        return true;
    }
}
