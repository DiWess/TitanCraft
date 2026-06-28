using Godot;

namespace TitanCraft.Player;

public static class FirstPersonMovement
{
    public static float ClampPitch(float pitchRadians, float maxLookAngleDegrees)
    {
        var maxAngle = Mathf.DegToRad(maxLookAngleDegrees);
        return Mathf.Clamp(pitchRadians, -maxAngle, maxAngle);
    }

    public static Vector3 GetMoveDirection(Basis basis, Vector2 inputDirection)
    {
        return (basis * new Vector3(inputDirection.X, 0.0f, inputDirection.Y)).Normalized();
    }

    public static bool HasValidParameters(float walkSpeed, float jumpVelocity, float mouseSensitivity, float maxLookAngleDegrees)
    {
        return walkSpeed > 0.0f
            && jumpVelocity > 0.0f
            && mouseSensitivity > 0.0f
            && maxLookAngleDegrees is > 0.0f and <= 89.0f;
    }
}
