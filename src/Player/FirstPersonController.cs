using System;
using Godot;
using TitanCraft.Combat;
using TitanCraft.Core;
using TitanCraft.Crafting;
using TitanCraft.Enemies;
using TitanCraft.Missions;
using TitanCraft.Resources;
using TitanCraft.UI;
using TitanCraft.World;

namespace TitanCraft.Player;

public partial class FirstPersonController : CharacterBody3D
{
    public event Action<string>? InteractionPromptChanged;
    public const string GalaxabrainScoutHitFeedback = "Mechanical Arm strike landed.";
    public const string GalaxabrainScoutDefeatFeedback = "Galaxabrain Scout disabled — recover the component.";
    public const string GalaxabrainComponentRecoveryFeedback = "Galaxabrain component recovered — fit it at the workbench as Mk II.";
    public const string SavePointSuccessFeedback = "Checkpoint saved — suit repaired.";
    public const string BeaconActivationFeedback = "Beacon activated — rescue signal online.";
    public const string BeaconChargingFeedback = "Power core installed — the beacon is charging. Hold the plaza!";
    public const string DefeatRetryFeedback = "Suit integrity failed — reload from checkpoint.";
    public const string ResourceCompletionFeedback = "Resources secured — craft the Mechanical Arm Mk I at the workbench.";
    public const string MechanicalArmCraftSuccessFeedback = "Mechanical Arm Mk I online — defeat the Galaxabrain Scout.";
    public const string Mk2CraftSuccessFeedback = "Mk II Magnet Clamp online — right click pulls. Open the Net-makers' Yard.";
    public const string Mk3CraftSuccessFeedback = "Mk III Breaker Coil online — hold left click to charge, release to strike.";
    public const string ChargedStrikeFeedback = "Charged strike landed.";
    public const string PullMissFeedback = "Nothing to pull there — aim at loose wreckage or a drone.";
    public const string PullBlockedFeedback = "Pull needs the Mk II Magnet Clamp — fit the Scout's component at the workbench.";
    public const string PowerCoreRecoveryFeedback = "Power core recovered — install it at the beacon.";
    public const string WorkbenchCheckpointSuffix = " Suit repaired, checkpoint saved.";

    public event Action<string>? ActionFeedbackChanged;

    /// <summary>Look travel this frame, in radians — drives onboarding step 1.</summary>
    public event Action<float>? LookTravelled;

    /// <summary>Ground distance travelled this frame, in metres — onboarding step 2.</summary>
    public event Action<float>? GroundTravelled;

    /// <summary>Raised on a jump that actually left the floor — onboarding step 3.</summary>
    public event Action? Jumped;

    /// <summary>Raised when a Mk I strike connects — onboarding step 6.</summary>
    public event Action? AttackLanded;

    /// <summary>Raised when the Mk II clamp yanks something.</summary>
    public event Action<IPullTarget>? PullLanded;

    /// <summary>Raised with the module tier after any workbench craft.</summary>
    public event Action<int>? ArmCrafted;
    [Export] public float WalkSpeed { get; set; } = 5.0f;
    [Export] public float JumpVelocity { get; set; } = 4.5f;
    [Export] public float MouseSensitivity { get; set; } = 0.0025f;
    [Export] public float MaxLookAngleDegrees { get; set; } = 85.0f;
    [Export] public float InteractionRange { get; set; } = 3.0f;
    [Export] public float AttackRange { get; set; } = MechanicalArmAttackLogic.DefaultRange;
    [Export] public int MechanicalArmDamage { get; set; } = MechanicalArmAttackLogic.DefaultMechanicalArmDamage;
    [Export] public float AttackCooldownSeconds { get; set; } = MechanicalArmAttackLogic.DefaultCooldownSeconds;
    [Export] public float FallDefeatHeight { get; set; } = -10.0f;
    [Export] public NodePath ArmHitAudioPath { get; set; } = "Head/Camera3D/ArmHitAudio";
    [Export] public NodePath DamageAudioPath { get; set; } = "Head/Camera3D/DamageAudio";
    [Export] public NodePath TimeManagerPath { get; set; } = "TimeManager";
    [Export] public float BaseFov { get; set; } = 75.0f;
    [Export] public float SprintFov { get; set; } = 90.0f;
    [Export] public float FovLerpSpeed { get; set; } = 8.0f;
    [Export] public float SprintSpeedMultiplier { get; set; } = 1.15f;
    [Export] public float WalkFootstepIntervalSeconds { get; set; } = 0.45f;
    [Export] public float PullRange { get; set; } = MagnetPullLogic.DefaultRange;
    [Export] public float ChargedStrikeRange { get; set; } = ChargedStrikeLogic.DefaultChargedRange;
    [Export] public int ChargedStrikeDamage { get; set; } = ChargedStrikeLogic.DefaultChargedDamage;

    /// <summary>Movement while holding a charge: the coil is heavy.</summary>
    [Export] public float ChargingSpeedMultiplier { get; set; } = 0.6f;
    [Export] public float SprintFootstepIntervalSeconds { get; set; } = 0.32f;

    // Upward pitch punch with a slight rightward bias, matching the arm's jab.
    public const float HitKickPitchRadians = 0.035f;
    public const float HitKickYawRadians = 0.012f;
    public const float SwingKickPitchRadians = 0.014f;

    public MvpInventory Inventory { get; } = new();

    public CrashSiteMissionState Mission { get; } = new();

    public PlayerHealth Health { get; } = new();

