using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnClickStart()
    {
        Debug.Log("Starting game...");
        SceneManager.LoadScene("LevelMap");
    }
    public void OnClickExit()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#endif
    }
}
