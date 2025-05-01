using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SkillType { None, Dash, Fly, Blink, Purify, Summon, AreaSlow };

public class SkillSystem : MonoBehaviour
{
    [System.Serializable]
    public class SkillUI
    {
        public SkillType skillType;
        public Image skillIcon;
        public Image cooldownOverlay;
        public TextMeshProUGUI cooldownText; // 修改为TextMeshProUGUI类型
    }
    public SkillType currentSkill = SkillType.None;
    private MoveCharacter moveCharacter;
    private StatusManager statusManager;
    public GameObject summonedCreaturePrefab;
    private string summonPrefabPath = "Prefab/Summon/Summon";
    private Dictionary<SkillType, bool> skillActives;
    private Dictionary<SkillType, float> cooldownTimes;
    private Dictionary<SkillType, float> currentCooldowns;
    private Dictionary<SkillType, int> manaCosts;
    private Dictionary<SkillType, SkillUI> skillUIDict = new Dictionary<SkillType, SkillUI>();
    private void Awake()
    {
        cooldownTimes = new Dictionary<SkillType, float>
        {
            { SkillType.Dash, 3f + 5f }, // cooldowntime + durationtime
            { SkillType.Fly, 5f + 5f },
            { SkillType.Blink, 7f },
            { SkillType.Purify, 10f + 3f },
            { SkillType.Summon, 30f + 30f },
            { SkillType.AreaSlow, 15f + 5f }
        };

        skillActives = new Dictionary<SkillType, bool>
        {
            { SkillType.Dash, false },
            { SkillType.Fly, false },
            { SkillType.Blink, false },
            { SkillType.Purify, false },
            { SkillType.Summon, false },
            { SkillType.AreaSlow, false}
        };

        manaCosts = new Dictionary<SkillType, int>
        {
            { SkillType.Dash, 10 },
            { SkillType.Fly, 15 },
            { SkillType.Blink, 20 },
            { SkillType.Purify, 25 },
            { SkillType.Summon, 30 },
            { SkillType.AreaSlow, 20}
        };

        currentCooldowns = new Dictionary<SkillType, float>();
        foreach (var skill in cooldownTimes.Keys)
        {
            currentCooldowns[skill] = 0f;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        moveCharacter = GetComponent<MoveCharacter>();
        statusManager = GetComponent<StatusManager>();
        summonedCreaturePrefab = Resources.Load<GameObject>(summonPrefabPath);
        if (summonedCreaturePrefab == null)
        {
            Debug.LogError($"无法加载Prefab: {summonPrefabPath}");
            enabled = false; // 禁用脚本
        }
        // Debug.Log("Here");

        Transform skillUIParent = GameObject.Find("SkillUI").transform;

        foreach (SkillType type in Enum.GetValues(typeof(SkillType)))
        {
            string skillName = type + "Skill";
            Transform skillTransform = skillUIParent.Find(skillName);

            if (skillTransform != null)
            {
                SkillUI ui = new SkillUI();
                ui.skillType = type;
                ui.skillIcon = skillTransform.Find("Icon")?.GetComponent<Image>();
                ui.cooldownOverlay = skillTransform.Find("CooldownOverlay")?.GetComponent<Image>();
                ui.cooldownText = skillTransform.Find("CooldownText")?.GetComponent<TextMeshProUGUI>();

                skillUIDict.Add(type, ui);
            }
        }
        // Debug.Log("Here!!!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ActivateSkill(SkillType.Dash);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            ActivateSkill(SkillType.Fly);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            ActivateSkill(SkillType.Blink);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            ActivateSkill(SkillType.Purify);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivateSkill(SkillType.Summon);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            // Debug.Log("Here!!!");
            ActivateSkill(SkillType.AreaSlow);
        }
        // 更新所有技能的冷却时间
        foreach (var skill in currentCooldowns.Keys.ToList())
        {
            if (currentCooldowns[skill] > 0)
            {
                currentCooldowns[skill] -= Time.deltaTime;
                UpdateSkillUI(skill);
            }
        }
    }

    void ActivateSkill(SkillType skill)
    {
        // 使用字典检查冷却状态
        if (skillActives[skill] || !skillUIDict.ContainsKey(skill) || currentCooldowns[skill] > 0)
            return;
        if(!moveCharacter.IsManaEnough(manaCosts[skill])) { return; }

        currentSkill = skill;
        skillActives[skill] = true;

        switch (skill)
        {
            case SkillType.Dash:
                StartCoroutine(Dash());
                break;
            case SkillType.Fly:
                StartCoroutine(Fly());
                break;
            case SkillType.Blink:
                StartCoroutine(Blink());
                break;
            case SkillType.Purify:
                StartCoroutine(Purify());
                break;
            case SkillType.Summon:
                StartCoroutine(Summon(summonedCreaturePrefab));
                break;
            case SkillType.AreaSlow:
                StartCoroutine(AreaSlow());
                break;
        }
        moveCharacter.UpdateMana(-manaCosts[skill]);

        // 设置冷却时间
        currentCooldowns[skill] = cooldownTimes[skill];
        UpdateSkillUI(skill);
    }

