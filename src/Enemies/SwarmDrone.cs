using Godot;
using TitanCraft.Combat;
using TitanCraft.Core;
using TitanCraft.Player;
using TitanCraft.World;

namespace TitanCraft.Enemies;

/// <summary>
/// The Galaxabrain swarm drone's body: flight, contact damage, the clamp's
/// yank and the death fall. All decisions live in <see cref="SwarmDroneBrain"/>.
/// </summary>
public partial class SwarmDrone : CharacterBody3D, IArmTarget, IPullTarget
{
    [Export] public NodePath? PlayerPath { get; set; }
    [Export] public NodePath AliveVisualPath { get; set; } = "Visual";
    [Export] public NodePath CoreGlowPath { get; set; } = "Visual/CoreGlow";
    [Export] public NodePath DeathBurstPath { get; set; } = "DeathBurst";

    [Export] public float HoverHeight { get; set; } = 3.4f;
    [Export] public float OrbitRadius { get; set; } = 5.0f;
    [Export] public float OrbitRadiansPerSecond { get; set; } = 0.55f;
    [Export] public float CruiseSpeed { get; set; } = 4.6f;
    [Export] public float DiveSpeed { get; set; } = 10.5f;
    [Export] public int ContactDamage { get; set; } = 8;
    [Export] public float FirstDiveDelaySeconds { get; set; } = SwarmDroneBrain.DefaultDiveIntervalSeconds;

    /// <summary>Starts awake (holdout arrivals) instead of perched (the yard's nest).</summary>
    [Export] public bool StartActive { get; set; }

    [Export] public float OrbitPhase { get; set; }

    private SwarmDroneBrain _brain = new();
    private SwarmDroneState _previousState = SwarmDroneState.Dormant;
    private Vector3 _diveDirection;
    private float _wobbleSeconds;
    private float _deadSeconds;
    private Tween? _pullTween;

    public SwarmDroneBrain Brain => _brain;

    /// <summary>The brain's state as a plain int, for tools outside C# (the walked playthrough).</summary>
    public int StateCode => (int)_brain.State;

    public bool IsDefeated => _brain.IsDead;

    public bool IsStrikeable => !_brain.IsDead && _brain.State != SwarmDroneState.Dormant;

    public string DeflectFeedback => string.Empty;

    public bool CanBePulled => _brain.CanBePulled;

    public Vector3 PullAnchor => GlobalPosition;

    public override void _Ready()
    {
        _brain = new SwarmDroneBrain(firstDiveDelaySeconds: FirstDiveDelaySeconds);
        AddToGroup(GalaxabrainScout.EnemyGroup);
        AddToGroup(FirstPersonController.PullTargetGroup);
        if (StartActive)
            Activate();
    }

    public void Activate()
    {
        _brain.Activate();
        if (_brain.State == SwarmDroneState.Orbit)
            AudioCue.Play3D(this, "AudioLayer_Enemy/Drone_Alert", GlobalPosition);
    }

