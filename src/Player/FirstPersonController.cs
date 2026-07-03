using System;
using Godot;
using TitanCraft.Core;
using TitanCraft.Crafting;
using TitanCraft.Enemies;
using TitanCraft.Missions;
using TitanCraft.Resources;
using TitanCraft.World;

namespace TitanCraft.Player;

public partial class FirstPersonController : CharacterBody3D
{
    public event Action<string>? InteractionPromptChanged;
    public event Action<string>? ActionFeedbackChanged;
    [Export] public float WalkSpeed { get; set; } = 5.0f;
    [Export] public float JumpVelocity { get; set; } = 4.5f;
    [Export] public float MouseSensitivity { get; set; } = 0.0025f;
    [Export] public float MaxLookAngleDegrees { get; set; } = 85.0f;
    [Export] public float InteractionRange { get; set; } = 3.0f;
    [Export] public float AttackRange { get; set; } = MechanicalArmAttackLogic.DefaultRange;
    [Export] public int MechanicalArmDamage { get; set; } = MechanicalArmAttackLogic.DefaultMechanicalArmDamage;
    [Export] public float AttackCooldownSeconds { get; set; } = MechanicalArmAttackLogic.DefaultCooldownSeconds;
    [Export] public float FallDefeatHeight { get; set; } = -10.0f;
    [Export] public NodePath ArmHitAudioPath { get; set; } = "Head/Camera3D/ArmHitAudio";
    [Export] public NodePath DamageAudioPath { get; set; } = "Head/Camera3D/DamageAudio";
    [Export] public NodePath TimeManagerPath { get; set; } = "TimeManager";
    [Export] public float BaseFov { get; set; } = 75.0f;
    [Export] public float SprintFov { get; set; } = 90.0f;
    [Export] public float FovLerpSpeed { get; set; } = 8.0f;
    [Export] public float SprintSpeedMultiplier { get; set; } = 1.15f;

    public MvpInventory Inventory { get; } = new();

    public CrashSiteMissionState Mission { get; } = new();

    public PlayerHealth Health { get; } = new();

    private Camera3D _camera = null!;
    private Node3D _head = null!;
    private MeshInstance3D? _mechanicalArmVisual;
    private float _gravity;
    private float _cameraPitch;
    private float _bodyYaw;
    private MechanicalArmAttackLogic _mechanicalArmAttack = null!;
    private MechanicalArmRecipe _mechanicalArmRecipe = null!;
    private CameraShaker? _cameraShaker;
    private TimeManager? _timeManager;
    private int _lastHealth;
    private string _interactionPrompt = string.Empty;
    private ILookHighlightTarget? _currentLookTarget;
    private bool _debugHighlightAllResourceDrops;

