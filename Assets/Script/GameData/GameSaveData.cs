using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public int currentLevel = 1;
    public bool[] levelUnlocked;
    public string[] levelGrade;

    // 可以添加方法
    public void CompleteLevel(int level)
    {
        currentLevel = level + 1;
        // totalScore += scoreEarned;
        if (levelUnlocked.Length > level)
        {
            levelUnlocked[level] = true;
        }
    }
}
