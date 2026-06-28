using Godot;

namespace TitanCraft.Player;

public partial class FirstPersonController : CharacterBody3D
{
    [Export] public float WalkSpeed { get; set; } = 5.0f;
    [Export] public float JumpVelocity { get; set; } = 4.5f;
    [Export] public float MouseSensitivity { get; set; } = 0.0025f;
    [Export] public float MaxLookAngleDegrees { get; set; } = 85.0f;

    private Camera3D _camera = null!;
    private Node3D _head = null!;
    private float _gravity;
    private float _cameraPitch;

    public override void _Ready()
    {
        _head = GetNode<Node3D>("Head");
        _camera = GetNode<Camera3D>("Head/Camera3D");
        _camera.Current = true;
        _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("quit_game"))
        {
            GetTree().Quit();
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

    public override void _PhysicsProcess(double delta)
    {
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
