using System;
using GdUnit4;
using TitanCraft.Resources;
using static GdUnit4.Assertions;

namespace TitanCraft.Tests.Unit;

[TestSuite]
public sealed class MvpInventoryTests
{
    [TestCase]
    public void StartsEmptyWithMissionFlagsUnset()
    {
        var inventory = new MvpInventory();

        AssertThat(inventory.Metal).IsEqual(0);
        AssertThat(inventory.Biomass).IsEqual(0);
        AssertThat(inventory.ElectronicComponents).IsEqual(0);
        AssertThat(inventory.IsMechanicalArmBuilt).IsFalse();
        AssertThat(inventory.HasGalaxabrainComponent).IsFalse();
    }

    [TestCase]
    public void AddResourcesIncreasesOnlyTrackedResourceCounts()
    {
        var inventory = new MvpInventory();

        inventory.AddResources(metal: 10, biomass: 3, electronicComponents: 2);
        inventory.AddResources(metal: 4, biomass: 1);

        AssertThat(inventory.Metal).IsEqual(14);
        AssertThat(inventory.Biomass).IsEqual(4);
        AssertThat(inventory.ElectronicComponents).IsEqual(2);
    }

    [TestCase]
    public void TrySpendResourcesRemovesAvailableResources()
    {
        var inventory = new MvpInventory();
        inventory.AddResources(metal: 10, biomass: 3, electronicComponents: 2);

        var didSpend = inventory.TrySpendResources(metal: 7, biomass: 1, electronicComponents: 2);

        AssertThat(didSpend).IsTrue();
        AssertThat(inventory.Metal).IsEqual(3);
        AssertThat(inventory.Biomass).IsEqual(2);
        AssertThat(inventory.ElectronicComponents).IsEqual(0);
    }

    [TestCase]
    public void TrySpendResourcesFailsWithoutChangingInventoryWhenInsufficient()
    {
        var inventory = new MvpInventory();
        inventory.AddResources(metal: 2, biomass: 3, electronicComponents: 1);

        var didSpend = inventory.TrySpendResources(metal: 2, biomass: 4, electronicComponents: 1);

        AssertThat(didSpend).IsFalse();
        AssertThat(inventory.Metal).IsEqual(2);
        AssertThat(inventory.Biomass).IsEqual(3);
        AssertThat(inventory.ElectronicComponents).IsEqual(1);
    }

    [TestCase]
    public void CanSpendResourcesReportsAffordabilityWithoutMutatingCounts()
    {
        var inventory = new MvpInventory();
        inventory.AddResources(metal: 5, biomass: 1, electronicComponents: 1);

        AssertThat(inventory.CanSpendResources(metal: 5, biomass: 1, electronicComponents: 1)).IsTrue();
        AssertThat(inventory.CanSpendResources(metal: 6)).IsFalse();
        AssertThat(inventory.Metal).IsEqual(5);
        AssertThat(inventory.Biomass).IsEqual(1);
        AssertThat(inventory.ElectronicComponents).IsEqual(1);
    }

    [TestCase]
    public void NegativeResourceInputsAreRejected()
    {
        var inventory = new MvpInventory();

        AssertThrown(() => inventory.AddResources(metal: -1)).IsInstanceOf<ArgumentOutOfRangeException>();
        AssertThrown(() => inventory.CanSpendResources(biomass: -1)).IsInstanceOf<ArgumentOutOfRangeException>();
        AssertThrown(() => inventory.TrySpendResources(electronicComponents: -1)).IsInstanceOf<ArgumentOutOfRangeException>();
        AssertThat(inventory.Metal).IsEqual(0);
        AssertThat(inventory.Biomass).IsEqual(0);
        AssertThat(inventory.ElectronicComponents).IsEqual(0);
    }

    [TestCase]
    public void StateFlagsCanBeMarkedForMvpProgression()
    {
        var inventory = new MvpInventory();

        inventory.MarkMechanicalArmBuilt();
        inventory.MarkGalaxabrainComponentCollected();

        AssertThat(inventory.IsMechanicalArmBuilt).IsTrue();
        AssertThat(inventory.HasGalaxabrainComponent).IsTrue();
    }
}
