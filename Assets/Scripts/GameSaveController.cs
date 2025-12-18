using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.AddressableAssets;

public class SaveSettings : ScriptableObject
{
    public string folderName, fileName;

    public bool isReadable = true;

    public string GetFullPath()
    {
        var folder = Path.Combine(Application.persistentDataPath, folderName);

        return Path.Combine(folder, fileName + ".json");
    }
}

public static class GameSaveController
{
    private const string SETTINGS_ADDRESS = "Save Settings";
    public static SaveSettings _settings;
    private static bool _isLoading;

    public static void Save(LevelData levelData)
    {
        if(_settings == null)
        {
            Debug.LogError("SaveSystem not initialized. Call SaveSystem.Initialize() once before saving.");
            return;
        }

        var fullPath = _settings.GetFullPath();

        SaveData allData = LoadOrCreate();

        if (allData != null)
        {
            bool newLevel = true;
            int levelIndex = -1;

            for (int i = 0; i < allData.levelSave.Count; i++)
            {
                if (allData.levelSave[i].level == levelData.level)
                {
                    newLevel = false;
                    levelIndex = i;
                    break;
                }
            }

            if (!newLevel)
                allData.levelSave[levelIndex] = levelData;
            else
                allData.levelSave.Add(levelData);
        }

        string jsonFile = JsonUtility.ToJson(allData, _settings.isReadable);
        File.WriteAllText(fullPath, jsonFile);
    }

    public static async void Initialize()
    {
        if (_settings != null || _isLoading)
            return;

        _isLoading = true;

        await Addressables.InitializeAsync().Task;
        _settings = await Addressables.LoadAssetAsync<SaveSettings>(SETTINGS_ADDRESS).Task;

        if(_settings == null)
            Debug.LogError($"SaveSettings failed to load. Check Addressables address: {SETTINGS_ADDRESS}");
        else
            Debug.Log($"SaveSettings loaded. Check Addressables address: {SETTINGS_ADDRESS}");

        _isLoading = false;
    }

    public static SaveData LoadOrCreate()
    {
        var fullPath = _settings.GetFullPath();
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

        SaveData allData = new SaveData();

        if (!File.Exists(fullPath))
        {
            string newJsonFile = JsonUtility.ToJson(new SaveData(), _settings.isReadable);
            File.WriteAllText(fullPath, newJsonFile);
        }

        string jsonFile = File.ReadAllText(fullPath);
        allData = JsonUtility.FromJson<SaveData>(jsonFile);

        return allData;
    }

    public static LevelData LoadData(int level)
    {
        SaveData allData = LoadOrCreate();

        if (allData != null)
        {
            for (int i = 0; i < allData.levelSave.Count; i++)
            {
                if (allData.levelSave[i].level == level)
                {
                    return allData.levelSave[i];
                }
            }
        }

        return new LevelData();
    }
}

[Serializable]
public class SaveData
{
    public List<LevelData> levelSave=new();
}

[Serializable]
public class LevelData
{
    public int level;
    public int players;

}

public class ResetAllSaveData
{
    [MenuItem("Tools/BoltsTools/Reset All Save Data")]
    public static void ResetSave()
    {
        bool confirm = EditorUtility.DisplayDialog(
            "WARNING!!!",
            "This Will Delete All Saved Data\n\nAre You Sure You Wanna Do This?",
            "Yes",
            "No");

        if (confirm)
        {
            if (File.Exists(GameSaveController._settings.GetFullPath()))
            {
                File.Delete(GameSaveController._settings.GetFullPath());
            }
        }
    }
}