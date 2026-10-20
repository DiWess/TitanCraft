using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using TitanCraft.Combat;
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
    // Raised from 19 when the Mwezi Quarter district added the map's built
    // environment (scenes/Environment/MweziQuarterDistrict.tscn): 23 shapes,
    // one box per building plus four arcade piers and the terrace ramp. The
    // budget still exists to stop collision bloat from decorative dressing --
    // every district prop without gameplay meaning is collisionless, and the
    // generator asserts arena and pickup clearance before the scene is written.
    //
    // Raised from 48 for workstreams B and C: the Net-makers' Yard and tower
    // court walls (8 boxes and a corner pier), the quarter's invisible edge
    // (4 boxes, because the visual hills carry no collision), the two gates,
    // and seven more pickups' scrap bodies.
    private const int MaxStaticCollisionShapes = 68;
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
            await TestGatesSealTheirCourts();
            await TestPlayerScene();
            await TestGalaxabrainScoutDeathPickup();
            await TestUiScenes();
            await TestMainMenuContinueState();
            await TestPauseMenuListsControls();
            await TestSettingsApply();
            await TestHudBinding();
            await TestEndScreenNavigation();
            await TestVictoryTransitionWithSceneChangesEnabled();
            await TestVictoryScreenShowsTheEndingFrame();
            TestLocalSaveGameStoreLoadStates();
            await TestSaveLoadFlow();
            await TestFullMissionPlaythrough();
            await TestDefeatedScoutPersistenceAcrossReload();
            await TestFallingOutOfBoundsLeadsToDefeatFlow();
            await TestBeaconVisualState();
            await TestRuntimeSceneContracts();
            await TestPhysicsAndMovement();
            await TestJumpAndCamera();
            await TestEnvironmentMotion();
            await TestObjectivesHaveThresholdLights();
            await TestAmbienceLoopsPlay();
            TestCloseCuesKeepTheirVolume();
            await TestPauseMenuAudioPlaysWhilePaused();
            await TestOnboardingTutorialJourney();
            await TestHudPromptsDoNotOverlapThePanel();
            await DrainAudioPlaybacks();
            GD.Print("TITANCRAFT_INTEGRATION_TESTS_PASS");
            GetTree().Quit(0);
        }
        catch (Exception exception)
        {
            GD.PushError(exception.ToString());
            GetTree().Quit(1);
        }
    }

    /// <summary>
    /// The district's animated dressing must actually be moving in the live
    /// scene. glTF clips import with looping disabled, so a regression here is
    /// silent: the props render correctly and simply never move again after
    /// their first five seconds.
    /// </summary>
    private async System.Threading.Tasks.Task TestEnvironmentMotion()
    {
        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        await Frames(6);

        var motion = main.GetNode<Node3D>("MweziQuarterDistrict/Motion");
        var motionNodes = motion.GetChildren().OfType<EnvironmentMotionPlayer>().ToList();
        Require(motionNodes.Count > 0, "District motion group has no EnvironmentMotionPlayer nodes");

        var phases = new List<float>();
        foreach (var node in motionNodes)
        {
            var players = node.FindChildren("*", "AnimationPlayer", true, false);
            Require(players.Count > 0, $"{node.Name} has no imported AnimationPlayer");
            var player = (AnimationPlayer)players[0];
            Require(player.IsPlaying(), $"{node.Name} animation is not playing");
            var animation = player.GetAnimation(player.CurrentAnimation);
            Require(animation is not null, $"{node.Name} has no current animation");
            Require(animation!.LoopMode == Animation.LoopModeEnum.Linear,
                $"{node.Name} animation is not looping; it would freeze after one cycle");
            Require(player.SpeedScale > 0.0f, $"{node.Name} animation speed is not positive");
            phases.Add((float)player.CurrentAnimationPosition);
        }

        // Distinct start phases are the whole point of the per-instance seed:
        // identical phases read as a machine rather than as wind.
        Require(phases.Distinct().Count() > 1,
            "All district motion props started at the same phase");

        main.QueueFree();
        await Frames(2);
    }

    /// <summary>
    /// The route is read from the street, not only the HUD: every objective
    /// arrival sits in a warm pool of light (workstream A, route readability).
    /// </summary>
    private async System.Threading.Tasks.Task TestObjectivesHaveThresholdLights()
    {
        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        await Frames(2);
        var lights = main.GetNode<Node3D>("MweziQuarterDistrict/RouteReading").GetChildren().OfType<OmniLight3D>().ToList();
        foreach (var objective in new[] { "ResourceDrop_MetalPickup", "ResourceDrop_BiomassPickup", "ResourceDrop_ElectronicsPickup",
                     "Placeholder_Workbench", "Placeholder_GalaxabrainScout", "Placeholder_SavePoint", "Placeholder_Beacon" })
        {
            var at = main.GetNode<Node3D>(objective).GlobalPosition;
            Require(lights.Any(light => HorizontalDistance(light.GlobalPosition, at) < 1.0f && light.OmniRange >= 4.0f),
                $"{objective} has no threshold light over its arrival");
        }
        main.QueueFree();
        await Frames(2);
    }

    /// <summary>
    /// The 2026-09-24 playtest found the ambient loops wired and never started;
    /// they were also silent files. This asserts the in-game half: every loop
    /// plays from scene load, loops rather than stopping after one pass, and
    /// the placed sources sit where the sound belongs. The files themselves are
    /// checked for silence by tools/test_audio_sources.py.
    /// </summary>
    private async System.Threading.Tasks.Task TestAmbienceLoopsPlay()
    {
        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        await Frames(6);

        foreach (var name in new[] { "AmbientLoop_Wind", "AmbientLoop_Rumble" })
        {
            var bed = main.GetNodeOrNull<AudioStreamPlayer>($"AudioLayer_Ambient/{name}");
            Require(bed is not null, $"{name} is missing or no longer a non-positional bed");
            RequireLoopingAmbience(name, bed!.Stream, bed.Playing);
        }

        var sea = main.GetNodeOrNull<AudioStreamPlayer3D>("AudioLayer_Ambient/AmbientLoop_Sea");
        var hum = main.GetNodeOrNull<AudioStreamPlayer3D>("AudioLayer_Ambient/AmbientLoop_Machinery");
        Require(sea is not null && hum is not null, "Placed ambient sources are missing");
        RequireLoopingAmbience("AmbientLoop_Sea", sea!.Stream, sea.Playing);
        RequireLoopingAmbience("AmbientLoop_Machinery", hum!.Stream, hum.Playing);

        // The sea belongs to the harbour and the hum to the wreck: each must
        // be nearer its own landmark than the other's.
        var beacon = main.GetNode<Node3D>("Placeholder_Beacon").GlobalPosition;
        var hull = main.GetNode<Node3D>("ProductionVisualRoot/CrashWreck/MainHull_BuriedIndustrial").GlobalPosition;
        Require(HorizontalDistance(sea.GlobalPosition, beacon) < HorizontalDistance(sea.GlobalPosition, hull),
            "Sea ambience is placed nearer the wreck than the harbour");
        Require(HorizontalDistance(hum.GlobalPosition, hull) < 3.0f,
            "Machinery hum is not placed at the crashed hull");

        main.QueueFree();
        await Frames(2);
    }

    /// <summary>
    /// An AudioStreamPlayer3D clamps volume_db plus distance gain at max_db.
    /// With the default 10 m unit size, a footstep 1 m from the camera gains
    /// +20 dB and every step hit the clamp, so volume_db did nothing -- found
    /// when the silent footstep files were replaced. Each close-range cue must
    /// stay under the clamp at the distance it is normally heard from.
    /// </summary>
    private static void TestCloseCuesKeepTheirVolume()
    {
        var main = LoadScene<Node3D>(MainScenePath);
        var cues = new (string Path, float HeardAtMetres)[]
        {
            ("AudioLayer_Player/Footsteps_Metal", 1.0f),
            ("AudioLayer_Player/Footsteps_Rock", 1.0f),
            ("AudioLayer_Player/Footsteps_Ash", 1.0f),
            ("AudioLayer_Enemy/Scout_Attack", 2.0f),
            ("AudioLayer_Enemy/Scout_Hurt", 2.0f),
            ("AudioLayer_Player/Weapon_Swing", 1.0f),
            ("AudioLayer_Player/Weapon_Impact", 1.0f),
            ("AudioLayer_Player/Weapon_Ready", 1.0f),
            ("AudioLayer_Enemy/Scout_Death", 2.0f),
            ("ResourceDrop_MetalPickup/AudioPlayers/SpatialPickupPlayer", 1.8f),
            ("ResourceDrop_BiomassPickup/AudioPlayers/SpatialPickupPlayer", 1.8f),
            ("ResourceDrop_ElectronicsPickup/AudioPlayers/SpatialPickupPlayer", 1.8f),
        };
        foreach (var (path, distance) in cues)
        {
            var cue = main.GetNodeOrNull<AudioStreamPlayer3D>(path);
            Require(cue is not null, $"{path} is missing");
            Require(cue!.AttenuationModel == AudioStreamPlayer3D.AttenuationModelEnum.InverseDistance,
                $"{path} changed attenuation model; update this check's formula");
            var level = cue.VolumeDb + 20.0f * Mathf.Log(cue.UnitSize / distance) / Mathf.Log(10.0f);
            Require(level < cue.MaxDb,
                $"{path} reaches max_db at {distance} m ({level:0.0} dB), so its volume_db has no effect");
        }

        main.Free();
    }

    /// <summary>
    /// PauseMenu pauses the tree and then plays its toggle, select and hover
    /// cues. A pausable AudioStreamPlayer is paused along with the tree, so
    /// those cues could never be heard -- hidden while their files were silent.
    /// Every player the pause menu uses must keep processing while paused.
    /// </summary>
    private async System.Threading.Tasks.Task TestPauseMenuAudioPlaysWhilePaused()
    {
        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        await Frames(2);

        var cues = new[] { "AudioLayer_UI/UI_Menu_Toggle", "AudioLayer_UI/UI_Select", "AudioLayer_UI/UI_Hover" };
        GetTree().Paused = true;
        try
        {
            foreach (var path in cues)
            {
                var cue = main.GetNodeOrNull<AudioStreamPlayer>(path);
                Require(cue is not null, $"{path} is missing");
                Require(cue!.CanProcess(), $"{path} is paused with the tree, so the pause menu cannot be heard");
            }
        }
        finally
        {
            GetTree().Paused = false;
        }

        main.QueueFree();
        await Frames(2);
    }

    private static void RequireLoopingAmbience(string name, AudioStream? stream, bool playing)
    {
        Require(stream is AudioStreamWav, $"{name} has no WAV stream");
        Require(((AudioStreamWav)stream!).LoopMode == AudioStreamWav.LoopModeEnum.Forward,
            $"{name} does not loop; it would fall silent after one pass");
        Require(playing, $"{name} is not playing after the scene loads");
        // Release the managed wrapper's reference now; left to the GC it
        // outlives the engine and is reported as a leaked resource at exit.
        stream.Dispose();
    }

    /// <summary>
    /// Walks the onboarding prompt through the same order a player meets it,
    /// driving it only through real gameplay state.
    /// </summary>
    private async System.Threading.Tasks.Task TestOnboardingTutorialJourney()
    {
        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        await Frames(2);

        var hud = main.GetNode<CrashSiteHud>("HUD");
        var binder = main.GetNode<CrashSiteHudBinder>("HudBinder");
        var player = main.GetNode<FirstPersonController>("Player");
        var prompt = hud.GetNode<Label>("OnboardingPrompt");

        Require(binder.OnboardingStep == OnboardingStep.Look,
            "Onboarding should start on the look step");
        Require(prompt.Visible && prompt.Text.Length > 0,
            "Onboarding prompt should be visible at the start of a run");

        // Collecting a resource proves the movement steps and jumps ahead -- to
        // Collect, not Craft: one resource is not a recipe.
        player.Inventory.AddResources(metal: 1, biomass: 0, electronicComponents: 0);
        await Frames(2);
        Require(binder.OnboardingStep == OnboardingStep.Collect,
            "One resource should advance onboarding to the collect step, not craft");

        player.Inventory.AddResources(metal: 9, biomass: 3, electronicComponents: 2);
        await Frames(2);
        Require(binder.OnboardingStep == OnboardingStep.Craft,
            "A complete recipe should advance onboarding to the craft step");

        player.Inventory.MarkMechanicalArmBuilt();
        await Frames(2);
        Require(binder.OnboardingStep == OnboardingStep.Attack,
            "Building the arm should advance onboarding to the attack step");
        Require(prompt.Visible && prompt.Text.Length > 0,
            "Onboarding prompt should still guide the player toward the attack step");

        main.QueueFree();
        await Frames(2);
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

    /// <summary>
    /// The courts are only worth gating if the gates actually seal them. This
    /// walks the quarter with the player's own capsule against the real
    /// physics world (flood fill from spawn, 0.5 m cells): with the wreck and
    /// the cracked infill in place the yard and the tower court must be
    /// unreachable, and every mission anchor reachable; with both gates open
    /// the courts must be reachable too. Enemies are left out of the fill:
    /// they move, walls do not.
    /// </summary>
    private async System.Threading.Tasks.Task TestGatesSealTheirCourts()
    {
        LocalSaveGameStore.DeleteSave();
        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        await Frames(3);
        var player = main.GetNode<CharacterBody3D>("Player");
        var exclude = new Godot.Collections.Array<Rid> { player.GetRid() };
        foreach (var body in main.FindChildren("*", "CharacterBody3D", true, false))
            exclude.Add(((CharacterBody3D)body).GetRid());

        var anchors = new Dictionary<string, Vector2>
        {
            ["metal pickup"] = new(8, -4), ["biomass pickup"] = new(-8, -4), ["electronics pickup"] = new(0, -10),
            ["workbench"] = new(12, -10.6f), ["scout arena"] = new(20, -16), ["save point"] = new(-12, -10.8f),
            ["beacon"] = new(28, -18), ["harbour salvage"] = new(24.4f, -9.4f), ["arcade salvage"] = new(2.9f, -15.4f),
            ["stall salvage"] = new(-9.6f, -14.8f), ["yard gate front"] = new(-14.0f, -18.8f), ["tower gate front"] = new(12.3f, -21.2f),
        };
        var sealedPoints = new Dictionary<string, Vector2>
        {
            ["yard"] = new(-17.0f, -24.5f), ["yard radio"] = new(-19.0f, -27.0f),
            ["tower court"] = new(9.5f, -26.0f), ["power core"] = new(7.6f, -26.2f),
        };

        var reached = FloodReachable(main, exclude);
        foreach (var (name, point) in anchors)
            Require(Reached(reached, point), $"{name} at {point} cannot be walked to from spawn");
        foreach (var (name, point) in sealedPoints)
            Require(!Reached(reached, point), $"{name} at {point} is reachable while its gate is closed");

        main.GetNode<PullableBarricade>("NetYard/WreckBarricade").RestoreFromMission(CrashSiteMissionStep.ClearDroneNest);
        main.GetNode<BreakableWall>("TowerCourt/CrackedGate").RestoreFromMission(CrashSiteMissionStep.DefeatWarden);
        await Frames(3);
        reached = FloodReachable(main, exclude);
        foreach (var (name, point) in sealedPoints)
            Require(Reached(reached, point), $"{name} at {point} is still sealed with its gate open");

        main.QueueFree();
        await Frames(2);
    }

    private const float ReachCell = 0.5f;

    private static HashSet<(int, int)> FloodReachable(Node3D main, Godot.Collections.Array<Rid> exclude)
    {
        var space = main.GetWorld3D().DirectSpaceState;
        var capsule = new CapsuleShape3D { Radius = 0.36f, Height = 1.5f };
        var query = new PhysicsShapeQueryParameters3D
        {
            Shape = capsule,
            CollisionMask = 1,
            CollideWithAreas = false,
            CollideWithBodies = true,
            Exclude = exclude,
        };
        bool Free((int X, int Z) cell)
        {
            query.Transform = new Transform3D(Basis.Identity, new Vector3(cell.X * ReachCell, 1.0f, cell.Z * ReachCell));
            return space.IntersectShape(query, 1).Count == 0;
        }

        var start = (0, 0);
        var reached = new HashSet<(int, int)> { start };
        var frontier = new Queue<(int, int)>();
        frontier.Enqueue(start);
        while (frontier.Count > 0)
        {
            var (x, z) = frontier.Dequeue();
            foreach (var next in new[] { (x + 1, z), (x - 1, z), (x, z + 1), (x, z - 1) })
            {
                if (Math.Abs(next.Item1) > 100 || Math.Abs(next.Item2) > 100 || reached.Contains(next) || !Free(next))
                    continue;
                reached.Add(next);
                frontier.Enqueue(next);
            }
        }
        return reached;
    }

    private static bool Reached(HashSet<(int, int)> reached, Vector2 point)
    {
        var cx = (int)MathF.Round(point.X / ReachCell);
        var cz = (int)MathF.Round(point.Y / ReachCell);
        for (var dx = -1; dx <= 1; dx++)
            for (var dz = -1; dz <= 1; dz++)
                if (reached.Contains((cx + dx, cz + dz)))
                    return true;
        return false;
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
        // The corpse stays visible (docs/art/crash-site-object-asset-inventory.md,
        // "Disabled/dead Scout read") instead of the whole body vanishing: the
        // alive visual root swaps off and the disabled/collapsed visual root
        // swaps on.
        Require(scout.Visible, "Galaxabrain Scout should stay visible after death so the corpse reads as defeated");
        Require(!scout.GetNode<Node3D>("V1BetaScoutVisualRoot").Visible, "Alive Scout visual stayed visible after death");
        Require(scout.GetNode<Node3D>("V1BetaScoutDisabledVisualRoot").Visible, "Disabled Scout visual did not become visible after death");
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




    /// <summary>
    /// The controls reference lives in the pause menu; the HUD teaches controls
    /// one step at a time through the onboarding prompt, and its panel no
    /// longer carries a permanent controls line (workstream A, HUD pass).
    /// </summary>
    private async System.Threading.Tasks.Task TestPauseMenuListsControls()
    {
        var pause = LoadScene<CanvasLayer>("res://scenes/UI/PauseMenu.tscn");
        AddChild(pause);
        await Frames(2);
        var controls = pause.GetNode<Label>("Panel/Menu/Controls").Text;
        foreach (var expected in new[] { "WASD", "ZQSD", "Mouse", "Space", "E —", "workbench", "Left click", "Mk I", "Esc" })
            Require(controls.Contains(expected), $"Pause menu controls reference is missing '{expected}'");
        pause.QueueFree();
        await Frames(2);

        var hud = LoadScene<CrashSiteHud>("res://scenes/UI/HUD.tscn");
        AddChild(hud);
        await Frames(2);
        Require(hud.GetNodeOrNull("Panel/Margin/VBox/StartTutorial") is null, "The HUD panel still carries a permanent controls line");
        hud.QueueFree();
        await Frames(2);
    }

    /// <summary>
    /// The settings screen's effects, end to end: a rebound key replaces the
    /// action's events in the live input map and round-trips through the saved
    /// string form; field of view, sensitivity and invert reach the player;
    /// resetting restores the project's own bindings; both menus open the screen.
    /// </summary>
    private async System.Threading.Tasks.Task TestSettingsApply()
    {
        var original = Core.GameSettings.Current.Copy();
        try
        {
            var key = Core.GameSettings.ToEvent("key:70");
            Require(key is InputEventKey && Core.GameSettings.FromEvent(key!) == "key:70", "A key binding did not round-trip");
            Require(Core.GameSettings.FromEvent(Core.GameSettings.ToEvent("mouse:2")!) == "mouse:2", "A mouse binding did not round-trip");

            var custom = new Core.GameSettingsData { FieldOfView = 90f, MouseSensitivityScale = 2f, InvertY = true };
            custom.Bindings["jump"] = "key:70";
            Core.GameSettings.Set(custom);
            var jumpEvents = InputMap.ActionGetEvents("jump");
            Require(jumpEvents.Count == 1 && jumpEvents[0] is InputEventKey { PhysicalKeycode: Key.F }, "Rebinding jump did not replace its events");

            var main = LoadScene<Node3D>(MainScenePath);
            AddChild(main);
            await Frames(2);
            var player = main.GetNode<FirstPersonController>("Player");
            Require(Mathf.IsEqualApprox(player.BaseFov, 90f), $"The field of view setting did not reach the player ({player.BaseFov})");
            Core.GameSettings.Set(new Core.GameSettingsData());
            Require(Mathf.IsEqualApprox(player.BaseFov, Core.GameSettingsData.DefaultFieldOfView), "Changing settings in play did not update the player");
            Require(InputMap.ActionGetEvents("jump").Count >= 1 && InputMap.ActionGetEvents("jump")[0] is InputEventKey { Keycode: Key.Space }, "Resetting did not restore the default jump key");

            var pause = main.GetNode<PauseMenu>("PauseMenu");
            pause.OpenSettings();
            await Frames(2);
            var panel = pause.GetNodeOrNull<SettingsPanel>("SettingsPanel");
            Require(panel is not null && panel.Visible, "The pause menu did not open the settings screen");
            Require(panel!.FindChildren("*", "Button", true, false).Count >= Core.GameSettings.RemappableActions.Count + 2, "The settings screen lists no key bindings");
            panel.Close();
            await Frames(1);
            Require(pause.GetNode<Control>("Panel").Visible, "Closing settings did not return to the pause menu");
            main.QueueFree();
            await Frames(2);

            var menu = LoadScene<Control>("res://scenes/UI/MainMenu.tscn");
            AddChild(menu);
            await Frames(2);
            Require(!menu.GetNode<Button>("Menu/SettingsButton").Disabled, "The main menu's Settings button is still disabled");
            ((MainMenu)menu).OpenSettings();
            await Frames(2);
            Require(menu.GetNodeOrNull<SettingsPanel>("SettingsPanel") is { Visible: true }, "The main menu did not open the settings screen");
            menu.QueueFree();
            await Frames(2);
        }
        finally
        {
            Core.GameSettings.Set(original);
        }
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
        main.QueueFree();
        await Frames(2);
    }


    // Every other scenario in this runner sets EnableSceneChanges = false, which
    // is exactly why the beacon's crash at the climax survived: the scene change
    // it depended on never ran here. This one keeps scene changes ON and only
    // lengthens the victory hold so the change is still pending while we assert.
    // Before the navigator deferred its change, beacon.Interact threw a
    // NullReferenceException from Beacon.AddExtractionTrauma (GetTree() == null),
    // which would fail this test at the Interact call.
    private async System.Threading.Tasks.Task TestVictoryTransitionWithSceneChangesEnabled()
    {
        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        await Frames(2);
        var player = main.GetNode<FirstPersonController>("Player");
        var beacon = main.GetNode<Beacon>("Placeholder_Beacon");
        var navigator = main.GetNode<CrashSiteEndScreenNavigator>("EndScreenNavigator");
        navigator.EnableSceneChanges = true;
        navigator.VictoryHoldSeconds = 600f;

        player.Mission.Restore(CrashSiteMissionStep.ActivateBeacon);
        player.Inventory.MarkPowerCoreCollected();
        main.GetNode<BeaconHoldout>("BeaconHoldout").ChargeSeconds = 0.3f;
        player.GlobalPosition = beacon.GlobalPosition + new Vector3(2.0f, 0.0f, 0.0f);

        Require(beacon.Interact(player.Inventory, player.Mission), "Beacon did not activate on the real scene-change path");
        for (var frame = 0; frame < 300 && !player.Mission.IsVictory; frame++)
            await Frames(1);
        Require(player.Mission.IsVictory, "The holdout did not produce victory with scene changes enabled");
        Require(beacon.IsInsideTree(), "Victory removed the beacon from the tree before its activation finished");
        Require(beacon.IsActivated, "Beacon activation did not complete with scene changes enabled");
        Require(navigator.LastRequestedScenePath == "res://scenes/UI/VictoryScreen.tscn", "Victory screen was not requested");
        Require(navigator.IsSceneChangePending, "Victory scene change should be scheduled, not performed inside the mission event");
        Require(!((ICrashSiteInteractable)beacon).IsInteractionAvailable, "An activated beacon should stop offering its interaction prompt");

        // The world must stay on screen through the hold so the activation is seen.
        await Frames(6);
        Require(main.IsInsideTree(), "Victory hold did not keep the world on screen");
        Require(navigator.IsSceneChangePending, "Victory scene change fired before its hold elapsed");

        // The hold is the in-world ending: the view cuts to the ending camera
        // on the lit beacon, the HUD steps aside for the letterbox and caption,
        // and the player no longer takes input.
        var ending = main.GetNode<VictoryEndingShot>("VictoryEndingShot");
        Require(ending.IsPlaying, "Victory did not start the in-world ending shot");
        Require(ending.GetNode<Camera3D>("EndingCamera").Current, "The ending camera is not the current view during the victory hold");
        Require(!main.GetNode<CanvasLayer>("HUD").Visible, "The HUD stayed up over the ending shot");
        Require(ending.GetNode<CanvasLayer>("Overlay").Visible, "The ending letterbox and caption did not appear");
        Require(player.ProcessMode == ProcessModeEnum.Disabled, "The player still takes input during the ending shot");
        Require(ending.GetNode<Camera3D>("EndingCamera").IsPositionInFrustum(beacon.GlobalPosition + Vector3.Up * 1.5f),
            "The beacon is not in frame in the ending shot");

        main.QueueFree();
        await Frames(2);
    }

    /// <summary>
    /// The victory screen is its own scene, so the ending's last frame is
    /// handed across; the screen shows it behind a lighter backdrop and
    /// consumes it, so a later visit does not show a stale frame.
    /// </summary>
    private async System.Threading.Tasks.Task TestVictoryScreenShowsTheEndingFrame()
    {
        var image = Image.CreateEmpty(8, 8, false, Image.Format.Rgba8);
        image.Fill(new Color(0.9f, 0.6f, 0.3f));
        EndingSnapshot.Set(ImageTexture.CreateFromImage(image));

        var screen = LoadScene<Control>("res://scenes/UI/VictoryScreen.tscn");
        AddChild(screen);
        await Frames(2);
        var snapshot = screen.GetNode<TextureRect>("Snapshot");
        Require(snapshot.Visible && snapshot.Texture is not null, "The victory screen did not show the ending's last frame");
        Require(screen.GetNode<ColorRect>("Backdrop").Color.A < 0.88f, "The backdrop still hides the ending frame at full darkness");
        Require(EndingSnapshot.Take() is null, "The victory screen did not consume the ending frame");
        screen.QueueFree();
        await Frames(2);

        LocalSaveGameStore.DeleteSave();
        var plain = LoadScene<Control>("res://scenes/UI/VictoryScreen.tscn");
        AddChild(plain);
        await Frames(2);
        Require(!plain.GetNode<TextureRect>("Snapshot").Visible, "The victory screen showed a frame when no ending was captured");
        // A player can win without ever using the save point; the victory
        // screen must still congratulate them, not talk about checkpoints.
        Require(!plain.GetNode<Label>("Menu/Summary").Text.Contains("checkpoint"),
            "The victory screen told a winning player that no checkpoint save was found");
        plain.QueueFree();
        await Frames(2);
    }

    // The walked playtest (2026-09-24, finding 3) showed the onboarding prompt
    // drawn across the HUD panel's edge in every early frame at the project's
    // 1280x720 design size -- invisible at 1920 wide, which is how it survived.
    // Stretch mode is "viewport", so layout at 1280x720 is layout everywhere.
    private async System.Threading.Tasks.Task TestHudPromptsDoNotOverlapThePanel()
    {
        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        await Frames(4);
        var hud = main.GetNode<CrashSiteHud>("HUD");
        var panel = hud.GetNode<Control>("Panel");
        var onboarding = hud.GetNode<Label>("OnboardingPrompt");
        var feedback = hud.GetNode<Label>("ActionFeedback");

        // Panel budget: 488x178 before the HUD pass, 400x150 after. A line that
        // does not wrap widens a PanelContainer silently, which is how the arm
        // progress line once held it at 517 px.
        Require(panel.Size.X <= 400.5f && panel.Size.Y <= 160f,
            $"HUD panel grew past its 400x160 budget: {panel.Size}");

        // Every objective in the run, at its widest (the arm line asking for
        // the next module's salvage), must keep the panel inside its budget.
        var budgetPlayer = main.GetNode<FirstPersonController>("Player");
        foreach (var step in Enum.GetValues<CrashSiteMissionStep>())
        {
            budgetPlayer.Mission.Restore(step);
            await Frames(2);
            Require(panel.Size.X <= 400.5f && panel.Size.Y <= 160f,
                $"HUD panel grew past its 400x160 budget at {step}: {panel.Size} ('{hud.GetNode<Label>("Panel/Margin/VBox/Objective").Text}')");
        }
        budgetPlayer.Mission.Restore(CrashSiteMissionStep.CollectResources);

        // The longest onboarding line, so the widest the prompt ever gets.
        var longest = new OnboardingTutorialState();
        longest.ReportResourceCollected();
        onboarding.Text = longest.CurrentPrompt;
        onboarding.Visible = true;
        await Frames(2);

        var panelRect = panel.GetGlobalRect();
        var promptRect = onboarding.GetGlobalRect();
        Require(!promptRect.Intersects(panelRect),
            $"Onboarding prompt {promptRect} overlaps the HUD panel {panelRect}");
        Require(!promptRect.Intersects(feedback.GetGlobalRect()),
            $"Onboarding prompt {promptRect} overlaps the action feedback line {feedback.GetGlobalRect()}");

        // Every interactable speaks to the player, not in scene-node names.
        var interactables = 0;
        foreach (var node in main.FindChildren("*", "", true, false))
        {
            if (node is not ICrashSiteInteractable interactable || node is Workbench)
                continue;
            interactables++;
            var line = interactable.InteractionPrompt;
            Require(line.StartsWith("Press E to ") && line != "Press E to interact",
                $"{node.Name} has no player-facing interaction prompt: '{line}'");
            Require(!line.Contains(node.Name.ToString().Replace("Placeholder_", string.Empty)),
                $"{node.Name} shows its scene-node name to the player: '{line}'");
        }
        Require(interactables >= 5, $"Expected the MVP's interactables in the scene, found {interactables}");

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
        victoryPlayer.Mission.Restore(CrashSiteMissionStep.ActivateBeacon);
        victoryPlayer.Mission.TryCompleteBeaconActivation();
        victoryPlayer.Mission.TryCompleteBeaconDefence();
        await Frames(2);
        Require(victoryNavigator.LastRequestedScenePath == "res://scenes/UI/VictoryScreen.tscn", "Victory screen was not requested after mission victory");
        victoryMain.QueueFree();
        await Frames(2);

        var defeatMain = LoadScene<Node3D>(MainScenePath);
        AddChild(defeatMain);
        await Frames(2);
        var defeatPlayer = defeatMain.GetNode<FirstPersonController>("Player");
        var defeatHud = defeatMain.GetNode<CrashSiteHud>("HUD");
        var defeatNavigator = defeatMain.GetNode<CrashSiteEndScreenNavigator>("EndScreenNavigator");
        RequireHudObjective(defeatHud, CrashSiteMissionStep.CollectResources, "defeat path before death");
        defeatNavigator.EnableSceneChanges = false;
        defeatPlayer.Health.ApplyDamage(PlayerHealth.DefaultMaxHealth);
        await Frames(2);
        Require(defeatPlayer.Health.IsDead, "Lethal damage did not put the player in defeat health state");
        Require(defeatNavigator.LastRequestedScenePath == "res://scenes/UI/DefeatScreen.tscn", "Defeat screen was not requested after player death");
        Require(defeatHud.GetNode<Label>("ActionFeedback").Text == FirstPersonController.DefeatRetryFeedback, "HUD did not show defeat retry feedback after player death");
        Require(!defeatPlayer.Mission.IsVictory, "Player defeat should not set victory state");
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
        Require(!beacon.Interact(player.Inventory, player.Mission), "Beacon activated without the power core");
        player.Inventory.MarkPowerCoreCollected();
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
        // The save point repairs the suit before it saves (a rest point, not only a bookmark).
        Require(loadedPlayer.Health.CurrentHealth == PlayerHealth.DefaultMaxHealth, "The save point did not repair the suit before saving");
        Require(loadedPlayer.Inventory.Metal == 6 && loadedPlayer.Inventory.ElectronicComponents == 2, "Loaded inventory mismatch");
        Require(loadedPlayer.Mission.CurrentStep == Missions.CrashSiteMissionStep.BuildMechanicalArm, "Loaded mission step mismatch");
        Require(HorizontalDistance(loadedPlayer.GlobalPosition, new Vector3(3.0f, 2.0f, -7.0f)) < 0.1f, "Loaded player position mismatch");
        loadedMain.QueueFree();
        await Frames(2);
        LocalSaveGameStore.DeleteSave();
    }

    private static void RequireHudObjective(CrashSiteHud hud, CrashSiteMissionStep expectedStep, string checkpoint)
    {
        var expectedText = new CrashSiteMissionState();
        expectedText.Restore(expectedStep);
        var actualText = hud.GetNode<Label>("Panel/Margin/VBox/Objective").Text;
        Require(actualText == expectedText.HudBreadcrumb, $"HUD objective mismatch at {checkpoint}. Expected '{expectedText.HudBreadcrumb}', got '{actualText}'");
        Require(!string.IsNullOrWhiteSpace(actualText), $"HUD objective missing at {checkpoint}");
        Require(!actualText.Contains("Unknown Objective", StringComparison.Ordinal), $"HUD objective is unknown at {checkpoint}");
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
        var bareArm = player.GetNode<MeshInstance3D>("Head/Camera3D/BareArmVisual");
        var scout = main.GetNode<GalaxabrainScout>("Placeholder_GalaxabrainScout");
        var component = scout.GetNode<GalaxabrainComponentPickup>("GalaxabrainComponentPickup");
        var workbench = main.GetNode<Workbench>("Placeholder_Workbench");
        var savePoint = main.GetNode<SavePoint>("Placeholder_SavePoint");
        var beacon = main.GetNode<Beacon>("Placeholder_Beacon");
        var hud = main.GetNode<CrashSiteHud>("HUD");
        var lastActionFeedback = string.Empty;
        player.ActionFeedbackChanged += message => lastActionFeedback = message;

        RequireHudObjective(hud, CrashSiteMissionStep.CollectResources, "initial spawn");
        Require(player.IsInsideTree(), "Player did not spawn into the Crash Site scene");
        Require(!arm.Visible, "Mechanical arm visual should start hidden");
        Require(bareArm.Visible, "Bare astronaut arm should be visible before crafting");
        LogMvpSmokeMilestone(1, "player spawned");
        Require(!workbench.Interact(player.Inventory, player.Mission), "Workbench must reject crafting before resources are collected");
        Require(!component.Interact(player.Inventory, player.Mission), "Component must reject recovery before Galaxabrain defeat");
        Require(!beacon.Interact(player.Inventory, player.Mission), "Beacon must reject activation before component recovery");
        Require(player.Mission.CurrentStep == CrashSiteMissionStep.CollectResources, "Invalid early interactions mutated mission state");

        var metalPickupVisual = main.GetNode<Node3D>("ResourceDrop_MetalPickup/VisualGroup");
        var pickupRotationBefore = metalPickupVisual.Rotation.Y;
        var savePointVisual = main.GetNode<Node3D>("Placeholder_SavePoint/VisualRoot");
        var savePointScaleBefore = savePointVisual.Scale.X;
        await Frames(6);
        var savePointScaleMid = savePointVisual.Scale.X;
        await Frames(6);
        Require(!Mathf.IsEqualApprox(metalPickupVisual.Rotation.Y, pickupRotationBefore), "Uncollected resource drop visual is not idle-spinning");
        Require(!Mathf.IsEqualApprox(savePointScaleMid, savePointScaleBefore) || !Mathf.IsEqualApprox(savePointVisual.Scale.X, savePointScaleBefore), "Save point visual is not breathing");

        foreach (var pickupName in new[] { "ResourceDrop_MetalPickup", "ResourceDrop_BiomassPickup" })
        {
            var pickup = main.GetNode<ResourceDrop>(pickupName);
            Require(pickup.Interact(player.Inventory, player.Mission), $"{pickupName} could not be collected through the real pickup path");
            Require(!pickup.Interact(player.Inventory, player.Mission), $"{pickupName} was collectable twice");
        }
        Require(hud.GetNode<Label>("Panel/Margin/VBox/Resources").Modulate != Colors.White, "HUD resources label did not pulse on collection");

        var finalPickup = main.GetNode<ResourceDrop>("ResourceDrop_ElectronicsPickup");
        player.GlobalPosition = finalPickup.GlobalPosition + new Vector3(1.5f, 0.0f, 0.0f);
        await Frames(2);
        player.GetNode<Node3D>("Head").LookAt(finalPickup.GlobalPosition, Vector3.Up);
        Require(player.TryInteract(), "Final resource could not be collected through the player interaction pickup path");
        Require(!finalPickup.Interact(player.Inventory, player.Mission), "Final resource was collectable twice");
        Require(player.Mission.CurrentStep == CrashSiteMissionStep.BuildMechanicalArm, "Collecting all resources did not advance to crafting");
        Require(lastActionFeedback == FirstPersonController.ResourceCompletionFeedback, "Final resource pickup did not emit resource completion feedback");
        Require(lastActionFeedback.Contains("Resources secured") && lastActionFeedback.Contains("craft") && lastActionFeedback.Contains("workbench"), "Resource completion feedback did not explain resource completion and workbench crafting guidance");
        Require(hud.GetNode<Label>("ActionFeedback").Text == FirstPersonController.ResourceCompletionFeedback, "HUD did not show resource completion feedback text");
        RequireHudObjective(hud, CrashSiteMissionStep.BuildMechanicalArm, "after resources complete");
        Require(!player.Inventory.IsMechanicalArmBuilt, "Mechanical Arm Mk I was granted before workbench crafting");
        Require(!arm.Visible, "Mechanical arm visual appeared before workbench crafting");
        Require(bareArm.Visible, "Bare astronaut arm disappeared before workbench crafting");
        LogMvpSmokeMilestone(2, "resources collected");

        player.GlobalPosition = workbench.GlobalPosition + new Vector3(1.5f, 0.0f, 0.0f);
        await Frames(2);
        player.GetNode<Node3D>("Head").LookAt(workbench.GlobalPosition, Vector3.Up);
        Require(player.TryInteract(), "Workbench crafting failed through the player interaction path with full resources");
        Require(player.Inventory.IsMechanicalArmBuilt, "Crafting did not build the mechanical arm");
        Require(player.Mission.CurrentStep == CrashSiteMissionStep.DefeatGalaxabrain, "Crafting did not advance to the defeat step");
        Require(arm.Visible, "Mechanical arm visual did not appear after crafting");
        Require(!bareArm.Visible, "Bare astronaut arm still visible after crafting the mechanical arm");
        Require(lastActionFeedback == FirstPersonController.MechanicalArmCraftSuccessFeedback, "Workbench crafting did not emit Mechanical Arm Mk I success feedback");
        Require(lastActionFeedback.Contains("Mechanical Arm Mk I online") && lastActionFeedback.Contains("defeat the Galaxabrain Scout"), "Mechanical Arm Mk I success feedback did not confirm arm readiness and Galaxabrain guidance");
        Require(hud.GetNode<Label>("ActionFeedback").Text == FirstPersonController.MechanicalArmCraftSuccessFeedback, "HUD did not show Mechanical Arm Mk I success feedback text");
        RequireHudObjective(hud, CrashSiteMissionStep.DefeatGalaxabrain, "after arm crafted");
        LogMvpSmokeMilestone(3, "Mechanical Arm Mk I crafted");
        Require(!workbench.Interact(player.Inventory, player.Mission), "Workbench crafted the arm twice");
        Require(!component.Interact(player.Inventory, player.Mission), "Component was recoverable while the Galaxabrain is alive");

        // The first combat hit goes through the real player attack path: aim the
        // Chase body language: with the player 6 m away (inside detection, outside
        // attack range) the scout chases; its alive visual must turn toward the
        // player (target yaw -PI/2 from +Z approach) and skitter-bob while moving.
        player.GlobalPosition = scout.GlobalPosition + new Vector3(0.0f, 0.0f, 6.0f);
        var scoutAliveVisual = scout.GetNode<Node3D>("V1BetaScoutVisualRoot");
        var scoutYawBefore = scoutAliveVisual.Rotation.Y;
        var scoutVisualYBefore = scoutAliveVisual.Position.Y;
        await Frames(4);
        var scoutVisualYMid = scoutAliveVisual.Position.Y;
        await Frames(4);
        Require(!Mathf.IsEqualApprox(scoutAliveVisual.Rotation.Y, scoutYawBefore), "Chasing Scout visual did not turn toward the player");
        Require(!Mathf.IsEqualApprox(scoutVisualYMid, scoutVisualYBefore) || !Mathf.IsEqualApprox(scoutAliveVisual.Position.Y, scoutVisualYBefore), "Chasing Scout visual is not gait-bobbing");

        // camera at the scout and raycast-attack so landed-hit HUD feedback is proven.
        player.GlobalPosition = scout.GlobalPosition + new Vector3(2.0f, 0.0f, 0.0f);
        await Frames(2);
        player.GetNode<Node3D>("Head").LookAt(scout.GlobalPosition, Vector3.Up);
        Require(player.TryAttack(), "Aimed mechanical arm attack did not hit the Galaxabrain");
        Require(scout.Brain.CurrentHealth == scout.Brain.MaxHealth - MechanicalArmAttackLogic.DefaultMechanicalArmDamage, "Aimed attack did not damage the Galaxabrain");
        Require(lastActionFeedback == FirstPersonController.GalaxabrainScoutHitFeedback, "Landed Galaxabrain hit did not emit hit feedback");
        Require(hud.GetNode<Label>("ActionFeedback").Text == FirstPersonController.GalaxabrainScoutHitFeedback, "HUD did not show landed Galaxabrain hit feedback");
        LogMvpSmokeMilestone(4, "Galaxabrain Scout engaged");

        for (var hit = 0; hit < 3; hit++)
            scout.ApplyDamage(MechanicalArmAttackLogic.DefaultMechanicalArmDamage);

        Require(scout.Brain.IsDead, "Galaxabrain survived four arm hits");
        Require(lastActionFeedback == FirstPersonController.GalaxabrainScoutDefeatFeedback, "Galaxabrain defeat did not emit recover-component feedback");
        Require(hud.GetNode<Label>("ActionFeedback").Text == FirstPersonController.GalaxabrainScoutDefeatFeedback, "HUD did not show Galaxabrain defeat feedback");
        Require(player.Mission.CurrentStep == CrashSiteMissionStep.RecoverGalaxabrainComponent, "Galaxabrain death did not advance to component recovery");
        Require(!player.Inventory.HasGalaxabrainComponent, "Galaxabrain death granted the component before pickup interaction");
        Require(!player.Mission.IsVictory && player.Mission.CurrentStep != CrashSiteMissionStep.ActivateBeacon, "Galaxabrain death skipped the component recovery step");
        Require(component.Visible && component.Monitoring, "Component pickup was not revealed by Galaxabrain death");
        RequireHudObjective(hud, CrashSiteMissionStep.RecoverGalaxabrainComponent, "after Scout defeated");
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
        Require(hud.GetNode<Label>("Panel/Margin/VBox/Resources").Text.Contains("Component: Recovered"), "HUD resource dashboard did not count the recovered Galaxabrain component");
        Require(player.Mission.CurrentStep == CrashSiteMissionStep.UpgradeArmMk2, "Component recovery did not advance to the Mk II upgrade");
        Require(!player.Mission.IsVictory, "Component recovery skipped the rest of the mission");
        Require(lastActionFeedback == FirstPersonController.GalaxabrainComponentRecoveryFeedback, "Component recovery did not emit its action feedback");
        Require(lastActionFeedback.Contains("component recovered") && lastActionFeedback.Contains("workbench"), "Component recovery action feedback did not explain the Mk II step");
        Require(!component.Visible && !component.Monitoring, "Component pickup stayed interactable after recovery");
        RequireHudObjective(hud, CrashSiteMissionStep.UpgradeArmMk2, "after component recovered");
        LogMvpSmokeMilestone(6, "component retrieved");
        Require(!component.Interact(player.Inventory, player.Mission), "Component was recoverable twice");
        Require(!beacon.Interact(player.Inventory, player.Mission), "Beacon activated without its power core");

        // --- Act 2: the Mk II magnet clamp and the Net-makers' Yard ---------
        foreach (var dropPath in new[] { "ActTwoSalvage/ResourceDrop_HarbourMetal", "ActTwoSalvage/ResourceDrop_ArcadeMetal", "ActTwoSalvage/ResourceDrop_StallElectronics" })
            Require(main.GetNode<ResourceDrop>(dropPath).Interact(player.Inventory, player.Mission), $"{dropPath} could not be collected");
        var saveCoordinator = main.GetNode<CrashSiteSaveCoordinator>("SaveCoordinator");
        player.GlobalPosition = workbench.GlobalPosition + new Vector3(1.5f, 0.0f, 0.0f);
        await Frames(2);
        player.GetNode<Node3D>("Head").LookAt(workbench.GlobalPosition, Vector3.Up);
        Require(player.TryInteract(), "Workbench did not fit Mk II with the component and salvage in hand");
        Require(player.Inventory.HasMagnetClamp && !player.Inventory.HasGalaxabrainComponent, "Mk II did not fit the Scout's component into the arm");
        Require(player.Mission.CurrentStep == CrashSiteMissionStep.OpenNetYard, "Mk II did not advance to opening the yard");
        Require(lastActionFeedback == FirstPersonController.Mk2CraftSuccessFeedback, "Mk II craft did not explain the pull");
        Require(player.GetNode<Node3D>("Head/Camera3D/MechanicalArmVisual/ClampModule").Visible, "The Mk II clamp is not visible on the arm");
        Require(saveCoordinator.LastSaveSucceeded, "Fitting Mk II did not write its checkpoint");
        LogMvpSmokeMilestone(12, "Mechanical Arm Mk II crafted");

        var barricade = main.GetNode<PullableBarricade>("NetYard/WreckBarricade");
        player.GlobalPosition = barricade.GlobalPosition + new Vector3(1.4f, 0.0f, 6.0f);
        await Frames(2);
        player.GetNode<Node3D>("Head").LookAt(barricade.PullAnchor, Vector3.Up);
        Require(player.TryPull(), "The Mk II clamp could not pull the wreck through the real aim path");
        Require(barricade.IsOpen, "The pulled wreck did not open the yard");
        Require(player.Mission.CurrentStep == CrashSiteMissionStep.ClearDroneNest, "Opening the yard did not advance to the drone nest");
        LogMvpSmokeMilestone(13, "Net-makers' Yard opened");

        var nest = main.GetNode<DroneNestEncounter>("NetYard/DroneNest");
        Require(nest.Drones.Count == 3, $"Expected three nest drones, found {nest.Drones.Count}");
        Require(nest.Drones.All(drone => drone.Brain.State != SwarmDroneState.Dormant), "Opening the yard did not wake the drones");
        var firstDrone = nest.Drones[0];
        // Inside the yard, past the clamp's cooldown from the wreck.
        player.GlobalPosition = new Vector3(-16.0f, 0.2f, -21.4f);
        await Frames(70);
        player.GetNode<Node3D>("Head").LookAt(firstDrone.GlobalPosition, Vector3.Up);
        Require(player.TryPull(), "The clamp could not pull a drone out of the air");
        Require(firstDrone.Brain.State == SwarmDroneState.Stunned, "A pulled drone was not grounded");
        await Frames(24);
        Require(firstDrone.GlobalPosition.Y < 1.2f, $"A pulled drone did not come down to the arm's reach (y {firstDrone.GlobalPosition.Y:0.00})");
        foreach (var drone in nest.Drones)
            drone.ReceiveStrike(new ArmStrike(drone.Brain.MaxHealth, false, player.GlobalPosition));
        await Frames(3);
        Require(player.Mission.CurrentStep == CrashSiteMissionStep.UpgradeArmMk3, "Clearing the nest did not advance to Mk III");
        LogMvpSmokeMilestone(14, "drone nest cleared");

        foreach (var dropPath in new[] { "NetYard/ResourceDrop_YardMetalStall", "NetYard/ResourceDrop_YardMetalPalm", "NetYard/ResourceDrop_YardBroodBiomass", "NetYard/ResourceDrop_YardRadioElectronics" })
            Require(main.GetNode<ResourceDrop>(dropPath).Interact(player.Inventory, player.Mission), $"{dropPath} could not be collected");
        player.GlobalPosition = workbench.GlobalPosition + new Vector3(1.5f, 0.0f, 0.0f);
        await Frames(2);
        player.GetNode<Node3D>("Head").LookAt(workbench.GlobalPosition, Vector3.Up);
        Require(player.TryInteract(), "Workbench did not fit Mk III from the yard's salvage");
        Require(player.Inventory.HasBreakerCoil, "Mk III was not fitted");
        Require(player.Mission.CurrentStep == CrashSiteMissionStep.BreachTowerGate, "Mk III did not advance to the tower gate");
        Require(player.GetNode<Node3D>("Head/Camera3D/MechanicalArmVisual/CoilModule").Visible, "The Mk III coil is not visible on the arm");
        LogMvpSmokeMilestone(15, "Mechanical Arm Mk III crafted");

        // --- Act 3: the tower court, the Warden and the power core ----------
        var gate = main.GetNode<BreakableWall>("TowerCourt/CrackedGate");
        player.GlobalPosition = gate.GlobalPosition + new Vector3(0.0f, 0.0f, 2.2f);
        await Frames(2);
        player.GetNode<Node3D>("Head").LookAt(gate.GlobalPosition + Vector3.Up * 1.4f, Vector3.Up);
        player.TryAttack();
        Require(!gate.IsBroken, "An ordinary strike broke the cracked gate");
        Require(lastActionFeedback == gate.DeflectFeedback, "A deflected strike on the gate did not say what would work");
        await Frames(60);
        Require(player.ChargeAndReleaseForTest(1.0f), "The charged strike did not land on the gate through the real strike path");
        Require(gate.IsBroken, "A charged strike did not break the cracked gate");
        Require(player.Mission.CurrentStep == CrashSiteMissionStep.DefeatWarden, "Breaking the gate did not advance to the Warden");
        LogMvpSmokeMilestone(16, "tower gate broken");

        var warden = main.GetNode<GalaxabrainWarden>("TowerCourt/Warden");
        await Frames(2);
        Require(warden.Brain.State != WardenState.Dormant, "Breaking the gate did not wake the Warden");
        Require(warden.ReceiveStrike(new ArmStrike(25, false, player.GlobalPosition)) == StrikeOutcome.Deflected, "The Warden's guard did not turn an ordinary strike");
        var wardenOutcome = StrikeOutcome.Ignored;
        for (var blow = 0; blow < 20 && wardenOutcome != StrikeOutcome.Killed; blow++)
            wardenOutcome = warden.ReceiveStrike(new ArmStrike(blow % 5 == 0 ? 70 : 25, blow % 5 == 0, player.GlobalPosition));
        Require(warden.Brain.IsDead, "The Warden survived its guard being broken twice");
        Require(player.Mission.CurrentStep == CrashSiteMissionStep.RecoverPowerCore, "The Warden's death did not advance to the power core");
        Require(lastActionFeedback == FirstPersonController.WardenDefeatFeedback, "The Warden's death did not point at the power core");
        LogMvpSmokeMilestone(17, "Warden defeated");

        var core = main.GetNode<PowerCorePickup>("TowerCourt/PowerCorePickup");
        player.GlobalPosition = core.GlobalPosition + new Vector3(1.5f, -0.2f, 0.0f);
        await Frames(2);
        player.GetNode<Node3D>("Head").LookAt(core.GlobalPosition + Vector3.Up * 0.5f, Vector3.Up);
        Require(player.TryInteract(), "The power core could not be taken through the interaction ray");
        Require(player.Inventory.HasPowerCore, "Taking the core did not put it in the inventory");
        Require(player.Mission.CurrentStep == CrashSiteMissionStep.ActivateBeacon, "The power core did not advance to the beacon");
        Require(hud.GetNode<Label>("Panel/Margin/VBox/Resources").Text.Contains("Power core"), "The HUD did not show the carried power core");
        RequireHudObjective(hud, CrashSiteMissionStep.ActivateBeacon, "after power core recovered");
        LogMvpSmokeMilestone(18, "power core recovered");

        player.GlobalPosition = savePoint.GlobalPosition + new Vector3(1.5f, 0.0f, 0.0f);
        await Frames(2);
        player.GetNode<Node3D>("Head").LookAt(savePoint.GlobalPosition, Vector3.Up);
        Require(player.TryInteract(), "Player interaction raycast could not use the save point before beacon activation");
        await Frames(2);
        Require(savePoint.HasSavedCheckpoint, "Save point did not record checkpoint usage");
        Require(saveCoordinator.LastSaveSucceeded, "Save point did not write the continuation save");
        Require(lastActionFeedback == FirstPersonController.SavePointSuccessFeedback, "Save point did not emit player-facing checkpoint feedback");
        Require(lastActionFeedback.Contains("Checkpoint saved") && lastActionFeedback.Contains("suit repaired"), "Save point feedback did not confirm the save and the repair");
        Require(hud.GetNode<Label>("ActionFeedback").Text == FirstPersonController.SavePointSuccessFeedback, "HUD did not show the checkpoint save feedback text");
        RequireHudObjective(hud, CrashSiteMissionStep.ActivateBeacon, "after save point use");
        LogMvpSmokeMilestone(7, "save point used");

        // Activate through the player raycast: the core goes in and the holdout starts.
        var holdout = main.GetNode<BeaconHoldout>("BeaconHoldout");
        holdout.ChargeSeconds = 2.0f;
        player.GlobalPosition = beacon.GlobalPosition + new Vector3(1.5f, 0.0f, 0.0f);
        await Frames(2);
        player.GetNode<Node3D>("Head").LookAt(beacon.GlobalPosition, Vector3.Up);
        Require(player.TryInteract(), "Player interaction raycast could not install the power core in the beacon");
        Require(beacon.IsActivated, "Beacon activation did not update the beacon state");
        Require(player.Mission.CurrentStep == CrashSiteMissionStep.DefendBeacon, "Beacon activation did not start the holdout");
        Require(!player.Mission.IsVictory, "Beacon activation skipped the holdout");
        Require(lastActionFeedback == FirstPersonController.BeaconChargingFeedback, "Beacon activation did not tell the player to hold the plaza");
        LogMvpSmokeMilestone(8, "beacon activated");
        Require(!beacon.Interact(player.Inventory, player.Mission), "Beacon activated twice");

        for (var frame = 0; frame < 900 && !player.Mission.IsVictory; frame++)
            await Frames(1);
        Require(holdout.Spawned.Count > 0, "The holdout never sent an enemy");
        Require(holdout.Spawned.Count <= holdout.Logic!.TotalEnemies, "The holdout spawned more enemies than its waves hold");
        Require(player.Mission.IsVictory, "Holding the plaza until the beacon charged did not produce victory");
        RequireHudObjective(hud, CrashSiteMissionStep.Victory, "after beacon holdout/victory");
        Require(hud.GetNode<Label>("ActionFeedback").Text == FirstPersonController.BeaconActivationFeedback, "Victory did not confirm the rescue signal");
        await Frames(2);
        foreach (var enemy in holdout.Spawned)
        {
            var alive = enemy switch
            {
                SwarmDrone drone => !drone.Brain.IsDead,
                GalaxabrainScout holdoutScout => !holdoutScout.Brain.IsDead,
                _ => false,
            };
            Require(!alive, $"{enemy.Name} survived the beacon firing");
        }
        LogMvpSmokeMilestone(19, "beacon holdout survived");
        var skyBeam = beacon.GetNode<SpotLight3D>("LandmarkVfx/SkyBeam");
        var beamEnergyBefore = skyBeam.LightEnergy;
        await Frames(6);
        var beamEnergyMid = skyBeam.LightEnergy;
        await Frames(6);
        Require(!Mathf.IsEqualApprox(beamEnergyMid, beamEnergyBefore) || !Mathf.IsEqualApprox(skyBeam.LightEnergy, beamEnergyBefore), "Activated beacon sky beam is not pulsing");
        Require(navigator.LastRequestedScenePath == "res://scenes/UI/VictoryScreen.tscn", "Victory screen was not requested after the holdout");
        LogMvpSmokeMilestone(9, "victory reached");

        main.QueueFree();
        await Frames(2);

        var defeatMain = LoadScene<Node3D>(MainScenePath);
        AddChild(defeatMain);
        await Frames(2);
        var defeatedPlayer = defeatMain.GetNode<FirstPersonController>("Player");
        var defeatHud = defeatMain.GetNode<CrashSiteHud>("HUD");
        var defeatNavigator = defeatMain.GetNode<CrashSiteEndScreenNavigator>("EndScreenNavigator");
        RequireHudObjective(defeatHud, CrashSiteMissionStep.ActivateBeacon, "defeat path before death after checkpoint continuation");
        defeatNavigator.EnableSceneChanges = false;
        defeatedPlayer.Health.ApplyDamage(PlayerHealth.DefaultMaxHealth);
        await Frames(2);
        Require(defeatedPlayer.Health.IsDead, "Defeat path did not kill the player");
        Require(defeatNavigator.LastRequestedScenePath == "res://scenes/UI/DefeatScreen.tscn", "Defeat path did not request the defeat screen");
        Require(defeatHud.GetNode<Label>("ActionFeedback").Text == FirstPersonController.DefeatRetryFeedback, "Defeat path did not emit retry feedback through the HUD");
        RequireHudObjective(defeatHud, CrashSiteMissionStep.ActivateBeacon, "after defeat/retry feedback from checkpoint continuation");
        Require(!defeatedPlayer.Mission.IsVictory, "Defeat path incorrectly set victory state");
        LogMvpSmokeMilestone(10, "defeat path verified");
        defeatMain.QueueFree();
        await Frames(2);

        var continuation = LoadScene<Node3D>(MainScenePath);
        AddChild(continuation);
        await Frames(2);
        var continuationPlayer = continuation.GetNode<FirstPersonController>("Player");
        var continuationHud = continuation.GetNode<CrashSiteHud>("HUD");
        Require(continuation.GetNode<CrashSiteSaveCoordinator>("SaveCoordinator").LastLoadSucceeded, "Save continuation did not load the checkpoint");
        Require(continuationPlayer.Mission.CurrentStep == CrashSiteMissionStep.ActivateBeacon, "Save continuation did not restore beacon objective");
        Require(continuationPlayer.Inventory.HasBreakerCoil && continuationPlayer.Inventory.HasPowerCore, "Save continuation did not restore the Mk III arm and the power core");
        await Frames(2);
        Require(continuation.GetNode<PullableBarricade>("NetYard/WreckBarricade").IsOpen, "Save continuation closed the yard again");
        Require(continuation.GetNode<BreakableWall>("TowerCourt/CrackedGate").IsBroken, "Save continuation rebuilt the tower gate");
        Require(continuation.GetNode<GalaxabrainWarden>("TowerCourt/Warden").Brain.IsDead, "Save continuation revived the Warden");
        Require(continuation.GetNode<DroneNestEncounter>("NetYard/DroneNest").Drones.All(drone => drone.Brain.IsDead), "Save continuation revived the nest drones");
        Require(continuation.GetNodeOrNull("ActTwoSalvage/ResourceDrop_HarbourMetal") is null || continuation.GetNode<ResourceDrop>("ActTwoSalvage/ResourceDrop_HarbourMetal").IsQueuedForDeletion(), "Save continuation handed collected salvage out again");
        RequireHudObjective(continuationHud, CrashSiteMissionStep.ActivateBeacon, "after save continuation");
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
        Require(reloadedScout.Visible, "Defeated Galaxabrain corpse should stay visible after reload");
        Require(!reloadedScout.GetNode<Node3D>("V1BetaScoutVisualRoot").Visible, "Alive Scout visual became visible again after reload");
        Require(reloadedScout.GetNode<Node3D>("V1BetaScoutDisabledVisualRoot").Visible, "Disabled Scout visual was not restored after reload");
        Require(reloadedScout.GetNode<CollisionShape3D>("CollisionShape3D").Disabled, "Restored dead Galaxabrain body collider stayed enabled");
        Require(reloadedComponent.Visible && reloadedComponent.Monitoring, "Unrecovered component was not available after reload");
        Require(reloadedPlayer.Inventory.IsMechanicalArmBuilt, "Mechanical arm ownership was lost across reload");
        Require(reloadedPlayer.GetNode<MeshInstance3D>("Head/Camera3D/MechanicalArmVisual").Visible, "Mechanical arm visual was not restored after reload");
        Require(!reloadedPlayer.GetNode<MeshInstance3D>("Head/Camera3D/BareArmVisual").Visible, "Bare astronaut arm reappeared after reload with the mechanical arm built");
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
        Require(finalScout.Brain.IsDead && finalScout.Visible, "Defeated Galaxabrain revived on the second reload");
        Require(finalScout.GetNode<Node3D>("V1BetaScoutDisabledVisualRoot").Visible, "Disabled Scout visual was not restored on the second reload");
        Require(!finalComponent.Visible && !finalComponent.Monitoring, "Recovered component became interactable again after reload");
        Require(!finalComponent.Interact(finalPlayer.Inventory, finalPlayer.Mission), "Recovered component could be collected twice across reloads");
        Require(finalPlayer.Inventory.HasGalaxabrainComponent, "Recovered component was lost across reload");
        Require(!finalBeacon.IsActivated, "Beacon restored as active before activation");
        Require(finalPlayer.Mission.CurrentStep == CrashSiteMissionStep.UpgradeArmMk2, "Mission step after recovery was not restored");
        Require(!finalBeacon.Interact(finalPlayer.Inventory, finalPlayer.Mission), "The beacon took the Scout's component as its power core");
        Require(finalNavigator.LastRequestedScenePath == string.Empty, "A reload past the Scout requested an end screen");
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

    /// <summary>
    /// The ambient loops autoplay in every Main instance. A freed player's
    /// playback is dropped by the audio mix thread, which runs on wall-clock
    /// time, not per physics frame; quitting before it has run reports the
    /// streams as leaked at exit. Wait a little real time before quitting.
    /// </summary>
    private async System.Threading.Tasks.Task DrainAudioPlaybacks()
    {
        var until = Time.GetTicksMsec() + 250;
        while (Time.GetTicksMsec() < until)
            await Frames(1);
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
