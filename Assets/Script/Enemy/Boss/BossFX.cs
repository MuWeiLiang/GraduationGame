using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFX : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActiveRampage()
    {
        Transform fx1 = transform.Find("fx1");
        if (fx1 != null)
        {
            fx1.gameObject.SetActive(true); // 激活特效
            Invoke("DeactiveRampage", 15f);
        }
    }
    private void DeactiveRampage()
    {
        Transform fx1 = transform.Find("fx1");
        if (fx1 != null)
        {
            fx1.gameObject.SetActive(false); // 关闭特效
        }
    }
    public void ActiveAreaDamage(Vector3 spawnPosition, int damage)
    {
        GameObject fx2Prefab = Resources.Load<GameObject>("Prefab/FX/fx2");
        if (fx2Prefab != null)
        {
            GameObject fx2Instance = Instantiate(fx2Prefab, spawnPosition, Quaternion.identity);
            fx2Instance.GetComponent<FX>().SetDamage(damage / 2); // 设置伤害值

            Destroy(fx2Instance, 3f); // 3秒后自动销毁
        }
        else
        {
            Debug.LogError("FX2 prefab is not assigned!");
        }
    }
}
