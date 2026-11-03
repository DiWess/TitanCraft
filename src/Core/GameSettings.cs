using System;
using System.Collections.Generic;
using Godot;

namespace TitanCraft.Core;

/// <summary>
/// The player's settings, as plain values with their legal ranges. Kept free
/// of engine state so the clamping and the bindings format are unit-tested;
/// <see cref="GameSettings"/> owns loading, saving and applying them.
/// </summary>
public sealed class GameSettingsData
{
    public const float MinSensitivityScale = 0.25f;
    public const float MaxSensitivityScale = 3.0f;
    public const float MinFieldOfView = 60f;
    public const float MaxFieldOfView = 100f;
    public const float DefaultFieldOfView = 75f;

    public float MouseSensitivityScale { get; set; } = 1f;
    public bool InvertY { get; set; }
    public float FieldOfView { get; set; } = DefaultFieldOfView;
    public float MasterVolume { get; set; } = 1f;
    public bool Fullscreen { get; set; }
    public bool VSync { get; set; } = true;
    public bool ReduceCameraShake { get; set; }

    /// <summary>
    /// Player-chosen primary binding per action, as "key:&lt;physical keycode&gt;"
    /// or "mouse:&lt;button index&gt;". Actions without an entry keep the
    /// project defaults (which include both QWERTY and AZERTY movement keys).
    /// </summary>
    public Dictionary<string, string> Bindings { get; } = new();

    public void Clamp()
    {
        MouseSensitivityScale = Math.Clamp(Finite(MouseSensitivityScale, 1f), MinSensitivityScale, MaxSensitivityScale);
        FieldOfView = Math.Clamp(Finite(FieldOfView, DefaultFieldOfView), MinFieldOfView, MaxFieldOfView);
        MasterVolume = Math.Clamp(Finite(MasterVolume, 1f), 0f, 1f);
        foreach (var action in new List<string>(Bindings.Keys))
        {
            if (!IsValidBinding(Bindings[action]))
                Bindings.Remove(action);
        }
    }

    public GameSettingsData Copy()
    {
        var copy = new GameSettingsData
        {
            MouseSensitivityScale = MouseSensitivityScale,
            InvertY = InvertY,
            FieldOfView = FieldOfView,
            MasterVolume = MasterVolume,
            Fullscreen = Fullscreen,
            VSync = VSync,
            ReduceCameraShake = ReduceCameraShake,
        };
        foreach (var (action, binding) in Bindings)
            copy.Bindings[action] = binding;
        return copy;
    }

    public static bool IsValidBinding(string? binding)
    {
        if (string.IsNullOrWhiteSpace(binding))
            return false;

        var parts = binding.Split(':');
        return parts.Length == 2
            && (parts[0] == "key" || parts[0] == "mouse")
            && long.TryParse(parts[1], out var code)
            && code > 0;
    }

    /// <summary>Linear 0-1 volume to the bus's decibels; 0 is silence.</summary>
    public static float VolumeToDb(float linear) => linear <= 0.001f ? -80f : 20f * MathF.Log10(linear);

    private static float Finite(float value, float fallback) => float.IsFinite(value) ? value : fallback;
}

/// <summary>
/// Loads, saves and applies <see cref="GameSettingsData"/> (user://settings.cfg,
/// local only). Applied once at the main menu and whenever the settings
/// screen changes something; gameplay nodes read <see cref="Current"/> and
/// listen to <see cref="Changed"/>.
/// </summary>
public static class GameSettings
{
    public const string DefaultPath = "user://settings.cfg";

    /// <summary>Actions the settings screen lets the player rebind.</summary>
    public static readonly IReadOnlyList<(string Action, string Label)> RemappableActions =
    [
        ("move_forward", "Move forward"),
        ("move_backward", "Move back"),
        ("move_left", "Move left"),
        ("move_right", "Move right"),
        ("jump", "Jump"),
        ("sprint", "Sprint"),
        ("interact", "Interact"),
        ("attack", "Strike / charge"),
        ("pull", "Pull (Mk II)"),
    ];

    public static GameSettingsData Current { get; private set; } = new();

    public static bool IsLoaded { get; private set; }

    public static event Action? Changed;

    public static void EnsureLoaded(string path = DefaultPath)
    {
        if (!IsLoaded)
            Load(path);
    }

    public static void Load(string path = DefaultPath)
    {
        var data = new GameSettingsData();
        var config = new ConfigFile();
        if (config.Load(path) == Error.Ok)
        {
            data.MouseSensitivityScale = (float)config.GetValue("controls", "mouse_sensitivity_scale", 1.0).AsDouble();
            data.InvertY = config.GetValue("controls", "invert_y", false).AsBool();
            data.FieldOfView = (float)config.GetValue("video", "field_of_view", GameSettingsData.DefaultFieldOfView).AsDouble();
            data.Fullscreen = config.GetValue("video", "fullscreen", false).AsBool();
            data.VSync = config.GetValue("video", "vsync", true).AsBool();
            data.ReduceCameraShake = config.GetValue("video", "reduce_camera_shake", false).AsBool();
            data.MasterVolume = (float)config.GetValue("audio", "master_volume", 1.0).AsDouble();
            if (config.HasSection("bindings"))
            {
                foreach (var action in config.GetSectionKeys("bindings"))
                    data.Bindings[action] = config.GetValue("bindings", action, string.Empty).AsString();
            }
        }

        data.Clamp();
        Current = data;
        IsLoaded = true;
    }

