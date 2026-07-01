using System;
using System.Text.Json;
using TitanCraft.Missions;
using Godot;

namespace TitanCraft.SaveSystem;

public static class LocalSaveGameStore
{
    public const string DefaultSavePath = "user://crash_site_save.json";

    public static bool SaveExists(string savePath = DefaultSavePath) => FileAccess.FileExists(savePath);

    public static void DeleteSave(string savePath = DefaultSavePath)
    {
        if (!FileAccess.FileExists(savePath))
            return;

        var error = DirAccess.RemoveAbsolute(ProjectSettings.GlobalizePath(savePath));
        if (error != Error.Ok)
            GD.PushWarning($"Could not delete local save '{savePath}': {error}");
    }

    public static bool TryLoad(out CrashSiteSaveData saveData, string savePath = DefaultSavePath)
    {
        saveData = new CrashSiteSaveData();
        if (!FileAccess.FileExists(savePath))
            return false;

        using var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Read);
        if (file is null)
            return false;

        try
        {
            var loaded = JsonSerializer.Deserialize<CrashSiteSaveData>(file.GetAsText());
            if (!IsValid(loaded))
            {
                GD.PushWarning($"Ignoring invalid Crash Site save data at '{savePath}'. Starting a new run.");
                return false;
            }

            saveData = loaded!;
            return true;
        }
        catch (JsonException)
        {
            GD.PushWarning($"Ignoring unreadable Crash Site save data at '{savePath}'. Starting a new run.");
            return false;
        }
        catch (NotSupportedException)
        {
            GD.PushWarning($"Ignoring unsupported Crash Site save data at '{savePath}'. Starting a new run.");
            return false;
        }
    }

    public static void Save(CrashSiteSaveData saveData, string savePath = DefaultSavePath)
    {
        using var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Write)
            ?? throw new InvalidOperationException($"Could not open local save for writing: {savePath}");
        file.StoreString(JsonSerializer.Serialize(saveData));
    }

    private static bool IsValid(CrashSiteSaveData? saveData)
    {
        return saveData is not null
            && saveData.SaveVersion == CrashSiteSaveData.CurrentSaveVersion
            && saveData.Health > 0
            && saveData.Metal >= 0
            && saveData.Biomass >= 0
            && saveData.ElectronicComponents >= 0
            && Enum.IsDefined(typeof(CrashSiteMissionStep), saveData.MissionStep)
            && HasConsistentMissionState(saveData);
    }

    /// <summary>
    /// Rejects saves where the mission step and the flags it depends on disagree (e.g. a
    /// hand-edited or corrupted file claiming component recovery without the arm ever being
    /// built), which would otherwise leave the player in an unwinnable state after reload.
    /// </summary>
    private static bool HasConsistentMissionState(CrashSiteSaveData saveData)
    {
        if (saveData.MissionStep >= CrashSiteMissionStep.DefeatGalaxabrain && !saveData.MechanicalArmBuilt)
            return false;

        if (saveData.MissionStep >= CrashSiteMissionStep.ActivateBeacon && !saveData.GalaxabrainComponentCollected)
            return false;

        return true;
    }
}
