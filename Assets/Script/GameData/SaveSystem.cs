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
            // �༭��ģʽ�±��浽����
            string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            string gameFolder = Path.Combine(desktopPath, "MyGameSave");
            if (!Directory.Exists(gameFolder)) Directory.CreateDirectory(gameFolder);
            return Path.Combine(gameFolder, "savegame.json");
#else
        // �����汾���浽�־û�·��
        return Path.Combine(Application.persistentDataPath, "savegame.json");
#endif
        }
    }

    public static void SaveGame(GameSaveData data)
    {
        try
        {
            // ����Dictionary��Unity��ֱ��֧�ֵ�����
            var wrapper = new Wrapper<GameSaveData> { Data = data };

            string jsonData = JsonUtility.ToJson(wrapper, true); // �ڶ����������Ƿ�������ʽ
            File.WriteAllText(SavePath, jsonData);
            Debug.Log("��Ϸ�ѱ�����: " + SavePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("����ʧ��: " + e.Message);
        }
    }

    public static GameSaveData LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("�޴浵�ļ��������´浵");
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
            Debug.LogError("��ȡ�浵ʧ��: " + e.Message);
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
        newData.levelUnlocked[0] = true; // ������һ��
        newData.levelGrade[0] = "D"; // Ĭ�ϳɼ�
        SaveGame(newData); // �����´浵
        return newData;
    }

    // ���ڰ�װ��֧�ֵ�����
    [System.Serializable]
    private class Wrapper<T>
    {
        public T Data;
    }
}
