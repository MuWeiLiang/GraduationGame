using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWing : MonoBehaviour
{
    private bool isFlying = false;
    private SPlayerController player;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponentInParent<SPlayerController>();
        if (player == null)
        {
            Debug.Log("MoveCharacter component not found on the same GameObject.");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var flag = player.GetIsFlying();
        if(!player.IsAlive()) anim.SetBool("isFlying", false);
        if (isFlying != flag)
        {
            isFlying = flag;
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
