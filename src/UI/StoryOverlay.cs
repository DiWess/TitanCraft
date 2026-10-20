using Godot;
using TitanCraft.World;

namespace TitanCraft.UI;

/// <summary>
/// Shows a discovered note's text over the play view: a title and a few
/// lines, fading out after a while or on the next note. Play continues
/// underneath; reading never pauses the game, so a note cannot be a trap.
/// </summary>
public partial class StoryOverlay : CanvasLayer
{
    [Export] public float HoldSeconds { get; set; } = 9f;

    private Control? _panel;
    private Label? _title;
    private Label? _body;
    private Label? _count;
    private Tween? _tween;

    public string ShownTitle => _title?.Text ?? string.Empty;

    public bool IsShowing => _panel is { Visible: true } && _panel.Modulate.A > 0.01f;

    public override void _Ready()
    {
        Layer = 4;
        _panel = new PanelContainer { Name = "NotePanel", Visible = false, MouseFilter = Control.MouseFilterEnum.Ignore };
        var style = new StyleBoxFlat
        {
            BgColor = new Color(0.07f, 0.065f, 0.06f, 0.88f),
            BorderColor = new Color(0.86f, 0.74f, 0.52f, 0.8f),
            ContentMarginLeft = 22, ContentMarginRight = 22, ContentMarginTop = 16, ContentMarginBottom = 16,
        };
        style.SetBorderWidthAll(1);
        style.SetCornerRadiusAll(3);
        _panel.AddThemeStyleboxOverride("panel", style);
        _panel.SetAnchorsPreset(Control.LayoutPreset.CenterRight);
        _panel.OffsetLeft = -470;
        _panel.OffsetRight = -28;
        _panel.OffsetTop = -120;
        _panel.OffsetBottom = 120;
        var column = new VBoxContainer();
        column.AddThemeConstantOverride("separation", 8);
        _title = new Label { Name = "Title" };
        _title.AddThemeFontSizeOverride("font_size", 19);
        _title.AddThemeColorOverride("font_color", new Color(0.95f, 0.82f, 0.58f));
        _body = new Label { Name = "Body", AutowrapMode = TextServer.AutowrapMode.WordSmart, CustomMinimumSize = new Vector2(400, 0) };
        _body.AddThemeFontSizeOverride("font_size", 15);
        _body.AddThemeColorOverride("font_color", new Color(0.92f, 0.9f, 0.86f));
        _count = new Label { Name = "Count" };
        _count.AddThemeFontSizeOverride("font_size", 12);
        _count.AddThemeColorOverride("font_color", new Color(0.7f, 0.68f, 0.64f));
        column.AddChild(_title);
        column.AddChild(_body);
        column.AddChild(_count);
        _panel.AddChild(column);
        AddChild(_panel);
        StoryNote.Read += OnNoteRead;
    }

    public override void _ExitTree()
    {
        StoryNote.Read -= OnNoteRead;
    }

    private void OnNoteRead(StoryNote note)
    {
        // Several game scenes can exist at once in tests: only show notes from ours.
        if (!IsInsideTree() || note.GetTree() != GetTree() || !GetParent().IsAncestorOf(note))
            return;

        Show(note.Title, note.Body, CountText());
    }

    public void Show(string title, string body, string footer)
    {
        if (_panel is null || _title is null || _body is null || _count is null)
            return;

        _title.Text = title;
        _body.Text = body;
        _count.Text = footer;
        _panel.Visible = true;
        _panel.Modulate = Colors.White with { A = 0f };
        _tween?.Kill();
        _tween = CreateTween();
        _tween.TweenProperty(_panel, "modulate:a", 1.0, 0.35);
        _tween.TweenInterval(HoldSeconds);
        _tween.TweenProperty(_panel, "modulate:a", 0.0, 0.8);
        _tween.TweenCallback(Callable.From(() => _panel.Visible = false));
    }

    private string CountText()
    {
        var total = 0;
        var read = 0;
        foreach (var node in GetParent().FindChildren("*", "", true, false))
        {
            if (node is not StoryNote note)
                continue;
            total++;
            if (note.HasBeenRead)
                read++;
        }

        return $"Discoveries: {read} of {total}";
    }
}
