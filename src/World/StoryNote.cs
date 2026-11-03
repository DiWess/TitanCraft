using System;
using Godot;
using TitanCraft.Core;
using TitanCraft.Missions;
using TitanCraft.Resources;

namespace TitanCraft.World;

/// <summary>
/// An optional discovery (workstream D): something left behind in the quarter
/// that tells part of its story without dialogue — a notice, a slate, a toy.
/// Reading one shows its text and counts it; nothing in the mission needs
/// them, so they reward looking rather than gate progress.
/// </summary>
public partial class StoryNote : Area3D, ICrashSiteInteractable, ILookHighlightTarget
{
    public const string Group = "story_notes";

    [Export] public string NoteId { get; set; } = string.Empty;

    /// <summary>What the prompt calls it: "Press E to read the {Noun}".</summary>
    [Export] public string Noun { get; set; } = "note";

    [Export] public string Title { get; set; } = string.Empty;

    [Export(PropertyHint.MultilineText)] public string Body { get; set; } = string.Empty;

    [Export] public NodePath GlintPath { get; set; } = "Glint";

    /// <summary>Raised when the note is read (first time or again).</summary>
    public static event Action<StoryNote>? Read;

    public bool HasBeenRead { get; private set; }

    public string InteractionPrompt => $"Press E to read the {Noun}";

    public bool IsInteractionAvailable => true;

    public override void _Ready()
    {
        AddToGroup(Group);
        CollisionLayer = 1u << 2;
        CollisionMask = 0;
        UpdateGlint();
    }

    public bool Interact(MvpInventory inventory, CrashSiteMissionState mission)
    {
        ArgumentNullException.ThrowIfNull(inventory);
        ArgumentNullException.ThrowIfNull(mission);

        var firstRead = !HasBeenRead;
        HasBeenRead = true;
        UpdateGlint();
        Read?.Invoke(this);
        if (firstRead)
            AudioCue.Play(this, "AudioLayer_State/State_Discovery");
        return true;
    }

    /// <summary>Reconstructs a note read before the save.</summary>
    public void RestoreRead()
    {
        HasBeenRead = true;
        UpdateGlint();
    }

    public void SetHighlighted(bool isHighlighted)
    {
        if (GetNodeOrNull<Node3D>("Visual") is { } visual)
            visual.Scale = Vector3.One * (isHighlighted ? 1.06f : 1f);
    }

    /// <summary>An unread note carries a faint glint so it can be found; a read one does not.</summary>
    private void UpdateGlint()
    {
        if (GetNodeOrNull<Node3D>(GlintPath) is { } glint)
            glint.Visible = !HasBeenRead;
    }
}
