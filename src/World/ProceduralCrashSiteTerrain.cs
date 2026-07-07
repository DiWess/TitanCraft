using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Godot;

namespace TitanCraft.World;

public partial class ProceduralCrashSiteTerrain : Node3D
{
    public const int Seed = 31071;
    public const float GridSpacing = 2.0f;
    public const float VisualMargin = 18.0f;
    public const float CorridorWidth = 5.0f;
    public const float CorridorHeight = 0.03f;
    public const float CorridorTolerance = 0.06f;
    public const float MaxOutsideHeight = 4.2f;
    public const float MaxSlope = 1.35f;
    public const float MinimumRouteSurfaceArea = 270.0f;
    private const string MeshNodeName = "TerrainMesh";
    public static readonly string[] RequiredSemanticMeshes = { "RouteSurface", "CentralPlateau", "SpawnBasaltShelf", "ResourceBasaltShelf", "WorkbenchRidge", "CombatRidge", "BeaconShelf", "CraterNorthwest", "CraterSoutheast", "HorizonSegments" };

    private static readonly string[] TargetPaths =
    {
        "Player",
        "ResourceDrop_MetalPickup",
        "ResourceDrop_BiomassPickup",
        "ResourceDrop_ElectronicsPickup",
        "Placeholder_Workbench",
        "Placeholder_GalaxabrainScout",
        "Placeholder_GalaxabrainScout/GalaxabrainComponentPickup",
        "Placeholder_SavePoint",
        "Placeholder_Beacon"
    };

    public override void _Ready()
    {
        Node3D worldRoot = GetParentOrNull<Node3D>() ?? this;
        TerrainBuild build = BuildForWorld(worldRoot);
        MeshInstance3D legacyMesh = GetNode<MeshInstance3D>(MeshNodeName);
        legacyMesh.Mesh = build.Mesh;
        legacyMesh.Visible = false;
        RebuildSemanticChildren(build.Report);
    }

    public static TerrainBuild BuildForWorld(Node3D worldRoot)
    {
        Dictionary<string, Vector3> targets = ReadTargets(worldRoot);
        TerrainReport report = GenerateReport(targets);
        ArrayMesh mesh = BuildMesh(report);
        return new TerrainBuild(mesh, report);
    }

    private void RebuildSemanticChildren(TerrainReport report)
    {
        foreach (string childName in RequiredSemanticMeshes)
            GetNodeOrNull<Node>(childName)?.QueueFree();
        foreach ((string name, ArrayMesh mesh, Color color) in BuildSemanticMeshes(report))
        {
            if (name == "HorizonSegments")
                continue;
            AddSemanticMesh(name, mesh, color);
        }
        var horizonRoot = new Node3D { Name = "HorizonSegments" };
        AddChild(horizonRoot);
        int index = 1;
        foreach ((string name, ArrayMesh mesh, Color color) in BuildHorizonSegmentMeshes(report))
        {
            var instance = new MeshInstance3D { Name = $"HorizonSegment_{index:00}", Mesh = mesh, MaterialOverride = Material(color) };
            horizonRoot.AddChild(instance);
            index++;
        }
    }

    private void AddSemanticMesh(string name, ArrayMesh mesh, Color color)
    {
        var instance = new MeshInstance3D { Name = name, Mesh = mesh, MaterialOverride = Material(color) };
        AddChild(instance);
    }

    private static StandardMaterial3D Material(Color color) => new() { AlbedoColor = color, Roughness = 0.92f };

    public static Dictionary<string, Vector3> ReadTargets(Node worldRoot)
    {
        var targets = new Dictionary<string, Vector3>();
        foreach (string path in TargetPaths)
        {
            Node3D node = worldRoot.GetNode<Node3D>(path);
            targets[path] = node.GlobalPosition;
        }
        return targets;
    }

