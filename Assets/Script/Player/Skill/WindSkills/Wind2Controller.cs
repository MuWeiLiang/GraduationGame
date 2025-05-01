using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSkill2Controller : SkillController
{
    public override void Initialize()
    {
        Init(ElementType.Wind, "Wind2", 1f, 0);
    }

    protected override void ApplyEffect()
    {
        playerController.StartOrStopFly();
    }
}
