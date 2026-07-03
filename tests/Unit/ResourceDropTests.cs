using GdUnit4;
using TitanCraft.Resources;
using TitanCraft.World;
using static GdUnit4.Assertions;

namespace TitanCraft.Tests.Unit;

[TestSuite]
public sealed class ResourceDropTests
{
    [TestCase]
    public void AddResourceToInventoryAddsConfiguredMetalQuantity()
    {
        var inventory = new MvpInventory();

        ResourceDrop.AddResourceToInventory(inventory, ResourceType.Metal, 3);

        AssertThat(inventory.Metal).IsEqual(3);
        AssertThat(inventory.Biomass).IsEqual(0);
        AssertThat(inventory.ElectronicComponents).IsEqual(0);
    }

    [TestCase]
    public void AddResourceToInventoryAddsConfiguredBiomassQuantity()
    {
        var inventory = new MvpInventory();

        ResourceDrop.AddResourceToInventory(inventory, ResourceType.Biomass, 2);

        AssertThat(inventory.Biomass).IsEqual(2);
    }

    [TestCase]
    public void AddResourceToInventoryAddsConfiguredElectronicsQuantity()
    {
        var inventory = new MvpInventory();

        ResourceDrop.AddResourceToInventory(inventory, ResourceType.Electronics, 4);

        AssertThat(inventory.ElectronicComponents).IsEqual(4);
    }

    [TestCase]
    public void AddResourceToInventoryRejectsNonPositiveQuantities()
    {
        var inventory = new MvpInventory();

        AssertThrown(() => ResourceDrop.AddResourceToInventory(inventory, ResourceType.Metal, 0))
            .IsInstanceOf<System.ArgumentOutOfRangeException>();
    }
}
