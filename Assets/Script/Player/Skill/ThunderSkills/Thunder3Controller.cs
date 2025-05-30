using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderSkill3Controller : SkillController
{
    private GameObject _fireInstance;
    GameObject prefab;
    private HashSet<Collider2D> _hittedEnemies = new HashSet<Collider2D>();

    public override void Initialize()
    {
        Init(ElementType.Thunder, "Thunder3", 15f, 20);
        prefab = Resources.Load<GameObject>("Prefab/Skill/Thunder3");
        if (prefab == null)
        {
            Debug.Log("Thunder3 prefab not found!");
            return;
        }
    }

    protected override void ApplyEffect()
    {
        if (spawnPoint == null || prefab == null) return;
        _hittedEnemies.Clear();

        spawnPoint.y += 1.2f;

        spawnPoint.x += 3f * Dir;
        SpawnSkill();
        spawnPoint.x += 2f * Dir;
        SpawnSkill();
        spawnPoint.x += 2f * Dir;
        SpawnSkill();
    }

    void SpawnSkill()
    {
        _fireInstance = Instantiate(prefab, spawnPoint, Quaternion.identity);
        var skill = _fireInstance.GetComponent<ThunderSkill3>();
        skill.SetDamage(playerController.GetDamage());
        skill._hittedEnemies = _hittedEnemies;
        if (Dir < 0)
        {
            Vector3 scale = _fireInstance.transform.localScale;
            scale.x *= -1;
            _fireInstance.transform.localScale = scale;
        }
    }
}
