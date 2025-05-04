using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class itemCollect : MonoBehaviour
{
    private HashSet<GameObject> triggeredObjects = new HashSet<GameObject>();
    private SLevelManager levelManager;
    private PropSoundBase propSoundBase;
    void Start()
    {
        levelManager = FindObjectOfType<SLevelManager>();
        if (levelManager == null)
        {
            //Debug.LogError("SLevelManager not found!");
        }
        propSoundBase = FindObjectOfType<PropSoundBase>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(propSoundBase == null) propSoundBase = FindObjectOfType<PropSoundBase>();
        SwitchTrigger(collision, "item-pumpkin", 1);
        SwitchTrigger(collision, "item-woodchest", 2);
        SwitchTrigger(collision, "item-goldenchest", 5);
    }
    void SwitchTrigger(Collider2D collision,string str,int s)
    {
        if(collision.gameObject.CompareTag(str) && !triggeredObjects.Contains(collision.gameObject))
        {
            triggeredObjects.Add(collision.gameObject);
            //FindObjectOfType<PropSoundBase>().Pickup();
            if(propSoundBase != null)
            {
                propSoundBase.Pickup();
            }
            else
            {
                Debug.LogWarning("propSoundBase == null");
            }
            Destroy(collision.gameObject);
            levelManager.AddScore(s);
        }
    }
}
