using System;
using System.Collections.Generic;
using Godot;
using TitanCraft.Enemies;
using TitanCraft.Player;
using TitanCraft.Missions;
using TitanCraft.SaveSystem;
using TitanCraft.UI;
using TitanCraft.World;

namespace TitanCraft.Tests.Integration;

public partial class IntegrationTestRunner : Node
{
    private const string MainScenePath = "res://scenes/Main/Main.tscn";
    private const string PlayerScenePath = "res://scenes/Player/Player.tscn";
    private const string GalaxabrainScoutScenePath = "res://scenes/Enemies/GalaxabrainScout.tscn";
    private static readonly string[] UiScenePaths = [
        "res://scenes/UI/HUD.tscn",
        "res://scenes/UI/MainMenu.tscn",
        "res://scenes/UI/PauseMenu.tscn",
        "res://scenes/UI/VictoryScreen.tscn",
        "res://scenes/UI/DefeatScreen.tscn",
    ];
    private static readonly string[] RequiredMaterials = [
        "res://assets/Materials/HumanIvory.tres",
        "res://assets/Materials/HumanGraphite.tres",
        "res://assets/Materials/HumanBronze.tres",
        "res://assets/Materials/HumanOrangeInteractive.tres",
        "res://assets/Materials/VolcanicRock.tres",
        "res://assets/Materials/AlienBlack.tres",
        "res://assets/Materials/AlienVioletEmissive.tres",
        "res://assets/Materials/BiomassRed.tres",
    ];
    private const int MaxStaticCollisionShapes = 19;
    private static readonly string[] ForbiddenCollisionPrefixes = [
        "Moon",
        "Background",
        "Distant",
        "Crystal_Decorative",
        "SmallRock",
        "Debris_Small",
        "Puddle",
        "Banner",
        "Lamp",
        "EnergyEffect",
        "DecorativeMech",
    ];
    private static readonly string[] RequiredActions = ["move_forward", "move_backward", "move_left", "move_right", "jump", "pause_menu"];

