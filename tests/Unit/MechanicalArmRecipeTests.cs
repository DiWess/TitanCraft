using GdUnit4;
using TitanCraft.Crafting;
using TitanCraft.Resources;
using static GdUnit4.Assertions;

namespace TitanCraft.Tests.Unit;

[TestSuite]
public sealed class MechanicalArmRecipeTests
{
    [TestCase]
    public void DefaultRecipeLoadsReadmeDefinedDataCosts()
    {
        var recipe = new MechanicalArmRecipe();

        AssertThat(recipe.MetalCost).IsEqual(10);
        AssertThat(recipe.BiomassCost).IsEqual(3);
        AssertThat(recipe.ElectronicComponentsCost).IsEqual(2);
    }

    [TestCase]
    public void TryCraftBuildsArmWhenInventoryHasMvpCost()
    {
        var inventory = new MvpInventory();
        var recipe = new MechanicalArmRecipe();
        inventory.AddResources(
            metal: recipe.MetalCost,
            biomass: recipe.BiomassCost,
            electronicComponents: recipe.ElectronicComponentsCost);

        var didCraft = recipe.TryCraft(inventory);

        AssertThat(didCraft).IsTrue();
        AssertThat(inventory.IsMechanicalArmBuilt).IsTrue();
    }

    [TestCase]
    public void TryCraftFailsWithoutChangingInventoryWhenResourcesAreInsufficient()
    {
        var inventory = new MvpInventory();
        var recipe = new MechanicalArmRecipe();
        inventory.AddResources(
            metal: recipe.MetalCost,
            biomass: recipe.BiomassCost,
            electronicComponents: recipe.ElectronicComponentsCost - 1);

        var didCraft = recipe.TryCraft(inventory);

        AssertThat(didCraft).IsFalse();
        AssertThat(inventory.IsMechanicalArmBuilt).IsFalse();
        AssertThat(inventory.Metal).IsEqual(recipe.MetalCost);
        AssertThat(inventory.Biomass).IsEqual(recipe.BiomassCost);
        AssertThat(inventory.ElectronicComponents).IsEqual(recipe.ElectronicComponentsCost - 1);
    }

    [TestCase]
    public void TryCraftPreventsDuplicateMechanicalArmCrafting()
    {
        var inventory = new MvpInventory();
        var recipe = new MechanicalArmRecipe();
        inventory.AddResources(
            metal: recipe.MetalCost * 2,
            biomass: recipe.BiomassCost * 2,
            electronicComponents: recipe.ElectronicComponentsCost * 2);

        var firstCraft = recipe.TryCraft(inventory);
        var secondCraft = recipe.TryCraft(inventory);

        AssertThat(firstCraft).IsTrue();
        AssertThat(secondCraft).IsFalse();
        AssertThat(inventory.IsMechanicalArmBuilt).IsTrue();
        AssertThat(inventory.Metal).IsEqual(recipe.MetalCost);
        AssertThat(inventory.Biomass).IsEqual(recipe.BiomassCost);
        AssertThat(inventory.ElectronicComponents).IsEqual(recipe.ElectronicComponentsCost);
    }

    [TestCase]
    public void TryCraftSpendsExactlyTheReadmeDefinedCost()
    {
        var inventory = new MvpInventory();
        var recipe = new MechanicalArmRecipe();
        inventory.AddResources(
            metal: recipe.MetalCost + 4,
            biomass: recipe.BiomassCost + 2,
            electronicComponents: recipe.ElectronicComponentsCost + 1);

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
        var recipe = new MechanicalArmRecipe();
        inventory.AddResources(
            metal: recipe.MetalCost,
            biomass: recipe.BiomassCost,
            electronicComponents: recipe.ElectronicComponentsCost);

        var canCraft = recipe.CanCraft(inventory);

        AssertThat(canCraft).IsTrue();
        AssertThat(inventory.IsMechanicalArmBuilt).IsFalse();
        AssertThat(inventory.Metal).IsEqual(recipe.MetalCost);
        AssertThat(inventory.Biomass).IsEqual(recipe.BiomassCost);
        AssertThat(inventory.ElectronicComponents).IsEqual(recipe.ElectronicComponentsCost);
    }

    [TestCase]
    public void ProgressTextShowsTrackableMechanicalArmSubObjectives()
    {
        var inventory = new MvpInventory();
        var recipe = new MechanicalArmRecipe();
        inventory.AddResources(metal: 4, biomass: 1, electronicComponents: 0);

        var progressText = recipe.GetProgressText(inventory);

        AssertThat(progressText).Contains("Metal 4/10");
        AssertThat(progressText).Contains("Biomass 1/3");
        AssertThat(progressText).Contains("Electronics 0/2");
    }

    [TestCase]
    public void ProgressTextReportsOnlineWhenMechanicalArmIsBuilt()
    {
        var inventory = new MvpInventory();
        var recipe = new MechanicalArmRecipe();
        inventory.AddResources(metal: 10, biomass: 3, electronicComponents: 2);
        recipe.TryCraft(inventory);

        AssertThat(recipe.GetProgressText(inventory)).Contains("ONLINE");
    }
}
