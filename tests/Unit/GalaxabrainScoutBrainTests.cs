using GdUnit4;
using Godot;
using TitanCraft.Enemies;
using static GdUnit4.Assertions;

namespace TitanCraft.Tests.Unit;

[TestSuite]
public sealed class GalaxabrainScoutBrainTests
{
    [TestCase]
    public void StartsIdleWithConfiguredHealth()
    {
        var brain = CreateBrain();

        AssertThat(brain.State).IsEqual(GalaxabrainScoutState.Idle);
        AssertThat(brain.CurrentHealth).IsEqual(100);
        AssertThat(brain.IsDead).IsFalse();
    }

    [TestCase]
    public void TransitionsFromIdleToChaseAndAttackByDistance()
    {
        var brain = CreateBrain();

        brain.Tick(0.1f, 8f);
        AssertThat(brain.State).IsEqual(GalaxabrainScoutState.Chase);

        brain.Tick(0.1f, 1.5f);
        AssertThat(brain.State).IsEqual(GalaxabrainScoutState.Attack);

        brain.Tick(0.1f, 20f);
        AssertThat(brain.State).IsEqual(GalaxabrainScoutState.Idle);
    }

    [TestCase]
    public void AttackUsesDeterministicCooldown()
    {
        var brain = CreateBrain();

        brain.Tick(0.1f, 1f);

        AssertThat(brain.TryConsumeAttack()).IsTrue();
        AssertThat(brain.TryConsumeAttack()).IsFalse();

        brain.Tick(0.7f, 1f);
        AssertThat(brain.TryConsumeAttack()).IsFalse();

        brain.Tick(0.11f, 1f);
        AssertThat(brain.TryConsumeAttack()).IsTrue();
    }

    [TestCase]
    public void DamageReducesHealthAndDeathLocksState()
    {
        var brain = CreateBrain();

        brain.ApplyDamage(25);
        AssertThat(brain.CurrentHealth).IsEqual(75);
        AssertThat(brain.State).IsEqual(GalaxabrainScoutState.Idle);

        brain.ApplyDamage(100);
        AssertThat(brain.CurrentHealth).IsEqual(0);
        AssertThat(brain.State).IsEqual(GalaxabrainScoutState.Dead);

        brain.Tick(1f, 1f);
        brain.ApplyDamage(10);
        AssertThat(brain.CurrentHealth).IsEqual(0);
        AssertThat(brain.State).IsEqual(GalaxabrainScoutState.Dead);
    }

    [TestCase]
    [RequireGodotRuntime]
    public void ScoutApplyDamageKillsEnemyAndExposesMissionComponent()
    {
        var scout = new GalaxabrainScout
        {
            Health = 100,
            MissionComponentPath = new NodePath("GalaxabrainComponentPickup"),
        };
        var missionComponent = new Area3D
        {
            Name = "GalaxabrainComponentPickup",
            Visible = true,
            Monitoring = true,
        };
        scout.AddChild(missionComponent);

        scout._Ready();
        AssertThat(missionComponent.Visible).IsFalse();
        AssertThat(missionComponent.Monitoring).IsFalse();

        scout.ApplyDamage(100);

        AssertThat(scout.Brain.IsDead).IsTrue();
        AssertThat(scout.Visible).IsFalse();
        AssertThat(missionComponent.Visible).IsTrue();
        AssertThat(missionComponent.Monitoring).IsTrue();
    }

    private static GalaxabrainScoutBrain CreateBrain()
    {
        return new GalaxabrainScoutBrain(maxHealth: 100, detectionRange: 10f, attackRange: 2f, attackCooldownSeconds: 0.8f);
    }
}
