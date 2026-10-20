using GdUnit4;
using TitanCraft.Combat;
using TitanCraft.Enemies;
using TitanCraft.World;
using static GdUnit4.Assertions;

namespace TitanCraft.Tests.Unit;

[TestSuite]
public sealed class GalaxabrainVariantTests
{
    [TestCase]
    public void ADroneSleepsUntilWokenThenTelegraphsEveryDive()
    {
        var drone = new SwarmDroneBrain(firstDiveDelaySeconds: 1f);

        drone.Tick(5f, 3f);
        AssertThat(drone.State).IsEqual(SwarmDroneState.Dormant);
        AssertThat(drone.ApplyDamage(25)).IsEqual(StrikeOutcome.Ignored);

        drone.Activate();
        drone.Tick(0.5f, 3f);
        AssertThat(drone.State).IsEqual(SwarmDroneState.Orbit);
        drone.Tick(0.6f, 3f);
        AssertThat(drone.State).IsEqual(SwarmDroneState.Telegraph);
        drone.Tick(SwarmDroneBrain.DefaultTelegraphSeconds + 0.01f, 3f);
        AssertThat(drone.State).IsEqual(SwarmDroneState.Dive);
        AssertThat(drone.ReportDiveContact()).IsTrue();
        AssertThat(drone.State).IsEqual(SwarmDroneState.Recover);
        AssertThat(drone.ReportDiveContact()).IsFalse();
        drone.Tick(SwarmDroneBrain.DefaultRecoverSeconds + 0.01f, 3f);
        AssertThat(drone.State).IsEqual(SwarmDroneState.Orbit);
    }

    [TestCase]
    public void ADroneOutOfRangeHoldsItsDive()
    {
        var drone = new SwarmDroneBrain(firstDiveDelaySeconds: 0.1f, engageRange: 10f);
        drone.Activate();
        drone.Tick(1f, 30f);
        AssertThat(drone.State).IsEqual(SwarmDroneState.Orbit);
        drone.Tick(0.01f, 5f);
        AssertThat(drone.State).IsEqual(SwarmDroneState.Telegraph);
    }

    [TestCase]
    public void TheClampGroundsADroneForAWindowOfStrikes()
    {
        var drone = new SwarmDroneBrain();
        AssertThat(drone.Pull()).IsFalse();
        drone.Activate();

        AssertThat(drone.Pull()).IsTrue();
        AssertThat(drone.State).IsEqual(SwarmDroneState.Stunned);
        AssertThat(drone.Pull()).IsFalse();
        AssertThat(drone.ApplyDamage(25)).IsEqual(StrikeOutcome.Hit);
        AssertThat(drone.ApplyDamage(25)).IsEqual(StrikeOutcome.Killed);
        AssertThat(drone.IsDead).IsTrue();
        AssertThat(drone.Pull()).IsFalse();
    }

    [TestCase]
    public void AStunEndsBackInTheAir()
    {
        var drone = new SwarmDroneBrain();
        drone.Activate();
        drone.Pull();
        drone.Tick(SwarmDroneBrain.DefaultStunSeconds + 0.01f, 3f);
        AssertThat(drone.State).IsEqual(SwarmDroneState.Orbit);
    }

    [TestCase]
    public void TheWardensGuardTurnsAsideEveryOrdinaryStrike()
    {
        var warden = new WardenBrain();
        AssertThat(warden.ReceiveStrike(70, isCharged: true)).IsEqual(StrikeOutcome.Ignored);
        warden.Awaken();

        for (var i = 0; i < 20; i++)
            AssertThat(warden.ReceiveStrike(25, isCharged: false)).IsEqual(StrikeOutcome.Deflected);
        AssertThat(warden.CurrentHealth).IsEqual(warden.MaxHealth);
    }

