using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss1 : BaseEnemy
{
    private int maxHealth = 300;
    private int maxMana = 100;
    public int AttackDamage = 20;
    [Header("移动设置")]
    [SerializeField] private float moveSpeed = 3f; // 移动速度
    [SerializeField] private float stoppingDistance = 2f; // 停止距离

    private float baseMoveSpeed = 3f; // 基础移动速度
    private float bonusMoveSpeed = 0f; // 额外移动速度
    public float DistanceToPlayer { get; private set; } // 玩家距离

    private float detectionRange = 50f; // 极限距离
    private float attackDistance = 2f; // 攻击距离
    private float minSafeDistance = 1f; // 安全距离
    private bool attackCooldown = false; // 攻击冷却
    private bool isAttacking = false;
    private bool canAttack = true;
    private float YOffset = 0.3f; // Y轴偏移量
    public Transform playerTarget; // 玩家目标
    private Rigidbody2D rb; // 刚体组件
    private Animator animator; // 用于控制动画
    private SPlayerController playerController; // 状态管理器

    void Start()
    {
        // 查找带有"Player"标签的玩家
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

        // 获取刚体组件
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = maxHealth; // 设置血量
        mana = maxMana; // 设置蓝量
    }

    void Update()
    {
        moveSpeed = baseMoveSpeed + bonusMoveSpeed; // 更新移动速度
        // Debug.Log("moveSpeed: " + moveSpeed);
        if (playerTarget == null) return;
        if (playerController.IsCloaked()) return;
        Vector3 direction = playerTarget.position - transform.position;
        direction.y += 0.5f; // 调整y轴高度 boss
        direction.y += 1f; // 调整y轴高度 玩家
        // Debug.Log("direction " + direction);
        float distanceToPlayer = direction.magnitude;
        DistanceToPlayer = distanceToPlayer; // 更新玩家距离
        if (distanceToPlayer > detectionRange)
        {
            // 如果玩家超出检测范围，停止移动
            rb.velocity = Vector3.zero;
            return;
        }
        if (isAttacking) return;
        // 平滑转向玩家
        if (direction.x * transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        if (distanceToPlayer <= attackDistance) 
        {
            if (distanceToPlayer < minSafeDistance)
            {
                direction.x = 1f;
                MoveToTarget(direction);
            } 
            if(Mathf.Abs(direction.y) >= YOffset)
            {
                direction.x = 0;
                MoveToTarget(direction);
            }
            Attack();         
        } 
        else
        {
            ResetAnimation();
            MoveToTarget(direction);
        }       
    }

    void MoveToTarget(Vector3 direction)
    {        
        if (true)
        {
            Vector3 movement = direction.normalized * moveSpeed * Time.deltaTime;
            transform.position += movement;
        }
    }
    void Attack()
    {
        if (attackCooldown)
            return;
        attackCooldown = true;
        // Debug.Log("攻击动画");
        isAttacking = true;
        animator.SetBool("isAttacking", true);
        Invoke("ResetIsAttack", 0.667f); // 攻击动画持续时间
        StartCoroutine(AttackCooldown());
    }
    private void ResetIsAttack()
    {
        isAttacking = false;
    }
    IEnumerator AttackCooldown()
    {
        // 等待额外的冷却时间
        yield return new WaitForSeconds(1f);

        // 重置冷却状态
        attackCooldown = false;
    }
    void ResetAnimation()
    {
        animator.SetBool("isAttacking", false);
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (!canAttack) return;
    //    int direction = transform.localScale.x > 0 ? 1 : -1;
    //    if (collision.CompareTag("Player"))
    //    {
    //        collision.GetComponent<SPlayerController>().GetHurt(direction, AttackDamage);
    //        canAttack = false;
    //        Invoke(nameof(ResetAttack), 0.667f);
    //    }
    //}
    //private void ResetAttack()
    //{
    //    canAttack = true;
    //}
    public void SetBaseMoveSpeed(float speed)
    {
        baseMoveSpeed = speed;
    }
    public void SetBonusMoveSpeed(float speed)
    {
        bonusMoveSpeed = speed;
    }

    public bool IsManaEnough(int manaCost)
    {
        return mana >= manaCost;
    }
    public void UpdateMana(int manaCost)
    {
        mana += manaCost;
    }
}
