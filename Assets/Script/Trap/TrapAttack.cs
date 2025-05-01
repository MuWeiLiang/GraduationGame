using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAttack : MonoBehaviour
{
    private bool canAttack = true;
    public int damage = 20; // …À∫¶
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canAttack) return;
        if (collision.CompareTag("Player"))
        {
            //Debug.Log("Trap Attack Damage : " + damage);
            collision.GetComponent<SPlayerController>().GetHurt(0,damage);
            canAttack = false;
            Invoke(nameof(ResetAttack), 1f);
        }
    }
    private void ResetAttack()
    {
        canAttack = true;
    }
}
