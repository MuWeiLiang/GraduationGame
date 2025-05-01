using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBaseData : MonoBehaviour
{
    public static LevelBaseData Instance { get; private set; }

    public int LevelMode = 0;
    public int currentLevel = 0;
    public int[] LevelCoin;
    public int[] LevelGoal;
    public float[][] LevelCamera;
    public float[][] SLevelCamera;

    public bool isComplete = false;
    public float timeUsed = 137f, timeAll = 180f;
    public float score = 20, scoreAll = 30, goalscore = 25;
    public float status = 67;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 可选：保持对象跨场景
        }
        else
        {
            Destroy(gameObject);
        }

        InitLevelMode();
        InitLevelData();
        InitEvalutionData();
    }

    private void InitLevelMode()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        bool flag = false;
        if (sceneName[5] >= '0' && sceneName[5] <= '9') flag = true;
        if (sceneName.StartsWith("Level") && flag)
        {
            LevelMode = 0;
            currentLevel = sceneName[5] - '0';
        }            
        else if (sceneName.StartsWith("SLevel"))
        {
            LevelMode = 1;
            currentLevel = sceneName[6] - '0';
        }
        else
            LevelMode = 2;
    }
    private void InitLevelData()
    {
        LevelCoin = new int[10];
        LevelGoal = new int[10];
        LevelCoin[1] = 23;LevelGoal[1] = 20;
        LevelCoin[2] = 19;LevelGoal[2] = 16;
        LevelCoin[3] = 21;LevelGoal[3] = 18;
        LevelCoin[4] = 19; LevelGoal[4] = 17;
        LevelCoin[5] = 21; LevelGoal[5] = 19;
        LevelCoin[6] = 18; LevelGoal[6] = 16;
        LevelCamera = new float[10][];// minX, maxX, minY, maxY
        LevelCamera[1] = new float[] { 23.04f, 92.03f, -22.41f, -18.17f };
        LevelCamera[2] = new float[] { 0.33f, 39f, 3.07f, 9.19f };
        LevelCamera[3] = new float[] { 2.23f, 46.45f, 4.05f, 11.43f };
        LevelCamera[4] = new float[] { -7.71f, 10.75f, -10.52f, 8.67f };
        LevelCamera[5] = new float[] { -7.61f, 5.5f, -1.97f, 19.5f };
        LevelCamera[6] = new float[] { -7.56f, 9.68f, -7.93f, 6.74f };
        SLevelCamera = new float[4][];
        SLevelCamera[1] = new float[] { 4.11f, 44.62f, 4.98f, 10.08f};
        SLevelCamera[2] = new float[] { -7.56f, 9.68f, -7.93f, 6.74f };
    }
    private void InitEvalutionData()
    {
        if (LevelMode == 0) { 
            timeAll = 180f;
            scoreAll = LevelCoin[currentLevel];
            goalscore = LevelGoal[currentLevel];
        }
        if (LevelMode == 1) {
            timeAll = 300f;
        }
    }

    public void UpdateEvalutionData(bool flag,float timeRemain, float score, float status)
    {
        isComplete = flag;
        timeUsed = timeAll - timeRemain;
        this.score = score;
        this.status = status;
    }
}
