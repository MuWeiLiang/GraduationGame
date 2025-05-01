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
        public TextMeshProUGUI cooldownText; // 修改为TextMeshProUGUI类型
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
                playerFX.ActiveSword(spawnPosition,direction); // 播放闪烁特效
            }
        }

    }
    public void Tick2()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ActivateSkill(NewSkillType.Purify);
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
    void ActivateSkill(NewSkillType skill)
    {
        // 使用字典检查冷却状态
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

        // 设置冷却时间
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
        float originalSpeed = playerMove.movePower;
        playerMove.movePower *= 1.5f;

        yield return new WaitForSeconds(5f); // 疾跑持续时间
        playerMove.movePower = originalSpeed;
        skillActives[NewSkillType.Dash] = false;
    }

    IEnumerator Blink()
    {
        if(playerFX != null)
        {
            playerFX.ActiveBlink(); // 播放闪烁特效
        }
        onPlayer?.Invoke("blink"); // 播放闪烁音效
        float blinkDistance = 5f;
        Vector3 blinkDirection = Vector3.zero;
        blinkDirection = playerMove.direction == 1 ? Vector3.right : Vector3.left;
        Transform transform = playerMove.GetTransform();

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
        skillActives[NewSkillType.Blink] = false;
    }
    IEnumerator Purify()
    {
        if (playerStatus.isControlled) // 如果当前被控制
        {
            playerStatus.DeactivateControl(); // 解除控制
        }
        playerStatus.isImmuneToControl = true;

        yield return new WaitForSeconds(3f); // 免疫持续时间

        playerStatus.isImmuneToControl = false;
        skillActives[NewSkillType.Purify] = false;

    }
}
