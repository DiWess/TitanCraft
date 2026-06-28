using System;
using Godot;
using TitanCraft.Player;

namespace TitanCraft.Tests.Integration;

public partial class IntegrationTestRunner : Node
{
    private const string MainScenePath = "res://scenes/Main/Main.tscn";
    private const string PlayerScenePath = "res://scenes/Player/Player.tscn";
    private static readonly string[] RequiredActions = ["move_forward", "move_backward", "move_left", "move_right", "jump", "quit_game"];

    public override async void _Ready()
    {
        try
        {
            TestInputMap();
            await TestMainScene();
            await TestPlayerScene();
            await TestPhysicsAndMovement();
            await TestJumpAndCamera();
            GD.Print("TITANCRAFT_INTEGRATION_TESTS_PASS");
            GetTree().Quit(0);
        }
        catch (Exception exception)
        {
            GD.PushError(exception.ToString());
            GetTree().Quit(1);
        }
    }

    private static void TestInputMap()
    {
        foreach (var action in RequiredActions)
            Require(InputMap.HasAction(action), $"Missing InputMap action: {action}");
        foreach (var action in RequiredActions[..4])
            RequireHasPhysicalKey(action);
        RequireHasKey("jump", Key.Space);
        RequireHasKey("quit_game", Key.Escape);
    }

    private async System.Threading.Tasks.Task TestMainScene()
    {
        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        await Frames(2);
        var player = main.GetNode<CharacterBody3D>("Player");
        var ground = main.GetNode<StaticBody3D>("Ground");
        Require(ground.GetNode<MeshInstance3D>("MeshInstance3D").Mesh is not null, "Ground mesh missing");
        Require(ground.GetNode<CollisionShape3D>("CollisionShape3D").Shape is not null, "Ground collision missing");
        Require(main.GetNode<DirectionalLight3D>("DirectionalLight3D") is not null, "Light missing");
        Require(player.GlobalPosition.Y > ground.GlobalPosition.Y, "Player is not above ground");
        main.QueueFree();
        await Frames(2);
    }

    private async System.Threading.Tasks.Task TestPlayerScene()
    {
        var player = LoadScene<FirstPersonController>(PlayerScenePath);
        AddChild(player);
        await Frames(2);
        Require(player is CharacterBody3D, "Player root is not CharacterBody3D");
        Require(player.GetNode<CollisionShape3D>("CollisionShape3D").Shape is CapsuleShape3D capsule && capsule.Radius > 0.0f && capsule.Height > 0.0f, "Player capsule invalid");
        Require(player.GetNode<Node3D>("Head") is not null, "Head missing");
        Require(player.GetNode<Camera3D>("Head/Camera3D").Current, "Camera inactive");
        Require(FirstPersonMovement.HasValidParameters(player.WalkSpeed, player.JumpVelocity, player.MouseSensitivity, player.MaxLookAngleDegrees), "Player exported parameters invalid");
        player.QueueFree();
        await Frames(2);
    }

    private async System.Threading.Tasks.Task TestPhysicsAndMovement()
    {
        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        var player = main.GetNode<CharacterBody3D>("Player");
        await Frames(180);
        Require(player.IsOnFloor(), "Player did not reach floor");
        RequireFinite(player.GlobalPosition, "Player position is not finite");
        Require(player.GlobalPosition.Y >= 0.0f, "Player crossed below ground");

        var forward = await MoveDistance(main, "move_forward", new Vector3(0, 0, -1));
        await MoveDistance(main, "move_backward", new Vector3(0, 0, 1));
        await MoveDistance(main, "move_left", new Vector3(-1, 0, 0));
        await MoveDistance(main, "move_right", new Vector3(1, 0, 0));
        var start = player.GlobalPosition;
        Input.ActionPress("move_forward");
        Input.ActionPress("move_right");
        await Frames(30);
        Input.ActionRelease("move_forward");
        Input.ActionRelease("move_right");
        var diagonal = HorizontalDistance(start, player.GlobalPosition);
        Require(diagonal <= forward + 0.25f, "Diagonal movement exceeded cardinal speed tolerance");
        main.QueueFree();
        await Frames(2);
    }

