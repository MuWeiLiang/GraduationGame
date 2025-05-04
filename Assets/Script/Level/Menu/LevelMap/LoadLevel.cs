using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    private AudioSource audioSource;
    void Start()
    {
        // ����������һЩ��ʼ������
        string buttonName = gameObject.name;
        // ��"Btn-Level1"����ȡ"Level1"
        string levelName = buttonName.Split('-')[1];

        GameSaveData loadedData = SaveSystem.LoadGame();
        if (levelName.StartsWith("Level"))
        {
            string numberPart = levelName.Substring(5);
            int levelNumber;
            if (int.TryParse(numberPart, out levelNumber))
            {

            }
            else
            {
                // ת��ʧ�ܴ���
                Debug.LogError($"�޷���'{numberPart}'��������");
                levelNumber = 1; // ����Ĭ��ֵ
            }
            if (loadedData.levelUnlocked[levelNumber - 1])
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        if (levelName.StartsWith("SLevel"))
        {
            string numberPart = levelName.Substring(6);
            int levelNumber;
            if (int.TryParse(numberPart, out levelNumber))
            {
            }
            else
            {
                // ת��ʧ�ܴ���
                Debug.LogError($"�޷���'{numberPart}'��������");
                levelNumber = 1; // ����Ĭ��ֵ
            }
            if (loadedData.SlevelUnlocked[levelNumber - 1] && loadedData.levelUnlocked[levelNumber*3])
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
    public void OnClick()
    {
        string buttonName = gameObject.name;
        // ��"Btn-Level1"����ȡ"Level1"
        string levelName = buttonName.Split('-')[1];
        Debug.Log("Loading level: " + levelName);
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource not found on " + gameObject.name);
        }
        StartCoroutine(LoadSceneAfterDelay(levelName, 0.5f)); // 0.5�����س���
    }
    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay); // �ȴ� delay ��
        SceneManager.LoadScene(sceneName); // ���س���
    }
}
