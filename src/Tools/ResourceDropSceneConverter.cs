using Godot;
using TitanCraft.World;

namespace TitanCraft.Tools;

[Tool]
public partial class ResourceDropSceneConverter : Node
{
    [Export] public PackedScene? ResourceDropScene { get; set; }

    [Export]
    public bool ExecuteConversion
    {
        get => false;
        set
        {
            if (value)
            {
                ConvertPlaceholders();
            }
        }
    }

    public static bool TryMapPlaceholderName(string nodeName, out ResourceType type)
    {
        if (nodeName.Contains("Metal", System.StringComparison.OrdinalIgnoreCase))
        {
            type = ResourceType.Metal;
            return true;
        }

        if (nodeName.Contains("Biomass", System.StringComparison.OrdinalIgnoreCase))
        {
            type = ResourceType.Biomass;
            return true;
        }

        if (nodeName.Contains("Electronics", System.StringComparison.OrdinalIgnoreCase))
        {
            type = ResourceType.Electronics;
            return true;
        }

        type = default;
        return false;
    }

    public void ConvertPlaceholders()
    {
        if (ResourceDropScene is null)
        {
            GD.PushWarning("[ResourceDropConverter] ResourceDropScene is not assigned; conversion skipped.");
            return;
        }

        var sceneRoot = GetTree().EditedSceneRoot ?? GetTree().CurrentScene;
        if (sceneRoot is null)
        {
            GD.PushWarning("[ResourceDropConverter] No scene root found; conversion skipped.");
            return;
        }

        var placeholders = new Godot.Collections.Array<Node>();
        CollectResourcePlaceholders(sceneRoot, placeholders);
        var converted = 0;

        foreach (var placeholder in placeholders)
        {
            if (placeholder.GetParent() is not Node parent
                || !TryMapPlaceholderName(placeholder.Name, out var type)
                || ResourceDropScene.Instantiate() is not ResourceDrop drop)
            {
                continue;
            }

            drop.Name = placeholder.Name.ToString().Replace("Placeholder_", "ResourceDrop_");
            drop.Type = type;
            drop.Quantity = ReadQuantity(placeholder, type);
            if (placeholder is Node3D placeholder3D)
            {
                drop.Transform = placeholder3D.Transform;
            }

            foreach (var group in placeholder.GetGroups())
            {
                drop.AddToGroup(group);
            }

            parent.AddChild(drop);
            parent.MoveChild(drop, placeholder.GetIndex());
            drop.Owner = sceneRoot;
            placeholder.QueueFree();
            converted++;
            GD.Print($"[ResourceDropConverter] {placeholder.Name} -> {drop.Name} type={drop.Type} quantity={drop.Quantity}");
        }

        GD.Print($"[ResourceDropConverter] Conversion complete. scanned={placeholders.Count} converted={converted}");
    }

    private static void CollectResourcePlaceholders(Node node, Godot.Collections.Array<Node> matches)
    {
        if (node.Name.ToString().StartsWith("Placeholder_", System.StringComparison.Ordinal)
            && TryMapPlaceholderName(node.Name, out _))
        {
            matches.Add(node);
        }

        foreach (var child in node.GetChildren())
        {
            CollectResourcePlaceholders(child, matches);
        }
    }

    private static int ReadQuantity(Node placeholder, ResourceType type)
    {
        var quantity = placeholder.Get("Quantity").AsInt32();
        if (quantity > 0)
        {
            return quantity;
        }

        return type switch
        {
            ResourceType.Metal => 10,
            ResourceType.Biomass => 3,
            ResourceType.Electronics => 2,
            _ => 1,
        };
    }
}
