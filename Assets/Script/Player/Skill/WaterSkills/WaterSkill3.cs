using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSkill3 : SkillBase
{
    private bool _isAttacked = false;

    void Start()
    {
        //Debug.Log("FireSkill2 Start");
        Destroy(gameObject, 2f);
    }
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_isAttacked) return;

        if (other.CompareTag("Boss") || other.CompareTag("monster"))
        {
            AttackEnemy(other);
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
    int ComputeDamage()
    {
        float damage = attackDamage;
        damage *= 2.5f;
        return (int)damage;
    }
}
