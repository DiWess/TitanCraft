using Godot;

namespace TitanCraft.Core;

public partial class TimeManager : Node
{
    [Export(PropertyHint.Range, "0.01,1,0.01")] public float DefaultHitStopScale { get; set; } = 0.05f;
    [Export(PropertyHint.Range, "0.01,0.5,0.01")] public float DefaultHitStopDuration { get; set; } = 0.12f;

    private ulong _hitStopVersion;

    public void TriggerDefaultHitStop() => TriggerHitStop(DefaultHitStopScale, DefaultHitStopDuration);

    public async void TriggerHitStop(float durationScale, float realTimeDuration)
    {
        var version = ++_hitStopVersion;
        Engine.TimeScale = ClampTimeScale(durationScale);

        var timer = GetTree().CreateTimer(
            Mathf.Max(0.0f, realTimeDuration),
            processAlways: true,
            processInPhysics: false,
            ignoreTimeScale: true);

        await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);

        if (version == _hitStopVersion)
        {
            Engine.TimeScale = 1.0;
        }
    }

    public override void _ExitTree()
    {
        Engine.TimeScale = 1.0;
    }

    public static float ClampTimeScale(float durationScale) => Mathf.Clamp(durationScale, 0.01f, 1.0f);
}