    private Camera3D _camera = null!;
    private Node3D _head = null!;
    private MeshInstance3D? _mechanicalArmVisual;
    private MeshInstance3D? _bareArmVisual;
    private Vector3 _mechanicalArmBaseRotation;
    private Tween? _armSwingTween;
    private float _gravity;
    private float _cameraPitch;
    private float _bodyYaw;
    private MechanicalArmAttackLogic _mechanicalArmAttack = null!;
    private MechanicalArmRecipe _mechanicalArmRecipe = null!;
    private readonly MagnetPullLogic _magnetPull = new();
    private readonly ChargedStrikeLogic _chargedStrike = new();
    private float _chargedRecoveryRemaining;
    private Vector3? _lastDamageOrigin;
    private Node3D? _clampModule;
    private Node3D? _coilModule;
    private MeshInstance3D? _coilGlow;
    private MeshInstance3D? _pullBeam;
    private Tween? _pullBeamTween;
    private bool _wasFullyCharged;
    private float _sensitivityScale = 1f;
    private bool _invertY;

    /// <summary>Base field of view before settings; sprint adds the same margin on top of the player's choice.</summary>
    private float _authoredBaseFov;
    private float _authoredSprintMargin;
    private bool _authoredFovCaptured;

    private void ApplySettings()
    {
        if (!_authoredFovCaptured)
        {
            _authoredBaseFov = BaseFov;
            _authoredSprintMargin = SprintFov - BaseFov;
            _authoredFovCaptured = true;
        }

        var settings = GameSettings.Current;
        _sensitivityScale = settings.MouseSensitivityScale;
        _invertY = settings.InvertY;
        BaseFov = settings.FieldOfView;
        SprintFov = settings.FieldOfView + _authoredSprintMargin;
    }
    private CameraShaker? _cameraShaker;
    private TimeManager? _timeManager;
    private int _lastHealth;
    private string _interactionPrompt = string.Empty;
    private ILookHighlightTarget? _currentLookTarget;
    private bool _debugHighlightAllResourceDrops;
    private float _footstepTimer;
    private int _footstepAudioIndex;
    private bool _wasAttackOnCooldown;
    private readonly ViewmodelSway _viewmodelSway = new();
    private CombatFeedbackOverlay? _combatOverlay;
    private Vector3 _mechanicalArmBasePosition;
    private Vector3 _bareArmBasePosition;
    private Vector3 _bareArmBaseRotation;
    private Vector2 _lookDeltaThisFrame;
    private bool _wasOnFloor = true;
    private float _previousFallSpeed;

    // No ground-material tagging exists yet, so this is a fixed rotation across
    // all three recorded surfaces rather than genuine surface detection -- it
    // exists so Footsteps_Metal/Rock/Ash (already produced, never triggered by
    // any code) actually play instead of the player moving in total silence.
    private static readonly string[] FootstepAudioPaths =
    {
        "AudioLayer_Player/Footsteps_Metal",
        "AudioLayer_Player/Footsteps_Rock",
        "AudioLayer_Player/Footsteps_Ash",
    };

    public override void _Ready()
    {
        _head = GetNode<Node3D>("Head");
        _camera = GetNode<Camera3D>("Head/Camera3D");
        _camera.Current = true;
        ApplySettings();
        GameSettings.Changed += ApplySettings;
        _camera.Fov = BaseFov;
        _cameraShaker = _camera as CameraShaker;
        _timeManager = GetNodeOrNull<TimeManager>(TimeManagerPath);
        _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
        _mechanicalArmAttack = new MechanicalArmAttackLogic(MechanicalArmDamage, AttackCooldownSeconds);
        _mechanicalArmRecipe = new MechanicalArmRecipe();
        _mechanicalArmVisual = GetNodeOrNull<MeshInstance3D>("Head/Camera3D/MechanicalArmVisual");
        _bareArmVisual = GetNodeOrNull<MeshInstance3D>("Head/Camera3D/BareArmVisual");
        _mechanicalArmBaseRotation = _mechanicalArmVisual?.Rotation ?? Vector3.Zero;
        _mechanicalArmBasePosition = _mechanicalArmVisual?.Position ?? Vector3.Zero;
        _bareArmBasePosition = _bareArmVisual?.Position ?? Vector3.Zero;
        _bareArmBaseRotation = _bareArmVisual?.Rotation ?? Vector3.Zero;
        _clampModule = GetNodeOrNull<Node3D>("Head/Camera3D/MechanicalArmVisual/ClampModule");
        _coilModule = GetNodeOrNull<Node3D>("Head/Camera3D/MechanicalArmVisual/CoilModule");
        _coilGlow = GetNodeOrNull<MeshInstance3D>("Head/Camera3D/MechanicalArmVisual/CoilModule/CoilGlow");
        _pullBeam = GetNodeOrNull<MeshInstance3D>("Head/Camera3D/PullBeam");
        if (_pullBeam is not null)
        {
            _pullBeam.Visible = false;
        }
        Inventory.Changed += UpdateMechanicalArmVisual;
        Health.Changed += OnHealthChanged;
        UpdateMechanicalArmVisual(Inventory);
        _lastHealth = Health.CurrentHealth;
        CaptureMouseForGameplay();
    }

    public override void _ExitTree()
    {
        GameSettings.Changed -= ApplySettings;
        Inventory.Changed -= UpdateMechanicalArmVisual;
        Health.Changed -= OnHealthChanged;
        ClearResourceLookTarget();
    }


    /// <summary>
    /// Resolved lazily through the scene group rather than at _Ready: the HUD
    /// is a sibling of the player in Main.tscn, so node-ready ordering is not
    /// guaranteed, and the overlay is optional in test scenes that instance the
    /// player on its own.
    /// </summary>
    private CombatFeedbackOverlay? CombatOverlay
    {
        get
        {
            if (_combatOverlay is not null && GodotObject.IsInstanceValid(_combatOverlay))
            {
                return _combatOverlay;
            }

            _combatOverlay = GetTree()?.GetFirstNodeInGroup(CombatFeedbackOverlay.OverlayGroup)
                as CombatFeedbackOverlay;
            return _combatOverlay;
        }
    }