    public override void _Ready()
    {
        _head = GetNode<Node3D>("Head");
        _camera = GetNode<Camera3D>("Head/Camera3D");
        _camera.Current = true;
        _camera.Fov = BaseFov;
        _cameraShaker = _camera as CameraShaker;
        _timeManager = GetNodeOrNull<TimeManager>(TimeManagerPath);
        _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
        _mechanicalArmAttack = new MechanicalArmAttackLogic(MechanicalArmDamage, AttackCooldownSeconds);
        _mechanicalArmRecipe = new MechanicalArmRecipe();
        _mechanicalArmVisual = GetNodeOrNull<MeshInstance3D>("Head/Camera3D/MechanicalArmVisual");
        Inventory.Changed += UpdateMechanicalArmVisual;
        Health.Changed += OnHealthChanged;
        UpdateMechanicalArmVisual(Inventory);
        _lastHealth = Health.CurrentHealth;
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _ExitTree()
    {
        Inventory.Changed -= UpdateMechanicalArmVisual;
        Health.Changed -= OnHealthChanged;
        ClearResourceLookTarget();
    }


    private void OnHealthChanged(PlayerHealth health)
    {
        if (health.CurrentHealth < _lastHealth)
        {
            _cameraShaker?.AddTrauma(0.4f);
            AudioCue.Play(this, DamageAudioPath);
        }

        _lastHealth = health.CurrentHealth;
    }

    private void UpdateMechanicalArmVisual(MvpInventory inventory)
    {
        // The inventory is the single authority for arm ownership; crafting and
        // save restoration both flow through its Changed event.
        if (_mechanicalArmVisual is not null)
        {
            _mechanicalArmVisual.Visible = inventory.IsMechanicalArmBuilt;
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("interact"))
        {
            TryInteract();
            return;
        }

        if (@event.IsActionPressed("attack"))
        {
            TryAttack();
            return;
        }

#if DEBUG
        if (@event is InputEventKey { Pressed: true, Echo: false, Keycode: Key.F3 })
        {
            ToggleDebugResourceHighlights();
            return;
        }
#endif

        if (@event is InputEventMouseMotion mouseMotion)
        {
            var look = FirstPersonMovement.ApplyMouseLook(
                new Vector2(_bodyYaw, _cameraPitch),
                mouseMotion.Relative,
                MouseSensitivity,
                MaxLookAngleDegrees);
            _bodyYaw = look.X;
            _cameraPitch = look.Y;
            Rotation = new Vector3(0.0f, _bodyYaw, 0.0f);
            _head.Rotation = new Vector3(_cameraPitch, 0.0f, 0.0f);
        }
    }

    public bool TryAttack()
    {
        if (!Inventory.IsMechanicalArmBuilt)
        {
            ShowActionFeedback("Attack blocked: Mk I is not built. Press E at the workbench after collecting resources.");
            return false;
        }

        if (_mechanicalArmAttack.IsOnCooldown)
        {
            ShowActionFeedback("Mk I recharging — wait a moment before the next punch.");
            return false;
        }

        var query = PhysicsRayQueryParameters3D.Create(
            _camera.GlobalPosition,
            _camera.GlobalPosition - _camera.GlobalTransform.Basis.Z * AttackRange);
        query.CollideWithAreas = false;
        query.CollideWithBodies = true;

        var hit = GetWorld3D().DirectSpaceState.IntersectRay(query);
        if (!hit.TryGetValue("collider", out var colliderVariant))
        {
            ShowActionFeedback("Attack missed: aim at the Galaxabrain within Mk I range.");
            return false;
        }

        if (colliderVariant.AsGodotObject() is not GalaxabrainScout scout || scout.Brain.IsDead)
        {
            ShowActionFeedback("Attack ready: aim at the living Galaxabrain, then left click.");
            return false;
        }

        if (!_mechanicalArmAttack.TryAttack(Inventory))
        {
            ShowActionFeedback("Mk I cannot strike yet.");
            return false;
        }

        scout.ApplyDamage(_mechanicalArmAttack.Damage);
        _timeManager?.TriggerDefaultHitStop();
        _cameraShaker?.AddTrauma(0.22f);
        AudioCue.Play(this, ArmHitAudioPath);
        ShowActionFeedback($"Mk I punch landed: Galaxabrain took {_mechanicalArmAttack.Damage} damage.");
        return true;
    }

    public bool TryInteract()
    {
        var query = PhysicsRayQueryParameters3D.Create(
            _camera.GlobalPosition,
            _camera.GlobalPosition - _camera.GlobalTransform.Basis.Z * InteractionRange);
        query.CollideWithAreas = true;
        query.CollideWithBodies = true;

        var hit = GetWorld3D().DirectSpaceState.IntersectRay(query);
        if (!hit.TryGetValue("collider", out var colliderVariant))
        {
            return false;
        }

        if (!TryFindInteractable(colliderVariant.AsGodotObject(), out var interactable))
        {
            return false;
        }

        var interacted = interactable.Interact(Inventory, Mission);
        if (interacted && interactable is ResourceDrop)
        {
            ClearResourceLookTarget();
        }

        return interacted;
    }

    private void UpdateInteractionPrompt()
    {
        var prompt = TryGetTargetInteractionPrompt(out var targetPrompt, out var highlightTarget)
            ? targetPrompt
            : string.Empty;

        UpdateLookTarget(highlightTarget);

        if (prompt == _interactionPrompt)
            return;

        _interactionPrompt = prompt;
        InteractionPromptChanged?.Invoke(prompt);
    }

    private bool TryGetTargetInteractionPrompt(out string prompt, out ILookHighlightTarget? highlightTarget)
    {
        prompt = string.Empty;
        highlightTarget = null;

        var query = PhysicsRayQueryParameters3D.Create(
            _camera.GlobalPosition,
            _camera.GlobalPosition - _camera.GlobalTransform.Basis.Z * InteractionRange);
        query.CollideWithAreas = true;
        query.CollideWithBodies = true;

        var hit = GetWorld3D().DirectSpaceState.IntersectRay(query);
        if (!hit.TryGetValue("collider", out var colliderVariant)
            || !TryFindInteractable(colliderVariant.AsGodotObject(), out var interactable)
            || interactable is not Node node)
        {
            return false;
        }

        highlightTarget = FindLookHighlightTarget(colliderVariant.AsGodotObject());
        prompt = node is Workbench ? BuildWorkbenchPrompt() : $"Press E to interact with {node.Name.ToString().Replace("Placeholder_", string.Empty)}";
        return true;
    }

    private void UpdateLookTarget(ILookHighlightTarget? nextTarget)
    {
        if (_currentLookTarget is GodotObject currentObject && !GodotObject.IsInstanceValid(currentObject))
        {
            _currentLookTarget = null;
        }

        if (ReferenceEquals(_currentLookTarget, nextTarget))
        {
            return;
        }

        _currentLookTarget?.SetHighlighted(false);
        _currentLookTarget = nextTarget;
        _currentLookTarget?.SetHighlighted(true);
    }

    private void ClearResourceLookTarget()
    {
        if (_currentLookTarget is GodotObject currentObject && GodotObject.IsInstanceValid(currentObject))
        {
            _currentLookTarget?.SetHighlighted(false);
        }

        _currentLookTarget = null;
    }

    private static bool TryFindInteractable(GodotObject? collider, out ICrashSiteInteractable interactable)
    {
        var node = collider as Node;
        while (node is not null)
        {
            if (node is ICrashSiteInteractable candidate)
            {
                interactable = candidate;
                return true;
            }

            node = node.GetParent();
        }

        interactable = null!;
        return false;
    }

    private static ILookHighlightTarget? FindLookHighlightTarget(GodotObject? collider)
    {
        var node = collider as Node;
        while (node is not null)
        {
            if (node is ILookHighlightTarget target)
            {
                return target;
            }

            node = node.GetParent();
        }

        return null;
    }

#if DEBUG
    private void ToggleDebugResourceHighlights()
    {
        _debugHighlightAllResourceDrops = !_debugHighlightAllResourceDrops;
        var highlightedCount = 0;

        foreach (var node in GetTree().GetNodesInGroup(ResourceDrop.ResourceDropGroup))
        {
            if (node is ResourceDrop resourceDrop && GodotObject.IsInstanceValid(resourceDrop))
            {
                resourceDrop.SetHighlighted(_debugHighlightAllResourceDrops);
                highlightedCount++;
            }
        }

        GD.Print($"[ResourceDropDebug] F3 highlight={_debugHighlightAllResourceDrops} affected={highlightedCount}");
    }
#endif

    private string BuildWorkbenchPrompt()
    {
        if (Inventory.IsMechanicalArmBuilt)
            return "Workbench: Mk I already built. Left click attacks the Galaxabrain.";

        var cost = $"needs {_mechanicalArmRecipe.MetalCost} Metal, {_mechanicalArmRecipe.BiomassCost} Biomass, {_mechanicalArmRecipe.ElectronicComponentsCost} Electronics";
        return _mechanicalArmRecipe.CanCraft(Inventory) && Mission.CurrentStep == CrashSiteMissionStep.BuildMechanicalArm
            ? $"Press E to craft Mechanical Arm Mk I ({cost})."
            : $"Workbench: Mechanical Arm Mk I {cost}. Collect resources first.";
    }

    private void ShowActionFeedback(string message)
    {
        // This explicit HUD feedback proves whether craft/attack input was accepted, blocked, or missed.
        ActionFeedbackChanged?.Invoke(message);
    }

    public override void _PhysicsProcess(double delta)
    {
        // The crash-site slab has no perimeter containment; leaving it must end in
        // the standard death flow (defeat screen -> reload last save), never an endless fall.
        if (GlobalPosition.Y < FallDefeatHeight && !Health.IsDead)
        {
            Health.ApplyDamage(Health.CurrentHealth);
            return;
        }

        _mechanicalArmAttack.Tick((float)delta);
        UpdateInteractionPrompt();

        var velocity = Velocity;

        if (!IsOnFloor())
        {
            velocity.Y -= _gravity * (float)delta;
        }

        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }

        var inputDirection = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        var moveDirection = FirstPersonMovement.GetMoveDirection(Transform.Basis, inputDirection);
        var isSprinting = Input.IsActionPressed("sprint") && inputDirection.LengthSquared() > 0.01f;
        var targetFov = isSprinting ? SprintFov : BaseFov;
        _camera.Fov = Mathf.Lerp(_camera.Fov, targetFov, 1.0f - Mathf.Exp(-FovLerpSpeed * (float)delta));

        var speed = WalkSpeed * (isSprinting ? SprintSpeedMultiplier : 1.0f);
        velocity.X = moveDirection.X * speed;
        velocity.Z = moveDirection.Z * speed;
        Velocity = velocity;
        MoveAndSlide();
    }
}
