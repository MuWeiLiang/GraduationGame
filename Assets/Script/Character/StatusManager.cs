using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public bool isControlled; // 是否被控制
    public bool isImmuneToControl; // 是否免疫控制
    public bool isCloaked; // 是否隐身
    public bool isStoned; // 是否石化
    private bool isMoveForbidden; // 是否禁止移动
    private MoveCharacter moveCharacter; // 引用MoveCharacter脚本
    private FightCharacter fightCharacter; // 引用FightCharacter脚本
    private SpriteEffectController spriteEffectController; // 引用SpriteEffectController脚本
    // Start is called before the first frame update
    void Start()
    {
        isControlled = false;
        isImmuneToControl = false;
        isCloaked = false;
        isStoned = false;
        isMoveForbidden = false;
        moveCharacter = GetComponent<MoveCharacter>();
        fightCharacter = GetComponent<FightCharacter>();
        spriteEffectController = GetComponent<SpriteEffectController>();
        if (moveCharacter == null)
        {
            Debug.LogError("MoveCharacter脚本未找到");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isStoned && !isControlled)
        {
            isControlled = true;
        }
        if(!isStoned && isControlled)
        {
            isControlled = false;
        }
        if (isControlled) {
            moveCharacter.SetCanMove(false);
            fightCharacter.SetCanFight(false);
            isMoveForbidden = true;
        }
        if(!isControlled && isMoveForbidden)
        {
            moveCharacter.SetCanMove(true);
            fightCharacter.SetCanFight(true);
            isMoveForbidden = false;
        }
    }
    public void ActivateCloak(float duration)
    {
        isCloaked = true;
        StartCoroutine(DeactivateCloak(duration));
    }

    private IEnumerator DeactivateCloak(float duration)
    {
        yield return new WaitForSeconds(duration);
        isCloaked = false;
    }
    public void ActivateStone(float duration)
    {
        isStoned = true;
        // StartCoroutine(DeactivateStone(duration));
        spriteEffectController.ApplyEffect("Stone", duration);
    }
    public void DeactivateStone()
    {
        isStoned = false;
        spriteEffectController.RemoveEffect("Stone");
    }
    public void DeactivateControl()
    {
        isControlled = false;
        if(isStoned)
        {
            DeactivateStone();
        }
        moveCharacter.SetCanMove(true);
        fightCharacter.SetCanFight(true);
    }
}
