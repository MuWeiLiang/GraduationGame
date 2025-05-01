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

    // �����ʾ��ϸ��Ϣ
    public void OnClick()
    {
        Debug.Log($"��ʾ�ؿ� {levelText.text} ����ϸ��Ϣ");
    }
}
