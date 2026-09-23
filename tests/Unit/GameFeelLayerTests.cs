// These tests use Godot value-type math only (Vector2/Vector3/Mathf); keeping them
// runtime-free prevents the GdUnit adapter connection timeout in CI, which is why
// this suite suppresses GdUnit0501 rather than adding [RequireGodotRuntime].
// Same rationale and pattern as tests/Unit/FirstPersonMovementTests.cs.
#pragma warning disable GdUnit0501

using Godot;
using GdUnit4;
using TitanCraft.Player;
using static GdUnit4.Assertions;

namespace TitanCraft.Tests.Unit;

/// <summary>
/// Covers the camera-feel and combat-feedback layers added for the game-feel
/// pass. Every behaviour here is pure logic precisely so it can be asserted
/// without a running scene -- the feel itself still needs a human at a Windows
/// build, but the maths behind it does not.
/// </summary>
[TestSuite]
public sealed class GameFeelLayerTests
{
    [TestCase]
    public void ViewBobIsSilentWhenTheresNoMovementIntensity()
    {
        var (offset, roll) = ViewBob.Sample(12.5f, 0.0f);

        AssertThat(offset).IsEqual(Vector3.Zero);
        AssertThat(roll).IsEqual(0.0f);
    }

    [TestCase]
    public void ViewBobAdvancesWithDistanceTravelledNotWithTime()
    {
        // Two samples one full stride apart must land on the same phase.
        var (firstOffset, firstRoll) = ViewBob.Sample(0.0f, 1.0f);
        var (strideLater, strideLaterRoll) = ViewBob.Sample(ViewBob.StrideLengthMeters * 2.0f, 1.0f);

        AssertThat(strideLater.Y).IsEqualApprox(firstOffset.Y, 0.0001f);
        AssertThat(strideLaterRoll).IsEqualApprox(firstRoll, 0.0001f);
    }

    [TestCase]
    public void ViewBobVerticalOffsetNeverRisesAboveTheRestPose()
    {
        // The head dips through the stride; it must not float upward, which
        // would read as bouncing rather than walking.
        for (var step = 0; step <= 40; step++)
        {
            var (offset, _) = ViewBob.Sample(step * 0.1f, 1.0f);
            AssertThat(offset.Y <= 0.0001f).IsTrue();
            AssertThat(Mathf.Abs(offset.Y) <= ViewBob.VerticalAmplitude + 0.0001f).IsTrue();
        }
    }

    [TestCase]
    public void ViewBobScalesWithIntensityAndClampsAboveOne()
    {
        var quarterStride = ViewBob.StrideLengthMeters * 0.25f;
        var (half, _) = ViewBob.Sample(quarterStride, 0.5f);
        var (full, _) = ViewBob.Sample(quarterStride, 1.0f);
        var (overdriven, _) = ViewBob.Sample(quarterStride, 4.0f);

        AssertThat(Mathf.Abs(half.Y) < Mathf.Abs(full.Y)).IsTrue();
        AssertThat(overdriven.Y).IsEqualApprox(full.Y, 0.0001f);
    }

    [TestCase]
    public void WeaponKickStartsCentredAndDecaysBackToCentre()
    {
        var kick = new WeaponKick();
        AssertThat(kick.IsActive).IsFalse();

        kick.Add(0.04f, 0.01f);
        AssertThat(kick.PitchRadians).IsEqual(0.04f);
        AssertThat(kick.IsActive).IsTrue();

        kick.Tick(1.0f);
        AssertThat(kick.PitchRadians < 0.04f).IsTrue();

        kick.Tick(5.0f);
        AssertThat(kick.IsActive).IsFalse();
        AssertThat(kick.PitchRadians).IsEqual(0.0f);
    }

    [TestCase]
    public void WeaponKickClampsAccumulatedImpulses()
    {
        var kick = new WeaponKick();
        for (var i = 0; i < 100; i++)
        {
            kick.Add(0.05f, 0.05f);
        }

        AssertThat(kick.PitchRadians).IsEqual(0.5f);
        AssertThat(kick.YawRadians).IsEqual(0.5f);
    }

