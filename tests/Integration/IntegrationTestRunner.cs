using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            TestLocalSaveGameStoreLoadStates();
            await TestSaveLoadFlow();
            await TestFullMissionPlaythrough();
            await TestDefeatedScoutPersistenceAcrossReload();
            await TestFallingOutOfBoundsLeadsToDefeatFlow();
            await TestBeaconVisualState();
            await TestRuntimeSceneContracts();
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
        Require(main.GetNode<Workbench>("Placeholder_Workbench") is not null, "Workbench missing");
        var beacon = main.GetNode<Beacon>("Placeholder_Beacon");
        Require(beacon.GetNode<Node3D>("ClosedVisual").Visible, "Closed beacon visual missing");
        Require(!beacon.GetNode<Node3D>("ActiveVisual").Visible, "Active beacon visual should start hidden");
        Require(main.GetNode<Area3D>("ResourceDrop_MetalPickup") is not null, "Metal pickup missing");
        Require(main.GetNode<Area3D>("ResourceDrop_BiomassPickup") is not null, "Biomass pickup missing");
        Require(main.GetNode<Area3D>("ResourceDrop_ElectronicsPickup") is not null, "Electronics pickup missing");
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
        var staticCollisionNodes = new List<CollisionShape3D>();
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
                staticCollisionNodes.Add(collision);
            }
        }

        Require(staticCollisionCount <= MaxStaticCollisionShapes, $"Static collision budget exceeded: {staticCollisionCount}");
        Require(main.GetNode<CollisionShape3D>("Ground/Collision_Ground").Shape is BoxShape3D, "Ground must use a BoxShape3D");
        Require(main.GetNode<CollisionShape3D>("C7_Wall_1/Collision_C7Wall").Shape is BoxShape3D, "C7 wall 1 collision missing");
        Require(main.GetNode<CollisionShape3D>("C7_Wall_2/Collision_C7Wall").Shape is BoxShape3D, "C7 wall 2 collision missing");
        Require(main.GetNode<CollisionShape3D>("C7_Wall_3/Collision_C7Wall").Shape is BoxShape3D, "C7 wall 3 collision missing");
        Require(main.GetNode<CollisionShape3D>("C7_Wall_4/Collision_C7Wall").Shape is BoxShape3D, "C7 wall 4 collision missing");
        Require(main.GetNode<CollisionShape3D>("Placeholder_Workbench/CollisionShape3D").Shape is BoxShape3D, "Workbench interaction collision missing");
        Require(main.GetNode<CollisionShape3D>("Placeholder_Beacon/CollisionShape3D").Shape is BoxShape3D, "Beacon interaction collision missing");

        foreach (var prefix in ForbiddenCollisionPrefixes)
            foreach (var node in main.FindChildren($"{prefix}*", "CollisionObject3D", true, false))
                Require(false, $"Forbidden decorative collision object: {node.GetPath()}");

        foreach (var pickupName in new[] { "ResourceDrop_MetalPickup", "ResourceDrop_BiomassPickup", "ResourceDrop_ElectronicsPickup" })
        {
            var pickup = main.GetNode<Node3D>(pickupName);
            foreach (var collision in staticCollisionNodes)
            {
                if (IsDescendantOf(collision, pickup))
                    continue;
                Require(HorizontalDistance(pickup.GlobalPosition, collision.GlobalPosition) > 1.2f, $"{pickupName} overlaps a static collision");
            }
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
        await Frames(2);
        Require(scout.GetNode<CollisionShape3D>("CollisionShape3D").Disabled, "Dead Galaxabrain Scout collider stayed enabled");

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

        foreach (var wallName in new[] { "C7_Wall_1", "C7_Wall_2", "C7_Wall_3", "C7_Wall_4" })
        {
            var wall = main.GetNode<Node3D>(wallName);
            Require(wall.GetNode<MeshInstance3D>("MeshInstance3D") is not null, $"{wallName} legacy gameplay wall mesh missing");
            Require(wall.GetNode<CollisionShape3D>("Collision_C7Wall") is not null, $"{wallName} gameplay collision missing");
        }

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
        Require(startTutorial.Text.Contains("craft at workbench"), "HUD start tutorial missing craft guidance");
        Require(startTutorial.Text.Contains("Left click"), "HUD start tutorial missing attack control");
        Require(startTutorial.Text.Contains("Mk I"), "HUD start tutorial missing built-arm attack gating");
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

        player.TryAttack();
        await Frames(2);
        Require(hud.GetNode<Label>("ActionFeedback").Text.Contains("Mk I is not built"), "HUD attack feedback should explain blocked unbuilt-arm attacks");

        player.Health.ApplyDamage(25);
        player.Inventory.AddResources(metal: 4, biomass: 2, electronicComponents: 1);
        player.Inventory.MarkMechanicalArmBuilt();
        player.Mission.TryCompleteResourceCollection();
        await Frames(2);

        Require(hud.GetNode<Label>("Panel/Margin/VBox/Health").Text == "Health: 75/100", "HUD health did not update from player health");
        Require(hud.GetNode<Label>("Panel/Margin/VBox/Resources").Text.Contains("Metal: 4"), "HUD resources did not update from inventory");
        Require(hud.GetNode<Label>("ActionFeedback").Text.Contains("online"), "HUD action feedback kept the stale not-built hint after the arm was built");
        Require(hud.GetNode<Label>("Panel/Margin/VBox/MechanicalArmState").Text.Contains("Left click"), "HUD arm state did not explain attack input after crafting");
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

    private static void TestLocalSaveGameStoreLoadStates()
    {
        const string testSavePath = "user://local_store_states_test.json";
        LocalSaveGameStore.DeleteSave(testSavePath);
        Require(!LocalSaveGameStore.TryLoad(out _, testSavePath), "Missing save data should not load");

        var validSaveData = new CrashSiteSaveData
        {
            CheckpointId = "crash_site_save_point",
            PlayerX = 3.0f,
            PlayerY = 2.0f,
            PlayerZ = -7.0f,
            Health = 60,
            Metal = 6,
            Biomass = 1,
            ElectronicComponents = 2,
            MissionStep = CrashSiteMissionStep.BuildMechanicalArm,
        };
        LocalSaveGameStore.Save(validSaveData, testSavePath);
        Require(LocalSaveGameStore.TryLoad(out var loadedSaveData, testSavePath), "Valid save data should load");
        Require(loadedSaveData.CheckpointId == "crash_site_save_point", "Valid save checkpoint id mismatch");
        Require(loadedSaveData.Health == 60, "Valid save health mismatch");

        using (var file = Godot.FileAccess.Open(testSavePath, Godot.FileAccess.ModeFlags.Write))
            file?.StoreString("{ invalid json");
        Require(!LocalSaveGameStore.TryLoad(out _, testSavePath), "Invalid save data should fail clearly instead of loading");
        LocalSaveGameStore.DeleteSave(testSavePath);
    }

    private async System.Threading.Tasks.Task TestSaveLoadFlow()
    {
        LocalSaveGameStore.DeleteSave();
        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        await Frames(2);
        var player = main.GetNode<FirstPersonController>("Player");
        var saveCoordinator = main.GetNode<CrashSiteSaveCoordinator>("SaveCoordinator");
        var savePoint = main.GetNode<SavePoint>("Placeholder_SavePoint");
        player.GlobalPosition = new Vector3(3.0f, 2.0f, -7.0f);
        player.Health.ApplyDamage(40);
        player.Inventory.AddResources(metal: 6, biomass: 1, electronicComponents: 2);
        player.Mission.TryCompleteResourceCollection();
        var saveFeedback = string.Empty;
        player.ActionFeedbackChanged += message => saveFeedback = message;
        Require(savePoint.Interact(player.Inventory, player.Mission), "SavePoint interaction failed");
        await Frames(2);
        Require(savePoint.HasSavedCheckpoint, "SavePoint did not mark the checkpoint as saved");
        Require(saveCoordinator.LastSaveSucceeded, "SavePoint interaction did not write a save file");
        Require(saveFeedback == string.Empty, "Direct SavePoint persistence should not bypass the player HUD feedback path");
        Require(LocalSaveGameStore.SaveExists(), "Save file does not exist after SavePoint interaction");
        Require(LocalSaveGameStore.TryLoad(out var checkpointSave), "SavePoint save data could not be reloaded");
        Require(checkpointSave.CheckpointId == savePoint.CheckpointId, "SavePoint did not persist the checkpoint reload target");
        main.QueueFree();
        await Frames(2);

        var loadedMain = LoadScene<Node3D>(MainScenePath);
        AddChild(loadedMain);
        await Frames(2);
        var loadedPlayer = loadedMain.GetNode<FirstPersonController>("Player");
        var loadedCoordinator = loadedMain.GetNode<CrashSiteSaveCoordinator>("SaveCoordinator");
        Require(loadedCoordinator.LastLoadSucceeded, "Continue load did not restore an existing checkpoint save");
        Require(loadedPlayer.Health.CurrentHealth == 60, "Loaded health mismatch");
        Require(loadedPlayer.Inventory.Metal == 6 && loadedPlayer.Inventory.ElectronicComponents == 2, "Loaded inventory mismatch");
        Require(loadedPlayer.Mission.CurrentStep == Missions.CrashSiteMissionStep.BuildMechanicalArm, "Loaded mission step mismatch");
        Require(HorizontalDistance(loadedPlayer.GlobalPosition, new Vector3(3.0f, 2.0f, -7.0f)) < 0.1f, "Loaded player position mismatch");
        loadedMain.QueueFree();
        await Frames(2);
        LocalSaveGameStore.DeleteSave();
    }

    private async System.Threading.Tasks.Task TestFullMissionPlaythrough()
    {
        LocalSaveGameStore.DeleteSave();
        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        await Frames(2);

        var player = main.GetNode<FirstPersonController>("Player");
        var navigator = main.GetNode<CrashSiteEndScreenNavigator>("EndScreenNavigator");
        navigator.EnableSceneChanges = false;
        var arm = player.GetNode<MeshInstance3D>("Head/Camera3D/MechanicalArmVisual");
        var scout = main.GetNode<GalaxabrainScout>("Placeholder_GalaxabrainScout");
        var component = scout.GetNode<GalaxabrainComponentPickup>("GalaxabrainComponentPickup");
        var workbench = main.GetNode<Workbench>("Placeholder_Workbench");
        var savePoint = main.GetNode<SavePoint>("Placeholder_SavePoint");
        var beacon = main.GetNode<Beacon>("Placeholder_Beacon");
        var hud = main.GetNode<CrashSiteHud>("HUD");
        var lastActionFeedback = string.Empty;
        player.ActionFeedbackChanged += message => lastActionFeedback = message;

        Require(player.IsInsideTree(), "Player did not spawn into the Crash Site scene");
        Require(!arm.Visible, "Mechanical arm visual should start hidden");
        LogMvpSmokeMilestone(1, "player spawned");
        Require(!workbench.Interact(player.Inventory, player.Mission), "Workbench must reject crafting before resources are collected");
        Require(!component.Interact(player.Inventory, player.Mission), "Component must reject recovery before Galaxabrain defeat");
        Require(!beacon.Interact(player.Inventory, player.Mission), "Beacon must reject activation before component recovery");
        Require(player.Mission.CurrentStep == CrashSiteMissionStep.CollectResources, "Invalid early interactions mutated mission state");

        foreach (var pickupName in new[] { "ResourceDrop_MetalPickup", "ResourceDrop_BiomassPickup", "ResourceDrop_ElectronicsPickup" })
        {
            var pickup = main.GetNode<ResourceDrop>(pickupName);
            Require(pickup.Interact(player.Inventory, player.Mission), $"{pickupName} could not be collected");
            Require(!pickup.Interact(player.Inventory, player.Mission), $"{pickupName} was collectable twice");
        }
        Require(player.Mission.CurrentStep == CrashSiteMissionStep.BuildMechanicalArm, "Collecting all resources did not advance to crafting");
        LogMvpSmokeMilestone(2, "resources collected");

        Require(workbench.Interact(player.Inventory, player.Mission), "Workbench crafting failed with full resources");
        Require(player.Inventory.IsMechanicalArmBuilt, "Crafting did not build the mechanical arm");
        Require(player.Mission.CurrentStep == CrashSiteMissionStep.DefeatGalaxabrain, "Crafting did not advance to the defeat step");
        Require(arm.Visible, "Mechanical arm visual did not appear after crafting");
        LogMvpSmokeMilestone(3, "Mechanical Arm Mk I crafted");
        Require(!workbench.Interact(player.Inventory, player.Mission), "Workbench crafted the arm twice");
        Require(!component.Interact(player.Inventory, player.Mission), "Component was recoverable while the Galaxabrain is alive");

        // First hit goes through the real input path: aim the camera at the scout
        // and raycast-attack, proving the fight works end to end, not just ApplyDamage.
        player.GlobalPosition = scout.GlobalPosition + new Vector3(2.0f, 0.0f, 0.0f);
        await Frames(2);
        player.GetNode<Node3D>("Head").LookAt(scout.GlobalPosition, Vector3.Up);
        Require(player.TryAttack(), "Aimed mechanical arm attack did not hit the Galaxabrain");
        Require(scout.Brain.CurrentHealth == scout.Brain.MaxHealth - MechanicalArmAttackLogic.DefaultMechanicalArmDamage, "Aimed attack did not damage the Galaxabrain");
        LogMvpSmokeMilestone(4, "Galaxabrain Scout engaged");

        for (var hit = 0; hit < 3; hit++)
            scout.ApplyDamage(MechanicalArmAttackLogic.DefaultMechanicalArmDamage);
        Require(scout.Brain.IsDead, "Galaxabrain survived four arm hits");
        Require(player.Mission.CurrentStep == CrashSiteMissionStep.RecoverGalaxabrainComponent, "Galaxabrain death did not advance to component recovery");
        Require(!player.Mission.IsVictory && player.Mission.CurrentStep != CrashSiteMissionStep.ActivateBeacon, "Galaxabrain death skipped the component recovery step");
        Require(component.Visible && component.Monitoring, "Component pickup was not revealed by Galaxabrain death");
        LogMvpSmokeMilestone(5, "Galaxabrain Scout defeated");
        Require(!beacon.Interact(player.Inventory, player.Mission), "Beacon activated before component recovery");

        // Recover through the real interaction raycast: the dead scout's collider must
        // not eat the ray, or the recovery objective soft-locks (Codex review finding).
        await Frames(2);
        Require(scout.GetNode<CollisionShape3D>("CollisionShape3D").Disabled, "Dead Galaxabrain body collider stayed enabled");
        player.GlobalPosition = component.GlobalPosition + new Vector3(1.5f, 0.0f, 0.0f);
        await Frames(2);
        player.GetNode<Node3D>("Head").LookAt(component.GlobalPosition, Vector3.Up);
        Require(player.TryInteract(), "Interaction raycast could not reach the revealed component pickup");
        Require(player.Inventory.HasGalaxabrainComponent, "Component recovery did not update inventory");
        Require(player.Mission.CurrentStep == CrashSiteMissionStep.ActivateBeacon, "Component recovery did not advance to beacon activation");
        Require(!player.Mission.IsVictory, "Component recovery skipped required beacon activation");
        Require(lastActionFeedback == FirstPersonController.GalaxabrainComponentRecoveryFeedback, "Component recovery did not emit beacon activation action feedback");
        Require(lastActionFeedback.Contains("component recovered") && lastActionFeedback.Contains("activate the beacon beam"), "Component recovery action feedback did not explain the next beacon step");
        Require(!component.Visible && !component.Monitoring, "Component pickup stayed interactable after recovery");
        LogMvpSmokeMilestone(6, "component retrieved");
        Require(!component.Interact(player.Inventory, player.Mission), "Component was recoverable twice");

        player.GlobalPosition = savePoint.GlobalPosition + new Vector3(1.5f, 0.0f, 0.0f);
        await Frames(2);
        player.GetNode<Node3D>("Head").LookAt(savePoint.GlobalPosition, Vector3.Up);
        Require(player.TryInteract(), "Player interaction raycast could not use the save point before beacon activation");
        await Frames(2);
        Require(savePoint.HasSavedCheckpoint, "Save point did not record checkpoint usage");
        Require(main.GetNode<CrashSiteSaveCoordinator>("SaveCoordinator").LastSaveSucceeded, "Save point did not write the continuation save");
        Require(lastActionFeedback == FirstPersonController.SavePointSuccessFeedback, "Save point did not emit player-facing checkpoint feedback");
        Require(lastActionFeedback.Contains("Checkpoint saved") && lastActionFeedback.Contains("continue to the beacon"), "Save point feedback did not confirm the save and next objective");
        Require(hud.GetNode<Label>("ActionFeedback").Text == FirstPersonController.SavePointSuccessFeedback, "HUD did not show the checkpoint save feedback text");
        LogMvpSmokeMilestone(7, "save point used");

        Require(beacon.Interact(player.Inventory, player.Mission), "Beacon activation failed with recovered component");
        Require(player.Mission.IsVictory, "Beacon activation did not produce victory");
        LogMvpSmokeMilestone(8, "beacon activated");
        Require(!beacon.Interact(player.Inventory, player.Mission), "Beacon activated twice");
        await Frames(2);
        Require(navigator.LastRequestedScenePath == "res://scenes/UI/VictoryScreen.tscn", "Victory screen was not requested after beacon activation");
        LogMvpSmokeMilestone(9, "victory reached");

        main.QueueFree();
        await Frames(2);

        var defeatMain = LoadScene<Node3D>(MainScenePath);
        AddChild(defeatMain);
        await Frames(2);
        var defeatedPlayer = defeatMain.GetNode<FirstPersonController>("Player");
        var defeatNavigator = defeatMain.GetNode<CrashSiteEndScreenNavigator>("EndScreenNavigator");
        defeatNavigator.EnableSceneChanges = false;
        defeatedPlayer.Health.ApplyDamage(PlayerHealth.DefaultMaxHealth);
        await Frames(2);
        Require(defeatedPlayer.Health.IsDead, "Defeat path did not kill the player");
        Require(defeatNavigator.LastRequestedScenePath == "res://scenes/UI/DefeatScreen.tscn", "Defeat path did not request the defeat screen");
        LogMvpSmokeMilestone(10, "defeat path verified");
        defeatMain.QueueFree();
        await Frames(2);

        var continuation = LoadScene<Node3D>(MainScenePath);
        AddChild(continuation);
        await Frames(2);
        var continuationPlayer = continuation.GetNode<FirstPersonController>("Player");
        Require(continuation.GetNode<CrashSiteSaveCoordinator>("SaveCoordinator").LastLoadSucceeded, "Save continuation did not load the checkpoint");
        Require(continuationPlayer.Mission.CurrentStep == CrashSiteMissionStep.ActivateBeacon, "Save continuation did not restore beacon objective");
        Require(continuationPlayer.Inventory.IsMechanicalArmBuilt && continuationPlayer.Inventory.HasGalaxabrainComponent, "Save continuation did not restore MVP inventory state");
        LogMvpSmokeMilestone(11, "save continuation verified");
        continuation.QueueFree();
        await Frames(2);
        LocalSaveGameStore.DeleteSave();
    }

    private static void LogMvpSmokeMilestone(int number, string milestone)
    {
        GD.Print($"MVP_SMOKE_MILESTONE_{number:00}: {milestone}");
    }

    private async System.Threading.Tasks.Task TestDefeatedScoutPersistenceAcrossReload()
    {
        LocalSaveGameStore.DeleteSave();
        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        await Frames(2);
        var player = main.GetNode<FirstPersonController>("Player");
        main.GetNode<CrashSiteEndScreenNavigator>("EndScreenNavigator").EnableSceneChanges = false;
        var scout = main.GetNode<GalaxabrainScout>("Placeholder_GalaxabrainScout");
        player.Inventory.AddResources(metal: 10, biomass: 3, electronicComponents: 2);
        player.Mission.TryCompleteResourceCollection();
        player.Inventory.TrySpendResources(metal: 10, biomass: 3, electronicComponents: 2);
        player.Inventory.MarkMechanicalArmBuilt();
        player.Mission.TryCompleteMechanicalArmConstruction();
        for (var hit = 0; hit < 4; hit++)
            scout.ApplyDamage(MechanicalArmAttackLogic.DefaultMechanicalArmDamage);
        Require(player.Mission.CurrentStep == CrashSiteMissionStep.RecoverGalaxabrainComponent, "Setup: defeat step not reached");
        Require(main.GetNode<SavePoint>("Placeholder_SavePoint").Interact(player.Inventory, player.Mission), "Setup: save failed after defeat");
        main.QueueFree();
        await Frames(2);

        var reloaded = LoadScene<Node3D>(MainScenePath);
        AddChild(reloaded);
        await Frames(2);
        var reloadedPlayer = reloaded.GetNode<FirstPersonController>("Player");
        var reloadedScout = reloaded.GetNode<GalaxabrainScout>("Placeholder_GalaxabrainScout");
        var reloadedComponent = reloadedScout.GetNode<GalaxabrainComponentPickup>("GalaxabrainComponentPickup");
        Require(reloaded.GetNode<CrashSiteSaveCoordinator>("SaveCoordinator").LastLoadSucceeded, "Reload did not restore the defeat-state save");
        Require(reloadedScout.Brain.IsDead, "Defeated Galaxabrain came back to life after reload");
        Require(!reloadedScout.Visible, "Defeated Galaxabrain became visible again after reload");
        Require(reloadedScout.GetNode<CollisionShape3D>("CollisionShape3D").Disabled, "Restored dead Galaxabrain body collider stayed enabled");
        Require(reloadedComponent.Visible && reloadedComponent.Monitoring, "Unrecovered component was not available after reload");
        Require(reloadedPlayer.Inventory.IsMechanicalArmBuilt, "Mechanical arm ownership was lost across reload");
        Require(reloadedPlayer.GetNode<MeshInstance3D>("Head/Camera3D/MechanicalArmVisual").Visible, "Mechanical arm visual was not restored after reload");
        Require(reloadedPlayer.Mission.CurrentStep == CrashSiteMissionStep.RecoverGalaxabrainComponent, "Mission step was not restored");

        Require(reloadedComponent.Interact(reloadedPlayer.Inventory, reloadedPlayer.Mission), "Component recovery failed after reload");
        Require(reloaded.GetNode<SavePoint>("Placeholder_SavePoint").Interact(reloadedPlayer.Inventory, reloadedPlayer.Mission), "Second checkpoint save failed");
        reloaded.QueueFree();
        await Frames(2);

        var finalRun = LoadScene<Node3D>(MainScenePath);
        AddChild(finalRun);
        await Frames(2);
        var finalPlayer = finalRun.GetNode<FirstPersonController>("Player");
        var finalNavigator = finalRun.GetNode<CrashSiteEndScreenNavigator>("EndScreenNavigator");
        finalNavigator.EnableSceneChanges = false;
        var finalScout = finalRun.GetNode<GalaxabrainScout>("Placeholder_GalaxabrainScout");
        var finalComponent = finalScout.GetNode<GalaxabrainComponentPickup>("GalaxabrainComponentPickup");
        var finalBeacon = finalRun.GetNode<Beacon>("Placeholder_Beacon");
        Require(finalScout.Brain.IsDead && !finalScout.Visible, "Defeated Galaxabrain revived on the second reload");
        Require(!finalComponent.Visible && !finalComponent.Monitoring, "Recovered component became interactable again after reload");
        Require(!finalComponent.Interact(finalPlayer.Inventory, finalPlayer.Mission), "Recovered component could be collected twice across reloads");
        Require(finalPlayer.Inventory.HasGalaxabrainComponent, "Recovered component was lost across reload");
        Require(!finalBeacon.IsActivated, "Beacon restored as active before activation");
        Require(finalPlayer.Mission.CurrentStep == CrashSiteMissionStep.ActivateBeacon, "Mission step after recovery was not restored");
        Require(finalBeacon.Interact(finalPlayer.Inventory, finalPlayer.Mission), "Beacon activation failed after reload");
        Require(finalPlayer.Mission.IsVictory, "Victory was not reached after reloads");
        await Frames(2);
        Require(finalNavigator.LastRequestedScenePath == "res://scenes/UI/VictoryScreen.tscn", "Victory screen was not requested after reloaded beacon activation");
        finalRun.QueueFree();
        await Frames(2);
        LocalSaveGameStore.DeleteSave();
    }

    private async System.Threading.Tasks.Task TestFallingOutOfBoundsLeadsToDefeatFlow()
    {
        LocalSaveGameStore.DeleteSave();
        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        await Frames(2);
        var player = main.GetNode<FirstPersonController>("Player");
        var navigator = main.GetNode<CrashSiteEndScreenNavigator>("EndScreenNavigator");
        navigator.EnableSceneChanges = false;

        // The ground slab is 150x150 with no perimeter containment; leaving it must
        // route through the standard death flow instead of an endless fall.
        player.GlobalPosition = new Vector3(80.0f, 2.0f, 0.0f);
        await Frames(240);

        Require(player.Health.IsDead, "Falling out of bounds did not kill the player");
        Require(navigator.LastRequestedScenePath == "res://scenes/UI/DefeatScreen.tscn", "Out-of-bounds fall did not request the defeat screen");
        main.QueueFree();
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

    private async System.Threading.Tasks.Task TestRuntimeSceneContracts()
    {
        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        await Frames(30);

        var player = main.GetNode<FirstPersonController>("Player");
        var camera = player.GetNode<Camera3D>("Head/Camera3D");
        var scout = main.GetNode<GalaxabrainScout>("Placeholder_GalaxabrainScout");
        var arm = player.GetNode<MeshInstance3D>("Head/Camera3D/MechanicalArmVisual");
        var visual = scout.GetNode<MeshInstance3D>("AuthenticatedRobotGalaxabrainVisual");
        var initialPlayerPosition = player.GlobalPosition;
        var initialScoutPosition = scout.GlobalPosition;
        var initialScoutVisualPosition = visual.GlobalPosition;

        Require(camera.Current, "Active camera is not current");
        Require(IsDescendantOf(camera, player), "Active camera does not belong to Player");
        Require(!IsDescendantOf(scout, player) && !IsDescendantOf(scout, camera), "Galaxabrain is camera/player-relative");
        Require(HorizontalDistance(initialPlayerPosition, initialScoutPosition) > 8.0f, "Galaxabrain spawned too close to player");
        Require(scout.GetNode<CollisionShape3D>("CollisionShape3D").Disabled == false, "Galaxabrain collision disabled");
        var scoutCapsule = scout.GetNode<CollisionShape3D>("CollisionShape3D").Shape as CapsuleShape3D;
        Require(scoutCapsule is not null, "Galaxabrain collision capsule missing");

        var before = scout.GlobalPosition;
        scout.PlayerPath = scout.GetPathTo(player);
        player.GlobalPosition = scout.GlobalPosition + new Vector3(6.0f, 0.0f, 0.0f);
        await Frames(90);
        Require(HorizontalDistance(before, scout.GlobalPosition) > 0.1f, "Galaxabrain did not move during controlled simulation");

        var visualSize = visual.GetAabb().Size * visual.Scale;
        Require(visualSize.Y / scoutCapsule!.Height < 4.0f, "Galaxabrain visual/collision height mismatch");
        Require(IsDescendantOf(arm, camera), "Mechanical arm is not under active camera");
        Require(arm.FindChildren("*", "CollisionObject3D", true, false).Count == 0, "Mechanical arm has collision descendants");
        Require(!IsUnsuitableFirstPersonArmActive(arm), "Complete George mech is active as first-person arm view model");

        foreach (var path in new[] { "Ground", "AuthenticatedCrashSiteVisuals", "ResourceDrop_MetalPickup", "Placeholder_Workbench", "Placeholder_SavePoint", "Placeholder_Beacon" })
        {
            var node = main.GetNode<Node>(path);
            Require(!IsDescendantOf(node, camera), $"{path} is under camera hierarchy");
        }

        TestProceduralTerrainContracts(main, camera);
        var flags = BuildRuntimeContractFlags(main, player, camera, scout, arm, initialPlayerPosition, initialScoutPosition, visual, scoutCapsule);
        Require(flags.Count == 0, $"Runtime contract flags present: {string.Join(", ", flags)}");

        foreach (var pickupName in new[] { "ResourceDrop_MetalPickup", "ResourceDrop_BiomassPickup", "ResourceDrop_ElectronicsPickup" })
        {
            var pickup = main.GetNode<ResourceDrop>(pickupName);
            Require(pickup.FindChildren("*", "CollisionShape3D", true, false).Count > 0, $"{pickupName} collision missing");
        }

        Require(main.GetNodeOrNull<MeshInstance3D>("ResourceDrop_MetalPickup/VisualGroup/HighlightRing") is not null, "Metal pickup readability ring missing");
        Require(main.GetNodeOrNull<MeshInstance3D>("ResourceDrop_BiomassPickup/VisualGroup/HighlightRing") is not null, "Biomass pickup readability ring missing");
        Require(main.GetNodeOrNull<MeshInstance3D>("ResourceDrop_ElectronicsPickup/VisualGroup/HighlightRing") is not null, "Electronics pickup readability ring missing");
        Require(main.GetNodeOrNull<MeshInstance3D>("Placeholder_Workbench/VisualBase/ControlPanel") is not null, "Workbench readable control panel missing");
        Require(main.GetNodeOrNull<MeshInstance3D>("Placeholder_SavePoint/SavePointReadabilityCore") is not null, "Save point readability core missing");
        Require(main.GetNodeOrNull<MeshInstance3D>("Placeholder_Beacon/ActiveVisual") is not null, "Beacon active visual missing");

        Require(main.GetNode<Workbench>("Placeholder_Workbench").GetNode<CollisionShape3D>("CollisionShape3D").Shape is not null, "Workbench behavior/collision missing");
        Require(main.GetNode<SavePoint>("Placeholder_SavePoint").GetNode<CollisionShape3D>("Collision_SavePoint").Shape is not null, "Save point behavior/collision missing");
        Require(main.GetNode<Beacon>("Placeholder_Beacon").GetNode<CollisionShape3D>("CollisionShape3D").Shape is not null, "Beacon behavior/collision missing");

        WriteRuntimeContractReport(main, initialPlayerPosition, initialScoutPosition, initialScoutVisualPosition, camera, scout, visual, scoutCapsule, arm, flags);
        main.QueueFree();
        await Frames(2);
    }

    private static bool IsUnsuitableFirstPersonArmActive(MeshInstance3D arm)
    {
        return arm.Visible
            && arm.Mesh?.ResourcePath?.Contains("George.obj", StringComparison.OrdinalIgnoreCase) == true
            && arm.GetAabb().Size.Y > 4.0f;
    }

    private static void TestProceduralTerrainContracts(Node3D main, Camera3D camera)
    {
        Require(main.GetNodeOrNull<Node3D>("AuthenticatedTerrainVisuals") is null, "Failed Pass 1 terrain visuals are still present in production Main.tscn");
        var terrain = main.GetNodeOrNull<Node3D>("ProceduralCrashSiteTerrain");
        Require(terrain is not null, "Procedural terrain is missing");
        Require(!IsDescendantOf(terrain!, camera), "Procedural terrain entered camera hierarchy");
        Require(terrain!.FindChildren("*", "CollisionObject3D", true, false).Count == 0, "Procedural terrain must not own collision");
        Require(terrain.GetNode<MeshInstance3D>("TerrainMesh").Mesh is ArrayMesh, "Procedural terrain legacy diagnostic mesh missing");
        foreach (string semanticName in ProceduralCrashSiteTerrain.RequiredSemanticMeshes)
            Require(terrain.GetNodeOrNull<Node>(semanticName) is not null, $"Semantic terrain mesh missing: {semanticName}");
        foreach (string meshName in ProceduralCrashSiteTerrain.RequiredSemanticMeshes.Where(name => name != "HorizonSegments"))
        {
            var semantic = terrain.GetNode<MeshInstance3D>(meshName);
            Require(!IsDescendantOf(semantic, camera), $"{meshName} entered camera hierarchy");
            Require(semantic.FindChildren("*", "CollisionObject3D", true, false).Count == 0, $"{meshName} must not own collision");
            Require(MeshVertexCount(semantic.Mesh) > 0, $"{meshName} mesh has no vertices");
        }
        Require(terrain.GetNode<Node3D>("HorizonSegments").FindChildren("HorizonSegment_*", "MeshInstance3D", true, false).Count >= 5, "Semantic horizon requires at least five segments");

        foreach (var mesh in main.FindChildren("*", "MeshInstance3D", true, false))
        {
            var meshInstance = (MeshInstance3D)mesh;
            var resourcePath = meshInstance.Mesh?.ResourcePath ?? string.Empty;
            Require(!resourcePath.Contains("rock_cliff_d.obj", StringComparison.OrdinalIgnoreCase)
                && !resourcePath.Contains("rock_irregular_c.obj", StringComparison.OrdinalIgnoreCase)
                && !resourcePath.Contains("rock_ridge_f.obj", StringComparison.OrdinalIgnoreCase)
                && !resourcePath.Contains("rock_spire_g.obj", StringComparison.OrdinalIgnoreCase), "Rejected terrain OBJ is used in production");
        }

        var report = ProceduralCrashSiteTerrain.BuildForWorld(main).Report;
        var repeat = ProceduralCrashSiteTerrain.BuildForWorld(main).Report;
        Require(report.MeshDataHash == repeat.MeshDataHash, "Procedural terrain mesh output is nondeterministic");
        Require(report.VertexCount > 0 && report.TriangleCount > 0, "Procedural terrain geometry is empty");
        Require(report.MaxHeight <= ProceduralCrashSiteTerrain.MaxOutsideHeight, "Procedural terrain exceeds max height");
        Require(report.MaxRouteHeightDeviation <= ProceduralCrashSiteTerrain.CorridorTolerance, "Procedural terrain route height deviation is too high");
        Require(report.EstimatedMaxSlope <= ProceduralCrashSiteTerrain.MaxSlope, "Procedural terrain slope exceeds limit");
        Require(report.RouteSurfaceArea >= ProceduralCrashSiteTerrain.MinimumRouteSurfaceArea, "Procedural terrain route surface is too small/readability disconnected");
        Require(report.BasaltShelfSurfaceArea > 100.0f, "Procedural terrain basalt shelves are missing");
        Require(report.Features.Count >= 10, "Procedural terrain directed feature map is incomplete");
        Require(report.ZoneTriangles.ContainsKey(TerrainZone.AshRoute) && report.ZoneTriangles[TerrainZone.AshRoute] > 0, "Procedural terrain ash route zone has no triangles");
        Require(report.ZoneTriangles.ContainsKey(TerrainZone.HorizonRidge) && report.ZoneTriangles[TerrainZone.HorizonRidge] > 0, "Procedural terrain horizon ridge zone has no triangles");
        Require(report.ZoneTriangles.ContainsKey(TerrainZone.WorkbenchRidge) && report.ZoneTriangles[TerrainZone.WorkbenchRidge] > 0, "Procedural terrain workbench ridge zone has no triangles");
        Require(report.ZoneTriangles.ContainsKey(TerrainZone.CombatRidge) && report.ZoneTriangles[TerrainZone.CombatRidge] > 0, "Procedural terrain combat ridge zone has no triangles");
        Require(report.ZoneTriangles.ContainsKey(TerrainZone.ImpactCrater) && report.ZoneTriangles[TerrainZone.ImpactCrater] > 0, "Procedural terrain impact crater zone has no triangles");
        Color routeColor = ProceduralCrashSiteTerrain.ColorForZone(TerrainZone.AshRoute);
        Color plateauColor = ProceduralCrashSiteTerrain.ColorForZone(TerrainZone.CentralPlateau);
        Color ridgeColor = ProceduralCrashSiteTerrain.ColorForZone(TerrainZone.CombatRidge);
        Require(ColorDistance(routeColor, plateauColor) > 0.15f && ColorDistance(routeColor, ridgeColor) > 0.20f, "Representative procedural terrain zones do not have materially different vertex colours");
        Require(Luminance(routeColor) > Luminance(plateauColor) + 0.07f, "Procedural terrain route colour is not brighter than surrounding basalt");
        Require(MeshHeightRange(terrain.GetNode<MeshInstance3D>("RouteSurface").Mesh).Max <= ProceduralCrashSiteTerrain.CorridorHeight + ProceduralCrashSiteTerrain.CorridorTolerance, "Semantic route height deviates from stable ground");
        Require(MeshHeightRange(terrain.GetNode<MeshInstance3D>("SpawnBasaltShelf").Mesh).Range > 0.35f && MeshHeightRange(terrain.GetNode<MeshInstance3D>("ResourceBasaltShelf").Mesh).Range > 0.35f, "Semantic shelves have insufficient side faces");
        Require(MeshHeightRange(terrain.GetNode<MeshInstance3D>("WorkbenchRidge").Mesh).Max >= 1.4f && MeshHeightRange(terrain.GetNode<MeshInstance3D>("CombatRidge").Mesh).Max >= 1.6f, "Semantic ridges have insufficient crest height");
        Require(MeshHeightRange(terrain.GetNode<MeshInstance3D>("CraterNorthwest").Mesh).Range > 0.25f && MeshHeightRange(terrain.GetNode<MeshInstance3D>("CraterSoutheast").Mesh).Range > 0.25f, "Semantic craters do not contain rim/slope/basin height samples");
        Require(MeshHorizontalExtent(terrain.GetNode<MeshInstance3D>("CentralPlateau").Mesh).UniqueEdgeCount > 10, "Semantic plateau boundary is too rectangular/simple");
        Require(ProceduralCrashSiteTerrain.DistanceToRoute(33, -24, report.Targets) > ProceduralCrashSiteTerrain.CorridorWidth * 0.5f, "Semantic beacon shelf overlaps beacon route clearance");
        Directory.CreateDirectory("artifacts");
        File.WriteAllText("artifacts/terrain-generation-report.json", report.ToJson());

        foreach (var target in new[] { "Player", "ResourceDrop_MetalPickup", "ResourceDrop_BiomassPickup", "ResourceDrop_ElectronicsPickup", "Placeholder_Workbench", "Placeholder_SavePoint", "Placeholder_Beacon", "Placeholder_GalaxabrainScout", "Placeholder_GalaxabrainScout/GalaxabrainComponentPickup" })
        {
            var node = main.GetNode<Node3D>(target);
            Require(node.GlobalPosition.Y >= 0.0f, $"{target} moved below stable ground");
            Require(report.Contains(node.GlobalPosition), $"{target} is outside procedural terrain bounds");
            Require(ProceduralCrashSiteTerrain.DistanceToRoute(node.GlobalPosition.X, node.GlobalPosition.Z, report.Targets) <= ProceduralCrashSiteTerrain.CorridorWidth * 0.5f, $"{target} is outside safe corridor");
        }
    }

    private static float ColorDistance(Color a, Color b) => MathF.Sqrt(MathF.Pow(a.R - b.R, 2.0f) + MathF.Pow(a.G - b.G, 2.0f) + MathF.Pow(a.B - b.B, 2.0f));

    private static float Luminance(Color color) => color.R * 0.2126f + color.G * 0.7152f + color.B * 0.0722f;

    private static int MeshVertexCount(Mesh? mesh) => mesh is null ? 0 : ((Godot.Collections.Array)mesh.SurfaceGetArrays(0))[(int)Mesh.ArrayType.Vertex].As<Vector3[]>().Length;

    private static (float Min, float Max, float Range) MeshHeightRange(Mesh? mesh)
    {
        if (mesh is null)
            return (0, 0, 0);
        var vertices = ((Godot.Collections.Array)mesh.SurfaceGetArrays(0))[(int)Mesh.ArrayType.Vertex].As<Vector3[]>();
        float min = vertices.Min(v => v.Y);
        float max = vertices.Max(v => v.Y);
        return (min, max, max - min);
    }

    private static (int UniqueEdgeCount, float Width, float Depth) MeshHorizontalExtent(Mesh? mesh)
    {
        if (mesh is null)
            return (0, 0, 0);
        var vertices = ((Godot.Collections.Array)mesh.SurfaceGetArrays(0))[(int)Mesh.ArrayType.Vertex].As<Vector3[]>();
        var unique = vertices.Select(v => $"{MathF.Round(v.X, 1)},{MathF.Round(v.Z, 1)}").Distinct().Count();
        return (unique, vertices.Max(v => v.X) - vertices.Min(v => v.X), vertices.Max(v => v.Z) - vertices.Min(v => v.Z));
    }

    private static List<string> BuildRuntimeContractFlags(Node3D main, FirstPersonController player, Camera3D camera, GalaxabrainScout scout, MeshInstance3D arm, Vector3 playerSpawn, Vector3 scoutSpawn, MeshInstance3D scoutVisual, CapsuleShape3D scoutCapsule)
    {
        var flags = new List<string>();
        if (!camera.Current || !IsDescendantOf(camera, player) || IsDescendantOf(scout, player) || IsDescendantOf(scout, camera))
            flags.Add("CAMERA_PARENTING_ERROR");
        if (HorizontalDistance(playerSpawn, scoutSpawn) <= 8.0f)
            flags.Add("ENEMY_AT_PLAYER_POSITION");
        if (scoutVisual.GetAabb().Size.Y * scoutVisual.Scale.Y / scoutCapsule.Height >= 4.0f)
            flags.Add("VISUAL_COLLISION_MISMATCH");
        if (IsUnsuitableFirstPersonArmActive(arm))
            flags.Add("BAD_MODEL_ORIGIN");
        if (!IsDescendantOf(arm, camera))
            flags.Add("VIEWMODEL_IN_WORLD_LAYER");

        foreach (var path in new[] { "Ground", "AuthenticatedCrashSiteVisuals", "ResourceDrop_MetalPickup", "Placeholder_Workbench", "Placeholder_SavePoint", "Placeholder_Beacon" })
        {
            var node = main.GetNodeOrNull<Node>(path);
            if (node is null)
                flags.Add("REQUIRED_NODE_PATH_MISSING");
            else if (IsDescendantOf(node, camera))
                flags.Add("WORLD_MODEL_IN_VIEWMODEL_LAYER");
        }

        AddProceduralTerrainFlags(main, camera, flags);
        return flags;
    }

    private static void AddProceduralTerrainFlags(Node3D main, Camera3D camera, List<string> flags)
    {
        var terrain = main.GetNodeOrNull<Node3D>("ProceduralCrashSiteTerrain");
        if (terrain is null)
        {
            flags.Add("PROCEDURAL_TERRAIN_MISSING");
            return;
        }
        if (IsDescendantOf(terrain, camera))
            flags.Add("PROCEDURAL_TERRAIN_CAMERA_PARENTING");
        if (terrain.FindChildren("*", "CollisionObject3D", true, false).Count > 0)
            flags.Add("PROCEDURAL_TERRAIN_COLLISION_FOUND");
        foreach (string semanticName in ProceduralCrashSiteTerrain.RequiredSemanticMeshes)
            if (terrain.GetNodeOrNull<Node>(semanticName) is null)
                flags.Add("SEMANTIC_TERRAIN_MESH_MISSING");
        if (terrain.FindChildren("*", "CollisionObject3D", true, false).Count > 0)
            flags.Add("SEMANTIC_TERRAIN_COLLISION_FOUND");
        if (terrain.GetNodeOrNull<Node3D>("HorizonSegments")?.FindChildren("HorizonSegment_*", "MeshInstance3D", true, false).Count < 5)
            flags.Add("SEMANTIC_HORIZON_SEGMENTS_INVALID");
        var report = ProceduralCrashSiteTerrain.BuildForWorld(main).Report;
        var repeat = ProceduralCrashSiteTerrain.BuildForWorld(main).Report;
        if (report.MeshDataHash != repeat.MeshDataHash)
        {
            flags.Add("PROCEDURAL_TERRAIN_NONDETERMINISTIC");
            flags.Add("SEMANTIC_TERRAIN_NONDETERMINISTIC");
        }
        if (report.VertexCount <= 0 || report.TriangleCount <= 0)
            flags.Add("PROCEDURAL_TERRAIN_INVALID_GEOMETRY");
        if (report.MaxHeight > ProceduralCrashSiteTerrain.MaxOutsideHeight || report.EstimatedMaxSlope > ProceduralCrashSiteTerrain.MaxSlope)
            flags.Add("PROCEDURAL_TERRAIN_EXTREME_HEIGHT");
        if (report.MaxRouteHeightDeviation > ProceduralCrashSiteTerrain.CorridorTolerance)
            flags.Add("PROCEDURAL_TERRAIN_ROUTE_HEIGHT_ERROR");
        if (report.RouteSurfaceArea < ProceduralCrashSiteTerrain.MinimumRouteSurfaceArea || !report.ZoneTriangles.ContainsKey(TerrainZone.AshRoute) || report.ZoneTriangles[TerrainZone.AshRoute] <= 0)
        {
            flags.Add("PROCEDURAL_TERRAIN_ROUTE_DISCONNECTED");
            flags.Add("SEMANTIC_ROUTE_DISCONNECTED");
        }
        if (report.Features.Count < 10 || report.BasaltShelfSurfaceArea <= 100.0f)
            flags.Add("PROCEDURAL_TERRAIN_DIRECTED_ZONE_MISSING");
        foreach (var target in report.Targets.Values)
            if (!report.Contains(target))
            {
                flags.Add("PROCEDURAL_TERRAIN_TARGET_OUTSIDE_BOUNDS");
                flags.Add("SEMANTIC_ROUTE_TARGET_MISSED");
            }
    }

    private static bool IsDescendantOf(Node node, Node ancestor)
    {
        for (var current = node.GetParent(); current is not null; current = current.GetParent())
            if (current == ancestor)
                return true;
        return false;
    }

    private static void WriteRuntimeContractReport(Node3D main, Vector3 playerSpawn, Vector3 scoutSpawn, Vector3 scoutVisualSpawn, Camera3D camera, GalaxabrainScout scout, MeshInstance3D scoutVisual, CapsuleShape3D scoutCapsule, MeshInstance3D arm, List<string> flags)
    {
        var ship = main.GetNodeOrNull<MeshInstance3D>("AuthenticatedCrashSiteVisuals/CrashedShip_AuthenticUltimateSpaceKit");
        var scoutVisualRatio = scoutVisual.GetAabb().Size.Y * scoutVisual.Scale.Y / scoutCapsule.Height;
        var formattedFlags = string.Join(",", flags.ConvertAll(flag => $"\"{flag}\""));
        var report = $$"""
        {
          "activeCameraPath": "{{camera.GetPath()}}",
          "playerGlobalPosition": "{{playerSpawn}}",
          "galaxabrainGlobalPosition": "{{scoutSpawn}}",
          "playerEnemySpawnDistance": {{HorizontalDistance(playerSpawn, scoutSpawn):0.###}},
          "galaxabrainParentPath": "{{scout.GetParent().GetPath()}}",
          "galaxabrainVisualGlobalPosition": "{{scoutVisualSpawn}}",
          "galaxabrainVisualAabb": "{{scoutVisual.GetAabb()}}",
          "galaxabrainCollisionSize": "radius={{scoutCapsule.Radius}}, height={{scoutCapsule.Height}}",
          "galaxabrainVisualToCollisionHeightRatio": {{scoutVisualRatio:0.###}},
          "mechanicalArmParentPath": "{{arm.GetParent().GetPath()}}",
          "mechanicalArmVisible": {{arm.Visible.ToString().ToLowerInvariant()}},
          "mechanicalArmVisualAabb": "{{arm.GetAabb()}}",
          "shipVisualGlobalPosition": "{{ship?.GlobalPosition.ToString() ?? "missing"}}",
          "shipVisualAabb": "{{ship?.GetAabb().ToString() ?? "missing"}}",
          "flags": [{{formattedFlags}}]
        }
        """;
        var artifactDir = ProjectSettings.GlobalizePath("res://artifacts");
        Directory.CreateDirectory(artifactDir);
        File.WriteAllText(Path.Combine(artifactDir, "runtime-contract-report.json"), report);
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
