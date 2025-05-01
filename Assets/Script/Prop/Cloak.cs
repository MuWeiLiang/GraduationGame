using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloak : MonoBehaviour
{
    private float cloakDuration; // 隐身持续时间
    private bool beUsed = false;
    // Start is called before the first frame update
    void Start()
    {
        cloakDuration = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !beUsed)
        {
            beUsed = true;
            SPlayerController playerController = other.GetComponent<SPlayerController>();

            if (playerController != null)
            {
                playerController.ActivateCloak(cloakDuration);
                FindObjectOfType<PropSoundBase>().Pickup(2);
            }
            Destroy(gameObject);
        }
    }
}
