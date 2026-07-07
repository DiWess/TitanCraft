using System;
using Godot;
using TitanCraft.Core;
using TitanCraft.Player;
using TitanCraft.World;

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

    public void MarkDefeated()
    {
        CurrentHealth = 0;
        State = GalaxabrainScoutState.Dead;
    }
}

public partial class GalaxabrainScout : CharacterBody3D
{
    private GalaxabrainScoutBrain _brain = new(
        GalaxabrainScoutBrain.DefaultHealth,
        detectionRange: 12f,
        attackRange: 2f,
        attackCooldownSeconds: 0.8f);

    private GalaxabrainScoutState _previousState = GalaxabrainScoutState.Idle;

    [Export] public int Health { get; set; } = GalaxabrainScoutBrain.DefaultHealth;

    [Export] public float DetectionRange { get; set; } = 12f;

    [Export] public float AttackRange { get; set; } = 2f;

    [Export] public int Damage { get; set; } = 10;

    [Export] public float AttackCooldownSeconds { get; set; } = 0.8f;

    [Export] public float ChaseSpeed { get; set; } = 3f;

    [Export] public NodePath? PlayerPath { get; set; }

    [Export] public NodePath DeathAudioPath { get; set; } = "DeathAudio";

    [Export] public NodePath? MissionComponentPath { get; set; }

    [Export] public NodePath? CombatTutorialPath { get; set; }

    [Export] public NodePath AliveVisualPath { get; set; } = "V1BetaScoutVisualRoot";

    [Export] public NodePath DisabledVisualPath { get; set; } = "V1BetaScoutDisabledVisualRoot";

    public GalaxabrainScoutBrain Brain => _brain;

    public GalaxabrainScoutState State => _brain.State;

    public override void _Ready()
    {
        _brain = new GalaxabrainScoutBrain(Health, DetectionRange, AttackRange, AttackCooldownSeconds);
        SetMissionComponentVisible(false);
        _previousState = GalaxabrainScoutState.Idle;
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

        // Trigger audio on state transitions
        if (_brain.State != _previousState)
        {
            OnScoutStateChanged(_previousState, _brain.State);
            _previousState = _brain.State;
        }

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
            // Play attack audio when scout actually strikes
            AudioCue.Play3D(this, "AudioLayer_Enemy/Scout_Attack", GlobalPosition);
            TryDamagePlayer(player);
        }
    }

    public void ApplyDamage(int damage)
    {
        bool wasAlive = !_brain.IsDead;
        _brain.ApplyDamage(damage);

        if (wasAlive && !_brain.IsDead)
        {
            // Play hurt audio on damage (but not on death, which has its own fanfare)
            AudioCue.Play3D(this, "AudioLayer_Enemy/Scout_Hurt", GlobalPosition);
        }

        if (wasAlive && _brain.IsDead)
        {
            Die();
        }
    }

    private void OnScoutStateChanged(GalaxabrainScoutState previousState, GalaxabrainScoutState newState)
    {
        // Trigger alert audio and combat tutorial when scout first detects player (Idle → Chase)
        if (previousState == GalaxabrainScoutState.Idle && newState == GalaxabrainScoutState.Chase)
        {
            AudioCue.Play3D(this, "AudioLayer_Enemy/Scout_Alert", GlobalPosition);

            // Show combat tutorial hint on first detection (Phase 7.4 Priority 1)
            if (CombatTutorialPath != null && !CombatTutorialPath.IsEmpty)
            {
                if (GetNodeOrNull<Player.CombatTutorialHint>(CombatTutorialPath) is { } tutorial)
                {
                    tutorial.ShowCombatTutorial();
                }
            }
        }
        // Trigger attack audio when scout enters close combat range (Chase → Attack)
        // Note: Actual attack strike audio is played in _PhysicsProcess when TryConsumeAttack succeeds
    }

    private void TryDamagePlayer(Node3D player)
    {
        if (player is FirstPersonController controller)
        {
            controller.Health.ApplyDamage(Damage);
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
        DisableBodyCollision();
        SetMissionComponentVisible(true);
        AudioCue.Play(this, DeathAudioPath);
        ShowDisabledVisual();

        // Gameplay death is the only event allowed to complete the defeat objective;
        // the component pickup afterwards completes only component recovery.
        if (GetConfiguredPlayer() is FirstPersonController controller)
        {
            controller.Mission.TryCompleteGalaxabrainDefeat(true);
            controller.ShowGalaxabrainScoutDefeatFeedback();
        }
    }

    /// <summary>
    /// Reconstructs the defeated state from a save without mutating mission progression.
    /// </summary>
    public void RestoreDefeated(bool isComponentAvailable)
    {
        _brain.MarkDefeated();
        Velocity = Vector3.Zero;
        SetPhysicsProcess(false);
        DisableBodyCollision();
        SetMissionComponentVisible(isComponentAvailable);
        ShowDisabledVisual();

        if (!isComponentAvailable && GetMissionComponent() is GalaxabrainComponentPickup pickup)
        {
            pickup.RestoreCollected();
        }
    }

    /// <summary>
    /// Swaps the alive rig for the collapsed/dimmed-core disabled rig instead of
    /// hiding the whole body, so the corpse reads as defeated rather than vanishing
    /// (docs/art/crash-site-object-asset-inventory.md, "Disabled/dead Scout read").
    /// </summary>
    private void ShowDisabledVisual()
    {
        if (GetNodeOrNull<Node3D>(AliveVisualPath) is { } alive)
        {
            alive.Visible = false;
        }

        if (GetNodeOrNull<Node3D>(DisabledVisualPath) is { } disabled)
        {
            disabled.Visible = true;
        }
    }

    private void DisableBodyCollision()
    {
        // The dead body must stop blocking rays: its collider shares the component
        // pickup's capsule, so an enabled corpse collider can eat the interaction
        // raycast and soft-lock component recovery.
        GetNodeOrNull<CollisionShape3D>("CollisionShape3D")?
            .SetDeferred(CollisionShape3D.PropertyName.Disabled, true);
    }

    private void SetMissionComponentVisible(bool isVisible)
    {
        Node3D? missionComponent = GetMissionComponent();
        if (missionComponent is not null)
        {
            missionComponent.Visible = isVisible;
            if (missionComponent is Area3D area)
            {
                area.Monitoring = isVisible;
            }
        }
    }

    private Node3D? GetMissionComponent()
    {
        return MissionComponentPath is null || MissionComponentPath.IsEmpty
            ? null
            : GetNodeOrNull<Node3D>(MissionComponentPath);
    }
}
