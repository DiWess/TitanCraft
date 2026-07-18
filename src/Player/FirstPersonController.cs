using System;
using Godot;
using TitanCraft.Core;
using TitanCraft.Crafting;
using TitanCraft.Enemies;
using TitanCraft.Missions;
using TitanCraft.Resources;
using TitanCraft.World;

namespace TitanCraft.Player;

public partial class FirstPersonController : CharacterBody3D
{
    public event Action<string>? InteractionPromptChanged;
    public const string GalaxabrainScoutHitFeedback = "Mechanical Arm strike landed.";
    public const string GalaxabrainScoutDefeatFeedback = "Galaxabrain Scout disabled — recover the component.";
    public const string GalaxabrainComponentRecoveryFeedback = "Galaxabrain component recovered — activate the beacon beam.";
    public const string SavePointSuccessFeedback = "Checkpoint saved — continue to the beacon.";
    public const string BeaconActivationFeedback = "Beacon activated — rescue signal online.";
    public const string DefeatRetryFeedback = "Suit integrity failed — reload from checkpoint.";
    public const string ResourceCompletionFeedback = "Resources secured — craft the Mechanical Arm Mk I at the workbench.";
    public const string MechanicalArmCraftSuccessFeedback = "Mechanical Arm Mk I online — defeat the Galaxabrain Scout.";

    public event Action<string>? ActionFeedbackChanged;
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
    [Export] public float SprintFootstepIntervalSeconds { get; set; } = 0.32f;

    public MvpInventory Inventory { get; } = new();

    public CrashSiteMissionState Mission { get; } = new();

    public PlayerHealth Health { get; } = new();

