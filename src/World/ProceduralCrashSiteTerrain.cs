using System;
using System.Collections.Generic;
using System.Globalization;
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
    private const string MeshNodeName = "TerrainMesh";

    private static readonly string[] TargetPaths =
    {
        "Player",
        "Placeholder_MetalPickup",
        "Placeholder_BiomassPickup",
        "Placeholder_ElectronicsPickup",
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
        GetNode<MeshInstance3D>(MeshNodeName).Mesh = build.Mesh;
    }

    public static TerrainBuild BuildForWorld(Node3D worldRoot)
    {
        Dictionary<string, Vector3> targets = ReadTargets(worldRoot);
        TerrainReport report = GenerateReport(targets);
        ArrayMesh mesh = BuildMesh(report);
        return new TerrainBuild(mesh, report);
    }

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
        float minX = float.PositiveInfinity;
        float maxX = float.NegativeInfinity;
        float minZ = float.PositiveInfinity;
        float maxZ = float.NegativeInfinity;
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
        float minHeight = float.PositiveInfinity;
        float maxHeight = float.NegativeInfinity;
        float maxRouteDeviation = 0.0f;
        float maxSlope = 0.0f;

        for (int z = 0; z < rows; z++)
        {
            for (int x = 0; x < columns; x++)
            {
                float worldX = minX + x * GridSpacing;
                float worldZ = minZ + z * GridSpacing;
                float distance = DistanceToRoute(worldX, worldZ, targets);
                float height = HeightAt(worldX, worldZ, distance, minX, maxX, minZ, maxZ);
                heights[x, z] = height;
                minHeight = MathF.Min(minHeight, height);
                maxHeight = MathF.Max(maxHeight, height);
                if (distance <= CorridorWidth * 0.5f)
                    maxRouteDeviation = MathF.Max(maxRouteDeviation, MathF.Abs(height - CorridorHeight));
            }
        }

        for (int z = 0; z < rows - 1; z++)
            for (int x = 0; x < columns - 1; x++)
            {
                maxSlope = MathF.Max(maxSlope, MathF.Abs(heights[x + 1, z] - heights[x, z]) / GridSpacing);
                maxSlope = MathF.Max(maxSlope, MathF.Abs(heights[x, z + 1] - heights[x, z]) / GridSpacing);
            }

        int quadCount = (columns - 1) * (rows - 1);
        int triangleCount = quadCount * 2;
        int vertexCount = triangleCount * 3;
        var report = new TerrainReport(targets, minX, maxX, minZ, maxZ, columns, rows, vertexCount, triangleCount, minHeight, maxHeight, maxRouteDeviation, maxSlope, heights);
        report.MeshDataHash = ComputeHash(report);
        return report;
    }

    private static ArrayMesh BuildMesh(TerrainReport report)
    {
        var vertices = new List<Vector3>(report.VertexCount);
        var normals = new List<Vector3>(report.VertexCount);
        var colors = new List<Color>(report.VertexCount);
        for (int z = 0; z < report.Rows - 1; z++)
            for (int x = 0; x < report.Columns - 1; x++)
            {
                Vector3 a = report.PositionAt(x, z);
                Vector3 b = report.PositionAt(x + 1, z);
                Vector3 c = report.PositionAt(x + 1, z + 1);
                Vector3 d = report.PositionAt(x, z + 1);
                AddTriangle(vertices, normals, colors, a, b, c);
                AddTriangle(vertices, normals, colors, a, c, d);
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

    private static void AddTriangle(List<Vector3> vertices, List<Vector3> normals, List<Color> colors, Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 normal = (b - a).Cross(c - a).Normalized();
        vertices.Add(a); vertices.Add(b); vertices.Add(c);
        normals.Add(normal); normals.Add(normal); normals.Add(normal);
        colors.Add(ColorFor(a.Y)); colors.Add(ColorFor(b.Y)); colors.Add(ColorFor(c.Y));
    }

    private static Color ColorFor(float height) => height < 0.12f ? new Color(0.10f, 0.085f, 0.075f) : height < 1.2f ? new Color(0.17f, 0.15f, 0.13f) : new Color(0.24f, 0.22f, 0.20f);

    public static float HeightAt(float x, float z, IReadOnlyDictionary<string, Vector3> targets)
        => HeightAt(x, z, DistanceToRoute(x, z, targets), Bounds(targets).minX, Bounds(targets).maxX, Bounds(targets).minZ, Bounds(targets).maxZ);

    private static float HeightAt(float x, float z, float distanceToRoute, float minX, float maxX, float minZ, float maxZ)
    {
        if (distanceToRoute <= CorridorWidth * 0.5f)
            return CorridorHeight;
        float blend = Mathf.Clamp((distanceToRoute - CorridorWidth * 0.5f) / 12.0f, 0.0f, 1.0f);
        float noise = Hash01((int)MathF.Floor(x * 3.1f) + Seed, (int)MathF.Floor(z * 2.7f) - Seed);
        float fracture = (noise - 0.35f) * 1.1f;
        float edge = MathF.Min(MathF.Min(x - minX, maxX - x), MathF.Min(z - minZ, maxZ - z));
        float ridge = edge < 9.0f ? (9.0f - edge) * 0.34f : 0.0f;
        float crater = CraterOffset(x, z, minX, maxX, minZ, maxZ);
        return Mathf.Clamp(CorridorHeight + blend * (0.35f + fracture + ridge + crater), CorridorHeight, MaxOutsideHeight);
    }

    private static float CraterOffset(float x, float z, float minX, float maxX, float minZ, float maxZ)
    {
        Span<Vector2> centers = stackalloc Vector2[] { new(minX + 10.0f, minZ + 12.0f), new(maxX - 12.0f, minZ + 18.0f), new(maxX - 16.0f, maxZ - 10.0f) };
        float offset = 0.0f;
        foreach (Vector2 center in centers)
        {
            float distance = new Vector2(x, z).DistanceTo(center);
            if (distance < 7.0f)
                offset -= (7.0f - distance) * 0.12f;
        }
        return offset;
    }

    private static (float minX, float maxX, float minZ, float maxZ) Bounds(IReadOnlyDictionary<string, Vector3> targets)
    {
        float minX = float.PositiveInfinity, maxX = float.NegativeInfinity, minZ = float.PositiveInfinity, maxZ = float.NegativeInfinity;
        foreach (Vector3 p in targets.Values) { minX = MathF.Min(minX, p.X) - VisualMargin; maxX = MathF.Max(maxX, p.X) + VisualMargin; minZ = MathF.Min(minZ, p.Z) - VisualMargin; maxZ = MathF.Max(maxZ, p.Z) + VisualMargin; }
        return (minX, maxX, minZ, maxZ);
    }

    public static float DistanceToRoute(float x, float z, IReadOnlyDictionary<string, Vector3> targets)
    {
        Vector2 p = new(x, z);
        string[][] routes =
        {
            new[] { "Player", "Placeholder_MetalPickup" }, new[] { "Player", "Placeholder_BiomassPickup" }, new[] { "Player", "Placeholder_ElectronicsPickup" },
            new[] { "Placeholder_MetalPickup", "Placeholder_Workbench" }, new[] { "Placeholder_BiomassPickup", "Placeholder_Workbench" }, new[] { "Placeholder_ElectronicsPickup", "Placeholder_Workbench" },
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
                builder.Append(CultureInfo.InvariantCulture, $"{report.Heights[x, z]:F4};");
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(builder.ToString()))).ToLowerInvariant();
    }
}

