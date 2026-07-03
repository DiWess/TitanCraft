using Godot;

namespace TitanCraft.Player;

public partial class CameraShaker : Camera3D
{
    public const string CameraShakerGroup = "camera_shakers";

    [Export(PropertyHint.Range, "0,2,0.01")] public float TraumaDecayPerSecond { get; set; } = 1.35f;
    [Export(PropertyHint.Range, "0,2,0.01")] public float MaxPositionOffset { get; set; } = 0.16f;
    [Export(PropertyHint.Range, "0,15,0.1")] public float MaxRotationDegrees { get; set; } = 5.5f;
    [Export(PropertyHint.Range, "0,100,0.1")] public float NoiseSpeed { get; set; } = 28.0f;

    private readonly FastNoiseLite _noise = new();
    private Vector3 _basePosition;
    private Vector3 _baseRotation;
    private float _trauma;
    private float _time;

    public float Trauma => _trauma;

    public override void _Ready()
    {
        _basePosition = Position;
        _baseRotation = Rotation;
        _noise.Seed = 1337;
        _noise.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
        AddToGroup(CameraShakerGroup);
    }

    public override void _Process(double delta)
    {
        _time += (float)delta * NoiseSpeed;
        _trauma = Mathf.Max(0.0f, _trauma - TraumaDecayPerSecond * (float)delta);

        var intensity = CalculateShakeIntensity(_trauma);
        if (intensity <= 0.0f)
        {
            Position = _basePosition;
            Rotation = _baseRotation;
            return;
        }

        Position = _basePosition + new Vector3(
            SampleNoise(0) * MaxPositionOffset * intensity,
            SampleNoise(17) * MaxPositionOffset * intensity,
            SampleNoise(31) * MaxPositionOffset * 0.45f * intensity);

        var maxRotationRadians = Mathf.DegToRad(MaxRotationDegrees) * intensity;
        Rotation = _baseRotation + new Vector3(
            SampleNoise(47) * maxRotationRadians,
            SampleNoise(59) * maxRotationRadians,
            SampleNoise(71) * maxRotationRadians * 0.65f);
    }

    public void AddTrauma(float amount)
    {
        _trauma = Mathf.Clamp(_trauma + amount, 0.0f, 1.0f);
    }

    public static float CalculateShakeIntensity(float trauma)
    {
        var clampedTrauma = Mathf.Clamp(trauma, 0.0f, 1.0f);
        return clampedTrauma * clampedTrauma;
    }

    private float SampleNoise(int channelOffset) => _noise.GetNoise1D(_time + channelOffset);
}
