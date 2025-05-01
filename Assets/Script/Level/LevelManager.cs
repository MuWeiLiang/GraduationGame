using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    // [SerializeField] private GameObject pausePanel; // ��ͣʱ��ʾ��UI���
    // [SerializeField] private Button resumeButton;  // �ָ���ť
    private bool isPaused = false;   // ��Ϸ�Ƿ���ͣ
    // Start is called before the first frame update
    void Start()
    {

        //pausePanel.SetActive(false);
        //// ��Ӱ�ť����¼�
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
        Time.timeScale = 0f; // ��ͣ��Ϸʱ��

        // pausePanel.SetActive(true);
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // �ָ���Ϸʱ��

        // ������ͣ���
        // pausePanel.SetActive(false);
    }
}
