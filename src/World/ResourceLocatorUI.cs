using System;
using Godot;
using TitanCraft.Core;

namespace TitanCraft.World;

/// <summary>
/// Resource Locator UI - Phase 7.4 Priority 2
/// Provides subtle visual/audio cues for nearby resource locations.
/// Reduces discovery time for new players without explicit waypoints.
/// </summary>
public partial class ResourceLocatorUI : Control
{
    [Export] public float ActivationRangeMeters { get; set; } = 15.0f;
    [Export] public float DeactivationRangeMeters { get; set; } = 20.0f;
    [Export] public float UpdateIntervalSeconds { get; set; } = 0.5f;
    [Export] public NodePath? PlayerPath { get; set; }
    [Export] public NodePath? IndicatorLabelPath { get; set; }

    private Node3D? _player;
    private Label? _indicatorLabel;
    private float _updateTimer;
    private string _lastIndicatorText = string.Empty;

    private const string MetalResourceNearby = "🟠 METAL NEARBY";
    private const string BiomassResourceNearby = "🟢 BIOMASS NEARBY";
    private const string ElectronicsResourceNearby = "🔵 ELECTRONICS NEARBY";
    private const string ResourcesLocatedText = "Resources located — Proceed to workbench";

    public override void _Ready()
    {
        if (PlayerPath != null && !PlayerPath.IsEmpty)
        {
            _player = GetNodeOrNull<Node3D>(PlayerPath);
        }

        if (IndicatorLabelPath != null && !IndicatorLabelPath.IsEmpty)
        {
            _indicatorLabel = GetNodeOrNull<Label>(IndicatorLabelPath);
        }

        _updateTimer = UpdateIntervalSeconds;
    }

    public override void _Process(double delta)
    {
        if (_player == null || _indicatorLabel == null)
        {
            return;
        }

        _updateTimer -= (float)delta;
        if (_updateTimer > 0)
        {
            return;
        }

        _updateTimer = UpdateIntervalSeconds;

        // Find all resources in scene
        var allResources = _player.GetTree().GetNodesInGroup("pickable_resource");
        var nearbyResources = new System.Collections.Generic.Dictionary<string, float>();

        foreach (var resource in allResources)
        {
            if (resource is not Node3D resourceNode)
            {
                continue;
            }

            float distance = _player.GlobalPosition.DistanceTo(resourceNode.GlobalPosition);

            if (distance > DeactivationRangeMeters)
            {
                continue;
            }

            if (distance <= ActivationRangeMeters)
            {
                string resourceType = GetResourceType(resourceNode);
                if (!nearbyResources.ContainsKey(resourceType) || nearbyResources[resourceType] > distance)
                {
                    nearbyResources[resourceType] = distance;
                }
            }
        }

        // Update indicator display
        UpdateIndicatorDisplay(nearbyResources);
    }

    private string GetResourceType(Node3D resourceNode)
    {
        // Fallback: infer from node name (most reliable approach)
        string nodeName = resourceNode.Name.ToString().ToLowerInvariant();

        if (nodeName.Contains("metal"))
        {
            return "metal";
        }
        if (nodeName.Contains("biomass"))
        {
            return "biomass";
        }
        if (nodeName.Contains("electronics"))
        {
            return "electronics";
        }

        return "unknown";
    }

    private void UpdateIndicatorDisplay(System.Collections.Generic.Dictionary<string, float> nearbyResources)
    {
        if (nearbyResources.Count == 0)
        {
            if (_lastIndicatorText != string.Empty)
            {
                _indicatorLabel!.Text = string.Empty;
                _lastIndicatorText = string.Empty;
            }
            return;
        }

        // Show closest nearby resource
        string indicatorText = nearbyResources.Count switch
        {
            1 when nearbyResources.ContainsKey("metal") => MetalResourceNearby,
            1 when nearbyResources.ContainsKey("biomass") => BiomassResourceNearby,
            1 when nearbyResources.ContainsKey("electronics") => ElectronicsResourceNearby,
            _ => ResourcesLocatedText,
        };

        if (_lastIndicatorText != indicatorText)
        {
            _indicatorLabel!.Text = indicatorText;
            _lastIndicatorText = indicatorText;

            // Optional: Play subtle audio cue when nearby resource is detected
            // AudioCue.Play(this, "AudioLayer_UI/resource_nearby_tone");
        }
    }

    /// <summary>
    /// Show all resources for testing/debug purposes.
    /// </summary>
    public void ShowAllResourcesDebug()
    {
        if (_player == null)
        {
            return;
        }

        var allResources = _player.GetTree().GetNodesInGroup("pickable_resource");
        GD.Print($"[Resource Locator] Found {allResources.Count} resources in scene");

        foreach (var resource in allResources)
        {
            if (resource is Node3D resourceNode)
            {
                float distance = _player.GlobalPosition.DistanceTo(resourceNode.GlobalPosition);
                GD.Print($"  - {resourceNode.Name}: {distance:F1}m away");
            }
        }
    }
}
