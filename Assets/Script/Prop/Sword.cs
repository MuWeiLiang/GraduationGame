using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private bool beUsed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !beUsed)
        {
            FightCharacter playerController = other.GetComponent<FightCharacter>();
            beUsed = true;

            if (playerController != null)
            {
                playerController.attackDamage *= 2;
                playerController.attackRange *= 2;               
                // Ïú»ÙµÀ¾ß
                Destroy(gameObject);
            }
        }
    }
}
