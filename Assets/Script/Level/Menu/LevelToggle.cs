using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelToggle : MonoBehaviour
{
    private AudioSource audioSource;
    private int SceneIndex = 0;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void NextLevel()
    {
        // 获取当前场景的索引
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // 计算下一个场景的索引
        SceneIndex = currentSceneIndex + 1;

        if(audioSource != null)
        {
            audioSource.Play();
        }

        // 检查索引是否超出已添加的场景数量
        if (SceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            //SceneManager.LoadScene(SceneIndex);
            Invoke("LoadSceneWithIndex", 0.5f);
        }
        else
        {
            SceneIndex = 0;
            // 如果已经是最后一个场景，可以回到第一个场景（循环）或给出提示
            Debug.LogWarning("已经是最后一个场景，回到第一个场景！");
            SceneManager.LoadScene(0);
        }
    }
    public void LastLevel()
    {
        // 获取当前场景的索引
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // 计算下一个场景的索引
        SceneIndex = currentSceneIndex - 1;

        if (audioSource != null)
        {
            audioSource.Play();
        }

        // 检查索引是否超出已添加的场景数量
        if (SceneIndex >= 0)
        {
            Invoke("LoadSceneWithIndex", 0.5f);
        }
        else
        {
            SceneIndex = 0;
            // 如果已经是最后一个场景，可以回到第一个场景（循环）或给出提示
            Debug.LogWarning("已经是第一个场景，回到第一个场景！");
            SceneManager.LoadScene(0);
        }
    }

    void LoadSceneWithIndex()
    {
        SceneManager.LoadScene(SceneIndex);
    }
}
