using System;
using System.IO;
using Godot;
using TitanCraft.Enemies;
using TitanCraft.Missions;
using TitanCraft.Player;
using TitanCraft.SaveSystem;
using TitanCraft.UI;
using TitanCraft.World;

namespace TitanCraft.Tests.Integration;

/// <summary>
/// Drives the complete Crash Site MVP loop against the real Main scene and captures
/// screenshot evidence at each release-gate moment. Requires a rendering context
/// (run under xvfb or a real display), unlike the headless IntegrationTestRunner.
/// </summary>
public partial class MvpPlaythroughCapture : Node
{
    private const string MainScenePath = "res://scenes/Main/Main.tscn";
    private const string VictoryScenePath = "res://scenes/UI/VictoryScreen.tscn";
    private const string OutputDir = "res://artifacts/mvp_closure/playthrough";

    public override async void _Ready()
    {
        try
        {
            await Run();
            GD.Print("MVP_PLAYTHROUGH_CAPTURE_PASS");
            GetTree().Quit(0);
        }
        catch (Exception exception)
        {
            GD.PushError(exception.ToString());
            GetTree().Quit(1);
        }
    }

    private async System.Threading.Tasks.Task Run()
    {
        Directory.CreateDirectory(ProjectSettings.GlobalizePath(OutputDir));
        LocalSaveGameStore.DeleteSave();

        var main = LoadScene<Node3D>(MainScenePath);
        AddChild(main);
        await Frames(30);

        var player = main.GetNode<FirstPersonController>("Player");
        var navigator = main.GetNode<CrashSiteEndScreenNavigator>("EndScreenNavigator");
        navigator.EnableSceneChanges = false;
        var scout = main.GetNode<GalaxabrainScout>("Placeholder_GalaxabrainScout");
        var component = scout.GetNode<GalaxabrainComponentPickup>("GalaxabrainComponentPickup");
        var workbench = main.GetNode<Workbench>("Placeholder_Workbench");
        var beacon = main.GetNode<Beacon>("Placeholder_Beacon");
        var savePoint = main.GetNode<SavePoint>("Placeholder_SavePoint");

        await Capture("01_initial_spawn");

        foreach (var pickupName in new[] { "Placeholder_MetalPickup", "Placeholder_BiomassPickup", "Placeholder_ElectronicsPickup" })
        {
            var pickup = main.GetNode<ResourcePickup>(pickupName);
            Require(pickup.Interact(player.Inventory, player.Mission), $"{pickupName} collection failed");
        }
        Require(player.Mission.CurrentStep == CrashSiteMissionStep.BuildMechanicalArm, "Resource collection did not advance the mission");
        await Capture("02_resources_collected");

        MovePlayerToSee(player, workbench.GlobalPosition);
        Require(workbench.Interact(player.Inventory, player.Mission), "Crafting failed");
        Require(player.GetNode<MeshInstance3D>("Head/Camera3D/MechanicalArmVisual").Visible, "Arm visual hidden after craft");
        await Capture("03_arm_crafted_visible");

        MovePlayerToSee(player, scout.GlobalPosition);
        for (var hit = 0; hit < 4; hit++)
            scout.ApplyDamage(MechanicalArmAttackLogic.DefaultMechanicalArmDamage);
        Require(scout.Brain.IsDead, "Scout survived");
        Require(player.Mission.CurrentStep == CrashSiteMissionStep.RecoverGalaxabrainComponent, "Death did not advance mission");
        await Capture("04_enemy_defeated_objective_updated");

        Require(component.Interact(player.Inventory, player.Mission), "Component recovery failed");
        Require(player.Mission.CurrentStep == CrashSiteMissionStep.ActivateBeacon, "Recovery did not advance mission");
        await Capture("05_component_recovered");

        MovePlayerToSee(player, savePoint.GlobalPosition);
        Require(savePoint.Interact(player.Inventory, player.Mission), "Checkpoint save failed");

        MovePlayerToSee(player, beacon.GlobalPosition);
        Require(beacon.Interact(player.Inventory, player.Mission), "Beacon activation failed");
        Require(player.Mission.IsVictory, "Victory not reached");
        await Capture("06_beacon_active");

        main.QueueFree();
        await Frames(2);

        var victoryScreen = LoadScene<Control>(VictoryScenePath);
        AddChild(victoryScreen);
        await Capture("07_victory_screen");
        victoryScreen.QueueFree();
        await Frames(2);

        var reloaded = LoadScene<Node3D>(MainScenePath);
        AddChild(reloaded);
        await Frames(30);
        reloaded.GetNode<CrashSiteEndScreenNavigator>("EndScreenNavigator").EnableSceneChanges = false;
        var reloadedPlayer = reloaded.GetNode<FirstPersonController>("Player");
        Require(reloaded.GetNode<CrashSiteSaveCoordinator>("SaveCoordinator").LastLoadSucceeded, "Reload did not restore the checkpoint");
        Require(reloadedPlayer.Mission.CurrentStep == CrashSiteMissionStep.ActivateBeacon, "Reloaded mission step mismatch");
        Require(reloadedPlayer.Inventory.IsMechanicalArmBuilt && reloadedPlayer.Inventory.HasGalaxabrainComponent, "Reloaded inventory mismatch");
        Require(reloaded.GetNode<GalaxabrainScout>("Placeholder_GalaxabrainScout").Brain.IsDead, "Reloaded scout came back to life");
        await Capture("08_restored_save_state");
        reloaded.QueueFree();
        await Frames(2);
        LocalSaveGameStore.DeleteSave();
    }

    private static void MovePlayerToSee(FirstPersonController player, Vector3 target)
    {
        var eyeTarget = new Vector3(target.X, player.GlobalPosition.Y, target.Z);
        var back = (player.GlobalPosition with { Y = target.Y } - target).Normalized();
        if (!back.IsFinite() || back.LengthSquared() < 0.01f)
            back = Vector3.Back;
        player.GlobalPosition = target + back * 4.0f + Vector3.Up * 1.2f;
        player.LookAt(eyeTarget, Vector3.Up);
        player.Rotation = new Vector3(0.0f, player.Rotation.Y, 0.0f);
    }

    private async System.Threading.Tasks.Task Capture(string name)
    {
        await Frames(6);
        var image = GetViewport().GetTexture().GetImage();
        Require(image is not null && !image.IsEmpty(), $"Viewport image empty for {name}");
        var path = $"{OutputDir}/{name}.png";
        Require(image!.SavePng(path) == Error.Ok, $"Failed to save {path}");
        GD.Print($"CAPTURED {path}");
    }

    private async System.Threading.Tasks.Task Frames(int count)
    {
        for (var i = 0; i < count; i++)
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
    }

    private static T LoadScene<T>(string path) where T : Node
    {
        var scene = ResourceLoader.Load<PackedScene>(path)
            ?? throw new InvalidOperationException($"Failed to load {path}");
        return scene.Instantiate<T>();
    }

    private static void Require(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }
}
