using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    private bool levelCompleted = false;
    private bool playerInRange = false;
    private SLevelManager levelManager;
    private KeyCode interactKey = KeyCode.F; // ��������
    private void Start()
    {
        levelManager = FindObjectOfType<SLevelManager>();
        if (levelManager == null)
        {
            Debug.LogError("SLevelManager not found!");
        }
    }
    private void Update()
    {
        if (playerInRange && !levelCompleted && Input.GetKeyDown(interactKey))
        {
            AttemptCompleteLevel();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            // ������������ʾUI��ʾ�����磺"�� F ��ɹؿ�"��
            if (levelManager != null) {
                levelManager.ActivePrompt("Press F");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            // ��������������UI��ʾ
        }
    }

    private void AttemptCompleteLevel()
    {
        if (levelManager == null) return;
        if (levelCompleted) return;

        if (IsLevelCompleted())
        {
            levelCompleted = true;
            levelManager.ActivePrompt("Complete Level!");
            Finishlevel();
        }
        else
        {
            levelManager.ActivePrompt("Level requirements not met!");
            Debug.Log("Level requirements not met!");
            // ���Դ�����ʾ��Ч��UI����
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.gameObject.name == "Player" && !levelCompleted)
    //    {
    //        levelCompleted = IsLevelCompleted();
    //        if(levelCompleted)
    //        {
    //            Finishlevel();
    //        }
    //        else
    //        {
    //            Debug.Log("Level Not Completed!");
    //        }
    //        // Invoke("CompleteLevel", 1f);
    //    }
    //}
    public void Finishlevel(bool flag = true)
    {
        levelManager.LevelComplete(flag);
        Invoke("CompleteLevel", 1f);
    }
    private void CompleteLevel()
    {
        SceneManager.LoadScene("LevelComplete");
    }
    private bool IsLevelCompleted()
    {
        return levelManager.IsCompleted();
    }
}
