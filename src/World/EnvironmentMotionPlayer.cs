using Godot;

namespace TitanCraft.World;

/// <summary>
/// Starts the looping animation carried by an imported animated glTF asset.
///
/// Two things the importer does not do for us:
///   1. glTF has no concept of a looping animation, so Godot imports every
///      clip with <see cref="Animation.LoopModeEnum.None"/>. Left alone, each
///      palm sways once and then stands frozen for the rest of the session.
///   2. Every instance of the same asset starts at frame 0, so a street of
///      them sways in perfect lockstep, which reads as a machine rather than
///      as wind.
///
/// The phase offset is derived from the instance's own position rather than
/// from a random number, so captures and tests are reproducible.
/// </summary>
public partial class EnvironmentMotionPlayer : Node3D
{
    [Export(PropertyHint.Range, "0.1,3,0.05")] public float SpeedScale { get; set; } = 1.0f;

    /// <summary>
    /// Per-instance speed variation, applied as +/- this fraction. Identical
    /// playback rates on neighbouring props read as mechanical even when their
    /// phases differ.
    /// </summary>
    [Export(PropertyHint.Range, "0,0.5,0.01")] public float SpeedVariation { get; set; } = 0.12f;

    private AnimationPlayer? _animationPlayer;

    public string? PlayingAnimationName { get; private set; }

    public override void _Ready()
    {
        _animationPlayer = FindAnimationPlayer(this);
        if (_animationPlayer is null)
        {
            // A static prop parented under a motion node is a wiring mistake
            // worth surfacing rather than a silent no-op.
            GD.PushWarning($"[EnvironmentMotionPlayer] No AnimationPlayer under {GetPath()}; nothing to play.");
            return;
        }

        var names = _animationPlayer.GetAnimationList();
        if (names.Length == 0)
        {
            GD.PushWarning($"[EnvironmentMotionPlayer] AnimationPlayer under {GetPath()} has no animations.");
            return;
        }

        var animationName = names[0];
        var animation = _animationPlayer.GetAnimation(animationName);
        if (animation is null)
        {
            return;
        }

        animation.LoopMode = Animation.LoopModeEnum.Linear;
        var seed = ComputeSeed(GlobalPosition);
        _animationPlayer.SpeedScale = SpeedForSeed(seed, SpeedScale, SpeedVariation);
        _animationPlayer.Play(animationName);
        _animationPlayer.Seek(PhaseOffsetFor(seed, (float)animation.Length), update: true);
        PlayingAnimationName = animationName;
    }

    private static AnimationPlayer? FindAnimationPlayer(Node node)
    {
        foreach (var child in node.GetChildren())
        {
            if (child is AnimationPlayer player)
            {
                return player;
            }

            if (FindAnimationPlayer(child) is { } nested)
            {
                return nested;
            }
        }

        return null;
    }

    /// <summary>
    /// Stable 0..1 seed from a world position. Quantised to a centimetre so
    /// floating-point noise in the scene transform cannot change the result
    /// between runs.
    /// </summary>
    public static float ComputeSeed(Vector3 position)
    {
        var x = Mathf.RoundToInt(position.X * 100.0f);
        var z = Mathf.RoundToInt(position.Z * 100.0f);
        unchecked
        {
            // FNV-1a over the two coordinates, then a full avalanche mix.
            // A single weighted sum was tried first and produced near-identical
            // seeds for two props 21 m apart, so they swayed in visible lockstep.
            var hash = 2166136261u;
            hash = (hash ^ (uint)x) * 16777619u;
            hash = (hash ^ (uint)z) * 16777619u;
            hash ^= hash >> 15;
            hash *= 2246822519u;
            hash ^= hash >> 13;
            hash *= 3266489917u;
            hash ^= hash >> 16;
            return (hash & 0xFFFFFF) / 16777215.0f;
        }
    }

    /// <summary>Start time within the clip, always inside [0, length).</summary>
    public static float PhaseOffsetFor(float seed, float animationLength)
    {
        if (animationLength <= 0.0f)
        {
            return 0.0f;
        }

        return Mathf.Clamp(seed, 0.0f, 0.9999f) * animationLength;
    }

    /// <summary>Playback rate for a seed, within +/- <paramref name="variation"/>.</summary>
    public static float SpeedForSeed(float seed, float baseSpeed, float variation)
    {
        var clampedSeed = Mathf.Clamp(seed, 0.0f, 1.0f);
        var clampedVariation = Mathf.Clamp(variation, 0.0f, 0.5f);
        var factor = 1.0f + ((clampedSeed * 2.0f) - 1.0f) * clampedVariation;
        return Mathf.Max(0.05f, baseSpeed * factor);
    }
}
