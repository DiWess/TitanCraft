using System;
using TitanCraft.Combat;

namespace TitanCraft.Enemies;

public enum WardenState
{
    /// <summary>Sealed in the tower court until its gate is broken.</summary>
    Dormant,

    /// <summary>Guard up, walking at the player.</summary>
    Advance,

    /// <summary>Arms raised: a slam lands at the end of this.</summary>
    Windup,

    /// <summary>Slam just landed; slow to raise its guard again.</summary>
    Recover,

    /// <summary>Guard broken by a charged strike: open to every blow.</summary>
    Staggered,

    Dead,
}

/// <summary>
/// Deterministic state machine for the Galaxabrain Warden, the heavy variant
/// (README section 6 amendment, D3). It is the fight the Mk III breaker coil
/// answers: its guard turns aside every ordinary strike, a charged strike
/// breaks the guard and staggers it, and only then do ordinary strikes land.
/// Its one attack is a slow, telegraphed melee slam (no ranged attack, README
/// section 12).
/// </summary>
public sealed class WardenBrain
{
    public const int DefaultHealth = 240;
    public const float DefaultSlamRange = 2.8f;
    public const float DefaultSlamReach = 3.3f;
    public const float DefaultWindupSeconds = 0.95f;
    public const float DefaultRecoverSeconds = 1.1f;
    public const float DefaultStaggerSeconds = 3.6f;

    /// <summary>A charged strike against the raised guard: breaks it, and some of the blow gets through.</summary>
    public const int GuardBreakDamage = 40;

    private readonly float _slamRange;
    private readonly float _windupSeconds;
    private readonly float _recoverSeconds;
    private readonly float _staggerSeconds;
    private float _timer;

    public WardenBrain(
        int maxHealth = DefaultHealth,
        float slamRange = DefaultSlamRange,
        float windupSeconds = DefaultWindupSeconds,
        float recoverSeconds = DefaultRecoverSeconds,
        float staggerSeconds = DefaultStaggerSeconds)
    {
        if (maxHealth <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxHealth), "Warden health must be greater than zero.");
        if (slamRange <= 0f || windupSeconds <= 0f || recoverSeconds <= 0f || staggerSeconds <= 0f)
            throw new ArgumentOutOfRangeException(nameof(slamRange), "Warden timings and range must be greater than zero.");

        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        _slamRange = slamRange;
        _windupSeconds = windupSeconds;
        _recoverSeconds = recoverSeconds;
        _staggerSeconds = staggerSeconds;
    }

    public int MaxHealth { get; }

    public int CurrentHealth { get; private set; }

    public WardenState State { get; private set; } = WardenState.Dormant;

    public bool IsDead => State == WardenState.Dead;

    public bool IsGuarding => State is WardenState.Advance or WardenState.Windup or WardenState.Recover;

    public float StateTimer => _timer;

    public void Awaken()
    {
        if (State == WardenState.Dormant)
            State = WardenState.Advance;
    }

    /// <summary>
    /// Advances timers. Returns true on the tick the slam lands, so the body can
    /// test reach and apply damage at that moment.
    /// </summary>
    public bool Tick(float deltaSeconds, float distanceToPlayer)
    {
        if (State is WardenState.Dormant or WardenState.Dead)
            return false;

        _timer -= Math.Max(0f, deltaSeconds);
        switch (State)
        {
            case WardenState.Advance:
                if (distanceToPlayer <= _slamRange)
                    Enter(WardenState.Windup, _windupSeconds);
                return false;
            case WardenState.Windup:
                if (_timer <= 0f)
                {
                    Enter(WardenState.Recover, _recoverSeconds);
                    return true;
                }
                return false;
            case WardenState.Recover:
            case WardenState.Staggered:
                if (_timer <= 0f)
                    State = WardenState.Advance;
                return false;
        }

        return false;
    }

    public StrikeOutcome ReceiveStrike(int damage, bool isCharged)
    {
        if (damage <= 0 || State is WardenState.Dead or WardenState.Dormant)
            return StrikeOutcome.Ignored;

        if (State == WardenState.Staggered)
            return Damage(damage);

        if (!isCharged)
            return StrikeOutcome.Deflected;

        var outcome = Damage(Math.Min(damage, GuardBreakDamage));
        if (outcome != StrikeOutcome.Killed)
            Enter(WardenState.Staggered, _staggerSeconds);
        return outcome;
    }

    public void MarkDead()
    {
        CurrentHealth = 0;
        State = WardenState.Dead;
    }

    private StrikeOutcome Damage(int damage)
    {
        CurrentHealth = Math.Max(0, CurrentHealth - damage);
        if (CurrentHealth > 0)
            return StrikeOutcome.Hit;

        State = WardenState.Dead;
        return StrikeOutcome.Killed;
    }

    private void Enter(WardenState state, float seconds)
    {
        State = state;
        _timer = seconds;
    }
}
