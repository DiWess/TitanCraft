using System.Text.Json;
using Godot;

namespace TitanCraft.SaveSystem;

public static class LocalSaveGameStore
{
    public const string DefaultSavePath = "user://crash_site_save.json";

    public static bool SaveExists(string savePath = DefaultSavePath) => FileAccess.FileExists(savePath);

    public static void DeleteSave(string savePath = DefaultSavePath)
    {
        if (FileAccess.FileExists(savePath))
            DirAccess.RemoveAbsolute(ProjectSettings.GlobalizePath(savePath));
    }

    public static bool TryLoad(out CrashSiteSaveData saveData, string savePath = DefaultSavePath)
    {
        saveData = new CrashSiteSaveData();
        if (!FileAccess.FileExists(savePath))
            return false;

        using var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Read);
        if (file is null)
            return false;

        var loaded = JsonSerializer.Deserialize<CrashSiteSaveData>(file.GetAsText());
        if (loaded is null || loaded.SaveVersion != 1)
            return false;

        saveData = loaded;
        return true;
    }

    public static void Save(CrashSiteSaveData saveData, string savePath = DefaultSavePath)
    {
        using var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Write);
        file.StoreString(JsonSerializer.Serialize(saveData));
    }
}