    public static void Save(string path = DefaultPath)
    {
        var config = new ConfigFile();
        config.SetValue("controls", "mouse_sensitivity_scale", Current.MouseSensitivityScale);
        config.SetValue("controls", "invert_y", Current.InvertY);
        config.SetValue("video", "field_of_view", Current.FieldOfView);
        config.SetValue("video", "fullscreen", Current.Fullscreen);
        config.SetValue("video", "vsync", Current.VSync);
        config.SetValue("video", "reduce_camera_shake", Current.ReduceCameraShake);
        config.SetValue("audio", "master_volume", Current.MasterVolume);
        foreach (var (action, binding) in Current.Bindings)
            config.SetValue("bindings", action, binding);
        var error = config.Save(path);
        if (error != Error.Ok)
            GD.PushWarning($"Could not save settings to '{path}': {error}");
    }

    /// <summary>Replaces the current settings, applies them, and tells listeners.</summary>
    public static void Set(GameSettingsData data)
    {
        data.Clamp();
        Current = data;
        IsLoaded = true;
        Apply();
    }

    public static void Apply()
    {
        var master = AudioServer.GetBusIndex("Master");
        if (master >= 0)
        {
            AudioServer.SetBusVolumeDb(master, GameSettingsData.VolumeToDb(Current.MasterVolume));
            AudioServer.SetBusMute(master, Current.MasterVolume <= 0.001f);
        }

        // A headless run has no window to resize.
        if (DisplayServer.GetName() != "headless")
        {
            DisplayServer.WindowSetMode(Current.Fullscreen
                ? DisplayServer.WindowMode.Fullscreen
                : DisplayServer.WindowMode.Windowed);
            DisplayServer.WindowSetVsyncMode(Current.VSync
                ? DisplayServer.VSyncMode.Enabled
                : DisplayServer.VSyncMode.Disabled);
        }

        ApplyBindings();
        Changed?.Invoke();
    }

    /// <summary>Restores the project's input map, then lays the player's choices over it.</summary>
    public static void ApplyBindings()
    {
        InputMap.LoadFromProjectSettings();
        foreach (var (action, binding) in Current.Bindings)
        {
            if (!InputMap.HasAction(action) || ToEvent(binding) is not { } inputEvent)
                continue;

            InputMap.ActionEraseEvents(action);
            InputMap.ActionAddEvent(action, inputEvent);
        }
    }

    public static string? FromEvent(InputEvent inputEvent) => inputEvent switch
    {
        InputEventKey key when key.PhysicalKeycode != Key.None => $"key:{(long)key.PhysicalKeycode}",
        InputEventKey key when key.Keycode != Key.None => $"key:{(long)key.Keycode}",
        InputEventMouseButton mouse when mouse.ButtonIndex != MouseButton.None => $"mouse:{(long)mouse.ButtonIndex}",
        _ => null,
    };

    public static InputEvent? ToEvent(string binding)
    {
        if (!GameSettingsData.IsValidBinding(binding))
            return null;

        var parts = binding.Split(':');
        var code = long.Parse(parts[1]);
        return parts[0] == "key"
            ? new InputEventKey { PhysicalKeycode = (Key)code }
            : new InputEventMouseButton { ButtonIndex = (MouseButton)code };
    }

    /// <summary>What an action is bound to, in words, for the settings screen.</summary>
    public static string Describe(string action)
    {
        if (!InputMap.HasAction(action))
            return "—";

        var names = new List<string>();
        foreach (var inputEvent in InputMap.ActionGetEvents(action))
        {
            names.Add(inputEvent switch
            {
                InputEventKey key when key.PhysicalKeycode != Key.None => OS.GetKeycodeString(DisplayServer.KeyboardGetKeycodeFromPhysical(key.PhysicalKeycode)),
                InputEventKey key => OS.GetKeycodeString(key.Keycode),
                InputEventMouseButton { ButtonIndex: MouseButton.Left } => "Left click",
                InputEventMouseButton { ButtonIndex: MouseButton.Right } => "Right click",
                InputEventMouseButton { ButtonIndex: MouseButton.Middle } => "Middle click",
                InputEventMouseButton mouse => $"Mouse {(int)mouse.ButtonIndex}",
                _ => inputEvent.AsText(),
            });
        }

        return names.Count == 0 ? "—" : string.Join(" / ", names);
    }
}
