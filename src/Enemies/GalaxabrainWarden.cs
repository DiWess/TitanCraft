using Godot;
using TitanCraft.Combat;
using TitanCraft.Core;
using TitanCraft.Missions;
using TitanCraft.Player;
using TitanCraft.SaveSystem;
using TitanCraft.World;

namespace TitanCraft.Enemies;

/// <summary>
/// The Galaxabrain Warden's body: a heavy, guarded biomech that keeps the
/// beacon's power core in the tower court. Decisions live in <see cref="WardenBrain"/>.
/// It wakes when its gate is broken and walks straight at the player, like the
/// Scout, so its court is kept clear of collision (the level generator asserts it).
/// </summary>
public partial class GalaxabrainWarden : CharacterBody3D, IArmTarget, IMissionRestorable
{
    [Export] public NodePath? PlayerPath { get; set; }
    [Export] public NodePath PowerCorePath { get; set; } = "../PowerCorePickup";
    [Export] public NodePath AliveVisualPath { get; set; } = "AliveVisual";
    [Export] public NodePath DisabledVisualPath { get; set; } = "DisabledVisual";
    [Export] public NodePath GuardVisualPath { get; set; } = "AliveVisual/Guard";
    [Export] public NodePath CoreGlowPath { get; set; } = "AliveVisual/CoreGlow";
    [Export] public int SlamDamage { get; set; } = 22;
    [Export] public float AdvanceSpeed { get; set; } = 2.2f;

    private WardenBrain _brain = new();
    private WardenState _previousState = WardenState.Dormant;
    private float _animSeconds;
    private Vector3 _guardBasePosition;
    private Vector3 _guardBaseRotation;

    public WardenBrain Brain => _brain;

    /// <summary>The brain's state as a plain int, for tools outside C# (the walked playthrough).</summary>
    public int StateCode => (int)_brain.State;

    public bool IsDefeated => _brain.IsDead;

    public bool IsStrikeable => !_brain.IsDead && _brain.State != WardenState.Dormant;

    public string DeflectFeedback => "The Warden's guard turns the blow — hold left click to charge a strike.";

    public override void _Ready()
    {
        _brain = new WardenBrain();
        AddToGroup(GalaxabrainScout.EnemyGroup);
        AddToGroup(IMissionRestorable.Group);
        if (GetNodeOrNull<Node3D>(GuardVisualPath) is { } guard)
        {
            _guardBasePosition = guard.Position;
            _guardBaseRotation = guard.Rotation;
        }

        if (GetConfiguredPlayer() is FirstPersonController player)
            player.Mission.Changed += OnMissionChanged;
    }

    public override void _ExitTree()
    {
        if (GetConfiguredPlayer() is FirstPersonController player)
            player.Mission.Changed -= OnMissionChanged;
    }

    /// <summary>The gate breaking is the Warden's cue; a load past that point wakes it too.</summary>
    private void OnMissionChanged(CrashSiteMissionState mission)
    {
        if (mission.CurrentStep == CrashSiteMissionStep.DefeatWarden && _brain.State == WardenState.Dormant)
            Awaken();
    }

    public void Awaken()
    {
        _brain.Awaken();
        AudioCue.Play3D(this, "AudioLayer_Enemy/Warden_Alert", GlobalPosition);
    }

    public override void _PhysicsProcess(double delta)
    {
        var dt = (float)delta;
        _animSeconds += dt;
        if (_brain.IsDead || _brain.State == WardenState.Dormant || GetConfiguredPlayer() is not { } player)
        {
            Velocity = Vector3.Zero;
            AnimateIdle(dt);
            return;
        }

        var distance = Flat(player.GlobalPosition - GlobalPosition).Length();
        var slamLanded = _brain.Tick(dt, distance);
        if (_brain.State != _previousState)
        {
            if (_brain.State == WardenState.Windup)
                AudioCue.Play3D(this, "AudioLayer_Enemy/Warden_Windup", GlobalPosition);
            _previousState = _brain.State;
        }

        if (slamLanded)
            Slam(player, distance);

        if (_brain.State == WardenState.Advance && distance > 1.6f)
        {
            var direction = Flat(player.GlobalPosition - GlobalPosition).Normalized();
            Velocity = new Vector3(direction.X * AdvanceSpeed, Velocity.Y, direction.Z * AdvanceSpeed);
        }
        else
        {
            Velocity = new Vector3(0f, Velocity.Y, 0f);
        }

        MoveAndSlide();
        Face(player.GlobalPosition, dt);
        AnimateCombat(dt);
    }

    public StrikeOutcome ReceiveStrike(ArmStrike strike)
    {
        if (!IsStrikeable)
            return StrikeOutcome.Ignored;

        var wasGuarding = _brain.IsGuarding;
        var outcome = _brain.ReceiveStrike(strike.Damage, strike.IsCharged);
        switch (outcome)
        {
            case StrikeOutcome.Deflected:
                AudioCue.Play3D(this, "AudioLayer_Player/Weapon_Deflect", GlobalPosition);
                break;
            case StrikeOutcome.Hit:
                AudioCue.Play3D(this, wasGuarding ? "AudioLayer_Enemy/Warden_GuardBreak" : "AudioLayer_Enemy/Warden_Hurt", GlobalPosition);
                break;
            case StrikeOutcome.Killed:
                Die();
                break;
        }

        return outcome;
    }

