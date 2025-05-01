using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSkill3Controller : SkillController
{
    private GameObject _fireInstance;
    GameObject prefab;

    public override void Initialize()
    {
        Init(ElementType.Wind, "Wind3", 15f, 20);
        prefab = Resources.Load<GameObject>("Prefab/Skill/Wind3");
        if (prefab == null)
        {
            Debug.Log("Wind3 prefab not found!");
            return;
        }
    }

    protected override void ApplyEffect()
    {
        if (spawnPoint == null || prefab == null) return;

        spawnPoint.y += 1.2f;
        spawnPoint.x += 5f * Dir;

        _fireInstance = Instantiate(prefab, spawnPoint, Quaternion.identity);
        if(Dir < 0)
        {
            Vector3 scale = _fireInstance.transform.localScale;
            scale.x *= -1;
            _fireInstance.transform.localScale = scale;
        }
    }
}
