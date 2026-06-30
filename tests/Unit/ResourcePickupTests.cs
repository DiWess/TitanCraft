using GdUnit4;
using TitanCraft.Crafting;
using TitanCraft.Missions;
using TitanCraft.Resources;
using TitanCraft.World;
using static GdUnit4.Assertions;

namespace TitanCraft.Tests.Unit;

[TestSuite]
public sealed class ResourcePickupTests
{
    [TestCase]
    public void CollectingPartialRecipeResourcesKeepsMissionOnCollectResources()
    {
        var inventory = new MvpInventory();
        var mission = new CrashSiteMissionState();
        var recipe = new MechanicalArmRecipe();

        inventory.AddResources(electronicComponents: recipe.ElectronicComponentsCost);
        AssertThat(ResourcePickup.TryAdvanceResourceObjective(inventory, mission, recipe)).IsFalse();

        inventory.AddResources(biomass: recipe.BiomassCost);
        AssertThat(ResourcePickup.TryAdvanceResourceObjective(inventory, mission, recipe)).IsFalse();

        inventory.AddResources(metal: recipe.MetalCost - 1);
        AssertThat(ResourcePickup.TryAdvanceResourceObjective(inventory, mission, recipe)).IsFalse();

        AssertThat(recipe.CanCraft(inventory)).IsFalse();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.CollectResources);
    }

    [TestCase]
    public void CollectingFinalRecipeResourceAdvancesMissionToBuildMechanicalArm()
    {
        var inventory = new MvpInventory();
        var mission = new CrashSiteMissionState();
        var recipe = new MechanicalArmRecipe();

        inventory.AddResources(biomass: recipe.BiomassCost);
        inventory.AddResources(metal: recipe.MetalCost);
        AssertThat(ResourcePickup.TryAdvanceResourceObjective(inventory, mission, recipe)).IsFalse();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.CollectResources);

        inventory.AddResources(electronicComponents: recipe.ElectronicComponentsCost);

        AssertThat(recipe.CanCraft(inventory)).IsTrue();
        AssertThat(ResourcePickup.TryAdvanceResourceObjective(inventory, mission, recipe)).IsTrue();
        AssertThat(mission.CurrentStep).IsEqual(CrashSiteMissionStep.BuildMechanicalArm);
    }
}
