using System;
using System.Threading.Tasks;
using Godot;
using TitanCraft.Crafting;

namespace TitanCraft.Core;

/// <summary>
/// A smoke test an exported build can run on itself:
/// <c>TitanCraft -- --titancraft-selftest</c>. It loads every arm recipe the
/// way the workbench does (from the packed project, away from any source
/// tree), opens the game scene for a few seconds, and prints
/// TITANCRAFT_SELFTEST_PASS or TITANCRAFT_SELFTEST_FAIL before quitting.
/// Written for the release check: the shipped game once found its recipes only
/// when run from inside the repository.
/// </summary>
public static class ReleaseSelfTest
{
    public const string Flag = "--titancraft-selftest";

    public static bool Requested() => Array.IndexOf(OS.GetCmdlineUserArgs(), Flag) >= 0;

    public static async void Run(Node host, string gameScenePath)
    {
        try
        {
            for (var tier = MechanicalArmRecipe.FirstTier; tier <= MechanicalArmRecipe.LastTier; tier++)
            {
                var recipe = new MechanicalArmRecipe(tier);
                GD.Print($"SELFTEST recipe {recipe.DisplayName}: {recipe.MetalCost}/{recipe.BiomassCost}/{recipe.ElectronicComponentsCost}");
            }

            var tree = host.GetTree();
            // Never change scene from inside the menu's own _Ready.
            await tree.ToSignal(tree, SceneTree.SignalName.ProcessFrame);
            var error = tree.ChangeSceneToFile(gameScenePath);
            if (error != Error.Ok)
                throw new InvalidOperationException($"could not open {gameScenePath}: {error}");

            for (var frame = 0; frame < 180; frame++)
                await tree.ToSignal(tree, SceneTree.SignalName.ProcessFrame);

            if (tree.CurrentScene?.GetNodeOrNull("Player") is null)
                throw new InvalidOperationException("the game scene has no player");

            GD.Print("TITANCRAFT_SELFTEST_PASS");
            tree.Quit(0);
        }
        catch (Exception exception)
        {
            GD.PrintErr($"TITANCRAFT_SELFTEST_FAIL {exception.Message}");
            host.GetTree().Quit(1);
        }
    }
}
