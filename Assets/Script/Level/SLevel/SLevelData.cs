using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SLevelData
{
    private int score = 0;
    private int goalScore = 5;
    private int magicStoneCount = 0; // 魔法石数量
    private int addNum = 6;
    // Start is called before the first frame update
    public void Initialize()
    {
        if (LevelBaseData.Instance.LevelMode == 0)
            goalScore = LevelBaseData.Instance.LevelGoal[LevelBaseData.Instance.currentLevel];
    }
    public void AddScore(int value)
    {
        score += value;
    }
    public bool IsGoalScoreReached()
    {
        return score >= goalScore;
    }

    public void AddMagicStone()
    {
        magicStoneCount++;
        goalScore += addNum;
    }
    public int GetScore() { 
        return score;
    }
    public int GetGoal() { 
        return goalScore; 
    }
    public int GetMagicStoneCount()
    {
        return magicStoneCount;
    }
    public bool IsCompleted()
    {
        if(LevelBaseData.Instance.LevelMode == 1)
            return magicStoneCount == 3;
        // Debug.Log(LevelBaseData.Instance.LevelMode);
        // Debug.Log("score = " + score + ", Goal = " + goalScore);
        return score >= goalScore;
    }
}
