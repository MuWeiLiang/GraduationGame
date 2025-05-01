using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static SPlayerController;

public class PlayerFight
{
    PlayerSoundEvent onPlayer;

    private Rigidbody2D rb;
    private Animator anim;
    private PlayerBase player;
    private Transform selfTransform;
    private AttackCheck attackCheck;
    // 攻击相关
    private int attackDamage = 20; // 攻击伤害
    private int defense = 0;
    // 受伤相关
    public bool isHurting = false;
    private int hitDirection = 0;
    private int direction = 1;
    private int hurtDamage = 20; // 受伤伤害
    public void Initialize(Rigidbody2D RB, Animator Anim, Transform Tran, PlayerBase playerBase, PlayerSoundEvent onPlayer)
    {
        // Initialize player movement settings here
        // Debug.Log("PlayerFight initialized");
        rb = RB;
        anim = Anim;
        selfTransform = Tran;
        player = playerBase;
        attackCheck = selfTransform.GetComponentInChildren<AttackCheck>();
        this.onPlayer = onPlayer;
    }
    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || isHurting)
        {
            Hurt();
        }
    }
    void Attack()
    {
        attackCheck.SetDamage(attackDamage);
        anim.SetTrigger("attack");
        // 播放攻击音效
        onPlayer?.Invoke("attack1");
    }

    void Hurt()
    {
        if (player.alive == false) return;
        anim.SetTrigger("hurt");
        player.TakeDamage(hurtDamage - defense);
        direction = selfTransform.localScale.x > 0 ? 1 : -1;
        if (hitDirection != 0 && hitDirection == direction)
        {
            direction = -direction;
            selfTransform.localScale = new Vector3(-selfTransform.localScale.x, selfTransform.localScale.y, selfTransform.localScale.z);
            hitDirection = 0;
        }
        if (direction == 1)
            rb.AddForce(new Vector2(-5f, 1f), ForceMode2D.Impulse);
        else
            rb.AddForce(new Vector2(5f, 1f), ForceMode2D.Impulse);
        isHurting = false;
        // 播放受伤音效
        if(player.GetHealth() > 0)
            onPlayer?.Invoke("hurt");
    }
    public void GetHurt(int dir = 0, int damageNum = 20)
    {
        isHurting = true;
        if (dir != 0)
            hitDirection = dir;
        if (damageNum != 0)
            hurtDamage = damageNum;
    }

    public int GetDamage() => attackDamage;

    public void MulDefense(float Num)
    {
        defense = (int)(defense * Num);
    }

    public void AddDamage(int Num)
    {
        attackDamage += Num;
    }
    public void AddDefense(int Num)
    {
        defense += Num;
    }
}
