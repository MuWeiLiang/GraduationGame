using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    private AudioSource audioSource;
    void Start()
    {
        // 这里可以添加一些初始化代码
        string buttonName = gameObject.name;
        // 从"Btn-Level1"中提取"Level1"
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
                // 转换失败处理
                Debug.LogError($"无法从'{numberPart}'解析数字");
                levelNumber = 1; // 设置默认值
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
                // 转换失败处理
                Debug.LogError($"无法从'{numberPart}'解析数字");
                levelNumber = 1; // 设置默认值
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
        // 从"Btn-Level1"中提取"Level1"
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
        StartCoroutine(LoadSceneAfterDelay(levelName, 0.5f)); // 0.5秒后加载场景
    }
    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay); // 等待 delay 秒
        SceneManager.LoadScene(sceneName); // 加载场景
    }
}
