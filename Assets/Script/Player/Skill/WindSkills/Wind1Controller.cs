using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSkill1Controller : SkillController
{
    private GameObject _fireballInstance;
    GameObject prefab;

    public override void Initialize()
    {
        //Debug.Log("FireSkill1 Initialize");
        Init(ElementType.Wind, "Wind1", 5f, 10);
        // 加载预制件（建议使用Addressables）
        prefab = Resources.Load<GameObject>("Prefab/Skill/Wind1");
        if (prefab == null)
        {
            Debug.Log("Wind1 prefab not found!");
            return;
        }
    }

    protected override void ApplyEffect()
    {
        if (spawnPoint == null || prefab == null) return;

        spawnPoint.y += 1f;
        spawnPoint.x += 0.5f * Dir;

        _fireballInstance = Instantiate(prefab, spawnPoint, Quaternion.identity);
        _fireballInstance.GetComponent<WindSkill1>().Init(Dir);
        _fireballInstance.GetComponent<WindSkill1>().SetDamage(playerController.GetDamage());
        if (Dir < 0)
        {
            Vector3 scale = _fireballInstance.transform.localScale;
            scale.x *= -1;
            _fireballInstance.transform.localScale = scale;
        }
    }
}
