using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SPlayerController;

public class PlayerSkill
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
    private PlayerSoundEvent onPlayer;
    private PlayerMove playerMove;
    private PlayerBase playerBase;
    private PlayerStatus playerStatus;
    private MonoBehaviour _monoBehaviour;
    private PlayerFX playerFX;
    private Dictionary<NewSkillType, bool> skillActives;
    private Dictionary<NewSkillType, float> cooldownTimes;
    private Dictionary<NewSkillType, float> currentCooldowns;
    private Dictionary<NewSkillType, int> manaCosts;
    private Dictionary<NewSkillType, SkillUI> skillUIDict = new Dictionary<NewSkillType, SkillUI>();
    public void Initialize(PlayerBase player,PlayerMove playerMove,PlayerStatus playerStatus,MonoBehaviour monoBehaviour, PlayerSoundEvent onPlayer)
    {
        // Initialize player skill related variables or states here
        playerBase = player;
        this.playerMove = playerMove;
        this.playerStatus = playerStatus;
        _monoBehaviour = monoBehaviour;
        playerFX = _monoBehaviour.GetComponentInChildren<PlayerFX>();
        this.onPlayer = onPlayer;

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
        // Initialize skill UI elements
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
    public void Tick1()
    {
        // Update player skill related logic here
        if (Input.GetKeyDown(KeyCode.U))
        {
            ActivateSkill(NewSkillType.Dash);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            ActivateSkill(NewSkillType.Blink);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            // ActivateSkill(NewSkillType.Purify);
            if(playerFX != null)
            {
                Vector3 spawnPosition = _monoBehaviour.transform.position;
                spawnPosition.y += 1.8f; // Adjust the spawn position if needed
                int direction = playerMove.direction;
                spawnPosition.x += direction > 0 ? 6f : -6f;
                playerFX.ActiveSword(spawnPosition,direction); // ������˸��Ч
            }
        }

    }
    public void Tick2()
    {
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
        if (!playerBase.IsManaEnough(manaCosts[skill])) { return; }

        currentSkill = skill;
        skillActives[skill] = true;

        switch (skill)
        {
            case NewSkillType.Dash:
                _monoBehaviour.StartCoroutine(Dash());
                break;
            case NewSkillType.Blink:
                _monoBehaviour.StartCoroutine(Blink());
                break;
            case NewSkillType.Purify:
                _monoBehaviour.StartCoroutine(Purify());
                break;
        }
        playerBase.UpdateMana(-manaCosts[skill]);

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
        float originalSpeed = playerMove.movePower;
        playerMove.movePower *= 1.5f;

        yield return new WaitForSeconds(5f); // ���ܳ���ʱ��
        playerMove.movePower = originalSpeed;
        skillActives[NewSkillType.Dash] = false;
    }

    IEnumerator Blink()
    {
        if(playerFX != null)
        {
            playerFX.ActiveBlink(); // ������˸��Ч
        }
        onPlayer?.Invoke("blink"); // ������˸��Ч
        float blinkDistance = 5f;
        Vector3 blinkDirection = Vector3.zero;
        blinkDirection = playerMove.direction == 1 ? Vector3.right : Vector3.left;
        Transform transform = playerMove.GetTransform();

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
        if (playerStatus.isControlled) // �����ǰ������
        {
            playerStatus.DeactivateControl(); // �������
        }
        playerStatus.isImmuneToControl = true;

        yield return new WaitForSeconds(3f); // ���߳���ʱ��

        playerStatus.isImmuneToControl = false;
        skillActives[NewSkillType.Purify] = false;

    }
}
