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
        public TextMeshProUGUI cooldownText; // �޸�ΪTextMeshProUGUI����
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
            Debug.LogError($"�޷�����Prefab: {summonPrefabPath}");
            enabled = false; // ���ýű�
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
        // �������м��ܵ���ȴʱ��
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
        // ʹ���ֵ�����ȴ״̬
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

        // ������ȴʱ��
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
                // ��ȴ״̬
                skillUI.cooldownOverlay.fillAmount = cooldown / maxCooldown;
                skillUI.cooldownOverlay.color = new Color(0, 0, 0, 0.7f); // ��͸���ĸ��ǲ�
                skillUI.cooldownText.text = Mathf.Ceil(cooldown).ToString();
                skillUI.cooldownText.gameObject.SetActive(true);
                skillUI.skillIcon.color = new Color(0.8f, 0.8f, 0.8f); // ��΢�䰵
            }
            else
            {
                // ����״̬
                skillUI.cooldownOverlay.gameObject.SetActive(false);
                skillUI.cooldownText.gameObject.SetActive(false);
                skillUI.skillIcon.color = skillActives[skill] ?
                    new Color(0.9f, 0.9f, 0.9f) : // ����״̬��΢�䰵
                    Color.white; // ��ȫ����״̬
            }
        }
    }

    IEnumerator Dash()
    {
        float originalSpeed = moveCharacter.movePower;
        moveCharacter.movePower *= 1.5f;

        yield return new WaitForSeconds(5f); // ���ܳ���ʱ��
        moveCharacter.movePower = originalSpeed;
        skillActives[SkillType.Dash] = false;
    }

    IEnumerator Fly()
    {
        moveCharacter.UpdateFly();

        yield return new WaitForSeconds(5f); // ���г���ʱ��

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
            blinkTarget = (Vector3)hit.point - blinkDirection * 0.5f; // 0.5f �ǰ�ȫ����
        }
        else
        {
            blinkTarget = transform.position + blinkDirection * blinkDistance;
        }

        transform.position = blinkTarget;

        yield return null; // ��������
        skillActives[SkillType.Blink] = false;
    }
    IEnumerator Purify()
    {
        if (statusManager.isControlled) // �����ǰ������
        {
            statusManager.isControlled = false; // �������
        }
        statusManager.isImmuneToControl = true;

        yield return new WaitForSeconds(3f); // ���߳���ʱ��

        statusManager.isImmuneToControl = false;
        skillActives[SkillType.Purify] = false;

    }

    IEnumerator AreaSlow(float slowFactor = 0.5f, float duration = 5f, float radius = 5f)
    {
        // ��ȡ�����ǰ��������е���
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

        // �洢���б����ٵĵ��˼���ԭʼ�ٶ�
        List<MoveEnemy> slowedEnemies = new List<MoveEnemy>();
        List<float> originalSpeeds = new List<float>();

        foreach (var hitCollider in hitColliders)
        {
            MoveEnemy enemy = hitCollider.GetComponent<MoveEnemy>();
            if (enemy != null)
            {
                // ����ԭʼ�ٶȲ���Ӽ���Ч��
                originalSpeeds.Add(enemy.moveSpeed);
                slowedEnemies.Add(enemy);

                enemy.moveSpeed *= slowFactor;
            }
        }

        yield return new WaitForSeconds(duration);

        // �ָ����б����ٵ��˵��ٶ�
        for (int i = 0; i < slowedEnemies.Count; i++)
        {
            if (slowedEnemies[i] != null) // �������Ƿ񻹴���
            {
                slowedEnemies[i].moveSpeed = originalSpeeds[i];
            }
        }

        skillActives[SkillType.AreaSlow] = false;
    }

    IEnumerator Summon(GameObject creaturePrefab, float duration = 30f)
    {
        yield return new WaitForSeconds(0.5f); // �ٻ�ʩ��ʱ��

        // ʵ�����ٻ���
        GameObject summonedCreature = Instantiate(
            creaturePrefab,
            transform.position + transform.forward * 2 + Vector3.up * 0.5f,
            Quaternion.identity
        );

        // �����ٻ�������
        SummonedCreature creatureScript = summonedCreature.GetComponent<SummonedCreature>();
        creatureScript.Initialize(transform); // ��ʼ������������

        // ����ʱ���������ʧ
        yield return new WaitForSeconds(duration);

        if (summonedCreature != null)
        {
            // Instantiate(unsummonEffect, summonedCreature.transform.position, Quaternion.identity);
            Destroy(summonedCreature);
        }

        skillActives[SkillType.Summon] = false;
    }
}
