using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem
{
    private static string SavePath
    {
        get
        {
#if UNITY_EDITOR
            // 编辑器模式下保存到桌面
            string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            string gameFolder = Path.Combine(desktopPath, "MyGameSave");
            if (!Directory.Exists(gameFolder)) Directory.CreateDirectory(gameFolder);
            return Path.Combine(gameFolder, "savegame.json");
#else
        // 发布版本保存到持久化路径
        return Path.Combine(Application.persistentDataPath, "savegame.json");
#endif
        }
    }

    public static void SaveGame(GameSaveData data)
    {
        try
        {
            // 处理Dictionary等Unity不直接支持的类型
            var wrapper = new Wrapper<GameSaveData> { Data = data };

            string jsonData = JsonUtility.ToJson(wrapper, true); // 第二个参数是是否美化格式
            File.WriteAllText(SavePath, jsonData);
            Debug.Log("游戏已保存至: " + SavePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("保存失败: " + e.Message);
        }
    }

    public static GameSaveData LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("无存档文件，创建新存档");
            return CreateNewSave();
        }

        try
        {
            string jsonData = File.ReadAllText(SavePath);
            var wrapper = JsonUtility.FromJson<Wrapper<GameSaveData>>(jsonData);
            return wrapper.Data;
        }
        catch (System.Exception e)
        {
            Debug.LogError("读取存档失败: " + e.Message);
            return CreateNewSave();
        }
    }

    private static GameSaveData CreateNewSave()
    {
        var newData = new GameSaveData
        {
            currentLevel = 1,
            levelUnlocked = new bool[10],
            levelGrade = new string[10]
        };
        newData.levelUnlocked[0] = true; // 解锁第一关
        newData.levelGrade[0] = "D"; // 默认成绩
        SaveGame(newData); // 保存新存档
        return newData;
    }

    // 用于包装不支持的类型
    [System.Serializable]
    private class Wrapper<T>
    {
        public T Data;
    }
}
