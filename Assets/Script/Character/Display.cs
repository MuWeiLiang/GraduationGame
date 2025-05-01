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
    [SerializeField] public Slider healthSlider; // ������UI Slider
    [SerializeField] public TextMeshProUGUI HPNum;
    [SerializeField] public Slider manaSlider; // ������UI Slider
    [SerializeField] public TextMeshProUGUI MPNum;
    // Start is called before the first frame update
    void Start()
    {
        baseCharacter = GetComponent<BaseCharacter>();
        currentHealth = baseCharacter.health;
        currentMana = baseCharacter.mana;
        UpdateSliderUI(healthSlider); // ����UI
        UpdateSliderUI(manaSlider); // ����UI
        UpdateHealthUI(); // ����UI
        UpdateManaUI(); // ����UI
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
        healthSlider.value = currentHealth; // ����Slider��ֵ
        HPNum.text = currentHealth + " / " + baseCharacter.maxHealth;
    }
    void UpdateManaUI()
    {
        manaSlider.value = currentMana; // ����Slider��ֵ
        MPNum.text = currentMana + " / " + baseCharacter.maxMana;
    }
    void UpdateSliderUI(Slider slider)
    {
        if (slider != null)
        {
            // ���� Handle
            Transform handle = slider.transform.Find("Handle Slide Area/Handle");
            if (handle != null) handle.gameObject.SetActive(false);

            // ���� Fill Area �� RectTransform
            RectTransform fillAreaRect = slider.fillRect.parent.GetComponent<RectTransform>();
            if (fillAreaRect != null)
            {
                fillAreaRect.anchorMin = new Vector2(0, 0.25f);
                fillAreaRect.anchorMax = new Vector2(1, 0.75f);
                fillAreaRect.offsetMin = Vector2.zero;
                fillAreaRect.offsetMax = Vector2.zero;
            }

            // ���� Fill �� RectTransform
            RectTransform fillRect = slider.fillRect;
            if (fillRect != null)
            {
                fillRect.anchorMin = new Vector2(0, 0.25f);
                fillRect.anchorMax = new Vector2(1, 0.75f);
                fillRect.offsetMin = Vector2.zero;
                fillRect.offsetMax = Vector2.zero;
            }

            // ���� Background �� RectTransform����ѡ��
            RectTransform bgRect = slider.transform.Find("Background").GetComponent<RectTransform>();
            if (bgRect != null)
            {
                bgRect.anchorMin = new Vector2(0, 0.25f);
                bgRect.anchorMax = new Vector2(1, 0.75f);
                bgRect.offsetMin = Vector2.zero;
                bgRect.offsetMax = Vector2.zero;
            }

            // ȷ�� Slider ������ȷ
            slider.direction = Slider.Direction.LeftToRight;
        }
    }
}