    [TestCase]
    public void WeaponKickIgnoresNonPositiveDeltas()
    {
        var kick = new WeaponKick();
        kick.Add(0.04f, 0.0f);
        kick.Tick(-1.0f);

        AssertThat(kick.PitchRadians).IsEqual(0.04f);
    }

    [TestCase]
    public void LandingImpactIgnoresSoftTouchdowns()
    {
        var landing = new LandingImpact();

        AssertThat(LandingImpact.StrengthFor(0.0f)).IsEqual(0.0f);
        AssertThat(LandingImpact.StrengthFor(LandingImpact.MinimumImpactSpeed)).IsEqual(0.0f);
        AssertThat(landing.Trigger(1.5f)).IsFalse();
        AssertThat(landing.IsActive).IsFalse();
        AssertThat(landing.CurrentDip()).IsEqual(0.0f);
    }

    [TestCase]
    public void LandingImpactStrengthRisesWithSpeedAndSaturates()
    {
        var soft = LandingImpact.StrengthFor(LandingImpact.MinimumImpactSpeed + 2.0f);
        var hard = LandingImpact.StrengthFor(LandingImpact.MaximumImpactSpeed);
        var extreme = LandingImpact.StrengthFor(LandingImpact.MaximumImpactSpeed * 4.0f);

        AssertThat(soft > 0.0f && soft < hard).IsTrue();
        AssertThat(hard).IsEqual(1.0f);
        AssertThat(extreme).IsEqual(1.0f);
    }

    [TestCase]
    public void LandingImpactDipsDownwardThenReturnsToRest()
    {
        var landing = new LandingImpact(durationSeconds: 0.4f);
        AssertThat(landing.Trigger(LandingImpact.MaximumImpactSpeed)).IsTrue();

        landing.Tick(0.2f);
        var midDip = landing.CurrentDip();
        AssertThat(midDip < 0.0f).IsTrue();
        AssertThat(Mathf.Abs(midDip) <= LandingImpact.MaximumDipMeters + 0.0001f).IsTrue();

        landing.Tick(0.4f);
        AssertThat(landing.IsActive).IsFalse();
        AssertThat(landing.CurrentDip()).IsEqual(0.0f);
    }

    [TestCase]
    public void ViewmodelSwayLagsBehindTheCameraThenSettles()
    {
        var sway = new ViewmodelSway();
        AssertThat(sway.Offset).IsEqual(Vector3.Zero);

        // A sustained turn pushes the arm off centre.
        for (var i = 0; i < 20; i++)
        {
            sway.Update(new Vector2(0.05f, 0.0f), 0.0f, 0.0f, 0.016f);
        }

        var turnedOffset = sway.Offset;
        AssertThat(Mathf.Abs(turnedOffset.X) > 0.001f).IsTrue();
        AssertThat(Mathf.Abs(turnedOffset.X) <= ViewmodelSway.MaxLateralOffset + 0.0001f).IsTrue();

        // Holding still returns it toward the rest pose.
        for (var i = 0; i < 60; i++)
        {
            sway.Update(Vector2.Zero, 0.0f, 0.0f, 0.016f);
        }

        AssertThat(sway.Offset.Length() < turnedOffset.Length()).IsTrue();
    }

    [TestCase]
    public void ViewmodelSwayIgnoresNonPositiveDeltas()
    {
        var sway = new ViewmodelSway();
        sway.Update(new Vector2(0.5f, 0.5f), 1.0f, 1.0f, 0.0f);

        AssertThat(sway.Offset).IsEqual(Vector3.Zero);
    }

    [TestCase]
    public void HitMarkerFadesOutOverItsDuration()
    {
        var marker = new HitMarkerState(durationSeconds: 0.2f);
        AssertThat(marker.IsVisible).IsFalse();

        marker.Trigger(lethal: false);
        AssertThat(marker.IsVisible).IsTrue();
        AssertThat(marker.IsLethal).IsFalse();
        AssertThat(marker.Opacity).IsEqual(1.0f);

        marker.Tick(0.1f);
        AssertThat(marker.Opacity).IsEqualApprox(0.5f, 0.0001f);

        marker.Tick(0.2f);
        AssertThat(marker.IsVisible).IsFalse();
        AssertThat(marker.Opacity).IsEqual(0.0f);
    }

