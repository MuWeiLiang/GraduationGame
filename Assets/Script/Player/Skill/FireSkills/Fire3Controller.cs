using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill3Controller : SkillController
{
    private GameObject _fireInstance;
    private bool isVampire = false;
    GameObject prefab;

    public override void Initialize()
    {
        Init(ElementType.Fire, "Fire3", 15f, 20);
        prefab = Resources.Load<GameObject>("Prefab/Skill/Fire3");
        if (prefab == null)
        {
            Debug.Log("Fire3 prefab not found!");
            return;
        }
    }

    protected override void ApplyEffect()
    {
        if (spawnPoint == null || prefab == null) return;

        spawnPoint.y += 1.2f;
        spawnPoint.x += 5f * Dir;

        _fireInstance = Instantiate(prefab, spawnPoint, Quaternion.identity);
        var fireSkill = _fireInstance.GetComponent<FireSkill3>();
        fireSkill.SetDamage(playerController.GetDamage());
        fireSkill.SetVampire(isVampire);
        if (Dir < 0)
        {
            Vector3 scale = _fireInstance.transform.localScale;
            scale.x *= -1;
            _fireInstance.transform.localScale = scale;
        }
    }
    public void SetVampire(bool isVampire)
    {
        this.isVampire = isVampire;
    }
}
