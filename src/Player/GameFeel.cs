using Godot;

namespace TitanCraft.Player;

/// <summary>
/// Head bob driven by distance travelled rather than by elapsed time, so the
/// step rhythm stays locked to the player's actual movement: stopping mid-stride
/// freezes the bob instead of letting the head keep swaying on the spot.
/// </summary>
public static class ViewBob
{
    public const float StrideLengthMeters = 1.55f;
    public const float VerticalAmplitude = 0.045f;
    public const float HorizontalAmplitude = 0.032f;
    public const float RollDegrees = 0.5f;

    /// <summary>
    /// Vertical offset, lateral offset and roll for a given travelled distance.
    /// The vertical component runs at twice the lateral frequency because both
    /// feet produce a rise and fall while the body sways once per stride.
    /// </summary>
    public static (Vector3 Offset, float RollRadians) Sample(float distanceTravelled, float intensity)
    {
        var clamped = Mathf.Clamp(intensity, 0.0f, 1.0f);
        if (clamped <= 0.0f)
        {
            return (Vector3.Zero, 0.0f);
        }

        var phase = distanceTravelled / StrideLengthMeters * Mathf.Tau;
        var vertical = -Mathf.Abs(Mathf.Sin(phase)) * VerticalAmplitude * clamped;
        var lateral = Mathf.Sin(phase * 0.5f) * HorizontalAmplitude * clamped;
        var roll = Mathf.Sin(phase * 0.5f) * Mathf.DegToRad(RollDegrees) * clamped;
        return (new Vector3(lateral, vertical, 0.0f), roll);
    }
}

/// <summary>
/// Weapon kick: an instant angular impulse that decays back to centre.
/// Separate from camera trauma because a strike should punch the view in a
/// known direction (up and slightly across), not rattle it randomly.
/// </summary>
public sealed class WeaponKick
{
    private readonly float _recoverySpeed;
    private float _pitch;
    private float _yaw;

    public WeaponKick(float recoverySpeed = 9.0f)
    {
        _recoverySpeed = Mathf.Max(0.01f, recoverySpeed);
    }

    public float PitchRadians => _pitch;

    public float YawRadians => _yaw;

    public bool IsActive => Mathf.Abs(_pitch) > 0.0001f || Mathf.Abs(_yaw) > 0.0001f;

    public void Add(float pitchRadians, float yawRadians)
    {
        _pitch = Mathf.Clamp(_pitch + pitchRadians, -0.5f, 0.5f);
        _yaw = Mathf.Clamp(_yaw + yawRadians, -0.5f, 0.5f);
    }

    public void Tick(float deltaSeconds)
    {
        if (deltaSeconds <= 0.0f)
        {
            return;
        }

        // Exponential recovery: fast at first, easing as it centres, which is
        // what makes a kick read as a snap rather than a slide.
        var decay = Mathf.Exp(-_recoverySpeed * deltaSeconds);
        _pitch *= decay;
        _yaw *= decay;
        if (Mathf.Abs(_pitch) < 0.0001f)
        {
            _pitch = 0.0f;
        }

        if (Mathf.Abs(_yaw) < 0.0001f)
        {
            _yaw = 0.0f;
        }
    }
}

/// <summary>
/// Landing dip: the camera drops and springs back in proportion to impact
/// speed, so a step off a kerb and a drop from the terrace do not feel alike.
/// </summary>
public sealed class LandingImpact
{
    public const float MinimumImpactSpeed = 3.0f;
    public const float MaximumImpactSpeed = 14.0f;
    public const float MaximumDipMeters = 0.16f;

    private readonly float _durationSeconds;
    private float _remaining;
    private float _strength;

    public LandingImpact(float durationSeconds = 0.32f)
    {
        _durationSeconds = Mathf.Max(0.01f, durationSeconds);
    }

    public bool IsActive => _remaining > 0.0f;

    /// <summary>0 for a landing too soft to feel, up to 1 for a hard drop.</summary>
    public static float StrengthFor(float impactSpeed)
    {
        if (impactSpeed <= MinimumImpactSpeed)
        {
            return 0.0f;
        }

        var range = MaximumImpactSpeed - MinimumImpactSpeed;
        return Mathf.Clamp((impactSpeed - MinimumImpactSpeed) / range, 0.0f, 1.0f);
    }

    public bool Trigger(float impactSpeed)
    {
        var strength = StrengthFor(impactSpeed);
        if (strength <= 0.0f)
        {
            return false;
        }

        _strength = Mathf.Max(_strength, strength);
        _remaining = _durationSeconds;
        return true;
    }

    public void Tick(float deltaSeconds)
    {
        if (deltaSeconds <= 0.0f || _remaining <= 0.0f)
        {
            return;
        }

        _remaining = Mathf.Max(0.0f, _remaining - deltaSeconds);
        if (_remaining <= 0.0f)
        {
            _strength = 0.0f;
        }
    }

    /// <summary>Downward camera offset in metres (negative Y), 0 when at rest.</summary>
    public float CurrentDip()
    {
        if (_remaining <= 0.0f)
        {
            return 0.0f;
        }

        // One half-sine over the duration: straight down, then back up.
        var progress = 1.0f - (_remaining / _durationSeconds);
        return -Mathf.Sin(progress * Mathf.Pi) * MaximumDipMeters * _strength;
    }
}

