using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    public int maxHealth = 100;
    public int health;
    public int maxMana = 100;
    public int mana;
    public bool alive = true;
    private int resurrectNum;
    private static int persistentResurrectNum = -1; // -1表示未初始化

    private string originalTag;

    public virtual void Start()
    {
        // Debug.Log("Here");
        health = maxHealth;
        mana = maxMana;
        resurrectNum = persistentResurrectNum >= 0 ? persistentResurrectNum : 3;
        originalTag = gameObject.tag;
    }
    void Update()
    {

    }
    // 通用方法：受到伤害
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            // Die();
        }
    }

    public void GetHeal(int amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    // 通用方法：死亡
    public virtual void Die()
    {
        Debug.Log("Character has died.");
        // 可以在这里添加死亡动画、销毁对象等逻辑
        Destroy(gameObject);
    }
    public bool CanResurrect()
    {
        return resurrectNum > 0;
    }
    public void UpdateResurrectNum(int num)
    {
        resurrectNum += num;
        persistentResurrectNum = resurrectNum;
    }

    public void UpdateMana(int Num)
    {
        mana += Num;
        if(mana < 0) mana = 0;
        if(mana > maxMana) mana = maxMana;
    }

    public bool IsManaEnough(int num)
    {
        return mana >= num;
    }
}
