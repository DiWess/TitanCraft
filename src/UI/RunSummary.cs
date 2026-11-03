namespace TitanCraft.UI;

/// <summary>
/// Carries a won run's numbers across the change to the victory screen, the
/// way <see cref="EndingSnapshot"/> carries its last frame: discoveries found
/// and time played. The screen consumes it once.
/// </summary>
public static class RunSummary
{
    private static (int Read, int Total, float Seconds)? _summary;

    public static void Set(int discoveriesRead, int discoveriesTotal, float playSeconds) =>
        _summary = (discoveriesRead, discoveriesTotal, playSeconds);

    public static (int Read, int Total, float Seconds)? Take()
    {
        var summary = _summary;
        _summary = null;
        return summary;
    }

    public static string Describe(int read, int total, float seconds)
    {
        var minutes = (int)(seconds / 60f);
        var rest = (int)(seconds % 60f);
        return $"Discoveries found: {read} of {total}    Time: {minutes}:{rest:00}";
    }
}
