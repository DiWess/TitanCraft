using System;
using Godot;
using TitanCraft.Missions;
using TitanCraft.Resources;

namespace TitanCraft.World;

public enum MvpResourceKind
{
    Metal,
    Biomass,
    ElectronicComponents,
}

public interface ICrashSiteInteractable
{
    bool Interact(MvpInventory inventory, CrashSiteMissionState mission);
}

public partial class ResourcePickup : Area3D, ICrashSiteInteractable
{
    [Export] public MvpResourceKind ResourceKind { get; set; }

    [Export] public int Quantity { get; set; } = 1;

    private bool _isCollected;

    public bool Interact(MvpInventory inventory, CrashSiteMissionState mission)
    {
        ArgumentNullException.ThrowIfNull(inventory);
        ArgumentNullException.ThrowIfNull(mission);

        if (_isCollected || Quantity <= 0)
        {
            return false;
        }

        AddResource(inventory);
        _isCollected = true;
        Visible = false;
        Monitoring = false;
        TryAdvanceResourceObjective(inventory, mission);
        return true;
    }

    private void AddResource(MvpInventory inventory)
    {
        switch (ResourceKind)
        {
            case MvpResourceKind.Metal:
                inventory.AddResources(metal: Quantity);
                break;
            case MvpResourceKind.Biomass:
                inventory.AddResources(biomass: Quantity);
                break;
            case MvpResourceKind.ElectronicComponents:
                inventory.AddResources(electronicComponents: Quantity);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(ResourceKind), ResourceKind, "Unknown MVP resource kind.");
        }
    }

    private static void TryAdvanceResourceObjective(MvpInventory inventory, CrashSiteMissionState mission)
    {
        if (mission.CurrentStep == CrashSiteMissionStep.CollectResources
            && inventory.Metal > 0
            && inventory.Biomass > 0
            && inventory.ElectronicComponents > 0)
        {
            mission.TryCompleteResourceCollection();
        }
    }
}
