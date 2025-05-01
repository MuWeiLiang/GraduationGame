using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderSkill2Controller : SkillController
{
    private GameObject _fireInstance;
    GameObject prefab;

    public override void Initialize()
    {
        Init(ElementType.Thunder, "Thunder2", 10f, 15);
        prefab = Resources.Load<GameObject>("Prefab/Skill/Thunder2");
        if (prefab == null)
        {
            Debug.Log("Thunder2 prefab not found!");
            return;
        }
    }

    protected override void ApplyEffect()
    {
        if (spawnPoint == null || prefab == null) return;

        spawnPoint.y += 0.9f;
        spawnPoint.x += 5f * Dir;

        _fireInstance = Instantiate(prefab, spawnPoint, Quaternion.identity);
        _fireInstance.GetComponent<ThunderSkill2>().SetDamage(playerController.GetDamage());
        if (Dir < 0)
        {
            Vector3 scale = _fireInstance.transform.localScale;
            scale.x *= -1;
            _fireInstance.transform.localScale = scale;
        }
    }
}
