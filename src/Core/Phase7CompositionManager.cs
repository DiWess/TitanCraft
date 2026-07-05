using Godot;
using System;
using System.Collections.Generic;

namespace TitanCraft.Core;

/// <summary>
/// Manages Phase 7 scene composition: terrain, hull, props, pickups, enemies, and player avatar.
/// Instantiates and positions all MVP assets according to PHASE_7_EXECUTION_GUIDE specifications.
/// </summary>
public partial class Phase7CompositionManager : Node3D
{
	private const string ASSETS_PATH = "res://assets/Production/Generated/MVP_Pack_V1/";

	// Asset paths
	private const string TERRAIN_GLB = ASSETS_PATH + "TC_TERRAIN_CrashBasin_V1.glb";
	private const string HULL_GLB = ASSETS_PATH + "TC_CRASH_HullMk1_V1.glb";

	private const string WORKBENCH_GLB = ASSETS_PATH + "TC_PROP_Workbench_V1.glb";
	private const string BEACON_DORMANT_GLB = ASSETS_PATH + "TC_PROP_Beacon_Dormant_V1.glb";
	private const string BEACON_ACTIVE_GLB = ASSETS_PATH + "TC_PROP_Beacon_Active_V1.glb";
	private const string SAVE_POINT_GLB = ASSETS_PATH + "TC_PROP_SavePoint_V1.glb";

	private const string SCOUT_GLB = ASSETS_PATH + "TC_CHAR_GalaxabrainScout_V1.glb";
	private const string ARM_GLB = ASSETS_PATH + "TC_PLAYER_MechanicalArm_V1.glb";

	// Pickup asset paths
	private readonly Dictionary<string, string> PickupAssets = new()
	{
		{ "Metal", ASSETS_PATH + "TC_PICKUP_Metal_V1.glb" },
		{ "Biomass", ASSETS_PATH + "TC_PICKUP_Biomass_V1.glb" },
		{ "Electronics", ASSETS_PATH + "TC_PICKUP_Electronics_V1.glb" },
		{ "Component", ASSETS_PATH + "TC_PICKUP_Component_V1.glb" },
	};

	// Pickup cluster definitions (from PHASE_7_EXECUTION_GUIDE)
	private readonly List<PickupSpawn> PickupSpawns = new()
	{
		// Cluster 1: Near Spawn (Early Exploration)
		new() { Type = "Metal", Position = new Vector3(5, 8, 0.1f) },
		new() { Type = "Electronics", Position = new Vector3(-3, 10, 0.1f) },

		// Cluster 2: Midfield (Resource Zone)
		new() { Type = "Biomass", Position = new Vector3(-15, 20, 0.1f) },
		new() { Type = "Component", Position = new Vector3(12, 22, 0.1f) },

		// Cluster 3: Arena/Danger Zone
		new() { Type = "Metal", Position = new Vector3(25, 40, 0.2f) },
	};

	public override void _Ready()
	{
		GD.Print("=== Phase 7 Scene Composition Started ===");

		ComposeScene();

		GD.Print("=== Phase 7 Scene Composition Complete ===");
	}

	private void ComposeScene()
	{
		// Step 1: Terrain placement
		PlaceTerrain();

		// Step 2: Hull placement
		PlaceHull();

		// Step 3: Interactive props
		PlaceWorkbench();
		PlaceSavePoint();
		PlaceBeacon();

		// Step 4: Pickups
		PlacePickups();

		// Step 5: Enemy
		PlaceEnemy();

		// Step 6: Player setup
		PlacePlayer();
	}

	private void PlaceTerrain()
	{
		if (!ResourceLoader.Exists(TERRAIN_GLB))
		{
			GD.PrintErr($"Terrain GLB not found: {TERRAIN_GLB}");
			return;
		}

		var terrain = GD.Load<PackedScene>(TERRAIN_GLB).Instantiate() as Node3D;
		if (terrain == null) return;

		terrain.Name = "Terrain";
		terrain.Position = Vector3.Zero;
		AddChild(terrain);
		GD.Print("✓ Terrain placed at origin");
	}

	private void PlaceHull()
	{
		if (!ResourceLoader.Exists(HULL_GLB))
		{
			GD.PrintErr($"Hull GLB not found: {HULL_GLB}");
			return;
		}

		var hull = GD.Load<PackedScene>(HULL_GLB).Instantiate() as Node3D;
		if (hull == null) return;

		hull.Name = "CrashHull";
		hull.Position = new Vector3(45, 30, 5);
		hull.RotationDegrees = new Vector3(15, -30, 8);
		AddChild(hull);
		GD.Print("✓ Hull placed at (45, 30, 5) with crash tilt");
	}

