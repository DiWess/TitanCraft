using Godot;

namespace TitanCraft.Core;

/// <summary>
/// Audio event trigger helper for gameplay integration.
/// Supports both local (relative to caller) and string paths for flexible audio architecture.
/// </summary>
public static class AudioCue
{
    /// <summary>
    /// Play an audio cue by NodePath (relative to owner).
    /// </summary>
    public static void Play(Node owner, NodePath cuePath)
    {
        if (cuePath.IsEmpty)
        {
            return;
        }

        owner.GetNodeOrNull<AudioStreamPlayer>(cuePath)?.Play();
    }

    /// <summary>
    /// Play an audio cue by string path. Tries local path first (relative to caller),
    /// then falls back to scene-root path if not found locally.
    /// </summary>
    /// <param name="caller">The node calling this method (typically 'this' from a gameplay script).</param>
    /// <param name="audioPath">Node path to the AudioStreamPlayer.
    /// Examples:
    ///   "Head/Camera3D/ArmHitAudio" — relative to caller (local path)
    ///   "AudioLayer_Player/Weapon_Swing" — relative to scene root (global path)
    /// </param>
    public static void Play(Node caller, string audioPath)
    {
        if (caller == null || string.IsNullOrEmpty(audioPath))
        {
            return;
        }

        // Try local path first (relative to caller)
        var audioPlayer = caller.GetNodeOrNull<AudioStreamPlayer>(audioPath);
        if (audioPlayer != null)
        {
            audioPlayer.Play();
            return;
        }

        // Fall back to a scene-level path ("AudioLayer_State/...")
        FindInAncestors<AudioStreamPlayer>(caller, audioPath)?.Play();
    }

    /// <summary>
    /// Resolves a scene-level cue path from the caller's nearest ancestor that
    /// has it. This used to be GetTree().Root.GetChild(0), which is the game
    /// scene in play but the test runner under test, and would become an
    /// autoload the moment the project gained one.
    /// </summary>
    private static T? FindInAncestors<T>(Node caller, string audioPath) where T : Node
    {
        for (var node = caller.GetParent(); node is not null; node = node.GetParent())
        {
            if (node.GetNodeOrNull<T>(audioPath) is { } found)
            {
                return found;
            }
        }

        return null;
    }

    /// <summary>
    /// Play a 3D audio cue at a specific position in the world.
    /// Supports both local and scene-root paths like Play().
    /// </summary>
    /// <param name="caller">The node calling this method.</param>
    /// <param name="audioPath">Node path to the AudioStreamPlayer3D.</param>
    /// <param name="globalPosition">World position where the sound should originate.</param>
    public static void Play3D(Node caller, string audioPath, Vector3 globalPosition)
    {
        if (caller == null || string.IsNullOrEmpty(audioPath))
        {
            return;
        }

        // Try local path first
        var audioPlayer = caller.GetNodeOrNull<AudioStreamPlayer3D>(audioPath);
        if (audioPlayer != null)
        {
            audioPlayer.GlobalPosition = globalPosition;
            audioPlayer.Play();
            return;
        }

        audioPlayer = FindInAncestors<AudioStreamPlayer3D>(caller, audioPath);
        if (audioPlayer != null)
        {
            audioPlayer.GlobalPosition = globalPosition;
            audioPlayer.Play();
        }
    }

    /// <summary>
    /// Stop an audio cue by path (local or scene-root).
    /// </summary>
    public static void Stop(Node caller, string audioPath)
    {
        if (caller == null || string.IsNullOrEmpty(audioPath))
        {
            return;
        }

        var audioPlayer = caller.GetNodeOrNull<AudioStreamPlayer>(audioPath);
        if (audioPlayer != null)
        {
            audioPlayer.Stop();
            return;
        }

        FindInAncestors<AudioStreamPlayer>(caller, audioPath)?.Stop();
    }
}
