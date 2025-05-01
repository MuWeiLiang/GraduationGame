using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager
{
    //private GameObject pausePanel;
    //private Button resumeButton;
    private GameObject pauseButton;
    private GameObject icon1,icon2;
    private bool isPaused;

    // 初始化暂停系统
    public void Initialize(GameObject btn_pause)
    {
        //pausePanel = panel;
        //isPaused = false;

        //if (pausePanel != null)
        //{
        //    pausePanel.SetActive(false);
        //    SetupResumeButton();
        //}
        //else
        //{
        //    Debug.LogError("PausePanel reference is missing!");
        //}
        pauseButton = btn_pause;
        isPaused = false;
        pauseButton.GetComponent<Button>().onClick.RemoveAllListeners();
        pauseButton.GetComponent<Button>().onClick.AddListener(TogglePause);
        icon1 = pauseButton.transform.Find("Icon_Pause").gameObject;
        icon2 = pauseButton.transform.Find("Icon_Play").gameObject;
    }

    // 设置继续按钮
    private void SetupResumeButton()
    {
        //// 查找继续按钮
        //resumeButton = pausePanel.transform.Find("Btn_Resume")?.GetComponent<Button>();

        //if (resumeButton != null)
        //{
        //    // 移除所有现有监听器
        //    resumeButton.onClick.RemoveAllListeners();
        //    // 添加新的点击监听
        //    resumeButton.onClick.AddListener(TogglePause);

        //    Debug.Log("继续按钮事件绑定成功");
        //}
        //else
        //{
        //    Debug.LogError("找不到Btn_Resume按钮组件");
        //}
    }

    // 切换暂停状态
    public void TogglePause()
    {
        isPaused = !isPaused;

        // 设置时间缩放
        Time.timeScale = isPaused ? 0f : 1f;

        //// 显示/隐藏暂停面板
        //if (pausePanel != null)
        //{
        //    pausePanel.SetActive(isPaused);
        //}
        if(icon1 != null && icon2 != null)
        {
            icon1.SetActive(!isPaused);
            icon2.SetActive(isPaused);
        }
        else
        {
            Debug.LogError("图标未找到");
        }

        Debug.Log($"游戏已{(isPaused ? "暂停" : "恢复")}");
    }

    // 获取当前暂停状态
    public bool IsPaused()
    {
        return isPaused;
    }
}