	private void PlaceWorkbench()
	{
		if (!ResourceLoader.Exists(WORKBENCH_GLB))
		{
			GD.PrintErr($"Workbench GLB not found: {WORKBENCH_GLB}");
			return;
		}

		var workbench = GD.Load<PackedScene>(WORKBENCH_GLB).Instantiate() as Node3D;
		if (workbench == null) return;

		workbench.Name = "Workbench";
		workbench.Position = new Vector3(0, 25, 0);
		workbench.RotationDegrees = new Vector3(0, 180, 0);
		AddChild(workbench);
		GD.Print("✓ Workbench placed at (0, 25, 0) - central hub");
	}

	private void PlaceSavePoint()
	{
		if (!ResourceLoader.Exists(SAVE_POINT_GLB))
		{
			GD.PrintErr($"SavePoint GLB not found: {SAVE_POINT_GLB}");
			return;
		}

		var savePoint = GD.Load<PackedScene>(SAVE_POINT_GLB).Instantiate() as Node3D;
		if (savePoint == null) return;

		savePoint.Name = "SavePoint";
		savePoint.Position = new Vector3(-8, 22, 0);
		savePoint.RotationDegrees = Vector3.Zero;
		AddChild(savePoint);
		GD.Print("✓ Save Point placed at (-8, 22, 0) - checkpoint refuge");
	}

	private void PlaceBeacon()
	{
		if (!ResourceLoader.Exists(BEACON_DORMANT_GLB))
		{
			GD.PrintErr($"Beacon Dormant GLB not found: {BEACON_DORMANT_GLB}");
			return;
		}

		var beaconDormant = GD.Load<PackedScene>(BEACON_DORMANT_GLB).Instantiate() as Node3D;
		if (beaconDormant == null) return;

		beaconDormant.Name = "BeaconDormant";
		beaconDormant.Position = new Vector3(50, 35, 5);
		beaconDormant.RotationDegrees = new Vector3(0, 45, 0);
		AddChild(beaconDormant);
		GD.Print("✓ Beacon (Dormant) placed at (50, 35, 5) - victory objective");
	}

	private void PlacePickups()
	{
		var pickupsNode = new Node3D { Name = "Pickups" };
		AddChild(pickupsNode);

		int pickupCount = 0;
		foreach (var spawn in PickupSpawns)
		{
			if (string.IsNullOrEmpty(spawn.Type) || !PickupAssets.TryGetValue(spawn.Type, out var assetPath))
			{
				GD.PrintErr($"Unknown pickup type: {spawn.Type}");
				continue;
			}

			if (!ResourceLoader.Exists(assetPath))
			{
				GD.PrintErr($"Pickup GLB not found: {assetPath}");
				continue;
			}

			var pickup = GD.Load<PackedScene>(assetPath).Instantiate() as Node3D;
			if (pickup == null) continue;

			pickup.Name = $"Pickup_{spawn.Type}_{pickupCount}";
			pickup.Position = spawn.Position;
			pickupsNode.AddChild(pickup);
			pickupCount++;
		}

		GD.Print($"✓ Placed {pickupCount} pickups in 3 clusters");
	}

	private void PlaceEnemy()
	{
		if (!ResourceLoader.Exists(SCOUT_GLB))
		{
			GD.PrintErr($"Scout GLB not found: {SCOUT_GLB}");
			return;
		}

		var scout = GD.Load<PackedScene>(SCOUT_GLB).Instantiate() as Node3D;
		if (scout == null) return;

		scout.Name = "Scout_Arena_01";
		scout.Position = new Vector3(35, 45, 0.5f);
		scout.RotationDegrees = Vector3.Zero;
		AddChild(scout);
		GD.Print("✓ Scout enemy placed at (35, 45, 0.5) - arena zone threat");
	}

	private void PlacePlayer()
	{
		// Create player node
		var player = new CharacterBody3D { Name = "Player" };
		player.Position = new Vector3(0, 0, 1.7f);

		// Create first-person camera
		var camera = new Camera3D { Name = "Camera3D" };
		camera.Fov = 75;
		camera.Near = 0.1f;
		camera.Far = 1000;
		player.AddChild(camera);
		camera.Owner = player;

		// Attach mechanical arm to camera (lower-right FPS corner)
		if (ResourceLoader.Exists(ARM_GLB))
		{
			var arm = GD.Load<PackedScene>(ARM_GLB).Instantiate() as Node3D;
			if (arm != null)
			{
				arm.Name = "MechanicalArm";
				arm.Position = new Vector3(0.15f, -0.3f, -0.5f);
				camera.AddChild(arm);
				arm.Owner = camera;
				GD.Print("✓ Mechanical arm attached to camera (FPS POV)");
			}
		}
		else
		{
			GD.PrintErr($"Mechanical Arm GLB not found: {ARM_GLB}");
		}

		AddChild(player);
		camera.Current = true;
		GD.Print("✓ Player spawned at (0, 0, 1.7) with camera and arm");
	}

	/// <summary>
	/// Helper class for pickup spawn definitions
	/// </summary>
	private class PickupSpawn
	{
		public string? Type { get; set; }
		public Vector3 Position { get; set; }
	}
}