    [TestCase]
    public void HitMarkerReportsLethalHitsAndClearsTheFlagOnExpiry()
    {
        var marker = new HitMarkerState(durationSeconds: 0.2f);
        marker.Trigger(lethal: true);
        AssertThat(marker.IsLethal).IsTrue();

        marker.Tick(0.5f);
        AssertThat(marker.IsLethal).IsFalse();
    }

    [TestCase]
    public void DamageIndicatorPointsStraightAheadForAThreatInFront()
    {
        // Godot's -Z is forward, so a source at -Z with zero yaw is dead ahead.
        var angle = DamageIndicatorState.ScreenAngleFor(
            Vector3.Zero, 0.0f, new Vector3(0.0f, 0.0f, -5.0f));

        AssertThat(Mathf.Abs(angle) < 0.0001f).IsTrue();
    }

    [TestCase]
    public void DamageIndicatorPointsBehindForAThreatAtTheBack()
    {
        var angle = DamageIndicatorState.ScreenAngleFor(
            Vector3.Zero, 0.0f, new Vector3(0.0f, 0.0f, 5.0f));

        AssertThat(Mathf.Abs(angle)).IsEqualApprox(Mathf.Pi, 0.0001f);
    }

    [TestCase]
    public void DamageIndicatorPointsToTheSideForAFlankingThreat()
    {
        var right = DamageIndicatorState.ScreenAngleFor(
            Vector3.Zero, 0.0f, new Vector3(5.0f, 0.0f, 0.0f));
        var left = DamageIndicatorState.ScreenAngleFor(
            Vector3.Zero, 0.0f, new Vector3(-5.0f, 0.0f, 0.0f));

        AssertThat(right).IsEqualApprox(Mathf.Pi / 2.0f, 0.0001f);
        AssertThat(left).IsEqualApprox(-Mathf.Pi / 2.0f, 0.0001f);
    }

    [TestCase]
    public void DamageIndicatorIsRelativeToWhereThePlayerIsFacing()
    {
        // Turning to face the threat must bring the marker back to centre.
        var source = new Vector3(5.0f, 0.0f, 0.0f);
        var facingAway = DamageIndicatorState.ScreenAngleFor(Vector3.Zero, 0.0f, source);
        var facingThreat = DamageIndicatorState.ScreenAngleFor(Vector3.Zero, facingAway, source);

        AssertThat(Mathf.Abs(facingThreat) < 0.0001f).IsTrue();
    }

    [TestCase]
    public void DamageIndicatorIgnoresHeightAndCoincidentSources()
    {
        var overhead = DamageIndicatorState.ScreenAngleFor(
            Vector3.Zero, 0.0f, new Vector3(0.0f, 9.0f, -5.0f));
        var coincident = DamageIndicatorState.ScreenAngleFor(
            Vector3.Zero, 0.0f, new Vector3(0.0f, 3.0f, 0.0f));

        AssertThat(Mathf.Abs(overhead) < 0.0001f).IsTrue();
        AssertThat(coincident).IsEqual(0.0f);
    }

    [TestCase]
    public void DamageIndicatorFadesOutOverItsDuration()
    {
        var indicator = new DamageIndicatorState(durationSeconds: 1.0f);
        AssertThat(indicator.IsVisible).IsFalse();

        indicator.Trigger(Mathf.Pi / 4.0f);
        AssertThat(indicator.IsVisible).IsTrue();
        AssertThat(indicator.AngleRadians).IsEqualApprox(Mathf.Pi / 4.0f, 0.0001f);

        indicator.Tick(0.5f);
        AssertThat(indicator.Opacity).IsEqualApprox(0.5f, 0.0001f);

        indicator.Tick(1.0f);
        AssertThat(indicator.IsVisible).IsFalse();
    }
}
