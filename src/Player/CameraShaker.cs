using Godot;

namespace TitanCraft.Player;

public partial class CameraShaker : Camera3D
{
    public const string CameraShakerGroup = "camera_shakers";

    [Export(PropertyHint.Range, "0,2,0.01")] public float TraumaDecayPerSecond { get; set; } = 1.35f;
    [Export(PropertyHint.Range, "0,2,0.01")] public float MaxPositionOffset { get; set; } = 0.16f;
    [Export(PropertyHint.Range, "0,15,0.1")] public float MaxRotationDegrees { get; set; } = 5.5f;
    [Export(PropertyHint.Range, "0,100,0.1")] public float NoiseSpeed { get; set; } = 28.0f;
    [Export(PropertyHint.Range, "0,6,0.05")] public float StrafeLeanDegrees { get; set; } = 1.6f;

    private readonly FastNoiseLite _noise = new();
    private readonly WeaponKick _kick = new();
    private readonly LandingImpact _landing = new();
    private Vector3 _basePosition;
    private Vector3 _baseRotation;
    private float _trauma;
    private float _time;
    private float _bobDistance;
    private float _bobIntensity;
    private float _strafeLean;

    public float Trauma => _trauma;

    /// <summary>
    /// Fed by the controller each physics frame. <paramref name="travelled"/> is
    /// the distance moved on the ground this frame, so bob advances with the
    /// player's stride instead of with wall-clock time.
    /// </summary>
    public void UpdateMovementFeel(float travelled, float intensity, float strafeInput)
    {
        _bobDistance += Mathf.Max(0.0f, travelled);
        _bobIntensity = Mathf.Clamp(intensity, 0.0f, 1.0f);
        _strafeLean = Mathf.Clamp(strafeInput, -1.0f, 1.0f);
    }

    public void AddKick(float pitchRadians, float yawRadians) => _kick.Add(pitchRadians, yawRadians);

    /// <summary>Returns true when the landing was hard enough to register.</summary>
    public bool ReportLanding(float impactSpeed) => _landing.Trigger(impactSpeed);

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
        var deltaSeconds = (float)delta;
        _time += deltaSeconds * NoiseSpeed;
        _trauma = Mathf.Max(0.0f, _trauma - TraumaDecayPerSecond * deltaSeconds);
        _kick.Tick(deltaSeconds);
        _landing.Tick(deltaSeconds);

        // Every camera-feel layer is additive on top of the rest pose, so they
        // compose instead of fighting over the transform: bob and lean while
        // moving, a dip on landing, a directional punch on a strike, and
        // random trauma shake on damage.
        var (bobOffset, bobRoll) = ViewBob.Sample(_bobDistance, _bobIntensity);
        var offset = bobOffset + new Vector3(0.0f, _landing.CurrentDip(), 0.0f);
        var rotation = new Vector3(
            _kick.PitchRadians,
            _kick.YawRadians,
            bobRoll - _strafeLean * Mathf.DegToRad(StrafeLeanDegrees));

        var intensity = CalculateShakeIntensity(_trauma);
        if (intensity > 0.0f)
        {
            offset += new Vector3(
                SampleNoise(0) * MaxPositionOffset * intensity,
                SampleNoise(17) * MaxPositionOffset * intensity,
                SampleNoise(31) * MaxPositionOffset * 0.45f * intensity);

            var maxRotationRadians = Mathf.DegToRad(MaxRotationDegrees) * intensity;
            rotation += new Vector3(
                SampleNoise(47) * maxRotationRadians,
                SampleNoise(59) * maxRotationRadians,
                SampleNoise(71) * maxRotationRadians * 0.65f);
        }

        Position = _basePosition + offset;
        Rotation = _baseRotation + rotation;
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
