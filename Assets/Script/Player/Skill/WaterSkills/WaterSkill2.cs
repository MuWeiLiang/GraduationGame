using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSkill2 : SkillBase
{
    private bool _isAttacked = false;

    void Start()
    {
        Destroy(gameObject, 1.8f);
    }
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_isAttacked) return;

        if (other.CompareTag("Boss"))
        {
            AttackEnemy(other);
            SlowEnemy(other);
        }
    }

    void AttackEnemy(Collider2D other)
    {
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(ComputeDamage());
            Debug.Log("Damage Enemy " + ComputeDamage());
        }
        else
        {
            Debug.Log("没有找到敌人组件");
        }
    }
    void SlowEnemy(Collider2D other)
    {
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.SlowSpeed(0.3f, 10f);
            Debug.Log("Slow Enemy " + 30 + "%");
        }
    }
    int ComputeDamage()
    {
        float damage = attackDamage;
        damage *= 1.5f;
        return (int)damage;

    }
}
