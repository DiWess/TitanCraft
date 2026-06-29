using System;
using TitanCraft.Resources;

namespace TitanCraft.Crafting;

public sealed class MechanicalArmRecipe
{
    public const int MetalCost = 10;
    public const int BiomassCost = 3;
    public const int ElectronicComponentsCost = 2;

    public bool CanCraft(MvpInventory inventory)
    {
        ArgumentNullException.ThrowIfNull(inventory);

        return !inventory.IsMechanicalArmBuilt
            && inventory.CanSpendResources(
                metal: MetalCost,
                biomass: BiomassCost,
                electronicComponents: ElectronicComponentsCost);
    }

    public bool TryCraft(MvpInventory inventory)
    {
        ArgumentNullException.ThrowIfNull(inventory);

        if (!CanCraft(inventory))
        {
            return false;
        }

        var didSpend = inventory.TrySpendResources(
            metal: MetalCost,
            biomass: BiomassCost,
            electronicComponents: ElectronicComponentsCost);

        if (!didSpend)
        {
            return false;
        }

        inventory.MarkMechanicalArmBuilt();
        return true;
    }
}
