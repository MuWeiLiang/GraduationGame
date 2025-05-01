using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private bool canAttack = true;
    public int damage = 20; // ÉËº¦
    public float duration = 0.667f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canAttack) return;
        int direction = transform.localScale.x > 0 ? 1 : -1;
        if (transform.parent != null)
        {
            direction = transform.parent.localScale.x > 0 ? 1 : -1;
        }
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<SPlayerController>().GetHurt(direction, damage);
            canAttack = false;
            Invoke(nameof(ResetAttack), duration);
        }
    }
    private void ResetAttack()
    {
        canAttack = true;
    }
}