    public static TerrainReport GenerateReport(IReadOnlyDictionary<string, Vector3> targets)
    {
        float minX = float.PositiveInfinity, maxX = float.NegativeInfinity, minZ = float.PositiveInfinity, maxZ = float.NegativeInfinity;
        foreach (Vector3 position in targets.Values)
        {
            minX = MathF.Min(minX, position.X);
            maxX = MathF.Max(maxX, position.X);
            minZ = MathF.Min(minZ, position.Z);
            maxZ = MathF.Max(maxZ, position.Z);
        }

        minX = MathF.Floor((minX - VisualMargin) / GridSpacing) * GridSpacing;
        maxX = MathF.Ceiling((maxX + VisualMargin) / GridSpacing) * GridSpacing;
        minZ = MathF.Floor((minZ - VisualMargin) / GridSpacing) * GridSpacing;
        maxZ = MathF.Ceiling((maxZ + VisualMargin) / GridSpacing) * GridSpacing;

        int columns = (int)MathF.Round((maxX - minX) / GridSpacing) + 1;
        int rows = (int)MathF.Round((maxZ - minZ) / GridSpacing) + 1;
        var heights = new float[columns, rows];
        var zones = new TerrainZone[columns, rows];
        var zoneTriangles = Enum.GetValues<TerrainZone>().ToDictionary(zone => zone, _ => 0);
        float minHeight = float.PositiveInfinity, maxHeight = float.NegativeInfinity, maxRouteDeviation = 0.0f, maxSlope = 0.0f;
        float routeArea = 0.0f, shelfArea = 0.0f;

        for (int z = 0; z < rows; z++)
        {
            for (int x = 0; x < columns; x++)
            {
                float worldX = minX + x * GridSpacing;
                float worldZ = minZ + z * GridSpacing;
                float distance = DistanceToRoute(worldX, worldZ, targets);
                TerrainZone zone = ZoneAt(worldX, worldZ, distance, targets, minX, maxX, minZ, maxZ);
                float height = HeightAt(worldX, worldZ, distance, zone, minX, maxX, minZ, maxZ);
                heights[x, z] = height;
                zones[x, z] = zone;
                minHeight = MathF.Min(minHeight, height);
                maxHeight = MathF.Max(maxHeight, height);
                if (distance <= CorridorWidth * 0.5f)
                {
                    maxRouteDeviation = MathF.Max(maxRouteDeviation, MathF.Abs(height - CorridorHeight));
                    routeArea += GridSpacing * GridSpacing;
                }
                if (zone is TerrainZone.SpawnBasaltShelf or TerrainZone.ResourceBasaltShelf or TerrainZone.BeaconBasaltShelf)
                    shelfArea += GridSpacing * GridSpacing;
            }
        }

        for (int z = 0; z < rows - 1; z++)
        {
            for (int x = 0; x < columns - 1; x++)
            {
                maxSlope = MathF.Max(maxSlope, MathF.Abs(heights[x + 1, z] - heights[x, z]) / GridSpacing);
                maxSlope = MathF.Max(maxSlope, MathF.Abs(heights[x, z + 1] - heights[x, z]) / GridSpacing);
                TerrainZone zone = DominantZone(zones[x, z], zones[x + 1, z], zones[x + 1, z + 1], zones[x, z + 1]);
                zoneTriangles[zone] += 2;
            }
        }

        int quadCount = (columns - 1) * (rows - 1);
        int triangleCount = quadCount * 2;
        int vertexCount = triangleCount * 3;
        var features = BuildFeatures(targets, minX, maxX, minZ, maxZ);
        var report = new TerrainReport(targets, features, minX, maxX, minZ, maxZ, columns, rows, vertexCount, triangleCount, minHeight, maxHeight, maxRouteDeviation, maxSlope, routeArea, shelfArea, heights, zones, zoneTriangles);
        report.MeshDataHash = ComputeHash(report);
        return report;
    }

    public static IReadOnlyList<(string Name, ArrayMesh Mesh, Color Color)> BuildSemanticMeshes(TerrainReport report)
    {
        Vector3[] route = RoutePoints(report.Targets).ToArray();
        return new List<(string, ArrayMesh, Color)>
        {
            ("RouteSurface", BuildRouteRibbon(route, CorridorWidth * 1.35f, CorridorHeight + 0.012f, ColorFor(TerrainZone.AshRoute, 0)), ColorFor(TerrainZone.AshRoute, 0)),
            ("CentralPlateau", BuildPrism(IrregularPolygon(new Vector2(8,-12), 28, 14, 0.18f), 0.018f, -0.18f, ColorFor(TerrainZone.CentralPlateau, 0)), ColorFor(TerrainZone.CentralPlateau, 0)),
            ("SpawnBasaltShelf", BuildPrism(IrregularPolygon(new Vector2(-14,-6), 7, 8, 0.42f), 0.58f, 0.04f, ColorFor(TerrainZone.SpawnBasaltShelf, 0)), ColorFor(TerrainZone.SpawnBasaltShelf, 0)),
            ("ResourceBasaltShelf", BuildPrism(IrregularPolygon(new Vector2(3,-6), 9, 9, 0.73f), 0.70f, 0.04f, ColorFor(TerrainZone.ResourceBasaltShelf, 0)), ColorFor(TerrainZone.ResourceBasaltShelf, 0)),
            ("WorkbenchRidge", BuildRidge(new [] { new Vector2(6,-22), new Vector2(11,-25), new Vector2(18,-23) }, 3.8f, 0.04f, 1.50f, ColorFor(TerrainZone.WorkbenchRidge, 0)), ColorFor(TerrainZone.WorkbenchRidge, 0)),
            ("CombatRidge", BuildRidge(new [] { new Vector2(16,-23), new Vector2(23,-27), new Vector2(31,-24) }, 4.3f, 0.04f, 1.85f, ColorFor(TerrainZone.CombatRidge, 0)), ColorFor(TerrainZone.CombatRidge, 0)),
            ("BeaconShelf", BuildPrism(IrregularPolygon(new Vector2(33,-24), 8, 9, 1.16f), 1.22f, 0.04f, ColorFor(TerrainZone.BeaconBasaltShelf, 0)), ColorFor(TerrainZone.BeaconBasaltShelf, 0)),
            ("CraterNorthwest", BuildCrater(new Vector2(-20,-24), 6.5f, ColorFor(TerrainZone.ImpactCrater, 0)), ColorFor(TerrainZone.ImpactCrater, 0)),
            ("CraterSoutheast", BuildCrater(new Vector2(34,-32), 7.5f, ColorFor(TerrainZone.ImpactCrater, 0)), ColorFor(TerrainZone.ImpactCrater, 0))
        };
    }