    private void OnHealthChanged(PlayerHealth health)
    {
        if (health.CurrentHealth < _lastHealth)
        {
            _cameraShaker?.AddTrauma(0.4f);
            AudioCue.Play(this, DamageAudioPath);
            ShowDamageDirection();
        }

        if (health.IsDead && _lastHealth > 0)
        {
            // Use the existing controller-to-HUD action feedback channel so the
            // current defeat flow clarifies retry without adding another UI path.
            ShowActionFeedback(DefeatRetryFeedback);
        }

        _lastHealth = health.CurrentHealth;
    }

    private void UpdateMechanicalArmVisual(MvpInventory inventory)
    {
        // The inventory is the single authority for arm ownership; crafting and
        // save restoration both flow through its Changed event. Exactly one of
        // the two first-person arms is visible: bare suit arm before crafting,
        // mechanical arm after.
        if (_mechanicalArmVisual is not null)
        {
            _mechanicalArmVisual.Visible = inventory.IsMechanicalArmBuilt;
        }

        if (_bareArmVisual is not null)
        {
            _bareArmVisual.Visible = !inventory.IsMechanicalArmBuilt;
        }

        // Each module is fitted onto the same arm, so the upgrade is visible
        // in the hand the player looks at, not only in the HUD line.
        if (_clampModule is not null)
        {
            _clampModule.Visible = inventory.HasMagnetClamp;
        }

        if (_coilModule is not null)
        {
            _coilModule.Visible = inventory.HasBreakerCoil;
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("interact"))
        {
            TryInteract();
            return;
        }

        if (@event.IsActionPressed("attack"))
        {
            // Recapture on attack so returning from window focus loss or a menu
            // cannot leave mouse-look clamped by the visible OS cursor.
            CaptureMouseForGameplay();
            if (Inventory.HasBreakerCoil)
            {
                // Mk III: the blow lands on release, charged if held long enough.
                _chargedStrike.Begin();
                _wasFullyCharged = false;
            }
            else
            {
                TryAttack();
            }
            return;
        }

        if (@event.IsActionReleased("attack") && _chargedStrike.IsCharging)
        {
            var charged = _chargedStrike.Release();
            CombatOverlay?.SetChargeFraction(0f);
            if (charged)
            {
                TryChargedStrike();
            }
            else
            {
                TryAttack();
            }
            return;
        }

        if (@event.IsActionPressed("pull"))
        {
            CaptureMouseForGameplay();
            TryPull();
            return;
        }

#if DEBUG
        if (@event is InputEventKey { Pressed: true, Echo: false, Keycode: Key.F3 })
        {
            ToggleDebugResourceHighlights();
            return;
        }
#endif

        if (@event is InputEventMouseMotion mouseMotion)
        {
            // Mouse-look depends on Godot's captured relative motion; forcing the
            // mode here preserves unlimited horizontal turning at screen edges.
            CaptureMouseForGameplay();
            var look = FirstPersonMovement.ApplyMouseLook(
                new Vector2(_bodyYaw, _cameraPitch),
                _invertY ? new Vector2(mouseMotion.Relative.X, -mouseMotion.Relative.Y) : mouseMotion.Relative,
                MouseSensitivity * _sensitivityScale,
                MaxLookAngleDegrees);
            // Accumulated here and consumed in _PhysicsProcess so viewmodel sway
            // reacts to the whole frame's turn, not to one input event of many.
            _lookDeltaThisFrame += new Vector2(look.X - _bodyYaw, look.Y - _cameraPitch);
            _bodyYaw = look.X;
            _cameraPitch = look.Y;
            Rotation = new Vector3(0.0f, _bodyYaw, 0.0f);
            _head.Rotation = new Vector3(_cameraPitch, 0.0f, 0.0f);
        }
    }

    public override void _Notification(int what)
    {
        // Regain captured input after the game window returns to focus so mouse
        // motion stays relative instead of stopping at desktop screen limits.
        if (what == NotificationApplicationFocusIn)
        {
            CaptureMouseForGameplay();
        }
    }

    private static void CaptureMouseForGameplay()
    {
        // Avoid redundant engine calls while still making captured mode the
        // controller's invariant during active gameplay.
        if (Input.MouseMode != Input.MouseModeEnum.Captured)
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
    }

    public bool TryAttack() => TryStrike(charged: false);

    /// <summary>Mk III: the heavy blow a full charge releases.</summary>
    public bool TryChargedStrike() => TryStrike(charged: true);

