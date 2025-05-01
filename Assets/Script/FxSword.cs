using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxSword : MonoBehaviour
{
    private int damage = 10; // 伤害值
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            Debug.Log("Boss碰撞FX2，造成伤害 = " + damage);
        }
    }
    public void SetDamage(int damage)
    {
        this.damage = damage; // 设置伤害值
    }
}