/// <summary>
/// Viewmodel sway: the arm lags behind the camera when the player turns or
/// strafes, then settles. Without it a first-person arm looks welded to the
/// screen, which is the single clearest tell of an unfinished FPS.
/// </summary>
public sealed class ViewmodelSway
{
    public const float MaxLateralOffset = 0.06f;
    public const float MaxVerticalOffset = 0.045f;
    public const float MaxTiltRadians = 0.09f;

    private readonly float _followSpeed;
    private Vector3 _offset;
    private Vector3 _tilt;

    public ViewmodelSway(float followSpeed = 7.5f)
    {
        _followSpeed = Mathf.Max(0.01f, followSpeed);
    }

    public Vector3 Offset => _offset;

    public Vector3 TiltRadians => _tilt;

    /// <summary>
    /// <paramref name="lookDelta"/> is this frame's mouse motion in radians;
    /// <paramref name="strafeInput"/> and <paramref name="forwardInput"/> are
    /// the -1..1 movement axes.
    /// </summary>
    public void Update(Vector2 lookDelta, float strafeInput, float forwardInput, float deltaSeconds)
    {
        if (deltaSeconds <= 0.0f)
        {
            return;
        }

        var targetOffset = new Vector3(
            Mathf.Clamp(-lookDelta.X * 1.6f - strafeInput * 0.35f, -1.0f, 1.0f) * MaxLateralOffset,
            Mathf.Clamp(-lookDelta.Y * 1.6f, -1.0f, 1.0f) * MaxVerticalOffset,
            Mathf.Clamp(-forwardInput * 0.3f, -1.0f, 1.0f) * MaxLateralOffset);
        var targetTilt = new Vector3(
            Mathf.Clamp(lookDelta.Y * 2.2f, -1.0f, 1.0f) * MaxTiltRadians,
            Mathf.Clamp(lookDelta.X * 2.2f, -1.0f, 1.0f) * MaxTiltRadians,
            Mathf.Clamp(strafeInput * 0.6f, -1.0f, 1.0f) * MaxTiltRadians);

        var weight = 1.0f - Mathf.Exp(-_followSpeed * deltaSeconds);
        _offset = _offset.Lerp(targetOffset, weight);
        _tilt = _tilt.Lerp(targetTilt, weight);
    }
}

/// <summary>Hit marker visibility timer: shown on a confirmed hit, then fades.</summary>
public sealed class HitMarkerState
{
    private readonly float _durationSeconds;
    private float _remaining;

    public HitMarkerState(float durationSeconds = 0.22f)
    {
        _durationSeconds = Mathf.Max(0.01f, durationSeconds);
    }

    public bool IsVisible => _remaining > 0.0f;

    public bool IsLethal { get; private set; }

    /// <summary>1 immediately after a hit, falling linearly to 0.</summary>
    public float Opacity => _remaining <= 0.0f ? 0.0f : _remaining / _durationSeconds;

    public void Trigger(bool lethal)
    {
        _remaining = _durationSeconds;
        IsLethal = lethal;
    }

    public void Tick(float deltaSeconds)
    {
        if (deltaSeconds <= 0.0f)
        {
            return;
        }

        _remaining = Mathf.Max(0.0f, _remaining - deltaSeconds);
        if (_remaining <= 0.0f)
        {
            IsLethal = false;
        }
    }
}

/// <summary>
/// Damage direction indicator: converts a world-space damage source into the
/// screen-space angle the marker is drawn at, so the player knows where the
/// Galaxabrain hit them from instead of only that their health dropped.
/// </summary>
public sealed class DamageIndicatorState
{
    private readonly float _durationSeconds;
    private float _remaining;

    public DamageIndicatorState(float durationSeconds = 1.1f)
    {
        _durationSeconds = Mathf.Max(0.01f, durationSeconds);
    }

    public bool IsVisible => _remaining > 0.0f;

    public float AngleRadians { get; private set; }

    public float Opacity => _remaining <= 0.0f ? 0.0f : _remaining / _durationSeconds;

    /// <summary>
    /// Screen angle for a hit, measured clockwise from straight ahead.
    /// 0 is dead ahead, +/-PI is directly behind the player.
    /// </summary>
    public static float ScreenAngleFor(Vector3 playerPosition, float playerYawRadians, Vector3 damageSource)
    {
        var toSource = damageSource - playerPosition;
        toSource.Y = 0.0f;
        if (toSource.LengthSquared() < 0.0001f)
        {
            return 0.0f;
        }

        // Godot's -Z is forward, and yaw increases counter-clockwise, so the
        // world-space bearing is negated to land in clockwise screen space.
        var worldAngle = Mathf.Atan2(toSource.X, -toSource.Z);
        return Mathf.Wrap(worldAngle - playerYawRadians, -Mathf.Pi, Mathf.Pi);
    }

    public void Trigger(float angleRadians)
    {
        AngleRadians = angleRadians;
        _remaining = _durationSeconds;
    }

    public void Tick(float deltaSeconds)
    {
        if (deltaSeconds <= 0.0f)
        {
            return;
        }

        _remaining = Mathf.Max(0.0f, _remaining - deltaSeconds);
    }
}
