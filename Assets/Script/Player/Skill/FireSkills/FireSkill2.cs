using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill2 : SkillBase
{
    private bool _isAttacked = false;

    void Start()
    {
        //Debug.Log("FireSkill2 Start");
        Destroy(gameObject, 1f);
    }
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_isAttacked) return;

        if (other.CompareTag("Boss"))
        {
            Explode(); // Åö×²µÐÈËÁ¢¼´±¬Õ¨
        }
    }

    private void Explode()
    {
        _isAttacked = true;
        //Destroy(gameObject,0.75f);
    }
}
