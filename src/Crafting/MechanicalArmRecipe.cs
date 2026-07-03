using System;
using System.IO;
using System.Text.Json;
using TitanCraft.Resources;

namespace TitanCraft.Crafting;

public sealed class MechanicalArmRecipe
{
    private const string RecipeRelativePath = "data/Recipes/mechanical_arm_mk1.json";

    private readonly RecipeCost cost;

    public MechanicalArmRecipe()
        : this(LoadDefaultCost())
    {
    }

    private MechanicalArmRecipe(RecipeCost cost)
    {
        this.cost = cost;
    }

    public int MetalCost => cost.Metal;

    public int BiomassCost => cost.Biomass;

    public int ElectronicComponentsCost => cost.ElectronicComponents;

    public string GetProgressText(MvpInventory inventory)
    {
        ArgumentNullException.ThrowIfNull(inventory);

        if (inventory.IsMechanicalArmBuilt)
        {
            return "Mechanical Arm Mk I: ONLINE";
        }

        return $"Mechanical Arm Mk I: Metal {ClampToCost(inventory.Metal, MetalCost)}/{MetalCost} | "
            + $"Biomass {ClampToCost(inventory.Biomass, BiomassCost)}/{BiomassCost} | "
            + $"Electronics {ClampToCost(inventory.ElectronicComponents, ElectronicComponentsCost)}/{ElectronicComponentsCost}";
    }

    public bool CanCraft(MvpInventory inventory)
    {
        ArgumentNullException.ThrowIfNull(inventory);

        return !inventory.IsMechanicalArmBuilt
            && inventory.CanSpendResources(
                metal: MetalCost,
                biomass: BiomassCost,
                electronicComponents: ElectronicComponentsCost);
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

        inventory.MarkMechanicalArmBuilt();
        return true;
    }

    private static RecipeCost LoadDefaultCost()
    {
        var recipePath = FindRecipePath();
        var recipeJson = File.ReadAllText(recipePath);
        var cost = JsonSerializer.Deserialize<RecipeCost>(recipeJson, JsonOptions);

        if (cost is null)
        {
            throw new InvalidDataException($"Mechanical arm recipe data is missing in {recipePath}.");
        }

        cost.Validate(recipePath);
        return cost;
    }

    private static string FindRecipePath()
    {
        foreach (var root in new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory })
        {
            var directory = new DirectoryInfo(root);

            while (directory is not null)
            {
                var candidatePath = Path.Combine(directory.FullName, RecipeRelativePath);

                if (File.Exists(candidatePath))
                {
                    return candidatePath;
                }

                directory = directory.Parent;
            }
        }

        throw new FileNotFoundException($"Mechanical arm recipe data file was not found: {RecipeRelativePath}");
    }

    private static int ClampToCost(int current, int cost) => Math.Min(current, cost);

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    private sealed record RecipeCost(int Metal, int Biomass, int ElectronicComponents)
    {
        public void Validate(string recipePath)
        {
            if (Metal < 0 || Biomass < 0 || ElectronicComponents < 0)
            {
                throw new InvalidDataException($"Mechanical arm recipe costs must be non-negative in {recipePath}.");
            }
        }
    }
}
