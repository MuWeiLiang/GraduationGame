using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Trap : MonoBehaviour
{
    // private BaseCharacter baseCharacter;
    private HashSet<GameObject> triggeredObjects = new HashSet<GameObject>();
    private SPlayerController playerController;
    private float lastHurtTime = -1f;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<SPlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("trap2") && !triggeredObjects.Contains(collision.gameObject))
        {
            triggeredObjects.Add(collision.gameObject);
            Destroy(collision.gameObject);
            playerController.GetHurt(0, 40);
        }
        //if(collision.gameObject.CompareTag("trap1"))
        //{
        //    if(Time.time -  lastHurtTime >= 1f)
        //    {
        //        lastHurtTime = Time.time;
        //        playerController.GetHurt(0, 20);
        //    }
        //}
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("trap1"))
        {
            if (Time.time - lastHurtTime >= 1f)
            {
                lastHurtTime = Time.time;
                playerController.GetHurt(0, 20);
            }
        }
    }
}
