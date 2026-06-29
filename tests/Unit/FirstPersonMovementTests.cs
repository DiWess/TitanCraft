// These tests use Godot value-type math only; keeping them runtime-free prevents the GdUnit adapter connection timeout in CI.
#pragma warning disable GdUnit0501

using GdUnit4;
using Godot;
using TitanCraft.Player;
using static GdUnit4.Assertions;

namespace TitanCraft.Tests.Unit;

[TestSuite]
public sealed class FirstPersonMovementTests
{
    [TestCase]
    public void ClampPitchLimitsVerticalAngle()
    {
        var max = Mathf.DegToRad(85.0f);
        AssertThat(FirstPersonMovement.ClampPitch(Mathf.DegToRad(120.0f), 85.0f)).IsEqual(max);
        AssertThat(FirstPersonMovement.ClampPitch(Mathf.DegToRad(-120.0f), 85.0f)).IsEqual(-max);
        AssertThat(FirstPersonMovement.ClampPitch(Mathf.DegToRad(30.0f), 85.0f)).IsEqual(Mathf.DegToRad(30.0f));
    }

    [TestCase]
    public void MoveDirectionIsNormalizedForDiagonalInput()
    {
        var direction = FirstPersonMovement.GetMoveDirection(Basis.Identity, new Vector2(1.0f, 1.0f));
        AssertThat(direction.Length()).IsLessEqual(1.0001f);
        AssertThat(direction.Y).IsEqual(0.0f);
    }

    [TestCase]
    public void CardinalDirectionsRemainHorizontalAndMeasurable()
    {
        AssertThat(FirstPersonMovement.GetMoveDirection(Basis.Identity, Vector2.Up).Z).IsLess(0.0f);
        AssertThat(FirstPersonMovement.GetMoveDirection(Basis.Identity, Vector2.Down).Z).IsGreater(0.0f);
        AssertThat(FirstPersonMovement.GetMoveDirection(Basis.Identity, Vector2.Left).X).IsLess(0.0f);
        AssertThat(FirstPersonMovement.GetMoveDirection(Basis.Identity, Vector2.Right).X).IsGreater(0.0f);
    }

    [TestCase]
    public void ControllerParametersAreValidInPrototypeRanges()
    {
        AssertThat(FirstPersonMovement.HasValidParameters(5.0f, 4.5f, 0.0025f, 85.0f)).IsTrue();
        AssertThat(FirstPersonMovement.HasValidParameters(0.0f, 4.5f, 0.0025f, 85.0f)).IsFalse();
        AssertThat(FirstPersonMovement.HasValidParameters(5.0f, 0.0f, 0.0025f, 85.0f)).IsFalse();
        AssertThat(FirstPersonMovement.HasValidParameters(5.0f, 4.5f, 0.0f, 85.0f)).IsFalse();
        AssertThat(FirstPersonMovement.HasValidParameters(5.0f, 4.5f, 0.0025f, 95.0f)).IsFalse();
    }
}

#pragma warning restore GdUnit0501
