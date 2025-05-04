using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill3 : SkillBase
{
    private bool _isAttacked = false;
    private bool isVampire = false;

    void Start()
    {
        //Debug.Log("FireSkill3 Start");
        Destroy(gameObject, 1.5f);
    }
    void Update()
    {
        //Debug.Log("FireSkill3 Update");
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
            if (isVampire)
            {
                FindObjectOfType<SPlayerController>().GetHeal(ComputeDamage() / 10);
            }
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

    public void SetVampire(bool flag)
    {
        isVampire = flag;
    }
}
