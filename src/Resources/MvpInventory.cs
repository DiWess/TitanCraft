using System;

namespace TitanCraft.Resources;

public sealed class MvpInventory
{
    public int Metal { get; private set; }

    public int Biomass { get; private set; }

    public int ElectronicComponents { get; private set; }

    public bool IsMechanicalArmBuilt { get; private set; }

    public bool HasGalaxabrainComponent { get; private set; }

    public void AddResources(int metal = 0, int biomass = 0, int electronicComponents = 0)
    {
        ThrowIfNegative(metal, nameof(metal));
        ThrowIfNegative(biomass, nameof(biomass));
        ThrowIfNegative(electronicComponents, nameof(electronicComponents));

        Metal += metal;
        Biomass += biomass;
        ElectronicComponents += electronicComponents;
    }

    public bool CanSpendResources(int metal = 0, int biomass = 0, int electronicComponents = 0)
    {
        ThrowIfNegative(metal, nameof(metal));
        ThrowIfNegative(biomass, nameof(biomass));
        ThrowIfNegative(electronicComponents, nameof(electronicComponents));

        return Metal >= metal
            && Biomass >= biomass
            && ElectronicComponents >= electronicComponents;
    }

    public bool TrySpendResources(int metal = 0, int biomass = 0, int electronicComponents = 0)
    {
        if (!CanSpendResources(metal, biomass, electronicComponents))
        {
            return false;
        }

        Metal -= metal;
        Biomass -= biomass;
        ElectronicComponents -= electronicComponents;
        return true;
    }

    public void MarkMechanicalArmBuilt()
    {
        IsMechanicalArmBuilt = true;
    }

    public void MarkGalaxabrainComponentCollected()
    {
        HasGalaxabrainComponent = true;
    }

    private static void ThrowIfNegative(int quantity, string parameterName)
    {
        if (quantity < 0)
        {
            throw new ArgumentOutOfRangeException(parameterName, "Resource quantities cannot be negative.");
        }
    }
}