    public override async void _Ready()
    {
        try
        {
            LocalSaveGameStore.DeleteSave();
            TestInputMap();
            TestVisualMaterialsLoad();
            await TestMainScene();
            await TestCollisionPolicy();
            await TestPlayerScene();
            await TestGalaxabrainScoutDeathPickup();
            await TestUiScenes();
            await TestMainMenuContinueState();
            await TestHudStartTutorial();
            await TestHudBinding();
            await TestEndScreenNavigation();
            await TestSaveLoadFlow();
            await TestBeaconVisualState();
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
        RequireHasPhysicalKey("move_forward", Key.W);
        RequireHasPhysicalKey("move_forward", Key.Z);
        RequireHasPhysicalKey("move_left", Key.A);
        RequireHasPhysicalKey("move_left", Key.Q);
        RequireHasPhysicalKey("move_backward", Key.S);
        RequireHasPhysicalKey("move_right", Key.D);
        RequireHasKey("jump", Key.Space);
        RequireHasKey("pause_menu", Key.Escape);
        RequireGameplayMovementDoesNotTriggerQuitGame();
    }

    private static void RequireGameplayMovementDoesNotTriggerQuitGame()
    {
        if (!InputMap.HasAction("quit_game"))
            return;

        foreach (var action in RequiredActions[..4])
            foreach (var inputEvent in InputMap.ActionGetEvents(action))
                Require(!inputEvent.IsAction("quit_game"), $"{action} also triggers quit_game");
    }

    private async System.Threading.Tasks.Task TestMainScene()
    {
        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        await Frames(2);
        var player = main.GetNode<CharacterBody3D>("Player");
        var ground = main.GetNode<StaticBody3D>("Ground");
        Require(ground.GetNode<MeshInstance3D>("MeshInstance3D").Mesh is not null, "Ground mesh missing");
        Require(ground.GetNode<CollisionShape3D>("Collision_Ground").Shape is not null, "Ground collision missing");
        Require(main.GetNode<DirectionalLight3D>("DirectionalLight3D") is not null, "Light missing");
        Require(player.GlobalPosition.Y > ground.GlobalPosition.Y, "Player is not above ground");
        Require(main.FindChildren("*", "WorldEnvironment", false, false).Count == 1, "Main scene must have one active WorldEnvironment");
        Require(main.GetNode<Area3D>("Placeholder_Workbench") is not null, "Workbench missing");
        var beacon = main.GetNode<Beacon>("Placeholder_Beacon");
        Require(beacon.GetNode<Node3D>("ClosedVisual").Visible, "Closed beacon visual missing");
        Require(!beacon.GetNode<Node3D>("ActiveVisual").Visible, "Active beacon visual should start hidden");
        Require(main.GetNode<Area3D>("Placeholder_MetalPickup") is not null, "Metal pickup missing");
        Require(main.GetNode<Area3D>("Placeholder_BiomassPickup") is not null, "Biomass pickup missing");
        Require(main.GetNode<Area3D>("Placeholder_ElectronicsPickup") is not null, "Electronics pickup missing");
        Require(main.GetNode<Node3D>("Moon") is not null, "Large moon missing");
        Require(main.GetNode<Node3D>("AlienCrystal_1") is not null, "Alien crystal route missing");
        main.QueueFree();
        await Frames(2);
    }


    private async System.Threading.Tasks.Task TestCollisionPolicy()
    {
        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        await Frames(2);

        var staticCollisionCount = 0;
        var staticCollisionPositions = new List<Vector3>();
        foreach (var body in main.FindChildren("*", "StaticBody3D", true, false))
        {
            var staticBody = (StaticBody3D)body;
            var shapes = staticBody.FindChildren("*", "CollisionShape3D", false, false);
            Require(shapes.Count > 0, $"StaticBody3D has no collision: {staticBody.Name}");
            foreach (var child in shapes)
            {
                var collision = (CollisionShape3D)child;
                Require(IsAllowedCollisionShape(collision.Shape), $"Unsupported collision shape on {staticBody.Name}/{collision.Name}");
                staticCollisionCount++;
                staticCollisionPositions.Add(collision.GlobalPosition);
            }
        }

        Require(staticCollisionCount <= MaxStaticCollisionShapes, $"Static collision budget exceeded: {staticCollisionCount}");
        Require(main.GetNode<CollisionShape3D>("Ground/Collision_Ground").Shape is BoxShape3D, "Ground must use a BoxShape3D");
        Require(main.GetNode<CollisionShape3D>("C7_Wall_1/Collision_C7Wall").Shape is BoxShape3D, "C7 wall 1 collision missing");
        Require(main.GetNode<CollisionShape3D>("C7_Wall_2/Collision_C7Wall").Shape is BoxShape3D, "C7 wall 2 collision missing");
        Require(main.GetNode<CollisionShape3D>("C7_Wall_3/Collision_C7Wall").Shape is BoxShape3D, "C7 wall 3 collision missing");
        Require(main.GetNode<CollisionShape3D>("C7_Wall_4/Collision_C7Wall").Shape is BoxShape3D, "C7 wall 4 collision missing");
        Require(main.GetNode<CollisionShape3D>("Placeholder_Workbench/Collision_Workbench").Shape is BoxShape3D, "Workbench interaction collision missing");
        Require(main.GetNode<CollisionShape3D>("Placeholder_Beacon/Collision_BeaconBase").Shape is BoxShape3D, "Beacon interaction collision missing");

        foreach (var prefix in ForbiddenCollisionPrefixes)
            foreach (var node in main.FindChildren($"{prefix}*", "CollisionObject3D", true, false))
                Require(false, $"Forbidden decorative collision object: {node.GetPath()}");

        foreach (var pickupName in new[] { "Placeholder_MetalPickup", "Placeholder_BiomassPickup", "Placeholder_ElectronicsPickup" })
        {
            var pickup = main.GetNode<Node3D>(pickupName);
            foreach (var collisionPosition in staticCollisionPositions)
                Require(HorizontalDistance(pickup.GlobalPosition, collisionPosition) > 1.2f, $"{pickupName} overlaps a static collision");
        }

        var player = main.GetNode<Node3D>("Player");
        foreach (var collisionPosition in staticCollisionPositions)
            Require(HorizontalDistance(player.GlobalPosition, collisionPosition) > 1.2f || player.GlobalPosition.Y > collisionPosition.Y + 0.5f, "Player spawn overlaps a static collision");

        main.QueueFree();
        await Frames(2);
    }

    private static bool IsAllowedCollisionShape(Shape3D? shape)
    {
        return shape is BoxShape3D or CapsuleShape3D or CylinderShape3D or SphereShape3D;
    }

    private static void TestVisualMaterialsLoad()
    {
        foreach (var path in RequiredMaterials)
            Require(ResourceLoader.Load<StandardMaterial3D>(path) is not null, $"Material missing or not StandardMaterial3D: {path}");
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


    private async System.Threading.Tasks.Task TestGalaxabrainScoutDeathPickup()
    {
        var scout = LoadScene<GalaxabrainScout>(GalaxabrainScoutScenePath);
        AddChild(scout);
        await Frames(2);

        var missionComponent = scout.GetNode<Area3D>("GalaxabrainComponentPickup");
        Require(!missionComponent.Visible, "Galaxabrain component pickup should start hidden");
        Require(!missionComponent.Monitoring, "Galaxabrain component pickup should start non-monitoring");

        for (var hit = 0; hit < 4; hit++)
            scout.ApplyDamage(MechanicalArmAttackLogic.DefaultMechanicalArmDamage);

        Require(scout.Brain.IsDead, "Galaxabrain Scout did not die after four arm hits");
        Require(!scout.Visible, "Galaxabrain Scout stayed visible after death");
        Require(missionComponent.Visible, "Galaxabrain component pickup did not become visible after scout death");
        Require(missionComponent.Monitoring, "Galaxabrain component pickup did not become collectable after scout death");

        scout.QueueFree();
        await Frames(2);
    }


    private async System.Threading.Tasks.Task TestUiScenes()
    {
        foreach (var path in UiScenePaths)
        {
            var scene = LoadScene<Node>(path);
            AddChild(scene);
            await Frames(2);
            Require(scene.GetTree() is not null, $"UI scene did not enter tree: {path}");
            scene.QueueFree();
            await Frames(2);
        }

        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        await Frames(2);
        Require(main.GetNode<CanvasLayer>("HUD") is not null, "Main scene HUD missing");
        var pause = main.GetNode<CanvasLayer>("PauseMenu");
        Require(pause.GetNode<Button>("Panel/Menu/ResumeButton") is not null, "Pause resume button missing");
        Require(pause.GetNode<Button>("Panel/Menu/MainMenuButton") is not null, "Pause main menu button missing");
        main.QueueFree();
        await Frames(2);
    }

    private async System.Threading.Tasks.Task TestMainMenuContinueState()
    {
        const string testSavePath = "user://crash_site_menu_test_save.json";
        LocalSaveGameStore.DeleteSave(testSavePath);

        var menuWithoutSave = LoadScene<MainMenu>("res://scenes/UI/MainMenu.tscn");
        menuWithoutSave.SavePath = testSavePath;
        AddChild(menuWithoutSave);
        await Frames(2);
        Require(menuWithoutSave.GameScenePath == MainScenePath, "Main menu New Game/Continue scene path changed away from Crash Site");
        Require(menuWithoutSave.GetNode<Button>("Menu/ContinueButton").Disabled, "Continue button should be disabled when no save exists");
        menuWithoutSave.QueueFree();
        await Frames(2);

        LocalSaveGameStore.Save(new CrashSiteSaveData(), testSavePath);
        var menuWithSave = LoadScene<MainMenu>("res://scenes/UI/MainMenu.tscn");
        menuWithSave.SavePath = testSavePath;
        AddChild(menuWithSave);
        await Frames(2);
        Require(!menuWithSave.GetNode<Button>("Menu/ContinueButton").Disabled, "Continue button should be enabled when a save exists");
        menuWithSave.QueueFree();
        await Frames(2);

        LocalSaveGameStore.DeleteSave(testSavePath);
    }




    private async System.Threading.Tasks.Task TestHudStartTutorial()
    {
        var hud = LoadScene<CrashSiteHud>("res://scenes/UI/HUD.tscn");
        AddChild(hud);
        await Frames(2);
        var startTutorial = hud.GetNode<Label>("Panel/Margin/VBox/StartTutorial");
        Require(startTutorial.Visible, "HUD start tutorial should start visible");
        Require(startTutorial.Text.Contains("ZQSD/WASD"), "HUD start tutorial missing movement controls");
        Require(startTutorial.Text.Contains("Mouse"), "HUD start tutorial missing mouse look control");
        Require(startTutorial.Text.Contains("Space"), "HUD start tutorial missing jump control");
        Require(startTutorial.Text.Contains("E"), "HUD start tutorial missing interact control");
        Require(startTutorial.Text.Contains("Left click"), "HUD start tutorial missing attack control");
        Require(startTutorial.Text.Contains("Esc"), "HUD start tutorial missing pause control");
        hud.QueueFree();
        await Frames(2);
    }

    private async System.Threading.Tasks.Task TestHudBinding()
    {
        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        await Frames(2);
        var player = main.GetNode<FirstPersonController>("Player");
        var hud = main.GetNode<CrashSiteHud>("HUD");

        player.Health.ApplyDamage(25);
        player.Inventory.AddResources(metal: 4, biomass: 2, electronicComponents: 1);
        player.Inventory.MarkMechanicalArmBuilt();
        player.Mission.TryCompleteResourceCollection();
        await Frames(2);

        Require(hud.GetNode<Label>("Panel/Margin/VBox/Health").Text == "Health: 75/100", "HUD health did not update from player health");
        Require(hud.GetNode<Label>("Panel/Margin/VBox/Resources").Text.Contains("Metal: 4"), "HUD resources did not update from inventory");
        Require(hud.GetNode<Label>("Panel/Margin/VBox/MechanicalArmState").Text.Contains("Online"), "HUD arm state did not update from inventory");
        Require(hud.GetNode<Label>("Panel/Margin/VBox/Objective").Text.Contains("Mechanical Arm Mk I"), "HUD objective did not update from mission state");
        Require(hud.GetNode<Label>("Panel/Margin/VBox/InteractionPrompt").Visible == false, "HUD interaction prompt should start hidden without a target");
        Require(hud.GetNode<Label>("Panel/Margin/VBox/StartTutorial").Visible == false, "HUD start tutorial should hide after mission progression");
        main.QueueFree();
        await Frames(2);
    }


    private async System.Threading.Tasks.Task TestEndScreenNavigation()
    {
        var victoryMain = LoadScene<Node3D>(MainScenePath);
        AddChild(victoryMain);
        await Frames(2);
        var victoryPlayer = victoryMain.GetNode<FirstPersonController>("Player");
        var victoryNavigator = victoryMain.GetNode<CrashSiteEndScreenNavigator>("EndScreenNavigator");
        victoryNavigator.EnableSceneChanges = false;
        victoryPlayer.Mission.TryCompleteResourceCollection();
        victoryPlayer.Mission.TryCompleteMechanicalArmConstruction();
        victoryPlayer.Mission.TryCompleteGalaxabrainDefeat(true);
        victoryPlayer.Mission.TryCompleteComponentRecovery();
        victoryPlayer.Mission.TryCompleteBeaconActivation();
        await Frames(2);
        Require(victoryNavigator.LastRequestedScenePath == "res://scenes/UI/VictoryScreen.tscn", "Victory screen was not requested after mission victory");
        victoryMain.QueueFree();
        await Frames(2);

        var defeatMain = LoadScene<Node3D>(MainScenePath);
        AddChild(defeatMain);
        await Frames(2);
        var defeatPlayer = defeatMain.GetNode<FirstPersonController>("Player");
        var defeatNavigator = defeatMain.GetNode<CrashSiteEndScreenNavigator>("EndScreenNavigator");
        defeatNavigator.EnableSceneChanges = false;
        defeatPlayer.Health.ApplyDamage(PlayerHealth.DefaultMaxHealth);
        await Frames(2);
        Require(defeatNavigator.LastRequestedScenePath == "res://scenes/UI/DefeatScreen.tscn", "Defeat screen was not requested after player death");
        defeatMain.QueueFree();
        await Frames(2);
    }


    private async System.Threading.Tasks.Task TestBeaconVisualState()
    {
        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        await Frames(2);
        var player = main.GetNode<FirstPersonController>("Player");
        main.GetNode<CrashSiteEndScreenNavigator>("EndScreenNavigator").EnableSceneChanges = false;
        var beacon = main.GetNode<Beacon>("Placeholder_Beacon");
        player.Mission.Restore(CrashSiteMissionStep.ActivateBeacon);
        player.Inventory.MarkGalaxabrainComponentCollected();
        Require(beacon.Interact(player.Inventory, player.Mission), "Beacon activation failed at valid mission state");
        Require(!beacon.GetNode<Node3D>("ClosedVisual").Visible, "Closed beacon visual stayed visible after activation");
        Require(beacon.GetNode<Node3D>("ActiveVisual").Visible, "Active beacon visual did not appear after activation");
        main.QueueFree();
        await Frames(2);
    }

    private async System.Threading.Tasks.Task TestSaveLoadFlow()
    {
        LocalSaveGameStore.DeleteSave();
        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        await Frames(2);
        var player = main.GetNode<FirstPersonController>("Player");
        var saveCoordinator = main.GetNode<CrashSiteSaveCoordinator>("SaveCoordinator");
        var pauseMenu = main.GetNode<PauseMenu>("PauseMenu");
        player.GlobalPosition = new Vector3(3.0f, 2.0f, -7.0f);
        player.Health.ApplyDamage(40);
        player.Inventory.AddResources(metal: 6, biomass: 1, electronicComponents: 2);
        player.Mission.TryCompleteResourceCollection();
        pauseMenu.GetNode<Button>("Panel/Menu/SaveButton").EmitSignal(Button.SignalName.Pressed);
        await Frames(2);
        Require(saveCoordinator.LastSaveSucceeded, "Pause Save did not write a save file");
        Require(LocalSaveGameStore.SaveExists(), "Save file does not exist after pause Save");
        main.QueueFree();
        await Frames(2);

        var loadedMain = LoadScene<Node3D>(MainScenePath);
        AddChild(loadedMain);
        await Frames(2);
        var loadedPlayer = loadedMain.GetNode<FirstPersonController>("Player");
        var loadedCoordinator = loadedMain.GetNode<CrashSiteSaveCoordinator>("SaveCoordinator");
        Require(loadedCoordinator.LastLoadSucceeded, "Continue load did not restore an existing save");
        Require(loadedPlayer.Health.CurrentHealth == 60, "Loaded health mismatch");
        Require(loadedPlayer.Inventory.Metal == 6 && loadedPlayer.Inventory.ElectronicComponents == 2, "Loaded inventory mismatch");
        Require(loadedPlayer.Mission.CurrentStep == Missions.CrashSiteMissionStep.BuildMechanicalArm, "Loaded mission step mismatch");
        Require(HorizontalDistance(loadedPlayer.GlobalPosition, new Vector3(3.0f, 2.0f, -7.0f)) < 0.1f, "Loaded player position mismatch");
        loadedMain.QueueFree();
        await Frames(2);
        LocalSaveGameStore.DeleteSave();
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

    private static void RequireHasPhysicalKey(string action, Key key)
    {
        foreach (var inputEvent in InputMap.ActionGetEvents(action))
            if (inputEvent is InputEventKey eventKey && eventKey.PhysicalKeycode == key)
                return;
        throw new InvalidOperationException($"{action} is not mapped to physical key {key}");
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
