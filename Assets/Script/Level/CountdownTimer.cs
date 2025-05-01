using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    public float timeRemaining;
    public float initialTime = 180f;
    private static float persistentTimeRemaining = -1f; // -1表示未初始化
    [SerializeField] private TextMeshProUGUI timeText;

    void Start()
    {
        timeRemaining = persistentTimeRemaining >= 0 ? persistentTimeRemaining : initialTime;
        UpdateCountdownText();
    }
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            persistentTimeRemaining = timeRemaining;
            UpdateCountdownText();
        }
        else
        {
            timeRemaining = 0;
            // 倒计时结束后的逻辑，例如游戏结束
            Debug.Log("Time's up!");
        }
    }

    void UpdateCountdownText()
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
}
