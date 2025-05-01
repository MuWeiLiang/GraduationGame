using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour
{
    private int score = 0;
    private int goalScore = 5;
    private int magicStoneCount = 0; // 魔法石数量
    private int addNum = 6;
    // private LevelUI levelUI;
    private SLevelManager levelManager;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = FindObjectOfType<SLevelManager>();
        levelManager.UpdateScoreAndGoal(score, goalScore);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddScore(int value)
    {
        score += value;
        if (levelManager != null)
        {
            // levelUI.UpdateScore(score); // 调用LevelUI的UpdateScore方法
            levelManager.UpdateScoreAndGoal(score, goalScore); // 更新分数和目标分数
        }
        else
        {
            Debug.LogError("LevelUI not found!");
        }
    }
    public bool IsGoalScoreReached()
    {
        return score >= goalScore;
    }

    public void AddMagicStone()
    {
        magicStoneCount++;
        goalScore += addNum;
        levelManager.UpdateScoreAndGoal(score, goalScore);
    }
}
