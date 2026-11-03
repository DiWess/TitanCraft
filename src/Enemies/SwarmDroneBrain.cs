using System;
using TitanCraft.Combat;

namespace TitanCraft.Enemies;

public enum SwarmDroneState
{
    /// <summary>Perched and inert until its nest is disturbed.</summary>
    Dormant,

    /// <summary>Circling above the player, out of the arm's reach.</summary>
    Orbit,

    /// <summary>Glowing and holding still: a dive is coming.</summary>
    Telegraph,

    /// <summary>Ramming down at the player; strikeable as it closes.</summary>
    Dive,

    /// <summary>Climbing back to its orbit after a dive.</summary>
    Recover,

    /// <summary>Yanked out of the air by the Mk II clamp; grounded and open.</summary>
    Stunned,

    Dead,
}

/// <summary>
/// Deterministic state machine for the Galaxabrain swarm drone, the third and
/// last variant the README section 6 amendment allows (D3). Its job in the
/// quarter is to be the enemy the Mk II magnet clamp answers: it hangs above the
/// arm's reach and only commits to brief, telegraphed dives, so a player can
/// either time a strike on the dive or pull it down and finish it on the ground.
/// It has no ranged attack (README section 12) and does not coordinate with
/// other drones: each one keeps its own timer.
/// </summary>
public sealed class SwarmDroneBrain
{
    public const int DefaultHealth = 50;
    public const float DefaultTelegraphSeconds = 0.7f;
    public const float DefaultDiveSeconds = 1.3f;
    public const float DefaultRecoverSeconds = 0.9f;
    public const float DefaultStunSeconds = 2.8f;
    public const float DefaultDiveIntervalSeconds = 2.6f;
    public const float DefaultEngageRange = 16f;

    private readonly float _telegraphSeconds;
    private readonly float _diveSeconds;
    private readonly float _recoverSeconds;
    private readonly float _stunSeconds;
    private readonly float _diveIntervalSeconds;
    private readonly float _engageRange;
    private float _timer;

    public SwarmDroneBrain(
        int maxHealth = DefaultHealth,
        float telegraphSeconds = DefaultTelegraphSeconds,
        float diveSeconds = DefaultDiveSeconds,
        float recoverSeconds = DefaultRecoverSeconds,
        float stunSeconds = DefaultStunSeconds,
        float diveIntervalSeconds = DefaultDiveIntervalSeconds,
        float engageRange = DefaultEngageRange,
        float firstDiveDelaySeconds = DefaultDiveIntervalSeconds)
    {
        if (maxHealth <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxHealth), "Drone health must be greater than zero.");
        if (telegraphSeconds <= 0f || diveSeconds <= 0f || recoverSeconds <= 0f || stunSeconds <= 0f || diveIntervalSeconds <= 0f)
            throw new ArgumentOutOfRangeException(nameof(telegraphSeconds), "Drone timings must be greater than zero.");
        if (engageRange <= 0f)
            throw new ArgumentOutOfRangeException(nameof(engageRange), "Engage range must be greater than zero.");

        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        _telegraphSeconds = telegraphSeconds;
        _diveSeconds = diveSeconds;
        _recoverSeconds = recoverSeconds;
        _stunSeconds = stunSeconds;
        _diveIntervalSeconds = diveIntervalSeconds;
        _engageRange = engageRange;
        _timer = Math.Max(0f, firstDiveDelaySeconds);
    }

    public int MaxHealth { get; }

    public int CurrentHealth { get; private set; }

    public SwarmDroneState State { get; private set; } = SwarmDroneState.Dormant;

    public bool IsDead => State == SwarmDroneState.Dead;

    /// <summary>Seconds left in the current timed state (telegraph, dive, recover, stun, or until the next dive).</summary>
    public float StateTimer => _timer;

    public void Activate()
    {
        if (State == SwarmDroneState.Dormant)
            State = SwarmDroneState.Orbit;
    }

    public void Tick(float deltaSeconds, float distanceToPlayer)
    {
        if (State is SwarmDroneState.Dormant or SwarmDroneState.Dead)
            return;

        _timer -= Math.Max(0f, deltaSeconds);
        switch (State)
        {
            case SwarmDroneState.Orbit:
                if (_timer <= 0f && distanceToPlayer <= _engageRange)
                    Enter(SwarmDroneState.Telegraph, _telegraphSeconds);
                break;
            case SwarmDroneState.Telegraph:
                if (_timer <= 0f)
                    Enter(SwarmDroneState.Dive, _diveSeconds);
                break;
            case SwarmDroneState.Dive:
                if (_timer <= 0f)
                    Enter(SwarmDroneState.Recover, _recoverSeconds);
                break;
            case SwarmDroneState.Recover:
            case SwarmDroneState.Stunned:
                if (_timer <= 0f)
                    Enter(SwarmDroneState.Orbit, _diveIntervalSeconds);
                break;
        }
    }

    /// <summary>The dive reached the player. True if this contact should deal damage.</summary>
    public bool ReportDiveContact()
    {
        if (State != SwarmDroneState.Dive)
            return false;

        Enter(SwarmDroneState.Recover, _recoverSeconds);
        return true;
    }

    public bool CanBePulled => State is SwarmDroneState.Orbit or SwarmDroneState.Telegraph
        or SwarmDroneState.Dive or SwarmDroneState.Recover;

    /// <summary>Mk II clamp: drags the drone to the ground, stunned.</summary>
    public bool Pull()
    {
        if (!CanBePulled)
            return false;

        Enter(SwarmDroneState.Stunned, _stunSeconds);
        return true;
    }

    public StrikeOutcome ApplyDamage(int damage)
    {
        if (damage <= 0 || State is SwarmDroneState.Dead or SwarmDroneState.Dormant)
            return StrikeOutcome.Ignored;

        CurrentHealth = Math.Max(0, CurrentHealth - damage);
        if (CurrentHealth == 0)
        {
            State = SwarmDroneState.Dead;
            return StrikeOutcome.Killed;
        }

        return StrikeOutcome.Hit;
    }

    public void MarkDead()
    {
        CurrentHealth = 0;
        State = SwarmDroneState.Dead;
    }

    private void Enter(SwarmDroneState state, float seconds)
    {
        State = state;
        _timer = seconds;
    }
}
