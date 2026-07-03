using System;
using Godot;
using TitanCraft.Core;
using TitanCraft.Crafting;
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

    [Export] public NodePath CollectionAudioPath { get; set; } = "CollectionAudio";

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
        AudioCue.Play(this, CollectionAudioPath);
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

    public static bool TryAdvanceResourceObjective(MvpInventory inventory, CrashSiteMissionState mission)
    {
        return TryAdvanceResourceObjective(inventory, mission, new MechanicalArmRecipe());
    }

    public static bool TryAdvanceResourceObjective(
        MvpInventory inventory,
        CrashSiteMissionState mission,
        MechanicalArmRecipe mechanicalArmRecipe)
    {
        ArgumentNullException.ThrowIfNull(inventory);
        ArgumentNullException.ThrowIfNull(mission);
        ArgumentNullException.ThrowIfNull(mechanicalArmRecipe);

        if (mission.CurrentStep != CrashSiteMissionStep.CollectResources
            || !mechanicalArmRecipe.CanCraft(inventory))
        {
            return false;
        }

        return mission.TryCompleteResourceCollection();
    }
}
