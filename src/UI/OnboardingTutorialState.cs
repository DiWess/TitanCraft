using Godot;

namespace TitanCraft.UI;

/// <summary>The onboarding steps, in the order the player meets them.</summary>
public enum OnboardingStep
{
    Look,
    Move,
    Jump,
    Collect,
    Craft,
    Attack,
    Complete,
}

/// <summary>
/// Action-driven onboarding.
///
/// README section 7 is explicit that the MVP must not depend on a long text
/// tutorial and should lean on short objectives and contextual messages. So
/// this shows exactly one short line at a time, advances only when the player
/// actually performs the action, and retires itself for good once the loop is
/// understood. Nothing here blocks input or pauses the game.
///
/// The existing controls-reference line on the HUD is unchanged and still
/// hides on mission progression; this sits alongside it as the thing that
/// teaches, while that remains the thing that reminds.
/// </summary>
public sealed class OnboardingTutorialState
{
    /// <summary>Look travel, in radians, that counts as "the player looked around".</summary>
    public const float LookThresholdRadians = 2.2f;

    /// <summary>Ground distance, in metres, that counts as "the player moved".</summary>
    public const float MoveThresholdMeters = 4.0f;

    /// <summary>
    /// A step stays up at least this long once shown. Without it a player who
    /// is already moving the mouse would see three prompts flash past in one
    /// second and read none of them.
    /// </summary>
    public const float MinimumStepSeconds = 1.2f;

    private float _lookAccumulated;
    private float _moveAccumulated;
    private float _timeOnCurrentStep;

    public OnboardingStep CurrentStep { get; private set; } = OnboardingStep.Look;

    public bool IsComplete => CurrentStep == OnboardingStep.Complete;

    /// <summary>The single line to show, or empty once onboarding is finished.</summary>
    public string CurrentPrompt => CurrentStep switch
    {
        OnboardingStep.Look => "Move the mouse to look around the quarter.",
        OnboardingStep.Move => "Use WASD or ZQSD to walk.",
        OnboardingStep.Jump => "Press Space to jump.",
        OnboardingStep.Collect => "Find Metal, Biomass and Electronics in the streets — press E to take them.",
        OnboardingStep.Craft => "Return to the workbench and press E to build the Mechanical Arm Mk I.",
        OnboardingStep.Attack => "Left click to strike the Galaxabrain with the Mk I.",
        _ => string.Empty,
    };

    public void Tick(float deltaSeconds)
    {
        if (IsComplete || deltaSeconds <= 0.0f)
        {
            return;
        }

        _timeOnCurrentStep += deltaSeconds;
        TryAdvance();
    }

    public void ReportLook(float deltaRadians)
    {
        if (CurrentStep == OnboardingStep.Look)
        {
            _lookAccumulated += Mathf.Abs(deltaRadians);
            TryAdvance();
        }
    }

    public void ReportGroundTravel(float distanceMeters)
    {
        if (CurrentStep == OnboardingStep.Move)
        {
            _moveAccumulated += Mathf.Max(0.0f, distanceMeters);
            TryAdvance();
        }
    }

    public void ReportJump()
    {
        if (CurrentStep == OnboardingStep.Jump)
        {
            Advance();
        }
    }

    public void ReportResourceCollected()
    {
        // Collecting proves both looking and walking, so a player who ignores
        // the first prompts and simply plays is never left behind them.
        if (CurrentStep <= OnboardingStep.Collect)
        {
            SetStep(OnboardingStep.Craft);
        }
    }

    public void ReportMechanicalArmBuilt()
    {
        if (CurrentStep <= OnboardingStep.Craft)
        {
            SetStep(OnboardingStep.Attack);
        }
    }

    public void ReportAttackLanded()
    {
        if (CurrentStep <= OnboardingStep.Attack)
        {
            SetStep(OnboardingStep.Complete);
        }
    }

    /// <summary>Retires onboarding outright — used when a save is loaded mid-run.</summary>
    public void Skip() => SetStep(OnboardingStep.Complete);

    private void TryAdvance()
    {
        if (_timeOnCurrentStep < MinimumStepSeconds)
        {
            return;
        }

        var satisfied = CurrentStep switch
        {
            OnboardingStep.Look => _lookAccumulated >= LookThresholdRadians,
            OnboardingStep.Move => _moveAccumulated >= MoveThresholdMeters,
            _ => false,
        };

        if (satisfied)
        {
            Advance();
        }
    }

    private void Advance() => SetStep(CurrentStep + 1);

    private void SetStep(OnboardingStep step)
    {
        if (step <= CurrentStep)
        {
            return;
        }

        CurrentStep = step;
        _timeOnCurrentStep = 0.0f;
    }
}
