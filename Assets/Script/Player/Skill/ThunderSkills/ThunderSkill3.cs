using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderSkill3 : SkillBase
{
    private bool _isAttacked = false;
    public HashSet<Collider2D> _hittedEnemies;

    void Start()
    {
        //Debug.Log("FireSkill2 Start");
        Destroy(gameObject, 1f);
    }
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_isAttacked) return;

        if (_hittedEnemies != null && _hittedEnemies.Contains(other))
        {
            //Debug.Log("ThunderSkill3 already hit enemy");
            return;
        }

        //Debug.Log("ThunderSkill3 OnTriggerEnter2D : " + other.name);
        if (other.CompareTag("Boss") || other.CompareTag("monster"))
        {
            //Debug.Log("ThunderSkill3 Hit");
            _hittedEnemies.Add(other);
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
