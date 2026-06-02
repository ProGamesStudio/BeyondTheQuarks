using UnityEngine;
using System.IO;

public class SaveSystem
{
    private static string path => Application.persistentDataPath + "/save.json";

    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(path, json);
    }

    public static SaveData LoadGame()
    {
        if (!SaveExists())
        {
            return null;
        }

        string json = File.ReadAllText(path);

        return JsonUtility.FromJson<SaveData>(json);
    }

    public static void DeleteSave()
    {
        if (SaveExists())
        {
            File.Delete(path);
        }
    }

    public static bool SaveExists()
    {
        return File.Exists(path);
    }
}
