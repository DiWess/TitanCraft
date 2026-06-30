using GdUnit4;
using TitanCraft.Player;
using TitanCraft.Resources;
using static GdUnit4.Assertions;

namespace TitanCraft.Tests.Unit;

[TestSuite]
public sealed class MechanicalArmAttackTests
{
    [TestCase]
    public void BuiltMechanicalArmAllowsConfiguredDamageApplication()
    {
        var inventory = new MvpInventory();
        inventory.MarkMechanicalArmBuilt();
        var attack = new MechanicalArmAttackLogic(damage: 25, cooldownSeconds: 0.8f);

        AssertThat(attack.TryAttack(inventory)).IsTrue();

        AssertThat(attack.Damage).IsEqual(25);
    }

    [TestCase]
    public void MechanicalArmCooldownBlocksRepeatedAttackUntilElapsed()
    {
        var inventory = new MvpInventory();
        inventory.MarkMechanicalArmBuilt();
        var attack = new MechanicalArmAttackLogic(damage: 25, cooldownSeconds: 0.8f);

        AssertThat(attack.TryAttack(inventory)).IsTrue();
        AssertThat(attack.TryAttack(inventory)).IsFalse();

        attack.Tick(0.79f);
        AssertThat(attack.TryAttack(inventory)).IsFalse();

        attack.Tick(0.01f);
        AssertThat(attack.TryAttack(inventory)).IsTrue();
    }

    [TestCase]
    public void UnbuiltMechanicalArmCannotApplyFullDamage()
    {
        var inventory = new MvpInventory();
        var attack = new MechanicalArmAttackLogic(damage: 25, cooldownSeconds: 0.8f);

        AssertThat(attack.TryAttack(inventory)).IsFalse();
    }
}