    public void RestoreFromMission(CrashSiteMissionStep step)
    {
        if (CrashSiteStateReconciler.RequiresWardenDefeated(step))
        {
            _brain.MarkDead();
            ShowDisabled();
            GetNodeOrNull<CollisionShape3D>("CollisionShape3D")?.SetDeferred(CollisionShape3D.PropertyName.Disabled, true);
        }
        else if (step == CrashSiteMissionStep.DefeatWarden && _brain.State == WardenState.Dormant)
        {
            _brain.Awaken();
        }
    }

    private void Slam(Node3D player, float distance)
    {
        AudioCue.Play3D(this, "AudioLayer_Enemy/Warden_Slam", GlobalPosition);
        foreach (var node in GetTree().GetNodesInGroup(CameraShaker.CameraShakerGroup))
        {
            if (node is CameraShaker shaker)
                shaker.AddTrauma(distance <= WardenBrain.DefaultSlamReach ? 0.5f : 0.2f);
        }

        if (distance <= WardenBrain.DefaultSlamReach)
            (player as FirstPersonController)?.TakeHit(SlamDamage, GlobalPosition);
    }

    private void Die()
    {
        Velocity = Vector3.Zero;
        AudioCue.Play3D(this, "AudioLayer_Enemy/Warden_Death", GlobalPosition);
        GetNodeOrNull<CollisionShape3D>("CollisionShape3D")?.SetDeferred(CollisionShape3D.PropertyName.Disabled, true);
        ShowDisabled();
        if (GetConfiguredPlayer() is FirstPersonController player)
        {
            player.Mission.TryCompleteWardenDefeat();
            player.ShowWardenDefeatFeedback();
        }
    }

    private void ShowDisabled()
    {
        if (GetNodeOrNull<Node3D>(AliveVisualPath) is { } alive)
            alive.Visible = false;
        if (GetNodeOrNull<Node3D>(DisabledVisualPath) is { } disabled)
            disabled.Visible = true;
    }

    private void Face(Vector3 point, float dt)
    {
        var to = Flat(point - GlobalPosition);
        if (to.LengthSquared() < 0.0001f || _brain.State == WardenState.Staggered)
            return;

        var yaw = Mathf.Atan2(-to.X, -to.Z);
        Rotation = Rotation with { Y = Mathf.LerpAngle(Rotation.Y, yaw, Mathf.Min(1f, 3.5f * dt)) };
    }

    private void AnimateIdle(float dt)
    {
        if (_brain.IsDead || GetNodeOrNull<Node3D>(AliveVisualPath) is not { } alive)
            return;

        alive.Position = alive.Position with { Y = 0.03f * Mathf.Sin(_animSeconds * 1.2f) };
    }

    /// <summary>
    /// Body language that carries the fight's rules: the guard plate held
    /// forward while guarding, raised on the wind-up, dropped and sagging while
    /// staggered, and the core flaring when it is open to blows.
    /// </summary>
    private void AnimateCombat(float dt)
    {
        var guard = GetNodeOrNull<Node3D>(GuardVisualPath);
        var alive = GetNodeOrNull<Node3D>(AliveVisualPath);
        var (guardOffset, guardTilt, bodyDip) = _brain.State switch
        {
            WardenState.Windup => (new Vector3(0f, 0.55f, 0.15f), -0.9f * Mathf.Clamp(1f - _brain.StateTimer / WardenBrain.DefaultWindupSeconds, 0f, 1f), 0f),
            WardenState.Recover => (new Vector3(0f, -0.15f, -0.25f), 0.35f, -0.08f),
            WardenState.Staggered => (new Vector3(0.35f, -0.55f, 0.1f), 0.9f, -0.22f + 0.04f * Mathf.Sin(_animSeconds * 7f)),
            _ => (Vector3.Zero, 0f, 0.04f * Mathf.Abs(Mathf.Sin(_animSeconds * 4.4f))),
        };

        if (guard is not null)
        {
            guard.Position = guard.Position.Lerp(_guardBasePosition + guardOffset, Mathf.Min(1f, 10f * dt));
            guard.Rotation = guard.Rotation.Lerp(_guardBaseRotation + new Vector3(guardTilt, 0f, 0f), Mathf.Min(1f, 10f * dt));
        }

        if (alive is not null)
            alive.Position = alive.Position.Lerp(new Vector3(0f, bodyDip, 0f), Mathf.Min(1f, 8f * dt));

        if (GetNodeOrNull<Node3D>(CoreGlowPath) is { } glow)
            glow.Scale = Vector3.One * (_brain.State == WardenState.Staggered ? 1.5f + 0.2f * Mathf.Sin(_animSeconds * 14f) : 1f);
    }

    private static Vector3 Flat(Vector3 v) => new(v.X, 0f, v.Z);

    private Node3D? GetConfiguredPlayer() =>
        PlayerPath is null || PlayerPath.IsEmpty ? null : GetNodeOrNull<Node3D>(PlayerPath);
}
