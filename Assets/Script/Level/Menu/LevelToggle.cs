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
        // ��ȡ��ǰ����������
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // ������һ������������
        SceneIndex = currentSceneIndex + 1;

        if(audioSource != null)
        {
            audioSource.Play();
        }

        // ��������Ƿ񳬳�����ӵĳ�������
        if (SceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            //SceneManager.LoadScene(SceneIndex);
            Invoke("LoadSceneWithIndex", 0.5f);
        }
        else
        {
            SceneIndex = 0;
            // ����Ѿ������һ�����������Իص���һ��������ѭ�����������ʾ
            Debug.LogWarning("�Ѿ������һ���������ص���һ��������");
            SceneManager.LoadScene(0);
        }
    }
    public void LastLevel()
    {
        // ��ȡ��ǰ����������
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // ������һ������������
        SceneIndex = currentSceneIndex - 1;

        if (audioSource != null)
        {
            audioSource.Play();
        }

        // ��������Ƿ񳬳�����ӵĳ�������
        if (SceneIndex >= 0)
        {
            Invoke("LoadSceneWithIndex", 0.5f);
        }
        else
        {
            SceneIndex = 0;
            // ����Ѿ������һ�����������Իص���һ��������ѭ�����������ʾ
            Debug.LogWarning("�Ѿ��ǵ�һ���������ص���һ��������");
            SceneManager.LoadScene(0);
        }
    }

    void LoadSceneWithIndex()
    {
        SceneManager.LoadScene(SceneIndex);
    }
}
