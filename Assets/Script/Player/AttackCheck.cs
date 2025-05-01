using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheck : MonoBehaviour
{
    private int attackDamage;
    private bool isVampire = false;
    private SPlayerController playerController;
    //private DamagePopupSystem damagePopupSystem;
    // Start is called before the first frame update
    void Start()
    {
        attackDamage = 0;
        playerController = GetComponentInParent<SPlayerController>();
        //damagePopupSystem = FindObjectOfType<DamagePopupSystem>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("攻击检测触发器进入");
        if (other.CompareTag("monster") || other.CompareTag("Boss"))
        {
            BaseEnemy enemy = other.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                // 生成伤害数字
                //damagePopupSystem.ShowDamage(attackDamage, enemy.GetShowPosition());
                Debug.Log("Hit");
                if (isVampire)
                {
                    playerController.GetHeal(attackDamage/10);
                }
            }
            else
            {
                Debug.Log("没有找到敌人组件");
            }
        }
    }
    public void SetDamage(int damage)
    {
        attackDamage = damage;
    }

    public void SetVampire(bool flag)
    {
        isVampire = flag;
    }
}
