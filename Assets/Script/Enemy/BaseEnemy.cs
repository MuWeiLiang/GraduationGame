using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public int health = 30; // 血量
    public int mana = 20;   // 蓝量
    public bool alive = true;
    protected DamagePopupSystem damagePopupSystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        ShowDamage(damage);
        if (health <= 0)
        {
            alive = false;
            Die();
        }
    }

    // 通用方法：死亡
    public virtual void Die()
    {
        Debug.Log("Enemy has died.");
        Destroy(gameObject);
    }

    public virtual void SlowSpeed(float ratio,float duration) { 

    }

    public virtual Vector3 GetShowPosition()
    {
        Vector3 position = transform.position;
        position.y += 1f;
        return position;
    }

    public virtual void ShowDamage(int damage)
    {
        if (damagePopupSystem == null) return;
        damagePopupSystem.ShowDamage(damage, GetShowPosition());
    }
}
