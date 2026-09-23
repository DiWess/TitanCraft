using Godot;
using GdUnit4;
using TitanCraft.UI;
using TitanCraft.World;
using static GdUnit4.Assertions;

namespace TitanCraft.Tests.Unit;

/// <summary>
/// Covers the action-driven onboarding flow and the environment motion
/// scheduling. Both are pure logic so the rules can be asserted without a
/// running scene.
/// </summary>
[TestSuite]
public sealed class OnboardingAndMotionTests
{
    private static OnboardingTutorialState Ready()
    {
        // Every step holds for a minimum time before it can advance; tests that
        // care about the action, not the timer, clear it first.
        var state = new OnboardingTutorialState();
        state.Tick(OnboardingTutorialState.MinimumStepSeconds);
        return state;
    }

    [TestCase]
    public void OnboardingStartsOnTheLookStepWithAPrompt()
    {
        var state = new OnboardingTutorialState();

        AssertThat(state.CurrentStep).IsEqual(OnboardingStep.Look);
        AssertThat(state.IsComplete).IsFalse();
        AssertThat(state.CurrentPrompt.Length > 0).IsTrue();
    }

    [TestCase]
    public void OnboardingHoldsEachStepLongEnoughToRead()
    {
        var state = new OnboardingTutorialState();
        // Enough look travel to satisfy the step, but no time elapsed yet.
        state.ReportLook(OnboardingTutorialState.LookThresholdRadians * 2.0f);

        AssertThat(state.CurrentStep).IsEqual(OnboardingStep.Look);

        state.Tick(OnboardingTutorialState.MinimumStepSeconds);
        AssertThat(state.CurrentStep).IsEqual(OnboardingStep.Move);
    }

    [TestCase]
    public void OnboardingAdvancesFromLookOnlyAfterEnoughLookTravel()
    {
        var state = Ready();
        state.ReportLook(OnboardingTutorialState.LookThresholdRadians * 0.4f);

        AssertThat(state.CurrentStep).IsEqual(OnboardingStep.Look);

        state.ReportLook(OnboardingTutorialState.LookThresholdRadians);
        AssertThat(state.CurrentStep).IsEqual(OnboardingStep.Move);
    }

    [TestCase]
    public void OnboardingCountsLookTravelInBothDirections()
    {
        var state = Ready();
        // Turning left then right is still looking around.
        state.ReportLook(OnboardingTutorialState.LookThresholdRadians * 0.6f);
        state.ReportLook(-OnboardingTutorialState.LookThresholdRadians * 0.6f);

        AssertThat(state.CurrentStep).IsEqual(OnboardingStep.Move);
    }

    [TestCase]
    public void OnboardingWalksThroughLookMoveJumpInOrder()
    {
        var state = Ready();
        state.ReportLook(OnboardingTutorialState.LookThresholdRadians);
        AssertThat(state.CurrentStep).IsEqual(OnboardingStep.Move);

        state.Tick(OnboardingTutorialState.MinimumStepSeconds);
        state.ReportGroundTravel(OnboardingTutorialState.MoveThresholdMeters);
        AssertThat(state.CurrentStep).IsEqual(OnboardingStep.Jump);

        state.ReportJump();
        AssertThat(state.CurrentStep).IsEqual(OnboardingStep.Collect);
    }

    [TestCase]
    public void OnboardingIgnoresActionsThatBelongToOtherSteps()
    {
        var state = Ready();
        // Jumping during the look step must not skip ahead.
        state.ReportJump();
        state.ReportGroundTravel(100.0f);

        AssertThat(state.CurrentStep).IsEqual(OnboardingStep.Look);
    }

    [TestCase]
    public void CollectingAResourceSkipsPastTheBasicMovementSteps()
    {
        // A player who ignores the prompts and simply plays must never be left
        // staring at "move the mouse" while holding resources.
        var state = new OnboardingTutorialState();
        state.ReportResourceCollected();

        AssertThat(state.CurrentStep).IsEqual(OnboardingStep.Craft);
    }