public sealed record TerrainBuild(ArrayMesh Mesh, TerrainReport Report);

public sealed class TerrainReport
{
    public TerrainReport(IReadOnlyDictionary<string, Vector3> targets, float minX, float maxX, float minZ, float maxZ, int columns, int rows, int vertexCount, int triangleCount, float minHeight, float maxHeight, float maxRouteDeviation, float maxSlope, float[,] heights)
    { Targets = targets; MinX = minX; MaxX = maxX; MinZ = minZ; MaxZ = maxZ; Columns = columns; Rows = rows; VertexCount = vertexCount; TriangleCount = triangleCount; MinHeight = minHeight; MaxHeight = maxHeight; MaxRouteHeightDeviation = maxRouteDeviation; EstimatedMaxSlope = maxSlope; Heights = heights; }
    public IReadOnlyDictionary<string, Vector3> Targets { get; }
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
    public float[,] Heights { get; }
    public string MeshDataHash { get; set; } = string.Empty;
    public Vector3 PositionAt(int x, int z) => new(MinX + x * ProceduralCrashSiteTerrain.GridSpacing, Heights[x, z], MinZ + z * ProceduralCrashSiteTerrain.GridSpacing);
    public bool Contains(Vector3 p) => p.X >= MinX && p.X <= MaxX && p.Z >= MinZ && p.Z <= MaxZ;
    public string ToJson() => $$"""
{
  "seed": {{ProceduralCrashSiteTerrain.Seed}},
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
  "meshDataHash": "{{MeshDataHash}}"
}
""";
}