    private async System.Threading.Tasks.Task TestJumpAndCamera()
    {
        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        var player = main.GetNode<FirstPersonController>("Player");
        await Frames(180);
        var floorY = player.GlobalPosition.Y;
        Input.ActionPress("jump");
        await Frames(1);
        Input.ActionRelease("jump");
        await Frames(10);
        Require(player.GlobalPosition.Y > floorY, "Jump did not increase height");
        var velocityAfterJump = player.Velocity.Y;
        Input.ActionPress("jump");
        await Frames(1);
        Input.ActionRelease("jump");
        Require(player.Velocity.Y <= velocityAfterJump, "Air double jump was applied");
        await Frames(180);
        Require(player.IsOnFloor(), "Player did not land after jump");

        var initialYaw = player.Rotation.Y;
        player._UnhandledInput(new InputEventMouseMotion { Relative = new Vector2(100.0f, 0.0f) });
        player._UnhandledInput(new InputEventMouseMotion { Relative = new Vector2(0.0f, 100000.0f) });
        player._UnhandledInput(new InputEventMouseMotion { Relative = new Vector2(0.0f, -200000.0f) });
        var pitch = player.GetNode<Node3D>("Head").Rotation.X;
        Require(!Mathf.IsEqualApprox(player.Rotation.Y, initialYaw), "Yaw did not change");
        Require(pitch >= Mathf.DegToRad(-85.0f) && pitch <= Mathf.DegToRad(85.0f), "Pitch escaped clamp");
        Require(float.IsFinite(pitch) && float.IsFinite(player.Rotation.Y), "Camera rotation is not finite");
        main.QueueFree();
        await Frames(2);
    }

    private async System.Threading.Tasks.Task<float> MoveDistance(Node3D main, string action, Vector3 expectedDirection)
    {
        var player = main.GetNode<CharacterBody3D>("Player");
        var start = player.GlobalPosition;
        Input.ActionPress(action);
        await Frames(30);
        Input.ActionRelease(action);
        await Frames(2);
        var delta = player.GlobalPosition - start;
        Require(delta.Dot(expectedDirection) > 0.1f, $"{action} did not move in expected direction");
        Require(Mathf.Abs(delta.Y) < 0.25f, $"{action} caused abnormal vertical movement");
        return HorizontalDistance(start, player.GlobalPosition);
    }

    private async System.Threading.Tasks.Task Frames(int count)
    {
        for (var i = 0; i < count; i++)
            await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
    }

    private static T LoadScene<T>(string path) where T : Node
    {
        var scene = ResourceLoader.Load<PackedScene>(path)
            ?? throw new InvalidOperationException($"Failed to load {path}");
        return scene.Instantiate<T>();
    }

    private static float HorizontalDistance(Vector3 from, Vector3 to) => new Vector2(to.X - from.X, to.Z - from.Z).Length();

    private static void RequireHasPhysicalKey(string action)
    {
        foreach (var inputEvent in InputMap.ActionGetEvents(action))
            if (inputEvent is InputEventKey { PhysicalKeycode: not Key.None })
                return;
        throw new InvalidOperationException($"{action} has no physical key");
    }

    private static void RequireHasKey(string action, Key key)
    {
        foreach (var inputEvent in InputMap.ActionGetEvents(action))
            if (inputEvent is InputEventKey eventKey && eventKey.Keycode == key)
                return;
        throw new InvalidOperationException($"{action} is not mapped to {key}");
    }

    private static void RequireFinite(Vector3 value, string message) => Require(float.IsFinite(value.X) && float.IsFinite(value.Y) && float.IsFinite(value.Z), message);

    private static void Require(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }
}
