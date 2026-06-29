using System;
using System.Text.Json;
using Godot;
using TitanCraft.Missions;

namespace TitanCraft.SaveSystem;

public sealed class CrashSiteSaveService
{
    public const string DefaultSavePath = "user://crash_site_save.json";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
    };

    private readonly string savePath;

    public CrashSiteSaveService(string savePath = DefaultSavePath)
    {
        this.savePath = savePath;
    }

    public bool HasSave()
    {
        return FileAccess.FileExists(savePath);
    }

    public CrashSiteSaveData LoadOrNewGame()
    {
        if (!HasSave())
        {
            return CrashSiteSaveData.NewGame();
        }

        using var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Read);
        if (file is null || FileAccess.GetOpenError() != Error.Ok)
        {
            return CrashSiteSaveData.NewGame();
        }

        return DeserializeOrNewGame(file.GetAsText());
    }

    public void Save(CrashSiteSaveData saveData)
    {
        var sanitizedSaveData = IsValid(saveData) ? saveData : CrashSiteSaveData.NewGame();
        using var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Write);
        file?.StoreString(Serialize(sanitizedSaveData));
    }

    public CrashSiteSaveData ResetNewGame()
    {
        var saveData = CrashSiteSaveData.NewGame();
        Save(saveData);
        return saveData;
    }

    public static string Serialize(CrashSiteSaveData saveData)
    {
        return JsonSerializer.Serialize(saveData, JsonOptions);
    }

    public static CrashSiteSaveData DeserializeOrNewGame(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return CrashSiteSaveData.NewGame();
        }

        try
        {
            var saveData = JsonSerializer.Deserialize<CrashSiteSaveData>(json, JsonOptions);
            return IsValid(saveData) ? saveData! : CrashSiteSaveData.NewGame();
        }
        catch (JsonException)
        {
            return CrashSiteSaveData.NewGame();
        }
        catch (NotSupportedException)
        {
            return CrashSiteSaveData.NewGame();
        }
    }

    private static bool IsValid(CrashSiteSaveData? saveData)
    {
        return saveData is not null
            && saveData.SaveVersion == CrashSiteSaveData.CurrentSaveVersion
            && saveData.Metal >= 0
            && saveData.Biomass >= 0
            && saveData.ElectronicComponents >= 0
            && Enum.IsDefined(saveData.MissionStep);
    }
}
