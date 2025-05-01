using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI evalutionText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private GameObject nextBtn;
    public bool isComplete = false;
    public float timeUsed = 137f, timeAll = 180f;
    public float score = 20, scoreAll = 30, goalscore = 20;
    public float status = 67;
    // Start is called before the first frame update
    void Start()
    {
        UpdateEvaData();
        DataDisplay();
        PlaySound();
    }
    void PlaySound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        string resourcePath = "Sounds/";
        resourcePath += (isComplete ? "success" : "fail");
        AudioClip audioClip = Resources.Load<AudioClip>(resourcePath);
        if (audioClip != null)
        {
            Debug.Log("Playing sound: " + resourcePath);
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            Debug.LogError("Audio clip not found at path: " + resourcePath);
        }
    }
    private void UpdateEvaData()
    {
        if (LevelBaseData.Instance != null)
        {
            isComplete = LevelBaseData.Instance.isComplete;
            timeUsed = LevelBaseData.Instance.timeUsed;
            timeAll = LevelBaseData.Instance.timeAll;
            score = LevelBaseData.Instance.score;
            scoreAll = LevelBaseData.Instance.scoreAll;
            goalscore = LevelBaseData.Instance.goalscore;
            status = LevelBaseData.Instance.status;
        }
    }
    public void DataDisplay()
    {
        int minutes = Mathf.FloorToInt(timeUsed / 60);
        int seconds = Mathf.FloorToInt(timeUsed % 60);
        string str = string.Format("{0:00}:{1:00}", minutes, seconds);
        resultText.text = isComplete ? "SUCCESS!!" : "FAIL!!";
        string evalution = GetEvalution();
        evalutionText.text = evalution;
        timeText.text = "Time   " + str;
        scoreText.text = "Score    " + score.ToString();
        statusText.text = "Status  " + status.ToString() + "%";
        if (!isComplete)
        {
            SetNextBtn(false);
        }
    }
    private string GetEvalution()
    {
        if (!isComplete)
            return "E";
        float favor1 = 1f - timeUsed / timeAll, favor2 = (score-goalscore) / (scoreAll - goalscore) * 0.4f + 0.60f, favor3 = status / 100f;
        float Evaluation = 0f;
        if (LevelBaseData.Instance.LevelMode == 1)
            Evaluation = favor1 * 30f + favor2 * 50f + favor3 * 20f;
        if (LevelBaseData.Instance.LevelMode == 0)
            Evaluation = favor1 * 50f + favor2 * 30f + favor3 * 20f;
        // Debug.Log("Favor1: " + favor1 + ", Favor2: " + favor2 + ", Favor3: " + favor3);
        // Debug.Log("Evaluation: " + Evaluation);
        if (Evaluation >= 90)
            return "S";
        else if (Evaluation >= 70)
            return "A";
        else if (Evaluation >= 50)
            return "B";
        else if (Evaluation >= 25)
            return "C";
        return "D";
    }
    private void SetNextBtn(bool isComplete)
    {
        nextBtn.SetActive(isComplete);
    }
    public void OnClick1()
    {
        string LevelName = LevelBaseData.Instance.LevelMode == 0 ? "Level" : "SLevel";
        LevelName += (LevelBaseData.Instance.currentLevel).ToString();
        Debug.Log("Loading level: " + LevelName);
        SceneManager.LoadScene(LevelName);
    }

    public void OnClick2()
    {
        string nextLevelName = LevelBaseData.Instance.LevelMode == 0 ? "Level" : "SLevel";
        nextLevelName += (LevelBaseData.Instance.currentLevel + 1).ToString();
        if (isComplete)
            SceneManager.LoadScene(nextLevelName);
    }

    public void OnClick3()
    {
        SceneManager.LoadScene("LevelMap");
    }
}
