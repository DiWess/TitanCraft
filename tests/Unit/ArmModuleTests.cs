using GdUnit4;
using Godot;
using TitanCraft.Combat;
using TitanCraft.Crafting;
using TitanCraft.Resources;
using static GdUnit4.Assertions;

namespace TitanCraft.Tests.Unit;

[TestSuite]
public sealed class ArmModuleTests
{
    [TestCase]
    public void EachTierLoadsItsOwnDataAndName()
    {
        var mk2 = new MechanicalArmRecipe(2);
        var mk3 = new MechanicalArmRecipe(3);

        AssertThat(mk2.Tier).IsEqual(2);
        AssertThat(mk2.DisplayName).IsEqual("Mk II Magnet Clamp");
        AssertThat(mk2.RequiresGalaxabrainComponent).IsTrue();
        AssertThat(mk3.Tier).IsEqual(3);
        AssertThat(mk3.DisplayName).IsEqual("Mk III Breaker Coil");
        AssertThat(mk3.RequiresGalaxabrainComponent).IsFalse();
        AssertThat(mk3.MetalCost + mk3.BiomassCost + mk3.ElectronicComponentsCost).IsGreater(0);
    }

    [TestCase]
    public void ModulesAreCraftedInOrderAndNeverSkipped()
    {
        var inventory = new MvpInventory();
        inventory.AddResources(metal: 50, biomass: 50, electronicComponents: 50);
        inventory.MarkGalaxabrainComponentCollected();
        var mk2 = new MechanicalArmRecipe(2);
        var mk3 = new MechanicalArmRecipe(3);

        AssertThat(mk2.TryCraft(inventory)).IsFalse();
        AssertThat(mk3.TryCraft(inventory)).IsFalse();
        AssertThat(new MechanicalArmRecipe().TryCraft(inventory)).IsTrue();
        AssertThat(mk3.TryCraft(inventory)).IsFalse();
        AssertThat(mk2.TryCraft(inventory)).IsTrue();
        AssertThat(inventory.ArmLevel).IsEqual(2);
        AssertThat(mk3.TryCraft(inventory)).IsTrue();
        AssertThat(inventory.ArmLevel).IsEqual(3);
        AssertThat(MechanicalArmRecipe.NextFor(inventory)).IsNull();
    }

    [TestCase]
    public void Mk2NeedsTheScoutsComponentAndFitsIt()
    {
        var inventory = new MvpInventory();
        inventory.MarkMechanicalArmBuilt();
        var mk2 = new MechanicalArmRecipe(2);
        inventory.AddResources(metal: mk2.MetalCost, biomass: mk2.BiomassCost, electronicComponents: mk2.ElectronicComponentsCost);

        AssertThat(mk2.CanCraft(inventory)).IsFalse();
        AssertThat(mk2.GetProgressText(inventory)).Contains("Component");

        inventory.MarkGalaxabrainComponentCollected();
        AssertThat(mk2.TryCraft(inventory)).IsTrue();
        AssertThat(inventory.HasMagnetClamp).IsTrue();
        AssertThat(inventory.HasGalaxabrainComponent).IsFalse();
        AssertThat(inventory.Metal + inventory.Biomass + inventory.ElectronicComponents).IsEqual(0);
    }

    [TestCase]
    public void ProgressTextNamesTheModuleTheArmIsWaitingFor()
    {
        var inventory = new MvpInventory();
        inventory.UpgradeArm(2);
        var mk3 = new MechanicalArmRecipe(3);

        AssertThat(mk3.GetProgressText(inventory)).StartsWith("Mk III: Metal 0/");
        inventory.UpgradeArm(3);
        AssertThat(mk3.GetProgressText(inventory)).Contains("Mk III ONLINE");
        AssertThat(MechanicalArmRecipe.OnlineText(2)).Contains("Right click");
    }

    [TestCase]
    public void ArmLevelOnlyRisesAndRestoresExactly()
    {
        var inventory = new MvpInventory();
        inventory.UpgradeArm(3);
        inventory.UpgradeArm(1);
        AssertThat(inventory.ArmLevel).IsEqual(3);

        inventory.Restore(1, 2, 3, armLevel: 2, hasGalaxabrainComponent: false, hasPowerCore: true);
        AssertThat(inventory.ArmLevel).IsEqual(2);
        AssertThat(inventory.HasPowerCore).IsTrue();
        AssertThat(inventory.HasBreakerCoil).IsFalse();
    }

    [TestCase]
    public void AChargeIsOnlyChargedOnceHeldLongEnough()
    {
        var charge = new ChargedStrikeLogic(chargeSeconds: 0.75f);

        AssertThat(charge.Release()).IsFalse();
        charge.Begin();
        charge.Tick(0.4f);
        AssertThat(charge.ChargeFraction).IsBetween(0.5f, 0.55f);
        AssertThat(charge.IsFullyCharged).IsFalse();
        AssertThat(charge.Release()).IsFalse();
        AssertThat(charge.IsCharging).IsFalse();

        charge.Begin();
        charge.Tick(0.5f);
        charge.Tick(0.3f);
        AssertThat(charge.IsFullyCharged).IsTrue();
        AssertThat(charge.ChargeFraction).IsEqual(1f);
        AssertThat(charge.Release()).IsTrue();
        AssertThat(charge.ChargeFraction).IsEqual(0f);
    }

    [TestCase]
    public void TheClampNeedsMk2AndRespectsItsCooldown()
    {
        var pull = new MagnetPullLogic(cooldownSeconds: 0.9f);
        var inventory = new MvpInventory();
        inventory.UpgradeArm(1);

        AssertThat(pull.TryFire(inventory)).IsFalse();
        inventory.UpgradeArm(2);
        AssertThat(pull.TryFire(inventory)).IsTrue();
        AssertThat(pull.TryFire(inventory)).IsFalse();
        pull.Tick(0.95f);
        AssertThat(pull.TryFire(inventory)).IsTrue();
    }

    [TestCase]
    public void AimAssistAcceptsANarrowConeWithinReachOnly()
    {
        var forward = new Vector3(0, 0, -1);

        AssertThat(MagnetPullLogic.IsWithinAssist(forward, new Vector3(0.5f, 0, -10f))).IsTrue();
        AssertThat(MagnetPullLogic.IsWithinAssist(forward, new Vector3(3f, 0, -10f))).IsFalse();
        AssertThat(MagnetPullLogic.IsWithinAssist(forward, new Vector3(0, 0, -20f))).IsFalse();
        AssertThat(MagnetPullLogic.IsWithinAssist(forward, new Vector3(0, 0, 5f))).IsFalse();
    }
}
