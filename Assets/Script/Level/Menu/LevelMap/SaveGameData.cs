using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveGameData : MonoBehaviour
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
        GameSaveData playerData = SaveSystem.LoadGame();
        playerData.currentLevel = 3;
        playerData.levelUnlocked[1] = true; // ������2��
        playerData.levelUnlocked[2] = true; // ������3��
        // ��������
        SaveSystem.SaveGame(playerData);
    }
}
