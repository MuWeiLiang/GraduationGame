using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SLevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private TextMeshProUGUI LogText;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject Prompt;
    private TimeManager timeManager;
    private PauseManager pauseManager;
    private LevelUI levelUI;
    private SLevelData levelData;

    // private LevelBaseData levelBaseData; // 0: normal, 1: boss, 2: other

    private void Start()
    {
        // ��ʼ������
        levelData = new SLevelData();
        levelData.Initialize();
        // ��ʼ����ʱ��
        timeManager = new TimeManager();
        timeManager.Initialize(timerText);
        // ��ʼ����ͣϵͳ
        pauseManager = new PauseManager();
        pauseManager.Initialize(pauseButton);
        // ��ʼ������UI  
        levelUI = new LevelUI();
        levelUI.Initialize();

        SPlayerController playerController = FindObjectOfType<SPlayerController>();
        if (playerController != null)
        {
            UpdateLevelHealth(playerController.GetResurrectNum());
            //UpdateLevelHealth(3);
        }
        else
        {
            Debug.LogError("PlayerController not found!");
        }
        // ��ʼ����ʾ
        LogText = Prompt.GetComponentInChildren<TextMeshProUGUI>();
        Prompt.SetActive(false); // ��ʼ������ʾ

        // �����˳���ť
        if (exitButton != null)
        {
            SetupExitBtn();
        }
        else
        {
            Debug.LogError("Exit button not found!");
        }
    }

    private void Update()
    {
        // ���C������
        if (Input.GetKeyDown(KeyCode.C))
        {
            PauseTimer();
        }

        timeManager.Tick(Time.deltaTime);
        if (!timeManager.IsRunning)
        {
            LevelComplete(false);
            SceneManager.LoadScene("LevelComplete");
        }
    }

    public void PauseTimer()
    {
        pauseManager.TogglePause();
    }
    public void AddTime(float seconds) => timeManager.AddTimeRemaining(seconds);
    public void ResetTimer() => timeManager.ResetTimer();
    public void UpdateScore(int score) => levelUI.UpdateScore(score);
    public void UpdateScoreAndGoal(int score, int goal) => levelUI.UpdateScoreAndGoal(score, goal);
    public void AddScore(int amount)
    {
        levelData.AddScore(amount);
        UpdateLevelScore(levelData.GetScore());
    }
    public bool IsGoalScoreReached() => levelData.IsGoalScoreReached();
    public void AddMagicStone()
    {
        levelData.AddMagicStone();
        UpdateLevelGoal(levelData.GetGoal());
        UpdateLevelDiamonds(levelData.GetMagicStoneCount());
    }
    public bool IsCompleted() => levelData.IsCompleted();

    public void UpdateLevelScore(int score) => levelUI.UpdateLevelScore(score);
    public void UpdateLevelGoal(int goal) => levelUI.UpdateLevelGoal(goal);
    public void UpdateLevelHealth(int health) => levelUI.UpdateLevelHealth(health);
    public void UpdateLevelDiamonds(int diamonds) => levelUI.UpdateLevelDiamonds(diamonds);

    public void ActivePrompt(string str)
    {
        Prompt.SetActive(true);
        if (LogText != null)
        {
            LogText.text = str;
        }
        else
        {
            Debug.LogError("LogText not found!");
        }
        Invoke("DeactivePrompt", 1.5f); // 1.5����Զ��ر���ʾ
    }
    public void DeactivePrompt()
    {
        Prompt.SetActive(false);
    }

    private void SetupExitBtn()
    {
        exitButton.GetComponent<Button>().onClick.RemoveAllListeners();
        exitButton.GetComponent<Button>().onClick.AddListener(ExitLevel);
    }
    public void ExitLevel()
    {
        // �˳��ؿ��߼�
        Debug.Log("Exit Game");
        SceneManager.LoadScene("LevelMap");
        // Application.Quit();
    }

    public void LevelComplete(bool flag = true)
    {
        // �ؿ�����߼�
        // Debug.Log("Level Completed!");
        float status = FindObjectOfType<SPlayerController>().GetEvaStatus();
        LevelBaseData.Instance.UpdateEvalutionData(flag,timeManager.GetTimeRemaining(), levelData.GetScore(),status);
    }
}
