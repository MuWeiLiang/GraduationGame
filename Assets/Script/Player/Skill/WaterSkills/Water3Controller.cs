using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSkill3Controller : SkillController
{
    private GameObject _fireInstance;
    GameObject prefab;

    public override void Initialize()
    {
        Init(ElementType.Water, "Water3", 15f, 20);
        prefab = Resources.Load<GameObject>("Prefab/Skill/Water3");
        if (prefab == null)
        {
            Debug.Log("Water3 prefab not found!");
            return;
        }
    }

    protected override void ApplyEffect()
    {
        if (spawnPoint == null || prefab == null) return;

        spawnPoint.y += 0.8f;
        spawnPoint.x += 5f * Dir;

        _fireInstance = Instantiate(prefab, spawnPoint, Quaternion.identity);
        _fireInstance.GetComponent<WaterSkill3>().SetDamage(playerController.GetDamage());
        if (Dir < 0)
        {
            Vector3 scale = _fireInstance.transform.localScale;
            scale.x *= -1;
            _fireInstance.transform.localScale = scale;
        }
    }
}
