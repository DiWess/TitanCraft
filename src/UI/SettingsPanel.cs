using System;
using System.Collections.Generic;
using Godot;
using TitanCraft.Core;

namespace TitanCraft.UI;

/// <summary>
/// The settings screen (README section 17, "paramètres minimaux"), shared by
/// the main menu and the pause menu: mouse sensitivity, invert look, field of
/// view, master volume, fullscreen, VSync, reduced camera shake, and a key for
/// every action. Changes apply as they are made and are saved locally when
/// the screen closes. Built in code so both menus get the same screen without
/// two copies of its layout.
/// </summary>
public partial class SettingsPanel : PanelContainer
{
    public event Action? Closed;

    private GameSettingsData _working = new();
    private readonly Dictionary<string, Button> _bindingButtons = new();
    private string? _listeningFor;
    private Label? _sensitivityValue;
    private Label? _fovValue;
    private Label? _volumeValue;

    public bool IsListeningForKey => _listeningFor is not null;

    public override void _Ready()
    {
        GameSettings.EnsureLoaded();
        _working = GameSettings.Current.Copy();
        ProcessMode = ProcessModeEnum.Always;
        Build();
    }

    public void Open()
    {
        _working = GameSettings.Current.Copy();
        foreach (var child in GetChildren())
            child.QueueFree();
        _bindingButtons.Clear();
        Build();
        Visible = true;
    }

    public void Close()
    {
        _listeningFor = null;
        GameSettings.Set(_working.Copy());
        GameSettings.Save();
        Visible = false;
        Closed?.Invoke();
    }

    private void Build()
    {
        var margin = new MarginContainer();
        foreach (var side in new[] { "left", "right", "top", "bottom" })
            margin.AddThemeConstantOverride($"margin_{side}", 18);
        AddChild(margin);
        var scroll = new ScrollContainer
        {
            HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
            CustomMinimumSize = new Vector2(480, 520),
        };
        margin.AddChild(scroll);
        var list = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
        list.AddThemeConstantOverride("separation", 8);
        scroll.AddChild(list);

        list.AddChild(Heading("Settings", 26));

        list.AddChild(Heading("Controls", 17));
        _sensitivityValue = AddSlider(list, "Mouse sensitivity", GameSettingsData.MinSensitivityScale,
            GameSettingsData.MaxSensitivityScale, 0.05, _working.MouseSensitivityScale,
            value => { _working.MouseSensitivityScale = (float)value; return $"{value:0.00}×"; });
        AddCheck(list, "Invert look up/down", _working.InvertY, on => _working.InvertY = on);

        list.AddChild(Heading("Video", 17));
        _fovValue = AddSlider(list, "Field of view", GameSettingsData.MinFieldOfView, GameSettingsData.MaxFieldOfView, 1,
            _working.FieldOfView, value => { _working.FieldOfView = (float)value; return $"{value:0}°"; });
        AddCheck(list, "Fullscreen", _working.Fullscreen, on => { _working.Fullscreen = on; ApplyLive(); });
        AddCheck(list, "VSync", _working.VSync, on => { _working.VSync = on; ApplyLive(); });
        AddCheck(list, "Reduce camera shake", _working.ReduceCameraShake, on => _working.ReduceCameraShake = on);

        list.AddChild(Heading("Audio", 17));
        _volumeValue = AddSlider(list, "Master volume", 0, 1, 0.05, _working.MasterVolume,
            value => { _working.MasterVolume = (float)value; ApplyLive(); return $"{value * 100:0}%"; });

        list.AddChild(Heading("Keys", 17));
        foreach (var (action, label) in GameSettings.RemappableActions)
        {
            var row = new HBoxContainer();
            row.AddChild(new Label { Text = label, SizeFlagsHorizontal = SizeFlags.ExpandFill });
            var button = new Button { Text = GameSettings.Describe(action), CustomMinimumSize = new Vector2(190, 0) };
            var captured = action;
            button.Pressed += () => BeginListening(captured);
            _bindingButtons[action] = button;
            row.AddChild(button);
            list.AddChild(row);
        }
        list.AddChild(new Label
        {
            Text = "Mouse look and Esc (pause) are fixed.",
            Modulate = new Color(1, 1, 1, 0.6f),
        });

        var buttons = new HBoxContainer { Alignment = BoxContainer.AlignmentMode.End };
        buttons.AddThemeConstantOverride("separation", 10);
        var reset = new Button { Text = "Reset to defaults" };
        reset.Pressed += ResetToDefaults;
        var back = new Button { Text = "Back", Name = "BackButton" };
        back.Pressed += Close;
        buttons.AddChild(reset);
        buttons.AddChild(back);
        list.AddChild(buttons);
    }

    private static Label Heading(string text, int size)
    {
        var label = new Label { Text = text };
        label.AddThemeFontSizeOverride("font_size", size);
        label.AddThemeColorOverride("font_color", new Color(0.95f, 0.79f, 0.55f));
        return label;
    }

    private Label AddSlider(VBoxContainer list, string label, double min, double max, double step, double value,
        Func<double, string> onChanged)
    {
        var row = new HBoxContainer();
        row.AddChild(new Label { Text = label, CustomMinimumSize = new Vector2(170, 0) });
        var slider = new HSlider
        {
            MinValue = min,
            MaxValue = max,
            Step = step,
            Value = value,
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ShrinkCenter,
            Name = label.Replace(" ", string.Empty),
        };
        var readout = new Label { CustomMinimumSize = new Vector2(64, 0), HorizontalAlignment = HorizontalAlignment.Right };
        readout.Text = onChanged(value);
        slider.ValueChanged += changed => readout.Text = onChanged(changed);
        row.AddChild(slider);
        row.AddChild(readout);
        list.AddChild(row);
        return readout;
    }

    private void AddCheck(VBoxContainer list, string label, bool value, Action<bool> onToggled)
    {
        var check = new CheckButton { Text = label, ButtonPressed = value, Name = label.Replace(" ", string.Empty) };
        check.Toggled += on => onToggled(on);
        list.AddChild(check);
    }

    private void ApplyLive() => GameSettings.Set(_working.Copy());

    private void BeginListening(string action)
    {
        _listeningFor = action;
        _bindingButtons[action].Text = "Press a key or button…";
    }

    public override void _Input(InputEvent @event)
    {
        if (_listeningFor is null || !Visible)
            return;

        if (@event is InputEventKey { Pressed: true, Keycode: Key.Escape })
        {
            // Escape cancels the rebind instead of becoming the binding.
            _bindingButtons[_listeningFor].Text = GameSettings.Describe(_listeningFor);
            _listeningFor = null;
            GetViewport().SetInputAsHandled();
            return;
        }

        if (@event is not (InputEventKey { Pressed: true, Echo: false } or InputEventMouseButton { Pressed: true }))
            return;

        if (GameSettings.FromEvent(@event) is not { } binding)
            return;

        _working.Bindings[_listeningFor] = binding;
        var action = _listeningFor;
        _listeningFor = null;
        GameSettings.Set(_working.Copy());
        _bindingButtons[action].Text = GameSettings.Describe(action);
        GetViewport().SetInputAsHandled();
    }

    private void ResetToDefaults()
    {
        _listeningFor = null;
        _working = new GameSettingsData();
        GameSettings.Set(_working.Copy());
        foreach (var child in GetChildren())
            child.QueueFree();
        _bindingButtons.Clear();
        Build();
    }
}