    public override void _PhysicsProcess(double delta)
    {
        var dt = (float)delta;
        _wobbleSeconds += dt;
        if (_brain.IsDead)
        {
            FallDead(dt);
            return;
        }

        if (GetConfiguredPlayer() is not { } player || _brain.State == SwarmDroneState.Dormant)
        {
            Bob(dt);
            return;
        }

        var chest = player.GlobalPosition + Vector3.Up * 1.0f;
        _brain.Tick(dt, GlobalPosition.DistanceTo(chest));
        if (_brain.State != _previousState)
            OnStateChanged(_previousState, _brain.State, chest);
        _previousState = _brain.State;

        switch (_brain.State)
        {
            case SwarmDroneState.Orbit:
            case SwarmDroneState.Recover:
                OrbitAround(player.GlobalPosition, dt);
                break;
            case SwarmDroneState.Telegraph:
                Velocity = Velocity.Lerp(Vector3.Zero, Mathf.Min(1f, 8f * dt));
                MoveAndSlide();
                FaceTowards(chest, dt);
                break;
            case SwarmDroneState.Dive:
                Velocity = _diveDirection * DiveSpeed;
                MoveAndSlide();
                if ((GlobalPosition.DistanceTo(chest) <= 1.15f || CollidedWith(player)) && _brain.ReportDiveContact())
                {
                    AudioCue.Play3D(this, "AudioLayer_Enemy/Drone_Hit", GlobalPosition);
                    (player as FirstPersonController)?.TakeHit(ContactDamage, GlobalPosition);
                }
                else if (GetSlideCollisionCount() > 0)
                {
                    // Struck a wall or the ground instead: the dive is spent.
                    _brain.ReportDiveContact();
                }
                break;
            case SwarmDroneState.Stunned:
                Velocity = Vector3.Zero;
                break;
        }

        UpdateGlow();
    }

    public StrikeOutcome ReceiveStrike(ArmStrike strike)
    {
        if (!IsStrikeable)
            return StrikeOutcome.Ignored;

        var outcome = _brain.ApplyDamage(strike.Damage);
        if (outcome == StrikeOutcome.Hit)
        {
            AudioCue.Play3D(this, "AudioLayer_Enemy/Drone_Hit", GlobalPosition);
            // Knocked back along the blow: a hit moves it, so the strike reads.
            var away = (GlobalPosition - strike.Origin) with { Y = 0.3f };
            Velocity = away.Normalized() * 4f;
            MoveAndSlide();
        }
        else if (outcome == StrikeOutcome.Killed)
        {
            Die();
        }

        return outcome;
    }