    private bool TryStrike(bool charged)
    {
        if (!Inventory.IsMechanicalArmBuilt)
        {
            ShowActionFeedback("Attack blocked: Mk I is not built. Press E at the workbench after collecting resources.");
            return false;
        }

        if (charged && !Inventory.HasBreakerCoil)
        {
            return false;
        }

        if (_mechanicalArmAttack.IsOnCooldown || _chargedRecoveryRemaining > 0f)
        {
            ShowActionFeedback("Arm recharging — wait a moment before the next punch.");
            return false;
        }

        PlayArmSwingFeedback(charged);

        var range = charged ? ChargedStrikeRange : AttackRange;
        var query = PhysicsRayQueryParameters3D.Create(
            _camera.GlobalPosition,
            _camera.GlobalPosition - _camera.GlobalTransform.Basis.Z * range);
        query.CollideWithAreas = false;
        query.CollideWithBodies = true;
        query.Exclude = new Godot.Collections.Array<Rid> { GetRid() };

        if (charged)
        {
            // The coil discharges whether or not it connects.
            _chargedRecoveryRemaining = ChargedStrikeLogic.DefaultRecoverySeconds;
            _cameraShaker?.AddTrauma(0.18f);
        }

        var hit = GetWorld3D().DirectSpaceState.IntersectRay(query);
        if (!hit.TryGetValue("collider", out var colliderVariant))
        {
            ShowActionFeedback(charged
                ? "Charged strike missed — get in close to the target."
                : "Attack missed: aim at the Galaxabrain within Mk I range.");
            return false;
        }

        var target = FindAncestor<IArmTarget>(colliderVariant.AsGodotObject());
        if (target is null || !target.IsStrikeable)
        {
            ShowActionFeedback("Attack ready: aim at the living Galaxabrain, then left click.");
            return false;
        }

        if (!charged && !_mechanicalArmAttack.TryAttack(Inventory))
        {
            ShowActionFeedback("Mk I cannot strike yet.");
            return false;
        }

        var damage = charged ? ChargedStrikeDamage : _mechanicalArmAttack.Damage;
        var outcome = target.ReceiveStrike(new ArmStrike(damage, charged, GlobalPosition));
        if (outcome == StrikeOutcome.Ignored)
        {
            ShowActionFeedback("Attack ready: aim at the living Galaxabrain, then left click.");
            return false;
        }

        var hitPoint = hit.TryGetValue("position", out var position) ? position.AsVector3() : GlobalPosition;
        if (outcome == StrikeOutcome.Deflected)
        {
            // A guard or sound stone turns the blow: a hard, dull stop rather
            // than the hit's punch, and the line names what would work.
            _cameraShaker?.AddTrauma(0.12f);
            _cameraShaker?.AddKick(-HitKickPitchRadians * 0.5f, 0.0f);
            AudioCue.Play3D(this, "AudioLayer_Player/Weapon_Deflect", hitPoint);
            ShowActionFeedback(target.DeflectFeedback);
            return true;
        }

        var lethal = outcome == StrikeOutcome.Killed;
        _timeManager?.TriggerDefaultHitStop();
        _cameraShaker?.AddTrauma(charged ? 0.45f : 0.22f);
        // A confirmed hit punches the view up and slightly across, so landing a
        // strike feels different from swinging at air.
        _cameraShaker?.AddKick(HitKickPitchRadians * (charged ? 1.8f : 1f), HitKickYawRadians);
        AudioCue.Play(this, ArmHitAudioPath);
        AudioCue.Play3D(this, charged ? "AudioLayer_Player/Weapon_ChargedImpact" : "AudioLayer_Player/Weapon_Impact", hitPoint);
        CombatOverlay?.ShowHitMarker(lethal);
        AttackLanded?.Invoke();
        if (!lethal)
        {
            ShowActionFeedback(charged ? ChargedStrikeFeedback : GalaxabrainScoutHitFeedback);
        }
        return true;
    }

    /// <summary>
    /// Mk II magnet clamp. The crosshair ray picks a pull target first; failing
    /// that, the nearest target inside a small cone around it with a clear line
    /// of sight, because drones are small and fast and an exact hit would make
    /// the module feel broken rather than skilful.
    /// </summary>
    public bool TryPull()
    {
        if (!Inventory.HasMagnetClamp)
        {
            if (Inventory.IsMechanicalArmBuilt)
            {
                ShowActionFeedback(PullBlockedFeedback);
            }
            return false;
        }

        if (_magnetPull.IsOnCooldown)
        {
            return false;
        }

        var target = FindPullTarget();
        if (target is null)
        {
            // A clamp shot at nothing still fires: it costs the cooldown.
            _magnetPull.TryFire(Inventory);
            AudioCue.Play3D(this, "AudioLayer_Player/Arm_Pull", GlobalPosition);
            ShowPullBeam(_camera.GlobalPosition - _camera.GlobalTransform.Basis.Z * 6f);
            ShowActionFeedback(PullMissFeedback);
            return false;
        }

        _magnetPull.TryFire(Inventory);
        var anchor = target.PullAnchor;
        if (!target.Pull(GlobalPosition))
        {
            return false;
        }

        AudioCue.Play3D(this, "AudioLayer_Player/Arm_Pull", GlobalPosition);
        ShowPullBeam(anchor);
        _cameraShaker?.AddKick(-SwingKickPitchRadians * 1.5f, 0.0f);
        _cameraShaker?.AddTrauma(0.12f);
        PullLanded?.Invoke(target);
        return true;
    }

    private IPullTarget? FindPullTarget()
    {
        var from = _camera.GlobalPosition;
        var aim = -_camera.GlobalTransform.Basis.Z;
        var query = PhysicsRayQueryParameters3D.Create(from, from + aim * PullRange);
        query.CollideWithAreas = false;
        query.CollideWithBodies = true;
        query.Exclude = new Godot.Collections.Array<Rid> { GetRid() };
        var hit = GetWorld3D().DirectSpaceState.IntersectRay(query);
        if (hit.TryGetValue("collider", out var colliderVariant)
            && FindAncestor<IPullTarget>(colliderVariant.AsGodotObject()) is { CanBePulled: true } direct)
        {
            return direct;
        }

        IPullTarget? best = null;
        var bestCos = -1f;
        foreach (var node in GetTree().GetNodesInGroup(PullTargetGroup))
        {
            if (node is not IPullTarget candidate || !candidate.CanBePulled)
                continue;

            var toTarget = candidate.PullAnchor - from;
            if (!MagnetPullLogic.IsWithinAssist(aim, toTarget, PullRange))
                continue;

            var cos = aim.Dot(toTarget.Normalized());
            if (cos <= bestCos || !HasLineOfSight(from, candidate))
                continue;

            best = candidate;
            bestCos = cos;
        }

        return best;
    }

