using System;
using System.Collections.Generic;
using Godot;

namespace TitanCraft.World;

/// <summary>
/// Gives the quarter's flat-colour materials surface (workstream D, "materials
/// move from flat colour to textured"). The district's models carry one plain
/// colour per material; this walks the scene once, finds materials by their
/// authored names (…Render, …Stone, …Weathered, …Timber), and swaps in a copy
/// that keeps the colour but adds a world-space (triplanar) noise texture and
/// normal map — lime-plaster grain, stone blocks, weathering, wood grain. The
/// textures are generated from noise at load: no downloaded texture, no new
/// licence, and no UV dependency on the generated models.
/// </summary>
public partial class QuarterSurfaceDetail : Node
{
    /// <summary>Subtrees to detail, relative to this node's parent.</summary>
    [Export] public Godot.Collections.Array<NodePath> Roots { get; set; } = new();

    public int MaterialsDetailed { get; private set; }

    private readonly Dictionary<Material, Material> _cache = new();

    public enum Family
    {
        None,
        Plaster,
        Stone,
        Weathered,
        Timber,
        Cloth,
        Sand,
    }

    /// <summary>Which surface family an authored material name belongs to.</summary>
    public static Family Classify(string materialName)
    {
        if (string.IsNullOrEmpty(materialName))
            return Family.None;

        var name = materialName.ToLowerInvariant();
        if (name.Contains("glow") || name.Contains("led") || name.Contains("glass") || name.Contains("crystal")
            || name.Contains("opening") || name.Contains("water") || name.Contains("core"))
            return Family.None;
        if (name.Contains("render") || name.Contains("plaster") || name.Contains("lime"))
            return Family.Plaster;
        if (name.Contains("weathered") || name.Contains("shadow") || name.Contains("rubblecore") || name.Contains("tidestain") || name.Contains("lossCore".ToLowerInvariant()))
            return Family.Weathered;
        if (name.Contains("stone") || name.Contains("coral") || name.Contains("rag") || name.Contains("coping")
            || name.Contains("lava") || name.Contains("basalt") || name.Contains("volcanic") || name.Contains("rock"))
            return Family.Stone;
        if (name.Contains("timber") || name.Contains("wood") || name.Contains("door") || name.Contains("shutter")
            || name.Contains("pole") || name.Contains("trunk") || name.Contains("basket"))
            return Family.Timber;
        if (name.Contains("cloth") || name.Contains("rope") || name.Contains("frond"))
            return Family.Cloth;
        if (name.Contains("sand"))
            return Family.Sand;
        return Family.None;
    }

    public override void _Ready()
    {
        // Deferred: instanced glTF children are fully in the tree by then.
        Callable.From(Apply).CallDeferred();
    }

    public void Apply()
    {
        var parent = GetParent();
        if (parent is null)
            return;

        var roots = new List<Node>();
        foreach (var path in Roots)
        {
            if (parent.GetNodeOrNull(path) is { } root)
                roots.Add(root);
        }

        foreach (var root in roots)
        {
            foreach (var node in root.FindChildren("*", "MeshInstance3D", true, false))
                DetailMesh((MeshInstance3D)node);
        }
    }

    private void DetailMesh(MeshInstance3D meshInstance)
    {
        var mesh = meshInstance.Mesh;
        if (mesh is null)
            return;

        for (var surface = 0; surface < mesh.GetSurfaceCount(); surface++)
        {
            var original = meshInstance.GetSurfaceOverrideMaterial(surface) ?? mesh.SurfaceGetMaterial(surface);
            if (original is not StandardMaterial3D standard)
                continue;

            if (!_cache.TryGetValue(standard, out var detailed))
            {
                var family = Classify(standard.ResourceName);
                detailed = family == Family.None ? standard : Detailed(standard, family);
                if (!ReferenceEquals(detailed, standard))
                    MaterialsDetailed++;
                _cache[standard] = detailed;
            }

            if (!ReferenceEquals(detailed, original))
                meshInstance.SetSurfaceOverrideMaterial(surface, detailed);
        }
    }

    private static StandardMaterial3D Detailed(StandardMaterial3D source, Family family)
    {
        var material = (StandardMaterial3D)source.Duplicate();
        var (albedo, normal, scale, bump) = Textures(family);
        material.AlbedoTexture = albedo;
        material.NormalEnabled = true;
        material.NormalTexture = normal;
        material.NormalScale = bump;
        material.Uv1Triplanar = true;
        material.Uv1WorldTriplanar = true;
        material.Uv1TriplanarSharpness = 4f;
        material.Uv1Scale = Vector3.One * scale;
        material.TextureFilter = BaseMaterial3D.TextureFilterEnum.LinearWithMipmapsAnisotropic;
        return material;
    }

    private static readonly Dictionary<Family, (Texture2D Albedo, Texture2D Normal, float Scale, float Bump)> Shared = new();

    /// <summary>One shared albedo/normal pair per family: generated once per run, reused by every material in it.</summary>
    private static (Texture2D Albedo, Texture2D Normal, float Scale, float Bump) Textures(Family family)
    {
        if (Shared.TryGetValue(family, out var cached))
            return cached;

        var (noiseType, frequency, octaves, low, high, scale, bump, seed) = family switch
        {
            Family.Plaster => (FastNoiseLite.NoiseTypeEnum.SimplexSmooth, 0.03f, 5, 0.80f, 1.00f, 0.35f, 0.55f, 11),
            Family.Stone => (FastNoiseLite.NoiseTypeEnum.Cellular, 0.018f, 3, 0.70f, 1.00f, 0.45f, 0.9f, 23),
            Family.Weathered => (FastNoiseLite.NoiseTypeEnum.Perlin, 0.02f, 5, 0.62f, 1.00f, 0.3f, 0.7f, 37),
            Family.Timber => (FastNoiseLite.NoiseTypeEnum.Perlin, 0.06f, 3, 0.72f, 1.00f, 0.8f, 0.5f, 41),
            Family.Cloth => (FastNoiseLite.NoiseTypeEnum.SimplexSmooth, 0.12f, 2, 0.86f, 1.00f, 1.2f, 0.25f, 53),
            _ => (FastNoiseLite.NoiseTypeEnum.SimplexSmooth, 0.05f, 4, 0.82f, 1.00f, 0.6f, 0.4f, 67),
        };

        FastNoiseLite Noise() => new()
        {
            NoiseType = noiseType,
            Frequency = frequency,
            FractalOctaves = octaves,
            FractalType = FastNoiseLite.FractalTypeEnum.Fbm,
            Seed = seed,
        };

        var ramp = new Gradient();
        ramp.SetColor(0, new Color(low, low, low));
        ramp.SetColor(1, new Color(high, high, high));
        var albedo = new NoiseTexture2D
        {
            Width = 512,
            Height = 512,
            Seamless = true,
            Noise = Noise(),
            ColorRamp = ramp,
            GenerateMipmaps = true,
        };
        var normal = new NoiseTexture2D
        {
            Width = 512,
            Height = 512,
            Seamless = true,
            AsNormalMap = true,
            BumpStrength = family == Family.Stone ? 12f : 6f,
            Noise = Noise(),
            GenerateMipmaps = true,
        };

        var result = (albedo as Texture2D, normal as Texture2D, scale, bump);
        Shared[family] = result;
        return result;
    }
}