    [TestCase]
    public void AChargedStrikeBreaksTheGuardAndOpensAWindow()
    {
        var warden = new WardenBrain();
        warden.Awaken();

        AssertThat(warden.ReceiveStrike(70, isCharged: true)).IsEqual(StrikeOutcome.Hit);
        AssertThat(warden.CurrentHealth).IsEqual(warden.MaxHealth - WardenBrain.GuardBreakDamage);
        AssertThat(warden.State).IsEqual(WardenState.Staggered);
        AssertThat(warden.ReceiveStrike(25, isCharged: false)).IsEqual(StrikeOutcome.Hit);

        warden.Tick(WardenBrain.DefaultStaggerSeconds + 0.01f, 10f);
        AssertThat(warden.State).IsEqual(WardenState.Advance);
        AssertThat(warden.ReceiveStrike(25, isCharged: false)).IsEqual(StrikeOutcome.Deflected);
    }

    [TestCase]
    public void TheWardenFallsToTwoGuardBreaksAndTheirFollowUps()
    {
        var warden = new WardenBrain();
        warden.Awaken();
        var outcome = StrikeOutcome.Ignored;
        for (var cycle = 0; cycle < 2 && outcome != StrikeOutcome.Killed; cycle++)
        {
            outcome = warden.ReceiveStrike(70, isCharged: true);
            for (var i = 0; i < 4 && outcome != StrikeOutcome.Killed; i++)
                outcome = warden.ReceiveStrike(25, isCharged: false);
            warden.Tick(WardenBrain.DefaultStaggerSeconds + 0.01f, 10f);
        }

        AssertThat(outcome).IsEqual(StrikeOutcome.Killed);
        AssertThat(warden.IsDead).IsTrue();
    }

    [TestCase]
    public void TheSlamIsTelegraphedAndLandsOnce()
    {
        var warden = new WardenBrain();
        warden.Awaken();

        AssertThat(warden.Tick(0.1f, 2f)).IsFalse();
        AssertThat(warden.State).IsEqual(WardenState.Windup);
        AssertThat(warden.Tick(WardenBrain.DefaultWindupSeconds * 0.5f, 2f)).IsFalse();
        AssertThat(warden.Tick(WardenBrain.DefaultWindupSeconds, 2f)).IsTrue();
        AssertThat(warden.State).IsEqual(WardenState.Recover);
        AssertThat(warden.Tick(0.1f, 2f)).IsFalse();
    }

    [TestCase]
    public void TheHoldoutChargesOnlyWhileThePlazaIsHeld()
    {
        var holdout = new BeaconHoldoutLogic(chargeSeconds: 10f, waves: [new HoldoutWave(0f, 1, 0)]);

        holdout.Tick(4f, playerHoldsPlaza: false, activeEnemies: 0);
        AssertThat(holdout.ChargedSeconds).IsEqual(0f);
        holdout.Tick(4f, playerHoldsPlaza: true, activeEnemies: 0);
        AssertThat(holdout.ChargeFraction).IsEqual(0.4f);
        holdout.Tick(7f, playerHoldsPlaza: true, activeEnemies: 0);
        AssertThat(holdout.IsComplete).IsTrue();
        AssertThat(holdout.Tick(1f, true, 0).Count).IsEqual(0);
    }

    [TestCase]
    public void HoldoutWavesArriveByChargeAndNeverExceedTheActiveCap()
    {
        var holdout = new BeaconHoldoutLogic(
            chargeSeconds: 60f,
            waves: [new HoldoutWave(0f, 2, 0), new HoldoutWave(10f, 1, 2)],
            maxActiveEnemies: 3);

        AssertThat(holdout.Tick(0.1f, true, 0).Count).IsEqual(2);
        AssertThat(holdout.Tick(5f, true, 2).Count).IsEqual(0);
        var arrivals = holdout.Tick(5f, true, 2);
        AssertThat(arrivals.Count).IsEqual(1);
        AssertThat(holdout.PendingSpawns).IsEqual(2);
        AssertThat(holdout.Tick(0.1f, true, 3).Count).IsEqual(0);
        AssertThat(holdout.Tick(0.1f, true, 1).Count).IsEqual(2);
        AssertThat(holdout.TotalEnemies).IsEqual(5);
    }

    [TestCase]
    public void TheDefaultHoldoutIsWinnableWithinTheCap()
    {
        var holdout = new BeaconHoldoutLogic();
        AssertThat(holdout.MaxActiveEnemies).IsLessEqual(3);
        AssertThat(holdout.TotalEnemies).IsBetween(6, 12);
        AssertThat(holdout.ChargeSeconds).IsBetween(45f, 120f);
    }
}