    public static IReadOnlyList<(string Name, ArrayMesh Mesh, Color Color)> BuildHorizonSegmentMeshes(TerrainReport report)
    {
        var centers = new[] { new Vector2(-22,-34), new Vector2(-6,-37), new Vector2(12,-36), new Vector2(31,-35), new Vector2(42,-22), new Vector2(-28,-17) };
        var result = new List<(string, ArrayMesh, Color)>();
        for (int i = 0; i < centers.Length; i++)
        {
            Vector2 c = centers[i];
            var poly = new[] { c + new Vector2(-5,1.4f), c + new Vector2(1,2.4f), c + new Vector2(7,1.2f), c + new Vector2(4,-1.5f), c + new Vector2(-2,-2.5f), c + new Vector2(-6,-1) };
            result.Add(($"HorizonSegment_{i+1:00}", BuildPrism(poly, 1.8f + i * 0.22f, 0.02f, ColorFor(TerrainZone.HorizonRidge, 0)), ColorFor(TerrainZone.HorizonRidge, 0)));
        }
        return result;
    }

    private static IEnumerable<Vector3> RoutePoints(IReadOnlyDictionary<string, Vector3> targets)
    {
        foreach (string key in new[] { "Player", "ResourceDrop_MetalPickup", "ResourceDrop_BiomassPickup", "ResourceDrop_ElectronicsPickup", "Placeholder_Workbench", "Placeholder_GalaxabrainScout", "Placeholder_GalaxabrainScout/GalaxabrainComponentPickup", "Placeholder_SavePoint", "Placeholder_Beacon" })
            yield return new Vector3(targets[key].X, CorridorHeight + 0.012f, targets[key].Z);
    }

    private static ArrayMesh BuildRouteRibbon(IReadOnlyList<Vector3> points, float width, float y, Color color)
    {
        var left = new List<Vector3>(); var right = new List<Vector3>(); float half = width * 0.5f;
        for (int i = 0; i < points.Count; i++)
        {
            Vector2 dir = Vector2.Zero;
            if (i > 0) dir += (Xz(points[i]) - Xz(points[i - 1])).Normalized();
            if (i < points.Count - 1) dir += (Xz(points[i + 1]) - Xz(points[i])).Normalized();
            if (dir.LengthSquared() < 0.001f) dir = Vector2.Right; else dir = dir.Normalized();
            Vector2 n = new(-dir.Y, dir.X);
            left.Add(new Vector3(points[i].X + n.X * half, y, points[i].Z + n.Y * half));
            right.Add(new Vector3(points[i].X - n.X * half, y, points[i].Z - n.Y * half));
        }
        var v = new List<Vector3>(); var colors = new List<Color>(); var normals = new List<Vector3>();
        for (int i = 0; i < points.Count - 1; i++) { AddTriangle(v,normals,colors,left[i],left[i+1],right[i+1],TerrainZone.AshRoute); AddTriangle(v,normals,colors,left[i],right[i+1],right[i],TerrainZone.AshRoute); }
        return MeshFrom(v,normals,colors);
    }

    private static ArrayMesh BuildPrism(IReadOnlyList<Vector2> polygon, float topY, float baseY, Color color)
    {
        var v = new List<Vector3>(); var n = new List<Vector3>(); var c = new List<Color>();
        Vector2 center = polygon.Aggregate(Vector2.Zero, (a,b) => a + b) / polygon.Count;
        Vector3 topCenter = new(center.X, topY, center.Y);
        for (int i = 0; i < polygon.Count; i++)
        {
            Vector2 a2 = polygon[i], b2 = polygon[(i+1)%polygon.Count];
            Vector3 a = new(a2.X, topY, a2.Y), b = new(b2.X, topY, b2.Y), abase = new(a2.X, baseY, a2.Y), bbase = new(b2.X, baseY, b2.Y);
            AddColored(v,n,c,topCenter,a,b,color); AddColored(v,n,c,a,abase,bbase,color); AddColored(v,n,c,a,bbase,b,color);
        }
        return MeshFrom(v,n,c);
    }

