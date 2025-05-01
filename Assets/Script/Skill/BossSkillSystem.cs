using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class BossSkillSystem : MonoBehaviour
{
    public enum BossSkillType { None, Rampage, Stone, AreaDamage };
    public BossSkillType currentSkill = BossSkillType.None;
    private Boss1 bossController;
    private Dictionary<BossSkillType, bool> skillActives;
    private Dictionary<BossSkillType, float> cooldownTimes;
    private Dictionary<BossSkillType, float> currentCooldowns;
    private Dictionary<BossSkillType, int> manaCosts;
    private BossFX bossFX;
    private float stoneRadius = 5f;
    private bool usingSkill = false; // �Ƿ�����ʹ�ü���

    private void Awake()
    {
        cooldownTimes = new Dictionary<BossSkillType, float>
        {
            { BossSkillType.Rampage, 15f + 15f }, // cooldowntime + durationtime
            { BossSkillType.Stone, 10f + 5f },
            { BossSkillType.AreaDamage, 20f + 0.5f },
        };

        skillActives = new Dictionary<BossSkillType, bool>
        {
            { BossSkillType.Rampage, false },
            { BossSkillType.Stone, false },
            { BossSkillType.AreaDamage, false },
        };

        manaCosts = new Dictionary<BossSkillType, int>
        {
            { BossSkillType.Rampage, 10 },
            { BossSkillType.Stone, 20 },
            { BossSkillType.AreaDamage, 25 },
        };

        currentCooldowns = new Dictionary<BossSkillType, float>();
        foreach (var skill in cooldownTimes.Keys)
        {
            currentCooldowns[skill] = 0f;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        bossController = GetComponent<Boss1>();
        bossFX = GetComponentInChildren<BossFX>();
        // statusManager = GetComponent<StatusManager>();
    }

    void Update()
    {
        Tick(); // ����״̬
        // usingSkill = false; // ���ü���ʹ��״̬
        float currentDistance = bossController.DistanceToPlayer;
        // Debug.Log("currentDistance: " + currentDistance);
        if (Input.GetKeyDown(KeyCode.Q) || currentDistance < 3f)
        {
            if(Time.time > 15f)
                ActivateSkill(BossSkillType.Rampage);
        }
        if (Input.GetKeyDown(KeyCode.E) || currentDistance < 7f)
        {
            if(Time.time > 30f)
                ActivateSkill(BossSkillType.AreaDamage);
        }
        //else if (Input.GetKeyDown(KeyCode.W) || currentDistance < 4f)
        //{
        //    ActivateSkill(BossSkillType.Stone);
        //}
        
        // �������м��ܵ���ȴʱ��
        foreach (var skill in currentCooldowns.Keys.ToList())
        {
            if (currentCooldowns[skill] > 0)
            {
                currentCooldowns[skill] -= Time.deltaTime;
            }
        }
    }

    void Tick()
    {
        float currentDistance = bossController.DistanceToPlayer;
        if(currentDistance >= 30f)
        {
            bossController.SetBonusMoveSpeed(3f);
        }
        if(currentDistance < 30f && currentDistance >= 15f)
        {
            bossController.SetBonusMoveSpeed(1.5f);
        }
        if(currentDistance < 15f)
        {
            bossController.SetBonusMoveSpeed(0f);
        }
    }

    void ActivateSkill(BossSkillType skill)
    {
        // ʹ���ֵ�����ȴ״̬
        if (skillActives[skill] || currentCooldowns[skill] > 0)
            return;
        if (!bossController.IsManaEnough(manaCosts[skill])) { return; }

        currentSkill = skill;
        skillActives[skill] = true;

        switch (skill)
        {
            case BossSkillType.Rampage:
                StartCoroutine(Rampage());
                break;
            case BossSkillType.Stone:
                StartCoroutine(Stone());
                break;
            case BossSkillType.AreaDamage:
                StartCoroutine(AreaDamage());
                break;
        }
        bossController.UpdateMana(-manaCosts[skill]);

        // ������ȴʱ��
        currentCooldowns[skill] = cooldownTimes[skill];
    }   

    IEnumerator Rampage()
    {
        if(bossFX != null)
        {
            bossFX.ActiveRampage(); // ���ż�����Ч
        }

        int originalAttackDamage = bossController.AttackDamage;
        bossController.AttackDamage *= 2;
        bossController.SetBaseMoveSpeed(6f); // �����ٶ�

        yield return new WaitForSeconds(15f); // �񱩳���ʱ��
        bossController.AttackDamage = originalAttackDamage;
        bossController.SetBaseMoveSpeed(3f); // �ָ��ٶ�
        skillActives[BossSkillType.Rampage] = false;
    }

    IEnumerator Stone()
    {
        int direction = transform.localScale.x > 0 ? 1 : -1;
        Vector3 castPosition = transform.position;
        if (direction == 1) castPosition.x += stoneRadius / 2;
        else castPosition.x -= stoneRadius / 2;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(
            castPosition,
            stoneRadius,
            1 << LayerMask.NameToLayer("Player")
        );
        List<StatusManager> stonedPlayers = new List<StatusManager>();
        foreach (var hitCollider in hitColliders)
        {
            StatusManager player = hitCollider.GetComponent<StatusManager>();
            if (player != null)
            {
                if(player.isImmuneToControl) continue; // ���������߿���������
                stonedPlayers.Add(player);
                // player.isStoned = true; // ʯ��״̬
                player.ActivateStone(5f); // ����ʯ��״̬
            }
        }

        yield return new WaitForSeconds(5f); // 5s����

        for (int i = 0; i < stonedPlayers.Count; i++)
        {
            if (stonedPlayers[i] != null) // �������Ƿ񻹴���
            {
                stonedPlayers[i].DeactivateStone(); // ���ʯ��״̬
            }
        }
        skillActives[BossSkillType.Stone] = false;
    }
    IEnumerator AreaDamage()
    {
        // Debug.Log("AreaDamage");
        yield return new WaitForSeconds(0.5f); // �ȴ�0.5��
        if(bossFX != null)
        {
            Vector3 spawnPostion = bossController.playerTarget.position;
            Vector3 direction = spawnPostion - transform.position;
            if(direction.magnitude > 10f)
            {
                spawnPostion = transform.position + direction.normalized * 10f; // ���Ʒ�Χ
            }
            bossFX.ActiveAreaDamage(spawnPostion, bossController.AttackDamage/2); // ���ż�����Ч
        }               

        yield return null; // 5s����
        skillActives[BossSkillType.AreaDamage] = false;
    }
}
