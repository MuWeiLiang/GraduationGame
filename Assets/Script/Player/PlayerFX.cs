using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActiveBlink()
    {
        GameObject fxPrefab = Resources.Load<GameObject>("Prefab/FX/fx-blink");
        Vector3 spawnPosition = transform.position;
        spawnPosition.x -= 0.69f;
        spawnPosition.y += 1.52f;
        if (fxPrefab != null)
        {
            GameObject fxInstance = Instantiate(fxPrefab, spawnPosition, Quaternion.identity);
            fxInstance.transform.SetParent(transform);
            fxInstance.SetActive(true); // ������Ч
            Destroy(fxInstance, 1f);
        }
    }
    private void DeactivateBlink()
    {
        Transform fx1 = transform.Find("fx1");
        if (fx1 != null)
        {
            fx1.gameObject.SetActive(false);
        }
    }
    public void ActiveSword(Vector3 spawnPosition,int dirction)
    {
        GameObject fxPrefab = Resources.Load<GameObject>("Prefab/FX/fx-sword");
        if (fxPrefab != null)
        {
            GameObject fxInstance = Instantiate(fxPrefab, spawnPosition, Quaternion.identity);
            fxInstance.transform.localScale = new Vector3(
                fxInstance.transform.localScale.x * dirction,  // ��ת X ��
                fxInstance.transform.localScale.y,   // Y �᲻��
                fxInstance.transform.localScale.z    // Z �᲻��
);
            fxInstance.GetComponent<FxSword>().SetDamage(10); // �����˺�ֵ

            Destroy(fxInstance, 2f); // 3����Զ�����
        }
        else
        {
            Debug.LogError("FX2 prefab is not assigned!");
        }
    }
}