    private static ArrayMesh BuildRidge(IReadOnlyList<Vector2> points, float halfWidth, float baseY, float crestY, Color color)
    {
        var v = new List<Vector3>(); var n = new List<Vector3>(); var c = new List<Color>();
        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector2 dir = (points[i+1] - points[i]).Normalized(); Vector2 normal = new(-dir.Y, dir.X);
            Vector3 l0 = V(points[i] + normal * halfWidth, baseY), m0 = V(points[i], crestY), r0 = V(points[i] - normal * halfWidth, baseY);
            Vector3 l1 = V(points[i+1] + normal * halfWidth, baseY), m1 = V(points[i+1], crestY + 0.18f * i), r1 = V(points[i+1] - normal * halfWidth, baseY);
            AddColored(v,n,c,l0,l1,m1,color); AddColored(v,n,c,l0,m1,m0,color); AddColored(v,n,c,m0,m1,r1,color); AddColored(v,n,c,m0,r1,r0,color);
        }
        return MeshFrom(v,n,c);
    }

    private static ArrayMesh BuildCrater(Vector2 center, float radius, Color color)
    {
        var v = new List<Vector3>(); var n = new List<Vector3>(); var c = new List<Color>(); int count = 13;
        for (int i = 0; i < count; i++)
        {
            float a0 = Mathf.Tau * i / count, a1 = Mathf.Tau * ((i+1)%count) / count;
            float r0 = radius * (0.88f + 0.16f * Hash01(i, count)), r1 = radius * (0.88f + 0.16f * Hash01(i+1, count));
            Vector2 o0 = center + new Vector2(MathF.Cos(a0), MathF.Sin(a0)) * r0, o1 = center + new Vector2(MathF.Cos(a1), MathF.Sin(a1)) * r1;
            Vector2 i0 = center + new Vector2(MathF.Cos(a0), MathF.Sin(a0)) * radius * 0.48f, i1 = center + new Vector2(MathF.Cos(a1), MathF.Sin(a1)) * radius * 0.48f;
            AddColored(v,n,c,V(o0,0.34f),V(o1,0.30f),V(i1,0.06f),color); AddColored(v,n,c,V(o0,0.34f),V(i1,0.06f),V(i0,0.05f),color); AddColored(v,n,c,V(center,0.025f),V(i0,0.05f),V(i1,0.06f),new Color(0.13f,0.11f,0.095f));
        }
        return MeshFrom(v,n,c);
    }

    private static Vector3 V(Vector2 p, float y) => new(p.X, y, p.Y);

    private static Vector2[] IrregularPolygon(Vector2 center, float radius, int count, float salt)
    {
        var pts = new Vector2[count];
        for (int i = 0; i < count; i++) { float a = Mathf.Tau * i / count + salt; float r = radius * (0.78f + 0.30f * Hash01(i, (int)(salt * 100))); pts[i] = center + new Vector2(MathF.Cos(a), MathF.Sin(a)) * r; }
        return pts;
    }

    private static void AddColored(List<Vector3> vertices, List<Vector3> normals, List<Color> colors, Vector3 a, Vector3 b, Vector3 c, Color color)
    {
        Vector3 normal = (c - a).Cross(b - a).Normalized(); vertices.Add(a); vertices.Add(b); vertices.Add(c); normals.Add(normal); normals.Add(normal); normals.Add(normal); colors.Add(color); colors.Add(color); colors.Add(color);
    }

    private static ArrayMesh MeshFrom(List<Vector3> vertices, List<Vector3> normals, List<Color> colors)
    {
        var arrays = new Godot.Collections.Array(); arrays.Resize((int)Mesh.ArrayType.Max); arrays[(int)Mesh.ArrayType.Vertex] = vertices.ToArray(); arrays[(int)Mesh.ArrayType.Normal] = normals.ToArray(); arrays[(int)Mesh.ArrayType.Color] = colors.ToArray(); var mesh = new ArrayMesh(); mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays); return mesh;
    }

    public static (int Vertices, int Triangles) MeshStats(ArrayMesh mesh)
    {
        var vertices = ((Godot.Collections.Array)mesh.SurfaceGetArrays(0))[(int)Mesh.ArrayType.Vertex].As<Vector3[]>();
        return (vertices.Length, vertices.Length / 3);
    }

    private static ArrayMesh BuildMesh(TerrainReport report)
    {
        var vertices = new List<Vector3>(report.VertexCount);
        var normals = new List<Vector3>(report.VertexCount);
        var colors = new List<Color>(report.VertexCount);
        for (int z = 0; z < report.Rows - 1; z++)
        {
            for (int x = 0; x < report.Columns - 1; x++)
            {
                Vector3 a = report.PositionAt(x, z);
                Vector3 b = report.PositionAt(x + 1, z);
                Vector3 c = report.PositionAt(x + 1, z + 1);
                Vector3 d = report.PositionAt(x, z + 1);
                TerrainZone zone = DominantZone(report.Zones[x, z], report.Zones[x + 1, z], report.Zones[x + 1, z + 1], report.Zones[x, z + 1]);
                AddTriangle(vertices, normals, colors, a, b, c, zone);
                AddTriangle(vertices, normals, colors, a, c, d, zone);
            }
        }

        var arrays = new Godot.Collections.Array();
        arrays.Resize((int)Mesh.ArrayType.Max);
        arrays[(int)Mesh.ArrayType.Vertex] = vertices.ToArray();
        arrays[(int)Mesh.ArrayType.Normal] = normals.ToArray();
        arrays[(int)Mesh.ArrayType.Color] = colors.ToArray();
        var mesh = new ArrayMesh();
        mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);
        return mesh;
    }

    private static void AddTriangle(List<Vector3> vertices, List<Vector3> normals, List<Color> colors, Vector3 a, Vector3 b, Vector3 c, TerrainZone zone)
    {
        Vector3 normal = (c - a).Cross(b - a).Normalized();
        Color color = ColorFor(zone, (a.Y + b.Y + c.Y) / 3.0f);
        vertices.Add(a); vertices.Add(b); vertices.Add(c);
        normals.Add(normal); normals.Add(normal); normals.Add(normal);
        colors.Add(color); colors.Add(color); colors.Add(color);
    }

    public static Color ColorForZone(TerrainZone zone) => ColorFor(zone, CorridorHeight);

    private static Color ColorFor(TerrainZone zone, float height) => zone switch
    {
        TerrainZone.AshRoute => new Color(0.40f, 0.35f, 0.29f),
        TerrainZone.CentralPlateau => new Color(0.27f, 0.24f, 0.20f),
        TerrainZone.SpawnBasaltShelf or TerrainZone.ResourceBasaltShelf or TerrainZone.BeaconBasaltShelf => new Color(0.22f, 0.20f, 0.18f),
        TerrainZone.WorkbenchRidge or TerrainZone.CombatRidge => new Color(0.18f, 0.17f, 0.16f),
        TerrainZone.ImpactCrater => new Color(0.16f, 0.135f, 0.115f),
        TerrainZone.HorizonRidge => new Color(0.145f, 0.135f, 0.13f),
        _ => height < 0.5f ? new Color(0.24f, 0.215f, 0.19f) : new Color(0.20f, 0.185f, 0.17f)
    };

    public static float HeightAt(float x, float z, IReadOnlyDictionary<string, Vector3> targets)
    {
        (float minX, float maxX, float minZ, float maxZ) = Bounds(targets);
        float distance = DistanceToRoute(x, z, targets);
        return HeightAt(x, z, distance, ZoneAt(x, z, distance, targets, minX, maxX, minZ, maxZ), minX, maxX, minZ, maxZ);
    }

    private static float HeightAt(float x, float z, float distanceToRoute, TerrainZone zone, float minX, float maxX, float minZ, float maxZ)
    {
        if (distanceToRoute <= CorridorWidth * 0.5f)
            return CorridorHeight;
        float faceted = FacetNoise(x, z);
        float transition = Mathf.Clamp((distanceToRoute - CorridorWidth * 0.5f) / 9.0f, 0.0f, 1.0f);
        float height = zone switch
        {
            TerrainZone.CentralPlateau => 0.10f + faceted * 0.10f,
            TerrainZone.SpawnBasaltShelf => 0.34f + faceted * 0.22f,
            TerrainZone.ResourceBasaltShelf => 0.42f + faceted * 0.24f,
            TerrainZone.WorkbenchRidge => 1.05f + faceted * 0.45f,
            TerrainZone.CombatRidge => 1.25f + faceted * 0.55f,
            TerrainZone.ImpactCrater => 0.05f + faceted * 0.08f,
            TerrainZone.BeaconBasaltShelf => 0.95f + faceted * 0.38f,
            TerrainZone.HorizonRidge => HorizonHeight(x, z, minX, maxX, minZ, maxZ) + faceted * 0.28f,
            _ => 0.22f + faceted * 0.16f
        };
        return Mathf.Clamp(Mathf.Lerp(CorridorHeight, height, transition), CorridorHeight, MaxOutsideHeight);
    }

    private static TerrainZone ZoneAt(float x, float z, float distanceToRoute, IReadOnlyDictionary<string, Vector3> targets, float minX, float maxX, float minZ, float maxZ)
    {
        if (distanceToRoute <= CorridorWidth * 0.5f)
            return TerrainZone.AshRoute;
        float edge = MathF.Min(MathF.Min(x - minX, maxX - x), MathF.Min(z - minZ, maxZ - z));
        if (edge < 8.5f)
            return TerrainZone.HorizonRidge;
        if (PointInBox(x, z, -18, -5, -14, 4))
            return TerrainZone.SpawnBasaltShelf;
        if (PointInBox(x, z, -9, 13, -11, -2))
            return TerrainZone.ResourceBasaltShelf;
        if (PointInBox(x, z, 6, 18, -27, -18))
            return TerrainZone.WorkbenchRidge;
        if (PointInBox(x, z, 15, 31, -28, -20))
            return TerrainZone.CombatRidge;
        if (PointInBox(x, z, 22, 40, -28, -13))
            return TerrainZone.BeaconBasaltShelf;
        if (InCrater(x, z, targets))
            return TerrainZone.ImpactCrater;
        return TerrainZone.CentralPlateau;
    }

    private static bool PointInBox(float x, float z, float minX, float maxX, float minZ, float maxZ) => x >= minX && x <= maxX && z >= minZ && z <= maxZ;

    private static bool InCrater(float x, float z, IReadOnlyDictionary<string, Vector3> targets)
    {
        foreach (TerrainFeature feature in BuildFeatures(targets, 0, 0, 0, 0).Where(f => f.Id.StartsWith("crater", StringComparison.Ordinal)))
        {
            if (new Vector2(x, z).DistanceTo(feature.Center) <= feature.Radius)
                return true;
        }
        return false;
    }

    private static List<TerrainFeature> BuildFeatures(IReadOnlyDictionary<string, Vector3> targets, float minX, float maxX, float minZ, float maxZ) =>
    [
        new("central_playable_plateau", "continuous low plateau", new Vector2(5, -10), 26, 0.03f, 0.28f),
        new("ash_route_main", "lighter worn-soil route linking mission targets", new Vector2(8, -10), CorridorWidth * 0.5f, CorridorHeight, CorridorHeight),
        new("spawn_left_basalt_shelf", "asymmetric low basalt shelf", new Vector2(-13, -5), 8, 0.34f, 0.58f),
        new("resource_right_basalt_shelf", "asymmetric low basalt shelf", new Vector2(4, -7), 11, 0.42f, 0.70f),
        new("workbench_midground_ridge", "ridge framing workbench", new Vector2(13, -14), 8, 1.05f, 1.50f),
        new("combat_midground_ridge", "ridge framing combat zone", new Vector2(23, -18), 10, 1.25f, 1.80f),
        new("crater_northwest", "shallow impact crater outside route", new Vector2(-20, -24), 6, 0.03f, 0.22f),
        new("crater_southeast", "shallow impact crater outside route", new Vector2(34, -32), 7, 0.03f, 0.24f),
        new("beacon_back_shelf", "raised basalt shelf behind beacon", new Vector2(31, -22), 9, 0.95f, 1.35f),
        new("irregular_horizon_ridge", "distant jagged boundary ridge", new Vector2((minX + maxX) * 0.5f, (minZ + maxZ) * 0.5f), MathF.Max(maxX - minX, maxZ - minZ) * 0.5f, 1.35f, MaxOutsideHeight)
    ];

    private static float HorizonHeight(float x, float z, float minX, float maxX, float minZ, float maxZ)
    {
        float edge = MathF.Min(MathF.Min(x - minX, maxX - x), MathF.Min(z - minZ, maxZ - z));
        return 1.30f + MathF.Max(0, 8.5f - edge) * 0.34f;
    }

    private static float FacetNoise(float x, float z)
    {
        int cellX = (int)MathF.Floor((x + Seed) / 4.0f);
        int cellZ = (int)MathF.Floor((z - Seed) / 4.0f);
        return Hash01(cellX, cellZ) - 0.5f;
    }

    private static TerrainZone DominantZone(params TerrainZone[] zones)
    {
        if (zones.Contains(TerrainZone.AshRoute)) return TerrainZone.AshRoute;
        if (zones.Contains(TerrainZone.HorizonRidge)) return TerrainZone.HorizonRidge;
        return zones.GroupBy(zone => zone).OrderByDescending(group => group.Count()).First().Key;
    }

    private static (float minX, float maxX, float minZ, float maxZ) Bounds(IReadOnlyDictionary<string, Vector3> targets)
    {
        float minX = float.PositiveInfinity, maxX = float.NegativeInfinity, minZ = float.PositiveInfinity, maxZ = float.NegativeInfinity;
        foreach (Vector3 p in targets.Values) { minX = MathF.Min(minX, p.X) - VisualMargin; maxX = MathF.Max(maxX, p.X) + VisualMargin; minZ = MathF.Min(minZ, p.Z) - VisualMargin; maxZ = MathF.Max(maxZ, p.Z) + VisualMargin; }
        return (MathF.Floor(minX / GridSpacing) * GridSpacing, MathF.Ceiling(maxX / GridSpacing) * GridSpacing, MathF.Floor(minZ / GridSpacing) * GridSpacing, MathF.Ceiling(maxZ / GridSpacing) * GridSpacing);
    }

    public static float DistanceToRoute(float x, float z, IReadOnlyDictionary<string, Vector3> targets)
    {
        Vector2 p = new(x, z);
        string[][] routes =
        {
            new[] { "Player", "ResourceDrop_MetalPickup" }, new[] { "Player", "ResourceDrop_BiomassPickup" }, new[] { "Player", "ResourceDrop_ElectronicsPickup" },
            new[] { "ResourceDrop_MetalPickup", "Placeholder_Workbench" }, new[] { "ResourceDrop_BiomassPickup", "Placeholder_Workbench" }, new[] { "ResourceDrop_ElectronicsPickup", "Placeholder_Workbench" },
            new[] { "Placeholder_Workbench", "Placeholder_GalaxabrainScout" }, new[] { "Placeholder_GalaxabrainScout", "Placeholder_GalaxabrainScout/GalaxabrainComponentPickup" },
            new[] { "Placeholder_GalaxabrainScout/GalaxabrainComponentPickup", "Placeholder_SavePoint" }, new[] { "Placeholder_SavePoint", "Placeholder_Beacon" }
        };
        float best = float.PositiveInfinity;
        foreach (string[] route in routes)
            best = MathF.Min(best, DistancePointSegment(p, Xz(targets[route[0]]), Xz(targets[route[1]])));
        foreach (Vector3 target in targets.Values)
            best = MathF.Min(best, p.DistanceTo(Xz(target)) - 2.0f);
        return MathF.Max(0.0f, best);
    }

    private static Vector2 Xz(Vector3 value) => new(value.X, value.Z);

    private static float DistancePointSegment(Vector2 p, Vector2 a, Vector2 b)
    {
        Vector2 ab = b - a;
        float denominator = ab.LengthSquared();
        if (denominator <= 0.0001f) return p.DistanceTo(a);
        float t = Mathf.Clamp((p - a).Dot(ab) / denominator, 0.0f, 1.0f);
        return p.DistanceTo(a + ab * t);
    }

    private static float Hash01(int x, int z)
    {
        unchecked
        {
            uint h = (uint)(x * 374761393 + z * 668265263 + Seed * 1442695041);
            h = (h ^ (h >> 13)) * 1274126177u;
            return ((h ^ (h >> 16)) & 0xFFFFFF) / 16777215.0f;
        }
    }

    private static string ComputeHash(TerrainReport report)
    {
        var builder = new StringBuilder();
        builder.Append(CultureInfo.InvariantCulture, $"{Seed}|{report.Columns}|{report.Rows}|{report.MinX:F3}|{report.MaxX:F3}|{report.MinZ:F3}|{report.MaxZ:F3}|");
        for (int z = 0; z < report.Rows; z++)
            for (int x = 0; x < report.Columns; x++)
                builder.Append(CultureInfo.InvariantCulture, $"{report.Heights[x, z]:F4}:{(int)report.Zones[x, z]};");
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(builder.ToString()))).ToLowerInvariant();
    }
}

