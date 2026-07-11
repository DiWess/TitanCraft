using GdUnit4;
using TitanCraft.Core;
using TitanCraft.Enemies;
using TitanCraft.Player;
using static GdUnit4.Assertions;

namespace TitanCraft.Tests.Unit;

[TestSuite]
public sealed class GameFeelTests
{
    [TestCase]
    public void CameraShakeIntensityUsesTraumaSquaredCurve()
    {
        AssertThat(CameraShaker.CalculateShakeIntensity(0.0f)).IsEqual(0.0f);
        AssertThat(CameraShaker.CalculateShakeIntensity(0.5f)).IsEqual(0.25f);
        AssertThat(CameraShaker.CalculateShakeIntensity(1.0f)).IsEqual(1.0f);
    }

    [TestCase]
    public void CameraShakeIntensityClampsTraumaBeforeSquaring()
    {
        AssertThat(CameraShaker.CalculateShakeIntensity(-1.0f)).IsEqual(0.0f);
        AssertThat(CameraShaker.CalculateShakeIntensity(2.0f)).IsEqual(1.0f);
    }

    [TestCase]
    public void TimeManagerClampsHitStopTimeScaleToSafeRange()
    {
        AssertThat(TimeManager.ClampTimeScale(0.0f)).IsEqual(0.01f);
        AssertThat(TimeManager.ClampTimeScale(0.05f)).IsEqual(0.05f);
        AssertThat(TimeManager.ClampTimeScale(2.0f)).IsEqual(1.0f);
    }

    [TestCase]
    public void ScoutHitFlinchRestsAtIdentityScaleUntilTriggered()
    {
        var flinch = new ScoutHitFlinch(durationSeconds: 0.2f, scalePunch: 0.1f);

        AssertThat(flinch.IsActive).IsFalse();
        AssertThat(flinch.CurrentScale).IsEqual(1.0f);
    }

    [TestCase]
    public void ScoutHitFlinchPunchesScaleOnTriggerThenDecaysLinearly()
    {
        var flinch = new ScoutHitFlinch(durationSeconds: 0.2f, scalePunch: 0.1f);

        flinch.Trigger();
        AssertThat(flinch.IsActive).IsTrue();
        AssertThat(flinch.CurrentScale).IsEqual(1.1f);

        flinch.Tick(0.1f);
        AssertThat(flinch.CurrentScale).IsEqualApprox(1.05f, 0.0001f);
    }

    [TestCase]
    public void ScoutHitFlinchReturnsToRestAfterFullDuration()
    {
        var flinch = new ScoutHitFlinch(durationSeconds: 0.2f, scalePunch: 0.1f);

        flinch.Trigger();
        flinch.Tick(0.5f);

        AssertThat(flinch.IsActive).IsFalse();
        AssertThat(flinch.CurrentScale).IsEqual(1.0f);
    }

    [TestCase]
    public void ScoutHitFlinchIgnoresNegativeTickDeltas()
    {
        var flinch = new ScoutHitFlinch(durationSeconds: 0.2f, scalePunch: 0.1f);

        flinch.Trigger();
        flinch.Tick(-1.0f);

        AssertThat(flinch.IsActive).IsTrue();
        AssertThat(flinch.CurrentScale).IsEqual(1.1f);
    }
}
