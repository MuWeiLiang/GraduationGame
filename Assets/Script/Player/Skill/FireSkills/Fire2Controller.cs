using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill2Controller : SkillController
{
    private GameObject _fireInstance;
    GameObject prefab;
    bool _active = false;

    public override void Initialize()
    {
        Init(ElementType.Fire, "Fire2", 300f, 15);
    }

    protected override void ApplyEffect()
    {
        if (_active) return;
        playerController.SetVampire(true);
        GetComponent<FireSkill3Controller>().SetVampire(true);
        _active = true;
    }
}