    /// <summary>Scene group every <see cref="IPullTarget"/> joins, for clamp aim assist.</summary>
    public const string PullTargetGroup = "pull_targets";

    private bool HasLineOfSight(Vector3 from, IPullTarget target)
    {
        var query = PhysicsRayQueryParameters3D.Create(from, target.PullAnchor);
        query.CollideWithAreas = false;
        query.CollideWithBodies = true;
        query.Exclude = new Godot.Collections.Array<Rid> { GetRid() };
        var hit = GetWorld3D().DirectSpaceState.IntersectRay(query);
        return !hit.TryGetValue("collider", out var collider)
            || ReferenceEquals(FindAncestor<IPullTarget>(collider.AsGodotObject()), target);
    }

    private void ShowPullBeam(Vector3 worldTarget)
    {
        if (_pullBeam is null)
        {
            return;
        }

        // A short-lived line from the clamp to what it grabbed: the pull reads
        // as an action at a distance, not as the object moving on its own.
        var start = _camera.GlobalPosition + _camera.GlobalTransform.Basis * new Vector3(0.32f, -0.28f, -0.6f);
        var length = start.DistanceTo(worldTarget);
        if (length < 0.1f)
        {
            return;
        }

        _pullBeam.GlobalPosition = (start + worldTarget) * 0.5f;
        _pullBeam.LookAt(worldTarget, Vector3.Up);
        _pullBeam.Scale = new Vector3(1f, 1f, length);
        _pullBeam.Visible = true;
        if (_pullBeamTween is { } previous && previous.IsValid())
        {
            previous.Kill();
        }

        _pullBeamTween = CreateTween();
        _pullBeamTween.TweenInterval(0.18f);
        _pullBeamTween.TweenCallback(Callable.From(() =>
        {
            if (_pullBeam is not null)
            {
                _pullBeam.Visible = false;
            }
        }));
    }

    /// <summary>
    /// Damage from any enemy: remembers where it came from so the damage
    /// indicator points at the right attacker, now that there is more than one kind.
    /// </summary>
    public void TakeHit(int damage, Vector3 sourcePosition)
    {
        _lastDamageOrigin = sourcePosition;
        Health.ApplyDamage(damage);
    }

    private static T? FindAncestor<T>(GodotObject? collider) where T : class
    {
        var node = collider as Node;
        while (node is not null)
        {
            if (node is T match)
            {
                return match;
            }

            node = node.GetParent();
        }

        return null;
    }

    private void PlayArmSwingFeedback(bool charged = false)
    {
        // Viewmodel punch: quick forward jab and spring back, plus the swing
        // whoosh — played for every unblocked swing, hit or miss, so attack
        // input always has visible and audible weight.
        AudioCue.Play3D(this, "AudioLayer_Player/Weapon_Swing", GlobalPosition);
        _cameraShaker?.AddKick(SwingKickPitchRadians, 0.0f);
        if (_mechanicalArmVisual is null)
        {
            return;
        }

        if (_armSwingTween is { } previous && previous.IsValid())
        {
            previous.Kill();
        }

        _mechanicalArmVisual.Rotation = _mechanicalArmBaseRotation;
        var jabRotation = _mechanicalArmBaseRotation + new Vector3(charged ? -0.6f : -0.35f, 0f, 0f);
        _armSwingTween = CreateTween();
        _armSwingTween.TweenProperty(_mechanicalArmVisual, "rotation", jabRotation, 0.07f)
            .SetTrans(Tween.TransitionType.Quad)
            .SetEase(Tween.EaseType.Out);
        _armSwingTween.TweenProperty(_mechanicalArmVisual, "rotation", _mechanicalArmBaseRotation, 0.16f)
            .SetTrans(Tween.TransitionType.Back)
            .SetEase(Tween.EaseType.Out);
    }

    public bool TryInteract()
    {
        var query = PhysicsRayQueryParameters3D.Create(
            _camera.GlobalPosition,
            _camera.GlobalPosition - _camera.GlobalTransform.Basis.Z * InteractionRange);
        query.CollideWithAreas = true;
        query.CollideWithBodies = true;

        var hit = GetWorld3D().DirectSpaceState.IntersectRay(query);
        if (!hit.TryGetValue("collider", out var colliderVariant))
        {
            return false;
        }

        if (!TryFindInteractable(colliderVariant.AsGodotObject(), out var interactable))
        {
            return false;
        }

        var previousMissionStep = Mission.CurrentStep;
        var interacted = interactable.Interact(Inventory, Mission);
        if (interacted && interactable is ResourceDrop)
        {
            ClearResourceLookTarget();
            if (previousMissionStep == CrashSiteMissionStep.CollectResources
                && Mission.CurrentStep == CrashSiteMissionStep.BuildMechanicalArm
                && _mechanicalArmRecipe.CanCraft(Inventory)
                && !Inventory.IsMechanicalArmBuilt)
            {
                ShowActionFeedback(ResourceCompletionFeedback);
                AudioCue.Play(this, "AudioLayer_State/State_Objective");
            }
        }
        else if (interacted && interactable is Workbench)
        {
            // Reuse the existing controller-to-HUD feedback signal for the craft success,
            // keeping the loop guidance in one player-facing action channel.
            var tier = Inventory.ArmLevel;
            ShowActionFeedback(tier switch
            {
                >= 3 => Mk3CraftSuccessFeedback,
                2 => Mk2CraftSuccessFeedback,
                _ => MechanicalArmCraftSuccessFeedback,
            });
            AudioCue.Play(this, "AudioLayer_UI/UI_Craft_Complete");
            ArmCrafted?.Invoke(tier);
        }
        else if (interacted && interactable is PowerCorePickup)
        {
            ShowActionFeedback(PowerCoreRecoveryFeedback);
            AudioCue.Play(this, "AudioLayer_State/State_Objective");
        }
        else if (interacted && interactable is GalaxabrainComponentPickup)
        {
            ShowActionFeedback(GalaxabrainComponentRecoveryFeedback);
            AudioCue.Play(this, "AudioLayer_State/State_Objective");
        }
        else if (interacted && interactable is SavePoint)
        {
            ShowActionFeedback(SavePointSuccessFeedback);
        }
        else if (interacted && interactable is Beacon)
        {
            // Activation starts the charge; the holdout's own feedback takes it from here.
            ShowActionFeedback(BeaconChargingFeedback);
        }

        return interacted;
    }