    public bool Pull(Vector3 pullerPosition)
    {
        if (!_brain.Pull())
            return false;

        AudioCue.Play3D(this, "AudioLayer_Enemy/Drone_Hit", GlobalPosition);
        // Yanked down to the ground in front of the player, where Mk I reaches.
        var toward = (pullerPosition - GlobalPosition) with { Y = 0f };
        var distance = toward.Length();
        var landing = distance > 2.4f
            ? GlobalPosition + toward / distance * (distance - 2.2f)
            : GlobalPosition;
        landing.Y = 0.75f;
        _pullTween?.Kill();
        _pullTween = CreateTween();
        _pullTween.TweenProperty(this, "global_position", landing, 0.28f)
            .SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.In);
        return true;
    }

    /// <summary>Removed from play without a death sequence (a load past its encounter).</summary>
    public void RestoreDead()
    {
        _brain.MarkDead();
        Visible = false;
        ProcessMode = ProcessModeEnum.Disabled;
        GetNodeOrNull<CollisionShape3D>("CollisionShape3D")?.SetDeferred(CollisionShape3D.PropertyName.Disabled, true);
    }

    /// <summary>The beacon's beam burning out the swarm at the end of the holdout.</summary>
    public void BurnOut()
    {
        if (_brain.IsDead)
            return;

        _brain.MarkDead();
        Die();
    }

    private void OnStateChanged(SwarmDroneState previous, SwarmDroneState next, Vector3 chest)
    {
        if (next == SwarmDroneState.Telegraph)
            AudioCue.Play3D(this, "AudioLayer_Enemy/Drone_Telegraph", GlobalPosition);
        else if (next == SwarmDroneState.Dive)
            _diveDirection = (chest - GlobalPosition).Normalized();
    }

    private void OrbitAround(Vector3 playerPosition, float dt)
    {
        var angle = OrbitPhase + _wobbleSeconds * OrbitRadiansPerSecond;
        var goal = playerPosition + new Vector3(Mathf.Cos(angle) * OrbitRadius, 0f, Mathf.Sin(angle) * OrbitRadius);
        goal.Y = HoverHeight + 0.18f * Mathf.Sin(_wobbleSeconds * 2.3f + OrbitPhase);
        var desired = goal - GlobalPosition;
        var speed = _brain.State == SwarmDroneState.Recover ? CruiseSpeed * 0.8f : CruiseSpeed;
        var target = desired.Length() > 0.05f ? desired.Normalized() * Mathf.Min(speed, desired.Length() * 3f) : Vector3.Zero;
        Velocity = Velocity.Lerp(target, Mathf.Min(1f, 4f * dt));
        MoveAndSlide();
        FaceTowards(playerPosition + Vector3.Up, dt);
    }

    private void Bob(float dt)
    {
        if (GetNodeOrNull<Node3D>(AliveVisualPath) is { } visual)
            visual.Position = visual.Position with { Y = 0.06f * Mathf.Sin(_wobbleSeconds * 1.7f + OrbitPhase) };
    }

    private void FaceTowards(Vector3 point, float dt)
    {
        if (GetNodeOrNull<Node3D>(AliveVisualPath) is not { } visual)
            return;

        var to = point - GlobalPosition;
        if (new Vector2(to.X, to.Z).LengthSquared() < 0.0001f)
            return;

        var yaw = Mathf.Atan2(-to.X, -to.Z);
        var rotation = visual.Rotation;
        rotation.Y = Mathf.LerpAngle(rotation.Y, yaw - Rotation.Y, Mathf.Min(1f, 6f * dt));
        rotation.Z = _brain.State == SwarmDroneState.Stunned ? 0.5f * Mathf.Sin(_wobbleSeconds * 9f) : 0f;
        visual.Rotation = rotation;
    }

    private void UpdateGlow()
    {
        if (GetNodeOrNull<Node3D>(CoreGlowPath) is not { } glow)
            return;

        var scale = _brain.State switch
        {
            SwarmDroneState.Telegraph => 1.3f + 0.35f * Mathf.Sin(_wobbleSeconds * 30f),
            SwarmDroneState.Dive => 1.45f,
            SwarmDroneState.Stunned => 0.6f,
            _ => 1f,
        };
        glow.Scale = Vector3.One * scale;
    }

    private void Die()
    {
        AudioCue.Play3D(this, "AudioLayer_Enemy/Drone_Death", GlobalPosition);
        GetNodeOrNull<CollisionShape3D>("CollisionShape3D")?.SetDeferred(CollisionShape3D.PropertyName.Disabled, true);
        if (GetNodeOrNull<GpuParticles3D>(DeathBurstPath) is { } burst)
        {
            burst.Restart();
            burst.Emitting = true;
        }

        if (GetNodeOrNull<Node3D>(CoreGlowPath) is { } glow)
            glow.Visible = false;
        _deadSeconds = 0f;
    }

    private void FallDead(float dt)
    {
        _deadSeconds += dt;
        if (GlobalPosition.Y > 0.25f)
        {
            GlobalPosition += Vector3.Down * Mathf.Min(GlobalPosition.Y - 0.25f, 9f * dt * Mathf.Max(0.4f, _deadSeconds * 3f));
            if (GetNodeOrNull<Node3D>(AliveVisualPath) is { } visual)
                visual.Rotation = visual.Rotation with { Z = Mathf.Min(1.2f, visual.Rotation.Z + 4f * dt) };
        }
        else if (_deadSeconds > 6f)
        {
            // The wreck lingers long enough to read as a kill, then clears so
            // repeated holdout waves do not pile up bodies.
            SetPhysicsProcess(false);
            Visible = false;
        }
    }

    private bool CollidedWith(Node3D body)
    {
        for (var i = 0; i < GetSlideCollisionCount(); i++)
        {
            if (GetSlideCollision(i).GetCollider() == body)
                return true;
        }

        return false;
    }

    private Node3D? GetConfiguredPlayer() =>
        PlayerPath is null || PlayerPath.IsEmpty ? null : GetNodeOrNull<Node3D>(PlayerPath);
}
