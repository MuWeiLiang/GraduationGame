using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PropSoundBase : MonoBehaviour
{
    [System.Serializable]
    public class PickupSoundEvent : UnityEvent<string> { }
    public PickupSoundEvent onPickup;
    private SoundManager soundManager;
    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        if (soundManager != null)
        {
            onPickup.AddListener(soundManager.PickupSound);
        }
        else
        {
            Debug.LogWarning("soundManager == null");
            // onPickup.AddListener(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Pickup(int x = 1)
    {
        // Debug.Log("Play sound");
        if(x == 1)
            onPickup?.Invoke("pickup");
        if(x == 2)
            onPickup?.Invoke("pickup2");
        if (x == 3)
            onPickup?.Invoke("inter");
    }
}
