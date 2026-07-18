using System;
using Godot;
using TitanCraft.Missions;
using TitanCraft.Resources;

namespace TitanCraft.World;

public partial class SavePoint : Area3D, ICrashSiteInteractable
{
    public event Action? SaveRequested;

    [Export] public string CheckpointId { get; set; } = "crash_site_save_point";

    public bool HasSavedCheckpoint { get; private set; }

    private const float BreatheRadiansPerSecond = 1.8f;
    private const float BreatheAmplitude = 0.02f;
    private Node3D? _visualRoot;
    private float _breatheSeconds;

    public override void _Ready()
    {
        _visualRoot = GetNodeOrNull<Node3D>("VisualRoot");
    }

    public override void _Process(double delta)
    {
        // Gentle scale breathing marks the pillar as an interactable checkpoint.
        if (_visualRoot is null)
        {
            return;
        }

        _breatheSeconds += (float)delta;
        var scale = 1f + BreatheAmplitude * Mathf.Sin(_breatheSeconds * BreatheRadiansPerSecond);
        _visualRoot.Scale = new Vector3(scale, scale, scale);
    }

    public bool Interact(MvpInventory inventory, CrashSiteMissionState mission)
    {
        ArgumentNullException.ThrowIfNull(inventory);
        ArgumentNullException.ThrowIfNull(mission);

        HasSavedCheckpoint = true;
        SaveRequested?.Invoke();
        return true;
    }
}
