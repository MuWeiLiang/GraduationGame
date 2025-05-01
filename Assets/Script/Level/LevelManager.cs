using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    // [SerializeField] private GameObject pausePanel; // 暂停时显示的UI面板
    // [SerializeField] private Button resumeButton;  // 恢复按钮
    private bool isPaused = false;   // 游戏是否暂停
    // Start is called before the first frame update
    void Start()
    {

        //pausePanel.SetActive(false);
        //// 添加按钮点击事件
        //if (resumeButton != null)
        //{
        //    resumeButton.onClick.AddListener(ResumeGame);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // 暂停游戏时间

        // pausePanel.SetActive(true);
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // 恢复游戏时间

        // 隐藏暂停面板
        // pausePanel.SetActive(false);
    }
}