    private Camera3D _camera = null!;
    private Node3D _head = null!;
    private MeshInstance3D? _mechanicalArmVisual;
    private MeshInstance3D? _bareArmVisual;
    private float _gravity;
    private float _cameraPitch;
    private float _bodyYaw;
    private MechanicalArmAttackLogic _mechanicalArmAttack = null!;
    private MechanicalArmRecipe _mechanicalArmRecipe = null!;
    private CameraShaker? _cameraShaker;
    private TimeManager? _timeManager;
    private int _lastHealth;
    private string _interactionPrompt = string.Empty;
    private ILookHighlightTarget? _currentLookTarget;
    private bool _debugHighlightAllResourceDrops;
    private float _footstepTimer;
    private int _footstepAudioIndex;
    private bool _wasAttackOnCooldown;

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
        _camera.Fov = BaseFov;
        _cameraShaker = _camera as CameraShaker;
        _timeManager = GetNodeOrNull<TimeManager>(TimeManagerPath);
        _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
        _mechanicalArmAttack = new MechanicalArmAttackLogic(MechanicalArmDamage, AttackCooldownSeconds);
        _mechanicalArmRecipe = new MechanicalArmRecipe();
        _mechanicalArmVisual = GetNodeOrNull<MeshInstance3D>("Head/Camera3D/MechanicalArmVisual");
        _bareArmVisual = GetNodeOrNull<MeshInstance3D>("Head/Camera3D/BareArmVisual");
        Inventory.Changed += UpdateMechanicalArmVisual;
        Health.Changed += OnHealthChanged;
        UpdateMechanicalArmVisual(Inventory);
        _lastHealth = Health.CurrentHealth;
        CaptureMouseForGameplay();
    }

    public override void _ExitTree()
    {
        Inventory.Changed -= UpdateMechanicalArmVisual;
        Health.Changed -= OnHealthChanged;
        ClearResourceLookTarget();
    }


    private void OnHealthChanged(PlayerHealth health)
    {
        if (health.CurrentHealth < _lastHealth)
        {
            _cameraShaker?.AddTrauma(0.4f);
            AudioCue.Play(this, DamageAudioPath);
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
            TryAttack();
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
                mouseMotion.Relative,
                MouseSensitivity,
                MaxLookAngleDegrees);
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

    public bool TryAttack()
    {
        if (!Inventory.IsMechanicalArmBuilt)
        {
            ShowActionFeedback("Attack blocked: Mk I is not built. Press E at the workbench after collecting resources.");
            return false;
        }

        if (_mechanicalArmAttack.IsOnCooldown)
        {
            ShowActionFeedback("Mk I recharging — wait a moment before the next punch.");
            return false;
        }

        var query = PhysicsRayQueryParameters3D.Create(
            _camera.GlobalPosition,
            _camera.GlobalPosition - _camera.GlobalTransform.Basis.Z * AttackRange);
        query.CollideWithAreas = false;
        query.CollideWithBodies = true;

        var hit = GetWorld3D().DirectSpaceState.IntersectRay(query);
        if (!hit.TryGetValue("collider", out var colliderVariant))
        {
            ShowActionFeedback("Attack missed: aim at the Galaxabrain within Mk I range.");
            return false;
        }

        if (colliderVariant.AsGodotObject() is not GalaxabrainScout scout || scout.Brain.IsDead)
        {
            ShowActionFeedback("Attack ready: aim at the living Galaxabrain, then left click.");
            return false;
        }

        if (!_mechanicalArmAttack.TryAttack(Inventory))
        {
            ShowActionFeedback("Mk I cannot strike yet.");
            return false;
        }

        scout.ApplyDamage(_mechanicalArmAttack.Damage);
        _timeManager?.TriggerDefaultHitStop();
        _cameraShaker?.AddTrauma(0.22f);
        AudioCue.Play(this, ArmHitAudioPath);
        if (!scout.Brain.IsDead)
        {
            ShowActionFeedback(GalaxabrainScoutHitFeedback);
        }
        return true;
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
            // keeping the MVP loop guidance in one player-facing action channel.
            ShowActionFeedback(MechanicalArmCraftSuccessFeedback);
            AudioCue.Play(this, "AudioLayer_UI/UI_Craft_Complete");
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
            // Beacon success uses the existing controller-to-HUD action feedback
            // pathway so victory confirmation appears without a new UI system.
            ShowActionFeedback(BeaconActivationFeedback);
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

        highlightTarget = FindLookHighlightTarget(colliderVariant.AsGodotObject());
        prompt = node is Workbench ? BuildWorkbenchPrompt() : $"Press E to interact with {node.Name.ToString().Replace("Placeholder_", string.Empty)}";
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
        if (Inventory.IsMechanicalArmBuilt)
            return "Workbench: Mk I already built. Left click attacks the Galaxabrain.";

        var cost = $"needs {_mechanicalArmRecipe.MetalCost} Metal, {_mechanicalArmRecipe.BiomassCost} Biomass, {_mechanicalArmRecipe.ElectronicComponentsCost} Electronics";
        return _mechanicalArmRecipe.CanCraft(Inventory) && Mission.CurrentStep == CrashSiteMissionStep.BuildMechanicalArm
            ? $"Press E to craft Mechanical Arm Mk I ({cost})."
            : $"Workbench: Mechanical Arm Mk I {cost}. Collect resources first.";
    }

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
        }

        var inputDirection = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        var moveDirection = FirstPersonMovement.GetMoveDirection(Transform.Basis, inputDirection);
        var isSprinting = Input.IsActionPressed("sprint") && inputDirection.LengthSquared() > 0.01f;
        var targetFov = isSprinting ? SprintFov : BaseFov;
        _camera.Fov = Mathf.Lerp(_camera.Fov, targetFov, 1.0f - Mathf.Exp(-FovLerpSpeed * (float)delta));

        var speed = WalkSpeed * (isSprinting ? SprintSpeedMultiplier : 1.0f);
        velocity.X = moveDirection.X * speed;
        velocity.Z = moveDirection.Z * speed;
        Velocity = velocity;
        MoveAndSlide();

        UpdateFootstepAudio(inputDirection, isSprinting, (float)delta);
    }

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
