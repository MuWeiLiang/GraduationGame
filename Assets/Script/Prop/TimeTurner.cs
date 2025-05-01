using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTurner : MonoBehaviour
{
    public float timeToAdd;
    private bool beUsed = false;
    // Start is called before the first frame update
    void Start()
    {
        timeToAdd = 60f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !beUsed)
        {
            //MoveCharacter playerController = other.GetComponent<MoveCharacter>();
            SLevelManager levelManager = FindObjectOfType<SLevelManager>();
            beUsed = true;

            if (levelManager != null)
            {
                levelManager.AddTime(timeToAdd);
                FindObjectOfType<PropSoundBase>().Pickup(2);
                // Ïú»ÙµÀ¾ß
                Destroy(gameObject);
            }
        }
    }
}
