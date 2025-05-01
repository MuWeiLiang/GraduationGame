using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public bool isControlled; // �Ƿ񱻿���
    public bool isImmuneToControl; // �Ƿ����߿���
    public bool isCloaked; // �Ƿ�����
    public bool isStoned; // �Ƿ�ʯ��
    private bool isMoveForbidden; // �Ƿ��ֹ�ƶ�
    private MoveCharacter moveCharacter; // ����MoveCharacter�ű�
    private FightCharacter fightCharacter; // ����FightCharacter�ű�
    private SpriteEffectController spriteEffectController; // ����SpriteEffectController�ű�
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
            Debug.LogError("MoveCharacter�ű�δ�ҵ�");
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