    [TestCase]
    public void BuildingTheArmSkipsToTheAttackStep()
    {
        var state = new OnboardingTutorialState();
        state.ReportMechanicalArmBuilt();

        AssertThat(state.CurrentStep).IsEqual(OnboardingStep.Attack);
    }

    [TestCase]
    public void LandingAnAttackCompletesOnboardingAndClearsThePrompt()
    {
        var state = new OnboardingTutorialState();
        state.ReportAttackLanded();

        AssertThat(state.CurrentStep).IsEqual(OnboardingStep.Complete);
        AssertThat(state.IsComplete).IsTrue();
        AssertThat(state.CurrentPrompt).IsEqual(string.Empty);
    }

    [TestCase]
    public void OnboardingNeverRunsBackwards()
    {
        var state = new OnboardingTutorialState();
        state.ReportMechanicalArmBuilt();
        AssertThat(state.CurrentStep).IsEqual(OnboardingStep.Attack);

        // Late-arriving earlier events (a second pickup, a jump) must not
        // rewind the player into an already-taught step.
        state.ReportResourceCollected();
        state.ReportJump();
        state.ReportLook(99.0f);

        AssertThat(state.CurrentStep).IsEqual(OnboardingStep.Attack);
    }

    [TestCase]
    public void SkipRetiresOnboardingOutright()
    {
        var state = new OnboardingTutorialState();
        state.Skip();

        AssertThat(state.IsComplete).IsTrue();
        AssertThat(state.CurrentPrompt).IsEqual(string.Empty);
    }

    [TestCase]
    public void CompletedOnboardingIgnoresFurtherTicks()
    {
        var state = new OnboardingTutorialState();
        state.Skip();
        state.Tick(10.0f);

        AssertThat(state.CurrentStep).IsEqual(OnboardingStep.Complete);
    }

    [TestCase]
    public void EveryUnfinishedOnboardingStepHasAPrompt()
    {
        var state = new OnboardingTutorialState();
        foreach (var step in new[]
                 {
                     OnboardingStep.Look, OnboardingStep.Move, OnboardingStep.Jump,
                     OnboardingStep.Collect, OnboardingStep.Craft, OnboardingStep.Attack,
                 })
        {
            AdvanceTo(state, step);
            AssertThat(state.CurrentPrompt.Length > 0).IsTrue();
        }
    }

    private static void AdvanceTo(OnboardingTutorialState state, OnboardingStep target)
    {
        var guard = 0;
        while (state.CurrentStep < target && guard++ < 50)
        {
            state.Tick(OnboardingTutorialState.MinimumStepSeconds);
            state.ReportLook(OnboardingTutorialState.LookThresholdRadians);
            state.ReportGroundTravel(OnboardingTutorialState.MoveThresholdMeters);
            state.ReportJump();
        }
    }

    [TestCase]
    public void MotionPhaseOffsetStaysInsideTheClip()
    {
        AssertThat(EnvironmentMotionPlayer.PhaseOffsetFor(0.0f, 5.0f)).IsEqual(0.0f);
        AssertThat(EnvironmentMotionPlayer.PhaseOffsetFor(1.0f, 5.0f) < 5.0f).IsTrue();
        AssertThat(EnvironmentMotionPlayer.PhaseOffsetFor(2.0f, 5.0f) < 5.0f).IsTrue();
        AssertThat(EnvironmentMotionPlayer.PhaseOffsetFor(-1.0f, 5.0f)).IsEqual(0.0f);
    }

    [TestCase]
    public void MotionPhaseOffsetHandlesAZeroLengthClip()
    {
        AssertThat(EnvironmentMotionPlayer.PhaseOffsetFor(0.7f, 0.0f)).IsEqual(0.0f);
    }

