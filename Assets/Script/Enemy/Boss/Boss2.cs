using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : BaseEnemy
{
    //int health = 100; // 血量
    int maxHealth = 80; // 最大血量
    //int mana = 100; // 魔法值
    int maxMana = 100; // 最大魔法值
    [Header("移动设置")]
    [SerializeField] private float moveSpeed = 3f; // 移动速度
    private EnemyAttack enemyAttack;

    private float baseMoveSpeed = 3f; // 基础移动速度
    private float bonusMoveSpeed = 0f; // 额外移动速度
    private float speedRatio = 0f;
    bool isWalking = false, isRunning = false, isIdle = true;
    public float DistanceToPlayer { get; private set; } // 玩家距离

    private float detectionRange = 50f; // 极限距离
    private float attackDistance = 2f; // 攻击距离
    float magicDistance = 5f;
    private bool attackCooldown = false; // 攻击冷却
    bool magicCooldown = false;
    bool isAttacking = false;
    bool isMagicing = false;

    bool skill1Cooldown = false; // 技能1冷却
    bool skill2Cooldown = false; // 技能2冷却
    private Transform playerTarget; // 玩家目标
    private Rigidbody2D rb; // 刚体组件
    private Animator animator; // 用于控制动画
    private SPlayerController playerController; // 状态管理器
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTarget = player.transform;
            playerController = player.GetComponent<SPlayerController>();
        }
        else
        {
            Debug.LogWarning("场景中没有找到带有'Player'标签的对象");
        }

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyAttack = GetComponentInChildren<EnemyAttack>();

        mana = maxMana; // 初始化魔法值
        health = maxHealth; // 初始化血量
        damagePopupSystem = FindObjectOfType<DamagePopupSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        moveSpeed = baseMoveSpeed + bonusMoveSpeed; // 更新移动速度
        moveSpeed *= (1- speedRatio);
        if (playerTarget == null) return;
        if (playerController.IsCloaked()) return;
        Vector3 direction = playerTarget.position - transform.position;
        direction = UpdateDir(direction);
        DistanceToPlayer = direction.magnitude; // 更新玩家距离
        if (DistanceToPlayer > detectionRange)
        {
            // 如果玩家超出检测范围，停止移动
            rb.velocity = Vector3.zero;
            return;
        }
        var t = (playerTarget.position - transform.position).x * transform.localScale.x;
        if (t < -0.1f)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        SwitchAni(DistanceToPlayer);
        MoveToPlayer(direction);
        if (DistanceToPlayer <= magicDistance && !magicCooldown && !isAttacking && Time.time >= 15f)
        {
            if (mana >= 10)
                Magic();
        }
        if (DistanceToPlayer <= attackDistance && !attackCooldown && !isMagicing)
        {
            Attack();
        }
        if (DistanceToPlayer <= 4f && !skill1Cooldown && Time.time >= 30f)
        {
            if (mana >= 15)
            {
                Vector3 spawnPosition = playerTarget.position;
                spawnPosition.y += 0.5f; // 调整y轴位置
                ActiveSkill1(spawnPosition, 30);
            }

        }
        //if (DistanceToPlayer <= 8f && !skill2Cooldown)
        //{
        //    if (mana >= 20)
        //    {
        //        Vector3 spawnPosition = playerTarget.position;
        //        ActiveSkill2(spawnPosition, 20);
        //    }
        //}
    }
    void SetAniStatus(bool isIdle, bool isWalking, bool isRunning)
    {
        this.isIdle = isIdle;
        this.isWalking = isWalking;
        this.isRunning = isRunning;
    }
    void SwitchAni(float distance)
    {
        if (distance < 10f && !isIdle)
        {
            animator.SetTrigger("idle");
            SetAniStatus(true, false, false);
        }
        if (distance < 20f && distance >= 10f && !isWalking)
        {
            animator.SetTrigger("walk");
            SetAniStatus(false, true, false);
        }
        if (!isRunning && distance >= 20f)
        {
            animator.SetTrigger("run");
            SetAniStatus(false, false, true);
        }
    }
    Vector3 UpdateDir(Vector3 direction)
    {
        direction.y += 1.6f;
        if (Mathf.Abs(direction.x) > 0.7f)
        {
            if (direction.x < 0)
                direction.x += 0.7f; // 调整x轴位置
            else
                direction.x -= 0.7f; // 调整x轴位置
        }
        else
        {
            if (direction.x < 0)
                direction.x = 0.7f + direction.x; // 调整x轴位置
            else
                direction.x = direction.x - 0.7f; // 调整x轴位置
        }
        return direction;
    }
    void MoveToPlayer(Vector3 direction)
    {
        Vector3 movement = direction.normalized * moveSpeed * Time.deltaTime;
        transform.position += movement;
    }
    void Attack()
    {
        if (attackCooldown)
            return;
        attackCooldown = true;
        isAttacking = true;
        int x = Random.Range(1, 3);
        animator.SetTrigger("attack" + x);
        //Debug.Log("攻击动画");
        Invoke("ResetAttackCooldown", 1.5f);
        Invoke("ResetIsAttack", 1f);
    }
    private void ResetAttackCooldown()
    {
        attackCooldown = false;
    }
    void ResetIsAttack()
    {
        isAttacking = false;
    }
    void Magic()
    {
        if (magicCooldown)
            return;
        magicCooldown = true;
        isMagicing = true;
        int x = Random.Range(1, 3);
        enemyAttack.damage = 30;
        animator.SetTrigger("magic" + x);
        mana -= 10; // 扣除魔法值
        //Debug.Log("攻击动画");
        Invoke("ResetMagicCooldown", 5f);
        Invoke("ResetIsMagic", 1.5f);
    }
    private void ResetMagicCooldown()
    {
        magicCooldown = false;
    }
    void ResetIsMagic()
    {
        isMagicing = false;
        enemyAttack.damage = 20;
    }
    public void ActiveSkill1(Vector3 spawnPosition, int damage)
    {
        GameObject fx2Prefab = Resources.Load<GameObject>("Prefab/FX/fx21");
        if (fx2Prefab != null)
        {
            GameObject fx2Instance = Instantiate(fx2Prefab, spawnPosition, Quaternion.identity);
            fx2Instance.GetComponent<FX>().SetDamage(damage); // 设置伤害值
            skill1Cooldown = true; // 设置技能冷却
            mana -= 15; // 扣除魔法值
            Invoke("ResetSkill1Cooldown", 10f); // 5秒后重置技能冷却

            Destroy(fx2Instance, 1.5f); // 3秒后自动销毁
        }
        else
        {
            Debug.LogError("FX2 prefab is not assigned!");
        }
    }
    private void ResetSkill1Cooldown()
    {
        skill1Cooldown = false;
    }
    public void ActiveSkill2(Vector3 spawnPosition, int damage)
    {
        GameObject fx2Prefab = Resources.Load<GameObject>("Prefab/FX/fx22");
        if (fx2Prefab != null)
        {
            GameObject fx2Instance = Instantiate(fx2Prefab, spawnPosition, Quaternion.identity);
            fx2Instance.GetComponent<FX>().SetDamage(damage); // 设置伤害值
            skill2Cooldown = true; // 设置技能冷却
            mana -= 20; // 扣除魔法值
            Invoke("ResetSkill1Cooldown", 30f); // 5秒后重置技能冷却

            Destroy(fx2Instance, 3f); // 3秒后自动销毁
        }
        else
        {
            Debug.LogError("FX2 prefab is not assigned!");
        }
    }
    private void ResetSkill2Cooldown()
    {
        skill2Cooldown = false;
    }

    public override void SlowSpeed(float ratio, float duration)
    {
        speedRatio = ratio;
        Invoke("ResetSpeedRatio", duration);
    }
    void ResetSpeedRatio()
    {
        speedRatio = 0;
    }

    public override Vector3 GetShowPosition()
    {
        Vector3 postion = base.GetShowPosition();
        int dir = transform.localScale.x > 0 ? 1 : -1;
        postion.x -= 0.35f * dir;
        postion.y += 0.4f;
        return postion;
    }
}
