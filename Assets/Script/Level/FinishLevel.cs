using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    private bool levelCompleted = false;
    private SLevelManager levelManager;
    private void Start()
    {
        levelManager = FindObjectOfType<SLevelManager>();
        if (levelManager == null)
        {
            Debug.LogError("SLevelManager not found!");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player" && !levelCompleted)
        {
            levelCompleted = IsLevelCompleted();
            if(levelCompleted)
            {
                // levelManager.CompleteLevel();
                // Debug.Log("Level Completed!");
                //levelManager.LevelComplete();
                //Invoke("CompleteLevel", 1f);
                Finishlevel();
            }
            else
            {
                Debug.Log("Level Not Completed!");
            }
            // Invoke("CompleteLevel", 1f);
        }
    }
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
