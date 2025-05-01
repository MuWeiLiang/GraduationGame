using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    protected int attackDamage = 20;
    public void SetDamage(int damage)
    {
        attackDamage = damage;
    }
}
