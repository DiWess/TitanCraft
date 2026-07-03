using GdUnit4;
using TitanCraft.Core;
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
}
