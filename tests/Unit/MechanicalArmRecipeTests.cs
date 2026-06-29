using GdUnit4;
using TitanCraft.Crafting;
using TitanCraft.Resources;
using static GdUnit4.Assertions;

namespace TitanCraft.Tests.Unit;

[TestSuite]
public sealed class MechanicalArmRecipeTests
{
    [TestCase]
    public void TryCraftBuildsArmWhenInventoryHasMvpCost()
    {
        var inventory = new MvpInventory();
        inventory.AddResources(
            metal: MechanicalArmRecipe.MetalCost,
            biomass: MechanicalArmRecipe.BiomassCost,
            electronicComponents: MechanicalArmRecipe.ElectronicComponentsCost);
        var recipe = new MechanicalArmRecipe();

        var didCraft = recipe.TryCraft(inventory);

        AssertThat(didCraft).IsTrue();
        AssertThat(inventory.IsMechanicalArmBuilt).IsTrue();
    }

    [TestCase]
    public void TryCraftFailsWithoutChangingInventoryWhenResourcesAreInsufficient()
    {
        var inventory = new MvpInventory();
        inventory.AddResources(
            metal: MechanicalArmRecipe.MetalCost,
            biomass: MechanicalArmRecipe.BiomassCost,
            electronicComponents: MechanicalArmRecipe.ElectronicComponentsCost - 1);
        var recipe = new MechanicalArmRecipe();

        var didCraft = recipe.TryCraft(inventory);

        AssertThat(didCraft).IsFalse();
        AssertThat(inventory.IsMechanicalArmBuilt).IsFalse();
        AssertThat(inventory.Metal).IsEqual(MechanicalArmRecipe.MetalCost);
        AssertThat(inventory.Biomass).IsEqual(MechanicalArmRecipe.BiomassCost);
        AssertThat(inventory.ElectronicComponents).IsEqual(MechanicalArmRecipe.ElectronicComponentsCost - 1);
    }

    [TestCase]
    public void TryCraftPreventsDuplicateMechanicalArmCrafting()
    {
        var inventory = new MvpInventory();
        inventory.AddResources(
            metal: MechanicalArmRecipe.MetalCost * 2,
            biomass: MechanicalArmRecipe.BiomassCost * 2,
            electronicComponents: MechanicalArmRecipe.ElectronicComponentsCost * 2);
        var recipe = new MechanicalArmRecipe();

        var firstCraft = recipe.TryCraft(inventory);
        var secondCraft = recipe.TryCraft(inventory);

        AssertThat(firstCraft).IsTrue();
        AssertThat(secondCraft).IsFalse();
        AssertThat(inventory.IsMechanicalArmBuilt).IsTrue();
        AssertThat(inventory.Metal).IsEqual(MechanicalArmRecipe.MetalCost);
        AssertThat(inventory.Biomass).IsEqual(MechanicalArmRecipe.BiomassCost);
        AssertThat(inventory.ElectronicComponents).IsEqual(MechanicalArmRecipe.ElectronicComponentsCost);
    }

    [TestCase]
    public void TryCraftSpendsExactlyTheReadmeDefinedCost()
    {
        var inventory = new MvpInventory();
        inventory.AddResources(
            metal: MechanicalArmRecipe.MetalCost + 4,
            biomass: MechanicalArmRecipe.BiomassCost + 2,
            electronicComponents: MechanicalArmRecipe.ElectronicComponentsCost + 1);
        var recipe = new MechanicalArmRecipe();

        var didCraft = recipe.TryCraft(inventory);

        AssertThat(didCraft).IsTrue();
        AssertThat(inventory.Metal).IsEqual(4);
        AssertThat(inventory.Biomass).IsEqual(2);
        AssertThat(inventory.ElectronicComponents).IsEqual(1);
    }

    [TestCase]
    public void CanCraftReportsCraftabilityWithoutSpendingResources()
    {
        var inventory = new MvpInventory();
        inventory.AddResources(
            metal: MechanicalArmRecipe.MetalCost,
            biomass: MechanicalArmRecipe.BiomassCost,
            electronicComponents: MechanicalArmRecipe.ElectronicComponentsCost);
        var recipe = new MechanicalArmRecipe();

        var canCraft = recipe.CanCraft(inventory);

        AssertThat(canCraft).IsTrue();
        AssertThat(inventory.IsMechanicalArmBuilt).IsFalse();
        AssertThat(inventory.Metal).IsEqual(MechanicalArmRecipe.MetalCost);
        AssertThat(inventory.Biomass).IsEqual(MechanicalArmRecipe.BiomassCost);
        AssertThat(inventory.ElectronicComponents).IsEqual(MechanicalArmRecipe.ElectronicComponentsCost);
    }
}
