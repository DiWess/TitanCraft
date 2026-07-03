using System;
using Godot;
using TitanCraft.Missions;
using TitanCraft.Resources;

namespace TitanCraft.World;

public enum ResourceType
{
    Metal,
    Biomass,
    Electronics,
}

public readonly record struct ResourceDropCollectedEvent(ResourceType Type, int Quantity);

public partial class ResourceDrop : Area3D, ICrashSiteInteractable, ILookHighlightTarget
{
    public const string ResourceDropGroup = "resource_drops";
    private const float DefaultCleanupDelaySeconds = 1.25f;

    [Signal]
    public delegate void CollectedEventHandler(ResourceType type, int quantity);

    [Export] public ResourceType Type { get; set; } = ResourceType.Metal;

    [Export(PropertyHint.Range, "1,99,1")]
    public int Quantity { get; set; } = 1;

    [Export] public NodePath ItemMeshPath { get; set; } = "VisualGroup/ItemMesh";
    [Export] public NodePath HighlightRingPath { get; set; } = "VisualGroup/HighlightRing";
    [Export] public NodePath AmbientGlowPath { get; set; } = "ParticleEffects/AmbientGlow";
    [Export] public NodePath PickupBurstPath { get; set; } = "ParticleEffects/PickupBurst";
    [Export] public NodePath InteractionCollisionPath { get; set; } = "InteractionCollision";
    [Export] public NodePath StaticPhysicsBodyPath { get; set; } = "StaticPhysicsBody";
    [Export] public NodePath SpatialPickupPlayerPath { get; set; } = "AudioPlayers/SpatialPickupPlayer";
    [Export] public Material? HighlightMaterial { get; set; }

    public event Action<ResourceDropCollectedEvent>? ResourceCollected;

    private MeshInstance3D? _itemMesh;
    private MeshInstance3D? _highlightRing;
    private GpuParticles3D? _ambientGlow;
    private GpuParticles3D? _pickupBurst;
    private CollisionShape3D? _interactionCollision;
    private CollisionObject3D? _staticPhysicsBody;
    private AudioStreamPlayer3D? _spatialPickupPlayer;
    private Material? _baseMaterial;
    private bool _isCollected;

    public override void _Ready()
    {
        _itemMesh = GetNodeOrNull<MeshInstance3D>(ItemMeshPath);
        _highlightRing = GetNodeOrNull<MeshInstance3D>(HighlightRingPath);
        _ambientGlow = GetNodeOrNull<GpuParticles3D>(AmbientGlowPath);
        _pickupBurst = GetNodeOrNull<GpuParticles3D>(PickupBurstPath);
        _interactionCollision = GetNodeOrNull<CollisionShape3D>(InteractionCollisionPath);
        _staticPhysicsBody = GetNodeOrNull<CollisionObject3D>(StaticPhysicsBodyPath);
        _spatialPickupPlayer = GetNodeOrNull<AudioStreamPlayer3D>(SpatialPickupPlayerPath);
        _baseMaterial = _itemMesh?.MaterialOverride;
        AddToGroup(ResourceDropGroup);

        CollisionLayer = 1u << 2;
        CollisionMask = 1u;
        SetHighlighted(false);
    }

    public bool Interact(MvpInventory inventory, CrashSiteMissionState mission)
    {
        ArgumentNullException.ThrowIfNull(inventory);
        ArgumentNullException.ThrowIfNull(mission);

        if (_isCollected || Quantity <= 0)
        {
            return false;
        }

        AddResourceToInventory(inventory, Type, Quantity);
        _isCollected = true;
        DisableInteraction();
        PlayFeedback();
        ResourcePickup.TryAdvanceResourceObjective(inventory, mission);
        EmitSignal(SignalName.Collected, Variant.From(Type), Quantity);
        ResourceCollected?.Invoke(new ResourceDropCollectedEvent(Type, Quantity));
        _ = CleanupAfterFeedbackAsync();
        return true;
    }

    public void SetHighlighted(bool isHighlighted)
    {
        if (_itemMesh is not null)
        {
            _itemMesh.MaterialOverride = isHighlighted && HighlightMaterial is not null
                ? HighlightMaterial
                : _baseMaterial;
        }

        if (_highlightRing is not null)
        {
            _highlightRing.Visible = isHighlighted && !_isCollected;
        }
    }

    public static void AddResourceToInventory(MvpInventory inventory, ResourceType type, int quantity)
    {
        ArgumentNullException.ThrowIfNull(inventory);

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Resource drop quantities must be positive.");
        }

        switch (type)
        {
            case ResourceType.Metal:
                inventory.AddResources(metal: quantity);
                break;
            case ResourceType.Biomass:
                inventory.AddResources(biomass: quantity);
                break;
            case ResourceType.Electronics:
                inventory.AddResources(electronicComponents: quantity);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown Crash Site resource type.");
        }
    }

    private void DisableInteraction()
    {
        Monitoring = false;
        Monitorable = false;
        CollisionLayer = 0;
        CollisionMask = 0;
        Visible = false;

        if (_interactionCollision is not null)
        {
            _interactionCollision.Disabled = true;
        }

        if (_staticPhysicsBody is not null)
        {
            _staticPhysicsBody.CollisionLayer = 0;
            _staticPhysicsBody.CollisionMask = 0;
        }

        if (_ambientGlow is not null)
        {
            _ambientGlow.Emitting = false;
        }
    }

    private void PlayFeedback()
    {
        if (_pickupBurst is not null)
        {
            _pickupBurst.Restart();
            _pickupBurst.Emitting = true;
        }

        _spatialPickupPlayer?.Play();
    }

    private async System.Threading.Tasks.Task CleanupAfterFeedbackAsync()
    {
        if (!IsInsideTree())
        {
            return;
        }

        var delay = _spatialPickupPlayer?.Stream is null
            ? DefaultCleanupDelaySeconds
            : Mathf.Max(DefaultCleanupDelaySeconds, (float)_spatialPickupPlayer.Stream.GetLength());

        await ToSignal(GetTree().CreateTimer(delay), SceneTreeTimer.SignalName.Timeout);
        QueueFree();
    }
}
