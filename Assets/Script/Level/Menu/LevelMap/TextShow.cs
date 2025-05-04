using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextShow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI slevelText;
    // Start is called before the first frame update
    void Start()
    {
        GameSaveData loadedData = SaveSystem.LoadGame();
        levelText.text = "Current Level : Level " + loadedData.currentLevel;
        slevelText.text = "Current SLevel : SLevel " + loadedData.currentSLevel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
