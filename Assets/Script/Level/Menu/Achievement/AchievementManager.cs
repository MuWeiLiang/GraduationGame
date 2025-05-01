using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class AchievementManager : MonoBehaviour
{
    [SerializeField] private GameObject achievementPrefab;
    [SerializeField] private Transform contentParent;
    string[] GradData = null;

    void Start()
    {
        LoadAchievements();
        GenerateAchievementItems();
    }

    private void GenerateAchievementItems()
    {
        string atlasPath = "UI Icon/Achievement Icons 32 x 32";
        Sprite[] allSprites = Resources.LoadAll<Sprite>(atlasPath);
        if (allSprites == null || allSprites.Length == 0)
        {
            Debug.LogError($"ͼ�� {atlasPath} δ�ҵ���Ϊ�գ�");
            return;
        }
        for (int i = 0; i < GradData.Length; i++)
        {
            var data = GradData[i];
            if (data != "S" && data != "A" && data != "B") continue;
            int Index = 51;
            if (data == "B") Index = 9;
            if (data == "A") Index = 83;
            GameObject item = Instantiate(achievementPrefab, contentParent);
            AchievementItem controller = item.GetComponent<AchievementItem>();
            controller.Initialize(i + 1, data, allSprites[Index]);
        }
    }

    private static readonly Dictionary<string, int> GradeOrder = new Dictionary<string, int>
    {
        { "S", 0 }, { "A", 1 }, { "B", 2 }, { "C", 3 }, { "D", 4 }
    };

    public void SaveLevelGrade(string levelName, string grade)
    {
        GameSaveData loadedData = SaveSystem.LoadGame();
        if (loadedData != null)
        {
            int levelIndex = int.Parse(levelName.Substring(levelName.Length - 1)) - 1;

            // ���������Ч��
            if (levelIndex < 0 || levelIndex >= loadedData.levelGrade.Length)
            {
                Debug.LogError($"��Ч�Ĺؿ�����: {levelIndex}");
                return;
            }

            // ��ȡ�ɳɼ����³ɼ�������ֵ
            string oldGrade = loadedData.levelGrade[levelIndex];
            int oldScore = GradeOrder.TryGetValue(oldGrade.ToUpper(), out var o) ? o : int.MaxValue;
            int newScore = GradeOrder.TryGetValue(grade.ToUpper(), out var n) ? n : int.MaxValue;

            // �����³ɼ�����ʱ���£���ֵ��С��
            if (newScore < oldScore)
            {
                loadedData.levelGrade[levelIndex] = grade.ToUpper();
                SaveSystem.SaveGame(loadedData);
                Debug.Log($"���³ɼ���{levelName} {oldGrade}->{grade}");
            }
            else
            {
                Debug.Log($"����ԭ�ɼ���{levelName} {oldGrade}���³ɼ�{grade}�����ھɳɼ���");
            }
        }
        else
        {
            Debug.LogError("������Ϸ����ʧ�ܣ��޷�����ɼ���");
        }
    }
    
    private void LoadAchievements()
    {
        // ���������Ӵ��ļ������ݿ���ص��߼�
        GameSaveData loadedData = SaveSystem.LoadGame();
        GradData = loadedData.levelGrade;
    }
}
