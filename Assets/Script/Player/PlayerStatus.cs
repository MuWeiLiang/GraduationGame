using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus
{
    public bool isControlled; // �Ƿ񱻿���
    public bool isImmuneToControl; // �Ƿ����߿���
    public bool isImmuneToDamage = false;
    public bool isCloaked; // �Ƿ�����
    public bool isStoned; // �Ƿ�ʯ��
    private MonoBehaviour _monoBehaviour;
    private SpriteEffectController spriteEffectController;
    public void Initialize(MonoBehaviour monoBehaviour,SpriteEffectController spriteEffectController)
    {
        isControlled = false;
        isImmuneToControl = false;
        isCloaked = false;
        isStoned = false;
        _monoBehaviour = monoBehaviour;
        this.spriteEffectController = spriteEffectController;
    }
    public void ActivateCloak(float duration)
    {
        isCloaked = true;
        spriteEffectController.ApplyEffect("Cloak", duration);
        _monoBehaviour.StartCoroutine(DeactivateCloak(duration));
    }

    private IEnumerator DeactivateCloak(float duration)
    {
        yield return new WaitForSeconds(duration);
        spriteEffectController.RemoveEffect("Cloak");
        isCloaked = false;
    }
    public void ActivateStone(float duration)
    {
        isStoned = true;
        // _monoBehaviour.StartCoroutine(DeactivateStone(duration));
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
        if (isStoned)
        {
            DeactivateStone();
        }
    }

    public void SetImmuneToDamage(bool flag)
    {
        isImmuneToDamage = flag;
    }
}
