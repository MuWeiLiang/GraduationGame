using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxSword : MonoBehaviour
{
    private int damage = 10; // �˺�ֵ
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            Debug.Log("Boss��ײFX2������˺� = " + damage);
        }
    }
    public void SetDamage(int damage)
    {
        this.damage = damage; // �����˺�ֵ
    }
}
