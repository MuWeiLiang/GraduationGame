using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGoblet : MonoBehaviour
{
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
        if (other.CompareTag("Player"))
        {
            SLevelManager sLevelManager = FindObjectOfType<SLevelManager>();
            if (sLevelManager != null)
            {
                if(sLevelManager.IsCompleted())
                {
                    Debug.Log("Game Completed");
                }
                else
                {
                    Debug.Log("Magic Stone Is Not Enough");
                }
            }
            else
            {
                Debug.Log("SLevelManager Not Found");
            }
        }
    }
}