    private void UpdateInteractionPrompt()
    {
        var prompt = TryGetTargetInteractionPrompt(out var targetPrompt, out var highlightTarget)
            ? targetPrompt
            : string.Empty;

        UpdateLookTarget(highlightTarget);

        if (prompt == _interactionPrompt)
            return;

        _interactionPrompt = prompt;
        InteractionPromptChanged?.Invoke(prompt);
    }

    private bool TryGetTargetInteractionPrompt(out string prompt, out ILookHighlightTarget? highlightTarget)
    {
        prompt = string.Empty;
        highlightTarget = null;
        if (TryGetInteractablePrompt(out prompt, out highlightTarget))
        {
            return true;
        }

        return TryGetAimHint(out prompt);
    }

    /// <summary>
    /// What the arm could do to the thing under the crosshair (pull the wreck,
    /// break the cracked stone), shown in the same prompt line as E-interactions
    /// so a new module is taught where it is needed, not in a menu.
    /// </summary>
    private bool TryGetAimHint(out string prompt)
    {
        prompt = string.Empty;
        if (!Inventory.IsMechanicalArmBuilt)
        {
            return false;
        }

        var query = PhysicsRayQueryParameters3D.Create(
            _camera.GlobalPosition,
            _camera.GlobalPosition - _camera.GlobalTransform.Basis.Z * PullRange);
        query.CollideWithAreas = false;
        query.CollideWithBodies = true;
        query.Exclude = new Godot.Collections.Array<Rid> { GetRid() };
        var hit = GetWorld3D().DirectSpaceState.IntersectRay(query);
        if (!hit.TryGetValue("collider", out var collider)
            || FindAncestor<IAimHint>(collider.AsGodotObject()) is not { } hint)
        {
            return false;
        }

        prompt = hint.GetAimHint(Inventory);
        return !string.IsNullOrEmpty(prompt);
    }

    private bool TryGetInteractablePrompt(out string prompt, out ILookHighlightTarget? highlightTarget)
    {
        prompt = string.Empty;
        highlightTarget = null;

        var query = PhysicsRayQueryParameters3D.Create(
            _camera.GlobalPosition,
            _camera.GlobalPosition - _camera.GlobalTransform.Basis.Z * InteractionRange);
        query.CollideWithAreas = true;
        query.CollideWithBodies = true;

        var hit = GetWorld3D().DirectSpaceState.IntersectRay(query);
        if (!hit.TryGetValue("collider", out var colliderVariant)
            || !TryFindInteractable(colliderVariant.AsGodotObject(), out var interactable)
            || interactable is not Node node)
        {
            return false;
        }

        if (!interactable.IsInteractionAvailable)
        {
            return false;
        }

        highlightTarget = FindLookHighlightTarget(colliderVariant.AsGodotObject());
        prompt = node is Workbench ? BuildWorkbenchPrompt() : interactable.InteractionPrompt;
        return true;
    }

    private void UpdateLookTarget(ILookHighlightTarget? nextTarget)
    {
        if (_currentLookTarget is GodotObject currentObject && !GodotObject.IsInstanceValid(currentObject))
        {
            _currentLookTarget = null;
        }

        if (ReferenceEquals(_currentLookTarget, nextTarget))
        {
            return;
        }

        _currentLookTarget?.SetHighlighted(false);
        _currentLookTarget = nextTarget;
        _currentLookTarget?.SetHighlighted(true);
    }

    private void ClearResourceLookTarget()
    {
        if (_currentLookTarget is GodotObject currentObject && GodotObject.IsInstanceValid(currentObject))
        {
            _currentLookTarget?.SetHighlighted(false);
        }

        _currentLookTarget = null;
    }

    private static bool TryFindInteractable(GodotObject? collider, out ICrashSiteInteractable interactable)
    {
        var node = collider as Node;
        while (node is not null)
        {
            if (node is ICrashSiteInteractable candidate)
            {
                interactable = candidate;
                return true;
            }

            node = node.GetParent();
        }

        interactable = null!;
        return false;
    }

    private static ILookHighlightTarget? FindLookHighlightTarget(GodotObject? collider)
    {
        var node = collider as Node;
        while (node is not null)
        {
            if (node is ILookHighlightTarget target)
            {
                return target;
            }

            node = node.GetParent();
        }

        return null;
    }

#if DEBUG
    private void ToggleDebugResourceHighlights()
    {
        _debugHighlightAllResourceDrops = !_debugHighlightAllResourceDrops;
        var highlightedCount = 0;

        foreach (var node in GetTree().GetNodesInGroup(ResourceDrop.ResourceDropGroup))
        {
            if (node is ResourceDrop resourceDrop && GodotObject.IsInstanceValid(resourceDrop))
            {
                resourceDrop.SetHighlighted(_debugHighlightAllResourceDrops);
                highlightedCount++;
            }
        }

        GD.Print($"[ResourceDropDebug] F3 highlight={_debugHighlightAllResourceDrops} affected={highlightedCount}");
    }
#endif

