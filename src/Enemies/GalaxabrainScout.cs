using System;
using Godot;

namespace TitanCraft.Enemies;

public enum GalaxabrainScoutState
{
    Idle,
    Chase,
    Attack,
    Dead,
}

public sealed class GalaxabrainScoutBrain
{
    public const int DefaultHealth = 100;

    private readonly float _detectionRange;
    private readonly float _attackRange;
    private readonly float _attackCooldownSeconds;
    private float _cooldownRemainingSeconds;

    public GalaxabrainScoutBrain(int maxHealth, float detectionRange, float attackRange, float attackCooldownSeconds)
    {
        if (maxHealth <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxHealth), "Enemy health must be greater than zero.");
        }

        if (detectionRange <= 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(detectionRange), "Detection range must be greater than zero.");
        }

        if (attackRange <= 0f || attackRange > detectionRange)
        {
            throw new ArgumentOutOfRangeException(nameof(attackRange), "Attack range must be greater than zero and no larger than detection range.");
        }

        if (attackCooldownSeconds < 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(attackCooldownSeconds), "Attack cooldown cannot be negative.");
        }

        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        _detectionRange = detectionRange;
        _attackRange = attackRange;
        _attackCooldownSeconds = attackCooldownSeconds;
    }

    public int MaxHealth { get; }

    public int CurrentHealth { get; private set; }

    public GalaxabrainScoutState State { get; private set; } = GalaxabrainScoutState.Idle;

    public bool IsDead => State == GalaxabrainScoutState.Dead;

    public bool CanAttack => State == GalaxabrainScoutState.Attack && _cooldownRemainingSeconds <= 0f;

    public void Tick(float deltaSeconds, float distanceToPlayer)
    {
        if (IsDead)
        {
            return;
        }

        _cooldownRemainingSeconds = Math.Max(0f, _cooldownRemainingSeconds - Math.Max(0f, deltaSeconds));

        State = distanceToPlayer switch
        {
            _ when distanceToPlayer <= _attackRange => GalaxabrainScoutState.Attack,
            _ when distanceToPlayer <= _detectionRange => GalaxabrainScoutState.Chase,
            _ => GalaxabrainScoutState.Idle,
        };
    }

    public bool TryConsumeAttack()
    {
        if (!CanAttack)
        {
            return false;
        }

        _cooldownRemainingSeconds = _attackCooldownSeconds;
        return true;
    }

    public void ApplyDamage(int damage)
    {
        if (damage <= 0 || IsDead)
        {
            return;
        }

        CurrentHealth = Math.Max(0, CurrentHealth - damage);
        if (CurrentHealth == 0)
        {
            State = GalaxabrainScoutState.Dead;
        }
    }
}

public partial class GalaxabrainScout : CharacterBody3D
{
    private GalaxabrainScoutBrain _brain = new(
        GalaxabrainScoutBrain.DefaultHealth,
        detectionRange: 12f,
        attackRange: 2f,
        attackCooldownSeconds: 0.8f);

    [Export] public int Health { get; set; } = GalaxabrainScoutBrain.DefaultHealth;

    [Export] public float DetectionRange { get; set; } = 12f;

    [Export] public float AttackRange { get; set; } = 2f;

    [Export] public int Damage { get; set; } = 10;

    [Export] public float AttackCooldownSeconds { get; set; } = 0.8f;

    [Export] public float ChaseSpeed { get; set; } = 3f;

    [Export] public NodePath? PlayerPath { get; set; }

    [Export] public NodePath? MissionComponentPath { get; set; }

    public GalaxabrainScoutState State => _brain.State;

    public override void _Ready()
    {
        _brain = new GalaxabrainScoutBrain(Health, DetectionRange, AttackRange, AttackCooldownSeconds);
        SetMissionComponentVisible(false);
    }

    public override void _PhysicsProcess(double delta)
    {
        Node3D? player = GetConfiguredPlayer();
        if (player is null || _brain.IsDead)
        {
            Velocity = Vector3.Zero;
            return;
        }

        float distanceToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);
        _brain.Tick((float)delta, distanceToPlayer);

        if (_brain.State == GalaxabrainScoutState.Chase)
        {
            Vector3 direction = (player.GlobalPosition - GlobalPosition).Normalized();
            Velocity = new Vector3(direction.X * ChaseSpeed, Velocity.Y, direction.Z * ChaseSpeed);
            MoveAndSlide();
            return;
        }

        Velocity = Vector3.Zero;
        if (_brain.TryConsumeAttack())
        {
            GD.Print($"Galaxabrain Scout attacks for {Damage} damage.");
        }
    }

    public void ApplyDamage(int damage)
    {
        bool wasAlive = !_brain.IsDead;
        _brain.ApplyDamage(damage);

        if (wasAlive && _brain.IsDead)
        {
            Die();
        }
    }

    private Node3D? GetConfiguredPlayer()
    {
        return PlayerPath is null || PlayerPath.IsEmpty ? null : GetNodeOrNull<Node3D>(PlayerPath);
    }

    private void Die()
    {
        Velocity = Vector3.Zero;
        SetPhysicsProcess(false);
        SetMissionComponentVisible(true);
        Visible = false;
    }

    private void SetMissionComponentVisible(bool isVisible)
    {
        if (MissionComponentPath is null || MissionComponentPath.IsEmpty)
        {
            return;
        }

        Node3D? missionComponent = GetNodeOrNull<Node3D>(MissionComponentPath);
        if (missionComponent is not null)
        {
            missionComponent.Visible = isVisible;
        }
    }
}
