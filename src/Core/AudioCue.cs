using Godot;

namespace TitanCraft.Core;

public static class AudioCue
{
    public static void Play(Node owner, NodePath cuePath)
    {
        if (cuePath.IsEmpty)
        {
            return;
        }

        owner.GetNodeOrNull<AudioStreamPlayer>(cuePath)?.Play();
    }
}
