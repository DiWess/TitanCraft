using System;
using System.IO;
using System.Text.Json;
using TitanCraft.Resources;

namespace TitanCraft.Crafting;

/// <summary>
/// One Mechanical Arm module recipe (Mk I, Mk II or Mk III), loaded from
/// data/Recipes/mechanical_arm_mk{tier}.json so costs live in one place.
/// Each tier upgrades the arm from the tier below it: the workbench never
/// skips a module.
/// </summary>
public sealed class MechanicalArmRecipe
{
    public const int FirstTier = 1;
    public const int LastTier = MvpInventory.MaxArmLevel;

    private readonly RecipeData data;

    public MechanicalArmRecipe()
        : this(FirstTier)
    {
    }

    public MechanicalArmRecipe(int tier)
        : this(LoadTier(tier))
    {
    }

    private MechanicalArmRecipe(RecipeData data)
    {
        this.data = data;
    }

    public int Tier => data.Tier;

    /// <summary>Player-facing module name: "Mk II Magnet Clamp".</summary>
    public string DisplayName => $"Mk {RomanTier(Tier)} {data.Name}";

    public string ShortName => $"Mk {RomanTier(Tier)}";

    public int MetalCost => data.Metal;

    public int BiomassCost => data.Biomass;

    public int ElectronicComponentsCost => data.ElectronicComponents;

    public bool RequiresGalaxabrainComponent => data.RequiresGalaxabrainComponent;

    /// <summary>The recipe that would upgrade the arm next, or null once the arm is complete.</summary>
    public static MechanicalArmRecipe? NextFor(MvpInventory inventory)
    {
        ArgumentNullException.ThrowIfNull(inventory);
        return inventory.ArmLevel >= LastTier ? null : new MechanicalArmRecipe(inventory.ArmLevel + 1);
    }

    public string GetProgressText(MvpInventory inventory)
    {
        ArgumentNullException.ThrowIfNull(inventory);

        if (inventory.ArmLevel >= Tier)
        {
            return OnlineText(inventory.ArmLevel);
        }

        // Short enough to sit on one line of the HUD panel at 1280x720.
        var text = $"{ShortName}: Metal {ClampToCost(inventory.Metal, MetalCost)}/{MetalCost}";
        if (BiomassCost > 0)
            text += $" · Biomass {ClampToCost(inventory.Biomass, BiomassCost)}/{BiomassCost}";
        if (ElectronicComponentsCost > 0)
            text += $" · Electronics {ClampToCost(inventory.ElectronicComponents, ElectronicComponentsCost)}/{ElectronicComponentsCost}";
        if (RequiresGalaxabrainComponent)
            text += inventory.HasGalaxabrainComponent ? " · Component ✓" : " · Component";
        return text;
    }

    /// <summary>What the arm does at <paramref name="armLevel"/>, in the controls the player uses.</summary>
    public static string OnlineText(int armLevel) => armLevel switch
    {
        >= 3 => "Mk III ONLINE · Hold left click to charge",
        2 => "Mk II ONLINE · Right click to pull",
        1 => "Mk I ONLINE · Left click to attack",
        _ => string.Empty,
    };

    public bool HasResources(MvpInventory inventory)
    {
        ArgumentNullException.ThrowIfNull(inventory);

        return inventory.CanSpendResources(
                metal: MetalCost,
                biomass: BiomassCost,
                electronicComponents: ElectronicComponentsCost)
            && (!RequiresGalaxabrainComponent || inventory.HasGalaxabrainComponent);
    }

    public bool CanCraft(MvpInventory inventory)
    {
        ArgumentNullException.ThrowIfNull(inventory);

        return inventory.ArmLevel == Tier - 1 && HasResources(inventory);
    }

    public bool TryCraft(MvpInventory inventory)
    {
        ArgumentNullException.ThrowIfNull(inventory);

        if (!CanCraft(inventory))
        {
            return false;
        }

        var didSpend = inventory.TrySpendResources(
            metal: MetalCost,
            biomass: BiomassCost,
            electronicComponents: ElectronicComponentsCost);

        if (!didSpend)
        {
            return false;
        }

        if (RequiresGalaxabrainComponent)
        {
            inventory.TryConsumeGalaxabrainComponent();
        }

        inventory.UpgradeArm(Tier);
        return true;
    }

    public static string RomanTier(int tier) => tier switch
    {
        1 => "I",
        2 => "II",
        3 => "III",
        _ => tier.ToString(),
    };

    private static RecipeData LoadTier(int tier)
    {
        if (tier < FirstTier || tier > LastTier)
        {
            throw new ArgumentOutOfRangeException(nameof(tier), $"Mechanical arm tiers run {FirstTier}-{LastTier}.");
        }

        var relativePath = $"data/Recipes/mechanical_arm_mk{tier}.json";
        var recipeJson = ReadRecipeText(relativePath);
        var data = JsonSerializer.Deserialize<RecipeData>(recipeJson, JsonOptions)
            ?? throw new InvalidDataException($"Mechanical arm recipe data is missing in {relativePath}.");

        data.Validate(relativePath, tier);
        return data;
    }

    /// <summary>
    /// Reads recipe data from the working tree when there is one (unit tests,
    /// the editor), otherwise from the exported game's packed resources.
    /// System.IO cannot see inside a .pck, and an exported build has no
    /// data/ folder beside it: before this fallback the Windows build only
    /// found its recipe when run from inside the repository.
    /// </summary>
    private static string ReadRecipeText(string relativePath)
    {
        foreach (var root in new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory })
        {
            var directory = new DirectoryInfo(root);

            while (directory is not null)
            {
                var candidatePath = Path.Combine(directory.FullName, relativePath);

                if (File.Exists(candidatePath))
                {
                    return File.ReadAllText(candidatePath);
                }

                directory = directory.Parent;
            }
        }

        var packedPath = $"res://{relativePath}";
        if (Godot.FileAccess.FileExists(packedPath))
        {
            return Godot.FileAccess.GetFileAsString(packedPath);
        }

        throw new FileNotFoundException($"Mechanical arm recipe data file was not found: {relativePath}");
    }

    private static int ClampToCost(int current, int cost) => Math.Min(current, cost);

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    private sealed record RecipeData(
        int Tier,
        string Name,
        int Metal,
        int Biomass,
        int ElectronicComponents,
        bool RequiresGalaxabrainComponent)
    {
        public void Validate(string recipePath, int expectedTier)
        {
            if (Metal < 0 || Biomass < 0 || ElectronicComponents < 0)
            {
                throw new InvalidDataException($"Mechanical arm recipe costs must be non-negative in {recipePath}.");
            }

            if (Tier != expectedTier)
            {
                throw new InvalidDataException($"{recipePath} declares tier {Tier}, expected {expectedTier}.");
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new InvalidDataException($"{recipePath} has no module name.");
            }
        }
    }
}
