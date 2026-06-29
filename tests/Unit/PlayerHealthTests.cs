using GdUnit4;
using TitanCraft.Player;
using static GdUnit4.Assertions;

namespace TitanCraft.Tests.Unit;

[TestSuite]
public sealed class PlayerHealthTests
{
    [TestCase]
    public void StartsWithConfiguredMaxHealth()
    {
        var health = new PlayerHealth(75);

        AssertThat(health.MaxHealth).IsEqual(75);
        AssertThat(health.CurrentHealth).IsEqual(75);
        AssertThat(health.IsDead).IsFalse();
    }

    [TestCase]
    public void DamageReducesCurrentHealth()
    {
        var health = new PlayerHealth(PlayerHealth.DefaultMaxHealth);

        health.ApplyDamage(25);

        AssertThat(health.CurrentHealth).IsEqual(75);
        AssertThat(health.IsDead).IsFalse();
    }

    [TestCase]
    public void DamageClampsAtZero()
    {
        var health = new PlayerHealth(40);

        health.ApplyDamage(100);

        AssertThat(health.CurrentHealth).IsEqual(0);
    }

    [TestCase]
    public void ZeroOrNegativeDamageDoesNotHealOrHurt()
    {
        var health = new PlayerHealth(40);

        health.ApplyDamage(10);
        health.ApplyDamage(0);
        health.ApplyDamage(-10);

        AssertThat(health.CurrentHealth).IsEqual(30);
    }

    [TestCase]
    public void DeathStateBeginsWhenHealthReachesZero()
    {
        var health = new PlayerHealth(10);

        health.ApplyDamage(10);

        AssertThat(health.CurrentHealth).IsEqual(0);
        AssertThat(health.IsDead).IsTrue();
    }

    [TestCase]
    public void DamageAfterDeathKeepsHealthAtZero()
    {
        var health = new PlayerHealth(10);

        health.ApplyDamage(10);
        health.ApplyDamage(5);

        AssertThat(health.CurrentHealth).IsEqual(0);
        AssertThat(health.IsDead).IsTrue();
    }

    [TestCase]
    public void PrepareRespawnRestoresMaxHealthAndClearsDeathState()
    {
        var health = new PlayerHealth(35);

        health.ApplyDamage(50);
        health.PrepareRespawn();

        AssertThat(health.CurrentHealth).IsEqual(35);
        AssertThat(health.IsDead).IsFalse();
    }
}
