using GdUnit4;
using TitanCraft.Enemies;
using TitanCraft.Player;
using TitanCraft.Resources;
using static GdUnit4.Assertions;

namespace TitanCraft.Tests.Unit;

[TestSuite]
public sealed class MechanicalArmAttackTests
{
    [TestCase]
    public void BuiltMechanicalArmDealsConfiguredDamage()
    {
        var inventory = new MvpInventory();
        inventory.MarkMechanicalArmBuilt();
        var target = CreateTarget();
        var attack = new MechanicalArmAttackLogic(damage: 25, cooldownSeconds: 0.8f);

        AssertThat(attack.TryAttack(inventory, target)).IsTrue();

        AssertThat(target.CurrentHealth).IsEqual(75);
    }

    [TestCase]
    public void MechanicalArmCooldownBlocksRepeatedDamageUntilElapsed()
    {
        var inventory = new MvpInventory();
        inventory.MarkMechanicalArmBuilt();
        var target = CreateTarget();
        var attack = new MechanicalArmAttackLogic(damage: 25, cooldownSeconds: 0.8f);

        AssertThat(attack.TryAttack(inventory, target)).IsTrue();
        AssertThat(attack.TryAttack(inventory, target)).IsFalse();
        AssertThat(target.CurrentHealth).IsEqual(75);

        attack.Tick(0.79f);
        AssertThat(attack.TryAttack(inventory, target)).IsFalse();

        attack.Tick(0.01f);
        AssertThat(attack.TryAttack(inventory, target)).IsTrue();
        AssertThat(target.CurrentHealth).IsEqual(50);
    }

    [TestCase]
    public void UnbuiltMechanicalArmCannotApplyFullDamage()
    {
        var inventory = new MvpInventory();
        var target = CreateTarget();
        var attack = new MechanicalArmAttackLogic(damage: 25, cooldownSeconds: 0.8f);

        AssertThat(attack.TryAttack(inventory, target)).IsFalse();

        AssertThat(target.CurrentHealth).IsEqual(100);
    }

    private static GalaxabrainScoutBrain CreateTarget()
    {
        return new GalaxabrainScoutBrain(maxHealth: 100, detectionRange: 10f, attackRange: 2f, attackCooldownSeconds: 0.8f);
    }
}
