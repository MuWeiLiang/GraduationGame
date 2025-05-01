using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 使用示例：技能管理器
public class SkillManager : MonoBehaviour
{
    [System.Serializable]
    public class SkillUI
    {
        public Image iconImage;
        public Image cooldownOverlay;
        public TMP_Text keyHintText;
    }
    [SerializeField] private SkillUI[] skillUIs = new SkillUI[3];
    SkillController[] skills;
    GameObject skillmanager;
    [SerializeField] ElementType elementType = ElementType.Fire; // 默认元素类型
    void Start()
    {
        skills = new SkillController[3];
        skillmanager = new GameObject("SkillManager");
        skillmanager.transform.SetParent(transform, true); // 设置父物体为当前物体
        SelectElementSkill(elementType);
        InitializeSkills();
        SetupSkillUI();
    }

    void SelectElementSkill(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                skills[0] = skillmanager.AddComponent<FireSkill1Controller>();
                skills[1] = skillmanager.AddComponent<FireSkill2Controller>();
                skills[2] = skillmanager.AddComponent<FireSkill3Controller>();
                break;
            case ElementType.Water:
                skills[0] = skillmanager.AddComponent<WaterSkill1Controller>();
                skills[1] = skillmanager.AddComponent<WaterSkill2Controller>();
                skills[2] = skillmanager.AddComponent<WaterSkill3Controller>();
                break;
            case ElementType.Thunder:
                skills[0] = skillmanager.AddComponent<ThunderSkill1Controller>();
                skills[1] = skillmanager.AddComponent<ThunderSkill2Controller>();
                skills[2] = skillmanager.AddComponent<ThunderSkill3Controller>();
                break;
            case ElementType.Wind:
                skills[0] = skillmanager.AddComponent<WindSkill1Controller>();
                skills[1] = skillmanager.AddComponent<WindSkill2Controller>();
                skills[2] = skillmanager.AddComponent<WindSkill3Controller>();
                break;
            case ElementType.Earth:
                skills[0] = skillmanager.AddComponent<EarthSkill1Controller>();
                skills[1] = skillmanager.AddComponent<EarthSkill2Controller>();
                skills[2] = skillmanager.AddComponent<EarthSkill3Controller>();
                break;
        }
    }
    void InitializeSkills()
    {
        var playerController = GetComponent<SPlayerController>();
        foreach (SkillController skill in skills)
        {
            if(skill == null) continue;
            skill.Initialize();
            skill.playerController = playerController;
        }
        playerController.InitSkillData(elementType);
    }
    void SetupSkillUI()
    {
        string iconPath = "SkillIcon/" + elementType.ToString();
        string[] key = new string[3] { "U", "I", "O" };
        for (int i = 0; i < skills.Length; i++)
        {
            string path = iconPath + (i + 1).ToString();
            if (skills[i] == null) continue;
            var sprite = Resources.Load<Sprite>(path);
            if (sprite == null)
            {
                Debug.LogError($"加载失败: {path}");
            } else
                skillUIs[i].iconImage.sprite = sprite;
            skillUIs[i].keyHintText.text = key[i];
            skillUIs[i].cooldownOverlay.fillAmount = 0;
        }
    }
    void Update()
    {
        HandleInput();
        UpdateSkillUI();
        foreach (SkillController skill in skills) { 
            skill.UpdateCooldown();
        }
    }

    void UpdateSkillUI()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i] == null || skillUIs[i] == null) continue;

            if (skills[i].IsReady) continue;

            // 更新冷却显示
            float cooldownPercent = Mathf.Clamp01(skills[i].currentCooldown / skills[i].Cooldown);
            skillUIs[i].cooldownOverlay.fillAmount = cooldownPercent;
        }
    }
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            skills[0].SetSpawnPotionAndDir(transform.position, transform.localScale.x > 0 ? 1 : -1);
            skills[0].Activate();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            skills[1].SetSpawnPotionAndDir(transform.position, transform.localScale.x > 0 ? 1 : -1);
            skills[1].Activate();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            skills[2].SetSpawnPotionAndDir(transform.position, transform.localScale.x > 0 ? 1 : -1);
            skills[2].Activate();
        }
    }
}
