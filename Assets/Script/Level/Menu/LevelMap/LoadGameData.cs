using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameData : MonoBehaviour
{
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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

        GameSaveData loadedData = SaveSystem.LoadGame();

        // ʹ�ü��ص�����
        Debug.Log($"��ǰ�ؿ�: {loadedData.currentLevel}");
    }
}