    private string BuildWorkbenchPrompt()
    {
        var next = MechanicalArmRecipe.NextFor(Inventory);
        if (next is null)
            return "Workbench: the arm is complete — Mk III online.";

        var cost = $"{next.MetalCost} Metal";
        if (next.BiomassCost > 0)
            cost += $", {next.BiomassCost} Biomass";
        if (next.ElectronicComponentsCost > 0)
            cost += $", {next.ElectronicComponentsCost} Electronics";
        if (next.RequiresGalaxabrainComponent)
            cost += ", the Scout's component";

        if (next.Tier == 1)
        {
            var mk1Cost = $"needs {_mechanicalArmRecipe.MetalCost} Metal, {_mechanicalArmRecipe.BiomassCost} Biomass, {_mechanicalArmRecipe.ElectronicComponentsCost} Electronics";
            return _mechanicalArmRecipe.CanCraft(Inventory) && Mission.CurrentStep == CrashSiteMissionStep.BuildMechanicalArm
                ? $"Press E to craft Mechanical Arm Mk I ({mk1Cost})."
                : $"Workbench: Mechanical Arm Mk I {mk1Cost}. Collect resources first.";
        }

        if (Mission.CurrentStep < Workbench.StepForTier(next.Tier))
            return Inventory.ArmLevel >= 1
                ? $"Workbench: {RomanArm()} online. Left click attacks."
                : $"Workbench: {next.DisplayName} needs {cost}.";

        return next.CanCraft(Inventory)
            ? $"Press E to fit {next.DisplayName} ({cost})."
            : $"Workbench: {next.DisplayName} needs {cost}.";
    }

    private string RomanArm() => $"Mk {MechanicalArmRecipe.RomanTier(Inventory.ArmLevel)}";

    public const string WardenDefeatFeedback = "Warden down — take the beacon's power core from its nest.";
    public const string HoldoutLeftFeedback = "The beacon stops charging out here — get back to the plaza!";
    public const string HoldoutReturnedFeedback = "Beacon charging — hold the plaza.";

    public void ShowWardenDefeatFeedback()
    {
        ShowActionFeedback(WardenDefeatFeedback);
        AudioCue.Play(this, "AudioLayer_State/State_Objective");
    }

    public void ShowHoldoutFeedback(bool holding) =>
        ShowActionFeedback(holding ? HoldoutReturnedFeedback : HoldoutLeftFeedback);

    public void ShowGalaxabrainScoutDefeatFeedback()
    {
        // Scout defeat remains separate from component pickup and victory; this
        // only clarifies the existing recovery objective through the HUD channel.
        ShowActionFeedback(GalaxabrainScoutDefeatFeedback);
        AudioCue.Play(this, "AudioLayer_State/State_Objective");
    }

    private void ShowActionFeedback(string message)
    {
        // This explicit HUD feedback proves whether craft/attack input was accepted, blocked, or missed.
        ActionFeedbackChanged?.Invoke(message);
    }

    public override void _PhysicsProcess(double delta)
    {
        // The crash-site slab has no perimeter containment; leaving it must end in
        // the standard death flow (defeat screen -> reload last save), never an endless fall.
        if (GlobalPosition.Y < FallDefeatHeight && !Health.IsDead)
        {
            Health.ApplyDamage(Health.CurrentHealth);
            return;
        }

        _mechanicalArmAttack.Tick((float)delta);
        _magnetPull.Tick((float)delta);
        _chargedRecoveryRemaining = Mathf.Max(0f, _chargedRecoveryRemaining - (float)delta);
        UpdateCharge((float)delta);
        UpdateWeaponReadyAudio();
        UpdateInteractionPrompt();

        var velocity = Velocity;

        if (!IsOnFloor())
        {
            velocity.Y -= _gravity * (float)delta;
        }

        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
            Jumped?.Invoke();
        }

        var inputDirection = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        var moveDirection = FirstPersonMovement.GetMoveDirection(Transform.Basis, inputDirection);
        var isSprinting = Input.IsActionPressed("sprint") && inputDirection.LengthSquared() > 0.01f;
        var targetFov = isSprinting ? SprintFov : BaseFov;
        _camera.Fov = Mathf.Lerp(_camera.Fov, targetFov, 1.0f - Mathf.Exp(-FovLerpSpeed * (float)delta));

        var speed = WalkSpeed * (isSprinting ? SprintSpeedMultiplier : 1.0f)
            * (_chargedStrike.IsCharging ? ChargingSpeedMultiplier : 1.0f);
        velocity.X = moveDirection.X * speed;
        velocity.Z = moveDirection.Z * speed;
        Velocity = velocity;

        // Captured before MoveAndSlide, because landing zeroes the fall speed
        // the impact strength has to be measured from.
        var fallSpeedBeforeMove = _previousFallSpeed;
        MoveAndSlide();
        UpdateLandingFeel(fallSpeedBeforeMove);
        UpdateCameraFeel(inputDirection, isSprinting, (float)delta);
        UpdateViewmodelSway(inputDirection, (float)delta);
        _previousFallSpeed = Velocity.Y < 0.0f ? -Velocity.Y : 0.0f;

