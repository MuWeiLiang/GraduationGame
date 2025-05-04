using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperEnemy : RandomMove
{
    private Animator animator;

    private int damage = 20; // 伤害

    private bool haveWeapon = false; // 是否有武器
    // 攻击冷却时间相关变量
    private float attackCooldown = 1f; // 攻击间隔时间
    private float lastAttackTime = -1f; // 上次攻击时间
    // 追击玩家
    public bool isChasing = false; // 是否正在追逐玩家
    public bool playerInRange = false;
    public GameObject player; // 玩家位置
    private Vector3 playerPosition; // 玩家位置
    private float chaseSpeed = 3f; // 追逐速度
    private float distanceToPlayer; // 敌人和玩家之间的距离

    private bool isDie = false; // 是否死亡


    void Start()
    {
        // 获取Animator组件
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on this GameObject.");
        }
        initialPosition = transform.position;
        StartMove();
        // player = GameObject.FindGameObjectWithTag("Player");
        health = 50; // 初始化血量

        damagePopupSystem = FindObjectOfType<DamagePopupSystem>();
    }

    void Update()
    {
        if(isDie) return; // 如果死亡则不执行后续代码
        if (player != null)
        {
            playerPosition = player.transform.position; // 获取玩家位置
            playerPosition.y += 0.6f; // 调整y轴位置
            float X = playerPosition.x - transform.position.x;
            if (Mathf.Abs(X) > 0.7f)
            {
                if (X < 0)
                    playerPosition.x += 0.7f; // 调整x轴位置
                else
                    playerPosition.x -= 0.7f; // 调整x轴位置
            } else
            {
                playerPosition.x = transform.position.x; // 调整x轴位置
            }

            distanceToPlayer = Vector2.Distance(transform.position, playerPosition); // 计算敌人和玩家之间的距离
        }
        ChasePlayer(); // 检查是否追逐玩家

        if (isChasing)
        {
            MoveToPlayer();
            if (!haveWeapon && distanceToPlayer < 5f) // 如果敌人和玩家之间的距离小于1.5f
            {
                HaveWeapon();
            }
            else if (haveWeapon && distanceToPlayer > 7f) // 如果敌人和玩家之间的距离小于1.5f
            {
                RemoveWeapon();
            }
            if (distanceToPlayer < 1f) // 如果敌人和玩家之间的距离小于1.5f
            {
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    lastAttackTime = Time.time;
                    Attack();
                }
            }
        }
        else
        {
            Tick();
        }
    }

    void MoveToPlayer()
    {
        if (playerPosition == null) return;

        // 计算到玩家的方向
        Vector2 direction = (playerPosition - transform.position).normalized;

        // 移动敌人朝向玩家
        transform.position = Vector2.MoveTowards(
            transform.position,
            playerPosition,
            chaseSpeed * Time.deltaTime
        );

        // 更新朝向
        UpdateFacingDirection(playerPosition);
    }

    void ChasePlayer()
    {
        IsInRange(playerPosition.x, playerPosition.y); // 检查玩家是否在攻击范围内
        if (playerInRange && !isChasing)
        {
            StartChasing();
        }
        else if (!playerInRange && isChasing)
        {
            StopChasing();
        }
    }

    void StartChasing()
    {
        isChasing = true;

        // 停止随机移动
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
        isMoving = false;
    }

    // 停止追逐，恢复随机移动
    void StopChasing()
    {
        isChasing = false;

        // 重新开始随机移动
        StartMove();
    }


    void AnimTest()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                lastAttackTime = Time.time;
                Attack();
                Debug.Log("Attack");
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            HaveWeapon();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            RemoveWeapon();
        }
    }

    void Attack()
    {
        animator.SetTrigger("attack");
    }
    void Idle()
    {
        animator.SetTrigger("idle");
    }
    void HaveWeapon()
    {
        if (haveWeapon)
        {
            return;
        }
        animator.SetBool("haveWeapon", true);
        haveWeapon = true;
        Invoke("Idle", 1f);
    }
    void RemoveWeapon()
    {
        if (!haveWeapon)
        {
            return;
        }
        animator.SetBool("haveWeapon", false);
        haveWeapon = false;
        Invoke("Idle", 0.5f);
    }

    void IsInRange(float x, float y)
    {
        if (x >= initialPosition.x - xOffset && x <= initialPosition.x + xOffset &&
           y >= initialPosition.y - yOffset && y <= initialPosition.y + yOffset)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        Hurt();
    }
    void Hurt()
    {
        animator.SetTrigger("hurt");
    }
    public override void Die()
    {
        // animator.SetTrigger("death");
        isDie = true;
        Destroy(gameObject, 0.5f);
    }
}
