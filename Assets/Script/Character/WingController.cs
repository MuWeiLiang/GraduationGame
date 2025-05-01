using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingController : MonoBehaviour
{
    private bool isFlying = false;
    private MoveCharacter moveCharacter;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        moveCharacter = GetComponentInParent<MoveCharacter>();
        if(moveCharacter == null)
        {
            Debug.Log("MoveCharacter component not found on the same GameObject.");
            return;
        }
        // Debug.Log("WingController Start");
    }

    // Update is called once per frame
    void Update()
    {
        if(isFlying != moveCharacter.isFlying)
        {
            isFlying = moveCharacter.isFlying;
            if (isFlying)
            {
                anim.SetBool("isFlying", true);
            }
            else
            {
                anim.SetBool("isFlying", false);
            }
        }
    }
}
