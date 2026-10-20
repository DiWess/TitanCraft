using GdUnit4;
using TitanCraft.Core;
using static GdUnit4.Assertions;

namespace TitanCraft.Tests.Unit;

[TestSuite]
public sealed class GameSettingsTests
{
    [TestCase]
    public void DefaultsMatchTheAuthoredGame()
    {
        var settings = new GameSettingsData();

        AssertThat(settings.MouseSensitivityScale).IsEqual(1f);
        AssertThat(settings.FieldOfView).IsEqual(75f);
        AssertThat(settings.MasterVolume).IsEqual(1f);
        AssertThat(settings.InvertY).IsFalse();
        AssertThat(settings.VSync).IsTrue();
        AssertThat(settings.Bindings.Count).IsEqual(0);
    }

    [TestCase]
    public void ClampKeepsEveryValueInItsRange()
    {
        var settings = new GameSettingsData
        {
            MouseSensitivityScale = 40f,
            FieldOfView = float.NaN,
            MasterVolume = -3f,
        };
        settings.Bindings["jump"] = "key:32";
        settings.Bindings["attack"] = "joystick:9";
        settings.Bindings["pull"] = "mouse:0";

        settings.Clamp();

        AssertThat(settings.MouseSensitivityScale).IsEqual(GameSettingsData.MaxSensitivityScale);
        AssertThat(settings.FieldOfView).IsEqual(GameSettingsData.DefaultFieldOfView);
        AssertThat(settings.MasterVolume).IsEqual(0f);
        AssertThat(settings.Bindings.ContainsKey("jump")).IsTrue();
        AssertThat(settings.Bindings.ContainsKey("attack")).IsFalse();
        AssertThat(settings.Bindings.ContainsKey("pull")).IsFalse();
    }

    [TestCase]
    public void ACopyIsIndependentOfTheOriginal()
    {
        var settings = new GameSettingsData { FieldOfView = 90f };
        settings.Bindings["jump"] = "key:32";

        var copy = settings.Copy();
        copy.FieldOfView = 70f;
        copy.Bindings["jump"] = "key:70";

        AssertThat(settings.FieldOfView).IsEqual(90f);
        AssertThat(settings.Bindings["jump"]).IsEqual("key:32");
    }

    [TestCase]
    public void VolumeMapsToDecibelsWithSilenceAtZero()
    {
        AssertThat(GameSettingsData.VolumeToDb(1f)).IsEqual(0f);
        AssertThat(GameSettingsData.VolumeToDb(0.5f)).IsBetween(-6.1f, -5.9f);
        AssertThat(GameSettingsData.VolumeToDb(0f)).IsEqual(-80f);
    }

    [TestCase]
    public void BindingStringsAreValidatedBeforeUse()
    {
        // Converting to engine InputEvents needs the running engine, so that
        // round trip is covered by the integration suite (TestSettingsApply).
        AssertThat(GameSettingsData.IsValidBinding("key:87")).IsTrue();
        AssertThat(GameSettingsData.IsValidBinding("mouse:2")).IsTrue();
        AssertThat(GameSettingsData.IsValidBinding("key:")).IsFalse();
        AssertThat(GameSettingsData.IsValidBinding("key:-4")).IsFalse();
        AssertThat(GameSettingsData.IsValidBinding(null)).IsFalse();
    }
}
