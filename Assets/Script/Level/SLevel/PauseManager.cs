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

    // ��ʼ����ͣϵͳ
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

    // ���ü�����ť
    private void SetupResumeButton()
    {
        //// ���Ҽ�����ť
        //resumeButton = pausePanel.transform.Find("Btn_Resume")?.GetComponent<Button>();

        //if (resumeButton != null)
        //{
        //    // �Ƴ��������м�����
        //    resumeButton.onClick.RemoveAllListeners();
        //    // ����µĵ������
        //    resumeButton.onClick.AddListener(TogglePause);

        //    Debug.Log("������ť�¼��󶨳ɹ�");
        //}
        //else
        //{
        //    Debug.LogError("�Ҳ���Btn_Resume��ť���");
        //}
    }

    // �л���ͣ״̬
    public void TogglePause()
    {
        isPaused = !isPaused;

        // ����ʱ������
        Time.timeScale = isPaused ? 0f : 1f;

        //// ��ʾ/������ͣ���
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
            Debug.LogError("ͼ��δ�ҵ�");
        }

        Debug.Log($"��Ϸ��{(isPaused ? "��ͣ" : "�ָ�")}");
    }

    // ��ȡ��ǰ��ͣ״̬
    public bool IsPaused()
    {
        return isPaused;
    }
}