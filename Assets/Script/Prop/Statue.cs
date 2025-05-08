using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour
{
    private bool playerInRange = false; // ����Ƿ��ڷ�Χ��
    public KeyCode pickupKey = KeyCode.F;
    private bool beUsed = false; // �Ƿ�����ȴ��
    private SLevelManager levelManager;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = FindObjectOfType<SLevelManager>();
        if (levelManager == null)
        {
            Debug.LogError("SLevelManager not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(pickupKey) && !beUsed)
        {
            FindObjectOfType<PropSoundBase>().Pickup(3);
            GameObject Prefab = Resources.Load<GameObject>("Prefab/Prop/timeTurner");
            if (Prefab != null) {
                beUsed = true;
                Vector3 spawnPosition = transform.position;
                spawnPosition.x += 2f;
                Instantiate(Prefab, spawnPosition, Quaternion.identity);
                Debug.Log($"{Prefab.name}�����ɣ�");
            }
            else {
                Debug.LogError("δ�ҵ�����Ԥ���壬����·����");
                return;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // ������������ʾUI��ʾ����"��Fʰȡ"
            if (levelManager != null)
            {
                levelManager.ActivePrompt("Press F");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // ��������������UI��ʾ
        }
    }
}
