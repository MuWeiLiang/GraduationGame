using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI
{
    public int currentHealth;
    public int currentMana;
    private Slider healthSlider; // 关联的UI Slider
    private TextMeshProUGUI HPNum;
    private Slider manaSlider; // 关联的UI Slider
    private TextMeshProUGUI MPNum;
    private PlayerBase playerBase; // 角色基类
    public void Initialize(PlayerBase player)
    {
        playerBase = player; // 角色基类
        GetCanvasCompenent();
        currentHealth = playerBase.health;
        currentMana = playerBase.mana;
        UpdateSliderUI(healthSlider); // 更新UI
        UpdateSliderUI(manaSlider); // 更新UI
        UpdateHealthUI(); // 更新UI
        UpdateManaUI(); // 更新UI
    }
    public void Tick()
    {
        if (currentHealth != playerBase.health)
        {
            currentHealth = playerBase.health;
            UpdateHealthUI();
        }
        if (currentMana != playerBase.mana)
        {
            currentMana = playerBase.mana;
            UpdateManaUI();
        }
    }
    void GetCanvasCompenent()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            Debug.LogError("Canvas not found!");
            return;
        }
        healthSlider = canvas.transform.Find("PlayerUI/HP/PlayerHP").GetComponent<Slider>();
        HPNum = canvas.transform.Find("PlayerUI/HP/HPNum").GetComponent<TextMeshProUGUI>();
        manaSlider = canvas.transform.Find("PlayerUI/MP/PlayerMP").GetComponent<Slider>();
        MPNum = canvas.transform.Find("PlayerUI/MP/MPNum").GetComponent<TextMeshProUGUI>();
        if(healthSlider == null || HPNum == null || manaSlider == null || MPNum == null)
        {
            Debug.LogError("UI components not found!");
            return;
        }
    }
    void UpdateHealthUI()
    {
        float healthPercent = (float)currentHealth / playerBase.maxHealth; // 计算血量百分比
        healthSlider.value = healthPercent * 100f; // 更新Slider的值
        HPNum.text = currentHealth + " / " + playerBase.maxHealth;
    }
    void UpdateManaUI()
    {
        float manaPercent = (float)currentMana / playerBase.maxMana; // 计算蓝量百分比
        manaSlider.value = manaPercent * 100f; // 更新Slider的值
        MPNum.text = currentMana + " / " + playerBase.maxMana;
    }
    void UpdateSliderUI(Slider slider)
    {
        if (slider != null)
        {
            // 隐藏 Handle
            Transform handle = slider.transform.Find("Handle Slide Area/Handle");
            if (handle != null) handle.gameObject.SetActive(false);

            // 调整 Fill Area 的 RectTransform
            RectTransform fillAreaRect = slider.fillRect.parent.GetComponent<RectTransform>();
            if (fillAreaRect != null)
            {
                fillAreaRect.anchorMin = new Vector2(0, 0.25f);
                fillAreaRect.anchorMax = new Vector2(1, 0.75f);
                fillAreaRect.offsetMin = Vector2.zero;
                fillAreaRect.offsetMax = Vector2.zero;
            }

            // 调整 Fill 的 RectTransform
            RectTransform fillRect = slider.fillRect;
            if (fillRect != null)
            {
                fillRect.anchorMin = new Vector2(0, 0.25f);
                fillRect.anchorMax = new Vector2(1, 0.75f);
                fillRect.offsetMin = Vector2.zero;
                fillRect.offsetMax = Vector2.zero;
            }

            // 调整 Background 的 RectTransform（可选）
            RectTransform bgRect = slider.transform.Find("Background").GetComponent<RectTransform>();
            if (bgRect != null)
            {
                bgRect.anchorMin = new Vector2(0, 0.25f);
                bgRect.anchorMax = new Vector2(1, 0.75f);
                bgRect.offsetMin = Vector2.zero;
                bgRect.offsetMax = Vector2.zero;
            }

            // 确保 Slider 方向正确
            slider.direction = Slider.Direction.LeftToRight;
        }
    }
}
