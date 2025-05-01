using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager
{
    public float initialTime = 300f;
    private float timeRemaining;
    private static float persistentTimeRemaining = -1f; // -1表示未初始化
    private TextMeshProUGUI timeText;
    private int levelMode = 0;

    public bool IsRunning { get; private set; }

    // 初始化计时器
    public void Initialize(TextMeshProUGUI timerText)
    {
        levelMode = LevelBaseData.Instance.LevelMode;
        if (levelMode == 0) initialTime = 180f;
        timeText = timerText;
        timeRemaining = persistentTimeRemaining >= 0 ? persistentTimeRemaining : initialTime;
        UpdateCountdownText();
        IsRunning = true;
    }

    // 手动更新计时器（替代 Update）
    public void Tick(float deltaTime)
    {
        if (!IsRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= deltaTime;
            persistentTimeRemaining = timeRemaining;
            UpdateCountdownText();
        }
        else
        {
            timeRemaining = 0;
            IsRunning = false;
            Debug.Log("Time's up!");
        }
    }

    private void UpdateCountdownText()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void AddTimeRemaining(float addAmount)
    {
        timeRemaining += addAmount;
        UpdateCountdownText();
    }

    public void ResetTimer()
    {
        timeRemaining = initialTime;
        persistentTimeRemaining = -1f;
        UpdateCountdownText();
        IsRunning = true;
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    public void Pause()
    {
        IsRunning = false;
    }

    public void Resume()
    {
        IsRunning = true;
    }
}
