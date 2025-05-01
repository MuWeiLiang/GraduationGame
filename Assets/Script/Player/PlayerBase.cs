using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SPlayerController;

public class PlayerBase
{
    PlayerSoundEvent onPlayer;
    private Rigidbody2D rb;
    private Animator anim;
    //private DamagePopupSystem damagePopupSystem;
    public int maxHealth = 100;
    public int health;
    public int maxMana = 100;
    public int mana;
    public bool alive = true;
    private int resurrectNum;
    private static int persistentResurrectNum = -1; // -1表示未初始化
    public bool ExecuteDie = false;
    private bool isResurrecting = false;
    public bool isGameOver = false;
    public void Initialize(Rigidbody2D RB, Animator Anim, PlayerSoundEvent onPlayer)
    {
        // Debug.Log("PlayerBase initialized");
        rb = RB;
        anim = Anim;
        this.onPlayer = onPlayer;
        //this.damagePopupSystem = damagePopupSystem;
        health = maxHealth;
        mana = maxMana;
        int Num = LevelBaseData.Instance.LevelMode == 0 ? 1 : 2;
        resurrectNum = persistentResurrectNum >= 0 ? persistentResurrectNum : Num;       
    }
    public void Tick()
    {
        if (health <= 0)
        {
            alive = false;
            health = 0;
        }
        if (!alive && !ExecuteDie)
        {
            Die();
        }
    }
    public void TakeDamage(int damage)
    {
        if (damage < 0) damage = 0;
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            // alive = false;
        }
    }

    public void GetHeal(int amount)
    {
        health += amount;
        //ShowHeal(amount);
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    //void ShowHeal(int amount)
    //{
    //    if (damagePopupSystem == null) return;
    //    damagePopupSystem.ShowHeal(amount);
    //}

    public void GetMana(int amount)
    {
        mana += amount;
        if (mana > maxMana)
        {
            mana = maxMana;
        }
    }

    // 通用方法：死亡
    public void Die()
    {
        if (ExecuteDie) return;

        anim.SetTrigger("die");
        alive = false;
        ExecuteDie = true;
        onPlayer?.Invoke("die");

        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        rb.isKinematic = true;

        if(resurrectNum <= 0) isGameOver = true;
    }
    public bool CanResurrect()
    {
        return resurrectNum > 0;
    }
    public int GetResurrectNum()
    {
        return resurrectNum;
    }
    public void UpdateResurrectNum(int num)
    {
        resurrectNum += num;
        persistentResurrectNum = resurrectNum;
        isResurrecting = true;
        // Debug.Log("ResurrectNum: " + resurrectNum);
    }

    public void UpdateMana(int Num)
    {
        mana += Num;
        if (mana < 0) mana = 0;
        if (mana > maxMana) mana = maxMana;
    }

    public bool IsManaEnough(int num)
    {
        return mana >= num;
    }

    public bool IsInResurrecting()
    {
        return isResurrecting;
    }
    public int GetHealth()
    {
        return health;
    }

    public void Resurrect()
    {
        health = maxHealth;
        mana = maxMana;
        alive = true;
        // rb.isKinematic = false;
        // rb.gravityScale = 1;
        // rb.velocity = Vector2.zero;
        // anim.SetTrigger("resurrect");
        // onPlayer?.Invoke("resurrect");
        isResurrecting = false;
    }

    //public void AddHealth(int Num)
    //{
    //    maxHealth += Num;
    //    health = maxHealth;
    //}
}
