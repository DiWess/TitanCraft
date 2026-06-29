using System;
using Godot;
using TitanCraft.Enemies;
using TitanCraft.Missions;
using TitanCraft.Resources;
using TitanCraft.World;

namespace TitanCraft.Player;

public partial class FirstPersonController : CharacterBody3D
{
    public event Action<string>? InteractionPromptChanged;
    [Export] public float WalkSpeed { get; set; } = 5.0f;
    [Export] public float JumpVelocity { get; set; } = 4.5f;
    [Export] public float MouseSensitivity { get; set; } = 0.0025f;
    [Export] public float MaxLookAngleDegrees { get; set; } = 85.0f;
    [Export] public float InteractionRange { get; set; } = 3.0f;
    [Export] public float AttackRange { get; set; } = MechanicalArmAttackLogic.DefaultRange;
    [Export] public int MechanicalArmDamage { get; set; } = MechanicalArmAttackLogic.DefaultMechanicalArmDamage;
    [Export] public float AttackCooldownSeconds { get; set; } = MechanicalArmAttackLogic.DefaultCooldownSeconds;

    public MvpInventory Inventory { get; } = new();

    public CrashSiteMissionState Mission { get; } = new();

    public PlayerHealth Health { get; } = new();

    private Camera3D _camera = null!;
    private Node3D _head = null!;
    private float _gravity;
    private float _cameraPitch;
    private MechanicalArmAttackLogic _mechanicalArmAttack = null!;
    private string _interactionPrompt = string.Empty;

    public override void _Ready()
    {
        _head = GetNode<Node3D>("Head");
        _camera = GetNode<Camera3D>("Head/Camera3D");
        _camera.Current = true;
        _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
        _mechanicalArmAttack = new MechanicalArmAttackLogic(MechanicalArmDamage, AttackCooldownSeconds);
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("quit_game"))
        {
            GetTree().Quit();
            return;
        }

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

        if (@event is InputEventMouseMotion mouseMotion)
        {
            RotateY(-mouseMotion.Relative.X * MouseSensitivity);
            _cameraPitch = FirstPersonMovement.ClampPitch(
                _cameraPitch - mouseMotion.Relative.Y * MouseSensitivity,
                MaxLookAngleDegrees);
            _head.Rotation = new Vector3(_cameraPitch, 0.0f, 0.0f);
        }
    }

    public bool TryAttack()
    {
        var query = PhysicsRayQueryParameters3D.Create(
            _camera.GlobalPosition,
            _camera.GlobalPosition - _camera.GlobalTransform.Basis.Z * AttackRange);
        query.CollideWithAreas = false;
        query.CollideWithBodies = true;

        var hit = GetWorld3D().DirectSpaceState.IntersectRay(query);
        if (!hit.TryGetValue("collider", out var colliderVariant))
        {
            return false;
        }

        return colliderVariant.AsGodotObject() is GalaxabrainScout scout
            && _mechanicalArmAttack.TryAttack(Inventory, scout.Brain);
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

        return colliderVariant.AsGodotObject() is ICrashSiteInteractable interactable
            && interactable.Interact(Inventory, Mission);
    }

    private void UpdateInteractionPrompt()
    {
        var prompt = TryGetTargetInteractableName(out var interactableName)
            ? $"Press E to interact with {interactableName}"
            : string.Empty;

        if (prompt == _interactionPrompt)
            return;

        _interactionPrompt = prompt;
        InteractionPromptChanged?.Invoke(prompt);
    }

    private bool TryGetTargetInteractableName(out string interactableName)
    {
        interactableName = string.Empty;

        var query = PhysicsRayQueryParameters3D.Create(
            _camera.GlobalPosition,
            _camera.GlobalPosition - _camera.GlobalTransform.Basis.Z * InteractionRange);
        query.CollideWithAreas = true;
        query.CollideWithBodies = true;

        var hit = GetWorld3D().DirectSpaceState.IntersectRay(query);
        if (!hit.TryGetValue("collider", out var colliderVariant)
            || colliderVariant.AsGodotObject() is not ICrashSiteInteractable
            || colliderVariant.AsGodotObject() is not Node node)
        {
            return false;
        }

        interactableName = node.Name.ToString().Replace("Placeholder_", string.Empty);
        return true;
    }

    public override void _PhysicsProcess(double delta)
    {
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

        velocity.X = moveDirection.X * WalkSpeed;
        velocity.Z = moveDirection.Z * WalkSpeed;
        Velocity = velocity;
        MoveAndSlide();
    }
}
