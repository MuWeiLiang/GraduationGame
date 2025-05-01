using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeEnemy : MoveEnemy
{
    private int damageNum = 40; // 伤害数值
    private float lastAttackTime = 0f; // 记录上次攻击时间
    private float attackCooldown = 1f; // 攻击冷却时间（1秒）
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start(); // 调用基类的 Start 方法
        moveRange = 5f;
        moveSpeed = 3f;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SPlayerController player = collision.gameObject.GetComponent<SPlayerController>();
            int direction = GetFaceDir();
            if (player != null && (Time.time - lastAttackTime) >= attackCooldown)
            {
                player.GetHurt(direction, damageNum);
                lastAttackTime = Time.time; // 更新上次攻击时间
            }
        }
    }
}
