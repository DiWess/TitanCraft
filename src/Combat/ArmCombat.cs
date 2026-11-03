using System;
using Godot;
using TitanCraft.Resources;

namespace TitanCraft.Combat;

/// <summary>One landed Mechanical Arm blow, as the target receives it.</summary>
public readonly record struct ArmStrike(int Damage, bool IsCharged, Vector3 Origin);

public enum StrikeOutcome
{
    /// <summary>The target took damage and survived.</summary>
    Hit,

    /// <summary>The blow finished the target.</summary>
    Killed,

    /// <summary>The target turned the blow aside (the Warden's guard, sound stone).</summary>
    Deflected,

    /// <summary>The target cannot be struck right now (already dead, already broken).</summary>
    Ignored,
}

/// <summary>Anything the arm's strike ray can land on and affect.</summary>
public interface IArmTarget
{
    bool IsStrikeable { get; }

    StrikeOutcome ReceiveStrike(ArmStrike strike);

    /// <summary>Player-facing reason a blow was deflected, naming what would work instead.</summary>
    string DeflectFeedback { get; }
}

/// <summary>Anything the Mk II magnet clamp can yank toward the player.</summary>
public interface IPullTarget
{
    bool CanBePulled { get; }

    /// <summary>Where the clamp's beam attaches, for aim assist and the beam visual.</summary>
    Vector3 PullAnchor { get; }

    /// <summary>Yank toward <paramref name="pullerPosition"/>; false if nothing happened.</summary>
    bool Pull(Vector3 pullerPosition);
}

/// <summary>A target that tells the player, while aimed at, which arm module affects it.</summary>
public interface IAimHint
{
    string GetAimHint(MvpInventory inventory);
}

/// <summary>
/// Mk III breaker coil timing: hold the attack button to charge, release to
/// strike. A tap still throws an ordinary Mk I blow on release, so Mk III never
/// takes the basic attack away.
/// </summary>
public sealed class ChargedStrikeLogic
{
    public const float DefaultChargeSeconds = 0.75f;
    public const int DefaultChargedDamage = 70;
    public const float DefaultChargedRange = 3.6f;
    public const float DefaultRecoverySeconds = 1.1f;

    public ChargedStrikeLogic(float chargeSeconds = DefaultChargeSeconds)
    {
        if (chargeSeconds <= 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(chargeSeconds), "Charge time must be greater than zero.");
        }

        ChargeSeconds = chargeSeconds;
    }

    public float ChargeSeconds { get; }

    public bool IsCharging { get; private set; }

    public float HeldSeconds { get; private set; }

    /// <summary>0 when the button goes down, 1 once a release would be a charged strike.</summary>
    public float ChargeFraction => IsCharging ? Math.Clamp(HeldSeconds / ChargeSeconds, 0f, 1f) : 0f;

    public bool IsFullyCharged => IsCharging && HeldSeconds >= ChargeSeconds;

    public void Begin()
    {
        IsCharging = true;
        HeldSeconds = 0f;
    }

    public void Tick(float deltaSeconds)
    {
        if (IsCharging)
        {
            HeldSeconds += Math.Max(0f, deltaSeconds);
        }
    }

    /// <summary>Ends the hold. True if the release is a charged strike.</summary>
    public bool Release()
    {
        if (!IsCharging)
        {
            return false;
        }

        var charged = HeldSeconds >= ChargeSeconds;
        Cancel();
        return charged;
    }

    public void Cancel()
    {
        IsCharging = false;
        HeldSeconds = 0f;
    }
}

/// <summary>Mk II magnet clamp cooldown, kept engine-free like the strike logic.</summary>
public sealed class MagnetPullLogic
{
    public const float DefaultRange = 16f;
    public const float DefaultCooldownSeconds = 0.9f;

    /// <summary>Aim assist: a target this many degrees off the crosshair still counts.</summary>
    public const float DefaultAssistDegrees = 7f;

    private float _cooldownRemaining;

    public MagnetPullLogic(float cooldownSeconds = DefaultCooldownSeconds)
    {
        if (cooldownSeconds < 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(cooldownSeconds), "Cooldown cannot be negative.");
        }

        CooldownSeconds = cooldownSeconds;
    }

    public float CooldownSeconds { get; }

    public bool IsOnCooldown => _cooldownRemaining > 0f;

    public void Tick(float deltaSeconds)
    {
        _cooldownRemaining = Math.Max(0f, _cooldownRemaining - Math.Max(0f, deltaSeconds));
    }

    public bool TryFire(MvpInventory inventory)
    {
        ArgumentNullException.ThrowIfNull(inventory);
        if (!inventory.HasMagnetClamp || IsOnCooldown)
        {
            return false;
        }

        _cooldownRemaining = CooldownSeconds;
        return true;
    }

    /// <summary>
    /// Whether a target at <paramref name="toTarget"/> from the eye lies within
    /// the assist cone around <paramref name="aimDirection"/> and the clamp's reach.
    /// </summary>
    public static bool IsWithinAssist(Vector3 aimDirection, Vector3 toTarget, float range = DefaultRange,
        float assistDegrees = DefaultAssistDegrees)
    {
        var distance = toTarget.Length();
        if (distance <= 0.001f || distance > range)
        {
            return false;
        }

        var cos = aimDirection.Normalized().Dot(toTarget / distance);
        return cos >= MathF.Cos(Mathf.DegToRad(assistDegrees));
    }
}