public enum TerrainZone
{
    AshRoute,
    CentralPlateau,
    SpawnBasaltShelf,
    ResourceBasaltShelf,
    WorkbenchRidge,
    CombatRidge,
    ImpactCrater,
    BeaconBasaltShelf,
    HorizonRidge
}

public sealed record TerrainBuild(ArrayMesh Mesh, TerrainReport Report);

public sealed record TerrainFeature(string Id, string Purpose, Vector2 Center, float Radius, float MinHeight, float MaxHeight);

public sealed class TerrainReport
{
    public TerrainReport(IReadOnlyDictionary<string, Vector3> targets, IReadOnlyList<TerrainFeature> features, float minX, float maxX, float minZ, float maxZ, int columns, int rows, int vertexCount, int triangleCount, float minHeight, float maxHeight, float maxRouteDeviation, float maxSlope, float routeSurfaceArea, float basaltShelfArea, float[,] heights, TerrainZone[,] zones, IReadOnlyDictionary<TerrainZone, int> zoneTriangles)
    { Targets = targets; Features = features; MinX = minX; MaxX = maxX; MinZ = minZ; MaxZ = maxZ; Columns = columns; Rows = rows; VertexCount = vertexCount; TriangleCount = triangleCount; MinHeight = minHeight; MaxHeight = maxHeight; MaxRouteHeightDeviation = maxRouteDeviation; EstimatedMaxSlope = maxSlope; RouteSurfaceArea = routeSurfaceArea; BasaltShelfSurfaceArea = basaltShelfArea; Heights = heights; Zones = zones; ZoneTriangles = zoneTriangles; }
    public IReadOnlyDictionary<string, Vector3> Targets { get; }
    public IReadOnlyList<TerrainFeature> Features { get; }
    public float MinX { get; }
    public float MaxX { get; }
    public float MinZ { get; }
    public float MaxZ { get; }
    public int Columns { get; }
    public int Rows { get; }
    public int VertexCount { get; }
    public int TriangleCount { get; }
    public float MinHeight { get; }
    public float MaxHeight { get; }
    public float MaxRouteHeightDeviation { get; }
    public float EstimatedMaxSlope { get; }
    public float RouteSurfaceArea { get; }
    public float BasaltShelfSurfaceArea { get; }
    public float TerrainLuminanceMinimum => 0.13f;
    public float TerrainLuminanceMaximum => 0.40f;
    public float[,] Heights { get; }
    public TerrainZone[,] Zones { get; }
    public IReadOnlyDictionary<TerrainZone, int> ZoneTriangles { get; }
    public string MeshDataHash { get; set; } = string.Empty;
    public Vector3 PositionAt(int x, int z) => new(MinX + x * ProceduralCrashSiteTerrain.GridSpacing, Heights[x, z], MinZ + z * ProceduralCrashSiteTerrain.GridSpacing);
    public bool Contains(Vector3 p) => p.X >= MinX && p.X <= MaxX && p.Z >= MinZ && p.Z <= MaxZ;
    public string ToJson()
    {
        string zonesJson = string.Join(",\n    ", ZoneTriangles.Select(pair => $"{{\"name\":\"{pair.Key}\",\"triangles\":{pair.Value}}}"));
        string featuresJson = string.Join(",\n    ", Features.Select(feature => string.Create(CultureInfo.InvariantCulture, $"{{\"id\":\"{feature.Id}\",\"purpose\":\"{feature.Purpose}\",\"center\":[{feature.Center.X:F2},{feature.Center.Y:F2}],\"radius\":{feature.Radius:F2},\"heightRange\":[{feature.MinHeight:F2},{feature.MaxHeight:F2}]}}")));
        string semanticJson = string.Join(",\n    ", ProceduralCrashSiteTerrain.BuildSemanticMeshes(this).Select(item => { var stats = ProceduralCrashSiteTerrain.MeshStats(item.Mesh); return $"{{\"name\":\"{item.Name}\",\"vertices\":{stats.Vertices},\"triangles\":{stats.Triangles}}}"; }));
        string horizonJson = string.Join(",\n    ", ProceduralCrashSiteTerrain.BuildHorizonSegmentMeshes(this).Select(item => { var stats = ProceduralCrashSiteTerrain.MeshStats(item.Mesh); return $"{{\"name\":\"{item.Name}\",\"vertices\":{stats.Vertices},\"triangles\":{stats.Triangles}}}"; }));
        return $$"""
{
  "seed": {{ProceduralCrashSiteTerrain.Seed}},
  "generationModel": "Pass 1E semantic low-poly landform composition",
  "vertexCount": {{VertexCount}},
  "triangleCount": {{TriangleCount}},
  "aabbMin": "({{MinX}}, {{MinHeight}}, {{MinZ}})",
  "aabbMax": "({{MaxX}}, {{MaxHeight}}, {{MaxZ}})",
  "minHeight": {{MinHeight.ToString(CultureInfo.InvariantCulture)}},
  "maxHeight": {{MaxHeight.ToString(CultureInfo.InvariantCulture)}},
  "longestDimension": {{MathF.Max(MaxX - MinX, MaxZ - MinZ).ToString(CultureInfo.InvariantCulture)}},
  "gridSpacing": {{ProceduralCrashSiteTerrain.GridSpacing.ToString(CultureInfo.InvariantCulture)}},
  "corridorWidth": {{ProceduralCrashSiteTerrain.CorridorWidth.ToString(CultureInfo.InvariantCulture)}},
  "maximumRouteHeightDeviation": {{MaxRouteHeightDeviation.ToString(CultureInfo.InvariantCulture)}},
  "maximumOutsideRouteHeight": {{MaxHeight.ToString(CultureInfo.InvariantCulture)}},
  "estimatedMaximumSlope": {{EstimatedMaxSlope.ToString(CultureInfo.InvariantCulture)}},
  "connectedTerrainSurfaceCount": 1,
  "routeSurfaceArea": {{RouteSurfaceArea.ToString(CultureInfo.InvariantCulture)}},
  "basaltShelfSurfaceArea": {{BasaltShelfSurfaceArea.ToString(CultureInfo.InvariantCulture)}},
  "horizonRidgeHeightRange": [1.35, {{ProceduralCrashSiteTerrain.MaxOutsideHeight.ToString(CultureInfo.InvariantCulture)}}],
  "terrainLuminanceInputRange": [{{TerrainLuminanceMinimum.ToString(CultureInfo.InvariantCulture)}}, {{TerrainLuminanceMaximum.ToString(CultureInfo.InvariantCulture)}}],
  "normalMode": "flat faceted per triangle",
  "materialAssignments": {"AshRoute":"VolcanicSoil vertex colour","CentralPlateau":"VolcanicRock vertex colour","RidgesAndShelves":"VolcanicRock/DamageScorch dark vertex colour"},
  "zones": [
    {{zonesJson}}
  ],
  "features": [
    {{featuresJson}}
  ],
  "semanticMeshes": [
    {{semanticJson}}
  ],
  "horizonSegments": [
    {{horizonJson}}
  ],
  "meshDataHash": "{{MeshDataHash}}"
}
""";
    }
}
