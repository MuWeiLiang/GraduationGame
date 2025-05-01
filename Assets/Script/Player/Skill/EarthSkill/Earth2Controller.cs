using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSkill2Controller : SkillController
{
    private GameObject _fireInstance;
    GameObject prefab;

    public override void Initialize()
    {
        Init(ElementType.Earth, "Earth2", 10f, 15);
        prefab = Resources.Load<GameObject>("Prefab/Skill/Earth2");
        if (prefab == null)
        {
            Debug.Log("Earth2 prefab not found!");
            return;
        }
    }

    protected override void ApplyEffect()
    {
        if (spawnPoint == null || prefab == null) return;

        spawnPoint.y += 1f;
        //spawnPoint.x += 2f * Dir;

        _fireInstance = Instantiate(prefab, spawnPoint, Quaternion.identity);
        _fireInstance.transform.parent = transform;
        playerController.MulDefense(2f);
        if(Dir < 0)
        {
            Vector3 scale = _fireInstance.transform.localScale;
            scale.x *= -1;
            _fireInstance.transform.localScale = scale;
        }
        Destroy(_fireInstance, 10f);
        Invoke("ResetDefense", 10f);
    }

    void ResetDefense()
    {
        playerController.MulDefense(0.5f);
    }
}
