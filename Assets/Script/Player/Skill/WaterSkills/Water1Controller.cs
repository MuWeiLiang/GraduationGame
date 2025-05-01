using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSkill1Controller : SkillController
{
    private GameObject _fireInstance;
    GameObject prefab;

    public override void Initialize()
    {
        Init(ElementType.Water, "Water1", 5f, 10);
        prefab = Resources.Load<GameObject>("Prefab/Skill/Water1");
        if (prefab == null)
        {
            Debug.Log("Water1 prefab not found!");
            return;
        }
    }

    protected override void ApplyEffect()
    {
        if (spawnPoint == null || prefab == null) return;

        spawnPoint.y += 0.8f;
        //spawnPoint.x += 2f * Dir;

        _fireInstance = Instantiate(prefab, spawnPoint, Quaternion.identity);
        _fireInstance.transform.SetParent(transform, true);
        var damage = playerController.GetDamage();
        playerController.GetHeal(damage);
        if(Dir < 0)
        {
            Vector3 scale = _fireInstance.transform.localScale;
            scale.x *= -1;
            _fireInstance.transform.localScale = scale;
        }
        Destroy(_fireInstance, 1.5f);
    }
}
