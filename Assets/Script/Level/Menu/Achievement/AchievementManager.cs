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
            Debug.LogError($"图集 {atlasPath} 未找到或为空！");
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

            // 检查索引有效性
            if (levelIndex < 0 || levelIndex >= loadedData.levelGrade.Length)
            {
                Debug.LogError($"无效的关卡索引: {levelIndex}");
                return;
            }

            // 获取旧成绩和新成绩的排序值
            string oldGrade = loadedData.levelGrade[levelIndex];
            int oldScore = GradeOrder.TryGetValue(oldGrade.ToUpper(), out var o) ? o : int.MaxValue;
            int newScore = GradeOrder.TryGetValue(grade.ToUpper(), out var n) ? n : int.MaxValue;

            // 仅在新成绩更好时更新（数值更小）
            if (newScore < oldScore)
            {
                loadedData.levelGrade[levelIndex] = grade.ToUpper();
                SaveSystem.SaveGame(loadedData);
                Debug.Log($"更新成绩：{levelName} {oldGrade}->{grade}");
            }
            else
            {
                Debug.Log($"保持原成绩：{levelName} {oldGrade}（新成绩{grade}不优于旧成绩）");
            }
        }
        else
        {
            Debug.LogError("加载游戏数据失败，无法保存成绩。");
        }
    }
    
    private void LoadAchievements()
    {
        // 这里可以添加从文件或数据库加载的逻辑
        GameSaveData loadedData = SaveSystem.LoadGame();
        GradData = loadedData.levelGrade;
    }
}