    void UpdateSkillUI(SkillType skill)
    {
        if (skillUIDict.TryGetValue(skill, out SkillUI skillUI))
        {
            float cooldown = currentCooldowns[skill];
            float maxCooldown = cooldownTimes[skill];

            if (cooldown > 0)
            {
                // 冷却状态
                skillUI.cooldownOverlay.fillAmount = cooldown / maxCooldown;
                skillUI.cooldownOverlay.color = new Color(0, 0, 0, 0.7f); // 更透明的覆盖层
                skillUI.cooldownText.text = Mathf.Ceil(cooldown).ToString();
                skillUI.cooldownText.gameObject.SetActive(true);
                skillUI.skillIcon.color = new Color(0.8f, 0.8f, 0.8f); // 轻微变暗
            }
            else
            {
                // 可用状态
                skillUI.cooldownOverlay.gameObject.SetActive(false);
                skillUI.cooldownText.gameObject.SetActive(false);
                skillUI.skillIcon.color = skillActives[skill] ?
                    new Color(0.9f, 0.9f, 0.9f) : // 激活状态轻微变暗
                    Color.white; // 完全可用状态
            }
        }
    }

    IEnumerator Dash()
    {
        float originalSpeed = moveCharacter.movePower;
        moveCharacter.movePower *= 1.5f;

        yield return new WaitForSeconds(5f); // 疾跑持续时间
        moveCharacter.movePower = originalSpeed;
        skillActives[SkillType.Dash] = false;
    }

    IEnumerator Fly()
    {
        moveCharacter.UpdateFly();

        yield return new WaitForSeconds(5f); // 飞行持续时间

        moveCharacter.UpdateFly();
        skillActives[SkillType.Fly] = false;
    }

    IEnumerator Blink()
    {
        float blinkDistance = 5f;
        Vector3 blinkDirection = Vector3.zero;
        blinkDirection = moveCharacter.direction == 1 ? Vector3.right : Vector3.left;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, blinkDirection, blinkDistance, LayerMask.GetMask("Ground"));

        Vector3 blinkTarget;
        if (hit.collider != null)
        {
            blinkTarget = (Vector3)hit.point - blinkDirection * 0.5f; // 0.5f 是安全距离
        }
        else
        {
            blinkTarget = transform.position + blinkDirection * blinkDistance;
        }

        transform.position = blinkTarget;

        yield return null; // 立即结束
        skillActives[SkillType.Blink] = false;
    }
    IEnumerator Purify()
    {
        if (statusManager.isControlled) // 如果当前被控制
        {
            statusManager.isControlled = false; // 解除控制
        }
        statusManager.isImmuneToControl = true;

        yield return new WaitForSeconds(3f); // 免疫持续时间

        statusManager.isImmuneToControl = false;
        skillActives[SkillType.Purify] = false;

    }

    IEnumerator AreaSlow(float slowFactor = 0.5f, float duration = 5f, float radius = 5f)
    {
        // 获取玩家面前区域的所有敌人
        int direction = moveCharacter.direction;
        Vector3 castPosition = transform.position;
        if (direction == 1) castPosition.x += radius / 2;
        else castPosition.x -= radius / 2;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(
            castPosition,
            radius,
            1 << LayerMask.NameToLayer("Enemy")
        );
        Debug.Log(transform.position + ",,," + castPosition);

        // 存储所有被减速的敌人及其原始速度
        List<MoveEnemy> slowedEnemies = new List<MoveEnemy>();
        List<float> originalSpeeds = new List<float>();

        foreach (var hitCollider in hitColliders)
        {
            MoveEnemy enemy = hitCollider.GetComponent<MoveEnemy>();
            if (enemy != null)
            {
                // 保存原始速度并添加减速效果
                originalSpeeds.Add(enemy.moveSpeed);
                slowedEnemies.Add(enemy);

                enemy.moveSpeed *= slowFactor;
            }
        }

        yield return new WaitForSeconds(duration);

        // 恢复所有被减速敌人的速度
        for (int i = 0; i < slowedEnemies.Count; i++)
        {
            if (slowedEnemies[i] != null) // 检查敌人是否还存在
            {
                slowedEnemies[i].moveSpeed = originalSpeeds[i];
            }
        }

        skillActives[SkillType.AreaSlow] = false;
    }

    IEnumerator Summon(GameObject creaturePrefab, float duration = 30f)
    {
        yield return new WaitForSeconds(0.5f); // 召唤施法时间

        // 实例化召唤物
        GameObject summonedCreature = Instantiate(
            creaturePrefab,
            transform.position + transform.forward * 2 + Vector3.up * 0.5f,
            Quaternion.identity
        );

        // 设置召唤物属性
        SummonedCreature creatureScript = summonedCreature.GetComponent<SummonedCreature>();
        creatureScript.Initialize(transform); // 初始化，设置主人

        // 持续时间结束后消失
        yield return new WaitForSeconds(duration);

        if (summonedCreature != null)
        {
            // Instantiate(unsummonEffect, summonedCreature.transform.position, Quaternion.identity);
            Destroy(summonedCreature);
        }

        skillActives[SkillType.Summon] = false;
    }
}
