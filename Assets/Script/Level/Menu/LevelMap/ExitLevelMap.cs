using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevelMap : MonoBehaviour
{
    private AudioSource audioSource;
    private string levelName = "StartScene";
    public void OnClick()
    {
        // Debug.Log("Loading level: " + levelName);
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource not found on " + gameObject.name);
        }
        // SceneManager.LoadScene(levelName);
        Invoke("LoadStartMenu", 0.5f);
    }
    private void LoadStartMenu()
    {
        SceneManager.LoadScene(levelName);
    }
}
