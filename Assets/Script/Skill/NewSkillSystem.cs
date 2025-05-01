using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class NewSkillSystem : MonoBehaviour
{
    public enum NewSkillType { None, Dash, Blink, Purify };
    [System.Serializable]
    public class SkillUI
    {
        public NewSkillType NewSkillType;
        public Image skillIcon;
        public Image cooldownOverlay;
        public TextMeshProUGUI cooldownText; // �޸�ΪTextMeshProUGUI����
    }
    public NewSkillType currentSkill = NewSkillType.None;
    private MoveCharacter moveCharacter;
    private StatusManager statusManager;
    private Dictionary<NewSkillType, bool> skillActives;
    private Dictionary<NewSkillType, float> cooldownTimes;
    private Dictionary<NewSkillType, float> currentCooldowns;
    private Dictionary<NewSkillType, int> manaCosts;
    private Dictionary<NewSkillType, SkillUI> skillUIDict = new Dictionary<NewSkillType, SkillUI>();
    private void Awake()
    {
        // Debug.Log("NewSkillSystem Awake");
        cooldownTimes = new Dictionary<NewSkillType, float>
        {
            { NewSkillType.Dash, 3f + 5f }, // cooldowntime + durationtime
            { NewSkillType.Blink, 7f },
            { NewSkillType.Purify, 10f + 3f },
        };

        skillActives = new Dictionary<NewSkillType, bool>
        {
            { NewSkillType.Dash, false },
            { NewSkillType.Blink, false },
            { NewSkillType.Purify, false },
        };

        manaCosts = new Dictionary<NewSkillType, int>
        {
            { NewSkillType.Dash, 10 },
            { NewSkillType.Blink, 20 },
            { NewSkillType.Purify, 25 },
        };

        currentCooldowns = new Dictionary<NewSkillType, float>();
        foreach (var skill in cooldownTimes.Keys)
        {
            currentCooldowns[skill] = 0f;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("NewSkillSystem Start");
        moveCharacter = GetComponent<MoveCharacter>();
        statusManager = GetComponent<StatusManager>();

        Transform skillUIParent = GameObject.Find("SkillUI").transform;

        if (skillUIParent == null)
        {
            Debug.Log("SkillUI parent not found!");
            return;
        }

        foreach (NewSkillType type in Enum.GetValues(typeof(NewSkillType)))
        {
            string skillName = type + "Skill";
            Transform skillTransform = skillUIParent.Find(skillName);

            if (skillTransform != null)
            {
                SkillUI ui = new SkillUI();
                ui.NewSkillType = type;
                ui.skillIcon = skillTransform.Find("Icon")?.GetComponent<Image>();
                ui.cooldownOverlay = skillTransform.Find("CooldownOverlay")?.GetComponent<Image>();
                ui.cooldownText = skillTransform.Find("CooldownText")?.GetComponent<TextMeshProUGUI>();

                skillUIDict.Add(type, ui);
            }
        }
    }

    void Update()
    {
        if (!statusManager.isControlled)
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                ActivateSkill(NewSkillType.Dash);
            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                ActivateSkill(NewSkillType.Blink);
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            ActivateSkill(NewSkillType.Purify);
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

    void ActivateSkill(NewSkillType skill)
    {
        // ʹ���ֵ�����ȴ״̬
        if (skillActives[skill] || !skillUIDict.ContainsKey(skill) || currentCooldowns[skill] > 0)
            return;
        if (!moveCharacter.IsManaEnough(manaCosts[skill])) { return; }

        currentSkill = skill;
        skillActives[skill] = true;

        switch (skill)
        {
            case NewSkillType.Dash:
                StartCoroutine(Dash());
                break;
            case NewSkillType.Blink:
                StartCoroutine(Blink());
                break;
            case NewSkillType.Purify:
                StartCoroutine(Purify());
                break;
        }
        moveCharacter.UpdateMana(-manaCosts[skill]);

        // ������ȴʱ��
        currentCooldowns[skill] = cooldownTimes[skill];
        UpdateSkillUI(skill);
    }

    void UpdateSkillUI(NewSkillType skill)
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
        skillActives[NewSkillType.Dash] = false;
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
        skillActives[NewSkillType.Blink] = false;
    }
    IEnumerator Purify()
    {
        if (statusManager.isControlled) // �����ǰ������
        {
            statusManager.DeactivateControl(); // �������
        }
        statusManager.isImmuneToControl = true;

        yield return new WaitForSeconds(3f); // ���߳���ʱ��

        statusManager.isImmuneToControl = false;
        skillActives[NewSkillType.Purify] = false;

    }
}
