using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonedCreature : MonoBehaviour
{
    [Header("基础设置")]
    public float moveSpeed = 3f;          // 移动速度
    public float attackRange = 2f;        // 攻击范围
    public float attackCooldown = 1f;     // 攻击冷却时间
    public int damage = 10;               // 攻击伤害

    [Header("跟随设置")]
    public float followDistance = 2f;     // 与主人的跟随距离
    public float tooCloseDistance = 0.5f;   // 太近的距离

    public Transform master;             // 主人(召唤者)

    private int direction = 1;
    private float localScaleNum;

    // 初始化召唤物
    public void Initialize(Transform masterTransform)
    {
        master = masterTransform;
    }

    void Start()
    {
        // rb = GetComponent<Rigidbody>();
        localScaleNum = transform.localScale.x;
    }

    void Update()
    {
        if (master == null)
        {
            // 主人不存在，自我销毁
            Destroy(gameObject);
            return;
        }

        FollowMaster();
    }

    // 跟随主人
    private void FollowMaster()
    {
        float distanceToMaster = Vector3.Distance(transform.position, master.position);
        if (distanceToMaster > 10f)
        {
            transform.position = master.position; // 直接传送到玩家位置
            return; // 闪现后不再执行后续逻辑
        }
        if (distanceToMaster > followDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, master.position, moveSpeed * Time.deltaTime);
        }
        else if (distanceToMaster < tooCloseDistance)
        {
            Vector3 retreatDirection = (transform.position - master.position).normalized;

            Vector3 retreatTarget = master.position + retreatDirection * tooCloseDistance;

            transform.position = Vector2.MoveTowards(
                transform.position,
                retreatTarget,
                moveSpeed * Time.deltaTime
            );
        }
        int newDirection = master.position.x - transform.position.x >= 0 ? 1 : -1;
        if (newDirection * direction < 0) {
            transform.localScale = new Vector3(newDirection * localScaleNum, localScaleNum, localScaleNum);
        }
        direction = newDirection;
    }
}
