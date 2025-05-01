using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUI
{
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI goalText;
    private TextMeshProUGUI healthText;
    private TextMeshProUGUI diamondText;

    public void Initialize()
    {
        scoreText = GameObject.Find("Canvas/LevelUI/ScoreUI/ScoreText").GetComponent<TextMeshProUGUI>();
        goalText = GameObject.Find("Canvas/LevelUI/GoalUI/GoalText").GetComponent<TextMeshProUGUI>();
        healthText = GameObject.Find("Canvas/LevelUI/HealthUI/HealthText").GetComponent<TextMeshProUGUI>();
        diamondText = GameObject.Find("Canvas/LevelUI/DiamondUI/DiamondText").GetComponent<TextMeshProUGUI>();
        // UpdateScoreAndGoal(0, 5); // 初始化分数和目标分数
        if (scoreText == null || goalText == null || healthText == null || diamondText == null) {
            Debug.LogError("UI elements not found!");
        }
        if(LevelBaseData.Instance.LevelMode == 0)
            GameObject.Find("Canvas/LevelUI/DiamondUI").SetActive(false);
        UpdateUI();
    }
    private void UpdateUI()
    {
        int levelmode = LevelBaseData.Instance.LevelMode;
        UpdateLevelScore(0);
        if (levelmode == 0)
            UpdateLevelGoal(LevelBaseData.Instance.LevelGoal[LevelBaseData.Instance.currentLevel]);
        else
            UpdateLevelGoal(5);
        UpdateLevelHealth(levelmode == 0 ? 1 : 2);
        UpdateLevelDiamonds(0);
    }
    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }
    public void UpdateScoreAndGoal(int score,int goal)
    {
        scoreText.text = "Score/Goal: " + score.ToString() + "/" + goal.ToString();
    }

    public void UpdateLevelScore(int score)
    {
        scoreText.text = score.ToString();
    }
    public void UpdateLevelHealth(int health)
    {
        healthText.text = (health+1).ToString();
    }
    public void UpdateLevelDiamonds(int diamonds)
    {
        diamondText.text = diamonds.ToString();
    }
    public void UpdateLevelGoal(int goal)
    {
        goalText.text = goal.ToString();
    }
}
