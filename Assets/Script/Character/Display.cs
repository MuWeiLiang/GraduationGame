using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Display : MonoBehaviour
{
    private BaseCharacter baseCharacter;
    public int currentHealth;
    public int currentMana;
    [SerializeField] public Slider healthSlider; // 关联的UI Slider
    [SerializeField] public TextMeshProUGUI HPNum;
    [SerializeField] public Slider manaSlider; // 关联的UI Slider
    [SerializeField] public TextMeshProUGUI MPNum;
    // Start is called before the first frame update
    void Start()
    {
        baseCharacter = GetComponent<BaseCharacter>();
        currentHealth = baseCharacter.health;
        currentMana = baseCharacter.mana;
        UpdateSliderUI(healthSlider); // 更新UI
        UpdateSliderUI(manaSlider); // 更新UI
        UpdateHealthUI(); // 更新UI
        UpdateManaUI(); // 更新UI
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth != baseCharacter.health)
        {
            currentHealth = baseCharacter.health;
            UpdateHealthUI();
        }
        if (currentMana != baseCharacter.mana)
        {
            currentMana = baseCharacter.mana;
            UpdateManaUI();
        }
    }
    void UpdateHealthUI()
    {
        healthSlider.value = currentHealth; // 更新Slider的值
        HPNum.text = currentHealth + " / " + baseCharacter.maxHealth;
    }
    void UpdateManaUI()
    {
        manaSlider.value = currentMana; // 更新Slider的值
        MPNum.text = currentMana + " / " + baseCharacter.maxMana;
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
