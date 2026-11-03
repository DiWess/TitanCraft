using System.Collections.Generic;
using System.Linq;
using Godot;
using TitanCraft.Enemies;
using TitanCraft.Missions;
using TitanCraft.Player;
using TitanCraft.SaveSystem;

namespace TitanCraft.World;

/// <summary>
/// The Net-makers' Yard encounter: the swarm drones perched in the yard wake
/// when its gate is pulled open, and the yard is cleared when the last one
/// falls. Every drone child of this node belongs to the nest.
/// </summary>
public partial class DroneNestEncounter : Node3D, IMissionRestorable
{
    [Export] public NodePath PlayerPath { get; set; } = "../../Player";

    private readonly List<SwarmDrone> _drones = new();
    private bool _awake;
    private bool _cleared;

    public bool IsCleared => _cleared;

    public IReadOnlyList<SwarmDrone> Drones => _drones;

    public override void _Ready()
    {
        AddToGroup(IMissionRestorable.Group);
        _drones.AddRange(GetChildren().OfType<SwarmDrone>());
        if (GetNodeOrNull<FirstPersonController>(PlayerPath) is { } player)
            player.Mission.Changed += OnMissionChanged;
    }

    public override void _ExitTree()
    {
        if (GetNodeOrNull<FirstPersonController>(PlayerPath) is { } player)
            player.Mission.Changed -= OnMissionChanged;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_awake || _cleared || _drones.Any(drone => !drone.Brain.IsDead))
            return;

        _cleared = true;
        GetNodeOrNull<FirstPersonController>(PlayerPath)?.Mission.TryCompleteDroneNest();
    }

    public void RestoreFromMission(CrashSiteMissionStep step)
    {
        if (CrashSiteStateReconciler.RequiresDroneNestCleared(step))
        {
            _cleared = true;
            foreach (var drone in _drones)
                drone.RestoreDead();
        }
        else if (step == CrashSiteMissionStep.ClearDroneNest)
        {
            Wake();
        }
    }

    private void OnMissionChanged(CrashSiteMissionState mission)
    {
        if (mission.CurrentStep == CrashSiteMissionStep.ClearDroneNest)
            Wake();
    }

    private void Wake()
    {
        if (_awake)
            return;

        _awake = true;
        foreach (var drone in _drones)
            drone.Activate();
    }
}
