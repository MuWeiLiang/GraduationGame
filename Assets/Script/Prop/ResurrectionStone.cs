using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurrectionStone : MonoBehaviour
{
    public int NumToAdd;
    private bool beUsed = false;
    // Start is called before the first frame update
    void Start()
    {
        NumToAdd = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !beUsed)
        {
            BaseCharacter playerController = other.GetComponent<BaseCharacter>();
            beUsed = true;

            if (playerController != null)
            {
                playerController.UpdateResurrectNum(NumToAdd);
                // Ïú»ÙµÀ¾ß
                Destroy(gameObject);
            }
        }
    }
}
