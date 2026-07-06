using Godot;

namespace TitanCraft.World;

/// <summary>
/// Interactive Object Highlight - Phase 7.4 Priority 3
/// Improves visibility of interactive objects (workbench, beacon) at distance.
/// Adds subtle glow/highlight effect to make them stand out in the environment.
/// </summary>
public partial class InteractiveObjectHighlight : Node3D
{
    [Export] public float BaseEmissionStrength { get; set; } = 1.5f;
    [Export] public float HighlightEmissionStrength { get; set; } = 3.0f;
    [Export] public float HighlightDistance { get; set; } = 25.0f;
    [Export] public float PulseSpeed { get; set; } = 1.5f;
    [Export] public bool UsePulseAnimation { get; set; } = true;
    [Export] public NodePath? TargetMeshPath { get; set; }
    [Export] public NodePath? PlayerPath { get; set; }

    private MeshInstance3D? _targetMesh;
    private Node3D? _player;
    private StandardMaterial3D? _highlightMaterial;
    private float _pulseTime;
    private bool _isHighlighted;

    public override void _Ready()
    {
        // Get target mesh (the interactive object itself)
        if (TargetMeshPath != null && !TargetMeshPath.IsEmpty)
        {
            _targetMesh = GetNodeOrNull<MeshInstance3D>(TargetMeshPath);
        }
        else
        {
            _targetMesh = GetNode<MeshInstance3D>(".");
        }

        // Get player reference for distance checking
        if (PlayerPath != null && !PlayerPath.IsEmpty)
        {
            _player = GetNodeOrNull<Node3D>(PlayerPath);
        }

        // Create highlight material
        CreateHighlightMaterial();

        _pulseTime = 0;
        _isHighlighted = false;
    }

    public override void _Process(double delta)
    {
        if (_targetMesh == null || _player == null)
        {
            return;
        }

        float distanceToPlayer = _player.GlobalPosition.DistanceTo(GlobalPosition);
        bool shouldHighlight = distanceToPlayer <= HighlightDistance;

        if (UsePulseAnimation && shouldHighlight)
        {
            _pulseTime += (float)delta * PulseSpeed;
            if (_pulseTime > Mathf.Tau)
            {
                _pulseTime -= Mathf.Tau;
            }

            // Pulsing emission strength
            float pulse = (Mathf.Sin(_pulseTime) + 1.0f) / 2.0f;
            float emissionStrength = Mathf.Lerp(BaseEmissionStrength, HighlightEmissionStrength, pulse);

            if (_highlightMaterial != null)
            {
                _highlightMaterial.Emission = new Color(1, 0.65f, 0.2f, 1.0f) * emissionStrength;
            }
        }
        else if (!UsePulseAnimation && shouldHighlight)
        {
            // Static emission
            if (_highlightMaterial != null)
            {
                _highlightMaterial.Emission = new Color(1, 0.65f, 0.2f, 1.0f) * HighlightEmissionStrength;
            }
        }
    }

    private void CreateHighlightMaterial()
    {
        if (_targetMesh == null)
        {
            return;
        }

        // Create highlight material with orange emission
        _highlightMaterial = new StandardMaterial3D();
        _highlightMaterial.AlbedoColor = new Color(1.0f, 0.8f, 0.4f, 1.0f);

        // Set initial emission (orange highlight)
        _highlightMaterial.Emission = new Color(1, 0.65f, 0.2f, 1.0f) * BaseEmissionStrength;
        _highlightMaterial.EmissionOperator = BaseMaterial3D.EmissionOperatorEnum.Add;

        // Apply to target mesh
        if (_targetMesh.GetSurfaceOverrideMaterial(0) is StandardMaterial3D existingMat)
        {
            // Enhance existing material
            existingMat.Emission = _highlightMaterial.Emission;
            existingMat.EmissionOperator = BaseMaterial3D.EmissionOperatorEnum.Add;
            _highlightMaterial = existingMat;
        }
        else
        {
            _targetMesh.SetSurfaceOverrideMaterial(0, _highlightMaterial);
        }
    }

    /// <summary>
    /// Force highlight visibility (for testing/specific scenarios).
    /// </summary>
    public void SetHighlight(bool enabled)
    {
        _isHighlighted = enabled;
        if (_highlightMaterial == null)
        {
            return;
        }

        if (enabled)
        {
            _highlightMaterial.Emission = new Color(1, 0.65f, 0.2f, 1.0f) * HighlightEmissionStrength;
        }
        else
        {
            _highlightMaterial.Emission = new Color(1, 0.65f, 0.2f, 1.0f) * BaseEmissionStrength;
        }
    }
}
