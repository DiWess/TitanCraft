using System;
using Godot;
using TitanCraft.Crafting;
using TitanCraft.Missions;
using TitanCraft.Resources;

namespace TitanCraft.World;

public partial class Workbench : Area3D, ICrashSiteInteractable
{
    private readonly MechanicalArmRecipe _mechanicalArmRecipe = new();

    public bool Interact(MvpInventory inventory, CrashSiteMissionState mission)
    {
        ArgumentNullException.ThrowIfNull(inventory);
        ArgumentNullException.ThrowIfNull(mission);

        if (mission.CurrentStep != CrashSiteMissionStep.BuildMechanicalArm)
        {
            return false;
        }

        if (!_mechanicalArmRecipe.TryCraft(inventory))
        {
            return false;
        }

        return mission.TryCompleteMechanicalArmConstruction();
    }
}
