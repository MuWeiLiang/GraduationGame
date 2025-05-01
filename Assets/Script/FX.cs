using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX : MonoBehaviour
{
    public float CooldownTime = 1f;
    private float lastDamageTime = -1f;
    private int damage = 10; // 伤害值
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检测碰撞对象是否是玩家
        if (other.CompareTag("Player"))
        {
            DealDamage(other);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DealDamage(collision);
        }
    }
    void DealDamage(Collider2D other)
    {
        float currentTime = Time.time;
        if (currentTime - lastDamageTime >= CooldownTime)
        {
            SPlayerController sPlayerController = other.GetComponent<SPlayerController>();
            if (sPlayerController != null)
            {
                sPlayerController.GetHurt(0, damage);
                lastDamageTime = currentTime;
            }
            else
            {
                Debug.LogWarning("SPlayerController component not found on the player.");
            }

        }
    }
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
}
