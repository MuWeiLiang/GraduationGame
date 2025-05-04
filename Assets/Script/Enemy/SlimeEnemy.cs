using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeEnemy : MoveEnemy
{
    private int damageNum = 20; // 伤害数值
    private float lastAttackTime = 0f; // 记录上次攻击时间
    private float attackCooldown = 1f; // 攻击冷却时间（1秒）
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start(); // 调用基类的 Start 方法
        damagePopupSystem = FindObjectOfType<DamagePopupSystem>();
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
            ContactPoint2D contact = collision.contacts[0];
            Vector2 normal = contact.normal;
            SPlayerController player = collision.gameObject.GetComponent<SPlayerController>();
            // 增加相对位置检查
            Collider2D myCollider = GetComponent<Collider2D>();
            Vector2 relativePosition = myCollider.bounds.center - player.transform.position;
            if (normal.y < -0.75f && relativePosition.y < -0.1f)
            {               
                Die();
            }
            else
            {
                // 检查是否在冷却时间内
                if (Time.time - lastAttackTime >= attackCooldown)
                {                  
                    int direction = GetFaceDir();
                    if (player != null)
                    {
                        player.GetHurt(direction, damageNum);
                        lastAttackTime = Time.time; // 更新上次攻击时间
                    }
                }
            }
        }
    }

    public override Vector3 GetShowPosition()
    {
        Vector3 position = transform.position;
        position.y -= 1f;
        return position;
    }
}
