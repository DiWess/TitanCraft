using System.Collections.Generic;
using System.Linq;
using Godot;
using TitanCraft.Core;
using TitanCraft.Enemies;
using TitanCraft.Missions;
using TitanCraft.Player;

namespace TitanCraft.World;

/// <summary>
/// The climax: once the power core is installed the beacon needs time to
/// charge, and its signal draws the swarm. The player holds the harbour plaza
/// while it charges; drones come in over the sea wall and Scouts up from the
/// harbour. When the charge completes the beam fires, burns out whatever is
/// still fighting, and the mission is won. Timing lives in <see cref="BeaconHoldoutLogic"/>.
/// </summary>
public partial class BeaconHoldout : Node3D
{
    [Export] public NodePath PlayerPath { get; set; } = "../Player";
    [Export] public NodePath BeaconPath { get; set; } = "../Placeholder_Beacon";
    [Export] public PackedScene? DroneScene { get; set; }
    [Export] public PackedScene? ScoutScene { get; set; }

    /// <summary>The player must stay this close to the beacon for it to charge.</summary>
    [Export] public float HoldRadius { get; set; } = 13f;

    [Export] public float ChargeSeconds { get; set; } = BeaconHoldoutLogic.DefaultChargeSeconds;

    /// <summary>Suit repair per second from the beacon's field while the plaza is held.</summary>
    [Export] public float FieldRepairPerSecond { get; set; } = 1.5f;

    [Export] public int HoldoutScoutHealth { get; set; } = 75;
    [Export] public int HoldoutScoutDamage { get; set; } = 7;
    [Export] public float HoldoutScoutAttackCooldown { get; set; } = 1.3f;
    [Export] public int HoldoutDroneDamage { get; set; } = 6;

    private float _repairCarry;

    private BeaconHoldoutLogic? _logic;
    private readonly List<Node3D> _spawned = new();
    private int _droneSpawnIndex;
    private int _scoutSpawnIndex;
    private bool _wasHolding = true;
    private FirstPersonController? _player;

    public bool IsRunning => _logic is not null && !_logic.IsComplete;

    public BeaconHoldoutLogic? Logic => _logic;

    public IReadOnlyList<Node3D> Spawned => _spawned;

    /// <summary>The arrivals so far, as a Godot array for tools outside C#.</summary>
    public Godot.Collections.Array<Node3D> SpawnedNodes => new(_spawned.Where(IsInstanceValid));

    public override void _Ready()
    {
        _player = GetNodeOrNull<FirstPersonController>(PlayerPath);
        if (_player is not null)
            _player.Mission.Changed += OnMissionChanged;
    }

    public override void _ExitTree()
    {
        if (_player is not null)
            _player.Mission.Changed -= OnMissionChanged;
    }

    private void OnMissionChanged(CrashSiteMissionState mission)
    {
        if (mission.CurrentStep == CrashSiteMissionStep.DefendBeacon && _logic is null)
            Begin();
    }

    public void Begin()
    {
        _logic = new BeaconHoldoutLogic(ChargeSeconds);
        GetNodeOrNull<Beacon>(BeaconPath)?.SetCharge(0f);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_logic is null || _logic.IsComplete || _player is null)
            return;

        var beacon = GetNodeOrNull<Node3D>(BeaconPath);
        var holding = beacon is null
            || Flat(_player.GlobalPosition - beacon.GlobalPosition).Length() <= HoldRadius;
        if (holding != _wasHolding)
        {
            _wasHolding = holding;
            _player.ShowHoldoutFeedback(holding);
        }

        _spawned.RemoveAll(node => !IsInstanceValid(node));
        var active = _spawned.Count(IsAlive);
        var arrivals = _logic.Tick((float)delta, holding, active);
        if (arrivals.Count > 0)
            AudioCue.Play(this, "AudioLayer_State/Holdout_Wave");
        foreach (var kind in arrivals)
            Spawn(kind);

        if (holding && !_player.Health.IsDead)
        {
            _repairCarry += FieldRepairPerSecond * (float)delta;
            var whole = (int)_repairCarry;
            if (whole > 0)
            {
                _repairCarry -= whole;
                _player.Health.Heal(whole);
            }
        }

        _player.Mission.SetBeaconCharge(_logic.ChargeFraction);
        (beacon as Beacon)?.SetCharge(_logic.ChargeFraction);
        if (_logic.IsComplete)
            Complete();
    }

    private void Complete()
    {
        // The beam fires: everything still fighting burns out in it.
        foreach (var node in _spawned.Where(IsAlive))
        {
            switch (node)
            {
                case SwarmDrone drone:
                    drone.BurnOut();
                    break;
                case GalaxabrainScout scout:
                    scout.ApplyDamage(scout.Brain.CurrentHealth);
                    break;
            }
        }

        _player?.Mission.TryCompleteBeaconDefence();
    }

    private void Spawn(HoldoutEnemyKind kind)
    {
        var markers = GetNodeOrNull<Node3D>(kind == HoldoutEnemyKind.Drone ? "DroneSpawns" : "ScoutSpawns")?
            .GetChildren().OfType<Node3D>().ToList();
        var scene = kind == HoldoutEnemyKind.Drone ? DroneScene : ScoutScene;
        if (scene is null || markers is null || markers.Count == 0)
            return;

        var index = kind == HoldoutEnemyKind.Drone ? _droneSpawnIndex++ : _scoutSpawnIndex++;
        var marker = markers[index % markers.Count];
        var enemy = scene.Instantiate<Node3D>();
        switch (enemy)
        {
            case SwarmDrone drone:
                drone.StartActive = true;
                drone.OrbitPhase = index * 2.1f;
                drone.FirstDiveDelaySeconds = 2.2f + index % 3 * 0.9f;
                drone.ContactDamage = HoldoutDroneDamage;
                break;
            case GalaxabrainScout scout:
                // The Scouts that answer the beacon are younger and lighter
                // than the one that held the plaza: the holdout is about
                // juggling several threats, not trading blows with each.
                scout.IsMissionScout = false;
                scout.DetectionRange = 40f;
                scout.Health = HoldoutScoutHealth;
                scout.Damage = HoldoutScoutDamage;
                scout.AttackCooldownSeconds = HoldoutScoutAttackCooldown;
                break;
        }

        // Added before the path is set so NodePaths resolve from inside the tree.
        GetParent().AddChild(enemy);
        enemy.GlobalPosition = marker.GlobalPosition;
        var playerPath = enemy.GetPathTo(_player!);
        if (enemy is SwarmDrone spawnedDrone)
            spawnedDrone.PlayerPath = playerPath;
        else if (enemy is GalaxabrainScout spawnedScout)
            spawnedScout.PlayerPath = playerPath;
        _spawned.Add(enemy);
        AudioCue.Play3D(this, kind == HoldoutEnemyKind.Drone ? "AudioLayer_Enemy/Drone_Alert" : "AudioLayer_Enemy/Scout_Alert", marker.GlobalPosition);
    }

    private static bool IsAlive(Node3D node) => node switch
    {
        SwarmDrone drone => !drone.Brain.IsDead,
        GalaxabrainScout scout => !scout.Brain.IsDead,
        _ => false,
    };

    private static Vector3 Flat(Vector3 v) => new(v.X, 0f, v.Z);
}