        UpdateFootstepAudio(inputDirection, isSprinting, (float)delta);
    }

    private void UpdateLandingFeel(float fallSpeedBeforeMove)
    {
        var onFloor = IsOnFloor();
        if (onFloor && !_wasOnFloor && _cameraShaker?.ReportLanding(fallSpeedBeforeMove) == true)
        {
            // Reuse the existing footstep bank for the touchdown thud rather
            // than adding an audio asset for a single cue.
            AudioCue.Play3D(this, FootstepAudioPaths[0], GlobalPosition);
        }

        _wasOnFloor = onFloor;
    }

    private void UpdateCameraFeel(Vector2 inputDirection, bool isSprinting, float deltaSeconds)
    {
        if (_cameraShaker is null)
        {
            return;
        }

        var grounded = IsOnFloor();
        var horizontalSpeed = new Vector2(Velocity.X, Velocity.Z).Length();
        var travelled = grounded ? horizontalSpeed * deltaSeconds : 0.0f;
        if (travelled > 0.0f)
        {
            GroundTravelled?.Invoke(travelled);
        }

        // Sprinting bobs harder; airborne does not bob at all.
        var intensity = grounded && horizontalSpeed > 0.1f
            ? Mathf.Clamp(horizontalSpeed / WalkSpeed, 0.0f, 1.0f) * (isSprinting ? 1.25f : 1.0f)
            : 0.0f;
        _cameraShaker.UpdateMovementFeel(travelled, intensity, grounded ? inputDirection.X : 0.0f);
    }

    private void UpdateViewmodelSway(Vector2 inputDirection, float deltaSeconds)
    {
        _viewmodelSway.Update(_lookDeltaThisFrame, inputDirection.X, inputDirection.Y, deltaSeconds);
        LookTravelled?.Invoke(_lookDeltaThisFrame.Length());
        _lookDeltaThisFrame = Vector2.Zero;

        // The swing tween owns the mechanical arm's rotation while it plays, so
        // sway is applied to position only there and to both on the bare arm.
        if (_mechanicalArmVisual is not null)
        {
            _mechanicalArmVisual.Position = _mechanicalArmBasePosition + _viewmodelSway.Offset;
        }

        if (_bareArmVisual is not null)
        {
            _bareArmVisual.Position = _bareArmBasePosition + _viewmodelSway.Offset;
            _bareArmVisual.Rotation = _bareArmBaseRotation + _viewmodelSway.TiltRadians;
        }
    }

    private void ShowDamageDirection()
    {
        var overlay = CombatOverlay;
        if (overlay is null)
        {
            return;
        }

        if (_lastDamageOrigin is { } origin)
        {
            overlay.ShowDamageFrom(GlobalPosition, _bodyYaw, origin);
            _lastDamageOrigin = null;
            return;
        }

        // Damage applied without a known source (tests, legacy callers): the
        // nearest living Scout is the best guess.
        foreach (var node in GetTree().GetNodesInGroup(GalaxabrainScout.GalaxabrainScoutGroup))
        {
            if (node is GalaxabrainScout scout && GodotObject.IsInstanceValid(scout) && !scout.Brain.IsDead)
            {
                overlay.ShowDamageFrom(GlobalPosition, _bodyYaw, scout.GlobalPosition);
                return;
            }
        }
    }

    private void UpdateCharge(float deltaSeconds)
    {
        if (!_chargedStrike.IsCharging)
        {
            if (_coilGlow is not null && _coilGlow.Scale.X > 1.001f)
            {
                _coilGlow.Scale = Vector3.One;
            }
            return;
        }

        // Releasing the button outside gameplay input (a menu opened) must
        // not leave a charge armed forever.
        if (!Input.IsActionPressed("attack"))
        {
            _chargedStrike.Cancel();
            CombatOverlay?.SetChargeFraction(0f);
            return;
        }

        _chargedStrike.Tick(deltaSeconds);
        var fraction = _chargedStrike.ChargeFraction;
        CombatOverlay?.SetChargeFraction(fraction);
        if (_coilGlow is not null)
        {
            _coilGlow.Scale = Vector3.One * (1f + 0.35f * fraction);
        }

        if (_mechanicalArmVisual is not null)
        {
            _mechanicalArmVisual.Rotation = _mechanicalArmBaseRotation + new Vector3(0.25f * fraction, 0f, 0f);
        }

        if (_chargedStrike.IsFullyCharged && !_wasFullyCharged)
        {
            _wasFullyCharged = true;
            AudioCue.Play3D(this, "AudioLayer_Player/Arm_ChargeReady", GlobalPosition);
            _cameraShaker?.AddTrauma(0.06f);
        }
    }

    /// <summary>Test seam: hold a Mk III charge for <paramref name="seconds"/> without input, then release.</summary>
    public bool ChargeAndReleaseForTest(float seconds)
    {
        if (!Inventory.HasBreakerCoil)
            return false;

        _chargedStrike.Begin();
        _chargedStrike.Tick(seconds);
        var charged = _chargedStrike.Release();
        return charged ? TryChargedStrike() : TryAttack();
    }

    public bool IsChargingStrike => _chargedStrike.IsCharging;

    private void UpdateFootstepAudio(Vector2 inputDirection, bool isSprinting, float deltaSeconds)
    {
        bool isWalking = IsOnFloor() && inputDirection.LengthSquared() > 0.01f;
        if (!isWalking)
        {
            _footstepTimer = 0f;
            return;
        }

        _footstepTimer -= deltaSeconds;
        if (_footstepTimer > 0f)
        {
            return;
        }

        AudioCue.Play3D(this, FootstepAudioPaths[_footstepAudioIndex], GlobalPosition);
        _footstepAudioIndex = (_footstepAudioIndex + 1) % FootstepAudioPaths.Length;
        _footstepTimer = isSprinting ? SprintFootstepIntervalSeconds : WalkFootstepIntervalSeconds;
    }

    private void UpdateWeaponReadyAudio()
    {
        bool isOnCooldown = _mechanicalArmAttack.IsOnCooldown;
        if (_wasAttackOnCooldown && !isOnCooldown && Inventory.IsMechanicalArmBuilt)
        {
            AudioCue.Play3D(this, "AudioLayer_Player/Weapon_Ready", GlobalPosition);
        }

        _wasAttackOnCooldown = isOnCooldown;
    }
}
