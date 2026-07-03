using GdUnit4;
using TitanCraft.Tools;
using TitanCraft.World;
using static GdUnit4.Assertions;

namespace TitanCraft.Tests.Unit;

[TestSuite]
public sealed class ResourceDropSceneConverterTests
{
    [TestCase]
    public void PlaceholderNamesMapToResourceTypes()
    {
        AssertThat(ResourceDropSceneConverter.TryMapPlaceholderName("Placeholder_Metal_01", out var metal)).IsTrue();
        AssertThat(metal).IsEqual(ResourceType.Metal);

        AssertThat(ResourceDropSceneConverter.TryMapPlaceholderName("Placeholder_BiomassPickup", out var biomass)).IsTrue();
        AssertThat(biomass).IsEqual(ResourceType.Biomass);

        AssertThat(ResourceDropSceneConverter.TryMapPlaceholderName("Placeholder_ElectronicsPickup", out var electronics)).IsTrue();
        AssertThat(electronics).IsEqual(ResourceType.Electronics);
    }

    [TestCase]
    public void NonResourcePlaceholderNamesAreIgnored()
    {
        AssertThat(ResourceDropSceneConverter.TryMapPlaceholderName("Placeholder_Workbench", out _)).IsFalse();
        AssertThat(ResourceDropSceneConverter.TryMapPlaceholderName("CrashSiteCargoCrate1", out _)).IsFalse();
    }
}