    [TestCase]
    public void MotionSeedIsStableForTheSamePosition()
    {
        var a = EnvironmentMotionPlayer.ComputeSeed(new Vector3(12.2f, 0.4f, -6.1f));
        var b = EnvironmentMotionPlayer.ComputeSeed(new Vector3(12.2f, 9.9f, -6.1f));

        // Height is deliberately excluded: the same prop at two heights on one
        // facade should still be one seed.
        AssertThat(a).IsEqual(b);
    }

    [TestCase]
    public void MotionSeedsSeparateNearbyProps()
    {
        // The first implementation produced near-identical seeds for these two
        // positions, so a pair of awnings 21 m apart swayed in lockstep.
        var a = EnvironmentMotionPlayer.ComputeSeed(new Vector3(-8.5f, 0.0f, 0.9f));
        var b = EnvironmentMotionPlayer.ComputeSeed(new Vector3(12.2f, 0.0f, -6.1f));

        AssertThat(Mathf.Abs(a - b) > 0.02f).IsTrue();
    }

    [TestCase]
    public void MotionSeedsAreWellSpreadAcrossTheDistrictPlacements()
    {
        // The actual placement coordinates from the district layout.
        var placements = new[]
        {
            new Vector3(6.2f, 0.0f, 1.2f), new Vector3(-10.6f, 0.0f, -9.4f),
            new Vector3(16.4f, 0.0f, -11.2f), new Vector3(25.6f, 0.0f, -14.4f),
            new Vector3(-9.8f, 0.0f, -6.4f), new Vector3(9.2f, 0.0f, -5.6f),
            new Vector3(-8.5f, 0.0f, 0.9f), new Vector3(12.2f, 0.0f, -6.1f),
            new Vector3(-12.5f, 0.0f, -3.2f), new Vector3(20.2f, 0.0f, -22.6f),
        };

        var smallestGap = 1.0f;
        for (var i = 0; i < placements.Length; i++)
        {
            for (var j = i + 1; j < placements.Length; j++)
            {
                var gap = Mathf.Abs(
                    EnvironmentMotionPlayer.ComputeSeed(placements[i])
                    - EnvironmentMotionPlayer.ComputeSeed(placements[j]));
                smallestGap = Mathf.Min(smallestGap, gap);
            }
        }

        AssertThat(smallestGap > 0.02f).IsTrue();
    }

    [TestCase]
    public void MotionSpeedVariesAroundTheBaseRateAndStaysPositive()
    {
        var slow = EnvironmentMotionPlayer.SpeedForSeed(0.0f, 1.0f, 0.2f);
        var mid = EnvironmentMotionPlayer.SpeedForSeed(0.5f, 1.0f, 0.2f);
        var fast = EnvironmentMotionPlayer.SpeedForSeed(1.0f, 1.0f, 0.2f);

        AssertThat(slow).IsEqualApprox(0.8f, 0.0001f);
        AssertThat(mid).IsEqualApprox(1.0f, 0.0001f);
        AssertThat(fast).IsEqualApprox(1.2f, 0.0001f);
    }

    [TestCase]
    public void MotionSpeedNeverStallsAProp()
    {
        // A zero or negative rate would freeze the prop mid-sway.
        AssertThat(EnvironmentMotionPlayer.SpeedForSeed(0.0f, 0.0f, 0.5f) > 0.0f).IsTrue();
        AssertThat(EnvironmentMotionPlayer.SpeedForSeed(0.0f, -3.0f, 0.5f) > 0.0f).IsTrue();
    }

    [TestCase]
    public void MotionSpeedVariationIsClamped()
    {
        // An out-of-range variation must not invert or stall playback.
        var extreme = EnvironmentMotionPlayer.SpeedForSeed(0.0f, 1.0f, 5.0f);
        AssertThat(extreme > 0.0f).IsTrue();
        AssertThat(extreme).IsEqualApprox(0.5f, 0.0001f);
    }
}
