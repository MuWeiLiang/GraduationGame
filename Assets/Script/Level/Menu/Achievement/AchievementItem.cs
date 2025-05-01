using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementItem : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI gradeText;
    //[SerializeField] private GameObject lockOverlay;

    public void Initialize(int LevelIndex, string data, Sprite Icon)
    {
        icon.sprite = Icon;
        levelText.text = "Level "+LevelIndex;
        gradeText.text = data;
        //lockOverlay.SetActive(!isUnlocked);
    }

    // 点击显示详细信息
    public void OnClick()
    {
        Debug.Log($"显示关卡 {levelText.text} 的详细信息");
    }
}
